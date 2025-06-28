using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // ����CountySelectionForm
using System.IO;

namespace TestArcMapAddin2.Forms
{
    public partial class BasicDataPreparationForm : Form
    {
        // �޸�˽���ֶ� - �ϲ��ֲ�ʪ���ղ����ݺͳ��򿪷��߽�����·��Ϊһ��
        private string dataSourcePath = "";
        private string outputGDBPath = "";

        public BasicDataPreparationForm()
        {
            InitializeComponent();
            InitializeFormState();
        }

        private void InitializeFormState()
        {
            // ���ݵ�ǰ����״̬��Ĭ��ֵ��ʼ����ǩ
            lblWorkspace.Text = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? "δѡ�����ռ�" : SharedWorkflowState.WorkspacePath;
            lblWorkspace.ForeColor = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? Color.Black : Color.DarkGreen;

            // ��ʼ���ϲ��������Դ·��״̬
            if (!string.IsNullOrEmpty(dataSourcePath))
            {
                txtDataPath.Text = dataSourcePath;
                txtDataPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtDataPath.Text = "��ѡ������ֲ�ʪ���ղ�����򿪷��߽����ݵ��ļ���";
                txtDataPath.ForeColor = Color.Gray;
            }

            if (!string.IsNullOrEmpty(outputGDBPath))
            {
                txtOutputGDBPath.Text = outputGDBPath;
                txtOutputGDBPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtOutputGDBPath.Text = "��ѡ��������GDB·��";
                txtOutputGDBPath.ForeColor = Color.Gray;
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasWorkspace = !string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath);
            bool hasDataSource = !string.IsNullOrEmpty(dataSourcePath);
            bool hasOutputGDB = !string.IsNullOrEmpty(outputGDBPath);

            // ��������Դѡ��ťʼ������
            btnBrowseData.Enabled = true;
            btnBrowseOutputGDB.Enabled = true;

            // OK��ť��Ҫ���б�Ҫ��Ϣ����ɺ������
            bool allDataSourcesSelected = hasWorkspace && hasDataSource && hasOutputGDB;
            
            btnOK.Enabled = allDataSourcesSelected;
        }

        private void BtnSelectWorkspace_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��Ŀ�����ݿ�·����File Geodatabase .gdb��";
                if (!string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
                {
                    dialog.SelectedPath = SharedWorkflowState.WorkspacePath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SelectedPath.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase) || System.IO.Directory.Exists(dialog.SelectedPath))
                    {
                        SharedWorkflowState.WorkspacePath = dialog.SelectedPath;
                        lblWorkspace.Text = SharedWorkflowState.WorkspacePath;
                        lblWorkspace.ForeColor = Color.DarkGreen;
                        InitializeFormState(); // ���³�ʼ���Է�ӳ״̬�仯
                    }
                    else
                    {
                        MessageBox.Show("��ѡ����Ч��File Geodatabase (.gdb) ·����", "����",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            UpdateButtonStates();
        }

        // �µĺϲ��������Դ�������
        private void BtnBrowseData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ������ֲ�ʪ���ղ�����򿪷��߽����ݵ��ļ���";
                if (!string.IsNullOrEmpty(dataSourcePath))
                {
                    dialog.SelectedPath = dataSourcePath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    dataSourcePath = dialog.SelectedPath;
                    txtDataPath.Text = dataSourcePath;
                    txtDataPath.ForeColor = Color.DarkGreen;
                    
                    // ���浽����״̬��
                    SharedWorkflowState.DataSourcePath = dataSourcePath;
                    
                    // ���Ұ���LCXZGX_P���ļ����ֲ�ʪ���ղ����ݣ�
                    List<ForestResourcePlugin.LCXZGXFileInfo> lcxzgxFiles = FindFilesWithPattern(dataSourcePath, "LCXZGX_P");
                    
                    // ���浽�������ݹ�����
                    ForestResourcePlugin.SharedDataManager.SetLCXZGXFiles(lcxzgxFiles);
                    
                    // ���Ұ���CZKFBJ���ļ������򿪷��߽����ݣ�
                    List<ForestResourcePlugin.LCXZGXFileInfo> czkfbjFiles = FindFilesWithPattern(dataSourcePath, "CZKFBJ");
                    
                    // ���浽�������ݹ�����
                    ForestResourcePlugin.SharedDataManager.SetCZKFBJFiles(czkfbjFiles);
                    
                    // ��ʾ�ļ��������
                    int totalFiles = lcxzgxFiles.Count + czkfbjFiles.Count;
                    if (totalFiles > 0)
                    {
                        MessageBox.Show($"��ͬһ�ļ������ҵ���\n- {lcxzgxFiles.Count} ���ֲ�ʪ���ղ������ļ�\n- {czkfbjFiles.Count} �����򿪷��߽������ļ�", 
                            "�ļ��������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("δ�ҵ���������ļ�����ȷ��ѡ����ļ����Ƿ���ȷ��", "��ʾ", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                    UpdateButtonStates();
                }
            }
        }

        // ���GDB·������������ֲ���
        private void BtnBrowseOutputGDB_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��������GDB·��";
                if (!string.IsNullOrEmpty(outputGDBPath))
                {
                    dialog.SelectedPath = outputGDBPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputGDBPath = dialog.SelectedPath;
                    txtOutputGDBPath.Text = outputGDBPath;
                    txtOutputGDBPath.ForeColor = Color.DarkGreen;
                    
                    // ���浽����״̬
                    SharedWorkflowState.OutputGDBPath = outputGDBPath;
                    
                    MessageBox.Show("������GDB·��ѡ����ɡ�", "�ɹ�", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateButtonStates();
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // ��֤���б����·������ѡ��
            if (string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
            {
                MessageBox.Show("��ѡ�����ռ䡣", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(dataSourcePath))
            {
                MessageBox.Show("��ѡ������Դ�ļ��С�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(outputGDBPath))
            {
                MessageBox.Show("��ѡ��������GDB·����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ����������ʵ�־����ҵ���߼������翪ʼ��������
            if (chkCreateCountyFolders.Checked)
            {
                MessageBox.Show($"׼��Ϊÿ���ش����ļ��в����ɽ�����\n" +
                              $"�����ռ䣺{SharedWorkflowState.WorkspacePath}\n" +
                              $"����Դ�ļ��У�{dataSourcePath}\n" +
                              $"���GDB·����{outputGDBPath}", 
                              "����ȷ��", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ���¹������ԣ����ⲿ����ѡ���·��
        public string DataSourcePath => dataSourcePath;
        public string OutputGDBPath => outputGDBPath;
        public bool CreateCountyFolders => chkCreateCountyFolders.Checked;

        private void BasicDataPreparationForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ��ȡ·���еĵ�һ���ļ������ƣ�������
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <param name="rootDir">��Ŀ¼</param>
        /// <returns>��һ���ļ������ƣ�������</returns>
        private string ExtractCountyNameFromPath(string filePath, string rootDir)
        {
            try
            {
                // �淶��·��
                string normalizedRoot = System.IO.Path.GetFullPath(rootDir).TrimEnd('\\', '/');
                string normalizedFile = System.IO.Path.GetFullPath(filePath);
                
                // �������·��
                string relativePath = "";
                if (normalizedFile.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = normalizedFile.Substring(normalizedRoot.Length).TrimStart('\\', '/');
                }
                else
                {
                    // ·����ƥ�䣬���Դ�����·����ȡ
                    System.Diagnostics.Debug.WriteLine($"����: �ļ�·�� {normalizedFile} ���ڸ�Ŀ¼ {normalizedRoot} ��");
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                }
                
                // �ָ�·������ȡ��һ��Ŀ¼���ƣ�������
                string[] pathParts = relativePath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 1)
                {
                    // ���ص�һ��Ŀ¼���ƣ���Ӧ��������
                    string countyName = pathParts[0];
                    System.Diagnostics.Debug.WriteLine($"��·�� {relativePath} ��ȡ����: {countyName}");
                    return countyName;
                }
                else
                {
                    // ���׷��������·������ʧ�ܣ�ʹ���ļ���
                    string fallbackName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    System.Diagnostics.Debug.WriteLine($"����: �޷���·����ȡ������ʹ���ļ���: {fallbackName}");
                    return fallbackName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡ����ʱ����: {ex.Message}");
                return System.IO.Path.GetFileNameWithoutExtension(filePath);
            }
        }

        /// <summary>
        /// ����ָ��Ŀ¼�����ư����ض��ַ������ļ�
        /// </summary>
        /// <param name="rootDir">��Ŀ¼</param>
        /// <param name="pattern">����ƥ��ģʽ</param>
        /// <returns>�ļ���Ϣ�б�</returns>
        private List<ForestResourcePlugin.LCXZGXFileInfo> FindFilesWithPattern(string rootDir, string pattern)
        {
            var result = new List<ForestResourcePlugin.LCXZGXFileInfo>();
            
            try
            {
                System.Diagnostics.Debug.WriteLine($"��ʼ�� {rootDir} Ŀ¼�²��Ұ��� {pattern} ���ļ�");
                
                // 1. ���Ȳ���GDBҪ����
                System.Diagnostics.Debug.WriteLine("��1��������GDBҪ����...");
                var gdbFeatureClasses = ForestResourcePlugin.GdbFeatureClassFinder.FindFeatureClassesWithPattern(
                    rootDir, pattern, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
                
                // ���ҵ���GDBҪ������ӵ������
                result.AddRange(gdbFeatureClasses);
                System.Diagnostics.Debug.WriteLine($"�ҵ� {gdbFeatureClasses.Count} ��GDBҪ����");
                
                // 2. �ٲ���Shapefile�ļ�
                System.Diagnostics.Debug.WriteLine("��2��������Shapefile�ļ�...");
                
                // ȷ��Ŀ¼����
                if (Directory.Exists(rootDir))
                {
                    string[] files = System.IO.Directory.GetFiles(rootDir, "*.shp", System.IO.SearchOption.AllDirectories);
                    System.Diagnostics.Debug.WriteLine($"�� {rootDir} Ŀ¼���ҵ� {files.Length} ��Shapefile�ļ�");
                    
                    // ɸѡ����ָ��ģʽ��Shapefile
                    int matchCount = 0;
                    foreach (string filePath in files)
                    {
                        if (filePath.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // ��ȡ��������һ���ļ������ƣ�
                            string countyName = ExtractCountyNameFromPath(filePath, rootDir);
                            
                            result.Add(new ForestResourcePlugin.LCXZGXFileInfo 
                            { 
                                FullPath = filePath,
                                DisplayName = countyName,
                                IsGdb = false,
                                GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon // ����Ϊ��
                            });
                            
                            matchCount++;
                            System.Diagnostics.Debug.WriteLine($"�ҵ�ƥ���Shapefile�ļ�[{matchCount}]: {filePath}, ����: {countyName}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"����: Ŀ¼ {rootDir} ������");
                }
                
                System.Diagnostics.Debug.WriteLine($"���ҵ� {result.Count} ��ƥ���ļ� (GDBҪ����: {gdbFeatureClasses.Count}, Shapefile: {result.Count - gdbFeatureClasses.Count})");
                
                // �����ϸ�Ľ����Ϣ
                for (int i = 0; i < result.Count; i++)
                {
                    var item = result[i];
                    System.Diagnostics.Debug.WriteLine($"���[{i+1}]: {item.DisplayName}, ·��: {item.FullPath}, ����: {(item.IsGdb ? "GDBҪ����" : "Shapefile")}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"�����ļ�ʱ����: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"�쳣����: {ex}");
                MessageBox.Show($"�����ļ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return result;
        }

        /// <summary>
        /// ��ȡ·���еĵڶ����ļ�������
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <param name="rootDir">��Ŀ¼</param>
        /// <returns>�ڶ����ļ�������</returns>
        [Obsolete("�˷����ѹ�ʱ����ʹ�� ExtractCountyNameFromPath ����")]
        private string ExtractSecondLevelFolderName(string filePath, string rootDir)
        {
            // ����ԭ������ά���������ԣ������Ϊ��ʱ
            // ���ڵ����µ�������ȡ����
            return ExtractCountyNameFromPath(filePath, rootDir);
        }
    }
}