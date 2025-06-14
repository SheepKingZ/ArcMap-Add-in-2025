using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestArcMapAddin2.Components;
using TestArcMapAddin2.Forms;

namespace TestArcMapAddin2.Forms
{
    public partial class MainProcessForm : Form
    {
        private string _workspacePath;
        private List<string> _selectedCounties;

        // 控件需要定义为私有字段
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button btnSelectWorkspace;
        private System.Windows.Forms.Button btnSelectCounties;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnHelp;

        private Button btnCreateCountyEmptyTables;
        private Label lblCountyEmptyTablesStatus;
        private Label lblWorkspace;
        private Label lblCounties;
        private Label lblPrerequisiteDataStatus;
        private Label lblForestProcessingStatus;
        private Label lblGrasslandProcessingStatus;
        private Label lblWetlandProcessingStatus;
        private Label lblFinalOutputStatus;
        private Label lblProgress;
        private Button btnLoadPrerequisiteData;
        private Button btnForestExtractScope;
        private Button btnForestCreateBasemapLinkPrice;
        private Button btnForestSupplementPrice;
        private Button btnForestCalculateValue;
        private Button btnForestCleanQA;
        private Button btnForestBuildDBTables;
        private Button btnGrasslandExtractScope;
        private Button btnGrasslandCreateBasemapLinkPrice;
        private Button btnGrasslandSupplementPrice;
        private Button btnGrasslandCalculateValue;
        private Button btnGrasslandCleanQA;
        private Button btnGrasslandBuildDBTables;
        private Button btnWetlandExtractScopeBasemap;
        private Button btnWetlandCleanQA;
        private Button btnWetlandBuildDBTables;
        private Button btnOverallQualityCheck;
        private Button btnStatisticalAggregation;
        private Button btnDataAnalysis;
        private Button btnExportDatasetDB;
        private Button btnExportSummaryTables;
        private Button btnGenerateReport;
        private Button btnGenerateThematicMaps;

        // 设计器要求的变量
        private System.ComponentModel.IContainer components = null;

        public MainProcessForm()
        {
            InitializeComponent();
            InitializeFormState();
            this.Load += MainProcessForm_Load;
        }

        // Dispose 方法对于设计器支持是必需的
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();

            this.btnSelectWorkspace = new System.Windows.Forms.Button();
            this.lblWorkspace = new System.Windows.Forms.Label();
            this.btnSelectCounties = new System.Windows.Forms.Button();
            this.lblCounties = new System.Windows.Forms.Label();
            this.btnLoadPrerequisiteData = new System.Windows.Forms.Button();
            this.lblPrerequisiteDataStatus = new System.Windows.Forms.Label();
            this.btnCreateCountyEmptyTables = new System.Windows.Forms.Button();
            this.lblCountyEmptyTablesStatus = new System.Windows.Forms.Label();
            this.btnForestExtractScope = new System.Windows.Forms.Button();
            this.btnForestCreateBasemapLinkPrice = new System.Windows.Forms.Button();
            this.btnForestSupplementPrice = new System.Windows.Forms.Button();
            this.btnForestCalculateValue = new System.Windows.Forms.Button();
            this.btnForestCleanQA = new System.Windows.Forms.Button();
            this.btnForestBuildDBTables = new System.Windows.Forms.Button();
            this.lblForestProcessingStatus = new System.Windows.Forms.Label();
            this.btnGrasslandExtractScope = new System.Windows.Forms.Button();
            this.btnGrasslandCreateBasemapLinkPrice = new System.Windows.Forms.Button();
            this.btnGrasslandSupplementPrice = new System.Windows.Forms.Button();
            this.btnGrasslandCalculateValue = new System.Windows.Forms.Button();
            this.btnGrasslandCleanQA = new System.Windows.Forms.Button();
            this.btnGrasslandBuildDBTables = new System.Windows.Forms.Button();
            this.lblGrasslandProcessingStatus = new System.Windows.Forms.Label();
            this.btnWetlandExtractScopeBasemap = new System.Windows.Forms.Button();
            this.btnWetlandCleanQA = new System.Windows.Forms.Button();
            this.btnWetlandBuildDBTables = new System.Windows.Forms.Button();
            this.lblWetlandProcessingStatus = new System.Windows.Forms.Label();
            this.btnOverallQualityCheck = new System.Windows.Forms.Button();
            this.btnStatisticalAggregation = new System.Windows.Forms.Button();
            this.btnDataAnalysis = new System.Windows.Forms.Button();
            this.btnExportDatasetDB = new System.Windows.Forms.Button();
            this.btnExportSummaryTables = new System.Windows.Forms.Button();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnGenerateThematicMaps = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblFinalOutputStatus = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();

            this.mainPanel.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();

            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = false; // Changed from true
            this.mainPanel.Controls.Add(this.titleLabel);
            this.mainPanel.Controls.Add(this.tabControlMain); // Added
            this.mainPanel.Controls.Add(this.bottomPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(15); // Adjusted from 22
            // Size will be set later based on content
            this.mainPanel.TabIndex = 0;

            // 
            // titleLabel
            // 
            this.titleLabel.Font = formTitleFont;
            this.titleLabel.ForeColor = defaultForeColor;
            this.titleLabel.Location = new System.Drawing.Point(15, 15); 
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(850, 30); // Original: 1275, 45. Adjusted for typical screen.
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "广东省全民所有自然资源（森林、草地、湿地）资产清查工具";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPage1);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Controls.Add(this.tabPage3);
            this.tabControlMain.Controls.Add(this.tabPage4);
            this.tabControlMain.Controls.Add(this.tabPage5);
            this.tabControlMain.Font = defaultFont;
            this.tabControlMain.ItemSize = new System.Drawing.Size(100, 22); // Adjust tab header size
            this.tabControlMain.Location = new System.Drawing.Point(15, titleLabel.Bottom + 10);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(850, 360); // Adjusted: Original group1 height 205, group5 height 200. Max content height ~250. TabControl height ~250 + 30 (header) + padding.
            this.tabControlMain.TabIndex = 1;

            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSelectWorkspace);
            this.tabPage1.Controls.Add(this.lblWorkspace);
            this.tabPage1.Controls.Add(this.btnSelectCounties);
            this.tabPage1.Controls.Add(this.lblCounties);
            this.tabPage1.Controls.Add(this.btnLoadPrerequisiteData);
            this.tabPage1.Controls.Add(this.lblPrerequisiteDataStatus);
            this.tabPage1.Controls.Add(this.btnCreateCountyEmptyTables);
            this.tabPage1.Controls.Add(this.lblCountyEmptyTablesStatus);
            this.tabPage1.Font = groupTitleFont; // Use groupTitleFont for TabPage content consistency if desired, or defaultFont
            this.tabPage1.ForeColor = defaultForeColor;
            this.tabPage1.Location = new System.Drawing.Point(4, 26); // (4, ItemSize.Height + few pixels)
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(10); // Consistent padding
            this.tabPage1.Size = new System.Drawing.Size(842, 330); // tabControlMain.ClientSize - padding for tab page itself
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "📁 基础数据准备";
            this.tabPage1.UseVisualStyleBackColor = true;

            // 
            // btnSelectWorkspace
            // 
            this.btnSelectWorkspace.Font = defaultFont;
            this.btnSelectWorkspace.ForeColor = defaultForeColor;
            this.btnSelectWorkspace.Location = new System.Drawing.Point(20, 30); // Original location from group1
            this.btnSelectWorkspace.Name = "btnSelectWorkspace";
            this.btnSelectWorkspace.Size = new System.Drawing.Size(110, 30); // Original size
            this.btnSelectWorkspace.TabIndex = 0;
            this.btnSelectWorkspace.Text = "选择工作空间";
            this.btnSelectWorkspace.UseVisualStyleBackColor = true;
            this.btnSelectWorkspace.Click += new System.EventHandler(this.BtnSelectWorkspace_Click);
            // 
            // lblWorkspace
            // 
            this.lblWorkspace.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblWorkspace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWorkspace.Font = defaultFont;
            this.lblWorkspace.ForeColor = defaultForeColor;
            this.lblWorkspace.Location = new System.Drawing.Point(160, 30); // Original location
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(400, 35); // Original size
            this.lblWorkspace.TabIndex = 1;
            this.lblWorkspace.Text = "未选择工作空间";
            this.lblWorkspace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSelectCounties
            // 
            this.btnSelectCounties.Font = defaultFont;
            this.btnSelectCounties.ForeColor = defaultForeColor;
            this.btnSelectCounties.Location = new System.Drawing.Point(580, 30); // Original location
            this.btnSelectCounties.Name = "btnSelectCounties";
            this.btnSelectCounties.Size = new System.Drawing.Size(90, 30); // Original size
            this.btnSelectCounties.TabIndex = 2;
            this.btnSelectCounties.Text = "选择县区";
            this.btnSelectCounties.UseVisualStyleBackColor = true;
            this.btnSelectCounties.Click += new System.EventHandler(this.BtnSelectCounties_Click);
            // 
            // lblCounties
            // 
            this.lblCounties.Font = defaultFont;
            this.lblCounties.ForeColor = defaultForeColor;
            this.lblCounties.Location = new System.Drawing.Point(20, 75); // Original location
            this.lblCounties.Name = "lblCounties";
            this.lblCounties.Size = new System.Drawing.Size(810, 25); // Original size
            this.lblCounties.TabIndex = 3;
            this.lblCounties.Text = "未选择县区";
            this.lblCounties.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLoadPrerequisiteData
            // 
            this.btnLoadPrerequisiteData.Enabled = false;
            this.btnLoadPrerequisiteData.Font = defaultFont;
            this.btnLoadPrerequisiteData.ForeColor = defaultForeColor;
            this.btnLoadPrerequisiteData.Location = new System.Drawing.Point(20, 110); // Original location
            this.btnLoadPrerequisiteData.Name = "btnLoadPrerequisiteData";
            this.btnLoadPrerequisiteData.Size = new System.Drawing.Size(110, 30); // Original size
            this.btnLoadPrerequisiteData.TabIndex = 4;
            this.btnLoadPrerequisiteData.Text = "加载前提数据";
            this.btnLoadPrerequisiteData.UseVisualStyleBackColor = true;
            this.btnLoadPrerequisiteData.Click += new System.EventHandler(this.BtnLoadPrerequisiteData_Click);
            // 
            // lblPrerequisiteDataStatus
            // 
            this.lblPrerequisiteDataStatus.Font = defaultFont;
            this.lblPrerequisiteDataStatus.ForeColor = defaultForeColor;
            this.lblPrerequisiteDataStatus.Location = new System.Drawing.Point(160, 110); // Original location
            this.lblPrerequisiteDataStatus.Name = "lblPrerequisiteDataStatus";
            this.lblPrerequisiteDataStatus.Size = new System.Drawing.Size(670, 35); // Original size
            this.lblPrerequisiteDataStatus.TabIndex = 5;
            this.lblPrerequisiteDataStatus.Text = "未加载前提数据";
            this.lblPrerequisiteDataStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCreateCountyEmptyTables
            // 
            this.btnCreateCountyEmptyTables.Enabled = false;
            this.btnCreateCountyEmptyTables.Font = defaultFont;
            this.btnCreateCountyEmptyTables.ForeColor = defaultForeColor;
            this.btnCreateCountyEmptyTables.Location = new System.Drawing.Point(20, 150); // Original location
            this.btnCreateCountyEmptyTables.Name = "btnCreateCountyEmptyTables";
            this.btnCreateCountyEmptyTables.Size = new System.Drawing.Size(135, 30); // Original size
            this.btnCreateCountyEmptyTables.TabIndex = 6;
            this.btnCreateCountyEmptyTables.Text = "创建县级空表";
            this.btnCreateCountyEmptyTables.UseVisualStyleBackColor = true;
            this.btnCreateCountyEmptyTables.Click += new System.EventHandler(this.BtnCreateCountyEmptyTables_Click);
            // 
            // lblCountyEmptyTablesStatus
            // 
            this.lblCountyEmptyTablesStatus.Font = defaultFont;
            this.lblCountyEmptyTablesStatus.ForeColor = defaultForeColor;
            this.lblCountyEmptyTablesStatus.Location = new System.Drawing.Point(190, 150); // Original location
            this.lblCountyEmptyTablesStatus.Name = "lblCountyEmptyTablesStatus";
            this.lblCountyEmptyTablesStatus.Size = new System.Drawing.Size(640, 35); // Original size
            this.lblCountyEmptyTablesStatus.TabIndex = 7;
            this.lblCountyEmptyTablesStatus.Text = "未创建县级空表";
            this.lblCountyEmptyTablesStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnForestExtractScope);
            this.tabPage2.Controls.Add(this.btnForestCreateBasemapLinkPrice);
            this.tabPage2.Controls.Add(this.btnForestSupplementPrice);
            this.tabPage2.Controls.Add(this.btnForestCalculateValue);
            this.tabPage2.Controls.Add(this.btnForestCleanQA);
            this.tabPage2.Controls.Add(this.btnForestBuildDBTables);
            this.tabPage2.Controls.Add(this.lblForestProcessingStatus);
            this.tabPage2.ForeColor = defaultForeColor;
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage2.Size = new System.Drawing.Size(842, 330);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "🌲 森林资源资产清查";
            this.tabPage2.UseVisualStyleBackColor = true;

            // Controls for tabPage2 (originally in group2)
            // btnForestExtractScope
            this.btnForestExtractScope.Enabled = false;
            this.btnForestExtractScope.Font = defaultFont;
            this.btnForestExtractScope.ForeColor = defaultForeColor;
            this.btnForestExtractScope.Location = new System.Drawing.Point(20, 30);
            this.btnForestExtractScope.Name = "btnForestExtractScope";
            this.btnForestExtractScope.Size = new System.Drawing.Size(160, 30);
            this.btnForestExtractScope.TabIndex = 0;
            this.btnForestExtractScope.Text = "提取森林工作范围";
            this.btnForestExtractScope.UseVisualStyleBackColor = true;
            this.btnForestExtractScope.Click += new System.EventHandler(this.BtnForestExtractScope_Click);
            // ... (similarly for other controls in group2, using their original Locations and Sizes)

            this.btnForestCreateBasemapLinkPrice.Enabled = false;
            this.btnForestCreateBasemapLinkPrice.Font = defaultFont;
            this.btnForestCreateBasemapLinkPrice.ForeColor = defaultForeColor;
            this.btnForestCreateBasemapLinkPrice.Location = new System.Drawing.Point(220, 30);
            this.btnForestCreateBasemapLinkPrice.Name = "btnForestCreateBasemapLinkPrice";
            this.btnForestCreateBasemapLinkPrice.Size = new System.Drawing.Size(160, 30);
            this.btnForestCreateBasemapLinkPrice.TabIndex = 1;
            this.btnForestCreateBasemapLinkPrice.Text = "森林底图与价格关联";
            this.btnForestCreateBasemapLinkPrice.UseVisualStyleBackColor = true;
            this.btnForestCreateBasemapLinkPrice.Click += new System.EventHandler(this.BtnForestCreateBasemapLinkPrice_Click);

            this.btnForestSupplementPrice.Enabled = false;
            this.btnForestSupplementPrice.Font = defaultFont;
            this.btnForestSupplementPrice.ForeColor = defaultForeColor;
            this.btnForestSupplementPrice.Location = new System.Drawing.Point(420, 30);
            this.btnForestSupplementPrice.Name = "btnForestSupplementPrice";
            this.btnForestSupplementPrice.Size = new System.Drawing.Size(160, 30);
            this.btnForestSupplementPrice.TabIndex = 2;
            this.btnForestSupplementPrice.Text = "补充森林基准价格";
            this.btnForestSupplementPrice.UseVisualStyleBackColor = true;
            this.btnForestSupplementPrice.Click += new System.EventHandler(this.BtnForestSupplementPrice_Click);

            this.btnForestCalculateValue.Enabled = false;
            this.btnForestCalculateValue.Font = defaultFont;
            this.btnForestCalculateValue.ForeColor = defaultForeColor;
            this.btnForestCalculateValue.Location = new System.Drawing.Point(20, 75);
            this.btnForestCalculateValue.Name = "btnForestCalculateValue";
            this.btnForestCalculateValue.Size = new System.Drawing.Size(160, 30);
            this.btnForestCalculateValue.TabIndex = 3;
            this.btnForestCalculateValue.Text = "计算森林资产价值";
            this.btnForestCalculateValue.UseVisualStyleBackColor = true;
            this.btnForestCalculateValue.Click += new System.EventHandler(this.BtnForestCalculateValue_Click);

            this.btnForestCleanQA.Enabled = false;
            this.btnForestCleanQA.Font = defaultFont;
            this.btnForestCleanQA.ForeColor = defaultForeColor;
            this.btnForestCleanQA.Location = new System.Drawing.Point(220, 75);
            this.btnForestCleanQA.Name = "btnForestCleanQA";
            this.btnForestCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnForestCleanQA.TabIndex = 4;
            this.btnForestCleanQA.Text = "森林数据清洗与质检";
            this.btnForestCleanQA.UseVisualStyleBackColor = true;
            this.btnForestCleanQA.Click += new System.EventHandler(this.BtnForestCleanQA_Click);

            this.btnForestBuildDBTables.Enabled = false;
            this.btnForestBuildDBTables.Font = defaultFont;
            this.btnForestBuildDBTables.ForeColor = defaultForeColor;
            this.btnForestBuildDBTables.Location = new System.Drawing.Point(420, 75);
            this.btnForestBuildDBTables.Name = "btnForestBuildDBTables";
            this.btnForestBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnForestBuildDBTables.TabIndex = 5;
            this.btnForestBuildDBTables.Text = "森林库表构建";
            this.btnForestBuildDBTables.UseVisualStyleBackColor = true;
            this.btnForestBuildDBTables.Click += new System.EventHandler(this.BtnForestBuildDBTables_Click);

            this.lblForestProcessingStatus.Font = defaultFont;
            this.lblForestProcessingStatus.ForeColor = defaultForeColor;
            this.lblForestProcessingStatus.Location = new System.Drawing.Point(20, 120);
            this.lblForestProcessingStatus.Name = "lblForestProcessingStatus";
            this.lblForestProcessingStatus.Size = new System.Drawing.Size(810, 25);
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
            this.tabPage3.ForeColor = defaultForeColor;
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage3.Size = new System.Drawing.Size(842, 330);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "🌿 草地资源资产清查";
            this.tabPage3.UseVisualStyleBackColor = true;
            
            // Controls for tabPage3 (originally in group3)
            this.btnGrasslandExtractScope.Enabled = false;
            this.btnGrasslandExtractScope.Font = defaultFont;
            this.btnGrasslandExtractScope.ForeColor = defaultForeColor;
            this.btnGrasslandExtractScope.Location = new System.Drawing.Point(20, 30);
            this.btnGrasslandExtractScope.Name = "btnGrasslandExtractScope";
            this.btnGrasslandExtractScope.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandExtractScope.TabIndex = 0;
            this.btnGrasslandExtractScope.Text = "提取草地工作范围";
            this.btnGrasslandExtractScope.UseVisualStyleBackColor = true;
            this.btnGrasslandExtractScope.Click += new System.EventHandler(this.BtnGrasslandExtractScope_Click);
            // ... (similarly for other controls in group3)

            this.btnGrasslandCreateBasemapLinkPrice.Enabled = false;
            this.btnGrasslandCreateBasemapLinkPrice.Font = defaultFont;
            this.btnGrasslandCreateBasemapLinkPrice.ForeColor = defaultForeColor;
            this.btnGrasslandCreateBasemapLinkPrice.Location = new System.Drawing.Point(220, 30);
            this.btnGrasslandCreateBasemapLinkPrice.Name = "btnGrasslandCreateBasemapLinkPrice";
            this.btnGrasslandCreateBasemapLinkPrice.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCreateBasemapLinkPrice.TabIndex = 1;
            this.btnGrasslandCreateBasemapLinkPrice.Text = "草地底图与价格关联";
            this.btnGrasslandCreateBasemapLinkPrice.UseVisualStyleBackColor = true;
            this.btnGrasslandCreateBasemapLinkPrice.Click += new System.EventHandler(this.BtnGrasslandCreateBasemapLinkPrice_Click);

            this.btnGrasslandSupplementPrice.Enabled = false;
            this.btnGrasslandSupplementPrice.Font = defaultFont;
            this.btnGrasslandSupplementPrice.ForeColor = defaultForeColor;
            this.btnGrasslandSupplementPrice.Location = new System.Drawing.Point(420, 30);
            this.btnGrasslandSupplementPrice.Name = "btnGrasslandSupplementPrice";
            this.btnGrasslandSupplementPrice.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandSupplementPrice.TabIndex = 2;
            this.btnGrasslandSupplementPrice.Text = "补充草地基准价格";
            this.btnGrasslandSupplementPrice.UseVisualStyleBackColor = true;
            this.btnGrasslandSupplementPrice.Click += new System.EventHandler(this.BtnGrasslandSupplementPrice_Click);

            this.btnGrasslandCalculateValue.Enabled = false;
            this.btnGrasslandCalculateValue.Font = defaultFont;
            this.btnGrasslandCalculateValue.ForeColor = defaultForeColor;
            this.btnGrasslandCalculateValue.Location = new System.Drawing.Point(20, 75);
            this.btnGrasslandCalculateValue.Name = "btnGrasslandCalculateValue";
            this.btnGrasslandCalculateValue.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCalculateValue.TabIndex = 3;
            this.btnGrasslandCalculateValue.Text = "计算草地资产价值";
            this.btnGrasslandCalculateValue.UseVisualStyleBackColor = true;
            this.btnGrasslandCalculateValue.Click += new System.EventHandler(this.BtnGrasslandCalculateValue_Click);

            this.btnGrasslandCleanQA.Enabled = false;
            this.btnGrasslandCleanQA.Font = defaultFont;
            this.btnGrasslandCleanQA.ForeColor = defaultForeColor;
            this.btnGrasslandCleanQA.Location = new System.Drawing.Point(220, 75);
            this.btnGrasslandCleanQA.Name = "btnGrasslandCleanQA";
            this.btnGrasslandCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandCleanQA.TabIndex = 4;
            this.btnGrasslandCleanQA.Text = "草地数据清洗与质检";
            this.btnGrasslandCleanQA.UseVisualStyleBackColor = true;
            this.btnGrasslandCleanQA.Click += new System.EventHandler(this.BtnGrasslandCleanQA_Click);

            this.btnGrasslandBuildDBTables.Enabled = false;
            this.btnGrasslandBuildDBTables.Font = defaultFont;
            this.btnGrasslandBuildDBTables.ForeColor = defaultForeColor;
            this.btnGrasslandBuildDBTables.Location = new System.Drawing.Point(420, 75);
            this.btnGrasslandBuildDBTables.Name = "btnGrasslandBuildDBTables";
            this.btnGrasslandBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnGrasslandBuildDBTables.TabIndex = 5;
            this.btnGrasslandBuildDBTables.Text = "草地库表构建";
            this.btnGrasslandBuildDBTables.UseVisualStyleBackColor = true;
            this.btnGrasslandBuildDBTables.Click += new System.EventHandler(this.BtnGrasslandBuildDBTables_Click);

            this.lblGrasslandProcessingStatus.Font = defaultFont;
            this.lblGrasslandProcessingStatus.ForeColor = defaultForeColor;
            this.lblGrasslandProcessingStatus.Location = new System.Drawing.Point(20, 120);
            this.lblGrasslandProcessingStatus.Name = "lblGrasslandProcessingStatus";
            this.lblGrasslandProcessingStatus.Size = new System.Drawing.Size(810, 25);
            this.lblGrasslandProcessingStatus.TabIndex = 6;
            this.lblGrasslandProcessingStatus.Text = "等待草地资源清查处理";
            this.lblGrasslandProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnWetlandExtractScopeBasemap);
            this.tabPage4.Controls.Add(this.btnWetlandCleanQA);
            this.tabPage4.Controls.Add(this.btnWetlandBuildDBTables);
            this.tabPage4.Controls.Add(this.lblWetlandProcessingStatus);
            this.tabPage4.ForeColor = defaultForeColor;
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage4.Size = new System.Drawing.Size(842, 330);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "🏞️ 湿地资源资产清查";
            this.tabPage4.UseVisualStyleBackColor = true;

            // Controls for tabPage4 (originally in group4)
            this.btnWetlandExtractScopeBasemap.Enabled = false;
            this.btnWetlandExtractScopeBasemap.Font = defaultFont;
            this.btnWetlandExtractScopeBasemap.ForeColor = defaultForeColor;
            this.btnWetlandExtractScopeBasemap.Location = new System.Drawing.Point(20, 30);
            this.btnWetlandExtractScopeBasemap.Name = "btnWetlandExtractScopeBasemap";
            this.btnWetlandExtractScopeBasemap.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandExtractScopeBasemap.TabIndex = 0;
            this.btnWetlandExtractScopeBasemap.Text = "湿地范围与底图制作";
            this.btnWetlandExtractScopeBasemap.UseVisualStyleBackColor = true;
            this.btnWetlandExtractScopeBasemap.Click += new System.EventHandler(this.BtnWetlandExtractScopeBasemap_Click);
            // ... (similarly for other controls in group4)

            this.btnWetlandCleanQA.Enabled = false;
            this.btnWetlandCleanQA.Font = defaultFont;
            this.btnWetlandCleanQA.ForeColor = defaultForeColor;
            this.btnWetlandCleanQA.Location = new System.Drawing.Point(220, 30);
            this.btnWetlandCleanQA.Name = "btnWetlandCleanQA";
            this.btnWetlandCleanQA.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandCleanQA.TabIndex = 1;
            this.btnWetlandCleanQA.Text = "湿地数据清洗与质检";
            this.btnWetlandCleanQA.UseVisualStyleBackColor = true;
            this.btnWetlandCleanQA.Click += new System.EventHandler(this.BtnWetlandCleanQA_Click);

            this.btnWetlandBuildDBTables.Enabled = false;
            this.btnWetlandBuildDBTables.Font = defaultFont;
            this.btnWetlandBuildDBTables.ForeColor = defaultForeColor;
            this.btnWetlandBuildDBTables.Location = new System.Drawing.Point(420, 30);
            this.btnWetlandBuildDBTables.Name = "btnWetlandBuildDBTables";
            this.btnWetlandBuildDBTables.Size = new System.Drawing.Size(160, 30);
            this.btnWetlandBuildDBTables.TabIndex = 2;
            this.btnWetlandBuildDBTables.Text = "湿地库表构建";
            this.btnWetlandBuildDBTables.UseVisualStyleBackColor = true;
            this.btnWetlandBuildDBTables.Click += new System.EventHandler(this.BtnWetlandBuildDBTables_Click);

            this.lblWetlandProcessingStatus.Font = defaultFont;
            this.lblWetlandProcessingStatus.ForeColor = defaultForeColor;
            this.lblWetlandProcessingStatus.Location = new System.Drawing.Point(20, 75);
            this.lblWetlandProcessingStatus.Name = "lblWetlandProcessingStatus";
            this.lblWetlandProcessingStatus.Size = new System.Drawing.Size(810, 25);
            this.lblWetlandProcessingStatus.TabIndex = 3;
            this.lblWetlandProcessingStatus.Text = "等待湿地资源清查处理";
            this.lblWetlandProcessingStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
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
            this.tabPage5.ForeColor = defaultForeColor;
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage5.Size = new System.Drawing.Size(842, 330);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "📊 综合质检、统计与成果输出";
            this.tabPage5.UseVisualStyleBackColor = true;

            // Controls for tabPage5 (originally in group5)
            this.btnOverallQualityCheck.Enabled = false;
            this.btnOverallQualityCheck.Font = defaultFont;
            this.btnOverallQualityCheck.ForeColor = defaultForeColor;
            this.btnOverallQualityCheck.Location = new System.Drawing.Point(20, 30);
            this.btnOverallQualityCheck.Name = "btnOverallQualityCheck";
            this.btnOverallQualityCheck.Size = new System.Drawing.Size(135, 30);
            this.btnOverallQualityCheck.TabIndex = 0;
            this.btnOverallQualityCheck.Text = "综合质量检查";
            this.btnOverallQualityCheck.UseVisualStyleBackColor = true;
            this.btnOverallQualityCheck.Click += new System.EventHandler(this.BtnOverallQualityCheck_Click);
            // ... (similarly for other controls in group5, except lblProgress)
            this.btnStatisticalAggregation.Enabled = false;
            this.btnStatisticalAggregation.Font = defaultFont;
            this.btnStatisticalAggregation.ForeColor = defaultForeColor;
            this.btnStatisticalAggregation.Location = new System.Drawing.Point(190, 30);
            this.btnStatisticalAggregation.Name = "btnStatisticalAggregation";
            this.btnStatisticalAggregation.Size = new System.Drawing.Size(135, 30);
            this.btnStatisticalAggregation.TabIndex = 1;
            this.btnStatisticalAggregation.Text = "数据统计汇总";
            this.btnStatisticalAggregation.UseVisualStyleBackColor = true;
            this.btnStatisticalAggregation.Click += new System.EventHandler(this.BtnStatisticalAggregation_Click);

            this.btnDataAnalysis.Enabled = false;
            this.btnDataAnalysis.Font = defaultFont;
            this.btnDataAnalysis.ForeColor = defaultForeColor;
            this.btnDataAnalysis.Location = new System.Drawing.Point(360, 30);
            this.btnDataAnalysis.Name = "btnDataAnalysis";
            this.btnDataAnalysis.Size = new System.Drawing.Size(135, 30);
            this.btnDataAnalysis.TabIndex = 2;
            this.btnDataAnalysis.Text = "数据分析与挖掘";
            this.btnDataAnalysis.UseVisualStyleBackColor = true;
            this.btnDataAnalysis.Click += new System.EventHandler(this.BtnDataAnalysis_Click);

            this.btnExportDatasetDB.Enabled = false;
            this.btnExportDatasetDB.Font = defaultFont;
            this.btnExportDatasetDB.ForeColor = defaultForeColor;
            this.btnExportDatasetDB.Location = new System.Drawing.Point(20, 75);
            this.btnExportDatasetDB.Name = "btnExportDatasetDB";
            this.btnExportDatasetDB.Size = new System.Drawing.Size(135, 30);
            this.btnExportDatasetDB.TabIndex = 3;
            this.btnExportDatasetDB.Text = "导出清查数据集";
            this.btnExportDatasetDB.UseVisualStyleBackColor = true;
            this.btnExportDatasetDB.Click += new System.EventHandler(this.BtnExportDatasetDB_Click);

            this.btnExportSummaryTables.Enabled = false;
            this.btnExportSummaryTables.Font = defaultFont;
            this.btnExportSummaryTables.ForeColor = defaultForeColor;
            this.btnExportSummaryTables.Location = new System.Drawing.Point(190, 75);
            this.btnExportSummaryTables.Name = "btnExportSummaryTables";
            this.btnExportSummaryTables.Size = new System.Drawing.Size(135, 30);
            this.btnExportSummaryTables.TabIndex = 4;
            this.btnExportSummaryTables.Text = "导出汇总表";
            this.btnExportSummaryTables.UseVisualStyleBackColor = true;
            this.btnExportSummaryTables.Click += new System.EventHandler(this.BtnExportSummaryTables_Click);

            this.btnGenerateReport.Enabled = false;
            this.btnGenerateReport.Font = defaultFont;
            this.btnGenerateReport.ForeColor = defaultForeColor;
            this.btnGenerateReport.Location = new System.Drawing.Point(360, 75);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(135, 30);
            this.btnGenerateReport.TabIndex = 5;
            this.btnGenerateReport.Text = "生成成果报告";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.BtnGenerateReport_Click);

            this.btnGenerateThematicMaps.Enabled = false;
            this.btnGenerateThematicMaps.Font = defaultFont;
            this.btnGenerateThematicMaps.ForeColor = defaultForeColor;
            this.btnGenerateThematicMaps.Location = new System.Drawing.Point(530, 75);
            this.btnGenerateThematicMaps.Name = "btnGenerateThematicMaps";
            this.btnGenerateThematicMaps.Size = new System.Drawing.Size(135, 30);
            this.btnGenerateThematicMaps.TabIndex = 6;
            this.btnGenerateThematicMaps.Text = "生成专题图";
            this.btnGenerateThematicMaps.UseVisualStyleBackColor = true;
            this.btnGenerateThematicMaps.Click += new System.EventHandler(this.BtnGenerateThematicMaps_Click);
            
            this.lblFinalOutputStatus.Font = defaultFont;
            this.lblFinalOutputStatus.ForeColor = defaultForeColor;
            this.lblFinalOutputStatus.Location = new System.Drawing.Point(20, 155); // Original Y was 155, lblProgress was at 120
            this.lblFinalOutputStatus.Name = "lblFinalOutputStatus";
            this.lblFinalOutputStatus.Size = new System.Drawing.Size(810, 25);
            this.lblFinalOutputStatus.TabIndex = 8; // Original TabIndex
            this.lblFinalOutputStatus.Text = "等待最终成果处理";
            this.lblFinalOutputStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.lblProgress); // Added
            this.bottomPanel.Controls.Add(this.btnHelp);
            this.bottomPanel.Controls.Add(this.btnClose);
            this.bottomPanel.Location = new System.Drawing.Point(15, tabControlMain.Bottom + 10);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(4);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(850, 60); // Increased height for lblProgress
            this.bottomPanel.TabIndex = 6; // Original TabIndex

            //
            // lblProgress (now in bottomPanel)
            //
            this.lblProgress.Font = defaultFont;
            this.lblProgress.ForeColor = defaultForeColor;
            this.lblProgress.Location = new System.Drawing.Point(10, 18); // Adjusted Y for centering
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(600, 25); // Adjusted width
            this.lblProgress.TabIndex = 7; // Original TabIndex from group5
            this.lblProgress.Text = "进度：等待开始处理";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnHelp
            // 
            this.btnHelp.Font = defaultFont;
            this.btnHelp.ForeColor = defaultForeColor;
            this.btnHelp.Location = new System.Drawing.Point(650, 15); // 固定位置而非动态计算
            this.btnHelp.Margin = new System.Windows.Forms.Padding(4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(70, 25); // Original size
            this.btnHelp.TabIndex = 0;
            this.btnHelp.Text = "帮助";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = defaultFont;
            this.btnClose.ForeColor = defaultForeColor;
            this.btnClose.Location = new System.Drawing.Point(750, 15); // 固定位置而非动态计算
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 25); // Original size
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            
            // Set mainPanel and Form Size
            this.mainPanel.Size = new System.Drawing.Size(880, this.bottomPanel.Bottom + 15); // Adjusted width slightly
            // 
            // MainProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = this.mainPanel.Size; // Set ClientSize to mainPanel's new size
            this.Controls.Add(this.mainPanel);
            this.Font = defaultFont;
            this.ForeColor = defaultForeColor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4); // Original: (4,4,4,4) -> (3,2,3,2) for 6F,12F
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainProcessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "广东省全民所有自然资源（森林、草地、湿地）资产清查工具";

            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

        private void InitializeFormState()
        {
            _selectedCounties = new List<string>();
            UpdateProgress("等待选择工作空间和县区");
            
            lblPrerequisiteDataStatus.Text = "未加载前提数据";
            lblPrerequisiteDataStatus.ForeColor = System.Drawing.Color.Black; 
            lblCountyEmptyTablesStatus.Text = "未创建县级空表"; 
            lblCountyEmptyTablesStatus.ForeColor = System.Drawing.Color.Black; 
            lblForestProcessingStatus.Text = "等待森林资源清查处理";
            lblForestProcessingStatus.ForeColor = System.Drawing.Color.Black;
            lblGrasslandProcessingStatus.Text = "等待草地资源清查处理";
            lblGrasslandProcessingStatus.ForeColor = System.Drawing.Color.Black;
            lblWetlandProcessingStatus.Text = "等待湿地资源清查处理";
            lblWetlandProcessingStatus.ForeColor = System.Drawing.Color.Black;
            lblFinalOutputStatus.Text = "等待最终成果处理";
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black;
            lblProgress.ForeColor = System.Drawing.Color.Black;
        }

        private void UpdateProgress(string message)
        {
            lblProgress.Text = $"进度：{message}";
            Application.DoEvents();
        }

        private void UpdateButtonStates()
        {
            bool hasWorkspace = !string.IsNullOrEmpty(_workspacePath);
            bool hasCounties = _selectedCounties != null && _selectedCounties.Count > 0;
            bool hasBasicSetup = hasWorkspace && hasCounties;

            // 步骤1按钮
            btnLoadPrerequisiteData.Enabled = hasBasicSetup;
            btnCreateCountyEmptyTables.Enabled = hasBasicSetup; // 新增
            // 假设前提数据已加载是另一种状态: bool prerequisiteDataLoaded = ...;

            // 步骤2: 森林资源按钮 (依赖于基础设置，并可能依赖于前提数据)
            bool enableStep2 = hasBasicSetup; // && prerequisiteDataLoaded; // 并且前提数据已加载
            btnForestExtractScope.Enabled = enableStep2;
            btnForestCreateBasemapLinkPrice.Enabled = enableStep2;
            btnForestSupplementPrice.Enabled = enableStep2;
            btnForestCalculateValue.Enabled = enableStep2;
            btnForestCleanQA.Enabled = enableStep2;
            btnForestBuildDBTables.Enabled = enableStep2;

            // 步骤3: 草地资源按钮 (依赖于基础设置，并可能依赖于前提数据)
            bool enableStep3 = hasBasicSetup; // && prerequisiteDataLoaded; // 并且前提数据已加载
            btnGrasslandExtractScope.Enabled = enableStep3;
            btnGrasslandCreateBasemapLinkPrice.Enabled = enableStep3;
            btnGrasslandSupplementPrice.Enabled = enableStep3;
            btnGrasslandCalculateValue.Enabled = enableStep3;
            btnGrasslandCleanQA.Enabled = enableStep3;
            btnGrasslandBuildDBTables.Enabled = enableStep3;

            // 步骤4: 湿地资源按钮 (依赖于基础设置，并可能依赖于前提数据)
            bool enableStep4 = hasBasicSetup; // && prerequisiteDataLoaded; // 并且前提数据已加载
            btnWetlandExtractScopeBasemap.Enabled = enableStep4;
            btnWetlandCleanQA.Enabled = enableStep4;
            btnWetlandBuildDBTables.Enabled = enableStep4;

            // 步骤5: 最终成果输出按钮 (依赖于前面步骤的完成)
            bool enableStep5 = hasBasicSetup; // && forestDone && grasslandDone && wetlandDone && overallQADone ...; // 并且森林、草地、湿地、综合质检等已完成
            btnOverallQualityCheck.Enabled = enableStep5; // 这个可能应该依赖于步骤2,3,4的完成
            btnStatisticalAggregation.Enabled = enableStep5;
            btnDataAnalysis.Enabled = enableStep5;
            btnExportDatasetDB.Enabled = enableStep5;
            btnExportSummaryTables.Enabled = enableStep5;
            btnGenerateReport.Enabled = enableStep5;
            btnGenerateThematicMaps.Enabled = enableStep5;
        }

        #region 事件处理器

        private void BtnSelectWorkspace_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择目标数据库路径（File Geodatabase .gdb）";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SelectedPath.EndsWith(".gdb") || System.IO.Directory.Exists(dialog.SelectedPath))
                    {
                        _workspacePath = dialog.SelectedPath;
                        lblWorkspace.Text = _workspacePath;
                        lblWorkspace.ForeColor = Color.DarkGreen;
                        UpdateButtonStates();
                        UpdateProgress("已选择工作空间");
                    }
                    else
                    {
                        MessageBox.Show("请选择有效的File Geodatabase (.gdb) 路径！", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void BtnSelectCounties_Click(object sender, EventArgs e)
        {
            using (CountySelectionForm form = new CountySelectionForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _selectedCounties = form.SelectedCounties;
                    lblCounties.Text = $"已选择 {_selectedCounties.Count} 个县区：{string.Join(", ", _selectedCounties)}";
                    lblCounties.ForeColor = Color.DarkGreen;
                    UpdateButtonStates();
                    UpdateProgress($"已选择 {_selectedCounties.Count} 个县区");
                }
            }
        }

        // 新的事件处理器 (存根)
        private void BtnLoadPrerequisiteData_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在加载前提数据...");
            // TODO: 实现前提数据加载逻辑
            // 示例: 加载普查数据, 分等/定级/基准地价数据, 城镇开发边界, 遥感影像
            lblPrerequisiteDataStatus.Text = "前提数据加载完成"; // 或特定状态
            lblPrerequisiteDataStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("前提数据加载完成");
            UpdateButtonStates(); // 如果其他按钮依赖此操作，则更新状态
        }

        private void BtnCreateCountyEmptyTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在创建县级空表...");
            // TODO: 实现为每个选定县区创建空表的逻辑
            // 这可能涉及到遍历 _selectedCounties
            // 并使用辅助方法 (可能在 DataTableCreator.cs 中)
            // 为每个县区创建森林、草地和湿地的数据表。
            lblCountyEmptyTablesStatus.Text = "县级空表创建完成"; // 或特定状态
            lblCountyEmptyTablesStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("县级空表创建完成");
            UpdateButtonStates(); // 如果其他按钮依赖此操作，则可能更新状态
        }

        // 森林资源处理器
        private void BtnForestExtractScope_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在提取森林工作范围...");
            // TODO: 实现逻辑
            lblForestProcessingStatus.Text = "森林工作范围提取完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林工作范围提取完成");
        }

        private void BtnForestCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在处理森林工作底图与价格关联...");
            // TODO: 实现逻辑
            lblForestProcessingStatus.Text = "森林底图与价格关联完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林底图与价格关联完成");
        }

        private void BtnForestSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在补充森林基准价格...");
            // TODO: 实现逻辑
            lblForestProcessingStatus.Text = "森林基准价格补充完成";
            lblForestProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("森林基准价格补充完成");
        }

        private void BtnForestCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在计算森林资产价值...");
            // TODO: 实现逻辑
            lblForestProcessingStatus.Text = "森林资产价值计算完成";
            lblForestProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("森林资产价值计算完成");
        }

        private void BtnForestCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行森林数据清洗与质检...");
            // TODO: 实现逻辑
            lblForestProcessingStatus.Text = "森林数据清洗与质检完成";
            lblForestProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("森林数据清洗与质检完成");
        }

        private void BtnForestBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建森林库表...");
            // TODO: 实现逻辑
            lblForestProcessingStatus.Text = "森林库表构建完成";
            lblForestProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("森林库表构建完成");
        }

        // 草地资源处理器
        private void BtnGrasslandExtractScope_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在提取草地工作范围...");
            // TODO: 实现逻辑
            lblGrasslandProcessingStatus.Text = "草地工作范围提取完成";
            lblGrasslandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("草地工作范围提取完成");
        }

        private void BtnGrasslandCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在处理草地工作底图与价格关联...");
            // TODO: 实现逻辑
            lblGrasslandProcessingStatus.Text = "草地底图与价格关联完成";
            lblGrasslandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("草地底图与价格关联完成");
        }

        private void BtnGrasslandSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在补充草地基准价格...");
            // TODO: 实现逻辑
            lblGrasslandProcessingStatus.Text = "草地基准价格补充完成";
            lblGrasslandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("草地基准价格补充完成");
        }

        private void BtnGrasslandCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在计算草地资产价值...");
            // TODO: 实现逻辑
            lblGrasslandProcessingStatus.Text = "草地资产价值计算完成";
            lblGrasslandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("草地资产价值计算完成");
        }

        private void BtnGrasslandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行草地数据清洗与质检...");
            // TODO: 实现逻辑
            lblGrasslandProcessingStatus.Text = "草地数据清洗与质检完成";
            lblGrasslandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("草地数据清洗与质检完成");
        }

        private void BtnGrasslandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建草地库表...");
            // TODO: 实现逻辑
            lblGrasslandProcessingStatus.Text = "草地库表构建完成";
            lblGrasslandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("草地库表构建完成");
        }

        // 湿地资源处理器
        private void BtnWetlandExtractScopeBasemap_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在制作湿地工作范围与底图...");
            // TODO: 实现逻辑
            lblWetlandProcessingStatus.Text = "湿地工作范围与底图制作完成";
            lblWetlandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("湿地工作范围与底图制作完成");
        }

        private void BtnWetlandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行湿地数据清洗与质检...");
            // TODO: 实现逻辑
            lblWetlandProcessingStatus.Text = "湿地数据清洗与质检完成";
            lblWetlandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("湿地数据清洗与质检完成");
        }

        private void BtnWetlandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建湿地库表...");
            // TODO: 实现逻辑
            lblWetlandProcessingStatus.Text = "湿地库表构建完成";
            lblWetlandProcessingStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("湿地库表构建完成");
        }

        // 最终成果输出处理器
        private void BtnOverallQualityCheck_Click(object sender, EventArgs e) // 原为 BtnQualityCheck_Click
        {
            UpdateProgress("正在进行综合质量检查...");
            // TODO: 实现逻辑
            lblFinalOutputStatus.Text = "综合质量检查完成";
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("综合质量检查完成");
        }

        private void BtnStatisticalAggregation_Click(object sender, EventArgs e) // 原为 BtnStatistics_Click
        {
            UpdateProgress("正在进行数据统计汇总...");
            // TODO: 实现逻辑
            lblFinalOutputStatus.Text = "数据统计汇总完成";
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("数据统计汇总完成");
        }

        private void BtnDataAnalysis_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行数据分析与挖掘...");
            // TODO: 实现逻辑
            lblFinalOutputStatus.Text = "数据分析与挖掘完成";
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("数据分析与挖掘完成");
        }

        private void BtnExportDatasetDB_Click(object sender, EventArgs e) // 原为 BtnExportResults_Click
        {
            UpdateProgress("正在导出清查数据集...");
            // TODO: 实现逻辑
            lblFinalOutputStatus.Text = "清查数据集导出完成";
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("清查数据集导出完成");
        }

        private void BtnExportSummaryTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在导出汇总表...");
            // TODO: 实现逻辑
            lblFinalOutputStatus.Text = "汇总表导出完成";
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("汇总表导出完成");
        }
        
        private void BtnGenerateReport_Click(object sender, EventArgs e) // 保留，逻辑可能更改
        {
            UpdateProgress("正在生成成果报告...");
            // TODO: 实现逻辑 (例如：工作总结报告, 数据自检报告)
            lblFinalOutputStatus.Text = "成果报告生成完成";
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("成果报告生成完成");
        }

        private void BtnGenerateThematicMaps_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在生成专题图...");
            // TODO: 实现逻辑
            lblFinalOutputStatus.Text = "专题图生成完成";
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black; // 从DarkGreen更改
            UpdateProgress("专题图生成完成");
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            string helpText = @"广东省全民所有自然资源（森林、草地、湿地）资产清查工具使用说明

基础数据准备
1. 选择工作空间：选择用于存放清查数据的File Geodatabase (.gdb)。
2. 选择县区：选择需要进行清查的一个或多个县区。
3. 加载前提数据：加载必要的背景数据，如林草湿荒普查数据、林地/草地分等定级和基准地价成果、城镇开发边界数据、最新遥感影像等。

森林资源资产清查
1. 提取森林工作范围：根据普查数据和开发边界，筛选国有林地及边界内集体林地。
2. 森林底图与价格关联：将工作范围与林地分等、定级数据关联，挂接基准地价。
3. 补充森林基准价格：处理空间挂接失败的图斑，补充基准价格。
4. 计算森林资产价值：基于修正因子计算宗地价格，进行期日和年期修正，得到核算价格并计算总价值。
5. 森林数据清洗与质检：检查属性规范性、数据完整性等。
6. 森林库表构建：按规范生成森林资源清查数据集和相关表格。

草地资源资产清查
1. 提取草地工作范围：筛选国有草地及边界内集体草地。
2. 草地底图与价格关联：将工作范围与草地分等、定级数据关联，挂接基准地价。
3. 补充草地基准价格：处理挂接失败图斑，补充基准价格。
4. 计算草地资产价值：计算宗地价格，修正得到核算价格并计算总价值。
5. 草地数据清洗与质检：检查属性与数据质量。
6. 草地库表构建：按规范生成草地资源清查数据集和相关表格。

湿地资源资产清查
1. 湿地范围与底图制作：提取国有湿地及边界内集体湿地，形成工作底图。
   (注意：本轮清查不估算湿地资源经济价值)
2. 湿地数据清洗与质检：检查属性与数据质量。
3. 湿地库表构建：按规范生成湿地资源清查数据集和相关表格。

综合质检、统计与成果输出
1. 综合质量检查：对各资源类型的清查数据进行最终检查。
2. 数据统计汇总：生成全省或各县区的森林、草地、湿地实物量与价值量统计表。
3. 数据分析与挖掘：对统计结果进行深入分析。
4. 导出清查数据集：按规范导出完整的空间和非空间数据库。
5. 导出汇总表：导出各类统计汇总表格。
6. 生成成果报告：生成工作总结报告和数据自检报告。
7. 生成专题图：制作反映清查成果的专题地图。

注意事项：
- 请尽量按步骤顺序执行。
- 每个操作完成后会有相应提示。
- 部分操作可能耗时较长，请耐心等待。";

            MessageBox.Show(helpText, "使用帮助", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MainProcessForm_Load(object sender, EventArgs e)
        {
            AdjustButtonPositions();
            this.bottomPanel.Resize += BottomPanel_Resize;
        }

        private void BottomPanel_Resize(object sender, EventArgs e)
        {
            AdjustButtonPositions();
        }

        private void AdjustButtonPositions()
        {
            btnHelp.Location = new Point(bottomPanel.Width - btnClose.Width - 10 - btnHelp.Width - 5, btnHelp.Location.Y);
            btnClose.Location = new Point(bottomPanel.Width - btnClose.Width - 10, btnClose.Location.Y);
        }

        #endregion
    }
}