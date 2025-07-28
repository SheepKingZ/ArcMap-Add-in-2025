using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestArcMapAddin2.Utils
{
    /// <summary>
    /// CZCDYDQC处理器 - 处理城镇村等用地调查数据
    /// 实现完整的城镇村范围内资源提取、裁剪、合并流程
    /// </summary>
    public class CZCDYDProcessor
    {
        /// <summary>
        /// 进度回调委托
        /// </summary>
        /// <param name="percentage">完成百分比</param>
        /// <param name="message">进度消息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 处理结果
        /// </summary>
        public class ProcessingResult
        {
            public bool Success { get; set; }
            public string OutputPath { get; set; }
            public int ProcessedFeatureCount { get; set; }
            public string ErrorMessage { get; set; }
            public List<string> TemporaryFiles { get; set; } = new List<string>();
        }

        /// <summary>
        /// 县级文件组信息
        /// </summary>
        public class CountyFiles
        {
            public string CountyCode { get; set; }
            public string SlzyzcDltbFile { get; set; }
            public string CyzyzcDltbFile { get; set; }
            public string SdzyzcDltbFile { get; set; }
            public string CzcdydFile { get; set; }
            public string OutputDirectory { get; set; }
        }

        /// <summary>
        /// 处理单个县的CZCDYDQC数据
        /// </summary>
        /// <param name="countyFiles">县级文件信息</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>处理结果</returns>
        public ProcessingResult ProcessCountyCZCDYDQC(CountyFiles countyFiles, ProgressCallback progressCallback = null)
        {
            var result = new ProcessingResult();

            try
            {
                progressCallback?.Invoke(0, $"开始处理县代码 {countyFiles.CountyCode} 的CZCDYDQC数据...");

                // 构建目标 Shapefile 路径
                string countyName = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyNameFromCode(countyFiles.CountyCode);
                string countyFolderName = $"{countyName}({countyFiles.CountyCode})全民所有自然资源资产清查数据成果";
                string targetShapefilePath = System.IO.Path.Combine(countyFiles.OutputDirectory, $"({countyFiles.CountyCode})CZCDYDQC.shp");

                // 验证目标 Shapefile 是否存在
                if (!File.Exists(targetShapefilePath))
                {
                    result.ErrorMessage = $"目标Shapefile不存在: {targetShapefilePath}";
                    return result;
                }

                // 步骤1: 处理城镇村等用地数据并直接写入目标 Shapefile
                progressCallback?.Invoke(20, "正在处理城镇村等用地数据...");
                var writeResult = ProcessAndWriteCZCDYDQCData(countyFiles, targetShapefilePath, progressCallback);

                if (!writeResult.Success)
                {
                    result.ErrorMessage = writeResult.ErrorMessage;
                    return result;
                }

                result.Success = true;
                result.OutputPath = targetShapefilePath;
                result.ProcessedFeatureCount = writeResult.ProcessedFeatureCount;
                progressCallback?.Invoke(100, "CZCDYDQC数据处理完成");

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"处理CZCDYDQC数据时出错: {ex.Message}");
                return result;
            }
        }
        /// <summary>
        /// 处理城镇村等用地数据并写入目标Shapefile
        /// </summary>
        /// <param name="countyFiles">县级文件信息</param>
        /// <param name="targetShapefilePath">目标Shapefile路径</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>写入操作结果</returns>
        private GISOperationUtils.WriteDataResult ProcessAndWriteCZCDYDQCData(CountyFiles countyFiles,
            string targetShapefilePath, ProgressCallback progressCallback)
        {
            var result = new GISOperationUtils.WriteDataResult();
            var featureDataList = new List<GISOperationUtils.FeatureData>();

            try
            {
                progressCallback?.Invoke(25, "正在处理森林资源数据...");
                var slData = ProcessResourceDataToFeatureData(countyFiles.SlzyzcDltbFile, countyFiles.CzcdydFile,
                    countyFiles.CountyCode, "SL");
                if (slData != null) featureDataList.AddRange(slData);

                progressCallback?.Invoke(45, "正在处理草地资源数据...");
                var cdData = ProcessResourceDataToFeatureData(countyFiles.CyzyzcDltbFile, countyFiles.CzcdydFile,
                    countyFiles.CountyCode, "CD");
                if (cdData != null) featureDataList.AddRange(cdData);

                progressCallback?.Invoke(65, "正在处理湿地资源数据...");
                var sdData = ProcessResourceDataToFeatureData(countyFiles.SdzyzcDltbFile, countyFiles.CzcdydFile,
                    countyFiles.CountyCode, "SD");
                if (sdData != null) featureDataList.AddRange(sdData);

                progressCallback?.Invoke(80, "正在写入数据到目标Shapefile...");

                // 写入到目标Shapefile
                var writeProgressCallback = new GISOperationUtils.ProgressCallback((percentage, message) =>
                {
                    int adjustedProgress = 80 + (percentage * 15) / 100;
                    progressCallback?.Invoke(adjustedProgress, message);
                });

                return GISOperationUtils.WriteDataToExistingShapefile(targetShapefilePath, featureDataList, writeProgressCallback);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"处理和写入CZCDYDQC数据时出错: {ex.Message}");
                return result;
            }
        }
        /// <summary>
        /// 处理单类资源数据并返回要素数据列表
        /// </summary>
        /// <param name="resourceFile">资源shapefile路径</param>
        /// <param name="czcdydFile">城镇村等用地文件路径</param>
        /// <param name="countyCode">县代码</param>
        /// <param name="resourceType">资源类型(SL/CD/SD)</param>
        /// <returns>要素数据列表</returns>
        private List<GISOperationUtils.FeatureData> ProcessResourceDataToFeatureData(string resourceFile,
            string czcdydFile, string countyCode, string resourceType)
        {
            var featureDataList = new List<GISOperationUtils.FeatureData>();

            try
            {
                if (string.IsNullOrEmpty(resourceFile) || !File.Exists(resourceFile))
                {
                    System.Diagnostics.Debug.WriteLine($"资源文件不存在: {resourceFile}");
                    return featureDataList;
                }

                // 打开资源数据
                var resourceResult = OpenShapefile(resourceFile);
                if (resourceResult.featureClass == null)
                {
                    return featureDataList;
                }

                // 打开城镇村数据
                var czcdydResult = OpenShapefile(czcdydFile);
                if (czcdydResult.featureClass == null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resourceResult.featureClass);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resourceResult.workspace);
                    return featureDataList;
                }

                try
                {
                    // 检查空间参考系统的一致性
                    ISpatialReference resourceSR = ((IGeoDataset)resourceResult.featureClass).SpatialReference;
                    ISpatialReference czcdydSR = ((IGeoDataset)czcdydResult.featureClass).SpatialReference;

                    System.Diagnostics.Debug.WriteLine($"资源数据空间参考: {resourceSR?.Name ?? "未定义"}");
                    System.Diagnostics.Debug.WriteLine($"城镇村数据空间参考: {czcdydSR?.Name ?? "未定义"}");

                    // 创建查询过滤器，提取城市、建制镇、村庄
                    IQueryFilter czcdFilter = new QueryFilterClass();
                    czcdFilter.WhereClause = "CZCLX IN ('201', '201A', '202', '202A', '203', '203A')";

                    // 收集城镇村几何并进行空间参考统一
                    var czcdGeometries = new List<IGeometry>();
                    IFeatureCursor czcdCursor = czcdydResult.featureClass.Search(czcdFilter, false);
                    IFeature czcdFeature;

                    while ((czcdFeature = czcdCursor.NextFeature()) != null)
                    {
                        try
                        {
                            if (czcdFeature.Shape != null && !czcdFeature.Shape.IsEmpty)
                            {
                                IGeometry czcdGeometry = czcdFeature.ShapeCopy;

                                // 🔥 修复1: 验证几何有效性
                                if (!IsGeometryValid(czcdGeometry))
                                {
                                    System.Diagnostics.Debug.WriteLine($"城镇村几何无效，尝试修复...");
                                    czcdGeometry = RepairGeometry(czcdGeometry);
                                    if (czcdGeometry == null || czcdGeometry.IsEmpty)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"几何修复失败，跳过此要素");
                                        continue;
                                    }
                                }

                                // 🔥 修复2: 统一空间参考系统
                                if (resourceSR != null && czcdydSR != null &&
                                    !IsSpatialReferenceEqual(resourceSR, czcdydSR))
                                {
                                    System.Diagnostics.Debug.WriteLine($"投影城镇村几何到资源数据坐标系");
                                    try
                                    {
                                        czcdGeometry.Project(resourceSR);
                                    }
                                    catch (Exception projEx)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"投影失败: {projEx.Message}，跳过此要素");
                                        continue;
                                    }
                                }

                                czcdGeometries.Add(czcdGeometry);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"处理城镇村要素时出错: {ex.Message}");
                        }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(czcdFeature);
                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czcdCursor);

                    if (czcdGeometries.Count == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"未找到有效的城镇村范围，跳过{resourceType}资源处理");
                        return featureDataList;
                    }

                    System.Diagnostics.Debug.WriteLine($"成功加载 {czcdGeometries.Count} 个城镇村几何");

                    // 处理资源数据
                    IFeatureCursor resourceCursor = resourceResult.featureClass.Search(null, false);
                    IFeature resourceFeature;
                    int fidCounter = 1;
                    int processedCount = 0;
                    int errorCount = 0;

                    while ((resourceFeature = resourceCursor.NextFeature()) != null)
                    {
                        try
                        {
                            processedCount++;
                            if (resourceFeature.Shape != null && !resourceFeature.Shape.IsEmpty)
                            {
                                IGeometry resourceGeometry = resourceFeature.ShapeCopy;

                                // 🔥 修复3: 验证资源几何有效性
                                if (!IsGeometryValid(resourceGeometry))
                                {
                                    System.Diagnostics.Debug.WriteLine($"资源几何无效，尝试修复...");
                                    resourceGeometry = RepairGeometry(resourceGeometry);
                                    if (resourceGeometry == null || resourceGeometry.IsEmpty)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"资源几何修复失败，跳过第{processedCount}个要素");
                                        errorCount++;
                                        continue;
                                    }
                                }

                                // 检查与城镇村范围的交集
                                bool foundIntersection = false;
                                foreach (var czcdGeometry in czcdGeometries)
                                {
                                    try
                                    {
                                        // 🔥 修复4: 改进的几何交集计算
                                        var intersectionResult = CalculateSafeIntersection(resourceGeometry, czcdGeometry);

                                        if (intersectionResult.Success && intersectionResult.Intersection != null &&
                                            !intersectionResult.Intersection.IsEmpty)
                                        {
                                            var featureData = new GISOperationUtils.FeatureData();
                                            featureData.Geometry = intersectionResult.Intersection;

                                            // 处理字段映射和特殊计算
                                            ProcessFeatureMappingToAttributes(resourceFeature, resourceResult.featureClass,
                                                featureData.Attributes, countyCode, fidCounter);

                                            featureDataList.Add(featureData);
                                            fidCounter++;
                                            foundIntersection = true;
                                            break; // 找到交集后跳出内层循环
                                        }

                                        // 释放临时交集几何
                                        if (intersectionResult.Intersection != null)
                                        {
                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(intersectionResult.Intersection);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"处理{resourceType}要素交集时出错: {ex.Message}");
                                        errorCount++;
                                    }
                                }

                                // 释放资源几何副本
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(resourceGeometry);
                            }

                            // 定期输出进度
                            if (processedCount % 100 == 0)
                            {
                                System.Diagnostics.Debug.WriteLine($"已处理{resourceType}资源 {processedCount} 个要素，生成 {featureDataList.Count} 个交集要素，错误 {errorCount} 个");
                            }
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            System.Diagnostics.Debug.WriteLine($"处理第{processedCount}个{resourceType}要素时出错: {ex.Message}");
                        }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(resourceFeature);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resourceCursor);

                    // 释放城镇村几何
                    foreach (var geom in czcdGeometries)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(geom);
                    }

                    System.Diagnostics.Debug.WriteLine($"处理{resourceType}资源完成，总计处理 {processedCount} 个要素，生成 {featureDataList.Count} 个交集要素，错误 {errorCount} 个");
                    return featureDataList;
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resourceResult.featureClass);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resourceResult.workspace);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czcdydResult.featureClass);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czcdydResult.workspace);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理{resourceType}资源数据时出错: {ex.Message}");
                return featureDataList;
            }
        }

        /// <summary>
        /// 🔥 新增: 安全的几何交集计算结果
        /// </summary>
        private class IntersectionResult
        {
            public bool Success { get; set; }
            public IGeometry Intersection { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// 🔥 新增: 安全的几何交集计算
        /// </summary>
        /// <param name="geometry1">几何1</param>
        /// <param name="geometry2">几何2</param>
        /// <returns>交集计算结果</returns>
        private IntersectionResult CalculateSafeIntersection(IGeometry geometry1, IGeometry geometry2)
        {
            var result = new IntersectionResult();

            try
            {
                // 验证几何对象
                if (geometry1 == null || geometry1.IsEmpty || geometry2 == null || geometry2.IsEmpty)
                {
                    result.ErrorMessage = "几何对象为空";
                    return result;
                }

                // 检查几何类型兼容性
                if (!AreGeometryTypesCompatible(geometry1, geometry2))
                {
                    result.ErrorMessage = "几何类型不兼容";
                    return result;
                }

                // 使用更安全的交集计算方法
                ITopologicalOperator topoOp = geometry1 as ITopologicalOperator;
                if (topoOp == null)
                {
                    result.ErrorMessage = "几何对象不支持拓扑操作";
                    return result;
                }

                // 🔥 关键修复: 使用try-catch包装ArcGIS几何操作
                try
                {
                    // 🔥 修复1: 使用正确的关系运算符方法检查相交性
                    IRelationalOperator relOp = geometry1 as IRelationalOperator;
                    if (relOp != null)
                    {
                        // 使用Disjoint方法检查是否不相交，如果不相交则跳过交集计算
                        if (relOp.Disjoint(geometry2))
                        {
                            result.Success = true; // 不相交也是成功的结果
                            result.Intersection = null;
                            return result;
                        }
                    }

                    // 执行交集计算
                    IGeometry intersection = topoOp.Intersect(geometry2, esriGeometryDimension.esriGeometry2Dimension);

                    result.Success = true;
                    result.Intersection = intersection;
                    return result;
                }
                catch (System.Runtime.InteropServices.COMException comEx)
                {
                    result.ErrorMessage = $"ArcGIS几何操作失败: HRESULT={comEx.ErrorCode:X8}, {comEx.Message}";

                    // 🔥 尝试简化几何后重试
                    if (comEx.ErrorCode == unchecked((int)0x80040215))
                    {
                        System.Diagnostics.Debug.WriteLine($"检测到0x80040215错误，尝试简化几何后重试...");
                        return RetryIntersectionWithSimplifiedGeometry(geometry1, geometry2);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"交集计算异常: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// 🔥 新增: 使用简化几何重试交集计算
        /// </summary>
        private IntersectionResult RetryIntersectionWithSimplifiedGeometry(IGeometry geometry1, IGeometry geometry2)
        {
            var result = new IntersectionResult();

            try
            {
                // 简化几何1
                IGeometry simplifiedGeom1 = SimplifyGeometry(geometry1);
                if (simplifiedGeom1 == null || simplifiedGeom1.IsEmpty)
                {
                    result.ErrorMessage = "几何1简化失败";
                    return result;
                }

                // 简化几何2
                IGeometry simplifiedGeom2 = SimplifyGeometry(geometry2);
                if (simplifiedGeom2 == null || simplifiedGeom2.IsEmpty)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(simplifiedGeom1);
                    result.ErrorMessage = "几何2简化失败";
                    return result;
                }

                try
                {
                    // 使用简化后的几何重新计算交集
                    ITopologicalOperator topoOp = simplifiedGeom1 as ITopologicalOperator;
                    IGeometry intersection = topoOp.Intersect(simplifiedGeom2, esriGeometryDimension.esriGeometry2Dimension);

                    result.Success = true;
                    result.Intersection = intersection;

                    return result;
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(simplifiedGeom1);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(simplifiedGeom2);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"简化几何重试失败: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// 🔥 新增: 验证几何对象有效性
        /// </summary>
        /// <param name="geometry">几何对象</param>
        /// <returns>是否有效</returns>
        private bool IsGeometryValid(IGeometry geometry)
        {
            if (geometry == null || geometry.IsEmpty)
                return false;

            try
            {
                // 检查几何是否简单
                ITopologicalOperator topoOp = geometry as ITopologicalOperator;
                if (topoOp != null)
                {
                    return topoOp.IsSimple;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 🔥 新增: 修复几何对象 - 修复版本
        /// </summary>
        /// <param name="geometry">待修复的几何对象</param>
        /// <returns>修复后的几何对象</returns>
        private IGeometry RepairGeometry(IGeometry geometry)
        {
            if (geometry == null || geometry.IsEmpty)
                return null;

            try
            {
                IGeometry repairedGeometry = ((IClone)geometry).Clone() as IGeometry;

                ITopologicalOperator topoOp = repairedGeometry as ITopologicalOperator;
                if (topoOp != null)
                {
                    // 简化几何
                    topoOp.Simplify();

                    // 🔥 修复2: 如果仍然无效，尝试使用缓冲区接口而不是类
                    if (!topoOp.IsSimple)
                    {
                        try
                        {
                            // 使用ITopologicalOperator的Buffer方法代替BufferConstructionClass
                            IGeometry bufferedGeometry = topoOp.Buffer(0);

                            if (bufferedGeometry != null && !bufferedGeometry.IsEmpty)
                            {
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(repairedGeometry);
                                repairedGeometry = bufferedGeometry;
                            }
                        }
                        catch (Exception bufferEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"缓冲区修复失败: {bufferEx.Message}");
                            // 如果缓冲区修复失败，返回简化后的几何
                        }
                    }
                }

                return repairedGeometry;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"修复几何失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 🔥 新增: 简化几何对象
        /// </summary>
        /// <param name="geometry">几何对象</param>
        /// <returns>简化后的几何对象</returns>
        private IGeometry SimplifyGeometry(IGeometry geometry)
        {
            if (geometry == null || geometry.IsEmpty)
                return null;

            try
            {
                IGeometry simplifiedGeometry = ((IClone)geometry).Clone() as IGeometry;

                ITopologicalOperator topoOp = simplifiedGeometry as ITopologicalOperator;
                if (topoOp != null)
                {
                    topoOp.Simplify();
                }

                return simplifiedGeometry;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"简化几何失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 🔥 新增: 检查几何类型兼容性
        /// </summary>
        /// <param name="geometry1">几何1</param>
        /// <param name="geometry2">几何2</param>
        /// <returns>是否兼容</returns>
        private bool AreGeometryTypesCompatible(IGeometry geometry1, IGeometry geometry2)
        {
            try
            {
                esriGeometryType type1 = geometry1.GeometryType;
                esriGeometryType type2 = geometry2.GeometryType;

                // 面几何与面几何兼容
                if ((type1 == esriGeometryType.esriGeometryPolygon || type1 == esriGeometryType.esriGeometryEnvelope) &&
                    (type2 == esriGeometryType.esriGeometryPolygon || type2 == esriGeometryType.esriGeometryEnvelope))
                {
                    return true;
                }

                // 其他类型组合的兼容性检查可以根据需要添加
                return true; // 默认认为兼容，让ArcGIS处理
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 🔥 新增: 比较空间参考系统是否相等
        /// </summary>
        /// <param name="sr1">空间参考1</param>
        /// <param name="sr2">空间参考2</param>
        /// <returns>是否相等</returns>
        private bool IsSpatialReferenceEqual(ISpatialReference sr1, ISpatialReference sr2)
        {
            try
            {
                if (sr1 == null && sr2 == null) return true;
                if (sr1 == null || sr2 == null) return false;

                IClone clone1 = sr1 as IClone;
                IClone clone2 = sr2 as IClone;

                if (clone1 != null && clone2 != null)
                {
                    return clone1.IsEqual(clone2);
                }

                // 简单比较名称
                return string.Equals(sr1.Name, sr2.Name, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 处理要素映射到属性字典
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFC">源要素类</param>
        /// <param name="attributes">目标属性字典</param>
        /// <param name="countyCode">县代码</param>
        /// <param name="fid">要素ID</param>
        private void ProcessFeatureMappingToAttributes(IFeature sourceFeature, IFeatureClass sourceFC,
            Dictionary<string, object> attributes, string countyCode, int fid)
        {
            try
            {
                var fieldMappings = CZCDYDFieldMappings.GetStandardFieldMappings();

                foreach (var mapping in fieldMappings)
                {
                    object value = null;

                    if (mapping.IsSpecialCalculation)
                    {
                        value = CalculateSpecialField(mapping, sourceFeature, sourceFC, countyCode, fid);
                    }
                    else
                    {
                        int sourceIndex = sourceFC.FindField(mapping.SourceField);
                        if (sourceIndex != -1)
                        {
                            value = sourceFeature.get_Value(sourceIndex);
                        }
                    }

                    if (value != null)
                    {
                        attributes[mapping.TargetField] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理要素映射到属性时出错: {ex.Message}");
            }
        }
        /// <summary>
        /// 计算特殊字段
        /// </summary>
        /// <param name="mapping">字段映射</param>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFC">源要素类</param>
        /// <param name="countyCode">县代码</param>
        /// <param name="fid">要素ID</param>
        /// <returns>计算结果</returns>
        private object CalculateSpecialField(CZCDYDFieldMappings.FieldMapping mapping,
            IFeature sourceFeature, IFeatureClass sourceFC, string countyCode, int fid)
        {
            try
            {
                switch (mapping.CalculationType)
                {
                    case "COUNTY_CODE_GENERATION":
                        // ZCQCBSM = 县代码 + 9110 + 12位FID
                        return $"{countyCode}9110{fid:D12}";

                    case "AREA_RATIO_CALCULATION":
                        // HRCZCMJ = GTDCTBMJ * area2 / area1
                        return CalculateAreaRatio(sourceFeature, sourceFC, "GTDCTBMJ", "area2", "area1");

                    case "FIXED_VALUE":
                        // HRCZCTKMJ = 0
                        return 0.0;

                    case "VALUE_RATIO_CALCULATION":
                        // HRCZCJJJZ = JJJZ * area2 / area1
                        return CalculateAreaRatio(sourceFeature, sourceFC, "JJJZ", "area2", "area1");

                    case "PRICE_CALCULATION":
                        // TKJJJJZ = HRCZCMJ * TKJHSJG
                        return CalculatePriceValue(sourceFeature, sourceFC);

                    case "COUNTY_PRICE_LOOKUP":
                        // TKJHSJG = 根据县代码查询最低价
                        return CountyPriceMapping.GetMinimumPrice(countyCode);

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"计算特殊字段 {mapping.TargetField} 时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 计算面积比例
        /// </summary>
        private object CalculateAreaRatio(IFeature feature, IFeatureClass fc, string baseField,
            string numeratorField, string denominatorField)
        {
            try
            {
                int baseIndex = fc.FindField(baseField);
                int numIndex = fc.FindField(numeratorField);
                int denIndex = fc.FindField(denominatorField);

                if (baseIndex == -1 || numIndex == -1 || denIndex == -1) return null;

                double baseValue = Convert.ToDouble(feature.get_Value(baseIndex));
                double numerator = Convert.ToDouble(feature.get_Value(numIndex));
                double denominator = Convert.ToDouble(feature.get_Value(denIndex));

                if (denominator == 0) return 0.0;

                double ratio = numerator / denominator;
                if (ratio > 1.0) ratio = 1.0; // 比例若大于1，强制等于1

                return baseValue * ratio;
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// 计算价格价值
        /// </summary>
        private object CalculatePriceValue(IFeature feature, IFeatureClass fc)
        {
            try
            {
                // 这里需要HRCZCMJ和TKJHSJG字段，但在当前要素中可能还没有设置
                // 暂时返回0，实际实现中需要在所有字段都计算完成后再计算此字段
                return 0.0;
            }
            catch
            {
                return 0.0;
            }
        }

        /// <summary>
        /// 打开shapefile
        /// </summary>
        /// <param name="shapefilePath">shapefile路径</param>
        /// <returns>打开结果</returns>
        private (IWorkspace workspace, IFeatureClass featureClass) OpenShapefile(string shapefilePath)
        {
            try
            {
                if (!File.Exists(shapefilePath))
                {
                    return (null, null);
                }

                string directory = System.IO.Path.GetDirectoryName(shapefilePath);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                IWorkspaceFactory factory = new ShapefileWorkspaceFactory();
                IWorkspace workspace = factory.OpenFromFile(directory, 0);
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);

                return (workspace, featureClass);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"打开shapefile {shapefilePath} 时出错: {ex.Message}");
                return (null, null);
            }
        }

        /// <summary>
        /// 清理临时文件
        /// </summary>
        /// <param name="temporaryFiles">临时文件列表</param>
        private void CleanupTemporaryFiles(List<string> temporaryFiles)
        {
            foreach (string filePath in temporaryFiles)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        // 删除shapefile相关的所有文件
                        string basePath = System.IO.Path.ChangeExtension(filePath, null);
                        string[] extensions = { ".shp", ".shx", ".dbf", ".prj", ".cpg", ".qpj" };

                        foreach (string ext in extensions)
                        {
                            string file = basePath + ext;
                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }
                        }

                        System.Diagnostics.Debug.WriteLine($"已删除临时文件: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"删除临时文件 {filePath} 时出错: {ex.Message}");
                }
            }
        }
    }
}