using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ForestResourcePlugin
{
    /// <summary>
    /// �����ڲ�ͬ����֮�乲�����ݵĹ�����
    /// Դ�����ļ���CZKFBJ�ļ���SLZY_DLTB�ļ���CYZY_DLTB�ļ���SDZY_DLTB�ļ�
    /// </summary>
    public static class SharedDataManager
    {
        // �洢�ҵ���Դ�����ļ���Ϣ��ֱ������ԭʼ SLZYZC��
        private static List<SourceDataFileInfo> sourceDataFiles = new List<SourceDataFileInfo>();

        // �洢�ҵ���CZKFBJ�ļ���Ϣ
        private static List<SourceDataFileInfo> czkfbjFiles = new List<SourceDataFileInfo>();

        // �洢�ҵ���SLZY_DLTB�ļ���Ϣ
        private static List<SourceDataFileInfo> slzyDltbFiles = new List<SourceDataFileInfo>();

        // �������洢�ҵ���CYZY_DLTB�ļ���Ϣ���ݵ���Դ����ͼ�����ݣ�
        private static List<SourceDataFileInfo> cyzyDltbFiles = new List<SourceDataFileInfo>();

        // �������洢�ҵ���SDZY_DLTB�ļ���Ϣ��ʪ����Դ����ͼ�����ݣ�
        private static List<SourceDataFileInfo> sdzyDltbFiles = new List<SourceDataFileInfo>();

        // ��������ѡ��״̬
        private static bool isForestSelected = true;
        private static bool isGrasslandSelected = false;
        private static bool isWetlandSelected = false;  // ������ʪ��ѡ��״̬

        /// <summary>
        /// ������������ѡ��״̬
        /// </summary>
        public static void SetDataTypeSelection(bool forest, bool grassland, bool wetland)
        {
            isForestSelected = forest;
            isGrasslandSelected = grassland;
            isWetlandSelected = wetland;
            System.Diagnostics.Debug.WriteLine($"��������ѡ��״̬�Ѹ��� - �ֵ�: {forest}, �ݵ�: {grassland}, ʪ��: {wetland}");
        }

        /// <summary>
        /// ��ȡ��������ѡ��״̬
        /// </summary>
        public static (bool Forest, bool Grassland, bool Wetland) GetDataTypeSelection()
        {
            return (isForestSelected, isGrasslandSelected, isWetlandSelected);
        }

        /// <summary>
        /// ����Դ�����ļ��б�ֱ������ԭʼ SLZYZC��
        /// </summary>
        public static void SetSourceDataFiles(List<SourceDataFileInfo> files)
        {
            // ȷ������null�б�
            if (files == null)
            {
                sourceDataFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������nullԴ�����ļ��б��ѳ�ʼ��Ϊ���б�");
            }
            else
            {
                sourceDataFiles = new List<SourceDataFileInfo>(files); // ���������Ա����ⲿ�޸�

                // ��¼��ϸ�ļ���Ϣ
                System.Diagnostics.Debug.WriteLine($"�ѱ��� {sourceDataFiles.Count} ��Դ�����ļ���ֱ������ԭʼ SLZYZC��:");
                foreach (var file in sourceDataFiles)
                {
                    System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" +
                        (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
                }
            }
        }

        /// <summary>
        /// ��ȡԴ�����ļ��б�ֱ������ԭʼ SLZYZC��
        /// </summary>
        public static List<SourceDataFileInfo> GetSourceDataFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {sourceDataFiles.Count} ��Դ�����ļ���ֱ������ԭʼ SLZYZC��");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<SourceDataFileInfo>(sourceDataFiles);
        }

        // �ֵ�HSJG�ļ�����
        private static List<SourceDataFileInfo> ldhsjgFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// ����LDHSJG�ļ��б��ֵغ���۸�
        /// </summary>
        /// <param name="files">LDHSJG�ļ��б�</param>
        public static void SetLDHSJGFiles(List<SourceDataFileInfo> files)
        {
            ldhsjgFiles = files ?? new List<SourceDataFileInfo>();
            System.Diagnostics.Debug.WriteLine($"SharedDataManager: �ѱ��� {ldhsjgFiles.Count} ��LDHSJG�ļ�");
        }

        /// <summary>
        /// ��ȡLDHSJG�ļ��б��ֵغ���۸�
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

        // �ݵ�HSJG�ļ�����
        private static List<SourceDataFileInfo> cdhsjgFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// ����CDHSJG�ļ��б��ݵغ���۸�
        /// </summary>
        /// <param name="files">CDHSJG�ļ��б�</param>
        public static void SetCDHSJGFiles(List<SourceDataFileInfo> files)
        {
            cdhsjgFiles = files ?? new List<SourceDataFileInfo>();
            System.Diagnostics.Debug.WriteLine($"SharedDataManager: �ѱ��� {cdhsjgFiles.Count} ��CDHSJG�ļ�");
        }

        /// <summary>
        /// ��ȡCDHSJG�ļ��б��ݵغ���۸�
        /// </summary>
        /// <returns>CDHSJG�ļ��б�</returns>
        public static List<SourceDataFileInfo> GetCDHSJGFiles()
        {
            return new List<SourceDataFileInfo>(cdhsjgFiles);
        }

        /// <summary>
        /// ���CDHSJG�ļ�
        /// </summary>
        /// <param name="file">Ҫ��ӵ�CDHSJG�ļ�</param>
        public static void AddCDHSJGFile(SourceDataFileInfo file)
        {
            if (file != null)
            {
                cdhsjgFiles.Add(file);
                System.Diagnostics.Debug.WriteLine($"SharedDataManager: �����CDHSJG�ļ� {file.DisplayName}");
            }
        }

        /// <summary>
        /// ���CDHSJG�ļ��б�
        /// </summary>
        public static void ClearCDHSJGFiles()
        {
            cdhsjgFiles.Clear();
            System.Diagnostics.Debug.WriteLine("SharedDataManager: �����CDHSJG�ļ��б�");
        }

        // ������ʪ��HSJG�ļ�����
        private static List<SourceDataFileInfo> sdhsjgFiles = new List<SourceDataFileInfo>();

        /// <summary>
        /// ����SDHSJG�ļ��б�ʪ�غ���۸�
        /// </summary>
        /// <param name="files">SDHSJG�ļ��б�</param>
        public static void SetSDHSJGFiles(List<SourceDataFileInfo> files)
        {
            sdhsjgFiles = files ?? new List<SourceDataFileInfo>();
            System.Diagnostics.Debug.WriteLine($"SharedDataManager: �ѱ��� {sdhsjgFiles.Count} ��SDHSJG�ļ�");
        }

        /// <summary>
        /// ��ȡSDHSJG�ļ��б�ʪ�غ���۸�
        /// </summary>
        /// <returns>SDHSJG�ļ��б�</returns>
        public static List<SourceDataFileInfo> GetSDHSJGFiles()
        {
            return new List<SourceDataFileInfo>(sdhsjgFiles);
        }

        /// <summary>
        /// ���SDHSJG�ļ�
        /// </summary>
        /// <param name="file">Ҫ��ӵ�SDHSJG�ļ�</param>
        public static void AddSDHSJGFile(SourceDataFileInfo file)
        {
            if (file != null)
            {
                sdhsjgFiles.Add(file);
                System.Diagnostics.Debug.WriteLine($"SharedDataManager: �����SDHSJG�ļ� {file.DisplayName}");
            }
        }

        /// <summary>
        /// ���SDHSJG�ļ��б�
        /// </summary>
        public static void ClearSDHSJGFiles()
        {
            sdhsjgFiles.Clear();
            System.Diagnostics.Debug.WriteLine("SharedDataManager: �����SDHSJG�ļ��б�");
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
        /// ����CYZY_DLTB�ļ��б��ݵ���Դ����ͼ�����ݣ�
        /// </summary>
        public static void SetCYZYDLTBFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                cyzyDltbFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������null��CYZY_DLTB�ļ��б��ѳ�ʼ��Ϊ���б�");
                return;
            }

            // ֱ�ӱ��棬����Ҫת��
            cyzyDltbFiles = new List<SourceDataFileInfo>(files);

            System.Diagnostics.Debug.WriteLine($"�ѱ��� {cyzyDltbFiles.Count} ��CYZY_DLTB�ļ�:");
            foreach (var file in cyzyDltbFiles)
            {
                System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" +
                    (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
            }
        }

        /// <summary>
        /// ��ȡCYZY_DLTB�ļ��б��ݵ���Դ����ͼ�����ݣ�
        /// </summary>
        public static List<SourceDataFileInfo> GetCYZYDLTBFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {cyzyDltbFiles.Count} ��CYZY_DLTB�ļ�");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<SourceDataFileInfo>(cyzyDltbFiles);
        }

        /// <summary>
        /// ����SDZY_DLTB�ļ��б�ʪ����Դ����ͼ�����ݣ�
        /// </summary>
        public static void SetSDZYDLTBFiles(List<SourceDataFileInfo> files)
        {
            if (files == null)
            {
                sdzyDltbFiles = new List<SourceDataFileInfo>();
                System.Diagnostics.Debug.WriteLine("���棺������null��SDZY_DLTB�ļ��б��ѳ�ʼ��Ϊ���б�");
                return;
            }

            // ֱ�ӱ��棬����Ҫת��
            sdzyDltbFiles = new List<SourceDataFileInfo>(files);

            System.Diagnostics.Debug.WriteLine($"�ѱ��� {sdzyDltbFiles.Count} ��SDZY_DLTB�ļ�:");
            foreach (var file in sdzyDltbFiles)
            {
                System.Diagnostics.Debug.WriteLine($" - {file.DisplayName}: {file.FullPath}" +
                    (file.IsGdb ? $" (GDBҪ����: {file.FeatureClassName})" : " (Shapefile)"));
            }
        }

        /// <summary>
        /// ��ȡSDZY_DLTB�ļ��б�ʪ����Դ����ͼ�����ݣ�
        /// </summary>
        public static List<SourceDataFileInfo> GetSDZYDLTBFiles()
        {
            System.Diagnostics.Debug.WriteLine($"��ȡ {sdzyDltbFiles.Count} ��SDZY_DLTB�ļ�");
            // ���ظ����Ա����ⲿ�޸��ڲ��б�
            return new List<SourceDataFileInfo>(sdzyDltbFiles);
        }

        /// <summary>
        /// ��ջ�������������
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
            System.Diagnostics.Debug.WriteLine("���������SharedDataManager�е�����");
        }
    }

    /// <summary>
    /// Դ�����ļ���Ϣ��ֱ������ԭʼ SLZYZC�����ԭ�� LCXZGX��
    /// </summary>
    public class SourceDataFileInfo
    {
        /// <summary>
        /// �ļ�����·���������GDBҪ���࣬������GDB·����
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// ��ʾ���ƣ�ͨ��Ϊ�ڶ���Ŀ¼��ʶ
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// �Ƿ�ΪGDBҪ���ࣨtrueΪGDB��falseΪShapefile��
        /// </summary>
        public bool IsGdb { get; set; }

        /// <summary>
        /// GDBҪ�������ƣ�����IsGdb=trueʱ��Ч
        /// </summary>
        public string FeatureClassName { get; set; }

        /// <summary>
        /// �����أ���λ���룩
        /// </summary>
        public string CountyCode { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        /// <summary>
        /// ����Դ�����ı�
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
            // ȷ����ʾ���Ʋ��ú��ʵĸ�ʽ
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
    /// �ֲ�ʪ��״�����ݻ���򿪷��߽��ļ���Ϣ���ѱ����ã���ʹ��SourceDataFileInfo����
    /// </summary>
    [Obsolete("��ʹ��SourceDataFileInfo����")]
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
        /// GDBҪ�������ƣ�����IsGdb=trueʱ��Ч
        /// </summary>
        public string FeatureClassName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public esriGeometryType GeometryType { get; set; }

        public override string ToString()
        {
            // ȷ����ʾ���Ʋ��ú��ʵĸ�ʽ
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