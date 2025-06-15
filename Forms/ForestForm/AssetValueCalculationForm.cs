using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace TestArcMapAddin2.Forms.ForestForm
{
    /// <summary>
    /// Form for calculating forest resource asset values based on the 7-step process
    /// </summary>
    public partial class AssetValueCalculationForm : ForestProcessingFormBase
    {
        // Processing state variables
        private bool processingCancelled = false;
        private string workingDirectory = string.Empty;
        private string exportDirectory = string.Empty;
        
        // Data storage for calculation steps
        private IFeatureClass forestScopeFeatures;
        private IFeatureClass urbanBoundaryFeatures;
        private IFeatureClass forestLandGradeFeatures;
        private IFeatureClass forestLandPriceFeatures;
        private Dictionary<string, double> priceFactors = new Dictionary<string, double>();
        private Dictionary<string, double> gradeWeights = new Dictionary<string, double>();
        
        // Step status tracking
        private Dictionary<string, bool> completedSteps = new Dictionary<string, bool>
        {
            { "extractScope", false },
            { "createBaseMap", false },
            { "extractPriceParams", false },
            { "supplementPrice", false },
            { "calculateAssetValue", false },
            { "cleanAndQA", false },
            { "buildDatabase", false }
        };

        public AssetValueCalculationForm()
        {
            InitializeComponent(); // Changed from InitializeComponents

            // Wire up the events
            this.btnSelectWorkspace.Click += (sender, e) => SelectWorkspace();
            this.workflowTabs.SelectedIndexChanged += (sender, e) => UpdateTabState(this.workflowTabs.SelectedIndex);

            // Pre-select a default working directory for convenience
            this.txtWorkspace.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ForestAssetCalculation");
            this.workingDirectory = this.txtWorkspace.Text; // Initialize workingDirectory
             if (!Directory.Exists(this.workingDirectory))
                Directory.CreateDirectory(this.workingDirectory);

            // Wire up events for ExtractScopeTabPage
            this.btnBrowseForestData.Click += (sender, e) => BrowseForData(this.txtForestData, "ѡ���ֲ�ʪ���ղ�����", "Shapefile�ļ� (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|�����ļ� (*.*)|*.*");
            this.btnBrowseUrbanBoundary.Click += (sender, e) => BrowseForData(this.txtUrbanBoundary, "ѡ����򿪷��߽�����", "Shapefile�ļ� (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|�����ļ� (*.*)|*.*");
            this.btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
            this.btnExtractScope.Click += BtnExtractScope_Click;

            // Wire up events for CreateBaseMapTabPage
            this.btnBrowseLandGradeData.Click += (sender, e) => BrowseForData(this.txtLandGradeData, "ѡ���ֵطֵ�����", "Shapefile�ļ� (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|�����ļ� (*.*)|*.*");
            this.btnBrowseLandPriceData.Click += (sender, e) => BrowseForData(this.txtLandPriceData, "ѡ���ֵض�������", "Shapefile�ļ� (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|�����ļ� (*.*)|*.*");
            this.btnLinkLandGradeData.Click += BtnLinkLandGradeData_Click;
            this.btnLinkLandPriceData.Click += BtnLinkLandPriceData_Click;
            this.btnSaveBaseMap.Click += BtnSaveBaseMap_Click;
            this.btnViewBaseMap.Click += BtnViewBaseMap_Click;
            
            // Wire up events for PriceParamsTabPage
            this.btnBrowseGradeIndices.Click += (sender, e) => BrowseForData(this.txtGradeIndices, "ѡ�񶨼�ָ������", "Excel�ļ� (*.xlsx)|*.xlsx|CSV�ļ� (*.csv)|*.csv|�����ļ� (*.*)|*.*");
            this.btnBrowseBasePriceData.Click += (sender, e) => BrowseForData(this.txtBasePriceData, "ѡ���׼�۸�����", "Excel�ļ� (*.xlsx)|*.xlsx|CSV�ļ� (*.csv)|*.csv|�����ļ� (*.*)|*.*");
            this.btnBrowseModifiers.Click += (sender, e) => BrowseForData(this.txtModifiers, "ѡ��۸�������������", "Excel�ļ� (*.xlsx)|*.xlsx|CSV�ļ� (*.csv)|*.csv|�����ļ� (*.*)|*.*");
            this.btnExtractPriceParams.Click += BtnExtractPriceParams_Click;
            this.btnLoadPriceParams.Click += BtnLoadPriceParams_Click;

            // Wire up events for SupplementPriceTabPage
            this.cboSupplementMethod.SelectedIndex = 0; // Already set in designer, but good for consistency
            this.btnSupplementPrice.Click += BtnSupplementPrice_Click;
            this.btnSaveSupplementResults.Click += BtnSaveSupplementResults_Click;
            this.btnViewPriceDistribution.Click += BtnViewPriceDistribution_Click;

            // Wire up events for CalculateValueTabPage
            this.cboCalcMethod.SelectedIndex = 0; // Already set in designer
            this.btnCalculateValue.Click += BtnCalculateValue_Click;
            this.btnSaveCalculationResults.Click += BtnSaveCalculationResults_Click;
            this.btnViewValueStats.Click += BtnViewValueStats_Click;
            this.chkExportResults.CheckedChanged += (sender, e) => UpdateExportOption();
            
            // Wire up events for CleanQATabPage
            this.btnBrowseFieldMapping.Click += (sender, e) => BrowseForData(this.txtFieldMapping, "ѡ���ֶ�ӳ���", "Excel�ļ� (*.xlsx)|*.xlsx|CSV�ļ� (*.csv)|*.csv|�����ļ� (*.*)|*.*");
            this.btnCleanQA.Click += BtnCleanQA_Click;
            this.btnViewQAReport.Click += BtnViewQAReport_Click;
            this.btnSaveCleanData.Click += BtnSaveCleanData_Click;

            // Wire up events for BuildDatabaseTabPage
            this.cboOutputFormat.SelectedIndex = 0; // Already set in designer
            for(int i = 0; i < this.clbOutputTables.Items.Count; i++) // Already set in designer
                this.clbOutputTables.SetItemChecked(i, true);
            this.btnBrowseOutputLocation.Click += (sender, e) => BrowseForOutputFolder(this.txtOutputLocation);
            this.btnBuildDatabase.Click += BtnBuildDatabase_Click;
            this.btnViewOutputFiles.Click += BtnViewOutputFiles_Click;
            this.btnGenerateReport.Click += BtnGenerateReport_Click;
        }

        // Removed InitializeComponents method
        // Removed all Create...TabPage methods

        #region Event Handlers

        private void SelectWorkspace()
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "ѡ����Ŀ¼";
                dlg.ShowNewFolderButton = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // var txtWorkspace = this.mainPanel.Controls.Find("txtWorkspace", true)[0] as TextBox; // Old
                    workingDirectory = dlg.SelectedPath;
                    this.txtWorkspace.Text = workingDirectory; // New
                    Log($"���ù���Ŀ¼: {workingDirectory}");

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(workingDirectory))
                        Directory.CreateDirectory(workingDirectory);
                }
            }
        }

        private void BrowseForData(TextBox textBox, string title, string filter)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = title;
                dlg.Filter = filter;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = dlg.FileName;
                }
            }
        }

        private void BrowseForOutputFolder(TextBox textBox)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "ѡ������ļ���";
                dlg.ShowNewFolderButton = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = dlg.SelectedPath;
                    exportDirectory = dlg.SelectedPath;
                }
            }
        }

        private void UpdateTabState(int selectedIndex)
        {
            // Logic to update UI based on the selected tab
            // For example, disable or enable specific steps based on 
            // the completion status of previous steps

            // Get the currently selected tab
            string selectedTabName;
            switch (selectedIndex)
            {
                case 0:
                    selectedTabName = "extractScope";
                    break;
                case 1:
                    selectedTabName = "createBaseMap";
                    break;
                case 2:
                    selectedTabName = "extractPriceParams";
                    break;
                case 3:
                    selectedTabName = "supplementPrice";
                    break;
                case 4:
                    selectedTabName = "calculateAssetValue";
                    break;
                case 5:
                    selectedTabName = "cleanAndQA";
                    break;
                case 6:
                    selectedTabName = "buildDatabase";
                    break;
                default:
                    selectedTabName = string.Empty;
                    break;
            }

            // Check dependencies
            // Each tab generally depends on the previous tab being completed
            switch (selectedTabName)
            {
                case "extractScope":
                    // First step, no dependencies
                    break;
                case "createBaseMap":
                    if (!completedSteps["extractScope"])
                        MessageBox.Show("������ɹ�����Χ��ȡ���衣", "����˳��", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "extractPriceParams":
                    if (!completedSteps["createBaseMap"])
                        MessageBox.Show("������ɹ�����ͼ�������衣", "����˳��", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "supplementPrice":
                    if (!completedSteps["extractPriceParams"])
                        MessageBox.Show("������ɼ۸������ȡ���衣", "����˳��", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "calculateAssetValue":
                    if (!completedSteps["supplementPrice"])
                        MessageBox.Show("������ɻ�׼�۸񲹳䲽�衣", "����˳��", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "cleanAndQA":
                    if (!completedSteps["calculateAssetValue"])
                        MessageBox.Show("���������Դ�ʲ���ֵ���㲽�衣", "����˳��", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "buildDatabase":
                    if (!completedSteps["cleanAndQA"])
                        MessageBox.Show("�������������ϴ�ʼ첽�衣", "����˳��", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void BtnLoadCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڴӵ�ǰ��ͼ��������...");
                // In a real implementation, this would use ArcObjects to access the current map

                // For demo purposes, just fill in some sample paths
                // var txtForestData = this.mainPanel.Controls.Find("txtForestData", true)[0] as TextBox; // Old
                // var txtUrbanBoundary = this.mainPanel.Controls.Find("txtUrbanBoundary", true)[0] as TextBox; // Old

                this.txtForestData.Text = @"C:\GIS_Data\�ֲ�ʪ���ղ�����.shp"; // New
                this.txtUrbanBoundary.Text = @"C:\GIS_Data\���򿪷��߽�.shp"; // New

                // Populate field dropdowns
                // var cboLandTypeField = this.mainPanel.Controls.Find("cboLandTypeField", true)[0] as ComboBox; // Old
                // var cboOwnershipField = this.mainPanel.Controls.Find("cboOwnershipField", true)[0] as ComboBox; // Old

                this.cboLandTypeField.Items.Clear(); // New
                this.cboLandTypeField.Items.AddRange(new object[] { "DLBM", "TDLYLX", "DLMC" }); // New
                this.cboLandTypeField.SelectedIndex = 0; // New

                this.cboOwnershipField.Items.Clear(); // New
                this.cboOwnershipField.Items.AddRange(new object[] { "QSDW", "QSLX", "QSXZ" }); // New
                this.cboOwnershipField.SelectedIndex = 0; // New

                Log("�����Ѵӵ�ǰ��ͼ����");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnExtractScope_Click(object sender, EventArgs e)
        {
            try
            {
                Log("������ȡ������Χ...");
                // In a real implementation, this would use ArcObjects to perform the extraction

                // For demo purposes, just simulate the extraction process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblExtractionResults = this.mainPanel.Controls.Find("lblExtractionResults", true)[0] as Label; // Old
                this.lblExtractionResults.Text = "������Χ��ȡ���"; // New

                completedSteps["extractScope"] = true;
                Log("������Χ��ȡ���");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ȡ������Χʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnLinkLandGradeData_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڹ����ֵطֵ�����...");
                // In a real implementation, this would use ArcObjects to perform the linking

                // For demo purposes, just simulate the linking process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblGradeMatchRateValue = this.mainPanel.Controls.Find("lblGradeMatchRateValue", true)[0] as Label; // Old
                this.lblGradeMatchRateValue.Text = "95%"; // New

                // var btnSaveBaseMap = this.mainPanel.Controls.Find("btnSaveBaseMap", true)[0] as Button; // Old
                // var btnViewBaseMap = this.mainPanel.Controls.Find("btnViewBaseMap", true)[0] as Button; // Old
                this.btnSaveBaseMap.Enabled = true; // New
                this.btnViewBaseMap.Enabled = true; // New

                completedSteps["createBaseMap"] = true;
                Log("�ֵطֵ����ݹ������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�����ֵطֵ�����ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnLinkLandPriceData_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڹҽӻ�׼�ؼ�...");
                // In a real implementation, this would use ArcObjects to perform the linking

                // For demo purposes, just simulate the linking process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblPriceMatchRateValue = this.mainPanel.Controls.Find("lblPriceMatchRateValue", true)[0] as Label; // Old
                this.lblPriceMatchRateValue.Text = "90%"; // New

                // var btnSaveBaseMap = this.mainPanel.Controls.Find("btnSaveBaseMap", true)[0] as Button; // Old
                // var btnViewBaseMap = this.mainPanel.Controls.Find("btnViewBaseMap", true)[0] as Button; // Old
                this.btnSaveBaseMap.Enabled = true; // New
                this.btnViewBaseMap.Enabled = true; // New

                completedSteps["createBaseMap"] = true;
                Log("��׼�ؼ۹ҽ����");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�ҽӻ�׼�ؼ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnSaveBaseMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڱ��湤����ͼ...");
                // In a real implementation, this would use ArcObjects to save the base map

                // For demo purposes, just simulate the save process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("������ͼ�������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���湤����ͼʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnViewBaseMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڲ鿴������ͼ...");
                // In a real implementation, this would use ArcObjects to display the base map

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("������ͼ�鿴���");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�鿴������ͼʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnExtractPriceParams_Click(object sender, EventArgs e)
        {
            try
            {
                Log("������ȡ�۸����...");
                // In a real implementation, this would use ArcObjects to extract the price parameters

                // For demo purposes, just simulate the extraction process
                System.Threading.Thread.Sleep(2000); // Simulate work

                completedSteps["extractPriceParams"] = true;
                Log("�۸������ȡ���");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ȡ�۸����ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnLoadPriceParams_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڼ��ؼ۸����...");
                // In a real implementation, this would use ArcObjects to load the price parameters

                // For demo purposes, just simulate the load process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var dgvPriceFactors = this.mainPanel.Controls.Find("dgvPriceFactors", true)[0] as DataGridView; // Old
                this.dgvPriceFactors.Rows.Clear(); // New
                this.dgvPriceFactors.Rows.Add("��������1", "0.5"); // New
                this.dgvPriceFactors.Rows.Add("��������2", "0.3"); // New
                this.dgvPriceFactors.Rows.Add("��������3", "0.2"); // New

                Log("�۸�����������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���ؼ۸����ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnSupplementPrice_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڲ����׼�ؼ�...");
                // In a real implementation, this would use ArcObjects to supplement the base price

                // For demo purposes, just simulate the supplement process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblBeforeCountValue = this.mainPanel.Controls.Find("lblBeforeCountValue", true)[0] as Label; // Old
                // var lblAfterCountValue = this.mainPanel.Controls.Find("lblAfterCountValue", true)[0] as Label; // Old
                this.lblBeforeCountValue.Text = "100"; // New
                this.lblAfterCountValue.Text = "0";   // New

                // var btnSaveSupplementResults = this.mainPanel.Controls.Find("btnSaveSupplementResults", true)[0] as Button; // Old
                // var btnViewPriceDistribution = this.mainPanel.Controls.Find("btnViewPriceDistribution", true)[0] as Button; // Old
                this.btnSaveSupplementResults.Enabled = true; // New
                this.btnViewPriceDistribution.Enabled = true; // New

                completedSteps["supplementPrice"] = true;
                Log("��׼�ؼ۲������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�����׼�ؼ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnSaveSupplementResults_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڱ��油����...");
                // In a real implementation, this would use ArcObjects to save the supplement results

                // For demo purposes, just simulate the save process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("�������������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���油����ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnViewPriceDistribution_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڲ鿴�۸�ֲ�...");
                // In a real implementation, this would use ArcObjects to display the price distribution

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("�۸�ֲ��鿴���");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�鿴�۸�ֲ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnCalculateValue_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڼ����ʲ���ֵ...");
                // In a real implementation, this would use ArcObjects to calculate the asset value

                // For demo purposes, just simulate the calculation process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblTotalValueResult = this.mainPanel.Controls.Find("lblTotalValueResult", true)[0] as Label; // Old
                // var lblAverageUnitPriceResult = this.mainPanel.Controls.Find("lblAverageUnitPriceResult", true)[0] as Label; // Old
                // var lblTotalAreaResult = this.mainPanel.Controls.Find("lblTotalAreaResult", true)[0] as Label; // Old
                // var lblParcelCountResult = this.mainPanel.Controls.Find("lblParcelCountResult", true)[0] as Label; // Old
                this.lblTotalValueResult.Text = "10652896"; // New
                this.lblAverageUnitPriceResult.Text = "8.42"; // New
                this.lblTotalAreaResult.Text = "1265.89"; // New
                this.lblParcelCountResult.Text = "150"; // New

                // var btnSaveCalculationResults = this.mainPanel.Controls.Find("btnSaveCalculationResults", true)[0] as Button; // Old
                // var btnViewValueStats = this.mainPanel.Controls.Find("btnViewValueStats", true)[0] as Button; // Old
                this.btnSaveCalculationResults.Enabled = true; // New
                this.btnViewValueStats.Enabled = true; // New

                completedSteps["calculateAssetValue"] = true;
                Log("�ʲ���ֵ�������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�����ʲ���ֵʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnSaveCalculationResults_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڱ��������...");
                // In a real implementation, this would use ArcObjects to save the calculation results

                // For demo purposes, just simulate the save process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("�������������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���������ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnViewValueStats_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڲ鿴��ֵͳ��...");
                // In a real implementation, this would use ArcObjects to display the value statistics

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("��ֵͳ�Ʋ鿴���");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�鿴��ֵͳ��ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnCleanQA_Click(object sender, EventArgs e)
        {
            try
            {
                Log("����ִ��������ϴ���ʼ�...");
                // In a real implementation, this would use ArcObjects to perform the cleaning and QA

                // For demo purposes, just simulate the cleaning and QA process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblIssuesFoundValue = this.mainPanel.Controls.Find("lblIssuesFoundValue", true)[0] as Label; // Old
                // var lblIssuesFixedValue = this.mainPanel.Controls.Find("lblIssuesFixedValue", true)[0] as Label; // Old
                // var lblQAPassRateValue = this.mainPanel.Controls.Find("lblQAPassRateValue", true)[0] as Label; // Old
                this.lblIssuesFoundValue.Text = "10"; // New
                this.lblIssuesFixedValue.Text = "10"; // New
                this.lblQAPassRateValue.Text = "100%"; // New

                // var btnViewQAReport = this.mainPanel.Controls.Find("btnViewQAReport", true)[0] as Button; // Old
                // var btnSaveCleanData = this.mainPanel.Controls.Find("btnSaveCleanData", true)[0] as Button; // Old
                this.btnViewQAReport.Enabled = true; // New
                this.btnSaveCleanData.Enabled = true; // New

                completedSteps["cleanAndQA"] = true;
                Log("������ϴ���ʼ����");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ִ��������ϴ���ʼ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnViewQAReport_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڲ鿴�ʼ챨��...");
                // In a real implementation, this would use ArcObjects to display the QA report

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("�ʼ챨��鿴���");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�鿴�ʼ챨��ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnSaveCleanData_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڱ�����ϴ������...");
                // In a real implementation, this would use ArcObjects to save the cleaned data

                // For demo purposes, just simulate the save process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("��ϴ�����ݱ������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ϴ������ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnBuildDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڹ������ݿ�...");
                // In a real implementation, this would use ArcObjects to build the database

                // For demo purposes, just simulate the build process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblOutputStatusValue = this.mainPanel.Controls.Find("lblOutputStatusValue", true)[0] as Label; // Old
                this.lblOutputStatusValue.Text = "���ݿ⹹�����"; // New

                // var btnViewOutputFiles = this.mainPanel.Controls.Find("btnViewOutputFiles", true)[0] as Button; // Old
                // var btnGenerateReport = this.mainPanel.Controls.Find("btnGenerateReport", true)[0] as Button; // Old
                this.btnViewOutputFiles.Enabled = true; // New
                this.btnGenerateReport.Enabled = true; // New

                completedSteps["buildDatabase"] = true;
                Log("���ݿ⹹�����");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�������ݿ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnViewOutputFiles_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڲ鿴����ļ�...");
                // In a real implementation, this would use ArcObjects to display the output files

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("����ļ��鿴���");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�鿴����ļ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                Log("����������鱨��...");
                // In a real implementation, this would use ArcObjects to generate the report

                // For demo purposes, just simulate the report generation process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("��鱨���������");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������鱨��ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        private void UpdateExportOption()
        {
            // var chkExportResults = this.mainPanel.Controls.Find("chkExportResults", true)[0] as CheckBox; // Old
            // var btnSaveCalculationResults = this.mainPanel.Controls.Find("btnSaveCalculationResults", true)[0] as Button; // Old
            this.btnSaveCalculationResults.Enabled = this.chkExportResults.Checked; // New
        }

        #endregion

        protected override void StartButton_Click(object sender, EventArgs e)
        {
            base.StartButton_Click(sender, e);

            // Get the file paths from the text boxes
            // var txtBasePriceData = this.mainPanel.Controls.Find("txtBasePriceData", true)[0] as TextBox; // Old
            // var txtModifiersData = this.mainPanel.Controls.Find("txtModifiersData", true)[0] as TextBox; // Old - Assuming txtModifiers is the correct field
            
            // Using the field names as defined in the designer
            if (string.IsNullOrEmpty(this.txtBasePriceData.Text) || string.IsNullOrEmpty(this.txtModifiers.Text))
            {
                MessageBox.Show("����ѡ�����������ļ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }

            // Get calculation settings
            // var numYieldRate = this.mainPanel.Controls.Find("numYieldRate", true)[0] as NumericUpDown; // Old
            // var numDateModifier = this.mainPanel.Controls.Find("numDateModifier", true)[0] as NumericUpDown; // Old
            // var numPeriodModifier = this.mainPanel.Controls.Find("numPeriodModifier", true)[0] as NumericUpDown; // Old
            // var chkExportResults = this.mainPanel.Controls.Find("chkExportResults", true)[0] as CheckBox; // Old

            decimal yieldRate = this.numYieldRate.Value; // New
            decimal dateModifier = this.numDateModifier.Value; // New
            decimal periodModifier = this.numPeriodModifier.Value; // New
            bool exportResults = this.chkExportResults.Checked; // New

            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessCalculateAssetValue(
                this.txtBasePriceData.Text, // New
                this.txtModifiers.Text,    // New - Assuming txtModifiers is the correct field
                yieldRate,
                dateModifier,
                periodModifier,
                exportResults));
        }

        private void ProcessCalculateAssetValue(
            string basePricePath,
            string modifiersPath,
            decimal yieldRate,
            decimal dateModifier,
            decimal periodModifier,
            bool exportResults)
        {
            try
            {
                UpdateStatus("����׼������...");
                Log("��ʼ��Դ�ʲ���ֵ��������");
                Log($"ʹ�û�׼�۸�����: {basePricePath}");
                Log($"ʹ��������������: {modifiersPath}");
                Log($"���滹ԭ��: {yieldRate}%");
                Log($"��������ϵ��: {dateModifier}");
                Log($"��������ϵ��: {periodModifier}");

                // Simulate the processing steps
                for (int i = 0; i <= 100; i += 5)
                {
                    if (processingCancelled) return;

                    switch (i)
                    {
                        case 0:
                            UpdateStatus("���ڼ��ػ�׼�۸�����...");
                            Log("���ػ�׼�۸�������...");
                            break;
                        case 10:
                            UpdateStatus("���ڼ���������������...");
                            Log("������������������...");
                            break;
                        case 20:
                            UpdateStatus("���ڼ����ڵؼ۸�...");
                            Log("�����ڵؼ۸���...");
                            break;
                        case 40:
                            UpdateStatus("���ڽ�����������...");
                            Log("��������������...");
                            break;
                        case 60:
                            UpdateStatus("���ڽ�����������...");
                            Log("��������������...");
                            break;
                        case 80:
                            UpdateStatus("���ڼ�����Դ�ʲ���ֵ...");
                            Log("������Դ�ʲ���ֵ��...");
                            break;
                        case 90:
                            if (exportResults)
                            {
                                UpdateStatus("���ڵ���������...");
                                Log("������������...");
                            }
                            break;
                    }

                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }

                Log("��Դ�ʲ���ֵ�������");
                Log("�ܼ�ֵ: 10652896Ԫ");
                Log("ƽ������: 8.42��Ԫ/����");

                // Complete the processing
                this.Invoke(new Action(() => OnProcessingComplete(true)));
            }
            catch (Exception ex)
            {
                Log($"����: {ex.Message}");
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show($"��������г���: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    OnProcessingComplete(false);
                }));
            }
        }

        protected override void CancelButton_Click(object sender, EventArgs e)
        {
            base.CancelButton_Click(sender, e);
            processingCancelled = true;
        }

        private void btnBrowseLandPriceData_Click(object sender, EventArgs e)
        {

        }

        private void cboSupplementMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}