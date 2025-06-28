using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Carto;
using ForestResourcePlugin; // ���������ռ�����

namespace TestArcMapAddin2.Forms
{
    public partial class SurveyDataVerificationForm : Form
    {
        #region ˽���ֶ�
        private IFeatureClass surveyFeatureClass;
        private List<string> fieldNames = new List<string>();
        private DataTable filteredDataTable;
        private string selectedFilePath = string.Empty;
        private string selectedLandTypeField = string.Empty;
        private string selectedLandCodeField = string.Empty;
        private string selectedLandCategoryField = string.Empty;
        #endregion

        public SurveyDataVerificationForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        #region ��ʼ������
        private void InitializeForm()
        {
            // ��ʼ�����Ⱥ�״̬
            UpdateProgress(0);
            UpdateStatus("����");
            
            // ��ʼ������״̬
            btnDataProcess.Enabled = false;
            btnExport.Enabled = false;
        }
        #endregion

        #region ���Ⱥ�״̬���·���
        /// <summary>
        /// ���½�����
        /// </summary>
        /// <param name="value">����ֵ��0-100��</param>
        private void UpdateProgress(int value)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() => progressBar.Value = Math.Max(0, Math.Min(100, value))));
            }
            else
            {
                progressBar.Value = Math.Max(0, Math.Min(100, value));
            }
            Application.DoEvents();
        }

        /// <summary>
        /// ����״̬��ǩ
        /// </summary>
        /// <param name="status">״̬��Ϣ</param>
        private void UpdateStatus(string status)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() => lblStatus.Text = $"״̬��{status}"));
            }
            else
            {
                lblStatus.Text = $"״̬��{status}";
            }
            Application.DoEvents();
        }

        /// <summary>
        /// ���ý��Ⱥ�״̬
        /// </summary>
        private void ResetProgress()
        {
            UpdateProgress(0);
            UpdateStatus("����");
        }
        #endregion

        #region �¼�������
        private void BtnBrowseSurveyData_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "ѡ���ֲ�ʪ���ղ�����";
                dlg.Filter = "Shapefile�ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*";
                dlg.FilterIndex = 1;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    UpdateStatus("���ڼ����ļ�...");
                    UpdateProgress(20);

                    try
                    {
                        selectedFilePath = dlg.FileName;
                        txtSurveyDataPath.Text = selectedFilePath;

                        LoadShapefileFields(selectedFilePath);
                        
                        UpdateProgress(100);
                        UpdateStatus("�ļ��������");
                    }
                    catch (Exception ex)
                    {
                        ResetProgress();
                        UpdateStatus("�ļ�����ʧ��");
                        MessageBox.Show($"�����ļ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnLoadFromCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("���ڻ�ȡ��ͼͼ��...");
                UpdateProgress(10);

                var layers = MapLayerUtilities.GetPolygonLayers();
                if (layers.Count == 0)
                {
                    ResetProgress();
                    UpdateStatus("δ�ҵ�ͼ��");
                    MessageBox.Show("��ǰ��ͼ��û���ҵ������ͼ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                UpdateProgress(30);

                // �򻯰�ͼ��ѡ��Ի���
                using (Form layerForm = new Form())
                {
                    layerForm.Text = "ѡ��ͼ��";
                    layerForm.Size = new Size(300, 200);
                    layerForm.StartPosition = FormStartPosition.CenterParent;
                    layerForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    layerForm.MaximizeBox = false;
                    layerForm.MinimizeBox = false;

                    ListBox listBox = new ListBox
                    {
                        Location = new Point(10, 10),
                        Size = new Size(260, 120),
                        DisplayMember = "Name"
                    };

                    foreach (var layer in layers)
                    {
                        listBox.Items.Add(layer);
                    }

                    Button okButton = new Button
                    {
                        Text = "ȷ��",
                        Location = new Point(120, 140),
                        Size = new Size(75, 23),
                        DialogResult = DialogResult.OK
                    };

                    Button cancelButton = new Button
                    {
                        Text = "ȡ��",
                        Location = new Point(200, 140),
                        Size = new Size(75, 23),
                        DialogResult = DialogResult.Cancel
                    };

                    layerForm.Controls.AddRange(new Control[] { listBox, okButton, cancelButton });
                    layerForm.AcceptButton = okButton;
                    layerForm.CancelButton = cancelButton;

                    if (layerForm.ShowDialog() == DialogResult.OK && listBox.SelectedItem != null)
                    {
                        UpdateProgress(70);
                        UpdateStatus("���ڼ���ѡ���ͼ��...");

                        var selectedLayer = listBox.SelectedItem as LayerInfo;
                        if (selectedLayer != null)
                        {
                            surveyFeatureClass = selectedLayer.FeatureClass;
                            txtSurveyDataPath.Text = $"��ǰ��ͼͼ��: {selectedLayer.Name}";
                            selectedFilePath = selectedLayer.Name;

                            LoadFeatureClassFields(surveyFeatureClass);
                            
                            UpdateProgress(100);
                            UpdateStatus("ͼ��������");
                        }
                    }
                    else
                    {
                        ResetProgress();
                        UpdateStatus("�û�ȡ������");
                    }
                }
            }
            catch (Exception ex)
            {
                ResetProgress();
                UpdateStatus("����ͼ��ʧ��");
                MessageBox.Show($"�ӵ�ǰ��ͼ��������ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                if (surveyFeatureClass == null && string.IsNullOrEmpty(selectedFilePath))
                {
                    MessageBox.Show("����ѡ���ղ������ļ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboLandTypeField.SelectedItem == null || cboLandCodeField.SelectedItem == null || cboLandCategoryField.SelectedItem == null)
                {
                    MessageBox.Show("��ѡ�����б�����ֶ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateStatus("����׼������У��...");
                UpdateProgress(5);

                selectedLandTypeField = cboLandTypeField.SelectedItem.ToString();
                selectedLandCodeField = cboLandCodeField.SelectedItem.ToString();
                selectedLandCategoryField = cboLandCategoryField.SelectedItem.ToString();

                // ִ��У��
                PerformValidation();
            }
            catch (Exception ex)
            {
                ResetProgress();
                UpdateStatus("У��ʧ��");
                MessageBox.Show($"У������г���: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDataProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (filteredDataTable == null || filteredDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("����ִ������У�飬ȷ���в�һ�µ�������Ҫ����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ���ݴ�����
                var result = MessageBox.Show(
                    $"ȷ��Ҫ�� {filteredDataTable.Rows.Count} ����һ�¼�¼�������ݴ�����\n\n" +
                    "���ݴ����᣺\n" +
                    "1. ��������¼\n" +
                    "2. ���ɴ�����\n" +
                    "3. ������������",
                    "���ݴ���ȷ��",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UpdateStatus("����ִ�����ݴ���...");
                    UpdateProgress(10);
                    PerformDataProcessing();
                }
            }
            catch (Exception ex)
            {
                ResetProgress();
                UpdateStatus("���ݴ���ʧ��");
                MessageBox.Show($"���ݴ�������г���: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (filteredDataTable == null || filteredDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("û�����ݿɵ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "ѡ�񵼳�λ��";
                    dlg.Filter = "Excel�ļ� (*.xlsx)|*.xlsx|CSV�ļ� (*.csv)|*.csv";
                    dlg.FilterIndex = 1;
                    dlg.FileName = $"�ղ�����У����_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        UpdateStatus("���ڵ�������...");
                        UpdateProgress(20);

                        ExportDataTable(filteredDataTable, dlg.FileName);
                        
                        UpdateProgress(100);
                        UpdateStatus("�������");
                        MessageBox.Show($"�����ѳɹ�������: {dlg.FileName}", "�������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ResetProgress();
                UpdateStatus("����ʧ��");
                MessageBox.Show($"���������г���: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ���ݴ�����
        private void PerformDataProcessing()
        {
            try
            {
                txtStatisticsResult.Text += "\r\n����ִ�����ݴ���...\r\n";
                UpdateProgress(20);
                
                // ģ�����ݴ������
                for (int i = 0; i < filteredDataTable.Rows.Count; i++)
                {
                    DataRow row = filteredDataTable.Rows[i];
                    
                    // ��Ӵ���״̬�У���������ڣ�
                    if (!filteredDataTable.Columns.Contains("����״̬"))
                    {
                        filteredDataTable.Columns.Add("����״̬", typeof(string));
                    }
                    if (!filteredDataTable.Columns.Contains("������"))
                    {
                        filteredDataTable.Columns.Add("������", typeof(string));
                    }
                    
                    // Ϊÿ����¼���ɴ�����
                    row["����״̬"] = "�ѱ��";
                    
                    var landTypeValue = row[selectedLandTypeField]?.ToString() ?? "";
                    var landCodeValue = row[selectedLandCodeField]?.ToString() ?? "";
                    
                    if (landTypeValue.Length >= 2 && landCodeValue.Length >= 2)
                    {
                        // ����ǰ��λ�жϴ�����
                        string prefix = landTypeValue.Substring(0, 2);
                        switch (prefix)
                        {
                            case "01":
                                row["������"] = "�����ʵ���ط��࣬ͳһ�����ʽ";
                                break;
                            case "02":
                                row["������"] = "�����ʵ԰�����ͣ�ȷ�ϵ������";
                                break;
                            case "03":
                                row["������"] = "�����ʵ�ֵ�Ȩ����ͳһ�����׼";
                                break;
                            case "04":
                                row["������"] = "�����ʵ�ݵ����÷�ʽ��ȷ�ϱ���";
                                break;
                            default:
                                row["������"] = "�����˹���ʵ��ȷ����ȷ�������";
                                break;
                        }
                    }
                    else
                    {
                        row["������"] = "�����ʽ�쳣����������¼��";
                    }

                    // ���½���
                    int progress = 20 + (i * 70 / filteredDataTable.Rows.Count);
                    UpdateProgress(progress);
                    UpdateStatus($"���ڴ����¼ {i + 1}/{filteredDataTable.Rows.Count}...");
                }
                
                UpdateProgress(95);
                UpdateStatus("����ˢ����ʾ...");
                
                // ˢ��Ԥ�����
                dgvPreview.DataSource = null;
                dgvPreview.DataSource = filteredDataTable;
                
                // ����ͳ����Ϣ
                txtStatisticsResult.Text += $"���ݴ�����ɣ�\r\n";
                txtStatisticsResult.Text += $"�Ѵ����¼��: {filteredDataTable.Rows.Count} ��\r\n";
                txtStatisticsResult.Text += $"����ʱ��: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n";
                txtStatisticsResult.Text += "���������¼�ѱ�ǲ����ɴ����顣\r\n";
                
                UpdateProgress(100);
                UpdateStatus("���ݴ������");
                
                MessageBox.Show("���ݴ�����ɣ���Ϊ���в�һ�¼�¼���ɴ����顣", "�������", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                txtStatisticsResult.Text += $"���ݴ������: {ex.Message}\r\n";
                throw;
            }
        }

        private void LoadShapefileFields(string shapefilePath)
        {
            try
            {
                UpdateStatus("���ڶ�ȡ�ļ��ֶ�...");
                UpdateProgress(40);

                // �򻯰汾��ֱ��ͨ��ArcObjects��ȡShapefile�ֶ�
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(
                    System.IO.Path.GetDirectoryName(shapefilePath), 0);
                
                string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);
                
                UpdateProgress(70);
                LoadFeatureClassFields(featureClass);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ȡShapefile�ֶ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetProgress();
                UpdateStatus("��ȡ�ֶ�ʧ��");
            }
        }

        private void LoadFeatureClassFields(IFeatureClass featureClass)
        {
            try
            {
                UpdateStatus("���ڼ����ֶ���Ϣ...");
                
                fieldNames.Clear();
                IFields fields = featureClass.Fields;
                
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    if (field.Type == esriFieldType.esriFieldTypeString || 
                        field.Type == esriFieldType.esriFieldTypeInteger ||
                        field.Type == esriFieldType.esriFieldTypeDouble)
                    {
                        fieldNames.Add(field.Name);
                    }
                }

                PopulateFieldComboBoxes();
                UpdateStatus($"�Ѽ��� {fieldNames.Count} ���ֶ�");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ȡҪ�����ֶ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetProgress();
                UpdateStatus("�����ֶ�ʧ��");
            }
        }

        private void PopulateFieldComboBoxes()
        {
            // ֱ��ʹ�ÿؼ�����
            cboLandTypeField.Items.Clear();
            cboLandCodeField.Items.Clear();
            cboLandCategoryField.Items.Clear();

            foreach (string fieldName in fieldNames)
            {
                cboLandTypeField.Items.Add(fieldName);
                cboLandCodeField.Items.Add(fieldName);
                cboLandCategoryField.Items.Add(fieldName);
            }
        }

        private void PerformValidation()
        {
            try
            {
                UpdateStatus("���ڼ�������...");
                UpdateProgress(10);

                DataTable allData;
                
                if (surveyFeatureClass != null)
                {
                    allData = ConvertFeatureClassToDataTable(surveyFeatureClass);
                }
                else
                {
                    // ͨ��ArcObjects��ȡShapefile����
                    IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                    IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(
                        System.IO.Path.GetDirectoryName(selectedFilePath), 0);
                    
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(selectedFilePath);
                    IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);
                    allData = ConvertFeatureClassToDataTable(featureClass);
                }

                UpdateProgress(30);
                UpdateStatus("����ִ�����ݱȶ�...");

                // ɸѡ��һ�µļ�¼
                var inconsistentRecords = new List<DataRow>();
                var statistics = new Dictionary<string, int>();

                int totalRows = allData.Rows.Count;
                for (int i = 0; i < totalRows; i++)
                {
                    DataRow row = allData.Rows[i];
                    var landTypeValue = row[selectedLandTypeField]?.ToString() ?? "";
                    var landCodeValue = row[selectedLandCodeField]?.ToString() ?? "";

                    // ����������͵�������Ƿ�һ��
                    if (!string.IsNullOrEmpty(landTypeValue) && !string.IsNullOrEmpty(landCodeValue))
                    {
                        // ����򻯴���ʵ��Ӧ���ݾ���ı��������бȽ�
                        if (!landTypeValue.Equals(landCodeValue, StringComparison.OrdinalIgnoreCase))
                        {
                            inconsistentRecords.Add(row);

                            // ����������ǰ��λ����ͳ��
                            string prefix = landTypeValue.Length >= 2 ? landTypeValue.Substring(0, 2) : landTypeValue;
                            if (statistics.ContainsKey(prefix))
                                statistics[prefix]++;
                            else
                                statistics[prefix] = 1;
                        }
                    }

                    // ���½���
                    if (i % 100 == 0 || i == totalRows - 1)
                    {
                        int progress = 30 + (i * 40 / totalRows);
                        UpdateProgress(progress);
                        UpdateStatus($"���ڱȶ����ݣ�{i + 1}/{totalRows}");
                    }
                }

                UpdateProgress(80);
                UpdateStatus("�������ɽ��...");

                // ��ʾͳ�ƽ��
                DisplayStatistics(inconsistentRecords.Count, statistics);

                // ����Ԥ�����ݱ�
                CreatePreviewDataTable(inconsistentRecords, allData);

                // ������ذ�ť
                btnDataProcess.Enabled = inconsistentRecords.Count > 0;
                btnExport.Enabled = inconsistentRecords.Count > 0;

                UpdateProgress(100);
                UpdateStatus($"У����ɣ����� {inconsistentRecords.Count} ����һ�¼�¼");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ִ��У��ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetProgress();
                UpdateStatus("У��ʧ��");
            }
        }

        private DataTable ConvertFeatureClassToDataTable(IFeatureClass featureClass)
        {
            DataTable dataTable = new DataTable();
            
            // �����
            IFields fields = featureClass.Fields;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                IField field = fields.get_Field(i);
                if (field.Type != esriFieldType.esriFieldTypeGeometry)
                {
                    Type dataType = GetDataType(field.Type);
                    dataTable.Columns.Add(field.Name, dataType);
                }
            }

            // �������
            IFeatureCursor cursor = featureClass.Search(null, false);
            IFeature feature;
            while ((feature = cursor.NextFeature()) != null)
            {
                DataRow row = dataTable.NewRow();
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    if (field.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        row[field.Name] = feature.get_Value(i) ?? DBNull.Value;
                    }
                }
                dataTable.Rows.Add(row);
            }
            
            System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            return dataTable;
        }

        private Type GetDataType(esriFieldType fieldType)
        {
            switch (fieldType)
            {
                case esriFieldType.esriFieldTypeString:
                    return typeof(string);
                case esriFieldType.esriFieldTypeInteger:
                    return typeof(int);
                case esriFieldType.esriFieldTypeDouble:
                    return typeof(double);
                case esriFieldType.esriFieldTypeDate:
                    return typeof(DateTime);
                default:
                    return typeof(object);
            }
        }

        private void DisplayStatistics(int totalInconsistent, Dictionary<string, int> categoryStats)
        {
            // ֱ��ʹ�ÿؼ�����
            var result = $"����У����ɣ�\r\n\r\n";
            result += $"���ֲ�һ�¼�¼����: {totalInconsistent} ��\r\n\r\n";
            
            if (categoryStats.Count > 0)
            {
                result += "����������ǰ��λ����ͳ��:\r\n";
                foreach (var category in categoryStats.OrderBy(x => x.Key))
                {
                    result += $"  {category.Key}: {category.Value} ��\r\n";
                }
            }
            else
            {
                result += "���м�¼�Ĺ�������͵�����붼һ�¡�\r\n";
            }

            txtStatisticsResult.Text = result; // ֱ��ʹ�ÿؼ�����
        }

        private void CreatePreviewDataTable(List<DataRow> inconsistentRecords, DataTable sourceTable)
        {
            filteredDataTable = new DataTable();
            
            // �����Ҫ��ʾ���У�BSM + �û�ѡ���3���ֶ�
            var requiredColumns = new[] { "BSM", selectedLandTypeField, selectedLandCodeField, selectedLandCategoryField };
            
            foreach (string columnName in requiredColumns)
            {
                if (sourceTable.Columns.Contains(columnName))
                {
                    Type columnType = sourceTable.Columns[columnName].DataType;
                    filteredDataTable.Columns.Add(columnName, columnType);
                }
            }

            // �������
            foreach (DataRow sourceRow in inconsistentRecords)
            {
                DataRow newRow = filteredDataTable.NewRow();
                foreach (string columnName in requiredColumns)
                {
                    if (sourceTable.Columns.Contains(columnName) && filteredDataTable.Columns.Contains(columnName))
                    {
                        newRow[columnName] = sourceRow[columnName];
                    }
                }
                filteredDataTable.Rows.Add(newRow);
            }

            // �󶨵�DataGridView - ֱ��ʹ�ÿؼ�����
            dgvPreview.DataSource = filteredDataTable;
        }

        private void ExportDataTable(DataTable dataTable, string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            
            if (extension == ".csv")
            {
                ExportToCsv(dataTable, filePath);
            }
            else if (extension == ".xlsx")
            {
                ExportToExcel(dataTable, filePath);
            }
        }

        private void ExportToCsv(DataTable dataTable, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                // д���б���
                string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                writer.WriteLine(string.Join(",", columnNames));

                // д��������
                foreach (DataRow row in dataTable.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => $"\"{field}\"").ToArray();
                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }

        private void ExportToExcel(DataTable dataTable, string filePath)
        {
            // �򻯰�Excel������ʵ����Ŀ�п�����Ҫʹ��EPPlus�������⣩
            // ����תΪCSV��ʽ��ʹ��.xlsx��չ����Ϊʾ��
            ExportToCsv(dataTable, filePath);
        }
        #endregion
    }
}