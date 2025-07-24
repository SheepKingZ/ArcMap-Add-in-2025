using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 用于在不同窗体之间共享数据的管理器
    /// 源数据文件、CZKFBJ文件、SLZY_DLTB文件、CYZY_DLTB文件、SDZY_DLTB文件
    /// </summary>
    public static class SharedDataManager
    {
        // 存储找到的源数据文件信息（直接来自原始 SLZYZC）
        private static List<SourceDataFileInfo> sourceDataFiles = new List<SourceDataFileInfo>();

        // 存储找到的CZKFBJ文件信息
        private static List<SourceDataFileInfo> czkfbjFiles = new List<SourceDataFileInfo>();

        // 存储找到的SLZY_DLTB文件信息
        private static List<SourceDataFileInfo> slzyDltbFiles = new List<SourceDataFileInfo>();

        // 新增：存储找到的CYZY_DLTB文件信息（草地资源地类图斑数据）
        private static List<SourceDataFileInfo> cyzyDltbFiles = new List<SourceDataFileInfo>();

        // 新增：存储找到的SDZY_DLTB文件信息（湿地资源地类图斑数据）
        private static List<SourceDataFileInfo> sdzyDltbFiles = new List<SourceDataFileInfo>();

        // 数据类型选择状态
        private static bool isForestSelected = true;
        private static bool isGrasslandSelected = false;
        private static bool isWetlandSelected = false;  // 新增：湿地选择状态

        /// <summary>
        /// 设置数据类型选择状态
        /// </summary>
        public static void SetDataTypeSelection(bool forest, bool grassland, bool wetland)
        {
            isForestSelected = forest;
            isGrasslandSelected = grassland;
            isWetlandSelected = wetland;
            System.Diagnostics.Debug.WriteLine($"数据类型选择状态已更新 - 林地: {forest}, 草地: {grassland}, 湿地: {wetland}");
        }

        /// <summary>
        /// 获取数据类型选择状态
        /// </summary>
        public static (bool Forest, bool Grassland, bool Wetland) GetDataTypeSelection()
        {
            return (isForestSelected, isGrasslandSelected, isWetlandSelected);
        }

        /// <summary>
        /// 设置源数据文件列表（直接来自原始 SLZYZC）
        /// </summary>
        public static void SetSourceDataFiles(List<SourceDataFileInfo> files)
        {
            // 确保处理null列表
            if (files == null)
            {
                sourceDataFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：输入了null源数据文件列表，已初始化为空列表");
            }
            else
            {
                sourceDataFiles = new List<SourceDataFileInfo>(files); // 创建副本以避免外部修改

                // 记录详细文件信息
                System.Diagnostics.Debug.WriteLine($"已保存 {sourceDataFiles.Count} 个源数据文件（直接来自原始 SLZYZC）:");
                foreach (var file in sourceDataFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" +
                        (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// 获取源数据文件列表（直接来自原始 SLZYZC）
        /// </summary>
        public static List<SourceDataFileInfo> GetSourceDataFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {sourceDataFiles.Count} 个源数据文件（直接来自原始 SLZYZC）");
            // 返回副本以避免外部修改内部列表
            return new List<SourceDataFileInfo>(sourceDataFiles);
        }

        // 林地HSJG文件处理
        private static List<SourceDataFileInfo> ldhsjgFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// 设置LDHSJG文件列表（林地核算价格）
        /// </summary>
        /// <param name="files">LDHSJG文件列表</param>
        public static void SetLDHSJGFiles(List<SourceDataFileInfo> files)
        {
            ldhsjgFiles = files ?? new List<SourceDataFileInfo>();
            System.Diagnostics.Debug.WriteLine($"SharedDataManager: 已保存 {ldhsjgFiles.Count} 个LDHSJG文件");
        }

        /// <summary>
        /// 获取LDHSJG文件列表（林地核算价格）
        /// </summary>
        /// <returns>LDHSJG文件列表</returns>
        public static List<SourceDataFileInfo> GetLDHSJGFiles()
        {
            return new List<SourceDataFileInfo>(ldhsjgFiles);
        }

        /// <summary>
        /// 添加LDHSJG文件
        /// </summary>
        /// <param name="file">要添加的LDHSJG文件</param>
        public static void AddLDHSJGFile(SourceDataFileInfo file)
        {
            if (file != null)
            {
                ldhsjgFiles.Add(file);
                System.Diagnostics.Debug.WriteLine($"SharedDataManager: 已添加LDHSJG文件 {file.DisplayName}");
            }
        }

        /// <summary>
        /// 清空LDHSJG文件列表
        /// </summary>
        public static void ClearLDHSJGFiles()
        {
            ldhsjgFiles.Clear();
            System.Diagnostics.Debug.WriteLine("SharedDataManager: 已清空LDHSJG文件列表");
        }

        // 草地HSJG文件处理
        private static List<SourceDataFileInfo> cdhsjgFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// 设置CDHSJG文件列表（草地核算价格）
        /// </summary>
        /// <param name="files">CDHSJG文件列表</param>
        public static void SetCDHSJGFiles(List<SourceDataFileInfo> files)
        {
            cdhsjgFiles = files ?? new List<SourceDataFileInfo>();
            System.Diagnostics.Debug.WriteLine($"SharedDataManager: 已保存 {cdhsjgFiles.Count} 个CDHSJG文件");
        }

        /// <summary>
        /// 获取CDHSJG文件列表（草地核算价格）
        /// </summary>
        /// <returns>CDHSJG文件列表</returns>
        public static List<SourceDataFileInfo> GetCDHSJGFiles()
        {
            return new List<SourceDataFileInfo>(cdhsjgFiles);
        }

        /// <summary>
        /// 添加CDHSJG文件
        /// </summary>
        /// <param name="file">要添加的CDHSJG文件</param>
        public static void AddCDHSJGFile(SourceDataFileInfo file)
        {
            if (file != null)
            {
                cdhsjgFiles.Add(file);
                System.Diagnostics.Debug.WriteLine($"SharedDataManager: 已添加CDHSJG文件 {file.DisplayName}");
            }
        }

        /// <summary>
        /// 清空CDHSJG文件列表
        /// </summary>
        public static void ClearCDHSJGFiles()
        {
            cdhsjgFiles.Clear();
            System.Diagnostics.Debug.WriteLine("SharedDataManager: 已清空CDHSJG文件列表");
        }

        // 新增：湿地HSJG文件处理
        private static List<SourceDataFileInfo> sdhsjgFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// 设置SDHSJG文件列表（湿地核算价格）
        /// </summary>
        /// <param name="files">SDHSJG文件列表</param>
        public static void SetSDHSJGFiles(List<SourceDataFileInfo> files)
        {
            sdhsjgFiles = files ?? new List<SourceDataFileInfo>();
            System.Diagnostics.Debug.WriteLine($"SharedDataManager: 已保存 {sdhsjgFiles.Count} 个SDHSJG文件");
        }

        /// <summary>
        /// 获取SDHSJG文件列表（湿地核算价格）
        /// </summary>
        /// <returns>SDHSJG文件列表</returns>
        public static List<SourceDataFileInfo> GetSDHSJGFiles()
        {
            return new List<SourceDataFileInfo>(sdhsjgFiles);
        }

        /// <summary>
        /// 添加SDHSJG文件
        /// </summary>
        /// <param name="file">要添加的SDHSJG文件</param>
        public static void AddSDHSJGFile(SourceDataFileInfo file)
        {
            if (file != null)
            {
                sdhsjgFiles.Add(file);
                System.Diagnostics.Debug.WriteLine($"SharedDataManager: 已添加SDHSJG文件 {file.DisplayName}");
            }
        }

        /// <summary>
        /// 清空SDHSJG文件列表
        /// </summary>
        public static void ClearSDHSJGFiles()
        {
            sdhsjgFiles.Clear();
            System.Diagnostics.Debug.WriteLine("SharedDataManager: 已清空SDHSJG文件列表");
        }

        /// <summary>
        /// 设置CZKFBJ文件列表（城镇开发边界数据）
        /// </summary>
        public static void SetCZKFBJFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                czkfbjFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：输入了null的CZKFBJ文件列表，已初始化为空列表");
                return;
            }

            // 直接保存，不需要转换
            czkfbjFiles = new List<SourceDataFileInfo>(files);

            System.Diagnostics.Debug.WriteLine($"已保存 {czkfbjFiles.Count} 个CZKFBJ文件:");
            foreach (var file in czkfbjFiles)
            {
                System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" +
                    (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
            }
        }

        /// <summary>
        /// 获取CZKFBJ文件列表（城镇开发边界数据）
        /// </summary>
        public static List<SourceDataFileInfo> GetCZKFBJFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {czkfbjFiles.Count} 个CZKFBJ文件");
            // 返回副本以避免外部修改内部列表
            return new List<SourceDataFileInfo>(czkfbjFiles);
        }

        /// <summary>
        /// 设置SLZY_DLTB文件列表（森林资源地类图斑数据）
        /// </summary>
        public static void SetSLZYDLTBFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                slzyDltbFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：输入了null的SLZY_DLTB文件列表，已初始化为空列表");
                return;
            }

            // 直接保存，不需要转换
            slzyDltbFiles = new List<SourceDataFileInfo>(files);

            System.Diagnostics.Debug.WriteLine($"已保存 {slzyDltbFiles.Count} 个SLZY_DLTB文件:");
            foreach (var file in slzyDltbFiles)
            {
                System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" +
                    (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
            }
        }

        /// <summary>
        /// 获取SLZY_DLTB文件列表（森林资源地类图斑数据）
        /// </summary>
        public static List<SourceDataFileInfo> GetSLZYDLTBFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {slzyDltbFiles.Count} 个SLZY_DLTB文件");
            // 返回副本以避免外部修改内部列表
            return new List<SourceDataFileInfo>(slzyDltbFiles);
        }

        /// <summary>
        /// 设置CYZY_DLTB文件列表（草地资源地类图斑数据）
        /// </summary>
        public static void SetCYZYDLTBFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                cyzyDltbFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：输入了null的CYZY_DLTB文件列表，已初始化为空列表");
                return;
            }

            // 直接保存，不需要转换
            cyzyDltbFiles = new List<SourceDataFileInfo>(files);

            System.Diagnostics.Debug.WriteLine($"已保存 {cyzyDltbFiles.Count} 个CYZY_DLTB文件:");
            foreach (var file in cyzyDltbFiles)
            {
                System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" +
                    (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
            }
        }

        /// <summary>
        /// 获取CYZY_DLTB文件列表（草地资源地类图斑数据）
        /// </summary>
        public static List<SourceDataFileInfo> GetCYZYDLTBFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {cyzyDltbFiles.Count} 个CYZY_DLTB文件");
            // 返回副本以避免外部修改内部列表
            return new List<SourceDataFileInfo>(cyzyDltbFiles);
        }

        /// <summary>
        /// 设置SDZY_DLTB文件列表（湿地资源地类图斑数据）
        /// </summary>
        public static void SetSDZYDLTBFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                sdzyDltbFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：输入了null的SDZY_DLTB文件列表，已初始化为空列表");
                return;
            }

            // 直接保存，不需要转换
            sdzyDltbFiles = new List<SourceDataFileInfo>(files);

            System.Diagnostics.Debug.WriteLine($"已保存 {sdzyDltbFiles.Count} 个SDZY_DLTB文件:");
            foreach (var file in sdzyDltbFiles)
            {
                System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" +
                    (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
            }
        }

        /// <summary>
        /// 获取SDZY_DLTB文件列表（湿地资源地类图斑数据）
        /// </summary>
        public static List<SourceDataFileInfo> GetSDZYDLTBFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {sdzyDltbFiles.Count} 个SDZY_DLTB文件");
            // 返回副本以避免外部修改内部列表
            return new List<SourceDataFileInfo>(sdzyDltbFiles);
        }

        /// <summary>
        /// 清空或重置所有数据
        /// </summary>
        public static void ClearAllData()
        {
            sourceDataFiles.Clear();
            czkfbjFiles.Clear();
            slzyDltbFiles.Clear();
            cyzyDltbFiles.Clear();
            sdzyDltbFiles.Clear();
            ldhsjgFiles.Clear();
            cdhsjgFiles.Clear();
            sdhsjgFiles.Clear();
            System.Diagnostics.Debug.WriteLine("已清空所有SharedDataManager中的数据");
        }
    }

    /// <summary>
    /// 源数据文件信息（直接来自原始 SLZYZC，替代原来 LCXZGX）
    /// </summary>
    public class SourceDataFileInfo
    {
        /// <summary>
        /// 文件完整路径（如果是GDB要素类，这里是GDB路径）
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 显示名称，通常为第二级目录标识
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否为GDB要素类（true为GDB，false为Shapefile）
        /// </summary>
        public bool IsGdb { get; set; }

        /// <summary>
        /// GDB要素类名称，仅在IsGdb=true时有效
        /// </summary>
        public string FeatureClassName { get; set; }

        /// <summary>
        /// 所属县（单位代码）
        /// </summary>
        public string CountyCode { get; set; }

        /// <summary>
        /// 几何类型
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        /// <summary>
        /// 数据源描述文本
        /// </summary>
        public string DataSourceType
        {
            get
            {
                return IsGdb ? "GDB要素类" : "Shapefile";
            }
        }

        public override string ToString()
        {
            // 确保显示名称采用合适的格式
            if (!string.IsNullOrEmpty(DisplayName))
            {
                return DisplayName;
            }
            else if (IsGdb && !string.IsNullOrEmpty(FeatureClassName))
            {
                return $"{System.IO.Path.GetFileName(FullPath)} - {FeatureClassName}";
            }
            else
            {
                // 如果没有显示名称，使用文件名
                return System.IO.Path.GetFileName(FullPath);
            }
        }
    }

    /// <summary>
    /// 林草湿荒状况数据或城镇开发边界文件信息，已被弃用，请使用SourceDataFileInfo代替
    /// </summary>
    [Obsolete("请使用SourceDataFileInfo代替")]
    public class LCXZGXFileInfo
    {
        /// <summary>
        /// 文件完整路径
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 显示名称，通常为第二级目录名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否为GDB要素类（true为GDB，false为Shapefile）
        /// </summary>
        public bool IsGdb { get; set; }

        /// <summary>
        /// GDB要素类名称，仅在IsGdb=true时有效
        /// </summary>
        public string FeatureClassName { get; set; }

        /// <summary>
        /// 几何类型
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        public override string ToString()
        {
            // 确保显示名称采用合适的格式
            if (!string.IsNullOrEmpty(DisplayName))
            {
                return DisplayName;
            }
            else if (IsGdb && !string.IsNullOrEmpty(FeatureClassName))
            {
                return $"{System.IO.Path.GetFileName(FullPath)} - {FeatureClassName}";
            }
            else
            {
                // 如果没有显示名称，使用文件名
                return System.IO.Path.GetFileName(FullPath);
            }
        }
    }
}