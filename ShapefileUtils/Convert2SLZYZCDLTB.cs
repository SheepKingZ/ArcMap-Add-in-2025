using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace TestArcMapAddin2.ShapefileUtils
{
    /// <summary>
    /// Shapefile结果表转换工具类 - 将SLZYZC.shp转换为SLZYZC_DLTB.shp
    /// </summary>
    public class Convert2SLZYZCDLTB
    {
        /// <summary>
        /// 进度回调委托 - 用于向UI层报告处理进度
        /// </summary>
        /// <param name="percentage">完成百分比 (0-100)</param>
        /// <param name="message">当前操作描述信息</param>
        public delegate void ProgressCallback(int percentage, string message);

        /// <summary>
        /// 将SLZYZC.shp转换为SLZYZC_DLTB.shp
        /// </summary>
        /// <param name="slzyzcShapefilePath">源SLZYZC.shp的完整路径</param>
        /// <param name="slzyzcDltbShapefilePath">目标SLZYZC_DLTB.shp的完整路径</param>
        /// <param name="fieldMappings">字段映射配置 (目标字段名 -> 源字段名)</param>
        /// <param name="progressCallback">进度回调</param>
        /// <returns>转换是否成功</returns>
        public bool ConvertSLZYZCToDLTB(
            string slzyzcShapefilePath,
            string slzyzcDltbShapefilePath,
            Dictionary<string, string> fieldMappings = null,
            ProgressCallback progressCallback = null)
        {
            // 参数验证
            if (string.IsNullOrEmpty(slzyzcShapefilePath) || !File.Exists(slzyzcShapefilePath))
            {
                throw new FileNotFoundException("源SLZYZC.shp文件不存在", slzyzcShapefilePath);
            }

            if (string.IsNullOrEmpty(slzyzcDltbShapefilePath))
            {
                throw new ArgumentException("目标SLZYZC_DLTB.shp路径不能为空");
            }

            progressCallback?.Invoke(5, "正在开始转换 SLZYZC 到 SLZYZC_DLTB...");

            // COM对象声明
            IWorkspace sourceWorkspace = null;
            IWorkspace targetWorkspace = null;
            IFeatureClass sourceFeatureClass = null;
            IFeatureClass targetFeatureClass = null;

            try
            {
                // 验证并打开源SLZYZC.shp
                progressCallback?.Invoke(15, "正在打开源SLZYZC.shp...");
                var sourceResult = OpenShapefileFeatureClass(slzyzcShapefilePath);
                sourceWorkspace = sourceResult.workspace;
                sourceFeatureClass = sourceResult.featureClass;
                if (sourceFeatureClass == null)
                {
                    throw new Exception("无法打开源SLZYZC.shp");
                }

                // 验证并打开目标SLZYZC_DLTB.shp
                progressCallback?.Invoke(25, "正在打开目标SLZYZC_DLTB.shp...");
                var targetResult = OpenShapefileFeatureClass(slzyzcDltbShapefilePath);
                targetWorkspace = targetResult.workspace;
                targetFeatureClass = targetResult.featureClass;
                if (targetFeatureClass == null)
                {
                    throw new Exception("无法打开目标SLZYZC_DLTB.shp");
                }

                // 清空目标SLZYZC_DLTB.shp的现有数据
                progressCallback?.Invoke(45, "正在清空目标SLZYZC_DLTB.shp的现有数据...");
                ClearShapefileData(targetFeatureClass);

                // 执行数据转换
                progressCallback?.Invoke(55, "开始转换数据...");
                int convertedCount = ConvertFeatures(
                    sourceFeatureClass,
                    targetFeatureClass,
                    fieldMappings,
                    progressCallback);

                progressCallback?.Invoke(100, $"成功转换 {convertedCount} 个要素到SLZYZC_DLTB.shp");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换SLZYZC到DLTB时出错: {ex.Message}");
                progressCallback?.Invoke(0, $"转换失败: {ex.Message}");
                throw;
            }
            finally
            {
                // 释放COM对象
                if (sourceFeatureClass != null) Marshal.ReleaseComObject(sourceFeatureClass);
                if (targetFeatureClass != null) Marshal.ReleaseComObject(targetFeatureClass);
                if (sourceWorkspace != null) Marshal.ReleaseComObject(sourceWorkspace);
                if (targetWorkspace != null) Marshal.ReleaseComObject(targetWorkspace);
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
        private ShapefileOpenResult OpenShapefileFeatureClass(string shapefilePath)
        {
            try
            {
                string shapefileDirectory = System.IO.Path.GetDirectoryName(shapefilePath);
                string shapefileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
                IWorkspace workspace = workspaceFactory.OpenFromFile(shapefileDirectory, 0);
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(shapefileName);

                if (featureClass == null)
                {
                    Marshal.ReleaseComObject(workspace);
                    throw new Exception($"无法打开要素类: {shapefileName}");
                }

                return new ShapefileOpenResult(workspace, featureClass);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"打开shapefile时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 清空shapefile的现有数据
        /// </summary>
        private void ClearShapefileData(IFeatureClass featureClass)
        {
            // 在编辑会话中执行删除操作以获得更好的性能
            IWorkspaceEdit workspaceEdit = ((IDataset)featureClass).Workspace as IWorkspaceEdit;
            if (workspaceEdit == null || !workspaceEdit.IsBeingEdited())
            {
                throw new Exception("无法获取工作空间编辑对象或工作空间未处于编辑状态。");
            }

            try
            {
                workspaceEdit.StartEditOperation();
                ((ITable)featureClass).DeleteSearchedRows(null);
                workspaceEdit.StopEditOperation();
            }
            catch (Exception ex)
            {
                workspaceEdit.AbortEditOperation();
                System.Diagnostics.Debug.WriteLine($"清空shapefile数据时出错: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 转换要素数据
        /// </summary>
        private int ConvertFeatures(
            IFeatureClass sourceFeatureClass,
            IFeatureClass targetFeatureClass,
            Dictionary<string, string> fieldMappings,
            ProgressCallback progressCallback)
        {
            IFeatureCursor insertCursor = targetFeatureClass.Insert(true);
            IFeatureBuffer featureBuffer = targetFeatureClass.CreateFeatureBuffer();
            IFeatureCursor sourceCursor = sourceFeatureClass.Search(null, false);

            try
            {
                int totalFeatures = sourceFeatureClass.FeatureCount(null);
                if (totalFeatures == 0) return 0;

                int processedCount = 0;

                // 获取字段映射，如果未提供则使用默认映射
                if (fieldMappings == null || fieldMappings.Count == 0)
                {
                    fieldMappings = CreateSLZYZC2DLTBFieldsMap();
                }

                IFeature sourceFeature;
                while ((sourceFeature = sourceCursor.NextFeature()) != null)
                {
                    using (var comReleaser = new ComReleaser(sourceFeature))
                    {
                        featureBuffer.Shape = sourceFeature.ShapeCopy;

                        // 复制属性
                        CopyFeatureAttributes(sourceFeature, featureBuffer, fieldMappings, sourceFeatureClass.Fields, targetFeatureClass.Fields);

                        insertCursor.InsertFeature(featureBuffer);
                        processedCount++;

                        // 更新进度
                        if (processedCount % 100 == 0)
                        {
                            int percentage = 55 + (int)((processedCount / (double)totalFeatures) * 40);
                            progressCallback?.Invoke(percentage, $"正在转换数据... ({processedCount}/{totalFeatures})");
                        }
                    }
                }

                insertCursor.Flush();
                return processedCount;
            }
            finally
            {
                if (sourceCursor != null) Marshal.ReleaseComObject(sourceCursor);
                if (insertCursor != null) Marshal.ReleaseComObject(insertCursor);
                if (featureBuffer != null) Marshal.ReleaseComObject(featureBuffer);
            }
        }

        /// <summary>
        /// 复制要素属性
        /// </summary>
        private void CopyFeatureAttributes(IFeature sourceFeature, IFeatureBuffer targetFeatureBuffer, Dictionary<string, string> fieldMappings, IFields sourceFields, IFields targetFields)
        {
            foreach (var mapping in fieldMappings)
            {
                string targetFieldName = mapping.Key;
                string sourceFieldName = mapping.Value;

                int sourceFieldIndex = sourceFields.FindField(sourceFieldName);
                int targetFieldIndex = targetFields.FindField(targetFieldName);

                if (sourceFieldIndex != -1 && targetFieldIndex != -1)
                {
                    object value = sourceFeature.get_Value(sourceFieldIndex);
                    if (value != null && value != DBNull.Value)
                    {
                        targetFeatureBuffer.set_Value(targetFieldIndex, value);
                    }
                }
            }
        }

        /// <summary>
        /// 创建从SLZYZC到SLZYZC_DLTB的默认字段映射
        /// </summary>
        /// <returns>字段映射字典</returns>
        public static Dictionary<string, string> CreateSLZYZC2DLTBFieldsMap()
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "YSDM", "YSDM" },                // 要素代码
                { "XZQDM", "XZQDM" },              // 行政区代码
                { "XZQMC", "XZQMC" },              // 行政区名称
                { "GTDCTBBSM", "GTDCTBBSM" },      // 国土调查图斑编码
                { "GTDCTBBH", "GTDCTBBH" },        // 国土调查图斑编号
                { "GTDCDLBM", "GTDCDLBM" },        // 国土调查地类编码
                { "GTDCDLMC", "GTDCDLMC" },        // 国土调查地类名称
                { "GTDCTDQS", "GTDCTDQS" },        // 国土调查权属状况
                { "QSDWDM", "QSDWDM" },            // 权属单位代码
                { "QSDWMC", "QSDWMC" },            // 权属单位名称
                { "ZLDWDM", "ZLDWDM" },            // 坐落单位代码
                { "ZLDWMC", "ZLDWMC" },            // 坐落单位名称
                { "GTDCTBMJ", "GTDCTBMJ" },        // 国土调查图斑面积
                { "CZKFBJMJ", "CZKFBJMJ" }         // 城镇开发边界面积
            };
            return result;
        }
    }

    /// <summary>
    /// 一个辅助类，用于确保COM对象在使用后被释放。
    /// </summary>
    internal sealed class ComReleaser : IDisposable
    {
        private readonly object _comObject;

        public ComReleaser(object comObject)
        {
            _comObject = comObject;
        }

        public void Dispose()
        {
            if (_comObject != null && Marshal.IsComObject(_comObject))
            {
                Marshal.ReleaseComObject(_comObject);
            }
        }
    }
}