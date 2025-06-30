using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 结果表转换工具类 - 将SLZYZC表转换为SLZYZC_DLTB表
    /// SLZYZC_DLTB表是森林资源调查地类图斑表，用于存储转换后的地类调查数据
    /// </summary>
    public class Convert3ResultTable
    {
        /// <summary>
        /// 进度回调委托 - 用于向UI层报告处理进度
        /// </summary>
        /// <param name="percentage">完成百分比 (0-100)</param>
        /// <param name="message">当前操作描述信息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 将SLZYZC表转换为SLZYZC_DLTB表
        /// 执行数据转换、字段映射和业务规则处理
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="databasePath">县级数据库路径</param>
        /// <param name="fieldMappings">字段映射配置（SLZYZC_DLTB字段名 -> SLZYZC字段名）</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>转换是否成功</returns>
        public bool ConvertSLZYZCToDLTB(
            string countyName,
            string databasePath,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null)
        {
            // 参数验证 - 确保输入数据的有效性
            if (string.IsNullOrEmpty(countyName))
            {
                throw new ArgumentException("县名不能为空");
            }

            if (string.IsNullOrEmpty(databasePath))
            {
                throw new ArgumentException("数据库路径不能为空");
            }

            progressCallback?.Invoke(5, $"正在连接到{countyName}数据库...");

            // COM对象声明 - 需要在finally块中显式释放以避免内存泄漏
            IWorkspace workspace = null;
            IFeatureClass slzyzcFeatureClass = null;
            IFeatureClass dltbFeatureClass = null;

            try
            {
                // 打开县级数据库
                workspace = OpenCountyDatabase(databasePath, countyName);
                if (workspace == null)
                {
                    throw new Exception($"无法打开{countyName}的数据库");
                }

                progressCallback?.Invoke(15, $"正在访问{countyName}的SLZYZC表...");

                // 获取源表 - SLZYZC要素类
                slzyzcFeatureClass = GetFeatureClass(workspace, "SLZYZC");
                if (slzyzcFeatureClass == null)
                {
                    throw new Exception($"无法找到{countyName}数据库中的SLZYZC表");
                }

                progressCallback?.Invoke(25, $"正在访问{countyName}的SLZYZC_DLTB表...");

                // 获取目标表 - SLZYZC_DLTB要素类
                dltbFeatureClass = GetFeatureClass(workspace, "SLZYZC_DLTB");
                if (dltbFeatureClass == null)
                {
                    throw new Exception($"无法找到{countyName}数据库中的SLZYZC_DLTB表");
                }

                progressCallback?.Invoke(35, $"开始转换{countyName}的数据...");

                // 执行数据转换操作
                int convertedCount = ConvertFeatures(
                    slzyzcFeatureClass,
                    dltbFeatureClass,
                    fieldMappings,
                    countyName,
                    progressCallback);

                progressCallback?.Invoke(100, $"成功转换 {convertedCount} 个要素到{countyName}的SLZYZC_DLTB表");

                System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC数据已成功转换为SLZYZC_DLTB表");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换{countyName}数据时出错: {ex.Message}");
                progressCallback?.Invoke(0, $"转换{countyName}数据失败: {ex.Message}");
                throw;
            }
            finally
            {
                // 重要：释放ArcGIS COM对象，防止内存泄漏
                if (slzyzcFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(slzyzcFeatureClass);
                }
                if (dltbFeatureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dltbFeatureClass);
                }
                if (workspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
                }
            }
        }

        /// <summary>
        /// 批量转换多个县的SLZYZC表为SLZYZC_DLTB表
        /// 提供批处理功能，提高多县数据处理的效率
        /// </summary>
        /// <param name="countyDatabasePaths">县级数据库路径映射（县名 -> 数据库路径）</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>批量转换结果（县名 -> 是否成功）</returns>
        public Dictionary<string, bool> BatchConvertSLZYZCToDLTB(
            Dictionary<string, string> countyDatabasePaths,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null)
        {
            if (countyDatabasePaths == null || countyDatabasePaths.Count == 0)
            {
                throw new ArgumentException("县级数据库路径映射不能为空");
            }

            var results = new Dictionary<string, bool>();
            int totalCounties = countyDatabasePaths.Count;
            int processedCounties = 0;

            progressCallback?.Invoke(0, $"开始批量转换{totalCounties}个县的数据...");

            // 遍历每个县的数据进行转换
            foreach (var countyData in countyDatabasePaths)
            {
                string countyName = countyData.Key;
                string databasePath = countyData.Value;

                try
                {
                    // 计算总体进度 - 基于已处理的县数量
                    int overallProgress = (processedCounties * 100) / totalCounties;
                    progressCallback?.Invoke(overallProgress, $"正在转换县: {countyName} ({processedCounties + 1}/{totalCounties})");

                    // 为每个县执行转换
                    // 注意：不传递子进度回调以避免进度报告混乱
                    bool success = ConvertSLZYZCToDLTB(countyName, databasePath, fieldMappings, null);
                    results[countyName] = success;

                    processedCounties++;

                    System.Diagnostics.Debug.WriteLine($"县{countyName}数据转换完成 ({processedCounties}/{totalCounties})");
                }
                catch (Exception ex)
                {
                    // 错误处理策略：记录错误但继续处理其他县
                    System.Diagnostics.Debug.WriteLine($"转换县{countyName}数据时出错: {ex.Message}");
                    results[countyName] = false;
                    processedCounties++;
                }
            }

            progressCallback?.Invoke(100, $"所有县的数据转换完成 ({processedCounties}/{totalCounties})");
            return results;
        }

        /// <summary>
        /// 打开县级数据库
        /// 使用ArcGIS File Geodatabase工厂创建数据库连接
        /// </summary>
        /// <param name="basePath">数据库基础路径</param>
        /// <param name="countyName">县名</param>
        /// <returns>数据库工作空间接口</returns>
        private IWorkspace OpenCountyDatabase(string basePath, string countyName)
        {
            try
            {
                // 构建县级数据库路径 - 标准路径结构：基础路径\县名\县名.gdb
                string countyGDBPath = System.IO.Path.Combine(basePath, countyName, countyName + ".gdb");

                // 验证数据库路径的存在性
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(countyGDBPath)))
                {
                    throw new DirectoryNotFoundException($"县级目录不存在: {System.IO.Path.GetDirectoryName(countyGDBPath)}");
                }

                if (!Directory.Exists(countyGDBPath))
                {
                    throw new DirectoryNotFoundException($"县级数据库不存在: {countyGDBPath}");
                }

                // 使用ProgID创建File Geodatabase工厂
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
        /// 获取指定名称的要素类
        /// 通用方法，可用于获取SLZYZC、SLZYZC_DLTB等要素类
        /// </summary>
        /// <param name="workspace">数据库工作空间</param>
        /// <param name="featureClassName">要素类名称</param>
        /// <returns>要素类接口</returns>
        private IFeatureClass GetFeatureClass(IWorkspace workspace, string featureClassName)
        {
            try
            {
                // 将工作空间转换为要素工作空间以访问要素类
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

                // 打开指定的要素类
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(featureClassName);

                System.Diagnostics.Debug.WriteLine($"成功获取{featureClassName}要素类");
                return featureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取{featureClassName}要素类时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 执行要素转换操作
        /// 从SLZYZC表读取数据，转换后写入SLZYZC_DLTB表
        /// </summary>
        /// <param name="sourceFeatureClass">源要素类（SLZYZC）</param>
        /// <param name="targetFeatureClass">目标要素类（SLZYZC_DLTB）</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>转换的要素数量</returns>
        private int ConvertFeatures(
            IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            ProgressCallback progressCallback)
        {
            // 创建要素缓冲区和插入游标
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);
            IFeatureCursor sourceCursor = null;

            try
            {
                // 获取源数据总数
                int totalFeatures = sourceFeatureClass.FeatureCount(null);
                int processedCount = 0;
                int successCount = 0;
                int errorCount = 0;

                System.Diagnostics.Debug.WriteLine($"开始转换{countyName}的数据：{totalFeatures}个要素从SLZYZC到SLZYZC_DLTB");

                // 创建查询游标读取所有源要素
                sourceCursor = sourceFeatureClass.Search(null, false);
                IFeature sourceFeature;

                // 如果没有提供自定义映射，使用默认的字段映射配置
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    fieldMappings = CreateSLZYZC2DLTBFieldsMap();
                }

                // 逐个处理要素
                while ((sourceFeature = sourceCursor.NextFeature()) != null)
                {
                    try
                    {
                        // 复制几何对象 - 使用ShapeCopy创建几何的副本
                        if (sourceFeature.Shape != null)
                        {
                            featureBuffer.Shape = sourceFeature.ShapeCopy;
                        }

                        // 复制属性值并执行字段映射转换
                        CopyAndConvertFeatureAttributes(
                            sourceFeature,
                            sourceFeatureClass,
                            featureBuffer,
                            targetFeatureClass,
                            fieldMappings,
                            countyName);

                        // 执行要素插入操作
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        processedCount++;

                        // 定期更新进度 - 每50个要素或最后一个要素时报告进度
                        if (processedCount % 50 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = 35 + (int)((processedCount / (double)totalFeatures) * 60);
                            progressCallback?.Invoke(percentage,
                                $"正在转换{countyName}的数据... ({processedCount}/{totalFeatures})");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"转换{countyName}要素时出错: {ex.Message}");
                        // 错误处理：记录错误但继续处理下一个要素
                    }
                    finally
                    {
                        // 释放当前要素对象
                        if (sourceFeature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceFeature);
                        }
                    }
                }

                // 提交所有插入操作到数据库
                insertCursor.Flush();

                System.Diagnostics.Debug.WriteLine($"{countyName}数据转换完成: 成功{successCount}个, 失败{errorCount}个");
                return successCount;
            }
            finally
            {
                // 重要：释放ArcGIS COM对象
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
        /// 复制要素属性并进行字段映射转换
        /// 处理SLZYZC表到SLZYZC_DLTB表的字段映射和数据转换
        /// </summary>
        /// <param name="sourceFeature">源要素（SLZYZC）</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFeatureBuffer">目标要素缓冲区（SLZYZC_DLTB）</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        private void CopyAndConvertFeatureAttributes(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            IFeatureBuffer targetFeatureBuffer,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName)
        {
            // 遍历所有字段映射进行数据复制和转换
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;    // SLZYZC_DLTB表字段名
                string sourceFieldName = mapping.Value;  // SLZYZC表字段名

                try
                {
                    // 获取目标字段的索引
                    int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);
                    if (targetFieldIndex == -1)
                    {
                        System.Diagnostics.Debug.WriteLine($"目标字段 {targetFieldName} 在SLZYZC_DLTB表中不存在");
                        continue;
                    }

                    // 处理特殊的字段映射规则
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
                    System.Diagnostics.Debug.WriteLine($"转换{countyName}字段{targetFieldName}时出错: {ex.Message}");
                    // 字段转换错误不影响其他字段的处理
                }
            }
        }

        /// <summary>
        /// 处理特殊的字段映射规则
        /// 包括字段合并、计算等复杂映射逻辑
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFieldName">目标字段名</param>
        /// <param name="sourceFieldName">源字段名（或特殊规则描述）</param>
        /// <param name="countyName">县名</param>
        /// <returns>转换后的字段值</returns>
        private object ProcessSpecialFieldMapping(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            string targetFieldName,
            string sourceFieldName,
            string countyName)
        {
            try
            {
                // 处理特殊的字段映射规则
                switch (targetFieldName)
                {
                    case "XZQMC":
                        // 直接使用县名参数，确保包含"县"字
                        return EnsureCountySuffix(countyName);

                    // 这里可以添加其他特殊字段处理规则
                    // case "特殊字段名":
                    //    return 特殊处理逻辑;

                    default:
                        // 普通字段映射
                        if (!string.IsNullOrEmpty(sourceFieldName))
                        {
                            int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                            if (sourceFieldIndex != -1)
                            {
                                object sourceValue = sourceFeature.get_Value(sourceFieldIndex);
                                return ConvertFieldValueForDLTB(sourceValue, targetFieldName, sourceFieldName, countyName);
                            }
                        }
                        break;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理特殊字段映射 {targetFieldName} 时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 确保县名以"县"字结尾
        /// </summary>
        /// <param name="countyName">原始县名</param>
        /// <returns>确保带有"县"字的县名</returns>
        private string EnsureCountySuffix(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                return string.Empty;
            }

            // 如果县名不以"县"结尾，添加"县"字
            if (!countyName.EndsWith("县") && !countyName.EndsWith("市") &&
                !countyName.EndsWith("区") && !countyName.EndsWith("旗"))
            {
                return countyName + "县";
            }

            return countyName; // 已经包含行政区单位名称，直接返回
        }

        /// <summary>
        /// 创建SLZYZC到SLZYZC_DLTB的字段映射配置
        /// </summary>
        /// <returns>字段映射字典（SLZYZC_DLTB字段名 -> SLZYZC字段名）</returns>
        public static Dictionary<string, string> CreateSLZYZC2DLTBFieldsMap()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            result.Add("YSDM", "YSDM");            // 要素代码
            result.Add("XZQDM", "XZQDM");          // 行政区代码
            result.Add("XZQMC", "XZQMC");          // 行政区名称
            result.Add("GTDCTBBSM", "GTDCTBBSM");  // 国土调查图斑编码
            result.Add("GTDCTBBH", "GTDCTBBH");    // 国土调查图斑编号
            result.Add("GTDCDLBM", "GTDCDLBM");    // 国土调查地类编码
            result.Add("GTDCDLMC", "GTDCDLMC");    // 国土调查地类名称
            result.Add("GTDCTDQS", "GTDCTDQS");    // 国土调查地类名称
            result.Add("QSDWDM", "QSDWDM");        // 权属单位代码
            result.Add("QSDWMC", "QSDWMC");        // 权属单位名称
            result.Add("ZLDWDM", "ZLDWDM");        // 坐落单位代码
            result.Add("ZLDWMC", "ZLDWMC");        // 坐落单位名称
            result.Add("GTDCTBMJ", "GTDCTBMJ");    // 国土调查图斑面积
            result.Add("CZKFBJMJ", "CZKFBJMJ");    // 城镇开发边界面积 - 特殊处理

            return result;
        }

        /// <summary>
        /// 转换字段值以适配SLZYZC_DLTB表
        /// 根据SLZYZC_DLTB表的业务规则和数据要求进行字段值转换
        /// </summary>
        /// <param name="sourceValue">源字段值</param>
        /// <param name="targetFieldName">目标字段名（SLZYZC_DLTB表）</param>
        /// <param name="sourceFieldName">源字段名（SLZYZC表）</param>
        /// <param name="countyName">县名</param>
        /// <returns>转换后的字段值</returns>
        private object ConvertFieldValueForDLTB(
            object sourceValue,
            string targetFieldName,
            string sourceFieldName,
            string countyName)
        {
            // 处理空值情况
            if (sourceValue == null || sourceValue == DBNull.Value)
            {
                return null;
            }

            try
            {
                // 根据SLZYZC_DLTB表的字段要求进行特殊转换
                switch (targetFieldName.ToUpper())
                {
                    case "XZQDM":
                        // 行政区代码可能需要特殊处理
                        return sourceValue.ToString();

                    case "TBMJ":
                    case "MJ":
                        // 面积字段确保为数值类型
                        if (double.TryParse(sourceValue.ToString(), out double area))
                        {
                            return area;
                        }
                        return 0.0;

                    // 可以根据需要添加其他字段的特殊转换规则

                    default:
                        // 默认保持原值
                        return sourceValue;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换{countyName}字段{targetFieldName}值时出错: {ex.Message}");
                return sourceValue; // 转换失败时返回原值，确保数据不丢失
            }
        }

        /// <summary>
        /// 验证转换配置的有效性
        /// 检查字段映射、数据库连接等转换前的准备工作
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="databasePath">数据库路径</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <returns>验证是否通过</returns>
        public bool ValidateConversionConfiguration(
            string countyName,
            string databasePath,
            Dictionary<string, string> fieldMappings)
        {
            try
            {
                // 验证基本参数
                if (string.IsNullOrEmpty(countyName) || string.IsNullOrEmpty(databasePath))
                {
                    return false;
                }

                // 验证数据库路径存在性
                string countyGDBPath = System.IO.Path.Combine(databasePath, countyName, countyName + ".gdb");
                if (!Directory.Exists(countyGDBPath))
                {
                    System.Diagnostics.Debug.WriteLine($"县级数据库不存在: {countyGDBPath}");
                    return false;
                }

                // 验证字段映射配置
                if (fieldMappings == null)
                {
                    fieldMappings = CreateSLZYZC2DLTBFieldsMap();
                }

                if (fieldMappings.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("字段映射配置为空");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"转换配置验证通过: {countyName}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"验证转换配置时出错: {ex.Message}");
                return false;
            }
        }
    }
}