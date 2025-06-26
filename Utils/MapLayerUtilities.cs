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
    /// ��ͼͼ�㹤���࣬���ڻ�ȡArcMap�е�ǰ���ص�ͼ��
    /// </summary>
    public static class MapLayerUtilities
    {
        /// <summary>
        /// ��ȡ��ǰArcMap�ĵ��е�����ͼ��
        /// </summary>
        /// <returns>ͼ����Ϣ�б�����ͼ�����ƺ�ͼ�����</returns>
        public static List<LayerInfo> GetMapLayers()
        {
            List<LayerInfo> layers = new List<LayerInfo>();
            
            try
            {
                // ��ȡArcMapӦ�ó���
                IApplication application = GetArcMapApplication();
                if (application == null)
                    return layers;

                // ��ȡ��ǰ��ͼ�ĵ�
                IMxDocument mxDocument = application.Document as IMxDocument;
                if (mxDocument == null)
                    return layers;

                // ��ȡ��ͼ
                IMap map = mxDocument.FocusMap;
                if (map == null)
                    return layers;

                // ��������ͼ��
                for (int i = 0; i < map.LayerCount; i++)
                {
                    ILayer layer = map.get_Layer(i);
                    
                    // �����ͼ���飬�ݹ��ȡ���е�ͼ��
                    if (layer is IGroupLayer)
                    {
                        GetLayersFromGroup(layer as IGroupLayer, layers);
                    }
                    else if (layer is IFeatureLayer)
                    {
                        // ���Ҫ��ͼ��
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
                System.Diagnostics.Debug.WriteLine($"��ȡ��ͼͼ�����: {ex.Message}");
            }

            return layers;
        }

        /// <summary>
        /// �ݹ��ȡͼ�����е�����ͼ��
        /// </summary>
        private static void GetLayersFromGroup(IGroupLayer groupLayer, List<LayerInfo> layers)
        {
            if (groupLayer == null)
                return;

            // ��ȡͼ�����е�����ͼ��
            ICompositeLayer compositeLayer = groupLayer as ICompositeLayer;
            for (int i = 0; i < compositeLayer.Count; i++)
            {
                ILayer layer = compositeLayer.get_Layer(i);
                
                // �����ͼ���飬�ݹ��ȡ
                if (layer is IGroupLayer)
                {
                    GetLayersFromGroup(layer as IGroupLayer, layers);
                }
                else if (layer is IFeatureLayer)
                {
                    // ���Ҫ��ͼ��
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
        /// ��ȡArcMapӦ�ó���ʵ��
        /// </summary>
        private static IApplication GetArcMapApplication()
        {
            try
            {
                // ��ȡ�������е�ArcMapʵ��
                UID uid = new UIDClass();
                uid.Value = "esriArcMapUI.MxDocument";
                
                // ��ȡESRIӦ�ó���
                return (IApplication)Marshal.GetActiveObject("esriFramework.AppRef");
            }
            catch
            {
                // ����޷���ȡ�������е�ArcMapʵ��������null
                return null;
            }
        }
    }

    /// <summary>
    /// ͼ����Ϣ�࣬�洢ͼ�����ƺ�ͼ�����
    /// </summary>
    public class LayerInfo
    {
        /// <summary>
        /// ͼ������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ͼ�����
        /// </summary>
        public ILayer Layer { get; set; }

        /// <summary>
        /// Ҫ�������
        /// </summary>
        public IFeatureClass FeatureClass { get; set; }

        /// <summary>
        /// ��дToString����������ͼ������
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}