namespace TestArcMapAddin2.Forms
{
    partial class ForestBasemapPriceAssociationForm
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
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxDataInput = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLandGradePricePath = new System.Windows.Forms.TextBox();
            this.btnBrowseLandGradePrice = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtForestResourcePath = new System.Windows.Forms.TextBox();
            this.btnBrowseForestResource = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.btnImportPriceMapping = new System.Windows.Forms.Button();
            this.btnPairData = new System.Windows.Forms.Button();
            this.btnViewPriceMapping = new System.Windows.Forms.Button();
            this.groupBoxDataPairs = new System.Windows.Forms.GroupBox();
            this.dataGridViewPairs = new System.Windows.Forms.DataGridView();
            this.groupBoxProcessing = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnProcessData = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabPagePriceMapping = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxTemplate = new System.Windows.Forms.GroupBox();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            this.btnExportTemplate = new System.Windows.Forms.Button();
            this.groupBoxMappingData = new System.Windows.Forms.GroupBox();
            this.dataGridViewPriceMapping = new System.Windows.Forms.DataGridView();
            this.lblMappingStatus = new System.Windows.Forms.Label();
            this.mainTabControl.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxDataInput.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxDataPairs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPairs)).BeginInit();
            this.groupBoxProcessing.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPagePriceMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxTemplate.SuspendLayout();
            this.groupBoxMappingData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPriceMapping)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.tabPageMain);
            this.mainTabControl.Controls.Add(this.tabPagePriceMapping);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainTabControl.Location = new System.Drawing.Point(0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(1090, 700);
            this.mainTabControl.TabIndex = 0;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.tableLayoutPanel1);
            this.tabPageMain.Location = new System.Drawing.Point(4, 33);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(1082, 663);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "数据处理";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxDataInput, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxDataPairs, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxProcessing, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1076, 657);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBoxDataInput
            // 
            this.groupBoxDataInput.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxDataInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDataInput.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxDataInput.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDataInput.Name = "groupBoxDataInput";
            this.groupBoxDataInput.Size = new System.Drawing.Size(1070, 194);
            this.groupBoxDataInput.TabIndex = 0;
            this.groupBoxDataInput.TabStop = false;
            this.groupBoxDataInput.Text = "数据导入";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLandGradePricePath, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseLandGradePrice, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtForestResourcePath, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseForestResource, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtOutputPath, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseOutput, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnImportPriceMapping, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnPairData, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnViewPriceMapping, 3, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1064, 164);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "林地基准地价定级数据:";
            // 
            // txtLandGradePricePath
            // 
            this.txtLandGradePricePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLandGradePricePath.ForeColor = System.Drawing.Color.Gray;
            this.txtLandGradePricePath.Location = new System.Drawing.Point(153, 3);
            this.txtLandGradePricePath.Name = "txtLandGradePricePath";
            this.txtLandGradePricePath.ReadOnly = true;
            this.txtLandGradePricePath.Size = new System.Drawing.Size(728, 31);
            this.txtLandGradePricePath.TabIndex = 1;
            this.txtLandGradePricePath.Text = "请选择林地基准地价定级数据文件夹路径";
            // 
            // btnBrowseLandGradePrice
            // 
            this.btnBrowseLandGradePrice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseLandGradePrice.Location = new System.Drawing.Point(887, 6);
            this.btnBrowseLandGradePrice.Name = "btnBrowseLandGradePrice";
            this.btnBrowseLandGradePrice.Size = new System.Drawing.Size(74, 23);
            this.btnBrowseLandGradePrice.TabIndex = 2;
            this.btnBrowseLandGradePrice.Text = "浏览...";
            this.btnBrowseLandGradePrice.UseVisualStyleBackColor = true;
            this.btnBrowseLandGradePrice.Click += new System.EventHandler(this.btnBrowseLandGradePrice_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 35);
            this.label2.TabIndex = 3;
            this.label2.Text = "森林资源地类图斑数据:";
            // 
            // txtForestResourcePath
            // 
            this.txtForestResourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtForestResourcePath.ForeColor = System.Drawing.Color.Gray;
            this.txtForestResourcePath.Location = new System.Drawing.Point(153, 38);
            this.txtForestResourcePath.Name = "txtForestResourcePath";
            this.txtForestResourcePath.ReadOnly = true;
            this.txtForestResourcePath.Size = new System.Drawing.Size(728, 31);
            this.txtForestResourcePath.TabIndex = 4;
            this.txtForestResourcePath.Text = "请选择森林资源地类图斑数据文件夹路径";
            // 
            // btnBrowseForestResource
            // 
            this.btnBrowseForestResource.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseForestResource.Location = new System.Drawing.Point(887, 41);
            this.btnBrowseForestResource.Name = "btnBrowseForestResource";
            this.btnBrowseForestResource.Size = new System.Drawing.Size(74, 23);
            this.btnBrowseForestResource.TabIndex = 5;
            this.btnBrowseForestResource.Text = "浏览...";
            this.btnBrowseForestResource.UseVisualStyleBackColor = true;
            this.btnBrowseForestResource.Click += new System.EventHandler(this.btnBrowseForestResource_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "输出路径:";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputPath.ForeColor = System.Drawing.Color.Gray;
            this.txtOutputPath.Location = new System.Drawing.Point(153, 73);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(728, 31);
            this.txtOutputPath.TabIndex = 7;
            this.txtOutputPath.Text = "请选择输出结果文件夹路径";
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseOutput.Location = new System.Drawing.Point(887, 76);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(74, 23);
            this.btnBrowseOutput.TabIndex = 8;
            this.btnBrowseOutput.Text = "浏览...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // btnImportPriceMapping
            // 
            this.btnImportPriceMapping.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnImportPriceMapping.BackColor = System.Drawing.Color.LightBlue;
            this.btnImportPriceMapping.Location = new System.Drawing.Point(10, 119);
            this.btnImportPriceMapping.Name = "btnImportPriceMapping";
            this.btnImportPriceMapping.Size = new System.Drawing.Size(130, 30);
            this.btnImportPriceMapping.TabIndex = 9;
            this.btnImportPriceMapping.Text = "导入价格映射表";
            this.btnImportPriceMapping.UseVisualStyleBackColor = false;
            this.btnImportPriceMapping.Click += new System.EventHandler(this.btnImportPriceMapping_Click);
            // 
            // btnPairData
            // 
            this.btnPairData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPairData.BackColor = System.Drawing.Color.LightGreen;
            this.btnPairData.Enabled = false;
            this.btnPairData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPairData.Location = new System.Drawing.Point(457, 119);
            this.btnPairData.Name = "btnPairData";
            this.btnPairData.Size = new System.Drawing.Size(120, 30);
            this.btnPairData.TabIndex = 10;
            this.btnPairData.Text = "配对读取";
            this.btnPairData.UseVisualStyleBackColor = false;
            this.btnPairData.Click += new System.EventHandler(this.btnPairData_Click);
            // 
            // btnViewPriceMapping
            // 
            this.btnViewPriceMapping.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnViewPriceMapping.BackColor = System.Drawing.Color.LightCyan;
            this.btnViewPriceMapping.Font = new System.Drawing.Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnViewPriceMapping.Location = new System.Drawing.Point(970, 119);
            this.btnViewPriceMapping.Name = "btnViewPriceMapping";
            this.btnViewPriceMapping.Size = new System.Drawing.Size(88, 30);
            this.btnViewPriceMapping.TabIndex = 11;
            this.btnViewPriceMapping.Text = "查看映射表";
            this.btnViewPriceMapping.UseVisualStyleBackColor = false;
            this.btnViewPriceMapping.Click += new System.EventHandler(this.btnViewPriceMapping_Click);
            // 
            // groupBoxDataPairs
            // 
            this.groupBoxDataPairs.Controls.Add(this.dataGridViewPairs);
            this.groupBoxDataPairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDataPairs.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxDataPairs.Location = new System.Drawing.Point(3, 203);
            this.groupBoxDataPairs.Name = "groupBoxDataPairs";
            this.groupBoxDataPairs.Size = new System.Drawing.Size(1070, 351);
            this.groupBoxDataPairs.TabIndex = 1;
            this.groupBoxDataPairs.TabStop = false;
            this.groupBoxDataPairs.Text = "数据配对结果";
            // 
            // dataGridViewPairs
            // 
            this.dataGridViewPairs.AllowUserToAddRows = false;
            this.dataGridViewPairs.AllowUserToDeleteRows = false;
            this.dataGridViewPairs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPairs.Location = new System.Drawing.Point(3, 27);
            this.dataGridViewPairs.Name = "dataGridViewPairs";
            this.dataGridViewPairs.RowHeadersWidth = 62;
            this.dataGridViewPairs.RowTemplate.Height = 23;
            this.dataGridViewPairs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPairs.Size = new System.Drawing.Size(1064, 321);
            this.dataGridViewPairs.TabIndex = 0;
            // 
            // groupBoxProcessing
            // 
            this.groupBoxProcessing.Controls.Add(this.tableLayoutPanel3);
            this.groupBoxProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcessing.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxProcessing.Location = new System.Drawing.Point(3, 560);
            this.groupBoxProcessing.Name = "groupBoxProcessing";
            this.groupBoxProcessing.Size = new System.Drawing.Size(1070, 94);
            this.groupBoxProcessing.TabIndex = 2;
            this.groupBoxProcessing.TabStop = false;
            this.groupBoxProcessing.Text = "数据处理";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.Controls.Add(this.btnProcessData, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.progressBar, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.statusLabel, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1064, 64);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // btnProcessData
            // 
            this.btnProcessData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnProcessData.BackColor = System.Drawing.Color.Orange;
            this.btnProcessData.Enabled = false;
            this.btnProcessData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProcessData.Location = new System.Drawing.Point(10, 17);
            this.btnProcessData.Name = "btnProcessData";
            this.btnProcessData.Size = new System.Drawing.Size(130, 30);
            this.btnProcessData.TabIndex = 0;
            this.btnProcessData.Text = "生成核算价格属性表";
            this.btnProcessData.UseVisualStyleBackColor = false;
            this.btnProcessData.Click += new System.EventHandler(this.btnProcessData_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(153, 22);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(628, 20);
            this.progressBar.TabIndex = 1;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(787, 20);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(46, 24);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "就绪";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Location = new System.Drawing.Point(987, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabPagePriceMapping
            // 
            this.tabPagePriceMapping.Controls.Add(this.splitContainer1);
            this.tabPagePriceMapping.Location = new System.Drawing.Point(4, 33);
            this.tabPagePriceMapping.Name = "tabPagePriceMapping";
            this.tabPagePriceMapping.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePriceMapping.Size = new System.Drawing.Size(1082, 663);
            this.tabPagePriceMapping.TabIndex = 1;
            this.tabPagePriceMapping.Text = "价格映射表";
            this.tabPagePriceMapping.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxTemplate);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBoxMappingData);
            this.splitContainer1.Size = new System.Drawing.Size(1076, 657);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBoxTemplate
            // 
            this.groupBoxTemplate.Controls.Add(this.txtTemplate);
            this.groupBoxTemplate.Controls.Add(this.btnExportTemplate);
            this.groupBoxTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTemplate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxTemplate.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTemplate.Name = "groupBoxTemplate";
            this.groupBoxTemplate.Size = new System.Drawing.Size(1076, 250);
            this.groupBoxTemplate.TabIndex = 0;
            this.groupBoxTemplate.TabStop = false;
            this.groupBoxTemplate.Text = "映射表模板说明";
            // 
            // txtTemplate
            // 
            this.txtTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTemplate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtTemplate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTemplate.Location = new System.Drawing.Point(6, 30);
            this.txtTemplate.Multiline = true;
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.ReadOnly = true;
            this.txtTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTemplate.Size = new System.Drawing.Size(1064, 175);
            this.txtTemplate.TabIndex = 0;
            // 
            // btnExportTemplate
            // 
            this.btnExportTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportTemplate.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnExportTemplate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExportTemplate.ForeColor = System.Drawing.Color.White;
            this.btnExportTemplate.Location = new System.Drawing.Point(970, 211);
            this.btnExportTemplate.Name = "btnExportTemplate";
            this.btnExportTemplate.Size = new System.Drawing.Size(100, 30);
            this.btnExportTemplate.TabIndex = 1;
            this.btnExportTemplate.Text = "导出模板";
            this.btnExportTemplate.UseVisualStyleBackColor = false;
            this.btnExportTemplate.Click += new System.EventHandler(this.btnExportTemplate_Click);
            // 
            // groupBoxMappingData
            // 
            this.groupBoxMappingData.Controls.Add(this.dataGridViewPriceMapping);
            this.groupBoxMappingData.Controls.Add(this.lblMappingStatus);
            this.groupBoxMappingData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMappingData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxMappingData.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMappingData.Name = "groupBoxMappingData";
            this.groupBoxMappingData.Size = new System.Drawing.Size(1076, 403);
            this.groupBoxMappingData.TabIndex = 0;
            this.groupBoxMappingData.TabStop = false;
            this.groupBoxMappingData.Text = "当前映射数据";
            // 
            // dataGridViewPriceMapping
            // 
            this.dataGridViewPriceMapping.AllowUserToAddRows = false;
            this.dataGridViewPriceMapping.AllowUserToDeleteRows = false;
            this.dataGridViewPriceMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPriceMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPriceMapping.Location = new System.Drawing.Point(6, 60);
            this.dataGridViewPriceMapping.Name = "dataGridViewPriceMapping";
            this.dataGridViewPriceMapping.ReadOnly = true;
            this.dataGridViewPriceMapping.RowHeadersWidth = 62;
            this.dataGridViewPriceMapping.RowTemplate.Height = 23;
            this.dataGridViewPriceMapping.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPriceMapping.Size = new System.Drawing.Size(1064, 337);
            this.dataGridViewPriceMapping.TabIndex = 1;
            // 
            // lblMappingStatus
            // 
            this.lblMappingStatus.AutoSize = true;
            this.lblMappingStatus.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMappingStatus.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblMappingStatus.Location = new System.Drawing.Point(6, 30);
            this.lblMappingStatus.Name = "lblMappingStatus";
            this.lblMappingStatus.Size = new System.Drawing.Size(282, 25);
            this.lblMappingStatus.TabIndex = 0;
            this.lblMappingStatus.Text = "状态：尚未导入任何价格映射数据";
            // 
            // ForestBasemapPriceAssociationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 700);
            this.Controls.Add(this.mainTabControl);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ForestBasemapPriceAssociationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "森林底图与价格关联处理";
            this.mainTabControl.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxDataInput.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBoxDataPairs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPairs)).EndInit();
            this.groupBoxProcessing.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabPagePriceMapping.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxTemplate.ResumeLayout(false);
            this.groupBoxTemplate.PerformLayout();
            this.groupBoxMappingData.ResumeLayout(false);
            this.groupBoxMappingData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPriceMapping)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxDataInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLandGradePricePath;
        private System.Windows.Forms.Button btnBrowseLandGradePrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtForestResourcePath;
        private System.Windows.Forms.Button btnBrowseForestResource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Button btnImportPriceMapping;
        private System.Windows.Forms.Button btnPairData;
        private System.Windows.Forms.Button btnViewPriceMapping;
        private System.Windows.Forms.GroupBox groupBoxDataPairs;
        private System.Windows.Forms.DataGridView dataGridViewPairs;
        private System.Windows.Forms.GroupBox groupBoxProcessing;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnProcessData;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabPage tabPagePriceMapping;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxTemplate;
        private System.Windows.Forms.TextBox txtTemplate;
        private System.Windows.Forms.Button btnExportTemplate;
        private System.Windows.Forms.GroupBox groupBoxMappingData;
        private System.Windows.Forms.DataGridView dataGridViewPriceMapping;
        private System.Windows.Forms.Label lblMappingStatus;
    }
}