using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ForestResourcePlugin
{
    /// <summary>
    /// Shapefile结果表转换工具类 - 将LCXZGX.shp转换为SLZYZC.shp
    /// 更新现有的SLZYZC shapefile，包含CZKFBJMJ字段的计算
    /// </summary>
    public class Convert2SLZYZC
    {
        /// <summary>
        /// 进度回调委托 - 用于向UI层报告处理进度
        /// </summary>
        /// <param name="percentage">完成百分比 (0-100)</param>
        /// <param name="message">当前操作描述信息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 将LCXZGX.shp转换为SLZYZC.shp
        /// 更新现有的SLZYZC shapefile而不是创建新的
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="shapefilePath">shapefile目录路径</param>
        /// <param name="fieldMappings">字段映射配置（SLZYZC字段名 -> LCXZGX字段名）</param>
        /// <param name="progressCallback">进度回调</param>
        /// <param name="czkfbjShapefilePath">CZKFBJ shapefile路径（可选）</param>
        /// <returns>转换是否成功</returns>
        public bool ConvertLCXZGXToSLZYZC(
            string countyName,
            string shapefilePath,
            Dictionary<string, string> fieldMappings = null,
            ProgressCallback progressCallback = null,
            string czkfbjShapefilePath = null)
        {
            // 参数验证
            if (string.IsNullOrEmpty(countyName))
            {
                throw new ArgumentException("县名不能为空");
            }

            if (string.IsNullOrEmpty(shapefilePath))
            {
                throw new ArgumentException("Shapefile路径不能为空");
            }

            progressCallback?.Invoke(5, $"正在开始转换{countyName}的LCXZGX到SLZYZC...");

            // COM对象声明
            IWorkspace lcxzgxWorkspace = null;
            IWorkspace slzyzcWorkspace = null;
            IWorkspace czkfbjWorkspace = null;
            IFeatureClass lcxzgxFeatureClass = null;
            IFeatureClass slzyzcFeatureClass = null;
            IFeatureClass czkfbjFeatureClass = null;

            try
            {
                // 构建shapefile路径
                string lcxzgxPath = shapefilePath;
                string countyPath = System.IO.Path.GetDirectoryName(lcxzgxPath);
                string slzyzcPath = System.IO.Path.Combine(countyPath, "SLZYZC.shp");

                // 验证LCXZGX.shp存在
                if (!File.Exists(lcxzgxPath))
                {
                    throw new FileNotFoundException($"LCXZGX.shp文件不存在: {lcxzgxPath}");
                }

                // 验证SLZYZC.shp存在
                if (!File.Exists(slzyzcPath))
                {
                    throw new FileNotFoundException($"SLZYZC.shp文件不存在: {slzyzcPath}");
                }

                progressCallback?.Invoke(15, $"正在打开{countyName}的LCXZGX.shp...");

                // 打开LCXZGX shapefile
                var lcxzgxResult = OpenShapefileFeatureClass(lcxzgxPath);
                lcxzgxWorkspace = lcxzgxResult.workspace;
                lcxzgxFeatureClass = lcxzgxResult.featureClass;

                if (lcxzgxFeatureClass == null)
                {
                    throw new Exception($"无法打开{countyName}的LCXZGX.shp");
                }

                progressCallback?.Invoke(25, $"正在打开{countyName}的SLZYZC.shp...");

                // 打开SLZYZC shapefile
                var slzyzcResult = OpenShapefileFeatureClass(slzyzcPath);
                slzyzcWorkspace = slzyzcResult.workspace;
                slzyzcFeatureClass = slzyzcResult.featureClass;

                if (slzyzcFeatureClass == null)
                {
                    throw new Exception($"无法打开{countyName}的SLZYZC.shp");
                }

                progressCallback?.Invoke(35, $"正在获取{countyName}的CZKFBJ数据...");

                // 获取CZKFBJ数据
                if (!string.IsNullOrEmpty(czkfbjShapefilePath))
                {
                    System.Diagnostics.Debug.WriteLine($"尝试从指定路径加载CZKFBJ: {czkfbjShapefilePath}");
                    var czkfbjResult = OpenShapefileFeatureClass(czkfbjShapefilePath);
                    czkfbjWorkspace = czkfbjResult.workspace;
                    czkfbjFeatureClass = czkfbjResult.featureClass;
                }
                else
                {
                    // 自动查找CZKFBJ文件
                    string autoDetectedPath = GetCZKFBJShapefilePath(countyName);
                    if (!string.IsNullOrEmpty(autoDetectedPath))
                    {
                        System.Diagnostics.Debug.WriteLine($"自动找到{countyName}的CZKFBJ shapefile: {autoDetectedPath}");
                        var czkfbjResult = OpenShapefileFeatureClass(autoDetectedPath);
                        czkfbjWorkspace = czkfbjResult.workspace;
                        czkfbjFeatureClass = czkfbjResult.featureClass;
                    }
                }

                if (czkfbjFeatureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 无法获取{countyName}的CZKFBJ数据，所有CZKFBJMJ将为0");
                }
                else
                {
                    int czkfbjCount = czkfbjFeatureClass.FeatureCount(null);
                    System.Diagnostics.Debug.WriteLine($"成功加载{countyName}的CZKFBJ数据，包含{czkfbjCount}个边界要素");
                }

                progressCallback?.Invoke(45, $"开始清空{countyName}的SLZYZC.shp现有数据...");

                // 清空SLZYZC现有数据
                ClearShapefileData(slzyzcFeatureClass);

                progressCallback?.Invoke(55, $"开始转换{countyName}的数据...");

                // 执行数据转换
                int convertedCount = ConvertAndUpdateFeatures(
                    lcxzgxFeatureClass,
                    slzyzcFeatureClass,
                    czkfbjFeatureClass,
                    fieldMappings,
                    countyName,
                    progressCallback);

                progressCallback?.Invoke(100, $"成功转换 {convertedCount} 个要素到{countyName}的SLZYZC.shp");

                System.Diagnostics.Debug.WriteLine($"县{countyName}的LCXZGX.shp已成功转换更新到SLZYZC.shp");
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
                // 释放COM对象
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
                if (lcxzgxWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lcxzgxWorkspace);
                }
                if (slzyzcWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(slzyzcWorkspace);
                }
                if (czkfbjWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjWorkspace);
                }
            }
        }

        /// <summary>
        /// 批量转换多个县的LCXZGX.shp为SLZYZC.shp
        /// </summary>
        /// <param name="countyShapefilePaths">县级shapefile路径映射（县名 -> shapefile目录路径）</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="progressCallback">进度回调</param>
        /// <param name="czkfbjShapefilePath">CZKFBJ shapefile路径（可选）</param>
        /// <returns>批量转换结果（县名 -> 是否成功）</returns>
        public Dictionary<string, bool> BatchConvertLCXZGXToSLZYZC(
            Dictionary<string, string> countyShapefilePaths,
            Dictionary<string, string> fieldMappings = null,
            ProgressCallback progressCallback = null,
            string czkfbjShapefilePath = null)
        {
            if (countyShapefilePaths == null || countyShapefilePaths.Count == 0)
            {
                throw new ArgumentException("县级shapefile路径映射不能为空");
            }

            var results = new Dictionary<string, bool>();
            int totalCounties = countyShapefilePaths.Count;
            int processedCounties = 0;

            progressCallback?.Invoke(0, $"开始批量转换{totalCounties}个县的数据...");

            foreach (var countyData in countyShapefilePaths)
            {
                string countyName = countyData.Key;
                string shapefilePath = countyData.Value;

                try
                {
                    int overallProgress = (processedCounties * 100) / totalCounties;
                    progressCallback?.Invoke(overallProgress, $"正在转换县: {countyName} ({processedCounties + 1}/{totalCounties})");

                    bool success = ConvertLCXZGXToSLZYZC(countyName, shapefilePath, fieldMappings, null, czkfbjShapefilePath);
                    results[countyName] = success;

                    processedCounties++;
                    System.Diagnostics.Debug.WriteLine($"县{countyName}数据转换完成 ({processedCounties}/{totalCounties})");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"转换县{countyName}数据时出错: {ex.Message}");
                    results[countyName] = false;
                    processedCounties++;
                }
            }

            progressCallback?.Invoke(100, $"所有县的数据转换完成 ({processedCounties}/{totalCounties})");
            return results;
        }

        /// <summary>
        /// Shapefile打开结果结构
        /// </summary>
        private struct ShapefileOpenResult
        {
            public IWorkspace workspace;
            public IFeatureClass featureClass;

            public ShapefileOpenResult(IWorkspace workspace, IFeatureClass featureClass)
            {
                this.workspace = workspace;
                this.featureClass = featureClass;
            }
        }

        /// <summary>
        /// 打开shapefile要素类
        /// </summary>
        /// <param name="shapefilePath">shapefile完整路径（包括.shp文件名）</param>
        /// <returns>包含工作空间和要素类的结果</returns>
        private ShapefileOpenResult OpenShapefileFeatureClass(string shapefilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(shapefilePath) || !File.Exists(shapefilePath))
                {
                    System.Diagnostics.Debug.WriteLine($"Shapefile路径无效或文件不存在: {shapefilePath}");
                    return new ShapefileOpenResult(null, null);
                }

                string shapefileDirectory = System.IO.Path.GetDirectoryName(shapefilePath);
                string shapefileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                System.Diagnostics.Debug.WriteLine($"正在打开shapefile: 目录={shapefileDirectory}, 文件名={shapefileName}");

                // 创建shapefile工作空间工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // 打开shapefile工作空间
                IWorkspace shapefileWorkspace = workspaceFactory.OpenFromFile(shapefileDirectory, 0);

                if (shapefileWorkspace == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法打开shapefile工作空间: {shapefileDirectory}");
                    return new ShapefileOpenResult(null, null);
                }

                // 获取要素工作空间
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)shapefileWorkspace;

                // 打开shapefile要素类
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(shapefileName);

                if (featureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法打开shapefile要素类: {shapefileName}");
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(shapefileWorkspace);
                    return new ShapefileOpenResult(null, null);
                }

                int featureCount = featureClass.FeatureCount(null);
                System.Diagnostics.Debug.WriteLine($"成功打开shapefile {shapefileName}，包含 {featureCount} 个要素");

                return new ShapefileOpenResult(shapefileWorkspace, featureClass);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"打开shapefile时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"错误堆栈: {ex.StackTrace}");
                return new ShapefileOpenResult(null, null);
            }
        }

        /// <summary>
        /// 从SharedDataManager获取CZKFBJ shapefile路径
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <returns>对应县的CZKFBJ shapefile路径，如果没有找到则返回null</returns>
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
                        System.Diagnostics.Debug.WriteLine($"找到{countyName}的CZKFBJ文件: {fileInfo.FullPath}");
                        return fileInfo.FullPath;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"未找到{countyName}的CZKFBJ文件");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取{countyName}的CZKFBJ shapefile路径时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 清空shapefile的现有数据
        /// </summary>
        /// <param name="featureClass">要清空的要素类</param>
        private void ClearShapefileData(IFeatureClass featureClass)
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

                System.Diagnostics.Debug.WriteLine("成功清空shapefile现有数据");
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
        /// 转换并更新要素数据
        /// </summary>
        /// <param name="sourceFeatureClass">源要素类（LCXZGX）</param>
        /// <param name="targetFeatureClass">目标要素类（SLZYZC）</param>
        /// <param name="czkfbjFeatureClass">城镇开发边界要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>转换的要素数量</returns>
        private int ConvertAndUpdateFeatures(
            IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass,
            IFeatureClass czkfbjFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            ProgressCallback progressCallback)
        {
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);
            IFeatureCursor sourceCursor = null;

            try
            {
                int totalFeatures = sourceFeatureClass.FeatureCount(null);
                int processedCount = 0;
                int successCount = 0;

                System.Diagnostics.Debug.WriteLine($"开始转换{countyName}的数据：{totalFeatures}个要素从LCXZGX.shp到SLZYZC.shp");

                // 获取字段映射
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    fieldMappings = Convert2ResultTable.CreateXZ2SLZYZCFieldsMap();
                }

                // 🔥 修改: 显式获取并检查 ZCQCBSM 和 CZKFBJMJ 字段索引
                int zcqcbsmIndex = targetFeatureClass.FindField("ZCQCBSM");
                if (zcqcbsmIndex == -1)
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 目标表 SLZYZC.shp 中未找到 ZCQCBSM 字段，将跳过该字段的计算。");
                }
                int czkfbjmjIndex = targetFeatureClass.FindField("CZKFBJMJ");
                if (czkfbjmjIndex == -1)
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 目标表中未找到CZKFBJMJ字段");
                }

                // 创建源要素游标
                sourceCursor = sourceFeatureClass.Search(null, false);
                IFeature sourceFeature;

                // 逐个处理要素
                while ((sourceFeature = sourceCursor.NextFeature()) != null)
                {
                    try
                    {
                        // 复制几何对象
                        if (sourceFeature.Shape != null)
                        {
                            featureBuffer.Shape = sourceFeature.ShapeCopy;
                        }

                        // 1. 复制通用属性并执行字段映射
                        CopyAndConvertFeatureAttributes(
                            sourceFeature,
                            sourceFeatureClass,
                            featureBuffer,
                            targetFeatureClass,
                            fieldMappings,
                            countyName,
                            successCount + 1); // 传递图斑序号

                        // 🔥 修改: 2. 独立处理 ZCQCBSM 字段，不再依赖于字段映射
                        if (zcqcbsmIndex != -1)
                        {
                            object zcqcbsmValue = ProcessSpecialFieldMapping(
                                sourceFeature,
                                sourceFeatureClass,
                                "ZCQCBSM", // 目标字段名
                                "",        // 源字段名（此处无用）
                                countyName,
                                successCount + 1); // 传递图斑序号

                            if (zcqcbsmValue != null)
                            {
                                featureBuffer.set_Value(zcqcbsmIndex, zcqcbsmValue);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"警告：为要素 {sourceFeature.OID} 计算 ZCQCBSM 失败。");
                            }
                        }

                        // 3. 处理CZKFBJMJ字段
                        if (czkfbjmjIndex != -1)
                        {
                            double intersectionArea = 0;
                            if (czkfbjFeatureClass != null && sourceFeature.Shape != null)
                            {
                                intersectionArea = CalculateIntersectionArea(sourceFeature.Shape, czkfbjFeatureClass);
                            }
                            featureBuffer.set_Value(czkfbjmjIndex, intersectionArea);
                        }

                        // 插入要素
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;
                        processedCount++;

                        // 更新进度
                        if (processedCount % 50 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = 55 + (int)((processedCount / (double)totalFeatures) * 40);
                            progressCallback?.Invoke(percentage,
                                $"正在转换{countyName}的数据... ({processedCount}/{totalFeatures})");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"转换{countyName}要素 {sourceFeature?.OID} 时出错: {ex.Message}");
                    }
                    finally
                    {
                        if (sourceFeature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceFeature);
                        }
                    }
                }

                // 提交所有插入操作
                insertCursor.Flush();

                System.Diagnostics.Debug.WriteLine($"{countyName}数据转换完成: 成功{successCount}个");
                return successCount;
            }
            finally
            {
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
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFeatureBuffer">目标要素缓冲区</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        /// <param name="featureSequence">当前图斑的序号</param>
        private void CopyAndConvertFeatureAttributes(
            IFeature sourceFeature,
            IFeatureClass sourceFeatureClass,
            IFeatureBuffer targetFeatureBuffer,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            int featureSequence)
        {
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;
                string sourceFieldName = mapping.Value;

                try
                {
                    int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);
                    if (targetFieldIndex == -1)
                    {
                        continue;
                    }

                    object targetValue = ProcessSpecialFieldMapping(
                        sourceFeature,
                        sourceFeatureClass,
                        targetFieldName,
                        sourceFieldName,
                        countyName,
                        featureSequence); // 传递序号

                    if (targetValue != null)
                    {
                        targetFeatureBuffer.set_Value(targetFieldIndex, targetValue);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"转换{countyName}字段{targetFieldName}时出错: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 处理特殊的字段映射规则
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFieldName">目标字段名</param>
        /// <param name="sourceFieldName">源字段名</param>
        /// <param name="countyName">县名</param>
        /// <param name="featureSequence">当前图斑的序号</param>
        /// <returns>转换后的字段值</returns>
        private object ProcessSpecialFieldMapping(
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
                    case "ZCQCBSM":
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
                        return 0;

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

                    case "CZKFBJMJ":
                        // 这个字段在主处理循环中单独处理
                        return 0;

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
                System.Diagnostics.Debug.WriteLine($"处理特殊字段映射 {targetFieldName} 时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 计算几何对象与城镇开发边界的交集面积
        /// </summary>
        /// <param name="geometry">要计算的几何对象</param>
        /// <param name="czkfbjFeatureClass">城镇开发边界要素类</param>
        /// <returns>交集面积（平方米）</returns>
        private double CalculateIntersectionArea(IGeometry geometry, IFeatureClass czkfbjFeatureClass)
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

                // 获取空间参考
                ISpatialReference sourceSR = geometry.SpatialReference;
                ISpatialReference targetSR = ((IGeoDataset)czkfbjFeatureClass).SpatialReference;

                // 投影变换
                IGeometry queryGeometry = geometry;
                if (sourceSR != null && targetSR != null && !sourceSR.Equals(targetSR))
                {
                    queryGeometry = ((IClone)geometry).Clone() as IGeometry;
                    queryGeometry.Project(targetSR);
                }

                // 创建空间查询过滤器
                spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = queryGeometry;
                spatialFilter.GeometryField = czkfbjFeatureClass.ShapeFieldName;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                // 查询相交的要素
                czkfbjCursor = czkfbjFeatureClass.Search(spatialFilter, false);
                IFeature czkfbjFeature;

                while ((czkfbjFeature = czkfbjCursor.NextFeature()) != null)
                {
                    try
                    {
                        if (czkfbjFeature.Shape != null && !czkfbjFeature.Shape.IsEmpty)
                        {
                            IGeometry czkfbjGeometry = czkfbjFeature.Shape;

                            // 计算交集
                            ITopologicalOperator topoOperator = (ITopologicalOperator)queryGeometry;
                            IGeometry intersectionGeometry = topoOperator.Intersect(czkfbjGeometry, esriGeometryDimension.esriGeometry2Dimension);

                            if (intersectionGeometry != null && !intersectionGeometry.IsEmpty)
                            {
                                IArea area = (IArea)intersectionGeometry;
                                double currentArea = Math.Abs(area.Area);
                                totalIntersectionArea += currentArea;

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

                // 释放临时几何对象
                if (queryGeometry != geometry)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(queryGeometry);
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

        /// <summary>
        /// 确保县名以"县"字结尾
        /// </summary>
        /// <param name="countyName">原始县名</param>
        /// <returns>确保带有"县"字的县名</returns>
        /// <summary>
        /// 确保县名以"县"字结尾，并移除名称中的非中文字符
        /// </summary>
        /// <param name="countyName">原始县名</param>
        /// <returns>处理后确保带有"县"字的县名</returns>
        private string EnsureCountySuffix(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                return string.Empty;
            }

            // 使用正则表达式仅保留中文字符
            string chineseOnlyCountyName = Regex.Replace(countyName, @"[^\u4e00-\u9fa5]", "");

            // 如果过滤后名称为空（例如，名称只包含数字），则回退到使用原始名称
            if (string.IsNullOrEmpty(chineseOnlyCountyName))
            {
                chineseOnlyCountyName = countyName;
            }

            return chineseOnlyCountyName;
        }

        /// <summary>
        /// 合并多个字段的值
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="fieldNames">要合并的字段名数组</param>
        /// <returns>合并后的字符串</returns>
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

        /// <summary>
        /// 计算两个字段的乘积
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="field1Name">第一个字段名</param>
        /// <param name="field2Name">第二个字段名</param>
        /// <returns>计算结果</returns>
        private double? CalculateFieldProduct(IFeature sourceFeature, IFeatureClass sourceFeatureClass,
            string field1Name, string field2Name)
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

        /// <summary>
        /// 根据索引获取字段名
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <param name="index">字段索引（从1开始）</param>
        /// <returns>字段名</returns>
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

        /// <summary>
        /// 转换字段值以适配SLZYZC表
        /// </summary>
        /// <param name="sourceValue">源字段值</param>
        /// <param name="targetFieldName">目标字段名</param>
        /// <param name="sourceFieldName">源字段名</param>
        /// <param name="countyName">县名</param>
        /// <returns>转换后的字段值</returns>
        private object ConvertFieldValueForSLZYZC(
            object sourceValue,
            string targetFieldName,
            string sourceFieldName,
            string countyName)
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