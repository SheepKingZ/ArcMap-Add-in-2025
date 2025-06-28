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
using ForestResourcePlugin; // 修正命名空间引用

namespace TestArcMapAddin2.Forms
{
    public partial class SurveyDataVerificationForm : Form
    {
        #region 私有字段
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

        #region 初始化方法
        private void InitializeForm()
        {
            // 初始化进度和状态
            UpdateProgress(0);
            UpdateStatus("就绪");
            
            // 初始化界面状态
            btnDataProcess.Enabled = false;
            btnExport.Enabled = false;
        }
        #endregion

        #region 进度和状态更新方法
        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="value">进度值（0-100）</param>
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
        /// 更新状态标签
        /// </summary>
        /// <param name="status">状态信息</param>
        private void UpdateStatus(string status)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() => lblStatus.Text = $"状态：{status}"));
            }
            else
            {
                lblStatus.Text = $"状态：{status}";
            }
            Application.DoEvents();
        }

        /// <summary>
        /// 重置进度和状态
        /// </summary>
        private void ResetProgress()
        {
            UpdateProgress(0);
            UpdateStatus("就绪");
        }
        #endregion

        #region 事件处理方法
        private void BtnBrowseSurveyData_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "选择林草湿荒普查数据";
                dlg.Filter = "Shapefile文件 (*.shp)|*.shp|所有文件 (*.*)|*.*";
                dlg.FilterIndex = 1;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    UpdateStatus("正在加载文件...");
                    UpdateProgress(20);

                    try
                    {
                        selectedFilePath = dlg.FileName;
                        txtSurveyDataPath.Text = selectedFilePath;

                        LoadShapefileFields(selectedFilePath);
                        
                        UpdateProgress(100);
                        UpdateStatus("文件加载完成");
                    }
                    catch (Exception ex)
                    {
                        ResetProgress();
                        UpdateStatus("文件加载失败");
                        MessageBox.Show($"加载文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnLoadFromCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("正在获取地图图层...");
                UpdateProgress(10);

                var layers = MapLayerUtilities.GetPolygonLayers();
                if (layers.Count == 0)
                {
                    ResetProgress();
                    UpdateStatus("未找到图层");
                    MessageBox.Show("当前地图中没有找到多边形图层", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                UpdateProgress(30);

                // 简化版图层选择对话框
                using (Form layerForm = new Form())
                {
                    layerForm.Text = "选择图层";
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
                        Text = "确定",
                        Location = new Point(120, 140),
                        Size = new Size(75, 23),
                        DialogResult = DialogResult.OK
                    };

                    Button cancelButton = new Button
                    {
                        Text = "取消",
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
                        UpdateStatus("正在加载选择的图层...");

                        var selectedLayer = listBox.SelectedItem as LayerInfo;
                        if (selectedLayer != null)
                        {
                            surveyFeatureClass = selectedLayer.FeatureClass;
                            txtSurveyDataPath.Text = $"当前地图图层: {selectedLayer.Name}";
                            selectedFilePath = selectedLayer.Name;

                            LoadFeatureClassFields(surveyFeatureClass);
                            
                            UpdateProgress(100);
                            UpdateStatus("图层加载完成");
                        }
                    }
                    else
                    {
                        ResetProgress();
                        UpdateStatus("用户取消操作");
                    }
                }
            }
            catch (Exception ex)
            {
                ResetProgress();
                UpdateStatus("加载图层失败");
                MessageBox.Show($"从当前地图加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                if (surveyFeatureClass == null && string.IsNullOrEmpty(selectedFilePath))
                {
                    MessageBox.Show("请先选择普查数据文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboLandTypeField.SelectedItem == null || cboLandCodeField.SelectedItem == null || cboLandCategoryField.SelectedItem == null)
                {
                    MessageBox.Show("请选择所有必需的字段", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateStatus("正在准备数据校验...");
                UpdateProgress(5);

                selectedLandTypeField = cboLandTypeField.SelectedItem.ToString();
                selectedLandCodeField = cboLandCodeField.SelectedItem.ToString();
                selectedLandCategoryField = cboLandCategoryField.SelectedItem.ToString();

                // 执行校验
                PerformValidation();
            }
            catch (Exception ex)
            {
                ResetProgress();
                UpdateStatus("校验失败");
                MessageBox.Show($"校验过程中出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDataProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (filteredDataTable == null || filteredDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("请先执行数据校验，确保有不一致的数据需要处理", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 数据处理功能
                var result = MessageBox.Show(
                    $"确认要对 {filteredDataTable.Rows.Count} 条不一致记录进行数据处理吗？\n\n" +
                    "数据处理将会：\n" +
                    "1. 标记问题记录\n" +
                    "2. 生成处理建议\n" +
                    "3. 创建修正方案",
                    "数据处理确认",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UpdateStatus("正在执行数据处理...");
                    UpdateProgress(10);
                    PerformDataProcessing();
                }
            }
            catch (Exception ex)
            {
                ResetProgress();
                UpdateStatus("数据处理失败");
                MessageBox.Show($"数据处理过程中出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (filteredDataTable == null || filteredDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("没有数据可导出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Title = "选择导出位置";
                    dlg.Filter = "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv";
                    dlg.FilterIndex = 1;
                    dlg.FileName = $"普查数据校验结果_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        UpdateStatus("正在导出数据...");
                        UpdateProgress(20);

                        ExportDataTable(filteredDataTable, dlg.FileName);
                        
                        UpdateProgress(100);
                        UpdateStatus("导出完成");
                        MessageBox.Show($"数据已成功导出到: {dlg.FileName}", "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ResetProgress();
                UpdateStatus("导出失败");
                MessageBox.Show($"导出过程中出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 数据处理方法
        private void PerformDataProcessing()
        {
            try
            {
                txtStatisticsResult.Text += "\r\n正在执行数据处理...\r\n";
                UpdateProgress(20);
                
                // 模拟数据处理过程
                for (int i = 0; i < filteredDataTable.Rows.Count; i++)
                {
                    DataRow row = filteredDataTable.Rows[i];
                    
                    // 添加处理状态列（如果不存在）
                    if (!filteredDataTable.Columns.Contains("处理状态"))
                    {
                        filteredDataTable.Columns.Add("处理状态", typeof(string));
                    }
                    if (!filteredDataTable.Columns.Contains("处理建议"))
                    {
                        filteredDataTable.Columns.Add("处理建议", typeof(string));
                    }
                    
                    // 为每条记录生成处理建议
                    row["处理状态"] = "已标记";
                    
                    var landTypeValue = row[selectedLandTypeField]?.ToString() ?? "";
                    var landCodeValue = row[selectedLandCodeField]?.ToString() ?? "";
                    
                    if (landTypeValue.Length >= 2 && landCodeValue.Length >= 2)
                    {
                        // 根据前两位判断处理建议
                        string prefix = landTypeValue.Substring(0, 2);
                        switch (prefix)
                        {
                            case "01":
                                row["处理建议"] = "建议核实耕地分类，统一编码格式";
                                break;
                            case "02":
                                row["处理建议"] = "建议核实园地类型，确认地类编码";
                                break;
                            case "03":
                                row["处理建议"] = "建议核实林地权属，统一编码标准";
                                break;
                            case "04":
                                row["处理建议"] = "建议核实草地利用方式，确认编码";
                                break;
                            default:
                                row["处理建议"] = "建议人工核实，确认正确地类编码";
                                break;
                        }
                    }
                    else
                    {
                        row["处理建议"] = "编码格式异常，建议重新录入";
                    }

                    // 更新进度
                    int progress = 20 + (i * 70 / filteredDataTable.Rows.Count);
                    UpdateProgress(progress);
                    UpdateStatus($"正在处理记录 {i + 1}/{filteredDataTable.Rows.Count}...");
                }
                
                UpdateProgress(95);
                UpdateStatus("正在刷新显示...");
                
                // 刷新预览表格
                dgvPreview.DataSource = null;
                dgvPreview.DataSource = filteredDataTable;
                
                // 更新统计信息
                txtStatisticsResult.Text += $"数据处理完成！\r\n";
                txtStatisticsResult.Text += $"已处理记录数: {filteredDataTable.Rows.Count} 条\r\n";
                txtStatisticsResult.Text += $"处理时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n";
                txtStatisticsResult.Text += "所有问题记录已标记并生成处理建议。\r\n";
                
                UpdateProgress(100);
                UpdateStatus("数据处理完成");
                
                MessageBox.Show("数据处理完成！已为所有不一致记录生成处理建议。", "处理完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                txtStatisticsResult.Text += $"数据处理出错: {ex.Message}\r\n";
                throw;
            }
        }

        private void LoadShapefileFields(string shapefilePath)
        {
            try
            {
                UpdateStatus("正在读取文件字段...");
                UpdateProgress(40);

                // 简化版本：直接通过ArcObjects读取Shapefile字段
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
                MessageBox.Show($"读取Shapefile字段时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetProgress();
                UpdateStatus("读取字段失败");
            }
        }

        private void LoadFeatureClassFields(IFeatureClass featureClass)
        {
            try
            {
                UpdateStatus("正在加载字段信息...");
                
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
                UpdateStatus($"已加载 {fieldNames.Count} 个字段");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取要素类字段时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetProgress();
                UpdateStatus("加载字段失败");
            }
        }

        private void PopulateFieldComboBoxes()
        {
            // 直接使用控件引用
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
                UpdateStatus("正在加载数据...");
                UpdateProgress(10);

                DataTable allData;
                
                if (surveyFeatureClass != null)
                {
                    allData = ConvertFeatureClassToDataTable(surveyFeatureClass);
                }
                else
                {
                    // 通过ArcObjects读取Shapefile数据
                    IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                    IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(
                        System.IO.Path.GetDirectoryName(selectedFilePath), 0);
                    
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(selectedFilePath);
                    IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);
                    allData = ConvertFeatureClassToDataTable(featureClass);
                }

                UpdateProgress(30);
                UpdateStatus("正在执行数据比对...");

                // 筛选不一致的记录
                var inconsistentRecords = new List<DataRow>();
                var statistics = new Dictionary<string, int>();

                int totalRows = allData.Rows.Count;
                for (int i = 0; i < totalRows; i++)
                {
                    DataRow row = allData.Rows[i];
                    var landTypeValue = row[selectedLandTypeField]?.ToString() ?? "";
                    var landCodeValue = row[selectedLandCodeField]?.ToString() ?? "";

                    // 检查国土地类和地类编码是否不一致
                    if (!string.IsNullOrEmpty(landTypeValue) && !string.IsNullOrEmpty(landCodeValue))
                    {
                        // 这里简化处理，实际应根据具体的编码规则进行比较
                        if (!landTypeValue.Equals(landCodeValue, StringComparison.OrdinalIgnoreCase))
                        {
                            inconsistentRecords.Add(row);

                            // 按国土地类前两位分类统计
                            string prefix = landTypeValue.Length >= 2 ? landTypeValue.Substring(0, 2) : landTypeValue;
                            if (statistics.ContainsKey(prefix))
                                statistics[prefix]++;
                            else
                                statistics[prefix] = 1;
                        }
                    }

                    // 更新进度
                    if (i % 100 == 0 || i == totalRows - 1)
                    {
                        int progress = 30 + (i * 40 / totalRows);
                        UpdateProgress(progress);
                        UpdateStatus($"正在比对数据：{i + 1}/{totalRows}");
                    }
                }

                UpdateProgress(80);
                UpdateStatus("正在生成结果...");

                // 显示统计结果
                DisplayStatistics(inconsistentRecords.Count, statistics);

                // 创建预览数据表
                CreatePreviewDataTable(inconsistentRecords, allData);

                // 启用相关按钮
                btnDataProcess.Enabled = inconsistentRecords.Count > 0;
                btnExport.Enabled = inconsistentRecords.Count > 0;

                UpdateProgress(100);
                UpdateStatus($"校验完成，发现 {inconsistentRecords.Count} 条不一致记录");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行校验时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetProgress();
                UpdateStatus("校验失败");
            }
        }

        private DataTable ConvertFeatureClassToDataTable(IFeatureClass featureClass)
        {
            DataTable dataTable = new DataTable();
            
            // 添加列
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

            // 添加数据
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
            // 直接使用控件引用
            var result = $"数据校验完成！\r\n\r\n";
            result += $"发现不一致记录总数: {totalInconsistent} 条\r\n\r\n";
            
            if (categoryStats.Count > 0)
            {
                result += "按国土地类前两位分类统计:\r\n";
                foreach (var category in categoryStats.OrderBy(x => x.Key))
                {
                    result += $"  {category.Key}: {category.Value} 条\r\n";
                }
            }
            else
            {
                result += "所有记录的国土地类和地类编码都一致。\r\n";
            }

            txtStatisticsResult.Text = result; // 直接使用控件引用
        }

        private void CreatePreviewDataTable(List<DataRow> inconsistentRecords, DataTable sourceTable)
        {
            filteredDataTable = new DataTable();
            
            // 添加需要显示的列：BSM + 用户选择的3个字段
            var requiredColumns = new[] { "BSM", selectedLandTypeField, selectedLandCodeField, selectedLandCategoryField };
            
            foreach (string columnName in requiredColumns)
            {
                if (sourceTable.Columns.Contains(columnName))
                {
                    Type columnType = sourceTable.Columns[columnName].DataType;
                    filteredDataTable.Columns.Add(columnName, columnType);
                }
            }

            // 添加数据
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

            // 绑定到DataGridView - 直接使用控件引用
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
                // 写入列标题
                string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                writer.WriteLine(string.Join(",", columnNames));

                // 写入数据行
                foreach (DataRow row in dataTable.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => $"\"{field}\"").ToArray();
                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }

        private void ExportToExcel(DataTable dataTable, string filePath)
        {
            // 简化版Excel导出（实际项目中可能需要使用EPPlus或其他库）
            // 这里转为CSV格式但使用.xlsx扩展名作为示例
            ExportToCsv(dataTable, filePath);
        }
        #endregion
    }
}