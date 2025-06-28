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
    /// ���ڲ��ҺͲ����������ݿ�Ҫ����Ĺ�����
    /// </summary>
    public static class GdbFeatureClassFinder
    {
        /// <summary>
        /// ����Ŀ¼�е�����GDB���ҳ�����ָ��ģʽ���Ƶ�Ҫ����
        /// </summary>
        /// <param name="rootDir">��Ŀ¼</param>
        /// <param name="pattern">����ƥ��ģʽ</param>
        /// <param name="geometryType">�������ͣ���ѡ��</param>
        /// <returns>Ҫ�����ļ���Ϣ�б�</returns>
        public static List<LCXZGXFileInfo> FindFeatureClassesWithPattern(string rootDir, string pattern, 
            esriGeometryType geometryType = esriGeometryType.esriGeometryAny)
        {
            var result = new List<LCXZGXFileInfo>();
            
            try
            {
                if (!Directory.Exists(rootDir))
                {
                    System.Diagnostics.Debug.WriteLine($"����: Ŀ¼������: {rootDir}");
                    return result;
                }
                
                System.Diagnostics.Debug.WriteLine($"��ʼ�� {rootDir} �������� '{pattern}' ��GDBҪ����");
                
                // ��������.gdbĿ¼
                var gdbs = Directory.GetDirectories(rootDir, "*.gdb", SearchOption.AllDirectories);
                System.Diagnostics.Debug.WriteLine($"�� {rootDir} Ŀ¼���ҵ� {gdbs.Length} ��GDB�ļ���");
                
                foreach (var gdbPath in gdbs)
                {
                    System.Diagnostics.Debug.WriteLine($"����GDB: {gdbPath}");
                    IWorkspace workspace = null;
                    
                    try
                    {
                        // ����FileGDB�����ռ乤��
                        Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                        if (factoryType == null)
                        {
                            System.Diagnostics.Debug.WriteLine("����: �޷���ȡFileGDBWorkspaceFactory������");
                            continue;
                        }
                        
                        IWorkspaceFactory workspaceFactory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;
                        if (workspaceFactory == null)
                        {
                            System.Diagnostics.Debug.WriteLine("����: �޷�����FileGDBWorkspaceFactoryʵ��");
                            continue;
                        }
                        
                        // ��֤·���Ƿ�����Ч�Ĺ����ռ�
                        if (!workspaceFactory.IsWorkspace(gdbPath))
                        {
                            System.Diagnostics.Debug.WriteLine($"����: {gdbPath} ������Ч��FileGDB�����ռ�");
                            continue;
                        }
                        
                        // �򿪹����ռ�
                        workspace = workspaceFactory.OpenFromFile(gdbPath, 0);
                        if (workspace == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"����: �޷��򿪹����ռ�: {gdbPath}");
                            continue;
                        }
                        
                        System.Diagnostics.Debug.WriteLine($"�ɹ���GDB�����ռ�: {gdbPath}");
                        
                        // ��ȡҪ���๤���ռ�ӿ�
                        IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                        if (featureWorkspace == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"����: {gdbPath} ��֧��Ҫ�������");
                            continue;
                        }
                            
                        // ��ȡҪ�����б�
                        IEnumDataset enumDataset = workspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                        if (enumDataset == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"����: �޷���ȡҪ����ö����");
                            continue;
                        }
                        
                        // ��������Ҫ����
                        enumDataset.Reset();
                        IDataset dataset = null;
                        
                        while ((dataset = enumDataset.Next()) != null)
                        {
                            try
                            {
                                System.Diagnostics.Debug.WriteLine($"����Ҫ����: {dataset.Name}");
                                
                                // ��������Ƿ�ƥ��
                                bool nameMatches = dataset.Name.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;
                                System.Diagnostics.Debug.WriteLine($"Ҫ��������ƥ����: {nameMatches} (����: {dataset.Name}, ģʽ: {pattern})");
                                
                                if (nameMatches)
                                {
                                    // ���Դ�Ҫ�����Ի�ȡ����������Ϣ
                                    IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(dataset.Name);
                                    if (featureClass == null)
                                    {
                                        System.Diagnostics.Debug.WriteLine($"����: �޷���Ҫ���� {dataset.Name}");
                                        continue;
                                    }
                                    
                                    // ���ָ���˼������ͣ���֤�Ƿ�ƥ��
                                    if (geometryType != esriGeometryType.esriGeometryAny)
                                    {
                                        bool geomMatches = featureClass.ShapeType == geometryType;
                                        System.Diagnostics.Debug.WriteLine($"��������ƥ����: {geomMatches} (��ǰ: {featureClass.ShapeType}, ����: {geometryType})");
                                        
                                        if (!geomMatches)
                                        {
                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                                            continue;
                                        }
                                    }
                                    
                                    // ��ȡҪ������
                                    int featureCount = featureClass.FeatureCount(null);
                                    System.Diagnostics.Debug.WriteLine($"Ҫ���� {dataset.Name} ���� {featureCount} ��Ҫ��");
                                    
                                    // ������Ϣ����
                                    string displayName = System.IO.Path.GetFileNameWithoutExtension(gdbPath) + " - " + dataset.Name;
                                    
                                    var fileInfo = new LCXZGXFileInfo
                                    {
                                        FullPath = gdbPath,
                                        DisplayName = displayName,
                                        IsGdb = true,
                                        FeatureClassName = dataset.Name,
                                        GeometryType = featureClass.ShapeType
                                    };
                                    
                                    result.Add(fileInfo);
                                    System.Diagnostics.Debug.WriteLine($"�ҵ�ƥ���GDBҪ����: {displayName}, ·��: {gdbPath}, ��������: {featureClass.ShapeType}");
                                    
                                    // �ͷ�Ҫ������Դ
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"����Ҫ���� {dataset.Name} ʱ����: {ex.Message}");
                                System.Diagnostics.Debug.WriteLine($"�쳣����: {ex}");
                            }
                            finally
                            {
                                if (dataset != null)
                                {
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dataset);
                                }
                            }
                        }
                        
                        // �ͷ�ö������Դ
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(enumDataset);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"��GDB�����ռ� {gdbPath} ʱ����: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"�쳣����: {ex}");
                    }
                    finally
                    {
                        // �ͷŹ����ռ���Դ
                        if (workspace != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
                            workspace = null;
                        }
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"���ҵ� {result.Count} ��ƥ���GDBҪ����");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����GDBҪ����ʱ����: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"�쳣����: {ex}");
            }
            
            return result;
        }
        
        /// <summary>
        /// ��GDB·����Ҫ�������Ƽ���Ҫ����
        /// </summary>
        /// <param name="gdbPath">GDB·��</param>
        /// <param name="featureClassName">Ҫ��������</param>
        /// <returns>Ҫ�������</returns>
        public static IFeatureClass OpenFeatureClassFromGdb(string gdbPath, string featureClassName)
        {
            IWorkspace workspace = null;
            
            try
            {
                System.Diagnostics.Debug.WriteLine($"���Դ�GDBҪ����: {gdbPath}, Ҫ����: {featureClassName}");
                
                if (string.IsNullOrEmpty(gdbPath))
                {
                    throw new ArgumentException("GDB·������Ϊ��");
                }
                
                if (string.IsNullOrEmpty(featureClassName))
                {
                    throw new ArgumentException("Ҫ�������Ʋ���Ϊ��");
                }
                
                if (!Directory.Exists(gdbPath))
                {
                    throw new DirectoryNotFoundException($"GDB·��������: {gdbPath}");
                }
                
                // ����FileGDB�����ռ乤��
                Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                if (factoryType == null)
                {
                    throw new InvalidOperationException("�޷���ȡFileGDBWorkspaceFactory������");
                }
                
                IWorkspaceFactory workspaceFactory = Activator.CreateInstance(factoryType) as IWorkspaceFactory;
                if (workspaceFactory == null)
                {
                    throw new InvalidOperationException("�޷�����FileGDBWorkspaceFactoryʵ��");
                }
                
                if (!workspaceFactory.IsWorkspace(gdbPath))
                {
                    throw new ArgumentException($"{gdbPath} ������Ч��FileGDB");
                }
                
                // �򿪹����ռ�
                workspace = workspaceFactory.OpenFromFile(gdbPath, 0);
                if (workspace == null)
                {
                    throw new InvalidOperationException($"�޷��򿪹����ռ�: {gdbPath}");
                }
                
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                if (featureWorkspace == null)
                {
                    throw new InvalidOperationException("�޷���ȡҪ�ع����ռ�");
                }
                
                // ��Ҫ����
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                if (featureClass == null)
                {
                    throw new InvalidOperationException($"�޷���Ҫ����: {featureClassName}");
                }
                
                System.Diagnostics.Debug.WriteLine($"�ɹ���GDBҪ����: {featureClassName}, Ҫ����: {featureClass.FeatureCount(null)}");
                
                return featureClass;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��GDBҪ����ʱ����: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"�쳣����: {ex}");
                throw new Exception($"����GDBҪ����ʧ��: {ex.Message}", ex);
            }
            finally
            {
                // ע�⣺���ﲻ�ͷ�workspace����ΪҪ��������Ҫʹ����
                // Ҫ����ʹ����Ϻ󣬵��÷���Ҫ�����ͷ���Դ
            }
        }
    }
}