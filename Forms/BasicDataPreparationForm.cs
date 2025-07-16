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
using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util; 

namespace TestArcMapAddin2.Forms
{
    public partial class BasicDataPreparationForm : Form
    {
        // 修改私有字段 - 合并林草湿荒普查数据和城镇开发边界数据路径为一个
        private string dataSourcePath = "";
        private string outputGDBPath = "";

        public ForestResourcePlugin.Basic ParentBasicForm { get; set;}
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

        // 合并数据源方法
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
                    List<ForestResourcePlugin.SourceDataFileInfo> lcxzgxFiles = FindFilesWithPattern(dataSourcePath, "LCXZGX_P");

                    // 🔥 修复：直接使用新的 SourceDataFileInfo 类型
                    ForestResourcePlugin.SharedDataManager.SetSourceDataFiles(lcxzgxFiles);

                    // 查找包含CZKFBJ的文件（城镇开发边界数据）
                    List<ForestResourcePlugin.SourceDataFileInfo> czkfbjFiles = FindFilesWithPattern(dataSourcePath, "CZKFBJ");

                    // 🔥 修复：直接使用新的 SourceDataFileInfo 类型
                    ForestResourcePlugin.SharedDataManager.SetCZKFBJFiles(czkfbjFiles);

                    // 新增：查找包含SLZY_DLTB的文件（森林资源地类图斑数据）
                    List<ForestResourcePlugin.SourceDataFileInfo> slzyDltbFiles = FindFilesWithPattern(dataSourcePath, "SLZY_DLTB");

                    // 🔥 修复：直接使用新的 SourceDataFileInfo 类型
                    ForestResourcePlugin.SharedDataManager.SetSLZYDLTBFiles(slzyDltbFiles);

                    // 显示文件搜索结果
                    int totalFiles = lcxzgxFiles.Count + czkfbjFiles.Count + slzyDltbFiles.Count;
                    if (totalFiles > 0)
                    {
                        MessageBox.Show($"在同一文件夹中找到：\n- {lcxzgxFiles.Count} 个林草湿荒普查数据文件\n- {czkfbjFiles.Count} 个城镇开发边界数据文件\n- {slzyDltbFiles.Count} 个森林资源地类图斑数据文件",
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

                    MessageBox.Show("输出结果路径选择完成。", "成功",
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
        private List<ForestResourcePlugin.SourceDataFileInfo> FindFilesWithPattern(string rootDir, string pattern)
        {
            var result = new List<ForestResourcePlugin.SourceDataFileInfo>();

            try
            {
                System.Diagnostics.Debug.WriteLine($"开始在 {rootDir} 目录下查找包含 {pattern} 的文件");

                // 1. 首先查找GDB要素类
                System.Diagnostics.Debug.WriteLine("第1步：查找GDB要素类...");
                var gdbFeatureClasses = ForestResourcePlugin.GdbFeatureClassFinder.FindFeatureClassesWithPatternAsSourceData(
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

                            result.Add(new ForestResourcePlugin.SourceDataFileInfo
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

        /// <summary>
        /// 为县创建空的Shapefile文件
        /// </summary>
        /// <param name="path">输出路径</param>
        /// <param name="countryName">县名</param>
        /// <param name="spatialReference">要使用的空间参考</param>
        /// <returns>是否成功</returns>
        private Boolean CreateTable4Country(String path, String countryName, ISpatialReference spatialReference)
        {
            if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(countryName))
            {
                DirectoryInfo sourceFolder = new DirectoryInfo(path);
                DirectoryInfo countryFolder = sourceFolder.CreateSubdirectory(countryName);
                if (countryFolder.Exists)
                {
                    try
                    {
                        // 创建Shapefile工作空间
                        Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                        IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                        IWorkspace workspace = workspaceFactory.OpenFromFile(countryFolder.FullName, 0);

                        // 创建两个空的Shapefile，并传入空间参考
                        if (!CreateEmptyShapefile("SLZYZC", workspace, spatialReference))
                        {
                            MessageBox.Show("创建SLZYZC Shapefile失败");
                            return false;
                        }
                        if (!CreateEmptyShapefile("SLZYZC_DLTB", workspace, spatialReference))
                        {
                            MessageBox.Show("创建SLZYZC_DLTB Shapefile失败");
                            return false;
                        }

                        System.Diagnostics.Debug.WriteLine($"成功为{countryName}创建两个空的Shapefile");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"为{countryName}创建Shapefile时出错: {ex.Message}");
                        MessageBox.Show($"为{countryName}创建Shapefile时出错: {ex.Message}");
                        return false;
                    }
                }
            }
            return false;
        }

        //根据成果表名创建表
        /// <summary>
        /// 创建空的Shapefile
        /// </summary>
        /// <param name="shapefileName">Shapefile名称</param>
        /// <param name="workspace">Shapefile工作空间</param>
        /// <returns>是否成功</returns>
         //根据成果表名创建表
        /// <summary>
        /// 创建空的Shapefile
        /// </summary>
        /// <param name="shapefileName">Shapefile名称</param>
        /// <param name="workspace">Shapefile工作空间</param>
        /// <param name="spatialReference">要使用的空间参考</param>
        /// <returns>是否成功</returns>
        private Boolean CreateEmptyShapefile(String shapefileName, IWorkspace workspace, ISpatialReference spatialReference)
        {
            IFeatureClass featureClass = null;
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

            try
            {
                // 检查Shapefile是否已存在
                if (((IWorkspace2)workspace).get_NameExists(esriDatasetType.esriDTFeatureClass, shapefileName))
                {
                    System.Diagnostics.Debug.WriteLine($"Shapefile {shapefileName} 已存在，将跳过创建。");
                    return true;
                }

                System.Diagnostics.Debug.WriteLine($"开始创建{shapefileName} Shapefile");

                // 创建字段集合
                IFields fields = new FieldsClass();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                // 添加OID字段
                IField oidField = new FieldClass();
                IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
                oidFieldEdit.Name_2 = "FID";
                oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldsEdit.AddField(oidField);

                // 添加几何字段
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;

                //使用传入的空间参考
                if (spatialReference != null)
                {
                    geometryDefEdit.SpatialReference_2 = spatialReference;
                    System.Diagnostics.Debug.WriteLine($"为{shapefileName}设置了源数据坐标系: {spatialReference.Name}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"警告：无法为{shapefileName}设置源坐标系，将使用默认坐标系");
                    geometryDefEdit.SpatialReference_2 = CreateCGCS2000SpatialReference(); // 备用方案
                }

                IField geometryField = new FieldClass();
                IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
                geometryFieldEdit.Name_2 = "Shape";
                geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                geometryFieldEdit.GeometryDef_2 = geometryDef;
                fieldsEdit.AddField(geometryField);

                // 根据Shapefile名称添加相应的业务字段
                switch (shapefileName)
                {
                    case "SLZYZC":
                        FeatureClassFieldsTemplate.GenerateSlzyzcFields(fieldsEdit);
                        break;
                    case "SLZYZC_DLTB":
                        FeatureClassFieldsTemplate.GenerateSlzyzc_dltbFields(fieldsEdit);
                        break;
                    case "CYZYZC":
                        FeatureClassFieldsTemplate.GenerateCyzyzcFields(fieldsEdit);
                        break;
                    case "CYZYZC_DLTB":
                        FeatureClassFieldsTemplate.GenerateCyzyzc_dltbFields(fieldsEdit);
                        break;
                    case "SDZYZC":
                        FeatureClassFieldsTemplate.GenerateSdzyzcFields(fieldsEdit);
                        break;
                    case "SDZYZC_DLTB":
                        FeatureClassFieldsTemplate.GenerateSdzyzc_dltbFields(fieldsEdit);
                        break;
                }

                fields = (IFields)fieldsEdit;

                // 创建Shapefile

                // 创建Shapefile
                featureClass = featureWorkspace.CreateFeatureClass(
                    shapefileName,
                    fields,
                    null,
                    null,
                    esriFeatureType.esriFTSimple,
                    "Shape",
                    "");

                if (featureClass == null)
                {
                    MessageBox.Show($"在路径 {workspace.PathName} 创建{shapefileName}失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"成功创建{shapefileName} Shapefile");

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建{shapefileName} Shapefile时出错: {ex.Message}");
                MessageBox.Show($"创建{shapefileName} Shapefile时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // 在 button1_Click 方法中更新县代码使用逻辑
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(outputGDBPath))
            {
                MessageBox.Show("输出结果路径为空，请先通过生成成果结构按钮选择或创建成果根目录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 使用新的 SourceDataFileInfo 类型
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetSourceDataFiles();
                if (lcxzgxFiles == null || lcxzgxFiles.Count == 0)
                {
                    MessageBox.Show("未能从共享数据中找到林草湿荒普查数据，无法继续操作。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 按县名对数据源进行分组，并获取唯一的县名列表
                var countyGroups = lcxzgxFiles.GroupBy(f => f.DisplayName)
                                              .ToDictionary(g => g.Key, g => g.First());

                if (countyGroups.Count == 0)
                {
                    MessageBox.Show("未能获取县列表，请先在数据源步骤中选择数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int successCount = 0;
                // 遍历每个县，创建Shapefile
                foreach (var countyEntry in countyGroups)
                {
                    string countyName = countyEntry.Key;
                    var firstFileInGroup = countyEntry.Value;

                    // 从源文件获取空间参考
                    ISpatialReference sourceSpatialRef = GetSpatialReferenceFromFile(firstFileInGroup);
                    if (sourceSpatialRef == null)
                    {
                        var userChoice = MessageBox.Show($"无法自动读取{ countyName}的源数据坐标系。\n\n是否继续并使用默认的CGCS2000坐标系？", "坐标系读取失败", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (userChoice == DialogResult.No)
                        {
                            MessageBox.Show($"已跳过{ countyName}的Shapefile创建。", "操作取消", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            continue; // 跳过当前县，继续处理下一个
                        }
                        sourceSpatialRef = CreateCGCS2000SpatialReference();
                    }

                    // 修改：使用县代码映射器获取真实的县代码
                    string countyCode = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyCode(countyName);
                    string countyFolderName = $"{countyName}（{countyCode}）全民所有自然资源资产清查数据成果";
                    string countyFolderPath = System.IO.Path.Combine(outputGDBPath, countyFolderName);
                    string dataSetPath = System.IO.Path.Combine(countyFolderPath, "清查数据集");

                    // 定义资源类型和对应的Shapefile名称
                    var resourceTypes = new Dictionary<string, string[]>
                    {
                        { "森林", new[] { "SLZYZC", "SLZYZC_DLTB" } },
                        { "草原", new[] { "CYZYZC", "CYZYZC_DLTB" } },
                        { "湿地", new[] { "SDZYZC", "SDZYZC_DLTB" } }
                    };

                    bool countySuccess = true;
                    foreach (var resource in resourceTypes)
                    {
                        string spatialDataPath = System.IO.Path.Combine(dataSetPath, resource.Key, "空间数据");
                        if (!Directory.Exists(spatialDataPath))
                        {
                            MessageBox.Show($"{ countyName}的目录结构不完整，找不到路径：\n{spatialDataPath}\n\n请先使用生成成果结构功能创建正确的目录。", "目录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            countySuccess = false;
                            break;
                        }

                        // 创建Shapefile工作空间
                        Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                        IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                        IWorkspace workspace = workspaceFactory.OpenFromFile(spatialDataPath, 0);

                        // 在指定路径下创建Shapefile
                        foreach (var shapefileName in resource.Value)
                        {
                            if (!CreateEmptyShapefile(shapefileName, workspace, sourceSpatialRef))
                            {
                                MessageBox.Show($"在路径 {spatialDataPath} 创建{shapefileName}失败。", "创建失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                countySuccess = false;
                                break;
                            }
                        }
                        if (!countySuccess) break;
                    }

                    if (countySuccess)
                    {
                        successCount++;
                        System.Diagnostics.Debug.WriteLine($"成功为{countyName}({countyCode})创建Shapefile");
                    }
                }

                if (successCount > 0)
                {
                    MessageBox.Show($"已成功为 {successCount} 个县在对应的成果目录中创建了所有必需的Shapefile。", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("未能成功创建任何县的Shapefile。请检查错误信息和目录结构。", "操作完成", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建Shapefile时发生错误：{ex.Message}", "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"创建Shapefile时出错: {ex}");
            }
        }

        private void lblOutputGDBPath_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 用于生成Excel报告的数据结构
        /// </summary>
        private class DataSourceReportItem
        {
            public string CountyName { get; set; }
            public string DataType { get; set; }
            public string Format { get; set; }
            public string Path { get; set; }
        }
        /// <summary>
        /// 从单个数据源文件获取空间参考
        /// </summary>
        /// <param name="fileInfo">数据源文件信息</param>
        /// <returns>空间参考对象，失败则返回null</returns>
        private ISpatialReference GetSpatialReferenceFromFile(ForestResourcePlugin.SourceDataFileInfo fileInfo)
        {
            if (fileInfo == null || string.IsNullOrEmpty(fileInfo.FullPath))
            {
                System.Diagnostics.Debug.WriteLine("GetSpatialReferenceFromFile失败: fileInfo或其路径为空。");
                return null;
            }

            IWorkspace workspace = null;
            IFeatureClass featureClass = null;
            try
            {
                string workspacePath;
                string featureClassName;

                if (fileInfo.IsGdb)
                {
                    // GDB路径处理
                    workspacePath = fileInfo.FullPath; // 对于GDB，FullPath应为.gdb目录的路径
                    featureClassName = fileInfo.FeatureClassName;
                    System.Diagnostics.Debug.WriteLine($"正在从GDB打开: Workspace='{workspacePath}', FeatureClass='{featureClassName}'");

                    Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                    IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                    workspace = workspaceFactory.OpenFromFile(workspacePath, 0);
                }
                else
                {
                    // Shapefile路径处理
                    workspacePath = System.IO.Path.GetDirectoryName(fileInfo.FullPath);
                    featureClassName = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullPath);
                    System.Diagnostics.Debug.WriteLine($"正在从Shapefile打开: Workspace='{workspacePath}', FeatureClass='{featureClassName}'");

                    Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                    IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                    workspace = workspaceFactory.OpenFromFile(workspacePath, 0);
                }

                featureClass = ((IFeatureWorkspace)workspace).OpenFeatureClass(featureClassName);
                if (featureClass != null)
                {
                    var spatialRef = ((IGeoDataset)featureClass).SpatialReference;
                    System.Diagnostics.Debug.WriteLine($"成功获取坐标系: {spatialRef?.Name ?? "未知"}");
                    return spatialRef;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从 {fileInfo.FullPath} 获取空间参考时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"错误详情: {ex.StackTrace}");
                return null;
            }
            finally
            {
                if (featureClass != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                if (workspace != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
            }
            return null;
        }
        private void btnResultExcel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(outputGDBPath))
            {
                MessageBox.Show("输出结果路径为空，请先通过生成成果结构按钮选择或创建成果根目录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 使用新的 SourceDataFileInfo 类型
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetSourceDataFiles();
                if (lcxzgxFiles == null || lcxzgxFiles.Count == 0)
                {
                    MessageBox.Show("未能从共享数据中找到林草湿荒普查数据，无法继续操作。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 按县名对数据源进行分组，并获取唯一的县名列表
                var countyGroups = lcxzgxFiles.GroupBy(f => f.DisplayName)
                                              .ToDictionary(g => g.Key, g => g.First());

                if (countyGroups.Count == 0)
                {
                    MessageBox.Show("未能获取县列表，请先在数据源步骤中选择数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int successCount = 0;
                int totalTables = 0;

                // 遍历每个县，生成三个Excel报表
                foreach (var countyEntry in countyGroups)
                {
                    string countyName = countyEntry.Key;

                    // 使用县代码映射器获取真实的县代码
                    string countyCode = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyCode(countyName);
                    string countyFolderName = $"{countyName}（{countyCode}）全民所有自然资源资产清查数据成果";
                    string countyFolderPath = System.IO.Path.Combine(outputGDBPath, countyFolderName);
                    string summaryTablePath = System.IO.Path.Combine(countyFolderPath, "汇总表格");
                    string forestTablePath = System.IO.Path.Combine(summaryTablePath, "森林");

                    // 检查目录是否存在
                    if (!Directory.Exists(forestTablePath))
                    {
                        MessageBox.Show($"{countyName}的目录结构不完整，找不到路径：\n{forestTablePath}\n\n请先使用生成成果结构功能创建正确的目录。", "目录错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    bool countySuccess = true;

                    // 生成第一个表格：全民所有森林资源资产清查实物量汇总表
                    try
                    {
                        string tableA2Name = $"（{countyCode}）全民所有森林资源资产清查实物量汇总表.xls";
                        string tableA2Path = System.IO.Path.Combine(forestTablePath, tableA2Name);
                        CreateTableA2(tableA2Path);
                        totalTables++;
                        System.Diagnostics.Debug.WriteLine($"成功为{countyName}创建A2表格: {tableA2Path}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"为{countyName}创建实物量汇总表时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        countySuccess = false;
                    }

                    // 生成第二个表格：全民所有森林资源资产清查价值量汇总表
                    try
                    {
                        string tableA4Name = $"（{countyCode}）全民所有森林资源资产清查价值量汇总表.xls";
                        string tableA4Path = System.IO.Path.Combine(forestTablePath, tableA4Name);
                        CreateTableA4(tableA4Path);
                        totalTables++;
                        System.Diagnostics.Debug.WriteLine($"成功为{countyName}创建A4表格: {tableA4Path}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"为{countyName}创建价值量汇总表时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        countySuccess = false;
                    }

                    // 生成第三个表格：全民所有森林资源资产清查林地汇总表
                    try
                    {
                        string tableA6Name = $"（{countyCode}）全民所有森林资源资产清查林地汇总表.xls";
                        string tableA6Path = System.IO.Path.Combine(forestTablePath, tableA6Name);
                        CreateTableA6(tableA6Path);
                        totalTables++;
                        System.Diagnostics.Debug.WriteLine($"成功为{countyName}创建A6表格: {tableA6Path}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"为{countyName}创建林地汇总表时出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        countySuccess = false;
                    }

                    if (countySuccess)
                    {
                        successCount++;
                    }
                }

                if (successCount > 0)
                {
                    MessageBox.Show($"已成功为 {successCount} 个县生成了 {totalTables} 个Excel报表。\n\n报表保存在各县的汇总表格\\森林目录下。", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("未能成功生成任何Excel报表。请检查错误信息和目录结构。", "操作完成", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成Excel报表时发生错误：{ex.Message}", "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"生成Excel报表时出错: {ex}");
            }
        }

        /// <summary>
        /// 创建A2表格：全民所有森林资源资产清查实物量汇总表
        /// </summary>
        /// <param name="excelFilePath">Excel文件保存路径</param>
        private void CreateTableA2(string excelFilePath)
        {
            // 创建工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("A2");

            //设置对齐
            NPOI.SS.UserModel.ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            // 插入标题行
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 1));
            NPOI.HSSF.UserModel.HSSFRow row0 = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(0);
            row0.CreateCell(0).SetCellValue("行政区");
            row0.GetCell(0).CellStyle = style;

            NPOI.HSSF.UserModel.HSSFRow row1 = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(1);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 0, 0));
            row1.CreateCell(0).SetCellValue("名称");
            row1.GetCell(0).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 2, 1, 1));
            row1.CreateCell(1).SetCellValue("代码");
            row1.GetCell(1).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 2, 2));
            row0.CreateCell(2).SetCellValue("国土变更调查权属");
            row0.GetCell(2).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 3, 3));
            row0.CreateCell(3).SetCellValue("林木所有权");
            row0.GetCell(3).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 4, 4));
            row0.CreateCell(4).SetCellValue("林种");
            row0.GetCell(4).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 5, 5));
            row0.CreateCell(5).SetCellValue("起源");
            row0.GetCell(5).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 6, 6));
            row0.CreateCell(6).SetCellValue("面积合计");
            row0.GetCell(6).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 7, 18));
            row0.CreateCell(7).SetCellValue("乔木林地");
            row0.GetCell(7).CellStyle = style;

            NPOI.HSSF.UserModel.HSSFRow row2 = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(2);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 7, 8));
            row1.CreateCell(7).SetCellValue("小计");
            row1.GetCell(7).CellStyle = style;
            row2.CreateCell(7).SetCellValue("面积");
            row2.GetCell(7).CellStyle = style;
            row2.CreateCell(8).SetCellValue("蓄积");
            row2.GetCell(8).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 9, 10));
            row1.CreateCell(9).SetCellValue("幼龄林");
            row1.GetCell(9).CellStyle = style;
            row2.CreateCell(9).SetCellValue("面积");
            row2.GetCell(9).CellStyle = style;
            row2.CreateCell(10).SetCellValue("蓄积");
            row2.GetCell(10).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 11, 12));
            row1.CreateCell(11).SetCellValue("中龄林");
            row1.GetCell(11).CellStyle = style;
            row2.CreateCell(11).SetCellValue("面积");
            row2.GetCell(11).CellStyle = style;
            row2.CreateCell(12).SetCellValue("蓄积");
            row2.GetCell(12).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 13, 14));
            row1.CreateCell(13).SetCellValue("近熟林");
            row1.GetCell(13).CellStyle = style;
            row2.CreateCell(13).SetCellValue("面积");
            row2.GetCell(13).CellStyle = style;
            row2.CreateCell(14).SetCellValue("蓄积");
            row2.GetCell(14).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 15, 16));
            row1.CreateCell(15).SetCellValue("成熟林");
            row1.GetCell(15).CellStyle = style;
            row2.CreateCell(15).SetCellValue("面积");
            row2.GetCell(15).CellStyle = style;
            row2.CreateCell(16).SetCellValue("蓄积");
            row2.GetCell(16).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 17, 18));
            row1.CreateCell(17).SetCellValue("过熟林");
            row1.GetCell(17).CellStyle = style;
            row2.CreateCell(17).SetCellValue("面积");
            row2.GetCell(17).CellStyle = style;
            row2.CreateCell(18).SetCellValue("蓄积");
            row2.GetCell(18).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 19, 20));
            row0.CreateCell(19).SetCellValue("竹林地");
            row0.GetCell(19).CellStyle = style;
            row2.CreateCell(19).SetCellValue("面积");
            row2.GetCell(19).CellStyle = style;
            row2.CreateCell(20).SetCellValue("株数");
            row2.GetCell(20).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 21, 21));
            row0.CreateCell(21).SetCellValue("灌木林地");
            row0.GetCell(21).CellStyle = style;
            row2.CreateCell(21).SetCellValue("面积");
            row2.GetCell(21).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 22, 22));
            row0.CreateCell(22).SetCellValue("其他林地");
            row0.GetCell(22).CellStyle = style;
            row2.CreateCell(22).SetCellValue("面积");
            row2.GetCell(22).CellStyle = style;

            //设置列宽
            sheet.SetColumnWidth(0, System.Text.Encoding.Default.GetBytes(row1.GetCell(0).StringCellValue).Length * 256 + 500);
            sheet.SetColumnWidth(1, System.Text.Encoding.Default.GetBytes(row1.GetCell(1).StringCellValue).Length * 256 + 500);

            for (int i = 2; i <= 6; i++)
            {
                sheet.SetColumnWidth(i, System.Text.Encoding.Default.GetBytes(row0.GetCell(i).StringCellValue).Length * 256 + 500);
            }

            for (int i = 7; i <= 20; i++)
            {
                sheet.SetColumnWidth(i, System.Text.Encoding.Default.GetBytes(row2.GetCell(i).StringCellValue).Length * 256 + 500);
            }

            sheet.SetColumnWidth(21, System.Text.Encoding.Default.GetBytes(row0.GetCell(21).StringCellValue).Length * 256 + 500);
            sheet.SetColumnWidth(22, System.Text.Encoding.Default.GetBytes(row0.GetCell(22).StringCellValue).Length * 256 + 500);

            // 保存文件
            using (System.IO.FileStream fs = new System.IO.FileStream(excelFilePath, FileMode.Create))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// 创建A4表格：全民所有森林资源资产清查价值量汇总表
        /// </summary>
        /// <param name="excelFilePath">Excel文件保存路径</param>
        private void CreateTableA4(string excelFilePath)
        {
            // 创建工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("A4");

            NPOI.SS.UserModel.ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            // 插入标题行
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 1));
            NPOI.HSSF.UserModel.HSSFRow row0 = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(0);
            row0.CreateCell(0).SetCellValue("行政区");
            row0.GetCell(0).CellStyle = style;

            NPOI.HSSF.UserModel.HSSFRow row1 = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue("名称");
            row1.GetCell(0).CellStyle = style;
            row1.CreateCell(1).SetCellValue("代码");
            row1.GetCell(1).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 2, 2));
            row0.CreateCell(2).SetCellValue("国土变更调查权属");
            row0.GetCell(2).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 3, 3));
            row0.CreateCell(3).SetCellValue("林木所有权");
            row0.GetCell(3).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 4, 4));
            row0.CreateCell(4).SetCellValue("地类");
            row0.GetCell(4).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 5, 5));
            row0.CreateCell(5).SetCellValue("面积");
            row0.GetCell(5).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 6, 6));
            row0.CreateCell(6).SetCellValue("划入城镇开发边界面积");
            row0.GetCell(6).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 7, 7));
            row0.CreateCell(7).SetCellValue("经济价值");
            row0.GetCell(7).CellStyle = style;

            //设置列宽
            sheet.SetColumnWidth(0, System.Text.Encoding.Default.GetBytes(row1.GetCell(0).StringCellValue).Length * 256 + 500);
            sheet.SetColumnWidth(1, System.Text.Encoding.Default.GetBytes(row1.GetCell(1).StringCellValue).Length * 256 + 500);
            for (int i = 2; i <= 7; i++)
            {
                sheet.SetColumnWidth(i, System.Text.Encoding.Default.GetBytes(row0.GetCell(i).StringCellValue).Length * 256 + 500);
            }

            // 保存文件
            using (System.IO.FileStream fs = new System.IO.FileStream(excelFilePath, FileMode.Create))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// 创建A6表格：全民所有森林资源资产清查林地汇总表
        /// </summary>
        /// <param name="excelFilePath">Excel文件保存路径</param>
        private void CreateTableA6(string excelFilePath)
        {
            // 创建工作簿
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("A6");
            NPOI.SS.UserModel.ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            // 插入标题行
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 1));
            NPOI.HSSF.UserModel.HSSFRow row0 = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(0);
            row0.CreateCell(0).SetCellValue("行政区");
            row0.GetCell(0).CellStyle = style;

            NPOI.HSSF.UserModel.HSSFRow row1 = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue("名称");
            row1.GetCell(0).CellStyle = style;
            row1.CreateCell(1).SetCellValue("代码");
            row1.GetCell(1).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 2, 2));
            row0.CreateCell(2).SetCellValue("地类");
            row0.GetCell(2).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 3, 3));
            row0.CreateCell(3).SetCellValue("林地等");
            row0.GetCell(3).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 4, 4));
            row0.CreateCell(4).SetCellValue("面积合计");
            row0.GetCell(4).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 5, 5));
            row0.CreateCell(5).SetCellValue("划入城镇开发边界面积");
            row0.GetCell(5).CellStyle = style;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 1, 6, 6));
            row0.CreateCell(6).SetCellValue("经济价值");
            row0.GetCell(6).CellStyle = style;

            // 自动调整列宽
            //for (int i = 0; i <= 6; i++)
            //{
            //    sheet.AutoSizeColumn(i);
            //}

            // 保存文件
            using (System.IO.FileStream fs = new System.IO.FileStream(excelFilePath, FileMode.Create))
            {
                workbook.Write(fs);
            }
        }

        // 在 buttonResultStructure_Click 方法中更新县代码使用逻辑
        private void buttonResultStructure_Click(object sender, EventArgs e)
        {
            // 1. 提示用户选择或输入成果的根文件夹名称和位置
            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "选择或输入成果文件夹名称";
                // 这是一个常用技巧，让SaveFileDialog表现得像一个文件夹选择器
                dialog.Filter = "文件夹|*.";
                dialog.FileName = "全民所有资源资产清查数据成果"; // 设置默认文件夹名称

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取用户选择的完整路径，这将作为成果的根目录
                    string rootPath = dialog.FileName;

                    try
                    {
                        // 使用新的 SourceDataFileInfo 类型
                        var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetSourceDataFiles();
                        var czkfbjFiles = ForestResourcePlugin.SharedDataManager.GetCZKFBJFiles();

                        var countyNames = new HashSet<string>();
                        foreach (var file in lcxzgxFiles)
                        {
                            if (!string.IsNullOrEmpty(file.DisplayName)) countyNames.Add(file.DisplayName);
                        }
                        foreach (var file in czkfbjFiles)
                        {
                            if (!string.IsNullOrEmpty(file.DisplayName)) countyNames.Add(file.DisplayName);
                        }

                        if (countyNames.Count == 0)
                        {
                            MessageBox.Show("未能获取县列表，请先在数据源步骤中选择数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // 3. 创建根目录
                        Directory.CreateDirectory(rootPath);
                        int createdCount = 0;

                        // 4. 遍历每个县，创建所需的多级目录结构
                        foreach (string countyName in countyNames)
                        {
                            // 修改：使用县代码映射器获取真实的县代码
                            string countyCode = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyCode(countyName);
                            string countyFolderName = $"{countyName}（{countyCode}）全民所有自然资源资产清查数据成果";
                            string countyFolderPath = System.IO.Path.Combine(rootPath, countyFolderName);

                            // 创建第三级目录
                            string dataSetPath = System.IO.Path.Combine(countyFolderPath, "清查数据集");
                            string summaryTablePath = System.IO.Path.Combine(countyFolderPath, "汇总表格");

                            // 创建第四级目录
                            string[] subFolders = { "森林", "草原", "湿地" };
                            foreach (var subFolder in subFolders)
                            {
                                // 清查数据集下的子目录
                                string resourcePath = System.IO.Path.Combine(dataSetPath, subFolder);
                                // 汇总表格下的子目录
                                string summaryPath = System.IO.Path.Combine(summaryTablePath, subFolder);

                                // 创建第五级目录 (空间数据)
                                string spatialDataPath = System.IO.Path.Combine(resourcePath, "空间数据");
                                Directory.CreateDirectory(spatialDataPath);

                                // 创建汇总表格下的第四级目录
                                Directory.CreateDirectory(summaryPath);
                            }

                            createdCount++;
                            System.Diagnostics.Debug.WriteLine($"为{countyName}({countyCode})创建目录结构完成");
                        }

                        // 5. 操作完成后向用户报告结果
                        MessageBox.Show($"成功为 {createdCount} 个县创建了成果目录结构。\n\n根目录路径：{rootPath}", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 6. （可选）在文件资源管理器中打开创建的根目录
                        //System.Diagnostics.Process.Start("explorer.exe", rootPath);

                        txtOutputGDBPath.Text = rootPath;
                        outputGDBPath = rootPath; // 更新全局变量
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"创建目录结构时发生错误：\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Diagnostics.Debug.WriteLine($"创建目录结构失败: {ex}");
                    }
                }
            }
        }

        private void topPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    
}