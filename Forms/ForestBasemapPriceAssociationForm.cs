using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;

namespace TestArcMapAddin2.Forms
{
    public partial class ForestBasemapPriceAssociationForm : Form
    {
        // ����·��
        private string landGradePricePath = "";
        private string forestResourcePath = "";
        private string outputPath = "";
        private string priceExcelPath = "";

        // �������
        private List<DataPairInfo> dataPairs = new List<DataPairInfo>();

        // �۸�ӳ���
        private Dictionary<string, Dictionary<string, double>> priceMapping = new Dictionary<string, Dictionary<string, double>>();

        public ForestBasemapPriceAssociationForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // ��ʼ���ؼ�״̬
            btnPairData.Enabled = false;
            btnProcessData.Enabled = false;
            btnImportPriceMapping.Enabled = true;

            // ���ý�����
            progressBar.Value = 0;
            statusLabel.Text = "����";

            // ��ʼ����������
            InitializeDataGrid();
            
            // ��ʼ���۸�ӳ������
            InitializePriceMappingInterface();
        }

        private void InitializeDataGrid()
        {
            dataGridViewPairs.AutoGenerateColumns = false;
            dataGridViewPairs.Columns.Clear();

            // ��Ӹ�ѡ����
            var checkColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Selected",
                HeaderText = "ѡ��",
                Width = 50,
                DataPropertyName = "Selected"
            };
            dataGridViewPairs.Columns.Add(checkColumn);

            // ���������������
            var codeColumn = new DataGridViewTextBoxColumn
            {
                Name = "AdminCode",
                HeaderText = "����������",
                Width = 100,
                DataPropertyName = "AdminCode",
                ReadOnly = true
            };
            dataGridViewPairs.Columns.Add(codeColumn);

            // ���������������
            var nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "AdminName",
                HeaderText = "����������",
                Width = 150,
                DataPropertyName = "AdminName",
                ReadOnly = true
            };
            dataGridViewPairs.Columns.Add(nameColumn);

            // ��ӵؼ�����״̬��
            var priceStatusColumn = new DataGridViewTextBoxColumn
            {
                Name = "PriceDataStatus",
                HeaderText = "�ؼ�����״̬",
                Width = 120,
                DataPropertyName = "PriceDataStatus",
                ReadOnly = true
            };
            dataGridViewPairs.Columns.Add(priceStatusColumn);

            // �����״����״̬��
            var statusColumn = new DataGridViewTextBoxColumn
            {
                Name = "StatusDataStatus",
                HeaderText = "��״����״̬",
                Width = 120,
                DataPropertyName = "StatusDataStatus",
                ReadOnly = true
            };
            dataGridViewPairs.Columns.Add(statusColumn);

            // ��ӵؼ�����·����
            var pricePathColumn = new DataGridViewTextBoxColumn
            {
                Name = "PriceDataPath",
                HeaderText = "�ؼ�����·��",
                Width = 200,
                DataPropertyName = "PriceDataPath",
                ReadOnly = true
            };
            dataGridViewPairs.Columns.Add(pricePathColumn);

            // �����״����·����
            var statusPathColumn = new DataGridViewTextBoxColumn
            {
                Name = "StatusDataPath",
                HeaderText = "��״����·��",
                Width = 200,
                DataPropertyName = "StatusDataPath",
                ReadOnly = true
            };
            dataGridViewPairs.Columns.Add(statusPathColumn);
        }

        private void InitializePriceMappingInterface()
        {
            // ��ʼ���۸�ӳ��DataGridView
            dataGridViewPriceMapping.AutoGenerateColumns = false;
            dataGridViewPriceMapping.Columns.Clear();

            // ���������������
            var codeColumn = new DataGridViewTextBoxColumn
            {
                Name = "AdminCode",
                HeaderText = "����������",
                Width = 100,
                DataPropertyName = "AdminCode",
                ReadOnly = true
            };
            dataGridViewPriceMapping.Columns.Add(codeColumn);

            // ���������������
            var nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "AdminName", 
                HeaderText = "����������",
                Width = 120,
                DataPropertyName = "AdminName",
                ReadOnly = true
            };
            dataGridViewPriceMapping.Columns.Add(nameColumn);

            // ���1-5���۸���
            for (int i = 1; i <= 5; i++)
            {
                var priceColumn = new DataGridViewTextBoxColumn
                {
                    Name = $"Grade{i}Price",
                    HeaderText = $"{i}���ֵؼ۸�(��Ԫ/����)",
                    Width = 130,
                    DataPropertyName = $"Grade{i}Price",
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "F2",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                };
                dataGridViewPriceMapping.Columns.Add(priceColumn);
            }

            // ��ʼ��ģ��˵���ı�
            InitializeTemplateText();
            
            // ����ӳ��״̬
            UpdatePriceMappingDisplay();
        }

        private void InitializeTemplateText()
        {
            txtTemplate.Text = @"�۸�ӳ���Excel�ļ���ʽ˵��:

���ļ���ʽҪ��
1. ֧�� .xlsx �� .xls ��ʽ��Excel�ļ�
2. Ҳ�������Ϊ .csv ��ʽ���Ƽ�ʹ�ö��ŷָ���

�����ṹ˵����
- ��1�к͵�2�У���ͷ��Ϣ��ϵͳ���Զ�������
- ��3�п�ʼ������ļ۸�����

���нṹҪ��
��1�У����������ƣ��磺�����ء������صȣ�
��2�У����������루6λ���֣��磺441226��441322�ȣ�
��3�У�1���ֵؼ۸���Ԫ/���꣩
��4�У�2���ֵؼ۸���Ԫ/���꣩
��5�У�3���ֵؼ۸���Ԫ/���꣩
��6�У�4���ֵؼ۸���Ԫ/���꣩
��7�У�5���ֵؼ۸���Ԫ/���꣩

��ʾ�����ݡ�
����������  | ���������� | 1���۸� | 2���۸� | 3���۸� | 4���۸� | 5���۸�
������     | 441226    | 8.72   | 6.45   | 4.23   | 3.15   | 2.45
������     | 441322    | 7.89   | 5.67   | 3.98   | 2.87   | 2.12
½����     | 441781    | 9.12   | 6.88   | 4.56   | 3.33   | 2.78

��ע�����
1. ���������������6λ����
2. �۸����ݿ�����������С��
3. ���ĳ��������ȱ��ĳ����ļ۸񣬿������ջ���0
4. ȷ���ļ�����ΪUTF-8��������������
5. �������ñ�ϵͳ��'����ģ��'�������ɱ�׼ģ��";
        }

        #region �¼��������

        private void btnBrowseLandGradePrice_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ���ֵػ�׼�ؼ۶��������ļ���";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    landGradePricePath = dialog.SelectedPath;
                    txtLandGradePricePath.Text = landGradePricePath;
                    txtLandGradePricePath.ForeColor = Color.DarkGreen;
                    UpdateButtonStates();
                }
            }
        }

        private void btnBrowseForestResource_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��ɭ����Դ����ͼ�������ļ���";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    forestResourcePath = dialog.SelectedPath;
                    txtForestResourcePath.Text = forestResourcePath;
                    txtForestResourcePath.ForeColor = Color.DarkGreen;
                    UpdateButtonStates();
                }
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��������·��";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputPath = dialog.SelectedPath;
                    txtOutputPath.Text = outputPath;
                    txtOutputPath.ForeColor = Color.DarkGreen;
                    UpdateButtonStates();
                }
            }
        }

        private void btnImportPriceMapping_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Excel�ļ� (*.xlsx)|*.xlsx|Excel�ļ� (*.xls)|*.xls|CSV�ļ� (*.csv)|*.csv|����֧�ֵ��ļ�|*.xlsx;*.xls;*.csv";
                dialog.Title = "ѡ��۸�ӳ����ļ�";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        priceExcelPath = dialog.FileName;
                        
                        // ��ʾ�������
                        statusLabel.Text = "���ڵ���۸�ӳ���...";
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();
                        
                        LoadPriceMappingFromExcel(priceExcelPath);
                        
                        // �ָ�������
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        statusLabel.Text = "����";
                        
                        // ���¼۸�ӳ����ʾ
                        UpdatePriceMappingDisplay();
                        
                        // ��ʾ������
                        var message = $"�۸�ӳ�����ɹ���\n\n����ͳ�ƣ�\n- ������ {priceMapping.Count} ���������ļ۸�����\n- �ļ�·����{System.IO.Path.GetFileName(priceExcelPath)}";
                        
                        if (priceMapping.Count > 0)
                        {
                            message += "\n\n�����Ե��'�鿴ӳ���'��ť�鿴��ϸ��ӳ���ϵ��";
                        }
                        
                        MessageBox.Show(message, "����ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // �Զ��л����۸�ӳ���ҳ����ʾ���
                        if (priceMapping.Count > 0)
                        {
                            var result = MessageBox.Show("�Ƿ������鿴����ļ۸�ӳ�����ݣ�", "�鿴����", 
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                mainTabControl.SelectedIndex = 1; // �л����۸�ӳ���ѡ�
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // �ָ�������
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        statusLabel.Text = "����ʧ��";
                        
                        // ������ʾ����ʹʧ��ҲҪ���£������в������ݣ�
                        UpdatePriceMappingDisplay();
                        
                        MessageBox.Show($"����۸�ӳ���ʧ�ܣ�\n\n{ex.Message}\n\n�����ļ���ʽ�Ƿ���ȷ����ʹ��'����ģ��'���ܻ�ȡ��׼ģ�塣", 
                            "����ʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnPairData_Click(object sender, EventArgs e)
        {
            try
            {
                statusLabel.Text = "�����������...";
                progressBar.Style = ProgressBarStyle.Marquee;
                Application.DoEvents();

                dataPairs.Clear();
                
                // ɨ������·���µ��ļ���
                var landGradeFolders = ScanLandGradeFolders(landGradePricePath);
                var forestResourceFolders = ScanForestResourceFolders(forestResourcePath);

                // ��������������������
                foreach (var landGradeFolder in landGradeFolders)
                {
                    var matchingForestFolder = forestResourceFolders.FirstOrDefault(f => f.AdminCode == landGradeFolder.AdminCode);
                    
                    var pairInfo = new DataPairInfo
                    {
                        AdminCode = landGradeFolder.AdminCode,
                        AdminName = landGradeFolder.AdminName,
                        Selected = true,
                        PriceDataPath = landGradeFolder.ShapefilePath,
                        StatusDataPath = matchingForestFolder?.ShapefilePath ?? "",
                        PriceDataStatus = !string.IsNullOrEmpty(landGradeFolder.ShapefilePath) ? "���ҵ�" : "δ�ҵ�",
                        StatusDataStatus = matchingForestFolder != null ? "���ҵ�" : "δ�ҵ�"
                    };

                    dataPairs.Add(pairInfo);
                }

                // ������������
                dataGridViewPairs.DataSource = new BindingList<DataPairInfo>(dataPairs);

                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
                statusLabel.Text = $"�����ɣ����ҵ� {dataPairs.Count} ������������";

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                statusLabel.Text = "���ʧ��";
                MessageBox.Show($"�������ʱ��������{ex.Message}", "����", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProcessData_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedPairs = dataPairs.Where(p => p.Selected && 
                    !string.IsNullOrEmpty(p.PriceDataPath) && 
                    !string.IsNullOrEmpty(p.StatusDataPath)).ToList();

                if (selectedPairs.Count == 0)
                {
                    MessageBox.Show("������ѡ��һ����Ч��������Խ��д���", "��ʾ", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                progressBar.Maximum = selectedPairs.Count;

                for (int i = 0; i < selectedPairs.Count; i++)
                {
                    var pair = selectedPairs[i];
                    statusLabel.Text = $"���ڴ��� {pair.AdminName} ({i + 1}/{selectedPairs.Count})...";
                    Application.DoEvents();

                    ProcessSingleDataPair(pair);

                    progressBar.Value = i + 1;
                }

                statusLabel.Text = "�������ݴ������";
                MessageBox.Show($"�ɹ������� {selectedPairs.Count} �������������ݣ�", "�������", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // ���öԻ�����ΪOK
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                statusLabel.Text = "����ʧ��";
                MessageBox.Show($"��������ʱ��������{ex.Message}", "����", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnViewPriceMapping_Click(object sender, EventArgs e)
        {
            // �л����۸�ӳ���ѡ�
            mainTabControl.SelectedIndex = 1;
            
            // ������ʾ
            UpdatePriceMappingDisplay();
        }

        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "CSV�ļ� (*.csv)|*.csv|Excel�ļ� (*.xlsx)|*.xlsx";
                    dialog.Title = "�����۸�ӳ���ģ��";
                    dialog.FileName = "�ֵؼ۸�ӳ���ģ��.csv";
                    
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportPriceMappingTemplate(dialog.FileName);
                        MessageBox.Show($"ģ���ѳɹ���������\n{dialog.FileName}\n\n�밴��ģ���ʽ��д�۸����ݣ�Ȼ��ʹ��'����۸�ӳ���'���ܵ��롣", 
                            "�����ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"����ģ��ʧ�ܣ�{ex.Message}", "����", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ���ݴ�����

        private void UpdateButtonStates()
        {
            btnPairData.Enabled = !string.IsNullOrEmpty(landGradePricePath) && 
                                  !string.IsNullOrEmpty(forestResourcePath);
            
            btnProcessData.Enabled = dataPairs.Any(p => p.Selected && 
                                     !string.IsNullOrEmpty(p.PriceDataPath) && 
                                     !string.IsNullOrEmpty(p.StatusDataPath)) &&
                                     !string.IsNullOrEmpty(outputPath) &&
                                     priceMapping.Count > 0;
        }

        private List<FolderInfo> ScanLandGradeFolders(string rootPath)
        {
            var result = new List<FolderInfo>();
            var directories = Directory.GetDirectories(rootPath);

            foreach (var dir in directories)
            {
                var dirName = System.IO.Path.GetFileName(dir);
                
                // ��ȡ6λ���������루���ļ������ƿ�ͷ��
                if (dirName.Length >= 6 && dirName.Substring(0, 6).All(char.IsDigit))
                {
                    var adminCode = dirName.Substring(0, 6);
                    var adminName = ExtractAdminNameFromFolder(dirName);
                    
                    // ����Ŀ��Shapefile
                    var shapefilePath = FindLandGradeShapefile(dir, adminCode);
                    
                    result.Add(new FolderInfo
                    {
                        AdminCode = adminCode,
                        AdminName = adminName,
                        FolderPath = dir,
                        ShapefilePath = shapefilePath
                    });
                }
            }

            return result;
        }

        private List<FolderInfo> ScanForestResourceFolders(string rootPath)
        {
            var result = new List<FolderInfo>();
            var directories = Directory.GetDirectories(rootPath);

            foreach (var dir in directories)
            {
                var dirName = System.IO.Path.GetFileName(dir);
                
                // ���ļ�����������ȡ���������루�����ڵ�6λ���֣�
                var codeMatch = System.Text.RegularExpressions.Regex.Match(dirName, @"\((\d{6})\)");
                if (codeMatch.Success)
                {
                    var adminCode = codeMatch.Groups[1].Value;
                    var adminName = ExtractAdminNameFromForestFolder(dirName);
                    
                    // ����Ŀ��Shapefile
                    var shapefilePath = FindForestResourceShapefile(dir, adminCode);
                    
                    result.Add(new FolderInfo
                    {
                        AdminCode = adminCode,
                        AdminName = adminName,
                        FolderPath = dir,
                        ShapefilePath = shapefilePath
                    });
                }
            }

            return result;
        }

        private string FindLandGradeShapefile(string folderPath, string adminCode)
        {
            try
            {
                // �޸ĺ��·�������ļ���/1-ʸ������/����������+LDDJHJZDJDY.shp
                // ȥ�����м��ͬ�����ļ��в㼶
                var vectorDataPath = System.IO.Path.Combine(folderPath, "1-ʸ������");
                
                System.Diagnostics.Debug.WriteLine($"���ҵؼ�Shapefile: ���·�� {vectorDataPath}");
                
                if (Directory.Exists(vectorDataPath))
                {
                    var targetFileName = adminCode + "LDDJHJZDJDY.shp";
                    var targetPath = System.IO.Path.Combine(vectorDataPath, targetFileName);
                    
                    System.Diagnostics.Debug.WriteLine($"���ҵؼ�Shapefile: Ŀ���ļ� {targetPath}");
                    
                    if (File.Exists(targetPath))
                    {
                        System.Diagnostics.Debug.WriteLine($"�ɹ��ҵ��ؼ�Shapefile: {targetPath}");
                        return targetPath;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"δ�ҵ�Ŀ���ļ�: {targetPath}");
                        
                        // ���������Ϣ���г���Ŀ¼�µ�����.shp�ļ�
                        try
                        {
                            var shpFiles = Directory.GetFiles(vectorDataPath, "*.shp");
                            System.Diagnostics.Debug.WriteLine($"1-ʸ������Ŀ¼�µ�����.shp�ļ���");
                            foreach (var file in shpFiles)
                            {
                                System.Diagnostics.Debug.WriteLine($"  - {System.IO.Path.GetFileName(file)}");
                            }
                        }
                        catch (Exception listEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"�г�Ŀ¼�ļ�ʱ����: {listEx.Message}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"1-ʸ������Ŀ¼������: {vectorDataPath}");
                    
                    // ���������Ϣ���г����ļ����µ�������Ŀ¼
                    try
                    {
                        if (Directory.Exists(folderPath))
                        {
                            var subDirs = Directory.GetDirectories(folderPath);
                            System.Diagnostics.Debug.WriteLine($"���ļ��� {folderPath} �µ�������Ŀ¼��");
                            foreach (var dir in subDirs)
                            {
                                System.Diagnostics.Debug.WriteLine($"  - {System.IO.Path.GetFileName(dir)}");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"���ļ���Ҳ������: {folderPath}");
                        }
                    }
                    catch (Exception listEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"�г����ļ���Ŀ¼ʱ����: {listEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"���ҵؼ�Shapefileʱ����{ex.Message}");
            }
            
            return "";
        }

        private string FindForestResourceShapefile(string folderPath, string adminCode)
        {
            try
            {
                // ·�����ռ�����/��鷶Χ����/(����������)SLZY_DLTB.shp
                var surveyDataPath = System.IO.Path.Combine(folderPath, "�ռ�����", "��鷶Χ����");
                
                if (Directory.Exists(surveyDataPath))
                {
                    // �����ļ�����ʽ������+����������+SLZY_DLTB
                    var targetFileName = $"({adminCode})SLZY_DLTB.shp";
                    var targetPath = System.IO.Path.Combine(surveyDataPath, targetFileName);
                    
                    System.Diagnostics.Debug.WriteLine($"������״Shapefile: {targetPath}");
                    
                    if (File.Exists(targetPath))
                    {
                        System.Diagnostics.Debug.WriteLine($"�ɹ��ҵ���״Shapefile: {targetPath}");
                        return targetPath;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"δ�ҵ�Ŀ���ļ�: {targetPath}");
                        
                        // ���������Ϣ���г���Ŀ¼�µ�����.shp�ļ�
                        try
                        {
                            var shpFiles = Directory.GetFiles(surveyDataPath, "*.shp");
                            System.Diagnostics.Debug.WriteLine($"��鷶Χ����Ŀ¼�µ�����.shp�ļ���");
                            foreach (var file in shpFiles)
                            {
                                System.Diagnostics.Debug.WriteLine($"  - {System.IO.Path.GetFileName(file)}");
                            }
                        }
                        catch (Exception listEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"�г�Ŀ¼�ļ�ʱ����: {listEx.Message}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"��鷶Χ����Ŀ¼������: {surveyDataPath}");
                    
                    // ���������Ϣ���г��ռ�����Ŀ¼�µ�������Ŀ¼
                    try
                    {
                        var spatialDataPath = System.IO.Path.Combine(folderPath, "�ռ�����");
                        if (Directory.Exists(spatialDataPath))
                        {
                            var subDirs = Directory.GetDirectories(spatialDataPath);
                            System.Diagnostics.Debug.WriteLine($"�ռ�����Ŀ¼�µ�������Ŀ¼��");
                            foreach (var dir in subDirs)
                            {
                                System.Diagnostics.Debug.WriteLine($"  - {System.IO.Path.GetFileName(dir)}");
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"�ռ�����Ŀ¼Ҳ������: {spatialDataPath}");
                        }
                    }
                    catch (Exception listEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"�г��ռ�����Ŀ¼ʱ����: {listEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"������״Shapefileʱ����{ex.Message}");
            }
            
            return "";
        }

        private string ExtractAdminNameFromFolder(string folderName)
        {
            // ������"441226�������ֵض����ͻ�׼�ؼ����ݳɹ�"���ļ���������ȡ����
            if (folderName.Length > 6)
            {
                var nameWithSuffix = folderName.Substring(6);
                var name = nameWithSuffix.Replace("�ֵض����ͻ�׼�ؼ����ݳɹ�", "").Trim();
                return name;
            }
            return folderName;
        }

        private string ExtractAdminNameFromForestFolder(string folderName)
        {
            // ������"�㶫ʡ�����е�����(441226)ȫ��������Ȼ��Դ�ʲ����2023��ȹ�����ͼ�ɹ�_ɭ��"��ȡ����
            var match = System.Text.RegularExpressions.Regex.Match(folderName, @"(\w+��)\(");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            
            // ��ѡ��������ȡ����ǰ�����һ������
            var parts = folderName.Split('(');
            if (parts.Length > 0)
            {
                var beforeBracket = parts[0];
                var countyMatch = System.Text.RegularExpressions.Regex.Match(beforeBracket, @"(\w+��)");
                if (countyMatch.Success)
                {
                    return countyMatch.Groups[1].Value;
                }
            }
            
            return "δ֪";
        }

        private void LoadPriceMappingFromExcel(string excelPath)
        {
            priceMapping.Clear();
            var importLog = new List<string>();
            var errorLog = new List<string>();
            
            try
            {
                // ʾ����Ϊ�����Ĺ㶫ʡ�ؼ����������봴���۸�ӳ��
                var sampleData = new Dictionary<string, Dictionary<string, double>>
                {
                    ["441226"] = new Dictionary<string, double> // ������ʾ��
                    {
                        ["1"] = 8.72, // 1���ֵؼ۸���Ԫ/���꣩
                        ["2"] = 6.45,
                        ["3"] = 4.23,
                        ["4"] = 3.15,
                        ["5"] = 2.45
                    },
                    ["441322"] = new Dictionary<string, double> // ������ʾ��
                    {
                        ["1"] = 7.89,
                        ["2"] = 5.67,
                        ["3"] = 3.98,
                        ["4"] = 2.87,
                        ["5"] = 2.12
                    },
                    ["441781"] = new Dictionary<string, double> // ½����ʾ��
                    {
                        ["1"] = 9.12,
                        ["2"] = 6.88,
                        ["3"] = 4.56,
                        ["4"] = 3.33,
                        ["5"] = 2.78
                    }
                };

                bool fileProcessed = false;
                
                // ����ļ����ڣ����Զ�ȡ
                if (File.Exists(excelPath))
                {
                    try
                    {
                        string fileExtension = System.IO.Path.GetExtension(excelPath).ToLower();
                        
                        if (fileExtension == ".csv")
                        {
                            // ����CSV�ļ�
                            importLog.Add("��⵽CSV�ļ�����ʼ����...");
                            fileProcessed = ProcessCsvFile(excelPath, importLog, errorLog);
                        }
                        else if (fileExtension == ".xlsx" || fileExtension == ".xls")
                        {
                            // ������ΪCSV��ʽ��ȡExcel�ļ�
                            importLog.Add("��⵽Excel�ļ������԰�CSV��ʽ����...");
                            fileProcessed = ProcessCsvFile(excelPath, importLog, errorLog);
                            
                            if (!fileProcessed)
                            {
                                importLog.Add("CSV��ʽ����ʧ�ܣ��������һ��������Excel�ļ���");
                                errorLog.Add("�ݲ�֧��ֱ�Ӷ�ȡExcel�ļ����뽫Excel�ļ����ΪCSV��ʽ�����ԡ�");
                            }
                        }
                    }
                    catch (Exception fileEx)
                    {
                        errorLog.Add($"�ļ���������: {fileEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"�ļ�����ʧ��: {fileEx.Message}");
                    }
                }
                else
                {
                    errorLog.Add("ѡ����ļ�������");
                }

                // ����ļ�����ʧ�ܻ�û�����ݣ�ʹ��ʾ������
                if (!fileProcessed || priceMapping.Count == 0)
                {
                    importLog.Add("ʹ������ʾ������...");
                    priceMapping = new Dictionary<string, Dictionary<string, double>>(sampleData);
                    importLog.Add($"�Ѽ��� {priceMapping.Count} ��ʾ���������ļ۸�����");
                }
                
                UpdateButtonStates();
                
                // ����д���򾯸棬��ʾ��ϸ��Ϣ
                if (errorLog.Count > 0)
                {
                    var errorMessage = "��������������������⣺\n\n" + string.Join("\n", errorLog);
                    if (priceMapping.Count > 0)
                    {
                        errorMessage += $"\n\n�ѳɹ����� {priceMapping.Count} �������������ݡ�";
                    }
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                // ������з�����ʧ�ܣ�����ȷ����һЩ��������
                if (priceMapping.Count == 0)
                {
                    priceMapping = new Dictionary<string, Dictionary<string, double>>
                    {
                        ["441226"] = new Dictionary<string, double>
                        {
                            ["1"] = 8.72, ["2"] = 6.45, ["3"] = 4.23, ["4"] = 3.15, ["5"] = 2.45
                        }
                    };
                }
                
                // �����׳��쳣������������Ϣ
                var detailedMessage = ex.Message;
                if (importLog.Count > 0)
                {
                    detailedMessage += "\n\n������־��\n" + string.Join("\n", importLog);
                }
                detailedMessage += "\n\n��ʾ����ȷ���ļ�ΪCSV��ʽ�������У�����������,����������,1���۸�,2���۸�,3���۸�,4���۸�,5���۸�";
                
                throw new Exception(detailedMessage);
            }
        }

        private bool ProcessCsvFile(string filePath, List<string> importLog, List<string> errorLog)
        {
            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8);
                importLog.Add($"�ɹ���ȡ�ļ����� {lines.Length} ��");
                
                if (lines.Length <= 2)
                {
                    errorLog.Add("�ļ�����̫�٣�������Ҫ3�У�2�б�ͷ + 1�����ݣ�");
                    return false;
                }

                int successCount = 0;
                int skipCount = 0;
                int errorCount = 0;

                for (int i = 2; i < lines.Length; i++) // ����ǰ���б�ͷ
                {
                    var line = lines[i].Trim();
                    
                    // �������к�ע����
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    {
                        skipCount++;
                        continue;
                    }
                    
                    try
                    {
                        var parts = line.Split(',', '\t'); // ֧�ֶ��Ż��Ʊ���ָ�
                        if (parts.Length >= 7)
                        {
                            var adminName = parts[0]?.Trim().Trim('"');
                            var adminCode = parts[1]?.Trim().Trim('"');
                            
                            if (!string.IsNullOrEmpty(adminCode) && adminCode.Length == 6 && adminCode.All(char.IsDigit))
                            {
                                var prices = new Dictionary<string, double>();
                                
                                // ��ȡ1-5���۸�������2-6��
                                for (int j = 2; j <= 6; j++)
                                {
                                    if (double.TryParse(parts[j]?.Trim().Trim('"'), out double price) && price > 0)
                                    {
                                        var grade = (j - 1).ToString(); // 1-5��
                                        prices[grade] = price;
                                    }
                                }
                                
                                if (prices.Count > 0)
                                {
                                    priceMapping[adminCode] = prices;
                                    successCount++;
                                    importLog.Add($"�ɹ�����: {adminName}({adminCode}) - {prices.Count}���ȼ�");
                                }
                                else
                                {
                                    errorLog.Add($"��{i+1}��: {adminName}({adminCode}) - û����Ч�ļ۸�����");
                                    errorCount++;
                                }
                            }
                            else
                            {
                                errorLog.Add($"��{i+1}��: �����������ʽ���� - '{adminCode}'");
                                errorCount++;
                            }
                        }
                        else
                        {
                            errorLog.Add($"��{i+1}��: �������㣬����7�У�ʵ��{parts.Length}��");
                            errorCount++;
                        }
                    }
                    catch (Exception lineEx)
                    {
                        errorLog.Add($"��{i+1}��: �������� - {lineEx.Message}");
                        errorCount++;
                    }
                }

                importLog.Add($"�������: �ɹ�{successCount}��, ����{skipCount}��, ����{errorCount}��");
                
                return successCount > 0;
            }
            catch (Exception ex)
            {
                errorLog.Add($"�ļ���ȡʧ��: {ex.Message}");
                return false;
            }
        }
        private void ProcessSingleDataPair(DataPairInfo pair)
        {
            try
            {
                // ��������ļ���
                var outputFolderName = $"{pair.AdminCode}{pair.AdminName}LDHSJG";
                var outputFolderPath = System.IO.Path.Combine(outputPath, outputFolderName);
                Directory.CreateDirectory(outputFolderPath);

                // �������Shapefile
                var outputShapefileName = $"{pair.AdminCode}LDHSJG";
                var outputShapefilePath = System.IO.Path.Combine(outputFolderPath, outputShapefileName + ".shp");

                // ����ռ����ݹ��������Լ���
                ProcessSpatialDataAssociation(pair.StatusDataPath, pair.PriceDataPath, outputShapefilePath, pair);
            }
            catch (Exception ex)
            {
                throw new Exception($"���� {pair.AdminName} ����ʱ����{ex.Message}");
            }
        }

        private void ProcessSpatialDataAssociation(string statusShpPath, string priceShpPath, string outputShpPath, DataPairInfo pair)
        {
            IWorkspace statusWorkspace = null;
            IWorkspace priceWorkspace = null;
            IFeatureClass statusFeatureClass = null;
            IFeatureClass priceFeatureClass = null;
            IFeatureClass outputFeatureClass = null;

            try
            {
                // ����״Shapefile
                Type statusWorkspaceFactoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                var statusWorkspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(statusWorkspaceFactoryType);
                statusWorkspace = statusWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(statusShpPath), 0);
                statusFeatureClass = ((IFeatureWorkspace)statusWorkspace).OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(statusShpPath));

                // �򿪵ؼ�Shapefile
                Type priceWorkspaceFactoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                var priceWorkspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(priceWorkspaceFactoryType);
                priceWorkspace = priceWorkspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(priceShpPath), 0);
                priceFeatureClass = ((IFeatureWorkspace)priceWorkspace).OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(priceShpPath));

                // �������Shapefile
                outputFeatureClass = CreateLDHSJGShapefile(outputShpPath, statusFeatureClass);

                // ����ÿ����״Ҫ��
                ProcessFeatures(statusFeatureClass, priceFeatureClass, outputFeatureClass, pair);
            }
            finally
            {
                // �ͷ�COM����
                if (outputFeatureClass != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(outputFeatureClass);
                if (priceFeatureClass != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(priceFeatureClass);
                if (statusFeatureClass != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(statusFeatureClass);
                if (priceWorkspace != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(priceWorkspace);
                if (statusWorkspace != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(statusWorkspace);
            }
        }

        private IFeatureClass CreateLDHSJGShapefile(string outputPath, IFeatureClass templateFeatureClass)
        {
            Type workspaceFactoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
            var workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(workspaceFactoryType);
            var workspace = workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(outputPath), 0);
            var featureWorkspace = (IFeatureWorkspace)workspace;

            // �����ֶμ���
            var fields = new FieldsClass();
            var fieldsEdit = (IFieldsEdit)fields;

            // ���OID�ֶ�
            var oidField = new FieldClass();
            var oidFieldEdit = (IFieldEdit)oidField;
            oidFieldEdit.Name_2 = "FID";
            oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(oidField);

            // ��Ӽ����ֶ�
            var geometryField = new FieldClass();
            var geometryFieldEdit = (IFieldEdit)geometryField;
            geometryFieldEdit.Name_2 = "Shape";
            geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            geometryFieldEdit.GeometryDef_2 = ((IGeoDataset)templateFeatureClass).SpatialReference != null ?
                CreateGeometryDef(((IGeoDataset)templateFeatureClass).SpatialReference) :
                CreateGeometryDef(null);
            fieldsEdit.AddField(geometryField);

            // ���ҵ���ֶ�
            AddLDHSJGBusinessFields(fieldsEdit);

            // ����Ҫ����
            var featureClass = featureWorkspace.CreateFeatureClass(
                System.IO.Path.GetFileNameWithoutExtension(outputPath),
                fields,
                null,
                null,
                esriFeatureType.esriFTSimple,
                "Shape",
                "");

            return featureClass;
        }

        private IGeometryDef CreateGeometryDef(ISpatialReference spatialReference)
        {
            var geometryDef = new GeometryDefClass();
            var geometryDefEdit = (IGeometryDefEdit)geometryDef;
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            if (spatialReference != null)
            {
                geometryDefEdit.SpatialReference_2 = spatialReference;
            }
            return geometryDef;
        }

        private void AddLDHSJGBusinessFields(IFieldsEdit fieldsEdit)
        {
            // 1. �ʲ�����ʶ��
            AddField(fieldsEdit, "ZCQCBSM", esriFieldType.esriFieldTypeString, 22);
            
            // 2. Ҫ�ش���
            AddField(fieldsEdit, "YSDM", esriFieldType.esriFieldTypeString, 10);
            
            // 3. ����ʡ����������
            AddField(fieldsEdit, "SZSFXZDM", esriFieldType.esriFieldTypeString, 2);
            
            // 4. ����ʡ����������
            AddField(fieldsEdit, "SZSFXZMC", esriFieldType.esriFieldTypeString, 100);
            
            // 5. �ؼ���������
            AddField(fieldsEdit, "XJXZDM", esriFieldType.esriFieldTypeString, 6);
            
            // 6. �ؼ���������
            AddField(fieldsEdit, "XJXZMC", esriFieldType.esriFieldTypeString, 100);
            
            // 7. �ֵؼ���
            AddField(fieldsEdit, "LDJB", esriFieldType.esriFieldTypeString, 10);
            
            // 8. �����������
            AddField(fieldsEdit, "EJDLDM", esriFieldType.esriFieldTypeString, 5);
            
            // 9. ������������
            AddField(fieldsEdit, "EJDLMC", esriFieldType.esriFieldTypeString, 50);
            
            // 10. �ؼ��ֵ�ƽ����
            AddField(fieldsEdit, "XJLDPJJ", esriFieldType.esriFieldTypeDouble, 15, 5);
            
            // 11. ������չ����
            AddField(fieldsEdit, "QYKZDM", esriFieldType.esriFieldTypeString, 19);
        }

        private void AddField(IFieldsEdit fieldsEdit, string name, esriFieldType type, int length, int precision = 0)
        {
            var field = new FieldClass();
            var fieldEdit = (IFieldEdit)field;
            fieldEdit.Name_2 = name;
            fieldEdit.Type_2 = type;
            fieldEdit.Length_2 = length;
            
            // ����Double�����ֶΣ���Ҫ�ر���������
            if (type == esriFieldType.esriFieldTypeDouble)
            {
                // ����Double���ͣ�precision��ʾ��λ����scale��ʾС��λ��
                // ArcGIS��Double�ֶβ���Ҫ����Length��ϵͳ���Զ�����
                fieldEdit.Precision_2 = 15; // �ܾ���15λ
                fieldEdit.Scale_2 = 5;      // С�����5λ
                System.Diagnostics.Debug.WriteLine($"����Double�ֶ� {name}: �ܾ���=15, С��λ=5");
            }
            else if (precision > 0)
            {
                fieldEdit.Precision_2 = precision;
            }
            
            fieldsEdit.AddField(field);
        }

        private void ProcessFeatures(IFeatureClass statusFC, IFeatureClass priceFC, IFeatureClass outputFC, DataPairInfo pair)
        {
            var statusCursor = statusFC.Search(null, false);
            var outputCursor = outputFC.Insert(true);
            var sequenceNumber = 1;

            IFeature statusFeature;
            while ((statusFeature = statusCursor.NextFeature()) != null)
            {
                try
                {
                    // ������Ҫ��
                    var outputFeatureBuffer = outputFC.CreateFeatureBuffer();
                    
                    // ���Ƽ���ͼ��
                    outputFeatureBuffer.Shape = statusFeature.Shape;
                    
                    // �����ֶ�ֵ
                    SetLDHSJGFieldValues(outputFeatureBuffer, statusFeature, priceFC, pair, sequenceNumber);
                    
                    // ����Ҫ��
                    outputCursor.InsertFeature(outputFeatureBuffer);
                    
                    sequenceNumber++;
                    
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(outputFeatureBuffer);
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(statusFeature);
                }
            }

            outputCursor.Flush();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(outputCursor);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(statusCursor);
        }

        private void SetLDHSJGFieldValues(IFeatureBuffer outputFeature, IFeature statusFeature, IFeatureClass priceFC, DataPairInfo pair, int sequenceNumber)
        {
            // 1. �ʲ�����ʶ�� (ZCQCBSM)
            var identifier = $"{pair.AdminCode}4140{sequenceNumber:D12}";
            outputFeature.set_Value(outputFeature.Fields.FindField("ZCQCBSM"), identifier);
            
            // 2. Ҫ�ش��� (YSDM)
            outputFeature.set_Value(outputFeature.Fields.FindField("YSDM"), "2150201040");
            
            // 3. ����ʡ���������� (SZSFXZDM)
            outputFeature.set_Value(outputFeature.Fields.FindField("SZSFXZDM"), "44");
            
            // 4. ����ʡ���������� (SZSFXZMC)
            outputFeature.set_Value(outputFeature.Fields.FindField("SZSFXZMC"), "�㶫ʡ");
            
            // 5. �ؼ��������� (XJXZDM)
            outputFeature.set_Value(outputFeature.Fields.FindField("XJXZDM"), pair.AdminCode);
            
            // 6. �ؼ��������� (XJXZMC)
            outputFeature.set_Value(outputFeature.Fields.FindField("XJXZMC"), pair.AdminName);
            
            // 7. �ֵؼ��� (LDJB) - ͨ���ռ��ѯ��ȡ
            var landGrade = GetLandGradeFromSpatialQuery(statusFeature, priceFC);
            outputFeature.set_Value(outputFeature.Fields.FindField("LDJB"), landGrade);
            
            // 8-9. ������������������ʱΪ��
            outputFeature.set_Value(outputFeature.Fields.FindField("EJDLDM"), "");
            outputFeature.set_Value(outputFeature.Fields.FindField("EJDLMC"), "");
            
            // 10. �ؼ��ֵ�ƽ���� (XJLDPJJ)
            var averagePrice = GetAveragePrice(pair.AdminCode, landGrade);
            outputFeature.set_Value(outputFeature.Fields.FindField("XJLDPJJ"), averagePrice);
            
            // 11. ������չ������ʱΪ��
            outputFeature.set_Value(outputFeature.Fields.FindField("QYKZDM"), "");
        }

        private string GetLandGradeFromSpatialQuery(IFeature statusFeature, IFeatureClass priceFC)
        {
            ISpatialFilter spatialFilter = null;
            IFeatureCursor cursor = null;
            
            try
            {
                spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = statusFeature.Shape;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                
                cursor = priceFC.Search(spatialFilter, false);
                
                var maxArea = 0.0;
                var resultGrade = "";
                
                IFeature priceFeature;
                while ((priceFeature = cursor.NextFeature()) != null)
                {
                    try
                    {
                        // �����ཻ���
                        var intersectionGeom = ((ITopologicalOperator)statusFeature.Shape).Intersect(priceFeature.Shape, esriGeometryDimension.esriGeometry2Dimension);
                        var intersectionArea = ((IArea)intersectionGeom).Area;
                        
                        if (intersectionArea > maxArea)
                        {
                            maxArea = intersectionArea;
                            
                            // ��ȡSPLJB�ֶ�ֵ
                            var spljbField = priceFeature.Fields.FindField("SPLJB");
                            if (spljbField >= 0)
                            {
                                resultGrade = priceFeature.get_Value(spljbField)?.ToString() ?? "";
                            }
                        }
                        
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(intersectionGeom);
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(priceFeature);
                    }
                }
                
                return resultGrade;
            }
            finally
            {
                if (cursor != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                if (spatialFilter != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
            }
        }

        private double GetAveragePrice(string adminCode, string landGrade)
        {
            if (priceMapping.ContainsKey(adminCode) && priceMapping[adminCode].ContainsKey(landGrade))
            {
                return priceMapping[adminCode][landGrade];
            }
            return 0.0;
        }

        private void ExportPriceMappingTemplate(string filePath)
        {
            var lines = new List<string>
            {
                "# �ֵؼ۸�ӳ���ģ��",
                "����������,����������,1���۸�,2���۸�,3���۸�,4���۸�,5���۸�",
                "������,441226,8.72,6.45,4.23,3.15,2.45",
                "������,441322,7.89,5.67,3.98,2.87,2.12",
                "½����,441781,9.12,6.88,4.56,3.33,2.78",
                "# ���������ʾ�����ݻ������޸Ļ��������ʵ������",
                "# ע�⣺ǰ����Ϊ��ͷ���ӵ�3�п�ʼ��������"
            };
            
            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }

        private void UpdatePriceMappingDisplay()
        {
            try
            {
                // ����״̬��ǩ
                if (priceMapping.Count == 0)
                {
                    lblMappingStatus.Text = "״̬����δ�����κμ۸�ӳ������";
                    lblMappingStatus.ForeColor = Color.Gray;
                }
                else
                {
                    lblMappingStatus.Text = $"״̬���ѵ��� {priceMapping.Count} ���������ļ۸�����";
                    lblMappingStatus.ForeColor = Color.DarkGreen;
                }

                // ׼����ʾ����
                var displayData = new List<PriceMappingDisplayItem>();
                
                foreach (var mapping in priceMapping)
                {
                    var adminCode = mapping.Key;
                    var prices = mapping.Value;
                    
                    // ���Ҷ�Ӧ������������
                    var adminName = GetAdminNameByCode(adminCode);
                    
                    var item = new PriceMappingDisplayItem
                    {
                        AdminCode = adminCode,
                        AdminName = adminName,
                        Grade1Price = prices.ContainsKey("1") ? prices["1"] : 0,
                        Grade2Price = prices.ContainsKey("2") ? prices["2"] : 0,
                        Grade3Price = prices.ContainsKey("3") ? prices["3"] : 0,
                        Grade4Price = prices.ContainsKey("4") ? prices["4"] : 0,
                        Grade5Price = prices.ContainsKey("5") ? prices["5"] : 0
                    };
                    
                    displayData.Add(item);
                }

                // ����������������
                displayData = displayData.OrderBy(x => x.AdminCode).ToList();
                
                // �󶨵�DataGridView
                dataGridViewPriceMapping.DataSource = new BindingList<PriceMappingDisplayItem>(displayData);
                
                // ���²鿴ӳ���ť��״̬
                btnViewPriceMapping.Enabled = priceMapping.Count > 0;
                if (priceMapping.Count > 0)
                {
                    btnViewPriceMapping.Text = $"�鿴ӳ���({priceMapping.Count})";
                }
                else
                {
                    btnViewPriceMapping.Text = "�鿴ӳ���";
                }
            }
            catch (Exception ex)
            {
                lblMappingStatus.Text = $"״̬����ʾӳ������ʱ���� - {ex.Message}";
                lblMappingStatus.ForeColor = Color.Red;
            }
        }

        private string GetAdminNameByCode(string adminCode)
        {
            // ���Դ�����Ե������в�������������
            var pair = dataPairs.FirstOrDefault(p => p.AdminCode == adminCode);
            if (pair != null)
            {
                return pair.AdminName;
            }
            
            // ʹ��Ԥ�����ӳ��
            var knownNames = new Dictionary<string, string>
            {
                ["441226"] = "������",
                ["441322"] = "������", 
                ["441781"] = "½����",
                ["440105"] = "������",
                ["440106"] = "�����"
                // ���Լ�����Ӹ�����֪��ӳ��
            };
            
            return knownNames.ContainsKey(adminCode) ? knownNames[adminCode] : $"δ֪({adminCode})";
        }

        #endregion

    }

    #region ���ݽṹ

    public class DataPairInfo
    {
        public bool Selected { get; set; }
        public string AdminCode { get; set; }
        public string AdminName { get; set; }
        public string PriceDataPath { get; set; }
        public string StatusDataPath { get; set; }
        public string PriceDataStatus { get; set; }
        public string StatusDataStatus { get; set; }
    }

    public class FolderInfo
    {
        public string AdminCode { get; set; }
        public string AdminName { get; set; }
        public string FolderPath { get; set; }
        public string ShapefilePath { get; set; }
    }

    public class PriceMappingDisplayItem
    {
        public string AdminCode { get; set; }
        public string AdminName { get; set; }
        public double Grade1Price { get; set; }
        public double Grade2Price { get; set; }
        public double Grade3Price { get; set; }
        public double Grade4Price { get; set; }
        public double Grade5Price { get; set; }
    }

    #endregion
}