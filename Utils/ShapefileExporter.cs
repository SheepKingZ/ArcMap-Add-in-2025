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
    /// 负责将处理后的林地要素数据导出到县级Shapefile文件中
    /// </summary>
    public class ShapefileExporter
    {
        /// <summary>
        /// CGCS2000坐标系WKT定义
        /// </summary>
        private const string CGCS2000_WKT = @"GEOGCS[""GCS_China_Geodetic_Coordinate_System_2000"",DATUM[""D_China_2000"",SPHEROID[""CGCS2000"",6378137.0,298.257222101]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]]";

        /// <summary>
        /// CGCS2000 3度带37带投影坐标系WKT定义
        /// </summary>
        private const string CGCS2000_3_DEGREE_GK_ZONE_37_WKT = @"PROJCS[""GCS_China_Geodetic_Coordinate_System_2000_3_Degree_GK_Zone_37"",GEOGCS[""GCS_China_Geodetic_Coordinate_System_2000"",DATUM[""D_China_2000"",SPHEROID[""CGCS2000"",6378137.0,298.257222101]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]],PROJECTION[""Gauss_Kruger""],PARAMETER[""False_Easting"",37500000.0],PARAMETER[""False_Northing"",0.0],PARAMETER[""Central_Meridian"",111.0],PARAMETER[""Scale_Factor"",1.0],PARAMETER[""Latitude_Of_Origin"",0.0],UNIT[""Meter"",1.0]]";

        /// <summary>
        /// 进度回调委托 - 用于向UI层报告处理进度
        /// </summary>
        /// <param name="percentage">完成百分比 (0-100)</param>
        /// <param name="message">当前操作描述信息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 将处理后的县级数据输出到Shapefile
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
            IFeatureClass lcxzgxFeatureClass = null;

            try
            {
                // 创建县级Shapefile工作空间
                shapefileWorkspace = CreateCountyShapefileWorkspace(outputPath, countyName);
                if (shapefileWorkspace == null)
                {
                    throw new Exception($"无法创建{countyName}的Shapefile工作空间");
                }

                progressCallback?.Invoke(15, $"正在创建{countyName}的LCXZGX Shapefile...");

                // 🔥 修改: 直接从当前处理的要素获取几何类型和空间参考，确保与源数据一致
                IFeature firstFeature = processedFeatures[0];
                esriGeometryType geometryType = firstFeature.Shape.GeometryType;
                ISpatialReference spatialReference = firstFeature.Shape.SpatialReference;

                // 创建LCXZGX要素类
                lcxzgxFeatureClass = CreateLCXZGXShapefile(shapefileWorkspace, geometryType, spatialReference);
                if (lcxzgxFeatureClass == null)
                {
                    throw new Exception($"无法创建{countyName}的LCXZGX Shapefile");
                }

                progressCallback?.Invoke(25, $"开始向{countyName}的LCXZGX Shapefile写入数据...");

                // 执行数据写入操作
                WriteFeaturesToShapefile(processedFeatures, sourceFeatureClass, lcxzgxFeatureClass,
                    fieldMappings, countyName, progressCallback);

                progressCallback?.Invoke(80, $"成功将 {processedFeatures.Count} 个要素写入到{countyName}的LCXZGX Shapefile");

                System.Diagnostics.Debug.WriteLine($"县{countyName}的数据已成功写入Shapefile");

                // 执行转换操作
                // 移除了此处固定的进度跳转，将进度控制移入 PerformAutoConversion
                PerformAutoConversion(countyName, outputPath, progressCallback);

                progressCallback?.Invoke(100, $"{countyName}的数据导入和转换已全部完成");
            }
            finally
            {
                // 重要：释放ArcGIS COM对象，防止内存泄漏
                if (lcxzgxFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lcxzgxFeatureClass);
                }
                if (shapefileWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(shapefileWorkspace);
                }
            }
        }

        /// <summary>
        /// 执行自动转换 - 在LCXZGX数据插入完成后自动转换为SLZYZC和SLZYZC_DLTB
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="progressCallback">进度回调</param>
        private void PerformAutoConversion(string countyName, string outputPath, ProgressCallback progressCallback)
        {
            try
            {
                // 🔥 修改: 在转换开始时立即更新进度，从上一个阶段的80%平滑过渡
                progressCallback?.Invoke(80, $"准备转换 {countyName} 的成果表...");
                System.Diagnostics.Debug.WriteLine($"开始自动转换县{countyName}的数据从LCXZGX到SLZYZC");

                // 构建文件路径
                string countyPath = System.IO.Path.Combine(outputPath, countyName);
                string lcxzgxShapefilePath = System.IO.Path.Combine(countyPath, "LCXZGX.shp");
                string slzyzcShapefilePath = System.IO.Path.Combine(countyPath, "SLZYZC.shp");

                // 创建转换器实例
                var converter = new Convert2SLZYZC();

                // 传递正确的参数 - lcxzgxShapefilePath是LCXZGX.shp的完整路径
                bool conversionSuccess = converter.ConvertLCXZGXToSLZYZC(
                    countyName,
                    lcxzgxShapefilePath,  // 这是LCXZGX.shp的完整路径，不是目录路径
                    null, // 使用默认字段映射
                    (subPercentage, subMessage) =>
                    {
                        int totalPercentage = 80 + (subPercentage * 10 / 100);
                        progressCallback?.Invoke(totalPercentage, $"{countyName}: {subMessage}");
                    });

                if (conversionSuccess)
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的LCXZGX数据已成功自动转换为SLZYZC表");
                    //在第二次转换开始前，将进度明确设置到90%
                    progressCallback?.Invoke(90, $"{countyName}的SLZYZC数据转换成功，准备转换DLTB...");

                    // 继续执行第二次转换 - SLZYZC转换为SLZYZC_DLTB
                    System.Diagnostics.Debug.WriteLine($"开始自动转换县{countyName}的数据从SLZYZC到SLZYZC_DLTB");
                    string slzyzcDltbShapefilePath = System.IO.Path.Combine(countyPath, "SLZYZC_DLTB.shp");
                    var dltbConverter = new Convert2SLZYZCDLTB();

                    bool conversion3Success = dltbConverter.ConvertSLZYZCToDLTB(
                        slzyzcShapefilePath,
                        slzyzcDltbShapefilePath,
                        null, // 使用默认字段映射
                        (subPercentage, subMessage) =>
                        {
                            // 🔥 修改: 将转换进度映射到总进度的90%-99%区间
                            int totalPercentage = 90 + (subPercentage * 9 / 100);
                            progressCallback?.Invoke(totalPercentage, $"{countyName}: {subMessage}");
                        });

                    if (conversion3Success)
                    {
                        System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC数据已成功自动转换为SLZYZC_DLTB表");
                        // 🔥 修改: 将最终进度设置为99%，为完成步骤留出空间
                        progressCallback?.Invoke(99, $"{countyName}的数据全部转换成功完成");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC_DLTB数据转换失败");
                        progressCallback?.Invoke(99, $"{countyName}的SLZYZC_DLTB数据转换失败，但SLZYZC数据已成功保存");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC数据转换失败");
                    progressCallback?.Invoke(90, $"{countyName}的数据转换失败，但LCXZGX数据已成功保存");
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
                // 构建县级Shapefile目录路径
                string countyShapefilePath = System.IO.Path.Combine(outputPath, countyName);

                // 确保目录存在
                if (!Directory.Exists(countyShapefilePath))
                {
                    Directory.CreateDirectory(countyShapefilePath);
                    System.Diagnostics.Debug.WriteLine($"创建县级目录: {countyShapefilePath}");
                }

                // 使用ProgID创建Shapefile工作空间工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // 打开工作空间
                IWorkspace workspace = workspaceFactory.OpenFromFile(countyShapefilePath, 0);

                System.Diagnostics.Debug.WriteLine($"成功创建{countyName}的Shapefile工作空间: {countyShapefilePath}");
                return workspace;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建{countyName}Shapefile工作空间时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 创建LCXZGX Shapefile要素类
        /// </summary>
        /// <param name="workspace">Shapefile工作空间</param>
        /// <param name="geometryType">几何类型</param>
        /// <param name="spatialReference">空间参考</param>
        /// <returns>LCXZGX要素类接口</returns>
        private IFeatureClass CreateLCXZGXShapefile(IWorkspace workspace, esriGeometryType geometryType, ISpatialReference spatialReference)
        {
            try
            {
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

                // 检查要素类是否已存在
                string featureClassName = "LCXZGX";
                try
                {
                    IFeatureClass existingFeatureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                    if (existingFeatureClass != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"LCXZGX Shapefile已存在，将直接使用");
                        return existingFeatureClass;
                    }
                }
                catch
                {
                    // 要素类不存在，继续创建
                }

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

                // 使用FeatureClassFieldsTemplate添加业务字段
                FeatureClassFieldsTemplate.GenerateLcxzgxFields(fieldsEdit);

                // 创建要素类
                IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(
                    featureClassName,
                    fields,
                    null,
                    null,
                    esriFeatureType.esriFTSimple,
                    "Shape",
                    "");

                System.Diagnostics.Debug.WriteLine($"成功创建LCXZGX Shapefile要素类");
                return featureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建LCXZGX Shapefile要素类时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 将要素写入Shapefile
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

            try
            {
                // 初始化统计变量
                int totalFeatures = sourceFeatures.Count;
                int processedCount = 0;
                int successCount = 0;
                int errorCount = 0;

                // 🔥 修改: 动态计算进度更新的间隔，确保进度条平滑更新
                // 目标是整个循环过程中大约更新100次进度
                int updateInterval = Math.Max(1, totalFeatures / 100);

                System.Diagnostics.Debug.WriteLine($"开始向{countyName}的LCXZGX Shapefile插入{totalFeatures}个要素");

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
                        CopyFeatureAttributesForShapefile(sourceFeature, sourceFeatureClass, featureBuffer,
                            targetFeatureClass, fieldMappings, countyName);

                        // 执行要素插入操作
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        processedCount++;

                        // 🔥 修改: 使用动态计算的间隔来更新进度
                        if (processedCount % updateInterval == 0 || processedCount == totalFeatures)
                        {
                            // 将此过程的进度映射到总体进度的 25% 到 80% 区间
                            int percentage = 25 + (int)((processedCount / (double)totalFeatures) * 55);
                            progressCallback?.Invoke(percentage,
                                $"正在写入{countyName}的LCXZGX Shapefile... ({processedCount}/{totalFeatures})");
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
            }
        }

        /// <summary>
        /// 复制要素属性并进行字段映射（Shapefile版本）
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFeatureBuffer">目标要素缓冲区</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        private void CopyFeatureAttributesForShapefile(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            IFeatureBuffer targetFeatureBuffer,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName)
        {
            // 如果没有提供自定义映射，使用默认的字段映射配置
            if (fieldMappings == null || fieldMappings.Count == 0)
            {
                fieldMappings = GetDefaultShapefileFieldMappings();
            }

            // 遍历所有字段映射进行数据复制
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;    // 目标字段名
                string sourceFieldName = mapping.Value;  // 源字段名

                try
                {
                    // 获取源字段和目标字段的索引
                    int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                    int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);

                    // 只有当两个字段都存在时才进行数据复制
                    if (sourceFieldIndex != -1 && targetFieldIndex != -1)
                    {
                        object sourceValue = sourceFeature.get_Value(sourceFieldIndex);

                        // 执行特殊的字段值转换以符合Shapefile要求
                        object targetValue = ConvertFieldValueForShapefile(sourceValue, targetFieldName,
                            sourceFieldName, countyName);

                        targetFeatureBuffer.set_Value(targetFieldIndex, targetValue);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"复制{countyName}字段{targetFieldName}时出错: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 获取默认Shapefile字段映射
        /// </summary>
        /// <returns>字段映射字典</returns>
        private Dictionary<string, string> GetDefaultShapefileFieldMappings()
        {
            return new Dictionary<string, string>
            {
                { "bsm", "BSM" },           // 标识码
                { "ysdm", "YSDM" },         // 要素代码
                { "tbybh", "TBYBH" },       // 图斑预编号
                { "tbbh", "TBBH" },         // 图斑编号
                { "dlbm", "DLBM" },         // 地类编码
                { "dlmc", "DLMC" },         // 地类名称
                { "qsxz", "QSXZ" },         // 权属性质
                { "qsdwdm", "QSDWDM" },     // 权属单位代码
                { "qsdwmc", "QSDWMC" },     // 权属单位名称
                { "zldwdm", "ZLDWDM" },     // 坐落单位代码
                { "zldwmc", "ZLDWMC" },     // 坐落单位名称
                { "tbmj", "TBMJ" },         // 图斑面积
                { "kcdlbm", "KCDLBM" },     // 扣除地类编码
                { "kcxs", "KCXS" },         // 扣除地类系数
                { "kcmj", "KCMJ" },         // 扣除地类面积
                { "tbdlmj", "TBDLMJ" },     // 图斑地类面积
                { "bz", "BZ" }              // 备注
            };
        }

        /// <summary>
        /// 转换字段值以适配Shapefile
        /// </summary>
        /// <param name="sourceValue">源字段值</param>
        /// <param name="targetFieldName">目标字段名</param>
        /// <param name="sourceFieldName">源字段名</param>
        /// <param name="countyName">县名</param>
        /// <returns>转换后的字段值</returns>

        private object ConvertFieldValueForShapefile(object sourceValue, string targetFieldName,
    string sourceFieldName, string countyName)
        {
            // 处理空值情况
            if (sourceValue == null || sourceValue == DBNull.Value)
            {
                return GetDefaultValueForField(targetFieldName);
            }

            try
            {
                // 直接根据字段名进行特殊处理，不依赖 ShapefileFieldTemplate
                switch (targetFieldName.ToLower())
                {
                    case "qsxz":  // 权属性质转换
                        return ConvertPropertyRights(sourceValue);

                    case "dlmc":  // 地类名称转换
                        return ConvertLandTypeName(sourceValue);

                    case "tbmj":  // 图斑面积处理
                    case "kcmj":  // 扣除地类面积
                    case "tbdlmj": // 图斑地类面积
                        return ConvertAreaValue(sourceValue);

                    case "kcxs":  // 扣除地类系数 - 浮点型
                        if (float.TryParse(sourceValue.ToString(), out float floatValue))
                        {
                            return floatValue;
                        }
                        return 0.0f;

                    case "zldwmc": // 坐落单位名称
                        return string.IsNullOrEmpty(sourceValue?.ToString()) ? countyName : sourceValue.ToString();

                    case "qsdwmc": // 权属单位名称
                        return ConvertPropertyOwner(sourceValue, countyName);

                    // 字符串类型字段 - 确保长度不超过限制
                    case "bsm":
                        return TruncateString(sourceValue?.ToString(), 18);
                    case "ysdm":
                        return TruncateString(sourceValue?.ToString(), 10);
                    case "tbybh":
                    case "tbbh":
                        return TruncateString(sourceValue?.ToString(), 8);
                    case "dlbm":
                    case "kcdlbm":
                        return TruncateString(sourceValue?.ToString(), 5);
                    case "qsdwdm":
                    case "zldwdm":
                        return TruncateString(sourceValue?.ToString(), 19);
                    case "bz":
                        return TruncateString(sourceValue?.ToString(), 254);

                    default:
                        return sourceValue;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换{countyName}字段{targetFieldName}值时出错: {ex.Message}");
                return GetDefaultValueForField(targetFieldName);
            }
        }
        /// <summary>
        /// 截断字符串以符合Shapefile字段长度限制
        /// </summary>
        /// <param name="value">原始字符串值</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns>截断后的字符串</returns>
        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return value.Length > maxLength ? value.Substring(0, maxLength) : value;
        }
        /// <summary>
        /// 获取字段的默认值
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <returns>默认值</returns>
        private object GetDefaultValueForField(string fieldName)
        {
            // 根据字段名返回合适的默认值
            switch (fieldName.ToLower())
            {
                case "tbmj":
                case "kcmj":
                case "tbdlmj":
                    return 0.0; // 面积字段返回 double 类型默认值
                case "kcxs":
                    return 0.0f; // 系数字段返回 float 类型默认值
                default:
                    return ""; // 其他字段返回空字符串
            }
        }

        // 保持原有的转换方法不变
        private string ConvertPropertyRights(object value)
        {
            string strValue = value?.ToString() ?? "";
            switch (strValue)
            {
                case "1":
                case "20":
                    return "国有";
                case "2":
                case "30":
                    return "集体";
                case "3":
                case "40":
                    return "其他";
                default:
                    return strValue;
            }
        }

        private string ConvertLandTypeName(object value)
        {
            string strValue = value?.ToString() ?? "";

            if (strValue.StartsWith("03"))
            {
                return "林地";
            }
            else if (strValue.StartsWith("04"))
            {
                return "草地";
            }
            else if (strValue.StartsWith("11"))
            {
                return "湿地";
            }
            return strValue;
        }

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

        private string ConvertPropertyOwner(object value, string countyName)
        {
            string strValue = value?.ToString() ?? "";
            if (string.IsNullOrEmpty(strValue))
            {
                return $"{countyName}人民政府";
            }
            return strValue;
        }
    }
}