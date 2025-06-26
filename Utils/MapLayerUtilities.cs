using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

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
                // 获取ArcMap应用程序
                IApplication application = GetArcMapApplication();
                if (application == null)
                    return layers;

                // 获取当前地图文档
                IMxDocument mxDocument = application.Document as IMxDocument;
                if (mxDocument == null)
                    return layers;

                // 获取地图
                IMap map = mxDocument.FocusMap;
                if (map == null)
                    return layers;

                // 遍历所有图层
                for (int i = 0; i < map.LayerCount; i++)
                {
                    ILayer layer = map.get_Layer(i);
                    
                    // 如果是图层组，递归获取其中的图层
                    if (layer is IGroupLayer)
                    {
                        GetLayersFromGroup(layer as IGroupLayer, layers);
                    }
                    else if (layer is IFeatureLayer)
                    {
                        // 添加要素图层
                        IFeatureLayer featureLayer = layer as IFeatureLayer;
                        layers.Add(new LayerInfo 
                        { 
                            Name = layer.Name, 
                            Layer = layer,
                            FeatureClass = featureLayer.FeatureClass
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取地图图层出错: {ex.Message}");
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

            // 获取图层组中的所有图层
            ICompositeLayer compositeLayer = groupLayer as ICompositeLayer;
            for (int i = 0; i < compositeLayer.Count; i++)
            {
                ILayer layer = compositeLayer.get_Layer(i);
                
                // 如果是图层组，递归获取
                if (layer is IGroupLayer)
                {
                    GetLayersFromGroup(layer as IGroupLayer, layers);
                }
                else if (layer is IFeatureLayer)
                {
                    // 添加要素图层
                    IFeatureLayer featureLayer = layer as IFeatureLayer;
                    layers.Add(new LayerInfo 
                    { 
                        Name = layer.Name, 
                        Layer = layer,
                        FeatureClass = featureLayer.FeatureClass
                    });
                }
            }
        }

        /// <summary>
        /// 获取ArcMap应用程序实例
        /// </summary>
        private static IApplication GetArcMapApplication()
        {
            try
            {
                // 获取正在运行的ArcMap实例
                UID uid = new UIDClass();
                uid.Value = "esriArcMapUI.MxDocument";
                
                // 获取ESRI应用程序
                return (IApplication)Marshal.GetActiveObject("esriFramework.AppRef");
            }
            catch
            {
                // 如果无法获取正在运行的ArcMap实例，返回null
                return null;
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
        /// 重写ToString方法，返回图层名称
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}