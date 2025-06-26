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
                System.Diagnostics.Debug.WriteLine("��ʼ��ȡ��ͼͼ��...");
                
                // ��ȡArcMapӦ�ó���
                IApplication application = GetArcMapApplication();
                if (application == null)
                {
                    System.Diagnostics.Debug.WriteLine("�޷���ȡArcMapӦ�ó���ʵ��");
                    return layers;
                }

                System.Diagnostics.Debug.WriteLine("�ɹ���ȡArcMapӦ�ó���ʵ��");

                // ��ȡ��ǰ��ͼ�ĵ�
                IMxDocument mxDocument = application.Document as IMxDocument;
                if (mxDocument == null)
                {
                    System.Diagnostics.Debug.WriteLine("�޷���ȡMxDocument");
                    return layers;
                }

                System.Diagnostics.Debug.WriteLine("�ɹ���ȡMxDocument");

                // ��ȡ��ͼ
                IMap map = mxDocument.FocusMap;
                if (map == null)
                {
                    System.Diagnostics.Debug.WriteLine("�޷���ȡFocusMap");
                    return layers;
                }

                System.Diagnostics.Debug.WriteLine($"�ɹ���ȡ��ͼ��ͼ������: {map.LayerCount}");

                // ��������ͼ��
                for (int i = 0; i < map.LayerCount; i++)
                {
                    ILayer layer = map.get_Layer(i);
                    System.Diagnostics.Debug.WriteLine($"����ͼ�� {i}: {layer.Name}");
                    
                    // �����ͼ���飬�ݹ��ȡ���е�ͼ��
                    if (layer is IGroupLayer)
                    {
                        System.Diagnostics.Debug.WriteLine($"ͼ����: {layer.Name}");
                        GetLayersFromGroup(layer as IGroupLayer, layers);
                    }
                    else if (layer is IFeatureLayer)
                    {
                        // ���Ҫ��ͼ��
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
                            System.Diagnostics.Debug.WriteLine($"���Ҫ��ͼ��: {layer.Name}, ��������: {layerInfo.GeometryType}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"����ͼ�� {layer.Name}��FeatureClass Ϊ��");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"������Ҫ��ͼ��: {layer.Name}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"����ȡ�� {layers.Count} ��Ҫ��ͼ��");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡ��ͼͼ�����: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"��ջ����: {ex.StackTrace}");
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

            try
            {
                // ��ȡͼ�����е�����ͼ��
                ICompositeLayer compositeLayer = groupLayer as ICompositeLayer;
                System.Diagnostics.Debug.WriteLine($"����ͼ����: {groupLayer.Name}, ��ͼ������: {compositeLayer.Count}");
                
                for (int i = 0; i < compositeLayer.Count; i++)
                {
                    ILayer layer = compositeLayer.get_Layer(i);
                    System.Diagnostics.Debug.WriteLine($"����ͼ�����е�ͼ��: {layer.Name}");
                    
                    // �����ͼ���飬�ݹ��ȡ
                    if (layer is IGroupLayer)
                    {
                        GetLayersFromGroup(layer as IGroupLayer, layers);
                    }
                    else if (layer is IFeatureLayer)
                    {
                        // ���Ҫ��ͼ��
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
                            System.Diagnostics.Debug.WriteLine($"��ͼ�������Ҫ��ͼ��: {layer.Name}, ��������: {layerInfo.GeometryType}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡͼ�����е�ͼ�����: {ex.Message}");
            }
        }

        /// <summary>
        /// ��ȡArcMapӦ�ó���ʵ�� - �޸��汾���Ƴ��������Parent���Է���
        /// </summary>
        private static IApplication GetArcMapApplication()
        {
            IApplication application = null;
            
            try
            {
                // ����1: ͨ��AppRef��ȡ
                System.Diagnostics.Debug.WriteLine("����ͨ��AppRef��ȡArcMapʵ��...");
                application = (IApplication)Marshal.GetActiveObject("esriFramework.AppRef");
                if (application != null)
                {
                    System.Diagnostics.Debug.WriteLine("ͨ��AppRef�ɹ���ȡArcMapʵ��");
                    return application;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ͨ��AppRef��ȡArcMapʵ��ʧ��: {ex.Message}");
            }

            try
            {
                // ����2: ͨ��Application��ȡ
                System.Diagnostics.Debug.WriteLine("����ͨ��Application��ȡArcMapʵ��...");
                application = (IApplication)Marshal.GetActiveObject("esriArcMapUI.Application");
                if (application != null)
                {
                    System.Diagnostics.Debug.WriteLine("ͨ��Application�ɹ���ȡArcMapʵ��");
                    return application;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ͨ��Application��ȡArcMapʵ��ʧ��: {ex.Message}");
            }

            try
            {
                // ����3: ֱ��ͨ��MxDocument��ȡӦ�ó������� - �޸��汾
                System.Diagnostics.Debug.WriteLine("����ͨ��MxDocument��ȡArcMapʵ��...");
                var mxDoc = Marshal.GetActiveObject("esriArcMapUI.MxDocument") as IMxDocument;
                if (mxDoc != null)
                {
                    // ����ͨ������ת����ȡӦ�ó���
                    if (mxDoc is IDocumentEvents_Event docEvents)
                    {
                        // �������ǲ���ֱ�ӷ���Parent���ԣ�������������
                        System.Diagnostics.Debug.WriteLine("MxDocument��ȡ�ɹ������޷�ֱ�ӷ���Parent����");
                    }
                    
                    // ��������������ȡApplication
                    try
                    {
                        // ͨ����ѯ�ӿڻ�ȡ
                        object docObj = mxDoc;
                        if (docObj != null)
                        {
                            System.Diagnostics.Debug.WriteLine("��ȡ��MxDocument���󣬳�������������ȡApplication");
                            // ����������������ȡApplication�ķ���
                        }
                    }
                    catch (Exception innerEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"ͨ��MxDocument��ȡApplicationʧ��: {innerEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ͨ��MxDocument��ȡArcMapʵ��ʧ��: {ex.Message}");
            }

            try
            {
                // ����4: ͨ����ǰ���̻�ȡ - �Ľ��汾
                System.Diagnostics.Debug.WriteLine("����ͨ����ǰ���̻�ȡArcMapʵ��...");
                
                // ���Ի�ȡ��ǰ���е�ArcMap����
                var processes = System.Diagnostics.Process.GetProcessesByName("ArcMap");
                if (processes.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"�ҵ� {processes.Length} ��ArcMap����");
                    
                    // ����ͨ��ROT (Running Object Table) ��ȡ
                    try
                    {
                        Type appType = Type.GetTypeFromProgID("esriFramework.AppRef");
                        if (appType != null)
                        {
                            application = Marshal.GetActiveObject("esriFramework.AppRef") as IApplication;
                            if (application != null)
                            {
                                System.Diagnostics.Debug.WriteLine("ͨ�����̼���ROT�ɹ���ȡArcMapʵ��");
                                return application;
                            }
                        }
                    }
                    catch (Exception rotEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"ͨ��ROT��ȡʧ��: {rotEx.Message}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("δ�ҵ������е�ArcMap����");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ͨ����ǰ���̻�ȡArcMapʵ��ʧ��: {ex.Message}");
            }

            System.Diagnostics.Debug.WriteLine("���з������޷���ȡArcMapʵ��");
            return null;
        }

        /// <summary>
        /// ���ArcMap�Ƿ���������
        /// </summary>
        /// <returns>�Ƿ���������</returns>
        public static bool IsArcMapRunning()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("���ArcMap�Ƿ���������...");
                
                // ����1: ������
                var processes = System.Diagnostics.Process.GetProcessesByName("ArcMap");
                if (processes.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"�ҵ� {processes.Length} ��ArcMap����");
                    
                    // ����2: ���Ի�ȡӦ�ó���ʵ��
                    try
                    {
                        var application = GetArcMapApplication();
                        bool isRunning = application != null;
                        System.Diagnostics.Debug.WriteLine($"ArcMap����״̬: {isRunning}");
                        return isRunning;
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("�޷���ȡArcMapӦ�ó���ʵ���������̴���");
                        return true; // ���̴��ڣ���Ϊ��������
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("δ�ҵ�ArcMap����");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"���ArcMap����״̬ʱ����: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        /// <param name="shapeType">��������</param>
        /// <returns>��������</returns>
        private static string GetGeometryTypeName(esriGeometryType shapeType)
        {
            switch (shapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    return "��";
                case esriGeometryType.esriGeometryPolyline:
                    return "��";
                case esriGeometryType.esriGeometryPolygon:
                    return "��";
                case esriGeometryType.esriGeometryMultipoint:
                    return "���";
                default:
                    return "����";
            }
        }

        /// <summary>
        /// ���ͼ���Ƿ�Ϊ�����ͼ�㣨�������ֲ���״�ͳ��򿪷��߽磩
        /// </summary>
        /// <param name="layerInfo">ͼ����Ϣ</param>
        /// <returns>�Ƿ�Ϊ�����ͼ��</returns>
        public static bool IsPolygonLayer(LayerInfo layerInfo)
        {
            if (layerInfo?.FeatureClass == null)
                return false;

            return layerInfo.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon;
        }

        /// <summary>
        /// ��������ģʽɸѡ�ʺϵ�ͼ��
        /// </summary>
        /// <param name="layers">ͼ���б�</param>
        /// <param name="namePatterns">����ģʽ����</param>
        /// <returns>ƥ���ͼ���б�</returns>
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
        /// ��ȡ�����ͼ���б������ֲ���״�ͳ��򿪷��߽磩
        /// </summary>
        /// <returns>�����ͼ���б�</returns>
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

            System.Diagnostics.Debug.WriteLine($"��ȡ�� {polygonLayers.Count} �������ͼ��");
            return polygonLayers;
        }

        /// <summary>
        /// ��֤ͼ������״̬
        /// </summary>
        /// <param name="layerInfo">ͼ����Ϣ</param>
        /// <returns>ͼ���Ƿ����</returns>
        public static bool ValidateLayerConnection(LayerInfo layerInfo)
        {
            try
            {
                if (layerInfo?.FeatureClass == null)
                    return false;

                // ���Է���ͼ��Ļ�����Ϣ
                var name = layerInfo.FeatureClass.AliasName;
                var count = layerInfo.FeatureClass.FeatureCount(null);
                
                System.Diagnostics.Debug.WriteLine($"ͼ����֤�ɹ�: {layerInfo.Name}, Ҫ������: {count}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ͼ����֤ʧ��: {layerInfo?.Name}, ����: {ex.Message}");
                return false;
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
        /// ��������
        /// </summary>
        public string GeometryType { get; set; }

        /// <summary>
        /// ��дToString����������ͼ������
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// ��дEquals���������ڱȽ�
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
        /// ��дGetHashCode����
        /// </summary>
        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}