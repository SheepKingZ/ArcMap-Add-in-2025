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

                // 修复：构建正确的文件路径
                string countyCode = Utils.CountyCodeMapper.GetCountyCode(countyName);
                string countyFolderName = $"{countyName}（{countyCode}）全民所有自然资源资产清查数据成果";
                string countyPath = System.IO.Path.Combine(outputPath, countyFolderName);
                string dataSetPath = System.IO.Path.Combine(countyPath, "清查数据集");
                string forestPath = System.IO.Path.Combine(dataSetPath, "森林");
                string spatialDataPath = System.IO.Path.Combine(forestPath, "空间数据");
                
                string slzyzcShapefilePath = System.IO.Path.Combine(spatialDataPath, "SLZYZC.shp");
                string slzyzcDltbShapefilePath = System.IO.Path.Combine(spatialDataPath, "SLZYZC_DLTB.shp");

                System.Diagnostics.Debug.WriteLine($"SLZYZC源文件路径: {slzyzcShapefilePath}");
                System.Diagnostics.Debug.WriteLine($"SLZYZC_DLTB目标文件路径: {slzyzcDltbShapefilePath}");

                // 检查源文件是否存在
                if (!System.IO.File.Exists(slzyzcShapefilePath))
                {
                    System.Diagnostics.Debug.WriteLine($"警告: SLZYZC源文件不存在: {slzyzcShapefilePath}");
                    progressCallback?.Invoke(99, $"{countyName}的SLZYZC源文件不存在，跳过转换");
                    return;
                }

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
                System.Diagnostics.Debug.WriteLine($"处理字段映射: 目标字段={targetFieldName}, 源字段={sourceFieldName}");

                switch (targetFieldName)
                {
                    case "PCTBBM":
                        if ("SPECIAL_PCTBBM".Equals(sourceFieldName, StringComparison.OrdinalIgnoreCase))
                        {
                            System.Diagnostics.Debug.WriteLine($"处理PCTBBM字段合并");
                            // 字段合并：xian + lin_ban + xiao_ban
                            return CombineFields(sourceFeature, sourceFeatureClass,
                                new[] { "xian", "lin_ban", "xiao_ban" });
                        }
                        break;

                    case "ZTBXJ":
                        if ("SPECIAL_ZTBXJ".Equals(sourceFieldName, StringComparison.OrdinalIgnoreCase))
                        {
                            System.Diagnostics.Debug.WriteLine($"处理ZTBXJ字段计算");
                            // 字段计算：xbmj * 第65个字段
                            return CalculateFieldProduct(sourceFeature, sourceFeatureClass,
                                "xbmj", GetFieldByIndex(sourceFeatureClass, 65));
                        }
                        break;

                    case "XZQMC":
                        System.Diagnostics.Debug.WriteLine($"处理XZQMC字段，sourceFieldName='{sourceFieldName}'");
                        
                        // 只有当sourceFieldName是特殊标识时才进行特殊处理
                        if ("SPECIAL_XZQMC".Equals(sourceFieldName, StringComparison.OrdinalIgnoreCase))
                        {
                            System.Diagnostics.Debug.WriteLine($"进入XZQMC特殊处理逻辑");
                            
                            // 首先尝试从XZQDM源字段获取行政区代码
                            int xzqdmIndex = sourceFeatureClass.FindField("xian");
                            if (xzqdmIndex == -1)
                            {
                                // 尝试其他可能的字段名
                                xzqdmIndex = sourceFeatureClass.FindField("XZQDM");
                            }

                            if (xzqdmIndex != -1)
                            {
                                object xzqdmValue = sourceFeature.get_Value(xzqdmIndex);
                                string xzqdm = xzqdmValue?.ToString();

                                if (!string.IsNullOrEmpty(xzqdm))
                                {
                                    System.Diagnostics.Debug.WriteLine($"从要素获取到XZQDM值: '{xzqdm}'");

                                    // 确保XZQDM是6位数字格式
                                    string normalizedXzqdm = xzqdm.Length > 6 ? xzqdm.Substring(0, 6) : xzqdm.PadLeft(6, '0');

                                    // 使用县代码映射器根据XZQDM获取行政区名称
                                    string administrativeName = Utils.CountyCodeMapper.GetCountyNameFromCode(normalizedXzqdm);

                                    if (!string.IsNullOrEmpty(administrativeName))
                                    {
                                        System.Diagnostics.Debug.WriteLine($"根据XZQDM '{normalizedXzqdm}' 获取到行政区名称: '{administrativeName}'");
                                        return administrativeName;
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine($"警告: 无法根据XZQDM '{normalizedXzqdm}' 找到对应的行政区名称，使用传入的县名");
                                        return countyName;
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"警告: 从要素获取的XZQDM值为空，使用传入的县名");
                                    return countyName;
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"警告: 在源要素类中找不到XZQDM相关字段(xian/XZQDM)，使用传入的县名");
                                return countyName;
                            }
                        }
                        else
                        {
                            // 普通字段映射处理
                            System.Diagnostics.Debug.WriteLine($"XZQMC使用普通字段映射，源字段: {sourceFieldName}");
                            break;
                        }

                    default:
                        // 继续到普通字段映射处理
                        break;
                }

                // 普通字段映射
                if (!string.IsNullOrEmpty(sourceFieldName))
                {
                    int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                    if (sourceFieldIndex != -1)
                    {
                        object sourceValue = sourceFeature.get_Value(sourceFieldIndex);
                        //System.Diagnostics.Debug.WriteLine($"普通字段映射: {targetFieldName} <- {sourceFieldName}, 值: {sourceValue}");
                        return ConvertFieldValueForSLZYZC(sourceValue, targetFieldName, sourceFieldName, countyName);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"警告: 源字段 {sourceFieldName} 在要素类中不存在");
                    }
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

        private double CalculateIntersectionArea(IGeometry geometry, IFeatureClass czkfbjFeatureClass)
        {
            return 0.0; // 保持现有的空实现
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
            System.Diagnostics.Debug.WriteLine($"EnsureCountySuffix: 输入县名 = '{countyName}'");

            if (string.IsNullOrEmpty(countyName))
            {
                System.Diagnostics.Debug.WriteLine($"EnsureCountySuffix: 县名为空，返回空字符串");
                return string.Empty;
            }

            // 使用正则表达式仅保留中文字符和常见的县级行政区划后缀
            string filteredCountyName = System.Text.RegularExpressions.Regex.Replace(countyName, @"[^\u4e00-\u9fa5县市区]", "");
            System.Diagnostics.Debug.WriteLine($"EnsureCountySuffix: 过滤后的县名 = '{filteredCountyName}'");

            // 如果过滤后名称为空，则使用原始名称
            if (string.IsNullOrEmpty(filteredCountyName))
            {
                System.Diagnostics.Debug.WriteLine($"EnsureCountySuffix: 过滤后为空，使用原始县名 = '{countyName}'");
                filteredCountyName = countyName;
            }

            System.Diagnostics.Debug.WriteLine($"EnsureCountySuffix: 最终结果 = '{filteredCountyName}'");
            return filteredCountyName;
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