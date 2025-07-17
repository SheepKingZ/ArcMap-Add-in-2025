using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using TestArcMapAddin2;
//using TestArcMapAddin2.ShapefileUtils;

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

            // 从输出路径提取真实的县名
            string extractedCountyName = ExtractCountyNameFromOutputPath(outputPath, countyName);
            if (!string.IsNullOrEmpty(extractedCountyName))
            {
                countyName = extractedCountyName;
                System.Diagnostics.Debug.WriteLine($"从输出路径提取到县名: '{countyName}'");
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

                // 执行SLZYZC_DLTB操作
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
        /// 从输出路径中提取县名
        /// 从第二级文件夹名称中提取县名，例如："绥中县（211421）全民所有自然资源资产清查数据成果"
        /// </summary>
        /// <param name="outputPath">输出基础路径</param>
        /// <param name="originalCountyName">原始县名</param>
        /// <returns>提取的县名</returns>
        private string ExtractCountyNameFromOutputPath(string outputPath, string originalCountyName)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"开始从输出路径提取县名: outputPath='{outputPath}', originalCountyName='{originalCountyName}'");

                // 构建县级文件夹路径
                string countyCode = Utils.CountyCodeMapper.GetCountyCode(originalCountyName);
                string expectedFolderName = $"{originalCountyName}（{countyCode}）全民所有自然资源资产清查数据成果";
                string countyFolderPath = System.IO.Path.Combine(outputPath, expectedFolderName);

                System.Diagnostics.Debug.WriteLine($"期望的文件夹路径: '{countyFolderPath}'");

                // 如果期望的文件夹存在，直接使用原始县名
                if (Directory.Exists(countyFolderPath))
                {
                    System.Diagnostics.Debug.WriteLine($"找到期望的文件夹，使用原始县名: '{originalCountyName}'");
                    return originalCountyName;
                }

                // 如果期望的文件夹不存在，搜索类似的文件夹
                if (Directory.Exists(outputPath))
                {
                    string[] directories = Directory.GetDirectories(outputPath);
                    System.Diagnostics.Debug.WriteLine($"输出路径下共有 {directories.Length} 个文件夹");

                    foreach (string directory in directories)
                    {
                        string folderName = System.IO.Path.GetFileName(directory);
                        System.Diagnostics.Debug.WriteLine($"检查文件夹: '{folderName}'");

                        // 尝试多种县名提取模式
                        string extractedName = ExtractCountyNameFromFolderName(folderName);
                        if (!string.IsNullOrEmpty(extractedName))
                        {
                            // 检查提取的县名是否与原始县名匹配
                            if (IsCountyNameMatch(extractedName, originalCountyName))
                            {
                                System.Diagnostics.Debug.WriteLine($"从文件夹名 '{folderName}' 提取到匹配的县名: '{extractedName}'");
                                return extractedName;
                            }
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"无法从输出路径提取县名，使用原始县名: '{originalCountyName}'");
                return originalCountyName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从输出路径提取县名时出错: {ex.Message}");
                return originalCountyName;
            }
        }

        /// <summary>
        /// 从文件夹名称中提取县名
        /// </summary>
        /// <param name="folderName">文件夹名称</param>
        /// <returns>提取的县名</returns>
        private string ExtractCountyNameFromFolderName(string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(folderName))
                {
                    return null;
                }

                // 模式1：匹配 "县名（代码）其他文本" 格式
                var match1 = System.Text.RegularExpressions.Regex.Match(folderName, @"^([^（]+[县市区])（\d+）");
                if (match1.Success)
                {
                    string extractedName = match1.Groups[1].Value;
                    System.Diagnostics.Debug.WriteLine($"模式1匹配成功: '{extractedName}'");
                    return extractedName;
                }

                // 模式2：匹配开头的县市区名称
                var match2 = System.Text.RegularExpressions.Regex.Match(folderName, @"^([^（）]+[县市区])");
                if (match2.Success)
                {
                    string extractedName = match2.Groups[1].Value;
                    System.Diagnostics.Debug.WriteLine($"模式2匹配成功: '{extractedName}'");
                    return extractedName;
                }

                // 模式3：匹配任何位置的县市区名称
                var match3 = System.Text.RegularExpressions.Regex.Match(folderName, @"([^（）\s]+[县市区])");
                if (match3.Success)
                {
                    string extractedName = match3.Groups[1].Value;
                    System.Diagnostics.Debug.WriteLine($"模式3匹配成功: '{extractedName}'");
                    return extractedName;
                }

                System.Diagnostics.Debug.WriteLine($"无法从文件夹名 '{folderName}' 提取县名");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从文件夹名提取县名时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 检查两个县名是否匹配
        /// </summary>
        /// <param name="extractedName">提取的县名</param>
        /// <param name="originalName">原始县名</param>
        /// <returns>是否匹配</returns>
        private bool IsCountyNameMatch(string extractedName, string originalName)
        {
            if (string.IsNullOrEmpty(extractedName) || string.IsNullOrEmpty(originalName))
            {
                return false;
            }

            // 直接匹配
            if (extractedName.Equals(originalName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // 移除非中文字符后匹配
            string cleanExtracted = System.Text.RegularExpressions.Regex.Replace(extractedName, @"[^\u4e00-\u9fa5]", "");
            string cleanOriginal = System.Text.RegularExpressions.Regex.Replace(originalName, @"[^\u4e00-\u9fa5]", "");

            if (cleanExtracted.Equals(cleanOriginal, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // 包含匹配
            if (extractedName.Contains(originalName) || originalName.Contains(extractedName))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 执行自动转换 - 根据SLZY_DLTB数据生成SLZYZC_DLTB
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="progressCallback">进度回调</param>
        private void PerformAutoConversion(string countyName, string outputPath, ProgressCallback progressCallback)
        {
            try
            {
                progressCallback?.Invoke(80, $"准备为{countyName}生成SLZYZC_DLTB数据...");
                System.Diagnostics.Debug.WriteLine($"开始为县{countyName}生成SLZYZC_DLTB数据");

                // 构建已存在的SLZYZC_DLTB文件路径
                string countyCode = Utils.CountyCodeMapper.GetCountyCode(countyName);
                string countyFolderName = $"{countyName}（{countyCode}）全民所有自然资源资产清查数据成果";
                string countyPath = System.IO.Path.Combine(outputPath, countyFolderName);
                string dataSetPath = System.IO.Path.Combine(countyPath, "清查数据集");
                string forestPath = System.IO.Path.Combine(dataSetPath, "森林");
                string spatialDataPath = System.IO.Path.Combine(forestPath, "空间数据");
                string slzyzcDltbShapefilePath = System.IO.Path.Combine(spatialDataPath, "SLZYZC_DLTB.shp");

                System.Diagnostics.Debug.WriteLine($"SLZYZC_DLTB目标文件路径: {slzyzcDltbShapefilePath}");

                // 检查SLZYZC_DLTB文件是否存在
                if (!File.Exists(slzyzcDltbShapefilePath))
                {
                    System.Diagnostics.Debug.WriteLine($"错误: SLZYZC_DLTB文件不存在: {slzyzcDltbShapefilePath}");
                    progressCallback?.Invoke(99, $"{countyName}的SLZYZC_DLTB文件不存在，请先创建空的Shapefile结构");
                    return;
                }

                // 获取对应县的SLZY_DLTB源数据
                string slzyDltbPath = GetSLZYDLTBShapefilePath(countyName);
                if (string.IsNullOrEmpty(slzyDltbPath))
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 未找到{countyName}的SLZY_DLTB源数据，跳过转换");
                    progressCallback?.Invoke(99, $"{countyName}的SLZY_DLTB源数据不存在，跳过转换");
                    return;
                }

                progressCallback?.Invoke(82, $"正在处理{countyName}的SLZY_DLTB数据...");

                // 获取对应县的CZKFBJ数据
                string czkfbjPath = GetCZKFBJShapefilePath(countyName);
                System.Diagnostics.Debug.WriteLine($"CZKFBJ数据路径: {czkfbjPath ?? "未找到"}");

                // 执行SLZYZC_DLTB生成操作
                bool conversionSuccess = GenerateSLZYZCDLTB(
                    slzyDltbPath,
                    czkfbjPath,
                    slzyzcDltbShapefilePath,
                    countyName,
                    (subPercentage, subMessage) =>
                    {
                        // 将子进度映射到总进度的82%-99%区间
                        int totalPercentage = 82 + (subPercentage * 17 / 100);
                        progressCallback?.Invoke(totalPercentage, $"{countyName}: {subMessage}");
                    });

                if (conversionSuccess)
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC_DLTB数据生成成功");
                    progressCallback?.Invoke(99, $"{countyName}的SLZYZC_DLTB数据生成成功");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的SLZYZC_DLTB数据生成失败");
                    progressCallback?.Invoke(99, $"{countyName}的SLZYZC_DLTB数据生成失败");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成{countyName}的SLZYZC_DLTB数据时出错: {ex.Message}");
                progressCallback?.Invoke(99, $"{countyName}的SLZYZC_DLTB数据生成出错: {ex.Message}");

                // 记录详细错误信息
                System.Diagnostics.Debug.WriteLine($"错误详情: {ex}");
            }
        }

        /// <summary>
        /// 生成SLZYZC_DLTB数据（写入到已存在的文件中）
        /// </summary>
        /// <param name="slzyDltbPath">SLZY_DLTB源数据路径</param>
        /// <param name="czkfbjPath">CZKFBJ数据路径</param>
        /// <param name="outputPath">输出SLZYZC_DLTB文件路径（已存在）</param>
        /// <param name="countyName">县名</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>是否成功</returns>
        private bool GenerateSLZYZCDLTB(
            string slzyDltbPath,
            string czkfbjPath,
            string outputPath,
            string countyName,
            ProgressCallback progressCallback)
        {
            // COM对象声明
            IWorkspace slzyDltbWorkspace = null;
            IFeatureClass slzyDltbFeatureClass = null;
            IWorkspace czkfbjWorkspace = null;
            IFeatureClass czkfbjFeatureClass = null;
            IFeatureClass outputFeatureClass = null;

            try
            {
                progressCallback?.Invoke(5, "正在打开SLZY_DLTB数据...");

                // 打开SLZY_DLTB源数据
                var slzyResult = OpenShapefileFeatureClass(slzyDltbPath);
                slzyDltbWorkspace = slzyResult.workspace;
                slzyDltbFeatureClass = slzyResult.featureClass;

                if (slzyDltbFeatureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法打开SLZY_DLTB数据: {slzyDltbPath}");
                    return false;
                }

                int totalFeatures = slzyDltbFeatureClass.FeatureCount(null);
                System.Diagnostics.Debug.WriteLine($"SLZY_DLTB包含{totalFeatures}个要素");

                progressCallback?.Invoke(15, "正在打开CZKFBJ数据...");

                // 打开CZKFBJ数据（如果存在）
                if (!string.IsNullOrEmpty(czkfbjPath))
                {
                    var czkfbjResult = OpenShapefileFeatureClass(czkfbjPath);
                    czkfbjWorkspace = czkfbjResult.workspace;
                    czkfbjFeatureClass = czkfbjResult.featureClass;

                    if (czkfbjFeatureClass != null)
                    {
                        int czkfbjCount = czkfbjFeatureClass.FeatureCount(null);
                        System.Diagnostics.Debug.WriteLine($"CZKFBJ包含{czkfbjCount}个要素");
                    }
                }

                progressCallback?.Invoke(25, "正在打开已存在的SLZYZC_DLTB文件...");

                // 打开已存在的SLZYZC_DLTB文件
                var outputResult = OpenShapefileFeatureClass(outputPath);
                if (outputResult.featureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法打开已存在的SLZYZC_DLTB文件: {outputPath}");
                    return false;
                }
                outputFeatureClass = outputResult.featureClass;

                // 清空现有数据
                ClearExistingShapefileData(outputFeatureClass);

                progressCallback?.Invoke(35, "开始复制和处理数据...");

                // 执行数据复制和处理
                int processedCount = CopyAndProcessFeatures(
                    slzyDltbFeatureClass,
                    czkfbjFeatureClass,
                    outputFeatureClass,
                    countyName,
                    (subPercentage, subMessage) =>
                    {
                        // 将数据处理进度映射到35%-95%区间
                        int totalPercentage = 35 + (subPercentage * 60 / 100);
                        progressCallback?.Invoke(totalPercentage, subMessage);
                    });

                progressCallback?.Invoke(100, $"成功处理{processedCount}个要素");
                return processedCount > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成SLZYZC_DLTB时出错: {ex.Message}");
                return false;
            }
            finally
            {
                // 释放所有COM对象
                if (slzyDltbFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(slzyDltbFeatureClass);
                if (slzyDltbWorkspace != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(slzyDltbWorkspace);
                if (czkfbjFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjFeatureClass);
                if (czkfbjWorkspace != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjWorkspace);
                if (outputFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(outputFeatureClass);
            }
        }

        /// <summary>
        /// 获取指定县的SLZY_DLTB数据路径
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <returns>SLZY_DLTB文件路径</returns>
        private string GetSLZYDLTBShapefilePath(string countyName)
        {
            try
            {
                var slzyDltbFiles = SharedDataManager.GetSLZYDLTBFiles();

                foreach (var fileInfo in slzyDltbFiles)
                {
                    if (fileInfo.DisplayName.Equals(countyName, StringComparison.OrdinalIgnoreCase) ||
                        fileInfo.DisplayName.Contains(countyName))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到{countyName}的SLZY_DLTB文件: {fileInfo.FullPath}");
                        return fileInfo.FullPath;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"未找到{countyName}的SLZY_DLTB文件");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取{countyName}的SLZY_DLTB路径时出错: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// 复制和处理要素数据
        /// </summary>
        /// <param name="sourceFeatureClass">源要素类(SLZY_DLTB)</param>
        /// <param name="czkfbjFeatureClass">CZKFBJ要素类</param>
        /// <param name="targetFeatureClass">目标要素类(SLZYZC_DLTB)</param>
        /// <param name="countyName">县名</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>处理的要素数量</returns>
        private int CopyAndProcessFeatures(
            IFeatureClass sourceFeatureClass,
            IFeatureClass czkfbjFeatureClass,
            IFeatureClass targetFeatureClass,
            string countyName,
            ProgressCallback progressCallback)
        {
            IFeatureCursor sourceCursor = null;
            IFeatureBuffer targetBuffer = null;
            IFeatureCursor insertCursor = null;
            IFeatureClass ldhsjgFeatureClass = null;

            try
            {
                int totalFeatures = sourceFeatureClass.FeatureCount(null);
                int processedCount = 0;
                int successCount = 0;

                // 获取字段映射
                var fieldMappings = GetSLZYDLTBToSLZYZCDLTBFieldMappings();

                // 预先获取字段索引
                var sourceFieldIndexes = new Dictionary<string, int>();
                var targetFieldIndexes = new Dictionary<string, int>();

                foreach (var mapping in fieldMappings)
                {
                    string targetField = mapping.Key;
                    string sourceField = mapping.Value;

                    int sourceIndex = sourceFeatureClass.FindField(sourceField);
                    int targetIndex = targetFeatureClass.FindField(targetField);

                    if (sourceIndex != -1 && targetIndex != -1)
                    {
                        sourceFieldIndexes[sourceField] = sourceIndex;
                        targetFieldIndexes[targetField] = targetIndex;
                    }
                }

                // 获取CZKFBJMJ和GTDCTBMJ字段索引
                int czkfbjmjIndex = targetFeatureClass.FindField("CZKFBJMJ");
                int gtdctbmjIndex = targetFeatureClass.FindField("GTDCTBMJ");

                // 获取LDHSJG相关字段索引
                int zcqcbsmIndex = targetFeatureClass.FindField("ZCQCBSM");
                int ysdmIndex = targetFeatureClass.FindField("YSDM");
                int xzqmcIndex = targetFeatureClass.FindField("XZQMC");
                int xzqdmIndex = targetFeatureClass.FindField("XZQDM");
                int hsjgIndex = targetFeatureClass.FindField("HSJG");

                // 获取对应县的LDHSJG数据
                var ldhsjgData = GetLDHSJGDataForCounty(countyName);
                if (ldhsjgData.featureClass != null)
                {
                    ldhsjgFeatureClass = ldhsjgData.featureClass;
                    int ldhsjgFeatureCount = ldhsjgFeatureClass.FeatureCount(null);
                    System.Diagnostics.Debug.WriteLine($"找到{countyName}的LDHSJG数据，包含{ldhsjgFeatureCount}个要素");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"警告：未找到{countyName}的LDHSJG数据");
                }

                // 创建游标
                sourceCursor = sourceFeatureClass.Search(null, false);
                targetBuffer = targetFeatureClass.CreateFeatureBuffer();
                insertCursor = targetFeatureClass.Insert(true);

                // 处理每个要素
                IFeature sourceFeature;
                while ((sourceFeature = sourceCursor.NextFeature()) != null)
                {
                    try
                    {
                        // 复制几何
                        if (sourceFeature.Shape != null)
                        {
                            targetBuffer.Shape = sourceFeature.ShapeCopy;
                        }

                        // 复制基本属性
                        foreach (var mapping in fieldMappings)
                        {
                            string targetField = mapping.Key;
                            string sourceField = mapping.Value;

                            if (sourceFieldIndexes.ContainsKey(sourceField) &&
                                targetFieldIndexes.ContainsKey(targetField))
                            {
                                object sourceValue = sourceFeature.get_Value(sourceFieldIndexes[sourceField]);
                                targetBuffer.set_Value(targetFieldIndexes[targetField], sourceValue);
                            }
                        }

                        // 计算CZKFBJMJ字段（增加与GTDCTBMJ的比较逻辑）
                        if (czkfbjmjIndex != -1)
                        {
                            double intersectionArea = 0;
                            if (czkfbjFeatureClass != null && sourceFeature.Shape != null)
                            {
                                intersectionArea = CalculateIntersectionAreaWithCZKFBJ(
                                    sourceFeature.Shape, czkfbjFeatureClass);
                            }

                            // 检查CZKFBJMJ是否超过GTDCTBMJ
                            if (gtdctbmjIndex != -1)
                            {
                                // 获取GTDCTBMJ字段的值
                                object gtdctbmjValue = targetBuffer.get_Value(gtdctbmjIndex);
                                double gtdctbmjArea = 0;

                                if (gtdctbmjValue != null && double.TryParse(gtdctbmjValue.ToString(), out gtdctbmjArea))
                                {
                                    // 如果计算出的CZKFBJMJ大于GTDCTBMJ，则使用GTDCTBMJ的值
                                    if (intersectionArea > gtdctbmjArea)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"CZKFBJMJ({intersectionArea:F2})超过GTDCTBMJ({gtdctbmjArea:F2})，使用GTDCTBMJ值");
                                        intersectionArea = gtdctbmjArea;
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"警告: 无法获取有效的GTDCTBMJ值，CZKFBJMJ将使用计算值: {intersectionArea:F2}");
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"警告: 未找到GTDCTBMJ字段，CZKFBJMJ将使用计算值: {intersectionArea:F2}");
                            }

                            targetBuffer.set_Value(czkfbjmjIndex, intersectionArea);
                            System.Diagnostics.Debug.WriteLine($"最终CZKFBJMJ值: {intersectionArea:F2}");
                        }

                        // 处理LDHSJG相关字段
                        if (ldhsjgFeatureClass != null)
                        {
                            var ldhsjgValues = GetLDHSJGValuesForFeature(sourceFeature, ldhsjgFeatureClass, countyName);

                            if (zcqcbsmIndex != -1 && !string.IsNullOrEmpty(ldhsjgValues.ZCQCBSM))
                            {
                                targetBuffer.set_Value(zcqcbsmIndex, ldhsjgValues.ZCQCBSM);
                            }

                            if (ysdmIndex != -1 && !string.IsNullOrEmpty(ldhsjgValues.YSDM))
                            {
                                targetBuffer.set_Value(ysdmIndex, ldhsjgValues.YSDM);
                            }

                            if (xzqmcIndex != -1 && !string.IsNullOrEmpty(ldhsjgValues.XZQMC))
                            {
                                targetBuffer.set_Value(xzqmcIndex, ldhsjgValues.XZQMC);
                            }

                            if (xzqdmIndex != -1 && !string.IsNullOrEmpty(ldhsjgValues.XZQDM))
                            {
                                targetBuffer.set_Value(xzqdmIndex, ldhsjgValues.XZQDM);
                            }

                            if (hsjgIndex != -1 && ldhsjgValues.HSJG != null)
                            {
                                targetBuffer.set_Value(hsjgIndex, ldhsjgValues.HSJG);
                            }
                        }

                        // 插入要素
                        insertCursor.InsertFeature(targetBuffer);
                        successCount++;

                        processedCount++;

                        // 更新进度
                        if (processedCount % 50 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = (int)((processedCount / (double)totalFeatures) * 100);
                            progressCallback?.Invoke(percentage,
                                $"正在处理{countyName}的数据... ({processedCount}/{totalFeatures})");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"处理要素时出错: {ex.Message}");
                    }
                    finally
                    {
                        if (sourceFeature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceFeature);
                        }
                    }
                }

                // 提交插入操作
                insertCursor.Flush();

                System.Diagnostics.Debug.WriteLine($"成功处理{successCount}个要素到SLZYZC_DLTB");
                return successCount;
            }
            finally
            {
                if (sourceCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceCursor);
                if (targetBuffer != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(targetBuffer);
                if (insertCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                if (ldhsjgFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ldhsjgFeatureClass);
            }
        }

        /// <summary>
        /// 获取指定县的LDHSJG数据
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <returns>LDHSJG要素类和工作空间</returns>
        private (IWorkspace workspace, IFeatureClass featureClass) GetLDHSJGDataForCounty(string countyName)
        {
            try
            {
                // 从SharedDataManager获取LDHSJG文件列表
                var ldhsjgFiles = SharedDataManager.GetLDHSJGFiles();

                foreach (var fileInfo in ldhsjgFiles)
                {
                    if (fileInfo.DisplayName.Equals(countyName, StringComparison.OrdinalIgnoreCase) ||
                        fileInfo.DisplayName.Contains(countyName))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到{countyName}的LDHSJG文件: {fileInfo.FullPath}");
                        return OpenShapefileFeatureClass(fileInfo.FullPath);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"未找到{countyName}的LDHSJG文件");
                return (null, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取{countyName}的LDHSJG数据时出错: {ex.Message}");
                return (null, null);
            }
        }

        /// <summary>
        /// LDHSJG字段值结构
        /// </summary>
        private struct LDHSJGValues
        {
            public string ZCQCBSM { get; set; }
            public string YSDM { get; set; }
            public string XZQMC { get; set; }
            public string XZQDM { get; set; }
            public object HSJG { get; set; }
        }

        /// <summary>
        /// 为当前要素获取对应的LDHSJG字段值
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="ldhsjgFeatureClass">LDHSJG要素类</param>
        /// <param name="countyName">县名</param>
        /// <returns>LDHSJG字段值</returns>
        private LDHSJGValues GetLDHSJGValuesForFeature(IFeature sourceFeature, IFeatureClass ldhsjgFeatureClass, string countyName)
        {
            var result = new LDHSJGValues();

            try
            {
                if (sourceFeature?.Shape == null || ldhsjgFeatureClass == null)
                {
                    return result;
                }

                // 获取LDHSJG字段索引
                int zcqcbsmFieldIndex = ldhsjgFeatureClass.FindField("ZCQCBSM");
                int ysdmFieldIndex = ldhsjgFeatureClass.FindField("YSDM");
                int xjxzmcFieldIndex = ldhsjgFeatureClass.FindField("XJXZMC");
                int xjxzdmFieldIndex = ldhsjgFeatureClass.FindField("XJXZDM");
                int xjldpjjFieldIndex = ldhsjgFeatureClass.FindField("XJLDPJJ");

                // 使用空间查询找到相交的LDHSJG要素
                ISpatialFilter spatialFilter = null;
                IFeatureCursor ldhsjgCursor = null;

                try
                {
                    spatialFilter = new SpatialFilterClass();
                    spatialFilter.Geometry = sourceFeature.Shape;
                    spatialFilter.GeometryField = ldhsjgFeatureClass.ShapeFieldName;
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                    ldhsjgCursor = ldhsjgFeatureClass.Search(spatialFilter, false);
                    IFeature ldhsjgFeature = ldhsjgCursor.NextFeature();

                    if (ldhsjgFeature != null)
                    {
                        // 获取字段值
                        if (zcqcbsmFieldIndex != -1)
                        {
                            result.ZCQCBSM = ldhsjgFeature.get_Value(zcqcbsmFieldIndex)?.ToString() ?? "";
                        }

                        if (ysdmFieldIndex != -1)
                        {
                            result.YSDM = ldhsjgFeature.get_Value(ysdmFieldIndex)?.ToString() ?? "";
                        }

                        if (xjxzmcFieldIndex != -1)
                        {
                            result.XZQMC = ldhsjgFeature.get_Value(xjxzmcFieldIndex)?.ToString() ?? "";
                        }

                        if (xjxzdmFieldIndex != -1)
                        {
                            result.XZQDM = ldhsjgFeature.get_Value(xjxzdmFieldIndex)?.ToString() ?? "";
                        }

                        if (xjldpjjFieldIndex != -1)
                        {
                            result.HSJG = ldhsjgFeature.get_Value(xjldpjjFieldIndex);
                        }

                        System.Diagnostics.Debug.WriteLine($"成功获取{countyName}的LDHSJG字段值: ZCQCBSM={result.ZCQCBSM}, YSDM={result.YSDM}");

                        // 释放要素
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ldhsjgFeature);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"警告：未找到与当前要素相交的{countyName}的LDHSJG要素");
                    }
                }
                finally
                {
                    if (ldhsjgCursor != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ldhsjgCursor);
                    if (spatialFilter != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取{countyName}的LDHSJG字段值时出错: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 获取SLZY_DLTB到SLZYZC_DLTB的字段映射
        /// </summary>
        /// <returns>字段映射字典</returns>
        private Dictionary<string, string> GetSLZYDLTBToSLZYZCDLTBFieldMappings()
        {
            return new Dictionary<string, string>
            {
                { "GTDCTBBSM", "BSM" },        // 国土调查图斑编码
                { "GTDCTBBH", "TBBH" },        // 国土调查图斑编号
                { "GTDCDLBM", "DLBM" },        // 国土调查地类编码
                { "GTDCDLMC", "DLMC" },        // 国土调查地类名称
                { "GTDCTDQS", "QSXZ" },        // 国土调查土地权属
                { "ZLDWDM", "ZLDWDM" },        // 坐落单位代码
                { "ZLDWMC", "ZLDWMC" },        // 坐落单位名称
                { "GTDCTBMJ", "TBMJ" },        // 国土调查图斑面积
                { "FRDBS", "FRDBS" },          // 飞入地标识
                { "QSDWDM", "QSDWDM" },        // 权属单位代码
                { "QSDWMC", "QSDWMC" }         // 权属单位名称
                // CZKFBJMJ 字段通过特殊计算处理
            };
        }

        /// <summary>
        /// 计算几何对象与CZKFBJ的相交面积
        /// </summary>
        /// <param name="geometry">要计算的几何对象</param>
        /// <param name="czkfbjFeatureClass">CZKFBJ要素类</param>
        /// <returns>相交面积</returns>
        private double CalculateIntersectionAreaWithCZKFBJ(IGeometry geometry, IFeatureClass czkfbjFeatureClass)
        {
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

                // 创建空间查询过滤器
                spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = geometry;
                spatialFilter.GeometryField = czkfbjFeatureClass.ShapeFieldName;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                // 查询相交的CZKFBJ要素
                czkfbjCursor = czkfbjFeatureClass.Search(spatialFilter, false);
                IFeature czkfbjFeature;

                while ((czkfbjFeature = czkfbjCursor.NextFeature()) != null)
                {
                    try
                    {
                        if (czkfbjFeature.Shape != null && !czkfbjFeature.Shape.IsEmpty)
                        {
                            // 计算交集
                            ITopologicalOperator topoOperator = (ITopologicalOperator)geometry;
                            IGeometry intersectionGeometry = topoOperator.Intersect(
                                czkfbjFeature.Shape,
                                esriGeometryDimension.esriGeometry2Dimension);

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
                System.Diagnostics.Debug.WriteLine($"计算与CZKFBJ交集面积时出错: {ex.Message}");
                return 0;
            }
            finally
            {
                if (czkfbjCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjCursor);
                if (spatialFilter != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
            }
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
                // 修改：使用县代码映射器获取真实的县代码
                string countyCode = Utils.CountyCodeMapper.GetCountyCode(countyName);
                string countyFolderName = $"{countyName}（{countyCode}）全民所有自然资源资产清查数据成果";
                string countyFolderPath = System.IO.Path.Combine(outputPath, countyFolderName);
                string dataSetPath = System.IO.Path.Combine(countyFolderPath, "清查数据集");
                string forestPath = System.IO.Path.Combine(dataSetPath, "森林");
                string spatialDataPath = System.IO.Path.Combine(forestPath, "空间数据");

                // 使用ProgID创建Shapefile工作空间工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // 打开工作空间
                IWorkspace workspace = workspaceFactory.OpenFromFile(spatialDataPath, 0);

                System.Diagnostics.Debug.WriteLine($"成功创建{countyName}({countyCode})的Shapefile工作空间: {spatialDataPath}");
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
            
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;


                string featureClassName = "SLZYZC";
       
                
                    IFeatureClass existingFeatureClass = featureWorkspace.OpenFeatureClass(featureClassName);

                    System.Diagnostics.Debug.WriteLine($"SLZYZC Shapefile已存在，将使用现有文件并清空数据");

                    // 清空现有数据
                    ClearExistingShapefileData(existingFeatureClass);

                    return existingFeatureClass;

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

                // 预先获取所有字段索引，减少重复查找
                int zcqcbsmIndex = targetFeatureClass.FindField("ZCQCBSM");
                int czkfbjmjIndex = targetFeatureClass.FindField("CZKFBJMJ");
                int pctbbmIndex = targetFeatureClass.FindField("PCTBBM");
                int ztbxjIndex = targetFeatureClass.FindField("ZTBXJ");
                int xzqmcIndex = targetFeatureClass.FindField("XZQMC");

                // 预先获取源字段索引（特殊字段）
                int xianIndex = sourceFeatureClass.FindField("XIAN");
                int linBanIndex = sourceFeatureClass.FindField("LIN_BAN");
                int xiaoBanIndex = sourceFeatureClass.FindField("XIAO_BAN");
                int xbmjIndex = sourceFeatureClass.FindField("XBMJ");
                int field65Index = sourceFeatureClass.FindField("MEI_GQ_XJ");
                int xzqdmSourceIndex = xianIndex != -1 ? xianIndex : sourceFeatureClass.FindField("XZQDM");

                System.Diagnostics.Debug.WriteLine($"预缓存字段索引完成: PCTBBM={pctbbmIndex}, ZTBXJ={ztbxjIndex}, XZQMC={xzqmcIndex}");

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

                        // 处理普通字段映射
                        foreach (var mapping in fieldMappings)
                        {
                            string targetFieldName = mapping.Key;
                            string sourceFieldName = mapping.Value;

                            // 跳过特殊字段，稍后单独处理
                            if (targetFieldName == "PCTBBM" || targetFieldName == "ZTBXJ" || targetFieldName == "XZQMC")
                            {
                                continue;
                            }

                            try
                            {
                                int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);
                                if (targetFieldIndex != -1 && !string.IsNullOrEmpty(sourceFieldName))
                                {
                                    int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                                    if (sourceFieldIndex != -1)
                                    {
                                        object sourceValue = sourceFeature.get_Value(sourceFieldIndex);
                                        object convertedValue = ConvertFieldValueForSLZYZC(sourceValue, targetFieldName, sourceFieldName, countyName);
                                        if (convertedValue != null)
                                        {
                                            featureBuffer.set_Value(targetFieldIndex, convertedValue);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"复制{countyName}字段{targetFieldName}时出错: {ex.Message}");
                            }
                        }

                        // 处理特殊字段：PCTBBM (xian + lin_ban + xiao_ban)
                        if (pctbbmIndex != -1)
                        {
                            try
                            {
                                string xianValue = xianIndex != -1 ? (sourceFeature.get_Value(xianIndex)?.ToString() ?? "") : "";
                                string linBanValue = linBanIndex != -1 ? (sourceFeature.get_Value(linBanIndex)?.ToString() ?? "") : "";
                                string xiaoBanValue = xiaoBanIndex != -1 ? (sourceFeature.get_Value(xiaoBanIndex)?.ToString() ?? "") : "";

                                string pctbbmValue = $"{xianValue}{linBanValue}{xiaoBanValue}";
                                featureBuffer.set_Value(pctbbmIndex, pctbbmValue);

                                System.Diagnostics.Debug.WriteLine($"PCTBBM字段合并完成: {pctbbmValue}");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"处理PCTBBM字段时出错: {ex.Message}");
                            }
                        }

                        // 处理特殊字段：ZTBXJ (xbmj * 第65个字段)
                        if (ztbxjIndex != -1)
                        {
                            try
                            {
                                if (xbmjIndex != -1 && field65Index != -1)
                                {
                                    object xbmjValue = sourceFeature.get_Value(xbmjIndex);
                                    object field65Value = sourceFeature.get_Value(field65Index);

                                    if (double.TryParse(xbmjValue?.ToString(), out double xbmjNum) &&
                                        double.TryParse(field65Value?.ToString(), out double field65Num))
                                    {
                                        double ztbxjValue = xbmjNum * field65Num;
                                        featureBuffer.set_Value(ztbxjIndex, ztbxjValue);
                                        System.Diagnostics.Debug.WriteLine($"ZTBXJ字段计算完成: {xbmjNum} * {field65Num} = {ztbxjValue}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"处理ZTBXJ字段时出错: {ex.Message}");
                            }
                        }

                        // 处理特殊字段：XZQMC (基于XZQDM获取行政区名称)
                        if (xzqmcIndex != -1)
                        {
                            try
                            {
                                string xzqmcValue = countyName; // 默认使用传入的县名

                                if (xzqdmSourceIndex != -1)
                                {
                                    object xzqdmValue = sourceFeature.get_Value(xzqdmSourceIndex);
                                    string xzqdm = xzqdmValue?.ToString();

                                    if (!string.IsNullOrEmpty(xzqdm))
                                    {
                                        // 确保XZQDM是6位数字格式
                                        string normalizedXzqdm = xzqdm.Length > 6 ? xzqdm.Substring(0, 6) : xzqdm.PadLeft(6, '0');

                                        // 使用县代码映射器根据XZQDM获取行政区名称
                                        string administrativeName = Utils.CountyCodeMapper.GetCountyNameFromCode(normalizedXzqdm);

                                        if (!string.IsNullOrEmpty(administrativeName))
                                        {
                                            xzqmcValue = administrativeName;
                                            System.Diagnostics.Debug.WriteLine($"根据XZQDM '{normalizedXzqdm}' 获取到行政区名称: '{administrativeName}'");
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine($"警告: 无法根据XZQDM '{normalizedXzqdm}' 找到对应的行政区名称，使用传入的县名");
                                        }
                                    }
                                }

                                featureBuffer.set_Value(xzqmcIndex, xzqmcValue);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"处理XZQMC字段时出错: {ex.Message}");
                                // 出错时设置为默认县名
                                featureBuffer.set_Value(xzqmcIndex, countyName);
                            }
                        }

                        // 处理特殊字段：ZCQCBSM
                        if (zcqcbsmIndex != -1)
                        {
                            string zcqcbsmValue = GenerateZCQCBSM(sourceFeature, sourceFeatureClass, countyName, successCount + 1);
                            if (!string.IsNullOrEmpty(zcqcbsmValue))
                            {
                                featureBuffer.set_Value(zcqcbsmIndex, zcqcbsmValue);
                            }
                        }

                        // 处理特殊字段：CZKFBJMJ（如果需要的话）
                        //if (czkfbjmjIndex != -1)
                        //{
                        //    double intersectionArea = 0;
                        //    if (czkfbjFeatureClass != null && sourceFeature.Shape != null)
                        //    {
                        //        intersectionArea = CalculateIntersectionArea(sourceFeature.Shape, czkfbjFeatureClass);
                        //    }
                        //    featureBuffer.set_Value(czkfbjmjIndex, intersectionArea);
                        //}

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
        /// 获取默认SLZYZC字段映射
        /// </summary>
        /// <returns>字段映射字典</returns>
        private Dictionary<string, string> GetDefaultSLZYZCFieldMappings()
        {
            return new Dictionary<string, string>
            {
                { "YSDM", "ysdm" },            // 要素代码
                { "XZQDM", "xian" },           // 行政区代码
                { "XZQMC", "SPECIAL_XZQMC" },  // 行政区名称（基于XZQDM字段获取）
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
                { "FRDBS", "frdbs" },         // 发育地被层
                { "PCTBBM", "SPECIAL_PCTBBM" }, // 普查图斑编码（字段合并）
                { "ZTBXJ", "SPECIAL_ZTBXJ" }
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