using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// �����ת�������� - ���ؼ�LCXZGX��ת��ΪSLZYZC��
    /// SLZYZC����ɭ����Դ����ɹ������ڴ洢ת������ֵص�������
    /// </summary>
    public class Convert2ResultTable
    {
        /// <summary>
        /// ���Ȼص�ί�� - ������UI�㱨�洦�����
        /// </summary>
        /// <param name="percentage">��ɰٷֱ� (0-100)</param>
        /// <param name="message">��ǰ����������Ϣ</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// ���ؼ�LCXZGX��ת��ΪSLZYZC��
        /// ִ������ת�����ֶ�ӳ���ҵ�������
        /// </summary>
        /// <param name="countyName">����</param>
        /// <param name="databasePath">�ؼ����ݿ�·��</param>
        /// <param name="fieldMappings">�ֶ�ӳ�����ã�SLZYZC�ֶ��� -> LCXZGX�ֶ�����</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>ת���Ƿ�ɹ�</returns>
        public bool ConvertLCXZGXToSLZYZC(
            string countyName,
            string databasePath,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null)
        {
            // ������֤ - ȷ���������ݵ���Ч��
            if (string.IsNullOrEmpty(countyName))
            {
                throw new ArgumentException("��������Ϊ��");
            }

            if (string.IsNullOrEmpty(databasePath))
            {
                throw new ArgumentException("���ݿ�·������Ϊ��");
            }

            progressCallback?.Invoke(5, $"�������ӵ�{countyName}���ݿ�...");

            // COM�������� - ��Ҫ��finally������ʽ�ͷ��Ա����ڴ�й©
            IWorkspace workspace = null;
            IFeatureClass lcxzgxFeatureClass = null;
            IFeatureClass slzyzcFeatureClass = null;
            IFeatureClass czkfbjFeatureClass = null;

            try
            {
                // ���ؼ����ݿ�
                workspace = OpenCountyDatabase(databasePath, countyName);
                if (workspace == null)
                {
                    throw new Exception($"�޷���{countyName}�����ݿ�");
                }

                progressCallback?.Invoke(15, $"���ڷ���{countyName}��LCXZGX��...");

                // ��ȡԴ�� - LCXZGXҪ����
                lcxzgxFeatureClass = GetFeatureClass(workspace, "LCXZGX");
                if (lcxzgxFeatureClass == null)
                {
                    throw new Exception($"�޷��ҵ�{countyName}���ݿ��е�LCXZGX��");
                }

                progressCallback?.Invoke(25, $"���ڷ���{countyName}��SLZYZC��...");

                // ��ȡĿ��� - SLZYZCҪ����
                slzyzcFeatureClass = GetFeatureClass(workspace, "SLZYZC");
                if (slzyzcFeatureClass == null)
                {
                    throw new Exception($"�޷��ҵ�{countyName}���ݿ��е�SLZYZC��");
                }

                progressCallback?.Invoke(30, $"���ڷ���{countyName}�ĳ��򿪷��߽�����...");

                // ��ȡ���򿪷��߽�Ҫ���� - CZKFBJ
                try
                {
                    czkfbjFeatureClass = GetFeatureClass(workspace, "CZKFBJ");
                    if (czkfbjFeatureClass == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"����: δ�ҵ�{countyName}��CZKFBJ��CZKFBJMJ������Ϊ0");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"��ȡ{countyName}��CZKFBJ��ʱ����: {ex.Message}��CZKFBJMJ������Ϊ0");
                }

                progressCallback?.Invoke(35, $"��ʼת��{countyName}������...");

                // ִ������ת������
                int convertedCount = ConvertFeatures(
                    lcxzgxFeatureClass,
                    slzyzcFeatureClass,
                    czkfbjFeatureClass,
                    fieldMappings,
                    countyName,
                    progressCallback);

                progressCallback?.Invoke(100, $"�ɹ�ת�� {convertedCount} ��Ҫ�ص�{countyName}��SLZYZC��");

                System.Diagnostics.Debug.WriteLine($"��{countyName}��LCXZGX�����ѳɹ�ת��ΪSLZYZC��");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ת��{countyName}����ʱ����: {ex.Message}");
                progressCallback?.Invoke(0, $"ת��{countyName}����ʧ��: {ex.Message}");
                throw;
            }
            finally
            {
                // ��Ҫ���ͷ�ArcGIS COM���󣬷�ֹ�ڴ�й©
                // ArcGIS COM������Ҫ��ʽ�ͷţ�����ᵼ���ڴ�ռ�ó�������
                if (lcxzgxFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lcxzgxFeatureClass);
                }
                if (slzyzcFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(slzyzcFeatureClass);
                }
                if (czkfbjFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjFeatureClass);
                }
                if (workspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
                }
            }
        }

        /// <summary>
        /// ����ת������ص�LCXZGX��ΪSLZYZC��
        /// �ṩ�������ܣ���߶������ݴ����Ч��
        /// </summary>
        /// <param name="countyDatabasePaths">�ؼ����ݿ�·��ӳ�䣨���� -> ���ݿ�·����</param>
        /// <param name="fieldMappings">�ֶ�ӳ������</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>����ת����������� -> �Ƿ�ɹ���</returns>
        public Dictionary<string, bool> BatchConvertLCXZGXToSLZYZC(
            Dictionary<string, string> countyDatabasePaths,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null)
        {
            if (countyDatabasePaths == null || countyDatabasePaths.Count == 0)
            {
                throw new ArgumentException("�ؼ����ݿ�·��ӳ�䲻��Ϊ��");
            }

            var results = new Dictionary<string, bool>();
            int totalCounties = countyDatabasePaths.Count;
            int processedCounties = 0;

            progressCallback?.Invoke(0, $"��ʼ����ת��{totalCounties}���ص�����...");

            // ����ÿ���ص����ݽ���ת��
            foreach (var countyData in countyDatabasePaths)
            {
                string countyName = countyData.Key;
                string databasePath = countyData.Value;

                try
                {
                    // ����������� - �����Ѵ����������
                    int overallProgress = (processedCounties * 100) / totalCounties;
                    progressCallback?.Invoke(overallProgress, $"����ת����: {countyName} ({processedCounties + 1}/{totalCounties})");

                    // Ϊÿ����ִ��ת��
                    // ע�⣺�������ӽ��Ȼص��Ա�����ȱ������
                    bool success = ConvertLCXZGXToSLZYZC(countyName, databasePath, fieldMappings, null);
                    results[countyName] = success;

                    processedCounties++;

                    System.Diagnostics.Debug.WriteLine($"��{countyName}����ת����� ({processedCounties}/{totalCounties})");
                }
                catch (Exception ex)
                {
                    // ��������ԣ���¼���󵫼�������������
                    // ��������ȷ��һ���صĴ��󲻻�Ӱ�������ص����ݴ���
                    System.Diagnostics.Debug.WriteLine($"ת����{countyName}����ʱ����: {ex.Message}");
                    results[countyName] = false;
                    processedCounties++;
                }
            }

            progressCallback?.Invoke(100, $"�����ص�����ת����� ({processedCounties}/{totalCounties})");
            return results;
        }

        /// <summary>
        /// ���ؼ����ݿ�
        /// ʹ��ArcGIS File Geodatabase�����������ݿ�����
        /// </summary>
        /// <param name="basePath">���ݿ����·��</param>
        /// <param name="countyName">����</param>
        /// <returns>���ݿ⹤���ռ�ӿ�</returns>
        private IWorkspace OpenCountyDatabase(string basePath, string countyName)
        {
            try
            {
                // �����ؼ����ݿ�·�� - ��׼·���ṹ������·��\����\����.gdb
                string countyGDBPath = System.IO.Path.Combine(basePath, countyName, countyName + ".gdb");

                // ��֤���ݿ�·���Ĵ�����
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(countyGDBPath)))
                {
                    throw new DirectoryNotFoundException($"�ؼ�Ŀ¼������: {System.IO.Path.GetDirectoryName(countyGDBPath)}");
                }

                if (!Directory.Exists(countyGDBPath))
                {
                    throw new DirectoryNotFoundException($"�ؼ����ݿⲻ����: {countyGDBPath}");
                }

                // ʹ��ProgID����File Geodatabase����
                // ����ArcGIS COM����ı�׼������ʽ
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // �򿪹����ռ� - ����0��ʾ�Զ�дģʽ��
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
        /// ��ȡָ�����Ƶ�Ҫ����
        /// ͨ�÷����������ڻ�ȡLCXZGX��SLZYZC��Ҫ����
        /// </summary>
        /// <param name="workspace">���ݿ⹤���ռ�</param>
        /// <param name="featureClassName">Ҫ��������</param>
        /// <returns>Ҫ����ӿ�</returns>
        private IFeatureClass GetFeatureClass(IWorkspace workspace, string featureClassName)
        {
            try
            {
                // �������ռ�ת��ΪҪ�ع����ռ��Է���Ҫ����
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

                // ��ָ����Ҫ����
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(featureClassName);

                System.Diagnostics.Debug.WriteLine($"�ɹ���ȡ{featureClassName}Ҫ����");
                return featureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡ{featureClassName}Ҫ����ʱ����: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// ִ��Ҫ��ת������
        /// ��LCXZGX���ȡ���ݣ�ת����д��SLZYZC��
        /// </summary>
        /// <param name="sourceFeatureClass">ԴҪ���ࣨLCXZGX��</param>
        /// <param name="targetFeatureClass">Ŀ��Ҫ���ࣨSLZYZC��</param>
        /// <param name="czkfbjFeatureClass">���򿪷��߽�Ҫ����</param>
        /// <param name="fieldMappings">�ֶ�ӳ������</param>
        /// <param name="countyName">����</param>
        /// <param name="progressCallback">���Ȼص�</param>
        /// <returns>ת����Ҫ������</returns>
        private int ConvertFeatures(
            IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass,
            IFeatureClass czkfbjFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            ProgressCallback progressCallback)
        {
            // ����Ҫ�ػ������Ͳ����α�
            // ʹ����������ģʽ(true����)��߲�������
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);
            IFeatureCursor sourceCursor = null;

            try
            {
                // ��ȡԴ��������
                int totalFeatures = sourceFeatureClass.FeatureCount(null);
                int processedCount = 0;
                int successCount = 0;
                int errorCount = 0;

                System.Diagnostics.Debug.WriteLine($"��ʼת��{countyName}�����ݣ�{totalFeatures}��Ҫ�ش�LCXZGX��SLZYZC");

                // ������ѯ�α��ȡ����ԴҪ��
                sourceCursor = sourceFeatureClass.Search(null, false);
                IFeature sourceFeature;

                // ���û���ṩ�Զ���ӳ�䣬ʹ��Ĭ�ϵ��ֶ�ӳ������
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    fieldMappings = CreateXZ2SLZYZCFieldsMap();
                }

                // ��ȡCZKFBJMJ�ֶε�����
                int czkfbjmjIndex = targetFeatureClass.FindField("CZKFBJMJ");
                if (czkfbjmjIndex == -1)
                {
                    System.Diagnostics.Debug.WriteLine($"����: Ŀ�����δ�ҵ�CZKFBJMJ�ֶ�");
                }

                // ��ȡȨ�������ֶε�����
                int qsxzIndex = sourceFeatureClass.FindField("qsxz");
                if (qsxzIndex == -1)
                {
                    // ���Ա����ֶ���
                    qsxzIndex = sourceFeatureClass.FindField("QSXZ");
                }

                // �������Ҫ��
                while ((sourceFeature = sourceCursor.NextFeature()) != null)
                {
                    try
                    {
                        // ���Ƽ��ζ��� - ʹ��ShapeCopy�������εĸ���
                        if (sourceFeature.Shape != null)
                        {
                            featureBuffer.Shape = sourceFeature.ShapeCopy;
                        }

                        // ��������ֵ��ִ���ֶ�ӳ��ת��
                        CopyAndConvertFeatureAttributes(
                            sourceFeature,
                            sourceFeatureClass,
                            featureBuffer,
                            targetFeatureClass,
                            fieldMappings,
                            countyName);

                        // ����CZKFBJMJ�ֶ� - ���㼯����������򿪷��߽�Ľ������
                        if (czkfbjmjIndex != -1)
                        {
                            double intersectionArea = 0;

                            // �ж��Ƿ�Ϊ��������
                            bool isCollectiveLand = false;
                            if (qsxzIndex != -1)
                            {
                                object qsxzValue = sourceFeature.get_Value(qsxzIndex);
                                string qsxzStr = qsxzValue?.ToString() ?? "";

                                // ����Ƿ�Ϊ�������صĶ��ֿ��ܱ���
                                isCollectiveLand = qsxzStr == "2" || qsxzStr == "30" ||
                                                  qsxzStr.Contains("����") || qsxzStr.Contains("����");
                            }

                            // ����Ǽ��������ҳ��򿪷��߽�Ҫ������ڣ����㽻�����
                            if (isCollectiveLand && czkfbjFeatureClass != null && sourceFeature.Shape != null)
                            {
                                intersectionArea = CalculateIntersectionArea(sourceFeature.Shape, czkfbjFeatureClass);
                            }

                            // ����CZKFBJMJ�ֶ�ֵ
                            featureBuffer.set_Value(czkfbjmjIndex, intersectionArea);
                        }

                        // ִ��Ҫ�ز������
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        processedCount++;

                        // ���ڸ��½��� - ÿ50��Ҫ�ػ����һ��Ҫ��ʱ�������
                        // ���ȷ�Χ��35%-95%��Ϊ���ղ�������5%
                        if (processedCount % 50 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = 35 + (int)((processedCount / (double)totalFeatures) * 60);
                            progressCallback?.Invoke(percentage,
                                $"����ת��{countyName}������... ({processedCount}/{totalFeatures})");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"ת��{countyName}Ҫ��ʱ����: {ex.Message}");
                        // ��������¼���󵫼���������һ��Ҫ��
                        // ��������������ݵĳɹ�ת����
                    }
                    finally
                    {
                        // �ͷŵ�ǰҪ�ض���
                        if (sourceFeature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceFeature);
                        }
                    }
                }

                // �ύ���в�����������ݿ�
                // Flushȷ�����л���Ĳ�������д�����
                insertCursor.Flush();

                System.Diagnostics.Debug.WriteLine($"{countyName}����ת�����: �ɹ�{successCount}��, ʧ��{errorCount}��");
                return successCount;
            }
            finally
            {
                // ��Ҫ���ͷ�ArcGIS COM����
                // �α�ͻ���������COM���󣬱�����ʽ�ͷ�
                if (sourceCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceCursor);
                }
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
        /// ���㼸�ζ�������򿪷��߽�Ľ������
        /// </summary>
        /// <param name="geometry">Ҫ����ļ��ζ���</param>
        /// <param name="czkfbjFeatureClass">���򿪷��߽�Ҫ����</param>
        /// <returns>���������ƽ���ף�</returns>
        private double CalculateIntersectionArea(IGeometry geometry, IFeatureClass czkfbjFeatureClass)
        {
            if (geometry == null || czkfbjFeatureClass == null)
            {
                return 0;
            }

            double totalIntersectionArea = 0;
            IFeatureCursor czkfbjCursor = null;

            try
            {
                // �����ռ��ѯ��������ֻ��ѯ�뵱ǰ���ζ����ཻ�ĳ��򿪷��߽�  
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = geometry;
                spatialFilter.GeometryField = czkfbjFeatureClass.ShapeFieldName;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                // ��ѯ�ཻ�ĳ��򿪷��߽�Ҫ��  
                czkfbjCursor = czkfbjFeatureClass.Search(spatialFilter, false);
                IFeature czkfbjFeature;

                // �ۼ������ཻ��������  
                while ((czkfbjFeature = czkfbjCursor.NextFeature()) != null)
                {
                    try
                    {
                        if (czkfbjFeature.Shape != null)
                        {
                            // ���㽻������  
                            ITopologicalOperator topoOperator = (ITopologicalOperator)geometry;
                            IGeometry intersectionGeometry = topoOperator.Intersect(czkfbjFeature.Shape, esriGeometryDimension.esriGeometry2Dimension);

                            // ���㽻�����  
                            if (intersectionGeometry != null && !intersectionGeometry.IsEmpty)
                            {
                                IArea area = (IArea)intersectionGeometry;
                                totalIntersectionArea += area.Area;

                                // �ͷŽ������ζ���  
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(intersectionGeometry);
                            }
                        }
                    }
                    finally
                    {
                        // �ͷŵ�ǰ���򿪷��߽�Ҫ��  
                        if (czkfbjFeature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjFeature);
                        }
                    }
                }

                return totalIntersectionArea;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"���㽻�����ʱ����: {ex.Message}");
                return 0;
            }
            finally
            {
                // �ͷ��α����  
                if (czkfbjCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjCursor);
                }
            }
        }

        /// <summary>
        /// ����Ҫ�����Բ������ֶ�ӳ��ת��
        /// ����LCXZGX��SLZYZC����ֶ�ӳ�������ת��
        /// </summary>
        /// <param name="sourceFeature">ԴҪ�أ�LCXZGX��</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="targetFeatureBuffer">Ŀ��Ҫ�ػ�������SLZYZC��</param>
        /// <param name="targetFeatureClass">Ŀ��Ҫ����</param>
        /// <param name="fieldMappings">�ֶ�ӳ������</param>
        /// <param name="countyName">����</param>
        private void CopyAndConvertFeatureAttributes(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            IFeatureBuffer targetFeatureBuffer,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName)
        {
            // ���������ֶ�ӳ��������ݸ��ƺ�ת��
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;    // SLZYZC���ֶ���
                string sourceFieldName = mapping.Value;  // LCXZGX���ֶ���

                try
                {
                    // ��ȡĿ���ֶε�����
                    int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);
                    if (targetFieldIndex == -1)
                    {
                        System.Diagnostics.Debug.WriteLine($"Ŀ���ֶ� {targetFieldName} ��SLZYZC���в�����");
                        continue;
                    }

                    // ����������ֶ�ӳ�����
                    object targetValue = ProcessSpecialFieldMapping(
                        sourceFeature,
                        sourceFeatureClass,
                        targetFieldName,
                        sourceFieldName,
                        countyName);

                    if (targetValue != null)
                    {
                        targetFeatureBuffer.set_Value(targetFieldIndex, targetValue);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ת��{countyName}�ֶ�{targetFieldName}ʱ����: {ex.Message}");
                    // �ֶ�ת������Ӱ�������ֶεĴ���
                }
            }
        }

        /// <summary>
        /// ����������ֶ�ӳ�����
        /// �����ֶκϲ�������ȸ���ӳ���߼�
        /// </summary>
        /// <param name="sourceFeature">ԴҪ��</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="targetFieldName">Ŀ���ֶ���</param>
        /// <param name="sourceFieldName">Դ�ֶ��������������������</param>
        /// <param name="countyName">����</param>
        /// <returns>ת������ֶ�ֵ</returns>
        /// <summary>
        /// ����������ֶ�ӳ�����
        /// �����ֶκϲ�������ȸ���ӳ���߼�
        /// </summary>
        /// <param name="sourceFeature">ԴҪ��</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="targetFieldName">Ŀ���ֶ���</param>
        /// <param name="sourceFieldName">Դ�ֶ��������������������</param>
        /// <param name="countyName">����</param>
        /// <returns>ת������ֶ�ֵ</returns>
        private object ProcessSpecialFieldMapping(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            string targetFieldName,
            string sourceFieldName,
            string countyName)
        {
            try
            {
                // ����������ֶ�ӳ�����
                switch (targetFieldName)
                {
                    case "PCTBBM":
                        // �ֶκϲ���xian + lin_ban + xiao_ban
                        return CombineFields(sourceFeature, sourceFeatureClass,
                            new[] { "xian", "lin_ban", "xiao_ban" });

                    case "ZTBXJ":
                        // �ֶμ��㣺xbmj * ��65���ֶ�
                        return CalculateFieldProduct(sourceFeature, sourceFeatureClass,
                            "xbmj", GetFieldByIndex(sourceFeatureClass, 65));

                    case "XZQMC":
                        // ֱ��ʹ������������ȷ������"��"��
                        return EnsureCountySuffix(countyName);

                    case "CZKFBJMJ":
                        // ����ֶ���ConvertFeatures������ͨ��������㴦��
                        return 0.0; // Ĭ��ֵ��ʵ��ֵ��������ѭ��������

                    default:
                        // ��ͨ�ֶ�ӳ��
                        if (!string.IsNullOrEmpty(sourceFieldName))
                        {
                            int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                            if (sourceFieldIndex != -1)
                            {
                                object sourceValue = sourceFeature.get_Value(sourceFieldIndex);
                                return ConvertFieldValueForSLZYZC(sourceValue, targetFieldName, sourceFieldName, countyName);
                            }
                        }
                        break;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"���������ֶ�ӳ�� {targetFieldName} ʱ����: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ȷ��������"��"�ֽ�β
        /// </summary>
        /// <param name="countyName">ԭʼ����</param>
        /// <returns>ȷ������"��"�ֵ�����</returns>
        private string EnsureCountySuffix(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                return string.Empty;
            }

            // �����������"��"��β�����"��"��
            if (!countyName.EndsWith("��") && !countyName.EndsWith("��") &&
                !countyName.EndsWith("��") && !countyName.EndsWith("��"))
            {
                return countyName + "��";
            }

            return countyName; // �Ѿ�������������λ���ƣ�ֱ�ӷ���
        }

        /// <summary>
        /// �ϲ�����ֶε�ֵ
        /// </summary>
        /// <param name="sourceFeature">ԴҪ��</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="fieldNames">Ҫ�ϲ����ֶ�������</param>
        /// <returns>�ϲ�����ַ���</returns>
        private string CombineFields(IFeature sourceFeature, IFeatureClass sourceFeatureClass, string[] fieldNames)
        {
            var values = new List<string>();

            foreach (string fieldName in fieldNames)
            {
                int fieldIndex = sourceFeatureClass.FindField(fieldName);
                if (fieldIndex != -1)
                {
                    object value = sourceFeature.get_Value(fieldIndex);
                    values.Add(value?.ToString() ?? "");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"�ֶ� {fieldName} ��Դ���в�����");
                    values.Add("");
                }
            }

            return string.Join("", values);
        }

        /// <summary>
        /// ���������ֶεĳ˻�
        /// </summary>
        /// <param name="sourceFeature">ԴҪ��</param>
        /// <param name="sourceFeatureClass">ԴҪ����</param>
        /// <param name="field1Name">��һ���ֶ���</param>
        /// <param name="field2Name">�ڶ����ֶ���</param>
        /// <returns>������</returns>
        private double? CalculateFieldProduct(IFeature sourceFeature, IFeatureClass sourceFeatureClass,
            string field1Name, string field2Name)
        {
            try
            {
                int field1Index = sourceFeatureClass.FindField(field1Name);
                int field2Index = sourceFeatureClass.FindField(field2Name);

                if (field1Index == -1 || field2Index == -1)
                {
                    System.Diagnostics.Debug.WriteLine($"�����ֶγ˻�ʱ���ֶβ�����: {field1Name} �� {field2Name}");
                    return null;
                }

                object value1 = sourceFeature.get_Value(field1Index);
                object value2 = sourceFeature.get_Value(field2Index);

                if (double.TryParse(value1?.ToString(), out double num1) &&
                    double.TryParse(value2?.ToString(), out double num2))
                {
                    return num1 * num2;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"�����ֶγ˻�ʱ����: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ����������ȡ�ֶ���
        /// </summary>
        /// <param name="featureClass">Ҫ����</param>
        /// <param name="index">�ֶ���������1��ʼ��</param>
        /// <returns>�ֶ���</returns>
        private string GetFieldByIndex(IFeatureClass featureClass, int index)
        {
            try
            {
                IFields fields = featureClass.Fields;
                if (index >= 1 && index <= fields.FieldCount)
                {
                    return fields.get_Field(index - 1).Name; // ת��Ϊ0������
                }

                System.Diagnostics.Debug.WriteLine($"�ֶ����� {index} ������Χ");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����������ȡ�ֶ���ʱ����: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ����LCXZGX��SLZYZC���ֶ�ӳ������
        /// �������ṩ���ֶ�ӳ��ģ��
        /// </summary>
        /// <returns>�ֶ�ӳ���ֵ䣨SLZYZC�ֶ��� -> LCXZGX�ֶ�����</returns>
        public static Dictionary<string, string> CreateXZ2SLZYZCFieldsMap()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // �����ֶ�ӳ��
            result.Add("YSDM", "ysdm");            // Ҫ�ش���
            result.Add("XZQDM", "xian");           // ����������
            result.Add("XZQMC", "SPECIAL_COUNTY");           // ����������
            result.Add("GTDCTBBSM", "bsm");         // ��������ͼ�߱���
            result.Add("GTDCTBBH", "tbbh");        // ��������ͼ�߱��
            result.Add("GTDCDLBM", "dlbm");        // ��������������
            result.Add("GTDCDLMC", "dlmc");        // ���������������
            result.Add("QSDWDM", "qsdwdm");        // Ȩ����λ����
            result.Add("QSDWMC", "qsdwmc");        // Ȩ����λ����
            result.Add("ZLDWDM", "zldwdm");        // ���䵥λ����
            result.Add("ZLDWMC", "zldwmc");        // ���䵥λ����
            result.Add("GTDCTBMJ", "tbmj");        // ��������ͼ�����
            result.Add("LYJ", "lin_ye_ju");        // ��ҵ��
            result.Add("LC", "lin_chang");         // �ֳ�
            result.Add("PCDL", "di_lei");          // �ղ����
            result.Add("ZTBMJ", "xbmj");           // ����ͼ�����
            result.Add("GTDCTDQS", "qsxz");        // ������������Ȩ��
            result.Add("LM_SUOYQ", "lmqs");        // ��ľ����Ȩ
            result.Add("LZ", "lin_zhong");         // ����
            result.Add("YSSZ", "you_shi_sz");      // ��������
            result.Add("QY", "qi_yuan");           // ��Դ
            result.Add("YBD", "yu_bi_du");         // ���ն�
            result.Add("PJNL", "pingjun_nl");      // ƽ������
            result.Add("LING_ZU", "ling_zu");      // ����
            result.Add("PJSG", "pingjun_sg");      // ƽ������
            result.Add("PJXJ", "pingjun_xj");      // ƽ���ؾ�
            result.Add("MGQZS", "mei_gq_zs");      // ÿ��������
            result.Add("FRDBS", "frdbs");          // �����ر���
            result.Add("CZKFBJMJ", "SPECIAL_CZKFBJ"); // ���򿪷��߽���� - ���⴦��

            // �����ֶ�ӳ�� - ��Щ�ֶ�������Ĵ����߼�
            result.Add("PCTBBM", "xian+lin_ban+xiao_ban");  // �ֶκϲ�
            result.Add("ZTBXJ", "xbmj*field65");             // �ֶμ���

            return result;
        }

        /// <summary>
        /// ת���ֶ�ֵ������SLZYZC��
        /// ����SLZYZC���ҵ����������Ҫ������ֶ�ֵת��
        /// </summary>
        /// <param name="sourceValue">Դ�ֶ�ֵ</param>
        /// <param name="targetFieldName">Ŀ���ֶ�����SLZYZC��</param>
        /// <param name="sourceFieldName">Դ�ֶ�����LCXZGX��</param>
        /// <param name="countyName">����</param>
        /// <returns>ת������ֶ�ֵ</returns>
        private object ConvertFieldValueForSLZYZC(
            object sourceValue,
            string targetFieldName,
            string sourceFieldName,
            string countyName)
        {
            // �����ֵ���
            if (sourceValue == null || sourceValue == DBNull.Value)
            {
                return null;
            }

            try
            {
                // ����SLZYZC����ֶ�Ҫ���������ת��
                switch (targetFieldName.ToUpper())
                {
                    case "XZQDM":
                        // ���������������Ҫ���⴦��
                        return sourceValue.ToString();

                    case "GTDCTBMJ":
                    case "ZTBMJ":
                        // ����ֶ�ȷ��Ϊ��ֵ����
                        if (double.TryParse(sourceValue.ToString(), out double area))
                        {
                            return area;
                        }
                        return 0.0;

                    case "YBD":
                        // ���նȿ�����Ҫ��ʽ��
                        if (double.TryParse(sourceValue.ToString(), out double canopyClosure))
                        {
                            return Math.Round(canopyClosure, 2);
                        }
                        return sourceValue;

                    case "PJNL":
                        // ƽ������ȷ��Ϊ����
                        if (int.TryParse(sourceValue.ToString(), out int age))
                        {
                            return age;
                        }
                        return sourceValue;

                    default:
                        // Ĭ�ϱ���ԭֵ
                        return sourceValue;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ת��{countyName}�ֶ�{targetFieldName}ֵʱ����: {ex.Message}");
                return sourceValue; // ת��ʧ��ʱ����ԭֵ��ȷ�����ݲ���ʧ
            }
        }

        /// <summary>
        /// ��֤ת�����õ���Ч��
        /// ����ֶ�ӳ�䡢���ݿ����ӵ�ת��ǰ��׼������
        /// </summary>
        /// <param name="countyName">����</param>
        /// <param name="databasePath">���ݿ�·��</param>
        /// <param name="fieldMappings">�ֶ�ӳ������</param>
        /// <returns>��֤�Ƿ�ͨ��</returns>
        public bool ValidateConversionConfiguration(
            string countyName,
            string databasePath,
            Dictionary<string, string> fieldMappings)
        {
            try
            {
                // ��֤��������
                if (string.IsNullOrEmpty(countyName) || string.IsNullOrEmpty(databasePath))
                {
                    return false;
                }

                // ��֤���ݿ�·��������
                string countyGDBPath = System.IO.Path.Combine(databasePath, countyName, countyName + ".gdb");
                if (!Directory.Exists(countyGDBPath))
                {
                    System.Diagnostics.Debug.WriteLine($"�ؼ����ݿⲻ����: {countyGDBPath}");
                    return false;
                }

                // ��֤�ֶ�ӳ������
                if (fieldMappings == null)
                {
                    fieldMappings = CreateXZ2SLZYZCFieldsMap();
                }

                if (fieldMappings.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("�ֶ�ӳ������Ϊ��");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"ת��������֤ͨ��: {countyName}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��֤ת������ʱ����: {ex.Message}");
                return false;
            }
        }
    }
}