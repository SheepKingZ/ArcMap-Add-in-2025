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
        // ����˽���ֶδ洢·��
        private string forestSurveyDataPath = "";
        private string urbanBoundaryDataPath = "";
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

            // ��ʼ����������Դ·��״̬
            if (!string.IsNullOrEmpty(forestSurveyDataPath))
            {
                txtForestSurveyDataPath.Text = forestSurveyDataPath;
                txtForestSurveyDataPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtForestSurveyDataPath.Text = "��ѡ���ֲ�ʪ���ղ�����Դ�ļ���";
                txtForestSurveyDataPath.ForeColor = Color.Gray;
            }

            if (!string.IsNullOrEmpty(urbanBoundaryDataPath))
            {
                txtUrbanBoundaryDataPath.Text = urbanBoundaryDataPath;
                txtUrbanBoundaryDataPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtUrbanBoundaryDataPath.Text = "��ѡ����򿪷��߽�����Դ�ļ���";
                txtUrbanBoundaryDataPath.ForeColor = Color.Gray;
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
            bool hasForestSurveyData = !string.IsNullOrEmpty(forestSurveyDataPath);
            bool hasUrbanBoundaryData = !string.IsNullOrEmpty(urbanBoundaryDataPath);
            bool hasOutputGDB = !string.IsNullOrEmpty(outputGDBPath);

            // ��������Դѡ��ťʼ������
            btnBrowseForestSurveyData.Enabled = true;
            btnBrowseUrbanBoundaryData.Enabled = true;
            btnBrowseOutputGDB.Enabled = true;

            // OK��ť��Ҫ���б�Ҫ��Ϣ����ɺ������
            bool allDataSourcesSelected = hasWorkspace && hasForestSurveyData && hasUrbanBoundaryData && hasOutputGDB;
            
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

        // �����¼����������ֲ�ʪ���ղ�����Դ���
        private void BtnBrowseForestSurveyData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ���ֲ�ʪ���ղ�����Դ�ļ���";
                if (!string.IsNullOrEmpty(forestSurveyDataPath))
                {
                    dialog.SelectedPath = forestSurveyDataPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    forestSurveyDataPath = dialog.SelectedPath;
                    txtForestSurveyDataPath.Text = forestSurveyDataPath;
                    txtForestSurveyDataPath.ForeColor = Color.DarkGreen;
                    
                    // ���Ұ���LCXZGX_P���ļ�
                    List<ForestResourcePlugin.LCXZGXFileInfo> lcxzgxFiles = FindFilesWithPattern(forestSurveyDataPath, "LCXZGX_P");
                    
                    // ���浽�������ݹ�����
                    ForestResourcePlugin.SharedDataManager.SetLCXZGXFiles(lcxzgxFiles);
                    
                    if (lcxzgxFiles.Count > 0)
                    {
                        MessageBox.Show($"���ҵ� {lcxzgxFiles.Count} ������LCXZGX_P���ļ���", "�ļ��������", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                    MessageBox.Show("�ֲ�ʪ���ղ�����Դѡ����ɡ�", "�ɹ�", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateButtonStates();
                }
            }
        }

        // �����¼������������򿪷��߽�����Դ���
        private void BtnBrowseUrbanBoundaryData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ����򿪷��߽�����Դ�ļ���";
                if (!string.IsNullOrEmpty(urbanBoundaryDataPath))
                {
                    dialog.SelectedPath = urbanBoundaryDataPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    urbanBoundaryDataPath = dialog.SelectedPath;
                    txtUrbanBoundaryDataPath.Text = urbanBoundaryDataPath;
                    txtUrbanBoundaryDataPath.ForeColor = Color.DarkGreen;
                    
                    // ���Ұ���CZKFBJ���ļ�
                    List<ForestResourcePlugin.LCXZGXFileInfo> czkfbjFiles = FindFilesWithPattern(urbanBoundaryDataPath, "CZKFBJ");
                    
                    // ���浽�������ݹ�����
                    ForestResourcePlugin.SharedDataManager.SetCZKFBJFiles(czkfbjFiles);
                    
                    if (czkfbjFiles.Count > 0)
                    {
                        MessageBox.Show($"���ҵ� {czkfbjFiles.Count} ������CZKFBJ���ļ���", "�ļ��������", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                    MessageBox.Show("���򿪷��߽�����Դѡ����ɡ�", "�ɹ�", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateButtonStates();
                }
            }
        }

        // �����¼������������GDB·�����
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

            if (string.IsNullOrEmpty(forestSurveyDataPath))
            {
                MessageBox.Show("��ѡ���ֲ�ʪ���ղ�����Դ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(urbanBoundaryDataPath))
            {
                MessageBox.Show("��ѡ����򿪷��߽�����Դ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                              $"�ֲ�ʪ���ղ�����Դ��{forestSurveyDataPath}\n" +
                              $"���򿪷��߽�����Դ��{urbanBoundaryDataPath}\n" +
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

        // �������ԣ����ⲿ����ѡ���·��
        public string ForestSurveyDataPath => forestSurveyDataPath;
        public string UrbanBoundaryDataPath => urbanBoundaryDataPath;
        public string OutputGDBPath => outputGDBPath;
        public bool CreateCountyFolders => chkCreateCountyFolders.Checked;

        private void BasicDataPreparationForm_Load(object sender, EventArgs e)
        {

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
                            // ��ȡ�ڶ����ļ���������Ϊ��ʾ����
                            string displayName = ExtractSecondLevelFolderName(filePath, rootDir);
                            
                            result.Add(new ForestResourcePlugin.LCXZGXFileInfo 
                            { 
                                FullPath = filePath,
                                DisplayName = displayName,
                                IsGdb = false,
                                GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon // ����Ϊ��
                            });
                            
                            matchCount++;
                            System.Diagnostics.Debug.WriteLine($"�ҵ�ƥ���Shapefile�ļ�[{matchCount}]: {filePath}, ��ʾ����: {displayName}");
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
        private string ExtractSecondLevelFolderName(string filePath, string rootDir)
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
                    // ·����ƥ�䣬ʹ������·�����ļ���
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                }
                
                // �ָ�·������ȡ�ڶ���Ŀ¼����
                string[] pathParts = relativePath.Split('\\', '/');
                if (pathParts.Length >= 2)
                {
                    return pathParts[1]; // �ڶ���Ŀ¼
                }
                else if (pathParts.Length == 1)
                {
                    // ���û�еڶ���Ŀ¼�������ļ�����������չ����
                    return System.IO.Path.GetFileNameWithoutExtension(filePath);
                }
                else
                {
                    // ���׷���
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡ�ļ�������ʱ����: {ex.Message}");
                return System.IO.Path.GetFileNameWithoutExtension(filePath);
            }
        }
    }
}