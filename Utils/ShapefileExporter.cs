using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// Shapefile����������
    /// </summary>
    public class ShapefileExporter
    {
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// ����Ҫ�ص�Shapefile
        /// </summary>
        /// <param name="features">Ҫ������Ҫ���б�</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="outputPath">���Shapefile·��</param>
        /// <param name="fieldMappings">�ֶ�ӳ������</param>
        /// <param name="coordinateSystem">����ϵ����</param>
        /// <param name="progressCallback">���Ȼص�</param>
        public void ExportToShapefile(
            List<IFeature> features,
            IFeatureClass sourceFeatureClass,
            string outputPath,
            Dictionary<string, string> fieldMappings,
            string coordinateSystem,
            ProgressCallback progressCallback = null)
        {
            if (features == null || features.Count == 0)
            {
                throw new ArgumentException("Ҫ���б���Ϊ��");
            }

            if (sourceFeatureClass == null)
            {
                throw new ArgumentException("ԴҪ���಻��Ϊ��");
            }

            progressCallback?.Invoke(5, "���ڴ�����������ռ�...");

            // ������������ռ�
            string outputDir = System.IO.Path.GetDirectoryName(outputPath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(outputPath);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
            IFeatureWorkspace outputWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(outputDir, 0);

            progressCallback?.Invoke(10, "���ڴ����ֶνṹ...");

            // �������Ҫ������ֶ�
            IFieldsEdit outputFields = CreateOutputFields(sourceFeatureClass, fieldMappings, features[0]);

            progressCallback?.Invoke(15, "���ڴ������Ҫ����...");

            // �������Ҫ����
            IFeatureClass outputFeatureClass = outputWorkspace.CreateFeatureClass(
                fileName,
                outputFields,
                null, // UID
                null, // CLSID
                esriFeatureType.esriFTSimple,
                "Shape", // geometry field name
                "" // config keyword
            );

            progressCallback?.Invoke(20, "������������ϵ...");

            // ��������ϵ�����ָ����
            if (!string.IsNullOrEmpty(coordinateSystem))
            {
                SetCoordinateSystem(outputFeatureClass, coordinateSystem);
            }

            progressCallback?.Invoke(25, "��ʼ����Ҫ��...");

            // ����Ҫ��
            CopyFeatures(features, sourceFeatureClass, outputFeatureClass, fieldMappings, progressCallback);

            progressCallback?.Invoke(100, $"�ɹ����� {features.Count} ��Ҫ�ص� {outputPath}");

            // �ͷ���Դ
            System.Runtime.InteropServices.Marshal.ReleaseComObject(outputFeatureClass);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(outputWorkspace);
        }

        /// <summary>
        /// ��������ֶνṹ
        /// </summary>
        private IFieldsEdit CreateOutputFields(IFeatureClass sourceFeatureClass, 
            Dictionary<string, string> fieldMappings, IFeature sampleFeature)
        {
            IFieldsEdit outputFields = new FieldsClass();

            // ��ӱ����OID�ֶ�
            IFieldEdit oidField = new FieldClass();
            oidField.Name_2 = "FID";
            oidField.Type_2 = esriFieldType.esriFieldTypeOID;
            outputFields.AddField(oidField);

            // ��Ӽ����ֶ�
            IFieldEdit shapeField = new FieldClass();
            shapeField.Name_2 = "Shape";
            shapeField.Type_2 = esriFieldType.esriFieldTypeGeometry;
            
            // ���ü��ζ���
            IGeometryDefEdit geometryDef = new GeometryDefClass();
            geometryDef.GeometryType_2 = sourceFeatureClass.ShapeType;
            geometryDef.HasZ_2 = false;
            geometryDef.HasM_2 = false;
            shapeField.GeometryDef_2 = geometryDef;
            
            outputFields.AddField(shapeField);

            // ���ӳ����ֶ�
            IFields sourceFields = sourceFeatureClass.Fields;
            
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;
                string sourceFieldName = mapping.Value;

                // ����Դ�ֶ�
                int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                if (sourceFieldIndex != -1)
                {
                    IField sourceField = sourceFields.get_Field(sourceFieldIndex);
                    IFieldEdit targetField = new FieldClass();

                    // ����Ŀ���ֶ�����
                    targetField.Name_2 = targetFieldName;
                    targetField.AliasName_2 = GetFieldDisplayName(targetFieldName);
                    targetField.Type_2 = sourceField.Type;
                    
                    // �����ֶγ��Ⱥ;���
                    if (sourceField.Type == esriFieldType.esriFieldTypeString)
                    {
                        targetField.Length_2 = Math.Max(sourceField.Length, 50); // ȷ���㹻�ĳ���
                    }
                    else if (sourceField.Type == esriFieldType.esriFieldTypeDouble || 
                             sourceField.Type == esriFieldType.esriFieldTypeSingle)
                    {
                        targetField.Precision_2 = sourceField.Precision;
                        targetField.Scale_2 = sourceField.Scale;
                    }

                    targetField.IsNullable_2 = true;
                    outputFields.AddField(targetField);
                }
            }

            return outputFields;
        }

        /// <summary>
        /// ����Ҫ������
        /// </summary>
        private void CopyFeatures(List<IFeature> sourceFeatures, IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass, Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback)
        {
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);

            try
            {
                int totalFeatures = sourceFeatures.Count;
                int processedCount = 0;

                foreach (IFeature sourceFeature in sourceFeatures)
                {
                    // ���Ƽ���
                    featureBuffer.Shape = sourceFeature.ShapeCopy;

                    // ��������ֵ
                    foreach (var mapping in fieldMappings)
                    {
                        string targetFieldName = mapping.Key;
                        string sourceFieldName = mapping.Value;

                        int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                        int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);

                        if (sourceFieldIndex != -1 && targetFieldIndex != -1)
                        {
                            object sourceValue = sourceFeature.get_Value(sourceFieldIndex);
                            
                            // ��������ֵת��
                            object targetValue = ConvertFieldValue(sourceValue, targetFieldName, sourceFieldName);
                            
                            featureBuffer.set_Value(targetFieldIndex, targetValue);
                        }
                    }

                    // ����Ҫ��
                    insertCursor.InsertFeature(featureBuffer);

                    processedCount++;
                    
                    // ���½���
                    if (processedCount % 10 == 0 || processedCount == totalFeatures)
                    {
                        int percentage = 25 + (int)((processedCount / (double)totalFeatures) * 75);
                        progressCallback?.Invoke(percentage, 
                            $"���ڸ���Ҫ��... ({processedCount}/{totalFeatures})");
                    }
                }

                // �ύ�������
                insertCursor.Flush();
            }
            finally
            {
                // �ͷ���Դ
                if (insertCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                }
                if (featureBuffer != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureBuffer);
                }
            }
        }

        /// <summary>
        /// ת���ֶ�ֵ
        /// </summary>
        private object ConvertFieldValue(object sourceValue, string targetFieldName, string sourceFieldName)
        {
            if (sourceValue == null || sourceValue == DBNull.Value)
            {
                return null;
            }

            // ����Ȩ��ֵת��
            if (targetFieldName.Equals("TDQS", StringComparison.OrdinalIgnoreCase))
            {
                string strValue = sourceValue.ToString();
                switch (strValue)
                {
                    case "1":
                    case "20":
                        return "����";
                    case "2":
                    case "30":
                        return "����";
                    default:
                        return strValue;
                }
            }

            // ��������ת��
            if (targetFieldName.Equals("DLMC", StringComparison.OrdinalIgnoreCase))
            {
                string strValue = sourceValue.ToString();
                if (strValue.StartsWith("03"))
                {
                    return "�ֵ�";
                }
                return strValue;
            }

            return sourceValue;
        }

        /// <summary>
        /// ��ȡ�ֶ���ʾ����
        /// </summary>
        private string GetFieldDisplayName(string fieldName)
        {
            var displayNames = new Dictionary<string, string>
            {
                { "TBDH", "ͼ�߱��" },
                { "DLMC", "��������" },
                { "TDQS", "����Ȩ��" },
                { "MJ", "���" },
                { "LZFL", "���ַ���" },
                { "QSXZ", "Ȩ������" }
            };

            return displayNames.ContainsKey(fieldName) ? displayNames[fieldName] : fieldName;
        }

        /// <summary>
        /// ��������ϵ
        /// </summary>
        private void SetCoordinateSystem(IFeatureClass featureClass, string coordinateSystemName)
        {
            try
            {
                // ������Ը�����Ҫʵ������ϵ����
                // ������������ϵ�Ƚϸ��ӣ�������ʱ����
                // ��ʵ��Ӧ���У�����ͨ��ISpatialReferenceFactory����������ϵ
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��������ϵʧ��: {ex.Message}");
                // ����ϵ����ʧ�ܲ�Ӱ����Ҫ����
            }
        }
    }
}