using System;
using System.Collections.Generic;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// Shapefile导出工具类
    /// </summary>
    public class ShapefileExporter
    {
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 导出要素到Shapefile
        /// </summary>
        /// <param name="features">要导出的要素列表</param>
        /// <param name="sourceFeatureClass">源要素类</param>
        /// <param name="outputPath">输出Shapefile路径</param>
        /// <param name="fieldMappings">字段映射配置</param>
        /// <param name="coordinateSystem">坐标系名称</param>
        /// <param name="progressCallback">进度回调</param>
        public void ExportToShapefile(
            List<IFeature> features,
            IFeatureClass sourceFeatureClass,
            string outputPath,
            Dictionary<string, string> fieldMappings,
            string coordinateSystem,
            ProgressCallback progressCallback = null)
        {
            if (features == null || features.Count == 0)
            {
                throw new ArgumentException("要素列表不能为空");
            }

            if (sourceFeatureClass == null)
            {
                throw new ArgumentException("源要素类不能为空");
            }

            progressCallback?.Invoke(5, "正在创建输出工作空间...");

            // 创建输出工作空间
            string outputDir = System.IO.Path.GetDirectoryName(outputPath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(outputPath);

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
            IFeatureWorkspace outputWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(outputDir, 0);

            progressCallback?.Invoke(10, "正在创建字段结构...");

            // 创建输出要素类的字段
            IFieldsEdit outputFields = CreateOutputFields(sourceFeatureClass, fieldMappings, features[0]);

            progressCallback?.Invoke(15, "正在创建输出要素类...");

            // 创建输出要素类
            IFeatureClass outputFeatureClass = outputWorkspace.CreateFeatureClass(
                fileName,
                outputFields,
                null, // UID
                null, // CLSID
                esriFeatureType.esriFTSimple,
                "Shape", // geometry field name
                "" // config keyword
            );

            progressCallback?.Invoke(20, "正在设置坐标系...");

            // 设置坐标系（如果指定）
            if (!string.IsNullOrEmpty(coordinateSystem))
            {
                SetCoordinateSystem(outputFeatureClass, coordinateSystem);
            }

            progressCallback?.Invoke(25, "开始复制要素...");

            // 复制要素
            CopyFeatures(features, sourceFeatureClass, outputFeatureClass, fieldMappings, progressCallback);

            progressCallback?.Invoke(100, $"成功导出 {features.Count} 个要素到 {outputPath}");

            // 释放资源
            System.Runtime.InteropServices.Marshal.ReleaseComObject(outputFeatureClass);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(outputWorkspace);
        }

        /// <summary>
        /// 创建输出字段结构
        /// </summary>
        private IFieldsEdit CreateOutputFields(IFeatureClass sourceFeatureClass, 
            Dictionary<string, string> fieldMappings, IFeature sampleFeature)
        {
            IFieldsEdit outputFields = new FieldsClass();

            // 添加必需的OID字段
            IFieldEdit oidField = new FieldClass();
            oidField.Name_2 = "FID";
            oidField.Type_2 = esriFieldType.esriFieldTypeOID;
            outputFields.AddField(oidField);

            // 添加几何字段
            IFieldEdit shapeField = new FieldClass();
            shapeField.Name_2 = "Shape";
            shapeField.Type_2 = esriFieldType.esriFieldTypeGeometry;
            
            // 设置几何定义
            IGeometryDefEdit geometryDef = new GeometryDefClass();
            geometryDef.GeometryType_2 = sourceFeatureClass.ShapeType;
            geometryDef.HasZ_2 = false;
            geometryDef.HasM_2 = false;
            shapeField.GeometryDef_2 = geometryDef;
            
            outputFields.AddField(shapeField);

            // 添加映射的字段
            IFields sourceFields = sourceFeatureClass.Fields;
            
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;
                string sourceFieldName = mapping.Value;

                // 查找源字段
                int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                if (sourceFieldIndex != -1)
                {
                    IField sourceField = sourceFields.get_Field(sourceFieldIndex);
                    IFieldEdit targetField = new FieldClass();

                    // 设置目标字段属性
                    targetField.Name_2 = targetFieldName;
                    targetField.AliasName_2 = GetFieldDisplayName(targetFieldName);
                    targetField.Type_2 = sourceField.Type;
                    
                    // 复制字段长度和精度
                    if (sourceField.Type == esriFieldType.esriFieldTypeString)
                    {
                        targetField.Length_2 = Math.Max(sourceField.Length, 50); // 确保足够的长度
                    }
                    else if (sourceField.Type == esriFieldType.esriFieldTypeDouble || 
                             sourceField.Type == esriFieldType.esriFieldTypeSingle)
                    {
                        targetField.Precision_2 = sourceField.Precision;
                        targetField.Scale_2 = sourceField.Scale;
                    }

                    targetField.IsNullable_2 = true;
                    outputFields.AddField(targetField);
                }
            }

            return outputFields;
        }

        /// <summary>
        /// 复制要素数据
        /// </summary>
        private void CopyFeatures(List<IFeature> sourceFeatures, IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass, Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback)
        {
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);

            try
            {
                int totalFeatures = sourceFeatures.Count;
                int processedCount = 0;

                foreach (IFeature sourceFeature in sourceFeatures)
                {
                    // 复制几何
                    featureBuffer.Shape = sourceFeature.ShapeCopy;

                    // 复制属性值
                    foreach (var mapping in fieldMappings)
                    {
                        string targetFieldName = mapping.Key;
                        string sourceFieldName = mapping.Value;

                        int sourceFieldIndex = sourceFeatureClass.FindField(sourceFieldName);
                        int targetFieldIndex = targetFeatureClass.FindField(targetFieldName);

                        if (sourceFieldIndex != -1 && targetFieldIndex != -1)
                        {
                            object sourceValue = sourceFeature.get_Value(sourceFieldIndex);
                            
                            // 处理特殊值转换
                            object targetValue = ConvertFieldValue(sourceValue, targetFieldName, sourceFieldName);
                            
                            featureBuffer.set_Value(targetFieldIndex, targetValue);
                        }
                    }

                    // 插入要素
                    insertCursor.InsertFeature(featureBuffer);

                    processedCount++;
                    
                    // 更新进度
                    if (processedCount % 10 == 0 || processedCount == totalFeatures)
                    {
                        int percentage = 25 + (int)((processedCount / (double)totalFeatures) * 75);
                        progressCallback?.Invoke(percentage, 
                            $"正在复制要素... ({processedCount}/{totalFeatures})");
                    }
                }

                // 提交插入操作
                insertCursor.Flush();
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
        /// 转换字段值
        /// </summary>
        private object ConvertFieldValue(object sourceValue, string targetFieldName, string sourceFieldName)
        {
            if (sourceValue == null || sourceValue == DBNull.Value)
            {
                return null;
            }

            // 土地权属值转换
            if (targetFieldName.Equals("TDQS", StringComparison.OrdinalIgnoreCase))
            {
                string strValue = sourceValue.ToString();
                switch (strValue)
                {
                    case "1":
                    case "20":
                        return "国有";
                    case "2":
                    case "30":
                        return "集体";
                    default:
                        return strValue;
                }
            }

            // 地类名称转换
            if (targetFieldName.Equals("DLMC", StringComparison.OrdinalIgnoreCase))
            {
                string strValue = sourceValue.ToString();
                if (strValue.StartsWith("03"))
                {
                    return "林地";
                }
                return strValue;
            }

            return sourceValue;
        }

        /// <summary>
        /// 获取字段显示名称
        /// </summary>
        private string GetFieldDisplayName(string fieldName)
        {
            var displayNames = new Dictionary<string, string>
            {
                { "TBDH", "图斑编号" },
                { "DLMC", "地类名称" },
                { "TDQS", "土地权属" },
                { "MJ", "面积" },
                { "LZFL", "林种分类" },
                { "QSXZ", "权属性质" }
            };

            return displayNames.ContainsKey(fieldName) ? displayNames[fieldName] : fieldName;
        }

        /// <summary>
        /// 设置坐标系
        /// </summary>
        private void SetCoordinateSystem(IFeatureClass featureClass, string coordinateSystemName)
        {
            try
            {
                // 这里可以根据需要实现坐标系设置
                // 由于设置坐标系比较复杂，这里暂时跳过
                // 在实际应用中，可以通过ISpatialReferenceFactory来创建坐标系
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"设置坐标系失败: {ex.Message}");
                // 坐标系设置失败不影响主要功能
            }
        }
    }
}