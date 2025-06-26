using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 地图图层工具类，用于获取ArcMap中当前加载的图层
    /// </summary>
    public static class MapLayerUtilities
    {
        /// <summary>
        /// 获取当前ArcMap文档中的所有图层
        /// </summary>
        /// <returns>图层信息列表，包含图层名称和图层对象</returns>
        public static List<LayerInfo> GetMapLayers()
        {
            List<LayerInfo> layers = new List<LayerInfo>();
            
            try
            {
                System.Diagnostics.Debug.WriteLine("开始获取地图图层...");
                
                // 获取ArcMap应用程序
                IApplication application = GetArcMapApplication();
                if (application == null)
                {
                    System.Diagnostics.Debug.WriteLine("无法获取ArcMap应用程序实例");
                    return layers;
                }

                System.Diagnostics.Debug.WriteLine("成功获取ArcMap应用程序实例");

                // 获取当前地图文档
                IMxDocument mxDocument = application.Document as IMxDocument;
                if (mxDocument == null)
                {
                    System.Diagnostics.Debug.WriteLine("无法获取MxDocument");
                    return layers;
                }

                System.Diagnostics.Debug.WriteLine("成功获取MxDocument");

                // 获取地图
                IMap map = mxDocument.FocusMap;
                if (map == null)
                {
                    System.Diagnostics.Debug.WriteLine("无法获取FocusMap");
                    return layers;
                }

                System.Diagnostics.Debug.WriteLine($"成功获取地图，图层数量: {map.LayerCount}");

                // 遍历所有图层
                for (int i = 0; i < map.LayerCount; i++)
                {
                    ILayer layer = map.get_Layer(i);
                    System.Diagnostics.Debug.WriteLine($"处理图层 {i}: {layer.Name}");
                    
                    // 如果是图层组，递归获取其中的图层
                    if (layer is IGroupLayer)
                    {
                        System.Diagnostics.Debug.WriteLine($"图层组: {layer.Name}");
                        GetLayersFromGroup(layer as IGroupLayer, layers);
                    }
                    else if (layer is IFeatureLayer)
                    {
                        // 添加要素图层
                        IFeatureLayer featureLayer = layer as IFeatureLayer;
                        if (featureLayer.FeatureClass != null)
                        {
                            LayerInfo layerInfo = new LayerInfo 
                            { 
                                Name = layer.Name, 
                                Layer = layer,
                                FeatureClass = featureLayer.FeatureClass,
                                GeometryType = GetGeometryTypeName(featureLayer.FeatureClass.ShapeType)
                            };
                            layers.Add(layerInfo);
                            System.Diagnostics.Debug.WriteLine($"添加要素图层: {layer.Name}, 几何类型: {layerInfo.GeometryType}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"跳过图层 {layer.Name}，FeatureClass 为空");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"跳过非要素图层: {layer.Name}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"共获取到 {layers.Count} 个要素图层");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取地图图层出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }

            return layers;
        }

        /// <summary>
        /// 递归获取图层组中的所有图层
        /// </summary>
        private static void GetLayersFromGroup(IGroupLayer groupLayer, List<LayerInfo> layers)
        {
            if (groupLayer == null)
                return;

            try
            {
                // 获取图层组中的所有图层
                ICompositeLayer compositeLayer = groupLayer as ICompositeLayer;
                System.Diagnostics.Debug.WriteLine($"处理图层组: {groupLayer.Name}, 子图层数量: {compositeLayer.Count}");
                
                for (int i = 0; i < compositeLayer.Count; i++)
                {
                    ILayer layer = compositeLayer.get_Layer(i);
                    System.Diagnostics.Debug.WriteLine($"处理图层组中的图层: {layer.Name}");
                    
                    // 如果是图层组，递归获取
                    if (layer is IGroupLayer)
                    {
                        GetLayersFromGroup(layer as IGroupLayer, layers);
                    }
                    else if (layer is IFeatureLayer)
                    {
                        // 添加要素图层
                        IFeatureLayer featureLayer = layer as IFeatureLayer;
                        if (featureLayer.FeatureClass != null)
                        {
                            LayerInfo layerInfo = new LayerInfo 
                            { 
                                Name = layer.Name, 
                                Layer = layer,
                                FeatureClass = featureLayer.FeatureClass,
                                GeometryType = GetGeometryTypeName(featureLayer.FeatureClass.ShapeType)
                            };
                            layers.Add(layerInfo);
                            System.Diagnostics.Debug.WriteLine($"从图层组添加要素图层: {layer.Name}, 几何类型: {layerInfo.GeometryType}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取图层组中的图层出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取ArcMap应用程序实例 - 修复版本，移除有问题的Parent属性访问
        /// </summary>
        private static IApplication GetArcMapApplication()
        {
            IApplication application = null;
            
            try
            {
                // 方法1: 通过AppRef获取
                System.Diagnostics.Debug.WriteLine("尝试通过AppRef获取ArcMap实例...");
                application = (IApplication)Marshal.GetActiveObject("esriFramework.AppRef");
                if (application != null)
                {
                    System.Diagnostics.Debug.WriteLine("通过AppRef成功获取ArcMap实例");
                    return application;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"通过AppRef获取ArcMap实例失败: {ex.Message}");
            }

            try
            {
                // 方法2: 通过Application获取
                System.Diagnostics.Debug.WriteLine("尝试通过Application获取ArcMap实例...");
                application = (IApplication)Marshal.GetActiveObject("esriArcMapUI.Application");
                if (application != null)
                {
                    System.Diagnostics.Debug.WriteLine("通过Application成功获取ArcMap实例");
                    return application;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"通过Application获取ArcMap实例失败: {ex.Message}");
            }

            try
            {
                // 方法3: 直接通过MxDocument获取应用程序引用 - 修复版本
                System.Diagnostics.Debug.WriteLine("尝试通过MxDocument获取ArcMap实例...");
                var mxDoc = Marshal.GetActiveObject("esriArcMapUI.MxDocument") as IMxDocument;
                if (mxDoc != null)
                {
                    // 尝试通过类型转换获取应用程序
                    if (mxDoc is IDocumentEvents_Event docEvents)
                    {
                        // 这里我们不能直接访问Parent属性，改用其他方法
                        System.Diagnostics.Debug.WriteLine("MxDocument获取成功，但无法直接访问Parent属性");
                    }
                    
                    // 尝试其他方法获取Application
                    try
                    {
                        // 通过查询接口获取
                        object docObj = mxDoc;
                        if (docObj != null)
                        {
                            System.Diagnostics.Debug.WriteLine("获取到MxDocument对象，尝试其他方法获取Application");
                            // 这里可以添加其他获取Application的方法
                        }
                    }
                    catch (Exception innerEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"通过MxDocument获取Application失败: {innerEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"通过MxDocument获取ArcMap实例失败: {ex.Message}");
            }

            try
            {
                // 方法4: 通过当前进程获取 - 改进版本
                System.Diagnostics.Debug.WriteLine("尝试通过当前进程获取ArcMap实例...");
                
                // 尝试获取当前运行的ArcMap进程
                var processes = System.Diagnostics.Process.GetProcessesByName("ArcMap");
                if (processes.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"找到 {processes.Length} 个ArcMap进程");
                    
                    // 尝试通过ROT (Running Object Table) 获取
                    try
                    {
                        Type appType = Type.GetTypeFromProgID("esriFramework.AppRef");
                        if (appType != null)
                        {
                            application = Marshal.GetActiveObject("esriFramework.AppRef") as IApplication;
                            if (application != null)
                            {
                                System.Diagnostics.Debug.WriteLine("通过进程检测和ROT成功获取ArcMap实例");
                                return application;
                            }
                        }
                    }
                    catch (Exception rotEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"通过ROT获取失败: {rotEx.Message}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("未找到运行中的ArcMap进程");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"通过当前进程获取ArcMap实例失败: {ex.Message}");
            }

            System.Diagnostics.Debug.WriteLine("所有方法都无法获取ArcMap实例");
            return null;
        }

        /// <summary>
        /// 检查ArcMap是否正在运行
        /// </summary>
        /// <returns>是否正在运行</returns>
        public static bool IsArcMapRunning()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("检查ArcMap是否正在运行...");
                
                // 方法1: 检查进程
                var processes = System.Diagnostics.Process.GetProcessesByName("ArcMap");
                if (processes.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"找到 {processes.Length} 个ArcMap进程");
                    
                    // 方法2: 尝试获取应用程序实例
                    try
                    {
                        var application = GetArcMapApplication();
                        bool isRunning = application != null;
                        System.Diagnostics.Debug.WriteLine($"ArcMap运行状态: {isRunning}");
                        return isRunning;
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("无法获取ArcMap应用程序实例，但进程存在");
                        return true; // 进程存在，认为正在运行
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("未找到ArcMap进程");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"检查ArcMap运行状态时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取几何类型名称
        /// </summary>
        /// <param name="shapeType">几何类型</param>
        /// <returns>类型名称</returns>
        private static string GetGeometryTypeName(esriGeometryType shapeType)
        {
            switch (shapeType)
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
        /// 检查图层是否为多边形图层（适用于林草现状和城镇开发边界）
        /// </summary>
        /// <param name="layerInfo">图层信息</param>
        /// <returns>是否为多边形图层</returns>
        public static bool IsPolygonLayer(LayerInfo layerInfo)
        {
            if (layerInfo?.FeatureClass == null)
                return false;

            return layerInfo.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon;
        }

        /// <summary>
        /// 根据名称模式筛选适合的图层
        /// </summary>
        /// <param name="layers">图层列表</param>
        /// <param name="namePatterns">名称模式数组</param>
        /// <returns>匹配的图层列表</returns>
        public static List<LayerInfo> FilterLayersByName(List<LayerInfo> layers, string[] namePatterns)
        {
            List<LayerInfo> filteredLayers = new List<LayerInfo>();

            foreach (LayerInfo layer in layers)
            {
                foreach (string pattern in namePatterns)
                {
                    if (layer.Name.ToUpper().Contains(pattern.ToUpper()))
                    {
                        filteredLayers.Add(layer);
                        break;
                    }
                }
            }

            return filteredLayers;
        }

        /// <summary>
        /// 获取多边形图层列表（用于林草现状和城镇开发边界）
        /// </summary>
        /// <returns>多边形图层列表</returns>
        public static List<LayerInfo> GetPolygonLayers()
        {
            List<LayerInfo> allLayers = GetMapLayers();
            List<LayerInfo> polygonLayers = new List<LayerInfo>();

            foreach (LayerInfo layer in allLayers)
            {
                if (IsPolygonLayer(layer))
                {
                    polygonLayers.Add(layer);
                }
            }

            System.Diagnostics.Debug.WriteLine($"获取到 {polygonLayers.Count} 个多边形图层");
            return polygonLayers;
        }

        /// <summary>
        /// 验证图层连接状态
        /// </summary>
        /// <param name="layerInfo">图层信息</param>
        /// <returns>图层是否可用</returns>
        public static bool ValidateLayerConnection(LayerInfo layerInfo)
        {
            try
            {
                if (layerInfo?.FeatureClass == null)
                    return false;

                // 尝试访问图层的基本信息
                var name = layerInfo.FeatureClass.AliasName;
                var count = layerInfo.FeatureClass.FeatureCount(null);
                
                System.Diagnostics.Debug.WriteLine($"图层验证成功: {layerInfo.Name}, 要素数量: {count}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"图层验证失败: {layerInfo?.Name}, 错误: {ex.Message}");
                return false;
            }
        }
    }

    /// <summary>
    /// 图层信息类，存储图层名称和图层对象
    /// </summary>
    public class LayerInfo
    {
        /// <summary>
        /// 图层名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图层对象
        /// </summary>
        public ILayer Layer { get; set; }

        /// <summary>
        /// 要素类对象
        /// </summary>
        public IFeatureClass FeatureClass { get; set; }

        /// <summary>
        /// 几何类型
        /// </summary>
        public string GeometryType { get; set; }

        /// <summary>
        /// 重写ToString方法，返回图层名称
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// 重写Equals方法，用于比较
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is LayerInfo other)
            {
                return Name == other.Name;
            }
            return false;
        }

        /// <summary>
        /// 重写GetHashCode方法
        /// </summary>
        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}