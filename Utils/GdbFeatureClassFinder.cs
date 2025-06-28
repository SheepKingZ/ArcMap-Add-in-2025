using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// 用于查找和操作地理数据库要素类的工具类
    /// </summary>
    public static class GdbFeatureClassFinder
    {
        /// <summary>
        /// 从GDB路径提取县名（第一级文件夹名称）
        /// </summary>
        /// <param name="gdbPath">GDB文件路径</param>
        /// <param name="rootDir">根目录路径</param>
        /// <returns>县名</returns>
        private static string ExtractCountyNameFromGdbPath(string gdbPath, string rootDir)
        {
            try
            {
                // 规范化路径
                string normalizedRoot = System.IO.Path.GetFullPath(rootDir).TrimEnd('\\', '/');
                string normalizedGdb = System.IO.Path.GetFullPath(gdbPath);
                
                // 计算相对路径
                string relativePath = "";
                if (normalizedGdb.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = normalizedGdb.Substring(normalizedRoot.Length).TrimStart('\\', '/');
                }
                else
                {
                    // 如果路径不匹配，尝试从GDB父目录提取
                    System.Diagnostics.Debug.WriteLine($"警告: GDB路径 {normalizedGdb} 不在根目录 {normalizedRoot} 下");
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(gdbPath));
                }
                
                // 分割路径并获取第一级目录名称（县名）
                string[] pathParts = relativePath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 1)
                {
                    // 返回第一级目录名称，这应该是县名
                    string countyName = pathParts[0];
                    System.Diagnostics.Debug.WriteLine($"从GDB路径 {relativePath} 提取县名: {countyName}");
                    return countyName;
                }
                else
                {
                    // 兜底方案：使用GDB文件夹的父目录名
                    string fallbackName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(gdbPath));
                    System.Diagnostics.Debug.WriteLine($"警告: 无法从GDB路径提取县名，使用父目录名: {fallbackName}");
                    return fallbackName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从GDB路径提取县名时出错: {ex.Message}");
                return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(gdbPath));
            }
        }

        /// <summary>
        /// 查找目录中的所有GDB并找出包含指定模式名称的要素类
        /// </summary>
        /// <param name="rootDir">根目录</param>
        /// <param name="pattern">名称匹配模式</param>
        /// <param name="geometryType">几何类型（可选）</param>
        /// <returns>要素类文件信息列表</returns>
        public static List<LCXZGXFileInfo> FindFeatureClassesWithPattern(string rootDir, string pattern, 
            esriGeometryType geometryType = esriGeometryType.esriGeometryAny)
        {
            var result = new List<LCXZGXFileInfo>();
            
            try
            {
                if (!Directory.Exists(rootDir))
                {
                    System.Diagnostics.Debug.WriteLine($"错误: 目录不存在: {rootDir}");
                    return result;
                }
                
                System.Diagnostics.Debug.WriteLine($"开始在 {rootDir} 搜索包含 '{pattern}' 的GDB要素类");
                
                // 查找所有.gdb目录
                var gdbs = Directory.GetDirectories(rootDir, "*.gdb", SearchOption.AllDirectories);
                System.Diagnostics.Debug.WriteLine($"在 {rootDir} 目录下找到 {gdbs.Length} 个GDB文件夹");
                
                foreach (var gdbPath in gdbs)
                {
                    System.Diagnostics.Debug.WriteLine($"处理GDB: {gdbPath}");
                    IWorkspace workspace = null;
                    
                    try
                    {
                        // 创建FileGDB工作空间工厂
                        Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                        if (factoryType == null)
                        {
                            System.Diagnostics.Debug.WriteLine("错误: 无法获取FileGDBWorkspaceFactory的类型");
                            continue;
                        }
                        
                        IWorkspaceFactory workspaceFactory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;
                        if (workspaceFactory == null)
                        {
                            System.Diagnostics.Debug.WriteLine("错误: 无法创建FileGDBWorkspaceFactory实例");
                            continue;
                        }
                        
                        // 验证路径是否是有效的工作空间
                        if (!workspaceFactory.IsWorkspace(gdbPath))
                        {
                            System.Diagnostics.Debug.WriteLine($"警告: {gdbPath} 不是有效的FileGDB工作空间");
                            continue;
                        }
                        
                        // 打开工作空间
                        workspace = workspaceFactory.OpenFromFile(gdbPath, 0);
                        if (workspace == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"错误: 无法打开工作空间: {gdbPath}");
                            continue;
                        }
                        
                        System.Diagnostics.Debug.WriteLine($"成功打开GDB工作空间: {gdbPath}");
                        
                        // 获取要素类工作空间接口
                        IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                        if (featureWorkspace == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"错误: {gdbPath} 不支持要素类操作");
                            continue;
                        }
                            
                        // 获取要素类列表
                        IEnumDataset enumDataset = workspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                        if (enumDataset == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"警告: 无法获取要素类枚举器");
                            continue;
                        }
                        
                        // 遍历所有要素类
                        enumDataset.Reset();
                        IDataset dataset = null;
                        
                        while ((dataset = enumDataset.Next()) != null)
                        {
                            try
                            {
                                System.Diagnostics.Debug.WriteLine($"处理要素类: {dataset.Name}");
                                
                                // 检查名称是否匹配
                                bool nameMatches = dataset.Name.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;
                                System.Diagnostics.Debug.WriteLine($"要素类名称匹配结果: {nameMatches} (名称: {dataset.Name}, 模式: {pattern})");
                                
                                if (nameMatches)
                                {
                                    // 尝试打开要素类以获取几何类型信息
                                    IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(dataset.Name);
                                    if (featureClass == null)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"错误: 无法打开要素类 {dataset.Name}");
                                        continue;
                                    }
                                    
                                    // 如果指定了几何类型，验证是否匹配
                                    if (geometryType != esriGeometryType.esriGeometryAny)
                                    {
                                        bool geomMatches = featureClass.ShapeType == geometryType;
                                        System.Diagnostics.Debug.WriteLine($"几何类型匹配结果: {geomMatches} (当前: {featureClass.ShapeType}, 期望: {geometryType})");
                                        
                                        if (!geomMatches)
                                        {
                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                                            continue;
                                        }
                                    }
                                    
                                    // 获取要素数量
                                    int featureCount = featureClass.FeatureCount(null);
                                    System.Diagnostics.Debug.WriteLine($"要素类 {dataset.Name} 包含 {featureCount} 个要素");
                                    
                                    // 提取县名（第一级文件夹名称）
                                    string countyName = ExtractCountyNameFromGdbPath(gdbPath, rootDir);
                                    
                                    var fileInfo = new LCXZGXFileInfo
                                    {
                                        FullPath = gdbPath,
                                        DisplayName = countyName, // 使用县名作为显示名称
                                        IsGdb = true,
                                        FeatureClassName = dataset.Name,
                                        GeometryType = featureClass.ShapeType
                                    };
                                    
                                    result.Add(fileInfo);
                                    System.Diagnostics.Debug.WriteLine($"找到匹配的GDB要素类: 县名={countyName}, GDB路径={gdbPath}, 要素类名={dataset.Name}, 几何类型={featureClass.ShapeType}");
                                    
                                    // 释放要素类资源
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"处理要素类 {dataset.Name} 时出错: {ex.Message}");
                                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                            }
                            finally
                            {
                                if (dataset != null)
                                {
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataset);
                                }
                            }
                        }
                        
                        // 释放枚举器资源
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(enumDataset);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"打开GDB工作空间 {gdbPath} 时出错: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                    }
                    finally
                    {
                        // 释放工作空间资源
                        if (workspace != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
                            workspace = null;
                        }
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"共找到 {result.Count} 个匹配的GDB要素类");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找GDB要素类时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
            }
            
            return result;
        }
        
        /// <summary>
        /// 从GDB路径和要素类名称加载要素类
        /// </summary>
        /// <param name="gdbPath">GDB路径</param>
        /// <param name="featureClassName">要素类名称</param>
        /// <returns>要素类对象</returns>
        public static IFeatureClass OpenFeatureClassFromGdb(string gdbPath, string featureClassName)
        {
            IWorkspace workspace = null;
            
            try
            {
                System.Diagnostics.Debug.WriteLine($"尝试打开GDB要素类: {gdbPath}, 要素类: {featureClassName}");
                
                if (string.IsNullOrEmpty(gdbPath))
                {
                    throw new ArgumentException("GDB路径不能为空");
                }
                
                if (string.IsNullOrEmpty(featureClassName))
                {
                    throw new ArgumentException("要素类名称不能为空");
                }
                
                if (!Directory.Exists(gdbPath))
                {
                    throw new DirectoryNotFoundException($"GDB路径不存在: {gdbPath}");
                }
                
                // 创建FileGDB工作空间工厂
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                if (factoryType == null)
                {
                    throw new InvalidOperationException("无法获取FileGDBWorkspaceFactory的类型");
                }
                
                IWorkspaceFactory workspaceFactory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;
                if (workspaceFactory == null)
                {
                    throw new InvalidOperationException("无法创建FileGDBWorkspaceFactory实例");
                }
                
                if (!workspaceFactory.IsWorkspace(gdbPath))
                {
                    throw new ArgumentException($"{gdbPath} 不是有效的FileGDB");
                }
                
                // 打开工作空间
                workspace = workspaceFactory.OpenFromFile(gdbPath, 0);
                if (workspace == null)
                {
                    throw new InvalidOperationException($"无法打开工作空间: {gdbPath}");
                }
                
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                if (featureWorkspace == null)
                {
                    throw new InvalidOperationException("无法获取要素工作空间");
                }
                
                // 打开要素类
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                if (featureClass == null)
                {
                    throw new InvalidOperationException($"无法打开要素类: {featureClassName}");
                }
                
                System.Diagnostics.Debug.WriteLine($"成功打开GDB要素类: {featureClassName}, 要素数: {featureClass.FeatureCount(null)}");
                
                return featureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"打开GDB要素类时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                throw new Exception($"加载GDB要素类失败: {ex.Message}", ex);
            }
            finally
            {
                // 注意：这里不释放workspace，因为要素类仍需要使用它
                // 要素类使用完毕后，调用方需要负责释放资源
            }
        }
    }
}