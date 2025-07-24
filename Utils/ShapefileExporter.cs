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

        public void ExportToShapefile(
    List<IFeature> processedFeatures,
    IFeatureClass sourceFeatureClass,
    string countyName,
    string outputPath,
    Dictionary<string, string> fieldMappings,
    string outputShapefileName,
    ProgressCallback progressCallback = null)
        {
            System.Diagnostics.Debug.WriteLine($"开始执行ExportToShapefile - 县名: {countyName}, 输出路径: {outputPath}");

            try
            {
                // 参数验证 - 确保输入数据的有效性
                if (processedFeatures == null)
                {
                    string errorMsg = "参数验证失败: processedFeatures 为 null";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new ArgumentNullException(nameof(processedFeatures), errorMsg);
                }

                if (processedFeatures.Count == 0)
                {
                    string errorMsg = "参数验证失败: processedFeatures 列表为空";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new ArgumentException(errorMsg, nameof(processedFeatures));
                }

                System.Diagnostics.Debug.WriteLine($"processedFeatures 验证通过，包含 {processedFeatures.Count} 个要素");

                if (string.IsNullOrEmpty(countyName))
                {
                    string errorMsg = "参数验证失败: 县名不能为空或null";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new ArgumentException(errorMsg, nameof(countyName));
                }

                if (string.IsNullOrWhiteSpace(countyName))
                {
                    string errorMsg = $"参数验证失败: 县名不能为空白字符串，当前值: '{countyName}'";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new ArgumentException(errorMsg, nameof(countyName));
                }

                System.Diagnostics.Debug.WriteLine($"县名验证通过: '{countyName}'");

                if (string.IsNullOrEmpty(outputPath))
                {
                    string errorMsg = "参数验证失败: 输出路径不能为空或null";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new ArgumentException(errorMsg, nameof(outputPath));
                }

                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    string errorMsg = $"参数验证失败: 输出路径不能为空白字符串，当前值: '{outputPath}'";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new ArgumentException(errorMsg, nameof(outputPath));
                }

                if (!Directory.Exists(outputPath))
                {
                    string errorMsg = $"参数验证失败: 输出路径不存在 - '{outputPath}'";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new DirectoryNotFoundException(errorMsg);
                }

                System.Diagnostics.Debug.WriteLine($"输出路径验证通过: '{outputPath}'");

                if (sourceFeatureClass == null)
                {
                    string errorMsg = "参数验证失败: sourceFeatureClass 为 null";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new ArgumentNullException(nameof(sourceFeatureClass), errorMsg);
                }

                System.Diagnostics.Debug.WriteLine($"sourceFeatureClass 验证通过，类型: {sourceFeatureClass.AliasName}");

                // 验证要素的有效性
                for (int i = 0; i < Math.Min(processedFeatures.Count, 5); i++)
                {
                    var feature = processedFeatures[i];
                    if (feature == null)
                    {
                        string errorMsg = $"要素验证失败: 第 {i} 个要素为 null";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        throw new ArgumentException(errorMsg, nameof(processedFeatures));
                    }

                    if (feature.Shape == null)
                    {
                        string errorMsg = $"要素验证失败: 第 {i} 个要素的几何形状为 null (OID: {feature.OID})";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        throw new ArgumentException(errorMsg, nameof(processedFeatures));
                    }
                }

                System.Diagnostics.Debug.WriteLine("要素列表验证通过");

                // 从输出路径提取真实的县名
                string extractedCountyName = null;
                try
                {
                    extractedCountyName = ExtractCountyNameFromOutputPath(outputPath, countyName);
                    if (!string.IsNullOrEmpty(extractedCountyName))
                    {
                        System.Diagnostics.Debug.WriteLine($"从输出路径提取到县名: '{extractedCountyName}'，原县名: '{countyName}'");
                        countyName = extractedCountyName;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"未能从输出路径提取县名，使用原县名: '{countyName}'");
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = $"从输出路径提取县名时出错: {ex.Message}";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                    // 不抛出异常，继续使用原县名
                }

                progressCallback?.Invoke(5, $"正在创建{countyName}的Shapefile输出目录...");

                // COM对象声明 - 需要在finally块中显式释放以避免内存泄漏
                IWorkspace shapefileWorkspace = null;
                IFeatureClass slzyzcFeatureClass = null;

                try
                {
                    System.Diagnostics.Debug.WriteLine($"开始创建县级Shapefile工作空间 - 县名: {countyName}");
                    

                    // 创建县级Shapefile工作空间
                    try
                    {
                        shapefileWorkspace = CreateCountyShapefileWorkspace(outputPath, countyName, outputShapefileName);
                    }
                    catch (Exception ex)
                    {
                        string errorMsg = $"创建县级Shapefile工作空间失败 - 县名: {countyName}, 路径: {outputPath}";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        System.Diagnostics.Debug.WriteLine($"CreateCountyShapefileWorkspace异常: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                        throw new Exception(errorMsg, ex);
                    }

                    if (shapefileWorkspace == null)
                    {
                        string errorMsg = $"创建县级Shapefile工作空间返回null - 县名: {countyName}, 路径: {outputPath}";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        throw new Exception(errorMsg);
                    }

                    System.Diagnostics.Debug.WriteLine($"县级Shapefile工作空间创建成功 - 县名: {countyName}");

                    progressCallback?.Invoke(15, $"正在创建{countyName}的SLZYZC Shapefile...");

                    // 验证和获取第一个要素的几何信息
                    IFeature firstFeature = null;
                    esriGeometryType geometryType;
                    ISpatialReference spatialReference = null;

                    try
                    {
                        firstFeature = processedFeatures[0];
                        if (firstFeature?.Shape == null)
                        {
                            string errorMsg = $"第一个要素或其几何形状为null - 县名: {countyName}";
                            System.Diagnostics.Debug.WriteLine(errorMsg);
                            throw new Exception(errorMsg);
                        }

                        geometryType = firstFeature.Shape.GeometryType;
                        spatialReference = firstFeature.Shape.SpatialReference;

                        System.Diagnostics.Debug.WriteLine($"几何信息获取成功 - 类型: {geometryType}, 空间参考: {spatialReference?.Name ?? "未知"}");
                    }
                    catch (Exception ex)
                    {
                        string errorMsg = $"获取要素几何信息失败 - 县名: {countyName}";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        System.Diagnostics.Debug.WriteLine($"几何信息获取异常: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                        throw new Exception(errorMsg, ex);
                    }

                    // 创建SLZYZC要素类
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"开始创建SLZYZC Shapefile - 县名: {countyName}");
                        slzyzcFeatureClass = CreateSLZYZCShapefile(shapefileWorkspace, geometryType, spatialReference);
                    }
                    catch (Exception ex)
                    {
                        string errorMsg = $"创建SLZYZC Shapefile失败 - 县名: {countyName}";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        System.Diagnostics.Debug.WriteLine($"CreateSLZYZCShapefile异常: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                        throw new Exception(errorMsg, ex);
                    }

                    if (slzyzcFeatureClass == null)
                    {
                        string errorMsg = $"创建SLZYZC Shapefile返回null - 县名: {countyName}";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        throw new Exception(errorMsg);
                    }

                    System.Diagnostics.Debug.WriteLine($"SLZYZC Shapefile创建成功 - 县名: {countyName}");

                    progressCallback?.Invoke(25, $"开始向{countyName}的SLZYZC Shapefile写入数据...");

                    // 执行数据写入操作
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"开始写入要素到Shapefile - 县名: {countyName}, 要素数量: {processedFeatures.Count}");

                        WriteFeaturesToShapefile(processedFeatures, sourceFeatureClass, slzyzcFeatureClass,
                            fieldMappings, countyName, progressCallback);

                        System.Diagnostics.Debug.WriteLine($"要素写入完成 - 县名: {countyName}");
                    }
                    catch (Exception ex)
                    {
                        string errorMsg = $"写入要素到Shapefile失败 - 县名: {countyName}, 要素数量: {processedFeatures.Count}";
                        //System.Diagnostics.Debug.WriteLine(errorMsg);
                        //System.Diagnostics.Debug.WriteLine($"WriteFeaturesToShapefile异常: {ex.Message}");
                        //System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                        throw new Exception(errorMsg, ex);
                    }

                    progressCallback?.Invoke(80, $"成功将 {processedFeatures.Count} 个要素写入到{countyName}的SLZYZC Shapefile");

                    //System.Diagnostics.Debug.WriteLine($"县{countyName}的数据已成功写入SLZYZC Shapefile");

                    // 执行SLZYZC_DLTB操作
                    try
                    {
                        //System.Diagnostics.Debug.WriteLine($"开始执行自动转换 - 县名: {countyName}");
                        PerformAutoConversion(countyName, outputPath, progressCallback);
                        //System.Diagnostics.Debug.WriteLine($"自动转换完成 - 县名: {countyName}");
                    }
                    catch (Exception ex)
                    {
                        string errorMsg = $"执行自动转换失败 - 县名: {countyName}";
                        //System.Diagnostics.Debug.WriteLine(errorMsg);
                        //System.Diagnostics.Debug.WriteLine($"PerformAutoConversion异常: {ex.Message}");
                        //System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                        // 注意：这里可以选择是否抛出异常，或者仅记录警告
                        throw new Exception(errorMsg, ex);
                    }

                    //progressCallback?.Invoke(100, $"{countyName}的数据导入和转换已全部完成");
                    //System.Diagnostics.Debug.WriteLine($"ExportToShapefile完成 - 县名: {countyName}");
                }
                catch (Exception ex)
                {
                    //string errorMsg = $"ExportToShapefile主要处理过程中出错 - 县名: {countyName}";
                    //System.Diagnostics.Debug.WriteLine(errorMsg);
                    //System.Diagnostics.Debug.WriteLine($"主要处理异常: {ex.Message}");
                    //System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");

                    // 重新抛出异常，保留原始堆栈跟踪
                    //throw new Exception(errorMsg, ex);
                }
                finally
                {
                    // 重要：释放ArcGIS COM对象，防止内存泄漏
                    try
                    {
                        if (slzyzcFeatureClass != null)
                        {
                            //System.Diagnostics.Debug.WriteLine($"释放slzyzcFeatureClass COM对象 - 县名: {countyName}");
                            //System.Runtime.InteropServices.Marshal.ReleaseComObject(slzyzcFeatureClass);
                        }
                    }
                    catch (Exception ex)
                    {
                        //System.Diagnostics.Debug.WriteLine($"释放slzyzcFeatureClass COM对象时出错: {ex.Message}");
                    }

                    try
                    {
                        if (shapefileWorkspace != null)
                        {
                            //System.Diagnostics.Debug.WriteLine($"释放shapefileWorkspace COM对象 - 县名: {countyName}");
                            //System.Runtime.InteropServices.Marshal.ReleaseComObject(shapefileWorkspace);
                        }
                    }
                    catch (Exception ex)
                    {
                        //System.Diagnostics.Debug.WriteLine($"释放shapefileWorkspace COM对象时出错: {ex.Message}");
                    }

                    //System.Diagnostics.Debug.WriteLine($"ExportToShapefile finally块完成 - 县名: {countyName}");
                }
            }
            catch (Exception ex)
            {
                //string errorMsg = $"ExportToShapefile顶级异常 - 县名: {countyName ?? "未知"}";
                //System.Diagnostics.Debug.WriteLine(errorMsg);
                //System.Diagnostics.Debug.WriteLine($"顶级异常: {ex.Message}");
                //System.Diagnostics.Debug.WriteLine($"完整异常信息: {ex}");

                // 抛出包含详细信息的异常
                //throw new Exception($"{errorMsg}: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// 在指定路径中查找符合模式的输出 Shapefile文件名（支持动态数据类型）
        /// 支持以下命名模式：
        /// 1. (县代码)SLZYZC.shp / (县代码)CYZYZC.shp / (县代码)SDZYZC.shp
        /// 2. SLZYZC.shp / CYZYZC.shp / SDZYZC.shp
        /// </summary>
        /// <param name="workspacePath">工作空间路径</param>
        /// <param name="outputShapefileName">输出Shapefile名称（SLZYZC、CYZYZC或SDZYZC）</param>
        /// <returns>找到的Shapefile文件名（不含扩展名），如果未找到返回null</returns>
        private string FindOutputShapefileName(string workspacePath, string outputShapefileName)
        {
            try
            {
                if (!Directory.Exists(workspacePath))
                {
                    System.Diagnostics.Debug.WriteLine($"工作空间路径不存在: {workspacePath}");
                    return null;
                }

                // 获取所有.shp文件
                string[] shapefiles = Directory.GetFiles(workspacePath, "*.shp");
                System.Diagnostics.Debug.WriteLine($"工作空间中找到 {shapefiles.Length} 个Shapefile文件");

                // 列出所有找到的文件
                foreach (string file in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    System.Diagnostics.Debug.WriteLine($"  发现Shapefile: {fileName}");
                }

                // 优先查找带县代码的文件：(县代码)outputShapefileName.shp
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式1: (数字)outputShapefileName 或 （数字）outputShapefileName
                    string pattern1 = @"^[（(]\d+[）)]" + outputShapefileName + "$";
                    if (System.Text.RegularExpressions.Regex.IsMatch(fileName, pattern1))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到带县代码的{outputShapefileName}文件: {fileName}");
                        return fileName;
                    }
                }

                // 如果没有找到带县代码的，查找标准的文件
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式2: 精确匹配 outputShapefileName
                    if (fileName.Equals(outputShapefileName, StringComparison.OrdinalIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到标准{outputShapefileName}文件: {fileName}");
                        return fileName;
                    }
                }

                // 最后尝试模糊匹配包含outputShapefileName的文件
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式3: 包含outputShapefileName的文件
                    if (fileName.Contains(outputShapefileName))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到包含{outputShapefileName}的文件: {fileName}");
                        return fileName;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"未找到任何符合模式的{outputShapefileName} Shapefile文件");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找{outputShapefileName} Shapefile文件时出错: {ex.Message}");
                return null;
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
        /// 执行自动转换 - 根据当前数据类型生成对应的_DLTB数据
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="progressCallback">进度回调</param>
        private void PerformAutoConversion(string countyName, string outputPath, ProgressCallback progressCallback)
        {
            try
            {
                // 🔥 修改：根据当前数据类型获取输出类型名称
                string outputBaseName = GetCurrentOutputShapefileName();
                string dltbTypeName = $"{outputBaseName}_DLTB";

                progressCallback?.Invoke(80, $"准备为{countyName}生成{dltbTypeName}数据...");
                System.Diagnostics.Debug.WriteLine($"开始为县{countyName}生成{dltbTypeName}数据");

                // 构建已存在的目标DLTB文件路径
                string countyCode = Utils.CountyCodeMapper.GetCountyCode(countyName);
                string countyFolderName = $"{countyName}({countyCode})全民所有自然资源资产清查数据成果";
                string countyPath = System.IO.Path.Combine(outputPath, countyFolderName);
                string dataSetPath = System.IO.Path.Combine(countyPath, "清查数据集");

                // 🔥 修改：根据数据类型选择正确的资源文件夹
                string resourceFolder = GetResourceFolderByOutputType(outputBaseName);
                string resourcePath = System.IO.Path.Combine(dataSetPath, resourceFolder);
                string spatialDataPath = System.IO.Path.Combine(resourcePath, "空间数据");

                System.Diagnostics.Debug.WriteLine($"目标文件夹结构: {countyName} -> {resourceFolder} -> {dltbTypeName}");

                // 查找目标DLTB文件（支持多种命名模式）
                string targetDltbShapefilePath = FindSLZYZCDLTBShapefilePath(spatialDataPath);

                if (string.IsNullOrEmpty(targetDltbShapefilePath))
                {
                    System.Diagnostics.Debug.WriteLine($"错误: 未找到{dltbTypeName}文件在路径: {spatialDataPath}");
                    progressCallback?.Invoke(99, $"{countyName}的{dltbTypeName}文件不存在，请先创建空的Shapefile结构");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"{dltbTypeName}目标文件路径: {targetDltbShapefilePath}");

                // 获取对应县的源数据（根据数据类型获取不同的源数据）
                string sourceDltbPath = GetSourceDLTBShapefilePath(countyName, outputBaseName);
                if (string.IsNullOrEmpty(sourceDltbPath))
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 未找到{countyName}的{outputBaseName}源数据，跳过转换");
                    progressCallback?.Invoke(99, $"{countyName}的{outputBaseName}源数据不存在，跳过转换");
                    return;
                }

                progressCallback?.Invoke(82, $"正在处理{countyName}的{outputBaseName}数据...");

                // 获取对应县的CZKFBJ数据
                string czkfbjPath = GetCZKFBJShapefilePath(countyName);
                System.Diagnostics.Debug.WriteLine($"CZKFBJ数据路径: {czkfbjPath ?? "未找到"}");

                // 执行目标DLTB生成操作
                bool conversionSuccess = GenerateTargetDLTB(
                    sourceDltbPath,
                    czkfbjPath,
                    targetDltbShapefilePath,
                    countyName,
                    outputBaseName,
                    (subPercentage, subMessage) =>
                    {
                        // 将子进度映射到总进度的82%-99%区间
                        int totalPercentage = 82 + (subPercentage * 17 / 100);
                        progressCallback?.Invoke(totalPercentage, $"{countyName}: {subMessage}");
                    });

                if (conversionSuccess)
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的{dltbTypeName}数据生成成功");
                    progressCallback?.Invoke(99, $"{countyName}的{dltbTypeName}数据生成成功");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"县{countyName}的{dltbTypeName}数据生成失败");
                    progressCallback?.Invoke(99, $"{countyName}的{dltbTypeName}数据生成失败");
                }
            }
            catch (Exception ex)
            {
                string outputBaseName = GetCurrentOutputShapefileName();
                string dltbTypeName = $"{outputBaseName}_DLTB";
                System.Diagnostics.Debug.WriteLine($"生成{countyName}的{dltbTypeName}数据时出错: {ex.Message}");
                progressCallback?.Invoke(99, $"{countyName}的{dltbTypeName}数据生成出错: {ex.Message}");

                // 记录详细错误信息
                System.Diagnostics.Debug.WriteLine($"错误详情: {ex}");
            }
        }
        /// <summary>
        /// 根据数据类型获取对应的源DLTB数据路径
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="outputBaseName">输出基础名称（SLZYZC、CYZYZC、SDZYZC）</param>
        /// <returns>源DLTB文件路径</returns>
        private string GetSourceDLTBShapefilePath(string countyName, string outputBaseName)
        {
            try
            {
                // 根据输出类型获取对应的源数据文件列表
                List<SourceDataFileInfo> sourceFiles = null;

                switch (outputBaseName)
                {
                    case "SLZYZC":
                        sourceFiles = SharedDataManager.GetSLZYDLTBFiles();
                        System.Diagnostics.Debug.WriteLine($"获取森林资源源数据: SLZY_DLTB");
                        break;
                    case "CYZYZC":
                        sourceFiles = SharedDataManager.GetCYZYDLTBFiles();
                        System.Diagnostics.Debug.WriteLine($"获取草地资源源数据: CYZY_DLTB");
                        break;
                    case "SDZYZC":
                        sourceFiles = SharedDataManager.GetSDZYDLTBFiles();
                        System.Diagnostics.Debug.WriteLine($"获取湿地资源源数据: SDZY_DLTB");
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine($"未知的输出类型: {outputBaseName}，使用默认SLZY_DLTB");
                        sourceFiles = SharedDataManager.GetSLZYDLTBFiles();
                        break;
                }

                if (sourceFiles == null)
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 无法获取{outputBaseName}的源数据文件列表");
                    return null;
                }

                foreach (var fileInfo in sourceFiles)
                {
                    if (fileInfo.DisplayName.Equals(countyName, StringComparison.OrdinalIgnoreCase) ||
                        fileInfo.DisplayName.Contains(countyName))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到{countyName}的{outputBaseName}对应源文件: {fileInfo.FullPath}");
                        return fileInfo.FullPath;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"未找到{countyName}的{outputBaseName}对应源文件");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取{countyName}的{outputBaseName}源路径时出错: {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// 在指定路径中查找输出_DLTB Shapefile文件（支持动态数据类型）
        /// 支持以下命名模式：
        /// 1. (县代码)SLZYZC_DLTB.shp / (县代码)CYZYZC_DLTB.shp / (县代码)SDZYZC_DLTB.shp
        /// 2. SLZYZC_DLTB.shp / CYZYZC_DLTB.shp / SDZYZC_DLTB.shp
        /// </summary>
        /// <param name="spatialDataPath">空间数据路径</param>
        /// <returns>找到的输出_DLTB文件完整路径，如果未找到返回null</returns>
        private string FindSLZYZCDLTBShapefilePath(string spatialDataPath)
        {
            try
            {
                if (!Directory.Exists(spatialDataPath))
                {
                    System.Diagnostics.Debug.WriteLine($"空间数据路径不存在: {spatialDataPath}");
                    return null;
                }

                // 🔥 修改：根据当前数据类型确定要查找的文件模式
                string outputBaseName = GetCurrentOutputShapefileName();
                string dltbFileName = $"{outputBaseName}_DLTB";

                System.Diagnostics.Debug.WriteLine($"正在查找{dltbFileName}文件...");

                // 获取所有.shp文件
                string[] shapefiles = Directory.GetFiles(spatialDataPath, "*.shp");
                System.Diagnostics.Debug.WriteLine($"空间数据路径中找到 {shapefiles.Length} 个Shapefile文件");

                // 列出所有找到的文件
                foreach (string file in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    System.Diagnostics.Debug.WriteLine($"  发现Shapefile: {fileName}");
                }

                // 优先查找带县代码的文件：(县代码)outputBaseName_DLTB.shp
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式1: (数字)outputBaseName_DLTB 或 （数字）outputBaseName_DLTB
                    string pattern1 = @"^[（(]\d+[）)]" + dltbFileName + "$";
                    if (System.Text.RegularExpressions.Regex.IsMatch(fileName, pattern1))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到带县代码的{dltbFileName}文件: {fileName}");
                        return shapefilePath;
                    }
                }

                // 如果没有找到带县代码的，查找标准的文件
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式2: 精确匹配 dltbFileName
                    if (fileName.Equals(dltbFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到标准{dltbFileName}文件: {fileName}");
                        return shapefilePath;
                    }
                }

                // 最后尝试模糊匹配包含dltbFileName的文件
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式3: 包含dltbFileName的文件
                    if (fileName.Contains(dltbFileName))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到包含{dltbFileName}的文件: {fileName}");
                        return shapefilePath;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"未找到任何符合模式的{dltbFileName} Shapefile文件");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找DLTB Shapefile文件时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 生成目标_DLTB数据（写入到已存在的文件中）
        /// </summary>
        /// <param name="sourceDltbPath">源DLTB数据路径</param>
        /// <param name="czkfbjPath">CZKFBJ数据路径</param>
        /// <param name="outputPath">输出目标_DLTB文件路径（已存在）</param>
        /// <param name="countyName">县名</param>
        /// <param name="outputBaseName">输出基础名称</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>是否成功</returns>
        private bool GenerateTargetDLTB(
            string sourceDltbPath,
            string czkfbjPath,
            string outputPath,
            string countyName,
            string outputBaseName,
            ProgressCallback progressCallback)
        {
            // COM对象声明
            IWorkspace sourceDltbWorkspace = null;
            IFeatureClass sourceDltbFeatureClass = null;
            IWorkspace czkfbjWorkspace = null;
            IFeatureClass czkfbjFeatureClass = null;
            IFeatureClass outputFeatureClass = null;

            try
            {
                string dltbTypeName = $"{outputBaseName}_DLTB";
                progressCallback?.Invoke(5, $"正在打开{outputBaseName}_DLTB源数据...");

                // 打开源DLTB数据
                var sourceResult = OpenShapefileFeatureClass(sourceDltbPath);
                sourceDltbWorkspace = sourceResult.workspace;
                sourceDltbFeatureClass = sourceResult.featureClass;

                if (sourceDltbFeatureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法打开{outputBaseName}_DLTB源数据: {sourceDltbPath}");
                    return false;
                }

                int totalFeatures = sourceDltbFeatureClass.FeatureCount(null);
                System.Diagnostics.Debug.WriteLine($"{outputBaseName}_DLTB包含{totalFeatures}个要素");

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

                progressCallback?.Invoke(25, $"正在打开已存在的{dltbTypeName}文件...");

                // 打开已存在的目标DLTB文件
                var outputResult = OpenShapefileFeatureClass(outputPath);
                if (outputResult.featureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法打开已存在的{dltbTypeName}文件: {outputPath}");
                    return false;
                }
                outputFeatureClass = outputResult.featureClass;

                // 清空现有数据
                ClearExistingShapefileData(outputFeatureClass);

                progressCallback?.Invoke(35, "开始复制和处理数据...");

                // 执行数据复制和处理
                int processedCount = CopyAndProcessFeatures(
                    sourceDltbFeatureClass,
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
                System.Diagnostics.Debug.WriteLine($"生成{outputBaseName}_DLTB时出错: {ex.Message}");
                return false;
            }
            finally
            {
                // 释放所有COM对象
                if (sourceDltbFeatureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceDltbFeatureClass);
                if (sourceDltbWorkspace != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceDltbWorkspace);
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
        /// 复制和处理要素数据（修改版：JJJZ使用HSJG×GTDCTBMJ计算）
        /// </summary>
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

                // 获取其他字段索引
                int zcqcbsmIndex = targetFeatureClass.FindField("ZCQCBSM");
                int ysdmIndex = targetFeatureClass.FindField("YSDM");
                int xzqmcIndex = targetFeatureClass.FindField("XZQMC");
                int xzqdmIndex = targetFeatureClass.FindField("XZQDM");
                int hsjgIndex = targetFeatureClass.FindField("HSJG");
                int jjjzIndex = targetFeatureClass.FindField("JJJZ");

                // 获取对应县的LDHSJG数据
                var ldhsjgData = GetLDHSJGDataForCounty(countyName);
                if (ldhsjgData.featureClass != null)
                {
                    ldhsjgFeatureClass = ldhsjgData.featureClass;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"警告：未找到{countyName}的LDHSJG数据");
                }

                // 预先获取该县的XZQDM值
                string countyXZQDM = GetCountyXZQDMFromLDHSJG(ldhsjgFeatureClass, countyName);

                // 创建游标
                sourceCursor = sourceFeatureClass.Search(null, false);
                targetBuffer = targetFeatureClass.CreateFeatureBuffer();
                insertCursor = targetFeatureClass.Insert(true);

                // 初始化序号计数器
                int sequenceNumber = 1;

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

                        // 设置YSDM字段为固定值
                        if (ysdmIndex != -1)
                        {
                            string currentOutputType = GetCurrentOutputShapefileName();
                            string ysdmValue;
                            switch (currentOutputType)
                            {
                                case "CYZYZC":
                                    ysdmValue = "2160301000";  // 草地资源
                                    break;
                                case "SDZYZC":
                                    ysdmValue = "2170401000";  // 湿地资源
                                    break;
                                default: // SLZYZC
                                    ysdmValue = "2150201010";  // 森林资源
                                    break;
                            }
                            targetBuffer.set_Value(ysdmIndex, ysdmValue);
                        }

                        // 使用新的比例计算逻辑计算CZKFBJMJ字段
                        if (czkfbjmjIndex != -1)
                        {
                            double czkfbjmjValue = 0;

                            // 获取GTDCTBMJ值
                            double gtdctbmjArea = 0;
                            if (gtdctbmjIndex != -1)
                            {
                                object gtdctbmjObject = targetBuffer.get_Value(gtdctbmjIndex);
                                if (gtdctbmjObject != null && double.TryParse(gtdctbmjObject.ToString(), out gtdctbmjArea))
                                {
                                    // 使用新的比例计算方法
                                    if (czkfbjFeatureClass != null && sourceFeature.Shape != null)
                                    {
                                        czkfbjmjValue = CalculateIntersectionAreaWithCZKFBJ(
                                            sourceFeature.Shape, czkfbjFeatureClass, gtdctbmjArea);

                                        System.Diagnostics.Debug.WriteLine($"要素OID={sourceFeature.OID}: GTDCTBMJ={gtdctbmjArea:F2}, 按比例计算CZKFBJMJ={czkfbjmjValue:F2}");
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"警告: 无法获取有效的GTDCTBMJ值，CZKFBJMJ将设为0");
                                }
                            }

                            targetBuffer.set_Value(czkfbjmjIndex, czkfbjmjValue);
                        }

                        // 声明HSJG值变量（用于后续JJJZ计算）
                        object hsjgValue = null;

                        // 改进：处理LDHSJG相关字段，使用修复后的方法
                        if (ldhsjgFeatureClass != null)
                        {
                            var ldhsjgValues = GetLDHSJGValuesForFeature(sourceFeature, ldhsjgFeatureClass, countyName);

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
                                hsjgValue = ldhsjgValues.HSJG; // 保存HSJG值用于JJJZ计算

                                // 调试：输出HSJG字段设置信息
                                System.Diagnostics.Debug.WriteLine($"为要素OID={sourceFeature.OID}设置HSJG值: {hsjgValue}");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"警告：要素OID={sourceFeature.OID}无法获取HSJG值");
                            }
                        }

                        // 计算JJJZ字段
                        if (jjjzIndex != -1)
                        {
                            double jjjzValue = 0.0;

                            // 使用GTDCTBMJ值计算经济价值
                            object gtdctbmjObject = targetBuffer.get_Value(gtdctbmjIndex);
                            double gtdctbmjArea = 0.0;

                            if (gtdctbmjObject != null && double.TryParse(gtdctbmjObject.ToString(), out gtdctbmjArea))
                            {
                                // 获取HSJG值
                                double hsjgNumber = 0.0;
                                if (hsjgValue != null && double.TryParse(hsjgValue.ToString(), out hsjgNumber))
                                {
                                    // 计算经济价值：GTDCTBMJ * HSJG
                                    jjjzValue = gtdctbmjArea * hsjgNumber;
                                    System.Diagnostics.Debug.WriteLine($"JJJZ计算: GTDCTBMJ({gtdctbmjArea:F2}) * HSJG({hsjgNumber:F2}) = {jjjzValue:F2}");
                                }
                                else
                                {
                                    //System.Diagnostics.Debug.WriteLine($"警告: 无法获取有效的HSJG值，JJJZ将设为0");
                                }
                            }
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine($"警告: 无法获取有效的GTDCTBMJ值，JJJZ将设为0");
                            }

                            // 设置JJJZ字段值
                            targetBuffer.set_Value(jjjzIndex, jjjzValue);
                        }

                        // 生成ZCQCBSM字段值
                        if (zcqcbsmIndex != -1)
                        {
                            string zcqcbsmValue = GenerateZCQCBSMForSLZYZCDLTBWithCountyXZQDM(countyXZQDM, sequenceNumber);
                            if (!string.IsNullOrEmpty(zcqcbsmValue))
                            {
                                targetBuffer.set_Value(zcqcbsmIndex, zcqcbsmValue);
                            }
                        }

                        // 插入要素
                        insertCursor.InsertFeature(targetBuffer);
                        successCount++;
                        sequenceNumber++;
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
        /// 🔥 新增：从LDHSJG数据中获取该县的XZQDM值
        /// </summary>
        /// <param name="ldhsjgFeatureClass">LDHSJG要素类</param>
        /// <param name="countyName">县名</param>
        /// <returns>该县的XZQDM值</returns>
        private string GetCountyXZQDMFromLDHSJG(IFeatureClass ldhsjgFeatureClass, string countyName)
        {
            if (ldhsjgFeatureClass == null)
            {
                System.Diagnostics.Debug.WriteLine($"警告：{countyName}的LDHSJG要素类为null，使用默认XZQDM");
                return "000000"; // 默认值
            }

            IFeatureCursor cursor = null;
            try
            {
                // 获取XJXZDM字段索引（行政区代码）
                int xjxzdmFieldIndex = ldhsjgFeatureClass.FindField("XJXZDM");
                if (xjxzdmFieldIndex == -1)
                {
                    System.Diagnostics.Debug.WriteLine($"警告：未找到{countyName}的LDHSJG中的XJXZDM字段，使用默认XZQDM");
                    return "000000";
                }

                // 获取第一个要素的XZQDM值（同一个县的所有要素XZQDM应该相同）
                cursor = ldhsjgFeatureClass.Search(null, false);
                IFeature firstFeature = cursor.NextFeature();

                if (firstFeature != null)
                {
                    object xzqdmValue = firstFeature.get_Value(xjxzdmFieldIndex);
                    string xzqdm = xzqdmValue?.ToString() ?? "";

                    // 确保XZQDM是6位数字
                    if (string.IsNullOrEmpty(xzqdm))
                    {
                        System.Diagnostics.Debug.WriteLine($"警告：{countyName}的LDHSJG中XJXZDM字段为空，使用默认值");
                        return "000000";
                    }
                    else if (xzqdm.Length > 6)
                    {
                        xzqdm = xzqdm.Substring(0, 6);
                        System.Diagnostics.Debug.WriteLine($"{countyName}的XZQDM长度超过6位，截取前6位: {xzqdm}");
                    }
                    else if (xzqdm.Length < 6)
                    {
                        xzqdm = xzqdm.PadLeft(6, '0');
                        System.Diagnostics.Debug.WriteLine($"{countyName}的XZQDM长度不足6位，前补0: {xzqdm}");
                    }

                    System.Diagnostics.Debug.WriteLine($"成功从{countyName}的LDHSJG获取XZQDM: {xzqdm}");

                    // 释放要素
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(firstFeature);

                    return xzqdm;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"警告：{countyName}的LDHSJG中没有要素，使用默认XZQDM");
                    return "000000";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从{countyName}的LDHSJG获取XZQDM时出错: {ex.Message}");
                return "000000"; // 出错时返回默认值
            }
            finally
            {
                if (cursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            }
        }

        /// <summary>
        /// 🔥 新增：使用县级XZQDM生成SLZYZC_DLTB的ZCQCBSM字段值
        /// 格式：XZQDM(6位) + 中间代码 + 序号(12位，从1开始，前补0)
        /// 总长度：22位
        /// </summary>
        /// <param name="countyXZQDM">县级XZQDM（6位）</param>
        /// <param name="sequenceNumber">序号（从1开始）</param>
        /// <returns>22位的ZCQCBSM值</returns>
        private string GenerateZCQCBSMForSLZYZCDLTBWithCountyXZQDM(string countyXZQDM, int sequenceNumber)
        {
            try
            {
                // 确保XZQDM是6位数字
                string xzqdm = countyXZQDM ?? "000000";

                if (string.IsNullOrEmpty(xzqdm))
                {
                    xzqdm = "000000";
                }
                else if (xzqdm.Length > 6)
                {
                    xzqdm = xzqdm.Substring(0, 6);
                }
                else if (xzqdm.Length < 6)
                {
                    xzqdm = xzqdm.PadLeft(6, '0');
                }

                // 🔥 修改：根据数据类型设置不同的中间代码
                string currentOutputType = GetCurrentOutputShapefileName();
                string middlePart;
                switch (currentOutputType)
                {
                    case "CYZYZC":
                        middlePart = "5110";  // 草地资源
                        break;
                    case "SDZYZC":
                        middlePart = "6110";  // 湿地资源
                        break;
                    default: // SLZYZC
                        middlePart = "4110";  // 森林资源
                        break;
                }

                // 序号格式化为12位，前补0
                string sequencePart = sequenceNumber.ToString("D12");

                // 组合成22位的ZCQCBSM
                string zcqcbsm = $"{xzqdm}{middlePart}{sequencePart}";

                return zcqcbsm;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成ZCQCBSM时出错: {ex.Message}");
                // 🔥 修改：出错时的默认值也要考虑数据类型
                string currentOutputType = GetCurrentOutputShapefileName();
                string defaultMiddle;
                switch (currentOutputType)
                {
                    case "CYZYZC":
                        defaultMiddle = "5110";
                        break;
                    case "SDZYZC":
                        defaultMiddle = "6110";
                        break;
                    default:
                        defaultMiddle = "4110";
                        break;
                }
                string defaultZcqcbsm = $"000000{defaultMiddle}{sequenceNumber.ToString("D12")}";
                return defaultZcqcbsm;
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
        /// 为当前要素获取对应的LDHSJG字段值（修复版）
        /// 增强空间查询精度和字段映射一致性
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
                    System.Diagnostics.Debug.WriteLine($"警告：{countyName} - 源要素或LDHSJG要素类为空");
                    return result;
                }

                // 🔥 修复：统一获取HSJG字段索引，确保HSJG和XJLDPJJ指向同一个字段
                int zcqcbsmFieldIndex = ldhsjgFeatureClass.FindField("ZCQCBSM");
                int ysdmFieldIndex = ldhsjgFeatureClass.FindField("YSDM");
                int xjxzmcFieldIndex = ldhsjgFeatureClass.FindField("XJXZMC");
                int xjxzdmFieldIndex = ldhsjgFeatureClass.FindField("XJXZDM");

                // 🔥 关键修复：统一HSJG字段获取逻辑，优先查找XJLDPJJ，然后查找其他可能的字段名
                int hsjgFieldIndex = -1;
                string[] possibleHsjgFields = { "XJLDPJJ", "HSJG", "林地定级平均价", "平均价格", "PJJ" };

                foreach (string fieldName in possibleHsjgFields)
                {
                    hsjgFieldIndex = ldhsjgFeatureClass.FindField(fieldName);
                    if (hsjgFieldIndex != -1)
                    {
                        System.Diagnostics.Debug.WriteLine($"找到HSJG字段: {fieldName} (索引: {hsjgFieldIndex})");
                        break;
                    }
                }

                if (hsjgFieldIndex == -1)
                {
                    System.Diagnostics.Debug.WriteLine($"警告：{countyName} - 未找到HSJG相关字段");
                    // 列出所有可用字段进行调试
                    ListAllFieldsInFeatureClass(ldhsjgFeatureClass, "LDHSJG");
                }

                // 🔥 改进空间查询：使用更精确的空间关系和容差
                ISpatialFilter spatialFilter = null;
                IFeatureCursor ldhsjgCursor = null;

                try
                {
                    spatialFilter = new SpatialFilterClass();
                    spatialFilter.Geometry = sourceFeature.Shape;
                    spatialFilter.GeometryField = ldhsjgFeatureClass.ShapeFieldName;

                    // 🔥 修复：使用更精确的空间关系 - 先尝试Intersects，如果没有结果再尝试Contains
                    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                    ldhsjgCursor = ldhsjgFeatureClass.Search(spatialFilter, false);
                    IFeature bestMatchFeature = null;
                    double maxOverlapArea = 0;

                    // 🔥 改进：寻找重叠面积最大的LDHSJG要素，而不是第一个相交的要素
                    IFeature ldhsjgFeature;
                    int candidateCount = 0;

                    while ((ldhsjgFeature = ldhsjgCursor.NextFeature()) != null)
                    {
                        candidateCount++;
                        try
                        {
                            if (ldhsjgFeature.Shape != null && !ldhsjgFeature.Shape.IsEmpty)
                            {
                                // 计算重叠面积
                                double overlapArea = CalculateOverlapArea(sourceFeature.Shape, ldhsjgFeature.Shape);

                                System.Diagnostics.Debug.WriteLine($"候选LDHSJG要素 {candidateCount}: 重叠面积 = {overlapArea:F2}");

                                if (overlapArea > maxOverlapArea)
                                {
                                    maxOverlapArea = overlapArea;

                                    // 释放之前的最佳匹配要素
                                    if (bestMatchFeature != null)
                                    {
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(bestMatchFeature);
                                    }

                                    bestMatchFeature = ldhsjgFeature;
                                    ldhsjgFeature = null; // 防止在finally中释放
                                }
                            }
                        }
                        finally
                        {
                            if (ldhsjgFeature != null)
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(ldhsjgFeature);
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"空间查询完成：找到 {candidateCount} 个候选要素，最大重叠面积: {maxOverlapArea:F2}");

                    // 使用最佳匹配的要素获取字段值
                    if (bestMatchFeature != null)
                    {
                        // 获取字段值
                        if (zcqcbsmFieldIndex != -1)
                        {
                            result.ZCQCBSM = bestMatchFeature.get_Value(zcqcbsmFieldIndex)?.ToString() ?? "";
                        }

                        if (ysdmFieldIndex != -1)
                        {
                            result.YSDM = bestMatchFeature.get_Value(ysdmFieldIndex)?.ToString() ?? "";
                        }

                        if (xjxzmcFieldIndex != -1)
                        {
                            result.XZQMC = bestMatchFeature.get_Value(xjxzmcFieldIndex)?.ToString() ?? "";
                        }

                        if (xjxzdmFieldIndex != -1)
                        {
                            result.XZQDM = bestMatchFeature.get_Value(xjxzdmFieldIndex)?.ToString() ?? "";
                        }

                        // 🔥 关键修复：统一HSJG字段值获取
                        if (hsjgFieldIndex != -1)
                        {
                            result.HSJG = bestMatchFeature.get_Value(hsjgFieldIndex);

                            // 调试输出：显示获取到的HSJG值和字段名
                            string fieldName = bestMatchFeature.Fields.get_Field(hsjgFieldIndex).Name;
                            //System.Diagnostics.Debug.WriteLine($"成功获取{countyName}的HSJG字段值: {result.HSJG} (字段名: {fieldName})");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"警告：{countyName} - HSJG字段索引为-1，无法获取价格信息");
                        }

                        // 释放最佳匹配要素
                        //System.Runtime.InteropServices.Marshal.ReleaseComObject(bestMatchFeature);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"警告：未找到与当前要素相交的{countyName}的LDHSJG要素");

                        // 🔥 备用方案：如果空间查询失败，尝试使用最近邻查询
                        result = TryNearestNeighborLDHSJGQuery(sourceFeature, ldhsjgFeatureClass, countyName);
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
                System.Diagnostics.Debug.WriteLine($"错误堆栈: {ex.StackTrace}");
            }

            return result;
        }

        /// <summary>
        /// 🔥 新增：计算两个几何体的重叠面积
        /// </summary>
        /// <param name="geometry1">几何体1</param>
        /// <param name="geometry2">几何体2</param>
        /// <returns>重叠面积</returns>
        private double CalculateOverlapArea(IGeometry geometry1, IGeometry geometry2)
        {
            try
            {
                if (geometry1 == null || geometry2 == null || geometry1.IsEmpty || geometry2.IsEmpty)
                {
                    return 0;
                }

                ITopologicalOperator topoOperator = (ITopologicalOperator)geometry1;
                IGeometry intersectionGeometry = topoOperator.Intersect(geometry2, esriGeometryDimension.esriGeometry2Dimension);

                if (intersectionGeometry != null && !intersectionGeometry.IsEmpty)
                {
                    IArea area = (IArea)intersectionGeometry;
                    double overlapArea = Math.Abs(area.Area);

                    // 释放交集几何对象
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(intersectionGeometry);

                    return overlapArea;
                }

                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"计算重叠面积时出错: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 🔥 新增：备用方案 - 使用最近邻查询获取LDHSJG数据
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="ldhsjgFeatureClass">LDHSJG要素类</param>
        /// <param name="countyName">县名</param>
        /// <returns>LDHSJG字段值</returns>
        private LDHSJGValues TryNearestNeighborLDHSJGQuery(IFeature sourceFeature, IFeatureClass ldhsjgFeatureClass, string countyName)
        {
            var result = new LDHSJGValues();

            try
            {
                System.Diagnostics.Debug.WriteLine($"尝试{countyName}的最近邻LDHSJG查询...");

                // 获取源要素的中心点
                IArea area = (IArea)sourceFeature.Shape;
                IPoint centerPoint = area.Centroid;

                // 查找距离中心点最近的LDHSJG要素
                IFeatureCursor cursor = null;
                try
                {
                    cursor = ldhsjgFeatureClass.Search(null, false);
                    IFeature nearestFeature = null;
                    double minDistance = double.MaxValue;

                    IFeature ldhsjgFeature;
                    while ((ldhsjgFeature = cursor.NextFeature()) != null)
                    {
                        try
                        {
                            if (ldhsjgFeature.Shape != null)
                            {
                                IArea ldhsjgArea = (IArea)ldhsjgFeature.Shape;
                                IPoint ldhsjgCenter = ldhsjgArea.Centroid;

                                // 计算距离
                                double distance = CalculateDistance(centerPoint, ldhsjgCenter);

                                if (distance < minDistance)
                                {
                                    minDistance = distance;

                                    if (nearestFeature != null)
                                    {
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(nearestFeature);
                                    }

                                    nearestFeature = ldhsjgFeature;
                                    ldhsjgFeature = null; // 防止释放
                                }

                                System.Runtime.InteropServices.Marshal.ReleaseComObject(ldhsjgCenter);
                            }
                        }
                        finally
                        {
                            if (ldhsjgFeature != null)
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(ldhsjgFeature);
                            }
                        }
                    }

                    if (nearestFeature != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"找到最近的LDHSJG要素，距离: {minDistance:F2}");

                        // 获取字段值（使用与主方法相同的逻辑）
                        int hsjgFieldIndex = -1;
                        string[] possibleHsjgFields = { "XJLDPJJ", "HSJG", "林地定级平均价", "平均价格", "PJJ" };

                        foreach (string fieldName in possibleHsjgFields)
                        {
                            hsjgFieldIndex = ldhsjgFeatureClass.FindField(fieldName);
                            if (hsjgFieldIndex != -1) break;
                        }

                        if (hsjgFieldIndex != -1)
                        {
                            result.HSJG = nearestFeature.get_Value(hsjgFieldIndex);
                            System.Diagnostics.Debug.WriteLine($"最近邻查询获取到HSJG值: {result.HSJG}");
                        }

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(nearestFeature);
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(centerPoint);
                }
                finally
                {
                    if (cursor != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"最近邻LDHSJG查询出错: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 🔥 新增：计算两点之间的距离
        /// </summary>
        /// <param name="point1">点1</param>
        /// <param name="point2">点2</param>
        /// <returns>距离</returns>
        private double CalculateDistance(IPoint point1, IPoint point2)
        {
            try
            {
                double dx = point1.X - point2.X;
                double dy = point1.Y - point2.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }
            catch
            {
                return double.MaxValue;
            }
        }

        /// <summary>
        /// 🔥 新增：列出要素类中的所有字段（调试用）
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <param name="description">描述</param>
        private void ListAllFieldsInFeatureClass(IFeatureClass featureClass, string description)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== {description} 要素类字段列表 ===");
                IFields fields = featureClass.Fields;

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    System.Diagnostics.Debug.WriteLine($"  字段 {i}: {field.Name} ({field.Type})");
                }

                System.Diagnostics.Debug.WriteLine($"=== 共 {fields.FieldCount} 个字段 ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"列出字段时出错: {ex.Message}");
            }
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
        /// 计算几何对象与CZKFBJ的相交面积（改进版：基于比例计算）
        /// 新逻辑：CZKFBJMJ = GTDCTBMJ × (图斑在CZKFBJ内的面积比例)
        /// </summary>
        /// <param name="geometry">要计算的几何对象</param>
        /// <param name="czkfbjFeatureClass">CZKFBJ要素类</param>
        /// <param name="gtdctbmjValue">图斑的GTDCTBMJ值</param>
        /// <returns>按比例计算的CZKFBJMJ值</returns>
        private double CalculateIntersectionAreaWithCZKFBJ(IGeometry geometry, IFeatureClass czkfbjFeatureClass, double gtdctbmjValue = 0)
        {
            if (geometry == null || czkfbjFeatureClass == null)
            {
                return 0;
            }

            IFeatureCursor czkfbjCursor = null;
            ISpatialFilter spatialFilter = null;

            try
            {
                if (geometry.IsEmpty)
                {
                    return 0;
                }

                // 计算原图斑的总面积
                IArea originalArea = (IArea)geometry;
                double totalOriginalArea = Math.Abs(originalArea.Area);

                if (totalOriginalArea <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("警告：原图斑面积为0或负数");
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

                double totalIntersectionArea = 0;

                while ((czkfbjFeature = czkfbjCursor.NextFeature()) != null)
                {
                    try
                    {
                        if (czkfbjFeature.Shape != null && !czkfbjFeature.Shape.IsEmpty)
                        {
                            // 计算交集几何
                            ITopologicalOperator topoOperator = (ITopologicalOperator)geometry;
                            IGeometry intersectionGeometry = topoOperator.Intersect(
                                czkfbjFeature.Shape,
                                esriGeometryDimension.esriGeometry2Dimension);

                            if (intersectionGeometry != null && !intersectionGeometry.IsEmpty)
                            {
                                IArea intersectionArea = (IArea)intersectionGeometry;
                                double currentIntersectionArea = Math.Abs(intersectionArea.Area);
                                totalIntersectionArea += currentIntersectionArea;

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

                // 🔥 新逻辑：计算比例并应用到GTDCTBMJ值
                double intersectionRatio = totalIntersectionArea / totalOriginalArea;

                // 确保比例在合理范围内（0-1之间）
                if (intersectionRatio > 1.0)
                {
                    System.Diagnostics.Debug.WriteLine($"警告：交集比例({intersectionRatio:F4})大于1，将其调整为1.0");
                    intersectionRatio = 1.0;
                }
                else if (intersectionRatio < 0)
                {
                    System.Diagnostics.Debug.WriteLine($"警告：交集比例({intersectionRatio:F4})小于0，将其调整为0");
                    intersectionRatio = 0;
                }

                // 根据比例计算CZKFBJMJ值
                double czkfbjmjValue = gtdctbmjValue * intersectionRatio;

                //System.Diagnostics.Debug.WriteLine($"CZKFBJMJ计算详情：");
                //System.Diagnostics.Debug.WriteLine($"  原图斑面积: {totalOriginalArea:F2}");
                //System.Diagnostics.Debug.WriteLine($"  交集面积: {totalIntersectionArea:F2}");
                //System.Diagnostics.Debug.WriteLine($"  交集比例: {intersectionRatio:F4} ({intersectionRatio * 100:F2}%)");
                //System.Diagnostics.Debug.WriteLine($"  GTDCTBMJ值: {gtdctbmjValue:F2}");
                //System.Diagnostics.Debug.WriteLine($"  计算的CZKFBJMJ: {czkfbjmjValue:F2} = {gtdctbmjValue:F2} × {intersectionRatio:F4}");

                return czkfbjmjValue;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"计算与CZKFBJ交集比例时出错: {ex.Message}");
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

        private IWorkspace CreateCountyShapefileWorkspace(string outputPath, string countyName, string outputShapefileName)
        {
            try
            {
                // 修改：使用县代码映射器获取真实的县代码
                string countyCode = Utils.CountyCodeMapper.GetCountyCode(countyName);
                string countyFolderName = $"{countyName}({countyCode})全民所有自然资源资产清查数据成果";
                string countyFolderPath = System.IO.Path.Combine(outputPath, countyFolderName);
                string dataSetPath = System.IO.Path.Combine(countyFolderPath, "清查数据集");

                // 🔥 修改：根据输出文件名选择资源文件夹的逻辑
                string resourceFolder = GetResourceFolderByOutputType(outputShapefileName);

                string forestPath = System.IO.Path.Combine(dataSetPath, resourceFolder);
                string spatialDataPath = System.IO.Path.Combine(forestPath, "空间数据");

                // 使用ProgID创建Shapefile工作空间工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // 打开工作空间
                IWorkspace workspace = workspaceFactory.OpenFromFile(spatialDataPath, 0);

                System.Diagnostics.Debug.WriteLine($"成功创建{countyName}({countyCode})的Shapefile工作空间: {spatialDataPath} (资源类型: {resourceFolder})");
                return workspace;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"创建{countyName}Shapefile工作空间时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 根据输出类型获取资源文件夹名称
        /// </summary>
        /// <param name="outputShapefileName">输出Shapefile名称</param>
        /// <returns>资源文件夹名称</returns>
        private string GetResourceFolderByOutputType(string outputShapefileName)
        {
            if (string.IsNullOrEmpty(outputShapefileName))
            {
                // 从当前数据类型选择动态获取
                string currentOutputType = GetCurrentOutputShapefileName();
                return GetResourceFolderByOutputType(currentOutputType);
            }

            if (outputShapefileName.Contains("CYZYZC"))
            {
                return "草原";
            }
            else if (outputShapefileName.Contains("SDZYZC"))
            {
                return "湿地";
            }
            else // SLZYZC 或其他情况
            {
                return "森林";
            }
        }

        /// <summary>
        /// 创建输出 Shapefile要素类（支持动态数据类型）
        /// </summary>
        /// <param name="workspace">Shapefile工作空间</param>
        /// <param name="geometryType">几何类型</param>
        /// <param name="spatialReference">空间参考</param>
        /// <param name="outputShapefileName">输出Shapefile名称</param>
        /// <returns>输出要素类接口</returns>
        private IFeatureClass CreateOutputShapefile(IWorkspace workspace, esriGeometryType geometryType, ISpatialReference spatialReference, string outputShapefileName)
        {
            IFeatureWorkspace featureWorkspace = null;

            try
            {
                System.Diagnostics.Debug.WriteLine($"开始创建{outputShapefileName} Shapefile，工作空间路径: {workspace.PathName}");

                featureWorkspace = (IFeatureWorkspace)workspace;

                // 检查工作空间路径是否存在
                string workspacePath = workspace.PathName;
                if (!Directory.Exists(workspacePath))
                {
                    string errorMsg = $"工作空间路径不存在: {workspacePath}";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new DirectoryNotFoundException(errorMsg);
                }

                // 查找符合模式的输出 Shapefile文件
                string targetFeatureClassName = FindOutputShapefileName(workspacePath, outputShapefileName);

                if (string.IsNullOrEmpty(targetFeatureClassName))
                {
                    string errorMsg = $"未找到符合模式的{outputShapefileName} Shapefile文件: {workspacePath}";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new FileNotFoundException(errorMsg);
                }

                System.Diagnostics.Debug.WriteLine($"找到目标Shapefile: {targetFeatureClassName}");

                // 检查相关的Shapefile组件文件是否完整
                string[] requiredExtensions = { ".shx", ".dbf", ".prj" };
                foreach (string ext in requiredExtensions)
                {
                    string componentFile = System.IO.Path.Combine(workspacePath, $"{targetFeatureClassName}{ext}");
                    if (!File.Exists(componentFile))
                    {
                        string errorMsg = $"Shapefile组件文件缺失: {componentFile}";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        throw new FileNotFoundException(errorMsg, componentFile);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Shapefile文件完整性检查通过: {targetFeatureClassName}");

                // 检查文件是否被锁定
                string shapefilePath = System.IO.Path.Combine(workspacePath, $"{targetFeatureClassName}.shp");
                try
                {
                    using (System.IO.FileStream fs = File.OpenRead(shapefilePath))
                    {
                        // 如果能打开文件，说明没有被锁定
                    }
                }
                catch (IOException ex)
                {
                    string errorMsg = $"Shapefile文件被锁定或无法访问: {shapefilePath}, 错误: {ex.Message}";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    throw new IOException(errorMsg, ex);
                }

                // 尝试打开要素类
                System.Diagnostics.Debug.WriteLine($"尝试打开要素类: {targetFeatureClassName}");

                try
                {
                    IFeatureClass existingFeatureClass = featureWorkspace.OpenFeatureClass(targetFeatureClassName);

                    if (existingFeatureClass == null)
                    {
                        string errorMsg = $"OpenFeatureClass返回null: {targetFeatureClassName}";
                        System.Diagnostics.Debug.WriteLine(errorMsg);
                        throw new Exception(errorMsg);
                    }

                    System.Diagnostics.Debug.WriteLine($"成功打开要素类: {targetFeatureClassName}");
                    System.Diagnostics.Debug.WriteLine($"要素类当前要素数量: {existingFeatureClass.FeatureCount(null)}");

                    // 清空现有数据
                    System.Diagnostics.Debug.WriteLine("开始清空现有Shapefile数据...");
                    ClearExistingShapefileData(existingFeatureClass);
                    System.Diagnostics.Debug.WriteLine("Shapefile数据清空完成");

                    return existingFeatureClass;
                }
                catch (System.Runtime.InteropServices.COMException comEx)
                {
                    string errorMsg = $"COM异常 - 无法打开要素类 '{targetFeatureClassName}': HRESULT=0x{comEx.HResult:X8}, 消息={comEx.Message}";
                    System.Diagnostics.Debug.WriteLine(errorMsg);
                    System.Diagnostics.Debug.WriteLine($"COM异常详情: {comEx}");

                    // 提供更详细的错误分析
                    switch ((uint)comEx.HResult)
                    {
                        case 0x80040351:
                            errorMsg += "\n原因分析: 要素类不存在或无法访问";
                            break;
                        case 0x80040352:
                            errorMsg += "\n原因分析: 工作空间类型不支持";
                            break;
                        case 0x80040353:
                            errorMsg += "\n原因分析: 数据源损坏或格式错误";
                            break;
                        default:
                            errorMsg += $"\n原因分析: 未知的COM错误 (HRESULT: 0x{comEx.HResult:X8})";
                            break;
                    }

                    throw new Exception(errorMsg, comEx);
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"创建{outputShapefileName} Shapefile时发生错误: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(errorMsg);
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                throw;
            }
        }
        /// <summary>
        /// 创建SLZYZC Shapefile要素类（重构为通用方法）
        /// </summary>
        /// <param name="workspace">Shapefile工作空间</param>
        /// <param name="geometryType">几何类型</param>
        /// <param name="spatialReference">空间参考</param>
        /// <returns>SLZYZC要素类接口</returns>
        private IFeatureClass CreateSLZYZCShapefile(IWorkspace workspace, esriGeometryType geometryType, ISpatialReference spatialReference)
        {
            // 🔥 修改：从Basic.cs获取当前的输出文件名类型
            string outputShapefileName = GetCurrentOutputShapefileName();

            System.Diagnostics.Debug.WriteLine($"创建Shapefile: 当前输出类型为 {outputShapefileName}");

            return CreateOutputShapefile(workspace, geometryType, spatialReference, outputShapefileName);
        }

        /// <summary>
        /// 获取当前的输出Shapefile名称（从Basic窗体或SharedDataManager）
        /// </summary>
        /// <returns>输出Shapefile名称（SLZYZC、CYZYZC或SDZYZC）</returns>
        private string GetCurrentOutputShapefileName()
        {
            try
            {
                // 从SharedDataManager获取数据类型选择状态
                var dataTypeSelection = SharedDataManager.GetDataTypeSelection();

                if (dataTypeSelection.Forest && !dataTypeSelection.Grassland && !dataTypeSelection.Wetland)
                {
                    return "SLZYZC"; // 森林资源
                }
                else if (!dataTypeSelection.Forest && dataTypeSelection.Grassland && !dataTypeSelection.Wetland)
                {
                    return "CYZYZC"; // 草地资源
                }
                else if (!dataTypeSelection.Forest && !dataTypeSelection.Grassland && dataTypeSelection.Wetland)
                {
                    return "SDZYZC"; // 湿地资源
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("警告：未检测到有效的数据类型选择，默认使用SLZYZC");
                    return "SLZYZC"; // 默认为森林资源
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取输出Shapefile名称时出错: {ex.Message}，使用默认值SLZYZC");
                return "SLZYZC"; // 出错时使用默认值
            }
        }



        /// <summary>
        /// 在指定路径中查找符合模式的SLZYZC Shapefile文件名
        /// 支持以下命名模式：
        /// 1. (县代码)SLZYZC.shp
        /// 2. SLZYZC.shp
        /// </summary>
        /// <param name="workspacePath">工作空间路径</param>
        /// <returns>找到的Shapefile文件名（不含扩展名），如果未找到返回null</returns>
        private string FindSLZYZCShapefileName(string workspacePath)
        {
            try
            {
                if (!Directory.Exists(workspacePath))
                {
                    System.Diagnostics.Debug.WriteLine($"工作空间路径不存在: {workspacePath}");
                    return null;
                }

                // 获取所有.shp文件
                string[] shapefiles = Directory.GetFiles(workspacePath, "*.shp");
                System.Diagnostics.Debug.WriteLine($"工作空间中找到 {shapefiles.Length} 个Shapefile文件");

                // 列出所有找到的文件
                foreach (string file in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    System.Diagnostics.Debug.WriteLine($"  发现Shapefile: {fileName}");
                }

                // 优先查找带县代码的SLZYZC文件：(县代码)SLZYZC.shp
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式1: (数字)SLZYZC 或 （数字）SLZYZC
                    if (System.Text.RegularExpressions.Regex.IsMatch(fileName, @"^[（(]\d+[）)]SLZYZC$"))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到带县代码的SLZYZC文件: {fileName}");
                        return fileName;
                    }
                }

                // 如果没有找到带县代码的，查找标准的SLZYZC文件
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式2: 精确匹配 SLZYZC
                    if (fileName.Equals("SLZYZC", StringComparison.OrdinalIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到标准SLZYZC文件: {fileName}");
                        return fileName;
                    }
                }

                // 最后尝试模糊匹配包含SLZYZC的文件
                foreach (string shapefilePath in shapefiles)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                    // 模式3: 包含SLZYZC的文件
                    if (fileName.Contains("SLZYZC"))
                    {
                        System.Diagnostics.Debug.WriteLine($"找到包含SLZYZC的文件: {fileName}");
                        return fileName;
                    }
                }

                System.Diagnostics.Debug.WriteLine("未找到任何符合模式的SLZYZC Shapefile文件");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找SLZYZC Shapefile文件时出错: {ex.Message}");
                return null;
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
                int ysdmIndex = targetFeatureClass.FindField("YSDM");  // 🔥 新增：获取YSDM字段索引
                int czkfbjmjIndex = targetFeatureClass.FindField("CZKFBJMJ");
                int pctbbmIndex = targetFeatureClass.FindField("PCTBBM");
                int ztbxjIndex = targetFeatureClass.FindField("ZTBXJ");
                int xzqmcIndex = targetFeatureClass.FindField("XZQMC");

                // 预先获取源字段索引（特殊字段）
                int xianIndex = sourceFeatureClass.FindField("XIAN");
                int linBanIndex = sourceFeatureClass.FindField("LIN_BAN");
                int xiaoBanIndex = sourceFeatureClass.FindField("XIAO_BAN");
                int xbmjIndex = sourceFeatureClass.FindField("XBMJ");
                int mgqxjIndex = sourceFeatureClass.FindField("MEI_GQ_XJ");
                int xzqdmSourceIndex = xianIndex != -1 ? xianIndex : sourceFeatureClass.FindField("XZQDM");

                System.Diagnostics.Debug.WriteLine($"预缓存字段索引完成: YSDM={ysdmIndex}, PCTBBM={pctbbmIndex}, ZTBXJ={ztbxjIndex}, XZQMC={xzqmcIndex}");

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

                        // 🔥 修改：根据数据类型设置不同的YSDM值
                        if (ysdmIndex != -1)
                        {
                            // 原代码：featureBuffer.set_Value(ysdmIndex, "2150201020");
                            string currentOutputType = GetCurrentOutputShapefileName();
                            string ysdmValue = currentOutputType == "CYZYZC" ? "2160301000" : "2150201020";
                            featureBuffer.set_Value(ysdmIndex, ysdmValue);
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

                                //System.Diagnostics.Debug.WriteLine($"PCTBBM字段合并完成: {pctbbmValue}");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"处理PCTBBM字段时出错: {ex.Message}");
                            }
                        }

                        // 处理特殊字段：ZTBXJ (XBMJ * MEI_GQ_XJ)
                        if (ztbxjIndex != -1)
                        {
                            try
                            {
                                if (xbmjIndex != -1 && mgqxjIndex != -1)
                                {
                                    object xbmjValue = sourceFeature.get_Value(xbmjIndex);
                                    object mgqxjValue = sourceFeature.get_Value(mgqxjIndex);

                                    if (double.TryParse(xbmjValue?.ToString(), out double xbmjNum) &&
                                        double.TryParse(mgqxjValue?.ToString(), out double mgqxjNum))
                                    {
                                        double ztbxjValue = xbmjNum * mgqxjNum;
                                        featureBuffer.set_Value(ztbxjIndex, ztbxjValue);
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
                //{ "YSDM", "ysdm" },            // 要素代码
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
                // 格式: XZQDM(6) + 中间代码 + 图斑序号(12)
                int xzqdmIndex = sourceFeatureClass.FindField("xian");
                if (xzqdmIndex != -1)
                {
                    string xzqdm = sourceFeature.get_Value(xzqdmIndex)?.ToString() ?? "";
                    if (xzqdm.Length > 6)
                    {
                        xzqdm = xzqdm.Substring(0, 6);
                    }

                    // 🔥 修改：根据数据类型设置不同的中间代码
                    string currentOutputType = GetCurrentOutputShapefileName();
                    string middleCode;
                    switch (currentOutputType)
                    {
                        case "CYZYZC":
                            middleCode = "5120";  // 草地资源
                            break;
                        case "SDZYZC":
                            middleCode = "6120";  // 湿地资源
                            break;
                        default: // SLZYZC
                            middleCode = "4120";  // 森林资源
                            break;
                    }

                    string sequenceStr = featureSequence.ToString("D12");
                    return $"{xzqdm}{middleCode}{sequenceStr}";
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