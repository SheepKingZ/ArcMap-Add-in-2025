using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 用于在不同窗体之间共享数据的管理器
    /// 包含源数据文件、CZKFBJ文件、SLZY_DLTB文件
    /// 移除了 LCXZGX 概念，直接使用源数据
    /// </summary>
    public static class SharedDataManager
    {
        // 存储找到的源数据文件信息（直接用于生成 SLZYZC）
        private static List<SourceDataFileInfo> sourceDataFiles = new List<SourceDataFileInfo>();
        
        // 存储找到的CZKFBJ文件信息
        private static List<SourceDataFileInfo> czkfbjFiles = new List<SourceDataFileInfo>();

        // 存储找到的SLZY_DLTB文件信息
        private static List<SourceDataFileInfo> slzyDltbFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// 设置源数据文件列表（直接用于生成 SLZYZC）
        /// </summary>
        public static void SetSourceDataFiles(List<SourceDataFileInfo> files)
        {
            // 确保不传入null列表
            if (files == null)
            {
                sourceDataFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：传入了null的源数据文件列表，已初始化为空列表");
            }
            else
            {
                sourceDataFiles = new List<SourceDataFileInfo>(files); // 创建副本以避免外部修改
                
                // 记录详细文件信息
                System.Diagnostics.Debug.WriteLine($"已保存 {sourceDataFiles.Count} 个源数据文件（直接用于生成 SLZYZC）:");
                foreach (var file in sourceDataFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                        (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// 获取源数据文件列表（直接用于生成 SLZYZC）
        /// </summary>
        public static List<SourceDataFileInfo> GetSourceDataFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {sourceDataFiles.Count} 个源数据文件（直接用于生成 SLZYZC）");
            // 返回副本以避免外部修改内部列表
            return new List<SourceDataFileInfo>(sourceDataFiles);
        }

        // 添加LDHSJG文件管理
        private static List<SourceDataFileInfo> ldhsjgFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// 设置LDHSJG文件列表
        /// </summary>
        /// <param name="files">LDHSJG文件列表</param>
        public static void SetLDHSJGFiles(List<SourceDataFileInfo> files)
        {
            ldhsjgFiles = files ?? new List<SourceDataFileInfo>();
            System.Diagnostics.Debug.WriteLine($"SharedDataManager: 设置了 {ldhsjgFiles.Count} 个LDHSJG文件");
        }

        /// <summary>
        /// 获取LDHSJG文件列表
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
                System.Diagnostics.Debug.WriteLine($"SharedDataManager: 添加了LDHSJG文件 {file.DisplayName}");
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

        /// <summary>
        /// 设置CZKFBJ文件列表（城镇开发边界数据）
        /// </summary>
        public static void SetCZKFBJFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                czkfbjFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：传入了null的CZKFBJ文件列表，已初始化为空列表");
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
                System.Diagnostics.Debug.WriteLine("警告：传入了null的SLZY_DLTB文件列表，已初始化为空列表");
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
        /// 清空所有缓存的数据
        /// </summary>
        public static void ClearAllData()
        {
            sourceDataFiles.Clear();
            czkfbjFiles.Clear();
            slzyDltbFiles.Clear();
            System.Diagnostics.Debug.WriteLine("已清空所有SharedDataManager缓存数据");
        }
    }

    /// <summary>
    /// 源数据文件信息（直接用于生成 SLZYZC，替代原有 LCXZGX）
    /// </summary>
    public class SourceDataFileInfo
    {
        /// <summary>
        /// 文件完整路径（对于GDB要素类，这是GDB路径）
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 显示名称，通常为县名或其他标识
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// 是否为GDB要素类（true为GDB，false为Shapefile）
        /// </summary>
        public bool IsGdb { get; set; }
        
        /// <summary>
        /// GDB要素类名称（仅在IsGdb=true时有效）
        /// </summary>
        public string FeatureClassName { get; set; }
        
        /// <summary>
        /// 几何类型
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        /// <summary>
        /// 数据源类型描述
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
            // 确保显示名有良好的格式
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
    /// 林草湿荒状况或城镇开发边界文件信息（已废弃，请使用SourceDataFileInfo）
    /// </summary>
    [Obsolete("请使用SourceDataFileInfo替代")]
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
        /// GDB要素类名称（仅在IsGdb=true时有效）
        /// </summary>
        public string FeatureClassName { get; set; }
        
        /// <summary>
        /// 几何类型
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        public override string ToString()
        {
            // 确保显示名有良好的格式
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