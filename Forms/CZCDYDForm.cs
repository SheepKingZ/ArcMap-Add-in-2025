using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ForestResourcePlugin;
using TestArcMapAddin2.Utils;

namespace TestArcMapAddin2.Forms
{
    public partial class CZCDYDForm : Form
    {
        private string selectedSlzyzcPath = "";
        private string selectedCyzyzcPath = "";
        private string selectedSdzyzcPath = "";
        private string selectedCzcdydPath = "";
        private string selectedOutputPath = ""; // ���������·��
        private string selectedResultOutputPath = ""; // ���������н���ļ���·��

        // �洢���ش��������ļ���Ϣ
        private Dictionary<string, CountyFileGroup> countyFileGroups = new Dictionary<string, CountyFileGroup>();

        // �洢ѡ�е����б�
        private List<string> selectedCounties = new List<string>();

        // �洢���ֵ�CZCDYDQC�ļ�ӳ�� (�ش��� -> shapefile·��)
        private Dictionary<string, string> czcdydqcFileMap = new Dictionary<string, string>();

        public CZCDYDForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // ��ʼ���ؼ�״̬
            btnOK.Enabled = false;
            progressBar.Value = 0;
            lblStatus.Text = "����ѡ������Դ�ļ���...";
            lblStatus.ForeColor = Color.DarkBlue;

            // ��ʼ����ѡ���б�
            InitializeCountySelectionList();
        }

        /// <summary>
        /// ��ʼ����ѡ���б����
        /// </summary>
        private void InitializeCountySelectionList()
        {
            checkedListBoxCounties.CheckOnClick = true;
            checkedListBoxCounties.ItemCheck += CheckedListBoxCounties_ItemCheck;

            // ��ʼ״̬�½���ȫѡ��ȫ��ѡ��ť
            btnSelectAll.Enabled = false;
            btnSelectNone.Enabled = false;
        }

        /// <summary>
        /// ��ѡ���б���Ŀ����¼�
        /// </summary>
        private void CheckedListBoxCounties_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var checkedListBox = sender as CheckedListBox;
            if (checkedListBox == null) return;

            // �ӳٸ���ѡ��״̬����ΪItemCheck�¼���״̬�ı�ǰ����
            this.BeginInvoke(new Action(() =>
            {
                selectedCounties.Clear();
                foreach (var item in checkedListBox.CheckedItems)
                {
                    var countyInfo = item.ToString();
                    // ����ʾ�ı�����ȡ�ش��� "����(�ش���)"
                    var match = Regex.Match(countyInfo, @"\((\d{6})\)");
                    if (match.Success)
                    {
                        selectedCounties.Add(match.Groups[1].Value);
                    }
                }
                UpdateButtonStates();
                UpdateStatusLabel();
            }));
        }

        private void UpdateButtonStates()
        {
            bool hasValidCounties = countyFileGroups.Any(kvp => kvp.Value.IsComplete);
            bool hasSelectedCounties = selectedCounties.Count > 0 &&
                                     countyFileGroups.Any(kvp => selectedCounties.Contains(kvp.Key) && kvp.Value.IsComplete);

            btnOK.Enabled = hasSelectedCounties;
            btnSelectAll.Enabled = checkedListBoxCounties.Items.Count > 0;
            btnSelectNone.Enabled = checkedListBoxCounties.Items.Count > 0 && checkedListBoxCounties.CheckedItems.Count > 0;
        }

        /// <summary>
        /// ����״̬��ǩ
        /// </summary>
        private void UpdateStatusLabel()
        {
            if (countyFileGroups.Count == 0)
            {
                lblStatus.Text = "����ѡ������Դ�ļ���...";
                lblStatus.ForeColor = Color.DarkBlue;
            }
            else if (selectedCounties.Count == 0)
            {
                int completeCount = countyFileGroups.Count(kvp => kvp.Value.IsComplete);
                lblStatus.Text = $"��ɨ�赽 {countyFileGroups.Count} ���ص����ݣ����� {completeCount} ����������������ѡ��Ҫ�������...";
                lblStatus.ForeColor = Color.DarkOrange;
            }
            else
            {
                lblStatus.Text = $"��ѡ�� {selectedCounties.Count} ���ؽ��д������'��ʼ����'����...";
                lblStatus.ForeColor = Color.DarkGreen;
            }
        }

        /// <summary>
        /// ������ѡ���б���ʾ
        /// </summary>
        private void UpdateCountySelectionList()
        {
            checkedListBoxCounties.Items.Clear();

            foreach (var kvp in countyFileGroups.OrderBy(x => x.Key))
            {
                string countyCode = kvp.Key;
                var fileGroup = kvp.Value;

                // ʹ��CountyCodeMapper��ȡ����
                string countyName = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyNameFromCode(countyCode);

                // ��ʾ��ʽ������(�ش���) - �ļ�������״̬
                string displayText = $"{countyName}({countyCode})";
                if (!fileGroup.IsComplete)
                {
                    displayText += " - �ļ�������";
                }

                checkedListBoxCounties.Items.Add(displayText);

                // ����ļ�������Ĭ��ѡ��
                if (fileGroup.IsComplete)
                {
                    checkedListBoxCounties.SetItemChecked(checkedListBoxCounties.Items.Count - 1, true);
                }
            }

            // ���°�ť״̬��״̬��ǩ
            UpdateButtonStates();
            UpdateStatusLabel();
        }

        #region �¼��������

        private void btnBrowseSlzyzc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��SLZYZC_DLTBԴ�ļ���";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "����ɨ��SLZYZC_DLTB�ļ�...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedSlzyzcPath = dialog.SelectedPath;
                        var files = FindShapefilesWithPattern(dialog.SelectedPath, "SLZYZC_DLTB");
                        ProcessFoundFiles(files, FileType.SLZYZC_DLTB);

                        txtSlzyzcPath.Text = $"{dialog.SelectedPath} (�ҵ� {files.Count} ���ļ�)";
                        txtSlzyzcPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateCountySelectionList();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "ɨ��SLZYZC_DLTB�ļ�ʱ����";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"ɨ��SLZYZC_DLTB�ļ�ʱ����: {ex.Message}", "����",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBrowseCyzyzc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��CYZYZC_DLTBԴ�ļ���";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "����ɨ��CYZYZC_DLTB�ļ�...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedCyzyzcPath = dialog.SelectedPath;
                        var files = FindShapefilesWithPattern(dialog.SelectedPath, "CYZYZC_DLTB");
                        ProcessFoundFiles(files, FileType.CYZYZC_DLTB);

                        txtCyzyzcPath.Text = $"{dialog.SelectedPath} (�ҵ� {files.Count} ���ļ�)";
                        txtCyzyzcPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateCountySelectionList();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "ɨ��CYZYZC_DLTB�ļ�ʱ����";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"ɨ��CYZYZC_DLTB�ļ�ʱ����: {ex.Message}", "����",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBrowseSdzyzc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��SDZYZC_DLTBԴ�ļ���";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "����ɨ��SDZYZC_DLTB�ļ�...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedSdzyzcPath = dialog.SelectedPath;
                        var files = FindShapefilesWithPattern(dialog.SelectedPath, "SDZYZC_DLTB");
                        ProcessFoundFiles(files, FileType.SDZYZC_DLTB);

                        txtSdzyzcPath.Text = $"{dialog.SelectedPath} (�ҵ� {files.Count} ���ļ�)";
                        txtSdzyzcPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateCountySelectionList();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "ɨ��SDZYZC_DLTB�ļ�ʱ����";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"ɨ��SDZYZC_DLTB�ļ�ʱ����: {ex.Message}", "����",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBrowseCzcdyd_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��CZCDYD����Դ�ļ���";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "����ɨ��CZCDYD�ļ�...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedCzcdydPath = dialog.SelectedPath;
                        var files = FindShapefilesWithPattern(dialog.SelectedPath, "CZCDYD");
                        ProcessFoundFiles(files, FileType.CZCDYD);

                        txtCzcdydPath.Text = $"{dialog.SelectedPath} (�ҵ� {files.Count} ���ļ�)";
                        txtCzcdydPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateCountySelectionList();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "ɨ��CZCDYD�ļ�ʱ����";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"ɨ��CZCDYD�ļ�ʱ����: {ex.Message}", "����",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��CZCDYDQC�������ļ���";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedOutputPath = dialog.SelectedPath;
                    txtOutputPath.Text = dialog.SelectedPath;
                    txtOutputPath.ForeColor = Color.DarkGreen;
                }
            }
        }

        // ������������·��ѡ���¼��������
        private void btnBrowseResultOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ������CZCDYDQC����ļ���";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "����ɨ��CZCDYDQC����ļ�...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedResultOutputPath = dialog.SelectedPath;

                        // ɨ�����ļ����е�CZCDYDQC�ļ�
                        var czcdydqcFiles = FindCZCDYDQCShapefiles(dialog.SelectedPath);
                        czcdydqcFileMap.Clear();

                        foreach (var file in czcdydqcFiles)
                        {
                            var countyCode = ExtractCountyCodeFromCZCDYDQCFileName(file);
                            if (!string.IsNullOrEmpty(countyCode))
                            {
                                czcdydqcFileMap[countyCode] = file;
                            }
                        }

                        txtResultOutputPath.Text = $"{dialog.SelectedPath} (�ҵ� {czcdydqcFiles.Count} ��CZCDYDQC�ļ�)";
                        txtResultOutputPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateStatusLabel();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "ɨ��CZCDYDQC����ļ�ʱ����";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"ɨ��CZCDYDQC����ļ�ʱ����: {ex.Message}", "����",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxCounties.Items.Count; i++)
            {
                // ֻѡ���ļ���������
                var displayText = checkedListBoxCounties.Items[i].ToString();
                if (!displayText.Contains("�ļ�������"))
                {
                    checkedListBoxCounties.SetItemChecked(i, true);
                }
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxCounties.Items.Count; i++)
            {
                checkedListBoxCounties.SetItemChecked(i, false);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedCounties.Count == 0)
                {
                    MessageBox.Show("��ѡ��Ҫ������ء�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                lblStatus.Text = "���ڴ���ѡ�е�������...";
                lblStatus.ForeColor = Color.Blue;
                progressBar.Value = 0;
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Maximum = selectedCounties.Count;

                var processingResults = new List<string>();

                // �жϴ���ģʽ���½��������д�����н��
                bool writeToExistingResults = !string.IsNullOrEmpty(selectedResultOutputPath) && czcdydqcFileMap.Count > 0;

                string outputBaseDir;
                if (writeToExistingResults)
                {
                    outputBaseDir = selectedResultOutputPath;
                }
                else
                {
                    // �����µ����Ŀ¼
                    outputBaseDir = !string.IsNullOrEmpty(selectedOutputPath) ? selectedOutputPath :
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CZCDYDQC_Output");
                    if (!Directory.Exists(outputBaseDir))
                    {
                        Directory.CreateDirectory(outputBaseDir);
                    }
                }

                for (int i = 0; i < selectedCounties.Count; i++)
                {
                    string countyCode = selectedCounties[i];
                    string countyName = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyNameFromCode(countyCode);

                    lblStatus.Text = $"���ڴ��� {countyName}({countyCode})...";
                    Application.DoEvents();

                    if (countyFileGroups.ContainsKey(countyCode))
                    {
                        var fileGroup = countyFileGroups[countyCode];

                        try
                        {
                            string targetShapefilePath;

                            if (writeToExistingResults && czcdydqcFileMap.ContainsKey(countyCode))
                            {
                                // д�����е�CZCDYDQC�ļ�
                                targetShapefilePath = czcdydqcFileMap[countyCode];
                            }
                            else
                            {
                                // �����µ�����ļ�
                                string countyOutputDir = Path.Combine(outputBaseDir, $"{countyName}_{countyCode}");
                                if (!Directory.Exists(countyOutputDir))
                                {
                                    Directory.CreateDirectory(countyOutputDir);
                                }
                                targetShapefilePath = Path.Combine(countyOutputDir, $"({countyCode})CZCDYDQC.shp");
                            }

                            // ����CZCDYD����������
                            var countyFiles = new CZCDYDProcessor.CountyFiles
                            {
                                CountyCode = countyCode,
                                SlzyzcDltbFile = fileGroup.SlzyzcDltbFile,
                                CyzyzcDltbFile = fileGroup.CyzyzcDltbFile,
                                SdzyzcDltbFile = fileGroup.SdzyzcDltbFile,
                                CzcdydFile = fileGroup.CzcdydFile,
                                OutputDirectory = Path.GetDirectoryName(targetShapefilePath)
                            };

                            // �������Ȼص�
                            CZCDYDProcessor.ProgressCallback progressCallback = (percentage, message) =>
                            {
                                // �����������
                                int overallProgress = (i * 100 + percentage) / selectedCounties.Count;

                                // ����UI��Ҫ��UI�߳���ִ��
                                if (this.InvokeRequired)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        lblStatus.Text = $"{countyName}: {message}";
                                        Application.DoEvents();
                                    }));
                                }
                                else
                                {
                                    lblStatus.Text = $"{countyName}: {message}";
                                    Application.DoEvents();
                                }
                            };

                            // ִ��CZCDYDQC����
                            var processor = new CZCDYDProcessor();
                            var result = processor.ProcessCountyCZCDYDQC(countyFiles, progressCallback);

                            if (result.Success)
                            {
                                string mode = writeToExistingResults ? "���������ļ�" : "�������ļ�";
                                processingResults.Add($"{countyName}({countyCode}) - ������� ({mode})");
                                System.Diagnostics.Debug.WriteLine($"�� {countyName}({countyCode}) ����ɹ�");
                                System.Diagnostics.Debug.WriteLine($"  ����ļ�: {result.OutputPath}");
                                System.Diagnostics.Debug.WriteLine($"  ����Ҫ����: {result.ProcessedFeatureCount}");
                            }
                            else
                            {
                                processingResults.Add($"{countyName}({countyCode}) - ����ʧ��: {result.ErrorMessage}");
                                System.Diagnostics.Debug.WriteLine($"�� {countyName}({countyCode}) ����ʧ��: {result.ErrorMessage}");
                            }
                        }
                        catch (Exception ex)
                        {
                            processingResults.Add($"{countyName}({countyCode}) - �����쳣: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"������ {countyName}({countyCode}) ʱ�����쳣: {ex.Message}");
                        }
                    }

                    progressBar.Value = i + 1;
                    Application.DoEvents();
                }

                lblStatus.Text = "������ɣ�";
                lblStatus.ForeColor = Color.DarkGreen;

                string processingMode = writeToExistingResults ? "��������CZCDYDQC�ļ�" : "������CZCDYDQC�ļ�";
                string resultMessage = $"{processingMode}��ɣ�\n\n���Ŀ¼��{outputBaseDir}\n\n��������\n{string.Join("\n", processingResults)}";
                MessageBox.Show(resultMessage, "�������", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ѯ���Ƿ�����Ŀ¼
                if (MessageBox.Show("�Ƿ�����Ŀ¼�鿴�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(outputBaseDir);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"�����Ŀ¼ʱ����: {ex.Message}");
                    }
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                lblStatus.Text = "����ʧ��";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"��������г��ִ���: {ex.Message}", "����",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��ָ��Ŀ¼�²��Ұ����ض�ģʽ��shapefile
        /// </summary>
        /// <param name="rootPath">��Ŀ¼·��</param>
        /// <param name="pattern">�ļ���ģʽ</param>
        /// <returns>�ҵ���shapefile�б�</returns>
        private List<string> FindShapefilesWithPattern(string rootPath, string pattern)
        {
            var foundFiles = new List<string>();

            try
            {
                // �ݹ��������.shp�ļ�
                var shpFiles = Directory.GetFiles(rootPath, "*.shp", SearchOption.AllDirectories);

                foreach (var file in shpFiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);

                    // ����ļ����Ƿ����ָ��ģʽ
                    if (fileName.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        foundFiles.Add(file);
                        System.Diagnostics.Debug.WriteLine($"�ҵ��ļ�: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"�����ļ�ʱ����: {ex.Message}");
                throw;
            }

            return foundFiles;
        }

        /// <summary>
        /// ����CZCDYDQC shapefile�ļ�
        /// </summary>
        /// <param name="rootPath">��Ŀ¼·��</param>
        /// <returns>�ҵ���CZCDYDQC shapefile�б�</returns>
        private List<string> FindCZCDYDQCShapefiles(string rootPath)
        {
            var foundFiles = new List<string>();

            try
            {
                // �ݹ��������.shp�ļ�
                var shpFiles = Directory.GetFiles(rootPath, "*.shp", SearchOption.AllDirectories);

                foreach (var file in shpFiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);

                    // ���ҷ���(��λ�ش���)CZCDYDQCģʽ���ļ�
                    if (Regex.IsMatch(fileName, @"^\(\d{6}\)CZCDYDQC$", RegexOptions.IgnoreCase))
                    {
                        foundFiles.Add(file);
                        System.Diagnostics.Debug.WriteLine($"�ҵ�CZCDYDQC�ļ�: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����CZCDYDQC�ļ�ʱ����: {ex.Message}");
                throw;
            }

            return foundFiles;
        }

        /// <summary>
        /// ��CZCDYDQC�ļ�������ȡ��λ�ش���
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns>�ش��룬���δ�ҵ��򷵻�null</returns>
        private string ExtractCountyCodeFromCZCDYDQCFileName(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            // ���������е���λ����
            var match = Regex.Match(fileName, @"^\((\d{6})\)CZCDYDQC$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        /// <summary>
        /// ���ļ�������ȡ��λ�ش���
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns>�ش��룬���δ�ҵ��򷵻�null</returns>
        private string ExtractCountyCodeFromFileName(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            // ���������е���λ����
            var match = Regex.Match(fileName, @"^\((\d{6})\)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // ���û���ҵ����Ÿ�ʽ�����Բ����ļ�����ͷ����λ���֣����ݾɸ�ʽ��
            match = Regex.Match(fileName, @"^(\d{6})");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        /// <summary>
        /// �����ҵ����ļ������ش������
        /// </summary>
        /// <param name="files">�ļ��б�</param>
        /// <param name="fileType">�ļ�����</param>
        private void ProcessFoundFiles(List<string> files, FileType fileType)
        {
            foreach (var file in files)
            {
                var countyCode = ExtractCountyCodeFromFileName(file);
                if (!string.IsNullOrEmpty(countyCode))
                {
                    if (!countyFileGroups.ContainsKey(countyCode))
                    {
                        countyFileGroups[countyCode] = new CountyFileGroup { CountyCode = countyCode };
                    }

                    // �����ļ�����������Ӧ���ļ�·��
                    switch (fileType)
                    {
                        case FileType.SLZYZC_DLTB:
                            countyFileGroups[countyCode].SlzyzcDltbFile = file;
                            break;
                        case FileType.CYZYZC_DLTB:
                            countyFileGroups[countyCode].CyzyzcDltbFile = file;
                            break;
                        case FileType.SDZYZC_DLTB:
                            countyFileGroups[countyCode].SdzyzcDltbFile = file;
                            break;
                        case FileType.CZCDYD:
                            countyFileGroups[countyCode].CzcdydFile = file;
                            break;
                    }

                    System.Diagnostics.Debug.WriteLine($"�ļ�����: {Path.GetFileName(file)} -> �ش���: {countyCode}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"����: �޷����ļ�����ȡ�ش���: {Path.GetFileName(file)}");
                }
            }
        }

        #endregion

        #region �ڲ����ö��

        /// <summary>
        /// �ļ�����ö��
        /// </summary>
        private enum FileType
        {
            SLZYZC_DLTB,
            CYZYZC_DLTB,
            SDZYZC_DLTB,
            CZCDYD
        }

        /// <summary>
        /// �ؼ��ļ�����Ϣ
        /// </summary>
        private class CountyFileGroup
        {
            public string CountyCode { get; set; }
            public string SlzyzcDltbFile { get; set; }
            public string CyzyzcDltbFile { get; set; }
            public string SdzyzcDltbFile { get; set; }
            public string CzcdydFile { get; set; }

            /// <summary>
            /// ����Ƿ����б�����ļ�������
            /// </summary>
            public bool IsComplete => !string.IsNullOrEmpty(SlzyzcDltbFile) &&
                                     !string.IsNullOrEmpty(CyzyzcDltbFile) &&
                                     !string.IsNullOrEmpty(SdzyzcDltbFile) &&
                                     !string.IsNullOrEmpty(CzcdydFile);
        }

        #endregion

        #region ����

        /// <summary>
        /// ��ȡѡ�е��ش����б�
        /// </summary>
        public List<string> SelectedCounties => new List<string>(selectedCounties);

        /// <summary>
        /// ��ȡ���ش��������ļ���Ϣ
        /// </summary>
        private Dictionary<string, CountyFileGroup> CountyFileGroups =>
            new Dictionary<string, CountyFileGroup>(countyFileGroups);

        /// <summary>
        /// ��ȡѡ���SLZYZC_DLTBԴ�ļ���·��
        /// </summary>
        public string SelectedSlzyzcPath => selectedSlzyzcPath;

        /// <summary>
        /// ��ȡѡ���CYZYZC_DLTBԴ�ļ���·��
        /// </summary>
        public string SelectedCyzyzcPath => selectedCyzyzcPath;

        /// <summary>
        /// ��ȡѡ���SDZYZC_DLTBԴ�ļ���·��
        /// </summary>
        public string SelectedSdzyzcPath => selectedSdzyzcPath;

        /// <summary>
        /// ��ȡѡ���CZCDYD����Դ�ļ���·��
        /// </summary>
        public string SelectedCzcdydPath => selectedCzcdydPath;

        /// <summary>
        /// ��ȡѡ������·��
        /// </summary>
        public string SelectedOutputPath => selectedOutputPath;

        /// <summary>
        /// ��ȡѡ��Ľ�����·��
        /// </summary>
        public string SelectedResultOutputPath => selectedResultOutputPath;

        #endregion
    }
}