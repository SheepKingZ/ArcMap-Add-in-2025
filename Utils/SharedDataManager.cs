using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 用于在不同窗体之间共享数据的工具类
    /// </summary>
    public static class SharedDataManager
    {
        // 存储找到的LCXZGX文件信息
        private static List<LCXZGXFileInfo> lcxzgxFiles = new List<LCXZGXFileInfo>();
        
        // 存储找到的CZKFBJ文件信息
        private static List<LCXZGXFileInfo> czkfbjFiles = new List<LCXZGXFileInfo>();

        // 新增：存储找到的SLZY_DLTB文件信息
        private static List<LCXZGXFileInfo> slzyDltbFiles = new List<LCXZGXFileInfo>();

        /// <summary>
        /// 设置LCXZGX文件列表
        /// </summary>
        public static void SetLCXZGXFiles(List<LCXZGXFileInfo> files)
        {
            // 确保传入非null的列表
            if (files == null)
            {
                lcxzgxFiles = new List<LCXZGXFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：传入了null的LCXZGX文件列表，已初始化为空列表");
            }
            else
            {
                lcxzgxFiles = new List<LCXZGXFileInfo>(files); // 创建副本以避免引用问题
                
                // 记录详细的文件信息
                System.Diagnostics.Debug.WriteLine($"已保存 {lcxzgxFiles.Count} 个LCXZGX文件信息:");
                foreach (var file in lcxzgxFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                        (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// 获取LCXZGX文件列表
        /// </summary>
        public static List<LCXZGXFileInfo> GetLCXZGXFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {lcxzgxFiles.Count} 个LCXZGX文件");
            // 返回副本以避免外部修改内部列表
            return new List<LCXZGXFileInfo>(lcxzgxFiles);
        }

        /// <summary>
        /// 设置CZKFBJ文件列表
        /// </summary>
        public static void SetCZKFBJFiles(List<LCXZGXFileInfo> files)
        {
            // 确保传入非null的列表
            if (files == null)
            {
                czkfbjFiles = new List<LCXZGXFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：传入了null的CZKFBJ文件列表，已初始化为空列表");
            }
            else
            {
                czkfbjFiles = new List<LCXZGXFileInfo>(files); // 创建副本以避免引用问题
                
                // 记录详细的文件信息
                System.Diagnostics.Debug.WriteLine($"已保存 {czkfbjFiles.Count} 个CZKFBJ文件信息:");
                foreach (var file in czkfbjFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                        (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// 获取CZKFBJ文件列表
        /// </summary>
        public static List<LCXZGXFileInfo> GetCZKFBJFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {czkfbjFiles.Count} 个CZKFBJ文件");
            // 返回副本以避免外部修改内部列表
            return new List<LCXZGXFileInfo>(czkfbjFiles);
        }

        /// <summary>
        /// 设置SLZY_DLTB文件列表
        /// </summary>
        public static void SetSLZYDLTBFiles(List<LCXZGXFileInfo> files)
        {
            // 确保传入非null列表
            if (files == null)
            {
                slzyDltbFiles = new List<LCXZGXFileInfo>();
                System.Diagnostics.Debug.WriteLine("警告：传入了null的SLZY_DLTB文件列表，已初始化为空列表");
            }
            else
            {
                slzyDltbFiles = new List<LCXZGXFileInfo>(files); // 创建副本以避免外部修改
                
                // 记录详细文件信息
                System.Diagnostics.Debug.WriteLine($"已保存 {slzyDltbFiles.Count} 个SLZY_DLTB文件信息:");
                foreach (var file in slzyDltbFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                        (file.IsGdb ? $" (GDB要素类: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// 获取SLZY_DLTB文件列表
        /// </summary>
        public static List<LCXZGXFileInfo> GetSLZYDLTBFiles()
        {
            System.Diagnostics.Debug.WriteLine($"获取 {slzyDltbFiles.Count} 个SLZY_DLTB文件");
            // 返回副本以避免外部修改内部列表
            return new List<LCXZGXFileInfo>(slzyDltbFiles);
        }

        /// <summary>
        /// 清空所有数据
        /// </summary>
        public static void ClearAll()
        {
            lcxzgxFiles.Clear();
            czkfbjFiles.Clear();
            slzyDltbFiles.Clear();
            System.Diagnostics.Debug.WriteLine("已清空所有共享数据");
        }
    }

    /// <summary>
    /// 林草现状或城镇开发边界文件信息
    /// </summary>
    public class LCXZGXFileInfo
    {
        /// <summary>
        /// 文件完整路径
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 显示名称（通常为第二级目录名）
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// 是否为GDB要素类（true为GDB，false为Shapefile）
        /// </summary>
        public bool IsGdb { get; set; }
        
        /// <summary>
        /// GDB要素类名称（仅当IsGdb=true时有效）
        /// </summary>
        public string FeatureClassName { get; set; }
        
        /// <summary>
        /// 几何类型
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        public override string ToString()
        {
            // 确保显示更友好的格式
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