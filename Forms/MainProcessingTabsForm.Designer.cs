namespace TestArcMapAddin2.Forms
{
    partial class MainProcessingTabsForm
    {
        private System.ComponentModel.IContainer components = null;

        // 主面板和控件
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage2; // 森林资源
        private System.Windows.Forms.TabPage tabPage3; // 草地资源
        private System.Windows.Forms.TabPage tabPage4; // 湿地资源
        private System.Windows.Forms.TabPage tabPage5; // 综合输出
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Label lblProgress;

        // 森林资源标签和按钮
        private System.Windows.Forms.Label lblForestProcessingStatus;
        private System.Windows.Forms.Button btnForestExtractScope;
        private System.Windows.Forms.Button btnForestCreateBasemapLinkPrice;
        private System.Windows.Forms.Button btnForestSupplementPrice;

        // 草地资源标签和按钮
        private System.Windows.Forms.Label lblGrasslandProcessingStatus;
        private System.Windows.Forms.Button btnGrasslandExtractScope;
        private System.Windows.Forms.Button btnGrasslandCreateBasemapLinkPrice;
        private System.Windows.Forms.Button btnGrasslandSupplementPrice;
        private System.Windows.Forms.Button btnGrasslandCalculateValue;
        private System.Windows.Forms.Button btnGrasslandCleanQA;
        private System.Windows.Forms.Button btnGrasslandBuildDBTables;

        // 湿地资源标签和按钮
        private System.Windows.Forms.Label lblWetlandProcessingStatus;
        private System.Windows.Forms.Button btnWetlandExtractScopeBasemap;
        private System.Windows.Forms.Button btnWetlandCleanQA;
        private System.Windows.Forms.Button btnWetlandBuildDBTables;

        // 综合输出标签和按钮
        private System.Windows.Forms.Label lblFinalOutputStatus;
        private System.Windows.Forms.Button btnOverallQualityCheck;
        private System.Windows.Forms.Button btnStatisticalAggregation;
        private System.Windows.Forms.Button btnDataAnalysis;
        private System.Windows.Forms.Button btnExportDatasetDB;
        private System.Windows.Forms.Button btnExportSummaryTables;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Button btnGenerateThematicMaps;
        private System.Windows.Forms.ProgressBar grasslandProgressBar;
        private System.Windows.Forms.ProgressBar wetlandProgressBar;
        private System.Windows.Forms.ProgressBar outputProgressBar;
        private System.Windows.Forms.Label grasslandStepLabel;
        private System.Windows.Forms.Label wetlandStepLabel;
        private System.Windows.Forms.Label outputStepLabel;

        // 森林详细信息面板和控件
        private System.Windows.Forms.Panel forestDetailPanel;
        private System.Windows.Forms.LinkLabel showForestWorkflowDetails;
        private System.Windows.Forms.TextBox forestWorkflowExplanation;
        private System.Windows.Forms.TextBox forestResultsTextBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart forestResourceChart;
        private System.Windows.Forms.PictureBox forestWorkflowImage;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainProcessingTabsForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.showForestWorkflowDetails = new System.Windows.Forms.LinkLabel();
            this.forestDetailPanel = new System.Windows.Forms.Panel();
            this.btnForestExtractScope = new System.Windows.Forms.Button();
            this.btnForestCreateBasemapLinkPrice = new System.Windows.Forms.Button();
            this.btnForestSupplementPrice = new System.Windows.Forms.Button();
            this.lblForestProcessingStatus = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnGrasslandExtractScope = new System.Windows.Forms.Button();
            this.btnGrasslandCreateBasemapLinkPrice = new System.Windows.Forms.Button();
            this.btnGrasslandSupplementPrice = new System.Windows.Forms.Button();
            this.btnGrasslandCalculateValue = new System.Windows.Forms.Button();
            this.btnGrasslandCleanQA = new System.Windows.Forms.Button();
            this.btnGrasslandBuildDBTables = new System.Windows.Forms.Button();
            this.lblGrasslandProcessingStatus = new System.Windows.Forms.Label();
            this.grasslandProgressBar = new System.Windows.Forms.ProgressBar();
            this.grasslandStepLabel = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnWetlandExtractScopeBasemap = new System.Windows.Forms.Button();
            this.btnWetlandCleanQA = new System.Windows.Forms.Button();
            this.btnWetlandBuildDBTables = new System.Windows.Forms.Button();
            this.lblWetlandProcessingStatus = new System.Windows.Forms.Label();
            this.wetlandProgressBar = new System.Windows.Forms.ProgressBar();
            this.wetlandStepLabel = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnOverallQualityCheck = new System.Windows.Forms.Button();
            this.btnStatisticalAggregation = new System.Windows.Forms.Button();
            this.btnDataAnalysis = new System.Windows.Forms.Button();
            this.btnExportDatasetDB = new System.Windows.Forms.Button();
            this.btnExportSummaryTables = new System.Windows.Forms.Button();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnGenerateThematicMaps = new System.Windows.Forms.Button();
            this.lblFinalOutputStatus = new System.Windows.Forms.Label();
            this.outputProgressBar = new System.Windows.Forms.ProgressBar();
            this.outputStepLabel = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.forestWorkflowExplanation = new System.Windows.Forms.TextBox();
            this.forestResultsTextBox = new System.Windows.Forms.TextBox();
            this.forestResourceChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.forestWorkflowImage = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forestResourceChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.forestWorkflowImage)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.titleLabel);
            this.mainPanel.Controls.Add(this.tabControlMain);
            this.mainPanel.Controls.Add(this.bottomPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(6);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(22);
            this.mainPanel.Size = new System.Drawing.Size(1320, 1080);
            this.mainPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point(22, 22);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(1275, 45);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "广东省全民所有自然资源（森林、草地、湿地）资产清查工具 - 主处理模块";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Controls.Add(this.tabPage3);
            this.tabControlMain.Controls.Add(this.tabPage4);
            this.tabControlMain.Controls.Add(this.tabPage5);
            this.tabControlMain.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControlMain.ItemSize = new System.Drawing.Size(100, 22);
            this.tabControlMain.Location = new System.Drawing.Point(22, 82);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1275, 870);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.showForestWorkflowDetails);
            this.tabPage2.Controls.Add(this.forestDetailPanel);
            this.tabPage2.Controls.Add(this.btnForestExtractScope);
            this.tabPage2.Controls.Add(this.btnForestCreateBasemapLinkPrice);
            this.tabPage2.Controls.Add(this.btnForestSupplementPrice);
            this.tabPage2.Controls.Add(this.lblForestProcessingStatus);
            this.tabPage2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage2.ForeColor = System.Drawing.Color.Black;
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(15);
            this.tabPage2.Size = new System.Drawing.Size(1267, 840);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "🌲 森林资源资产清查";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // showForestWorkflowDetails
            // 
            this.showForestWorkflowDetails.AutoSize = true;
            this.showForestWorkflowDetails.Location = new System.Drawing.Point(30, 240);
            this.showForestWorkflowDetails.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.showForestWorkflowDetails.Name = "showForestWorkflowDetails";
            this.showForestWorkflowDetails.Size = new System.Drawing.Size(209, 20);
            this.showForestWorkflowDetails.TabIndex = 9;
            this.showForestWorkflowDetails.TabStop = true;
            this.showForestWorkflowDetails.Text = "显示详细流程说明 ▼";
            this.showForestWorkflowDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ShowForestWorkflowDetails_LinkClicked);
            // 
            // forestDetailPanel
            // 
            this.forestDetailPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.forestDetailPanel.Location = new System.Drawing.Point(30, 270);
            this.forestDetailPanel.Margin = new System.Windows.Forms.Padding(4);
            this.forestDetailPanel.Name = "forestDetailPanel";
            this.forestDetailPanel.Size = new System.Drawing.Size(1214, 524);
            this.forestDetailPanel.TabIndex = 10;
            this.forestDetailPanel.Visible = false;
            // 
            // btnForestExtractScope
            // 
            this.btnForestExtractScope.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnForestExtractScope.Location = new System.Drawing.Point(30, 45);
            this.btnForestExtractScope.Margin = new System.Windows.Forms.Padding(4);
            this.btnForestExtractScope.Name = "btnForestExtractScope";
            this.btnForestExtractScope.Size = new System.Drawing.Size(240, 45);
            this.btnForestExtractScope.TabIndex = 0;
            this.btnForestExtractScope.Text = "1. 提取森林工作范围";
            this.btnForestExtractScope.UseVisualStyleBackColor = true;
            this.btnForestExtractScope.Click += new System.EventHandler(this.BtnForestExtractScope_Click);
            // 
            // btnForestCreateBasemapLinkPrice
            // 
            this.btnForestCreateBasemapLinkPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnForestCreateBasemapLinkPrice.Location = new System.Drawing.Point(330, 45);
            this.btnForestCreateBasemapLinkPrice.Margin = new System.Windows.Forms.Padding(4);
            this.btnForestCreateBasemapLinkPrice.Name = "btnForestCreateBasemapLinkPrice";
            this.btnForestCreateBasemapLinkPrice.Size = new System.Drawing.Size(240, 45);
            this.btnForestCreateBasemapLinkPrice.TabIndex = 1;
            this.btnForestCreateBasemapLinkPrice.Text = "2. 森林底图与价格关联";
            this.btnForestCreateBasemapLinkPrice.UseVisualStyleBackColor = true;
            this.btnForestCreateBasemapLinkPrice.Click += new System.EventHandler(this.BtnForestCreateBasemapLinkPrice_Click);
            // 
            // btnForestSupplementPrice
            // 
            this.btnForestSupplementPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnForestSupplementPrice.Location = new System.Drawing.Point(630, 45);
            this.btnForestSupplementPrice.Margin = new System.Windows.Forms.Padding(4);
            this.btnForestSupplementPrice.Name = "btnForestSupplementPrice";
            this.btnForestSupplementPrice.Size = new System.Drawing.Size(240, 45);
            this.btnForestSupplementPrice.TabIndex = 2;
            this.btnForestSupplementPrice.Text = "3. 补充森林基准价格";
            this.btnForestSupplementPrice.UseVisualStyleBackColor = true;
            this.btnForestSupplementPrice.Click += new System.EventHandler(this.BtnForestSupplementPrice_Click);
            // 
            // lblForestProcessingStatus
            // 
            this.lblForestProcessingStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblForestProcessingStatus.Location = new System.Drawing.Point(30, 180);
            this.lblForestProcessingStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForestProcessingStatus.Name = "lblForestProcessingStatus";
            this.lblForestProcessingStatus.Size = new System.Drawing.Size(1215, 38);
            this.lblForestProcessingStatus.TabIndex = 6;
            this.lblForestProcessingStatus.Text = "等待森林资源清查处理";
            this.lblForestProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnGrasslandExtractScope);
            this.tabPage3.Controls.Add(this.btnGrasslandCreateBasemapLinkPrice);
            this.tabPage3.Controls.Add(this.btnGrasslandSupplementPrice);
            this.tabPage3.Controls.Add(this.btnGrasslandCalculateValue);
            this.tabPage3.Controls.Add(this.btnGrasslandCleanQA);
            this.tabPage3.Controls.Add(this.btnGrasslandBuildDBTables);
            this.tabPage3.Controls.Add(this.lblGrasslandProcessingStatus);
            this.tabPage3.Controls.Add(this.grasslandProgressBar);
            this.tabPage3.Controls.Add(this.grasslandStepLabel);
            this.tabPage3.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage3.ForeColor = System.Drawing.Color.Black;
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(15);
            this.tabPage3.Size = new System.Drawing.Size(1267, 840);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "🌿 草地资源资产清查";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnGrasslandExtractScope
            // 
            this.btnGrasslandExtractScope.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGrasslandExtractScope.Location = new System.Drawing.Point(30, 45);
            this.btnGrasslandExtractScope.Margin = new System.Windows.Forms.Padding(4);
            this.btnGrasslandExtractScope.Name = "btnGrasslandExtractScope";
            this.btnGrasslandExtractScope.Size = new System.Drawing.Size(240, 45);
            this.btnGrasslandExtractScope.TabIndex = 0;
            this.btnGrasslandExtractScope.Text = "1. 提取草地工作范围";
            this.btnGrasslandExtractScope.UseVisualStyleBackColor = true;
            this.btnGrasslandExtractScope.Click += new System.EventHandler(this.BtnGrasslandExtractScope_Click);
            // 
            // btnGrasslandCreateBasemapLinkPrice
            // 
            this.btnGrasslandCreateBasemapLinkPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGrasslandCreateBasemapLinkPrice.Location = new System.Drawing.Point(330, 45);
            this.btnGrasslandCreateBasemapLinkPrice.Margin = new System.Windows.Forms.Padding(4);
            this.btnGrasslandCreateBasemapLinkPrice.Name = "btnGrasslandCreateBasemapLinkPrice";
            this.btnGrasslandCreateBasemapLinkPrice.Size = new System.Drawing.Size(240, 45);
            this.btnGrasslandCreateBasemapLinkPrice.TabIndex = 1;
            this.btnGrasslandCreateBasemapLinkPrice.Text = "2. 草地底图与价格关联";
            this.btnGrasslandCreateBasemapLinkPrice.UseVisualStyleBackColor = true;
            this.btnGrasslandCreateBasemapLinkPrice.Click += new System.EventHandler(this.BtnGrasslandCreateBasemapLinkPrice_Click);
            // 
            // btnGrasslandSupplementPrice
            // 
            this.btnGrasslandSupplementPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGrasslandSupplementPrice.Location = new System.Drawing.Point(630, 45);
            this.btnGrasslandSupplementPrice.Margin = new System.Windows.Forms.Padding(4);
            this.btnGrasslandSupplementPrice.Name = "btnGrasslandSupplementPrice";
            this.btnGrasslandSupplementPrice.Size = new System.Drawing.Size(240, 45);
            this.btnGrasslandSupplementPrice.TabIndex = 2;
            this.btnGrasslandSupplementPrice.Text = "3. 补充草地基准价格";
            this.btnGrasslandSupplementPrice.UseVisualStyleBackColor = true;
            this.btnGrasslandSupplementPrice.Click += new System.EventHandler(this.BtnGrasslandSupplementPrice_Click);
            // 
            // btnGrasslandCalculateValue
            // 
            this.btnGrasslandCalculateValue.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGrasslandCalculateValue.Location = new System.Drawing.Point(30, 112);
            this.btnGrasslandCalculateValue.Margin = new System.Windows.Forms.Padding(4);
            this.btnGrasslandCalculateValue.Name = "btnGrasslandCalculateValue";
            this.btnGrasslandCalculateValue.Size = new System.Drawing.Size(240, 45);
            this.btnGrasslandCalculateValue.TabIndex = 3;
            this.btnGrasslandCalculateValue.Text = "4. 计算草地资产价值";
            this.btnGrasslandCalculateValue.UseVisualStyleBackColor = true;
            this.btnGrasslandCalculateValue.Click += new System.EventHandler(this.BtnGrasslandCalculateValue_Click);
            // 
            // btnGrasslandCleanQA
            // 
            this.btnGrasslandCleanQA.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGrasslandCleanQA.Location = new System.Drawing.Point(330, 112);
            this.btnGrasslandCleanQA.Margin = new System.Windows.Forms.Padding(4);
            this.btnGrasslandCleanQA.Name = "btnGrasslandCleanQA";
            this.btnGrasslandCleanQA.Size = new System.Drawing.Size(240, 45);
            this.btnGrasslandCleanQA.TabIndex = 4;
            this.btnGrasslandCleanQA.Text = "5. 草地数据清洗与质检";
            this.btnGrasslandCleanQA.UseVisualStyleBackColor = true;
            this.btnGrasslandCleanQA.Click += new System.EventHandler(this.BtnGrasslandCleanQA_Click);
            // 
            // btnGrasslandBuildDBTables
            // 
            this.btnGrasslandBuildDBTables.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGrasslandBuildDBTables.Location = new System.Drawing.Point(630, 112);
            this.btnGrasslandBuildDBTables.Margin = new System.Windows.Forms.Padding(4);
            this.btnGrasslandBuildDBTables.Name = "btnGrasslandBuildDBTables";
            this.btnGrasslandBuildDBTables.Size = new System.Drawing.Size(240, 45);
            this.btnGrasslandBuildDBTables.TabIndex = 5;
            this.btnGrasslandBuildDBTables.Text = "6. 草地库表构建";
            this.btnGrasslandBuildDBTables.UseVisualStyleBackColor = true;
            this.btnGrasslandBuildDBTables.Click += new System.EventHandler(this.BtnGrasslandBuildDBTables_Click);
            // 
            // lblGrasslandProcessingStatus
            // 
            this.lblGrasslandProcessingStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGrasslandProcessingStatus.Location = new System.Drawing.Point(30, 180);
            this.lblGrasslandProcessingStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGrasslandProcessingStatus.Name = "lblGrasslandProcessingStatus";
            this.lblGrasslandProcessingStatus.Size = new System.Drawing.Size(1215, 38);
            this.lblGrasslandProcessingStatus.TabIndex = 6;
            this.lblGrasslandProcessingStatus.Text = "等待草地资源清查处理";
            this.lblGrasslandProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grasslandProgressBar
            // 
            this.grasslandProgressBar.Location = new System.Drawing.Point(900, 112);
            this.grasslandProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.grasslandProgressBar.Name = "grasslandProgressBar";
            this.grasslandProgressBar.Size = new System.Drawing.Size(345, 30);
            this.grasslandProgressBar.TabIndex = 7;
            // 
            // grasslandStepLabel
            // 
            this.grasslandStepLabel.AutoSize = true;
            this.grasslandStepLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grasslandStepLabel.Location = new System.Drawing.Point(900, 52);
            this.grasslandStepLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.grasslandStepLabel.Name = "grasslandStepLabel";
            this.grasslandStepLabel.Size = new System.Drawing.Size(143, 18);
            this.grasslandStepLabel.TabIndex = 8;
            this.grasslandStepLabel.Text = "已完成 0/6 步骤";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnWetlandExtractScopeBasemap);
            this.tabPage4.Controls.Add(this.btnWetlandCleanQA);
            this.tabPage4.Controls.Add(this.btnWetlandBuildDBTables);
            this.tabPage4.Controls.Add(this.lblWetlandProcessingStatus);
            this.tabPage4.Controls.Add(this.wetlandProgressBar);
            this.tabPage4.Controls.Add(this.wetlandStepLabel);
            this.tabPage4.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage4.ForeColor = System.Drawing.Color.Black;
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(15);
            this.tabPage4.Size = new System.Drawing.Size(1267, 840);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "🏞️ 湿地资源资产清查";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnWetlandExtractScopeBasemap
            // 
            this.btnWetlandExtractScopeBasemap.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWetlandExtractScopeBasemap.Location = new System.Drawing.Point(30, 45);
            this.btnWetlandExtractScopeBasemap.Margin = new System.Windows.Forms.Padding(4);
            this.btnWetlandExtractScopeBasemap.Name = "btnWetlandExtractScopeBasemap";
            this.btnWetlandExtractScopeBasemap.Size = new System.Drawing.Size(240, 45);
            this.btnWetlandExtractScopeBasemap.TabIndex = 0;
            this.btnWetlandExtractScopeBasemap.Text = "1. 湿地范围与底图制作";
            this.btnWetlandExtractScopeBasemap.UseVisualStyleBackColor = true;
            this.btnWetlandExtractScopeBasemap.Click += new System.EventHandler(this.BtnWetlandExtractScopeBasemap_Click);
            // 
            // btnWetlandCleanQA
            // 
            this.btnWetlandCleanQA.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWetlandCleanQA.Location = new System.Drawing.Point(330, 45);
            this.btnWetlandCleanQA.Margin = new System.Windows.Forms.Padding(4);
            this.btnWetlandCleanQA.Name = "btnWetlandCleanQA";
            this.btnWetlandCleanQA.Size = new System.Drawing.Size(240, 45);
            this.btnWetlandCleanQA.TabIndex = 1;
            this.btnWetlandCleanQA.Text = "2. 湿地数据清洗与质检";
            this.btnWetlandCleanQA.UseVisualStyleBackColor = true;
            this.btnWetlandCleanQA.Click += new System.EventHandler(this.BtnWetlandCleanQA_Click);
            // 
            // btnWetlandBuildDBTables
            // 
            this.btnWetlandBuildDBTables.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWetlandBuildDBTables.Location = new System.Drawing.Point(630, 45);
            this.btnWetlandBuildDBTables.Margin = new System.Windows.Forms.Padding(4);
            this.btnWetlandBuildDBTables.Name = "btnWetlandBuildDBTables";
            this.btnWetlandBuildDBTables.Size = new System.Drawing.Size(240, 45);
            this.btnWetlandBuildDBTables.TabIndex = 2;
            this.btnWetlandBuildDBTables.Text = "3. 湿地库表构建";
            this.btnWetlandBuildDBTables.UseVisualStyleBackColor = true;
            this.btnWetlandBuildDBTables.Click += new System.EventHandler(this.BtnWetlandBuildDBTables_Click);
            // 
            // lblWetlandProcessingStatus
            // 
            this.lblWetlandProcessingStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWetlandProcessingStatus.Location = new System.Drawing.Point(30, 112);
            this.lblWetlandProcessingStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWetlandProcessingStatus.Name = "lblWetlandProcessingStatus";
            this.lblWetlandProcessingStatus.Size = new System.Drawing.Size(1215, 38);
            this.lblWetlandProcessingStatus.TabIndex = 3;
            this.lblWetlandProcessingStatus.Text = "等待湿地资源清查处理";
            this.lblWetlandProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // wetlandProgressBar
            // 
            this.wetlandProgressBar.Location = new System.Drawing.Point(900, 45);
            this.wetlandProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.wetlandProgressBar.Name = "wetlandProgressBar";
            this.wetlandProgressBar.Size = new System.Drawing.Size(345, 30);
            this.wetlandProgressBar.TabIndex = 4;
            // 
            // wetlandStepLabel
            // 
            this.wetlandStepLabel.AutoSize = true;
            this.wetlandStepLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.wetlandStepLabel.Location = new System.Drawing.Point(900, 15);
            this.wetlandStepLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.wetlandStepLabel.Name = "wetlandStepLabel";
            this.wetlandStepLabel.Size = new System.Drawing.Size(143, 18);
            this.wetlandStepLabel.TabIndex = 5;
            this.wetlandStepLabel.Text = "已完成 0/3 步骤";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnOverallQualityCheck);
            this.tabPage5.Controls.Add(this.btnStatisticalAggregation);
            this.tabPage5.Controls.Add(this.btnDataAnalysis);
            this.tabPage5.Controls.Add(this.btnExportDatasetDB);
            this.tabPage5.Controls.Add(this.btnExportSummaryTables);
            this.tabPage5.Controls.Add(this.btnGenerateReport);
            this.tabPage5.Controls.Add(this.btnGenerateThematicMaps);
            this.tabPage5.Controls.Add(this.lblFinalOutputStatus);
            this.tabPage5.Controls.Add(this.outputProgressBar);
            this.tabPage5.Controls.Add(this.outputStepLabel);
            this.tabPage5.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage5.ForeColor = System.Drawing.Color.Black;
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(15);
            this.tabPage5.Size = new System.Drawing.Size(1267, 840);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "📊 综合质检、统计与成果输出";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnOverallQualityCheck
            // 
            this.btnOverallQualityCheck.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOverallQualityCheck.Location = new System.Drawing.Point(30, 45);
            this.btnOverallQualityCheck.Margin = new System.Windows.Forms.Padding(4);
            this.btnOverallQualityCheck.Name = "btnOverallQualityCheck";
            this.btnOverallQualityCheck.Size = new System.Drawing.Size(202, 45);
            this.btnOverallQualityCheck.TabIndex = 0;
            this.btnOverallQualityCheck.Text = "1. 综合质量检查";
            this.btnOverallQualityCheck.UseVisualStyleBackColor = true;
            this.btnOverallQualityCheck.Click += new System.EventHandler(this.BtnOverallQualityCheck_Click);
            // 
            // btnStatisticalAggregation
            // 
            this.btnStatisticalAggregation.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStatisticalAggregation.Location = new System.Drawing.Point(285, 45);
            this.btnStatisticalAggregation.Margin = new System.Windows.Forms.Padding(4);
            this.btnStatisticalAggregation.Name = "btnStatisticalAggregation";
            this.btnStatisticalAggregation.Size = new System.Drawing.Size(202, 45);
            this.btnStatisticalAggregation.TabIndex = 1;
            this.btnStatisticalAggregation.Text = "2. 数据统计汇总";
            this.btnStatisticalAggregation.UseVisualStyleBackColor = true;
            this.btnStatisticalAggregation.Click += new System.EventHandler(this.BtnStatisticalAggregation_Click);
            // 
            // btnDataAnalysis
            // 
            this.btnDataAnalysis.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDataAnalysis.Location = new System.Drawing.Point(540, 45);
            this.btnDataAnalysis.Margin = new System.Windows.Forms.Padding(4);
            this.btnDataAnalysis.Name = "btnDataAnalysis";
            this.btnDataAnalysis.Size = new System.Drawing.Size(202, 45);
            this.btnDataAnalysis.TabIndex = 2;
            this.btnDataAnalysis.Text = "3. 数据分析与挖掘";
            this.btnDataAnalysis.UseVisualStyleBackColor = true;
            this.btnDataAnalysis.Click += new System.EventHandler(this.BtnDataAnalysis_Click);
            // 
            // btnExportDatasetDB
            // 
            this.btnExportDatasetDB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExportDatasetDB.Location = new System.Drawing.Point(30, 112);
            this.btnExportDatasetDB.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportDatasetDB.Name = "btnExportDatasetDB";
            this.btnExportDatasetDB.Size = new System.Drawing.Size(202, 45);
            this.btnExportDatasetDB.TabIndex = 3;
            this.btnExportDatasetDB.Text = "4. 导出清查数据集";
            this.btnExportDatasetDB.UseVisualStyleBackColor = true;
            this.btnExportDatasetDB.Click += new System.EventHandler(this.BtnExportDatasetDB_Click);
            // 
            // btnExportSummaryTables
            // 
            this.btnExportSummaryTables.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExportSummaryTables.Location = new System.Drawing.Point(285, 112);
            this.btnExportSummaryTables.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportSummaryTables.Name = "btnExportSummaryTables";
            this.btnExportSummaryTables.Size = new System.Drawing.Size(202, 45);
            this.btnExportSummaryTables.TabIndex = 4;
            this.btnExportSummaryTables.Text = "5. 导出汇总表";
            this.btnExportSummaryTables.UseVisualStyleBackColor = true;
            this.btnExportSummaryTables.Click += new System.EventHandler(this.BtnExportSummaryTables_Click);
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGenerateReport.Location = new System.Drawing.Point(540, 112);
            this.btnGenerateReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(202, 45);
            this.btnGenerateReport.TabIndex = 5;
            this.btnGenerateReport.Text = "6. 生成成果报告";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.BtnGenerateReport_Click);
            // 
            // btnGenerateThematicMaps
            // 
            this.btnGenerateThematicMaps.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGenerateThematicMaps.Location = new System.Drawing.Point(795, 112);
            this.btnGenerateThematicMaps.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerateThematicMaps.Name = "btnGenerateThematicMaps";
            this.btnGenerateThematicMaps.Size = new System.Drawing.Size(202, 45);
            this.btnGenerateThematicMaps.TabIndex = 6;
            this.btnGenerateThematicMaps.Text = "7. 生成专题图";
            this.btnGenerateThematicMaps.UseVisualStyleBackColor = true;
            this.btnGenerateThematicMaps.Click += new System.EventHandler(this.BtnGenerateThematicMaps_Click);
            // 
            // lblFinalOutputStatus
            // 
            this.lblFinalOutputStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFinalOutputStatus.Location = new System.Drawing.Point(30, 180);
            this.lblFinalOutputStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFinalOutputStatus.Name = "lblFinalOutputStatus";
            this.lblFinalOutputStatus.Size = new System.Drawing.Size(1215, 38);
            this.lblFinalOutputStatus.TabIndex = 7;
            this.lblFinalOutputStatus.Text = "等待最终成果处理";
            this.lblFinalOutputStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // outputProgressBar
            // 
            this.outputProgressBar.Location = new System.Drawing.Point(900, 45);
            this.outputProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.outputProgressBar.Name = "outputProgressBar";
            this.outputProgressBar.Size = new System.Drawing.Size(345, 30);
            this.outputProgressBar.TabIndex = 8;
            // 
            // outputStepLabel
            // 
            this.outputStepLabel.AutoSize = true;
            this.outputStepLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.outputStepLabel.Location = new System.Drawing.Point(900, 15);
            this.outputStepLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.outputStepLabel.Name = "outputStepLabel";
            this.outputStepLabel.Size = new System.Drawing.Size(143, 18);
            this.outputStepLabel.TabIndex = 9;
            this.outputStepLabel.Text = "已完成 0/7 步骤";
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.lblProgress);
            this.bottomPanel.Controls.Add(this.btnHelp);
            this.bottomPanel.Controls.Add(this.btnClose);
            this.bottomPanel.Location = new System.Drawing.Point(22, 968);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(6);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(1275, 90);
            this.bottomPanel.TabIndex = 6;
            // 
            // lblProgress
            // 
            this.lblProgress.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProgress.Location = new System.Drawing.Point(15, 27);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(900, 38);
            this.lblProgress.TabIndex = 7;
            this.lblProgress.Text = "进度：等待开始处理";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnHelp
            // 
            this.btnHelp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnHelp.Location = new System.Drawing.Point(975, 22);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(6);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(105, 38);
            this.btnHelp.TabIndex = 0;
            this.btnHelp.Text = "帮助";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(1125, 22);
            this.btnClose.Margin = new System.Windows.Forms.Padding(6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 38);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // forestWorkflowExplanation
            // 
            this.forestWorkflowExplanation.BackColor = System.Drawing.Color.White;
            this.forestWorkflowExplanation.Location = new System.Drawing.Point(30, 190);
            this.forestWorkflowExplanation.Multiline = true;
            this.forestWorkflowExplanation.Name = "forestWorkflowExplanation";
            this.forestWorkflowExplanation.ReadOnly = true;
            this.forestWorkflowExplanation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.forestWorkflowExplanation.Size = new System.Drawing.Size(380, 120);
            this.forestWorkflowExplanation.TabIndex = 11;
            this.forestWorkflowExplanation.Text = resources.GetString("forestWorkflowExplanation.Text");
            this.forestWorkflowExplanation.Visible = false;
            // 
            // forestResultsTextBox
            // 
            this.forestResultsTextBox.BackColor = System.Drawing.Color.LightYellow;
            this.forestResultsTextBox.Location = new System.Drawing.Point(30, 320);
            this.forestResultsTextBox.Multiline = true;
            this.forestResultsTextBox.Name = "forestResultsTextBox";
            this.forestResultsTextBox.ReadOnly = true;
            this.forestResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.forestResultsTextBox.Size = new System.Drawing.Size(380, 200);
            this.forestResultsTextBox.TabIndex = 12;
            this.forestResultsTextBox.Text = "处理结果将在这里显示...";
            this.forestResultsTextBox.Visible = false;
            // 
            // forestResourceChart
            // 
            chartArea1.Name = "ChartArea1";
            this.forestResourceChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.forestResourceChart.Legends.Add(legend1);
            this.forestResourceChart.Location = new System.Drawing.Point(420, 190);
            this.forestResourceChart.Name = "forestResourceChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.forestResourceChart.Series.Add(series1);
            this.forestResourceChart.Size = new System.Drawing.Size(400, 200);
            this.forestResourceChart.TabIndex = 13;
            this.forestResourceChart.Text = "森林资源清查进度";
            this.forestResourceChart.Visible = false;
            // 
            // forestWorkflowImage
            // 
            this.forestWorkflowImage.BackColor = System.Drawing.Color.LightSteelBlue;
            this.forestWorkflowImage.Location = new System.Drawing.Point(420, 400);
            this.forestWorkflowImage.Name = "forestWorkflowImage";
            this.forestWorkflowImage.Size = new System.Drawing.Size(400, 120);
            this.forestWorkflowImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.forestWorkflowImage.TabIndex = 14;
            this.forestWorkflowImage.TabStop = false;
            this.forestWorkflowImage.Visible = false;
            // 
            // MainProcessingTabsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 1080);
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainProcessingTabsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "广东省全民所有自然资源资产清查工具 - 主处理";
            this.mainPanel.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.forestResourceChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.forestWorkflowImage)).EndInit();
            this.ResumeLayout(false);

        }
    }
}