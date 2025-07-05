using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using System.Linq;

namespace ForestResourcePlugin
{
    public partial class Basic : Form
    {
        private DataTable previewData;
        private DataTable mappingData;
        private List<CountyDataInfo> selectedCountyData; // 存储选中县的数据信息
        private List<LayerInfo> mapLayers; // 当前地图文档中的图层列表
        
        // Add missing IFeatureClass fields
        private IFeatureClass lcxzgxFeatureClass;
        private IFeatureClass czkfbjFeatureClass;

        public Basic()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // 初始化坐标系下拉框
            cmbCoordSystem.Items.AddRange(new string[]
            {
                "CGCS2000_3_Degree_GK_CM_117E",
                "CGCS2000_3_Degree_GK_CM_120E",
                "CGCS2000_3_Degree_GK_CM_123E",
                "Beijing_1954_3_Degree_GK_CM_117E",
                "WGS_1984_UTM_Zone_49N",
                "WGS_1984_UTM_Zone_50N"
            });
            cmbCoordSystem.SelectedIndex = 0;

            // 初始化处理选项
            chkTopologyCheck.Checked = true;
            chkGeometryValidation.Checked = true;

            // 初始化字段映射表格
            InitializeMappingGrid();

            // 初始化筛选条件复选框
            chkForestLand.Checked = true;
            chkStateOwned.Checked = true;
            chkCollectiveInBoundary.Checked = true;

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

            // 初始化字段下拉框（基于选中的县）
            LoadFieldsFromSelectedCounties();

            // 加载县列表
            LoadCounties();
        }
        /// <summary>
        /// 县数据信息类
        /// </summary>
        private class CountyDataInfo
        {
            public string CountyName { get; set; }
            public LCXZGXFileInfo LCXZGXFile { get; set; }
            public LCXZGXFileInfo CZKFBJFile { get; set; }
        }
        // 改进的加载地图图层方法

        /// <summary>
        /// 从共享数据管理器加载县列表
        /// </summary>
        private void LoadCountiesFromSharedData()
        {
            try
            {
                // 获取林草现状文件列表
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetLCXZGXFiles();

                // 获取城镇开发边界文件列表
                var czkfbjFiles = ForestResourcePlugin.SharedDataManager.GetCZKFBJFiles();

                // 获取所有唯一的县名（第二级目录名）
                var countyNames = new HashSet<string>();

                // 从林草现状文件中提取县名
                foreach (var file in lcxzgxFiles)
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
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetLCXZGXFiles();
                var czkfbjFiles = ForestResourcePlugin.SharedDataManager.GetCZKFBJFiles();

                var allFieldNames = new HashSet<string>();

                foreach (var countyName in selectedCounties)
                {
                    var countyInfo = new CountyDataInfo { CountyName = countyName };

                    // 查找该县的林草现状数据
                    var lcxzgxFile = lcxzgxFiles.FirstOrDefault(f => f.DisplayName == countyName);
                    if (lcxzgxFile != null)
                    {
                        countyInfo.LCXZGXFile = lcxzgxFile;

                        // 获取该文件的字段
                        var fields = GetFieldsFromFile(lcxzgxFile);
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
                    System.Diagnostics.Debug.WriteLine($"县 {countyName}: LCXZGX={lcxzgxFile?.FullPath ?? "无"}, CZKFBJ={czkfbjFile?.FullPath ?? "无"}");
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
                    int landTypeIndex = FindBestMatchIndex(cmbLandTypeField.Items, new[] { "地类", "Y_DLBM", "LandType", "DL", "Land", "Type", "DLDM" });
                    cmbLandTypeField.SelectedIndex = landTypeIndex >= 0 ? landTypeIndex : 0;

                    int landOwnerIndex = FindBestMatchIndex(cmbLandOwnerField.Items, new[] {"QSXZ"});
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
        private List<string> GetFieldsFromFile(LCXZGXFileInfo fileInfo)
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

        private void btnBrowseLCXZGX_Click(object sender, EventArgs e)
        {
            MessageBox.Show("请在基础数据准备中选择数据源，然后在县列表中选择要处理的县。", "提示",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBrowseCZKFBJ_Click(object sender, EventArgs e)
        {
            MessageBox.Show("请在基础数据准备中选择数据源，然后在县列表中选择要处理的县。", "提示",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择输出路径";
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

        // 林草现状图层下拉框选择改变事件
        private void cmbLCXZGXPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"林草现状图层选择改变，索引: {cmbLCXZGXPath.SelectedIndex}");
                
                // 如果选择的是提示项，则不执行任何操作
                if (cmbLCXZGXPath.SelectedIndex == 0)
                {
                    lcxzgxFeatureClass = null;
                    System.Diagnostics.Debug.WriteLine("选择了提示项，清空要素类");
                    return;
                }

                // 获取选择的项
                object selectedItem = cmbLCXZGXPath.SelectedItem;
                System.Diagnostics.Debug.WriteLine($"选择的项类型: {selectedItem?.GetType().Name}");
                
                if (selectedItem is ForestResourcePlugin.LCXZGXFileInfo fileInfo)
                {
                    System.Diagnostics.Debug.WriteLine($"从共享数据源加载: {fileInfo.DisplayName}");
                    // 将路径设置到下拉框文本中，这样现有的加载代码可以处理
                    LoadLCXZGXFields(); // 调用现有的加载方法
                }
                else if (selectedItem is string filePath && !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine($"从文件路径加载: {filePath}");
                    // 从文件路径加载
                    LoadLCXZGXFields();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"无效的选择项: {selectedItem}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载林草现状图层失败: {ex.Message}");
                MessageBox.Show($"加载林草现状图层失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("加载林草现状图层失败");
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
                
                if (selectedItem is ForestResourcePlugin.LCXZGXFileInfo fileInfo)
                {
                    System.Diagnostics.Debug.WriteLine($"从共享数据源加载: {fileInfo.DisplayName}");
                    // 将路径设置到下拉框文本中，这样现有的加载代码可以处理
                    LoadCZKFBJFromPath(fileInfo.FullPath); // 调用现有的加载方法
                }
                else if (selectedItem is string filePath && !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine($"从文件路径加载: {filePath}");
                    // 从文件路径加载
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

        // 从要素类加载字段
        private void LoadFieldsFromFeatureClass(IFeatureClass featureClass)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("开始从要素类加载字段");
                
                // Clear existing items in dropdown lists
                cmbLandTypeField.Items.Clear();
                cmbLandOwnerField.Items.Clear();
                
                if (featureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine("要素类为空，无法加载字段");
                    return;
                }
                
                UpdateStatus("正在读取图层字段...");
                
                List<string> fieldNames = new List<string>();
                
                // 读取要素类的字段
                IFields fields = featureClass.Fields;
                System.Diagnostics.Debug.WriteLine($"要素类字段总数: {fields.FieldCount}");
                
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    System.Diagnostics.Debug.WriteLine($"字段 {i}: {field.Name} ({field.Type})");
                    
                    // 只添加字符串和数值类型的字段
                    if (field.Type == esriFieldType.esriFieldTypeString ||
                        field.Type == esriFieldType.esriFieldTypeSmallInteger ||
                        field.Type == esriFieldType.esriFieldTypeInteger ||
                        field.Type == esriFieldType.esriFieldTypeSingle ||
                        field.Type == esriFieldType.esriFieldTypeDouble)
                    {
                        fieldNames.Add(field.Name);
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"有效字段数量: {fieldNames.Count}");
                
                // 添加字段到下拉列表
                foreach (string fieldName in fieldNames)
                {
                    cmbLandTypeField.Items.Add(fieldName);
                    cmbLandOwnerField.Items.Add(fieldName);
                }
                
                // 选择默认字段（如果有字段）
                if (cmbLandTypeField.Items.Count > 0)
                {
                    // 尝试查找与地类相关的字段
                    int landTypeIndex = FindBestMatchIndex(cmbLandTypeField.Items, new[] { "地类", "Y_DLBM", "LandType", "DL", "Land", "Type", "DLDM" });
                    cmbLandTypeField.SelectedIndex = landTypeIndex >= 0 ? landTypeIndex : 0;
                    System.Diagnostics.Debug.WriteLine($"选择地类字段: {cmbLandTypeField.SelectedItem}");
                    
                    // 尝试查找与土地权属相关的字段
                    int landOwnerIndex = FindBestMatchIndex(cmbLandOwnerField.Items, new[] { "权属", "土地权属", "TDQS", "LD_QS", "Ownership", "Owner", "QS", "QSXZ" });
                    cmbLandOwnerField.SelectedIndex = landOwnerIndex >= 0 ? landOwnerIndex : 0;
                    System.Diagnostics.Debug.WriteLine($"选择权属字段: {cmbLandOwnerField.SelectedItem}");
                }
                
                UpdateStatus($"已读取 {fieldNames.Count} 个字段");
                System.Diagnostics.Debug.WriteLine("字段加载完成");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"读取图层字段时出错: {ex.Message}");
                MessageBox.Show($"读取图层字段时出错: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("读取字段失败");
            }
        }

        private void LoadLCXZGXFields()
        {
            try
            {
                // Clear existing items in dropdown lists
                cmbLandTypeField.Items.Clear();
                cmbLandOwnerField.Items.Clear();
                
                UpdateStatus("正在读取图层字段...");
                progressBar.Value = 30;
                
                List<string> fieldNames = new List<string>();
                
                // 检查是否已有加载的要素类
                if (lcxzgxFeatureClass != null)
                {
                    // 直接从要素类对象获取字段
                    System.Diagnostics.Debug.WriteLine("从已加载的要素类对象读取字段");
                    fieldNames = GeodatabaseUtilities.GetFeatureClassFields(lcxzgxFeatureClass);
                }
                else
                {
                    // 检查选择项是否为GDB要素类
                    object selectedItem = cmbLCXZGXPath.SelectedItem;
                    System.Diagnostics.Debug.WriteLine($"LoadLCXZGXFields: 当前选择项类型 {selectedItem?.GetType().Name ?? "null"}");
                    
                    if (selectedItem is ForestResourcePlugin.LCXZGXFileInfo fileInfo && fileInfo.IsGdb)
                    {
                        System.Diagnostics.Debug.WriteLine($"从GDB加载字段: {fileInfo.FullPath}, 要素类: {fileInfo.FeatureClassName}");
                        // 加载林草现状图层
                        LoadLCXZGXLayer(fileInfo.FullPath);
                        
                        // 使用加载后的要素类获取字段
                        if (lcxzgxFeatureClass != null)
                        {
                            fieldNames = GeodatabaseUtilities.GetFeatureClassFields(lcxzgxFeatureClass);
                        }
                    }
                    else
                    {
                        // 使用原有的Shapefile读取逻辑
                        string path = cmbLCXZGXPath.Text;
                        
                        // Check if file exists
                        if (string.IsNullOrEmpty(path) || !File.Exists(path))
                        {
                            MessageBox.Show("选择的Shapefile文件不存在或无效", "读取字段失败", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                        // 使用ShapefileReader工具类读取字段
                        fieldNames = ShapefileReader.GetShapefileFieldNames(path);
                        
                        // 加载林草现状图层
                        LoadLCXZGXLayer(path);
                    }
                }
                
                // 添加字段到下拉列表
                foreach (string fieldName in fieldNames)
                {
                    cmbLandTypeField.Items.Add(fieldName);
                    cmbLandOwnerField.Items.Add(fieldName);
                }
                
                // 选择默认字段（如果有字段）
                if (cmbLandTypeField.Items.Count > 0)
                {
                    // 尝试查找与地类相关的字段
                    int landTypeIndex = FindBestMatchIndex(cmbLandTypeField.Items, new[] { "地类", "Y_DLBM", "LandType", "DL", "Land", "Type", "DLDM" });
                    cmbLandTypeField.SelectedIndex = landTypeIndex >= 0 ? landTypeIndex : 0;
                    
                    // 尝试查找与土地权属相关的字段
                    int landOwnerIndex = FindBestMatchIndex(cmbLandOwnerField.Items, new[] { "权属", "土地权属", "TDQS", "LD_QS", "Ownership", "Owner", "QS", "QSXZ" });
                    cmbLandOwnerField.SelectedIndex = landOwnerIndex >= 0 ? landOwnerIndex : 0;
                }
                
                progressBar.Value = 100;
                UpdateStatus($"已读取 {fieldNames.Count} 个字段");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取图层字段时出错: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("读取字段失败");
                System.Diagnostics.Debug.WriteLine($"LoadLCXZGXFields错误: {ex}");
            }
        }

        // 加载林草现状图层
        private void LoadLCXZGXLayer(string path)
        {
            try
            {
                // 检查选择项是否为GDB要素类
                object selectedItem = cmbLCXZGXPath.SelectedItem;
                System.Diagnostics.Debug.WriteLine($"LoadLCXZGXLayer: 当前选择项类型 {selectedItem?.GetType().Name ?? "null"}");
                
                if (selectedItem is ForestResourcePlugin.LCXZGXFileInfo fileInfo)
                {
                    System.Diagnostics.Debug.WriteLine($"LoadLCXZGXLayer: 文件信息 - IsGdb={fileInfo.IsGdb}, Path={fileInfo.FullPath}, FeatureClassName={fileInfo.FeatureClassName}");
                    
                    if (fileInfo.IsGdb)
                    {
                        // 从GDB加载要素类
                        System.Diagnostics.Debug.WriteLine($"从GDB加载林草现状图层: {fileInfo.FullPath}, 要素类: {fileInfo.FeatureClassName}");
                        try
                        {
                            lcxzgxFeatureClass = ForestResourcePlugin.GdbFeatureClassFinder.OpenFeatureClassFromGdb(
                                fileInfo.FullPath, fileInfo.FeatureClassName);
                            
                            if (lcxzgxFeatureClass != null)
                            {
                                System.Diagnostics.Debug.WriteLine($"成功加载GDB要素类，要素数: {lcxzgxFeatureClass.FeatureCount(null)}");
                                // 加载字段信息
                                LoadFieldsFromFeatureClass(lcxzgxFeatureClass);
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
                System.Diagnostics.Debug.WriteLine($"从Shapefile加载林草现状图层: {path}");
                string directory = System.IO.Path.GetDirectoryName(path);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                // 创建工作空间工厂并打开Shapefile
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(directory, 0);
                lcxzgxFeatureClass = featureWorkspace.OpenFeatureClass(fileName);
                
                if (lcxzgxFeatureClass != null)
                {
                    System.Diagnostics.Debug.WriteLine($"成功加载Shapefile，要素数: {lcxzgxFeatureClass.FeatureCount(null)}");
                    // 加载字段信息
                    LoadFieldsFromFeatureClass(lcxzgxFeatureClass);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Shapefile加载失败: 返回为null");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载林草现状图层失败: {ex.Message}");
                throw new Exception($"加载林草现状图层失败: {ex.Message}", ex);
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
                
                if (selectedItem is ForestResourcePlugin.LCXZGXFileInfo fileInfo)
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

        // 加载城镇开发边界图层 (保持向后兼容)
        private void LoadCZKFBJLayer(string shapefilePath)
        {
            LoadCZKFBJFromPath(shapefilePath);
        }

        // Helper method to find the best matching field name
        private int FindBestMatchIndex(ComboBox.ObjectCollection items, string[] searchTerms)
        {
            foreach (string term in searchTerms)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    string item = items[i].ToString();
                    if (item.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return i;
                    }
                }
            }
            return -1; // No match found
        }

        /// <summary>
        /// 生成多县预览数据
        /// </summary>
        private PreviewQueryResult GenerateMultiCountyPreview(string landTypeField, string landOwnerField)
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
                    if (countyInfo.LCXZGXFile == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"县 {countyInfo.CountyName} 没有林草现状数据，跳过");
                        continue;
                    }

                    countyIndex++;
                    UpdateStatus($"正在处理县 {countyInfo.CountyName} ({countyIndex}/{selectedCountyData.Count})...");

                    // 计算进度
                    int baseProgress = 20 + (countyIndex - 1) * 60 / selectedCountyData.Count;
                    progressBar.Value = baseProgress;

                    // 处理单个县的数据
                    var countyResult = ProcessSingleCountyPreview(countyInfo, landTypeField, landOwnerField);

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
        private PreviewQueryResult ProcessSingleCountyPreview(CountyDataInfo countyInfo, string landTypeField, string landOwnerField)
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
                IFeatureClass lcxzgxFeatureClass = LoadFeatureClass(countyInfo.LCXZGXFile);
                IFeatureClass czkfbjFeatureClass = null;

                if (countyInfo.CZKFBJFile != null && chkCollectiveInBoundary.Checked)
                {
                    czkfbjFeatureClass = LoadFeatureClass(countyInfo.CZKFBJFile);
                }

                if (lcxzgxFeatureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法加载县 {countyInfo.CountyName} 的林草现状数据");
                    return result;
                }

                // 使用现有的查询逻辑处理单个要素类
                string optimizedWhereClause = BuildOptimizedWhereClause(landTypeField, landOwnerField);
                result = ExecuteOptimizedQueryForFeatureClass(
                    lcxzgxFeatureClass,
                    czkfbjFeatureClass,
                    optimizedWhereClause,
                    landTypeField,
                    landOwnerField,
                    200); // 每个县最多200条记录用于预览

                // 清理COM对象
                if (lcxzgxFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lcxzgxFeatureClass);
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
            IFeatureClass lcxzgxFeatureClass,
            IFeatureClass czkfbjFeatureClass,
            string whereClause,
            string landTypeField,
            string landOwnerField,
            int maxCount)
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
                var fieldIndices = GetFieldIndicesForFeatureClass(lcxzgxFeatureClass, landTypeField, landOwnerField);

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
                    cursor = lcxzgxFeatureClass.Search(queryFilter, false);

                    while ((feature = cursor.NextFeature()) != null && result.ProcessedCount < maxCount)
                    {
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
            if (chkStateOwned.Checked && (ownerValue == "1" || ownerValue == "20"))
            {
                return true;
            }

            // 集体林地需要检查是否在城镇开发边界内
            if (chkCollectiveInBoundary.Checked &&
                (ownerValue == "2" || ownerValue == "30") &&
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
                var allPreviewData = GenerateMultiCountyPreview(landTypeField, landOwnerField);

                // 显示结果
                DisplayPreviewResults(allPreviewData);

                progressBar.Value = 100;
                UpdateStatus("预览生成完成");

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成预览时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("预览生成失败");
                progressBar.Value = 0;
                System.Diagnostics.Debug.WriteLine($"btnPreview_Click错误: {ex}");
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
        private IFeatureClass LoadFeatureClass(LCXZGXFileInfo fileInfo)
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
            btnExecute.Enabled = false;
            btnCancel.Enabled = true;
            progressBar.Value = 0;

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
                var batchResults = ExecuteMultiCountyBatchProcessing(landTypeField, landOwnerField);

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
            catch (Exception ex)
            {
                progressBar.Value = 0;
                UpdateStatus("处理失败");
                MessageBox.Show($"处理过程中发生错误：\n{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"ExecuteProcessing错误: {ex}");
            }
            finally
            {
                btnExecute.Enabled = true;
                btnCancel.Enabled = false;
            }
        }

        /// <summary>
        /// 执行多县批量处理
        /// </summary>
        private List<CountyProcessingResult> ExecuteMultiCountyBatchProcessing(string landTypeField, string landOwnerField)
        {
            var results = new List<CountyProcessingResult>();
            var selectedCounties = GetSelectedCounties();
            int totalCounties = selectedCounties.Count;
            int processedCounties = 0;

            foreach (var countyName in selectedCounties)
            {
                try
                {
                    processedCounties++;
                    UpdateStatus($"正在处理县 {countyName} ({processedCounties}/{totalCounties})...");

                    // 计算进度（20-90之间分配给批量处理）
                    int baseProgress = 20 + (processedCounties - 1) * 70 / totalCounties;
                    progressBar.Value = baseProgress;

                    // 查找县数据
                    var countyInfo = selectedCountyData.FirstOrDefault(c => c.CountyName == countyName);
                    if (countyInfo?.LCXZGXFile == null)
                    {
                        results.Add(new CountyProcessingResult
                        {
                            CountyName = countyName,
                            Success = false,
                            ErrorMessage = "未找到该县的林草现状数据"
                        });
                        continue;
                    }

                    // 处理单个县
                    var countyResult = ProcessSingleCounty(countyInfo, landTypeField, landOwnerField);
                    results.Add(countyResult);

                    // 更新进度
                    progressBar.Value = baseProgress + (70 / totalCounties);
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

                // 检查必需字段的映射
                var requiredFields = new Dictionary<string, string>
                {

                };

                var missingFields = new List<string>();

                foreach (var required in requiredFields)
                {
                    if (!fieldMappings.ContainsKey(required.Key) ||
                        string.IsNullOrEmpty(fieldMappings[required.Key]))
                    {
                        missingFields.Add(required.Value);
                    }
                }

                if (missingFields.Count > 0)
                {
                    string message = $"以下必需字段缺少映射配置：\n{string.Join("\n", missingFields)}\n\n请在字段映射表格中配置这些字段的映射关系。";
                    MessageBox.Show(message, "字段映射验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        /// <summary>
        /// 处理单个县
        /// </summary>
        private CountyProcessingResult ProcessSingleCounty(CountyDataInfo countyInfo, string landTypeField, string landOwnerField)
        {
            var result = new CountyProcessingResult
            {
                CountyName = countyInfo.CountyName,
                Success = false
            };

            try
            {
                // 加载要素类
                IFeatureClass lcxzgxFeatureClass = LoadFeatureClass(countyInfo.LCXZGXFile);
                IFeatureClass czkfbjFeatureClass = null;

                if (countyInfo.CZKFBJFile != null && chkCollectiveInBoundary.Checked)
                {
                    czkfbjFeatureClass = LoadFeatureClass(countyInfo.CZKFBJFile);
                }

                if (lcxzgxFeatureClass == null)
                {
                    result.ErrorMessage = "无法加载林草现状数据";
                    return result;
                }

                // 查询符合条件的要素
                var filteredFeatures = QueryFilteredFeaturesForCounty(
                    lcxzgxFeatureClass, 
                    czkfbjFeatureClass, 
                    landTypeField, 
                    landOwnerField);

                if (filteredFeatures.Count == 0)
                {
                    result.Success = true;
                    result.ProcessedFeatureCount = 0;
                    result.OutputPath = "无符合条件的图斑";
                    return result;
                }

                // 创建县级输出文件
                string countyOutputPath = System.IO.Path.Combine(txtOutputPath.Text, countyInfo.CountyName);
                if (!Directory.Exists(countyOutputPath))
                {
                    Directory.CreateDirectory(countyOutputPath);
                }

                string outputShapefilePath = System.IO.Path.Combine(countyOutputPath,
                    $"{countyInfo.CountyName}_ForestScope_{DateTime.Now:yyyyMMdd_HHmmss}.shp");

                // 导出数据到数据库
                var exporter = new ShapefileExporter();
                var fieldMappings = GetFieldMappingsFromGrid();

                // 构建县级数据库路径 - 假设输出路径是数据库的基础路径
                string countyDatabasePath = txtOutputPath.Text;

                // 执行数据库导出操作
                // 使用ShapefileExporter将筛选后的林草现状要素写入到对应县的数据库LCXZGX表中
                exporter.ExportToShapefile(
                    filteredFeatures,          // 已筛选的符合条件的要素列表（国有林地和集体林地）
                    lcxzgxFeatureClass,        // 源林草现状要素类，用于获取字段定义和要素数据
                    countyInfo.CountyName,     // 当前处理的县名，用于确定目标数据库路径
                    countyDatabasePath,        // 县级数据库基础路径，最终会拼接为：{countyDatabasePath}/{县名}/{县名}.gdb
                    fieldMappings,             // 字段映射配置，定义源字段与目标字段的对应关系
                    (percentage, message) => { // 进度回调函数，用于实时更新处理进度和状态信息
                        // 进度回调处理：更新界面进度条和状态显示
                        try
                        {
                            // 确保进度值不超过100，更新进度条显示
                            progressBar.Value = Math.Min(percentage, 100);
                            // 更新状态标签显示当前操作信息
                            UpdateStatus(message);
                            // 处理Windows消息队列，保持界面响应
                            Application.DoEvents();
                        }
                        catch (Exception ex)
                        {
                            // 记录进度更新过程中的异常，不影响主要的导出流程
                            System.Diagnostics.Debug.WriteLine($"更新进度时出错: {ex.Message}");
                        }
                    });

                // 导出成功后设置处理结果
                result.Success = true;                              // 标记县处理成功
                result.ProcessedFeatureCount = filteredFeatures.Count;  // 记录实际处理的要素数量
                // 构建输出数据库的完整路径：{基础路径}/{县名}/{县名}.gdb
                result.OutputPath = System.IO.Path.Combine(countyDatabasePath, countyInfo.CountyName, $"{countyInfo.CountyName}.gdb");

                // 清理COM对象
                foreach (var feature in filteredFeatures)
                {
                    if (feature != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                }

                if (lcxzgxFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lcxzgxFeatureClass);
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
            IFeatureClass lcxzgxFeatureClass,
            IFeatureClass czkfbjFeatureClass,
            string landTypeField,
            string landOwnerField)
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

                var fieldIndices = GetFieldIndicesForFeatureClass(lcxzgxFeatureClass, landTypeField, landOwnerField);

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
                    cursor = lcxzgxFeatureClass.Search(queryFilter, false);

                    while ((feature = cursor.NextFeature()) != null)
                    {
                        if (ShouldIncludeFeatureForCounty(feature, fieldIndices, cachedSpatialFilter, czkfbjFeatureClass))
                        {
                            features.Add(feature);
                            feature = null; // 防止在 finally 中释放
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
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                    if (cursor != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
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

        // **新增方法: 构建优化的WHERE子句**
        private string BuildOptimizedWhereClause(string landTypeField, string landOwnerField)
        {
            var conditions = new List<string>();

            if (chkForestLand.Checked)
            {
                // 优化: 使用单一条件而不是多个OR条件
                var subconditions = new List<string>();
                
                if (chkStateOwned.Checked)
                {
                    subconditions.Add($"{landOwnerField} = '20'");
                }
                
                if (chkCollectiveInBoundary.Checked)
                {
                    subconditions.Add($"{landOwnerField} = '30'");
                }

                if (subconditions.Count > 0)
                {
                    string ownerCondition = subconditions.Count == 1 ? 
                        subconditions[0] : 
                        $"({string.Join(" OR ", subconditions)})";
                    
                    conditions.Add($"{landTypeField} LIKE '03%' AND {ownerCondition}");
                }
            }

            return conditions.Count > 0 ? string.Join(" OR ", conditions) : "";
        }

        // **新增方法: 执行优化查询**
        private PreviewQueryResult ExecuteOptimizedQuery(
            string whereClause, 
            string landTypeField, 
            string landOwnerField, 
            int maxCount, 
            int chunkSize)
        {
            var result = new PreviewQueryResult();
            
            // 创建查询过滤器
            IQueryFilter queryFilter = new QueryFilterClass();
            if (!string.IsNullOrEmpty(whereClause))
            {
                queryFilter.WhereClause = whereClause;
            }

            // **优化: 预先获取字段索引**
            var fieldIndices = GetFieldIndices(landTypeField, landOwnerField);
            
            // **优化: 缓存空间过滤器以提高性能**
            ISpatialFilter cachedSpatialFilter = null;
            if (chkCollectiveInBoundary.Checked && czkfbjFeatureClass != null)
            {
                cachedSpatialFilter = new SpatialFilterClass();
                cachedSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            }

            IFeatureCursor cursor = null;
            try
            {
                cursor = lcxzgxFeatureClass.Search(queryFilter, false);
                result = ProcessFeaturesInChunks(cursor, fieldIndices, cachedSpatialFilter, maxCount, chunkSize);
            }
            finally
            {
                if (cursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }

            return result;
        }

        // **新增方法: 分块处理要素**
        private PreviewQueryResult ProcessFeaturesInChunks(
            IFeatureCursor cursor, 
            FieldIndices fieldIndices, 
            ISpatialFilter spatialFilter, 
            int maxCount, 
            int chunkSize)
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

            var featuresBatch = new List<IFeature>();
            IFeature feature = null;
            int processedInCurrentChunk = 0;

            try
            {
                while ((feature = cursor.NextFeature()) != null && result.ProcessedCount < maxCount)
                {
                    result.TotalCount++;
                    featuresBatch.Add(feature);
                    processedInCurrentChunk++;

                    // **优化: 当达到块大小时批量处理**
                    if (processedInCurrentChunk >= chunkSize)
                    {
                        ProcessFeatureBatch(featuresBatch, fieldIndices, spatialFilter, result);
                        
                        // 清理当前批次
                        foreach (var f in featuresBatch)
                        {
                            if (f != null)
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(f);
                        }
                        featuresBatch.Clear();
                        processedInCurrentChunk = 0;

                        // **优化: 更新进度**
                        int progress = Math.Min(90, 50 + (result.TotalCount * 40 / maxCount));
                        progressBar.Value = progress;
                        UpdateStatus($"正在处理数据: 已处理 {result.TotalCount} 条，符合条件 {result.ProcessedCount} 条");
                        Application.DoEvents();

                        // **优化: 提前终止条件**
                        if (result.ProcessedCount >= maxCount)
                            break;
                    }

                    feature = null; // 防止在finally中重复释放
                }

                // 处理最后一批
                if (featuresBatch.Count > 0)
                {
                    ProcessFeatureBatch(featuresBatch, fieldIndices, spatialFilter, result);
                }
            }
            finally
            {
                // 清理资源
                foreach (var f in featuresBatch)
                {
                    if (f != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(f);
                }
                if (feature != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                }
            }

            return result;
        }

        // **新增方法: 批量处理要素**
        private void ProcessFeatureBatch(
            List<IFeature> features, 
            FieldIndices fieldIndices, 
            ISpatialFilter spatialFilter, 
            PreviewQueryResult result)
        {
            foreach (var feature in features)
            {
                if (result.ProcessedCount >= 1000) // 硬性限制
                    break;

                if (ShouldIncludeFeature(feature, fieldIndices, spatialFilter))
                {
                    var row = CreateDataRow(feature, fieldIndices, result.PreviewData);
                    result.PreviewData.Rows.Add(row);
                    result.ProcessedCount++;
                }
            }
        }

        // **新增方法: 判断是否应包含要素**
        private bool ShouldIncludeFeature(IFeature feature, FieldIndices fieldIndices, ISpatialFilter spatialFilter)
        {
            // 获取土地权属值
            string ownerValue = GetFieldValue(feature, fieldIndices.QsxzIndex, fieldIndices.TdqsIndex);
            
            // 国有林地直接添加
            if (chkStateOwned.Checked && (ownerValue == "1" || ownerValue == "20"))
            {
                return true;
            }
            
            // 集体林地需要检查是否在城镇开发边界内
            if (chkCollectiveInBoundary.Checked && 
                (ownerValue == "2" || ownerValue == "30") && 
                czkfbjFeatureClass != null)
            {
                return IsFeatureInBoundaryOptimized(feature, spatialFilter);
            }

            return false;
        }

        // **优化的空间查询方法**
        private bool IsFeatureInBoundaryOptimized(IFeature feature, ISpatialFilter spatialFilter)
        {
            try
            {
                if (spatialFilter == null)
                    return false;

                // **优化: 复用空间过滤器对象**
                spatialFilter.Geometry = feature.Shape;
                
                // **优化: 使用计数查询而不是获取要素**
                int count = czkfbjFeatureClass.FeatureCount(spatialFilter);
                return count > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"优化空间查询出错: {ex.Message}");
                return false;
            }
        }

        // **新增辅助类和方法**
        private FieldIndices GetFieldIndices(string landTypeField, string landOwnerField)
        {
            var indices = new FieldIndices();
            
            // 缓存字段索引以避免重复查找
            indices.TbdhIndex = lcxzgxFeatureClass.FindField("TBDH");
            if (indices.TbdhIndex == -1) indices.TbdhIndex = lcxzgxFeatureClass.FindField("BSM");
            if (indices.TbdhIndex == -1) indices.TbdhIndex = lcxzgxFeatureClass.FindField("OBJECTID");

            indices.DlmcIndex = lcxzgxFeatureClass.FindField(landTypeField);
            indices.TdqsIndex = lcxzgxFeatureClass.FindField(landOwnerField);
            indices.TbmjIndex = lcxzgxFeatureClass.FindField("TBMJ");
            indices.QsxzIndex = lcxzgxFeatureClass.FindField("QSXZ");
            
            // 备选面积字段
            if (indices.TbmjIndex == -1)
            {
                indices.TbmjIndex = lcxzgxFeatureClass.FindField("MJ");
                if (indices.TbmjIndex == -1) indices.TbmjIndex = lcxzgxFeatureClass.FindField("AREA");
                if (indices.TbmjIndex == -1) indices.TbmjIndex = lcxzgxFeatureClass.FindField("面积");
            }
            
            if (indices.QsxzIndex == -1 && indices.TdqsIndex != -1)
            {
                indices.QsxzIndex = indices.TdqsIndex;
            }

            return indices;
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

        private DataRow CreateDataRow(IFeature feature, FieldIndices fieldIndices, DataTable dataTable)
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

        private string TranslateOwnershipCode(string ownerValue)
        {
            switch (ownerValue)
            {
                case "1":
                case "20":
                    return "国有";
                case "2":
                case "30":
                    return "集体";
                default:
                    return ownerValue;
            }
        }

        private void DisplayPreviewResults(PreviewQueryResult result, int maxCount)
        {
            // 显示数据
            dgvPreview.DataSource = result.PreviewData;

            // 更新显示信息
            if (result.TotalCount > result.ProcessedCount)
            {
                lblPreviewCount.Text = $"预览结果：{result.ProcessedCount}/{result.TotalCount} 个图斑 (仅显示前 {maxCount} 个)";
            }
            else
            {
                lblPreviewCount.Text = $"预览结果：{result.ProcessedCount} 个图斑";
            }
        }

        // **新增辅助类**
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

        private List<IFeature> QueryFilteredFeaturesForExport(string landTypeField, string landOwnerField)
        {
            var features = new List<IFeature>();
            
            try
            {
                // **优化: 使用相同的优化查询逻辑**
                string optimizedWhereClause = BuildOptimizedWhereClause(landTypeField, landOwnerField);
                
                IQueryFilter queryFilter = new QueryFilterClass();
                if (!string.IsNullOrEmpty(optimizedWhereClause))
                {
                    queryFilter.WhereClause = optimizedWhereClause;
                }

                var fieldIndices = GetFieldIndices(landTypeField, landOwnerField);
                
                // **优化: 缓存空间过滤器**
                ISpatialFilter cachedSpatialFilter = null;
                if (chkCollectiveInBoundary.Checked && czkfbjFeatureClass != null)
                {
                    cachedSpatialFilter = new SpatialFilterClass();
                    cachedSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                }

                IFeatureCursor cursor = null;
                IFeature feature = null;
                int processedCount = 0;

                try
                {
                    cursor = lcxzgxFeatureClass.Search(queryFilter, false);

                    while ((feature = cursor.NextFeature()) != null)
                    {
                        if (ShouldIncludeFeature(feature, fieldIndices, cachedSpatialFilter))
                        {
                            features.Add(feature);
                            feature = null; // 防止在 finally 中释放
                        }
                        else if (feature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                            feature = null;
                        }

                        processedCount++;
                        
                        // **优化: 减少UI更新频率**
                        if (processedCount % 500 == 0)
                        {
                            UpdateStatus($"正在筛选图斑: 已处理 {processedCount} 条记录...");
                            Application.DoEvents();
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
                System.Diagnostics.Debug.WriteLine($"查询导出要素时出错: {ex.Message}");
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

        // 在地图上高亮显示筛选出的要素
        private void HighlightFeaturesOnMap(List<IFeature> features)
        {
            try
            {
                // 这里实现与ArcMap交互的代码，将筛选出的要素在地图上高亮显示
                // 通常涉及获取当前地图文档和图层，然后设置选择集
                // 此部分需要根据具体的ArcObjects环境实现
                
                // 示例代码（需要根据实际ArcObjects环境调整）
                 //IActiveView activeView = (ArcMap.Document.FocusMap as IActiveView);
                 //IMap map = activeView.FocusMap;
                 //map.ClearSelection();
                
                 //foreach (IFeature feature in features)
                 //{
                 //    map.SelectFeature(lcxzgxFeatureLayer, feature);
                 //}
                
                 //activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"高亮显示要素出错: {ex.Message}");
                // 这里不抛出异常，因为高亮显示失败不应阻止其他操作
            }
        }

        private void InitializeMappingGrid()
        {
            mappingData = new DataTable();
            mappingData.Columns.Add("目标字段");
            mappingData.Columns.Add("源字段");
            mappingData.Columns.Add("映射状态");

            // 添加示例映射字段
            string[] targetFields = { "TBDH", "DLMC", "TDQS", "MJ", "LZFL" };
            foreach (string field in targetFields)
            {
                mappingData.Rows.Add(field, "", "未映射");
            }

            dgvMapping.DataSource = mappingData;
        }

        private void btnAutoMapping_Click(object sender, EventArgs e)
        {
            // 执行自动映射逻辑
            UpdateStatus("正在执行自动映射...");
            progressBar.Value = 30;

            // 模拟自动映射
            foreach (DataRow row in mappingData.Rows)
            {
                string targetField = row["目标字段"].ToString();
                switch (targetField)
                {
                    case "TBDH":
                        row["源字段"] = "图斑编号";
                        row["映射状态"] = "已映射";
                        break;
                    case "DLMC":
                        row["源字段"] = "地类";
                        row["映射状态"] = "已映射";
                        break;
                    case "TDQS":
                        row["源字段"] = "土地权属";
                        row["映射状态"] = "已映射";
                        break;
                }
            }

            dgvMapping.Refresh();
            progressBar.Value = 100;
            UpdateStatus("自动映射完成");
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
                dialog.FileName = "FieldMappingTemplate.xml";
                
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
                    // 修复：使用 Attributes 属性访问 XML 属性值
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
                Name = "自定义字段映射模板",
                Description = "基于当前字段映射配置生成的模板",
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
            string[] requiredFields = { "TBDH", "DLMC", "TDQS", "MJ" };
            return requiredFields.Contains(fieldName);
        }

        /// <summary>
        /// 获取字段描述
        /// </summary>
        private string GetFieldDescription(string fieldName)
        {
            var descriptions = new Dictionary<string, string>
            {
                { "TBDH", "图斑编号，唯一标识符" },
                { "DLMC", "地类名称" },
                { "TDQS", "土地权属性质" },
                { "MJ", "图斑面积，单位：公顷" },
                { "LZFL", "林种分类信息" }
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
            UpdateStatus("操作已取消");
            btnExecute.Enabled = true;
            btnCancel.Enabled = false;
            progressBar.Value = 0;
        }

        private void UpdateStatus(string message)
        {
            lblStatus.Text = $"状态：{message}";
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
                // 获取林草现状文件列表
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetLCXZGXFiles();
                System.Diagnostics.Debug.WriteLine($"从SharedDataManager加载 {lcxzgxFiles.Count} 个LCXZGX文件");
                
                // 清空下拉框
                cmbLCXZGXPath.Items.Clear();
                
                // 添加提示项
                cmbLCXZGXPath.Items.Add("-- 请选择林草现状图层 --");
                
                // 添加找到的文件
                if (lcxzgxFiles.Count > 0)
                {
                    foreach (var file in lcxzgxFiles)
                    {
                        cmbLCXZGXPath.Items.Add(file);
                        System.Diagnostics.Debug.WriteLine($"添加LCXZGX文件到下拉框: {file.DisplayName} -> {file.FullPath}");
                    }
                    
                    // 如果有文件，选择第一个文件项（索引为1，因为索引0是提示项）
                    if (cmbLCXZGXPath.Items.Count > 1)
                    {
                        cmbLCXZGXPath.SelectedIndex = 1;
                        System.Diagnostics.Debug.WriteLine($"自动选择第一个LCXZGX文件: {((LCXZGXFileInfo)cmbLCXZGXPath.SelectedItem).DisplayName}");
                    }
                    else
                    {
                        cmbLCXZGXPath.SelectedIndex = 0;
                    }
                }
                else
                {
                    // 没有文件时选择提示项
                    cmbLCXZGXPath.SelectedIndex = 0;
                    System.Diagnostics.Debug.WriteLine("没有LCXZGX文件可用");
                }
                
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
                        System.Diagnostics.Debug.WriteLine($"自动选择第一个CZKFBJ文件: {((LCXZGXFileInfo)cmbCZKFBJPath.SelectedItem).DisplayName}");
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
                if (lcxzgxFiles.Count > 0 || czkfbjFiles.Count > 0)
                {
                    UpdateStatus($"已加载共享数据源: {lcxzgxFiles.Count}个林草现状文件, {czkfbjFiles.Count}个城镇开发边界文件");
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
                // 获取林草现状文件列表
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetLCXZGXFiles();
                System.Diagnostics.Debug.WriteLine($"获取到 {lcxzgxFiles.Count} 个林草现状文件");

                // 从林草现状文件中提取县名
                foreach (var file in lcxzgxFiles)
                {
                    if (!string.IsNullOrEmpty(file.DisplayName))
                    {
                        countyNames.Add(file.DisplayName);
                        System.Diagnostics.Debug.WriteLine($"林草现状文件县名: {file.DisplayName}");
                    }
                }

                // 获取城镇开发边界文件列表
                var czkfbjFiles = ForestResourcePlugin.SharedDataManager.GetCZKFBJFiles();
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
        /// 验证县数据完整性
        /// </summary>
        private bool ValidateCountyData(string countyName)
        {
            try
            {
                // 检查该县是否有林草现状数据
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetLCXZGXFiles();
                bool hasLCXZGX = lcxzgxFiles.Any(f => f.DisplayName == countyName);

                // 检查该县是否有城镇开发边界数据
                var czkfbjFiles = ForestResourcePlugin.SharedDataManager.GetCZKFBJFiles();
                bool hasCZKFBJ = czkfbjFiles.Any(f => f.DisplayName == countyName);

                System.Diagnostics.Debug.WriteLine($"县 {countyName} 数据完整性: 林草现状={hasLCXZGX}, 城镇开发边界={hasCZKFBJ}");

                // 至少需要有林草现状数据
                return hasLCXZGX;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"验证县 {countyName} 数据时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 刷新县列表
        /// </summary>
        private void RefreshCountyList()
        {
            try
            {
                // 保存当前选中状态
                var selectedCounties = GetSelectedCounties();

                // 重新加载县列表
                LoadCounties();

                // 恢复选中状态
                RestoreCountySelection(selectedCounties);

                UpdateStatus("县列表已刷新");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"刷新县列表时出错: {ex.Message}");
                UpdateStatus("刷新县列表失败");
            }
        }

        /// <summary>
        /// 恢复县选中状态
        /// </summary>
        private void RestoreCountySelection(List<string> selectedCounties)
        {
            try
            {
                for (int i = 0; i < chkListCounties.Items.Count; i++)
                {
                    string countyName = chkListCounties.Items[i].ToString();
                    bool shouldBeChecked = selectedCounties.Contains(countyName);
                    chkListCounties.SetItemChecked(i, shouldBeChecked);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"恢复县选中状态时出错: {ex.Message}");
            }
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
