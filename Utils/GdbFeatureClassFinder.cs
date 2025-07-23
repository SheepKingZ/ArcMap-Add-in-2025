using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        /// 从字符串中提取六位数字的县级代码
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>六位数字的县级代码，如果没有找到则返回原字符串</returns>
        private static string ExtractCountyCode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // 查找连续的6位数字
            var match = Regex.Match(input, @"\d{6}");
            if (match.Success)
            {
                System.Diagnostics.Debug.WriteLine($"从 '{input}' 中提取出县级代码: {match.Value}");
                return match.Value;
            }

            // 如果没有找到6位数字，返回原字符串
            System.Diagnostics.Debug.WriteLine($"在 '{input}' 中未找到六位数字县级代码，保留原字符串");
            return input;
        }

        /// <summary>
        /// 公共方法 - 从任意路径中提取县级代码
        /// 这个方法可以被其他类和函数调用，确保整个应用中县级代码提取的一致性
        /// </summary>
        /// <param name="path">任意文件或文件夹路径</param>
        /// <returns>六位数字的县级代码，如果没有找到则返回原字符串</returns>
        public static string GetCountyCodeFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            try
            {
                // 分析路径的各个部分
                string[] pathParts = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

                // 从路径的各个部分中查找县级代码
                foreach (string part in pathParts)
                {
                    var match = Regex.Match(part, @"\d{6}");
                    if (match.Success)
                    {
                        System.Diagnostics.Debug.WriteLine($"从路径 '{path}' 中提取出县级代码: {match.Value}");
                        return match.Value;
                    }
                }

                // 如果在路径各部分中都没找到，尝试从整个路径字符串中提取
                var directMatch = Regex.Match(path, @"\d{6}");
                if (directMatch.Success)
                {
                    System.Diagnostics.Debug.WriteLine($"从路径 '{path}' 中直接提取出县级代码: {directMatch.Value}");
                    return directMatch.Value;
                }

                System.Diagnostics.Debug.WriteLine($"警告: 从路径 '{path}' 中未找到六位数县代码");
                return path;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从路径 '{path}' 提取县级代码时出错: {ex.Message}");
                return path;
            }
        }

        /// <summary>
        /// 从GDB路径提取县名（第一级文件夹名称中的六位数字代码）
        /// </summary>
        /// <param name="gdbPath">GDB文件路径</param>
        /// <param name="rootDir">根目录路径</param>
        /// <returns>县级代码（六位数字）</returns>
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
                    string parentDirName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(gdbPath));
                    return GetCountyCodeFromPath(parentDirName); // 🔥 使用新的公共方法
                }

                // 分割路径并获取第一级目录名称
                string[] pathParts = relativePath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 1)
                {
                    // 从第一级目录名称中提取县级代码
                    string firstLevelDir = pathParts[0];
                    string countyCode = GetCountyCodeFromPath(firstLevelDir); // 🔥 使用新的公共方法
                    System.Diagnostics.Debug.WriteLine($"从GDB路径 {relativePath} 提取县级代码: {countyCode} (原文件夹名: {firstLevelDir})");
                    return countyCode;
                }
                else
                {
                    // 兜底方案：使用GDB文件夹的父目录名
                    string fallbackName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(gdbPath));
                    string countyCode = GetCountyCodeFromPath(fallbackName); // 🔥 使用新的公共方法
                    System.Diagnostics.Debug.WriteLine($"警告: 无法从GDB路径提取县名，使用父目录名提取县级代码: {countyCode}");
                    return countyCode;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从GDB路径提取县名时出错: {ex.Message}");
                string fallbackName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(gdbPath));
                return GetCountyCodeFromPath(fallbackName); // 🔥 使用新的公共方法
            }
        }

        /// <summary>
        /// 查找目录中的所有GDB并找出包含指定模式名称的要素类作为源数据
        /// 专门用于SLZYZC直接数据源查找，替代原有的LCXZGX中间过程
        /// 增强版：支持从包含额外文字的文件夹名称中提取县级代码
        /// </summary>
        /// <param name="rootDir">根目录</param>
        /// <param name="pattern">名称匹配模式</param>
        /// <param name="geometryType">几何类型（可选）</param>
        /// <param name="countyCode">县级代码（可选，用于筛选特定县的数据）</param>
        /// <returns>源数据文件信息列表</returns>
        public static List<SourceDataFileInfo> FindFeatureClassesWithPatternAsSourceData(string rootDir, string pattern,
            esriGeometryType geometryType = esriGeometryType.esriGeometryAny, string countyCode = null)
        {
            var result = new List<SourceDataFileInfo>();

            try
            {
                if (!Directory.Exists(rootDir))
                {
                    System.Diagnostics.Debug.WriteLine($"错误: 目录不存在: {rootDir}");
                    return result;
                }

                System.Diagnostics.Debug.WriteLine($"开始在 {rootDir} 搜索包含 '{pattern}' 的源数据GDB要素类" +
                    (string.IsNullOrEmpty(countyCode) ? "" : $" (筛选县级代码: {countyCode})"));

                // 查找所有.gdb目录
                var gdbs = Directory.GetDirectories(rootDir, "*.gdb", SearchOption.AllDirectories);
                System.Diagnostics.Debug.WriteLine($"在 {rootDir} 目录下找到 {gdbs.Length} 个GDB文件夹");

                foreach (var gdbPath in gdbs)
                {
                    // 🔥 新增：如果指定了县级代码，先检查是否匹配
                    if (!string.IsNullOrEmpty(countyCode))
                    {
                        // 提取GDB路径对应的县级代码
                        string gdbCountyCode = ExtractCountyNameFromGdbPath(gdbPath, rootDir);

                        // 如果提取的县级代码不匹配，跳过此GDB
                        if (!string.Equals(gdbCountyCode, countyCode, StringComparison.OrdinalIgnoreCase))
                        {
                            System.Diagnostics.Debug.WriteLine($"跳过GDB {gdbPath}: 县级代码不匹配 (期望: {countyCode}, 实际: {gdbCountyCode})");
                            continue;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"处理源数据GDB: {gdbPath}");
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

                        System.Diagnostics.Debug.WriteLine($"成功打开源数据GDB工作空间: {gdbPath}");

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
                                //System.Diagnostics.Debug.WriteLine($"处理源数据要素类: {dataset.Name}");

                                // 检查名称是否匹配
                                bool nameMatches = dataset.Name.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;
                                //System.Diagnostics.Debug.WriteLine($"源数据要素类名称匹配结果: {nameMatches} (名称: {dataset.Name}, 模式: {pattern})");

                                if (nameMatches)
                                {
                                    // 尝试打开要素类以获取几何类型信息
                                    IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(dataset.Name);
                                    if (featureClass == null)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"错误: 无法打开源数据要素类 {dataset.Name}");
                                        continue;
                                    }

                                    // 如果指定了几何类型，验证是否匹配
                                    if (geometryType != esriGeometryType.esriGeometryAny)
                                    {
                                        bool geomMatches = featureClass.ShapeType == geometryType;
                                        //System.Diagnostics.Debug.WriteLine($"几何类型匹配结果: {geomMatches} (当前: {featureClass.ShapeType}, 期望: {geometryType})");

                                        if (!geomMatches)
                                        {
                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                                            continue;
                                        }
                                    }

                                    // 获取要素数量
                                    int featureCount = featureClass.FeatureCount(null);
                                    //System.Diagnostics.Debug.WriteLine($"源数据要素类 {dataset.Name} 包含 {featureCount} 个要素");

                                    // 提取县级代码（从第一级文件夹名称中提取六位数字）
                                    string extractedCountyCode = ExtractCountyNameFromGdbPath(gdbPath, rootDir);

                                    // 创建源数据文件信息，使用提取的县级代码作为显示名称
                                    var sourceDataInfo = new SourceDataFileInfo
                                    {
                                        FullPath = gdbPath,
                                        DisplayName = extractedCountyCode, // 使用提取的县级代码
                                        IsGdb = true,
                                        FeatureClassName = dataset.Name,
                                        GeometryType = featureClass.ShapeType,
                                        CountyCode = extractedCountyCode // 设置县级代码
                                    };

                                    result.Add(sourceDataInfo);
                                    //System.Diagnostics.Debug.WriteLine($"找到匹配的源数据GDB要素类: 县级代码={extractedCountyCode}, GDB路径={gdbPath}, 要素类名={dataset.Name}, 几何类型={featureClass.ShapeType}");

                                    // 释放要素类资源
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"处理源数据要素类 {dataset.Name} 时出错: {ex.Message}");
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
                        //System.Diagnostics.Debug.WriteLine($"打开源数据GDB工作空间 {gdbPath} 时出错: {ex.Message}");
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

                //System.Diagnostics.Debug.WriteLine($"共找到 {result.Count} 个匹配的源数据GDB要素类");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找源数据GDB要素类时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
            }

            return result;
        }

        /// <summary>
        /// 根据县级代码查找对应的LCXZGX_P数据
        /// 支持从包含额外文字的文件夹名称中提取和匹配县级代码
        /// </summary>
        /// <param name="rootDir">根目录</param>
        /// <param name="countyCode">县级代码</param>
        /// <param name="geometryType">几何类型（可选）</param>
        /// <returns>匹配的源数据文件信息列表</returns>
        public static List<SourceDataFileInfo> FindLCXZGXDataByCountyCode(string rootDir, string countyCode,
            esriGeometryType geometryType = esriGeometryType.esriGeometryPolygon)
        {
            if (string.IsNullOrEmpty(countyCode))
            {
                System.Diagnostics.Debug.WriteLine("错误: 县级代码不能为空");
                return new List<SourceDataFileInfo>();
            }

            System.Diagnostics.Debug.WriteLine($"开始根据县级代码 '{countyCode}' 查找LCXZGX_P数据");

            // 使用增强版的查找方法，传入县级代码进行精确匹配
            var results = FindFeatureClassesWithPatternAsSourceData(rootDir, "LCXZGX_P", geometryType, countyCode);

            System.Diagnostics.Debug.WriteLine($"根据县级代码 '{countyCode}' 找到 {results.Count} 个LCXZGX_P数据源");

            return results;
        }

        /// <summary>
        /// 对源数据文件列表按县级代码进行分组和验证
        /// 确保所有数据都有有效的县级代码
        /// </summary>
        /// <param name="sourceDataFiles">源数据文件列表</param>
        /// <returns>按县级代码分组的数据，只包含有效县级代码的数据</returns>
        public static Dictionary<string, List<SourceDataFileInfo>> GroupSourceDataByCountyCode(List<SourceDataFileInfo> sourceDataFiles)
        {
            var result = new Dictionary<string, List<SourceDataFileInfo>>();

            if (sourceDataFiles == null || sourceDataFiles.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("源数据文件列表为空");
                return result;
            }

            System.Diagnostics.Debug.WriteLine($"开始对 {sourceDataFiles.Count} 个源数据文件进行县级代码分组");

            foreach (var file in sourceDataFiles)
            {
                try
                {
                    // 确保使用统一的县级代码提取逻辑
                    string countyCode = !string.IsNullOrEmpty(file.CountyCode)
                        ? file.CountyCode
                        : GetCountyCodeFromPath(file.FullPath);

                    // 验证县级代码是否为6位数字
                    if (Regex.IsMatch(countyCode, @"^\d{6}$"))
                    {
                        // 更新文件信息中的县级代码
                        file.CountyCode = countyCode;
                        file.DisplayName = countyCode;

                        if (!result.ContainsKey(countyCode))
                        {
                            result[countyCode] = new List<SourceDataFileInfo>();
                        }
                        result[countyCode].Add(file);

                        System.Diagnostics.Debug.WriteLine($"文件 {file.FullPath} 归类到县级代码: {countyCode}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"警告: 从路径 {file.FullPath} 中未找到六位数县代码，跳过该文件");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"处理文件 {file.FullPath} 时出错: {ex.Message}");
                }
            }

            System.Diagnostics.Debug.WriteLine($"县级代码分组完成，共找到 {result.Count} 个县的数据：");
            foreach (var group in result)
            {
                System.Diagnostics.Debug.WriteLine($"  县级代码 {group.Key}: {group.Value.Count} 个文件");
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