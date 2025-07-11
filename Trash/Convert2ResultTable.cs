using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.IO;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 结果表转换工具类 - 将县级LCXZGX表转换为SLZYZC表
    /// SLZYZC表是森林资源调查成果表，用于存储转换后的林地调查数据
    /// </summary>
    public class Convert2ResultTable
    {
        /// <summary>
        /// 进度回调委托 - 用于向UI层报告处理进度
        /// </summary>
        /// <param name="percentage">完成百分比 (0-100)</param>
        /// <param name="message">当前操作描述信息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 将县级LCXZGX表转换为SLZYZC表
        /// 支持从外部shapefile读取CZKFBJ数据
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <param name="databasePath">县级数据库路径</param>
        /// <param name="czkfbjShapefilePath">CZKFBJ shapefile路径（可选）</param>
        /// <param name="fieldMappings">字段映射配置（SLZYZC字段名 -> LCXZGX字段名）</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>转换是否成功</returns>
        public bool ConvertLCXZGXToSLZYZC(
            string countyName,
            string databasePath,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null,
            string czkfbjShapefilePath = null)
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
            IFeatureClass lcxzgxFeatureClass = null;
            IFeatureClass slzyzcFeatureClass = null;
            IFeatureClass czkfbjFeatureClass = null;
            IWorkspace shapefileWorkspace = null;

            try
            {
                // 打开县级数据库
                workspace = OpenCountyDatabase(databasePath, countyName);
                if (workspace == null)
                {
                    throw new Exception($"无法打开{countyName}的数据库");
                }

                progressCallback?.Invoke(15, $"正在访问{countyName}的LCXZGX表...");

                // 获取源表 - LCXZGX要素类
                lcxzgxFeatureClass = GetFeatureClass(workspace, "LCXZGX");
                if (lcxzgxFeatureClass == null)
                {
                    throw new Exception($"无法找到{countyName}数据库中的LCXZGX表");
                }

                progressCallback?.Invoke(25, $"正在访问{countyName}的SLZYZC表...");

                // 获取目标表 - SLZYZC要素类
                slzyzcFeatureClass = GetFeatureClass(workspace, "SLZYZC");
                if (slzyzcFeatureClass == null)
                {
                    throw new Exception($"无法找到{countyName}数据库中的SLZYZC表");
                }

                progressCallback?.Invoke(30, $"正在访问{countyName}的城镇开发边界数据...");

                // 获取城镇开发边界要素类 - 优先从外部shapefile读取
                progressCallback?.Invoke(30, $"正在访问{countyName}的城镇开发边界数据...");

                // 获取城镇开发边界要素类 - 优先从外部shapefile读取
                if (!string.IsNullOrEmpty(czkfbjShapefilePath))
                {
                    System.Diagnostics.Debug.WriteLine($"尝试从shapefile加载CZKFBJ: {czkfbjShapefilePath}");
                    ShapefileOpenResult result = OpenShapefileFeatureClass(czkfbjShapefilePath);
                    shapefileWorkspace = result.workspace;
                    czkfbjFeatureClass = result.featureClass;
                }
                else
                {
                    // 优先尝试从SharedDataManager获取对应县的CZKFBJ shapefile路径
                    string autoDetectedPath = GetCZKFBJShapefilePath(countyName);

                    if (!string.IsNullOrEmpty(autoDetectedPath))
                    {
                        System.Diagnostics.Debug.WriteLine($"自动找到{countyName}的CZKFBJ shapefile: {autoDetectedPath}");
                        ShapefileOpenResult result = OpenShapefileFeatureClass(autoDetectedPath);
                        shapefileWorkspace = result.workspace;
                        czkfbjFeatureClass = result.featureClass;
                    }
                    else
                    {
                        // 如果SharedDataManager中没有找到，尝试从GDB中获取
                        System.Diagnostics.Debug.WriteLine($"未找到{countyName}的CZKFBJ shapefile，尝试从GDB获取");
                        try
                        {
                            czkfbjFeatureClass = GetCZKFBJFeatureClass(workspace, countyName);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"从GDB获取{countyName}的CZKFBJ失败: {ex.Message}");
                        }
                    }
                }

                if (czkfbjFeatureClass == null)
                {
                    System.Diagnostics.Debug.WriteLine($"严重警告: 无法获取{countyName}的CZKFBJ数据，所有CZKFBJMJ将为0");
                }
                else
                {
                    int czkfbjCount = czkfbjFeatureClass.FeatureCount(null);
                    System.Diagnostics.Debug.WriteLine($"成功加载{countyName}的CZKFBJ数据，包含{czkfbjCount}个边界要素");
                }

                progressCallback?.Invoke(35, $"开始转换{countyName}的数据...");

                // 执行数据转换操作
                int convertedCount = ConvertFeatures(
                    lcxzgxFeatureClass,
                    slzyzcFeatureClass,
                    czkfbjFeatureClass,
                    fieldMappings,
                    countyName,
                    progressCallback);

                progressCallback?.Invoke(100, $"成功转换 {convertedCount} 个要素到{countyName}的SLZYZC表");

                System.Diagnostics.Debug.WriteLine($"县{countyName}的LCXZGX数据已成功转换为SLZYZC表");
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
                if (shapefileWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(shapefileWorkspace);
                }
                if (workspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
                }
            }
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

                // 获取shapefile所在目录和文件名
                string shapefileDirectory = System.IO.Path.GetDirectoryName(shapefilePath);
                string shapefileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                System.Diagnostics.Debug.WriteLine($"正在打开shapefile: 目录={shapefileDirectory}, 文件名={shapefileName}");

                // 创建shapefile工作空间工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                // 打开shapefile工作空间（目录）
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
        public static string GetCZKFBJShapefilePath(string countyName)
        {
            try
            {
                var czkfbjFiles = SharedDataManager.GetCZKFBJFiles();

                // 查找匹配的县名
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
        /// 批量转换多个县的LCXZGX表为SLZYZC表
        /// 提供批处理功能，提高多县数据处理的效率
        /// </summary>
        /// <param name="countyDatabasePaths">县级数据库路径映射（县名 -> 数据库路径）</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="progressCallback">进度回调</param>
        /// <param name="czkfbjShapefilePath">CZKFBJ shapefile路径（可选）</param>
        /// <returns>批量转换结果（县名 -> 是否成功）</returns>
        public Dictionary<string, bool> BatchConvertLCXZGXToSLZYZC(
            Dictionary<string, string> countyDatabasePaths,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback = null,
            string czkfbjShapefilePath = null)
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

                    // 为每个县执行转换，传递CZKFBJ shapefile路径
                    bool success = ConvertLCXZGXToSLZYZC(countyName, databasePath, fieldMappings, null, czkfbjShapefilePath);
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
        /// 获取指定名称的要素类
        /// 通用方法，可用于获取LCXZGX、SLZYZC等要素类
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
        /// 从LCXZGX表读取数据，转换后写入SLZYZC表
        /// </summary>
        /// <param name="sourceFeatureClass">源要素类（LCXZGX）</param>
        /// <param name="targetFeatureClass">目标要素类（SLZYZC）</param>
        /// <param name="czkfbjFeatureClass">城镇开发边界要素类</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="countyName">县名</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>转换的要素数量</returns>
        private int ConvertFeatures(
            IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass,
            IFeatureClass czkfbjFeatureClass,
            Dictionary<string, string> fieldMappings,
            string countyName,
            ProgressCallback progressCallback)
        {
            // 创建要素缓冲区和插入游标
            // 使用批量插入模式(true参数)提高插入性能
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

                System.Diagnostics.Debug.WriteLine($"开始转换{countyName}的数据：{totalFeatures}个要素从LCXZGX到SLZYZC");

                // 创建查询游标读取所有源要素
                sourceCursor = sourceFeatureClass.Search(null, false);
                IFeature sourceFeature;

                // 如果没有提供自定义映射，使用默认的字段映射配置
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    fieldMappings = CreateXZ2SLZYZCFieldsMap();
                }

                // 获取CZKFBJMJ字段的索引
                int czkfbjmjIndex = targetFeatureClass.FindField("CZKFBJMJ");
                if (czkfbjmjIndex == -1)
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 目标表中未找到CZKFBJMJ字段");
                }

                // 获取权属性质字段的索引
                int qsxzIndex = sourceFeatureClass.FindField("qsxz");
                if (qsxzIndex == -1)
                {
                    // 尝试备用字段名
                    qsxzIndex = sourceFeatureClass.FindField("QSXZ");
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

                        // 处理CZKFBJMJ字段 - 计算集体土地与城镇开发边界的交集面积
                        if (czkfbjmjIndex != -1)
                        {
                            double intersectionArea = 0;

                            // 🔥 修改：移除权属性质判断，为所有图斑计算与城镇开发边界的交集面积
                            // 无论是国有土地还是集体土地，都需要计算与城镇开发边界的交集面积
                            if (czkfbjFeatureClass != null && sourceFeature.Shape != null)
                            {
                                intersectionArea = CalculateIntersectionArea(sourceFeature.Shape, czkfbjFeatureClass);

                                // 输出调试信息，记录每个图斑的交集面积计算结果
                                if (intersectionArea > 0)
                                {
                                    System.Diagnostics.Debug.WriteLine($"图斑与城镇开发边界交集面积: {intersectionArea:F2} 平方米");
                                }
                            }

                            // 设置CZKFBJMJ字段值（所有图斑都设置，包括国有和集体土地）
                            featureBuffer.set_Value(czkfbjmjIndex, intersectionArea);
                        }

                        // 执行要素插入操作
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        processedCount++;

                        // 定期更新进度 - 每50个要素或最后一个要素时报告进度
                        // 进度范围：35%-95%，为最终操作保留5%
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
                        // 这样可以最大化数据的成功转换率
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
                // Flush确保所有缓存的操作都被写入磁盘
                insertCursor.Flush();

                System.Diagnostics.Debug.WriteLine($"{countyName}数据转换完成: 成功{successCount}个, 失败{errorCount}个");
                return successCount;
            }
            finally
            {
                // 重要：释放ArcGIS COM对象
                // 游标和缓冲区都是COM对象，必须显式释放
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
        /// 计算几何对象与城镇开发边界的交集面积
        /// 适用于所有权属性质的土地（国有、集体等）
        /// </summary>
        /// <param name="geometry">要计算的几何对象</param>
        /// <param name="czkfbjFeatureClass">城镇开发边界要素类</param>
        /// <returns>交集面积（平方米）</returns>
        private double CalculateIntersectionArea(IGeometry geometry, IFeatureClass czkfbjFeatureClass)
        {
            if (geometry == null || czkfbjFeatureClass == null)
            {
                System.Diagnostics.Debug.WriteLine("计算交集面积失败: 几何对象或CZKFBJ要素类为空");
                return 0;
            }

            double totalIntersectionArea = 0;
            IFeatureCursor czkfbjCursor = null;
            ISpatialFilter spatialFilter = null;

            try
            {
                // 验证几何对象的有效性
                if (geometry.IsEmpty)
                {
                    System.Diagnostics.Debug.WriteLine("源几何对象为空，跳过交集计算");
                    return 0;
                }

                // 检查CZKFBJ要素类中是否有数据
                int czkfbjCount = czkfbjFeatureClass.FeatureCount(null);
                System.Diagnostics.Debug.WriteLine($"CZKFBJ要素类包含{czkfbjCount}个要素");

                if (czkfbjCount == 0)
                {
                    System.Diagnostics.Debug.WriteLine("CZKFBJ要素类为空，无法计算交集");
                    return 0;
                }

                // 获取空间参考信息进行调试
                ISpatialReference sourceSR = geometry.SpatialReference;
                ISpatialReference targetSR = ((IGeoDataset)czkfbjFeatureClass).SpatialReference;

                System.Diagnostics.Debug.WriteLine($"源几何空间参考: {sourceSR?.Name ?? "未定义"}");
                System.Diagnostics.Debug.WriteLine($"CZKFBJ空间参考: {targetSR?.Name ?? "未定义"}");

                // 如果空间参考不同，进行投影变换
                IGeometry queryGeometry = geometry;
                if (sourceSR != null && targetSR != null && !sourceSR.Equals(targetSR))
                {
                    System.Diagnostics.Debug.WriteLine("检测到空间参考不匹配，进行投影变换...");
                    queryGeometry = ((IClone)geometry).Clone() as IGeometry;
                    queryGeometry.Project(targetSR);
                }

                // 创建空间查询过滤器
                spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = queryGeometry;
                spatialFilter.GeometryField = czkfbjFeatureClass.ShapeFieldName;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                System.Diagnostics.Debug.WriteLine($"开始空间查询，使用几何字段: {spatialFilter.GeometryField}");

                // 查询相交的城镇开发边界要素
                czkfbjCursor = czkfbjFeatureClass.Search(spatialFilter, false);
                IFeature czkfbjFeature;
                int intersectCount = 0;

                // 累计所有相交区域的面积
                while ((czkfbjFeature = czkfbjCursor.NextFeature()) != null)
                {
                    try
                    {
                        intersectCount++;
                        System.Diagnostics.Debug.WriteLine($"处理第{intersectCount}个相交的CZKFBJ要素");

                        if (czkfbjFeature.Shape != null && !czkfbjFeature.Shape.IsEmpty)
                        {
                            // 确保使用相同的空间参考系统进行计算
                            IGeometry czkfbjGeometry = czkfbjFeature.Shape;
                            IGeometry calculationGeometry = queryGeometry;

                            // 计算交集区域
                            ITopologicalOperator topoOperator = (ITopologicalOperator)calculationGeometry;
                            IGeometry intersectionGeometry = topoOperator.Intersect(czkfbjGeometry, esriGeometryDimension.esriGeometry2Dimension);

                            // 计算交集面积
                            if (intersectionGeometry != null && !intersectionGeometry.IsEmpty)
                            {
                                IArea area = (IArea)intersectionGeometry;
                                double currentArea = Math.Abs(area.Area); // 使用绝对值避免负面积
                                totalIntersectionArea += currentArea;

                                System.Diagnostics.Debug.WriteLine($"第{intersectCount}个相交要素面积: {currentArea:F2} 平方米");
                                System.Diagnostics.Debug.WriteLine($"累计交集面积: {totalIntersectionArea:F2} 平方米");

                                // 释放交集几何对象
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(intersectionGeometry);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"第{intersectCount}个要素：交集几何为空");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"第{intersectCount}个CZKFBJ要素的几何为空");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"处理第{intersectCount}个CZKFBJ要素时出错: {ex.Message}");
                    }
                    finally
                    {
                        // 释放当前城镇开发边界要素
                        if (czkfbjFeature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(czkfbjFeature);
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"空间查询完成，找到{intersectCount}个相交要素，总交集面积: {totalIntersectionArea:F2} 平方米");

                // 如果进行了投影变换，释放临时几何对象
                if (queryGeometry != geometry)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(queryGeometry);
                }

                return totalIntersectionArea;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"计算交集面积时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"错误堆栈: {ex.StackTrace}");
                return 0;
            }
            finally
            {
                // 释放游标和过滤器对象
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
        /// 改进的CZKFBJ要素类获取方法，增强错误诊断
        /// </summary>
        /// <param name="workspace">工作空间</param>
        /// <param name="countyName">县名</param>
        /// <returns>CZKFBJ要素类或null</returns>
        private IFeatureClass GetCZKFBJFeatureClass(IWorkspace workspace, string countyName)
        {
            try
            {
                // 首先尝试标准名称
                string[] possibleNames = { "CZKFBJ", "czkfbj", "城镇开发边界", "CZKFBJ_POLYGON" };

                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

                foreach (string name in possibleNames)
                {
                    try
                    {
                        IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(name);
                        if (featureClass != null)
                        {
                            int featureCount = featureClass.FeatureCount(null);
                            System.Diagnostics.Debug.WriteLine($"成功获取{countyName}的{name}要素类，包含{featureCount}个要素");
                            return featureClass;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"尝试打开{name}失败: {ex.Message}");
                    }
                }

                // 如果标准名称都失败，列出所有要素类进行诊断
                System.Diagnostics.Debug.WriteLine($"开始列出{countyName}数据库中的所有要素类:");
                IEnumDataset enumDataset = workspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                enumDataset.Reset();
                IDataset dataset;
                while ((dataset = enumDataset.Next()) != null)
                {
                    System.Diagnostics.Debug.WriteLine($"  发现要素类: {dataset.Name}");
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataset);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(enumDataset);

                System.Diagnostics.Debug.WriteLine($"警告: 在{countyName}数据库中未找到CZKFBJ相关要素类");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取{countyName}的CZKFBJ要素类时发生严重错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 复制要素属性并进行字段映射转换
        /// 处理LCXZGX表到SLZYZC表的字段映射和数据转换
        /// </summary>
        /// <param name="sourceFeature">源要素（LCXZGX）</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetFeatureBuffer">目标要素缓冲区（SLZYZC）</param>
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
                string targetFieldName = mapping.Key;    // SLZYZC表字段名
                string sourceFieldName = mapping.Value;  // LCXZGX表字段名

                try
                {
                    // 获取目标字段的索引
                    int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);
                    if (targetFieldIndex == -1)
                    {
                        System.Diagnostics.Debug.WriteLine($"目标字段 {targetFieldName} 在SLZYZC表中不存在");
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
                    case "PCTBBM":
                        // 字段合并：xian + lin_ban + xiao_ban
                        return CombineFields(sourceFeature, sourceFeatureClass,
                            new[] { "xian", "lin_ban", "xiao_ban" });

                    case "ZTBXJ":
                        // 字段计算：xbmj * 第65个字段
                        return CalculateFieldProduct(sourceFeature, sourceFeatureClass,
                            "xbmj", GetFieldByIndex(sourceFeatureClass, 65));

                    case "XZQMC":
                        // 直接使用县名参数，确保包含"县"字
                        return EnsureCountySuffix(countyName);

                    case "CZKFBJMJ":
                        // 这个字段在ConvertFeatures方法中通过特殊计算处理
                        return 1; // 默认值，实际值在主处理循环中设置

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
                    System.Diagnostics.Debug.WriteLine($"字段 {fieldName} 在源表中不存在");
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
                    System.Diagnostics.Debug.WriteLine($"计算字段乘积时，字段不存在: {field1Name} 或 {field2Name}");
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
                    return fields.get_Field(index - 1).Name; // 转换为0基索引
                }

                System.Diagnostics.Debug.WriteLine($"字段索引 {index} 超出范围");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"根据索引获取字段名时出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 创建LCXZGX到SLZYZC的字段映射配置
        /// 基于您提供的字段映射模板
        /// </summary>
        /// <returns>字段映射字典（SLZYZC字段名 -> LCXZGX字段名）</returns>
        public static Dictionary<string, string> CreateXZ2SLZYZCFieldsMap()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // 基础字段映射
            result.Add("YSDM", "ysdm");            // 要素代码
            result.Add("XZQDM", "xian");           // 行政区代码
            result.Add("XZQMC", "SPECIAL_COUNTY");           // 行政区名称
            result.Add("GTDCTBBSM", "bsm");         // 国土调查图斑编码
            result.Add("GTDCTBBH", "tbbh");        // 国土调查图斑编号
            result.Add("GTDCDLBM", "dlbm");        // 国土调查地类编码
            result.Add("GTDCDLMC", "dlmc");        // 国土调查地类名称
            result.Add("QSDWDM", "qsdwdm");        // 权属单位代码
            result.Add("QSDWMC", "qsdwmc");        // 权属单位名称
            result.Add("ZLDWDM", "zldwdm");        // 坐落单位代码
            result.Add("ZLDWMC", "zldwmc");        // 坐落单位名称
            result.Add("GTDCTBMJ", "tbmj");        // 国土调查图斑面积
            result.Add("LYJ", "lin_ye_ju");        // 林业局
            result.Add("LC", "lin_chang");         // 林场
            result.Add("PCDL", "di_lei");          // 普查地类
            result.Add("ZTBMJ", "xbmj");           // 株数图斑面积
            result.Add("GTDCTDQS", "qsxz");        // 国土调查土地权属
            result.Add("LM_SUOYQ", "lmqs");        // 林木所有权
            result.Add("LZ", "lin_zhong");         // 林种
            result.Add("YSSZ", "you_shi_sz");      // 优势树种
            result.Add("QY", "qi_yuan");           // 起源
            result.Add("YBD", "yu_bi_du");         // 郁闭度
            result.Add("PJNL", "pingjun_nl");      // 平均年龄
            result.Add("LING_ZU", "ling_zu");      // 龄组
            result.Add("PJSG", "pingjun_sg");      // 平均树高
            result.Add("PJXJ", "pingjun_xj");      // 平均胸径
            result.Add("MGQZS", "mei_gq_zs");      // 每公顷株数
            result.Add("FRDBS", "frdbs");          // 发育地被层
            result.Add("CZKFBJMJ", "SPECIAL_CZKFBJ"); // 城镇开发边界面积 - 为所有图斑计算交集面积

            // 特殊字段映射 - 这些字段有特殊的处理逻辑
            result.Add("PCTBBM", "xian+lin_ban+xiao_ban");  // 字段合并
            result.Add("ZTBXJ", "xbmj*field65");             // 字段计算

            return result;
        }

        /// <summary>
        /// 转换字段值以适配SLZYZC表
        /// 根据SLZYZC表的业务规则和数据要求进行字段值转换
        /// </summary>
        /// <param name="sourceValue">源字段值</param>
        /// <param name="targetFieldName">目标字段名（SLZYZC表）</param>
        /// <param name="sourceFieldName">源字段名（LCXZGX表）</param>
        /// <param name="countyName">县名</param>
        /// <returns>转换后的字段值</returns>
        private object ConvertFieldValueForSLZYZC(
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
                // 根据SLZYZC表的字段要求进行特殊转换
                switch (targetFieldName.ToUpper())
                {
                    case "XZQDM":
                        // 行政区代码可能需要特殊处理
                        return sourceValue.ToString();

                    case "GTDCTBMJ":
                    case "ZTBMJ":
                        // 面积字段确保为数值类型
                        if (double.TryParse(sourceValue.ToString(), out double area))
                        {
                            return area;
                        }
                        return 0.0;

                    case "YBD":
                        // 郁闭度可能需要格式化
                        if (double.TryParse(sourceValue.ToString(), out double canopyClosure))
                        {
                            return Math.Round(canopyClosure, 2);
                        }
                        return sourceValue;

                    case "PJNL":
                        // 平均年龄确保为整数
                        if (int.TryParse(sourceValue.ToString(), out int age))
                        {
                            return age;
                        }
                        return sourceValue;

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
                    fieldMappings = CreateXZ2SLZYZCFieldsMap();
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