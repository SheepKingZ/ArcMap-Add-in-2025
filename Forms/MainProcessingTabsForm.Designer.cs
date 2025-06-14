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
        private System.Windows.Forms.Button btnForestCalculateValue;
        private System.Windows.Forms.Button btnForestCleanQA;
        private System.Windows.Forms.Button btnForestBuildDBTables;

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

        // 进度条和步骤标签
        private System.Windows.Forms.ProgressBar forestProgressBar;
        private System.Windows.Forms.ProgressBar grasslandProgressBar;
        private System.Windows.Forms.ProgressBar wetlandProgressBar;
        private System.Windows.Forms.ProgressBar outputProgressBar;
        private System.Windows.Forms.Label forestStepLabel;
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Drawing.Font defaultFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font groupTitleFont = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font formTitleFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Color defaultForeColor = System.Drawing.Color.Black;

            // 创建控件实例
            this.mainPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();

            // 森林资源选项卡控件
            this.btnForestExtractScope = new System.Windows.Forms.Button();
            this.btnForestCreateBasemapLinkPrice = new System.Windows.Forms.Button();
            this.btnForestSupplementPrice = new System.Windows.Forms.Button();
            this.btnForestCalculateValue = new System.Windows.Forms.Button();
            this.btnForestCleanQA = new System.Windows.Forms.Button();
            this.btnForestBuildDBTables = new System.Windows.Forms.Button();
            this.lblForestProcessingStatus = new System.Windows.Forms.Label();
            this.forestProgressBar = new System.Windows.Forms.ProgressBar();
            this.forestStepLabel = new System.Windows.Forms.Label();
            this.forestDetailPanel = new System.Windows.Forms.Panel();
            this.showForestWorkflowDetails = new System.Windows.Forms.LinkLabel();
            this.forestWorkflowExplanation = new System.Windows.Forms.TextBox();
            this.forestResultsTextBox = new System.Windows.Forms.TextBox();
            this.forestResourceChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.forestWorkflowImage = new System.Windows.Forms.PictureBox();

            // 草地资源选项卡控件
            this.btnGrasslandExtractScope = new System.Windows.Forms.Button();
            this.btnGrasslandCreateBasemapLinkPrice = new System.Windows.Forms.Button();
            this.btnGrasslandSupplementPrice = new System.Windows.Forms.Button();
            this.btnGrasslandCalculateValue = new System.Windows.Forms.Button();
            this.btnGrasslandCleanQA = new System.Windows.Forms.Button();
            this.btnGrasslandBuildDBTables = new System.Windows.Forms.Button();
            this.lblGrasslandProcessingStatus = new System.Windows.Forms.Label();
            this.grasslandProgressBar = new System.Windows.Forms.ProgressBar();
            this.grasslandStepLabel = new System.Windows.Forms.Label();

            // 湿地资源选项卡控件
            this.btnWetlandExtractScopeBasemap = new System.Windows.Forms.Button();
            this.btnWetlandCleanQA = new System.Windows.Forms.Button();
            this.btnWetlandBuildDBTables = new System.Windows.Forms.Button();
            this.lblWetlandProcessingStatus = new System.Windows.Forms.Label();
            this.wetlandProgressBar = new System.Windows.Forms.ProgressBar();
            this.wetlandStepLabel = new System.Windows.Forms.Label();

            // 综合输出选项卡控件
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

            // 底部面板控件
            this.lblProgress = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();

            // 暂停控件布局
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

            // 主面板设置
            this.mainPanel.Controls.Add(this.titleLabel);
            this.mainPanel.Controls.Add(this.tabControlMain);
            this.mainPanel.Controls.Add(this.bottomPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(15);
            this.mainPanel.TabIndex = 0;

            // 标题标签
            this.titleLabel.Font = formTitleFont;
            this.titleLabel.ForeColor = defaultForeColor;
            this.titleLabel.Location = new System.Drawing.Point(15, 15);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(850, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "广东省全民所有自然资源（森林、草地、湿地）资产清查工具 - 主处理模块";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 选项卡控件
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Controls.Add(this.tabPage3);
            this.tabControlMain.Controls.Add(this.tabPage4);
            this.tabControlMain.Controls.Add(this.tabPage5);
            this.tabControlMain.Font = defaultFont;
            this.tabControlMain.ItemSize = new System.Drawing.Size(100, 22);
            this.tabControlMain.Location = new System.Drawing.Point(15, 55);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(850, 580); // 增加高度以适应更多内容
            this.tabControlMain.TabIndex = 1;

            // 森林资源选项卡
            this.tabPage2.Controls.Add(this.showForestWorkflowDetails);
            this.tabPage2.Controls.Add(this.forestDetailPanel);
            this.tabPage2.Controls.Add(this.btnForestExtractScope);
            this.tabPage2.Controls.Add(this.btnForestCreateBasemapLinkPrice);
            this.tabPage2.Controls.Add(this.btnForestSupplementPrice);
            this.tabPage2.Controls.Add(this.btnForestCalculateValue);
            this.tabPage2.Controls.Add(this.btnForestCleanQA);
            this.tabPage2.Controls.Add(this.btnForestBuildDBTables);
            this.tabPage2.Controls.Add(this.lblForestProcessingStatus);
            this.tabPage2.Controls.Add(this.forestProgressBar);
            this.tabPage2.Controls.Add(this.forestStepLabel);
            this.tabPage2.Font = groupTitleFont;
            this.tabPage2.ForeColor = defaultForeColor;
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage2.Size = new System.Drawing.Size(842, 550); // 增加高度
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "🌲 森林资源资产清查";
            this.tabPage2.UseVisualStyleBackColor = true;

            // 森林详情链接
            this.showForestWorkflowDetails.AutoSize = true;
            this.showForestWorkflowDetails.Location = new System.Drawing.Point(20, 160);
            this.showForestWorkflowDetails.Name = "showForestWorkflowDetails";
            this.showForestWorkflowDetails.Size = new System.Drawing.Size(150, 15);
            this.showForestWorkflowDetails.TabIndex = 9;
            this.showForestWorkflowDetails.TabStop = true;
            this.showForestWorkflowDetails.Text = "显示详细流程说明 ▼";
            this.showForestWorkflowDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ShowForestWorkflowDetails_LinkClicked);

            // 森林详情面板
            this.forestDetailPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.forestDetailPanel.Location = new System.Drawing.Point(20, 180);
            this.forestDetailPanel.Name = "forestDetailPanel";
            this.forestDetailPanel.Size = new System.Drawing.Size(810, 350);
            this.forestDetailPanel.TabIndex = 10;
            this.forestDetailPanel.Visible = false;

            // 森林工作流说明文本框
            this.forestWorkflowExplanation.BackColor = System.Drawing.Color.White;
            this.forestWorkflowExplanation.Location = new System.Drawing.Point(30, 190);
            this.forestWorkflowExplanation.Multiline = true;
            this.forestWorkflowExplanation.Name = "forestWorkflowExplanation";
            this.forestWorkflowExplanation.ReadOnly = true;
            this.forestWorkflowExplanation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.forestWorkflowExplanation.Size = new System.Drawing.Size(380, 120);
            this.forestWorkflowExplanation.TabIndex = 11;
            this.forestWorkflowExplanation.Text = "森林资源资产清查流程说明：\r\n\r\n1. 提取工作范围：筛选属性为国有的林地图斑或位于城镇开发边界内的集体林地图斑\r\n\r\n2. 制作工作底图：工作范围与林地分等数据关联，补充完善数据库；与林地定级数据关联，将基准地价通过空间挂接\r\n\r\n3. 林地基准价格参数提取：梳理提取县（区）林地定级指标及权重信息、基准价格信息等\r\n\r\n4. 补充完善林地基准地价：针对图斑挂接基准地价时出现落空的情况进行补充\r\n\r\n5. 资源资产价值计算：基于修正因子计算宗地价格，进行期日修正和年期修正\r\n\r\n6. 数据清洗与质检：确保数据符合规范要求";
            this.forestWorkflowExplanation.Visible = false;

            // 森林结果文本框
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

            // 森林资源图表
            chartArea.Name = "ChartArea1";
            this.forestResourceChart.ChartAreas.Add(chartArea);
            legend.Name = "Legend1";
            this.forestResourceChart.Legends.Add(legend);
            this.forestResourceChart.Location = new System.Drawing.Point(420, 190);
            this.forestResourceChart.Name = "forestResourceChart";
            series.ChartArea = "ChartArea1";
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series.Legend = "Legend1";
            series.Name = "Series1";
            this.forestResourceChart.Series.Add(series);
            this.forestResourceChart.Size = new System.Drawing.Size(400, 200);
            this.forestResourceChart.TabIndex = 13;
            this.forestResourceChart.Text = "森林资源清查进度";
            this.forestResourceChart.Visible = false;

            // 森林工作流程图
            this.forestWorkflowImage.Location = new System.Drawing.Point(420, 400);
            this.forestWorkflowImage.Name = "forestWorkflowImage";
            this.forestWorkflowImage.Size = new System.Drawing.Size(400, 120);
            this.forestWorkflowImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.forestWorkflowImage.TabIndex = 14;
            this.forestWorkflowImage.TabStop = false;
            this.forestWorkflowImage.BackColor = System.Drawing.Color.LightSteelBlue;
            this.forestWorkflowImage.Visible = false;

            // 提取森林工作范围按钮
            this.btnForestExtractScope.Font = defaultFont;
            this.btnForestExtractScope.Location = new System.Drawing.Point(20, 30);
            this.btnForestExtractScope.Name = "btnForestExtractScope";
            this.btnForestExtractScope.Size = new System.Drawing.Size(160, 30);
            this.btnForestExtractScope.TabIndex = 0;
            this.btnForestExtractScope.Text = "1. 提取森林工作范围";
            this.btnForestExtractScope.UseVisualStyleBackColor = true;
            this.btnForestExtractScope.Click += new System.EventHandler(this.BtnForestExtractScope_Click);

            // 森林底图与价格关联按钮
            this.btnForestCreateBasemapLinkPrice.Font = defaultFont;
            this.btnForestCreateBasemapLinkPrice.Location = new System.Drawing.Point(220, 30);
            this.btnForestCreateBasemapLinkPrice.Name = "btnForestCreateBasemapLinkPrice";
            this.btnForestCreateBasemapLinkPrice.Size = new System.Drawing.Size(160, 30);
            this.btnForestCreateBasemapLinkPrice.TabIndex = 1;
            this.btnForestCreateBasemapLinkPrice.Text = "2. 森林底图与价格关联";
            this.btnForestCreateBasemapLinkPrice.UseVisualStyleBackColor = true;
            this.btnForestCreateBasemapLinkPrice.Click += new System.EventHandler(this.BtnForestCreateBasemapLinkPrice_Click);

            // 补充森林基准价格按钮
            this.btnForestSupplementPrice.Font = defaultFont;
            this.btnForestSupplementPrice.Location = new System.Drawing.Point(420, 30);
            this.btnForestSupplementPrice.Name = "btnForestSupplementPrice";
            this.btnForestSupplementPrice.Size = new System.Drawing.Size(160, 30);
            this.btnForestSupplementPrice.TabIndex = 2;
            this.btnForestSupplementPrice.Text = "3. 补充森林基准价格";
            this.btnForestSupplementPrice.UseVisualStyleBackColor = true;
            this.btnForestSupplementPrice.Click += new System.EventHandler(this.BtnForestSupplementPrice_Click);

            // 计算森林资产价值按钮
            this.btnForestCalculateValue.Font = defaultFont;
            this.btnForestCalculateValue.Location = new System.Drawing.Point(20, 75);
            this.btnForestCalculateValue.Name = "btnForestCalculateValue";
            this.btnForestCalculateValue.Size = new System.Drawing.Size(160, 30);
            this.btnForestCalculateValue.TabIndex = 3;
            this.btnForestCalculateValue.Text = "4. 计算森林资产价值";
            this.btnForestCalculateValue.UseVisualStyleBackColor = true;
            this.btnForestCalculateValue.Click += new System.EventHandler(this.BtnForestCalculateValue_Click);

            // 森林数据清洗与质检按钮
            this.btnForestCleanQA.Font = defaultFont;
            this.btnForestCleanQA.Location = new System.Drawing.Point(220, 75);
            this.btnForestCleanQA.Name = "btnForestCleanQA";
            this.btnForestCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnForestCleanQA.TabIndex = 4;
            this.btnForestCleanQA.Text = "5. 森林数据清洗与质检";
            this.btnForestCleanQA.UseVisualStyleBackColor = true;
            this.btnForestCleanQA.Click += new System.EventHandler(this.BtnForestCleanQA_Click);

            // 森林库表构建按钮
            this.btnForestBuildDBTables.Font = defaultFont;
            this.btnForestBuildDBTables.Location = new System.Drawing.Point(420, 75);
            this.btnForestBuildDBTables.Name = "btnForestBuildDBTables";
            this.btnForestBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnForestBuildDBTables.TabIndex = 5;
            this.btnForestBuildDBTables.Text = "6. 森林库表构建";
            this.btnForestBuildDBTables.UseVisualStyleBackColor = true;
            this.btnForestBuildDBTables.Click += new System.EventHandler(this.BtnForestBuildDBTables_Click);

            // 森林处理状态标签
            this.lblForestProcessingStatus.Font = defaultFont;
            this.lblForestProcessingStatus.Location = new System.Drawing.Point(20, 120);
            this.lblForestProcessingStatus.Name = "lblForestProcessingStatus";
            this.lblForestProcessingStatus.Size = new System.Drawing.Size(810, 25);
            this.lblForestProcessingStatus.TabIndex = 6;
            this.lblForestProcessingStatus.Text = "等待森林资源清查处理";
            this.lblForestProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 森林进度条
            this.forestProgressBar.Location = new System.Drawing.Point(600, 75);
            this.forestProgressBar.Name = "forestProgressBar";
            this.forestProgressBar.Size = new System.Drawing.Size(230, 20);
            this.forestProgressBar.TabIndex = 7;

            // 森林步骤标签
            this.forestStepLabel.AutoSize = true;
            this.forestStepLabel.Font = defaultFont;
            this.forestStepLabel.Location = new System.Drawing.Point(600, 35);
            this.forestStepLabel.Name = "forestStepLabel";
            this.forestStepLabel.Size = new System.Drawing.Size(104, 15);
            this.forestStepLabel.TabIndex = 8;
            this.forestStepLabel.Text = "已完成 0/6 步骤";

            // 草地资源选项卡
            this.tabPage3.Controls.Add(this.btnGrasslandExtractScope);
            this.tabPage3.Controls.Add(this.btnGrasslandCreateBasemapLinkPrice);
            this.tabPage3.Controls.Add(this.btnGrasslandSupplementPrice);
            this.tabPage3.Controls.Add(this.btnGrasslandCalculateValue);
            this.tabPage3.Controls.Add(this.btnGrasslandCleanQA);
            this.tabPage3.Controls.Add(this.btnGrasslandBuildDBTables);
            this.tabPage3.Controls.Add(this.lblGrasslandProcessingStatus);
            this.tabPage3.Controls.Add(this.grasslandProgressBar);
            this.tabPage3.Controls.Add(this.grasslandStepLabel);
            this.tabPage3.Font = groupTitleFont;
            this.tabPage3.ForeColor = defaultForeColor;
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage3.Size = new System.Drawing.Size(842, 550);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "🌿 草地资源资产清查";
            this.tabPage3.UseVisualStyleBackColor = true;

            // 草地按钮
            this.btnGrasslandExtractScope.Font = defaultFont;
            this.btnGrasslandExtractScope.Location = new System.Drawing.Point(20, 30);
            this.btnGrasslandExtractScope.Name = "btnGrasslandExtractScope";
            this.btnGrasslandExtractScope.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandExtractScope.TabIndex = 0;
            this.btnGrasslandExtractScope.Text = "1. 提取草地工作范围";
            this.btnGrasslandExtractScope.UseVisualStyleBackColor = true;
            this.btnGrasslandExtractScope.Click += new System.EventHandler(this.BtnGrasslandExtractScope_Click);

            this.btnGrasslandCreateBasemapLinkPrice.Font = defaultFont;
            this.btnGrasslandCreateBasemapLinkPrice.Location = new System.Drawing.Point(220, 30);
            this.btnGrasslandCreateBasemapLinkPrice.Name = "btnGrasslandCreateBasemapLinkPrice";
            this.btnGrasslandCreateBasemapLinkPrice.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCreateBasemapLinkPrice.TabIndex = 1;
            this.btnGrasslandCreateBasemapLinkPrice.Text = "2. 草地底图与价格关联";
            this.btnGrasslandCreateBasemapLinkPrice.UseVisualStyleBackColor = true;
            this.btnGrasslandCreateBasemapLinkPrice.Click += new System.EventHandler(this.BtnGrasslandCreateBasemapLinkPrice_Click);

            this.btnGrasslandSupplementPrice.Font = defaultFont;
            this.btnGrasslandSupplementPrice.Location = new System.Drawing.Point(420, 30);
            this.btnGrasslandSupplementPrice.Name = "btnGrasslandSupplementPrice";
            this.btnGrasslandSupplementPrice.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandSupplementPrice.TabIndex = 2;
            this.btnGrasslandSupplementPrice.Text = "3. 补充草地基准价格";
            this.btnGrasslandSupplementPrice.UseVisualStyleBackColor = true;
            this.btnGrasslandSupplementPrice.Click += new System.EventHandler(this.BtnGrasslandSupplementPrice_Click);

            this.btnGrasslandCalculateValue.Font = defaultFont;
            this.btnGrasslandCalculateValue.Location = new System.Drawing.Point(20, 75);
            this.btnGrasslandCalculateValue.Name = "btnGrasslandCalculateValue";
            this.btnGrasslandCalculateValue.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCalculateValue.TabIndex = 3;
            this.btnGrasslandCalculateValue.Text = "4. 计算草地资产价值";
            this.btnGrasslandCalculateValue.UseVisualStyleBackColor = true;
            this.btnGrasslandCalculateValue.Click += new System.EventHandler(this.BtnGrasslandCalculateValue_Click);

            this.btnGrasslandCleanQA.Font = defaultFont;
            this.btnGrasslandCleanQA.Location = new System.Drawing.Point(220, 75);
            this.btnGrasslandCleanQA.Name = "btnGrasslandCleanQA";
            this.btnGrasslandCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCleanQA.TabIndex = 4;
            this.btnGrasslandCleanQA.Text = "5. 草地数据清洗与质检";
            this.btnGrasslandCleanQA.UseVisualStyleBackColor = true;
            this.btnGrasslandCleanQA.Click += new System.EventHandler(this.BtnGrasslandCleanQA_Click);

            this.btnGrasslandBuildDBTables.Font = defaultFont;
            this.btnGrasslandBuildDBTables.Location = new System.Drawing.Point(420, 75);
            this.btnGrasslandBuildDBTables.Name = "btnGrasslandBuildDBTables";
            this.btnGrasslandBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandBuildDBTables.TabIndex = 5;
            this.btnGrasslandBuildDBTables.Text = "6. 草地库表构建";
            this.btnGrasslandBuildDBTables.UseVisualStyleBackColor = true;
            this.btnGrasslandBuildDBTables.Click += new System.EventHandler(this.BtnGrasslandBuildDBTables_Click);

            this.lblGrasslandProcessingStatus.Font = defaultFont;
            this.lblGrasslandProcessingStatus.Location = new System.Drawing.Point(20, 120);
            this.lblGrasslandProcessingStatus.Name = "lblGrasslandProcessingStatus";
            this.lblGrasslandProcessingStatus.Size = new System.Drawing.Size(810, 25);
            this.lblGrasslandProcessingStatus.TabIndex = 6;
            this.lblGrasslandProcessingStatus.Text = "等待草地资源清查处理";
            this.lblGrasslandProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 草地进度条和步骤标签
            this.grasslandProgressBar.Location = new System.Drawing.Point(600, 75);
            this.grasslandProgressBar.Name = "grasslandProgressBar";
            this.grasslandProgressBar.Size = new System.Drawing.Size(230, 20);
            this.grasslandProgressBar.TabIndex = 7;

            this.grasslandStepLabel.AutoSize = true;
            this.grasslandStepLabel.Font = defaultFont;
            this.grasslandStepLabel.Location = new System.Drawing.Point(600, 35);
            this.grasslandStepLabel.Name = "grasslandStepLabel";
            this.grasslandStepLabel.Size = new System.Drawing.Size(104, 15);
            this.grasslandStepLabel.TabIndex = 8;
            this.grasslandStepLabel.Text = "已完成 0/6 步骤";

            // 湿地资源选项卡
            this.tabPage4.Controls.Add(this.btnWetlandExtractScopeBasemap);
            this.tabPage4.Controls.Add(this.btnWetlandCleanQA);
            this.tabPage4.Controls.Add(this.btnWetlandBuildDBTables);
            this.tabPage4.Controls.Add(this.lblWetlandProcessingStatus);
            this.tabPage4.Controls.Add(this.wetlandProgressBar);
            this.tabPage4.Controls.Add(this.wetlandStepLabel);
            this.tabPage4.Font = groupTitleFont;
            this.tabPage4.ForeColor = defaultForeColor;
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage4.Size = new System.Drawing.Size(842, 550);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "🏞️ 湿地资源资产清查";
            this.tabPage4.UseVisualStyleBackColor = true;

            // 湿地按钮
            this.btnWetlandExtractScopeBasemap.Font = defaultFont;
            this.btnWetlandExtractScopeBasemap.Location = new System.Drawing.Point(20, 30);
            this.btnWetlandExtractScopeBasemap.Name = "btnWetlandExtractScopeBasemap";
            this.btnWetlandExtractScopeBasemap.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandExtractScopeBasemap.TabIndex = 0;
            this.btnWetlandExtractScopeBasemap.Text = "1. 湿地范围与底图制作";
            this.btnWetlandExtractScopeBasemap.UseVisualStyleBackColor = true;
            this.btnWetlandExtractScopeBasemap.Click += new System.EventHandler(this.BtnWetlandExtractScopeBasemap_Click);

            this.btnWetlandCleanQA.Font = defaultFont;
            this.btnWetlandCleanQA.Location = new System.Drawing.Point(220, 30);
            this.btnWetlandCleanQA.Name = "btnWetlandCleanQA";
            this.btnWetlandCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandCleanQA.TabIndex = 1;
            this.btnWetlandCleanQA.Text = "2. 湿地数据清洗与质检";
            this.btnWetlandCleanQA.UseVisualStyleBackColor = true;
            this.btnWetlandCleanQA.Click += new System.EventHandler(this.BtnWetlandCleanQA_Click);

            this.btnWetlandBuildDBTables.Font = defaultFont;
            this.btnWetlandBuildDBTables.Location = new System.Drawing.Point(420, 30);
            this.btnWetlandBuildDBTables.Name = "btnWetlandBuildDBTables";
            this.btnWetlandBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandBuildDBTables.TabIndex = 2;
            this.btnWetlandBuildDBTables.Text = "3. 湿地库表构建";
            this.btnWetlandBuildDBTables.UseVisualStyleBackColor = true;
            this.btnWetlandBuildDBTables.Click += new System.EventHandler(this.BtnWetlandBuildDBTables_Click);

            this.lblWetlandProcessingStatus.Font = defaultFont;
            this.lblWetlandProcessingStatus.Location = new System.Drawing.Point(20, 75);
            this.lblWetlandProcessingStatus.Name = "lblWetlandProcessingStatus";
            this.lblWetlandProcessingStatus.Size = new System.Drawing.Size(810, 25);
            this.lblWetlandProcessingStatus.TabIndex = 3;
            this.lblWetlandProcessingStatus.Text = "等待湿地资源清查处理";
            this.lblWetlandProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 湿地进度条和步骤标签
            this.wetlandProgressBar.Location = new System.Drawing.Point(600, 30);
            this.wetlandProgressBar.Name = "wetlandProgressBar";
            this.wetlandProgressBar.Size = new System.Drawing.Size(230, 20);
            this.wetlandProgressBar.TabIndex = 4;

            this.wetlandStepLabel.AutoSize = true;
            this.wetlandStepLabel.Font = defaultFont;
            this.wetlandStepLabel.Location = new System.Drawing.Point(600, 10);
            this.wetlandStepLabel.Name = "wetlandStepLabel";
            this.wetlandStepLabel.Size = new System.Drawing.Size(104, 15);
            this.wetlandStepLabel.TabIndex = 5;
            this.wetlandStepLabel.Text = "已完成 0/3 步骤";

            // 综合输出选项卡
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
            this.tabPage5.Font = groupTitleFont;
            this.tabPage5.ForeColor = defaultForeColor;
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage5.Size = new System.Drawing.Size(842, 550);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "📊 综合质检、统计与成果输出";
            this.tabPage5.UseVisualStyleBackColor = true;

            // 输出按钮
            this.btnOverallQualityCheck.Font = defaultFont;
            this.btnOverallQualityCheck.Location = new System.Drawing.Point(20, 30);
            this.btnOverallQualityCheck.Name = "btnOverallQualityCheck";
            this.btnOverallQualityCheck.Size = new System.Drawing.Size(135, 30);
            this.btnOverallQualityCheck.TabIndex = 0;
            this.btnOverallQualityCheck.Text = "1. 综合质量检查";
            this.btnOverallQualityCheck.UseVisualStyleBackColor = true;
            this.btnOverallQualityCheck.Click += new System.EventHandler(this.BtnOverallQualityCheck_Click);

            this.btnStatisticalAggregation.Font = defaultFont;
            this.btnStatisticalAggregation.Location = new System.Drawing.Point(190, 30);
            this.btnStatisticalAggregation.Name = "btnStatisticalAggregation";
            this.btnStatisticalAggregation.Size = new System.Drawing.Size(135, 30);
            this.btnStatisticalAggregation.TabIndex = 1;
            this.btnStatisticalAggregation.Text = "2. 数据统计汇总";
            this.btnStatisticalAggregation.UseVisualStyleBackColor = true;
            this.btnStatisticalAggregation.Click += new System.EventHandler(this.BtnStatisticalAggregation_Click);

            this.btnDataAnalysis.Font = defaultFont;
            this.btnDataAnalysis.Location = new System.Drawing.Point(360, 30);
            this.btnDataAnalysis.Name = "btnDataAnalysis";
            this.btnDataAnalysis.Size = new System.Drawing.Size(135, 30);
            this.btnDataAnalysis.TabIndex = 2;
            this.btnDataAnalysis.Text = "3. 数据分析与挖掘";
            this.btnDataAnalysis.UseVisualStyleBackColor = true;
            this.btnDataAnalysis.Click += new System.EventHandler(this.BtnDataAnalysis_Click);

            this.btnExportDatasetDB.Font = defaultFont;
            this.btnExportDatasetDB.Location = new System.Drawing.Point(20, 75);
            this.btnExportDatasetDB.Name = "btnExportDatasetDB";
            this.btnExportDatasetDB.Size = new System.Drawing.Size(135, 30);
            this.btnExportDatasetDB.TabIndex = 3;
            this.btnExportDatasetDB.Text = "4. 导出清查数据集";
            this.btnExportDatasetDB.UseVisualStyleBackColor = true;
            this.btnExportDatasetDB.Click += new System.EventHandler(this.BtnExportDatasetDB_Click);

            this.btnExportSummaryTables.Font = defaultFont;
            this.btnExportSummaryTables.Location = new System.Drawing.Point(190, 75);
            this.btnExportSummaryTables.Name = "btnExportSummaryTables";
            this.btnExportSummaryTables.Size = new System.Drawing.Size(135, 30);
            this.btnExportSummaryTables.TabIndex = 4;
            this.btnExportSummaryTables.Text = "5. 导出汇总表";
            this.btnExportSummaryTables.UseVisualStyleBackColor = true;
            this.btnExportSummaryTables.Click += new System.EventHandler(this.BtnExportSummaryTables_Click);

            this.btnGenerateReport.Font = defaultFont;
            this.btnGenerateReport.Location = new System.Drawing.Point(360, 75);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(135, 30);
            this.btnGenerateReport.TabIndex = 5;
            this.btnGenerateReport.Text = "6. 生成成果报告";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.BtnGenerateReport_Click);

            this.btnGenerateThematicMaps.Font = defaultFont;
            this.btnGenerateThematicMaps.Location = new System.Drawing.Point(530, 75);
            this.btnGenerateThematicMaps.Name = "btnGenerateThematicMaps";
            this.btnGenerateThematicMaps.Size = new System.Drawing.Size(135, 30);
            this.btnGenerateThematicMaps.TabIndex = 6;
            this.btnGenerateThematicMaps.Text = "7. 生成专题图";
            this.btnGenerateThematicMaps.UseVisualStyleBackColor = true;
            this.btnGenerateThematicMaps.Click += new System.EventHandler(this.BtnGenerateThematicMaps_Click);

            this.lblFinalOutputStatus.Font = defaultFont;
            this.lblFinalOutputStatus.Location = new System.Drawing.Point(20, 120);
            this.lblFinalOutputStatus.Name = "lblFinalOutputStatus";
            this.lblFinalOutputStatus.Size = new System.Drawing.Size(810, 25);
            this.lblFinalOutputStatus.TabIndex = 7;
            this.lblFinalOutputStatus.Text = "等待最终成果处理";
            this.lblFinalOutputStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 输出进度条和步骤标签
            this.outputProgressBar.Location = new System.Drawing.Point(600, 30);
            this.outputProgressBar.Name = "outputProgressBar";
            this.outputProgressBar.Size = new System.Drawing.Size(230, 20);
            this.outputProgressBar.TabIndex = 8;

            this.outputStepLabel.AutoSize = true;
            this.outputStepLabel.Font = defaultFont;
            this.outputStepLabel.Location = new System.Drawing.Point(600, 10);
            this.outputStepLabel.Name = "outputStepLabel";
            this.outputStepLabel.Size = new System.Drawing.Size(104, 15);
            this.outputStepLabel.TabIndex = 9;
            this.outputStepLabel.Text = "已完成 0/7 步骤";

            // 底部面板
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.lblProgress);
            this.bottomPanel.Controls.Add(this.btnHelp);
            this.bottomPanel.Controls.Add(this.btnClose);
            this.bottomPanel.Location = new System.Drawing.Point(15, 645);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(4);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(850, 60);
            this.bottomPanel.TabIndex = 6;

            // 进度标签
            this.lblProgress.Font = defaultFont;
            this.lblProgress.Location = new System.Drawing.Point(10, 18);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(600, 25);
            this.lblProgress.TabIndex = 7;
            this.lblProgress.Text = "进度：等待开始处理";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 帮助按钮
            this.btnHelp.Font = defaultFont;
            this.btnHelp.Location = new System.Drawing.Point(650, 15);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(70, 25);
            this.btnHelp.TabIndex = 0;
            this.btnHelp.Text = "帮助";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);

            // 关闭按钮
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = defaultFont;
            this.btnClose.Location = new System.Drawing.Point(750, 15);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 25);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            // 设置主面板和窗体大小
            this.mainPanel.Size = new System.Drawing.Size(880, 720);

            // MainProcessingTabsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 720);
            this.Controls.Add(this.mainPanel);
            this.Font = defaultFont;
            this.ForeColor = defaultForeColor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainProcessingTabsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "广东省全民所有自然资源资产清查工具 - 主处理";

            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.forestResourceChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.forestWorkflowImage)).EndInit();
            this.ResumeLayout(false);
        }
    }
}