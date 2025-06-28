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
            // �Ƴ� InitializeCustomControls() ���ã���Ϊ�ؼ����������������
        }

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
                    selectedFilePath = dlg.FileName;
                    txtSurveyDataPath.Text = selectedFilePath; // ֱ��ʹ�ÿؼ�����

                    LoadShapefileFields(selectedFilePath);
                }
            }
        }

        private void BtnLoadFromCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                var layers = MapLayerUtilities.GetPolygonLayers();
                if (layers.Count == 0)
                {
                    MessageBox.Show("��ǰ��ͼ��û���ҵ������ͼ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

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
                        var selectedLayer = listBox.SelectedItem as LayerInfo;
                        if (selectedLayer != null)
                        {
                            surveyFeatureClass = selectedLayer.FeatureClass;
                            txtSurveyDataPath.Text = $"��ǰ��ͼͼ��: {selectedLayer.Name}"; // ֱ��ʹ�ÿؼ�����
                            selectedFilePath = selectedLayer.Name;

                            LoadFeatureClassFields(surveyFeatureClass);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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

                // ֱ��ʹ�ÿؼ�����
                if (cboLandTypeField.SelectedItem == null || cboLandCodeField.SelectedItem == null || cboLandCategoryField.SelectedItem == null)
                {
                    MessageBox.Show("��ѡ�����б�����ֶ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                selectedLandTypeField = cboLandTypeField.SelectedItem.ToString();
                selectedLandCodeField = cboLandCodeField.SelectedItem.ToString();
                selectedLandCategoryField = cboLandCategoryField.SelectedItem.ToString();

                // ִ��У��
                PerformValidation();
            }
            catch (Exception ex)
            {
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
                    PerformDataProcessing();
                }
            }
            catch (Exception ex)
            {
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
                        ExportDataTable(filteredDataTable, dlg.FileName);
                        MessageBox.Show($"�����ѳɹ�������: {dlg.FileName}", "�������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
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
                }
                
                // ˢ��Ԥ�����
                dgvPreview.DataSource = null;
                dgvPreview.DataSource = filteredDataTable;
                
                // ����ͳ����Ϣ
                txtStatisticsResult.Text += $"���ݴ�����ɣ�\r\n";
                txtStatisticsResult.Text += $"�Ѵ����¼��: {filteredDataTable.Rows.Count} ��\r\n";
                txtStatisticsResult.Text += $"����ʱ��: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n";
                txtStatisticsResult.Text += "���������¼�ѱ�ǲ����ɴ����顣\r\n";
                
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
                // �򻯰汾��ֱ��ͨ��ArcObjects��ȡShapefile�ֶ�
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(
                    System.IO.Path.GetDirectoryName(shapefilePath), 0);
                
                string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);
                
                LoadFeatureClassFields(featureClass);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ȡShapefile�ֶ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFeatureClassFields(IFeatureClass featureClass)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ȡҪ�����ֶ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // ɸѡ��һ�µļ�¼
                var inconsistentRecords = new List<DataRow>();
                var statistics = new Dictionary<string, int>();

                foreach (DataRow row in allData.Rows)
                {
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
                }

                // ��ʾͳ�ƽ��
                DisplayStatistics(inconsistentRecords.Count, statistics);

                // ����Ԥ�����ݱ�
                CreatePreviewDataTable(inconsistentRecords, allData);

                // ���õ�����ť
                btnExport.Enabled = inconsistentRecords.Count > 0; // ֱ��ʹ�ÿؼ�����
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ִ��У��ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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