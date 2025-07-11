﻿using System;
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

                        // 创建三个空的Shapefile，并传入空间参考
                        if (!CreateEmptyShapefile("LCXZGX", workspace, spatialReference))
                        {
                            MessageBox.Show("创建LCXZGX Shapefile失败");
                            return false;
                        }
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

                        System.Diagnostics.Debug.WriteLine($"成功为{countryName}创建三个空的Shapefile");
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

                // 🔥 修改: 使用传入的空间参考
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
                    MessageBox.Show($"创建{shapefileName} Shapefile失败");
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
            if (string.IsNullOrEmpty(dataSourcePath))
            {
                MessageBox.Show("数据源路径为空，请先通过浏览按钮选择。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(outputGDBPath))
            {
                MessageBox.Show("输出结果路径为空，请先通过浏览按钮选择。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetLCXZGXFiles();
                if (lcxzgxFiles == null || lcxzgxFiles.Count == 0)
                {
                    MessageBox.Show("未能从共享数据中找到林草湿荒普查数据，无法确定源坐标系。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 按县名对数据源进行分组
                var countyGroups = lcxzgxFiles.GroupBy(f => f.DisplayName);

                foreach (var group in countyGroups)
                {
                    string countyName = group.Key;
                    var firstFileInGroup = group.First(); // 获取该县的第一个数据源文件

                    // 从源文件获取空间参考
                    ISpatialReference sourceSpatialRef = GetSpatialReferenceFromFile(firstFileInGroup);
                    if (sourceSpatialRef == null)
                    {
                        var userChoice = MessageBox.Show($"无法自动读取“{countyName}”的源数据坐标系。\n\n是否继续并使用默认的CGCS2000坐标系？", "坐标系读取失败", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (userChoice == DialogResult.No)
                        {
                            MessageBox.Show("操作已取消。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        // 如果无法获取，则使用默认的CGCS2000作为备用
                        sourceSpatialRef = CreateCGCS2000SpatialReference();
                    }

                    // 创建表时传入获取到的空间参考
                    if (!CreateTable4Country(outputGDBPath, countyName, sourceSpatialRef))
                    {
                        MessageBox.Show($"创建“{countyName}”的数据库及表结构失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // 如果一个县失败，则停止后续操作
                    }
                }

                MessageBox.Show("已为所有县建立成果数据库及表结构。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"创建Shapefile时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"创建Shapefile时出错: {ex}");
            }
        }

        /// <summary>
        /// 从单个数据源文件获取空间参考（重构版）
        /// </summary>
        /// <param name="fileInfo">数据源文件信息</param>
        /// <returns>空间参考对象，失败则返回null</returns>
        private ISpatialReference GetSpatialReferenceFromFile(ForestResourcePlugin.LCXZGXFileInfo fileInfo)
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

        private void btnResultExcel_Click(object sender, EventArgs e)
        {
            // 1. 验证输出路径是否已选择
            if (string.IsNullOrEmpty(outputGDBPath))
            {
                MessageBox.Show("请先选择输出结果路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. 从共享管理器收集数据
                var lcxzgxFiles = ForestResourcePlugin.SharedDataManager.GetLCXZGXFiles();
                var czkfbjFiles = ForestResourcePlugin.SharedDataManager.GetCZKFBJFiles();

                if (lcxzgxFiles.Count == 0 && czkfbjFiles.Count == 0)
                {
                    MessageBox.Show("没有找到任何可报告的数据源文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 3. 将数据整理为报告格式
                var reportData = new List<DataSourceReportItem>();
                foreach (var file in lcxzgxFiles)
                {
                    reportData.Add(new DataSourceReportItem
                    {
                        CountyName = file.DisplayName,
                        DataType = "林草现状",
                        Format = file.IsGdb ? "GDB要素类" : "Shapefile",
                        Path = file.FullPath
                    });
                }
                foreach (var file in czkfbjFiles)
                {
                    reportData.Add(new DataSourceReportItem
                    {
                        CountyName = file.DisplayName,
                        DataType = "城镇开发边界",
                        Format = file.IsGdb ? "GDB要素类" : "Shapefile",
                        Path = file.FullPath
                    });
                }

                // 按县名和数据类型排序，使报告更清晰
                reportData = reportData.OrderBy(r => r.CountyName).ThenBy(r => r.DataType).ToList();

                // 4. 定义Excel表头
                var headers = new Dictionary<string, string>
                {
                    { "CountyName", "县名" },
                    { "DataType", "数据类型" },
                    { "Format", "数据格式" },
                    { "Path", "文件路径" }
                };

                // 5. 定义输出文件路径
                string excelFilePath = System.IO.Path.Combine(outputGDBPath, "数据源准备情况报告.xlsx");

                // 6. 调用导出工具
                ForestResourcePlugin.Utils.ExcelExporter.ExportToExcel(reportData, headers, excelFilePath);

                // 7. 提示用户成功
                MessageBox.Show($"数据源准备情况报告已成功导出到：\n\n{excelFilePath}", "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出Excel报告时发生错误：\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"导出Excel报告失败: {ex}");
            }
        }
    }
}