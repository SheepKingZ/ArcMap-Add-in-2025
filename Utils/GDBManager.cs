using System;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using System.IO;

namespace ForestResourcePlugin
{
    /// <summary>
    /// ����������ݿ�(GDB)�����Ͳ����Ĺ�����
    /// </summary>
    public static class GDBManager
    {
        /// <summary>
        /// �����ļ��������ݿ�(File Geodatabase)
        /// </summary>
        /// <param name="path">Ҫ������GDB·��(����.gdb��׺)</param>
        /// <returns>�ɹ���������true�����򷵻�false</returns>
        public static bool CreateFileGDB(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                System.Diagnostics.Debug.WriteLine("����: GDB·������Ϊ��");
                return false;
            }

            // ȷ��·����.gdb��β
            if (!path.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase))
            {
                path = path + ".gdb";
                System.Diagnostics.Debug.WriteLine($"������·�������.gdb��׺: {path}");
            }

            // ���GDB�Ƿ��Ѵ���
            if (Directory.Exists(path))
            {
                System.Diagnostics.Debug.WriteLine($"GDB�Ѵ���: {path}");
                return true;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"��ʼ����File GDB: {path}");
                
                // ������Ŀ¼�������Ҫ��
                string parentDir = System.IO.Path.GetDirectoryName(path);
                if (!Directory.Exists(parentDir))
                {
                    Directory.CreateDirectory(parentDir);
                    System.Diagnostics.Debug.WriteLine($"�Ѵ�����Ŀ¼: {parentDir}");
                }

                // ��ȡFileGDB�����ռ乤������
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                if (factoryType == null)
                {
                    System.Diagnostics.Debug.WriteLine("����: �޷���ȡFileGDBWorkspaceFactory������");
                    return false;
                }
                
                // ���������ռ乤��ʵ��
                IWorkspaceFactory workspaceFactory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;
                if (workspaceFactory == null)
                {
                    System.Diagnostics.Debug.WriteLine("����: �޷�����FileGDBWorkspaceFactoryʵ��");
                    return false;
                }

                // ���������ռ�
                IWorkspaceName workspaceName = workspaceFactory.Create(
                    parentDir, System.IO.Path.GetFileName(path), null, 0);

                if (workspaceName != null)
                {
                    System.Diagnostics.Debug.WriteLine($"�ɹ�����File GDB: {path}");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("����GDBʧ��: �����ռ�����Ϊ��");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����File GDBʱ����: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"�쳣����: {ex}");
                return false;
            }
        }

        /// <summary>
        /// ���GDB�Ƿ���ڣ�����������򴴽�
        /// </summary>
        /// <param name="gdbPath">GDB·��</param>
        /// <returns>GDB·�����ڻ򴴽��ɹ�����true�����򷵻�false</returns>
        public static bool EnsureGDBExists(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            // ȷ��·����.gdb��β
            if (!path.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase))
            {
                path = path + ".gdb";
            }

            // ���GDB�Ƿ��Ѵ���
            if (Directory.Exists(path))
            {
                return true;
            }

            // ����GDB
            return CreateFileGDB(path);
        }
        
        /// <summary>
        /// ��GDB�д���Ҫ����
        /// </summary>
        /// <param name="gdbPath">GDB·��</param>
        /// <param name="featureClassName">Ҫ��������</param>
        /// <param name="geometryType">��������</param>
        /// <param name="spatialReference">�ռ�ο�</param>
        /// <returns>������Ҫ���࣬ʧ���򷵻�null</returns>
        public static IFeatureClass CreateFeatureClass(string gdbPath, string featureClassName, 
            esriGeometryType geometryType, ISpatialReference spatialReference = null)
        {
            if (string.IsNullOrEmpty(gdbPath) || string.IsNullOrEmpty(featureClassName))
            {
                System.Diagnostics.Debug.WriteLine("����: GDB·����Ҫ�������Ʋ���Ϊ��");
                return null;
            }

            // ȷ��GDB����
            if (!EnsureGDBExists(gdbPath))
            {
                System.Diagnostics.Debug.WriteLine($"����: �޷�ȷ��GDB����: {gdbPath}");
                return null;
            }

            IWorkspace workspace = null;
            try
            {
                // ��GDB�����ռ�
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;
                workspace = workspaceFactory.OpenFromFile(gdbPath, 0);

                if (workspace == null)
                {
                    System.Diagnostics.Debug.WriteLine($"����: �޷���GDB�����ռ�: {gdbPath}");
                    return null;
                }

                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                if (featureWorkspace == null)
                {
                    System.Diagnostics.Debug.WriteLine("����: �޷���ȡҪ�ع����ռ�ӿ�");
                    return null;
                }

                // ���Ҫ�����Ƿ��Ѵ���
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

                // ���Ҫ�����Ѵ��ڣ������
                if (featureClassExists)
                {
                    System.Diagnostics.Debug.WriteLine($"Ҫ�����Ѵ���: {featureClassName}, ��ֱ�Ӵ���");
                    return featureWorkspace.OpenFeatureClass(featureClassName);
                }

                // �����ֶμ���
                IFields fields = new FieldsClass();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                // ���OID�ֶ�
                IField oidField = new FieldClass();
                IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
                oidFieldEdit.Name_2 = "OBJECTID";
                oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldsEdit.AddField(oidField);

                // ��Ӽ����ֶ�
                IField shapeField = new FieldClass();
                IFieldEdit shapeFieldEdit = (IFieldEdit)shapeField;
                shapeFieldEdit.Name_2 = "SHAPE";
                shapeFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

                // ���ü��ζ���
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.GeometryType_2 = geometryType;
                geometryDefEdit.HasZ_2 = false;
                geometryDefEdit.HasM_2 = false;

                // ���ÿռ�ο�
                if (spatialReference != null)
                {
                    geometryDefEdit.SpatialReference_2 = spatialReference;
                }

                shapeFieldEdit.GeometryDef_2 = geometryDef;
                fieldsEdit.AddField(shapeField);

                // ����Ҫ����
                IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(
                    featureClassName, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

                System.Diagnostics.Debug.WriteLine($"�ɹ�����Ҫ����: {featureClassName}");
                return featureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����Ҫ����ʱ����: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"�쳣����: {ex}");
                return null;
            }
        }
    }
}