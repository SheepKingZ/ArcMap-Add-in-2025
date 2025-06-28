using System;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using System.IO;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 管理地理数据库(GDB)创建和操作的工具类
    /// </summary>
    public static class GDBManager
    {
        /// <summary>
        /// 创建文件地理数据库(File Geodatabase)
        /// </summary>
        /// <param name="path">要创建的GDB路径(包含.gdb后缀)</param>
        /// <returns>成功创建返回true，否则返回false</returns>
        public static bool CreateFileGDB(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                System.Diagnostics.Debug.WriteLine("错误: GDB路径不能为空");
                return false;
            }

            // 确保路径以.gdb结尾
            if (!path.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase))
            {
                path = path + ".gdb";
                System.Diagnostics.Debug.WriteLine($"已修正路径，添加.gdb后缀: {path}");
            }

            // 检查GDB是否已存在
            if (Directory.Exists(path))
            {
                System.Diagnostics.Debug.WriteLine($"GDB已存在: {path}");
                return true;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"开始创建File GDB: {path}");
                
                // 创建父目录（如果需要）
                string parentDir = System.IO.Path.GetDirectoryName(path);
                if (!Directory.Exists(parentDir))
                {
                    Directory.CreateDirectory(parentDir);
                    System.Diagnostics.Debug.WriteLine($"已创建父目录: {parentDir}");
                }

                // 获取FileGDB工作空间工厂类型
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                if (factoryType == null)
                {
                    System.Diagnostics.Debug.WriteLine("错误: 无法获取FileGDBWorkspaceFactory的类型");
                    return false;
                }
                
                // 创建工作空间工厂实例
                IWorkspaceFactory workspaceFactory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;
                if (workspaceFactory == null)
                {
                    System.Diagnostics.Debug.WriteLine("错误: 无法创建FileGDBWorkspaceFactory实例");
                    return false;
                }

                // 创建工作空间
                IWorkspaceName workspaceName = workspaceFactory.Create(
                    parentDir, System.IO.Path.GetFileName(path), null, 0);

                if (workspaceName != null)
                {
                    System.Diagnostics.Debug.WriteLine($"成功创建File GDB: {path}");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("创建GDB失败: 工作空间名称为空");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建File GDB时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                return false;
            }
        }

        /// <summary>
        /// 检查GDB是否存在，如果不存在则创建
        /// </summary>
        /// <param name="gdbPath">GDB路径</param>
        /// <returns>GDB路径存在或创建成功返回true，否则返回false</returns>
        public static bool EnsureGDBExists(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            // 确保路径以.gdb结尾
            if (!path.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase))
            {
                path = path + ".gdb";
            }

            // 检查GDB是否已存在
            if (Directory.Exists(path))
            {
                return true;
            }

            // 创建GDB
            return CreateFileGDB(path);
        }
        
        /// <summary>
        /// 在GDB中创建要素类
        /// </summary>
        /// <param name="gdbPath">GDB路径</param>
        /// <param name="featureClassName">要素类名称</param>
        /// <param name="geometryType">几何类型</param>
        /// <param name="spatialReference">空间参考</param>
        /// <returns>创建的要素类，失败则返回null</returns>
        public static IFeatureClass CreateFeatureClass(string gdbPath, string featureClassName, 
            esriGeometryType geometryType, ISpatialReference spatialReference = null)
        {
            if (string.IsNullOrEmpty(gdbPath) || string.IsNullOrEmpty(featureClassName))
            {
                System.Diagnostics.Debug.WriteLine("错误: GDB路径或要素类名称不能为空");
                return null;
            }

            // 确保GDB存在
            if (!EnsureGDBExists(gdbPath))
            {
                System.Diagnostics.Debug.WriteLine($"错误: 无法确保GDB存在: {gdbPath}");
                return null;
            }

            IWorkspace workspace = null;
            try
            {
                // 打开GDB工作空间
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;
                workspace = workspaceFactory.OpenFromFile(gdbPath, 0);

                if (workspace == null)
                {
                    System.Diagnostics.Debug.WriteLine($"错误: 无法打开GDB工作空间: {gdbPath}");
                    return null;
                }

                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                if (featureWorkspace == null)
                {
                    System.Diagnostics.Debug.WriteLine("错误: 无法获取要素工作空间接口");
                    return null;
                }

                // 检查要素类是否已存在
                bool featureClassExists = false;
                IEnumDatasetName enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
                enumDatasetName.Reset();
                IDatasetName datasetName;
                while ((datasetName = enumDatasetName.Next()) != null)
                {
                    if (datasetName.Name.Equals(featureClassName, StringComparison.OrdinalIgnoreCase))
                    {
                        featureClassExists = true;
                        break;
                    }
                }

                // 如果要素类已存在，则打开它
                if (featureClassExists)
                {
                    System.Diagnostics.Debug.WriteLine($"要素类已存在: {featureClassName}, 将直接打开它");
                    return featureWorkspace.OpenFeatureClass(featureClassName);
                }

                // 创建字段集合
                IFields fields = new FieldsClass();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                // 添加OID字段
                IField oidField = new FieldClass();
                IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
                oidFieldEdit.Name_2 = "OBJECTID";
                oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldsEdit.AddField(oidField);

                // 添加几何字段
                IField shapeField = new FieldClass();
                IFieldEdit shapeFieldEdit = (IFieldEdit)shapeField;
                shapeFieldEdit.Name_2 = "SHAPE";
                shapeFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

                // 设置几何定义
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.GeometryType_2 = geometryType;
                geometryDefEdit.HasZ_2 = false;
                geometryDefEdit.HasM_2 = false;

                // 设置空间参考
                if (spatialReference != null)
                {
                    geometryDefEdit.SpatialReference_2 = spatialReference;
                }

                shapeFieldEdit.GeometryDef_2 = geometryDef;
                fieldsEdit.AddField(shapeField);

                // 创建要素类
                IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(
                    featureClassName, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

                System.Diagnostics.Debug.WriteLine($"成功创建要素类: {featureClassName}");
                return featureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建要素类时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                return null;
            }
        }
    }
}