using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ForestResourcePlugin
{
    public partial class Basic : Form
    {
        private CancellationTokenSource _cancellationTokenSource;
        private DataTable previewData;
        private DataTable mappingData;
        private List<CountyDataInfo> selectedCountyData; // 存储选中县的数据信息
        private List<LayerInfo> mapLayers; // 当前地图文档中的图层列表

        // 保留城镇开发边界要素类字段
        private IFeatureClass czkfbjFeatureClass;

        public Basic()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // 初始化处理选项
            chkTopologyCheck.Checked = true;
            chkGeometryValidation.Checked = true;

            // 初始化字段映射表格
            InitializeMappingGrid();

            // 初始化筛选条件复选框
            chkForestLand.Checked = true;
            chkStateOwned.Checked = true;
            chkCollectiveInBoundary.Checked = false;

            // 为复选框添加事件处理程序
            chkForestLand.CheckedChanged += FilterCheckBox_CheckedChanged;
            chkStateOwned.CheckedChanged += FilterCheckBox_CheckedChanged;
            chkCollectiveInBoundary.CheckedChanged += FilterCheckBox_CheckedChanged;

            // 加载地图图层
            LoadMapLayers();

            // 尝试加载之前查找到的数据源
            LoadSharedDataSources();

            // 移除原有的图层加载逻辑，改为加载县列表
            LoadCountiesFromSharedData();

            // 加载县列表
            LoadCounties();
        }

        /// <summary>
        /// 县数据信息类
        /// </summary>
        private class CountyDataInfo
        {
            public string CountyName { get; set; }
            public SourceDataFileInfo SourceDataFile { get; set; } // 重命名为SourceDataFile，表示源数据文件
            public SourceDataFileInfo CZKFBJFile { get; set; }
        }

        /// <summary>
        /// 从共享数据管理器加载县列表
        /// </summary>
        private void LoadCountiesFromSharedData()
        {
            try
            {
                // 获取源数据文件列表（原LCXZGX文件列表）
                var sourceDataFiles = SharedDataManager.GetSourceDataFiles();

                // 获取城镇开发边界文件列表
                var czkfbjFiles = SharedDataManager.GetCZKFBJFiles();

                // 获取所有唯一的县名（第二级目录名）
                var countyNames = new HashSet<string>();

                // 从源数据文件中提取县名
                foreach (var file in sourceDataFiles)
                {
                    if (!string.IsNullOrEmpty(file.DisplayName))
                    {
                        countyNames.Add(file.DisplayName);
                    }
                }

                // 从城镇开发边界文件中提取县名
                foreach (var file in czkfbjFiles)
                {
                    if (!string.IsNullOrEmpty(file.DisplayName))
                    {
                        countyNames.Add(file.DisplayName);
                    }
                }

                // 清空并重新填充县列表
                chkListCounties.Items.Clear();

                if (countyNames.Count > 0)
                {
                    var sortedCounties = countyNames.OrderBy(name => name).ToList();
                    foreach (var countyName in sortedCounties)
                    {
                        chkListCounties.Items.Add(countyName, false);
                    }

                    UpdateStatus($"已加载 {countyNames.Count} 个县的数据");
                    System.Diagnostics.Debug.WriteLine($"已加载县列表: {string.Join(", ", sortedCounties)}");
                }
                else
                {
                    UpdateStatus("未找到县数据，请先在基础数据准备中选择数据源");
                    System.Diagnostics.Debug.WriteLine("未找到任何县数据");
                }

                // 为县列表添加选择变化事件
                chkListCounties.ItemCheck += ChkListCounties_ItemCheck;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载县列表时出错: {ex.Message}");
                UpdateStatus("加载县列表失败");
                MessageBox.Show($"加载县列表时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据选中的县加载字段信息
        /// </summary>
        private void LoadFieldsFromSelectedCounties()
        {
            try
            {
                var selectedCounties = GetSelectedCounties();

                // 清空字段下拉框
                cmbLandTypeField.Items.Clear();
                cmbLandOwnerField.Items.Clear();

                if (selectedCounties.Count == 0)
                {
                    UpdateStatus("请选择至少一个县");
                    return;
                }

                // 准备选中县的数据信息
                selectedCountyData = new List<CountyDataInfo>();

                // 获取共享数据
                var sourceDataFiles = SharedDataManager.GetSourceDataFiles();
                var czkfbjFiles = SharedDataManager.GetCZKFBJFiles();

                var allFieldNames = new HashSet<string>();

                foreach (var countyName in selectedCounties)
                {
                    var countyInfo = new CountyDataInfo { CountyName = countyName };

                    // 查找该县的源数据文件
                    var sourceDataFile = sourceDataFiles.FirstOrDefault(f => f.DisplayName == countyName);
                    if (sourceDataFile != null)
                    {
                        countyInfo.SourceDataFile = sourceDataFile;

                        // 获取该文件的字段
                        var fields = GetFieldsFromFile(sourceDataFile);
                        foreach (var field in fields)
                        {
                            allFieldNames.Add(field);
                        }
                    }

                    // 查找该县的城镇开发边界数据
                    var czkfbjFile = czkfbjFiles.FirstOrDefault(f => f.DisplayName == countyName);
                    if (czkfbjFile != null)
                    {
                        countyInfo.CZKFBJFile = czkfbjFile;
                    }

                    selectedCountyData.Add(countyInfo);
                    System.Diagnostics.Debug.WriteLine($"县 {countyName}: 源数据={sourceDataFile?.FullPath ?? "无"}, CZKFBJ={czkfbjFile?.FullPath ?? "无"}");
                }

                // 将所有字段添加到下拉框
                var sortedFields = allFieldNames.OrderBy(name => name).ToList();
                foreach (var fieldName in sortedFields)
                {
                    cmbLandTypeField.Items.Add(fieldName);
                    cmbLandOwnerField.Items.Add(fieldName);
                }

                // 选择默认字段
                if (cmbLandTypeField.Items.Count > 0)
                {
                    int landTypeIndex = FindBestMatchIndex(cmbLandTypeField.Items, new[] { "DLBM" });
                    cmbLandTypeField.SelectedIndex = landTypeIndex >= 0 ? landTypeIndex : 0;

                    int landOwnerIndex = FindBestMatchIndex(cmbLandOwnerField.Items, new[] { "QSXZ" });
                    cmbLandOwnerField.SelectedIndex = landOwnerIndex >= 0 ? landOwnerIndex : 0;
                }

                UpdateStatus($"已加载 {selectedCounties.Count} 个县的数据，共 {sortedFields.Count} 个字段");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载字段时出错: {ex.Message}");
                UpdateStatus("加载字段失败");
                MessageBox.Show($"加载字段时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 从文件获取字段列表
        /// </summary>
        private List<string> GetFieldsFromFile(SourceDataFileInfo fileInfo)
        {
            var fields = new List<string>();

            try
            {
                if (fileInfo.IsGdb)
                {
                    // 从GDB要素类获取字段
                    var featureClass = ForestResourcePlugin.GdbFeatureClassFinder.OpenFeatureClassFromGdb(
                        fileInfo.FullPath, fileInfo.FeatureClassName);

                    if (featureClass != null)
                    {
                        fields = GeodatabaseUtilities.GetFeatureClassFields(featureClass);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                    }
                }
                else
                {
                    // 从Shapefile获取字段
                    fields = ShapefileReader.GetShapefileFieldNames(fileInfo.FullPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取文件字段时出错 ({fileInfo.FullPath}): {ex.Message}");
            }

            return fields;
        }

        /// <summary>
        /// 县列表选择变化事件处理
        /// </summary>
        private void ChkListCounties_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 使用BeginInvoke确保事件在UI更新后执行
            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    var selectedCounties = GetSelectedCounties();
                    System.Diagnostics.Debug.WriteLine($"选中的县: {string.Join(", ", selectedCounties)}");

                    // 更新字段下拉框
                    LoadFieldsFromSelectedCounties();

                    // 清空预览数据
                    ClearPreviewData();

                    UpdateStatus($"已选择 {selectedCounties.Count} 个县");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"处理县选择变化时出错: {ex.Message}");
                }
            }));
        }

        /// <summary>
        /// 获取选中的县名列表
        /// </summary>
        private List<string> GetSelectedCounties()
        {
            var selectedCounties = new List<string>();
            for (int i = 0; i < chkListCounties.Items.Count; i++)
            {
                if (chkListCounties.GetItemChecked(i))
                {
                    selectedCounties.Add(chkListCounties.Items[i].ToString());
                }
            }
            return selectedCounties;
        }

        private void LoadMapLayers()
        {
            try
            {
                // 设置下拉框为仅使用文件模式
                SetupDropdownsForFileMode();

                // 尝试从SharedDataManager加载已经找到的文件
                LoadSharedDataSources();

                UpdateStatus("已准备好使用共享数据源");
                progressBar.Value = 100;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化数据源失败: {ex.Message}");
                UpdateStatus("初始化数据源失败");
                progressBar.Value = 0;
            }
        }

        /// <summary>
        /// 设置下拉框为文件模式
        /// </summary>
        private void SetupDropdownsForFileMode()
        {
            try
            {
                // 清空图层列表
                mapLayers = new List<LayerInfo>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"设置下拉框失败: {ex.Message}");
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "请选择输出文件夹";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnRefreshLayers_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("用户点击刷新按钮");

            // 显示刷新状态
            btnRefreshLayers.Enabled = false;
            btnRefreshLayers.Text = "刷新中...";

            try
            {
                // 重新加载县数据
                LoadCountiesFromSharedData();
                UpdateStatus("数据源已刷新");
            }
            finally
            {
                // 恢复按钮状态
                btnRefreshLayers.Enabled = true;
                btnRefreshLayers.Text = "刷新数据源";
            }
        }

        // 城镇开发边界图层下拉框选择改变事件
        private void cmbCZKFBJPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"城镇开发边界图层选择改变，索引: {cmbCZKFBJPath.SelectedIndex}");

                // 如果选择的是提示项，则不执行任何操作
                if (cmbCZKFBJPath.SelectedIndex == 0)
                {
                    czkfbjFeatureClass = null;
                    System.Diagnostics.Debug.WriteLine("选择了提示项，清空要素类");
                    return;
                }

                // 获取选择的项
                object selectedItem = cmbCZKFBJPath.SelectedItem;
                System.Diagnostics.Debug.WriteLine($"选择的项类型: {selectedItem?.GetType().Name}");

                if (selectedItem is SourceDataFileInfo fileInfo)
                {
                    System.Diagnostics.Debug.WriteLine($"从共享数据源加载: {fileInfo.DisplayName}");
                    LoadCZKFBJFromPath(fileInfo.FullPath);
                }
                else if (selectedItem is string filePath && !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine($"从文件路径加载: {filePath}");
                    LoadCZKFBJFromPath(filePath);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"无效的选择项: {selectedItem}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载城镇开发边界图层失败: {ex.Message}");
                MessageBox.Show($"加载城镇开发边界图层失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("加载城镇开发边界图层失败");
            }
        }

        // 从文件路径加载城镇开发边界图层
        private void LoadCZKFBJFromPath(string path)
        {
            try
            {
                // 检查选择项是否为GDB要素类
                object selectedItem = cmbCZKFBJPath.SelectedItem;
                System.Diagnostics.Debug.WriteLine($"LoadCZKFBJFromPath: 当前选择项类型 {selectedItem?.GetType().Name ?? "null"}");

                if (selectedItem is ForestResourcePlugin.SourceDataFileInfo fileInfo)
                {
                    System.Diagnostics.Debug.WriteLine($"LoadCZKFBJFromPath: 文件信息 - IsGdb={fileInfo.IsGdb}, Path={fileInfo.FullPath}, FeatureClassName={fileInfo.FeatureClassName}");

                    if (fileInfo.IsGdb)
                    {
                        // 从GDB加载要素类
                        System.Diagnostics.Debug.WriteLine($"从GDB加载城镇开发边界图层: {fileInfo.FullPath}, 要素类: {fileInfo.FeatureClassName}");
                        try
                        {
                            czkfbjFeatureClass = ForestResourcePlugin.GdbFeatureClassFinder.OpenFeatureClassFromGdb(
                                fileInfo.FullPath, fileInfo.FeatureClassName);

                            if (czkfbjFeatureClass != null)
                            {
                                System.Diagnostics.Debug.WriteLine($"成功加载GDB要素类，要素数: {czkfbjFeatureClass.FeatureCount(null)}");
                                return;
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("GDB要素类加载失败: 返回为null");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"GDB要素类加载异常: {ex.Message}");
                            throw;
                        }
                    }
                }

                // 如果不是GDB要素类或加载GDB失败，尝试从Shapefile加载
                System.Diagnostics.Debug.WriteLine($"从Shapefile加载城镇开发边界图层: {path}");
                string directory = System.IO.Path.GetDirectoryName(path);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                // 创建工作空间工厂并打开Shapefile
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(directory, 0);
                czkfbjFeatureClass = featureWorkspace.OpenFeatureClass(fileName);

                if (czkfbjFeatureClass != null)
                {
                    System.Diagnostics.Debug.WriteLine($"成功加载Shapefile，要素数: {czkfbjFeatureClass.FeatureCount(null)}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Shapefile加载失败: 返回为null");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载城镇开发边界图层失败: {ex.Message}");
                throw new Exception($"加载城镇开发边界图层失败: {ex.Message}", ex);
            }
        }

        // Helper method to find the best matching field name
        // Helper method to find the best matching field name
        private int FindBestMatchIndex(ComboBox.ObjectCollection items, string[] searchTerms)
        {
            // 第一轮：精确匹配（忽略大小写）
            foreach (string term in searchTerms)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    string item = items[i].ToString();
                    if (item.Equals(term, StringComparison.OrdinalIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine($"FindBestMatchIndex: 精确匹配找到 '{term}' -> '{item}' (索引: {i})");
                        return i;
                    }
                }
            }

            // 第二轮：包含匹配（如果没有精确匹配）
            foreach (string term in searchTerms)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    string item = items[i].ToString();
                    if (item.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"FindBestMatchIndex: 包含匹配找到 '{term}' -> '{item}' (索引: {i})");
                        return i;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"FindBestMatchIndex: 未找到匹配项，搜索词: {string.Join(", ", searchTerms)}");
            return -1; // No match found
        }

        /// <summary>
        /// 生成多县预览数据
        /// </summary>
        private PreviewQueryResult GenerateMultiCountyPreview(string landTypeField, string landOwnerField, CancellationToken token)
        {
            var combinedResult = new PreviewQueryResult
            {
                PreviewData = new DataTable()
            };

            // 初始化数据表结构（增加县名列）
            combinedResult.PreviewData.Columns.Add("县名");
            combinedResult.PreviewData.Columns.Add("图斑编号");
            combinedResult.PreviewData.Columns.Add("地类");
            combinedResult.PreviewData.Columns.Add("土地权属");
            combinedResult.PreviewData.Columns.Add("面积(公顷)");

            int countyIndex = 0;
            foreach (var countyInfo in selectedCountyData)
            {
                try
                {
                    token.ThrowIfCancellationRequested();

                    if (countyInfo.SourceDataFile == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"县 {countyInfo.CountyName} 没有源数据，跳过");
                        continue;
                    }

                    countyIndex++;
                    UpdateStatus($"正在处理县 {countyInfo.CountyName} ({countyIndex}/{selectedCountyData.Count})...");

                    // 计算进度
                    int baseProgress = 20 + (countyIndex - 1) * 60 / selectedCountyData.Count;
                    progressBar.Value = baseProgress;

                    // 处理单个县的数据
                    var countyResult = ProcessSingleCountyPreview(countyInfo, landTypeField, landOwnerField, token);

                    // 合并结果
                    foreach (DataRow row in countyResult.PreviewData.Rows)
                    {
                        var newRow = combinedResult.PreviewData.NewRow();
                        newRow["县名"] = countyInfo.CountyName;
                        newRow["图斑编号"] = row["图斑编号"];
                        newRow["地类"] = row["地类"];
                        newRow["土地权属"] = row["土地权属"];
                        newRow["面积(公顷)"] = row["面积(公顷)"];

                        combinedResult.PreviewData.Rows.Add(newRow);
                    }

                    combinedResult.TotalCount += countyResult.TotalCount;
                    combinedResult.ProcessedCount += countyResult.ProcessedCount;

                    // 限制预览数量
                    if (combinedResult.ProcessedCount >= 1000)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"处理县 {countyInfo.CountyName} 时出错: {ex.Message}");
                    continue;
                }
            }

            return combinedResult;
        }

        /// <summary>
        /// 处理单个县的预览数据
        /// </summary>
        private PreviewQueryResult ProcessSingleCountyPreview(CountyDataInfo countyInfo, string landTypeField, string landOwnerField, CancellationToken token)
        {
            var result = new PreviewQueryResult
            {
                PreviewData = new DataTable()
            };

            // 初始化数据表结构（不包含县名列）
            result.PreviewData.Columns.Add("图斑编号");
            result.PreviewData.Columns.Add("地类");
            result.PreviewData.Columns.Add("土地权属");
            result.PreviewData.Columns.Add("面积(公顷)");

            try
            {
                // 加载要素类
                IFeatureClass sourceFeatureClass = LoadFeatureClass(countyInfo.SourceDataFile);
                IFeatureClass czkfbjFeatureClass = null;

                if (countyInfo.CZKFBJFile != null && chkCollectiveInBoundary.Checked)
                {
                    czkfbjFeatureClass = LoadFeatureClass(countyInfo.CZKFBJFile);
                }

                if (sourceFeatureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法加载县 {countyInfo.CountyName} 的源数据");
                    return result;
                }

                // 使用现有的查询逻辑处理单个要素类
                string optimizedWhereClause = BuildOptimizedWhereClause(landTypeField, landOwnerField);
                result = ExecuteOptimizedQueryForFeatureClass(
                    sourceFeatureClass,
                    czkfbjFeatureClass,
                    optimizedWhereClause,
                    landTypeField,
                    landOwnerField,
                    200, // 每个县最多200条记录用于预览
                    token);

                // 清理COM对象
                if (sourceFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceFeatureClass);
                if (czkfbjFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjFeatureClass);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理县 {countyInfo.CountyName} 预览时出错: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 为单个要素类执行优化查询
        /// </summary>
        private PreviewQueryResult ExecuteOptimizedQueryForFeatureClass(
            IFeatureClass sourceFeatureClass,
            IFeatureClass czkfbjFeatureClass,
            string whereClause,
            string landTypeField,
            string landOwnerField,
            int maxCount,
            CancellationToken token)
        {
            var result = new PreviewQueryResult
            {
                PreviewData = new DataTable()
            };

            // 初始化数据表结构
            result.PreviewData.Columns.Add("图斑编号");
            result.PreviewData.Columns.Add("地类");
            result.PreviewData.Columns.Add("土地权属");
            result.PreviewData.Columns.Add("面积(公顷)");

            try
            {
                // 创建查询过滤器
                IQueryFilter queryFilter = new QueryFilterClass();
                if (!string.IsNullOrEmpty(whereClause))
                {
                    queryFilter.WhereClause = whereClause;
                }

                // 获取字段索引
                var fieldIndices = GetFieldIndicesForFeatureClass(sourceFeatureClass, landTypeField, landOwnerField);

                // 空间过滤器
                ISpatialFilter cachedSpatialFilter = null;
                if (chkCollectiveInBoundary.Checked && czkfbjFeatureClass != null)
                {
                    cachedSpatialFilter = new SpatialFilterClass();
                    cachedSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                }

                IFeatureCursor cursor = null;
                IFeature feature = null;

                try
                {
                    cursor = sourceFeatureClass.Search(queryFilter, false);

                    while ((feature = cursor.NextFeature()) != null && result.ProcessedCount < maxCount)
                    {
                        token.ThrowIfCancellationRequested();
                        result.TotalCount++;

                        if (ShouldIncludeFeatureForCounty(feature, fieldIndices, cachedSpatialFilter, czkfbjFeatureClass))
                        {
                            var row = CreateDataRowForFeatureClass(feature, fieldIndices, result.PreviewData);
                            result.PreviewData.Rows.Add(row);
                            result.ProcessedCount++;
                        }

                        // 释放当前要素
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                        feature = null;
                    }
                }
                finally
                {
                    if (feature != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                    if (cursor != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"执行查询时出错: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 为特定要素类获取字段索引
        /// </summary>
        private FieldIndices GetFieldIndicesForFeatureClass(IFeatureClass featureClass, string landTypeField, string landOwnerField)
        {
            var indices = new FieldIndices();

            // 缓存字段索引以避免重复查找
            indices.TbdhIndex = featureClass.FindField("TBDH");
            if (indices.TbdhIndex == -1) indices.TbdhIndex = featureClass.FindField("BSM");
            if (indices.TbdhIndex == -1) indices.TbdhIndex = featureClass.FindField("OBJECTID");

            indices.DlmcIndex = featureClass.FindField(landTypeField);
            indices.TdqsIndex = featureClass.FindField(landOwnerField);
            indices.TbmjIndex = featureClass.FindField("TBMJ");
            indices.QsxzIndex = featureClass.FindField("QSXZ");

            // 备选面积字段
            if (indices.TbmjIndex == -1)
            {
                indices.TbmjIndex = featureClass.FindField("MJ");
                if (indices.TbmjIndex == -1) indices.TbmjIndex = featureClass.FindField("AREA");
                if (indices.TbmjIndex == -1) indices.TbmjIndex = featureClass.FindField("面积");
            }

            if (indices.QsxzIndex == -1 && indices.TdqsIndex != -1)
            {
                indices.QsxzIndex = indices.TdqsIndex;
            }

            return indices;
        }

        /// <summary>
        /// 判断是否应包含要素（针对特定县）
        /// </summary>
        private bool ShouldIncludeFeatureForCounty(IFeature feature, FieldIndices fieldIndices, ISpatialFilter spatialFilter, IFeatureClass czkfbjFeatureClass)
        {
            // 获取土地权属值
            string ownerValue = GetFieldValue(feature, fieldIndices.QsxzIndex, fieldIndices.TdqsIndex);

            // 国有林地直接添加
            if (chkStateOwned.Checked && (ownerValue == "10" || ownerValue == "20"))
            {
                return true;
            }

            // 集体林地需要检查是否在城镇开发边界内
            if (chkCollectiveInBoundary.Checked &&
                (ownerValue == "30" || ownerValue == "40") &&
                czkfbjFeatureClass != null)
            {
                return IsFeatureInBoundaryForCounty(feature, spatialFilter, czkfbjFeatureClass);
            }

            return false;
        }

        /// <summary>
        /// 检查要素是否在边界内（针对特定县）
        /// </summary>
        private bool IsFeatureInBoundaryForCounty(IFeature feature, ISpatialFilter spatialFilter, IFeatureClass czkfbjFeatureClass)
        {
            try
            {
                if (spatialFilter == null || czkfbjFeatureClass == null)
                    return false;

                spatialFilter.Geometry = feature.Shape;
                int count = czkfbjFeatureClass.FeatureCount(spatialFilter);
                return count > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"空间查询出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 为特定要素类创建数据行
        /// </summary>
        private DataRow CreateDataRowForFeatureClass(IFeature feature, FieldIndices fieldIndices, DataTable dataTable)
        {
            DataRow row = dataTable.NewRow();

            // 图斑编号
            row["图斑编号"] = fieldIndices.TbdhIndex != -1 ?
                feature.get_Value(fieldIndices.TbdhIndex)?.ToString() ?? feature.OID.ToString() :
                feature.OID.ToString();

            // 地类
            row["地类"] = fieldIndices.DlmcIndex != -1 ?
                feature.get_Value(fieldIndices.DlmcIndex)?.ToString() ?? "" : "";

            // 土地权属
            string ownerValue = GetFieldValue(feature, fieldIndices.QsxzIndex, fieldIndices.TdqsIndex);
            row["土地权属"] = TranslateOwnershipCode(ownerValue);

            // 面积
            if (fieldIndices.TbmjIndex != -1)
            {
                object mjValue = feature.get_Value(fieldIndices.TbmjIndex);
                if (mjValue != null && double.TryParse(mjValue.ToString(), out double mjDouble))
                {
                    row["面积(公顷)"] = mjDouble.ToString("F2");
                }
                else
                {
                    row["面积(公顷)"] = mjValue?.ToString() ?? "";
                }
            }
            else
            {
                row["面积(公顷)"] = "";
            }

            return row;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            btnExecute.Enabled = false;
            btnCancel.Enabled = true;
            btnPreview.Enabled = false;
            try
            {
                // 验证输入
                var selectedCounties = GetSelectedCounties();
                if (selectedCounties.Count == 0)
                {
                    MessageBox.Show("请至少选择一个县", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (selectedCountyData == null || selectedCountyData.Count == 0)
                {
                    MessageBox.Show("县数据未正确加载，请重新选择", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string landTypeField = cmbLandTypeField.SelectedItem?.ToString();
                string landOwnerField = cmbLandOwnerField.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(landTypeField) || string.IsNullOrEmpty(landOwnerField))
                {
                    MessageBox.Show("请选择地类字段和土地权属字段", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateStatus("正在生成预览...");
                progressBar.Value = 10;

                // 生成多县预览数据
                var allPreviewData = GenerateMultiCountyPreview(landTypeField, landOwnerField, token);

                // 显示结果
                DisplayPreviewResults(allPreviewData);

                progressBar.Value = 100;
                UpdateStatus("预览生成完成");

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (OperationCanceledException)
            {
                UpdateStatus("预览操作已取消");
                progressBar.Value = 0;
                ClearPreviewData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成预览时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("预览生成失败");
                progressBar.Value = 0;
                System.Diagnostics.Debug.WriteLine($"btnPreview_Click错误: {ex}");
            }
            finally
            {
                btnExecute.Enabled = true;
                btnCancel.Enabled = false;
                btnPreview.Enabled = true;
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }

        private bool ValidateInputs()
        {
            var selectedCounties = GetSelectedCounties();
            if (selectedCounties.Count == 0)
            {
                MessageBox.Show("请至少选择一个县", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtOutputPath.Text))
            {
                MessageBox.Show("请设置输出路径", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 清空预览数据
        /// </summary>
        private void ClearPreviewData()
        {
            if (previewData != null)
            {
                previewData.Clear();
                dgvPreview.DataSource = null;
                lblPreviewCount.Text = "预览结果：0 个图斑";
            }
        }

        private void DisplayPreviewResults(PreviewQueryResult result)
        {
            dgvPreview.DataSource = result.PreviewData;

            if (result.TotalCount > result.ProcessedCount)
            {
                lblPreviewCount.Text = $"预览结果：{result.ProcessedCount}/{result.TotalCount} 个图斑 (多县数据，仅显示前1000个)";
            }
            else
            {
                lblPreviewCount.Text = $"预览结果：{result.ProcessedCount} 个图斑";
            }
        }

        /// <summary>
        /// 加载要素类
        /// </summary>
        private IFeatureClass LoadFeatureClass(SourceDataFileInfo fileInfo)
        {
            try
            {
                if (fileInfo.IsGdb)
                {
                    return ForestResourcePlugin.GdbFeatureClassFinder.OpenFeatureClassFromGdb(
                        fileInfo.FullPath, fileInfo.FeatureClassName);
                }
                else
                {
                    string directory = System.IO.Path.GetDirectoryName(fileInfo.FullPath);
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullPath);

                    IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                    IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(directory, 0);
                    return featureWorkspace.OpenFeatureClass(fileName);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载要素类失败 ({fileInfo.FullPath}): {ex.Message}");
                return null;
            }
        }

        private void ExecuteProcessing()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            btnExecute.Enabled = false;
            btnCancel.Enabled = true;
            progressBar.Value = 0;
            totalProgressBar.Value = 0;

            try
            {
                UpdateStatus("开始处理数据...");
                progressBar.Value = 5;

                // 1. 验证输入
                if (!ValidateInputs())
                {
                    return;
                }

                // 2. 验证字段映射
                if (!ValidateFieldMapping())
                {
                    return;
                }

                progressBar.Value = 10;
                UpdateStatus("正在构建查询条件...");

                // 3. 获取字段信息
                string landTypeField = cmbLandTypeField.SelectedItem?.ToString();
                string landOwnerField = cmbLandOwnerField.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(landTypeField) || string.IsNullOrEmpty(landOwnerField))
                {
                    MessageBox.Show("请选择地类字段和土地权属字段", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                progressBar.Value = 20;
                UpdateStatus("开始批量处理县数据...");

                // 4. 执行多县批量处理
                var batchResults = ExecuteMultiCountyBatchProcessing(landTypeField, landOwnerField, token);

                totalProgressBar.Value = 100;
                progressBar.Value = 90;
                UpdateStatus("正在生成处理报告...");

                // 5. 显示批量处理结果
                DisplayBatchProcessingResults(batchResults);

                progressBar.Value = 100;
                UpdateStatus("批量处理完成！");

                // 6. 询问是否打开输出文件夹
                if (MessageBox.Show("是否打开输出文件夹？", "处理完成",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", txtOutputPath.Text);
                }
            }
            catch (OperationCanceledException)
            {
                progressBar.Value = 0;
                totalProgressBar.Value = 0;
                UpdateStatus("处理操作已取消");
                MessageBox.Show("处理操作已由用户取消。", "操作取消",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                progressBar.Value = 0;
                totalProgressBar.Value = 0;
                UpdateStatus("处理失败");
                MessageBox.Show($"处理过程中发生错误：\n{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"ExecuteProcessing错误: {ex}");
            }
            finally
            {
                btnExecute.Enabled = true;
                btnCancel.Enabled = false;
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }

        /// <summary>
        /// 执行多县批量处理
        /// </summary>
        private List<CountyProcessingResult> ExecuteMultiCountyBatchProcessing(string landTypeField, string landOwnerField, CancellationToken token)
        {
            var results = new List<CountyProcessingResult>();
            var selectedCounties = GetSelectedCounties();
            int totalCounties = selectedCounties.Count;
            int processedCounties = 0;

            foreach (var countyName in selectedCounties)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    processedCounties++;
                    UpdateTotalStatus($"正在处理县 {countyName} ({processedCounties}/{totalCounties})...");

                    // 计算总进度
                    int totalProgress = 20 + (processedCounties - 1) * 70 / totalCounties;
                    totalProgressBar.Value = totalProgress;

                    // 查找县数据
                    var countyInfo = selectedCountyData.FirstOrDefault(c => c.CountyName == countyName);
                    if (countyInfo?.SourceDataFile == null)
                    {
                        results.Add(new CountyProcessingResult
                        {
                            CountyName = countyName,
                            Success = false,
                            ErrorMessage = "未找到该县的源数据"
                        });
                        continue;
                    }

                    // 处理单个县
                    var countyResult = ProcessSingleCounty(countyInfo, landTypeField, landOwnerField, token);
                    results.Add(countyResult);

                    // 更新总进度
                    totalProgressBar.Value = totalProgress + (70 / totalCounties);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"处理县 {countyName} 时出错: {ex.Message}");
                    results.Add(new CountyProcessingResult
                    {
                        CountyName = countyName,
                        Success = false,
                        ErrorMessage = ex.Message
                    });
                }
            }

            return results;
        }

        /// <summary>
        /// 处理单个县
        /// </summary>
        private CountyProcessingResult ProcessSingleCounty(CountyDataInfo countyInfo, string landTypeField, string landOwnerField, CancellationToken token)
        {
            var result = new CountyProcessingResult
            {
                CountyName = countyInfo.CountyName,
                Success = false
            };

            try
            {
                // 加载要素类
                IFeatureClass sourceFeatureClass = LoadFeatureClass(countyInfo.SourceDataFile);
                IFeatureClass czkfbjFeatureClass = null;

                if (countyInfo.CZKFBJFile != null && chkCollectiveInBoundary.Checked)
                {
                    czkfbjFeatureClass = LoadFeatureClass(countyInfo.CZKFBJFile);
                }

                if (sourceFeatureClass == null)
                {
                    result.ErrorMessage = "无法加载源数据";
                    return result;
                }

                // 查询符合条件的要素
                var filteredFeatures = QueryFilteredFeaturesForCounty(
                    sourceFeatureClass,
                    czkfbjFeatureClass,
                    landTypeField,
                    landOwnerField,
                    token);

                if (filteredFeatures.Count == 0)
                {
                    result.Success = true;
                    result.ProcessedFeatureCount = 0;
                    result.OutputPath = "无符合条件的图斑";
                    return result;
                }

                // 创建县级输出文件
                //string countyOutputPath = System.IO.Path.Combine(txtOutputPath.Text, countyInfo.CountyName);
                //if (!Directory.Exists(countyOutputPath))
                //{
                //    Directory.CreateDirectory(countyOutputPath);
                //}

                // 构建县级数据库路径
                string countyDatabasePath = txtOutputPath.Text;

                // 获取字段映射
                var fieldMappings = GetFieldMappingsFromGrid();

                // 使用ShapefileExporter将筛选后的要素直接写入到SLZYZC图层
                var exporter = new ShapefileExporter();
                exporter.ExportToShapefile(
                    filteredFeatures,              // 已筛选的符合条件的要素列表
                    sourceFeatureClass,            // 源要素类，用于获取字段定义和要素数据
                    countyInfo.CountyName,         // 县名，用于确定目标数据库路径
                    countyDatabasePath,            // 数据库基础路径
                    fieldMappings,                 // 字段映射配置
                    (percentage, message) =>
                    {     // 进度回调函数
                        try
                        {
                            progressBar.Value = Math.Min(percentage, 100);
                            UpdateStatus(message);
                            Application.DoEvents();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"更新进度时出错: {ex.Message}");
                        }
                    });

                // 导出成功后设置处理结果
                result.Success = true;
                result.ProcessedFeatureCount = filteredFeatures.Count;
                result.OutputPath = System.IO.Path.Combine(countyDatabasePath, countyInfo.CountyName, $"{countyInfo.CountyName}.gdb");

                // 清理COM对象
                foreach (var feature in filteredFeatures)
                {
                    if (feature != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                }

                if (sourceFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceFeatureClass);
                if (czkfbjFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjFeatureClass);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"处理县 {countyInfo.CountyName} 时出错: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 为特定县查询符合条件的要素
        /// </summary>
        private List<IFeature> QueryFilteredFeaturesForCounty(
            IFeatureClass sourceFeatureClass,
            IFeatureClass czkfbjFeatureClass,
            string landTypeField,
            string landOwnerField,
            CancellationToken token)
        {
            var features = new List<IFeature>();

            try
            {
                string optimizedWhereClause = BuildOptimizedWhereClause(landTypeField, landOwnerField);

                IQueryFilter queryFilter = new QueryFilterClass();
                if (!string.IsNullOrEmpty(optimizedWhereClause))
                {
                    queryFilter.WhereClause = optimizedWhereClause;
                }

                var fieldIndices = GetFieldIndicesForFeatureClass(sourceFeatureClass, landTypeField, landOwnerField);

                ISpatialFilter cachedSpatialFilter = null;
                if (chkCollectiveInBoundary.Checked && czkfbjFeatureClass != null)
                {
                    cachedSpatialFilter = new SpatialFilterClass();
                    cachedSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                }

                IFeatureCursor cursor = null;
                IFeature feature = null;

                try
                {
                    cursor = sourceFeatureClass.Search(queryFilter, false);

                    while ((feature = cursor.NextFeature()) != null)
                    {
                        token.ThrowIfCancellationRequested();
                        if (ShouldIncludeFeatureForCounty(feature, fieldIndices, cachedSpatialFilter, czkfbjFeatureClass))
                        {
                            features.Add(feature);
                            feature = null; // 防止在finally中释放
                        }
                        else if (feature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                            feature = null;
                        }
                    }
                }
                finally
                {
                    if (feature != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                    }
                    if (cursor != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查询要素时出错: {ex.Message}");
                // 清理已分配的要素
                foreach (var f in features)
                {
                    if (f != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(f);
                }
                features.Clear();
                throw;
            }

            return features;
        }

        /// <summary>
        /// 显示批量处理结果
        /// </summary>
        private void DisplayBatchProcessingResults(List<CountyProcessingResult> results)
        {
            var successful = results.Where(r => r.Success).ToList();
            var failed = results.Where(r => !r.Success).ToList();

            string resultMessage = $"多县批量处理完成！\n\n";
            resultMessage += $"处理结果统计：\n";
            resultMessage += $"总县数：{results.Count}\n";
            resultMessage += $"成功处理：{successful.Count} 个县\n";
            resultMessage += $"处理失败：{failed.Count} 个县\n\n";

            if (successful.Count > 0)
            {
                resultMessage += "成功处理的县：\n";
                foreach (var result in successful)
                {
                    resultMessage += $"- {result.CountyName}: {result.ProcessedFeatureCount} 个图斑\n";
                }
                resultMessage += "\n";
            }

            if (failed.Count > 0)
            {
                resultMessage += "处理失败的县：\n";
                foreach (var result in failed)
                {
                    resultMessage += $"- {result.CountyName}: {result.ErrorMessage}\n";
                }
            }

            resultMessage += $"\n输出路径：{txtOutputPath.Text}";

            MessageBox.Show(resultMessage, "批量处理完成",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 县处理结果类
        /// </summary>
        private class CountyProcessingResult
        {
            public string CountyName { get; set; }
            public bool Success { get; set; }
            public int ProcessedFeatureCount { get; set; }
            public string OutputPath { get; set; }
            public string ErrorMessage { get; set; }
        }

        private string BuildOptimizedWhereClause(string landTypeField, string landOwnerField)
        {
            var conditions = new List<string>();

            if (chkForestLand.Checked)
            {
                // 优化: 使用单一条件而不是多个OR条件
                var subconditions = new List<string>();

                if (chkStateOwned.Checked)
                {
                    subconditions.Add($"{landOwnerField} IN ('10', '20')");
                }

                if (chkCollectiveInBoundary.Checked)
                {
                    subconditions.Add($"{landOwnerField} IN ('30', '40')");
                }

                if (subconditions.Count > 0)
                {
                    string ownerCondition = subconditions.Count == 1 ?
                        subconditions[0] :
                        $"({string.Join(" OR ", subconditions)})";

                    // 定义林地的精确地类编码列表
                    string landTypeCodes = "'0301', '0302', '0305', '0307', '0301K', '0302K', '0307K'";
                    string landTypeCondition = $"{landTypeField} IN ({landTypeCodes})";

                    conditions.Add($"{landTypeCondition} AND {ownerCondition}");
                }
            }

            return conditions.Count > 0 ? string.Join(" OR ", conditions) : "";
        }

        private string GetFieldValue(IFeature feature, int primaryIndex, int fallbackIndex)
        {
            if (primaryIndex != -1)
            {
                return feature.get_Value(primaryIndex)?.ToString() ?? "";
            }
            else if (fallbackIndex != -1)
            {
                return feature.get_Value(fallbackIndex)?.ToString() ?? "";
            }
            return "";
        }

        private string TranslateOwnershipCode(string ownerValue)
        {
            switch (ownerValue)
            {
                case "10":
                case "20":
                    return "国有";
                case "30":
                case "40":
                    return "集体";
                default:
                    return ownerValue;
            }
        }

        /// <summary>
        /// 从映射表格获取字段映射配置
        /// </summary>
        private Dictionary<string, string> GetFieldMappingsFromGrid()
        {
            var fieldMappings = new Dictionary<string, string>();

            try
            {
                if (mappingData != null && mappingData.Rows.Count > 0)
                {
                    foreach (DataRow row in mappingData.Rows)
                    {
                        string targetField = row["目标字段"]?.ToString();
                        string sourceField = row["源字段"]?.ToString();
                        string status = row["映射状态"]?.ToString();

                        // 只添加已映射且有效的字段
                        if (!string.IsNullOrEmpty(targetField) &&
                            !string.IsNullOrEmpty(sourceField) &&
                            status == "已映射")
                        {
                            fieldMappings[targetField] = sourceField;
                        }
                    }
                }

                // 如果映射表格为空或没有有效映射，使用默认映射
                if (fieldMappings.Count == 0)
                {
                    fieldMappings = GetDefaultFieldMappings();
                }

                System.Diagnostics.Debug.WriteLine($"获取字段映射配置完成，共 {fieldMappings.Count} 个映射");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取字段映射时出错: {ex.Message}");
                // 返回默认映射
                fieldMappings = GetDefaultFieldMappings();
            }

            return fieldMappings;
        }

        /// <summary>
        /// 获取默认字段映射配置
        /// </summary>
        private Dictionary<string, string> GetDefaultFieldMappings()
        {
            var defaultMappings = new Dictionary<string, string>();

            try
            {
                // 获取当前选择的字段
                string landTypeField = cmbLandTypeField.SelectedItem?.ToString();
                string landOwnerField = cmbLandOwnerField.SelectedItem?.ToString();

                // 基本字段映射
                if (!string.IsNullOrEmpty(landTypeField))
                {
                    defaultMappings["DLMC"] = landTypeField;
                }

                if (!string.IsNullOrEmpty(landOwnerField))
                {
                    defaultMappings["TDQS"] = landOwnerField;
                    defaultMappings["QSXZ"] = landOwnerField;
                }

                // 其他常用字段映射
                var fieldNames = GetAvailableSourceFields();

                // 图斑编号
                string tbdhField = fieldNames.FirstOrDefault(f =>
                    f.Equals("TBDH", StringComparison.OrdinalIgnoreCase) ||
                    f.Equals("BSM", StringComparison.OrdinalIgnoreCase) ||
                    f.Equals("图斑编号", StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(tbdhField))
                {
                    defaultMappings["TBDH"] = tbdhField;
                }

                // 面积字段
                string mjField = fieldNames.FirstOrDefault(f =>
                    f.Equals("TBMJ", StringComparison.OrdinalIgnoreCase) ||
                    f.Equals("MJ", StringComparison.OrdinalIgnoreCase) ||
                    f.Equals("AREA", StringComparison.OrdinalIgnoreCase) ||
                    f.Equals("面积", StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(mjField))
                {
                    defaultMappings["MJ"] = mjField;
                    defaultMappings["TBMJ"] = mjField;
                }

                System.Diagnostics.Debug.WriteLine($"生成默认字段映射，共 {defaultMappings.Count} 个映射");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成默认字段映射时出错: {ex.Message}");
            }

            return defaultMappings;
        }

        /// <summary>
        /// 验证字段映射配置
        /// </summary>
        private bool ValidateFieldMapping()
        {
            try
            {
                var fieldMappings = GetFieldMappingsFromGrid();

                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    MessageBox.Show("字段映射配置为空，请配置字段映射或确保已选择相关字段",
                        "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // 验证源字段是否存在
                var availableFields = GetAvailableSourceFields();
                var invalidMappings = new List<string>();

                foreach (var mapping in fieldMappings)
                {
                    if (!availableFields.Contains(mapping.Value))
                    {
                        invalidMappings.Add($"{mapping.Key} -> {mapping.Value}");
                    }
                }

                if (invalidMappings.Count > 0)
                {
                    string message = $"以下字段映射的源字段不存在：\n{string.Join("\n", invalidMappings)}\n\n请检查并修正字段映射配置。";
                    MessageBox.Show(message, "字段映射验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                System.Diagnostics.Debug.WriteLine("字段映射验证通过");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"验证字段映射时出错: {ex.Message}");
                MessageBox.Show($"验证字段映射时出错: {ex.Message}", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void InitializeMappingGrid()
        {
            mappingData = new DataTable();
            mappingData.Columns.Add("目标字段");
            mappingData.Columns.Add("源字段");
            mappingData.Columns.Add("映射状态");

            // 更新为SLZYZC相关字段
            string[] targetFields = { "YSDM", "XZQDM", "GTDCTBBSM", "GTDCDLBM", "GTDCDLMC", "QSDWDM", "QSDWMC", "GTDCTBMJ", "GTDCTDQS" };
            foreach (string field in targetFields)
            {
                mappingData.Rows.Add(field, "", "未映射");
            }

            dgvMapping.DataSource = mappingData;
        }

        private void btnAutoMapping_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("正在执行自动字段映射...");

                // 1. 获取可用的源字段列表
                var availableSourceFields = GetAvailableSourceFields();
                if (availableSourceFields.Count == 0)
                {
                    MessageBox.Show("无法进行自动映射，因为没有可用的源字段。\n\n请先在左侧选择至少一个县。",
                        "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateStatus("自动映射失败：无源字段");
                    return;
                }

                // 2. 获取SLZYZC字段映射规则
                var defaultMappings = GetDefaultSLZYZCMappingRules();

                // 3. 清空并重新填充映射表格
                mappingData.Clear();
                int mappedCount = 0;

                foreach (var mapping in defaultMappings)
                {
                    string targetField = mapping.Key;
                    string idealSourceField = mapping.Value;

                    // 查找最佳匹配的源字段（不区分大小写）
                    string matchedSourceField = availableSourceFields.FirstOrDefault(f =>
                        f.Equals(idealSourceField, StringComparison.OrdinalIgnoreCase));

                    string status;
                    if (!string.IsNullOrEmpty(matchedSourceField))
                    {
                        status = "已映射";
                        mappedCount++;
                    }
                    else
                    {
                        status = "未映射";
                        matchedSourceField = ""; // 如果未找到匹配，则源字段为空
                    }

                    mappingData.Rows.Add(targetField, matchedSourceField, status);
                }

                // 刷新表格显示
                dgvMapping.DataSource = mappingData;
                dgvMapping.Refresh();

                UpdateStatus($"自动映射完成，成功映射 {mappedCount} / {defaultMappings.Count} 个字段");
                MessageBox.Show($"自动映射完成！\n\n成功匹配 {mappedCount} 个字段。", "操作成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"自动映射时发生错误: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("自动映射失败");
                System.Diagnostics.Debug.WriteLine($"btnAutoMapping_Click 错误: {ex}");
            }
        }

        /// <summary>
        /// 获取SLZYZC字段映射规则
        /// </summary>
        private Dictionary<string, string> GetDefaultSLZYZCMappingRules()
        {
            return new Dictionary<string, string>
            {
                { "YSDM", "ysdm" },            // 要素代码
                { "XZQDM", "xian" },           // 行政区代码
                { "GTDCTBBSM", "bsm" },        // 国土调查图斑编码
                { "GTDCTBBH", "tbbh" },        // 国土调查图斑编号
                { "GTDCDLBM", "dlbm" },        // 国土调查地类编码
                { "GTDCDLMC", "dlmc" },        // 国土调查地类名称
                { "QSDWDM", "qsdwdm" },        // 权属单位代码
                { "QSDWMC", "qsdwmc" },        // 权属单位名称
                { "ZLDWDM", "zldwdm" },        // 坐落单位代码
                { "ZLDWMC", "zldwmc" },        // 坐落单位名称
                { "GTDCTBMJ", "tbmj" },        // 国土调查图斑面积
                { "GTDCTDQS", "qsxz" },        // 国土调查土地权属
                { "LYJ", "lin_ye_ju" },        // 林业局
                { "LC", "lin_chang" },         // 林场
                { "ZTBMJ", "xbmj" },           // 株数图斑面积
                { "LM_SUOYQ", "lmqs" },        // 林木所有权
                { "LZ", "lin_zhong" },         // 林种
                { "YSSZ", "you_shi_sz" },      // 优势树种
                { "QY", "qi_yuan" },           // 起源
                { "YBD", "yu_bi_du" },         // 郁闭度
                { "PJNL", "pingjun_nl" },      // 平均年龄
                { "LING_ZU", "ling_zu" },      // 龄组
                { "PJSG", "pingjun_sg" },      // 平均树高
                { "PJXJ", "pingjun_xj" },      // 平均胸径
                { "MGQZS", "mei_gq_zs" },      // 每公顷株数
                { "FRDBS", "frdbs" }           // 发育地被层
            };
        }

        private void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "映射模板 (*.xml)|*.xml|All Files (*.*)|*.*";
                dialog.Title = "加载字段映射模板";
                dialog.InitialDirectory = System.IO.Path.Combine(Application.StartupPath, "Templates");

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        LoadFieldMappingTemplate(dialog.FileName);
                        UpdateStatus($"已加载模板：{System.IO.Path.GetFileName(dialog.FileName)}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"加载模板失败：{ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        UpdateStatus("加载模板失败");
                    }
                }
            }
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "映射模板 (*.xml)|*.xml|All Files (*.*)|*.*";
                dialog.Title = "保存字段映射模板";
                dialog.InitialDirectory = System.IO.Path.Combine(Application.StartupPath, "Templates");
                dialog.FileName = "SLZYZCFieldMappingTemplate.xml";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        SaveFieldMappingTemplate(dialog.FileName);
                        UpdateStatus($"模板已保存：{System.IO.Path.GetFileName(dialog.FileName)}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"保存模板失败：{ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        UpdateStatus("保存模板失败");
                    }
                }
            }
        }

        /// <summary>
        /// 加载字段映射模板
        /// </summary>
        private void LoadFieldMappingTemplate(string templatePath)
        {
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"模板文件不存在：{templatePath}");
            }

            try
            {
                var template = LoadTemplateFromXml(templatePath);
                ApplyTemplateToMappingGrid(template);

                MessageBox.Show($"成功加载模板：{template.Name}\n描述：{template.Description}",
                    "模板加载成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception($"解析模板文件失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 保存字段映射模板
        /// </summary>
        private void SaveFieldMappingTemplate(string templatePath)
        {
            try
            {
                var template = CreateTemplateFromMappingGrid();
                SaveTemplateToXml(template, templatePath);

                MessageBox.Show("模板保存成功！", "保存完成",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存模板文件失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从 XML 文件加载模板
        /// </summary>
        private FieldMappingTemplate LoadTemplateFromXml(string xmlPath)
        {
            var template = new FieldMappingTemplate();
            var doc = new System.Xml.XmlDocument();
            doc.Load(xmlPath);

            // 读取模板信息
            var infoNode = doc.SelectSingleNode("//TemplateInfo");
            if (infoNode != null)
            {
                template.Name = infoNode.SelectSingleNode("Name")?.InnerText ?? "";
                template.Description = infoNode.SelectSingleNode("Description")?.InnerText ?? "";
                template.Version = infoNode.SelectSingleNode("Version")?.InnerText ?? "";

                if (DateTime.TryParse(infoNode.SelectSingleNode("CreateDate")?.InnerText, out DateTime createDate))
                {
                    template.CreateDate = createDate;
                }
            }

            // 读取字段映射
            var mappingNodes = doc.SelectNodes("//FieldMappings/Mapping");
            foreach (System.Xml.XmlNode node in mappingNodes)
            {
                var mapping = new FieldMapping
                {
                    TargetField = node.SelectSingleNode("TargetField")?.InnerText ?? "",
                    SourceField = node.SelectSingleNode("SourceField")?.InnerText ?? "",
                    FieldType = node.SelectSingleNode("FieldType")?.InnerText ?? "",
                    Required = bool.Parse(node.SelectSingleNode("Required")?.InnerText ?? "false"),
                    Description = node.SelectSingleNode("Description")?.InnerText ?? ""
                };

                // 读取备选字段
                var altFields = node.SelectNodes("AlternativeFields/Field");
                foreach (System.Xml.XmlNode altField in altFields)
                {
                    mapping.AlternativeFields.Add(altField.InnerText);
                }

                template.FieldMappings.Add(mapping);
            }

            // 读取值映射
            var valueMappingNodes = doc.SelectNodes("//ValueMappings/ValueMapping");
            foreach (System.Xml.XmlNode node in valueMappingNodes)
            {
                var valueMapping = new ValueMapping
                {
                    TargetField = node.Attributes?["TargetField"]?.Value ?? ""
                };

                var valueMapNodes = node.SelectNodes("ValueMap");
                foreach (System.Xml.XmlNode mapNode in valueMapNodes)
                {
                    string sourceValue = mapNode.SelectSingleNode("SourceValue")?.InnerText ?? "";
                    string targetValue = mapNode.SelectSingleNode("TargetValue")?.InnerText ?? "";
                    valueMapping.ValueMap[sourceValue] = targetValue;
                }

                template.ValueMappings.Add(valueMapping);
            }

            return template;
        }

        /// <summary>
        /// 将模板保存到 XML 文件
        /// </summary>
        private void SaveTemplateToXml(FieldMappingTemplate template, string xmlPath)
        {
            var doc = new System.Xml.XmlDocument();
            var root = doc.CreateElement("FieldMappingTemplate");
            doc.AppendChild(root);

            // 创建模板信息节点
            var infoNode = doc.CreateElement("TemplateInfo");
            infoNode.AppendChild(CreateTextElement(doc, "Name", template.Name));
            infoNode.AppendChild(CreateTextElement(doc, "Description", template.Description));
            infoNode.AppendChild(CreateTextElement(doc, "Version", template.Version));
            infoNode.AppendChild(CreateTextElement(doc, "CreateDate", template.CreateDate.ToString("yyyy-MM-dd")));
            root.AppendChild(infoNode);

            // 创建字段映射节点
            var mappingsNode = doc.CreateElement("FieldMappings");
            foreach (var mapping in template.FieldMappings)
            {
                var mappingNode = doc.CreateElement("Mapping");
                mappingNode.AppendChild(CreateTextElement(doc, "TargetField", mapping.TargetField));
                mappingNode.AppendChild(CreateTextElement(doc, "SourceField", mapping.SourceField));
                mappingNode.AppendChild(CreateTextElement(doc, "FieldType", mapping.FieldType));
                mappingNode.AppendChild(CreateTextElement(doc, "Required", mapping.Required.ToString()));
                mappingNode.AppendChild(CreateTextElement(doc, "Description", mapping.Description));

                // 添加备选字段
                if (mapping.AlternativeFields.Count > 0)
                {
                    var altFieldsNode = doc.CreateElement("AlternativeFields");
                    foreach (var altField in mapping.AlternativeFields)
                    {
                        altFieldsNode.AppendChild(CreateTextElement(doc, "Field", altField));
                    }
                    mappingNode.AppendChild(altFieldsNode);
                }

                mappingsNode.AppendChild(mappingNode);
            }
            root.AppendChild(mappingsNode);

            // 保存文档
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(xmlPath));
            doc.Save(xmlPath);
        }

        /// <summary>
        /// 应用模板到映射表格
        /// </summary>
        private void ApplyTemplateToMappingGrid(FieldMappingTemplate template)
        {
            // 清空现有数据
            mappingData.Clear();

            // 获取当前可用的源字段列表
            List<string> availableSourceFields = GetAvailableSourceFields();

            // 应用模板映射
            foreach (var mapping in template.FieldMappings)
            {
                string selectedSourceField = "";

                // 首先尝试使用模板中指定的源字段
                if (availableSourceFields.Contains(mapping.SourceField))
                {
                    selectedSourceField = mapping.SourceField;
                }
                else
                {
                    // 如果指定的源字段不存在，尝试使用备选字段
                    foreach (var altField in mapping.AlternativeFields)
                    {
                        if (availableSourceFields.Contains(altField))
                        {
                            selectedSourceField = altField;
                            break;
                        }
                    }
                }

                string status = string.IsNullOrEmpty(selectedSourceField) ? "未映射" : "已映射";
                mappingData.Rows.Add(mapping.TargetField, selectedSourceField, status);
            }

            dgvMapping.Refresh();
        }

        /// <summary>
        /// 从映射表格创建模板
        /// </summary>
        private FieldMappingTemplate CreateTemplateFromMappingGrid()
        {
            var template = new FieldMappingTemplate
            {
                Name = "SLZYZC字段映射模板",
                Description = "基于当前SLZYZC字段映射配置生成的模板",
                Version = "1.0",
                CreateDate = DateTime.Now
            };

            foreach (DataRow row in mappingData.Rows)
            {
                var mapping = new FieldMapping
                {
                    TargetField = row["目标字段"].ToString(),
                    SourceField = row["源字段"].ToString(),
                    FieldType = "String", // 默认字符串类型
                    Required = IsRequiredField(row["目标字段"].ToString()),
                    Description = GetFieldDescription(row["目标字段"].ToString())
                };

                template.FieldMappings.Add(mapping);
            }

            return template;
        }

        /// <summary>
        /// 获取可用的源字段列表
        /// </summary>
        private List<string> GetAvailableSourceFields()
        {
            var fields = new List<string>();

            if (cmbLandTypeField.Items.Count > 0)
            {
                foreach (object item in cmbLandTypeField.Items)
                {
                    fields.Add(item.ToString());
                }
            }

            return fields.Distinct().ToList();
        }

        /// <summary>
        /// 判断字段是否必需
        /// </summary>
        private bool IsRequiredField(string fieldName)
        {
            string[] requiredFields = { "YSDM", "XZQDM", "GTDCTBBSM", "GTDCDLBM", "GTDCDLMC", "QSDWDM", "GTDCTBMJ" };
            return requiredFields.Contains(fieldName);
        }

        /// <summary>
        /// 获取字段描述
        /// </summary>
        private string GetFieldDescription(string fieldName)
        {
            var descriptions = new Dictionary<string, string>
            {
                { "YSDM", "要素代码" },
                { "XZQDM", "行政区代码" },
                { "GTDCTBBSM", "国土调查图斑编码" },
                { "GTDCDLBM", "国土调查地类编码" },
                { "GTDCDLMC", "国土调查地类名称" },
                { "QSDWDM", "权属单位代码" },
                { "QSDWMC", "权属单位名称" },
                { "GTDCTBMJ", "国土调查图斑面积" },
                { "GTDCTDQS", "国土调查土地权属" }
            };

            return descriptions.ContainsKey(fieldName) ? descriptions[fieldName] : "";
        }

        /// <summary>
        /// 创建文本元素
        /// </summary>
        private System.Xml.XmlElement CreateTextElement(System.Xml.XmlDocument doc, string name, string value)
        {
            var element = doc.CreateElement(name);
            element.InnerText = value ?? "";
            return element;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                ExecuteProcessing();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消处理逻辑
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                UpdateStatus("正在取消操作...");
            }
        }

        private void UpdateStatus(string message)
        {
            lblStatus.Text = $"状态：{message}";
            Application.DoEvents();
        }

        private void UpdateTotalStatus(string message)
        {
            lblTotalStatus.Text = $"总状态：{message}";
            Application.DoEvents();
        }

        // 复选框状态改变事件处理
        private void FilterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"筛选条件复选框状态改变: {((CheckBox)sender).Name} = {((CheckBox)sender).Checked}");

                // 当任何筛选条件变化时清空预览数据
                if (previewData != null)
                {
                    previewData.Clear();
                    dgvPreview.DataSource = null;
                    lblPreviewCount.Text = "预览结果：0 个图斑";

                    // 提示用户重新生成预览
                    UpdateStatus("筛选条件已更改，请重新生成预览");
                }

                // 根据复选框状态更新界面状态
                CheckBox checkbox = sender as CheckBox;
                if (checkbox != null)
                {
                    switch (checkbox.Name)
                    {
                        case "chkCollectiveInBoundary":
                            // 如果启用了集体林在城镇开发边界内的筛选，确保城镇开发边界图层已加载
                            if (checkbox.Checked && czkfbjFeatureClass == null)
                            {
                                UpdateStatus("启用了集体林筛选条件，请确保已选择城镇开发边界图层");
                            }
                            break;
                        case "chkForestLand":
                            // 如果禁用了林地筛选，提示用户
                            if (!checkbox.Checked)
                            {
                                UpdateStatus("已禁用林地筛选条件");
                            }
                            break;
                        case "chkStateOwned":
                            // 如果禁用了国有林筛选，提示用户
                            if (!checkbox.Checked)
                            {
                                UpdateStatus("已禁用国有林筛选条件");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理筛选条件变化时出错: {ex.Message}");
                UpdateStatus("处理筛选条件变化时出错");
            }
        }

        private void btnPreview_MouseCaptureChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 从SharedDataManager加载共享的数据源
        /// </summary>
        private void LoadSharedDataSources()
        {
            try
            {
                // 获取城镇开发边界文件列表
                var czkfbjFiles = ForestResourcePlugin.SharedDataManager.GetCZKFBJFiles();
                System.Diagnostics.Debug.WriteLine($"从SharedDataManager加载 {czkfbjFiles.Count} 个CZKFBJ文件");

                // 清空下拉框
                cmbCZKFBJPath.Items.Clear();

                // 添加提示项
                cmbCZKFBJPath.Items.Add("-- 请选择城镇开发边界图层 --");

                // 添加找到的文件
                if (czkfbjFiles.Count > 0)
                {
                    foreach (var file in czkfbjFiles)
                    {
                        cmbCZKFBJPath.Items.Add(file);
                        System.Diagnostics.Debug.WriteLine($"添加CZKFBJ文件到下拉框: {file.DisplayName} -> {file.FullPath}");
                    }

                    // 如果有文件，选择第一个文件项（索引为1）
                    if (cmbCZKFBJPath.Items.Count > 1)
                    {
                        cmbCZKFBJPath.SelectedIndex = 1;
                        System.Diagnostics.Debug.WriteLine($"自动选择第一个CZKFBJ文件: {((SourceDataFileInfo)cmbCZKFBJPath.SelectedItem).DisplayName}");
                    }
                    else
                    {
                        cmbCZKFBJPath.SelectedIndex = 0;
                    }
                }
                else
                {
                    // 没有文件时选择提示项
                    cmbCZKFBJPath.SelectedIndex = 0;
                    System.Diagnostics.Debug.WriteLine("没有CZKFBJ文件可用");
                }

                // 更新状态提示
                if (czkfbjFiles.Count > 0)
                {
                    UpdateStatus($"已加载共享数据源: {czkfbjFiles.Count}个城镇开发边界文件");
                }
                else
                {
                    UpdateStatus("未找到共享数据源");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载共享数据源时出错: {ex.Message}");
                UpdateStatus("加载共享数据源失败");
            }
        }

        // 新增方法：加载县列表
        /// <summary>
        /// 加载县列表 - 从文件夹结构中提取县名
        /// </summary>
        private void LoadCounties()
        {
            try
            {
                // 从SharedDataManager获取县名列表
                var countyNames = GetCountyNamesFromDataSources();

                chkListCounties.Items.Clear();

                if (countyNames.Count > 0)
                {
                    // 按字母顺序排序县名
                    var sortedCounties = countyNames.OrderBy(name => name).ToList();

                    foreach (var countyName in sortedCounties)
                    {
                        chkListCounties.Items.Add(countyName, false); // 默认不选中
                    }

                    UpdateStatus($"已加载 {countyNames.Count} 个县");
                    System.Diagnostics.Debug.WriteLine($"LoadCounties完成: {string.Join(", ", sortedCounties)}");
                }
                else
                {
                    UpdateStatus("未找到县数据，请先在基础数据准备中选择数据源");
                    System.Diagnostics.Debug.WriteLine("LoadCounties: 未找到任何县数据");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadCounties出错: {ex.Message}");
                UpdateStatus("加载县列表失败");
                MessageBox.Show($"加载县列表时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 从数据源中获取所有唯一的县名
        /// </summary>
        private HashSet<string> GetCountyNamesFromDataSources()
        {
            var countyNames = new HashSet<string>();

            try
            {
                // 获取源数据文件列表（原LCXZGX文件列表）
                var sourceDataFiles = SharedDataManager.GetSourceDataFiles();
                System.Diagnostics.Debug.WriteLine($"获取到 {sourceDataFiles.Count} 个源数据文件");

                // 从源数据文件中提取县名
                foreach (var file in sourceDataFiles)
                {
                    if (!string.IsNullOrEmpty(file.DisplayName))
                    {
                        countyNames.Add(file.DisplayName);
                        System.Diagnostics.Debug.WriteLine($"源数据文件县名: {file.DisplayName}");
                    }
                }

                // 获取城镇开发边界文件列表
                var czkfbjFiles = SharedDataManager.GetCZKFBJFiles();
                System.Diagnostics.Debug.WriteLine($"获取到 {czkfbjFiles.Count} 个城镇开发边界文件");

                // 从城镇开发边界文件中提取县名
                foreach (var file in czkfbjFiles)
                {
                    if (!string.IsNullOrEmpty(file.DisplayName))
                    {
                        countyNames.Add(file.DisplayName);
                        System.Diagnostics.Debug.WriteLine($"城镇开发边界文件县名: {file.DisplayName}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"共找到 {countyNames.Count} 个唯一县名: {string.Join(", ", countyNames)}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取县名时出错: {ex.Message}");
            }

            return countyNames;
        }

        /// <summary>
        /// 全选/取消全选县列表
        /// </summary>
        private void ToggleAllCounties(bool selectAll)
        {
            try
            {
                for (int i = 0; i < chkListCounties.Items.Count; i++)
                {
                    chkListCounties.SetItemChecked(i, selectAll);
                }

                string action = selectAll ? "全选" : "取消全选";
                UpdateStatus($"已{action} {chkListCounties.Items.Count} 个县");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"切换县选择状态时出错: {ex.Message}");
            }
        }

        private void Basic_Load(object sender, EventArgs e)
        {

        }

        private void cbxSelectAllCounty_Click(object sender, EventArgs e)
        {
            // 检查事件发送者是否为CheckBox
            if (sender is CheckBox cb)
            {
                // 调用已有的方法来切换所有县的选中状态
                ToggleAllCounties(cb.Checked);
            }
        }

        // 新增辅助类
        private class FieldIndices
        {
            public int TbdhIndex { get; set; } = -1;
            public int DlmcIndex { get; set; } = -1;
            public int TdqsIndex { get; set; } = -1;
            public int TbmjIndex { get; set; } = -1;
            public int QsxzIndex { get; set; } = -1;
        }

        private class PreviewQueryResult
        {
            public DataTable PreviewData { get; set; }
            public int TotalCount { get; set; } = 0;
            public int ProcessedCount { get; set; } = 0;
        }
    }

    /// <summary>
    /// 字段映射配置类
    /// </summary>
    public class FieldMapping
    {
        public string TargetField { get; set; }
        public string SourceField { get; set; }
        public string FieldType { get; set; }
        public bool Required { get; set; }
        public string Description { get; set; }
        public List<string> AlternativeFields { get; set; } = new List<string>();
    }

    /// <summary>
    /// 值映射配置类
    /// </summary>
    public class ValueMapping
    {
        public string TargetField { get; set; }
        public Dictionary<string, string> ValueMap { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 字段映射模板类
    /// </summary>
    public class FieldMappingTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public DateTime CreateDate { get; set; }
        public List<FieldMapping> FieldMappings { get; set; } = new List<FieldMapping>();
        public List<ValueMapping> ValueMappings { get; set; } = new List<ValueMapping>();
    }
}
