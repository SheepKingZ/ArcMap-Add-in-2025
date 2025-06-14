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
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "森林资源资产清查计算工具";
            this.titleLabel.Text = "森林资源资产清查计算";
            this.Size = new Size(750, 650);

            this.descriptionTextBox.Text =
                "本工具基于林草湿荒普查数据，结合城镇开发边界、林地分等定级和基准地价等数据，" +
                "进行森林资源资产价值计算，包括工作范围提取、底图制作、价格参数提取、价格补充、" +
                "资产价值计算、数据质检和数据库构建等步骤。";

            // Create tabbed interface for the workflow steps
            TabControl workflowTabs = new TabControl
            {
                Location = new Point(15, 150),
                Size = new Size(700, 380),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                SelectedIndex = 0
            };

            // Create tabs for each workflow step
            TabPage extractScopeTab = CreateExtractScopeTabPage();
            TabPage createBaseMapTab = CreateBaseMapTabPage();
            TabPage priceParamsTab = CreatePriceParamsTabPage();
            TabPage supplementPriceTab = CreateSupplementPriceTabPage();
            TabPage calculateValueTab = CreateCalculateValueTabPage();
            TabPage cleanQATab = CreateCleanQATabPage();
            TabPage buildDatabaseTab = CreateBuildDatabaseTabPage();

            // Add tabs to the control
            workflowTabs.TabPages.Add(extractScopeTab);
            workflowTabs.TabPages.Add(createBaseMapTab);
            workflowTabs.TabPages.Add(priceParamsTab);
            workflowTabs.TabPages.Add(supplementPriceTab);
            workflowTabs.TabPages.Add(calculateValueTab);
            workflowTabs.TabPages.Add(cleanQATab);
            workflowTabs.TabPages.Add(buildDatabaseTab);

            // Add workspace selection controls above the tabs
            GroupBox workspaceGroupBox = new GroupBox
            {
                Text = "工作目录设置",
                Location = new Point(15, 80),
                Size = new Size(700, 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblWorkspace = new Label
            {
                Text = "工作目录:",
                Location = new Point(15, 25),
                Size = new Size(80, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtWorkspace = new TextBox
            {
                Location = new Point(100, 25),
                Size = new Size(480, 20),
                ReadOnly = true,
                Name = "txtWorkspace"
            };

            Button btnSelectWorkspace = new Button
            {
                Text = "选择...",
                Location = new Point(590, 24),
                Size = new Size(80, 23)
            };

            workspaceGroupBox.Controls.Add(lblWorkspace);
            workspaceGroupBox.Controls.Add(txtWorkspace);
            workspaceGroupBox.Controls.Add(btnSelectWorkspace);

            // Adjust the location of the existing log text box
            this.logTextBox.Location = new Point(15, 540);
            this.logTextBox.Size = new Size(700, 60);
            this.logTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // Adjust position of status and progress controls
            this.statusLabel.Location = new Point(15, 605);
            this.statusLabel.Size = new Size(700, 20);
            this.statusLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            this.progressBar.Location = new Point(15, 625);
            this.progressBar.Size = new Size(700, 20);
            this.progressBar.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // Add controls to the main panel
            this.mainPanel.Controls.Add(workspaceGroupBox);
            this.mainPanel.Controls.Add(workflowTabs);

            // Wire up the events
            btnSelectWorkspace.Click += (sender, e) => SelectWorkspace();
            workflowTabs.SelectedIndexChanged += (sender, e) => UpdateTabState(workflowTabs.SelectedIndex);

            // Pre-select a default working directory for convenience
            txtWorkspace.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ForestAssetCalculation");
        }

        #region Tab Pages Creation

        private TabPage CreateExtractScopeTabPage()
        {
            TabPage tab = new TabPage("1. 提取工作范围");
            
            GroupBox dataSourceGroupBox = new GroupBox
            {
                Text = "数据来源",
                Location = new Point(10, 10),
                Size = new Size(670, 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblForestData = new Label
            {
                Text = "林草湿荒普查数据:",
                Location = new Point(15, 25),
                Size = new Size(140, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtForestData = new TextBox
            {
                Location = new Point(160, 25),
                Size = new Size(390, 20),
                ReadOnly = true,
                Name = "txtForestData"
            };

            Button btnBrowseForestData = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 24),
                Size = new Size(80, 23),
                Name = "btnBrowseForestData"
            };

            Label lblUrbanBoundary = new Label
            {
                Text = "城镇开发边界数据:",
                Location = new Point(15, 55),
                Size = new Size(140, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtUrbanBoundary = new TextBox
            {
                Location = new Point(160, 55),
                Size = new Size(390, 20),
                ReadOnly = true,
                Name = "txtUrbanBoundary"
            };

            Button btnBrowseUrbanBoundary = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 54),
                Size = new Size(80, 23),
                Name = "btnBrowseUrbanBoundary"
            };

            Button btnLoadCurrentMap = new Button
            {
                Text = "加载当前地图数据",
                Location = new Point(160, 85),
                Size = new Size(180, 25),
                Name = "btnLoadCurrentMap"
            };

            dataSourceGroupBox.Controls.Add(lblForestData);
            dataSourceGroupBox.Controls.Add(txtForestData);
            dataSourceGroupBox.Controls.Add(btnBrowseForestData);
            dataSourceGroupBox.Controls.Add(lblUrbanBoundary);
            dataSourceGroupBox.Controls.Add(txtUrbanBoundary);
            dataSourceGroupBox.Controls.Add(btnBrowseUrbanBoundary);
            dataSourceGroupBox.Controls.Add(btnLoadCurrentMap);

            GroupBox extractionSettingsGroupBox = new GroupBox
            {
                Text = "提取设置",
                Location = new Point(10, 140),
                Size = new Size(670, 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblLandTypeField = new Label
            {
                Text = "地类字段:",
                Location = new Point(15, 25),
                Size = new Size(130, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            ComboBox cboLandTypeField = new ComboBox
            {
                Location = new Point(150, 25),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cboLandTypeField"
            };
            
            Label lblOwnershipField = new Label
            {
                Text = "土地权属性质字段:",
                Location = new Point(15, 55),
                Size = new Size(130, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            ComboBox cboOwnershipField = new ComboBox
            {
                Location = new Point(150, 55),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cboOwnershipField"
            };

            Label lblForestValue = new Label
            {
                Text = "林地地类值:",
                Location = new Point(370, 25),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtForestValue = new TextBox
            {
                Location = new Point(465, 25),
                Size = new Size(80, 20),
                Text = "03",
                Name = "txtForestValue"
            };

            Label lblStateOwnershipValue = new Label
            {
                Text = "国有值:",
                Location = new Point(370, 55),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtStateOwnershipValue = new TextBox
            {
                Location = new Point(465, 55),
                Size = new Size(80, 20),
                Text = "1",
                Name = "txtStateOwnershipValue"
            };

            Label lblCollectiveValue = new Label
            {
                Text = "集体值:",
                Location = new Point(370, 85),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtCollectiveValue = new TextBox
            {
                Location = new Point(465, 85),
                Size = new Size(80, 20),
                Text = "2",
                Name = "txtCollectiveValue"
            };

            Button btnExtractScope = new Button
            {
                Text = "执行工作范围提取",
                Location = new Point(150, 85),
                Size = new Size(180, 25),
                Name = "btnExtractScope"
            };

            extractionSettingsGroupBox.Controls.Add(lblLandTypeField);
            extractionSettingsGroupBox.Controls.Add(cboLandTypeField);
            extractionSettingsGroupBox.Controls.Add(lblOwnershipField);
            extractionSettingsGroupBox.Controls.Add(cboOwnershipField);
            extractionSettingsGroupBox.Controls.Add(lblForestValue);
            extractionSettingsGroupBox.Controls.Add(txtForestValue);
            extractionSettingsGroupBox.Controls.Add(lblStateOwnershipValue);
            extractionSettingsGroupBox.Controls.Add(txtStateOwnershipValue);
            extractionSettingsGroupBox.Controls.Add(lblCollectiveValue);
            extractionSettingsGroupBox.Controls.Add(txtCollectiveValue);
            extractionSettingsGroupBox.Controls.Add(btnExtractScope);

            GroupBox resultsGroupBox = new GroupBox
            {
                Text = "结果",
                Location = new Point(10, 270),
                Size = new Size(670, 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            Label lblExtractionResults = new Label
            {
                Location = new Point(15, 25),
                Size = new Size(640, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "尚未执行工作范围提取",
                Name = "lblExtractionResults"
            };

            resultsGroupBox.Controls.Add(lblExtractionResults);

            tab.Controls.Add(dataSourceGroupBox);
            tab.Controls.Add(extractionSettingsGroupBox);
            tab.Controls.Add(resultsGroupBox);

            // Wire up events
            btnBrowseForestData.Click += (sender, e) => BrowseForData(txtForestData, "选择林草湿荒普查数据", "Shapefile文件 (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|所有文件 (*.*)|*.*");
            btnBrowseUrbanBoundary.Click += (sender, e) => BrowseForData(txtUrbanBoundary, "选择城镇开发边界数据", "Shapefile文件 (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|所有文件 (*.*)|*.*");
            btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
            btnExtractScope.Click += BtnExtractScope_Click;

            return tab;
        }

        private TabPage CreateBaseMapTabPage()
        {
            TabPage tab = new TabPage("2. 制作工作底图");

            GroupBox landGradeGroupBox = new GroupBox
            {
                Text = "林地分等数据",
                Location = new Point(10, 10),
                Size = new Size(670, 90),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblLandGradeData = new Label
            {
                Text = "林地分等数据:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtLandGradeData = new TextBox
            {
                Location = new Point(140, 25),
                Size = new Size(410, 20),
                ReadOnly = true,
                Name = "txtLandGradeData"
            };

            Button btnBrowseLandGradeData = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 24),
                Size = new Size(80, 23),
                Name = "btnBrowseLandGradeData"
            };

            Button btnLinkLandGradeData = new Button
            {
                Text = "关联林地分等数据",
                Location = new Point(140, 55),
                Size = new Size(180, 25),
                Name = "btnLinkLandGradeData"
            };

            landGradeGroupBox.Controls.Add(lblLandGradeData);
            landGradeGroupBox.Controls.Add(txtLandGradeData);
            landGradeGroupBox.Controls.Add(btnBrowseLandGradeData);
            landGradeGroupBox.Controls.Add(btnLinkLandGradeData);

            GroupBox landPriceGroupBox = new GroupBox
            {
                Text = "林地定级与基准地价数据",
                Location = new Point(10, 110),
                Size = new Size(670, 90),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblLandPriceData = new Label
            {
                Text = "林地定级数据:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtLandPriceData = new TextBox
            {
                Location = new Point(140, 25),
                Size = new Size(410, 20),
                ReadOnly = true,
                Name = "txtLandPriceData"
            };

            Button btnBrowseLandPriceData = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 24),
                Size = new Size(80, 23),
                Name = "btnBrowseLandPriceData"
            };

            Button btnLinkLandPriceData = new Button
            {
                Text = "挂接基准地价",
                Location = new Point(140, 55),
                Size = new Size(180, 25),
                Name = "btnLinkLandPriceData"
            };

            landPriceGroupBox.Controls.Add(lblLandPriceData);
            landPriceGroupBox.Controls.Add(txtLandPriceData);
            landPriceGroupBox.Controls.Add(btnBrowseLandPriceData);
            landPriceGroupBox.Controls.Add(btnLinkLandPriceData);

            GroupBox linkResultsGroupBox = new GroupBox
            {
                Text = "关联结果",
                Location = new Point(10, 210),
                Size = new Size(670, 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            Label lblGradeMatchRate = new Label
            {
                Text = "分等关联匹配率:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblGradeMatchRateValue = new Label
            {
                Text = "尚未执行",
                Location = new Point(140, 25),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblGradeMatchRateValue"
            };

            Label lblPriceMatchRate = new Label
            {
                Text = "基准价格匹配率:",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblPriceMatchRateValue = new Label
            {
                Text = "尚未执行",
                Location = new Point(140, 55),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblPriceMatchRateValue"
            };

            Button btnSaveBaseMap = new Button
            {
                Text = "保存工作底图",
                Location = new Point(15, 85),
                Size = new Size(180, 25),
                Name = "btnSaveBaseMap",
                Enabled = false
            };

            Button btnViewBaseMap = new Button
            {
                Text = "查看工作底图",
                Location = new Point(210, 85),
                Size = new Size(180, 25),
                Name = "btnViewBaseMap",
                Enabled = false
            };

            linkResultsGroupBox.Controls.Add(lblGradeMatchRate);
            linkResultsGroupBox.Controls.Add(lblGradeMatchRateValue);
            linkResultsGroupBox.Controls.Add(lblPriceMatchRate);
            linkResultsGroupBox.Controls.Add(lblPriceMatchRateValue);
            linkResultsGroupBox.Controls.Add(btnSaveBaseMap);
            linkResultsGroupBox.Controls.Add(btnViewBaseMap);

            tab.Controls.Add(landGradeGroupBox);
            tab.Controls.Add(landPriceGroupBox);
            tab.Controls.Add(linkResultsGroupBox);

            // Wire up events
            btnBrowseLandGradeData.Click += (sender, e) => BrowseForData(txtLandGradeData, "选择林地分等数据", "Shapefile文件 (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|所有文件 (*.*)|*.*");
            btnBrowseLandPriceData.Click += (sender, e) => BrowseForData(txtLandPriceData, "选择林地定级数据", "Shapefile文件 (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb|所有文件 (*.*)|*.*");
            btnLinkLandGradeData.Click += BtnLinkLandGradeData_Click;
            btnLinkLandPriceData.Click += BtnLinkLandPriceData_Click;
            btnSaveBaseMap.Click += BtnSaveBaseMap_Click;
            btnViewBaseMap.Click += BtnViewBaseMap_Click;

            return tab;
        }

        private TabPage CreatePriceParamsTabPage()
        {
            TabPage tab = new TabPage("3. 价格参数提取");

            GroupBox priceParamsGroupBox = new GroupBox
            {
                Text = "林地定级价格参数",
                Location = new Point(10, 10),
                Size = new Size(670, 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblGradeIndices = new Label
            {
                Text = "定级指标数据:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtGradeIndices = new TextBox
            {
                Location = new Point(140, 25),
                Size = new Size(410, 20),
                ReadOnly = true,
                Name = "txtGradeIndices"
            };

            Button btnBrowseGradeIndices = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 24),
                Size = new Size(80, 23),
                Name = "btnBrowseGradeIndices"
            };

            Label lblBasePriceData = new Label
            {
                Text = "基准价格数据:",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtBasePriceData = new TextBox
            {
                Location = new Point(140, 55),
                Size = new Size(410, 20),
                ReadOnly = true,
                Name = "txtBasePriceData"
            };

            Button btnBrowseBasePriceData = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 54),
                Size = new Size(80, 23),
                Name = "btnBrowseBasePriceData"
            };

            Button btnExtractPriceParams = new Button
            {
                Text = "提取价格参数",
                Location = new Point(140, 85),
                Size = new Size(180, 25),
                Name = "btnExtractPriceParams"
            };

            priceParamsGroupBox.Controls.Add(lblGradeIndices);
            priceParamsGroupBox.Controls.Add(txtGradeIndices);
            priceParamsGroupBox.Controls.Add(btnBrowseGradeIndices);
            priceParamsGroupBox.Controls.Add(lblBasePriceData);
            priceParamsGroupBox.Controls.Add(txtBasePriceData);
            priceParamsGroupBox.Controls.Add(btnBrowseBasePriceData);
            priceParamsGroupBox.Controls.Add(btnExtractPriceParams);

            GroupBox priceFactorsGroupBox = new GroupBox
            {
                Text = "价格修正因子及收益还原率",
                Location = new Point(10, 140),
                Size = new Size(670, 185),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            Label lblModifiers = new Label
            {
                Text = "价格修正因子:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtModifiers = new TextBox
            {
                Location = new Point(140, 25),
                Size = new Size(410, 20),
                ReadOnly = true,
                Name = "txtModifiers"
            };

            Button btnBrowseModifiers = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 24),
                Size = new Size(80, 23),
                Name = "btnBrowseModifiers"
            };

            Label lblYieldRate = new Label
            {
                Text = "收益还原率(%):",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            NumericUpDown numYieldRate = new NumericUpDown
            {
                Location = new Point(140, 55),
                Size = new Size(80, 20),
                DecimalPlaces = 2,
                Increment = 0.01M,
                Minimum = 0.01M,
                Maximum = 10M,
                Value = 3.86M,
                Name = "numYieldRate"
            };

            Button btnLoadPriceParams = new Button
            {
                Text = "加载参数",
                Location = new Point(140, 85),
                Size = new Size(180, 25),
                Name = "btnLoadPriceParams"
            };

            DataGridView dgvPriceFactors = new DataGridView
            {
                Location = new Point(15, 115),
                Size = new Size(640, 60),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Name = "dgvPriceFactors",
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };
            
            dgvPriceFactors.Columns.Add("FactorName", "修正因子名称");
            dgvPriceFactors.Columns.Add("Weight", "权重");

            priceFactorsGroupBox.Controls.Add(lblModifiers);
            priceFactorsGroupBox.Controls.Add(txtModifiers);
            priceFactorsGroupBox.Controls.Add(btnBrowseModifiers);
            priceFactorsGroupBox.Controls.Add(lblYieldRate);
            priceFactorsGroupBox.Controls.Add(numYieldRate);
            priceFactorsGroupBox.Controls.Add(btnLoadPriceParams);
            priceFactorsGroupBox.Controls.Add(dgvPriceFactors);

            tab.Controls.Add(priceParamsGroupBox);
            tab.Controls.Add(priceFactorsGroupBox);

            // Wire up events
            btnBrowseGradeIndices.Click += (sender, e) => BrowseForData(txtGradeIndices, "选择定级指标数据", "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*");
            btnBrowseBasePriceData.Click += (sender, e) => BrowseForData(txtBasePriceData, "选择基准价格数据", "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*");
            btnBrowseModifiers.Click += (sender, e) => BrowseForData(txtModifiers, "选择价格修正因子数据", "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*");
            btnExtractPriceParams.Click += BtnExtractPriceParams_Click;
            btnLoadPriceParams.Click += BtnLoadPriceParams_Click;

            return tab;
        }

        private TabPage CreateSupplementPriceTabPage()
        {
            TabPage tab = new TabPage("4. 补充基准地价");

            GroupBox supplementSettingsGroupBox = new GroupBox
            {
                Text = "补充设置",
                Location = new Point(10, 10),
                Size = new Size(670, 90),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblSupplementMethod = new Label
            {
                Text = "补充方法:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            ComboBox cboSupplementMethod = new ComboBox
            {
                Location = new Point(140, 25),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cboSupplementMethod"
            };
            
            cboSupplementMethod.Items.AddRange(new object[] { "加权平均法", "邻近插值法", "反距离加权插值", "克里金插值法" });
            cboSupplementMethod.SelectedIndex = 0;

            Label lblDefaultBasePrice = new Label
            {
                Text = "默认基准价格:",
                Location = new Point(350, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            NumericUpDown numDefaultBasePrice = new NumericUpDown
            {
                Location = new Point(480, 25),
                Size = new Size(100, 20),
                DecimalPlaces = 2,
                Increment = 0.01M,
                ThousandsSeparator = true,
                Minimum = 0.01M,
                Maximum = 100000M,
                Value = 5.00M,
                Name = "numDefaultBasePrice"
            };

            Button btnSupplementPrice = new Button
            {
                Text = "补充基准地价",
                Location = new Point(140, 55),
                Size = new Size(180, 25),
                Name = "btnSupplementPrice"
            };

            supplementSettingsGroupBox.Controls.Add(lblSupplementMethod);
            supplementSettingsGroupBox.Controls.Add(cboSupplementMethod);
            supplementSettingsGroupBox.Controls.Add(lblDefaultBasePrice);
            supplementSettingsGroupBox.Controls.Add(numDefaultBasePrice);
            supplementSettingsGroupBox.Controls.Add(btnSupplementPrice);

            GroupBox supplementResultsGroupBox = new GroupBox
            {
                Text = "补充结果",
                Location = new Point(10, 110),
                Size = new Size(670, 215),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            Label lblBeforeCount = new Label
            {
                Text = "补充前缺失数量:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblBeforeCountValue = new Label
            {
                Text = "尚未执行",
                Location = new Point(140, 25),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblBeforeCountValue"
            };

            Label lblAfterCount = new Label
            {
                Text = "补充后缺失数量:",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblAfterCountValue = new Label
            {
                Text = "尚未执行",
                Location = new Point(140, 55),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblAfterCountValue"
            };

            Button btnSaveSupplementResults = new Button
            {
                Text = "保存补充结果",
                Location = new Point(15, 85),
                Size = new Size(180, 25),
                Name = "btnSaveSupplementResults",
                Enabled = false
            };

            Button btnViewPriceDistribution = new Button
            {
                Text = "查看价格分布",
                Location = new Point(210, 85),
                Size = new Size(180, 25),
                Name = "btnViewPriceDistribution",
                Enabled = false
            };
            
            Panel priceDistributionPanel = new Panel
            {
                Location = new Point(15, 120),
                Size = new Size(640, 85),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BorderStyle = BorderStyle.FixedSingle,
                Name = "priceDistributionPanel"
            };

            supplementResultsGroupBox.Controls.Add(lblBeforeCount);
            supplementResultsGroupBox.Controls.Add(lblBeforeCountValue);
            supplementResultsGroupBox.Controls.Add(lblAfterCount);
            supplementResultsGroupBox.Controls.Add(lblAfterCountValue);
            supplementResultsGroupBox.Controls.Add(btnSaveSupplementResults);
            supplementResultsGroupBox.Controls.Add(btnViewPriceDistribution);
            supplementResultsGroupBox.Controls.Add(priceDistributionPanel);

            tab.Controls.Add(supplementSettingsGroupBox);
            tab.Controls.Add(supplementResultsGroupBox);

            // Wire up events
            btnSupplementPrice.Click += BtnSupplementPrice_Click;
            btnSaveSupplementResults.Click += BtnSaveSupplementResults_Click;
            btnViewPriceDistribution.Click += BtnViewPriceDistribution_Click;

            return tab;
        }

        private TabPage CreateCalculateValueTabPage()
        {
            TabPage tab = new TabPage("5. 资源资产价值计算");

            GroupBox calculationSettingsGroupBox = new GroupBox
            {
                Text = "计算设置",
                Location = new Point(10, 10),
                Size = new Size(670, 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblDateModifier = new Label
            {
                Text = "期日修正系数:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            NumericUpDown numDateModifier = new NumericUpDown
            {
                Location = new Point(140, 25),
                Size = new Size(80, 20),
                DecimalPlaces = 2,
                Increment = 0.01M,
                Minimum = 0.50M,
                Maximum = 2.00M,
                Value = 1.0M,
                Name = "numDateModifier"
            };

            Label lblPeriodModifier = new Label
            {
                Text = "年期修正系数:",
                Location = new Point(260, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            NumericUpDown numPeriodModifier = new NumericUpDown
            {
                Location = new Point(385, 25),
                Size = new Size(80, 20),
                DecimalPlaces = 2,
                Increment = 0.01M,
                Minimum = 0.50M,
                Maximum = 2.00M,
                Value = 1.0M,
                Name = "numPeriodModifier"
            };

            Label lblCalcMethod = new Label
            {
                Text = "计算方法:",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            ComboBox cboCalcMethod = new ComboBox
            {
                Location = new Point(140, 55),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cboCalcMethod"
            };
            
            cboCalcMethod.Items.AddRange(new object[] { "基准价格法", "收益还原法", "市场比较法" });
            cboCalcMethod.SelectedIndex = 0;

            CheckBox chkExportResults = new CheckBox
            {
                Text = "导出计算结果",
                Location = new Point(385, 55),
                Size = new Size(120, 20),
                Checked = true,
                Name = "chkExportResults"
            };

            Button btnCalculateValue = new Button
            {
                Text = "计算资产价值",
                Location = new Point(140, 85),
                Size = new Size(180, 25),
                Name = "btnCalculateValue"
            };

            calculationSettingsGroupBox.Controls.Add(lblDateModifier);
            calculationSettingsGroupBox.Controls.Add(numDateModifier);
            calculationSettingsGroupBox.Controls.Add(lblPeriodModifier);
            calculationSettingsGroupBox.Controls.Add(numPeriodModifier);
            calculationSettingsGroupBox.Controls.Add(lblCalcMethod);
            calculationSettingsGroupBox.Controls.Add(cboCalcMethod);
            calculationSettingsGroupBox.Controls.Add(chkExportResults);
            calculationSettingsGroupBox.Controls.Add(btnCalculateValue);

            GroupBox resultSummaryGroupBox = new GroupBox
            {
                Text = "计算结果摘要",
                Location = new Point(10, 140),
                Size = new Size(670, 185),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            Label lblTotalValue = new Label
            {
                Text = "总价值(万元):",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblTotalValueResult = new Label
            {
                Text = "尚未计算",
                Location = new Point(140, 25),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("宋体", 9F, FontStyle.Bold),
                Name = "lblTotalValueResult"
            };

            Label lblAverageUnitPrice = new Label
            {
                Text = "平均单价(万元/公顷):",
                Location = new Point(300, 25),
                Size = new Size(140, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblAverageUnitPriceResult = new Label
            {
                Text = "尚未计算",
                Location = new Point(445, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("宋体", 9F, FontStyle.Bold),
                Name = "lblAverageUnitPriceResult"
            };

            Label lblTotalArea = new Label
            {
                Text = "总面积(公顷):",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblTotalAreaResult = new Label
            {
                Text = "尚未计算",
                Location = new Point(140, 55),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblTotalAreaResult"
            };

            Label lblParcelCount = new Label
            {
                Text = "图斑数量:",
                Location = new Point(300, 55),
                Size = new Size(140, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblParcelCountResult = new Label
            {
                Text = "尚未计算",
                Location = new Point(445, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblParcelCountResult"
            };

            Button btnSaveCalculationResults = new Button
            {
                Text = "保存计算结果",
                Location = new Point(15, 85),
                Size = new Size(180, 25),
                Name = "btnSaveCalculationResults",
                Enabled = false
            };

            Button btnViewValueStats = new Button
            {
                Text = "查看价值统计",
                Location = new Point(210, 85),
                Size = new Size(180, 25),
                Name = "btnViewValueStats",
                Enabled = false
            };
            
            Panel valueDistributionPanel = new Panel
            {
                Location = new Point(15, 120),
                Size = new Size(640, 55),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BorderStyle = BorderStyle.FixedSingle,
                Name = "valueDistributionPanel"
            };

            resultSummaryGroupBox.Controls.Add(lblTotalValue);
            resultSummaryGroupBox.Controls.Add(lblTotalValueResult);
            resultSummaryGroupBox.Controls.Add(lblAverageUnitPrice);
            resultSummaryGroupBox.Controls.Add(lblAverageUnitPriceResult);
            resultSummaryGroupBox.Controls.Add(lblTotalArea);
            resultSummaryGroupBox.Controls.Add(lblTotalAreaResult);
            resultSummaryGroupBox.Controls.Add(lblParcelCount);
            resultSummaryGroupBox.Controls.Add(lblParcelCountResult);
            resultSummaryGroupBox.Controls.Add(btnSaveCalculationResults);
            resultSummaryGroupBox.Controls.Add(btnViewValueStats);
            resultSummaryGroupBox.Controls.Add(valueDistributionPanel);

            tab.Controls.Add(calculationSettingsGroupBox);
            tab.Controls.Add(resultSummaryGroupBox);

            // Wire up events
            btnCalculateValue.Click += BtnCalculateValue_Click;
            btnSaveCalculationResults.Click += BtnSaveCalculationResults_Click;
            btnViewValueStats.Click += BtnViewValueStats_Click;
            chkExportResults.CheckedChanged += (sender, e) => UpdateExportOption();

            return tab;
        }

        private TabPage CreateCleanQATabPage()
        {
            TabPage tab = new TabPage("6. 数据清洗质检");

            GroupBox cleaningSettingsGroupBox = new GroupBox
            {
                Text = "清洗设置",
                Location = new Point(10, 10),
                Size = new Size(670, 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblFieldMapping = new Label
            {
                Text = "字段映射表:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtFieldMapping = new TextBox
            {
                Location = new Point(140, 25),
                Size = new Size(410, 20),
                ReadOnly = true,
                Name = "txtFieldMapping"
            };

            Button btnBrowseFieldMapping = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 24),
                Size = new Size(80, 23),
                Name = "btnBrowseFieldMapping"
            };

            CheckBox chkRemoveTempFields = new CheckBox
            {
                Text = "删除临时字段",
                Location = new Point(140, 55),
                Size = new Size(120, 20),
                Checked = true,
                Name = "chkRemoveTempFields"
            };

            CheckBox chkFixGeometryIssues = new CheckBox
            {
                Text = "修复几何问题",
                Location = new Point(270, 55),
                Size = new Size(120, 20),
                Checked = true,
                Name = "chkFixGeometryIssues"
            };

            CheckBox chkValidateDomainValues = new CheckBox
            {
                Text = "验证值域范围",
                Location = new Point(400, 55),
                Size = new Size(120, 20),
                Checked = true,
                Name = "chkValidateDomainValues"
            };

            Button btnCleanQA = new Button
            {
                Text = "执行数据清洗与质检",
                Location = new Point(140, 85),
                Size = new Size(180, 25),
                Name = "btnCleanQA"
            };

            cleaningSettingsGroupBox.Controls.Add(lblFieldMapping);
            cleaningSettingsGroupBox.Controls.Add(txtFieldMapping);
            cleaningSettingsGroupBox.Controls.Add(btnBrowseFieldMapping);
            cleaningSettingsGroupBox.Controls.Add(chkRemoveTempFields);
            cleaningSettingsGroupBox.Controls.Add(chkFixGeometryIssues);
            cleaningSettingsGroupBox.Controls.Add(chkValidateDomainValues);
            cleaningSettingsGroupBox.Controls.Add(btnCleanQA);

            GroupBox qaResultsGroupBox = new GroupBox
            {
                Text = "质检结果",
                Location = new Point(10, 140),
                Size = new Size(670, 185),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            Label lblIssuesFound = new Label
            {
                Text = "发现问题数量:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblIssuesFoundValue = new Label
            {
                Text = "尚未执行",
                Location = new Point(140, 25),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblIssuesFoundValue"
            };

            Label lblIssuesFixed = new Label
            {
                Text = "已修复问题数量:",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblIssuesFixedValue = new Label
            {
                Text = "尚未执行",
                Location = new Point(140, 55),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblIssuesFixedValue"
            };

            Label lblQAPassRate = new Label
            {
                Text = "质检通过率:",
                Location = new Point(280, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblQAPassRateValue = new Label
            {
                Text = "尚未执行",
                Location = new Point(405, 25),
                Size = new Size(100, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblQAPassRateValue"
            };

            Button btnViewQAReport = new Button
            {
                Text = "查看质检报告",
                Location = new Point(15, 85),
                Size = new Size(180, 25),
                Name = "btnViewQAReport",
                Enabled = false
            };

            Button btnSaveCleanData = new Button
            {
                Text = "保存清洗后数据",
                Location = new Point(210, 85),
                Size = new Size(180, 25),
                Name = "btnSaveCleanData",
                Enabled = false
            };

            DataGridView dgvQAIssues = new DataGridView
            {
                Location = new Point(15, 120),
                Size = new Size(640, 55),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Name = "dgvQAIssues",
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };
            
            dgvQAIssues.Columns.Add("IssueType", "问题类型");
            dgvQAIssues.Columns.Add("Count", "数量");
            dgvQAIssues.Columns.Add("Fixed", "已修复");
            dgvQAIssues.Columns.Add("Description", "描述");

            qaResultsGroupBox.Controls.Add(lblIssuesFound);
            qaResultsGroupBox.Controls.Add(lblIssuesFoundValue);
            qaResultsGroupBox.Controls.Add(lblIssuesFixed);
            qaResultsGroupBox.Controls.Add(lblIssuesFixedValue);
            qaResultsGroupBox.Controls.Add(lblQAPassRate);
            qaResultsGroupBox.Controls.Add(lblQAPassRateValue);
            qaResultsGroupBox.Controls.Add(btnViewQAReport);
            qaResultsGroupBox.Controls.Add(btnSaveCleanData);
            qaResultsGroupBox.Controls.Add(dgvQAIssues);

            tab.Controls.Add(cleaningSettingsGroupBox);
            tab.Controls.Add(qaResultsGroupBox);

            // Wire up events
            btnBrowseFieldMapping.Click += (sender, e) => BrowseForData(txtFieldMapping, "选择字段映射表", "Excel文件 (*.xlsx)|*.xlsx|CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*");
            btnCleanQA.Click += BtnCleanQA_Click;
            btnViewQAReport.Click += BtnViewQAReport_Click;
            btnSaveCleanData.Click += BtnSaveCleanData_Click;

            return tab;
        }

        private TabPage CreateBuildDatabaseTabPage()
        {
            TabPage tab = new TabPage("7. 构建数据库");

            GroupBox dbSettingsGroupBox = new GroupBox
            {
                Text = "数据库设置",
                Location = new Point(10, 10),
                Size = new Size(670, 120),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label lblOutputLocation = new Label
            {
                Text = "输出位置:",
                Location = new Point(15, 25),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtOutputLocation = new TextBox
            {
                Location = new Point(140, 25),
                Size = new Size(410, 20),
                ReadOnly = true,
                Name = "txtOutputLocation"
            };

            Button btnBrowseOutputLocation = new Button
            {
                Text = "浏览...",
                Location = new Point(560, 24),
                Size = new Size(80, 23),
                Name = "btnBrowseOutputLocation"
            };

            Label lblOutputName = new Label
            {
                Text = "输出名称:",
                Location = new Point(15, 55),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            TextBox txtOutputName = new TextBox
            {
                Location = new Point(140, 55),
                Size = new Size(200, 20),
                Text = "ForestAssetInventory",
                Name = "txtOutputName"
            };

            Label lblOutputFormat = new Label
            {
                Text = "输出格式:",
                Location = new Point(350, 55),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            ComboBox cboOutputFormat = new ComboBox
            {
                Location = new Point(445, 55),
                Size = new Size(160, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cboOutputFormat"
            };
            
            cboOutputFormat.Items.AddRange(new object[] { "File Geodatabase", "个人地理数据库", "Excel工作簿", "Shapefile" });
            cboOutputFormat.SelectedIndex = 0;

            Button btnBuildDatabase = new Button
            {
                Text = "构建数据库",
                Location = new Point(140, 85),
                Size = new Size(180, 25),
                Name = "btnBuildDatabase"
            };

            dbSettingsGroupBox.Controls.Add(lblOutputLocation);
            dbSettingsGroupBox.Controls.Add(txtOutputLocation);
            dbSettingsGroupBox.Controls.Add(btnBrowseOutputLocation);
            dbSettingsGroupBox.Controls.Add(lblOutputName);
            dbSettingsGroupBox.Controls.Add(txtOutputName);
            dbSettingsGroupBox.Controls.Add(lblOutputFormat);
            dbSettingsGroupBox.Controls.Add(cboOutputFormat);
            dbSettingsGroupBox.Controls.Add(btnBuildDatabase);

            GroupBox outputTablesGroupBox = new GroupBox
            {
                Text = "输出数据表",
                Location = new Point(10, 140),
                Size = new Size(670, 185),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            CheckedListBox clbOutputTables = new CheckedListBox
            {
                Location = new Point(15, 25),
                Size = new Size(640, 85),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Name = "clbOutputTables",
                CheckOnClick = true
            };
            
            clbOutputTables.Items.AddRange(new object[] { 
                "森林资源资产空间数据集", 
                "森林资源资产基础数表", 
                "森林资源资产统计表", 
                "林地分等数据表",
                "林地基准地价表" 
            });
            
            // Check all items by default
            for(int i = 0; i < clbOutputTables.Items.Count; i++)
                clbOutputTables.SetItemChecked(i, true);

            Label lblOutputStatus = new Label
            {
                Text = "输出状态:",
                Location = new Point(15, 120),
                Size = new Size(120, 20),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblOutputStatusValue = new Label
            {
                Text = "尚未执行",
                Location = new Point(140, 120),
                Size = new Size(515, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                Name = "lblOutputStatusValue"
            };

            Button btnViewOutputFiles = new Button
            {
                Text = "查看输出文件",
                Location = new Point(15, 150),
                Size = new Size(180, 25),
                Name = "btnViewOutputFiles",
                Enabled = false
            };

            Button btnGenerateReport = new Button
            {
                Text = "生成清查报告",
                Location = new Point(210, 150),
                Size = new Size(180, 25),
                Name = "btnGenerateReport",
                Enabled = false
            };

            outputTablesGroupBox.Controls.Add(clbOutputTables);
            outputTablesGroupBox.Controls.Add(lblOutputStatus);
            outputTablesGroupBox.Controls.Add(lblOutputStatusValue);
            outputTablesGroupBox.Controls.Add(btnViewOutputFiles);
            outputTablesGroupBox.Controls.Add(btnGenerateReport);

            tab.Controls.Add(dbSettingsGroupBox);
            tab.Controls.Add(outputTablesGroupBox);

            // Wire up events
            btnBrowseOutputLocation.Click += (sender, e) => BrowseForOutputFolder(txtOutputLocation);
            btnBuildDatabase.Click += BtnBuildDatabase_Click;
            btnViewOutputFiles.Click += BtnViewOutputFiles_Click;
            btnGenerateReport.Click += BtnGenerateReport_Click;

            return tab;
        }

        #endregion

        #region Event Handlers

        private void SelectWorkspace()
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "选择工作目录";
                dlg.ShowNewFolderButton = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var txtWorkspace = this.mainPanel.Controls.Find("txtWorkspace", true)[0] as TextBox;
                    workingDirectory = dlg.SelectedPath;
                    txtWorkspace.Text = workingDirectory;
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
                var txtForestData = this.mainPanel.Controls.Find("txtForestData", true)[0] as TextBox;
                var txtUrbanBoundary = this.mainPanel.Controls.Find("txtUrbanBoundary", true)[0] as TextBox;

                txtForestData.Text = @"C:\GIS_Data\林草湿荒普查数据.shp";
                txtUrbanBoundary.Text = @"C:\GIS_Data\城镇开发边界.shp";

                // Populate field dropdowns
                var cboLandTypeField = this.mainPanel.Controls.Find("cboLandTypeField", true)[0] as ComboBox;
                var cboOwnershipField = this.mainPanel.Controls.Find("cboOwnershipField", true)[0] as ComboBox;

                cboLandTypeField.Items.Clear();
                cboLandTypeField.Items.AddRange(new object[] { "DLBM", "TDLYLX", "DLMC" });
                cboLandTypeField.SelectedIndex = 0;

                cboOwnershipField.Items.Clear();
                cboOwnershipField.Items.AddRange(new object[] { "QSDW", "QSLX", "QSXZ" });
                cboOwnershipField.SelectedIndex = 0;

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

                var lblExtractionResults = this.mainPanel.Controls.Find("lblExtractionResults", true)[0] as Label;
                lblExtractionResults.Text = "工作范围提取完成";

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

                var lblGradeMatchRateValue = this.mainPanel.Controls.Find("lblGradeMatchRateValue", true)[0] as Label;
                lblGradeMatchRateValue.Text = "95%";

                var btnSaveBaseMap = this.mainPanel.Controls.Find("btnSaveBaseMap", true)[0] as Button;
                var btnViewBaseMap = this.mainPanel.Controls.Find("btnViewBaseMap", true)[0] as Button;
                btnSaveBaseMap.Enabled = true;
                btnViewBaseMap.Enabled = true;

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

                var lblPriceMatchRateValue = this.mainPanel.Controls.Find("lblPriceMatchRateValue", true)[0] as Label;
                lblPriceMatchRateValue.Text = "90%";

                var btnSaveBaseMap = this.mainPanel.Controls.Find("btnSaveBaseMap", true)[0] as Button;
                var btnViewBaseMap = this.mainPanel.Controls.Find("btnViewBaseMap", true)[0] as Button;
                btnSaveBaseMap.Enabled = true;
                btnViewBaseMap.Enabled = true;

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

                var dgvPriceFactors = this.mainPanel.Controls.Find("dgvPriceFactors", true)[0] as DataGridView;
                dgvPriceFactors.Rows.Clear();
                dgvPriceFactors.Rows.Add("修正因子1", "0.5");
                dgvPriceFactors.Rows.Add("修正因子2", "0.3");
                dgvPriceFactors.Rows.Add("修正因子3", "0.2");

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

                var lblBeforeCountValue = this.mainPanel.Controls.Find("lblBeforeCountValue", true)[0] as Label;
                var lblAfterCountValue = this.mainPanel.Controls.Find("lblAfterCountValue", true)[0] as Label;
                lblBeforeCountValue.Text = "100";
                lblAfterCountValue.Text = "0";

                var btnSaveSupplementResults = this.mainPanel.Controls.Find("btnSaveSupplementResults", true)[0] as Button;
                var btnViewPriceDistribution = this.mainPanel.Controls.Find("btnViewPriceDistribution", true)[0] as Button;
                btnSaveSupplementResults.Enabled = true;
                btnViewPriceDistribution.Enabled = true;

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

                var lblTotalValueResult = this.mainPanel.Controls.Find("lblTotalValueResult", true)[0] as Label;
                var lblAverageUnitPriceResult = this.mainPanel.Controls.Find("lblAverageUnitPriceResult", true)[0] as Label;
                var lblTotalAreaResult = this.mainPanel.Controls.Find("lblTotalAreaResult", true)[0] as Label;
                var lblParcelCountResult = this.mainPanel.Controls.Find("lblParcelCountResult", true)[0] as Label;
                lblTotalValueResult.Text = "10652896";
                lblAverageUnitPriceResult.Text = "8.42";
                lblTotalAreaResult.Text = "1265.89";
                lblParcelCountResult.Text = "150";

                var btnSaveCalculationResults = this.mainPanel.Controls.Find("btnSaveCalculationResults", true)[0] as Button;
                var btnViewValueStats = this.mainPanel.Controls.Find("btnViewValueStats", true)[0] as Button;
                btnSaveCalculationResults.Enabled = true;
                btnViewValueStats.Enabled = true;

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

                var lblIssuesFoundValue = this.mainPanel.Controls.Find("lblIssuesFoundValue", true)[0] as Label;
                var lblIssuesFixedValue = this.mainPanel.Controls.Find("lblIssuesFixedValue", true)[0] as Label;
                var lblQAPassRateValue = this.mainPanel.Controls.Find("lblQAPassRateValue", true)[0] as Label;
                lblIssuesFoundValue.Text = "10";
                lblIssuesFixedValue.Text = "10";
                lblQAPassRateValue.Text = "100%";

                var btnViewQAReport = this.mainPanel.Controls.Find("btnViewQAReport", true)[0] as Button;
                var btnSaveCleanData = this.mainPanel.Controls.Find("btnSaveCleanData", true)[0] as Button;
                btnViewQAReport.Enabled = true;
                btnSaveCleanData.Enabled = true;

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

                var lblOutputStatusValue = this.mainPanel.Controls.Find("lblOutputStatusValue", true)[0] as Label;
                lblOutputStatusValue.Text = "数据库构建完成";

                var btnViewOutputFiles = this.mainPanel.Controls.Find("btnViewOutputFiles", true)[0] as Button;
                var btnGenerateReport = this.mainPanel.Controls.Find("btnGenerateReport", true)[0] as Button;
                btnViewOutputFiles.Enabled = true;
                btnGenerateReport.Enabled = true;

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
            var chkExportResults = this.mainPanel.Controls.Find("chkExportResults", true)[0] as CheckBox;
            var btnSaveCalculationResults = this.mainPanel.Controls.Find("btnSaveCalculationResults", true)[0] as Button;
            btnSaveCalculationResults.Enabled = chkExportResults.Checked;
        }

        #endregion

        protected override void StartButton_Click(object sender, EventArgs e)
        {
            base.StartButton_Click(sender, e);

            // Get the file paths from the text boxes
            var txtBasePriceData = this.mainPanel.Controls.Find("txtBasePriceData", true)[0] as TextBox;
            var txtModifiersData = this.mainPanel.Controls.Find("txtModifiersData", true)[0] as TextBox;

            if (string.IsNullOrEmpty(txtBasePriceData.Text) || string.IsNullOrEmpty(txtModifiersData.Text))
            {
                MessageBox.Show("请先选择所需数据文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }

            // Get calculation settings
            var numYieldRate = this.mainPanel.Controls.Find("numYieldRate", true)[0] as NumericUpDown;
            var numDateModifier = this.mainPanel.Controls.Find("numDateModifier", true)[0] as NumericUpDown;
            var numPeriodModifier = this.mainPanel.Controls.Find("numPeriodModifier", true)[0] as NumericUpDown;
            var chkExportResults = this.mainPanel.Controls.Find("chkExportResults", true)[0] as CheckBox;

            decimal yieldRate = numYieldRate.Value;
            decimal dateModifier = numDateModifier.Value;
            decimal periodModifier = numPeriodModifier.Value;
            bool exportResults = chkExportResults.Checked;

            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessCalculateAssetValue(
                txtBasePriceData.Text,
                txtModifiersData.Text,
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
    }
}