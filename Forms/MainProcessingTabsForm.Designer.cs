namespace TestArcMapAddin2.Forms
{
    partial class MainProcessingTabsForm
    {
        private System.ComponentModel.IContainer components = null;

        // Copied from MainProcessForm, excluding TabPage1 controls
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TabControl tabControlMain;
        // TabPage1 is removed
        private System.Windows.Forms.TabPage tabPage2; // Forest
        private System.Windows.Forms.TabPage tabPage3; // Grassland
        private System.Windows.Forms.TabPage tabPage4; // Wetland
        private System.Windows.Forms.TabPage tabPage5; // Output
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnHelp;

        // Labels for status
        private System.Windows.Forms.Label lblForestProcessingStatus;
        private System.Windows.Forms.Label lblGrasslandProcessingStatus;
        private System.Windows.Forms.Label lblWetlandProcessingStatus;
        private System.Windows.Forms.Label lblFinalOutputStatus;
        private System.Windows.Forms.Label lblProgress;

        // Buttons for TabPage2 (Forest)
        private System.Windows.Forms.Button btnForestExtractScope;
        private System.Windows.Forms.Button btnForestCreateBasemapLinkPrice;
        private System.Windows.Forms.Button btnForestSupplementPrice;
        private System.Windows.Forms.Button btnForestCalculateValue;
        private System.Windows.Forms.Button btnForestCleanQA;
        private System.Windows.Forms.Button btnForestBuildDBTables;

        // Buttons for TabPage3 (Grassland)
        private System.Windows.Forms.Button btnGrasslandExtractScope;
        private System.Windows.Forms.Button btnGrasslandCreateBasemapLinkPrice;
        private System.Windows.Forms.Button btnGrasslandSupplementPrice;
        private System.Windows.Forms.Button btnGrasslandCalculateValue;
        private System.Windows.Forms.Button btnGrasslandCleanQA;
        private System.Windows.Forms.Button btnGrasslandBuildDBTables;

        // Buttons for TabPage4 (Wetland)
        private System.Windows.Forms.Button btnWetlandExtractScopeBasemap;
        private System.Windows.Forms.Button btnWetlandCleanQA;
        private System.Windows.Forms.Button btnWetlandBuildDBTables;

        // Buttons for TabPage5 (Output)
        private System.Windows.Forms.Button btnOverallQualityCheck;
        private System.Windows.Forms.Button btnStatisticalAggregation;
        private System.Windows.Forms.Button btnDataAnalysis;
        private System.Windows.Forms.Button btnExportDatasetDB;
        private System.Windows.Forms.Button btnExportSummaryTables;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Button btnGenerateThematicMaps;


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
            this.components = new System.ComponentModel.Container();
            System.Drawing.Font defaultFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font groupTitleFont = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font formTitleFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Color defaultForeColor = System.Drawing.Color.Black;

            this.mainPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            // tabPage1 is removed
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();

            // Initialize buttons and labels (copied from original InitializeComponent, adjust as needed)
            // TabPage2 Forest Controls
            this.btnForestExtractScope = new System.Windows.Forms.Button();
            this.btnForestCreateBasemapLinkPrice = new System.Windows.Forms.Button();
            this.btnForestSupplementPrice = new System.Windows.Forms.Button();
            this.btnForestCalculateValue = new System.Windows.Forms.Button();
            this.btnForestCleanQA = new System.Windows.Forms.Button();
            this.btnForestBuildDBTables = new System.Windows.Forms.Button();
            this.lblForestProcessingStatus = new System.Windows.Forms.Label();

            // TabPage3 Grassland Controls
            this.btnGrasslandExtractScope = new System.Windows.Forms.Button();
            this.btnGrasslandCreateBasemapLinkPrice = new System.Windows.Forms.Button();
            this.btnGrasslandSupplementPrice = new System.Windows.Forms.Button();
            this.btnGrasslandCalculateValue = new System.Windows.Forms.Button();
            this.btnGrasslandCleanQA = new System.Windows.Forms.Button();
            this.btnGrasslandBuildDBTables = new System.Windows.Forms.Button();
            this.lblGrasslandProcessingStatus = new System.Windows.Forms.Label();

            // TabPage4 Wetland Controls
            this.btnWetlandExtractScopeBasemap = new System.Windows.Forms.Button();
            this.btnWetlandCleanQA = new System.Windows.Forms.Button();
            this.btnWetlandBuildDBTables = new System.Windows.Forms.Button();
            this.lblWetlandProcessingStatus = new System.Windows.Forms.Label();

            // TabPage5 Final Output Controls
            this.btnOverallQualityCheck = new System.Windows.Forms.Button();
            this.btnStatisticalAggregation = new System.Windows.Forms.Button();
            this.btnDataAnalysis = new System.Windows.Forms.Button();
            this.btnExportDatasetDB = new System.Windows.Forms.Button();
            this.btnExportSummaryTables = new System.Windows.Forms.Button();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnGenerateThematicMaps = new System.Windows.Forms.Button();
            this.lblFinalOutputStatus = new System.Windows.Forms.Label();
            
            this.lblProgress = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();

            this.mainPanel.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();

            // mainPanel
            this.mainPanel.Controls.Add(this.titleLabel);
            this.mainPanel.Controls.Add(this.tabControlMain);
            this.mainPanel.Controls.Add(this.bottomPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(15);
            this.mainPanel.TabIndex = 0;

            // titleLabel
            this.titleLabel.Font = formTitleFont;
            this.titleLabel.ForeColor = defaultForeColor;
            this.titleLabel.Location = new System.Drawing.Point(15, 15);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(850, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "广东省全民所有自然资源（森林、草地、湿地）资产清查工具 - 主处理模块";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // tabControlMain
            // TabPage1 is removed
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Controls.Add(this.tabPage3);
            this.tabControlMain.Controls.Add(this.tabPage4);
            this.tabControlMain.Controls.Add(this.tabPage5);
            this.tabControlMain.Font = defaultFont;
            this.tabControlMain.ItemSize = new System.Drawing.Size(100, 22);
            this.tabControlMain.Location = new System.Drawing.Point(15, titleLabel.Bottom + 10);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(850, 360); 
            this.tabControlMain.TabIndex = 1;

            // tabPage2 (Forest)
            this.tabPage2.Controls.Add(this.btnForestExtractScope);
            this.tabPage2.Controls.Add(this.btnForestCreateBasemapLinkPrice);
            this.tabPage2.Controls.Add(this.btnForestSupplementPrice);
            this.tabPage2.Controls.Add(this.btnForestCalculateValue);
            this.tabPage2.Controls.Add(this.btnForestCleanQA);
            this.tabPage2.Controls.Add(this.btnForestBuildDBTables);
            this.tabPage2.Controls.Add(this.lblForestProcessingStatus);
            this.tabPage2.Font = groupTitleFont;
            this.tabPage2.ForeColor = defaultForeColor;
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage2.Size = new System.Drawing.Size(842, 330);
            this.tabPage2.TabIndex = 1; // Index adjusted as tabPage1 is removed
            this.tabPage2.Text = "🌲 森林资源资产清查";
            this.tabPage2.UseVisualStyleBackColor = true;

            // btnForestExtractScope
            this.btnForestExtractScope.Font = defaultFont;
            this.btnForestExtractScope.Location = new System.Drawing.Point(20, 30);
            this.btnForestExtractScope.Name = "btnForestExtractScope";
            this.btnForestExtractScope.Size = new System.Drawing.Size(160, 30);
            this.btnForestExtractScope.TabIndex = 0;
            this.btnForestExtractScope.Text = "提取森林工作范围";
            this.btnForestExtractScope.UseVisualStyleBackColor = true;
            this.btnForestExtractScope.Click += new System.EventHandler(this.BtnForestExtractScope_Click);
            // ... (Add other Forest buttons similarly, positions from original TestForm)
            this.btnForestCreateBasemapLinkPrice.Font = defaultFont;
            this.btnForestCreateBasemapLinkPrice.Location = new System.Drawing.Point(220, 30);
            this.btnForestCreateBasemapLinkPrice.Name = "btnForestCreateBasemapLinkPrice";
            this.btnForestCreateBasemapLinkPrice.Size = new System.Drawing.Size(160, 30);
            this.btnForestCreateBasemapLinkPrice.TabIndex = 1;
            this.btnForestCreateBasemapLinkPrice.Text = "森林底图与价格关联";
            this.btnForestCreateBasemapLinkPrice.UseVisualStyleBackColor = true;
            this.btnForestCreateBasemapLinkPrice.Click += new System.EventHandler(this.BtnForestCreateBasemapLinkPrice_Click);

            this.btnForestSupplementPrice.Font = defaultFont;
            this.btnForestSupplementPrice.Location = new System.Drawing.Point(420, 30);
            this.btnForestSupplementPrice.Name = "btnForestSupplementPrice";
            this.btnForestSupplementPrice.Size = new System.Drawing.Size(160, 30);
            this.btnForestSupplementPrice.TabIndex = 2;
            this.btnForestSupplementPrice.Text = "补充森林基准价格";
            this.btnForestSupplementPrice.UseVisualStyleBackColor = true;
            this.btnForestSupplementPrice.Click += new System.EventHandler(this.BtnForestSupplementPrice_Click);

            this.btnForestCalculateValue.Font = defaultFont;
            this.btnForestCalculateValue.Location = new System.Drawing.Point(20, 75);
            this.btnForestCalculateValue.Name = "btnForestCalculateValue";
            this.btnForestCalculateValue.Size = new System.Drawing.Size(160, 30);
            this.btnForestCalculateValue.TabIndex = 3;
            this.btnForestCalculateValue.Text = "计算森林资产价值";
            this.btnForestCalculateValue.UseVisualStyleBackColor = true;
            this.btnForestCalculateValue.Click += new System.EventHandler(this.BtnForestCalculateValue_Click);

            this.btnForestCleanQA.Font = defaultFont;
            this.btnForestCleanQA.Location = new System.Drawing.Point(220, 75);
            this.btnForestCleanQA.Name = "btnForestCleanQA";
            this.btnForestCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnForestCleanQA.TabIndex = 4;
            this.btnForestCleanQA.Text = "森林数据清洗与质检";
            this.btnForestCleanQA.UseVisualStyleBackColor = true;
            this.btnForestCleanQA.Click += new System.EventHandler(this.BtnForestCleanQA_Click);

            this.btnForestBuildDBTables.Font = defaultFont;
            this.btnForestBuildDBTables.Location = new System.Drawing.Point(420, 75);
            this.btnForestBuildDBTables.Name = "btnForestBuildDBTables";
            this.btnForestBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnForestBuildDBTables.TabIndex = 5;
            this.btnForestBuildDBTables.Text = "森林库表构建";
            this.btnForestBuildDBTables.UseVisualStyleBackColor = true;
            this.btnForestBuildDBTables.Click += new System.EventHandler(this.BtnForestBuildDBTables_Click);
            
            this.lblForestProcessingStatus.Font = defaultFont;
            this.lblForestProcessingStatus.Location = new System.Drawing.Point(20, 120);
            this.lblForestProcessingStatus.Name = "lblForestProcessingStatus";
            this.lblForestProcessingStatus.Size = new System.Drawing.Size(810, 25);
            this.lblForestProcessingStatus.TabIndex = 6;
            this.lblForestProcessingStatus.Text = "等待森林资源清查处理";
            this.lblForestProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;


            // tabPage3 (Grassland)
            this.tabPage3.Controls.Add(this.btnGrasslandExtractScope);
            this.tabPage3.Controls.Add(this.btnGrasslandCreateBasemapLinkPrice);
            this.tabPage3.Controls.Add(this.btnGrasslandSupplementPrice);
            this.tabPage3.Controls.Add(this.btnGrasslandCalculateValue);
            this.tabPage3.Controls.Add(this.btnGrasslandCleanQA);
            this.tabPage3.Controls.Add(this.btnGrasslandBuildDBTables);
            this.tabPage3.Controls.Add(this.lblGrasslandProcessingStatus);
            this.tabPage3.Font = groupTitleFont;
            this.tabPage3.ForeColor = defaultForeColor;
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage3.Size = new System.Drawing.Size(842, 330);
            this.tabPage3.TabIndex = 2; // Index adjusted
            this.tabPage3.Text = "🌿 草地资源资产清查";
            this.tabPage3.UseVisualStyleBackColor = true;
            // ... (Add Grassland buttons and label similarly)
            this.btnGrasslandExtractScope.Font = defaultFont;
            this.btnGrasslandExtractScope.Location = new System.Drawing.Point(20, 30);
            this.btnGrasslandExtractScope.Name = "btnGrasslandExtractScope";
            this.btnGrasslandExtractScope.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandExtractScope.TabIndex = 0;
            this.btnGrasslandExtractScope.Text = "提取草地工作范围";
            this.btnGrasslandExtractScope.UseVisualStyleBackColor = true;
            this.btnGrasslandExtractScope.Click += new System.EventHandler(this.BtnGrasslandExtractScope_Click);

            this.btnGrasslandCreateBasemapLinkPrice.Font = defaultFont;
            this.btnGrasslandCreateBasemapLinkPrice.Location = new System.Drawing.Point(220, 30);
            this.btnGrasslandCreateBasemapLinkPrice.Name = "btnGrasslandCreateBasemapLinkPrice";
            this.btnGrasslandCreateBasemapLinkPrice.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCreateBasemapLinkPrice.TabIndex = 1;
            this.btnGrasslandCreateBasemapLinkPrice.Text = "草地底图与价格关联";
            this.btnGrasslandCreateBasemapLinkPrice.UseVisualStyleBackColor = true;
            this.btnGrasslandCreateBasemapLinkPrice.Click += new System.EventHandler(this.BtnGrasslandCreateBasemapLinkPrice_Click);

            this.btnGrasslandSupplementPrice.Font = defaultFont;
            this.btnGrasslandSupplementPrice.Location = new System.Drawing.Point(420, 30);
            this.btnGrasslandSupplementPrice.Name = "btnGrasslandSupplementPrice";
            this.btnGrasslandSupplementPrice.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandSupplementPrice.TabIndex = 2;
            this.btnGrasslandSupplementPrice.Text = "补充草地基准价格";
            this.btnGrasslandSupplementPrice.UseVisualStyleBackColor = true;
            this.btnGrasslandSupplementPrice.Click += new System.EventHandler(this.BtnGrasslandSupplementPrice_Click);

            this.btnGrasslandCalculateValue.Font = defaultFont;
            this.btnGrasslandCalculateValue.Location = new System.Drawing.Point(20, 75);
            this.btnGrasslandCalculateValue.Name = "btnGrasslandCalculateValue";
            this.btnGrasslandCalculateValue.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCalculateValue.TabIndex = 3;
            this.btnGrasslandCalculateValue.Text = "计算草地资产价值";
            this.btnGrasslandCalculateValue.UseVisualStyleBackColor = true;
            this.btnGrasslandCalculateValue.Click += new System.EventHandler(this.BtnGrasslandCalculateValue_Click);

            this.btnGrasslandCleanQA.Font = defaultFont;
            this.btnGrasslandCleanQA.Location = new System.Drawing.Point(220, 75);
            this.btnGrasslandCleanQA.Name = "btnGrasslandCleanQA";
            this.btnGrasslandCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCleanQA.TabIndex = 4;
            this.btnGrasslandCleanQA.Text = "草地数据清洗与质检";
            this.btnGrasslandCleanQA.UseVisualStyleBackColor = true;
            this.btnGrasslandCleanQA.Click += new System.EventHandler(this.BtnGrasslandCleanQA_Click);

            this.btnGrasslandBuildDBTables.Font = defaultFont;
            this.btnGrasslandBuildDBTables.Location = new System.Drawing.Point(420, 75);
            this.btnGrasslandBuildDBTables.Name = "btnGrasslandBuildDBTables";
            this.btnGrasslandBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandBuildDBTables.TabIndex = 5;
            this.btnGrasslandBuildDBTables.Text = "草地库表构建";
            this.btnGrasslandBuildDBTables.UseVisualStyleBackColor = true;
            this.btnGrasslandBuildDBTables.Click += new System.EventHandler(this.BtnGrasslandBuildDBTables_Click);

            this.lblGrasslandProcessingStatus.Font = defaultFont;
            this.lblGrasslandProcessingStatus.Location = new System.Drawing.Point(20, 120);
            this.lblGrasslandProcessingStatus.Name = "lblGrasslandProcessingStatus";
            this.lblGrasslandProcessingStatus.Size = new System.Drawing.Size(810, 25);
            this.lblGrasslandProcessingStatus.TabIndex = 6;
            this.lblGrasslandProcessingStatus.Text = "等待草地资源清查处理";
            this.lblGrasslandProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;


            // tabPage4 (Wetland)
            this.tabPage4.Controls.Add(this.btnWetlandExtractScopeBasemap);
            this.tabPage4.Controls.Add(this.btnWetlandCleanQA);
            this.tabPage4.Controls.Add(this.btnWetlandBuildDBTables);
            this.tabPage4.Controls.Add(this.lblWetlandProcessingStatus);
            this.tabPage4.Font = groupTitleFont;
            this.tabPage4.ForeColor = defaultForeColor;
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage4.Size = new System.Drawing.Size(842, 330);
            this.tabPage4.TabIndex = 3; // Index adjusted
            this.tabPage4.Text = "🏞️ 湿地资源资产清查";
            this.tabPage4.UseVisualStyleBackColor = true;
            // ... (Add Wetland buttons and label similarly)
            this.btnWetlandExtractScopeBasemap.Font = defaultFont;
            this.btnWetlandExtractScopeBasemap.Location = new System.Drawing.Point(20, 30);
            this.btnWetlandExtractScopeBasemap.Name = "btnWetlandExtractScopeBasemap";
            this.btnWetlandExtractScopeBasemap.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandExtractScopeBasemap.TabIndex = 0;
            this.btnWetlandExtractScopeBasemap.Text = "湿地范围与底图制作";
            this.btnWetlandExtractScopeBasemap.UseVisualStyleBackColor = true;
            this.btnWetlandExtractScopeBasemap.Click += new System.EventHandler(this.BtnWetlandExtractScopeBasemap_Click);

            this.btnWetlandCleanQA.Font = defaultFont;
            this.btnWetlandCleanQA.Location = new System.Drawing.Point(220, 30);
            this.btnWetlandCleanQA.Name = "btnWetlandCleanQA";
            this.btnWetlandCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandCleanQA.TabIndex = 1;
            this.btnWetlandCleanQA.Text = "湿地数据清洗与质检";
            this.btnWetlandCleanQA.UseVisualStyleBackColor = true;
            this.btnWetlandCleanQA.Click += new System.EventHandler(this.BtnWetlandCleanQA_Click);

            this.btnWetlandBuildDBTables.Font = defaultFont;
            this.btnWetlandBuildDBTables.Location = new System.Drawing.Point(420, 30);
            this.btnWetlandBuildDBTables.Name = "btnWetlandBuildDBTables";
            this.btnWetlandBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandBuildDBTables.TabIndex = 2;
            this.btnWetlandBuildDBTables.Text = "湿地库表构建";
            this.btnWetlandBuildDBTables.UseVisualStyleBackColor = true;
            this.btnWetlandBuildDBTables.Click += new System.EventHandler(this.BtnWetlandBuildDBTables_Click);

            this.lblWetlandProcessingStatus.Font = defaultFont;
            this.lblWetlandProcessingStatus.Location = new System.Drawing.Point(20, 75);
            this.lblWetlandProcessingStatus.Name = "lblWetlandProcessingStatus";
            this.lblWetlandProcessingStatus.Size = new System.Drawing.Size(810, 25);
            this.lblWetlandProcessingStatus.TabIndex = 3;
            this.lblWetlandProcessingStatus.Text = "等待湿地资源清查处理";
            this.lblWetlandProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // tabPage5 (Output)
            this.tabPage5.Controls.Add(this.btnOverallQualityCheck);
            this.tabPage5.Controls.Add(this.btnStatisticalAggregation);
            this.tabPage5.Controls.Add(this.btnDataAnalysis);
            this.tabPage5.Controls.Add(this.btnExportDatasetDB);
            this.tabPage5.Controls.Add(this.btnExportSummaryTables);
            this.tabPage5.Controls.Add(this.btnGenerateReport);
            this.tabPage5.Controls.Add(this.btnGenerateThematicMaps);
            this.tabPage5.Controls.Add(this.lblFinalOutputStatus);
            this.tabPage5.Font = groupTitleFont;
            this.tabPage5.ForeColor = defaultForeColor;
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage5.Size = new System.Drawing.Size(842, 330);
            this.tabPage5.TabIndex = 4; // Index adjusted
            this.tabPage5.Text = "📊 综合质检、统计与成果输出";
            this.tabPage5.UseVisualStyleBackColor = true;
            // ... (Add Output buttons and label similarly)
            this.btnOverallQualityCheck.Font = defaultFont;
            this.btnOverallQualityCheck.Location = new System.Drawing.Point(20, 30);
            this.btnOverallQualityCheck.Name = "btnOverallQualityCheck";
            this.btnOverallQualityCheck.Size = new System.Drawing.Size(135, 30);
            this.btnOverallQualityCheck.TabIndex = 0;
            this.btnOverallQualityCheck.Text = "综合质量检查";
            this.btnOverallQualityCheck.UseVisualStyleBackColor = true;
            this.btnOverallQualityCheck.Click += new System.EventHandler(this.BtnOverallQualityCheck_Click);

            this.btnStatisticalAggregation.Font = defaultFont;
            this.btnStatisticalAggregation.Location = new System.Drawing.Point(190, 30);
            this.btnStatisticalAggregation.Name = "btnStatisticalAggregation";
            this.btnStatisticalAggregation.Size = new System.Drawing.Size(135, 30);
            this.btnStatisticalAggregation.TabIndex = 1;
            this.btnStatisticalAggregation.Text = "数据统计汇总";
            this.btnStatisticalAggregation.UseVisualStyleBackColor = true;
            this.btnStatisticalAggregation.Click += new System.EventHandler(this.BtnStatisticalAggregation_Click);

            this.btnDataAnalysis.Font = defaultFont;
            this.btnDataAnalysis.Location = new System.Drawing.Point(360, 30);
            this.btnDataAnalysis.Name = "btnDataAnalysis";
            this.btnDataAnalysis.Size = new System.Drawing.Size(135, 30);
            this.btnDataAnalysis.TabIndex = 2;
            this.btnDataAnalysis.Text = "数据分析与挖掘";
            this.btnDataAnalysis.UseVisualStyleBackColor = true;
            this.btnDataAnalysis.Click += new System.EventHandler(this.BtnDataAnalysis_Click);

            this.btnExportDatasetDB.Font = defaultFont;
            this.btnExportDatasetDB.Location = new System.Drawing.Point(20, 75);
            this.btnExportDatasetDB.Name = "btnExportDatasetDB";
            this.btnExportDatasetDB.Size = new System.Drawing.Size(135, 30);
            this.btnExportDatasetDB.TabIndex = 3;
            this.btnExportDatasetDB.Text = "导出清查数据集";
            this.btnExportDatasetDB.UseVisualStyleBackColor = true;
            this.btnExportDatasetDB.Click += new System.EventHandler(this.BtnExportDatasetDB_Click);

            this.btnExportSummaryTables.Font = defaultFont;
            this.btnExportSummaryTables.Location = new System.Drawing.Point(190, 75);
            this.btnExportSummaryTables.Name = "btnExportSummaryTables";
            this.btnExportSummaryTables.Size = new System.Drawing.Size(135, 30);
            this.btnExportSummaryTables.TabIndex = 4;
            this.btnExportSummaryTables.Text = "导出汇总表";
            this.btnExportSummaryTables.UseVisualStyleBackColor = true;
            this.btnExportSummaryTables.Click += new System.EventHandler(this.BtnExportSummaryTables_Click);

            this.btnGenerateReport.Font = defaultFont;
            this.btnGenerateReport.Location = new System.Drawing.Point(360, 75);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(135, 30);
            this.btnGenerateReport.TabIndex = 5;
            this.btnGenerateReport.Text = "生成成果报告";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.BtnGenerateReport_Click);

            this.btnGenerateThematicMaps.Font = defaultFont;
            this.btnGenerateThematicMaps.Location = new System.Drawing.Point(530, 75);
            this.btnGenerateThematicMaps.Name = "btnGenerateThematicMaps";
            this.btnGenerateThematicMaps.Size = new System.Drawing.Size(135, 30);
            this.btnGenerateThematicMaps.TabIndex = 6;
            this.btnGenerateThematicMaps.Text = "生成专题图";
            this.btnGenerateThematicMaps.UseVisualStyleBackColor = true;
            this.btnGenerateThematicMaps.Click += new System.EventHandler(this.BtnGenerateThematicMaps_Click);
            
            this.lblFinalOutputStatus.Font = defaultFont;
            this.lblFinalOutputStatus.Location = new System.Drawing.Point(20, 155);
            this.lblFinalOutputStatus.Name = "lblFinalOutputStatus";
            this.lblFinalOutputStatus.Size = new System.Drawing.Size(810, 25);
            this.lblFinalOutputStatus.TabIndex = 8;
            this.lblFinalOutputStatus.Text = "等待最终成果处理";
            this.lblFinalOutputStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // bottomPanel
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.lblProgress);
            this.bottomPanel.Controls.Add(this.btnHelp);
            this.bottomPanel.Controls.Add(this.btnClose);
            this.bottomPanel.Location = new System.Drawing.Point(15, tabControlMain.Bottom + 10);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(4);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(850, 60);
            this.bottomPanel.TabIndex = 6;

            // lblProgress
            this.lblProgress.Font = defaultFont;
            this.lblProgress.Location = new System.Drawing.Point(10, 18);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(600, 25);
            this.lblProgress.TabIndex = 7;
            this.lblProgress.Text = "进度：等待开始处理";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // btnHelp
            this.btnHelp.Font = defaultFont;
            this.btnHelp.Location = new System.Drawing.Point(650, 15); // Will be adjusted by AdjustButtonPositions
            this.btnHelp.Margin = new System.Windows.Forms.Padding(4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(70, 25);
            this.btnHelp.TabIndex = 0;
            this.btnHelp.Text = "帮助";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            
            // btnClose
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = defaultFont;
            this.btnClose.Location = new System.Drawing.Point(750, 15); // Will be adjusted by AdjustButtonPositions
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            
            // Set mainPanel and Form Size
            this.mainPanel.Size = new System.Drawing.Size(880, this.bottomPanel.Bottom + 15);
            
            // MainProcessingTabsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = this.mainPanel.Size;
            this.Controls.Add(this.mainPanel);
            this.Font = defaultFont;
            this.ForeColor = defaultForeColor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainProcessingTabsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "广东省全民所有自然资源资产清查工具 - 主处理"; // Form Title

            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}