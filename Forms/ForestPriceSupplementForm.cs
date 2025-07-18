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
    public partial class ForestPriceSupplementForm : Form
    {
        // ����·��
        private string inputDataPath = "";
        private string outputPath = "";

        // ���ݽṹ
        private List<CountyDataInfo> countyDataList = new List<CountyDataInfo>();
        
        // �۸�ӳ���
        private Dictionary<string, Dictionary<string, double>> priceMapping = new Dictionary<string, Dictionary<string, double>>();

        public ForestPriceSupplementForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // ��ʼ���ؼ�״̬
            btnScanData.Enabled = false;
            btnSupplementPrice.Enabled = false;
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
            dataGridViewCounties.AutoGenerateColumns = false;
            dataGridViewCounties.Columns.Clear();

            // ��Ӹ�ѡ����
            var checkColumn = new DataGridViewCheckBoxColumn
            {
                Name = "Selected",
                HeaderText = "ѡ��",
                Width = 50,
                DataPropertyName = "Selected"
            };
            dataGridViewCounties.Columns.Add(checkColumn);

            // ���������������
            var codeColumn = new DataGridViewTextBoxColumn
            {
                Name = "AdminCode",
                HeaderText = "����������",
                Width = 100,
                DataPropertyName = "AdminCode",
                ReadOnly = true
            };
            dataGridViewCounties.Columns.Add(codeColumn);

            // ���������������
            var nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "AdminName",
                HeaderText = "����������",
                Width = 120,
                DataPropertyName = "AdminName",
                ReadOnly = true
            };
            dataGridViewCounties.Columns.Add(nameColumn);

            // ���ͼ��������
            var totalFeaturesColumn = new DataGridViewTextBoxColumn
            {
                Name = "TotalFeatures",
                HeaderText = "ͼ������",
                Width = 80,
                DataPropertyName = "TotalFeatures",
                ReadOnly = true
            };
            dataGridViewCounties.Columns.Add(totalFeaturesColumn);

            // ���ȱʧ�۸�ͼ������
            var missingPriceFeaturesColumn = new DataGridViewTextBoxColumn
            {
                Name = "MissingPriceFeatures",
                HeaderText = "ȱʧ�۸�ͼ��",
                Width = 120,
                DataPropertyName = "MissingPriceFeatures",
                ReadOnly = true
            };
            dataGridViewCounties.Columns.Add(missingPriceFeaturesColumn);

            // ��������ļ�·����
            var dataPathColumn = new DataGridViewTextBoxColumn
            {
                Name = "DataPath",
                HeaderText = "�����ļ�·��",
                Width = 300,
                DataPropertyName = "DataPath",
                ReadOnly = true
            };
            dataGridViewCounties.Columns.Add(dataPathColumn);
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

            // ����ӳ��״̬
            UpdatePriceMappingDisplay();
        }

        #region �¼��������

        private void btnBrowseInputPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ�����۸������ļ���·������������LDHSJG���ݣ�";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    inputDataPath = dialog.SelectedPath;
                    txtInputPath.Text = inputDataPath;
                    txtInputPath.ForeColor = Color.DarkGreen;
                    UpdateButtonStates();
                }
            }
        }

        private void btnBrowseOutputPath_Click(object sender, EventArgs e)
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
                        // ��ʾ�������
                        statusLabel.Text = "���ڵ���۸�ӳ���...";
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();
                        
                        LoadPriceMappingFromFile(dialog.FileName);
                        
                        // �ָ�������
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        statusLabel.Text = "����";
                        
                        // ���¼۸�ӳ����ʾ
                        UpdatePriceMappingDisplay();
                        
                        MessageBox.Show($"�۸�ӳ�����ɹ���\n\n����ͳ�ƣ�\n- ������ {priceMapping.Count} ���������ļ۸�����", 
                            "����ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        UpdateButtonStates();
                    }
                    catch (Exception ex)
                    {
                        // �ָ�������
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        statusLabel.Text = "����ʧ��";
                        
                        MessageBox.Show($"����۸�ӳ���ʧ�ܣ�\n\n{ex.Message}", 
                            "����ʧ��", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnScanData_Click(object sender, EventArgs e)
        {
            try
            {
                statusLabel.Text = "����ɨ������...";
                progressBar.Style = ProgressBarStyle.Marquee;
                Application.DoEvents();

                countyDataList.Clear();
                
                // ɨ������·���µ��ļ���
                ScanCountyDataFolders();

                // ������������
                dataGridViewCounties.DataSource = new BindingList<CountyDataInfo>(countyDataList);

                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
                statusLabel.Text = $"ɨ����ɣ����ҵ� {countyDataList.Count} ���ص�����";

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                statusLabel.Text = "ɨ��ʧ��";
                MessageBox.Show($"ɨ������ʱ��������{ex.Message}", "����", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSupplementPrice_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedCounties = countyDataList.Where(c => c.Selected).ToList();

                if (selectedCounties.Count == 0)
                {
                    MessageBox.Show("������ѡ��һ���ؽ��д���", "��ʾ", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (priceMapping.Count == 0)
                {
                    MessageBox.Show("���ȵ���۸�ӳ���", "��ʾ", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                progressBar.Maximum = selectedCounties.Count;

                var globalStartTime = DateTime.Now;
                int totalProcessedFeatures = 0;
                int totalSupplementedFeatures = 0;

                for (int i = 0; i < selectedCounties.Count; i++)
                {
                    var county = selectedCounties[i];
                    
                    // ����Ԥ��ʣ��ʱ��
                    var elapsed = DateTime.Now - globalStartTime;
                    var estimatedTotal = i > 0 ? TimeSpan.FromTicks(elapsed.Ticks * selectedCounties.Count / i) : TimeSpan.Zero;
                    var estimatedRemaining = estimatedTotal - elapsed;
                    
                    var statusMessage = $"���ڴ���� {i + 1}/{selectedCounties.Count} ����: {county.AdminName}";
                    if (i > 0 && estimatedRemaining.TotalMinutes > 1)
                    {
                        statusMessage += $" - Ԥ��ʣ��: {estimatedRemaining:mm\\:ss}";
                    }
                    
                    statusLabel.Text = statusMessage;
                    Application.DoEvents();

                    try
                    {
                        var result = ProcessSingleCounty(county);
                        totalProcessedFeatures += result.ProcessedFeatures;
                        totalSupplementedFeatures += result.SupplementedFeatures;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"���� {county.AdminName} ʱ����: {ex.Message}");
                        statusLabel.Text = $"���� {county.AdminName} ʱ��������������һ��...";
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(2000);
                    }

                    progressBar.Value = i + 1;
                }

                var totalTime = DateTime.Now - globalStartTime;
                statusLabel.Text = $"�������ݴ�����ɣ�����ʱ: {totalTime:mm\\:ss}";
                MessageBox.Show($"�۸����ݲ�����ɣ�\n\n����ͳ�ƣ�\n- ����������{selectedCounties.Count}\n- ����ͼ��������{totalProcessedFeatures}\n- ����۸�ͼ������{totalSupplementedFeatures}\n- �ܴ���ʱ�䣺{totalTime:mm\\:ss}", 
                    "�������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
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

        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "CSV�ļ� (*.csv)|*.csv";
                    dialog.Title = "�����۸�ӳ���ģ��";
                    dialog.FileName = "�ֵؼ۸�ӳ���ģ��.csv";
                    
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportPriceMappingTemplate(dialog.FileName);
                        MessageBox.Show($"ģ���ѳɹ���������\n{dialog.FileName}", 
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
            btnScanData.Enabled = !string.IsNullOrEmpty(inputDataPath);
            
            btnSupplementPrice.Enabled = countyDataList.Any(c => c.Selected) &&
                                         !string.IsNullOrEmpty(outputPath) &&
                                         priceMapping.Count > 0;
        }

        private void ScanCountyDataFolders()
        {
            var directories = Directory.GetDirectories(inputDataPath);

            foreach (var dir in directories)
            {
                var dirName = System.IO.Path.GetFileName(dir);
                
                // ������6λ���ֿ�ͷ�Ұ���LDHSJG���ļ���
                if (dirName.Length >= 6 && dirName.Substring(0, 6).All(char.IsDigit) && dirName.Contains("LDHSJG"))
                {
                    var adminCode = dirName.Substring(0, 6);
                    var adminName = ExtractAdminNameFromFolderName(dirName);
                    
                    // ���Һ���۸�shapefile
                    var shapefilePath = FindLDHSJGShapefile(dir, adminCode);
                    
                    if (!string.IsNullOrEmpty(shapefilePath))
                    {
                        // ����shapefile��ȡͼ��ͳ����Ϣ
                        var (totalFeatures, missingPriceFeatures) = AnalyzeShapefileFeatures(shapefilePath);
                        
                        var countyInfo = new CountyDataInfo
                        {
                            AdminCode = adminCode,
                            AdminName = adminName,
                            Selected = true,
                            DataPath = shapefilePath,
                            TotalFeatures = totalFeatures,
                            MissingPriceFeatures = missingPriceFeatures
                        };

                        countyDataList.Add(countyInfo);
                        System.Diagnostics.Debug.WriteLine($"�ҵ�������: {adminName} ({adminCode}), ��ͼ��: {totalFeatures}, ȱʧ�۸�: {missingPriceFeatures}");
                    }
                }
            }
        }

        private string ExtractAdminNameFromFolderName(string folderName)
        {
            // ������"441226������LDHSJG"���ļ���������ȡ����
            if (folderName.Length > 6)
            {
                var nameWithSuffix = folderName.Substring(6);
                var name = nameWithSuffix.Replace("LDHSJG", "").Trim();
                return name;
            }
            return folderName;
        }

        private string FindLDHSJGShapefile(string folderPath, string adminCode)
        {
            try
            {
                // ���Ҹ�ʽΪ������������+LDHSJG.shp���ļ�
                var targetFileName = adminCode + "LDHSJG.shp";
                var targetPath = System.IO.Path.Combine(folderPath, targetFileName);
                
                if (File.Exists(targetPath))
                {
                    return targetPath;
                }

                // ���ֱ��ƥ�䲻��������ģ��ƥ��
                var shpFiles = Directory.GetFiles(folderPath, "*.shp");
                foreach (var file in shpFiles)
                {
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    if (fileName.Contains("LDHSJG") || fileName.Contains(adminCode))
                    {
                        return file;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����LDHSJG Shapefileʱ����{ex.Message}");
            }
            
            return "";
        }

        private (int totalFeatures, int missingPriceFeatures) AnalyzeShapefileFeatures(string shapefilePath)
        {
            IWorkspace workspace = null;
            IFeatureClass featureClass = null;
            
            try
            {
                Type workspaceFactoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                var workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(workspaceFactoryType);
                workspace = workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(shapefilePath), 0);
                featureClass = ((IFeatureWorkspace)workspace).OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(shapefilePath));

                int totalFeatures = featureClass.FeatureCount(null);
                int missingPriceFeatures = 0;

                // �����ֵؼ�����ؼ��ֵ�ƽ�����ֶ�
                int ldjbFieldIndex = featureClass.Fields.FindField("LDJB");
                int xjldpjjFieldIndex = featureClass.Fields.FindField("XJLDPJJ");

                if (ldjbFieldIndex >= 0 && xjldpjjFieldIndex >= 0)
                {
                    var cursor = featureClass.Search(null, false);
                    IFeature feature;
                    
                    while ((feature = cursor.NextFeature()) != null)
                    {
                        try
                        {
                            var ldjbValue = feature.get_Value(ldjbFieldIndex);
                            var xjldpjjValue = feature.get_Value(xjldpjjFieldIndex);

                            // ����Ƿ�Ϊ�ջ�0
                            bool ldjbIsEmpty = ldjbValue == null || string.IsNullOrEmpty(ldjbValue.ToString()) || ldjbValue.ToString() == "0";
                            bool xjldpjjIsZero = xjldpjjValue == null || Convert.ToDouble(xjldpjjValue) == 0.0;

                            if (ldjbIsEmpty || xjldpjjIsZero)
                            {
                                missingPriceFeatures++;
                            }
                        }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                        }
                    }
                    
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }

                return (totalFeatures, missingPriceFeatures);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����ShapefileҪ��ʱ����{ex.Message}");
                return (0, 0);
            }
            finally
            {
                if (featureClass != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                if (workspace != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workspace);
            }
        }

        private ProcessingResult ProcessSingleCounty(CountyDataInfo county)
        {
            var result = new ProcessingResult();
            
            try
            {
                // ��������ļ���
                var outputFolderName = "R" + System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(county.DataPath));
                var outputFolderPath = System.IO.Path.Combine(outputPath, outputFolderName);
                Directory.CreateDirectory(outputFolderPath);

                // �������Shapefile·��
                var outputShapefileName = System.IO.Path.GetFileNameWithoutExtension(county.DataPath) + ".shp";
                var outputShapefilePath = System.IO.Path.Combine(outputFolderPath, outputShapefileName);

                // ����۸񲹳�
                result = ProcessPriceSupplement(county.DataPath, outputShapefilePath, county);
            }
            catch (Exception ex)
            {
                throw new Exception($"���� {county.AdminName} ����ʱ����{ex.Message}");
            }

            return result;
        }

        private ProcessingResult ProcessPriceSupplement(string inputShpPath, string outputShpPath, CountyDataInfo county)
        {
            var result = new ProcessingResult();
            IWorkspace inputWorkspace = null;
            IWorkspace outputWorkspace = null;
            IFeatureClass inputFeatureClass = null;
            IFeatureClass outputFeatureClass = null;

            try
            {
                // ������Shapefile
                Type workspaceFactoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
                var workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(workspaceFactoryType);
                inputWorkspace = workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(inputShpPath), 0);
                inputFeatureClass = ((IFeatureWorkspace)inputWorkspace).OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(inputShpPath));

                // �������Shapefile
                outputFeatureClass = CreateOutputShapefile(outputShpPath, inputFeatureClass);

                // ����Ҫ�ز���۸�
                result = ProcessFeaturesForPriceSupplement(inputFeatureClass, outputFeatureClass, county);
            }
            finally
            {
                // �ͷ�COM����
                if (outputFeatureClass != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(outputFeatureClass);
                if (inputFeatureClass != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(inputFeatureClass);
                if (outputWorkspace != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(outputWorkspace);
                if (inputWorkspace != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(inputWorkspace);
            }

            return result;
        }

        private IFeatureClass CreateOutputShapefile(string outputPath, IFeatureClass templateFeatureClass)
        {
            Type workspaceFactoryType = Type.GetTypeFromProgID("esriDataSourcesFile.ShapefileWorkspaceFactory");
            var workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(workspaceFactoryType);
            var workspace = workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(outputPath), 0);
            var featureWorkspace = (IFeatureWorkspace)workspace;

            // ��������Ҫ������ֶνṹ
            var featureClass = featureWorkspace.CreateFeatureClass(
                System.IO.Path.GetFileNameWithoutExtension(outputPath),
                templateFeatureClass.Fields,
                null,
                null,
                esriFeatureType.esriFTSimple,
                templateFeatureClass.ShapeFieldName,
                "");

            return featureClass;
        }

        private ProcessingResult ProcessFeaturesForPriceSupplement(IFeatureClass inputFC, IFeatureClass outputFC, CountyDataInfo county)
        {
            var result = new ProcessingResult();
            var inputCursor = inputFC.Search(null, false);
            var outputCursor = outputFC.Insert(true);

            // ��ȡ�ֶ�����
            int ldjbFieldIndex = inputFC.Fields.FindField("LDJB");
            int xjldpjjFieldIndex = inputFC.Fields.FindField("XJLDPJJ");

            if (ldjbFieldIndex < 0 || xjldpjjFieldIndex < 0)
            {
                throw new Exception($"�� {county.AdminName} ��������δ�ҵ�������ֶ� LDJB �� XJLDPJJ");
            }

            // ��ȡ��ǰ�صļ۸�ӳ��
            var countyPriceMap = priceMapping.ContainsKey(county.AdminCode) ? priceMapping[county.AdminCode] : null;
            var (minPrice, minGrade) = GetMinimumPriceAndGrade(countyPriceMap);

            // ���ȸ���
            int totalFeatures = inputFC.FeatureCount(null);
            int processedFeatures = 0;
            var processingStartTime = DateTime.Now;
            var lastProgressUpdate = DateTime.Now;

            IFeature inputFeature;
            while ((inputFeature = inputCursor.NextFeature()) != null)
            {
                try
                {
                    var outputFeatureBuffer = outputFC.CreateFeatureBuffer();
                    
                    // ���������ֶ�ֵ
                    for (int i = 0; i < inputFeature.Fields.FieldCount; i++)
                    {
                        if (inputFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeOID &&
                            inputFeature.Fields.get_Field(i).Type != esriFieldType.esriFieldTypeGeometry)
                        {
                            outputFeatureBuffer.set_Value(i, inputFeature.get_Value(i));
                        }
                    }
                    
                    // ���Ƽ���ͼ��
                    outputFeatureBuffer.Shape = inputFeature.Shape;

                    // ��鲢����۸�����
                    var ldjbValue = inputFeature.get_Value(ldjbFieldIndex);
                    var xjldpjjValue = inputFeature.get_Value(xjldpjjFieldIndex);

                    bool ldjbIsEmpty = ldjbValue == null || string.IsNullOrEmpty(ldjbValue.ToString()) || ldjbValue.ToString() == "0";
                    bool xjldpjjIsZero = xjldpjjValue == null || Convert.ToDouble(xjldpjjValue) == 0.0;

                    if ((ldjbIsEmpty || xjldpjjIsZero) && minPrice > 0)
                    {
                        // ����۸�����
                        outputFeatureBuffer.set_Value(ldjbFieldIndex, minGrade);
                        outputFeatureBuffer.set_Value(xjldpjjFieldIndex, minPrice);
                        result.SupplementedFeatures++;
                    }

                    outputCursor.InsertFeature(outputFeatureBuffer);
                    result.ProcessedFeatures++;
                    processedFeatures++;

                    // ���½�����ʾ
                    var currentTime = DateTime.Now;
                    var shouldUpdateProgress = processedFeatures % 50 == 0 || 
                                             (currentTime - lastProgressUpdate).TotalSeconds >= 3 || 
                                             processedFeatures == totalFeatures;
                    
                    if (shouldUpdateProgress)
                    {
                        double currentProgress = (double)processedFeatures / totalFeatures * 100;
                        var elapsed = currentTime - processingStartTime;
                        var estimatedRemainingTime = processedFeatures > 0 ? 
                            TimeSpan.FromSeconds((elapsed.TotalSeconds / processedFeatures) * (totalFeatures - processedFeatures)) : 
                            TimeSpan.Zero;
                        
                        var statusMessage = $"���ڴ��� {county.AdminName} - ͼ�߽���: {processedFeatures}/{totalFeatures} ({currentProgress:F1}%)";
                        
                        if (processedFeatures > 0)
                        {
                            statusMessage += $" | �ٶ�: {(processedFeatures / elapsed.TotalMinutes):F1}��/����";
                            if (estimatedRemainingTime.TotalMinutes > 1)
                            {
                                statusMessage += $" | Ԥ��ʣ��: {estimatedRemainingTime:mm\\:ss}";
                            }
                        }
                        
                        if (result.SupplementedFeatures > 0)
                        {
                            statusMessage += $" | �Ѳ���: {result.SupplementedFeatures}��";
                        }
                        
                        try
                        {
                            statusLabel.Text = statusMessage;
                            if (processedFeatures % 200 == 0)
                            {
                                Application.DoEvents();
                            }
                        }
                        catch (Exception uiEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"UI�����쳣: {uiEx.Message}");
                        }
                        
                        lastProgressUpdate = currentTime;
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(outputFeatureBuffer);
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(inputFeature);
                }
            }

            outputCursor.Flush();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(outputCursor);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(inputCursor);

            // ��������ļ�
            CopyShapefileComponents(System.IO.Path.ChangeExtension(county.DataPath, null), 
                                   System.IO.Path.ChangeExtension(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(outputFC.FeatureDataset?.Workspace.PathName ?? ""), 
                                   ((IDataset)outputFC).Name), null));

            return result;
        }

        private void CopyShapefileComponents(string sourcePathWithoutExt, string targetPathWithoutExt)
        {
            string[] extensions = { ".shx", ".dbf", ".prj", ".cpg" };
            
            foreach (string ext in extensions)
            {
                string sourceFile = sourcePathWithoutExt + ext;
                string targetFile = targetPathWithoutExt + ext;
                
                if (File.Exists(sourceFile))
                {
                    try
                    {
                        File.Copy(sourceFile, targetFile, true);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"�����ļ� {ext} ʱ����: {ex.Message}");
                    }
                }
            }
        }

        private (double minPrice, string minGrade) GetMinimumPriceAndGrade(Dictionary<string, double> countyPriceMap)
        {
            if (countyPriceMap == null || countyPriceMap.Count == 0)
            {
                return (0, "");
            }

            var nonZeroPrices = countyPriceMap.Where(kvp => kvp.Value > 0);
            if (!nonZeroPrices.Any())
            {
                return (0, "");
            }

            var minPriceEntry = nonZeroPrices.OrderBy(kvp => kvp.Value).First();
            return (minPriceEntry.Value, minPriceEntry.Key);
        }

        private void LoadPriceMappingFromFile(string filePath)
        {
            priceMapping.Clear();
            
            try
            {
                // ʾ�����ݣ�ʵ����Ӧ���ļ���ȡ
                var sampleData = new Dictionary<string, Dictionary<string, double>>
                {
                    ["441226"] = new Dictionary<string, double> // ������ʾ��
                    {
                        ["1"] = 8.72, ["2"] = 6.45, ["3"] = 4.23, ["4"] = 3.15, ["5"] = 2.45
                    },
                    ["441322"] = new Dictionary<string, double> // ������ʾ��  
                    {
                        ["1"] = 7.89, ["2"] = 5.67, ["3"] = 3.98, ["4"] = 2.87, ["5"] = 2.12
                    },
                    ["441781"] = new Dictionary<string, double> // ½����ʾ��
                    {
                        ["1"] = 9.12, ["2"] = 6.88, ["3"] = 4.56, ["4"] = 3.33, ["5"] = 2.78
                    }
                };

                bool fileProcessed = false;
                
                if (File.Exists(filePath))
                {
                    string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();
                    
                    if (fileExtension == ".csv")
                    {
                        fileProcessed = ProcessCsvFile(filePath);
                    }
                }

                // ����ļ�����ʧ�ܣ�ʹ��ʾ������
                if (!fileProcessed || priceMapping.Count == 0)
                {
                    priceMapping = new Dictionary<string, Dictionary<string, double>>(sampleData);
                }
                
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
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
                
                throw new Exception($"�ļ�����ʧ��: {ex.Message}");
            }
        }

        private bool ProcessCsvFile(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8);
                
                if (lines.Length <= 2) return false;

                int successCount = 0;

                for (int i = 2; i < lines.Length; i++) // ����ǰ���б�ͷ
                {
                    var line = lines[i].Trim();
                    
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;
                    
                    var parts = line.Split(',', '\t');
                    if (parts.Length >= 7)
                    {
                        var adminCode = parts[1]?.Trim().Trim('"');
                        
                        if (!string.IsNullOrEmpty(adminCode) && adminCode.Length == 6 && adminCode.All(char.IsDigit))
                        {
                            var prices = new Dictionary<string, double>();
                            
                            for (int j = 2; j <= 6; j++)
                            {
                                if (double.TryParse(parts[j]?.Trim().Trim('"'), out double price) && price > 0)
                                {
                                    prices[(j - 1).ToString()] = price;
                                }
                            }
                            
                            if (prices.Count > 0)
                            {
                                priceMapping[adminCode] = prices;
                                successCount++;
                            }
                        }
                    }
                }

                return successCount > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CSV�ļ�����ʧ��: {ex.Message}");
                return false;
            }
        }

        private void ExportPriceMappingTemplate(string filePath)
        {
            var lines = new List<string>
            {
                "# �ֵؼ۸�ӳ���ģ��",
                "����������,����������,1���۸�,2���۸�,3���۸�,4���۸�,5���۸�",
                "������,441226,8.72,6.45,4.23,3.15,2.45",
                "������,441322,7.89,5.67,3.98,2.87,2.12",
                "½����,441781,9.12,6.88,4.56,3.33,2.78"
            };
            
            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }

        private void UpdatePriceMappingDisplay()
        {
            try
            {
                var displayData = new List<PriceMappingDisplayItem>();
                
                foreach (var mapping in priceMapping)
                {
                    var adminCode = mapping.Key;
                    var prices = mapping.Value;
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

                displayData = displayData.OrderBy(x => x.AdminCode).ToList();
                dataGridViewPriceMapping.DataSource = new BindingList<PriceMappingDisplayItem>(displayData);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"���¼۸�ӳ����ʾʱ����: {ex.Message}");
            }
        }

        private string GetAdminNameByCode(string adminCode)
        {
            // ���Դ��������б��в���
            var county = countyDataList.FirstOrDefault(c => c.AdminCode == adminCode);
            if (county != null)
            {
                return county.AdminName;
            }
            
            // ʹ�������Ĺ㶫ʡ������ӳ���
            var knownNames = new Dictionary<string, string>
            {
                ["441226"] = "������",
                ["441322"] = "������", 
                ["441781"] = "½����",
                // ���������Ӹ���������ӳ��
            };
            
            return knownNames.ContainsKey(adminCode) ? knownNames[adminCode] : $"δ֪({adminCode})";
        }

        #endregion
    }

    #region ���ݽṹ

    public class CountyDataInfo
    {
        public bool Selected { get; set; }
        public string AdminCode { get; set; }
        public string AdminName { get; set; }
        public string DataPath { get; set; }
        public int TotalFeatures { get; set; }
        public int MissingPriceFeatures { get; set; }
    }

    public class ProcessingResult
    {
        public int ProcessedFeatures { get; set; }
        public int SupplementedFeatures { get; set; }
    }

    #endregion
}