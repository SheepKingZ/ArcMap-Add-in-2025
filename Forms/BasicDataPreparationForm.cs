using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // 用于CountySelectionForm
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry; // 新增：用于空间参考系统
using ESRI.ArcGIS.esriSystem; // 新增：用于空间参考系统
using System.Reflection; // 新增：用于反射调用

namespace TestArcMapAddin2.Forms
{
    public partial class BasicDataPreparationForm : Form
    {
        // 修改私有字段 - 合并林草湿荒普查数据和城镇开发边界数据路径为一个
        private string dataSourcePath = "";
        private string outputGDBPath = "";

        /// <summary>
        /// CGCS2000坐标系WKT定义
        /// </summary>
        private const string CGCS2000_WKT = @"GEOGCS[""GCS_China_Geodetic_Coordinate_System_2000"",DATUM[""D_China_2000"",SPHEROID[""CGCS2000"",6378137.0,298.257222101]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]]";

        /// <summary>
        /// CGCS2000 3度带37带投影坐标系WKT定义
        /// </summary>
        private const string CGCS2000_3_DEGREE_GK_ZONE_37_WKT = @"PROJCS[""GCS_China_Geodetic_Coordinate_System_2000_3_Degree_GK_Zone_37"",GEOGCS[""GCS_China_Geodetic_Coordinate_System_2000"",DATUM[""D_China_2000"",SPHEROID[""CGCS2000"",6378137.0,298.257222101]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]],PROJECTION[""Gauss_Kruger""],PARAMETER[""False_Easting"",37500000.0],PARAMETER[""False_Northing"",0.0],PARAMETER[""Central_Meridian"",111.0],PARAMETER[""Scale_Factor"",1.0],PARAMETER[""Latitude_Of_Origin"",0.0],UNIT[""Meter"",1.0]]";

        public BasicDataPreparationForm()
        {
            InitializeComponent();
            InitializeFormState();
        }

        private void InitializeFormState()
        {
            // 根据当前共享状态或默认值初始化标签
            lblWorkspace.Text = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? "未选择工作空间" : SharedWorkflowState.WorkspacePath;
            lblWorkspace.ForeColor = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? Color.Black : Color.DarkGreen;

            // 初始化合并后的数据源路径状态
            if (!string.IsNullOrEmpty(dataSourcePath))
            {
                txtDataPath.Text = dataSourcePath;
                txtDataPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtDataPath.Text = "请选择包含林草湿荒普查与城镇开发边界数据的文件夹";
                txtDataPath.ForeColor = Color.Gray;
            }

            if (!string.IsNullOrEmpty(outputGDBPath))
            {
                txtOutputGDBPath.Text = outputGDBPath;
                txtOutputGDBPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtOutputGDBPath.Text = "请选择输出结果GDB路径";
                txtOutputGDBPath.ForeColor = Color.Gray;
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasWorkspace = !string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath);
            bool hasDataSource = !string.IsNullOrEmpty(dataSourcePath);
            bool hasOutputGDB = !string.IsNullOrEmpty(outputGDBPath);

            // 基础数据源选择按钮始终启用
            btnBrowseData.Enabled = true;
            btnBrowseOutputGDB.Enabled = true;

            // OK按钮需要所有必要信息都完成后才启用
            bool allDataSourcesSelected = hasWorkspace && hasDataSource && hasOutputGDB;

            btnOK.Enabled = allDataSourcesSelected;
        }

        private void BtnSelectWorkspace_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择目标数据库路径（File Geodatabase .gdb）";
                if (!string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
                {
                    dialog.SelectedPath = SharedWorkflowState.WorkspacePath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SelectedPath.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase) || System.IO.Directory.Exists(dialog.SelectedPath))
                    {
                        SharedWorkflowState.WorkspacePath = dialog.SelectedPath;
                        lblWorkspace.Text = SharedWorkflowState.WorkspacePath;
                        lblWorkspace.ForeColor = Color.DarkGreen;
                        InitializeFormState(); // 重新初始化以反映状态变化
                    }
                    else
                    {
                        MessageBox.Show("请选择有效的File Geodatabase (.gdb) 路径！", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            UpdateButtonStates();
        }

        // 新的合并后的数据源浏览方法
        private void BtnBrowseData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择包含林草湿荒普查与城镇开发边界数据的文件夹";
                if (!string.IsNullOrEmpty(dataSourcePath))
                {
                    dialog.SelectedPath = dataSourcePath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    dataSourcePath = dialog.SelectedPath;
                    txtDataPath.Text = dataSourcePath;
                    txtDataPath.ForeColor = Color.DarkGreen;

                    // 保存到共享状态中
                    SharedWorkflowState.DataSourcePath = dataSourcePath;

                    // 查找包含LCXZGX_P的文件（林草湿荒普查数据）
                    List<ForestResourcePlugin.LCXZGXFileInfo> lcxzgxFiles = FindFilesWithPattern(dataSourcePath, "LCXZGX_P");

                    // 保存到共享数据管理器
                    ForestResourcePlugin.SharedDataManager.SetLCXZGXFiles(lcxzgxFiles);

                    // 查找包含CZKFBJ的文件（城镇开发边界数据）
                    List<ForestResourcePlugin.LCXZGXFileInfo> czkfbjFiles = FindFilesWithPattern(dataSourcePath, "CZKFBJ");

                    // 保存到共享数据管理器
                    ForestResourcePlugin.SharedDataManager.SetCZKFBJFiles(czkfbjFiles);

                    // 显示文件搜索结果
                    int totalFiles = lcxzgxFiles.Count + czkfbjFiles.Count;
                    if (totalFiles > 0)
                    {
                        MessageBox.Show($"在同一文件夹中找到：\n- {lcxzgxFiles.Count} 个林草湿荒普查数据文件\n- {czkfbjFiles.Count} 个城镇开发边界数据文件",
                            "文件搜索结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("未找到相关数据文件，请确认选择的文件夹是否正确。", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    UpdateButtonStates();
                }
            }
        }

        // 输出GDB路径浏览方法保持不变
        private void BtnBrowseOutputGDB_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择输出结果GDB路径";
                if (!string.IsNullOrEmpty(outputGDBPath))
                {
                    dialog.SelectedPath = outputGDBPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputGDBPath = dialog.SelectedPath;
                    txtOutputGDBPath.Text = outputGDBPath;
                    txtOutputGDBPath.ForeColor = Color.DarkGreen;

                    // 保存到共享状态
                    SharedWorkflowState.OutputGDBPath = outputGDBPath;

                    MessageBox.Show("输出结果GDB路径选择完成。", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateButtonStates();
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // 验证所有必需的路径都已选择
            if (string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
            {
                MessageBox.Show("请选择工作空间。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(dataSourcePath))
            {
                MessageBox.Show("请选择数据源文件夹。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(outputGDBPath))
            {
                MessageBox.Show("请选择输出结果GDB路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 可以在这里实现具体的业务逻辑，比如开始处理数据
            if (chkCreateCountyFolders.Checked)
            {

                MessageBox.Show($"准备为每个县创建文件夹并生成结果表格。\n" +
                              $"工作空间：{SharedWorkflowState.WorkspacePath}\n" +
                              $"数据源文件夹：{dataSourcePath}\n" +
                              $"输出GDB路径：{outputGDBPath}",
                              "处理确认", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //为每个县创建数据库及其成果表
            /*  if (!String.IsNullOrEmpty(dataSourcePath))
              {
                  System.IO.DirectoryInfo theFolder = new System.IO.DirectoryInfo(dataSourcePath);
                  System.IO.DirectoryInfo[] dir_Countries = theFolder.GetDirectories();
                  foreach (System.IO.DirectoryInfo dirInfo in dir_Countries) {
                      String curDir = dirInfo.FullName;
                      String countryName = curDir.Substring(curDir.LastIndexOf('\\')+1);
                      if(!CreateTable4Country(outputGDBPath, countryName))
                      {
                          MessageBox.Show("创建"+countryName+"数据库失败");
                      }
                  }
              }*/



            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // 更新公共属性，供外部访问选择的路径
        public string DataSourcePath => dataSourcePath;
        public string OutputGDBPath => outputGDBPath;
        public bool CreateCountyFolders => chkCreateCountyFolders.Checked;

        private void BasicDataPreparationForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 提取路径中的第一级文件夹名称（县名）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="rootDir">根目录</param>
        /// <returns>第一级文件夹名称（县名）</returns>
        private string ExtractCountyNameFromPath(string filePath, string rootDir)
        {
            try
            {
                // 规范化路径
                string normalizedRoot = System.IO.Path.GetFullPath(rootDir).TrimEnd('\\', '/');
                string normalizedFile = System.IO.Path.GetFullPath(filePath);

                // 计算相对路径
                string relativePath = "";
                if (normalizedFile.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = normalizedFile.Substring(normalizedRoot.Length).TrimStart('\\', '/');
                }
                else
                {
                    // 路径不匹配，尝试从完整路径提取
                    System.Diagnostics.Debug.WriteLine($"警告: 文件路径 {normalizedFile} 不在根目录 {normalizedRoot} 下");
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                }

                // 分割路径并获取第一级目录名称（县名）
                string[] pathParts = relativePath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 1)
                {
                    // 返回第一级目录名称，这应该是县名
                    string countyName = pathParts[0];
                    System.Diagnostics.Debug.WriteLine($"从路径 {relativePath} 提取县名: {countyName}");
                    return countyName;
                }
                else
                {
                    // 兜底方案：如果路径解析失败，使用文件名
                    string fallbackName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    System.Diagnostics.Debug.WriteLine($"警告: 无法从路径提取县名，使用文件名: {fallbackName}");
                    return fallbackName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"提取县名时出错: {ex.Message}");
                return System.IO.Path.GetFileNameWithoutExtension(filePath);
            }
        }

        /// <summary>
        /// 查找指定目录下名称包含特定字符串的文件
        /// </summary>
        /// <param name="rootDir">根目录</param>
        /// <param name="pattern">名称匹配模式</param>
        /// <returns>文件信息列表</returns>
        private List<ForestResourcePlugin.LCXZGXFileInfo> FindFilesWithPattern(string rootDir, string pattern)
        {
            var result = new List<ForestResourcePlugin.LCXZGXFileInfo>();

            try
            {
                System.Diagnostics.Debug.WriteLine($"开始在 {rootDir} 目录下查找包含 {pattern} 的文件");

                // 1. 首先查找GDB要素类
                System.Diagnostics.Debug.WriteLine("第1步：查找GDB要素类...");
                var gdbFeatureClasses = ForestResourcePlugin.GdbFeatureClassFinder.FindFeatureClassesWithPattern(
                    rootDir, pattern, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);

                // 将找到的GDB要素类添加到结果中
                result.AddRange(gdbFeatureClasses);
                System.Diagnostics.Debug.WriteLine($"找到 {gdbFeatureClasses.Count} 个GDB要素类");

                // 2. 再查找Shapefile文件
                System.Diagnostics.Debug.WriteLine("第2步：查找Shapefile文件...");

                // 确保目录存在
                if (Directory.Exists(rootDir))
                {
                    string[] files = System.IO.Directory.GetFiles(rootDir, "*.shp", System.IO.SearchOption.AllDirectories);
                    System.Diagnostics.Debug.WriteLine($"在 {rootDir} 目录下找到 {files.Length} 个Shapefile文件");

                    // 筛选包含指定模式的Shapefile
                    int matchCount = 0;
                    foreach (string filePath in files)
                    {
                        if (filePath.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // 提取县名（第一级文件夹名称）
                            string countyName = ExtractCountyNameFromPath(filePath, rootDir);

                            result.Add(new ForestResourcePlugin.LCXZGXFileInfo
                            {
                                FullPath = filePath,
                                DisplayName = countyName,
                                IsGdb = false,
                                GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon // 假设为面
                            });

                            matchCount++;
                            System.Diagnostics.Debug.WriteLine($"找到匹配的Shapefile文件[{matchCount}]: {filePath}, 县名: {countyName}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 目录 {rootDir} 不存在");
                }

                System.Diagnostics.Debug.WriteLine($"共找到 {result.Count} 个匹配文件 (GDB要素类: {gdbFeatureClasses.Count}, Shapefile: {result.Count - gdbFeatureClasses.Count})");

                // 输出详细的结果信息
                for (int i = 0; i < result.Count; i++)
                {
                    var item = result[i];
                    System.Diagnostics.Debug.WriteLine($"结果[{i + 1}]: {item.DisplayName}, 路径: {item.FullPath}, 类型: {(item.IsGdb ? "GDB要素类" : "Shapefile")}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找文件时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                MessageBox.Show($"查找文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        /// <summary>
        /// 提取路径中的第二级文件夹名称
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="rootDir">根目录</param>
        /// <returns>第二级文件夹名称</returns>
        [Obsolete("此方法已过时，请使用 ExtractCountyNameFromPath 方法")]
        private string ExtractSecondLevelFolderName(string filePath, string rootDir)
        {
            // 保留原方法以维持向后兼容性，但标记为过时
            // 现在调用新的县名提取方法
            return ExtractCountyNameFromPath(filePath, rootDir);
        }

        private Boolean CreateTable4Country(String path, String countryName)
        {
            if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(countryName))
            {
                DirectoryInfo sourceFolder = new DirectoryInfo(path);
                DirectoryInfo countryFolder = sourceFolder.CreateSubdirectory(countryName);
                if (countryFolder.Exists)
                {
                    Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                    IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                    IWorkspaceName workspaceName = workspaceFactory.Create(countryFolder.FullName, countryName + ".gdb", null, 0);

                    // Cast the workspace name object to the IName interface and open the workspace.
                    ESRI.ArcGIS.esriSystem.IName name = (ESRI.ArcGIS.esriSystem.IName)workspaceName;
                    IWorkspace workspace = (IWorkspace)name.Open();

                    if (!CreateResultTable("LCXZGX", workspace))
                    {
                        MessageBox.Show("创建现状要素类失败");
                        return false;
                    }
                    if (!CreateResultTable("SLZYZC", workspace))
                    {
                        MessageBox.Show("创建清查成果要素类失败");
                        return false;
                    }
                    if (!CreateResultTable("SLZYZC_DLTB", workspace))
                    {
                        MessageBox.Show("创建清查林地要素类失败");
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        //根据成果表名创建表
        private Boolean CreateResultTable(String TableName, IWorkspace workspace)
        {
            String featureClassName = TableName;
            IFeatureClass featureClass = null;
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

            try
            {
                // 检查要素类是否已存在
                bool featureClassExists = false;
                try
                {
                    featureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                    featureClassExists = true;
                    System.Diagnostics.Debug.WriteLine($"要素类{featureClassName}已存在，将设置CGCS2000坐标系");

                    // 为已存在的要素类设置CGCS2000坐标系
                    SetCoordinateSystemForExistingFeatureClass(featureClass, featureClassName);

                    return true;
                }
                catch
                {
                    // 要素类不存在，继续创建
                    featureClassExists = false;
                }

                if (!featureClassExists)
                {
                    System.Diagnostics.Debug.WriteLine($"开始创建{featureClassName}要素类并设置CGCS2000坐标系");

                    ESRI.ArcGIS.esriSystem.UID CLSID = new ESRI.ArcGIS.esriSystem.UIDClass();
                    CLSID.Value = "esriGeoDatabase.Feature";

                    IObjectClassDescription objectClassDescription = new FeatureClassDescriptionClass();
                    IFields fields = objectClassDescription.RequiredFields;
                    IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                    // 根据表名生成相应的字段
                    switch (featureClassName)
                    {
                        case "LCXZGX":
                            FeatureClassFieldsTemplate.GenerateLcxzgxFields(fieldsEdit);
                            break;
                        case "SLZYZC":
                            FeatureClassFieldsTemplate.GenerateSlzyzcFields(fieldsEdit);
                            break;
                        case "SLZYZC_DLTB":
                            FeatureClassFieldsTemplate.GenerateSlzyzc_dltbFields(fieldsEdit);
                            break;
                    }

                    fields = (IFields)fieldsEdit;

                    // 查找几何字段并设置CGCS2000坐标系
                    String strShapeField = "";
                    for (int j = 0; j < fields.FieldCount; j++)
                    {
                        IField field = fields.get_Field(j);
                        if (field.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            strShapeField = field.Name;

                            // 获取几何字段定义
                            IGeometryDef geometryDef = field.GeometryDef;
                            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

                            // 创建并设置CGCS2000空间参考系统
                            ISpatialReference cgcs2000SpatialRef = CreateCGCS2000SpatialReference();
                            if (cgcs2000SpatialRef != null)
                            {
                                geometryDefEdit.SpatialReference_2 = cgcs2000SpatialRef;
                                System.Diagnostics.Debug.WriteLine($"为{featureClassName}的几何字段{strShapeField}设置CGCS2000坐标系");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"警告：无法为{featureClassName}设置CGCS2000坐标系，将使用默认坐标系");
                            }
                            break;
                        }
                    }

                    // Use IFieldChecker to create a validated fields collection.
                    IFieldChecker fieldChecker = new FieldCheckerClass();
                    IEnumFieldError enumFieldError = null;
                    IFields validatedFields = null;
                    fieldChecker.ValidateWorkspace = (IWorkspace)workspace;
                    fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                    if (enumFieldError != null)
                    {
                        MessageBox.Show("字段校验失败：" + featureClassName);
                        return false;
                    }

                    // 创建要素类
                    featureClass = featureWorkspace.CreateFeatureClass(featureClassName, validatedFields, CLSID, null, esriFeatureType.esriFTSimple, strShapeField, "");

                    if (featureClass == null)
                    {
                        MessageBox.Show("为" + featureClassName + "创建要素类失败");
                        return false;
                    }

                    System.Diagnostics.Debug.WriteLine($"成功创建{featureClassName}要素类并设置CGCS2000坐标系");
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建/设置{featureClassName}要素类时出错: {ex.Message}");
                MessageBox.Show($"创建/设置{featureClassName}要素类时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // 释放COM对象
                if (featureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                }
            }
        }

        /// <summary>
        /// 创建CGCS2000投影坐标系（优先使用3度带37带投影坐标系）
        /// </summary>
        /// <returns>CGCS2000坐标系对象</returns>
        private ISpatialReference CreateCGCS2000SpatialReference()
        {
            // 首先尝试创建投影坐标系
            ISpatialReference projectedSpatialRef = CreateCGCS2000ProjectedSpatialReference();
            if (projectedSpatialRef != null)
            {
                return projectedSpatialRef;
            }

            // 如果投影坐标系创建失败，使用地理坐标系作为备用
            return CreateCGCS2000GeographicSpatialReference();
        }

        /// <summary>
        /// 创建CGCS2000 3度带37带投影坐标系
        /// </summary>
        /// <returns>CGCS2000 3度带37带投影坐标系对象</returns>
        private ISpatialReference CreateCGCS2000ProjectedSpatialReference()
        {
            try
            {
                // 方法1：尝试使用自定义EPSG代码创建CGCS2000 3度带37带投影坐标系
                try
                {
                    // 创建空间参考系统环境接口
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);
                    ISpatialReferenceFactory spatialRefFactory = spatialRefEnvObj as ISpatialReferenceFactory;

                    if (spatialRefFactory != null)
                    {
                        // 尝试使用可能的EPSG代码（CGCS2000 3度带投影坐标系通常以4491+带号的形式）
                        try
                        {
                           
                            IProjectedCoordinateSystem projectedCS = spatialRefFactory.CreateProjectedCoordinateSystem(4525);
                            if (projectedCS != null)
                            {
                                System.Diagnostics.Debug.WriteLine("成功使用EPSG 4528创建CGCS2000 3度带37带投影坐标系");
                                return projectedCS as ISpatialReference;
                            }
                        }
                        catch (Exception ex1)
                        {
                            System.Diagnostics.Debug.WriteLine($"使用EPSG 4528创建投影坐标系失败: {ex1.Message}");
                        }
                    }
                }
                catch (Exception ex1)
                {
                    System.Diagnostics.Debug.WriteLine($"使用EPSG代码创建CGCS2000投影坐标系失败: {ex1.Message}");
                }

                // 方法2：使用WKT字符串创建投影坐标系
                try
                {
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);

                    // 使用反射调用CreateESRISpatialReferenceFromPRJString方法
                    System.Reflection.MethodInfo createFromPrjMethod = spatialRefEnvType.GetMethod("CreateESRISpatialReferenceFromPRJString");
                    if (createFromPrjMethod != null)
                    {
                        object[] parameters = new object[] { CGCS2000_3_DEGREE_GK_ZONE_37_WKT, null, null };
                        object result = createFromPrjMethod.Invoke(spatialRefEnvObj, parameters);

                        if (result != null && result is ISpatialReference)
                        {
                            System.Diagnostics.Debug.WriteLine("成功使用WKT字符串创建CGCS2000 3度带37带投影坐标系");
                            return result as ISpatialReference;
                        }
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"使用WKT字符串创建CGCS2000投影坐标系失败: {ex2.Message}");
                }

                System.Diagnostics.Debug.WriteLine("所有投影坐标系创建方法都失败");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建CGCS2000投影坐标系时出现意外错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 创建CGCS2000地理坐标系（备用方案）
        /// </summary>
        /// <returns>CGCS2000地理坐标系对象</returns>
        private ISpatialReference CreateCGCS2000GeographicSpatialReference()
        {
            try
            {
                // 方法1：尝试使用EPSG代码4490创建CGCS2000地理坐标系
                try
                {
                    // 创建空间参考系统环境接口
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);
                    ISpatialReferenceFactory spatialRefFactory = spatialRefEnvObj as ISpatialReferenceFactory;

                    if (spatialRefFactory != null)
                    {
                        // 使用EPSG代码4490创建CGCS2000地理坐标系
                        IGeographicCoordinateSystem geographicCS = spatialRefFactory.CreateGeographicCoordinateSystem(4490);
                        if (geographicCS != null)
                        {
                            System.Diagnostics.Debug.WriteLine("成功使用EPSG 4490创建CGCS2000地理坐标系");
                            return geographicCS as ISpatialReference;
                        }
                    }
                }
                catch (Exception ex1)
                {
                    System.Diagnostics.Debug.WriteLine($"使用EPSG 4490创建CGCS2000失败: {ex1.Message}");
                }

                // 方法2：备用方案 - 尝试从WKT字符串创建
                try
                {
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);

                    // 使用反射调用CreateESRISpatialReferenceFromPRJString方法
                    System.Reflection.MethodInfo createFromPrjMethod = spatialRefEnvType.GetMethod("CreateESRISpatialReferenceFromPRJString");
                    if (createFromPrjMethod != null)
                    {
                        object[] parameters = new object[] { CGCS2000_WKT, null, null };
                        object result = createFromPrjMethod.Invoke(spatialRefEnvObj, parameters);

                        if (result != null && result is ISpatialReference)
                        {
                            System.Diagnostics.Debug.WriteLine("成功使用WKT字符串创建CGCS2000地理坐标系");
                            return result as ISpatialReference;
                        }
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"使用WKT字符串创建CGCS2000失败: {ex2.Message}");
                }

                // 方法3：最后的备用方案 - 创建一个通用的地理坐标系
                try
                {
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);
                    ISpatialReferenceFactory spatialRefFactory = spatialRefEnvObj as ISpatialReferenceFactory;

                    if (spatialRefFactory != null)
                    {
                        // 创建WGS84地理坐标系作为备用
                        IGeographicCoordinateSystem wgs84CS = spatialRefFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                        if (wgs84CS != null)
                        {
                            System.Diagnostics.Debug.WriteLine("警告：使用WGS84坐标系作为备用方案");
                            return wgs84CS as ISpatialReference;
                        }
                    }
                }
                catch (Exception ex3)
                {
                    System.Diagnostics.Debug.WriteLine($"创建备用坐标系也失败: {ex3.Message}");
                }

                System.Diagnostics.Debug.WriteLine("所有创建CGCS2000地理坐标系的方法都失败了");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建CGCS2000地理坐标系时出现意外错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 为已存在的要素类设置CGCS2000坐标系
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <param name="featureClassName">要素类名称</param>
        private void SetCoordinateSystemForExistingFeatureClass(IFeatureClass featureClass, string featureClassName)
        {
            try
            {
                // 创建CGCS2000空间参考系统
                ISpatialReference cgcs2000SpatialRef = CreateCGCS2000SpatialReference();
                if (cgcs2000SpatialRef == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法为{featureClassName}设置坐标系：空间参考系统创建失败");
                    return;
                }

                // 从要素类获取地理数据集对象
                IGeoDataset geoDataset = (IGeoDataset)featureClass;

                // 检查当前坐标系
                ISpatialReference currentSpatialRef = geoDataset.SpatialReference;
                if (currentSpatialRef != null)
                {
                    System.Diagnostics.Debug.WriteLine($"{featureClassName}当前已有坐标系，准备更改为CGCS2000");
                }

                // 转换为IGeoDatasetSchemaEdit对象以修改坐标系
                IGeoDatasetSchemaEdit schemaEdit = (IGeoDatasetSchemaEdit)geoDataset;

                // 为要素类定义CGCS2000坐标系
                schemaEdit.AlterSpatialReference(cgcs2000SpatialRef);

                System.Diagnostics.Debug.WriteLine($"成功为{featureClassName}要素类设置CGCS2000坐标系");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"为{featureClassName}设置坐标系时出错: {ex.Message}");
                // 不抛出异常，确保其他处理流程继续
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "请选择数据总目录";
            // folderBrowser.ShowNewFolderButton = true;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                dataSourcePath = folderBrowser.SelectedPath;
            }

            if (!String.IsNullOrEmpty(dataSourcePath))
            {
                System.IO.DirectoryInfo theFolder = new System.IO.DirectoryInfo(dataSourcePath);
                System.IO.DirectoryInfo[] dir_Countries = theFolder.GetDirectories();
                foreach (System.IO.DirectoryInfo dirInfo in dir_Countries)
                {
                    String curDir = dirInfo.FullName;
                    String countryName = curDir.Substring(curDir.LastIndexOf('\\') + 1);
                    if (!CreateTable4Country(outputGDBPath, countryName))
                    {
                        MessageBox.Show("创建" + countryName + "数据库失败");
                        return;
                    }
                }
                MessageBox.Show("已建立成果数据库及表结构");
            }
            else
            {
                MessageBox.Show("源路径为空");
            }
        }
    }
}