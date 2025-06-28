using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesOleDB;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 地理数据库工具类，用于处理各种类型的地理数据库操作
    /// </summary>
    public static class GeodatabaseUtilities
    {
        /// <summary>
        /// 地理数据库类型枚举
        /// </summary>
        public enum GeodatabaseType
        {
            FileGeodatabase,    // File GDB (.gdb)
            PersonalGeodatabase, // Personal GDB (.mdb)
            SDEGeodatabase,     // SDE数据库
            Unknown
        }

        /// <summary>
        /// 要素类信息类
        /// </summary>
        public class FeatureClassInfo
        {
            public string Name { get; set; }
            public string AliasName { get; set; }
            public esriGeometryType GeometryType { get; set; }
            public string GeometryTypeName { get; set; }
            public IFeatureClass FeatureClass { get; set; }
            public int FeatureCount { get; set; }

            public override string ToString()
            {
                return $"{Name} ({GeometryTypeName}, {FeatureCount} 要素)";
            }
        }

        /// <summary>
        /// 工作空间信息类
        /// </summary>
        public class WorkspaceInfo
        {
            public string Path { get; set; }
            public GeodatabaseType Type { get; set; }
            public IWorkspace Workspace { get; set; }
            public bool IsConnected { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// 检测地理数据库类型
        /// </summary>
        /// <param name="path">数据库路径</param>
        /// <returns>数据库类型</returns>
        public static GeodatabaseType DetectGeodatabaseType(string path)
        {
            if (string.IsNullOrEmpty(path))
                return GeodatabaseType.Unknown;

            try
            {
                // File Geodatabase (.gdb folder)
                if (Directory.Exists(path) && path.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase))
                {
                    return GeodatabaseType.FileGeodatabase;
                }

                // Personal Geodatabase (.mdb file)
                if (File.Exists(path) && path.EndsWith(".mdb", StringComparison.OrdinalIgnoreCase))
                {
                    return GeodatabaseType.PersonalGeodatabase;
                }

                // SDE connection (could be various formats)
                if (path.Contains("@") || path.ToLower().Contains("server=") || path.EndsWith(".sde", StringComparison.OrdinalIgnoreCase))
                {
                    return GeodatabaseType.SDEGeodatabase;
                }

                return GeodatabaseType.Unknown;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"检测地理数据库类型时出错: {ex.Message}");
                return GeodatabaseType.Unknown;
            }
        }

        /// <summary>
        /// 打开地理数据库工作空间
        /// </summary>
        /// <param name="path">数据库路径</param>
        /// <returns>工作空间信息</returns>
        public static WorkspaceInfo OpenGeodatabase(string path)
        {
            var workspaceInfo = new WorkspaceInfo
            {
                Path = path,
                Type = DetectGeodatabaseType(path),
                IsConnected = false
            };

            try
            {
                IWorkspaceFactory workspaceFactory = null;
                
                switch (workspaceInfo.Type)
                {
                    case GeodatabaseType.FileGeodatabase:
                        //workspaceFactory = new FileGDBWorkspaceFactoryClass();
                        break;
                    
                    case GeodatabaseType.PersonalGeodatabase:
                        //workspaceFactory = new AccessWorkspaceFactoryClass();
                        break;
                    
                    case GeodatabaseType.SDEGeodatabase:
                        //workspaceFactory = new SdeWorkspaceFactoryClass();
                        break;
                    
                    default:
                        workspaceInfo.ErrorMessage = "不支持的地理数据库类型";
                        return workspaceInfo;
                }

                // 验证路径是否存在
                if (workspaceInfo.Type == GeodatabaseType.FileGeodatabase && !Directory.Exists(path))
                {
                    workspaceInfo.ErrorMessage = "文件地理数据库路径不存在";
                    return workspaceInfo;
                }
                else if (workspaceInfo.Type == GeodatabaseType.PersonalGeodatabase && !File.Exists(path))
                {
                    workspaceInfo.ErrorMessage = "个人地理数据库文件不存在";
                    return workspaceInfo;
                }

                // 打开工作空间
                workspaceInfo.Workspace = workspaceFactory.OpenFromFile(path, 0);
                
                if (workspaceInfo.Workspace != null)
                {
                    workspaceInfo.IsConnected = true;
                    System.Diagnostics.Debug.WriteLine($"成功打开地理数据库: {path} (类型: {workspaceInfo.Type})");
                }
                else
                {
                    workspaceInfo.ErrorMessage = "无法打开地理数据库";
                }
            }
            catch (Exception ex)
            {
                workspaceInfo.ErrorMessage = $"打开地理数据库时出错: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"打开地理数据库失败: {path}, 错误: {ex.Message}");
            }

            return workspaceInfo;
        }

        /// <summary>
        /// 获取地理数据库中的所有要素类
        /// </summary>
        /// <param name="workspaceInfo">工作空间信息</param>
        /// <returns>要素类信息列表</returns>
        public static List<FeatureClassInfo> GetFeatureClasses(WorkspaceInfo workspaceInfo)
        {
            var featureClasses = new List<FeatureClassInfo>();

            if (!workspaceInfo.IsConnected || workspaceInfo.Workspace == null)
            {
                return featureClasses;
            }

            try
            {
                IFeatureWorkspace featureWorkspace = workspaceInfo.Workspace as IFeatureWorkspace;
                if (featureWorkspace == null)
                {
                    System.Diagnostics.Debug.WriteLine("工作空间不支持要素类操作");
                    return featureClasses;
                }

                // 获取要素类枚举器
                IEnumDataset enumDataset = workspaceInfo.Workspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                IDataset dataset = enumDataset.Next();

                while (dataset != null)
                {
                    try
                    {
                        IFeatureClass featureClass = dataset as IFeatureClass;
                        if (featureClass != null)
                        {
                            var featureClassInfo = new FeatureClassInfo
                            {
                                Name = dataset.Name,
                                AliasName = featureClass.AliasName,
                                GeometryType = featureClass.ShapeType,
                                GeometryTypeName = GetGeometryTypeName(featureClass.ShapeType),
                                FeatureClass = featureClass,
                                FeatureCount = featureClass.FeatureCount(null)
                            };

                            featureClasses.Add(featureClassInfo);
                            System.Diagnostics.Debug.WriteLine($"发现要素类: {featureClassInfo.Name} ({featureClassInfo.GeometryTypeName}, {featureClassInfo.FeatureCount} 要素)");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"处理要素类 {dataset.Name} 时出错: {ex.Message}");
                    }

                    dataset = enumDataset.Next();
                }

                System.Diagnostics.Debug.WriteLine($"共发现 {featureClasses.Count} 个要素类");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取要素类列表时出错: {ex.Message}");
            }

            return featureClasses;
        }

        /// <summary>
        /// 获取多边形要素类（用于林草现状和城镇开发边界）
        /// </summary>
        /// <param name="workspaceInfo">工作空间信息</param>
        /// <returns>多边形要素类列表</returns>
        public static List<FeatureClassInfo> GetPolygonFeatureClasses(WorkspaceInfo workspaceInfo)
        {
            var allFeatureClasses = GetFeatureClasses(workspaceInfo);
            var polygonFeatureClasses = new List<FeatureClassInfo>();

            foreach (var fc in allFeatureClasses)
            {
                if (fc.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    polygonFeatureClasses.Add(fc);
                }
            }

            System.Diagnostics.Debug.WriteLine($"找到 {polygonFeatureClasses.Count} 个多边形要素类");
            return polygonFeatureClasses;
        }

        /// <summary>
        /// 根据名称获取要素类
        /// </summary>
        /// <param name="workspaceInfo">工作空间信息</param>
        /// <param name="featureClassName">要素类名称</param>
        /// <returns>要素类对象</returns>
        public static IFeatureClass GetFeatureClass(WorkspaceInfo workspaceInfo, string featureClassName)
        {
            if (!workspaceInfo.IsConnected || workspaceInfo.Workspace == null || string.IsNullOrEmpty(featureClassName))
            {
                return null;
            }

            try
            {
                IFeatureWorkspace featureWorkspace = workspaceInfo.Workspace as IFeatureWorkspace;
                if (featureWorkspace == null)
                {
                    return null;
                }

                return featureWorkspace.OpenFeatureClass(featureClassName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取要素类 {featureClassName} 时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 验证要素类连接状态
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <returns>是否连接正常</returns>
        public static bool ValidateFeatureClass(IFeatureClass featureClass)
        {
            try
            {
                if (featureClass == null)
                    return false;

                // 尝试访问基本属性
                var name = featureClass.AliasName;
                var fieldCount = featureClass.Fields.FieldCount;
                var hasGeometry = featureClass.ShapeType != esriGeometryType.esriGeometryNull;

                System.Diagnostics.Debug.WriteLine($"要素类验证成功: {name}, 字段数: {fieldCount}, 几何类型: {featureClass.ShapeType}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"要素类验证失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取要素类的字段列表
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <returns>字段名称列表</returns>
        public static List<string> GetFeatureClassFields(IFeatureClass featureClass)
        {
            var fieldNames = new List<string>();

            if (featureClass == null)
                return fieldNames;

            try
            {
                IFields fields = featureClass.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    
                    // 只添加适合的字段类型
                    if (field.Type == esriFieldType.esriFieldTypeString ||
                        field.Type == esriFieldType.esriFieldTypeSmallInteger ||
                        field.Type == esriFieldType.esriFieldTypeInteger ||
                        field.Type == esriFieldType.esriFieldTypeSingle ||
                        field.Type == esriFieldType.esriFieldTypeDouble)
                    {
                        fieldNames.Add(field.Name);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"要素类 {featureClass.AliasName} 有 {fieldNames.Count} 个有效字段");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取要素类字段时出错: {ex.Message}");
            }

            return fieldNames;
        }

        /// <summary>
        /// 智能匹配要素类名称
        /// </summary>
        /// <param name="featureClasses">要素类列表</param>
        /// <param name="namePatterns">匹配模式</param>
        /// <returns>匹配的要素类</returns>
        public static FeatureClassInfo FindBestMatchFeatureClass(List<FeatureClassInfo> featureClasses, string[] namePatterns)
        {
            foreach (string pattern in namePatterns)
            {
                foreach (var fc in featureClasses)
                {
                    if (fc.Name.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        fc.AliasName.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"智能匹配成功: {fc.Name} 匹配模式 {pattern}");
                        return fc;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取几何类型名称
        /// </summary>
        /// <param name="geometryType">几何类型</param>
        /// <returns>类型名称</returns>
        private static string GetGeometryTypeName(esriGeometryType geometryType)
        {
            switch (geometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    return "点";
                case esriGeometryType.esriGeometryPolyline:
                    return "线";
                case esriGeometryType.esriGeometryPolygon:
                    return "面";
                case esriGeometryType.esriGeometryMultipoint:
                    return "多点";
                default:
                    return "其他";
            }
        }

        /// <summary>
        /// 创建地理数据库连接字符串
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="instance">实例</param>
        /// <param name="database">数据库</param>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>连接字符串</returns>
        public static string CreateSdeConnectionString(string server, string instance, string database, string user, string password)
        {
            return $"SERVER={server};INSTANCE={instance};DATABASE={database};USER={user};PASSWORD={password};VERSION=sde.DEFAULT";
        }

        /// <summary>
        /// 释放工作空间资源
        /// </summary>
        /// <param name="workspaceInfo">工作空间信息</param>
        public static void ReleaseWorkspace(WorkspaceInfo workspaceInfo)
        {
            try
            {
                if (workspaceInfo?.Workspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspaceInfo.Workspace);
                    workspaceInfo.Workspace = null;
                    workspaceInfo.IsConnected = false;
                    System.Diagnostics.Debug.WriteLine("工作空间资源已释放");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"释放工作空间资源时出错: {ex.Message}");
            }
        }
    }
}