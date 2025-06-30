using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 数据库输出管理器 - 将处理结果写入到button1创建的数据库中
    /// </summary>
    public class DatabaseOutputManager
    {
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 将处理后的县级数据输出到数据库LCXZGX表
        /// </summary>
        /// <param name="processedFeatures">处理后的要素列表</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="countyName">县名</param>
        /// <param name="outputGDBPath">输出数据库路径</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="progressCallback">进度回调</param>
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

            IWorkspace targetWorkspace = null;
            IFeatureClass lcxzgxFeatureClass = null;

            try
            {
                // 打开县级数据库
                targetWorkspace = OpenCountyDatabase(outputGDBPath, countyName);
                if (targetWorkspace == null)
                {
                    throw new Exception($"无法打开{countyName}的数据库");
                }

                progressCallback?.Invoke(15, $"正在访问{countyName}的LCXZGX表...");

                // 获取LCXZGX要素类
                lcxzgxFeatureClass = GetLCXZGXFeatureClass(targetWorkspace);
                if (lcxzgxFeatureClass == null)
                {
                    throw new Exception($"无法找到{countyName}数据库中的LCXZGX表");
                }

                progressCallback?.Invoke(25, $"开始向{countyName}的LCXZGX表写入数据...");

                // 将处理后的要素写入LCXZGX表
                WriteFeaturesToDatabase(processedFeatures, sourceFeatureClass, lcxzgxFeatureClass, 
                    fieldMappings, countyName, progressCallback);

                progressCallback?.Invoke(100, $"成功将 {processedFeatures.Count} 个要素写入到{countyName}的LCXZGX表");

                System.Diagnostics.Debug.WriteLine($"县{countyName}的数据已成功写入数据库LCXZGX表");
            }
            finally
            {
                // 释放资源
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
        /// 打开县级数据库
        /// </summary>
        private IWorkspace OpenCountyDatabase(string outputGDBPath, string countyName)
        {
            try
            {
                // 构建县级数据库路径
                string countyGDBPath = Path.Combine(outputGDBPath, countyName, countyName + ".gdb");
                
                if (!Directory.Exists(Path.GetDirectoryName(countyGDBPath)))
                {
                    throw new DirectoryNotFoundException($"县级目录不存在: {Path.GetDirectoryName(countyGDBPath)}");
                }

                if (!Directory.Exists(countyGDBPath))
                {
                    throw new DirectoryNotFoundException($"县级数据库不存在: {countyGDBPath}");
                }

                // 使用ProgID获取File Geodatabase工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // 打开工作空间
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
        /// </summary>
        private IFeatureClass GetLCXZGXFeatureClass(IWorkspace workspace)
        {
            try
            {
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;
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

                System.Diagnostics.Debug.WriteLine($"开始向{countyName}的LCXZGX表插入{totalFeatures}个要素");

                foreach (IFeature sourceFeature in sourceFeatures)
                {
                    try
                    {
                        // 复制几何
                        if (sourceFeature.Shape != null)
                        {
                            featureBuffer.Shape = sourceFeature.ShapeCopy;
                        }

                        // 复制属性值并进行字段映射
                        CopyFeatureAttributes(sourceFeature, sourceFeatureClass, featureBuffer, 
                            targetFeatureClass, fieldMappings, countyName);

                        // 插入要素
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        processedCount++;

                        // 更新进度
                        if (processedCount % 10 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = 25 + (int)((processedCount / (double)totalFeatures) * 70);
                            progressCallback?.Invoke(percentage,
                                $"正在写入{countyName}的LCXZGX表... ({processedCount}/{totalFeatures})");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"插入{countyName}要素时出错: {ex.Message}");
                        // 继续处理下一个要素
                    }
                }

                // 提交插入操作
                insertCursor.Flush();
                
                System.Diagnostics.Debug.WriteLine($"{countyName}数据写入完成: 成功{successCount}个, 失败{errorCount}个");
            }
            finally
            {
                // 释放资源
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
        /// 复制要素属性并进行字段映射
        /// </summary>
        private void CopyFeatureAttributes(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            IFeatureBuffer targetFeatureBuffer,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName)
        {
            // 默认字段映射（如果没有提供自定义映射）
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

                        // 处理特殊值转换
                        object targetValue = ConvertFieldValueForDatabase(sourceValue, targetFieldName, 
                            sourceFieldName, countyName);

                        targetFeatureBuffer.set_Value(targetFieldIndex, targetValue);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"复制{countyName}字段{targetFieldName}时出错: {ex.Message}");
                    // 继续处理其他字段
                }
            }
        }

        /// <summary>
        /// 获取默认字段映射
        /// </summary>
        private Dictionary<string, string> GetDefaultFieldMappings()
        {
            return new Dictionary<string, string>
            {
                { "BSM", "BSM" },           // 标识码
                { "YSDM", "YSDM" },         // 要素代码  
                { "TBYBH", "TBYBH" },       // 图斑预编号
                { "TBBH", "TBBH" },         // 图斑编号
                { "ZLDWDM", "ZLDWDM" },     // 坐落单位代码
                { "ZLDWMC", "ZLDWMC" },     // 坐落单位名称
                { "DLBM", "DLBM" },         // 地类编码
                { "DLMC", "DLMC" },         // 地类名称
                { "QSXZ", "QSXZ" },         // 权属性质
                { "QSDWDM", "QSDWDM" },     // 权属单位代码
                { "QSDWMC", "QSDWMC" },     // 权属单位名称
                { "TBMJ", "TBMJ" },         // 图斑面积
                { "KCDLBM", "KCDLBM" },     // 科次地类编码
                { "KCDLMC", "KCDLMC" },     // 科次地类名称
                { "KCXS", "KCXS" },         // 科次小数
                { "TBDH", "TBDH" },         // 图斑代号
                { "BZ", "BZ" }              // 备注
            };
        }

        /// <summary>
        /// 转换字段值以适配数据库
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
                // 根据LCXZGX表的字段要求进行特殊转换
                switch (targetFieldName.ToUpper())
                {
                    case "QSXZ":  // 权属性质转换
                        return ConvertPropertyRights(sourceValue);
                        
                    case "DLMC":  // 地类名称转换
                        return ConvertLandTypeName(sourceValue);
                        
                    case "TBMJ":  // 图斑面积处理
                        return ConvertAreaValue(sourceValue);
                        
                    case "ZLDWMC": // 坐落单位名称 - 使用县名
                        return string.IsNullOrEmpty(sourceValue?.ToString()) ? countyName : sourceValue.ToString();
                        
                    case "QSDWMC": // 权属单位名称
                        return ConvertPropertyOwner(sourceValue, countyName);
                        
                    default:
                        return sourceValue;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换{countyName}字段{targetFieldName}值时出错: {ex.Message}");
                return sourceValue; // 转换失败时返回原值
            }
        }

        /// <summary>
        /// 转换权属性质
        /// </summary>
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

        /// <summary>
        /// 转换地类名称
        /// </summary>
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

        /// <summary>
        /// 转换面积值
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
        /// 转换权属单位名称
        /// </summary>
        private string ConvertPropertyOwner(object value, string countyName)
        {
            string strValue = value?.ToString() ?? "";
            if (string.IsNullOrEmpty(strValue))
            {
                return $"{countyName}人民政府"; // 默认权属单位
            }
            return strValue;
        }

        /// <summary>
        /// 批量输出多个县的数据到各自的数据库
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
                throw new ArgumentException("县级要素映射不能为空");
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
                        $"正在处理县: {countyName} ({processedCounties + 1}/{totalCounties})");

                    OutputToDatabase(countyFeatures, sourceFeatureClass, countyName, outputGDBPath, 
                        fieldMappings, null); // 不传递子进度回调以避免混乱

                    processedCounties++;
                    
                    System.Diagnostics.Debug.WriteLine($"县{countyName}数据输出完成 ({processedCounties}/{totalCounties})");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"输出县{countyName}数据时出错: {ex.Message}");
                    // 继续处理下一个县
                    processedCounties++;
                }
            }

            progressCallback?.Invoke(100, $"所有县的数据输出完成 ({processedCounties}/{totalCounties})");
        }
    }
}