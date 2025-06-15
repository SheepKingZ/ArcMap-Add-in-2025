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
            this.btnBrowseForestData.Click += (sender, e) => BrowseForData(this.txtForestData, "选择林草湿荒普查数据", "Shapefile文件 (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|所有文件 (*.*)|*.*");
            this.btnBrowseUrbanBoundary.Click += (sender, e) => BrowseForData(this.txtUrbanBoundary, "选择城镇开发边界数据", "Shapefile文件 (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|所有文件 (*.*)|*.*");
            this.btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
            this.btnExtractScope.Click += BtnExtractScope_Click;

            // Wire up events for CreateBaseMapTabPage
            this.btnBrowseLandGradeData.Click += (sender, e) => BrowseForData(this.txtLandGradeData, "选择林地分等数据", "Shapefile文件 (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|所有文件 (*.*)|*.*");
            this.btnBrowseLandPriceData.Click += (sender, e) => BrowseForData(this.txtLandPriceData, "选择林地定级数据", "Shapefile文件 (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|所有文件 (*.*)|*.*");
            this.btnLinkLandGradeData.Click += BtnLinkLandGradeData_Click;
            this.btnLinkLandPriceData.Click += BtnLinkLandPriceData_Click;
            this.btnSaveBaseMap.Click += BtnSaveBaseMap_Click;
            this.btnViewBaseMap.Click += BtnViewBaseMap_Click;
            
            // Wire up events for PriceParamsTabPage
            this.btnBrowseGradeIndices.Click += (sender, e) => BrowseForData(this.txtGradeIndices, "选择定级指标数据", "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*");
            this.btnBrowseBasePriceData.Click += (sender, e) => BrowseForData(this.txtBasePriceData, "选择基准价格数据", "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*");
            this.btnBrowseModifiers.Click += (sender, e) => BrowseForData(this.txtModifiers, "选择价格修正因子数据", "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*");
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
            this.btnBrowseFieldMapping.Click += (sender, e) => BrowseForData(this.txtFieldMapping, "选择字段映射表", "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*");
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
                dlg.Description = "选择工作目录";
                dlg.ShowNewFolderButton = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // var txtWorkspace = this.mainPanel.Controls.Find("txtWorkspace", true)[0] as TextBox; // Old
                    workingDirectory = dlg.SelectedPath;
                    this.txtWorkspace.Text = workingDirectory; // New
                    Log($"设置工作目录: {workingDirectory}");

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
                dlg.Description = "选择输出文件夹";
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
                        MessageBox.Show("请先完成工作范围提取步骤。", "步骤顺序", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "extractPriceParams":
                    if (!completedSteps["createBaseMap"])
                        MessageBox.Show("请先完成工作底图制作步骤。", "步骤顺序", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "supplementPrice":
                    if (!completedSteps["extractPriceParams"])
                        MessageBox.Show("请先完成价格参数提取步骤。", "步骤顺序", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "calculateAssetValue":
                    if (!completedSteps["supplementPrice"])
                        MessageBox.Show("请先完成基准价格补充步骤。", "步骤顺序", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "cleanAndQA":
                    if (!completedSteps["calculateAssetValue"])
                        MessageBox.Show("请先完成资源资产价值计算步骤。", "步骤顺序", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "buildDatabase":
                    if (!completedSteps["cleanAndQA"])
                        MessageBox.Show("请先完成数据清洗质检步骤。", "步骤顺序", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void BtnLoadCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在从当前地图加载数据...");
                // In a real implementation, this would use ArcObjects to access the current map

                // For demo purposes, just fill in some sample paths
                // var txtForestData = this.mainPanel.Controls.Find("txtForestData", true)[0] as TextBox; // Old
                // var txtUrbanBoundary = this.mainPanel.Controls.Find("txtUrbanBoundary", true)[0] as TextBox; // Old

                this.txtForestData.Text = @"C:\GIS_Data\林草湿荒普查数据.shp"; // New
                this.txtUrbanBoundary.Text = @"C:\GIS_Data\城镇开发边界.shp"; // New

                // Populate field dropdowns
                // var cboLandTypeField = this.mainPanel.Controls.Find("cboLandTypeField", true)[0] as ComboBox; // Old
                // var cboOwnershipField = this.mainPanel.Controls.Find("cboOwnershipField", true)[0] as ComboBox; // Old

                this.cboLandTypeField.Items.Clear(); // New
                this.cboLandTypeField.Items.AddRange(new object[] { "DLBM", "TDLYLX", "DLMC" }); // New
                this.cboLandTypeField.SelectedIndex = 0; // New

                this.cboOwnershipField.Items.Clear(); // New
                this.cboOwnershipField.Items.AddRange(new object[] { "QSDW", "QSLX", "QSXZ" }); // New
                this.cboOwnershipField.SelectedIndex = 0; // New

                Log("数据已从当前地图加载");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnExtractScope_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在提取工作范围...");
                // In a real implementation, this would use ArcObjects to perform the extraction

                // For demo purposes, just simulate the extraction process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblExtractionResults = this.mainPanel.Controls.Find("lblExtractionResults", true)[0] as Label; // Old
                this.lblExtractionResults.Text = "工作范围提取完成"; // New

                completedSteps["extractScope"] = true;
                Log("工作范围提取完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提取工作范围时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnLinkLandGradeData_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在关联林地分等数据...");
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
                Log("林地分等数据关联完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"关联林地分等数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnLinkLandPriceData_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在挂接基准地价...");
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
                Log("基准地价挂接完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"挂接基准地价时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnSaveBaseMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在保存工作底图...");
                // In a real implementation, this would use ArcObjects to save the base map

                // For demo purposes, just simulate the save process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("工作底图保存完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存工作底图时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnViewBaseMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在查看工作底图...");
                // In a real implementation, this would use ArcObjects to display the base map

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("工作底图查看完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查看工作底图时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnExtractPriceParams_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在提取价格参数...");
                // In a real implementation, this would use ArcObjects to extract the price parameters

                // For demo purposes, just simulate the extraction process
                System.Threading.Thread.Sleep(2000); // Simulate work

                completedSteps["extractPriceParams"] = true;
                Log("价格参数提取完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提取价格参数时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnLoadPriceParams_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在加载价格参数...");
                // In a real implementation, this would use ArcObjects to load the price parameters

                // For demo purposes, just simulate the load process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var dgvPriceFactors = this.mainPanel.Controls.Find("dgvPriceFactors", true)[0] as DataGridView; // Old
                this.dgvPriceFactors.Rows.Clear(); // New
                this.dgvPriceFactors.Rows.Add("修正因子1", "0.5"); // New
                this.dgvPriceFactors.Rows.Add("修正因子2", "0.3"); // New
                this.dgvPriceFactors.Rows.Add("修正因子3", "0.2"); // New

                Log("价格参数加载完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载价格参数时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnSupplementPrice_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在补充基准地价...");
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
                Log("基准地价补充完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"补充基准地价时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnSaveSupplementResults_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在保存补充结果...");
                // In a real implementation, this would use ArcObjects to save the supplement results

                // For demo purposes, just simulate the save process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("补充结果保存完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存补充结果时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnViewPriceDistribution_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在查看价格分布...");
                // In a real implementation, this would use ArcObjects to display the price distribution

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("价格分布查看完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查看价格分布时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnCalculateValue_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在计算资产价值...");
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
                Log("资产价值计算完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算资产价值时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnSaveCalculationResults_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在保存计算结果...");
                // In a real implementation, this would use ArcObjects to save the calculation results

                // For demo purposes, just simulate the save process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("计算结果保存完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存计算结果时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnViewValueStats_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在查看价值统计...");
                // In a real implementation, this would use ArcObjects to display the value statistics

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("价值统计查看完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查看价值统计时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnCleanQA_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在执行数据清洗与质检...");
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
                Log("数据清洗与质检完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行数据清洗与质检时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnViewQAReport_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在查看质检报告...");
                // In a real implementation, this would use ArcObjects to display the QA report

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("质检报告查看完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查看质检报告时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnSaveCleanData_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在保存清洗后数据...");
                // In a real implementation, this would use ArcObjects to save the cleaned data

                // For demo purposes, just simulate the save process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("清洗后数据保存完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存清洗后数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnBuildDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在构建数据库...");
                // In a real implementation, this would use ArcObjects to build the database

                // For demo purposes, just simulate the build process
                System.Threading.Thread.Sleep(2000); // Simulate work

                // var lblOutputStatusValue = this.mainPanel.Controls.Find("lblOutputStatusValue", true)[0] as Label; // Old
                this.lblOutputStatusValue.Text = "数据库构建完成"; // New

                // var btnViewOutputFiles = this.mainPanel.Controls.Find("btnViewOutputFiles", true)[0] as Button; // Old
                // var btnGenerateReport = this.mainPanel.Controls.Find("btnGenerateReport", true)[0] as Button; // Old
                this.btnViewOutputFiles.Enabled = true; // New
                this.btnGenerateReport.Enabled = true; // New

                completedSteps["buildDatabase"] = true;
                Log("数据库构建完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"构建数据库时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnViewOutputFiles_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在查看输出文件...");
                // In a real implementation, this would use ArcObjects to display the output files

                // For demo purposes, just simulate the view process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("输出文件查看完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查看输出文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在生成清查报告...");
                // In a real implementation, this would use ArcObjects to generate the report

                // For demo purposes, just simulate the report generation process
                System.Threading.Thread.Sleep(2000); // Simulate work

                Log("清查报告生成完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成清查报告时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
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
                MessageBox.Show("请先选择所需数据文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                UpdateStatus("正在准备数据...");
                Log("开始资源资产价值计算流程");
                Log($"使用基准价格数据: {basePricePath}");
                Log($"使用修正因子数据: {modifiersPath}");
                Log($"收益还原率: {yieldRate}%");
                Log($"期日修正系数: {dateModifier}");
                Log($"年期修正系数: {periodModifier}");

                // Simulate the processing steps
                for (int i = 0; i <= 100; i += 5)
                {
                    if (processingCancelled) return;

                    switch (i)
                    {
                        case 0:
                            UpdateStatus("正在加载基准价格数据...");
                            Log("加载基准价格数据中...");
                            break;
                        case 10:
                            UpdateStatus("正在加载修正因子数据...");
                            Log("加载修正因子数据中...");
                            break;
                        case 20:
                            UpdateStatus("正在计算宗地价格...");
                            Log("计算宗地价格中...");
                            break;
                        case 40:
                            UpdateStatus("正在进行期日修正...");
                            Log("进行期日修正中...");
                            break;
                        case 60:
                            UpdateStatus("正在进行年期修正...");
                            Log("进行年期修正中...");
                            break;
                        case 80:
                            UpdateStatus("正在计算资源资产价值...");
                            Log("计算资源资产价值中...");
                            break;
                        case 90:
                            if (exportResults)
                            {
                                UpdateStatus("正在导出计算结果...");
                                Log("导出计算结果中...");
                            }
                            break;
                    }

                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }

                Log("资源资产价值计算完成");
                Log("总价值: 10652896元");
                Log("平均单价: 8.42万元/公顷");

                // Complete the processing
                this.Invoke(new Action(() => OnProcessingComplete(true)));
            }
            catch (Exception ex)
            {
                Log($"错误: {ex.Message}");
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show($"处理过程中出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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