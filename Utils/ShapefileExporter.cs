using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using TestArcMapAddin2;
using TestArcMapAddin2.ShapefileUtils;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 数据库导出工具类 - 将处理结果写入到Shapefile中
    /// 负责将处理后的林地要素数据直接导出为县级SLZYZC Shapefile文件
    /// </summary>
    public class ShapefileExporter
    {
        /// <summary>
        /// 进度回调委托 - 用于向UI层报告处理进度
        /// </summary>
        /// <param name="percentage">完成百分比 (0-100)</param>
        /// <param name="message">当前操作描述信息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 将处理后的县级数据直接输出为SLZYZC Shapefile
        /// </summary>
        /// <param name="processedFeatures">处理后的要素列表</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="countyName">县名</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="progressCallback">进度回调</param>
        public void ExportToShapefile(
            List<IFeature> processedFeatures,
            IFeatureClass sourceFeatureClass,
            string countyName,
            string outputPath,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null)
        {
            // 参数验证 - 确保输入数据的有效性
            if (processedFeatures == null || processedFeatures.Count == 0)
            {
                throw new ArgumentException("处理后的要素列表不能为空");
            }

            if (string.IsNullOrEmpty(countyName))
            {
                throw new ArgumentException("县名不能为空");
            }

            if (string.IsNullOrEmpty(outputPath))
            {
                throw new ArgumentException("输出路径不能为空");
            }

            progressCallback?.Invoke(5, $"正在创建{countyName}的Shapefile输出目录...");

            // COM对象声明 - 需要在finally块中显式释放以避免内存泄漏
            IWorkspace shapefileWorkspace = null;
            IFeatureClass slzyzcFeatureClass = null;

            try
            {
                // 创建县级Shapefile工作空间
                shapefileWorkspace = CreateCountyShapefileWorkspace(outputPath, countyName);
                if (shapefileWorkspace == null)
                {
                    throw new Exception($"无法创建{countyName}的Shapefile工作空间");
                }

                progressCallback?.Invoke(15, $"正在创建{countyName}的SLZYZC Shapefile...");

                // 直接从当前处理的要素获取几何类型和空间参考，确保与源数据一致
                IFeature firstFeature = processedFeatures[0];
                esriGeometryType geometryType = firstFeature.Shape.GeometryType;
                ISpatialReference spatialReference = firstFeature.Shape.SpatialReference;

                // 直接创建SLZYZC要素类
                slzyzcFeatureClass = CreateSLZYZCShapefile(shapefileWorkspace, geometryType, spatialReference);
                if (slzyzcFeatureClass == null)
                {
                    throw new Exception($"无法创建{countyName}的SLZYZC Shapefile");
                }

                progressCallback?.Invoke(25, $"开始向{countyName}的SLZYZC Shapefile写入数据...");

                // 执行数据写入操作 - 直接写入SLZYZC格式
                WriteFeaturesToShapefile(processedFeatures, sourceFeatureClass, slzyzcFeatureClass,
                    fieldMappings, countyName, progressCallback);

                progressCallback?.Invoke(80, $"成功将 {processedFeatures.Count} 个要素写入到{countyName}的SLZYZC Shapefile");

                System.Diagnostics.Debug.WriteLine($"县{countyName}的数据已成功写入SLZYZC Shapefile");

                // 执行SLZYZC_DLTB转换操作
                PerformAutoConversion(countyName, outputPath, progressCallback);

                progressCallback?.Invoke(100, $"{countyName}的数据导入和转换已全部完成");
            }
            finally
            {
                // 重要：释放ArcGIS COM对象，防止内存泄漏
                if (slzyzcFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(slzyzcFeatureClass);
                }
                if (shapefileWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(shapefileWorkspace);
                }
            }
        }

        /// <summary>
        /// 执行自动转换 - 将SLZYZC数据转换为SLZYZC_DLTB
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="progressCallback">进度回调</param>
        private void PerformAutoConversion(string countyName, string outputPath, ProgressCallback progressCallback)
        {
            try
            {
                // 从80%开始，直接进行SLZYZC到SLZYZC_DLTB的转换
                progressCallback?.Invoke(80, $"准备转换{countyName}的SLZYZC到DLTB成果表...");
                System.Diagnostics.Debug.WriteLine($"开始自动转换县{countyName}的数据从SLZYZC到SLZYZC_DLTB");

                // 构建文件路径
                string countyPath = System.IO.Path.Combine(outputPath, countyName);
                string slzyzcShapefilePath = System.IO.Path.Combine(countyPath, "SLZYZC.shp");
                string slzyzcDltbShapefilePath = System.IO.Path.Combine(countyPath, "SLZYZC_DLTB.shp");

                // 直接执行SLZYZC转换为SLZYZC_DLTB
                var dltbConverter = new Convert2SLZYZCDLTB();

                bool conversionSuccess = dltbConverter.ConvertSLZYZCToDLTB(
                    slzyzcShapefilePath,
                    slzyzcDltbShapefilePath,
                    null, // 使用默认字段映射
                    (subPercentage, subMessage) =>
                    {
                        // 将转换进度映射到总进度的80%-99%区间
                        int totalPercentage = 80 + (subPercentage * 19 / 100);
                        progressCallback?.Invoke(totalPercentage, $"{countyName}: {subMessage}");
                    });

                if (conversionSuccess)
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC数据已成功自动转换为SLZYZC_DLTB表");
                    progressCallback?.Invoke(99, $"{countyName}的数据全部转换成功完成");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC_DLTB数据转换失败");
                    progressCallback?.Invoke(99, $"{countyName}的SLZYZC_DLTB数据转换失败，但SLZYZC数据已成功保存");
                }
            }
            catch (Exception ex)
            {
                // 转换失败不应影响主要的数据插入流程
                System.Diagnostics.Debug.WriteLine($"自动转换县{countyName}数据时出错: {ex.Message}");
                progressCallback?.Invoke(99, $"{countyName}的数据转换出错: {ex.Message}");

                // 记录错误但不抛出异常，确保主流程继续
                System.Diagnostics.Debug.WriteLine($"转换错误详情: {ex}");
            }
        }

        /// <summary>
        /// 批量输出多个县的数据到各自的Shapefile
        /// </summary>
        /// <param name="countyFeaturesMap">县级要素映射（县名 -> 要素列表）</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="progressCallback">进度回调</param>
        public void BatchExportToShapefile(
            Dictionary<string, List<IFeature>> countyFeaturesMap,
            IFeatureClass sourceFeatureClass,
            string outputPath,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null)
        {
            if (countyFeaturesMap == null || countyFeaturesMap.Count == 0)
            {
                throw new ArgumentException("县级要素映射不能为空");
            }

            int totalCounties = countyFeaturesMap.Count;
            int processedCounties = 0;

            progressCallback?.Invoke(0, $"开始批量处理{totalCounties}个县的数据...");

            // 遍历每个县的数据进行处理
            foreach (var countyData in countyFeaturesMap)
            {
                string countyName = countyData.Key;
                List<IFeature> countyFeatures = countyData.Value;

                try
                {
                    // 计算总体进度 - 基于已处理的县数量
                    int overallProgress = (processedCounties * 100) / totalCounties;
                    progressCallback?.Invoke(overallProgress, $"正在处理县: {countyName} ({processedCounties + 1}/{totalCounties})");

                    // 为每个县输出数据到Shapefile
                    ExportToShapefile(countyFeatures, sourceFeatureClass, countyName, outputPath,
                        fieldMappings, (percentage, message) =>
                        {
                            // 将单个县的进度（0-100）映射到当前县的总体进度范围内
                            int countyOverallProgress = overallProgress + (percentage * (100 / totalCounties) / 100);
                            progressCallback?.Invoke(countyOverallProgress, message);
                        });

                    processedCounties++;

                    System.Diagnostics.Debug.WriteLine($"县{countyName}数据输出和转换完成 ({processedCounties}/{totalCounties})");
                }
                catch (Exception ex)
                {
                    // 错误处理策略：记录错误但继续处理其他县
                    System.Diagnostics.Debug.WriteLine($"输出县{countyName}数据时出错: {ex.Message}");
                    processedCounties++;
                }
            }

            progressCallback?.Invoke(100, $"所有县的数据输出和转换完成 ({processedCounties}/{totalCounties})");
        }

        /// <summary>
        /// 创建县级Shapefile工作空间
        /// </summary>
        /// <param name="outputPath">输出路径</param>
        /// <param name="countyName">县名</param>
        /// <returns>Shapefile工作空间接口</returns>
        private IWorkspace CreateCountyShapefileWorkspace(string outputPath, string countyName)
        {
            try
            {
                // 修改：使用成果目录结构而不是简单的县名目录
                string countyCode = "XXXXXX"; // 这里应该从实际数据获取
                string countyFolderName = $"{countyName}（{countyCode}）全民所有自然资源资产清查数据成果";
                string countyFolderPath = System.IO.Path.Combine(outputPath, countyFolderName);
                string dataSetPath = System.IO.Path.Combine(countyFolderPath, "清查数据集");
                string forestPath = System.IO.Path.Combine(dataSetPath, "森林");
                string spatialDataPath = System.IO.Path.Combine(forestPath, "空间数据");

                // 确保目录存在
                if (!Directory.Exists(spatialDataPath))
                {
                    Directory.CreateDirectory(spatialDataPath);
                    System.Diagnostics.Debug.WriteLine($"创建目录结构: {spatialDataPath}");
                }

                // 使用ProgID创建Shapefile工作空间工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // 打开工作空间
                IWorkspace workspace = workspaceFactory.OpenFromFile(spatialDataPath, 0);

                System.Diagnostics.Debug.WriteLine($"成功创建{countyName}的Shapefile工作空间: {spatialDataPath}");
                return workspace;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建{countyName}Shapefile工作空间时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 创建SLZYZC Shapefile要素类
        /// </summary>
        /// <param name="workspace">Shapefile工作空间</param>
        /// <param name="geometryType">几何类型</param>
        /// <param name="spatialReference">空间参考</param>
        /// <returns>SLZYZC要素类接口</returns>
        private IFeatureClass CreateSLZYZCShapefile(IWorkspace workspace, esriGeometryType geometryType, ISpatialReference spatialReference)
        {
            try
            {
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

                // 检查要素类是否已存在
                string featureClassName = "SLZYZC";
                try
                {
                    IFeatureClass existingFeatureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                    if (existingFeatureClass != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"SLZYZC Shapefile已存在，将使用现有文件并清空数据");

                        // 清空现有数据
                        ClearExistingShapefileData(existingFeatureClass);

                        return existingFeatureClass;
                    }
                }
                catch
                {
                    // 要素类不存在，继续创建
                }

                // 如果文件不存在，创建新的Shapefile
                // ... 保持原有的创建逻辑 ...

                // 创建字段集合
                IFields fields = new FieldsClass();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                // 添加OID字段
                IField oidField = new FieldClass();
                IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
                oidFieldEdit.Name_2 = "FID";
                oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldsEdit.AddField(oidField);

                // 添加几何字段
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.GeometryType_2 = geometryType;
                geometryDefEdit.SpatialReference_2 = spatialReference; // 直接使用传入的空间参考

                IField geometryField = new FieldClass();
                IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
                geometryFieldEdit.Name_2 = "Shape";
                geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                geometryFieldEdit.GeometryDef_2 = geometryDef;
                fieldsEdit.AddField(geometryField);

                // 使用SLZYZC字段模板添加业务字段
                GenerateSLZYZCFields(fieldsEdit);

                // 创建要素类
                IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(
                    featureClassName,
                    fields,
                    null,
                    null,
                    esriFeatureType.esriFTSimple,
                    "Shape",
                    "");

                System.Diagnostics.Debug.WriteLine($"成功创建SLZYZC Shapefile要素类");
                return featureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建SLZYZC Shapefile要素类时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 清空现有Shapefile的数据
        /// </summary>
        /// <param name="featureClass">要清空的要素类</param>
        private void ClearExistingShapefileData(IFeatureClass featureClass)
        {
            IFeatureCursor deleteCursor = null;
            try
            {
                // 获取所有要素的游标
                deleteCursor = featureClass.Search(null, false);
                IFeature feature;

                // 逐个删除所有要素
                while ((feature = deleteCursor.NextFeature()) != null)
                {
                    try
                    {
                        feature.Delete();
                    }
                    finally
                    {
                        if (feature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine("成功清空现有shapefile数据");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"清空shapefile数据时出错: {ex.Message}");
                throw;
            }
            finally
            {
                if (deleteCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(deleteCursor);
                }
            }
        }

        /// <summary>
        /// 生成SLZYZC字段结构（移除LCXZGX相关字段）
        /// </summary>
        /// <param name="fieldsEdit">字段编辑接口</param>
        private void GenerateSLZYZCFields(IFieldsEdit fieldsEdit)
        {
            // 只保留 SLZYZC 需要的字段，移除 LCXZGX 特有字段
            var slzyzcFields = new Dictionary<string, esriFieldType>
            {
                { "YSDM", esriFieldType.esriFieldTypeString },
                { "XZQDM", esriFieldType.esriFieldTypeString },
                { "XZQMC", esriFieldType.esriFieldTypeString },
                { "GTDCTBBSM", esriFieldType.esriFieldTypeString },
                { "GTDCTBBH", esriFieldType.esriFieldTypeString },
                { "GTDCDLBM", esriFieldType.esriFieldTypeString },
                { "GTDCDLMC", esriFieldType.esriFieldTypeString },
                { "QSDWDM", esriFieldType.esriFieldTypeString },
                { "QSDWMC", esriFieldType.esriFieldTypeString },
                { "ZLDWDM", esriFieldType.esriFieldTypeString },
                { "ZLDWMC", esriFieldType.esriFieldTypeString },
                { "GTDCTBMJ", esriFieldType.esriFieldTypeDouble },
                { "LYJ", esriFieldType.esriFieldTypeString },
                { "LC", esriFieldType.esriFieldTypeString },
                { "PCDL", esriFieldType.esriFieldTypeString },
                { "ZTBMJ", esriFieldType.esriFieldTypeDouble },
                { "GTDCTDQS", esriFieldType.esriFieldTypeString },
                { "LM_SUOYQ", esriFieldType.esriFieldTypeString },
                { "LZ", esriFieldType.esriFieldTypeString },
                { "YSSZ", esriFieldType.esriFieldTypeString },
                { "QY", esriFieldType.esriFieldTypeString },
                { "YBD", esriFieldType.esriFieldTypeDouble },
                { "PJNL", esriFieldType.esriFieldTypeInteger },
                { "LING_ZU", esriFieldType.esriFieldTypeString },
                { "PJSG", esriFieldType.esriFieldTypeDouble },
                { "PJXJ", esriFieldType.esriFieldTypeDouble },
                { "MGQZS", esriFieldType.esriFieldTypeInteger },
                { "FRDBS", esriFieldType.esriFieldTypeString },
                { "CZKFBJMJ", esriFieldType.esriFieldTypeDouble },
                { "PCTBBM", esriFieldType.esriFieldTypeString },
                { "ZTBXJ", esriFieldType.esriFieldTypeDouble },
                { "ZCQCBSM", esriFieldType.esriFieldTypeString }
            };

            // 添加所有SLZYZC字段
            foreach (var fieldDef in slzyzcFields)
            {
                IField field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = fieldDef.Key;
                fieldEdit.Type_2 = fieldDef.Value;

                // 设置字符串字段的长度
                if (fieldDef.Value == esriFieldType.esriFieldTypeString)
                {
                    switch (fieldDef.Key)
                    {
                        case "ZCQCBSM":
                            fieldEdit.Length_2 = 30;
                            break;
                        case "GTDCTBBSM":
                        case "PCTBBM":
                            fieldEdit.Length_2 = 20;
                            break;
                        case "XZQDM":
                        case "QSDWDM":
                        case "ZLDWDM":
                            fieldEdit.Length_2 = 19;
                            break;
                        case "GTDCTBBH":
                            fieldEdit.Length_2 = 8;
                            break;
                        default:
                            fieldEdit.Length_2 = 50;
                            break;
                    }
                }

                fieldsEdit.AddField(field);
            }
        }

        /// <summary>
        /// 将要素写入SLZYZC Shapefile
        /// </summary>
        /// <param name="sourceFeatures">源要素列表</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        /// <param name="progressCallback">进度回调</param>
        private void WriteFeaturesToShapefile(
            List<IFeature> sourceFeatures,
            IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            ProgressCallback progressCallback)
        {
            // 创建要素缓冲区和插入游标
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);
            IFeatureClass czkfbjFeatureClass = null;

            try
            {
                // 获取CZKFBJ数据用于CZKFBJMJ字段计算
                string autoDetectedPath = GetCZKFBJShapefilePath(countyName);
                if (!string.IsNullOrEmpty(autoDetectedPath))
                {
                    var czkfbjResult = OpenShapefileFeatureClass(autoDetectedPath);
                    czkfbjFeatureClass = czkfbjResult.featureClass;
                }

                // 初始化统计变量
                int totalFeatures = sourceFeatures.Count;
                int processedCount = 0;
                int successCount = 0;
                int errorCount = 0;

                // 动态计算进度更新的间隔，确保进度条平滑更新
                int updateInterval = Math.Max(1, totalFeatures / 100);

                System.Diagnostics.Debug.WriteLine($"开始向{countyName}的SLZYZC Shapefile插入{totalFeatures}个要素");

                // 获取字段映射 - 使用SLZYZC字段映射
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    fieldMappings = GetDefaultSLZYZCFieldMappings();
                }

                // 获取特殊字段索引
                int zcqcbsmIndex = targetFeatureClass.FindField("ZCQCBSM");
                int czkfbjmjIndex = targetFeatureClass.FindField("CZKFBJMJ");

                // 逐个处理要素
                foreach (IFeature sourceFeature in sourceFeatures)
                {
                    try
                    {
                        // 复制几何对象
                        if (sourceFeature.Shape != null)
                        {
                            featureBuffer.Shape = sourceFeature.ShapeCopy;
                        }

                        // 复制属性值并执行字段映射转换
                        CopyFeatureAttributesForSLZYZC(sourceFeature, sourceFeatureClass, featureBuffer,
                            targetFeatureClass, fieldMappings, countyName, successCount + 1);

                        // 处理特殊字段：ZCQCBSM
                        if (zcqcbsmIndex != -1)
                        {
                            string zcqcbsmValue = GenerateZCQCBSM(sourceFeature, sourceFeatureClass, countyName, successCount + 1);
                            if (!string.IsNullOrEmpty(zcqcbsmValue))
                            {
                                featureBuffer.set_Value(zcqcbsmIndex, zcqcbsmValue);
                            }
                        }

                        // 处理特殊字段：CZKFBJMJ
                        if (czkfbjmjIndex != -1)
                        {
                            double intersectionArea = 0;
                            if (czkfbjFeatureClass != null && sourceFeature.Shape != null)
                            {
                                intersectionArea = CalculateIntersectionArea(sourceFeature.Shape, czkfbjFeatureClass);
                            }
                            featureBuffer.set_Value(czkfbjmjIndex, intersectionArea);
                        }

                        // 执行要素插入操作
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        processedCount++;

                        // 使用动态计算的间隔来更新进度
                        if (processedCount % updateInterval == 0 || processedCount == totalFeatures)
                        {
                            // 将此过程的进度映射到总体进度的 25% 到 80% 区间
                            int percentage = 25 + (int)((processedCount / (double)totalFeatures) * 55);
                            progressCallback?.Invoke(percentage,
                                $"正在写入{countyName}的SLZYZC Shapefile... ({processedCount}/{totalFeatures})");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"插入{countyName}要素时出错: {ex.Message}");
                    }
                }

                // 提交所有插入操作
                insertCursor.Flush();

                System.Diagnostics.Debug.WriteLine($"{countyName}数据写入完成: 成功{successCount}个, 失败{errorCount}个");
            }
            finally
            {
                // 重要：释放ArcGIS COM对象
                if (insertCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                }
                if (featureBuffer != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureBuffer);
                }
                if (czkfbjFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjFeatureClass);
                }
            }
        }

        /// <summary>
        /// 复制要素属性并进行字段映射（SLZYZC版本）
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFeatureBuffer">目标要素缓冲区</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        /// <param name="featureSequence">要素序号</param>
        private void CopyFeatureAttributesForSLZYZC(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            IFeatureBuffer targetFeatureBuffer,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            int featureSequence)
        {
            // 遍历所有字段映射进行数据复制
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;    // 目标字段名
                string sourceFieldName = mapping.Value;  // 源字段名

                try
                {
                    // 获取源字段和目标字段的索引
                    int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);

                    // 只有当目标字段存在时才进行数据复制
                    if (targetFieldIndex != -1)
                    {
                        object targetValue = ProcessSLZYZCFieldMapping(sourceFeature, sourceFeatureClass,
                            targetFieldName, sourceFieldName, countyName, featureSequence);

                        if (targetValue != null)
                        {
                            targetFeatureBuffer.set_Value(targetFieldIndex, targetValue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"复制{countyName}字段{targetFieldName}时出错: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 处理SLZYZC字段映射转换
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFieldName">目标字段名</param>
        /// <param name="sourceFieldName">源字段名</param>
        /// <param name="countyName">县名</param>
        /// <param name="featureSequence">要素序号</param>
        /// <returns>转换后的字段值</returns>
        private object ProcessSLZYZCFieldMapping(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            string targetFieldName,
            string sourceFieldName,
            string countyName,
            int featureSequence)
        {
            try
            {
                switch (targetFieldName)
                {
                    case "PCTBBM":
                        // 字段合并：xian + lin_ban + xiao_ban
                        return CombineFields(sourceFeature, sourceFeatureClass,
                            new[] { "xian", "lin_ban", "xiao_ban" });

                    case "ZTBXJ":
                        // 字段计算：xbmj * 第65个字段
                        return CalculateFieldProduct(sourceFeature, sourceFeatureClass,
                            "xbmj", GetFieldByIndex(sourceFeatureClass, 65));

                    case "XZQMC":
                        // 使用县名
                        return EnsureCountySuffix(countyName);

                    default:
                        // 普通字段映射
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
                System.Diagnostics.Debug.WriteLine($"处理SLZYZC字段映射 {targetFieldName} 时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取默认SLZYZC字段映射
        /// </summary>
        /// <returns>字段映射字典</returns>
        private Dictionary<string, string> GetDefaultSLZYZCFieldMappings()
        {
            return new Dictionary<string, string>
            {
                { "YSDM", "ysdm" },            // 要素代码
                { "XZQDM", "xian" },           // 行政区代码
                { "GTDCTBBSM", "bsm" },        // 国土调查图斑编码
                { "GTDCTBBH", "tbbh" },        // 国土调查图斑编号
                { "GTDCDLBM", "dlbm" },        // 国土调查地类编码
                { "GTDCDLMC", "dlmc" },        // 国土调查地类名称
                { "QSDWDM", "qsdwdm" },        // 权属单位代码
                { "QSDWMC", "qsdwmc" },        // 权属单位名称
                { "ZLDWDM", "zldwdm" },        // 坐落单位代码
                { "ZLDWMC", "zldwmc" },        // 坐落单位名称
                { "GTDCTBMJ", "tbmj" },        // 国土调查图斑面积
                { "LYJ", "lin_ye_ju" },        // 林业局
                { "LC", "lin_chang" },         // 林场
                { "PCDL", "di_lei" },          // 普查地类
                { "ZTBMJ", "xbmj" },           // 株数图斑面积
                { "GTDCTDQS", "qsxz" },        // 国土调查土地权属
                { "LM_SUOYQ", "lmqs" },        // 林木所有权
                { "LZ", "lin_zhong" },         // 林种
                { "YSSZ", "you_shi_sz" },      // 优势树种
                { "QY", "qi_yuan" },           // 起源
                { "YBD", "yu_bi_du" },         // 郁闭度
                { "PJNL", "pingjun_nl" },      // 平均年龄
                { "LING_ZU", "ling_zu" },      // 龄组
                { "PJSG", "pingjun_sg" },      // 平均树高
                { "PJXJ", "pingjun_xj" },      // 平均胸径
                { "MGQZS", "mei_gq_zs" },      // 每公顷株数
                { "FRDBS", "frdbs" }           // 发育地被层
            };
        }

        /// <summary>
        /// 生成ZCQCBSM值
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="countyName">县名</param>
        /// <param name="featureSequence">要素序号</param>
        /// <returns>ZCQCBSM值</returns>
        private string GenerateZCQCBSM(IFeature sourceFeature, IFeatureClass sourceFeatureClass, string countyName, int featureSequence)
        {
            try
            {
                // 格式: XZQDM(6) + "4120" + 图斑序号(12)
                int xzqdmIndex = sourceFeatureClass.FindField("xian");
                if (xzqdmIndex != -1)
                {
                    string xzqdm = sourceFeature.get_Value(xzqdmIndex)?.ToString() ?? "";
                    if (xzqdm.Length > 6)
                    {
                        xzqdm = xzqdm.Substring(0, 6);
                    }
                    string sequenceStr = featureSequence.ToString("D12"); // 格式化为12位，前补0
                    return $"{xzqdm}4120{sequenceStr}";
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成ZCQCBSM时出错: {ex.Message}");
                return null;
            }
        }

        // 保持现有的辅助方法
        private string GetCZKFBJShapefilePath(string countyName)
        {
            try
            {
                var czkfbjFiles = SharedDataManager.GetCZKFBJFiles();

                foreach (var fileInfo in czkfbjFiles)
                {
                    if (fileInfo.DisplayName.Equals(countyName, StringComparison.OrdinalIgnoreCase) ||
                        fileInfo.DisplayName.Contains(countyName))
                    {
                        return fileInfo.FullPath;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取{countyName}的CZKFBJ shapefile路径时出错: {ex.Message}");
                return null;
            }
        }

        private (IWorkspace workspace, IFeatureClass featureClass) OpenShapefileFeatureClass(string shapefilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(shapefilePath) || !File.Exists(shapefilePath))
                {
                    return (null, null);
                }

                string shapefileDirectory = System.IO.Path.GetDirectoryName(shapefilePath);
                string shapefileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                IWorkspace workspace = workspaceFactory.OpenFromFile(shapefileDirectory, 0);
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(shapefileName);

                return (workspace, featureClass);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"打开shapefile时出错: {ex.Message}");
                return (null, null);
            }
        }

        private double CalculateIntersectionArea(IGeometry geometry, IFeatureClass czkfbjFeatureClass)
        {
            // 复用Convert2SLZYZC中的交集面积计算逻辑
            if (geometry == null || czkfbjFeatureClass == null)
            {
                return 0;
            }

            double totalIntersectionArea = 0;
            IFeatureCursor czkfbjCursor = null;
            ISpatialFilter spatialFilter = null;

            try
            {
                if (geometry.IsEmpty)
                {
                    return 0;
                }

                spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = geometry;
                spatialFilter.GeometryField = czkfbjFeatureClass.ShapeFieldName;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                czkfbjCursor = czkfbjFeatureClass.Search(spatialFilter, false);
                IFeature czkfbjFeature;

                while ((czkfbjFeature = czkfbjCursor.NextFeature()) != null)
                {
                    try
                    {
                        if (czkfbjFeature.Shape != null && !czkfbjFeature.Shape.IsEmpty)
                        {
                            ITopologicalOperator topoOperator = (ITopologicalOperator)geometry;
                            IGeometry intersectionGeometry = topoOperator.Intersect(czkfbjFeature.Shape, esriGeometryDimension.esriGeometry2Dimension);

                            if (intersectionGeometry != null && !intersectionGeometry.IsEmpty)
                            {
                                IArea area = (IArea)intersectionGeometry;
                                totalIntersectionArea += Math.Abs(area.Area);
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(intersectionGeometry);
                            }
                        }
                    }
                    finally
                    {
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
                System.Diagnostics.Debug.WriteLine($"计算交集面积时出错: {ex.Message}");
                return 0;
            }
            finally
            {
                if (czkfbjCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjCursor);
                }
                if (spatialFilter != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
                }
            }
        }

        // 保持现有的辅助方法，但适配SLZYZC
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
                    values.Add("");
                }
            }
            return string.Join("", values);
        }

        private double? CalculateFieldProduct(IFeature sourceFeature, IFeatureClass sourceFeatureClass, string field1Name, string field2Name)
        {
            try
            {
                int field1Index = sourceFeatureClass.FindField(field1Name);
                int field2Index = sourceFeatureClass.FindField(field2Name);

                if (field1Index == -1 || field2Index == -1)
                {
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
                System.Diagnostics.Debug.WriteLine($"计算字段乘积时出错: {ex.Message}");
                return null;
            }
        }

        private string GetFieldByIndex(IFeatureClass featureClass, int index)
        {
            try
            {
                IFields fields = featureClass.Fields;
                if (index >= 1 && index <= fields.FieldCount)
                {
                    return fields.get_Field(index - 1).Name;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"根据索引获取字段名时出错: {ex.Message}");
                return null;
            }
        }

        private string EnsureCountySuffix(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                return string.Empty;
            }

            // 使用正则表达式仅保留中文字符
            string chineseOnlyCountyName = System.Text.RegularExpressions.Regex.Replace(countyName, @"[^\u4e00-\u9fa5]", "");

            // 如果过滤后名称为空，则回退到使用原始名称
            if (string.IsNullOrEmpty(chineseOnlyCountyName))
            {
                chineseOnlyCountyName = countyName;
            }

            return chineseOnlyCountyName;
        }

        private object ConvertFieldValueForSLZYZC(object sourceValue, string targetFieldName, string sourceFieldName, string countyName)
        {
            if (sourceValue == null || sourceValue == DBNull.Value)
            {
                return null;
            }

            try
            {
                switch (targetFieldName.ToUpper())
                {
                    case "XZQDM":
                        return sourceValue.ToString();

                    case "GTDCTBMJ":
                    case "ZTBMJ":
                        if (double.TryParse(sourceValue.ToString(), out double area))
                        {
                            return area;
                        }
                        return 0.0;

                    case "YBD":
                        if (double.TryParse(sourceValue.ToString(), out double canopyClosure))
                        {
                            return Math.Round(canopyClosure, 2);
                        }
                        return sourceValue;

                    case "PJNL":
                        if (int.TryParse(sourceValue.ToString(), out int age))
                        {
                            return age;
                        }
                        return sourceValue;

                    default:
                        return sourceValue;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换{countyName}字段{targetFieldName}值时出错: {ex.Message}");
                return sourceValue;
            }
        }
    }
}