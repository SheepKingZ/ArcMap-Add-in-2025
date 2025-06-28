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
    /// �������ݿ⹤���࣬���ڴ���������͵ĵ������ݿ����
    /// </summary>
    public static class GeodatabaseUtilities
    {
        /// <summary>
        /// �������ݿ�����ö��
        /// </summary>
        public enum GeodatabaseType
        {
            FileGeodatabase,    // File GDB (.gdb)
            PersonalGeodatabase, // Personal GDB (.mdb)
            SDEGeodatabase,     // SDE���ݿ�
            Unknown
        }

        /// <summary>
        /// Ҫ������Ϣ��
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
                return $"{Name} ({GeometryTypeName}, {FeatureCount} Ҫ��)";
            }
        }

        /// <summary>
        /// �����ռ���Ϣ��
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
        /// ���������ݿ�����
        /// </summary>
        /// <param name="path">���ݿ�·��</param>
        /// <returns>���ݿ�����</returns>
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
                System.Diagnostics.Debug.WriteLine($"���������ݿ�����ʱ����: {ex.Message}");
                return GeodatabaseType.Unknown;
            }
        }

        /// <summary>
        /// �򿪵������ݿ⹤���ռ�
        /// </summary>
        /// <param name="path">���ݿ�·��</param>
        /// <returns>�����ռ���Ϣ</returns>
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
                        workspaceInfo.ErrorMessage = "��֧�ֵĵ������ݿ�����";
                        return workspaceInfo;
                }

                // ��֤·���Ƿ����
                if (workspaceInfo.Type == GeodatabaseType.FileGeodatabase && !Directory.Exists(path))
                {
                    workspaceInfo.ErrorMessage = "�ļ��������ݿ�·��������";
                    return workspaceInfo;
                }
                else if (workspaceInfo.Type == GeodatabaseType.PersonalGeodatabase && !File.Exists(path))
                {
                    workspaceInfo.ErrorMessage = "���˵������ݿ��ļ�������";
                    return workspaceInfo;
                }

                // �򿪹����ռ�
                workspaceInfo.Workspace = workspaceFactory.OpenFromFile(path, 0);
                
                if (workspaceInfo.Workspace != null)
                {
                    workspaceInfo.IsConnected = true;
                    System.Diagnostics.Debug.WriteLine($"�ɹ��򿪵������ݿ�: {path} (����: {workspaceInfo.Type})");
                }
                else
                {
                    workspaceInfo.ErrorMessage = "�޷��򿪵������ݿ�";
                }
            }
            catch (Exception ex)
            {
                workspaceInfo.ErrorMessage = $"�򿪵������ݿ�ʱ����: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"�򿪵������ݿ�ʧ��: {path}, ����: {ex.Message}");
            }

            return workspaceInfo;
        }

        /// <summary>
        /// ��ȡ�������ݿ��е�����Ҫ����
        /// </summary>
        /// <param name="workspaceInfo">�����ռ���Ϣ</param>
        /// <returns>Ҫ������Ϣ�б�</returns>
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
                    System.Diagnostics.Debug.WriteLine("�����ռ䲻֧��Ҫ�������");
                    return featureClasses;
                }

                // ��ȡҪ����ö����
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
                            System.Diagnostics.Debug.WriteLine($"����Ҫ����: {featureClassInfo.Name} ({featureClassInfo.GeometryTypeName}, {featureClassInfo.FeatureCount} Ҫ��)");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"����Ҫ���� {dataset.Name} ʱ����: {ex.Message}");
                    }

                    dataset = enumDataset.Next();
                }

                System.Diagnostics.Debug.WriteLine($"������ {featureClasses.Count} ��Ҫ����");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡҪ�����б�ʱ����: {ex.Message}");
            }

            return featureClasses;
        }

        /// <summary>
        /// ��ȡ�����Ҫ���ࣨ�����ֲ���״�ͳ��򿪷��߽磩
        /// </summary>
        /// <param name="workspaceInfo">�����ռ���Ϣ</param>
        /// <returns>�����Ҫ�����б�</returns>
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

            System.Diagnostics.Debug.WriteLine($"�ҵ� {polygonFeatureClasses.Count} �������Ҫ����");
            return polygonFeatureClasses;
        }

        /// <summary>
        /// �������ƻ�ȡҪ����
        /// </summary>
        /// <param name="workspaceInfo">�����ռ���Ϣ</param>
        /// <param name="featureClassName">Ҫ��������</param>
        /// <returns>Ҫ�������</returns>
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
                System.Diagnostics.Debug.WriteLine($"��ȡҪ���� {featureClassName} ʱ����: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ��֤Ҫ��������״̬
        /// </summary>
        /// <param name="featureClass">Ҫ����</param>
        /// <returns>�Ƿ���������</returns>
        public static bool ValidateFeatureClass(IFeatureClass featureClass)
        {
            try
            {
                if (featureClass == null)
                    return false;

                // ���Է��ʻ�������
                var name = featureClass.AliasName;
                var fieldCount = featureClass.Fields.FieldCount;
                var hasGeometry = featureClass.ShapeType != esriGeometryType.esriGeometryNull;

                System.Diagnostics.Debug.WriteLine($"Ҫ������֤�ɹ�: {name}, �ֶ���: {fieldCount}, ��������: {featureClass.ShapeType}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ҫ������֤ʧ��: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ��ȡҪ������ֶ��б�
        /// </summary>
        /// <param name="featureClass">Ҫ����</param>
        /// <returns>�ֶ������б�</returns>
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
                    
                    // ֻ����ʺϵ��ֶ�����
                    if (field.Type == esriFieldType.esriFieldTypeString ||
                        field.Type == esriFieldType.esriFieldTypeSmallInteger ||
                        field.Type == esriFieldType.esriFieldTypeInteger ||
                        field.Type == esriFieldType.esriFieldTypeSingle ||
                        field.Type == esriFieldType.esriFieldTypeDouble)
                    {
                        fieldNames.Add(field.Name);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Ҫ���� {featureClass.AliasName} �� {fieldNames.Count} ����Ч�ֶ�");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡҪ�����ֶ�ʱ����: {ex.Message}");
            }

            return fieldNames;
        }

        /// <summary>
        /// ����ƥ��Ҫ��������
        /// </summary>
        /// <param name="featureClasses">Ҫ�����б�</param>
        /// <param name="namePatterns">ƥ��ģʽ</param>
        /// <returns>ƥ���Ҫ����</returns>
        public static FeatureClassInfo FindBestMatchFeatureClass(List<FeatureClassInfo> featureClasses, string[] namePatterns)
        {
            foreach (string pattern in namePatterns)
            {
                foreach (var fc in featureClasses)
                {
                    if (fc.Name.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        fc.AliasName.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"����ƥ��ɹ�: {fc.Name} ƥ��ģʽ {pattern}");
                        return fc;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        /// <param name="geometryType">��������</param>
        /// <returns>��������</returns>
        private static string GetGeometryTypeName(esriGeometryType geometryType)
        {
            switch (geometryType)
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
        /// �����������ݿ������ַ���
        /// </summary>
        /// <param name="server">������</param>
        /// <param name="instance">ʵ��</param>
        /// <param name="database">���ݿ�</param>
        /// <param name="user">�û���</param>
        /// <param name="password">����</param>
        /// <returns>�����ַ���</returns>
        public static string CreateSdeConnectionString(string server, string instance, string database, string user, string password)
        {
            return $"SERVER={server};INSTANCE={instance};DATABASE={database};USER={user};PASSWORD={password};VERSION=sde.DEFAULT";
        }

        /// <summary>
        /// �ͷŹ����ռ���Դ
        /// </summary>
        /// <param name="workspaceInfo">�����ռ���Ϣ</param>
        public static void ReleaseWorkspace(WorkspaceInfo workspaceInfo)
        {
            try
            {
                if (workspaceInfo?.Workspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspaceInfo.Workspace);
                    workspaceInfo.Workspace = null;
                    workspaceInfo.IsConnected = false;
                    System.Diagnostics.Debug.WriteLine("�����ռ���Դ���ͷ�");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"�ͷŹ����ռ���Դʱ����: {ex.Message}");
            }
        }
    }
}