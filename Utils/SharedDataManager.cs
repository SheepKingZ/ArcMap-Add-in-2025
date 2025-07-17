using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// �����ڲ�ͬ����֮�乲�����ݵĹ�����
    /// ����Դ�����ļ���CZKFBJ�ļ���SLZY_DLTB�ļ�
    /// �Ƴ��� LCXZGX ���ֱ��ʹ��Դ����
    /// </summary>
    public static class SharedDataManager
    {
        // �洢�ҵ���Դ�����ļ���Ϣ��ֱ���������� SLZYZC��
        private static List<SourceDataFileInfo> sourceDataFiles = new List<SourceDataFileInfo>();
        
        // �洢�ҵ���CZKFBJ�ļ���Ϣ
        private static List<SourceDataFileInfo> czkfbjFiles = new List<SourceDataFileInfo>();

        // �洢�ҵ���SLZY_DLTB�ļ���Ϣ
        private static List<SourceDataFileInfo> slzyDltbFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// ����Դ�����ļ��б�ֱ���������� SLZYZC��
        /// </summary>
        public static void SetSourceDataFiles(List<SourceDataFileInfo> files)
        {
            // ȷ��������null�б�
            if (files == null)
            {
                sourceDataFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������null��Դ�����ļ��б��ѳ�ʼ��Ϊ���б�");
            }
            else
            {
                sourceDataFiles = new List<SourceDataFileInfo>(files); // ���������Ա����ⲿ�޸�
                
                // ��¼��ϸ�ļ���Ϣ
                System.Diagnostics.Debug.WriteLine($"�ѱ��� {sourceDataFiles.Count} ��Դ�����ļ���ֱ���������� SLZYZC��:");
                foreach (var file in sourceDataFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                        (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// ��ȡԴ�����ļ��б�ֱ���������� SLZYZC��
        /// </summary>
        public static List<SourceDataFileInfo> GetSourceDataFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {sourceDataFiles.Count} ��Դ�����ļ���ֱ���������� SLZYZC��");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<SourceDataFileInfo>(sourceDataFiles);
        }

        // ���LDHSJG�ļ�����
        private static List<SourceDataFileInfo> ldhsjgFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// ����LDHSJG�ļ��б�
        /// </summary>
        /// <param name="files">LDHSJG�ļ��б�</param>
        public static void SetLDHSJGFiles(List<SourceDataFileInfo> files)
        {
            ldhsjgFiles = files ?? new List<SourceDataFileInfo>();
            System.Diagnostics.Debug.WriteLine($"SharedDataManager: ������ {ldhsjgFiles.Count} ��LDHSJG�ļ�");
        }

        /// <summary>
        /// ��ȡLDHSJG�ļ��б�
        /// </summary>
        /// <returns>LDHSJG�ļ��б�</returns>
        public static List<SourceDataFileInfo> GetLDHSJGFiles()
        {
            return new List<SourceDataFileInfo>(ldhsjgFiles);
        }

        /// <summary>
        /// ���LDHSJG�ļ�
        /// </summary>
        /// <param name="file">Ҫ��ӵ�LDHSJG�ļ�</param>
        public static void AddLDHSJGFile(SourceDataFileInfo file)
        {
            if (file != null)
            {
                ldhsjgFiles.Add(file);
                System.Diagnostics.Debug.WriteLine($"SharedDataManager: �����LDHSJG�ļ� {file.DisplayName}");
            }
        }

        /// <summary>
        /// ���LDHSJG�ļ��б�
        /// </summary>
        public static void ClearLDHSJGFiles()
        {
            ldhsjgFiles.Clear();
            System.Diagnostics.Debug.WriteLine("SharedDataManager: �����LDHSJG�ļ��б�");
        }

        /// <summary>
        /// ����CZKFBJ�ļ��б����򿪷��߽����ݣ�
        /// </summary>
        public static void SetCZKFBJFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                czkfbjFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������null��CZKFBJ�ļ��б��ѳ�ʼ��Ϊ���б�");
                return;
            }

            // ֱ�ӱ��棬����Ҫת��
            czkfbjFiles = new List<SourceDataFileInfo>(files);
            
            System.Diagnostics.Debug.WriteLine($"�ѱ��� {czkfbjFiles.Count} ��CZKFBJ�ļ�:");
            foreach (var file in czkfbjFiles)
            {
                System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                    (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
            }
        }

        /// <summary>
        /// ��ȡCZKFBJ�ļ��б����򿪷��߽����ݣ�
        /// </summary>
        public static List<SourceDataFileInfo> GetCZKFBJFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {czkfbjFiles.Count} ��CZKFBJ�ļ�");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<SourceDataFileInfo>(czkfbjFiles);
        }

        /// <summary>
        /// ����SLZY_DLTB�ļ��б�ɭ����Դ����ͼ�����ݣ�
        /// </summary>
        public static void SetSLZYDLTBFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                slzyDltbFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������null��SLZY_DLTB�ļ��б��ѳ�ʼ��Ϊ���б�");
                return;
            }

            // ֱ�ӱ��棬����Ҫת��
            slzyDltbFiles = new List<SourceDataFileInfo>(files);
            
            System.Diagnostics.Debug.WriteLine($"�ѱ��� {slzyDltbFiles.Count} ��SLZY_DLTB�ļ�:");
            foreach (var file in slzyDltbFiles)
            {
                System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                    (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
            }
        }

        /// <summary>
        /// ��ȡSLZY_DLTB�ļ��б�ɭ����Դ����ͼ�����ݣ�
        /// </summary>
        public static List<SourceDataFileInfo> GetSLZYDLTBFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {slzyDltbFiles.Count} ��SLZY_DLTB�ļ�");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<SourceDataFileInfo>(slzyDltbFiles);
        }

        /// <summary>
        /// ������л��������
        /// </summary>
        public static void ClearAllData()
        {
            sourceDataFiles.Clear();
            czkfbjFiles.Clear();
            slzyDltbFiles.Clear();
            System.Diagnostics.Debug.WriteLine("���������SharedDataManager��������");
        }
    }

    /// <summary>
    /// Դ�����ļ���Ϣ��ֱ���������� SLZYZC�����ԭ�� LCXZGX��
    /// </summary>
    public class SourceDataFileInfo
    {
        /// <summary>
        /// �ļ�����·��������GDBҪ���࣬����GDB·����
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// ��ʾ���ƣ�ͨ��Ϊ������������ʶ
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// �Ƿ�ΪGDBҪ���ࣨtrueΪGDB��falseΪShapefile��
        /// </summary>
        public bool IsGdb { get; set; }
        
        /// <summary>
        /// GDBҪ�������ƣ�����IsGdb=trueʱ��Ч��
        /// </summary>
        public string FeatureClassName { get; set; }
        
        /// <summary>
        /// ��������
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        /// <summary>
        /// ����Դ��������
        /// </summary>
        public string DataSourceType
        {
            get
            {
                return IsGdb ? "GDBҪ����" : "Shapefile";
            }
        }

        public override string ToString()
        {
            // ȷ����ʾ�������õĸ�ʽ
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
                // ���û����ʾ���ƣ�ʹ���ļ���
                return System.IO.Path.GetFileName(FullPath);
            }
        }
    }

    /// <summary>
    /// �ֲ�ʪ��״������򿪷��߽��ļ���Ϣ���ѷ�������ʹ��SourceDataFileInfo��
    /// </summary>
    [Obsolete("��ʹ��SourceDataFileInfo���")]
    public class LCXZGXFileInfo
    {
        /// <summary>
        /// �ļ�����·��
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// ��ʾ���ƣ�ͨ��Ϊ�ڶ���Ŀ¼��
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// �Ƿ�ΪGDBҪ���ࣨtrueΪGDB��falseΪShapefile��
        /// </summary>
        public bool IsGdb { get; set; }
        
        /// <summary>
        /// GDBҪ�������ƣ�����IsGdb=trueʱ��Ч��
        /// </summary>
        public string FeatureClassName { get; set; }
        
        /// <summary>
        /// ��������
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        public override string ToString()
        {
            // ȷ����ʾ�������õĸ�ʽ
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
                // ���û����ʾ���ƣ�ʹ���ļ���
                return System.IO.Path.GetFileName(FullPath);
            }
        }
    }
}