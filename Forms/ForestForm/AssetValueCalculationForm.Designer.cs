namespace TestArcMapAddin2.Forms.ForestForm
{
    partial class AssetValueCalculationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.workspaceGroupBox = new System.Windows.Forms.GroupBox();
            this.lblWorkspace = new System.Windows.Forms.Label();
            this.txtWorkspace = new System.Windows.Forms.TextBox();
            this.btnSelectWorkspace = new System.Windows.Forms.Button();
            this.workflowTabs = new System.Windows.Forms.TabControl();
            this.extractScopeTab = new System.Windows.Forms.TabPage();
            this.dataSourceGroupBox = new System.Windows.Forms.GroupBox();
            this.lblForestData = new System.Windows.Forms.Label();
            this.txtForestData = new System.Windows.Forms.TextBox();
            this.btnBrowseForestData = new System.Windows.Forms.Button();
            this.lblUrbanBoundary = new System.Windows.Forms.Label();
            this.txtUrbanBoundary = new System.Windows.Forms.TextBox();
            this.btnBrowseUrbanBoundary = new System.Windows.Forms.Button();
            this.btnLoadCurrentMap = new System.Windows.Forms.Button();
            this.extractionSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblLandTypeField = new System.Windows.Forms.Label();
            this.cboLandTypeField = new System.Windows.Forms.ComboBox();
            this.lblOwnershipField = new System.Windows.Forms.Label();
            this.cboOwnershipField = new System.Windows.Forms.ComboBox();
            this.lblForestValue = new System.Windows.Forms.Label();
            this.txtForestValue = new System.Windows.Forms.TextBox();
            this.lblStateOwnershipValue = new System.Windows.Forms.Label();
            this.txtStateOwnershipValue = new System.Windows.Forms.TextBox();
            this.lblCollectiveValue = new System.Windows.Forms.Label();
            this.txtCollectiveValue = new System.Windows.Forms.TextBox();
            this.btnExtractScope = new System.Windows.Forms.Button();
            this.resultsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblExtractionResults = new System.Windows.Forms.Label();
            this.createBaseMapTab = new System.Windows.Forms.TabPage();
            this.landGradeGroupBox = new System.Windows.Forms.GroupBox();
            this.lblLandGradeData = new System.Windows.Forms.Label();
            this.txtLandGradeData = new System.Windows.Forms.TextBox();
            this.btnBrowseLandGradeData = new System.Windows.Forms.Button();
            this.btnLinkLandGradeData = new System.Windows.Forms.Button();
            this.landPriceGroupBox = new System.Windows.Forms.GroupBox();
            this.lblLandPriceData = new System.Windows.Forms.Label();
            this.txtLandPriceData = new System.Windows.Forms.TextBox();
            this.btnBrowseLandPriceData = new System.Windows.Forms.Button();
            this.btnLinkLandPriceData = new System.Windows.Forms.Button();
            this.linkResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblGradeMatchRate = new System.Windows.Forms.Label();
            this.lblGradeMatchRateValue = new System.Windows.Forms.Label();
            this.lblPriceMatchRate = new System.Windows.Forms.Label();
            this.lblPriceMatchRateValue = new System.Windows.Forms.Label();
            this.btnSaveBaseMap = new System.Windows.Forms.Button();
            this.btnViewBaseMap = new System.Windows.Forms.Button();
            this.priceParamsTab = new System.Windows.Forms.TabPage();
            this.priceParamsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblGradeIndices = new System.Windows.Forms.Label();
            this.txtGradeIndices = new System.Windows.Forms.TextBox();
            this.btnBrowseGradeIndices = new System.Windows.Forms.Button();
            this.lblBasePriceData = new System.Windows.Forms.Label();
            this.txtBasePriceData = new System.Windows.Forms.TextBox();
            this.btnBrowseBasePriceData = new System.Windows.Forms.Button();
            this.btnExtractPriceParams = new System.Windows.Forms.Button();
            this.priceFactorsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblModifiers = new System.Windows.Forms.Label();
            this.txtModifiers = new System.Windows.Forms.TextBox();
            this.btnBrowseModifiers = new System.Windows.Forms.Button();
            this.lblYieldRate = new System.Windows.Forms.Label();
            this.numYieldRate = new System.Windows.Forms.NumericUpDown();
            this.btnLoadPriceParams = new System.Windows.Forms.Button();
            this.dgvPriceFactors = new System.Windows.Forms.DataGridView();
            this.FactorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplementPriceTab = new System.Windows.Forms.TabPage();
            this.supplementSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblSupplementMethod = new System.Windows.Forms.Label();
            this.cboSupplementMethod = new System.Windows.Forms.ComboBox();
            this.lblDefaultBasePrice = new System.Windows.Forms.Label();
            this.numDefaultBasePrice = new System.Windows.Forms.NumericUpDown();
            this.btnSupplementPrice = new System.Windows.Forms.Button();
            this.supplementResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblBeforeCount = new System.Windows.Forms.Label();
            this.lblBeforeCountValue = new System.Windows.Forms.Label();
            this.lblAfterCount = new System.Windows.Forms.Label();
            this.lblAfterCountValue = new System.Windows.Forms.Label();
            this.btnSaveSupplementResults = new System.Windows.Forms.Button();
            this.btnViewPriceDistribution = new System.Windows.Forms.Button();
            this.priceDistributionPanel = new System.Windows.Forms.Panel();
            this.calculateValueTab = new System.Windows.Forms.TabPage();
            this.calculationSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblDateModifier = new System.Windows.Forms.Label();
            this.numDateModifier = new System.Windows.Forms.NumericUpDown();
            this.lblPeriodModifier = new System.Windows.Forms.Label();
            this.numPeriodModifier = new System.Windows.Forms.NumericUpDown();
            this.lblCalcMethod = new System.Windows.Forms.Label();
            this.cboCalcMethod = new System.Windows.Forms.ComboBox();
            this.chkExportResults = new System.Windows.Forms.CheckBox();
            this.btnCalculateValue = new System.Windows.Forms.Button();
            this.resultSummaryGroupBox = new System.Windows.Forms.GroupBox();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblTotalValueResult = new System.Windows.Forms.Label();
            this.lblAverageUnitPrice = new System.Windows.Forms.Label();
            this.lblAverageUnitPriceResult = new System.Windows.Forms.Label();
            this.lblTotalArea = new System.Windows.Forms.Label();
            this.lblTotalAreaResult = new System.Windows.Forms.Label();
            this.lblParcelCount = new System.Windows.Forms.Label();
            this.lblParcelCountResult = new System.Windows.Forms.Label();
            this.btnSaveCalculationResults = new System.Windows.Forms.Button();
            this.btnViewValueStats = new System.Windows.Forms.Button();
            this.valueDistributionPanel = new System.Windows.Forms.Panel();
            this.cleanQATab = new System.Windows.Forms.TabPage();
            this.cleaningSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblFieldMapping = new System.Windows.Forms.Label();
            this.txtFieldMapping = new System.Windows.Forms.TextBox();
            this.btnBrowseFieldMapping = new System.Windows.Forms.Button();
            this.chkRemoveTempFields = new System.Windows.Forms.CheckBox();
            this.chkFixGeometryIssues = new System.Windows.Forms.CheckBox();
            this.chkValidateDomainValues = new System.Windows.Forms.CheckBox();
            this.btnCleanQA = new System.Windows.Forms.Button();
            this.qaResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblIssuesFound = new System.Windows.Forms.Label();
            this.lblIssuesFoundValue = new System.Windows.Forms.Label();
            this.lblIssuesFixed = new System.Windows.Forms.Label();
            this.lblIssuesFixedValue = new System.Windows.Forms.Label();
            this.lblQAPassRate = new System.Windows.Forms.Label();
            this.lblQAPassRateValue = new System.Windows.Forms.Label();
            this.btnViewQAReport = new System.Windows.Forms.Button();
            this.btnSaveCleanData = new System.Windows.Forms.Button();
            this.dgvQAIssues = new System.Windows.Forms.DataGridView();
            this.IssueType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fixed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buildDatabaseTab = new System.Windows.Forms.TabPage();
            this.dbSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.lblOutputLocation = new System.Windows.Forms.Label();
            this.txtOutputLocation = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputLocation = new System.Windows.Forms.Button();
            this.lblOutputName = new System.Windows.Forms.Label();
            this.txtOutputName = new System.Windows.Forms.TextBox();
            this.lblOutputFormat = new System.Windows.Forms.Label();
            this.cboOutputFormat = new System.Windows.Forms.ComboBox();
            this.btnBuildDatabase = new System.Windows.Forms.Button();
            this.outputTablesGroupBox = new System.Windows.Forms.GroupBox();
            this.clbOutputTables = new System.Windows.Forms.CheckedListBox();
            this.lblOutputStatus = new System.Windows.Forms.Label();
            this.lblOutputStatusValue = new System.Windows.Forms.Label();
            this.btnViewOutputFiles = new System.Windows.Forms.Button();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.workspaceGroupBox.SuspendLayout();
            this.workflowTabs.SuspendLayout();
            this.extractScopeTab.SuspendLayout();
            this.dataSourceGroupBox.SuspendLayout();
            this.extractionSettingsGroupBox.SuspendLayout();
            this.resultsGroupBox.SuspendLayout();
            this.createBaseMapTab.SuspendLayout();
            this.landGradeGroupBox.SuspendLayout();
            this.landPriceGroupBox.SuspendLayout();
            this.linkResultsGroupBox.SuspendLayout();
            this.priceParamsTab.SuspendLayout();
            this.priceParamsGroupBox.SuspendLayout();
            this.priceFactorsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYieldRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceFactors)).BeginInit();
            this.supplementPriceTab.SuspendLayout();
            this.supplementSettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultBasePrice)).BeginInit();
            this.supplementResultsGroupBox.SuspendLayout();
            this.calculateValueTab.SuspendLayout();
            this.calculationSettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDateModifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPeriodModifier)).BeginInit();
            this.resultSummaryGroupBox.SuspendLayout();
            this.cleanQATab.SuspendLayout();
            this.cleaningSettingsGroupBox.SuspendLayout();
            this.qaResultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQAIssues)).BeginInit();
            this.buildDatabaseTab.SuspendLayout();
            this.dbSettingsGroupBox.SuspendLayout();
            this.outputTablesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.workspaceGroupBox);
            this.mainPanel.Controls.Add(this.workflowTabs);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(6);
            this.mainPanel.Padding = new System.Windows.Forms.Padding(33, 30, 33, 30);
            this.mainPanel.Size = new System.Drawing.Size(1101, 825);
            this.mainPanel.Controls.SetChildIndex(this.workflowTabs, 0);
            this.mainPanel.Controls.SetChildIndex(this.workspaceGroupBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.statusLabel, 0);
            this.mainPanel.Controls.SetChildIndex(this.logTextBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.descriptionTextBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.progressBar, 0);
            this.mainPanel.Controls.SetChildIndex(this.titleLabel, 0);
            // 
            // titleLabel
            // 
            this.titleLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.titleLabel.Size = new System.Drawing.Size(1101, 40);
            this.titleLabel.Text = "森林资源资产清查计算";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(32, 68);
            this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.descriptionTextBox.Size = new System.Drawing.Size(1041, 41);
            this.descriptionTextBox.Text = "本工具基于林草湿荒普查数据，结合城镇开发边界、林地分等定级和基准地价等数据，进行森林资源资产价值计算，包括工作范围提取、底图制作、价格参数提取、价格补充、资产价值" +
    "计算、数据质检和数据库构建等步骤。";
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(47, 1132);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.logTextBox.Size = new System.Drawing.Size(1280, 103);
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusLabel.Location = new System.Drawing.Point(22, 1275);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.statusLabel.Size = new System.Drawing.Size(1025, 28);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(22, 1302);
            this.progressBar.Margin = new System.Windows.Forms.Padding(6);
            this.progressBar.Size = new System.Drawing.Size(1025, 28);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Location = new System.Drawing.Point(0, 825);
            this.bottomPanel.Size = new System.Drawing.Size(1101, 90);
            // 
            // workspaceGroupBox
            // 
            this.workspaceGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workspaceGroupBox.Controls.Add(this.lblWorkspace);
            this.workspaceGroupBox.Controls.Add(this.txtWorkspace);
            this.workspaceGroupBox.Controls.Add(this.btnSelectWorkspace);
            this.workspaceGroupBox.Location = new System.Drawing.Point(22, 111);
            this.workspaceGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.workspaceGroupBox.Name = "workspaceGroupBox";
            this.workspaceGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.workspaceGroupBox.Size = new System.Drawing.Size(1051, 83);
            this.workspaceGroupBox.TabIndex = 0;
            this.workspaceGroupBox.TabStop = false;
            this.workspaceGroupBox.Text = "工作目录设置";
            // 
            // lblWorkspace
            // 
            this.lblWorkspace.Location = new System.Drawing.Point(22, 35);
            this.lblWorkspace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(120, 28);
            this.lblWorkspace.TabIndex = 0;
            this.lblWorkspace.Text = "工作目录:";
            this.lblWorkspace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWorkspace
            // 
            this.txtWorkspace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkspace.Location = new System.Drawing.Point(150, 35);
            this.txtWorkspace.Margin = new System.Windows.Forms.Padding(4);
            this.txtWorkspace.Name = "txtWorkspace";
            this.txtWorkspace.ReadOnly = true;
            this.txtWorkspace.Size = new System.Drawing.Size(719, 28);
            this.txtWorkspace.TabIndex = 1;
            // 
            // btnSelectWorkspace
            // 
            this.btnSelectWorkspace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectWorkspace.Location = new System.Drawing.Point(885, 33);
            this.btnSelectWorkspace.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectWorkspace.Name = "btnSelectWorkspace";
            this.btnSelectWorkspace.Size = new System.Drawing.Size(120, 32);
            this.btnSelectWorkspace.TabIndex = 2;
            this.btnSelectWorkspace.Text = "选择...";
            this.btnSelectWorkspace.UseVisualStyleBackColor = true;
            // 
            // workflowTabs
            // 
            this.workflowTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workflowTabs.Controls.Add(this.extractScopeTab);
            this.workflowTabs.Controls.Add(this.createBaseMapTab);
            this.workflowTabs.Controls.Add(this.priceParamsTab);
            this.workflowTabs.Controls.Add(this.supplementPriceTab);
            this.workflowTabs.Controls.Add(this.calculateValueTab);
            this.workflowTabs.Controls.Add(this.cleanQATab);
            this.workflowTabs.Controls.Add(this.buildDatabaseTab);
            this.workflowTabs.Location = new System.Drawing.Point(22, 208);
            this.workflowTabs.Margin = new System.Windows.Forms.Padding(4);
            this.workflowTabs.Name = "workflowTabs";
            this.workflowTabs.SelectedIndex = 0;
            this.workflowTabs.Size = new System.Drawing.Size(1051, 610);
            this.workflowTabs.TabIndex = 1;
            // 
            // extractScopeTab
            // 
            this.extractScopeTab.Controls.Add(this.dataSourceGroupBox);
            this.extractScopeTab.Controls.Add(this.extractionSettingsGroupBox);
            this.extractScopeTab.Controls.Add(this.resultsGroupBox);
            this.extractScopeTab.Location = new System.Drawing.Point(4, 28);
            this.extractScopeTab.Margin = new System.Windows.Forms.Padding(4);
            this.extractScopeTab.Name = "extractScopeTab";
            this.extractScopeTab.Padding = new System.Windows.Forms.Padding(4);
            this.extractScopeTab.Size = new System.Drawing.Size(1043, 578);
            this.extractScopeTab.TabIndex = 0;
            this.extractScopeTab.Text = "1. 提取工作范围";
            this.extractScopeTab.UseVisualStyleBackColor = true;
            // 
            // dataSourceGroupBox
            // 
            this.dataSourceGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataSourceGroupBox.Controls.Add(this.lblForestData);
            this.dataSourceGroupBox.Controls.Add(this.txtForestData);
            this.dataSourceGroupBox.Controls.Add(this.btnBrowseForestData);
            this.dataSourceGroupBox.Controls.Add(this.lblUrbanBoundary);
            this.dataSourceGroupBox.Controls.Add(this.txtUrbanBoundary);
            this.dataSourceGroupBox.Controls.Add(this.btnBrowseUrbanBoundary);
            this.dataSourceGroupBox.Controls.Add(this.btnLoadCurrentMap);
            this.dataSourceGroupBox.Location = new System.Drawing.Point(15, 14);
            this.dataSourceGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.dataSourceGroupBox.Name = "dataSourceGroupBox";
            this.dataSourceGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.dataSourceGroupBox.Size = new System.Drawing.Size(1005, 166);
            this.dataSourceGroupBox.TabIndex = 0;
            this.dataSourceGroupBox.TabStop = false;
            this.dataSourceGroupBox.Text = "数据来源";
            // 
            // lblForestData
            // 
            this.lblForestData.Location = new System.Drawing.Point(22, 35);
            this.lblForestData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForestData.Name = "lblForestData";
            this.lblForestData.Size = new System.Drawing.Size(210, 28);
            this.lblForestData.TabIndex = 0;
            this.lblForestData.Text = "林草湿荒普查数据:";
            this.lblForestData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtForestData
            // 
            this.txtForestData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtForestData.Location = new System.Drawing.Point(240, 35);
            this.txtForestData.Margin = new System.Windows.Forms.Padding(4);
            this.txtForestData.Name = "txtForestData";
            this.txtForestData.ReadOnly = true;
            this.txtForestData.Size = new System.Drawing.Size(583, 28);
            this.txtForestData.TabIndex = 1;
            // 
            // btnBrowseForestData
            // 
            this.btnBrowseForestData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseForestData.Location = new System.Drawing.Point(841, 33);
            this.btnBrowseForestData.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseForestData.Name = "btnBrowseForestData";
            this.btnBrowseForestData.Size = new System.Drawing.Size(120, 32);
            this.btnBrowseForestData.TabIndex = 2;
            this.btnBrowseForestData.Text = "浏览...";
            this.btnBrowseForestData.UseVisualStyleBackColor = true;
            // 
            // lblUrbanBoundary
            // 
            this.lblUrbanBoundary.Location = new System.Drawing.Point(22, 76);
            this.lblUrbanBoundary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUrbanBoundary.Name = "lblUrbanBoundary";
            this.lblUrbanBoundary.Size = new System.Drawing.Size(210, 28);
            this.lblUrbanBoundary.TabIndex = 3;
            this.lblUrbanBoundary.Text = "城镇开发边界数据:";
            this.lblUrbanBoundary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUrbanBoundary
            // 
            this.txtUrbanBoundary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrbanBoundary.Location = new System.Drawing.Point(240, 76);
            this.txtUrbanBoundary.Margin = new System.Windows.Forms.Padding(4);
            this.txtUrbanBoundary.Name = "txtUrbanBoundary";
            this.txtUrbanBoundary.ReadOnly = true;
            this.txtUrbanBoundary.Size = new System.Drawing.Size(583, 28);
            this.txtUrbanBoundary.TabIndex = 4;
            // 
            // btnBrowseUrbanBoundary
            // 
            this.btnBrowseUrbanBoundary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseUrbanBoundary.Location = new System.Drawing.Point(841, 75);
            this.btnBrowseUrbanBoundary.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseUrbanBoundary.Name = "btnBrowseUrbanBoundary";
            this.btnBrowseUrbanBoundary.Size = new System.Drawing.Size(120, 32);
            this.btnBrowseUrbanBoundary.TabIndex = 5;
            this.btnBrowseUrbanBoundary.Text = "浏览...";
            this.btnBrowseUrbanBoundary.UseVisualStyleBackColor = true;
            // 
            // btnLoadCurrentMap
            // 
            this.btnLoadCurrentMap.Location = new System.Drawing.Point(240, 118);
            this.btnLoadCurrentMap.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadCurrentMap.Name = "btnLoadCurrentMap";
            this.btnLoadCurrentMap.Size = new System.Drawing.Size(270, 35);
            this.btnLoadCurrentMap.TabIndex = 6;
            this.btnLoadCurrentMap.Text = "加载当前地图数据";
            this.btnLoadCurrentMap.UseVisualStyleBackColor = true;
            // 
            // extractionSettingsGroupBox
            // 
            this.extractionSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extractionSettingsGroupBox.Controls.Add(this.lblLandTypeField);
            this.extractionSettingsGroupBox.Controls.Add(this.cboLandTypeField);
            this.extractionSettingsGroupBox.Controls.Add(this.lblOwnershipField);
            this.extractionSettingsGroupBox.Controls.Add(this.cboOwnershipField);
            this.extractionSettingsGroupBox.Controls.Add(this.lblForestValue);
            this.extractionSettingsGroupBox.Controls.Add(this.txtForestValue);
            this.extractionSettingsGroupBox.Controls.Add(this.lblStateOwnershipValue);
            this.extractionSettingsGroupBox.Controls.Add(this.txtStateOwnershipValue);
            this.extractionSettingsGroupBox.Controls.Add(this.lblCollectiveValue);
            this.extractionSettingsGroupBox.Controls.Add(this.txtCollectiveValue);
            this.extractionSettingsGroupBox.Controls.Add(this.btnExtractScope);
            this.extractionSettingsGroupBox.Location = new System.Drawing.Point(15, 194);
            this.extractionSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.extractionSettingsGroupBox.Name = "extractionSettingsGroupBox";
            this.extractionSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.extractionSettingsGroupBox.Size = new System.Drawing.Size(1005, 166);
            this.extractionSettingsGroupBox.TabIndex = 1;
            this.extractionSettingsGroupBox.TabStop = false;
            this.extractionSettingsGroupBox.Text = "提取设置";
            // 
            // lblLandTypeField
            // 
            this.lblLandTypeField.Location = new System.Drawing.Point(22, 35);
            this.lblLandTypeField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLandTypeField.Name = "lblLandTypeField";
            this.lblLandTypeField.Size = new System.Drawing.Size(195, 28);
            this.lblLandTypeField.TabIndex = 0;
            this.lblLandTypeField.Text = "地类字段:";
            this.lblLandTypeField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboLandTypeField
            // 
            this.cboLandTypeField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLandTypeField.FormattingEnabled = true;
            this.cboLandTypeField.Location = new System.Drawing.Point(225, 35);
            this.cboLandTypeField.Margin = new System.Windows.Forms.Padding(4);
            this.cboLandTypeField.Name = "cboLandTypeField";
            this.cboLandTypeField.Size = new System.Drawing.Size(298, 26);
            this.cboLandTypeField.TabIndex = 1;
            // 
            // lblOwnershipField
            // 
            this.lblOwnershipField.Location = new System.Drawing.Point(22, 76);
            this.lblOwnershipField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOwnershipField.Name = "lblOwnershipField";
            this.lblOwnershipField.Size = new System.Drawing.Size(195, 28);
            this.lblOwnershipField.TabIndex = 2;
            this.lblOwnershipField.Text = "土地权属性质字段:";
            this.lblOwnershipField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboOwnershipField
            // 
            this.cboOwnershipField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOwnershipField.FormattingEnabled = true;
            this.cboOwnershipField.Location = new System.Drawing.Point(225, 76);
            this.cboOwnershipField.Margin = new System.Windows.Forms.Padding(4);
            this.cboOwnershipField.Name = "cboOwnershipField";
            this.cboOwnershipField.Size = new System.Drawing.Size(298, 26);
            this.cboOwnershipField.TabIndex = 3;
            // 
            // lblForestValue
            // 
            this.lblForestValue.Location = new System.Drawing.Point(555, 35);
            this.lblForestValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForestValue.Name = "lblForestValue";
            this.lblForestValue.Size = new System.Drawing.Size(135, 28);
            this.lblForestValue.TabIndex = 4;
            this.lblForestValue.Text = "林地地类值:";
            this.lblForestValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtForestValue
            // 
            this.txtForestValue.Location = new System.Drawing.Point(698, 35);
            this.txtForestValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtForestValue.Name = "txtForestValue";
            this.txtForestValue.Size = new System.Drawing.Size(118, 28);
            this.txtForestValue.TabIndex = 5;
            this.txtForestValue.Text = "03";
            // 
            // lblStateOwnershipValue
            // 
            this.lblStateOwnershipValue.Location = new System.Drawing.Point(555, 76);
            this.lblStateOwnershipValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStateOwnershipValue.Name = "lblStateOwnershipValue";
            this.lblStateOwnershipValue.Size = new System.Drawing.Size(135, 28);
            this.lblStateOwnershipValue.TabIndex = 6;
            this.lblStateOwnershipValue.Text = "国有值:";
            this.lblStateOwnershipValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStateOwnershipValue
            // 
            this.txtStateOwnershipValue.Location = new System.Drawing.Point(698, 76);
            this.txtStateOwnershipValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtStateOwnershipValue.Name = "txtStateOwnershipValue";
            this.txtStateOwnershipValue.Size = new System.Drawing.Size(118, 28);
            this.txtStateOwnershipValue.TabIndex = 7;
            this.txtStateOwnershipValue.Text = "1";
            // 
            // lblCollectiveValue
            // 
            this.lblCollectiveValue.Location = new System.Drawing.Point(555, 118);
            this.lblCollectiveValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCollectiveValue.Name = "lblCollectiveValue";
            this.lblCollectiveValue.Size = new System.Drawing.Size(135, 28);
            this.lblCollectiveValue.TabIndex = 8;
            this.lblCollectiveValue.Text = "集体值:";
            this.lblCollectiveValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCollectiveValue
            // 
            this.txtCollectiveValue.Location = new System.Drawing.Point(698, 118);
            this.txtCollectiveValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtCollectiveValue.Name = "txtCollectiveValue";
            this.txtCollectiveValue.Size = new System.Drawing.Size(118, 28);
            this.txtCollectiveValue.TabIndex = 9;
            this.txtCollectiveValue.Text = "2";
            // 
            // btnExtractScope
            // 
            this.btnExtractScope.Location = new System.Drawing.Point(225, 118);
            this.btnExtractScope.Margin = new System.Windows.Forms.Padding(4);
            this.btnExtractScope.Name = "btnExtractScope";
            this.btnExtractScope.Size = new System.Drawing.Size(270, 35);
            this.btnExtractScope.TabIndex = 10;
            this.btnExtractScope.Text = "执行工作范围提取";
            this.btnExtractScope.UseVisualStyleBackColor = true;
            // 
            // resultsGroupBox
            // 
            this.resultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGroupBox.Controls.Add(this.lblExtractionResults);
            this.resultsGroupBox.Location = new System.Drawing.Point(15, 374);
            this.resultsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.resultsGroupBox.Name = "resultsGroupBox";
            this.resultsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.resultsGroupBox.Size = new System.Drawing.Size(1005, 223);
            this.resultsGroupBox.TabIndex = 2;
            this.resultsGroupBox.TabStop = false;
            this.resultsGroupBox.Text = "结果";
            // 
            // lblExtractionResults
            // 
            this.lblExtractionResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExtractionResults.Location = new System.Drawing.Point(22, 35);
            this.lblExtractionResults.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExtractionResults.Name = "lblExtractionResults";
            this.lblExtractionResults.Size = new System.Drawing.Size(961, 194);
            this.lblExtractionResults.TabIndex = 0;
            this.lblExtractionResults.Text = "尚未执行工作范围提取";
            this.lblExtractionResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // createBaseMapTab
            // 
            this.createBaseMapTab.Controls.Add(this.landGradeGroupBox);
            this.createBaseMapTab.Controls.Add(this.landPriceGroupBox);
            this.createBaseMapTab.Controls.Add(this.linkResultsGroupBox);
            this.createBaseMapTab.Location = new System.Drawing.Point(4, 28);
            this.createBaseMapTab.Margin = new System.Windows.Forms.Padding(4);
            this.createBaseMapTab.Name = "createBaseMapTab";
            this.createBaseMapTab.Padding = new System.Windows.Forms.Padding(4);
            this.createBaseMapTab.Size = new System.Drawing.Size(1043, 578);
            this.createBaseMapTab.TabIndex = 1;
            this.createBaseMapTab.Text = "2. 制作工作底图";
            this.createBaseMapTab.UseVisualStyleBackColor = true;
            // 
            // landGradeGroupBox
            // 
            this.landGradeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.landGradeGroupBox.Controls.Add(this.lblLandGradeData);
            this.landGradeGroupBox.Controls.Add(this.txtLandGradeData);
            this.landGradeGroupBox.Controls.Add(this.btnBrowseLandGradeData);
            this.landGradeGroupBox.Controls.Add(this.btnLinkLandGradeData);
            this.landGradeGroupBox.Location = new System.Drawing.Point(15, 14);
            this.landGradeGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.landGradeGroupBox.Name = "landGradeGroupBox";
            this.landGradeGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.landGradeGroupBox.Size = new System.Drawing.Size(1006, 125);
            this.landGradeGroupBox.TabIndex = 0;
            this.landGradeGroupBox.TabStop = false;
            this.landGradeGroupBox.Text = "林地分等数据";
            // 
            // lblLandGradeData
            // 
            this.lblLandGradeData.Location = new System.Drawing.Point(22, 35);
            this.lblLandGradeData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLandGradeData.Name = "lblLandGradeData";
            this.lblLandGradeData.Size = new System.Drawing.Size(180, 28);
            this.lblLandGradeData.TabIndex = 0;
            this.lblLandGradeData.Text = "林地分等数据:";
            this.lblLandGradeData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLandGradeData
            // 
            this.txtLandGradeData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLandGradeData.Location = new System.Drawing.Point(210, 35);
            this.txtLandGradeData.Margin = new System.Windows.Forms.Padding(4);
            this.txtLandGradeData.Name = "txtLandGradeData";
            this.txtLandGradeData.ReadOnly = true;
            this.txtLandGradeData.Size = new System.Drawing.Size(592, 28);
            this.txtLandGradeData.TabIndex = 1;
            // 
            // btnBrowseLandGradeData
            // 
            this.btnBrowseLandGradeData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseLandGradeData.Location = new System.Drawing.Point(810, 33);
            this.btnBrowseLandGradeData.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseLandGradeData.Name = "btnBrowseLandGradeData";
            this.btnBrowseLandGradeData.Size = new System.Drawing.Size(80, 32);
            this.btnBrowseLandGradeData.TabIndex = 2;
            this.btnBrowseLandGradeData.Text = "浏览...";
            this.btnBrowseLandGradeData.UseVisualStyleBackColor = true;
            // 
            // btnLinkLandGradeData
            // 
            this.btnLinkLandGradeData.Location = new System.Drawing.Point(404, 82);
            this.btnLinkLandGradeData.Margin = new System.Windows.Forms.Padding(4);
            this.btnLinkLandGradeData.Name = "btnLinkLandGradeData";
            this.btnLinkLandGradeData.Size = new System.Drawing.Size(270, 35);
            this.btnLinkLandGradeData.TabIndex = 3;
            this.btnLinkLandGradeData.Text = "关联林地分等数据";
            this.btnLinkLandGradeData.UseVisualStyleBackColor = true;
            // 
            // landPriceGroupBox
            // 
            this.landPriceGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.landPriceGroupBox.Controls.Add(this.lblLandPriceData);
            this.landPriceGroupBox.Controls.Add(this.txtLandPriceData);
            this.landPriceGroupBox.Controls.Add(this.btnBrowseLandPriceData);
            this.landPriceGroupBox.Controls.Add(this.btnLinkLandPriceData);
            this.landPriceGroupBox.Location = new System.Drawing.Point(15, 152);
            this.landPriceGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.landPriceGroupBox.Name = "landPriceGroupBox";
            this.landPriceGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.landPriceGroupBox.Size = new System.Drawing.Size(1006, 125);
            this.landPriceGroupBox.TabIndex = 1;
            this.landPriceGroupBox.TabStop = false;
            this.landPriceGroupBox.Text = "林地定级与基准地价数据";
            // 
            // lblLandPriceData
            // 
            this.lblLandPriceData.Location = new System.Drawing.Point(22, 35);
            this.lblLandPriceData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLandPriceData.Name = "lblLandPriceData";
            this.lblLandPriceData.Size = new System.Drawing.Size(180, 28);
            this.lblLandPriceData.TabIndex = 0;
            this.lblLandPriceData.Text = "林地定级数据:";
            this.lblLandPriceData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLandPriceData
            // 
            this.txtLandPriceData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLandPriceData.Location = new System.Drawing.Point(210, 35);
            this.txtLandPriceData.Margin = new System.Windows.Forms.Padding(4);
            this.txtLandPriceData.Name = "txtLandPriceData";
            this.txtLandPriceData.ReadOnly = true;
            this.txtLandPriceData.Size = new System.Drawing.Size(592, 28);
            this.txtLandPriceData.TabIndex = 1;
            // 
            // btnBrowseLandPriceData
            // 
            this.btnBrowseLandPriceData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseLandPriceData.Location = new System.Drawing.Point(810, 33);
            this.btnBrowseLandPriceData.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseLandPriceData.Name = "btnBrowseLandPriceData";
            this.btnBrowseLandPriceData.Size = new System.Drawing.Size(80, 32);
            this.btnBrowseLandPriceData.TabIndex = 2;
            this.btnBrowseLandPriceData.Text = "浏览...";
            this.btnBrowseLandPriceData.UseVisualStyleBackColor = true;
            this.btnBrowseLandPriceData.Click += new System.EventHandler(this.btnBrowseLandPriceData_Click);
            // 
            // btnLinkLandPriceData
            // 
            this.btnLinkLandPriceData.Location = new System.Drawing.Point(404, 82);
            this.btnLinkLandPriceData.Margin = new System.Windows.Forms.Padding(4);
            this.btnLinkLandPriceData.Name = "btnLinkLandPriceData";
            this.btnLinkLandPriceData.Size = new System.Drawing.Size(270, 35);
            this.btnLinkLandPriceData.TabIndex = 3;
            this.btnLinkLandPriceData.Text = "挂接基准地价";
            this.btnLinkLandPriceData.UseVisualStyleBackColor = true;
            // 
            // linkResultsGroupBox
            // 
            this.linkResultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkResultsGroupBox.Controls.Add(this.lblGradeMatchRate);
            this.linkResultsGroupBox.Controls.Add(this.lblGradeMatchRateValue);
            this.linkResultsGroupBox.Controls.Add(this.lblPriceMatchRate);
            this.linkResultsGroupBox.Controls.Add(this.lblPriceMatchRateValue);
            this.linkResultsGroupBox.Controls.Add(this.btnSaveBaseMap);
            this.linkResultsGroupBox.Controls.Add(this.btnViewBaseMap);
            this.linkResultsGroupBox.Location = new System.Drawing.Point(15, 290);
            this.linkResultsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.linkResultsGroupBox.Name = "linkResultsGroupBox";
            this.linkResultsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.linkResultsGroupBox.Size = new System.Drawing.Size(1006, 280);
            this.linkResultsGroupBox.TabIndex = 2;
            this.linkResultsGroupBox.TabStop = false;
            this.linkResultsGroupBox.Text = "关联结果";
            // 
            // lblGradeMatchRate
            // 
            this.lblGradeMatchRate.Location = new System.Drawing.Point(22, 35);
            this.lblGradeMatchRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGradeMatchRate.Name = "lblGradeMatchRate";
            this.lblGradeMatchRate.Size = new System.Drawing.Size(180, 28);
            this.lblGradeMatchRate.TabIndex = 0;
            this.lblGradeMatchRate.Text = "分等关联匹配率:";
            this.lblGradeMatchRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGradeMatchRateValue
            // 
            this.lblGradeMatchRateValue.Location = new System.Drawing.Point(210, 35);
            this.lblGradeMatchRateValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGradeMatchRateValue.Name = "lblGradeMatchRateValue";
            this.lblGradeMatchRateValue.Size = new System.Drawing.Size(150, 28);
            this.lblGradeMatchRateValue.TabIndex = 1;
            this.lblGradeMatchRateValue.Text = "尚未执行";
            this.lblGradeMatchRateValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPriceMatchRate
            // 
            this.lblPriceMatchRate.Location = new System.Drawing.Point(22, 76);
            this.lblPriceMatchRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPriceMatchRate.Name = "lblPriceMatchRate";
            this.lblPriceMatchRate.Size = new System.Drawing.Size(180, 28);
            this.lblPriceMatchRate.TabIndex = 2;
            this.lblPriceMatchRate.Text = "基准价格匹配率:";
            this.lblPriceMatchRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPriceMatchRateValue
            // 
            this.lblPriceMatchRateValue.Location = new System.Drawing.Point(210, 76);
            this.lblPriceMatchRateValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPriceMatchRateValue.Name = "lblPriceMatchRateValue";
            this.lblPriceMatchRateValue.Size = new System.Drawing.Size(150, 28);
            this.lblPriceMatchRateValue.TabIndex = 3;
            this.lblPriceMatchRateValue.Text = "尚未执行";
            this.lblPriceMatchRateValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSaveBaseMap
            // 
            this.btnSaveBaseMap.Enabled = false;
            this.btnSaveBaseMap.Location = new System.Drawing.Point(22, 118);
            this.btnSaveBaseMap.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveBaseMap.Name = "btnSaveBaseMap";
            this.btnSaveBaseMap.Size = new System.Drawing.Size(270, 35);
            this.btnSaveBaseMap.TabIndex = 4;
            this.btnSaveBaseMap.Text = "保存工作底图";
            this.btnSaveBaseMap.UseVisualStyleBackColor = true;
            // 
            // btnViewBaseMap
            // 
            this.btnViewBaseMap.Enabled = false;
            this.btnViewBaseMap.Location = new System.Drawing.Point(315, 118);
            this.btnViewBaseMap.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewBaseMap.Name = "btnViewBaseMap";
            this.btnViewBaseMap.Size = new System.Drawing.Size(270, 35);
            this.btnViewBaseMap.TabIndex = 5;
            this.btnViewBaseMap.Text = "查看工作底图";
            this.btnViewBaseMap.UseVisualStyleBackColor = true;
            // 
            // priceParamsTab
            // 
            this.priceParamsTab.Controls.Add(this.priceParamsGroupBox);
            this.priceParamsTab.Controls.Add(this.priceFactorsGroupBox);
            this.priceParamsTab.Location = new System.Drawing.Point(4, 28);
            this.priceParamsTab.Margin = new System.Windows.Forms.Padding(4);
            this.priceParamsTab.Name = "priceParamsTab";
            this.priceParamsTab.Padding = new System.Windows.Forms.Padding(4);
            this.priceParamsTab.Size = new System.Drawing.Size(1043, 578);
            this.priceParamsTab.TabIndex = 2;
            this.priceParamsTab.Text = "3. 价格参数提取";
            this.priceParamsTab.UseVisualStyleBackColor = true;
            // 
            // priceParamsGroupBox
            // 
            this.priceParamsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.priceParamsGroupBox.Controls.Add(this.lblGradeIndices);
            this.priceParamsGroupBox.Controls.Add(this.txtGradeIndices);
            this.priceParamsGroupBox.Controls.Add(this.btnBrowseGradeIndices);
            this.priceParamsGroupBox.Controls.Add(this.lblBasePriceData);
            this.priceParamsGroupBox.Controls.Add(this.txtBasePriceData);
            this.priceParamsGroupBox.Controls.Add(this.btnBrowseBasePriceData);
            this.priceParamsGroupBox.Controls.Add(this.btnExtractPriceParams);
            this.priceParamsGroupBox.Location = new System.Drawing.Point(15, 14);
            this.priceParamsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.priceParamsGroupBox.Name = "priceParamsGroupBox";
            this.priceParamsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.priceParamsGroupBox.Size = new System.Drawing.Size(1286, 166);
            this.priceParamsGroupBox.TabIndex = 0;
            this.priceParamsGroupBox.TabStop = false;
            this.priceParamsGroupBox.Text = "林地定级价格参数";
            // 
            // lblGradeIndices
            // 
            this.lblGradeIndices.Location = new System.Drawing.Point(22, 35);
            this.lblGradeIndices.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGradeIndices.Name = "lblGradeIndices";
            this.lblGradeIndices.Size = new System.Drawing.Size(180, 28);
            this.lblGradeIndices.TabIndex = 0;
            this.lblGradeIndices.Text = "定级指标数据:";
            this.lblGradeIndices.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGradeIndices
            // 
            this.txtGradeIndices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGradeIndices.Location = new System.Drawing.Point(210, 35);
            this.txtGradeIndices.Margin = new System.Windows.Forms.Padding(4);
            this.txtGradeIndices.Name = "txtGradeIndices";
            this.txtGradeIndices.ReadOnly = true;
            this.txtGradeIndices.Size = new System.Drawing.Size(864, 28);
            this.txtGradeIndices.TabIndex = 1;
            // 
            // btnBrowseGradeIndices
            // 
            this.btnBrowseGradeIndices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseGradeIndices.Location = new System.Drawing.Point(1082, 29);
            this.btnBrowseGradeIndices.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseGradeIndices.Name = "btnBrowseGradeIndices";
            this.btnBrowseGradeIndices.Size = new System.Drawing.Size(88, 32);
            this.btnBrowseGradeIndices.TabIndex = 2;
            this.btnBrowseGradeIndices.Text = "浏览...";
            this.btnBrowseGradeIndices.UseVisualStyleBackColor = true;
            // 
            // lblBasePriceData
            // 
            this.lblBasePriceData.Location = new System.Drawing.Point(22, 76);
            this.lblBasePriceData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBasePriceData.Name = "lblBasePriceData";
            this.lblBasePriceData.Size = new System.Drawing.Size(180, 28);
            this.lblBasePriceData.TabIndex = 3;
            this.lblBasePriceData.Text = "基准价格数据:";
            this.lblBasePriceData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBasePriceData
            // 
            this.txtBasePriceData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBasePriceData.Location = new System.Drawing.Point(210, 76);
            this.txtBasePriceData.Margin = new System.Windows.Forms.Padding(4);
            this.txtBasePriceData.Name = "txtBasePriceData";
            this.txtBasePriceData.ReadOnly = true;
            this.txtBasePriceData.Size = new System.Drawing.Size(864, 28);
            this.txtBasePriceData.TabIndex = 4;
            // 
            // btnBrowseBasePriceData
            // 
            this.btnBrowseBasePriceData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseBasePriceData.Location = new System.Drawing.Point(1082, 76);
            this.btnBrowseBasePriceData.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseBasePriceData.Name = "btnBrowseBasePriceData";
            this.btnBrowseBasePriceData.Size = new System.Drawing.Size(88, 32);
            this.btnBrowseBasePriceData.TabIndex = 5;
            this.btnBrowseBasePriceData.Text = "浏览...";
            this.btnBrowseBasePriceData.UseVisualStyleBackColor = true;
            // 
            // btnExtractPriceParams
            // 
            this.btnExtractPriceParams.Location = new System.Drawing.Point(396, 123);
            this.btnExtractPriceParams.Margin = new System.Windows.Forms.Padding(4);
            this.btnExtractPriceParams.Name = "btnExtractPriceParams";
            this.btnExtractPriceParams.Size = new System.Drawing.Size(270, 35);
            this.btnExtractPriceParams.TabIndex = 6;
            this.btnExtractPriceParams.Text = "提取价格参数";
            this.btnExtractPriceParams.UseVisualStyleBackColor = true;
            // 
            // priceFactorsGroupBox
            // 
            this.priceFactorsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.priceFactorsGroupBox.Controls.Add(this.lblModifiers);
            this.priceFactorsGroupBox.Controls.Add(this.txtModifiers);
            this.priceFactorsGroupBox.Controls.Add(this.btnBrowseModifiers);
            this.priceFactorsGroupBox.Controls.Add(this.lblYieldRate);
            this.priceFactorsGroupBox.Controls.Add(this.numYieldRate);
            this.priceFactorsGroupBox.Controls.Add(this.btnLoadPriceParams);
            this.priceFactorsGroupBox.Controls.Add(this.dgvPriceFactors);
            this.priceFactorsGroupBox.Location = new System.Drawing.Point(15, 194);
            this.priceFactorsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.priceFactorsGroupBox.Name = "priceFactorsGroupBox";
            this.priceFactorsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.priceFactorsGroupBox.Size = new System.Drawing.Size(1286, 685);
            this.priceFactorsGroupBox.TabIndex = 1;
            this.priceFactorsGroupBox.TabStop = false;
            this.priceFactorsGroupBox.Text = "价格修正因子及收益还原率";
            // 
            // lblModifiers
            // 
            this.lblModifiers.Location = new System.Drawing.Point(22, 35);
            this.lblModifiers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModifiers.Name = "lblModifiers";
            this.lblModifiers.Size = new System.Drawing.Size(180, 28);
            this.lblModifiers.TabIndex = 0;
            this.lblModifiers.Text = "价格修正因子:";
            this.lblModifiers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtModifiers
            // 
            this.txtModifiers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModifiers.Location = new System.Drawing.Point(210, 35);
            this.txtModifiers.Margin = new System.Windows.Forms.Padding(4);
            this.txtModifiers.Name = "txtModifiers";
            this.txtModifiers.ReadOnly = true;
            this.txtModifiers.Size = new System.Drawing.Size(864, 28);
            this.txtModifiers.TabIndex = 1;
            // 
            // btnBrowseModifiers
            // 
            this.btnBrowseModifiers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseModifiers.Location = new System.Drawing.Point(1082, 33);
            this.btnBrowseModifiers.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseModifiers.Name = "btnBrowseModifiers";
            this.btnBrowseModifiers.Size = new System.Drawing.Size(88, 32);
            this.btnBrowseModifiers.TabIndex = 2;
            this.btnBrowseModifiers.Text = "浏览...";
            this.btnBrowseModifiers.UseVisualStyleBackColor = true;
            // 
            // lblYieldRate
            // 
            this.lblYieldRate.Location = new System.Drawing.Point(22, 76);
            this.lblYieldRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYieldRate.Name = "lblYieldRate";
            this.lblYieldRate.Size = new System.Drawing.Size(180, 28);
            this.lblYieldRate.TabIndex = 3;
            this.lblYieldRate.Text = "收益还原率(%):";
            this.lblYieldRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numYieldRate
            // 
            this.numYieldRate.DecimalPlaces = 2;
            this.numYieldRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numYieldRate.Location = new System.Drawing.Point(210, 76);
            this.numYieldRate.Margin = new System.Windows.Forms.Padding(4);
            this.numYieldRate.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numYieldRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numYieldRate.Name = "numYieldRate";
            this.numYieldRate.Size = new System.Drawing.Size(120, 28);
            this.numYieldRate.TabIndex = 4;
            this.numYieldRate.Value = new decimal(new int[] {
            386,
            0,
            0,
            131072});
            // 
            // btnLoadPriceParams
            // 
            this.btnLoadPriceParams.Location = new System.Drawing.Point(210, 118);
            this.btnLoadPriceParams.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadPriceParams.Name = "btnLoadPriceParams";
            this.btnLoadPriceParams.Size = new System.Drawing.Size(270, 35);
            this.btnLoadPriceParams.TabIndex = 5;
            this.btnLoadPriceParams.Text = "加载参数";
            this.btnLoadPriceParams.UseVisualStyleBackColor = true;
            // 
            // dgvPriceFactors
            // 
            this.dgvPriceFactors.AllowUserToAddRows = false;
            this.dgvPriceFactors.AllowUserToDeleteRows = false;
            this.dgvPriceFactors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPriceFactors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPriceFactors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPriceFactors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPriceFactors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FactorName,
            this.Weight});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPriceFactors.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPriceFactors.Location = new System.Drawing.Point(22, 159);
            this.dgvPriceFactors.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPriceFactors.Name = "dgvPriceFactors";
            this.dgvPriceFactors.ReadOnly = true;
            this.dgvPriceFactors.RowHeadersWidth = 62;
            this.dgvPriceFactors.Size = new System.Drawing.Size(1148, 440);
            this.dgvPriceFactors.TabIndex = 6;
            // 
            // FactorName
            // 
            this.FactorName.HeaderText = "修正因子名称";
            this.FactorName.MinimumWidth = 8;
            this.FactorName.Name = "FactorName";
            this.FactorName.ReadOnly = true;
            // 
            // Weight
            // 
            this.Weight.HeaderText = "权重";
            this.Weight.MinimumWidth = 8;
            this.Weight.Name = "Weight";
            this.Weight.ReadOnly = true;
            // 
            // supplementPriceTab
            // 
            this.supplementPriceTab.Controls.Add(this.supplementSettingsGroupBox);
            this.supplementPriceTab.Controls.Add(this.supplementResultsGroupBox);
            this.supplementPriceTab.Location = new System.Drawing.Point(4, 28);
            this.supplementPriceTab.Margin = new System.Windows.Forms.Padding(4);
            this.supplementPriceTab.Name = "supplementPriceTab";
            this.supplementPriceTab.Padding = new System.Windows.Forms.Padding(4);
            this.supplementPriceTab.Size = new System.Drawing.Size(1043, 578);
            this.supplementPriceTab.TabIndex = 3;
            this.supplementPriceTab.Text = "4. 补充基准地价";
            this.supplementPriceTab.UseVisualStyleBackColor = true;
            // 
            // supplementSettingsGroupBox
            // 
            this.supplementSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.supplementSettingsGroupBox.Controls.Add(this.lblSupplementMethod);
            this.supplementSettingsGroupBox.Controls.Add(this.cboSupplementMethod);
            this.supplementSettingsGroupBox.Controls.Add(this.lblDefaultBasePrice);
            this.supplementSettingsGroupBox.Controls.Add(this.numDefaultBasePrice);
            this.supplementSettingsGroupBox.Controls.Add(this.btnSupplementPrice);
            this.supplementSettingsGroupBox.Location = new System.Drawing.Point(15, 14);
            this.supplementSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.supplementSettingsGroupBox.Name = "supplementSettingsGroupBox";
            this.supplementSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.supplementSettingsGroupBox.Size = new System.Drawing.Size(1286, 125);
            this.supplementSettingsGroupBox.TabIndex = 0;
            this.supplementSettingsGroupBox.TabStop = false;
            this.supplementSettingsGroupBox.Text = "补充设置";
            // 
            // lblSupplementMethod
            // 
            this.lblSupplementMethod.Location = new System.Drawing.Point(22, 35);
            this.lblSupplementMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSupplementMethod.Name = "lblSupplementMethod";
            this.lblSupplementMethod.Size = new System.Drawing.Size(180, 28);
            this.lblSupplementMethod.TabIndex = 0;
            this.lblSupplementMethod.Text = "补充方法:";
            this.lblSupplementMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboSupplementMethod
            // 
            this.cboSupplementMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSupplementMethod.FormattingEnabled = true;
            this.cboSupplementMethod.Items.AddRange(new object[] {
            "加权平均法",
            "邻近插值法",
            "反距离加权插值",
            "克里金插值法"});
            this.cboSupplementMethod.Location = new System.Drawing.Point(210, 35);
            this.cboSupplementMethod.Margin = new System.Windows.Forms.Padding(4);
            this.cboSupplementMethod.Name = "cboSupplementMethod";
            this.cboSupplementMethod.Size = new System.Drawing.Size(298, 26);
            this.cboSupplementMethod.TabIndex = 1;
            this.cboSupplementMethod.SelectedIndexChanged += new System.EventHandler(this.cboSupplementMethod_SelectedIndexChanged);
            // 
            // lblDefaultBasePrice
            // 
            this.lblDefaultBasePrice.Location = new System.Drawing.Point(525, 35);
            this.lblDefaultBasePrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefaultBasePrice.Name = "lblDefaultBasePrice";
            this.lblDefaultBasePrice.Size = new System.Drawing.Size(180, 28);
            this.lblDefaultBasePrice.TabIndex = 2;
            this.lblDefaultBasePrice.Text = "默认基准价格:";
            this.lblDefaultBasePrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numDefaultBasePrice
            // 
            this.numDefaultBasePrice.DecimalPlaces = 2;
            this.numDefaultBasePrice.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numDefaultBasePrice.Location = new System.Drawing.Point(720, 35);
            this.numDefaultBasePrice.Margin = new System.Windows.Forms.Padding(4);
            this.numDefaultBasePrice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numDefaultBasePrice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numDefaultBasePrice.Name = "numDefaultBasePrice";
            this.numDefaultBasePrice.Size = new System.Drawing.Size(150, 28);
            this.numDefaultBasePrice.TabIndex = 3;
            this.numDefaultBasePrice.ThousandsSeparator = true;
            this.numDefaultBasePrice.Value = new decimal(new int[] {
            500,
            0,
            0,
            131072});
            // 
            // btnSupplementPrice
            // 
            this.btnSupplementPrice.Location = new System.Drawing.Point(210, 76);
            this.btnSupplementPrice.Margin = new System.Windows.Forms.Padding(4);
            this.btnSupplementPrice.Name = "btnSupplementPrice";
            this.btnSupplementPrice.Size = new System.Drawing.Size(270, 35);
            this.btnSupplementPrice.TabIndex = 4;
            this.btnSupplementPrice.Text = "补充基准地价";
            this.btnSupplementPrice.UseVisualStyleBackColor = true;
            // 
            // supplementResultsGroupBox
            // 
            this.supplementResultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.supplementResultsGroupBox.Controls.Add(this.lblBeforeCount);
            this.supplementResultsGroupBox.Controls.Add(this.lblBeforeCountValue);
            this.supplementResultsGroupBox.Controls.Add(this.lblAfterCount);
            this.supplementResultsGroupBox.Controls.Add(this.lblAfterCountValue);
            this.supplementResultsGroupBox.Controls.Add(this.btnSaveSupplementResults);
            this.supplementResultsGroupBox.Controls.Add(this.btnViewPriceDistribution);
            this.supplementResultsGroupBox.Controls.Add(this.priceDistributionPanel);
            this.supplementResultsGroupBox.Location = new System.Drawing.Point(15, 152);
            this.supplementResultsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.supplementResultsGroupBox.Name = "supplementResultsGroupBox";
            this.supplementResultsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.supplementResultsGroupBox.Size = new System.Drawing.Size(1286, 726);
            this.supplementResultsGroupBox.TabIndex = 1;
            this.supplementResultsGroupBox.TabStop = false;
            this.supplementResultsGroupBox.Text = "补充结果";
            // 
            // lblBeforeCount
            // 
            this.lblBeforeCount.Location = new System.Drawing.Point(22, 35);
            this.lblBeforeCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBeforeCount.Name = "lblBeforeCount";
            this.lblBeforeCount.Size = new System.Drawing.Size(180, 28);
            this.lblBeforeCount.TabIndex = 0;
            this.lblBeforeCount.Text = "补充前缺失数量:";
            this.lblBeforeCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBeforeCountValue
            // 
            this.lblBeforeCountValue.Location = new System.Drawing.Point(210, 35);
            this.lblBeforeCountValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBeforeCountValue.Name = "lblBeforeCountValue";
            this.lblBeforeCountValue.Size = new System.Drawing.Size(150, 28);
            this.lblBeforeCountValue.TabIndex = 1;
            this.lblBeforeCountValue.Text = "尚未执行";
            this.lblBeforeCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAfterCount
            // 
            this.lblAfterCount.Location = new System.Drawing.Point(22, 76);
            this.lblAfterCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAfterCount.Name = "lblAfterCount";
            this.lblAfterCount.Size = new System.Drawing.Size(180, 28);
            this.lblAfterCount.TabIndex = 2;
            this.lblAfterCount.Text = "补充后缺失数量:";
            this.lblAfterCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAfterCountValue
            // 
            this.lblAfterCountValue.Location = new System.Drawing.Point(210, 76);
            this.lblAfterCountValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAfterCountValue.Name = "lblAfterCountValue";
            this.lblAfterCountValue.Size = new System.Drawing.Size(150, 28);
            this.lblAfterCountValue.TabIndex = 3;
            this.lblAfterCountValue.Text = "尚未执行";
            this.lblAfterCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSaveSupplementResults
            // 
            this.btnSaveSupplementResults.Enabled = false;
            this.btnSaveSupplementResults.Location = new System.Drawing.Point(22, 118);
            this.btnSaveSupplementResults.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveSupplementResults.Name = "btnSaveSupplementResults";
            this.btnSaveSupplementResults.Size = new System.Drawing.Size(270, 35);
            this.btnSaveSupplementResults.TabIndex = 4;
            this.btnSaveSupplementResults.Text = "保存补充结果";
            this.btnSaveSupplementResults.UseVisualStyleBackColor = true;
            // 
            // btnViewPriceDistribution
            // 
            this.btnViewPriceDistribution.Enabled = false;
            this.btnViewPriceDistribution.Location = new System.Drawing.Point(315, 118);
            this.btnViewPriceDistribution.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewPriceDistribution.Name = "btnViewPriceDistribution";
            this.btnViewPriceDistribution.Size = new System.Drawing.Size(270, 35);
            this.btnViewPriceDistribution.TabIndex = 5;
            this.btnViewPriceDistribution.Text = "查看价格分布";
            this.btnViewPriceDistribution.UseVisualStyleBackColor = true;
            // 
            // priceDistributionPanel
            // 
            this.priceDistributionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.priceDistributionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.priceDistributionPanel.Location = new System.Drawing.Point(22, 166);
            this.priceDistributionPanel.Margin = new System.Windows.Forms.Padding(4);
            this.priceDistributionPanel.Name = "priceDistributionPanel";
            this.priceDistributionPanel.Size = new System.Drawing.Size(998, 252);
            this.priceDistributionPanel.TabIndex = 6;
            // 
            // calculateValueTab
            // 
            this.calculateValueTab.Controls.Add(this.calculationSettingsGroupBox);
            this.calculateValueTab.Controls.Add(this.resultSummaryGroupBox);
            this.calculateValueTab.Location = new System.Drawing.Point(4, 28);
            this.calculateValueTab.Margin = new System.Windows.Forms.Padding(4);
            this.calculateValueTab.Name = "calculateValueTab";
            this.calculateValueTab.Padding = new System.Windows.Forms.Padding(4);
            this.calculateValueTab.Size = new System.Drawing.Size(1043, 578);
            this.calculateValueTab.TabIndex = 4;
            this.calculateValueTab.Text = "5. 资源资产价值计算";
            this.calculateValueTab.UseVisualStyleBackColor = true;
            // 
            // calculationSettingsGroupBox
            // 
            this.calculationSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calculationSettingsGroupBox.Controls.Add(this.lblDateModifier);
            this.calculationSettingsGroupBox.Controls.Add(this.numDateModifier);
            this.calculationSettingsGroupBox.Controls.Add(this.lblPeriodModifier);
            this.calculationSettingsGroupBox.Controls.Add(this.numPeriodModifier);
            this.calculationSettingsGroupBox.Controls.Add(this.lblCalcMethod);
            this.calculationSettingsGroupBox.Controls.Add(this.cboCalcMethod);
            this.calculationSettingsGroupBox.Controls.Add(this.chkExportResults);
            this.calculationSettingsGroupBox.Controls.Add(this.btnCalculateValue);
            this.calculationSettingsGroupBox.Location = new System.Drawing.Point(15, 14);
            this.calculationSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.calculationSettingsGroupBox.Name = "calculationSettingsGroupBox";
            this.calculationSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.calculationSettingsGroupBox.Size = new System.Drawing.Size(1297, 166);
            this.calculationSettingsGroupBox.TabIndex = 0;
            this.calculationSettingsGroupBox.TabStop = false;
            this.calculationSettingsGroupBox.Text = "计算设置";
            // 
            // lblDateModifier
            // 
            this.lblDateModifier.Location = new System.Drawing.Point(22, 35);
            this.lblDateModifier.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDateModifier.Name = "lblDateModifier";
            this.lblDateModifier.Size = new System.Drawing.Size(180, 28);
            this.lblDateModifier.TabIndex = 0;
            this.lblDateModifier.Text = "期日修正系数:";
            this.lblDateModifier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numDateModifier
            // 
            this.numDateModifier.DecimalPlaces = 2;
            this.numDateModifier.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numDateModifier.Location = new System.Drawing.Point(210, 35);
            this.numDateModifier.Margin = new System.Windows.Forms.Padding(4);
            this.numDateModifier.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numDateModifier.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            131072});
            this.numDateModifier.Name = "numDateModifier";
            this.numDateModifier.Size = new System.Drawing.Size(120, 28);
            this.numDateModifier.TabIndex = 1;
            this.numDateModifier.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblPeriodModifier
            // 
            this.lblPeriodModifier.Location = new System.Drawing.Point(390, 35);
            this.lblPeriodModifier.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPeriodModifier.Name = "lblPeriodModifier";
            this.lblPeriodModifier.Size = new System.Drawing.Size(180, 28);
            this.lblPeriodModifier.TabIndex = 2;
            this.lblPeriodModifier.Text = "年期修正系数:";
            this.lblPeriodModifier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPeriodModifier
            // 
            this.numPeriodModifier.DecimalPlaces = 2;
            this.numPeriodModifier.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numPeriodModifier.Location = new System.Drawing.Point(578, 35);
            this.numPeriodModifier.Margin = new System.Windows.Forms.Padding(4);
            this.numPeriodModifier.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numPeriodModifier.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            131072});
            this.numPeriodModifier.Name = "numPeriodModifier";
            this.numPeriodModifier.Size = new System.Drawing.Size(120, 28);
            this.numPeriodModifier.TabIndex = 3;
            this.numPeriodModifier.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCalcMethod
            // 
            this.lblCalcMethod.Location = new System.Drawing.Point(22, 76);
            this.lblCalcMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCalcMethod.Name = "lblCalcMethod";
            this.lblCalcMethod.Size = new System.Drawing.Size(180, 28);
            this.lblCalcMethod.TabIndex = 4;
            this.lblCalcMethod.Text = "计算方法:";
            this.lblCalcMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboCalcMethod
            // 
            this.cboCalcMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCalcMethod.FormattingEnabled = true;
            this.cboCalcMethod.Items.AddRange(new object[] {
            "基准价格法",
            "收益还原法",
            "市场比较法"});
            this.cboCalcMethod.Location = new System.Drawing.Point(210, 76);
            this.cboCalcMethod.Margin = new System.Windows.Forms.Padding(4);
            this.cboCalcMethod.Name = "cboCalcMethod";
            this.cboCalcMethod.Size = new System.Drawing.Size(298, 26);
            this.cboCalcMethod.TabIndex = 5;
            // 
            // chkExportResults
            // 
            this.chkExportResults.AutoSize = true;
            this.chkExportResults.Checked = true;
            this.chkExportResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExportResults.Location = new System.Drawing.Point(578, 79);
            this.chkExportResults.Margin = new System.Windows.Forms.Padding(4);
            this.chkExportResults.Name = "chkExportResults";
            this.chkExportResults.Size = new System.Drawing.Size(142, 22);
            this.chkExportResults.TabIndex = 6;
            this.chkExportResults.Text = "导出计算结果";
            this.chkExportResults.UseVisualStyleBackColor = true;
            // 
            // btnCalculateValue
            // 
            this.btnCalculateValue.Location = new System.Drawing.Point(210, 118);
            this.btnCalculateValue.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalculateValue.Name = "btnCalculateValue";
            this.btnCalculateValue.Size = new System.Drawing.Size(270, 35);
            this.btnCalculateValue.TabIndex = 7;
            this.btnCalculateValue.Text = "计算资产价值";
            this.btnCalculateValue.UseVisualStyleBackColor = true;
            // 
            // resultSummaryGroupBox
            // 
            this.resultSummaryGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultSummaryGroupBox.Controls.Add(this.lblTotalValue);
            this.resultSummaryGroupBox.Controls.Add(this.lblTotalValueResult);
            this.resultSummaryGroupBox.Controls.Add(this.lblAverageUnitPrice);
            this.resultSummaryGroupBox.Controls.Add(this.lblAverageUnitPriceResult);
            this.resultSummaryGroupBox.Controls.Add(this.lblTotalArea);
            this.resultSummaryGroupBox.Controls.Add(this.lblTotalAreaResult);
            this.resultSummaryGroupBox.Controls.Add(this.lblParcelCount);
            this.resultSummaryGroupBox.Controls.Add(this.lblParcelCountResult);
            this.resultSummaryGroupBox.Controls.Add(this.btnSaveCalculationResults);
            this.resultSummaryGroupBox.Controls.Add(this.btnViewValueStats);
            this.resultSummaryGroupBox.Controls.Add(this.valueDistributionPanel);
            this.resultSummaryGroupBox.Location = new System.Drawing.Point(15, 194);
            this.resultSummaryGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.resultSummaryGroupBox.Name = "resultSummaryGroupBox";
            this.resultSummaryGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.resultSummaryGroupBox.Size = new System.Drawing.Size(1297, 685);
            this.resultSummaryGroupBox.TabIndex = 1;
            this.resultSummaryGroupBox.TabStop = false;
            this.resultSummaryGroupBox.Text = "计算结果摘要";
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.Location = new System.Drawing.Point(22, 35);
            this.lblTotalValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(180, 28);
            this.lblTotalValue.TabIndex = 0;
            this.lblTotalValue.Text = "总价值(万元):";
            this.lblTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalValueResult
            // 
            this.lblTotalValueResult.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalValueResult.Location = new System.Drawing.Point(210, 35);
            this.lblTotalValueResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalValueResult.Name = "lblTotalValueResult";
            this.lblTotalValueResult.Size = new System.Drawing.Size(225, 28);
            this.lblTotalValueResult.TabIndex = 1;
            this.lblTotalValueResult.Text = "尚未计算";
            this.lblTotalValueResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAverageUnitPrice
            // 
            this.lblAverageUnitPrice.Location = new System.Drawing.Point(450, 35);
            this.lblAverageUnitPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAverageUnitPrice.Name = "lblAverageUnitPrice";
            this.lblAverageUnitPrice.Size = new System.Drawing.Size(210, 28);
            this.lblAverageUnitPrice.TabIndex = 2;
            this.lblAverageUnitPrice.Text = "平均单价(万元/公顷):";
            this.lblAverageUnitPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAverageUnitPriceResult
            // 
            this.lblAverageUnitPriceResult.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.lblAverageUnitPriceResult.Location = new System.Drawing.Point(668, 35);
            this.lblAverageUnitPriceResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAverageUnitPriceResult.Name = "lblAverageUnitPriceResult";
            this.lblAverageUnitPriceResult.Size = new System.Drawing.Size(180, 28);
            this.lblAverageUnitPriceResult.TabIndex = 3;
            this.lblAverageUnitPriceResult.Text = "尚未计算";
            this.lblAverageUnitPriceResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalArea
            // 
            this.lblTotalArea.Location = new System.Drawing.Point(22, 76);
            this.lblTotalArea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalArea.Name = "lblTotalArea";
            this.lblTotalArea.Size = new System.Drawing.Size(180, 28);
            this.lblTotalArea.TabIndex = 4;
            this.lblTotalArea.Text = "总面积(公顷):";
            this.lblTotalArea.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalAreaResult
            // 
            this.lblTotalAreaResult.Location = new System.Drawing.Point(210, 76);
            this.lblTotalAreaResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalAreaResult.Name = "lblTotalAreaResult";
            this.lblTotalAreaResult.Size = new System.Drawing.Size(225, 28);
            this.lblTotalAreaResult.TabIndex = 5;
            this.lblTotalAreaResult.Text = "尚未计算";
            this.lblTotalAreaResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblParcelCount
            // 
            this.lblParcelCount.Location = new System.Drawing.Point(450, 76);
            this.lblParcelCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParcelCount.Name = "lblParcelCount";
            this.lblParcelCount.Size = new System.Drawing.Size(210, 28);
            this.lblParcelCount.TabIndex = 6;
            this.lblParcelCount.Text = "图斑数量:";
            this.lblParcelCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblParcelCountResult
            // 
            this.lblParcelCountResult.Location = new System.Drawing.Point(668, 76);
            this.lblParcelCountResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParcelCountResult.Name = "lblParcelCountResult";
            this.lblParcelCountResult.Size = new System.Drawing.Size(180, 28);
            this.lblParcelCountResult.TabIndex = 7;
            this.lblParcelCountResult.Text = "尚未计算";
            this.lblParcelCountResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSaveCalculationResults
            // 
            this.btnSaveCalculationResults.Enabled = false;
            this.btnSaveCalculationResults.Location = new System.Drawing.Point(156, 123);
            this.btnSaveCalculationResults.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveCalculationResults.Name = "btnSaveCalculationResults";
            this.btnSaveCalculationResults.Size = new System.Drawing.Size(270, 35);
            this.btnSaveCalculationResults.TabIndex = 8;
            this.btnSaveCalculationResults.Text = "保存计算结果";
            this.btnSaveCalculationResults.UseVisualStyleBackColor = true;
            // 
            // btnViewValueStats
            // 
            this.btnViewValueStats.Enabled = false;
            this.btnViewValueStats.Location = new System.Drawing.Point(499, 123);
            this.btnViewValueStats.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewValueStats.Name = "btnViewValueStats";
            this.btnViewValueStats.Size = new System.Drawing.Size(270, 35);
            this.btnViewValueStats.TabIndex = 9;
            this.btnViewValueStats.Text = "查看价值统计";
            this.btnViewValueStats.UseVisualStyleBackColor = true;
            // 
            // valueDistributionPanel
            // 
            this.valueDistributionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueDistributionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.valueDistributionPanel.Location = new System.Drawing.Point(8, 166);
            this.valueDistributionPanel.Margin = new System.Windows.Forms.Padding(4);
            this.valueDistributionPanel.Name = "valueDistributionPanel";
            this.valueDistributionPanel.Size = new System.Drawing.Size(978, 210);
            this.valueDistributionPanel.TabIndex = 10;
            // 
            // cleanQATab
            // 
            this.cleanQATab.Controls.Add(this.cleaningSettingsGroupBox);
            this.cleanQATab.Controls.Add(this.qaResultsGroupBox);
            this.cleanQATab.Location = new System.Drawing.Point(4, 28);
            this.cleanQATab.Margin = new System.Windows.Forms.Padding(4);
            this.cleanQATab.Name = "cleanQATab";
            this.cleanQATab.Padding = new System.Windows.Forms.Padding(4);
            this.cleanQATab.Size = new System.Drawing.Size(1043, 578);
            this.cleanQATab.TabIndex = 5;
            this.cleanQATab.Text = "6. 数据清洗质检";
            this.cleanQATab.UseVisualStyleBackColor = true;
            // 
            // cleaningSettingsGroupBox
            // 
            this.cleaningSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cleaningSettingsGroupBox.Controls.Add(this.lblFieldMapping);
            this.cleaningSettingsGroupBox.Controls.Add(this.txtFieldMapping);
            this.cleaningSettingsGroupBox.Controls.Add(this.btnBrowseFieldMapping);
            this.cleaningSettingsGroupBox.Controls.Add(this.chkRemoveTempFields);
            this.cleaningSettingsGroupBox.Controls.Add(this.chkFixGeometryIssues);
            this.cleaningSettingsGroupBox.Controls.Add(this.chkValidateDomainValues);
            this.cleaningSettingsGroupBox.Controls.Add(this.btnCleanQA);
            this.cleaningSettingsGroupBox.Location = new System.Drawing.Point(15, 14);
            this.cleaningSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.cleaningSettingsGroupBox.Name = "cleaningSettingsGroupBox";
            this.cleaningSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.cleaningSettingsGroupBox.Size = new System.Drawing.Size(1286, 166);
            this.cleaningSettingsGroupBox.TabIndex = 0;
            this.cleaningSettingsGroupBox.TabStop = false;
            this.cleaningSettingsGroupBox.Text = "清洗设置";
            // 
            // lblFieldMapping
            // 
            this.lblFieldMapping.Location = new System.Drawing.Point(22, 35);
            this.lblFieldMapping.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFieldMapping.Name = "lblFieldMapping";
            this.lblFieldMapping.Size = new System.Drawing.Size(180, 28);
            this.lblFieldMapping.TabIndex = 0;
            this.lblFieldMapping.Text = "字段映射表:";
            this.lblFieldMapping.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFieldMapping
            // 
            this.txtFieldMapping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFieldMapping.Location = new System.Drawing.Point(210, 35);
            this.txtFieldMapping.Margin = new System.Windows.Forms.Padding(4);
            this.txtFieldMapping.Name = "txtFieldMapping";
            this.txtFieldMapping.ReadOnly = true;
            this.txtFieldMapping.Size = new System.Drawing.Size(856, 28);
            this.txtFieldMapping.TabIndex = 1;
            // 
            // btnBrowseFieldMapping
            // 
            this.btnBrowseFieldMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseFieldMapping.Location = new System.Drawing.Point(1074, 33);
            this.btnBrowseFieldMapping.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseFieldMapping.Name = "btnBrowseFieldMapping";
            this.btnBrowseFieldMapping.Size = new System.Drawing.Size(96, 32);
            this.btnBrowseFieldMapping.TabIndex = 2;
            this.btnBrowseFieldMapping.Text = "浏览...";
            this.btnBrowseFieldMapping.UseVisualStyleBackColor = true;
            // 
            // chkRemoveTempFields
            // 
            this.chkRemoveTempFields.AutoSize = true;
            this.chkRemoveTempFields.Checked = true;
            this.chkRemoveTempFields.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemoveTempFields.Location = new System.Drawing.Point(210, 79);
            this.chkRemoveTempFields.Margin = new System.Windows.Forms.Padding(4);
            this.chkRemoveTempFields.Name = "chkRemoveTempFields";
            this.chkRemoveTempFields.Size = new System.Drawing.Size(142, 22);
            this.chkRemoveTempFields.TabIndex = 3;
            this.chkRemoveTempFields.Text = "删除临时字段";
            this.chkRemoveTempFields.UseVisualStyleBackColor = true;
            // 
            // chkFixGeometryIssues
            // 
            this.chkFixGeometryIssues.AutoSize = true;
            this.chkFixGeometryIssues.Checked = true;
            this.chkFixGeometryIssues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFixGeometryIssues.Location = new System.Drawing.Point(405, 79);
            this.chkFixGeometryIssues.Margin = new System.Windows.Forms.Padding(4);
            this.chkFixGeometryIssues.Name = "chkFixGeometryIssues";
            this.chkFixGeometryIssues.Size = new System.Drawing.Size(142, 22);
            this.chkFixGeometryIssues.TabIndex = 4;
            this.chkFixGeometryIssues.Text = "修复几何问题";
            this.chkFixGeometryIssues.UseVisualStyleBackColor = true;
            // 
            // chkValidateDomainValues
            // 
            this.chkValidateDomainValues.AutoSize = true;
            this.chkValidateDomainValues.Checked = true;
            this.chkValidateDomainValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkValidateDomainValues.Location = new System.Drawing.Point(600, 79);
            this.chkValidateDomainValues.Margin = new System.Windows.Forms.Padding(4);
            this.chkValidateDomainValues.Name = "chkValidateDomainValues";
            this.chkValidateDomainValues.Size = new System.Drawing.Size(142, 22);
            this.chkValidateDomainValues.TabIndex = 5;
            this.chkValidateDomainValues.Text = "验证值域范围";
            this.chkValidateDomainValues.UseVisualStyleBackColor = true;
            // 
            // btnCleanQA
            // 
            this.btnCleanQA.Location = new System.Drawing.Point(405, 123);
            this.btnCleanQA.Margin = new System.Windows.Forms.Padding(4);
            this.btnCleanQA.Name = "btnCleanQA";
            this.btnCleanQA.Size = new System.Drawing.Size(270, 35);
            this.btnCleanQA.TabIndex = 6;
            this.btnCleanQA.Text = "执行数据清洗与质检";
            this.btnCleanQA.UseVisualStyleBackColor = true;
            // 
            // qaResultsGroupBox
            // 
            this.qaResultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.qaResultsGroupBox.Controls.Add(this.lblIssuesFound);
            this.qaResultsGroupBox.Controls.Add(this.lblIssuesFoundValue);
            this.qaResultsGroupBox.Controls.Add(this.lblIssuesFixed);
            this.qaResultsGroupBox.Controls.Add(this.lblIssuesFixedValue);
            this.qaResultsGroupBox.Controls.Add(this.lblQAPassRate);
            this.qaResultsGroupBox.Controls.Add(this.lblQAPassRateValue);
            this.qaResultsGroupBox.Controls.Add(this.btnViewQAReport);
            this.qaResultsGroupBox.Controls.Add(this.btnSaveCleanData);
            this.qaResultsGroupBox.Controls.Add(this.dgvQAIssues);
            this.qaResultsGroupBox.Location = new System.Drawing.Point(15, 194);
            this.qaResultsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.qaResultsGroupBox.Name = "qaResultsGroupBox";
            this.qaResultsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.qaResultsGroupBox.Size = new System.Drawing.Size(1286, 685);
            this.qaResultsGroupBox.TabIndex = 1;
            this.qaResultsGroupBox.TabStop = false;
            this.qaResultsGroupBox.Text = "质检结果";
            // 
            // lblIssuesFound
            // 
            this.lblIssuesFound.Location = new System.Drawing.Point(22, 35);
            this.lblIssuesFound.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIssuesFound.Name = "lblIssuesFound";
            this.lblIssuesFound.Size = new System.Drawing.Size(180, 28);
            this.lblIssuesFound.TabIndex = 0;
            this.lblIssuesFound.Text = "发现问题数量:";
            this.lblIssuesFound.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblIssuesFoundValue
            // 
            this.lblIssuesFoundValue.Location = new System.Drawing.Point(210, 35);
            this.lblIssuesFoundValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIssuesFoundValue.Name = "lblIssuesFoundValue";
            this.lblIssuesFoundValue.Size = new System.Drawing.Size(150, 28);
            this.lblIssuesFoundValue.TabIndex = 1;
            this.lblIssuesFoundValue.Text = "尚未执行";
            this.lblIssuesFoundValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblIssuesFixed
            // 
            this.lblIssuesFixed.Location = new System.Drawing.Point(22, 76);
            this.lblIssuesFixed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIssuesFixed.Name = "lblIssuesFixed";
            this.lblIssuesFixed.Size = new System.Drawing.Size(180, 28);
            this.lblIssuesFixed.TabIndex = 2;
            this.lblIssuesFixed.Text = "已修复问题数量:";
            this.lblIssuesFixed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblIssuesFixedValue
            // 
            this.lblIssuesFixedValue.Location = new System.Drawing.Point(210, 76);
            this.lblIssuesFixedValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIssuesFixedValue.Name = "lblIssuesFixedValue";
            this.lblIssuesFixedValue.Size = new System.Drawing.Size(150, 28);
            this.lblIssuesFixedValue.TabIndex = 3;
            this.lblIssuesFixedValue.Text = "尚未执行";
            this.lblIssuesFixedValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblQAPassRate
            // 
            this.lblQAPassRate.Location = new System.Drawing.Point(420, 35);
            this.lblQAPassRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQAPassRate.Name = "lblQAPassRate";
            this.lblQAPassRate.Size = new System.Drawing.Size(180, 28);
            this.lblQAPassRate.TabIndex = 4;
            this.lblQAPassRate.Text = "质检通过率:";
            this.lblQAPassRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQAPassRateValue
            // 
            this.lblQAPassRateValue.Location = new System.Drawing.Point(608, 35);
            this.lblQAPassRateValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQAPassRateValue.Name = "lblQAPassRateValue";
            this.lblQAPassRateValue.Size = new System.Drawing.Size(150, 28);
            this.lblQAPassRateValue.TabIndex = 5;
            this.lblQAPassRateValue.Text = "尚未执行";
            this.lblQAPassRateValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnViewQAReport
            // 
            this.btnViewQAReport.Enabled = false;
            this.btnViewQAReport.Location = new System.Drawing.Point(169, 123);
            this.btnViewQAReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewQAReport.Name = "btnViewQAReport";
            this.btnViewQAReport.Size = new System.Drawing.Size(270, 35);
            this.btnViewQAReport.TabIndex = 6;
            this.btnViewQAReport.Text = "查看质检报告";
            this.btnViewQAReport.UseVisualStyleBackColor = true;
            // 
            // btnSaveCleanData
            // 
            this.btnSaveCleanData.Enabled = false;
            this.btnSaveCleanData.Location = new System.Drawing.Point(534, 118);
            this.btnSaveCleanData.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveCleanData.Name = "btnSaveCleanData";
            this.btnSaveCleanData.Size = new System.Drawing.Size(270, 35);
            this.btnSaveCleanData.TabIndex = 7;
            this.btnSaveCleanData.Text = "保存清洗后数据";
            this.btnSaveCleanData.UseVisualStyleBackColor = true;
            // 
            // dgvQAIssues
            // 
            this.dgvQAIssues.AllowUserToAddRows = false;
            this.dgvQAIssues.AllowUserToDeleteRows = false;
            this.dgvQAIssues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvQAIssues.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvQAIssues.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvQAIssues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQAIssues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IssueType,
            this.Count,
            this.Fixed,
            this.Description});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvQAIssues.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvQAIssues.Location = new System.Drawing.Point(22, 166);
            this.dgvQAIssues.Margin = new System.Windows.Forms.Padding(4);
            this.dgvQAIssues.Name = "dgvQAIssues";
            this.dgvQAIssues.ReadOnly = true;
            this.dgvQAIssues.RowHeadersWidth = 62;
            this.dgvQAIssues.Size = new System.Drawing.Size(1148, 437);
            this.dgvQAIssues.TabIndex = 8;
            // 
            // IssueType
            // 
            this.IssueType.HeaderText = "问题类型";
            this.IssueType.MinimumWidth = 8;
            this.IssueType.Name = "IssueType";
            this.IssueType.ReadOnly = true;
            // 
            // Count
            // 
            this.Count.HeaderText = "数量";
            this.Count.MinimumWidth = 8;
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            // 
            // Fixed
            // 
            this.Fixed.HeaderText = "已修复";
            this.Fixed.MinimumWidth = 8;
            this.Fixed.Name = "Fixed";
            this.Fixed.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.HeaderText = "描述";
            this.Description.MinimumWidth = 8;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // buildDatabaseTab
            // 
            this.buildDatabaseTab.Controls.Add(this.dbSettingsGroupBox);
            this.buildDatabaseTab.Controls.Add(this.outputTablesGroupBox);
            this.buildDatabaseTab.Location = new System.Drawing.Point(4, 28);
            this.buildDatabaseTab.Margin = new System.Windows.Forms.Padding(4);
            this.buildDatabaseTab.Name = "buildDatabaseTab";
            this.buildDatabaseTab.Padding = new System.Windows.Forms.Padding(4);
            this.buildDatabaseTab.Size = new System.Drawing.Size(1043, 578);
            this.buildDatabaseTab.TabIndex = 6;
            this.buildDatabaseTab.Text = "7. 构建数据库";
            this.buildDatabaseTab.UseVisualStyleBackColor = true;
            // 
            // dbSettingsGroupBox
            // 
            this.dbSettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dbSettingsGroupBox.Controls.Add(this.lblOutputLocation);
            this.dbSettingsGroupBox.Controls.Add(this.txtOutputLocation);
            this.dbSettingsGroupBox.Controls.Add(this.btnBrowseOutputLocation);
            this.dbSettingsGroupBox.Controls.Add(this.lblOutputName);
            this.dbSettingsGroupBox.Controls.Add(this.txtOutputName);
            this.dbSettingsGroupBox.Controls.Add(this.lblOutputFormat);
            this.dbSettingsGroupBox.Controls.Add(this.cboOutputFormat);
            this.dbSettingsGroupBox.Controls.Add(this.btnBuildDatabase);
            this.dbSettingsGroupBox.Location = new System.Drawing.Point(15, 14);
            this.dbSettingsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.dbSettingsGroupBox.Name = "dbSettingsGroupBox";
            this.dbSettingsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.dbSettingsGroupBox.Size = new System.Drawing.Size(1286, 166);
            this.dbSettingsGroupBox.TabIndex = 0;
            this.dbSettingsGroupBox.TabStop = false;
            this.dbSettingsGroupBox.Text = "数据库设置";
            // 
            // lblOutputLocation
            // 
            this.lblOutputLocation.Location = new System.Drawing.Point(22, 35);
            this.lblOutputLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputLocation.Name = "lblOutputLocation";
            this.lblOutputLocation.Size = new System.Drawing.Size(180, 28);
            this.lblOutputLocation.TabIndex = 0;
            this.lblOutputLocation.Text = "输出位置:";
            this.lblOutputLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOutputLocation
            // 
            this.txtOutputLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputLocation.Location = new System.Drawing.Point(210, 35);
            this.txtOutputLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputLocation.Name = "txtOutputLocation";
            this.txtOutputLocation.ReadOnly = true;
            this.txtOutputLocation.Size = new System.Drawing.Size(846, 28);
            this.txtOutputLocation.TabIndex = 1;
            // 
            // btnBrowseOutputLocation
            // 
            this.btnBrowseOutputLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutputLocation.Location = new System.Drawing.Point(1079, 33);
            this.btnBrowseOutputLocation.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseOutputLocation.Name = "btnBrowseOutputLocation";
            this.btnBrowseOutputLocation.Size = new System.Drawing.Size(81, 30);
            this.btnBrowseOutputLocation.TabIndex = 2;
            this.btnBrowseOutputLocation.Text = "浏览...";
            this.btnBrowseOutputLocation.UseVisualStyleBackColor = true;
            // 
            // lblOutputName
            // 
            this.lblOutputName.Location = new System.Drawing.Point(22, 76);
            this.lblOutputName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputName.Name = "lblOutputName";
            this.lblOutputName.Size = new System.Drawing.Size(180, 28);
            this.lblOutputName.TabIndex = 3;
            this.lblOutputName.Text = "输出名称:";
            this.lblOutputName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOutputName
            // 
            this.txtOutputName.Location = new System.Drawing.Point(210, 76);
            this.txtOutputName.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputName.Name = "txtOutputName";
            this.txtOutputName.Size = new System.Drawing.Size(298, 28);
            this.txtOutputName.TabIndex = 4;
            this.txtOutputName.Text = "ForestAssetInventory";
            // 
            // lblOutputFormat
            // 
            this.lblOutputFormat.Location = new System.Drawing.Point(525, 76);
            this.lblOutputFormat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputFormat.Name = "lblOutputFormat";
            this.lblOutputFormat.Size = new System.Drawing.Size(135, 28);
            this.lblOutputFormat.TabIndex = 5;
            this.lblOutputFormat.Text = "输出格式:";
            this.lblOutputFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboOutputFormat
            // 
            this.cboOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOutputFormat.FormattingEnabled = true;
            this.cboOutputFormat.Items.AddRange(new object[] {
            "File Geodatabase",
            "个人地理数据库",
            "Excel工作簿",
            "Shapefile"});
            this.cboOutputFormat.Location = new System.Drawing.Point(668, 76);
            this.cboOutputFormat.Margin = new System.Windows.Forms.Padding(4);
            this.cboOutputFormat.Name = "cboOutputFormat";
            this.cboOutputFormat.Size = new System.Drawing.Size(238, 26);
            this.cboOutputFormat.TabIndex = 6;
            // 
            // btnBuildDatabase
            // 
            this.btnBuildDatabase.Location = new System.Drawing.Point(210, 118);
            this.btnBuildDatabase.Margin = new System.Windows.Forms.Padding(4);
            this.btnBuildDatabase.Name = "btnBuildDatabase";
            this.btnBuildDatabase.Size = new System.Drawing.Size(270, 35);
            this.btnBuildDatabase.TabIndex = 7;
            this.btnBuildDatabase.Text = "构建数据库";
            this.btnBuildDatabase.UseVisualStyleBackColor = true;
            // 
            // outputTablesGroupBox
            // 
            this.outputTablesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTablesGroupBox.Controls.Add(this.clbOutputTables);
            this.outputTablesGroupBox.Controls.Add(this.lblOutputStatus);
            this.outputTablesGroupBox.Controls.Add(this.lblOutputStatusValue);
            this.outputTablesGroupBox.Controls.Add(this.btnViewOutputFiles);
            this.outputTablesGroupBox.Controls.Add(this.btnGenerateReport);
            this.outputTablesGroupBox.Location = new System.Drawing.Point(15, 194);
            this.outputTablesGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.outputTablesGroupBox.Name = "outputTablesGroupBox";
            this.outputTablesGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.outputTablesGroupBox.Size = new System.Drawing.Size(1286, 692);
            this.outputTablesGroupBox.TabIndex = 1;
            this.outputTablesGroupBox.TabStop = false;
            this.outputTablesGroupBox.Text = "输出数据表";
            // 
            // clbOutputTables
            // 
            this.clbOutputTables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbOutputTables.CheckOnClick = true;
            this.clbOutputTables.FormattingEnabled = true;
            this.clbOutputTables.Items.AddRange(new object[] {
            "森林资源资产空间数据集",
            "森林资源资产基础数表",
            "森林资源资产统计表",
            "林地分等数据表",
            "林地基准地价表"});
            this.clbOutputTables.Location = new System.Drawing.Point(22, 35);
            this.clbOutputTables.Margin = new System.Windows.Forms.Padding(4);
            this.clbOutputTables.Name = "clbOutputTables";
            this.clbOutputTables.Size = new System.Drawing.Size(1239, 104);
            this.clbOutputTables.TabIndex = 0;
            // 
            // lblOutputStatus
            // 
            this.lblOutputStatus.Location = new System.Drawing.Point(22, 166);
            this.lblOutputStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputStatus.Name = "lblOutputStatus";
            this.lblOutputStatus.Size = new System.Drawing.Size(180, 28);
            this.lblOutputStatus.TabIndex = 1;
            this.lblOutputStatus.Text = "输出状态:";
            this.lblOutputStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutputStatusValue
            // 
            this.lblOutputStatusValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOutputStatusValue.Location = new System.Drawing.Point(210, 166);
            this.lblOutputStatusValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputStatusValue.Name = "lblOutputStatusValue";
            this.lblOutputStatusValue.Size = new System.Drawing.Size(1053, 28);
            this.lblOutputStatusValue.TabIndex = 2;
            this.lblOutputStatusValue.Text = "尚未执行";
            this.lblOutputStatusValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnViewOutputFiles
            // 
            this.btnViewOutputFiles.Enabled = false;
            this.btnViewOutputFiles.Location = new System.Drawing.Point(22, 208);
            this.btnViewOutputFiles.Margin = new System.Windows.Forms.Padding(4);
            this.btnViewOutputFiles.Name = "btnViewOutputFiles";
            this.btnViewOutputFiles.Size = new System.Drawing.Size(270, 35);
            this.btnViewOutputFiles.TabIndex = 3;
            this.btnViewOutputFiles.Text = "查看输出文件";
            this.btnViewOutputFiles.UseVisualStyleBackColor = true;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Enabled = false;
            this.btnGenerateReport.Location = new System.Drawing.Point(315, 208);
            this.btnGenerateReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(270, 35);
            this.btnGenerateReport.TabIndex = 4;
            this.btnGenerateReport.Text = "生成清查报告";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            // 
            // AssetValueCalculationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 915);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AssetValueCalculationForm";
            this.Text = "森林资源资产清查计算工具";
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.workspaceGroupBox.ResumeLayout(false);
            this.workspaceGroupBox.PerformLayout();
            this.workflowTabs.ResumeLayout(false);
            this.extractScopeTab.ResumeLayout(false);
            this.dataSourceGroupBox.ResumeLayout(false);
            this.dataSourceGroupBox.PerformLayout();
            this.extractionSettingsGroupBox.ResumeLayout(false);
            this.extractionSettingsGroupBox.PerformLayout();
            this.resultsGroupBox.ResumeLayout(false);
            this.createBaseMapTab.ResumeLayout(false);
            this.landGradeGroupBox.ResumeLayout(false);
            this.landGradeGroupBox.PerformLayout();
            this.landPriceGroupBox.ResumeLayout(false);
            this.landPriceGroupBox.PerformLayout();
            this.linkResultsGroupBox.ResumeLayout(false);
            this.priceParamsTab.ResumeLayout(false);
            this.priceParamsGroupBox.ResumeLayout(false);
            this.priceParamsGroupBox.PerformLayout();
            this.priceFactorsGroupBox.ResumeLayout(false);
            this.priceFactorsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYieldRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceFactors)).EndInit();
            this.supplementPriceTab.ResumeLayout(false);
            this.supplementSettingsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultBasePrice)).EndInit();
            this.supplementResultsGroupBox.ResumeLayout(false);
            this.calculateValueTab.ResumeLayout(false);
            this.calculationSettingsGroupBox.ResumeLayout(false);
            this.calculationSettingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDateModifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPeriodModifier)).EndInit();
            this.resultSummaryGroupBox.ResumeLayout(false);
            this.cleanQATab.ResumeLayout(false);
            this.cleaningSettingsGroupBox.ResumeLayout(false);
            this.cleaningSettingsGroupBox.PerformLayout();
            this.qaResultsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQAIssues)).EndInit();
            this.buildDatabaseTab.ResumeLayout(false);
            this.dbSettingsGroupBox.ResumeLayout(false);
            this.dbSettingsGroupBox.PerformLayout();
            this.outputTablesGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox workspaceGroupBox;
        private System.Windows.Forms.Label lblWorkspace;
        private System.Windows.Forms.TextBox txtWorkspace;
        private System.Windows.Forms.Button btnSelectWorkspace;
        private System.Windows.Forms.TabControl workflowTabs;
        private System.Windows.Forms.TabPage extractScopeTab;
        private System.Windows.Forms.GroupBox dataSourceGroupBox;
        private System.Windows.Forms.Label lblForestData;
        private System.Windows.Forms.TextBox txtForestData;
        private System.Windows.Forms.Button btnBrowseForestData;
        private System.Windows.Forms.Label lblUrbanBoundary;
        private System.Windows.Forms.TextBox txtUrbanBoundary;
        private System.Windows.Forms.Button btnBrowseUrbanBoundary;
        private System.Windows.Forms.Button btnLoadCurrentMap;
        private System.Windows.Forms.GroupBox extractionSettingsGroupBox;
        private System.Windows.Forms.Label lblLandTypeField;
        private System.Windows.Forms.ComboBox cboLandTypeField;
        private System.Windows.Forms.Label lblOwnershipField;
        private System.Windows.Forms.ComboBox cboOwnershipField;
        private System.Windows.Forms.Label lblForestValue;
        private System.Windows.Forms.TextBox txtForestValue;
        private System.Windows.Forms.Label lblStateOwnershipValue;
        private System.Windows.Forms.TextBox txtStateOwnershipValue;
        private System.Windows.Forms.Label lblCollectiveValue;
        private System.Windows.Forms.TextBox txtCollectiveValue;
        private System.Windows.Forms.Button btnExtractScope;
        private System.Windows.Forms.GroupBox resultsGroupBox;
        private System.Windows.Forms.Label lblExtractionResults;
        private System.Windows.Forms.TabPage createBaseMapTab;
        private System.Windows.Forms.GroupBox landGradeGroupBox;
        private System.Windows.Forms.Label lblLandGradeData;
        private System.Windows.Forms.TextBox txtLandGradeData;
        private System.Windows.Forms.Button btnBrowseLandGradeData;
        private System.Windows.Forms.Button btnLinkLandGradeData;
        private System.Windows.Forms.GroupBox landPriceGroupBox;
        private System.Windows.Forms.Label lblLandPriceData;
        private System.Windows.Forms.TextBox txtLandPriceData;
        private System.Windows.Forms.Button btnBrowseLandPriceData;
        private System.Windows.Forms.Button btnLinkLandPriceData;
        private System.Windows.Forms.GroupBox linkResultsGroupBox;
        private System.Windows.Forms.Label lblGradeMatchRate;
        private System.Windows.Forms.Label lblGradeMatchRateValue;
        private System.Windows.Forms.Label lblPriceMatchRate;
        private System.Windows.Forms.Label lblPriceMatchRateValue;
        private System.Windows.Forms.Button btnSaveBaseMap;
        private System.Windows.Forms.Button btnViewBaseMap;
        private System.Windows.Forms.TabPage priceParamsTab;
        private System.Windows.Forms.GroupBox priceParamsGroupBox;
        private System.Windows.Forms.Label lblGradeIndices;
        private System.Windows.Forms.TextBox txtGradeIndices;
        private System.Windows.Forms.Button btnBrowseGradeIndices;
        private System.Windows.Forms.Label lblBasePriceData; // Renamed from original for clarity
        private System.Windows.Forms.TextBox txtBasePriceData; // Renamed from original for clarity
        private System.Windows.Forms.Button btnBrowseBasePriceData; // Renamed from original for clarity
        private System.Windows.Forms.Button btnExtractPriceParams;
        private System.Windows.Forms.GroupBox priceFactorsGroupBox;
        private System.Windows.Forms.Label lblModifiers;
        private System.Windows.Forms.TextBox txtModifiers;
        private System.Windows.Forms.Button btnBrowseModifiers;
        private System.Windows.Forms.Label lblYieldRate;
        private System.Windows.Forms.NumericUpDown numYieldRate;
        private System.Windows.Forms.Button btnLoadPriceParams;
        private System.Windows.Forms.DataGridView dgvPriceFactors;
        private System.Windows.Forms.TabPage supplementPriceTab;
        private System.Windows.Forms.GroupBox supplementSettingsGroupBox;
        private System.Windows.Forms.Label lblSupplementMethod;
        private System.Windows.Forms.ComboBox cboSupplementMethod;
        private System.Windows.Forms.Label lblDefaultBasePrice;
        private System.Windows.Forms.NumericUpDown numDefaultBasePrice;
        private System.Windows.Forms.Button btnSupplementPrice;
        private System.Windows.Forms.GroupBox supplementResultsGroupBox;
        private System.Windows.Forms.Label lblBeforeCount;
        private System.Windows.Forms.Label lblBeforeCountValue;
        private System.Windows.Forms.Label lblAfterCount;
        private System.Windows.Forms.Label lblAfterCountValue;
        private System.Windows.Forms.Button btnSaveSupplementResults;
        private System.Windows.Forms.Button btnViewPriceDistribution;
        private System.Windows.Forms.Panel priceDistributionPanel;
        private System.Windows.Forms.TabPage calculateValueTab;
        private System.Windows.Forms.GroupBox calculationSettingsGroupBox;
        private System.Windows.Forms.Label lblDateModifier;
        private System.Windows.Forms.NumericUpDown numDateModifier;
        private System.Windows.Forms.Label lblPeriodModifier;
        private System.Windows.Forms.NumericUpDown numPeriodModifier;
        private System.Windows.Forms.Label lblCalcMethod;
        private System.Windows.Forms.ComboBox cboCalcMethod;
        private System.Windows.Forms.CheckBox chkExportResults;
        private System.Windows.Forms.Button btnCalculateValue;
        private System.Windows.Forms.GroupBox resultSummaryGroupBox;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblTotalValueResult;
        private System.Windows.Forms.Label lblAverageUnitPrice;
        private System.Windows.Forms.Label lblAverageUnitPriceResult;
        private System.Windows.Forms.Label lblTotalArea;
        private System.Windows.Forms.Label lblTotalAreaResult;
        private System.Windows.Forms.Label lblParcelCount;
        private System.Windows.Forms.Label lblParcelCountResult;
        private System.Windows.Forms.Button btnSaveCalculationResults;
        private System.Windows.Forms.Button btnViewValueStats;
        private System.Windows.Forms.Panel valueDistributionPanel;
        private System.Windows.Forms.TabPage cleanQATab;
        private System.Windows.Forms.GroupBox cleaningSettingsGroupBox;
        private System.Windows.Forms.Label lblFieldMapping;
        private System.Windows.Forms.TextBox txtFieldMapping;
        private System.Windows.Forms.Button btnBrowseFieldMapping;
        private System.Windows.Forms.CheckBox chkRemoveTempFields;
        private System.Windows.Forms.CheckBox chkFixGeometryIssues;
        private System.Windows.Forms.CheckBox chkValidateDomainValues;
        private System.Windows.Forms.Button btnCleanQA;
        private System.Windows.Forms.GroupBox qaResultsGroupBox;
        private System.Windows.Forms.Label lblIssuesFound;
        private System.Windows.Forms.Label lblIssuesFoundValue;
        private System.Windows.Forms.Label lblIssuesFixed;
        private System.Windows.Forms.Label lblIssuesFixedValue;
        private System.Windows.Forms.Label lblQAPassRate;
        private System.Windows.Forms.Label lblQAPassRateValue;
        private System.Windows.Forms.Button btnViewQAReport;
        private System.Windows.Forms.Button btnSaveCleanData;
        private System.Windows.Forms.DataGridView dgvQAIssues;
        private System.Windows.Forms.TabPage buildDatabaseTab;
        private System.Windows.Forms.GroupBox dbSettingsGroupBox;
        private System.Windows.Forms.Label lblOutputLocation;
        private System.Windows.Forms.TextBox txtOutputLocation;
        private System.Windows.Forms.Button btnBrowseOutputLocation;
        private System.Windows.Forms.Label lblOutputName;
        private System.Windows.Forms.TextBox txtOutputName;
        private System.Windows.Forms.Label lblOutputFormat;
        private System.Windows.Forms.ComboBox cboOutputFormat;
        private System.Windows.Forms.Button btnBuildDatabase;
        private System.Windows.Forms.GroupBox outputTablesGroupBox;
        private System.Windows.Forms.CheckedListBox clbOutputTables;
        private System.Windows.Forms.Label lblOutputStatus;
        private System.Windows.Forms.Label lblOutputStatusValue;
        private System.Windows.Forms.Button btnViewOutputFiles;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn IssueType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fixed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
    }
}