using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;

namespace TestArcMapAddin2.Utils
{
    /// <summary>
    /// GIS操作工具类
    /// 提供几何运算、空间分析、字段操作等GIS核心功能
    /// </summary>
    public static class GISOperationUtils
    {
        /// <summary>
        /// 进度回调委托
        /// </summary>
        /// <param name="percentage">完成百分比</param>
        /// <param name="message">进度消息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 裁剪操作结果
        /// </summary>
        public class ClipResult
        {
            public bool Success { get; set; }
            public string OutputPath { get; set; }
            public int ProcessedFeatureCount { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// Shapefile打开结果
        /// </summary>
        public class ShapefileOpenResult
        {
            public IWorkspace Workspace { get; set; }
            public IFeatureClass FeatureClass { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// 数据写入操作结果
        /// </summary>
        public class WriteDataResult
        {
            public bool Success { get; set; }
            public string OutputPath { get; set; }
            public int ProcessedFeatureCount { get; set; }
            public string ErrorMessage { get; set; }
        }

        /// <summary>
        /// 要素数据结构 - 用于传递几何和属性数据
        /// </summary>
        public class FeatureData
        {
            public IGeometry Geometry { get; set; }
            public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
        }

        /// <summary>
        /// 县级处理信息
        /// </summary>
        public class CountyProcessingInfo
        {
            public string CountyCode { get; set; }
            public string CountyName { get; set; }
            public IFeatureClass SourceFeatureClass { get; set; }
            public Dictionary<string, string> FieldMappings { get; set; }
            public string WhereClause { get; set; }
            public string ResourceType { get; set; } // "森林", "草原", "湿地", 等
        }

        /// <summary>
        /// 给要素类添加字段
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="length">字段长度</param>
        /// <param name="precision">精度</param>
        /// <returns>是否成功添加</returns>
        public static bool AddField(IFeatureClass featureClass, string fieldName, esriFieldType fieldType,
                                  int length = 50, int precision = 0)
        {
            try
            {
                // 检查字段是否已存在
                if (featureClass.FindField(fieldName) != -1)
                {
                    System.Diagnostics.Debug.WriteLine($"字段 {fieldName} 已存在");
                    return true;
                }

                // 创建字段定义
                IField field = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)field;

                fieldEdit.Name_2 = fieldName;
                fieldEdit.Type_2 = fieldType;

                if (fieldType == esriFieldType.esriFieldTypeString)
                {
                    fieldEdit.Length_2 = length;
                }
                else if (fieldType == esriFieldType.esriFieldTypeDouble && precision > 0)
                {
                    fieldEdit.Precision_2 = precision;
                    fieldEdit.Scale_2 = 2; // 小数位数
                }

                // 添加字段到要素类
                featureClass.AddField(field);

                System.Diagnostics.Debug.WriteLine($"成功添加字段: {fieldName}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"添加字段 {fieldName} 失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 计算要素的几何面积并更新到指定字段
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <param name="areaFieldName">面积字段名</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>计算成功的要素数量</returns>
        public static int CalculateGeometricArea(IFeatureClass featureClass, string areaFieldName,
                                               ProgressCallback progressCallback = null)
        {
            int successCount = 0;
            IFeatureCursor cursor = null;
            IFeatureCursor updateCursor = null;

            try
            {
                // 确保面积字段存在
                int areaFieldIndex = featureClass.FindField(areaFieldName);
                if (areaFieldIndex == -1)
                {
                    if (!AddField(featureClass, areaFieldName, esriFieldType.esriFieldTypeDouble))
                    {
                        throw new Exception($"无法创建面积字段: {areaFieldName}");
                    }
                    areaFieldIndex = featureClass.FindField(areaFieldName);
                }

                int totalFeatures = featureClass.FeatureCount(null);
                progressCallback?.Invoke(0, $"开始计算 {totalFeatures} 个要素的几何面积...");

                // 创建更新游标
                updateCursor = featureClass.Update(null, false);
                IFeature feature;
                int processedCount = 0;

                while ((feature = updateCursor.NextFeature()) != null)
                {
                    try
                    {
                        if (feature.Shape != null && !feature.Shape.IsEmpty)
                        {
                            // 计算几何面积
                            IArea areaInterface = feature.Shape as IArea;
                            if (areaInterface != null)
                            {
                                double area = Math.Abs(areaInterface.Area);
                                feature.set_Value(areaFieldIndex, area);
                                updateCursor.UpdateFeature(feature);
                                successCount++;
                            }
                        }

                        processedCount++;

                        // 更新进度
                        if (processedCount % 100 == 0 || processedCount == totalFeatures)
                        {
                            int percentage = (processedCount * 100) / totalFeatures;
                            progressCallback?.Invoke(percentage,
                                $"已计算 {processedCount}/{totalFeatures} 个要素的面积");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"计算要素面积时出错: {ex.Message}");
                    }
                    finally
                    {
                        if (feature != null)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"面积计算完成: 成功 {successCount}/{totalFeatures}");
                return successCount;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"计算几何面积时出错: {ex.Message}");
                throw;
            }
            finally
            {
                if (cursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                if (updateCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            }
        }

        /// <summary>
        /// 复制要素属性
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetBuffer">目标缓冲区</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        public static void CopyFeatureAttributes(IFeature sourceFeature, IFeatureClass sourceFeatureClass,
                                               IFeatureBuffer targetBuffer, IFeatureClass targetFeatureClass)
        {
            try
            {
                IFields sourceFields = sourceFeatureClass.Fields;
                IFields targetFields = targetFeatureClass.Fields;

                for (int i = 0; i < sourceFields.FieldCount; i++)
                {
                    IField sourceField = sourceFields.get_Field(i);

                    // 跳过OID和几何字段
                    if (sourceField.Type == esriFieldType.esriFieldTypeOID ||
                        sourceField.Type == esriFieldType.esriFieldTypeGeometry)
                        continue;

                    // 查找目标字段
                    int targetIndex = targetFeatureClass.FindField(sourceField.Name);
                    if (targetIndex != -1)
                    {
                        try
                        {
                            object value = sourceFeature.get_Value(i);
                            if (value != null && value != DBNull.Value)
                            {
                                targetBuffer.set_Value(targetIndex, value);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"复制字段 {sourceField.Name} 时出错: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"复制要素属性时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 复制要素属性（使用字段映射）
        /// </summary>
        /// <param name="sourceFeature">源要素</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetBuffer">目标缓冲区</param>
        /// <param name="targetFeatureClass">目标要素类</param>
        /// <param name="fieldMappings">字段映射（目标字段名 -> 源字段名）</param>
        public static void CopyFeatureAttributesWithMapping(IFeature sourceFeature, IFeatureClass sourceFeatureClass,
                                                           IFeatureBuffer targetBuffer, IFeatureClass targetFeatureClass,
                                                           Dictionary<string, string> fieldMappings)
        {
            try
            {
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    // 如果没有字段映射，使用默认的复制方法
                    CopyFeatureAttributes(sourceFeature, sourceFeatureClass, targetBuffer, targetFeatureClass);
                    return;
                }

                foreach (var mapping in fieldMappings)
                {
                    string targetFieldName = mapping.Key;
                    string sourceFieldName = mapping.Value;

                    try
                    {
                        int sourceIndex = sourceFeatureClass.FindField(sourceFieldName);
                        int targetIndex = targetFeatureClass.FindField(targetFieldName);

                        if (sourceIndex != -1 && targetIndex != -1)
                        {
                            object value = sourceFeature.get_Value(sourceIndex);
                            if (value != null && value != DBNull.Value)
                            {
                                targetBuffer.set_Value(targetIndex, value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"复制字段映射 {targetFieldName} <- {sourceFieldName} 时出错: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"使用字段映射复制要素属性时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 打开shapefile
        /// </summary>
        /// <param name="shapefilePath">shapefile路径</param>
        /// <returns>打开结果</returns>
        public static ShapefileOpenResult OpenShapefile(string shapefilePath)
        {
            var result = new ShapefileOpenResult();

            try
            {
                if (!File.Exists(shapefilePath))
                {
                    result.ErrorMessage = $"文件不存在: {shapefilePath}";
                    return result;
                }

                string directory = System.IO.Path.GetDirectoryName(shapefilePath);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                // 使用 Type.GetTypeFromProgID 和 Activator.CreateInstance 来创建工作空间工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory factory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;

                IWorkspace workspace = factory.OpenFromFile(directory, 0);
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);

                result.Workspace = workspace;
                result.FeatureClass = featureClass;
                result.Success = true;

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"打开shapefile {shapefilePath} 时出错: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// 🔥 重载：向现有Shapefile写入数据（保持原有接口兼容性）
        /// </summary>
        /// <param name="targetShapefilePath">目标Shapefile路径</param>
        /// <param name="sourceData">源数据列表（几何和属性）</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>写入操作结果</returns>
        public static WriteDataResult WriteDataToExistingShapefile(string targetShapefilePath,
            List<FeatureData> sourceData, ProgressCallback progressCallback = null)
        {
            // 默认清空现有数据
            return WriteDataToExistingShapefile(targetShapefilePath, sourceData, true, progressCallback);
        }

        /// <summary>
        /// 从源要素类读取数据并写入到目标Shapefile
        /// </summary>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="targetShapefilePath">目标Shapefile路径</param>
        /// <param name="fieldMappings">字段映射（目标字段名 -> 源字段名）</param>
        /// <param name="whereClause">查询条件（可选）</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>写入操作结果</returns>
        public static WriteDataResult WriteFeatureClassToShapefile(IFeatureClass sourceFeatureClass,
            string targetShapefilePath, Dictionary<string, string> fieldMappings = null,
            string whereClause = null, ProgressCallback progressCallback = null)
        {
            var result = new WriteDataResult();
            IFeatureCursor sourceCursor = null;
            var featureDataList = new List<FeatureData>();

            try
            {
                progressCallback?.Invoke(0, "正在读取源数据...");

                // 创建查询过滤器
                IQueryFilter queryFilter = null;
                if (!string.IsNullOrEmpty(whereClause))
                {
                    queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = whereClause;
                }

                // 读取源数据
                sourceCursor = sourceFeatureClass.Search(queryFilter, false);
                IFeature sourceFeature;
                int readCount = 0;

                while ((sourceFeature = sourceCursor.NextFeature()) != null)
                {
                    try
                    {
                        var featureData = new FeatureData();

                        // 复制几何
                        if (sourceFeature.Shape != null)
                        {
                            featureData.Geometry = sourceFeature.ShapeCopy;
                        }

                        // 复制属性
                        if (fieldMappings != null)
                        {
                            foreach (var mapping in fieldMappings)
                            {
                                string targetField = mapping.Key;
                                string sourceField = mapping.Value;

                                int sourceIndex = sourceFeatureClass.FindField(sourceField);
                                if (sourceIndex != -1)
                                {
                                    object value = sourceFeature.get_Value(sourceIndex);
                                    featureData.Attributes[targetField] = value;
                                }
                            }
                        }
                        else
                        {
                            // 如果没有字段映射，复制所有非几何字段
                            IFields fields = sourceFeatureClass.Fields;
                            for (int i = 0; i < fields.FieldCount; i++)
                            {
                                IField field = fields.get_Field(i);
                                if (field.Type != esriFieldType.esriFieldTypeOID &&
                                    field.Type != esriFieldType.esriFieldTypeGeometry)
                                {
                                    object value = sourceFeature.get_Value(i);
                                    featureData.Attributes[field.Name] = value;
                                }
                            }
                        }

                        featureDataList.Add(featureData);
                        readCount++;

                        if (readCount % 100 == 0)
                        {
                            progressCallback?.Invoke(readCount % 50, $"已读取 {readCount} 个要素...");
                        }
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceFeature);
                    }
                }

                progressCallback?.Invoke(50, $"数据读取完成，共读取 {readCount} 个要素");

                // 写入到目标Shapefile
                var writeResult = WriteDataToExistingShapefile(targetShapefilePath, featureDataList,
                    (percentage, message) => {
                        int adjustedProgress = 50 + (percentage * 50) / 100;
                        progressCallback?.Invoke(adjustedProgress, message);
                    });

                return writeResult;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"从要素类写入Shapefile时出错: {ex.Message}");
                return result;
            }
            finally
            {
                if (sourceCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sourceCursor);
            }
        }

        /// <summary>
        /// 批量处理多个县的数据写入
        /// </summary>
        /// <param name="countyDataMappings">县级数据映射（县代码 -> 数据信息）</param>
        /// <param name="outputBasePath">输出基础路径</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>批量处理结果</returns>
        public static Dictionary<string, WriteDataResult> BatchWriteCountyData(
            Dictionary<string, CountyProcessingInfo> countyDataMappings,
            string outputBasePath, ProgressCallback progressCallback = null)
        {
            var results = new Dictionary<string, WriteDataResult>();

            try
            {
                int totalCounties = countyDataMappings.Count;
                int processedCounties = 0;

                progressCallback?.Invoke(0, $"开始批量处理 {totalCounties} 个县的数据写入...");

                foreach (var countyData in countyDataMappings)
                {
                    string countyCode = countyData.Key;
                    var processingInfo = countyData.Value;

                    try
                    {
                        int startProgress = (processedCounties * 100) / totalCounties;
                        int endProgress = ((processedCounties + 1) * 100) / totalCounties;

                        progressCallback?.Invoke(startProgress, $"正在处理县代码 {countyCode}...");

                        // 构建县级输出路径
                        string countyName = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyNameFromCode(countyCode);
                        string countyFolderName = $"{countyName}({countyCode})全民所有自然资源资产清查数据成果";
                        string countyOutputPath = System.IO.Path.Combine(outputBasePath, countyFolderName);

                        // 处理该县的数据
                        var countyResult = ProcessSingleCountyData(processingInfo, countyOutputPath,
                            (percentage, message) => {
                                int adjustedProgress = startProgress + (percentage * (endProgress - startProgress)) / 100;
                                progressCallback?.Invoke(adjustedProgress, $"[{countyCode}] {message}");
                            });

                        results[countyCode] = countyResult;
                        processedCounties++;

                        if (countyResult.Success)
                        {
                            System.Diagnostics.Debug.WriteLine($"县 {countyCode} 数据写入成功");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"县 {countyCode} 数据写入失败: {countyResult.ErrorMessage}");
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorResult = new WriteDataResult
                        {
                            Success = false,
                            ErrorMessage = ex.Message
                        };
                        results[countyCode] = errorResult;
                        processedCounties++;

                        System.Diagnostics.Debug.WriteLine($"处理县 {countyCode} 时出现异常: {ex.Message}");
                    }
                }

                progressCallback?.Invoke(100, "批量数据写入完成");
                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"批量写入县级数据时出错: {ex.Message}");
                return results;
            }
        }

        /// <summary>
        /// 处理单个县的数据
        /// </summary>
        /// <param name="processingInfo">处理信息</param>
        /// <param name="countyOutputPath">县级输出路径</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>处理结果</returns>
        private static WriteDataResult ProcessSingleCountyData(CountyProcessingInfo processingInfo,
            string countyOutputPath, ProgressCallback progressCallback = null)
        {
            try
            {
                // 构建目标Shapefile路径
                string resourcePath = System.IO.Path.Combine(countyOutputPath, "清查数据集", processingInfo.ResourceType, "空间数据");
                string targetShapefileName = GetTargetShapefileName(processingInfo.CountyCode, processingInfo.ResourceType);
                string targetShapefilePath = System.IO.Path.Combine(resourcePath, targetShapefileName + ".shp");

                // 验证目标路径存在
                if (!Directory.Exists(resourcePath))
                {
                    return new WriteDataResult
                    {
                        Success = false,
                        ErrorMessage = $"目标路径不存在: {resourcePath}"
                    };
                }

                // 验证目标Shapefile存在
                if (!File.Exists(targetShapefilePath))
                {
                    return new WriteDataResult
                    {
                        Success = false,
                        ErrorMessage = $"目标Shapefile不存在: {targetShapefilePath}"
                    };
                }

                // 执行数据写入
                return WriteFeatureClassToShapefile(
                    processingInfo.SourceFeatureClass,
                    targetShapefilePath,
                    processingInfo.FieldMappings,
                    processingInfo.WhereClause,
                    progressCallback);
            }
            catch (Exception ex)
            {
                return new WriteDataResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 根据县代码和资源类型获取目标Shapefile名称
        /// </summary>
        /// <param name="countyCode">县代码</param>
        /// <param name="resourceType">资源类型</param>
        /// <returns>Shapefile名称</returns>
        private static string GetTargetShapefileName(string countyCode, string resourceType)
        {
            switch (resourceType)
            {
                case "森林":
                    return $"({countyCode})SLZYZC";
                case "草原":
                    return $"({countyCode})CYZYZC";
                case "湿地":
                    return $"({countyCode})SDZYZC";
                case "城镇村等用地":
                    return $"({countyCode})CZCDYDQC";
                default:
                    return $"({countyCode}){resourceType}";
            }
        }

        /// <summary>
        /// 删除临时文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void DeleteTemporaryFiles(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    // 删除shapefile相关的所有文件
                    string basePath = System.IO.Path.ChangeExtension(filePath, null);
                    string[] extensions = { ".shp", ".shx", ".dbf", ".prj", ".cpg", ".qpj", ".sbn", ".sbx" };

                    foreach (string ext in extensions)
                    {
                        string file = basePath + ext;
                        if (File.Exists(file))
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"删除文件 {file} 时出错: {ex.Message}");
                            }
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

        /// <summary>
        /// 删除多个临时文件
        /// </summary>
        /// <param name="filePaths">文件路径列表</param>
        public static void DeleteTemporaryFiles(IEnumerable<string> filePaths)
        {
            foreach (string filePath in filePaths)
            {
                DeleteTemporaryFiles(filePath);
            }
        }

        /// <summary>
        /// 验证shapefile文件的有效性
        /// </summary>
        /// <param name="shapefilePath">shapefile路径</param>
        /// <returns>是否有效</returns>
        public static bool ValidateShapefile(string shapefilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(shapefilePath) || !File.Exists(shapefilePath))
                {
                    return false;
                }

                // 检查必需的shapefile组件文件
                string basePath = System.IO.Path.ChangeExtension(shapefilePath, null);
                string[] requiredExtensions = { ".shp", ".shx", ".dbf" };

                foreach (string ext in requiredExtensions)
                {
                    if (!File.Exists(basePath + ext))
                    {
                        return false;
                    }
                }

                // 尝试打开shapefile验证其完整性
                var openResult = OpenShapefile(shapefilePath);
                if (openResult.Success && openResult.FeatureClass != null)
                {
                    try
                    {
                        // 尝试获取要素数量以验证数据完整性
                        int count = openResult.FeatureClass.FeatureCount(null);
                        return true;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(openResult.FeatureClass);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(openResult.Workspace);
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"验证shapefile {shapefilePath} 时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取要素类的字段信息
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <returns>字段信息列表</returns>
        public static List<string> GetFeatureClassFields(IFeatureClass featureClass)
        {
            var fieldNames = new List<string>();

            try
            {
                IFields fields = featureClass.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    fieldNames.Add(field.Name);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取要素类字段信息时出错: {ex.Message}");
            }

            return fieldNames;
        }
        /// <summary>
        /// 🔥 新增：清空Shapefile中的所有数据
        /// </summary>
        /// <param name="shapefilePath">shapefile路径</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>清空操作结果</returns>
        public static WriteDataResult ClearShapefileData(string shapefilePath, ProgressCallback progressCallback = null)
        {
            var result = new WriteDataResult();
            IWorkspace workspace = null;
            IFeatureClass featureClass = null;
            IFeatureCursor deleteCursor = null;

            try
            {
                progressCallback?.Invoke(0, "正在打开Shapefile...");

                // 打开Shapefile
                var openResult = OpenShapefile(shapefilePath);
                if (!openResult.Success)
                {
                    result.ErrorMessage = openResult.ErrorMessage;
                    return result;
                }

                workspace = openResult.Workspace;
                featureClass = openResult.FeatureClass;

                // 获取当前要素数量
                int totalFeatures = featureClass.FeatureCount(null);
                progressCallback?.Invoke(10, $"发现 {totalFeatures} 个要素需要清空...");

                if (totalFeatures == 0)
                {
                    progressCallback?.Invoke(100, "Shapefile已经为空，无需清空");
                    result.Success = true;
                    result.ProcessedFeatureCount = 0;
                    result.OutputPath = shapefilePath;
                    return result;
                }

                progressCallback?.Invoke(20, "开始清空数据...");

                // 创建删除游标
                deleteCursor = featureClass.Search(null, false);
                IFeature feature;
                int deletedCount = 0;

                // 🔥 使用UpdateCursor而不是直接删除，这样更安全
                IFeatureCursor updateCursor = featureClass.Update(null, false);

                try
                {
                    while ((feature = updateCursor.NextFeature()) != null)
                    {
                        try
                        {
                            // 删除要素
                            updateCursor.DeleteFeature();
                            deletedCount++;

                            // 更新进度
                            if (deletedCount % 100 == 0 || deletedCount == totalFeatures)
                            {
                                int percentage = 20 + (deletedCount * 70) / totalFeatures;
                                progressCallback?.Invoke(percentage, $"已清空 {deletedCount}/{totalFeatures} 个要素");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"删除第 {deletedCount + 1} 个要素时出错: {ex.Message}");
                        }
                        finally
                        {
                            if (feature != null)
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                        }
                    }

                    // 提交删除操作
                    updateCursor.Flush();
                }
                finally
                {
                    if (updateCursor != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
                }

                result.Success = true;
                result.ProcessedFeatureCount = deletedCount;
                result.OutputPath = shapefilePath;

                progressCallback?.Invoke(100, $"成功清空 {deletedCount} 个要素");
                System.Diagnostics.Debug.WriteLine($"成功清空 {shapefilePath} 中的 {deletedCount} 个要素");

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"清空Shapefile数据时出错: {ex.Message}");
                return result;
            }
            finally
            {
                // 释放资源
                if (deleteCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(deleteCursor);
                if (featureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                if (workspace != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
            }
        }

        /// <summary>
        /// 🔥 修改：向现有Shapefile写入数据（带清空功能）
        /// </summary>
        /// <param name="targetShapefilePath">目标Shapefile路径</param>
        /// <param name="sourceData">源数据列表（几何和属性）</param>
        /// <param name="clearExistingData">是否清空现有数据</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>写入操作结果</returns>
        public static WriteDataResult WriteDataToExistingShapefile(string targetShapefilePath,
            List<FeatureData> sourceData, bool clearExistingData = true, ProgressCallback progressCallback = null)
        {
            var result = new WriteDataResult();
            IWorkspace workspace = null;
            IFeatureClass featureClass = null;
            IFeatureCursor insertCursor = null;

            try
            {
                progressCallback?.Invoke(0, "正在打开目标Shapefile...");

                // 打开现有的Shapefile
                var openResult = OpenShapefile(targetShapefilePath);
                if (!openResult.Success)
                {
                    result.ErrorMessage = openResult.ErrorMessage;
                    return result;
                }

                workspace = openResult.Workspace;
                featureClass = openResult.FeatureClass;

                // 🔥 新增：如果需要清空现有数据
                if (clearExistingData)
                {
                    progressCallback?.Invoke(5, "正在清空现有数据...");

                    var clearResult = ClearShapefileDataInternal(featureClass, (percentage, message) =>
                    {
                        int adjustedProgress = 5 + (percentage * 10) / 100; // 清空操作占用5%-15%的进度
                        progressCallback?.Invoke(adjustedProgress, $"清空数据: {message}");
                    });

                    if (!clearResult.Success)
                    {
                        result.ErrorMessage = $"清空现有数据失败: {clearResult.ErrorMessage}";
                        return result;
                    }

                    System.Diagnostics.Debug.WriteLine($"成功清空 {clearResult.ProcessedFeatureCount} 个现有要素");
                }

                progressCallback?.Invoke(15, "正在准备数据写入...");

                // 创建插入游标
                insertCursor = featureClass.Insert(true);
                int totalFeatures = sourceData.Count;
                int successCount = 0;

                progressCallback?.Invoke(20, $"开始写入 {totalFeatures} 个要素...");

                for (int i = 0; i < totalFeatures; i++)
                {
                    try
                    {
                        var data = sourceData[i];

                        // 创建要素缓冲区
                        IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();

                        // 设置几何
                        if (data.Geometry != null)
                        {
                            featureBuffer.Shape = data.Geometry;
                        }

                        // 设置属性
                        if (data.Attributes != null)
                        {
                            foreach (var attr in data.Attributes)
                            {
                                int fieldIndex = featureClass.FindField(attr.Key);
                                if (fieldIndex != -1)
                                {
                                    try
                                    {
                                        featureBuffer.set_Value(fieldIndex, attr.Value ?? DBNull.Value);
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"设置字段 {attr.Key} 值时出错: {ex.Message}");
                                    }
                                }
                            }
                        }

                        // 插入要素
                        insertCursor.InsertFeature(featureBuffer);
                        successCount++;

                        // 释放缓冲区
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(featureBuffer);

                        // 更新进度
                        if (i % 50 == 0 || i == totalFeatures - 1)
                        {
                            int percentage = 20 + (i * 70) / totalFeatures;
                            progressCallback?.Invoke(percentage, $"已写入 {i + 1}/{totalFeatures} 个要素");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"写入第 {i + 1} 个要素时出错: {ex.Message}");
                    }
                }

                // 提交数据
                insertCursor.Flush();

                result.Success = true;
                result.ProcessedFeatureCount = successCount;
                result.OutputPath = targetShapefilePath;

                progressCallback?.Invoke(100, $"数据写入完成，成功写入 {successCount}/{totalFeatures} 个要素");

                System.Diagnostics.Debug.WriteLine($"向 {targetShapefilePath} 成功写入 {successCount} 个要素");
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"向Shapefile写入数据时出错: {ex.Message}");
                return result;
            }
            finally
            {
                // 释放资源
                if (insertCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                if (featureClass != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                if (workspace != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
            }
        }

        /// <summary>
        /// 🔥 内部方法：清空要素类中的所有数据（不释放要素类对象）
        /// </summary>
        /// <param name="featureClass">要素类</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>清空操作结果</returns>
        private static WriteDataResult ClearShapefileDataInternal(IFeatureClass featureClass, ProgressCallback progressCallback = null)
        {
            var result = new WriteDataResult();
            IFeatureCursor updateCursor = null;

            try
            {
                // 获取当前要素数量
                int totalFeatures = featureClass.FeatureCount(null);

                if (totalFeatures == 0)
                {
                    progressCallback?.Invoke(100, "要素类已经为空");
                    result.Success = true;
                    result.ProcessedFeatureCount = 0;
                    return result;
                }

                progressCallback?.Invoke(0, $"开始清空 {totalFeatures} 个要素...");

                // 创建更新游标
                updateCursor = featureClass.Update(null, false);
                IFeature feature;
                int deletedCount = 0;

                while ((feature = updateCursor.NextFeature()) != null)
                {
                    try
                    {
                        // 删除要素
                        updateCursor.DeleteFeature();
                        deletedCount++;

                        // 更新进度
                        if (deletedCount % 100 == 0 || deletedCount == totalFeatures)
                        {
                            int percentage = (deletedCount * 100) / totalFeatures;
                            progressCallback?.Invoke(percentage, $"已清空 {deletedCount}/{totalFeatures} 个要素");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"删除第 {deletedCount + 1} 个要素时出错: {ex.Message}");
                    }
                    finally
                    {
                        if (feature != null)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                    }
                }

                // 提交删除操作
                updateCursor.Flush();

                result.Success = true;
                result.ProcessedFeatureCount = deletedCount;
                progressCallback?.Invoke(100, $"成功清空 {deletedCount} 个要素");

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"清空要素类数据时出错: {ex.Message}");
                return result;
            }
            finally
            {
                if (updateCursor != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            }
        }

      
        /// <summary>
        /// 批量处理CZCDYDQC数据
        /// </summary>
        /// <param name="countyFiles">县级文件信息列表</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>处理结果列表</returns>
        public static List<CZCDYDProcessor.ProcessingResult> BatchProcessCZCDYDQC(
            List<CZCDYDProcessor.CountyFiles> countyFiles, ProgressCallback progressCallback = null)
        {
            var results = new List<CZCDYDProcessor.ProcessingResult>();
            var processor = new CZCDYDProcessor();

            try
            {
                int totalCounties = countyFiles.Count;
                progressCallback?.Invoke(0, $"开始批量处理 {totalCounties} 个县的CZCDYDQC数据...");

                for (int i = 0; i < totalCounties; i++)
                {
                    var countyFile = countyFiles[i];
                    int startProgress = (i * 100) / totalCounties;
                    int endProgress = ((i + 1) * 100) / totalCounties;

                    progressCallback?.Invoke(startProgress, $"正在处理县代码 {countyFile.CountyCode}...");

                    // 创建县级进度回调
                    CZCDYDProcessor.ProgressCallback countyProgress = (percentage, message) =>
                    {
                        int adjustedProgress = startProgress + (percentage * (endProgress - startProgress)) / 100;
                        progressCallback?.Invoke(adjustedProgress, $"[{countyFile.CountyCode}] {message}");
                    };

                    var result = processor.ProcessCountyCZCDYDQC(countyFile, countyProgress);
                    results.Add(result);

                    if (!result.Success)
                    {
                        System.Diagnostics.Debug.WriteLine($"处理县 {countyFile.CountyCode} 失败: {result.ErrorMessage}");
                    }
                }

                progressCallback?.Invoke(100, "批量处理完成");
                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"批量处理CZCDYDQC数据时出错: {ex.Message}");
                return results;
            }
        }
    }
}