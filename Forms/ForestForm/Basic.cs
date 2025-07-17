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
                // 获取源数据文件列表
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
                { "PCDL", "di_lei" },          // 普查地类
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

        private void buttonLDHSJGPath_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "请选择包含LDHSJG数据的根文件夹";
                    dialog.ShowNewFolderButton = false;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedPath = dialog.SelectedPath;

                        // 显示选择的路径到textbox（假设textbox名称为txtLDHSJGPath）
                        // 如果textbox不存在，您需要添加一个
                        if (this.Controls.Find("txtLDHSJGPath", true).FirstOrDefault() is TextBox txtLDHSJG)
                        {
                            txtLDHSJG.Text = selectedPath;
                        }

                        // 查找并匹配LDHSJG文件
                        var ldhsjgFiles = FindAndMatchLDHSJGFiles(selectedPath);

                        // 显示匹配结果
                        DisplayLDHSJGMatchResults(ldhsjgFiles);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择LDHSJG数据路径时出错：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("选择LDHSJG数据路径失败");
                System.Diagnostics.Debug.WriteLine($"buttonLDHSJGPath_Click出错: {ex}");
            }
        }

        /// <summary>
        /// 查找并匹配LDHSJG文件到对应的县
        /// </summary>
        /// <param name="rootPath">根文件夹路径</param>
        /// <returns>匹配结果列表</returns>
        /// <summary>
        /// 查找并匹配LDHSJG文件到对应的县
        /// </summary>
        /// <param name="rootPath">根文件夹路径</param>
        /// <returns>匹配结果列表</returns>
        private List<LDHSJGFileInfo> FindAndMatchLDHSJGFiles(string rootPath)
        {
            var ldhsjgFiles = new List<LDHSJGFileInfo>();

            try
            {
                UpdateStatus("正在搜索LDHSJG数据文件...");

                // 获取已加载的县名列表
                var availableCounties = GetCountyNamesFromDataSources();
                System.Diagnostics.Debug.WriteLine($"可用县名: {string.Join(", ", availableCounties)}");

                // 直接遍历根目录下的所有文件夹（第一级），这些文件夹名称包含县名
                var firstLevelDirectories = Directory.GetDirectories(rootPath);
                System.Diagnostics.Debug.WriteLine($"找到 {firstLevelDirectories.Length} 个第一级目录");

                foreach (var firstLevelDir in firstLevelDirectories)
                {
                    string directoryName = System.IO.Path.GetFileName(firstLevelDir);
                    System.Diagnostics.Debug.WriteLine($"正在处理第一级目录: {directoryName}");

                    // 尝试从第一级目录名中提取县名
                    string extractedCountyName = ExtractCountyNameFromDirectory(directoryName, availableCounties);

                    if (!string.IsNullOrEmpty(extractedCountyName))
                    {
                        System.Diagnostics.Debug.WriteLine($"成功匹配县名: {directoryName} -> {extractedCountyName}");

                        // 在该目录及其子目录中查找LDHSJG文件
                        var foundFiles = FindLDHSJGFilesInDirectory(firstLevelDir);

                        foreach (var filePath in foundFiles)
                        {
                            var ldhsjgInfo = new LDHSJGFileInfo
                            {
                                FilePath = filePath,
                                CountyName = extractedCountyName,
                                DirectoryName = directoryName,
                                FileName = System.IO.Path.GetFileNameWithoutExtension(filePath),
                                IsMatched = true
                            };

                            ldhsjgFiles.Add(ldhsjgInfo);
                            System.Diagnostics.Debug.WriteLine($"添加LDHSJG文件: {ldhsjgInfo.FileName} -> {extractedCountyName}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"未能匹配县名: {directoryName}");

                        // 即使未匹配到县名，也记录找到的LDHSJG文件
                        var foundFiles = FindLDHSJGFilesInDirectory(firstLevelDir);
                        foreach (var filePath in foundFiles)
                        {
                            var ldhsjgInfo = new LDHSJGFileInfo
                            {
                                FilePath = filePath,
                                CountyName = "未匹配",
                                DirectoryName = directoryName,
                                FileName = System.IO.Path.GetFileNameWithoutExtension(filePath),
                                IsMatched = false
                            };

                            ldhsjgFiles.Add(ldhsjgInfo);
                        }
                    }
                }

                UpdateStatus($"LDHSJG文件搜索完成，找到 {ldhsjgFiles.Count} 个文件");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找LDHSJG文件时出错: {ex.Message}");
                UpdateStatus("搜索LDHSJG文件时出错");
                throw;
            }

            return ldhsjgFiles;
        }

        /// <summary>
        /// 在指定目录中查找包含LDHSJG的shapefile文件
        /// </summary>
        /// <param name="directory">要搜索的目录</param>
        /// <returns>找到的文件路径列表</returns>
        private List<string> FindLDHSJGFilesInDirectory(string directory)
        {
            var foundFiles = new List<string>();

            try
            {
                // 递归搜索所有.shp文件
                var shapefiles = Directory.GetFiles(directory, "*.shp", SearchOption.AllDirectories);

                // 筛选包含LDHSJG的文件
                foreach (var shpFile in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shpFile);
                    if (fileName.IndexOf("LDHSJG", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        foundFiles.Add(shpFile);
                        System.Diagnostics.Debug.WriteLine($"找到LDHSJG文件: {shpFile}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"在目录 {directory} 中搜索LDHSJG文件时出错: {ex.Message}");
            }

            return foundFiles;
        }

        /// <summary>
        /// 从目录名中提取县名
        /// </summary>
        /// <param name="directoryName">目录名称</param>
        /// <param name="availableCounties">可用的县名列表</param>
        /// <returns>匹配的县名，如果没有匹配则返回空字符串</returns>
        private string ExtractCountyNameFromDirectory(string directoryName, HashSet<string> availableCounties)
        {
            try
            {
                // 方法1：直接匹配
                foreach (var countyName in availableCounties)
                {
                    if (directoryName.Contains(countyName))
                    {
                        return countyName;
                    }
                }

                // 方法2：清理目录名后再匹配
                string cleanedDirectoryName = CleanDirectoryName(directoryName);

                foreach (var countyName in availableCounties)
                {
                    if (cleanedDirectoryName.Contains(countyName) || countyName.Contains(cleanedDirectoryName))
                    {
                        return countyName;
                    }
                }

                // 方法3：模糊匹配 - 去除常见后缀后匹配
                var commonSuffixes = new[] { "县", "市", "区", "全民所有自然资源资产清查数据成果", "成果", "数据" };
                string simplifiedName = directoryName;

                foreach (var suffix in commonSuffixes)
                {
                    simplifiedName = simplifiedName.Replace(suffix, "");
                }

                foreach (var countyName in availableCounties)
                {
                    string simplifiedCounty = countyName;
                    foreach (var suffix in commonSuffixes)
                    {
                        simplifiedCounty = simplifiedCounty.Replace(suffix, "");
                    }

                    if (simplifiedName.Contains(simplifiedCounty) || simplifiedCounty.Contains(simplifiedName))
                    {
                        return countyName;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"提取县名时出错: {ex.Message}");
            }

            return string.Empty;
        }

        /// <summary>
        /// 清理目录名称，移除特殊字符和编码
        /// </summary>
        /// <param name="directoryName">原始目录名</param>
        /// <returns>清理后的目录名</returns>
        private string CleanDirectoryName(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName))
                return string.Empty;

            // 移除括号及其内容
            string cleaned = System.Text.RegularExpressions.Regex.Replace(directoryName, @"[（(].*?[）)]", "");

            // 移除特定关键词
            var keywordsToRemove = new[] { "全民所有自然资源资产清查数据成果", "成果", "数据", "清查" };
            foreach (var keyword in keywordsToRemove)
            {
                cleaned = cleaned.Replace(keyword, "");
            }

            return cleaned.Trim();
        }

        /// <summary>
        /// 显示LDHSJG文件匹配结果
        /// </summary>
        /// <param name="ldhsjgFiles">匹配结果列表</param>
        private void DisplayLDHSJGMatchResults(List<LDHSJGFileInfo> ldhsjgFiles)
        {
            try
            {
                var matchedFiles = ldhsjgFiles.Where(f => f.IsMatched).ToList();
                var unmatchedFiles = ldhsjgFiles.Where(f => !f.IsMatched).ToList();

                // 将匹配的LDHSJG文件转换为SourceDataFileInfo格式并保存到SharedDataManager
                var sourceDataFiles = new List<SourceDataFileInfo>();
                foreach (var ldhsjgFile in matchedFiles)
                {
                    var sourceDataFile = new SourceDataFileInfo
                    {
                        FullPath = ldhsjgFile.FilePath,
                        DisplayName = ldhsjgFile.CountyName,
                        IsGdb = false,
                        GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon
                    };
                    sourceDataFiles.Add(sourceDataFile);
                }

                // 保存到SharedDataManager
                ForestResourcePlugin.SharedDataManager.SetLDHSJGFiles(sourceDataFiles);

                string message = $"LDHSJG文件搜索完成！\n\n";
                message += $"搜索结果统计：\n";
                message += $"总文件数：{ldhsjgFiles.Count}\n";
                message += $"成功匹配：{matchedFiles.Count} 个文件\n";
                message += $"未匹配：{unmatchedFiles.Count} 个文件\n\n";

                if (matchedFiles.Count > 0)
                {
                    message += "成功匹配的文件：\n";
                    var groupedByCounty = matchedFiles.GroupBy(f => f.CountyName);
                    foreach (var group in groupedByCounty)
                    {
                        message += $"▶ {group.Key}: {group.Count()} 个文件\n";
                        foreach (var file in group.Take(3)) // 只显示前3个
                        {
                            message += $"  - {file.FileName}\n";
                        }
                        if (group.Count() > 3)
                        {
                            message += $"  - ... 还有 {group.Count() - 3} 个文件\n";
                        }
                    }
                    message += "\n";
                }

                if (unmatchedFiles.Count > 0)
                {
                    message += "未匹配的文件：\n";
                    foreach (var file in unmatchedFiles.Take(5)) // 只显示前5个
                    {
                        message += $"- {file.FileName} (目录: {file.DirectoryName})\n";
                    }
                    if (unmatchedFiles.Count > 5)
                    {
                        message += $"- ... 还有 {unmatchedFiles.Count - 5} 个未匹配文件\n";
                    }
                }

                MessageBox.Show(message, "LDHSJG文件搜索结果",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                UpdateStatus($"LDHSJG数据加载完成：{matchedFiles.Count} 个文件已匹配并保存到共享数据管理器");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示LDHSJG匹配结果时出错: {ex.Message}");
                MessageBox.Show($"显示搜索结果时出错：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// LDHSJG文件信息类
        /// </summary>
        private class LDHSJGFileInfo
        {
            /// <summary>
            /// 文件完整路径
            /// </summary>
            public string FilePath { get; set; }

            /// <summary>
            /// 匹配的县名
            /// </summary>
            public string CountyName { get; set; }

            /// <summary>
            /// 所在目录名称
            /// </summary>
            public string DirectoryName { get; set; }

            /// <summary>
            /// 文件名（不含扩展名）
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// 是否成功匹配到县
            /// </summary>
            public bool IsMatched { get; set; }

            public override string ToString()
            {
                return $"{FileName} -> {CountyName} ({(IsMatched ? "已匹配" : "未匹配")})";
            }
        }

        private void buttonForestExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证是否选择了县
                var selectedCounties = GetSelectedCounties();
                if (selectedCounties.Count == 0)
                {
                    MessageBox.Show("请至少选择一个县", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 验证输出路径
                if (string.IsNullOrEmpty(txtOutputPath.Text))
                {
                    MessageBox.Show("请设置输出路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 显示进度
                buttonForestExcel.Enabled = false;
                UpdateStatus("正在生成森林资源汇总表...");

                int successCount = 0;

                // 为每个县处理数据并写入A2表格
                foreach (var countyName in selectedCounties)
                {
                    try
                    {
                        UpdateStatus($"正在处理县：{countyName}");

                        // 查找县的SLZYZC数据
                        var slzyzcPath = FindSLZYZCPath(countyName);
                        if (string.IsNullOrEmpty(slzyzcPath))
                        {
                            System.Diagnostics.Debug.WriteLine($"未找到县 {countyName} 的SLZYZC数据");
                            continue;
                        }

                        // 统计数据
                        var forestStatistics = CalculateForestStatistics(slzyzcPath);

                        // 查找A2表格文件并写入数据
                        var a2FilePath = FindA2TablePath(countyName);
                        if (!string.IsNullOrEmpty(a2FilePath) && File.Exists(a2FilePath))
                        {
                            WriteDataToA2Table(a2FilePath, countyName, forestStatistics);
                            successCount++;
                            System.Diagnostics.Debug.WriteLine($"已为县 {countyName} 写入A2表格数据：{a2FilePath}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"未找到县 {countyName} 的A2表格文件");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"处理县 {countyName} 时出错: {ex.Message}");
                        MessageBox.Show($"处理县 {countyName} 时出错: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                UpdateStatus("森林资源汇总表数据写入完成");
                MessageBox.Show($"已成功为 {successCount} 个县写入森林资源汇总表数据", "生成完成",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成森林资源汇总表时发生错误：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"buttonForestExcel_Click错误: {ex}");
            }
            finally
            {
                buttonForestExcel.Enabled = true;
            }
        }

        /// <summary>
        /// 查找指定县的SLZYZC数据路径
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <returns>SLZYZC数据路径</returns>
        private string FindSLZYZCPath(string countyName)
        {
            try
            {
                // 获取县代码
                string countyCode = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyCode(countyName);

                // 构建可能的路径
                string countyFolderName = $"{countyName}({countyCode})全民所有自然资源资产清查数据成果";
                string baseCountyPath = System.IO.Path.Combine(txtOutputPath.Text, countyFolderName);

                // 方法1：查找GDB中的SLZYZC要素类
                string gdbPath = System.IO.Path.Combine(baseCountyPath, "清查数据集", "森林", "空间数据", $"{countyName}.gdb");
                if (Directory.Exists(gdbPath))
                {
                    return gdbPath; // 返回GDB路径，要素类名为SLZYZC
                }

                // 方法2：查找Shapefile
                string shapefilePath = System.IO.Path.Combine(baseCountyPath, "清查数据集", "森林", "空间数据", "SLZYZC.shp");
                if (File.Exists(shapefilePath))
                {
                    return shapefilePath;
                }

                // 方法3：查找带县代码的Shapefile
                string codeShapefilePath = System.IO.Path.Combine(baseCountyPath, "清查数据集", "森林", "空间数据", $"({countyCode})SLZYZC.shp");
                if (File.Exists(codeShapefilePath))
                {
                    return codeShapefilePath;
                }

                System.Diagnostics.Debug.WriteLine($"未找到县 {countyName} 的SLZYZC数据路径");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找县 {countyName} SLZYZC路径时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 查找指定县的A2表格文件路径
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <returns>A2表格文件路径</returns>
        private string FindA2TablePath(string countyName)
        {
            try
            {
                // 获取县代码
                string countyCode = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyCode(countyName);

                // 构建A2表格文件路径
                string countyFolderName = $"{countyName}({countyCode})全民所有自然资源资产清查数据成果";
                string tableA2Name = $"({countyCode})全民所有森林资源资产清查实物量汇总表.xls";
                string tableA2Path = System.IO.Path.Combine(txtOutputPath.Text, countyFolderName, "汇总表格", "森林", tableA2Name);

                return tableA2Path;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找县 {countyName} A2表格路径时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 计算森林资源统计数据
        /// </summary>
        /// <param name="slzyzcPath">SLZYZC数据路径</param>
        /// <returns>森林统计数据</returns>
        private ForestStatistics CalculateForestStatistics(string slzyzcPath)
        {
            var statistics = new ForestStatistics();
            IFeatureClass featureClass = null;

            try
            {
                // 加载要素类
                if (slzyzcPath.EndsWith(".gdb"))
                {
                    // 从GDB加载
                    featureClass = ForestResourcePlugin.GdbFeatureClassFinder.OpenFeatureClassFromGdb(slzyzcPath, "SLZYZC");
                }
                else
                {
                    // 从Shapefile加载
                    string directory = System.IO.Path.GetDirectoryName(slzyzcPath);
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(slzyzcPath);
                    IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                    IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(directory, 0);
                    featureClass = featureWorkspace.OpenFeatureClass(fileName);
                }

                if (featureClass == null)
                {
                    throw new Exception($"无法加载要素类：{slzyzcPath}");
                }

                // 获取字段索引
                var fieldIndices = GetSlzyzcFieldIndices(featureClass);

                // 遍历所有要素进行统计
                IFeatureCursor cursor = null;
                IFeature feature = null;

                try
                {
                    cursor = featureClass.Search(null, false);
                    while ((feature = cursor.NextFeature()) != null)
                    {
                        try
                        {
                            ProcessFeatureForStatistics(feature, fieldIndices, statistics);
                        }
                        finally
                        {
                            if (feature != null)
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                                feature = null;
                            }
                        }
                    }
                }
                finally
                {
                    if (cursor != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            finally
            {
                if (featureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
            }

            return statistics;
        }

        /// <summary>
        /// 获取SLZYZC字段索引
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <returns>字段索引结构</returns>
        private SlzyzcFieldIndices GetSlzyzcFieldIndices(IFeatureClass featureClass)
        {
            return new SlzyzcFieldIndices
            {
                GTDCDLBM = featureClass.FindField("GTDCDLBM"),      // 国土调查地类编码
                GTDCTDQS = featureClass.FindField("GTDCTDQS"),      // 国土调查土地权属
                LM_SUOYQ = featureClass.FindField("LM_SUOYQ"),      // 林木所有权
                LZ = featureClass.FindField("LZ"),                  // 林种
                QY = featureClass.FindField("QY"),                  // 起源
                ZTBMJ = featureClass.FindField("ZTBMJ"),            // 子图斑面积
                ZTBXJ = featureClass.FindField("ZTBXJ"),            // 子图斑蓄积
                LING_ZU = featureClass.FindField("LING_ZU"),        // 龄组
                MGQZS = featureClass.FindField("MGQZS"),            // 每公顷株数
                GTDCDLMC = featureClass.FindField("GTDCDLMC")               // 普查地类
            };
        }

                /// <summary>
        /// 处理单个要素进行统计
        /// </summary>
        /// <param name="feature">要素</param>
        /// <param name="fieldIndices">字段索引</param>
        /// <param name="statistics">统计数据</param>
        private void ProcessFeatureForStatistics(IFeature feature, SlzyzcFieldIndices fieldIndices, ForestStatistics statistics)
        {
            try
            {
                // 获取基础字段值
                string landTypeCode = GetFieldStringValue(feature, fieldIndices.GTDCDLBM);
                string landOwnership = GetFieldStringValue(feature, fieldIndices.GTDCTDQS);
                string forestOwnership = GetFieldStringValue(feature, fieldIndices.LM_SUOYQ);
                string forestType = GetFieldStringValue(feature, fieldIndices.LZ);
                string origin = GetFieldStringValue(feature, fieldIndices.QY);
                string ageGroup = GetFieldStringValue(feature, fieldIndices.LING_ZU);
                string surveyLandType = GetFieldStringValue(feature, fieldIndices.GTDCDLMC);

                double area = GetFieldDoubleValue(feature, fieldIndices.ZTBMJ);
                double volume = GetFieldDoubleValue(feature, fieldIndices.ZTBXJ);
                int stocksPerHectare = GetFieldIntValue(feature, fieldIndices.MGQZS);

                // 创建统计键值（用于分组统计）- 允许各组成部分为空
                string statisticsKey = CreateUniqueStatisticsKey(landOwnership, forestOwnership, forestType, origin);

                // 添加详细的调试信息
                System.Diagnostics.Debug.WriteLine($"处理要素 OID:{feature.OID}");
                System.Diagnostics.Debug.WriteLine($"  landOwnership='{landOwnership}', forestOwnership='{forestOwnership}', forestType='{forestType}', origin='{origin}'");
                System.Diagnostics.Debug.WriteLine($"  统计键值: {statisticsKey}");
                System.Diagnostics.Debug.WriteLine($"  surveyLandType='{surveyLandType}', area={area}, volume={volume}");

                // 确保统计项存在
                if (!statistics.StatisticsItems.ContainsKey(statisticsKey))
                {
                    statistics.StatisticsItems[statisticsKey] = new ForestStatisticsItem
                    {
                        LandOwnership = landOwnership ?? "",
                        ForestOwnership = forestOwnership ?? "",
                        ForestType = forestType ?? "",
                        Origin = origin ?? ""
                    };
                    System.Diagnostics.Debug.WriteLine($"  创建新的统计项: {statisticsKey}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"  使用已存在的统计项: {statisticsKey}");
                }

                var item = statistics.StatisticsItems[statisticsKey];

                // 根据普查地类进行分类统计
                switch (surveyLandType)
                {
                    case "乔木林地":
                        item.TreeLandArea += area;
                        item.TreeLandVolume += volume;
                        System.Diagnostics.Debug.WriteLine($"  添加到乔木林地: area={area}, volume={volume}");

                        // 按龄组统计乔木林地
                        switch (ageGroup)
                        {
                            case "1":
                                item.YoungForestArea += area;
                                item.YoungForestVolume += volume;
                                System.Diagnostics.Debug.WriteLine($"  添加到幼龄林: area={area}, volume={volume}");
                                break;
                            case "2":
                                item.MiddleAgedForestArea += area;
                                item.MiddleAgedForestVolume += volume;
                                System.Diagnostics.Debug.WriteLine($"  添加到中龄林: area={area}, volume={volume}");
                                break;
                            case "3":
                                item.NearMatureForestArea += area;
                                item.NearMatureForestVolume += volume;
                                System.Diagnostics.Debug.WriteLine($"  添加到近熟林: area={area}, volume={volume}");
                                break;
                            case "4":
                                item.MatureForestArea += area;
                                item.MatureForestVolume += volume;
                                System.Diagnostics.Debug.WriteLine($"  添加到成熟林: area={area}, volume={volume}");
                                break;
                            case "5":
                                item.OverMatureForestArea += area;
                                item.OverMatureForestVolume += volume;
                                System.Diagnostics.Debug.WriteLine($"  添加到过熟林: area={area}, volume={volume}");
                                break;
                            default:
                                System.Diagnostics.Debug.WriteLine($"  未识别的龄组: '{ageGroup}'");
                                break;
                        }
                        break;

                    case "竹林地":
                        item.BambooLandArea += area;
                        item.BambooStocks += stocksPerHectare * (area / 10000); // 转换为总株数
                        System.Diagnostics.Debug.WriteLine($"  添加到竹林地: area={area}, stocks={stocksPerHectare * (area / 10000)}");
                        break;

                    case "灌木林地":
                        item.ShrubLandArea += area;
                        System.Diagnostics.Debug.WriteLine($"  添加到灌木林地: area={area}");
                        break;

                    case "其他林地":
                        item.OtherForestLandArea += area;
                        System.Diagnostics.Debug.WriteLine($"  添加到其他林地: area={area}");
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine($"  未匹配的普查地类: '{surveyLandType}'");
                        break;
                }

                // 计算总面积
                item.TotalArea += area;
                System.Diagnostics.Debug.WriteLine($"  更新总面积: {item.TotalArea}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理要素统计时出错: {ex.Message}");
            }
        }
        /// <summary>
        /// 创建唯一的统计键值，允许各组成部分为空
        /// </summary>
        /// <param name="landOwnership">土地权属</param>
        /// <param name="forestOwnership">林木所有权</param>
        /// <param name="forestType">林种</param>
        /// <param name="origin">起源</param>
        /// <returns>唯一的统计键值</returns>
        private string CreateUniqueStatisticsKey(string landOwnership, string forestOwnership, string forestType, string origin)
        {
            // 将null值转换为空字符串，确保一致性
            string normalizedLandOwnership = landOwnership ?? "";
            string normalizedForestOwnership = forestOwnership ?? "";
            string normalizedForestType = forestType ?? "";
            string normalizedOrigin = origin ?? "";

            // 使用更明确的分隔符和长度标识符来避免冲突
            // 格式: [长度:值]|[长度:值]|[长度:值]|[长度:值]
            return $"[{normalizedLandOwnership.Length}:{normalizedLandOwnership}]|[{normalizedForestOwnership.Length}:{normalizedForestOwnership}]|[{normalizedForestType.Length}:{normalizedForestType}]|[{normalizedOrigin.Length}:{normalizedOrigin}]";
        }
        /// <summary>
        /// 获取字符串字段值
        /// </summary>
        private string GetFieldStringValue(IFeature feature, int fieldIndex)
        {
            if (fieldIndex == -1) return "";

            object value = feature.get_Value(fieldIndex);
            return value?.ToString() ?? "";
        }

        /// <summary>
        /// 获取双精度字段值
        /// </summary>
        private double GetFieldDoubleValue(IFeature feature, int fieldIndex)
        {
            if (fieldIndex == -1) return 0.0;

            object value = feature.get_Value(fieldIndex);
            if (value != null && double.TryParse(value.ToString(), out double result))
                return result;
            return 0.0;
        }

        /// <summary>
        /// 获取整数字段值
        /// </summary>
        private int GetFieldIntValue(IFeature feature, int fieldIndex)
        {
            if (fieldIndex == -1) return 0;

            object value = feature.get_Value(fieldIndex);
            if (value != null && int.TryParse(value.ToString(), out int result))
                return result;
            return 0;
        }

        /// <summary>
        /// 将统计数据写入A2表格
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="countyName">县名</param>
        /// <param name="statistics">统计数据</param>
        private void WriteDataToA2Table(string filePath, string countyName, ForestStatistics statistics)
        {
            try
            {
                // 读取现有Excel文件
                NPOI.HSSF.UserModel.HSSFWorkbook workbook = null;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(file);
                }

                NPOI.SS.UserModel.ISheet sheet = workbook.GetSheet("A2");
                if (sheet == null)
                {
                    throw new Exception("未找到A2工作表");
                }

                // 获取县代码
                string countyCode = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyCode(countyName);

                // 从第3行开始写入数据（前3行是表头）
                int currentRow = 3;

                foreach (var item in statistics.StatisticsItems.Values)
                {
                    NPOI.SS.UserModel.IRow dataRow = sheet.CreateRow(currentRow);

                    // 列0: 行政区名称
                    dataRow.CreateCell(0).SetCellValue(countyName);

                    // 列1: 行政区代码
                    dataRow.CreateCell(1).SetCellValue(countyCode);

                    // 列2: 国土变更调查权属
                    dataRow.CreateCell(2).SetCellValue(TranslateLandOwnership(item.LandOwnership));

                    // 列3: 林木所有权
                    dataRow.CreateCell(3).SetCellValue(TranslateForestOwnership(item.ForestOwnership));

                    // 列4: 林种
                    dataRow.CreateCell(4).SetCellValue(TranslateForestType(item.ForestType));

                    // 列5: 起源
                    dataRow.CreateCell(5).SetCellValue(TranslateOrigin(item.Origin));

                    // 列6: 面积合计
                    dataRow.CreateCell(6).SetCellValue(item.TotalArea);

                    // 列7-8: 乔木林地小计 - 面积和蓄积
                    dataRow.CreateCell(7).SetCellValue(item.TreeLandArea);
                    dataRow.CreateCell(8).SetCellValue(item.TreeLandVolume);

                    // 列9-10: 幼龄林 - 面积和蓄积
                    dataRow.CreateCell(9).SetCellValue(item.YoungForestArea);
                    dataRow.CreateCell(10).SetCellValue(item.YoungForestVolume);

                    // 列11-12: 中龄林 - 面积和蓄积
                    dataRow.CreateCell(11).SetCellValue(item.MiddleAgedForestArea);
                    dataRow.CreateCell(12).SetCellValue(item.MiddleAgedForestVolume);

                    // 列13-14: 近熟林 - 面积和蓄积
                    dataRow.CreateCell(13).SetCellValue(item.NearMatureForestArea);
                    dataRow.CreateCell(14).SetCellValue(item.NearMatureForestVolume);

                    // 列15-16: 成熟林 - 面积和蓄积
                    dataRow.CreateCell(15).SetCellValue(item.MatureForestArea);
                    dataRow.CreateCell(16).SetCellValue(item.MatureForestVolume);

                    // 列17-18: 过熟林 - 面积和蓄积
                    dataRow.CreateCell(17).SetCellValue(item.OverMatureForestArea);
                    dataRow.CreateCell(18).SetCellValue(item.OverMatureForestVolume);

                    // 列19-20: 竹林地 - 面积和株数
                    dataRow.CreateCell(19).SetCellValue(item.BambooLandArea);
                    dataRow.CreateCell(20).SetCellValue(item.BambooStocks);

                    // 列21: 灌木林地面积
                    dataRow.CreateCell(21).SetCellValue(item.ShrubLandArea);

                    // 列22: 其他林地面积
                    dataRow.CreateCell(22).SetCellValue(item.OtherForestLandArea);

                    currentRow++;
                }

                // 保存文件
                using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(file);
                }

                System.Diagnostics.Debug.WriteLine($"成功写入县 {countyName} 的A2表格数据，共 {statistics.StatisticsItems.Count} 行");
            }
            catch (Exception ex)
            {
                throw new Exception($"写入A2表格数据时出错: {ex.Message}", ex);
            }
        }

        // 翻译方法
        private string TranslateLandOwnership(string code)
        {
            switch (code)
            {
                case "10": return "10";
                case "20": return "20";
                case "30": return "30";
                case "40": return "40";
                default: return code;
            }
        }

        private string TranslateForestOwnership(string code)
        {
            switch (code)
            {
                case "10": return "国有";
                case "20": return "国有";
                case "30": return "集体";
                case "40": return "集体";
                default: return code;
            }
        }

        private string TranslateForestType(string code)
        {
            // 根据实际林种编码进行翻译
            switch (code)
            {
                case "110": return "生态公益林";
                case "120": return "商品林";
                case "210": return "防护林";
                case "220": return "特用林";
                case "310": return "用材林";
                case "320": return "经济林";
                case "330": return "薪炭林";
                default: return code;
            }
        }

        private string TranslateOrigin(string code)
        {
            switch (code)
            {
                case "1": return "人工";
                case "2": return "天然";
                default: return code;
            }
        }

        /// <summary>
        /// 森林统计数据类
        /// </summary>
        private class ForestStatistics
        {
            public Dictionary<string, ForestStatisticsItem> StatisticsItems { get; set; } = new Dictionary<string, ForestStatisticsItem>();
        }

        /// <summary>
        /// 森林统计项目类
        /// </summary>
        private class ForestStatisticsItem
        {
            public string LandOwnership { get; set; }           // 土地权属
            public string ForestOwnership { get; set; }         // 林木所有权
            public string ForestType { get; set; }              // 林种
            public string Origin { get; set; }                  // 起源

            public double TotalArea { get; set; }               // 总面积

            // 乔木林地
            public double TreeLandArea { get; set; }            // 乔木林地面积
            public double TreeLandVolume { get; set; }          // 乔木林地蓄积

            // 按龄组分类的乔木林地
            public double YoungForestArea { get; set; }         // 幼龄林面积
            public double YoungForestVolume { get; set; }       // 幼龄林蓄积
            public double MiddleAgedForestArea { get; set; }     // 中龄林面积
            public double MiddleAgedForestVolume { get; set; }   // 中龄林蓄积
            public double NearMatureForestArea { get; set; }     // 近熟林面积
            public double NearMatureForestVolume { get; set; }   // 近熟林蓄积
            public double MatureForestArea { get; set; }         // 成熟林面积
            public double MatureForestVolume { get; set; }       // 成熟林蓄积
            public double OverMatureForestArea { get; set; }     // 过熟林面积
            public double OverMatureForestVolume { get; set; }   // 过熟林蓄积

            // 其他林地类型
            public double BambooLandArea { get; set; }           // 竹林地面积
            public double BambooStocks { get; set; }             // 竹林株数
            public double ShrubLandArea { get; set; }            // 灌木林地面积
            public double OtherForestLandArea { get; set; }      // 其他林地面积
        }

        /// <summary>
        /// SLZYZC字段索引类
        /// </summary>
        private class SlzyzcFieldIndices
        {
            public int GTDCDLBM { get; set; } = -1;    // 国土调查地类编码
            public int GTDCTDQS { get; set; } = -1;    // 国土调查土地权属
            public int LM_SUOYQ { get; set; } = -1;    // 林木所有权
            public int LZ { get; set; } = -1;          // 林种
            public int QY { get; set; } = -1;          // 起源
            public int ZTBMJ { get; set; } = -1;       // 子图斑面积
            public int ZTBXJ { get; set; } = -1;       // 子图斑蓄积
            public int LING_ZU { get; set; } = -1;     // 龄组
            public int MGQZS { get; set; } = -1;       // 每公顷株数
            public int GTDCDLMC { get; set; } = -1;        // 国土调查地类名称
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
