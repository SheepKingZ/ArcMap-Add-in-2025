using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 数据库导出工具类 - 将处理结果写入到button1创建的数据库中
    /// 负责将处理后的林地要素数据导出到县级文件地理数据库的LCXZGX表中
    /// </summary>
    public class ShapefileExporter
    {
        /// <summary>
        /// CGCS2000坐标系WKT定义
        /// </summary>
        private const string CGCS2000_WKT = @"GEOGCS[""GCS_China_Geodetic_Coordinate_System_2000"",DATUM[""D_China_2000"",SPHEROID[""CGCS2000"",6378137.0,298.257222101]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]]";

        /// <summary>
        /// 进度回调委托 - 用于向UI层报告处理进度
        /// </summary>
        /// <param name="percentage">完成百分比 (0-100)</param>
        /// <param name="message">当前操作描述信息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 将处理后的县级数据输出到数据库LCXZGX表
        /// LCXZGX表是林地管理中的核心要素表，存储图斑的基本属性信息
        /// </summary>
        /// <param name="processedFeatures">处理后的要素列表</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="countyName">县名</param>
        /// <param name="outputGDBPath">输出数据库路径</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="progressCallback">进度回调</param>
        public void ExportToDatabase(
            List<IFeature> processedFeatures,
            IFeatureClass sourceFeatureClass,
            string countyName,
            string outputGDBPath,
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

            if (string.IsNullOrEmpty(outputGDBPath))
            {
                throw new ArgumentException("输出数据库路径不能为空");
            }

            progressCallback?.Invoke(5, $"正在连接到{countyName}数据库...");

            // COM对象声明 - 需要在finally块中显式释放以避免内存泄漏
            IWorkspace targetWorkspace = null;
            IFeatureClass lcxzgxFeatureClass = null;

            try
            {
                // 打开县级数据库 - 每个县都有独立的.gdb文件
                // 数据库路径结构: outputGDBPath\县名\县名.gdb
                targetWorkspace = OpenCountyDatabase(outputGDBPath, countyName);
                if (targetWorkspace == null)
                {
                    throw new Exception($"无法打开{countyName}的数据库");
                }

                progressCallback?.Invoke(15, $"正在访问{countyName}的LCXZGX表...");

                // 获取LCXZGX要素类 - 这是标准的林地管理要素表
                lcxzgxFeatureClass = GetLCXZGXFeatureClass(targetWorkspace);
                if (lcxzgxFeatureClass == null)
                {
                    throw new Exception($"无法找到{countyName}数据库中的LCXZGX表");
                }

                progressCallback?.Invoke(25, $"开始向{countyName}的LCXZGX表写入数据...");

                // 执行数据写入操作 - 使用批量插入提高性能
                WriteFeaturesToDatabase(processedFeatures, sourceFeatureClass, lcxzgxFeatureClass,
                    fieldMappings, countyName, progressCallback);

                progressCallback?.Invoke(80, $"成功将 {processedFeatures.Count} 个要素写入到{countyName}的LCXZGX表");

                System.Diagnostics.Debug.WriteLine($"县{countyName}的数据已成功写入数据库LCXZGX表");

                // 🔥 新增：数据插入完成后立即执行转换
                progressCallback?.Invoke(85, $"开始转换{countyName}的LCXZGX数据到SLZYZC表...");
                PerformAutoConversion(countyName, outputGDBPath, progressCallback);

                progressCallback?.Invoke(100, $"{countyName}的数据导入和转换已全部完成");
            }
            finally
            {
                // 重要：释放ArcGIS COM对象，防止内存泄漏
                // ArcGIS COM对象需要显式释放，否则会导致内存占用持续增长
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
        /// 执行自动转换 - 在LCXZGX数据插入完成后自动转换为SLZYZC
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="outputGDBPath">数据库基础路径</param>
        /// <param name="progressCallback">进度回调</param>
        /// <summary>
        /// 执行自动转换 - 在LCXZGX数据插入完成后自动转换为SLZYZC和SLZYZC_DLTB
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="outputGDBPath">数据库基础路径</param>
        /// <param name="progressCallback">进度回调</param>
        private void PerformAutoConversion(string countyName, string outputGDBPath, ProgressCallback progressCallback)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"开始自动转换县{countyName}的数据从LCXZGX到SLZYZC");

                // 创建转换器实例
                var converter = new Convert2ResultTable();

                // 执行第一次转换 - LCXZGX转换为SLZYZC
                bool conversionSuccess = converter.ConvertLCXZGXToSLZYZC(
                    countyName,
                    outputGDBPath,
                    null, // 使用默认字段映射
                    (subPercentage, subMessage) =>
                    {
                        // 将转换进度映射到总进度的85%-90%区间
                        int totalPercentage = 85 + (subPercentage * 5 / 100);
                        progressCallback?.Invoke(totalPercentage, $"{countyName}: {subMessage}");
                    });

                if (conversionSuccess)
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的LCXZGX数据已成功自动转换为SLZYZC表");
                    progressCallback?.Invoke(90, $"{countyName}的SLZYZC数据转换成功完成");

                    // 继续执行第二次转换 - SLZYZC转换为SLZYZC_DLTB
                    System.Diagnostics.Debug.WriteLine($"开始自动转换县{countyName}的数据从SLZYZC到SLZYZC_DLTB");
                    var converter3 = new Convert3ResultTable();

                    bool conversion3Success = converter3.ConvertSLZYZCToDLTB(
                        countyName,
                        outputGDBPath,
                        null, // 使用默认字段映射
                        (subPercentage, subMessage) =>
                        {
                            // 将转换进度映射到总进度的90%-95%区间
                            int totalPercentage = 90 + (subPercentage * 5 / 100);
                            progressCallback?.Invoke(totalPercentage, $"{countyName}: {subMessage}");
                        });

                    if (conversion3Success)
                    {
                        System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC数据已成功自动转换为SLZYZC_DLTB表");
                        progressCallback?.Invoke(95, $"{countyName}的数据全部转换成功完成");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC_DLTB数据转换失败");
                        progressCallback?.Invoke(95, $"{countyName}的SLZYZC_DLTB数据转换失败，但SLZYZC数据已成功保存");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC数据转换失败");
                    progressCallback?.Invoke(95, $"{countyName}的数据转换失败，但LCXZGX数据已成功保存");
                }
            }
            catch (Exception ex)
            {
                // 转换失败不应影响主要的数据插入流程
                System.Diagnostics.Debug.WriteLine($"自动转换县{countyName}数据时出错: {ex.Message}");
                progressCallback?.Invoke(95, $"{countyName}的数据转换出错: {ex.Message}");

                // 记录错误但不抛出异常，确保主流程继续
                System.Diagnostics.Debug.WriteLine($"转换错误详情: {ex}");
            }
        }

        /// <summary>
        /// 批量输出多个县的数据到各自的数据库
        /// 提供批处理功能，提高多县数据处理的效率
        /// </summary>
        /// <param name="countyFeaturesMap">县级要素映射（县名 -> 要素列表）</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="outputGDBPath">输出数据库基础路径</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="progressCallback">进度回调</param>
        public void BatchExportToDatabase(
            Dictionary<string, List<IFeature>> countyFeaturesMap,
            IFeatureClass sourceFeatureClass,
            string outputGDBPath,
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

                    // 为每个县输出数据到数据库（包含自动转换）
                    // 注意：不传递子进度回调以避免进度报告混乱
                    ExportToDatabase(countyFeatures, sourceFeatureClass, countyName, outputGDBPath,
                        fieldMappings, null);

                    processedCounties++;

                    System.Diagnostics.Debug.WriteLine($"县{countyName}数据输出和转换完成 ({processedCounties}/{totalCounties})");
                }
                catch (Exception ex)
                {
                    // 错误处理策略：记录错误但继续处理其他县
                    // 这样可以确保一个县的错误不会影响其他县的数据处理
                    System.Diagnostics.Debug.WriteLine($"输出县{countyName}数据时出错: {ex.Message}");
                    processedCounties++;
                }
            }

            progressCallback?.Invoke(100, $"所有县的数据输出和转换完成 ({processedCounties}/{totalCounties})");
        }

        /// <summary>
        /// 打开县级数据库
        /// 使用ArcGIS File Geodatabase工厂创建数据库连接
        /// </summary>
        /// <param name="outputGDBPath">输出数据库基础路径</param>
        /// <param name="countyName">县名</param>
        /// <returns>数据库工作空间接口</returns>
        private IWorkspace OpenCountyDatabase(string outputGDBPath, string countyName)
        {
            try
            {
                // 构建县级数据库路径 - 标准路径结构：基础路径\县名\县名.gdb
                string countyGDBPath = System.IO.Path.Combine(outputGDBPath, countyName, countyName + ".gdb");

                // 验证目录结构的存在性
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(countyGDBPath)))
                {
                    throw new DirectoryNotFoundException($"县级目录不存在: {System.IO.Path.GetDirectoryName(countyGDBPath)}");
                }

                if (!Directory.Exists(countyGDBPath))
                {
                    throw new DirectoryNotFoundException($"县级数据库不存在: {countyGDBPath}");
                }

                // 使用ProgID创建File Geodatabase工厂
                // 这是ArcGIS COM对象的标准创建方式
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // 打开工作空间 - 参数0表示以读写模式打开
                IWorkspace workspace = workspaceFactory.OpenFromFile(countyGDBPath, 0);

                System.Diagnostics.Debug.WriteLine($"成功打开{countyName}的数据库: {countyGDBPath}");
                return workspace;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"打开{countyName}数据库时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 获取LCXZGX要素类
        /// LCXZGX是"林地持续行政管理"的简称，是林业管理中的标准要素表
        /// </summary>
        /// <param name="workspace">数据库工作空间</param>
        /// <returns>LCXZGX要素类接口</returns>
        private IFeatureClass GetLCXZGXFeatureClass(IWorkspace workspace)
        {
            try
            {
                // 将工作空间转换为要素工作空间以访问要素类
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

                // 打开LCXZGX要素类 - 这是预定义的表名
                IFeatureClass lcxzgxFeatureClass = featureWorkspace.OpenFeatureClass("LCXZGX");

                System.Diagnostics.Debug.WriteLine("成功获取LCXZGX要素类");
                return lcxzgxFeatureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取LCXZGX要素类时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 将要素写入数据库
        /// 使用批量插入模式提高性能，并提供详细的进度报告
        /// </summary>
        /// <param name="sourceFeatures">源要素列表</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        /// <param name="progressCallback">进度回调</param>
        private void WriteFeaturesToDatabase(
            List<IFeature> sourceFeatures,
            IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            ProgressCallback progressCallback)
        {
            // 创建要素缓冲区和插入游标
            // 使用批量插入模式(true参数)提高插入性能
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);

            try
            {
                // 初始化统计变量
                int totalFeatures = sourceFeatures.Count;
                int processedCount = 0;
                int successCount = 0;
                int errorCount = 0;

                System.Diagnostics.Debug.WriteLine($"开始向{countyName}的LCXZGX表插入{totalFeatures}个要素");

                // 逐个处理要素
                foreach (IFeature sourceFeature in sourceFeatures)
                {
                    try
                    {
                        // 复制几何对象 - 使用ShapeCopy创建几何的副本
                        if (sourceFeature.Shape != null)
                        {
                            featureBuffer.Shape = sourceFeature.ShapeCopy;
                        }

                        // 复制属性值并执行字段映射转换
                        CopyFeatureAttributesForDatabase(sourceFeature, sourceFeatureClass, featureBuffer,
                            targetFeatureClass, fieldMappings, countyName);

                        // 执行要素插入操作
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        processedCount++;

                        // 定期更新进度 - 每10个要素或最后一个要素时报告进度
                        // 进度范围：25%-80%，为转换操作保留15%
                        if (processedCount % 10 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = 25 + (int)((processedCount / (double)totalFeatures) * 55);
                            progressCallback?.Invoke(percentage,
                                $"正在写入{countyName}的LCXZGX表... ({processedCount}/{totalFeatures})");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"插入{countyName}要素时出错: {ex.Message}");
                        // 错误处理：记录错误但继续处理下一个要素
                        // 这样可以最大化数据的成功写入率
                    }
                }

                // 提交所有插入操作到数据库
                // Flush确保所有缓存的操作都被写入磁盘
                insertCursor.Flush();

                System.Diagnostics.Debug.WriteLine($"{countyName}数据写入完成: 成功{successCount}个, 失败{errorCount}个");
            }
            finally
            {
                // 重要：释放ArcGIS COM对象
                // 游标和缓冲区都是COM对象，必须显式释放
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
        /// 复制要素属性并进行字段映射（数据库版本）
        /// 处理源要素类到目标数据库表的字段映射和数据转换
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFeatureBuffer">目标要素缓冲区</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        private void CopyFeatureAttributesForDatabase(
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
                fieldMappings = GetDefaultDatabaseFieldMappings();
            }

            // 遍历所有字段映射进行数据复制
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;    // 目标表字段名
                string sourceFieldName = mapping.Value;  // 源表字段名

                try
                {
                    // 获取源字段和目标字段的索引
                    int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                    int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);

                    // 只有当两个字段都存在时才进行数据复制
                    if (sourceFieldIndex != -1 && targetFieldIndex != -1)
                    {
                        object sourceValue = sourceFeature.get_Value(sourceFieldIndex);

                        // 执行特殊的字段值转换以符合数据库要求
                        object targetValue = ConvertFieldValueForDatabase(sourceValue, targetFieldName,
                            sourceFieldName, countyName);

                        targetFeatureBuffer.set_Value(targetFieldIndex, targetValue);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"复制{countyName}字段{targetFieldName}时出错: {ex.Message}");
                    // 字段复制错误不影响其他字段的处理
                }
            }
        }

        /// <summary>
        /// 获取默认数据库字段映射
        /// 定义了源数据到LCXZGX表的标准字段映射关系
        /// 这些字段是林地管理中的标准字段
        /// </summary>
        /// <returns>字段映射字典</returns>
        private Dictionary<string, string> GetDefaultDatabaseFieldMappings()
        {
            return new Dictionary<string, string>
            {
                { "BSM", "BSM" },           // 标识码 - 唯一标识符
                { "YSDM", "YSDM" },         // 要素代码 - 要素类型编码
                { "TBYBH", "TBYBH" },       // 图斑预编号 - 临时编号
                { "TBBH", "TBBH" },         // 图斑编号 - 正式编号
                { "ZLDWDM", "ZLDWDM" },     // 坐落单位代码 - 行政区划代码
                { "ZLDWMC", "ZLDWMC" },     // 坐落单位名称 - 行政区划名称
                { "DLBM", "DLBM" },         // 地类编码 - 土地利用类型编码
                { "DLMC", "DLMC" },         // 地类名称 - 土地利用类型名称
                { "QSXZ", "QSXZ" },         // 权属性质 - 土地所有权性质
                { "QSDWDM", "QSDWDM" },     // 权属单位代码 - 权属单位编码
                { "QSDWMC", "QSDWMC" },     // 权属单位名称 - 权属单位名称
                { "TBMJ", "TBMJ" },         // 图斑面积 - 以平方米为单位
                { "KCDLBM", "KCDLBM" },     // 科次地类编码 - 详细分类编码
                { "KCDLMC", "KCDLMC" },     // 科次地类名称 - 详细分类名称
                { "KCXS", "KCXS" },         // 科次小数 - 科次系数
                { "TBDH", "TBDH" },         // 图斑代号 - 图斑标识代号
                { "BZ", "BZ" }              // 备注 - 附加说明信息
            };
        }

        /// <summary>
        /// 转换字段值以适配数据库
        /// 根据LCXZGX表的业务规则和数据要求进行字段值转换
        /// </summary>
        /// <param name="sourceValue">源字段值</param>
        /// <param name="targetFieldName">目标字段名</param>
        /// <param name="sourceFieldName">源字段名</param>
        /// <param name="countyName">县名</param>
        /// <returns>转换后的字段值</returns>
        private object ConvertFieldValueForDatabase(object sourceValue, string targetFieldName,
            string sourceFieldName, string countyName)
        {
            // 处理空值情况
            if (sourceValue == null || sourceValue == DBNull.Value)
            {
                return null;
            }

            try
            {
                // 根据LCXZGX表的字段要求进行特殊转换
                // 这些转换规则基于林地管理的业务需求
                switch (targetFieldName.ToUpper())
                {
                    case "QSXZ":  // 权属性质转换 - 将数字编码转换为中文描述
                        return ConvertPropertyRights(sourceValue);

                    case "DLMC":  // 地类名称转换 - 根据地类编码确定名称
                        return ConvertLandTypeName(sourceValue);

                    case "TBMJ":  // 图斑面积处理 - 确保数值类型正确
                        return ConvertAreaValue(sourceValue);

                    case "ZLDWMC": // 坐落单位名称 - 默认使用县名
                        return string.IsNullOrEmpty(sourceValue?.ToString()) ? countyName : sourceValue.ToString();

                    case "QSDWMC": // 权属单位名称 - 默认使用县政府
                        return ConvertPropertyOwner(sourceValue, countyName);

                    default:
                        return sourceValue; // 其他字段保持原值
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换{countyName}字段{targetFieldName}值时出错: {ex.Message}");
                return sourceValue; // 转换失败时返回原值，确保数据不丢失
            }
        }

        /// <summary>
        /// 转换权属性质
        /// 将数字编码转换为标准的中文权属描述
        /// 支持多种编码体系的兼容性
        /// </summary>
        /// <param name="value">权属性质编码</param>
        /// <returns>权属性质中文描述</returns>
        private string ConvertPropertyRights(object value)
        {
            string strValue = value?.ToString() ?? "";
            switch (strValue)
            {
                case "1":   // 第一套编码体系
                case "20":  // 第二套编码体系
                    return "国有";
                case "2":
                case "30":
                    return "集体";
                case "3":
                case "40":
                    return "其他";
                default:
                    return strValue; // 如果是已经是中文或其他格式，保持不变
            }
        }

        /// <summary>
        /// 转换地类名称
        /// 根据国家土地分类标准进行地类名称转换
        /// </summary>
        /// <param name="value">地类编码或名称</param>
        /// <returns>标准地类名称</returns>
        private string ConvertLandTypeName(object value)
        {
            string strValue = value?.ToString() ?? "";

            // 根据国家土地分类标准GB/T 21010-2017进行转换
            if (strValue.StartsWith("03"))
            {
                return "林地"; // 03开头为林地类
            }
            else if (strValue.StartsWith("04"))
            {
                return "草地"; // 04开头为草地类
            }
            else if (strValue.StartsWith("11"))
            {
                return "湿地"; // 11开头为湿地类
            }
            return strValue; // 其他情况保持原值
        }

        /// <summary>
        /// 转换面积值
        /// 确保面积值为正确的数值类型，支持多种输入格式
        /// </summary>
        /// <param name="value">面积值</param>
        /// <returns>标准化的面积数值</returns>
        private double? ConvertAreaValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            // 处理已经是数值类型的情况
            if (value is double || value is float || value is decimal)
            {
                return Convert.ToDouble(value);
            }
            // 尝试从字符串解析数值
            else if (double.TryParse(value.ToString(), out double areaValue))
            {
                return areaValue;
            }
            return null; // 无法转换时返回null
        }

        /// <summary>
        /// 转换权属单位名称
        /// 为空值提供默认的权属单位名称
        /// </summary>
        /// <param name="value">原权属单位名称</param>
        /// <param name="countyName">县名</param>
        /// <returns>标准化的权属单位名称</returns>
        private string ConvertPropertyOwner(object value, string countyName)
        {
            string strValue = value?.ToString() ?? "";
            if (string.IsNullOrEmpty(strValue))
            {
                // 默认权属单位设置为县人民政府
                return $"{countyName}人民政府";
            }
            return strValue;
        }
    }
}