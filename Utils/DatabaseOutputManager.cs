using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// ���ݿ���������� - ��������д�뵽button1���������ݿ���
    /// </summary>
    public class DatabaseOutputManager
    {
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// ���������ؼ�������������ݿ�LCXZGX��
        /// </summary>
        /// <param name="processedFeatures">������Ҫ���б�</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="countyName">����</param>
        /// <param name="outputGDBPath">������ݿ�·��</param>
        /// <param name="fieldMappings">�ֶ�ӳ������</param>
        /// <param name="progressCallback">���Ȼص�</param>
        public void OutputToDatabase(
            List<IFeature> processedFeatures,
            IFeatureClass sourceFeatureClass,
            string countyName,
            string outputGDBPath,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null)
        {
            if (processedFeatures == null || processedFeatures.Count == 0)
            {
                throw new ArgumentException("������Ҫ���б���Ϊ��");
            }

            if (string.IsNullOrEmpty(countyName))
            {
                throw new ArgumentException("��������Ϊ��");
            }

            if (string.IsNullOrEmpty(outputGDBPath))
            {
                throw new ArgumentException("������ݿ�·������Ϊ��");
            }

            progressCallback?.Invoke(5, $"�������ӵ�{countyName}���ݿ�...");

            IWorkspace targetWorkspace = null;
            IFeatureClass lcxzgxFeatureClass = null;

            try
            {
                // ���ؼ����ݿ�
                targetWorkspace = OpenCountyDatabase(outputGDBPath, countyName);
                if (targetWorkspace == null)
                {
                    throw new Exception($"�޷���{countyName}�����ݿ�");
                }

                progressCallback?.Invoke(15, $"���ڷ���{countyName}��LCXZGX��...");

                // ��ȡLCXZGXҪ����
                lcxzgxFeatureClass = GetLCXZGXFeatureClass(targetWorkspace);
                if (lcxzgxFeatureClass == null)
                {
                    throw new Exception($"�޷��ҵ�{countyName}���ݿ��е�LCXZGX��");
                }

                progressCallback?.Invoke(25, $"��ʼ��{countyName}��LCXZGX��д������...");

                // ��������Ҫ��д��LCXZGX��
                WriteFeaturesToDatabase(processedFeatures, sourceFeatureClass, lcxzgxFeatureClass, 
                    fieldMappings, countyName, progressCallback);

                progressCallback?.Invoke(100, $"�ɹ��� {processedFeatures.Count} ��Ҫ��д�뵽{countyName}��LCXZGX��");

                System.Diagnostics.Debug.WriteLine($"��{countyName}�������ѳɹ�д�����ݿ�LCXZGX��");
            }
            finally
            {
                // �ͷ���Դ
                if (lcxzgxFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lcxzgxFeatureClass);
                }
                if (targetWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(targetWorkspace);
                }
            }
        }

        /// <summary>
        /// ���ؼ����ݿ�
        /// </summary>
        private IWorkspace OpenCountyDatabase(string outputGDBPath, string countyName)
        {
            try
            {
                // �����ؼ����ݿ�·��
                string countyGDBPath = Path.Combine(outputGDBPath, countyName, countyName + ".gdb");
                
                if (!Directory.Exists(Path.GetDirectoryName(countyGDBPath)))
                {
                    throw new DirectoryNotFoundException($"�ؼ�Ŀ¼������: {Path.GetDirectoryName(countyGDBPath)}");
                }

                if (!Directory.Exists(countyGDBPath))
                {
                    throw new DirectoryNotFoundException($"�ؼ����ݿⲻ����: {countyGDBPath}");
                }

                // ʹ��ProgID��ȡFile Geodatabase����
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // �򿪹����ռ�
                IWorkspace workspace = workspaceFactory.OpenFromFile(countyGDBPath, 0);
                
                System.Diagnostics.Debug.WriteLine($"�ɹ���{countyName}�����ݿ�: {countyGDBPath}");
                return workspace;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��{countyName}���ݿ�ʱ����: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// ��ȡLCXZGXҪ����
        /// </summary>
        private IFeatureClass GetLCXZGXFeatureClass(IWorkspace workspace)
        {
            try
            {
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;
                IFeatureClass lcxzgxFeatureClass = featureWorkspace.OpenFeatureClass("LCXZGX");
                
                System.Diagnostics.Debug.WriteLine("�ɹ���ȡLCXZGXҪ����");
                return lcxzgxFeatureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡLCXZGXҪ����ʱ����: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// ��Ҫ��д�����ݿ�
        /// </summary>
        private void WriteFeaturesToDatabase(
            List<IFeature> sourceFeatures,
            IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            ProgressCallback progressCallback)
        {
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);

            try
            {
                int totalFeatures = sourceFeatures.Count;
                int processedCount = 0;
                int successCount = 0;
                int errorCount = 0;

                System.Diagnostics.Debug.WriteLine($"��ʼ��{countyName}��LCXZGX�����{totalFeatures}��Ҫ��");

                foreach (IFeature sourceFeature in sourceFeatures)
                {
                    try
                    {
                        // ���Ƽ���
                        if (sourceFeature.Shape != null)
                        {
                            featureBuffer.Shape = sourceFeature.ShapeCopy;
                        }

                        // ��������ֵ�������ֶ�ӳ��
                        CopyFeatureAttributes(sourceFeature, sourceFeatureClass, featureBuffer, 
                            targetFeatureClass, fieldMappings, countyName);

                        // ����Ҫ��
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        processedCount++;

                        // ���½���
                        if (processedCount % 10 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = 25 + (int)((processedCount / (double)totalFeatures) * 70);
                            progressCallback?.Invoke(percentage,
                                $"����д��{countyName}��LCXZGX��... ({processedCount}/{totalFeatures})");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"����{countyName}Ҫ��ʱ����: {ex.Message}");
                        // ����������һ��Ҫ��
                    }
                }

                // �ύ�������
                insertCursor.Flush();
                
                System.Diagnostics.Debug.WriteLine($"{countyName}����д�����: �ɹ�{successCount}��, ʧ��{errorCount}��");
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
        /// ����Ҫ�����Բ������ֶ�ӳ��
        /// </summary>
        private void CopyFeatureAttributes(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            IFeatureBuffer targetFeatureBuffer,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName)
        {
            // Ĭ���ֶ�ӳ�䣨���û���ṩ�Զ���ӳ�䣩
            if (fieldMappings == null || fieldMappings.Count == 0)
            {
                fieldMappings = GetDefaultFieldMappings();
            }

            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;
                string sourceFieldName = mapping.Value;

                try
                {
                    int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                    int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);

                    if (sourceFieldIndex != -1 && targetFieldIndex != -1)
                    {
                        object sourceValue = sourceFeature.get_Value(sourceFieldIndex);

                        // ��������ֵת��
                        object targetValue = ConvertFieldValueForDatabase(sourceValue, targetFieldName, 
                            sourceFieldName, countyName);

                        targetFeatureBuffer.set_Value(targetFieldIndex, targetValue);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"����{countyName}�ֶ�{targetFieldName}ʱ����: {ex.Message}");
                    // �������������ֶ�
                }
            }
        }

        /// <summary>
        /// ��ȡĬ���ֶ�ӳ��
        /// </summary>
        private Dictionary<string, string> GetDefaultFieldMappings()
        {
            return new Dictionary<string, string>
            {
                { "BSM", "BSM" },           // ��ʶ��
                { "YSDM", "YSDM" },         // Ҫ�ش���  
                { "TBYBH", "TBYBH" },       // ͼ��Ԥ���
                { "TBBH", "TBBH" },         // ͼ�߱��
                { "ZLDWDM", "ZLDWDM" },     // ���䵥λ����
                { "ZLDWMC", "ZLDWMC" },     // ���䵥λ����
                { "DLBM", "DLBM" },         // �������
                { "DLMC", "DLMC" },         // ��������
                { "QSXZ", "QSXZ" },         // Ȩ������
                { "QSDWDM", "QSDWDM" },     // Ȩ����λ����
                { "QSDWMC", "QSDWMC" },     // Ȩ����λ����
                { "TBMJ", "TBMJ" },         // ͼ�����
                { "KCDLBM", "KCDLBM" },     // �ƴε������
                { "KCDLMC", "KCDLMC" },     // �ƴε�������
                { "KCXS", "KCXS" },         // �ƴ�С��
                { "TBDH", "TBDH" },         // ͼ�ߴ���
                { "BZ", "BZ" }              // ��ע
            };
        }

        /// <summary>
        /// ת���ֶ�ֵ���������ݿ�
        /// </summary>
        private object ConvertFieldValueForDatabase(object sourceValue, string targetFieldName, 
            string sourceFieldName, string countyName)
        {
            if (sourceValue == null || sourceValue == DBNull.Value)
            {
                return null;
            }

            try
            {
                // ����LCXZGX����ֶ�Ҫ���������ת��
                switch (targetFieldName.ToUpper())
                {
                    case "QSXZ":  // Ȩ������ת��
                        return ConvertPropertyRights(sourceValue);
                        
                    case "DLMC":  // ��������ת��
                        return ConvertLandTypeName(sourceValue);
                        
                    case "TBMJ":  // ͼ���������
                        return ConvertAreaValue(sourceValue);
                        
                    case "ZLDWMC": // ���䵥λ���� - ʹ������
                        return string.IsNullOrEmpty(sourceValue?.ToString()) ? countyName : sourceValue.ToString();
                        
                    case "QSDWMC": // Ȩ����λ����
                        return ConvertPropertyOwner(sourceValue, countyName);
                        
                    default:
                        return sourceValue;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ת��{countyName}�ֶ�{targetFieldName}ֵʱ����: {ex.Message}");
                return sourceValue; // ת��ʧ��ʱ����ԭֵ
            }
        }

        /// <summary>
        /// ת��Ȩ������
        /// </summary>
        private string ConvertPropertyRights(object value)
        {
            string strValue = value?.ToString() ?? "";
            switch (strValue)
            {
                case "1":
                case "20":
                    return "����";
                case "2":
                case "30":
                    return "����";
                case "3":
                case "40":
                    return "����";
                default:
                    return strValue;
            }
        }

        /// <summary>
        /// ת����������
        /// </summary>
        private string ConvertLandTypeName(object value)
        {
            string strValue = value?.ToString() ?? "";
            if (strValue.StartsWith("03"))
            {
                return "�ֵ�";
            }
            else if (strValue.StartsWith("04"))
            {
                return "�ݵ�";
            }
            else if (strValue.StartsWith("11"))
            {
                return "ʪ��";
            }
            return strValue;
        }

        /// <summary>
        /// ת�����ֵ
        /// </summary>
        private double? ConvertAreaValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            if (value is double || value is float || value is decimal)
            {
                return Convert.ToDouble(value);
            }
            else if (double.TryParse(value.ToString(), out double areaValue))
            {
                return areaValue;
            }
            return null;
        }

        /// <summary>
        /// ת��Ȩ����λ����
        /// </summary>
        private string ConvertPropertyOwner(object value, string countyName)
        {
            string strValue = value?.ToString() ?? "";
            if (string.IsNullOrEmpty(strValue))
            {
                return $"{countyName}��������"; // Ĭ��Ȩ����λ
            }
            return strValue;
        }

        /// <summary>
        /// �����������ص����ݵ����Ե����ݿ�
        /// </summary>
        public void BatchOutputToDatabase(
            Dictionary<string, List<IFeature>> countyFeaturesMap,
            IFeatureClass sourceFeatureClass,
            string outputGDBPath,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null)
        {
            if (countyFeaturesMap == null || countyFeaturesMap.Count == 0)
            {
                throw new ArgumentException("�ؼ�Ҫ��ӳ�䲻��Ϊ��");
            }

            int totalCounties = countyFeaturesMap.Count;
            int processedCounties = 0;

            foreach (var countyData in countyFeaturesMap)
            {
                string countyName = countyData.Key;
                List<IFeature> countyFeatures = countyData.Value;

                try
                {
                    progressCallback?.Invoke(
                        (processedCounties * 100) / totalCounties,
                        $"���ڴ�����: {countyName} ({processedCounties + 1}/{totalCounties})");

                    OutputToDatabase(countyFeatures, sourceFeatureClass, countyName, outputGDBPath, 
                        fieldMappings, null); // �������ӽ��Ȼص��Ա������

                    processedCounties++;
                    
                    System.Diagnostics.Debug.WriteLine($"��{countyName}���������� ({processedCounties}/{totalCounties})");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"�����{countyName}����ʱ����: {ex.Message}");
                    // ����������һ����
                    processedCounties++;
                }
            }

            progressCallback?.Invoke(100, $"�����ص����������� ({processedCounties}/{totalCounties})");
        }
    }
}