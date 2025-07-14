using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// �����ڲ�ͬ����֮�乲�����ݵĹ�����
    /// </summary>
    public static class SharedDataManager
    {
        // �洢�ҵ���LCXZGX�ļ���Ϣ
        private static List<LCXZGXFileInfo> lcxzgxFiles = new List<LCXZGXFileInfo>();
        
        // �洢�ҵ���CZKFBJ�ļ���Ϣ
        private static List<LCXZGXFileInfo> czkfbjFiles = new List<LCXZGXFileInfo>();

        // �������洢�ҵ���SLZY_DLTB�ļ���Ϣ
        private static List<LCXZGXFileInfo> slzyDltbFiles = new List<LCXZGXFileInfo>();

        /// <summary>
        /// ����LCXZGX�ļ��б�
        /// </summary>
        public static void SetLCXZGXFiles(List<LCXZGXFileInfo> files)
        {
            // ȷ�������null���б�
            if (files == null)
            {
                lcxzgxFiles = new List<LCXZGXFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������null��LCXZGX�ļ��б��ѳ�ʼ��Ϊ���б�");
            }
            else
            {
                lcxzgxFiles = new List<LCXZGXFileInfo>(files); // ���������Ա�����������
                
                // ��¼��ϸ���ļ���Ϣ
                System.Diagnostics.Debug.WriteLine($"�ѱ��� {lcxzgxFiles.Count} ��LCXZGX�ļ���Ϣ:");
                foreach (var file in lcxzgxFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                        (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// ��ȡLCXZGX�ļ��б�
        /// </summary>
        public static List<LCXZGXFileInfo> GetLCXZGXFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {lcxzgxFiles.Count} ��LCXZGX�ļ�");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<LCXZGXFileInfo>(lcxzgxFiles);
        }

        /// <summary>
        /// ����CZKFBJ�ļ��б�
        /// </summary>
        public static void SetCZKFBJFiles(List<LCXZGXFileInfo> files)
        {
            // ȷ�������null���б�
            if (files == null)
            {
                czkfbjFiles = new List<LCXZGXFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������null��CZKFBJ�ļ��б��ѳ�ʼ��Ϊ���б�");
            }
            else
            {
                czkfbjFiles = new List<LCXZGXFileInfo>(files); // ���������Ա�����������
                
                // ��¼��ϸ���ļ���Ϣ
                System.Diagnostics.Debug.WriteLine($"�ѱ��� {czkfbjFiles.Count} ��CZKFBJ�ļ���Ϣ:");
                foreach (var file in czkfbjFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                        (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// ��ȡCZKFBJ�ļ��б�
        /// </summary>
        public static List<LCXZGXFileInfo> GetCZKFBJFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {czkfbjFiles.Count} ��CZKFBJ�ļ�");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<LCXZGXFileInfo>(czkfbjFiles);
        }

        /// <summary>
        /// ����SLZY_DLTB�ļ��б�
        /// </summary>
        public static void SetSLZYDLTBFiles(List<LCXZGXFileInfo> files)
        {
            // ȷ�������null�б�
            if (files == null)
            {
                slzyDltbFiles = new List<LCXZGXFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������null��SLZY_DLTB�ļ��б��ѳ�ʼ��Ϊ���б�");
            }
            else
            {
                slzyDltbFiles = new List<LCXZGXFileInfo>(files); // ���������Ա����ⲿ�޸�
                
                // ��¼��ϸ�ļ���Ϣ
                System.Diagnostics.Debug.WriteLine($"�ѱ��� {slzyDltbFiles.Count} ��SLZY_DLTB�ļ���Ϣ:");
                foreach (var file in slzyDltbFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" + 
                        (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// ��ȡSLZY_DLTB�ļ��б�
        /// </summary>
        public static List<LCXZGXFileInfo> GetSLZYDLTBFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {slzyDltbFiles.Count} ��SLZY_DLTB�ļ�");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<LCXZGXFileInfo>(slzyDltbFiles);
        }

        /// <summary>
        /// �����������
        /// </summary>
        public static void ClearAll()
        {
            lcxzgxFiles.Clear();
            czkfbjFiles.Clear();
            slzyDltbFiles.Clear();
            System.Diagnostics.Debug.WriteLine("��������й�������");
        }
    }

    /// <summary>
    /// �ֲ���״����򿪷��߽��ļ���Ϣ
    /// </summary>
    public class LCXZGXFileInfo
    {
        /// <summary>
        /// �ļ�����·��
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// ��ʾ���ƣ�ͨ��Ϊ�ڶ���Ŀ¼����
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
            // ȷ����ʾ���Ѻõĸ�ʽ
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