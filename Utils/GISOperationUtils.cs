using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;

namespace TestArcMapAddin2.Utils
{
    /// <summary>
    /// GIS����������
    /// �ṩ�������㡢�ռ�������ֶβ�����GIS���Ĺ���
    /// </summary>
    public static class GISOperationUtils
    {
        /// <summary>
        /// ���Ȼص�ί��
        /// </summary>
        /// <param name="percentage">��ɰٷֱ�</param>
        /// <param name="message">������Ϣ</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// �ü��������
        /// </summary>
        public class ClipResult
        {
            public bool Success { get; set; }
            public string OutputPath { get; set; }
            public int ProcessedFeatureCount { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// Shapefile�򿪽��
        /// </summary>
        public class ShapefileOpenResult
        {
            public IWorkspace Workspace { get; set; }
            public IFeatureClass FeatureClass { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// ����д��������
        /// </summary>
        public class WriteDataResult
        {
            public bool Success { get; set; }
            public string OutputPath { get; set; }
            public int ProcessedFeatureCount { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// Ҫ�����ݽṹ - ���ڴ��ݼ��κ���������
        /// </summary>
        public class FeatureData
        {
            public IGeometry Geometry { get; set; }
            public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
        }

        /// <summary>
        /// �ؼ�������Ϣ
        /// </summary>
        public class CountyProcessingInfo
        {
            public string CountyCode { get; set; }
            public string CountyName { get; set; }
            public IFeatureClass SourceFeatureClass { get; set; }
            public Dictionary<string, string> FieldMappings { get; set; }
            public string WhereClause { get; set; }
            public string ResourceType { get; set; } // "ɭ��", "��ԭ", "ʪ��", ��
        }

        /// <summary>
        /// ��Ҫ��������ֶ�
        /// </summary>
        /// <param name="featureClass">Ҫ����</param>
        /// <param name="fieldName">�ֶ���</param>
        /// <param name="fieldType">�ֶ�����</param>
        /// <param name="length">�ֶγ���</param>
        /// <param name="precision">����</param>
        /// <returns>�Ƿ�ɹ����</returns>
        public static bool AddField(IFeatureClass featureClass, string fieldName, esriFieldType fieldType,
                                  int length = 50, int precision = 0)
        {
            try
            {
                // ����ֶ��Ƿ��Ѵ���
                if (featureClass.FindField(fieldName) != -1)
                {
                    System.Diagnostics.Debug.WriteLine($"�ֶ� {fieldName} �Ѵ���");
                    return true;
                }

                // �����ֶζ���
                IField field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;

                fieldEdit.Name_2 = fieldName;
                fieldEdit.Type_2 = fieldType;

                if (fieldType == esriFieldType.esriFieldTypeString)
                {
                    fieldEdit.Length_2 = length;
                }
                else if (fieldType == esriFieldType.esriFieldTypeDouble && precision > 0)
                {
                    fieldEdit.Precision_2 = precision;
                    fieldEdit.Scale_2 = 2; // С��λ��
                }

                // ����ֶε�Ҫ����
                featureClass.AddField(field);

                System.Diagnostics.Debug.WriteLine($"�ɹ�����ֶ�: {fieldName}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����ֶ� {fieldName} ʧ��: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ����Ҫ�صļ�����������µ�ָ���ֶ�
        /// </summary>
        /// <param name="featureClass">Ҫ����</param>
        /// <param name="areaFieldName">����ֶ���</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>����ɹ���Ҫ������</returns>
        public static int CalculateGeometricArea(IFeatureClass featureClass, string areaFieldName,
                                               ProgressCallback progressCallback = null)
        {
            int successCount = 0;
            IFeatureCursor cursor = null;
            IFeatureCursor updateCursor = null;

            try
            {
                // ȷ������ֶδ���
                int areaFieldIndex = featureClass.FindField(areaFieldName);
                if (areaFieldIndex == -1)
                {
                    if (!AddField(featureClass, areaFieldName, esriFieldType.esriFieldTypeDouble))
                    {
                        throw new Exception($"�޷���������ֶ�: {areaFieldName}");
                    }
                    areaFieldIndex = featureClass.FindField(areaFieldName);
                }

                int totalFeatures = featureClass.FeatureCount(null);
                progressCallback?.Invoke(0, $"��ʼ���� {totalFeatures} ��Ҫ�صļ������...");

                // ���������α�
                updateCursor = featureClass.Update(null, false);
                IFeature feature;
                int processedCount = 0;

                while ((feature = updateCursor.NextFeature()) != null)
                {
                    try
                    {
                        if (feature.Shape != null && !feature.Shape.IsEmpty)
                        {
                            // ���㼸�����
                            IArea areaInterface = feature.Shape as IArea;
                            if (areaInterface != null)
                            {
                                double area = Math.Abs(areaInterface.Area);
                                feature.set_Value(areaFieldIndex, area);
                                updateCursor.UpdateFeature(feature);
                                successCount++;
                            }
                        }

                        processedCount++;

                        // ���½���
                        if (processedCount % 100 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = (processedCount * 100) / totalFeatures;
                            progressCallback?.Invoke(percentage,
                                $"�Ѽ��� {processedCount}/{totalFeatures} ��Ҫ�ص����");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"����Ҫ�����ʱ����: {ex.Message}");
                    }
                    finally
                    {
                        if (feature != null)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"����������: �ɹ� {successCount}/{totalFeatures}");
                return successCount;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"���㼸�����ʱ����: {ex.Message}");
                throw;
            }
            finally
            {
                if (cursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                if (updateCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            }
        }

        /// <summary>
        /// ����Ҫ������
        /// </summary>
        /// <param name="sourceFeature">ԴҪ��</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="targetBuffer">Ŀ�껺����</param>
        /// <param name="targetFeatureClass">Ŀ��Ҫ����</param>
        public static void CopyFeatureAttributes(IFeature sourceFeature, IFeatureClass sourceFeatureClass,
                                               IFeatureBuffer targetBuffer, IFeatureClass targetFeatureClass)
        {
            try
            {
                IFields sourceFields = sourceFeatureClass.Fields;
                IFields targetFields = targetFeatureClass.Fields;

                for (int i = 0; i < sourceFields.FieldCount; i++)
                {
                    IField sourceField = sourceFields.get_Field(i);

                    // ����OID�ͼ����ֶ�
                    if (sourceField.Type == esriFieldType.esriFieldTypeOID ||
                        sourceField.Type == esriFieldType.esriFieldTypeGeometry)
                        continue;

                    // ����Ŀ���ֶ�
                    int targetIndex = targetFeatureClass.FindField(sourceField.Name);
                    if (targetIndex != -1)
                    {
                        try
                        {
                            object value = sourceFeature.get_Value(i);
                            if (value != null && value != DBNull.Value)
                            {
                                targetBuffer.set_Value(targetIndex, value);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"�����ֶ� {sourceField.Name} ʱ����: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����Ҫ������ʱ����: {ex.Message}");
            }
        }

        /// <summary>
        /// ����Ҫ�����ԣ�ʹ���ֶ�ӳ�䣩
        /// </summary>
        /// <param name="sourceFeature">ԴҪ��</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="targetBuffer">Ŀ�껺����</param>
        /// <param name="targetFeatureClass">Ŀ��Ҫ����</param>
        /// <param name="fieldMappings">�ֶ�ӳ�䣨Ŀ���ֶ��� -> Դ�ֶ�����</param>
        public static void CopyFeatureAttributesWithMapping(IFeature sourceFeature, IFeatureClass sourceFeatureClass,
                                                           IFeatureBuffer targetBuffer, IFeatureClass targetFeatureClass,
                                                           Dictionary<string, string> fieldMappings)
        {
            try
            {
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    // ���û���ֶ�ӳ�䣬ʹ��Ĭ�ϵĸ��Ʒ���
                    CopyFeatureAttributes(sourceFeature, sourceFeatureClass, targetBuffer, targetFeatureClass);
                    return;
                }

                foreach (var mapping in fieldMappings)
                {
                    string targetFieldName = mapping.Key;
                    string sourceFieldName = mapping.Value;

                    try
                    {
                        int sourceIndex = sourceFeatureClass.FindField(sourceFieldName);
                        int targetIndex = targetFeatureClass.FindField(targetFieldName);

                        if (sourceIndex != -1 && targetIndex != -1)
                        {
                            object value = sourceFeature.get_Value(sourceIndex);
                            if (value != null && value != DBNull.Value)
                            {
                                targetBuffer.set_Value(targetIndex, value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"�����ֶ�ӳ�� {targetFieldName} <- {sourceFieldName} ʱ����: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ʹ���ֶ�ӳ�临��Ҫ������ʱ����: {ex.Message}");
            }
        }

        /// <summary>
        /// ��shapefile
        /// </summary>
        /// <param name="shapefilePath">shapefile·��</param>
        /// <returns>�򿪽��</returns>
        public static ShapefileOpenResult OpenShapefile(string shapefilePath)
        {
            var result = new ShapefileOpenResult();

            try
            {
                if (!File.Exists(shapefilePath))
                {
                    result.ErrorMessage = $"�ļ�������: {shapefilePath}";
                    return result;
                }

                string directory = System.IO.Path.GetDirectoryName(shapefilePath);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                // ʹ�� Type.GetTypeFromProgID �� Activator.CreateInstance �����������ռ乤��
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory factory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;

                IWorkspace workspace = factory.OpenFromFile(directory, 0);
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);

                result.Workspace = workspace;
                result.FeatureClass = featureClass;
                result.Success = true;

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"��shapefile {shapefilePath} ʱ����: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// ������Shapefileд������
        /// </summary>
        /// <param name="targetShapefilePath">Ŀ��Shapefile·��</param>
        /// <param name="sourceData">Դ�����б����κ����ԣ�</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>д��������</returns>
        public static WriteDataResult WriteDataToExistingShapefile(string targetShapefilePath,
            List<FeatureData> sourceData, ProgressCallback progressCallback = null)
        {
            var result = new WriteDataResult();
            IWorkspace workspace = null;
            IFeatureClass featureClass = null;
            IFeatureCursor insertCursor = null;

            try
            {
                progressCallback?.Invoke(0, "���ڴ�Ŀ��Shapefile...");

                // �����е�Shapefile
                var openResult = OpenShapefile(targetShapefilePath);
                if (!openResult.Success)
                {
                    result.ErrorMessage = openResult.ErrorMessage;
                    return result;
                }

                workspace = openResult.Workspace;
                featureClass = openResult.FeatureClass;

                progressCallback?.Invoke(10, "����׼������д��...");

                // ���������α�
                insertCursor = featureClass.Insert(true);
                int totalFeatures = sourceData.Count;
                int successCount = 0;

                progressCallback?.Invoke(20, $"��ʼд�� {totalFeatures} ��Ҫ��...");

                for (int i = 0; i < totalFeatures; i++)
                {
                    try
                    {
                        var data = sourceData[i];

                        // ����Ҫ�ػ�����
                        IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();

                        // ���ü���
                        if (data.Geometry != null)
                        {
                            featureBuffer.Shape = data.Geometry;
                        }

                        // ��������
                        if (data.Attributes != null)
                        {
                            foreach (var attr in data.Attributes)
                            {
                                int fieldIndex = featureClass.FindField(attr.Key);
                                if (fieldIndex != -1)
                                {
                                    try
                                    {
                                        featureBuffer.set_Value(fieldIndex, attr.Value ?? DBNull.Value);
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"�����ֶ� {attr.Key} ֵʱ����: {ex.Message}");
                                    }
                                }
                            }
                        }

                        // ����Ҫ��
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        // �ͷŻ�����
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(featureBuffer);

                        // ���½���
                        if (i % 50 == 0 || i == totalFeatures - 1)
                        {
                            int percentage = 20 + (i * 70) / totalFeatures;
                            progressCallback?.Invoke(percentage, $"��д�� {i + 1}/{totalFeatures} ��Ҫ��");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"д��� {i + 1} ��Ҫ��ʱ����: {ex.Message}");
                    }
                }

                // �ύ����
                insertCursor.Flush();

                result.Success = true;
                result.ProcessedFeatureCount = successCount;
                result.OutputPath = targetShapefilePath;

                progressCallback?.Invoke(100, $"����д����ɣ��ɹ�д�� {successCount}/{totalFeatures} ��Ҫ��");

                System.Diagnostics.Debug.WriteLine($"�� {targetShapefilePath} �ɹ�д�� {successCount} ��Ҫ��");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"��Shapefileд������ʱ����: {ex.Message}");
                return result;
            }
            finally
            {
                // �ͷ���Դ
                if (insertCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                if (featureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                if (workspace != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
            }
        }

        /// <summary>
        /// ��ԴҪ�����ȡ���ݲ�д�뵽Ŀ��Shapefile
        /// </summary>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="targetShapefilePath">Ŀ��Shapefile·��</param>
        /// <param name="fieldMappings">�ֶ�ӳ�䣨Ŀ���ֶ��� -> Դ�ֶ�����</param>
        /// <param name="whereClause">��ѯ��������ѡ��</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>д��������</returns>
        public static WriteDataResult WriteFeatureClassToShapefile(IFeatureClass sourceFeatureClass,
            string targetShapefilePath, Dictionary<string, string> fieldMappings = null,
            string whereClause = null, ProgressCallback progressCallback = null)
        {
            var result = new WriteDataResult();
            IFeatureCursor sourceCursor = null;
            var featureDataList = new List<FeatureData>();

            try
            {
                progressCallback?.Invoke(0, "���ڶ�ȡԴ����...");

                // ������ѯ������
                IQueryFilter queryFilter = null;
                if (!string.IsNullOrEmpty(whereClause))
                {
                    queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = whereClause;
                }

                // ��ȡԴ����
                sourceCursor = sourceFeatureClass.Search(queryFilter, false);
                IFeature sourceFeature;
                int readCount = 0;

                while ((sourceFeature = sourceCursor.NextFeature()) != null)
                {
                    try
                    {
                        var featureData = new FeatureData();

                        // ���Ƽ���
                        if (sourceFeature.Shape != null)
                        {
                            featureData.Geometry = sourceFeature.ShapeCopy;
                        }

                        // ��������
                        if (fieldMappings != null)
                        {
                            foreach (var mapping in fieldMappings)
                            {
                                string targetField = mapping.Key;
                                string sourceField = mapping.Value;

                                int sourceIndex = sourceFeatureClass.FindField(sourceField);
                                if (sourceIndex != -1)
                                {
                                    object value = sourceFeature.get_Value(sourceIndex);
                                    featureData.Attributes[targetField] = value;
                                }
                            }
                        }
                        else
                        {
                            // ���û���ֶ�ӳ�䣬�������зǼ����ֶ�
                            IFields fields = sourceFeatureClass.Fields;
                            for (int i = 0; i < fields.FieldCount; i++)
                            {
                                IField field = fields.get_Field(i);
                                if (field.Type != esriFieldType.esriFieldTypeOID &&
                                    field.Type != esriFieldType.esriFieldTypeGeometry)
                                {
                                    object value = sourceFeature.get_Value(i);
                                    featureData.Attributes[field.Name] = value;
                                }
                            }
                        }

                        featureDataList.Add(featureData);
                        readCount++;

                        if (readCount % 100 == 0)
                        {
                            progressCallback?.Invoke(readCount % 50, $"�Ѷ�ȡ {readCount} ��Ҫ��...");
                        }
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceFeature);
                    }
                }

                progressCallback?.Invoke(50, $"���ݶ�ȡ��ɣ�����ȡ {readCount} ��Ҫ��");

                // д�뵽Ŀ��Shapefile
                var writeResult = WriteDataToExistingShapefile(targetShapefilePath, featureDataList,
                    (percentage, message) => {
                        int adjustedProgress = 50 + (percentage * 50) / 100;
                        progressCallback?.Invoke(adjustedProgress, message);
                    });

                return writeResult;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"��Ҫ����д��Shapefileʱ����: {ex.Message}");
                return result;
            }
            finally
            {
                if (sourceCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceCursor);
            }
        }

        /// <summary>
        /// �����������ص�����д��
        /// </summary>
        /// <param name="countyDataMappings">�ؼ�����ӳ�䣨�ش��� -> ������Ϣ��</param>
        /// <param name="outputBasePath">�������·��</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>����������</returns>
        public static Dictionary<string, WriteDataResult> BatchWriteCountyData(
            Dictionary<string, CountyProcessingInfo> countyDataMappings,
            string outputBasePath, ProgressCallback progressCallback = null)
        {
            var results = new Dictionary<string, WriteDataResult>();

            try
            {
                int totalCounties = countyDataMappings.Count;
                int processedCounties = 0;

                progressCallback?.Invoke(0, $"��ʼ�������� {totalCounties} ���ص�����д��...");

                foreach (var countyData in countyDataMappings)
                {
                    string countyCode = countyData.Key;
                    var processingInfo = countyData.Value;

                    try
                    {
                        int startProgress = (processedCounties * 100) / totalCounties;
                        int endProgress = ((processedCounties + 1) * 100) / totalCounties;

                        progressCallback?.Invoke(startProgress, $"���ڴ����ش��� {countyCode}...");

                        // �����ؼ����·��
                        string countyName = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyNameFromCode(countyCode);
                        string countyFolderName = $"{countyName}({countyCode})ȫ��������Ȼ��Դ�ʲ�������ݳɹ�";
                        string countyOutputPath = System.IO.Path.Combine(outputBasePath, countyFolderName);

                        // ������ص�����
                        var countyResult = ProcessSingleCountyData(processingInfo, countyOutputPath,
                            (percentage, message) => {
                                int adjustedProgress = startProgress + (percentage * (endProgress - startProgress)) / 100;
                                progressCallback?.Invoke(adjustedProgress, $"[{countyCode}] {message}");
                            });

                        results[countyCode] = countyResult;
                        processedCounties++;

                        if (countyResult.Success)
                        {
                            System.Diagnostics.Debug.WriteLine($"�� {countyCode} ����д��ɹ�");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"�� {countyCode} ����д��ʧ��: {countyResult.ErrorMessage}");
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorResult = new WriteDataResult
                        {
                            Success = false,
                            ErrorMessage = ex.Message
                        };
                        results[countyCode] = errorResult;
                        processedCounties++;

                        System.Diagnostics.Debug.WriteLine($"������ {countyCode} ʱ�����쳣: {ex.Message}");
                    }
                }

                progressCallback?.Invoke(100, "��������д�����");
                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����д���ؼ�����ʱ����: {ex.Message}");
                return results;
            }
        }

        /// <summary>
        /// �������ص�����
        /// </summary>
        /// <param name="processingInfo">������Ϣ</param>
        /// <param name="countyOutputPath">�ؼ����·��</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>������</returns>
        private static WriteDataResult ProcessSingleCountyData(CountyProcessingInfo processingInfo,
            string countyOutputPath, ProgressCallback progressCallback = null)
        {
            try
            {
                // ����Ŀ��Shapefile·��
                string resourcePath = System.IO.Path.Combine(countyOutputPath, "������ݼ�", processingInfo.ResourceType, "�ռ�����");
                string targetShapefileName = GetTargetShapefileName(processingInfo.CountyCode, processingInfo.ResourceType);
                string targetShapefilePath = System.IO.Path.Combine(resourcePath, targetShapefileName + ".shp");

                // ��֤Ŀ��·������
                if (!Directory.Exists(resourcePath))
                {
                    return new WriteDataResult
                    {
                        Success = false,
                        ErrorMessage = $"Ŀ��·��������: {resourcePath}"
                    };
                }

                // ��֤Ŀ��Shapefile����
                if (!File.Exists(targetShapefilePath))
                {
                    return new WriteDataResult
                    {
                        Success = false,
                        ErrorMessage = $"Ŀ��Shapefile������: {targetShapefilePath}"
                    };
                }

                // ִ������д��
                return WriteFeatureClassToShapefile(
                    processingInfo.SourceFeatureClass,
                    targetShapefilePath,
                    processingInfo.FieldMappings,
                    processingInfo.WhereClause,
                    progressCallback);
            }
            catch (Exception ex)
            {
                return new WriteDataResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// �����ش������Դ���ͻ�ȡĿ��Shapefile����
        /// </summary>
        /// <param name="countyCode">�ش���</param>
        /// <param name="resourceType">��Դ����</param>
        /// <returns>Shapefile����</returns>
        private static string GetTargetShapefileName(string countyCode, string resourceType)
        {
            switch (resourceType)
            {
                case "ɭ��":
                    return $"({countyCode})SLZYZC";
                case "��ԭ":
                    return $"({countyCode})CYZYZC";
                case "ʪ��":
                    return $"({countyCode})SDZYZC";
                case "�������õ�":
                    return $"({countyCode})CZCDYDQC";
                default:
                    return $"({countyCode}){resourceType}";
            }
        }

        /// <summary>
        /// ɾ����ʱ�ļ�
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        public static void DeleteTemporaryFiles(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    // ɾ��shapefile��ص������ļ�
                    string basePath = System.IO.Path.ChangeExtension(filePath, null);
                    string[] extensions = { ".shp", ".shx", ".dbf", ".prj", ".cpg", ".qpj", ".sbn", ".sbx" };

                    foreach (string ext in extensions)
                    {
                        string file = basePath + ext;
                        if (File.Exists(file))
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"ɾ���ļ� {file} ʱ����: {ex.Message}");
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"��ɾ����ʱ�ļ�: {filePath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ɾ����ʱ�ļ� {filePath} ʱ����: {ex.Message}");
            }
        }

        /// <summary>
        /// ɾ�������ʱ�ļ�
        /// </summary>
        /// <param name="filePaths">�ļ�·���б�</param>
        public static void DeleteTemporaryFiles(IEnumerable<string> filePaths)
        {
            foreach (string filePath in filePaths)
            {
                DeleteTemporaryFiles(filePath);
            }
        }

        /// <summary>
        /// ��֤shapefile�ļ�����Ч��
        /// </summary>
        /// <param name="shapefilePath">shapefile·��</param>
        /// <returns>�Ƿ���Ч</returns>
        public static bool ValidateShapefile(string shapefilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(shapefilePath) || !File.Exists(shapefilePath))
                {
                    return false;
                }

                // �������shapefile����ļ�
                string basePath = System.IO.Path.ChangeExtension(shapefilePath, null);
                string[] requiredExtensions = { ".shp", ".shx", ".dbf" };

                foreach (string ext in requiredExtensions)
                {
                    if (!File.Exists(basePath + ext))
                    {
                        return false;
                    }
                }

                // ���Դ�shapefile��֤��������
                var openResult = OpenShapefile(shapefilePath);
                if (openResult.Success && openResult.FeatureClass != null)
                {
                    try
                    {
                        // ���Ի�ȡҪ����������֤����������
                        int count = openResult.FeatureClass.FeatureCount(null);
                        return true;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(openResult.FeatureClass);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(openResult.Workspace);
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��֤shapefile {shapefilePath} ʱ����: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ��ȡҪ������ֶ���Ϣ
        /// </summary>
        /// <param name="featureClass">Ҫ����</param>
        /// <returns>�ֶ���Ϣ�б�</returns>
        public static List<string> GetFeatureClassFields(IFeatureClass featureClass)
        {
            var fieldNames = new List<string>();

            try
            {
                IFields fields = featureClass.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    fieldNames.Add(field.Name);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡҪ�����ֶ���Ϣʱ����: {ex.Message}");
            }

            return fieldNames;
        }

        /// <summary>
        /// ��������CZCDYDQC����
        /// </summary>
        /// <param name="countyFiles">�ؼ��ļ���Ϣ�б�</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>�������б�</returns>
        public static List<CZCDYDProcessor.ProcessingResult> BatchProcessCZCDYDQC(
            List<CZCDYDProcessor.CountyFiles> countyFiles, ProgressCallback progressCallback = null)
        {
            var results = new List<CZCDYDProcessor.ProcessingResult>();
            var processor = new CZCDYDProcessor();

            try
            {
                int totalCounties = countyFiles.Count;
                progressCallback?.Invoke(0, $"��ʼ�������� {totalCounties} ���ص�CZCDYDQC����...");

                for (int i = 0; i < totalCounties; i++)
                {
                    var countyFile = countyFiles[i];
                    int startProgress = (i * 100) / totalCounties;
                    int endProgress = ((i + 1) * 100) / totalCounties;

                    progressCallback?.Invoke(startProgress, $"���ڴ����ش��� {countyFile.CountyCode}...");

                    // �����ؼ����Ȼص�
                    CZCDYDProcessor.ProgressCallback countyProgress = (percentage, message) =>
                    {
                        int adjustedProgress = startProgress + (percentage * (endProgress - startProgress)) / 100;
                        progressCallback?.Invoke(adjustedProgress, $"[{countyFile.CountyCode}] {message}");
                    };

                    var result = processor.ProcessCountyCZCDYDQC(countyFile, countyProgress);
                    results.Add(result);

                    if (!result.Success)
                    {
                        System.Diagnostics.Debug.WriteLine($"������ {countyFile.CountyCode} ʧ��: {result.ErrorMessage}");
                    }
                }

                progressCallback?.Invoke(100, "�����������");
                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��������CZCDYDQC����ʱ����: {ex.Message}");
                return results;
            }
        }
    }
}