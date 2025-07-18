namespace TestArcMapAddin2.Forms
{
    partial class ForestPriceSupplementForm
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
            this.txtInputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseInputPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputPath = new System.Windows.Forms.Button();
            this.btnImportPriceMapping = new System.Windows.Forms.Button();
            this.btnScanData = new System.Windows.Forms.Button();
            this.groupBoxCountyData = new System.Windows.Forms.GroupBox();
            this.dataGridViewCounties = new System.Windows.Forms.DataGridView();
            this.groupBoxProcessing = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSupplementPrice = new System.Windows.Forms.Button();
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
            this.groupBoxCountyData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCounties)).BeginInit();
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
            this.mainTabControl.Size = new System.Drawing.Size(1100, 700);
            this.mainTabControl.TabIndex = 0;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Controls.Add(this.tableLayoutPanel1);
            this.tabPageMain.Location = new System.Drawing.Point(4, 33);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(1092, 663);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "价格数据补充";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxDataInput, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxCountyData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxProcessing, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1086, 657);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBoxDataInput
            // 
            this.groupBoxDataInput.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxDataInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDataInput.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxDataInput.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDataInput.Name = "groupBoxDataInput";
            this.groupBoxDataInput.Size = new System.Drawing.Size(1080, 154);
            this.groupBoxDataInput.TabIndex = 0;
            this.groupBoxDataInput.TabStop = false;
            this.groupBoxDataInput.Text = "数据输入";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtInputPath, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseInputPath, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtOutputPath, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseOutputPath, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnImportPriceMapping, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnScanData, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1074, 124);
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
            this.label1.Text = "核算价格数据路径:";
            // 
            // txtInputPath
            // 
            this.txtInputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputPath.ForeColor = System.Drawing.Color.Gray;
            this.txtInputPath.Location = new System.Drawing.Point(153, 3);
            this.txtInputPath.Name = "txtInputPath";
            this.txtInputPath.ReadOnly = true;
            this.txtInputPath.Size = new System.Drawing.Size(718, 31);
            this.txtInputPath.TabIndex = 1;
            this.txtInputPath.Text = "请选择包含各县LDHSJG数据的文件夹路径";
            // 
            // btnBrowseInputPath
            // 
            this.btnBrowseInputPath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseInputPath.Location = new System.Drawing.Point(877, 6);
            this.btnBrowseInputPath.Name = "btnBrowseInputPath";
            this.btnBrowseInputPath.Size = new System.Drawing.Size(74, 23);
            this.btnBrowseInputPath.TabIndex = 2;
            this.btnBrowseInputPath.Text = "浏览...";
            this.btnBrowseInputPath.UseVisualStyleBackColor = true;
            this.btnBrowseInputPath.Click += new System.EventHandler(this.btnBrowseInputPath_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 35);
            this.label2.TabIndex = 3;
            this.label2.Text = "输出路径:";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputPath.ForeColor = System.Drawing.Color.Gray;
            this.txtOutputPath.Location = new System.Drawing.Point(153, 38);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(718, 31);
            this.txtOutputPath.TabIndex = 4;
            this.txtOutputPath.Text = "请选择输出处理结果的文件夹路径";
            // 
            // btnBrowseOutputPath
            // 
            this.btnBrowseOutputPath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseOutputPath.Location = new System.Drawing.Point(877, 41);
            this.btnBrowseOutputPath.Name = "btnBrowseOutputPath";
            this.btnBrowseOutputPath.Size = new System.Drawing.Size(74, 23);
            this.btnBrowseOutputPath.TabIndex = 5;
            this.btnBrowseOutputPath.Text = "浏览...";
            this.btnBrowseOutputPath.UseVisualStyleBackColor = true;
            this.btnBrowseOutputPath.Click += new System.EventHandler(this.btnBrowseOutputPath_Click);
            // 
            // btnImportPriceMapping
            // 
            this.btnImportPriceMapping.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnImportPriceMapping.BackColor = System.Drawing.Color.LightBlue;
            this.btnImportPriceMapping.Location = new System.Drawing.Point(10, 84);
            this.btnImportPriceMapping.Name = "btnImportPriceMapping";
            this.btnImportPriceMapping.Size = new System.Drawing.Size(130, 30);
            this.btnImportPriceMapping.TabIndex = 6;
            this.btnImportPriceMapping.Text = "导入价格映射表";
            this.btnImportPriceMapping.UseVisualStyleBackColor = false;
            this.btnImportPriceMapping.Click += new System.EventHandler(this.btnImportPriceMapping_Click);
            // 
            // btnScanData
            // 
            this.btnScanData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnScanData.BackColor = System.Drawing.Color.LightGreen;
            this.btnScanData.Enabled = false;
            this.btnScanData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnScanData.Location = new System.Drawing.Point(512, 84);
            this.btnScanData.Name = "btnScanData";
            this.btnScanData.Size = new System.Drawing.Size(120, 30);
            this.btnScanData.TabIndex = 7;
            this.btnScanData.Text = "扫描数据";
            this.btnScanData.UseVisualStyleBackColor = false;
            this.btnScanData.Click += new System.EventHandler(this.btnScanData_Click);
            // 
            // groupBoxCountyData
            // 
            this.groupBoxCountyData.Controls.Add(this.dataGridViewCounties);
            this.groupBoxCountyData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxCountyData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxCountyData.Location = new System.Drawing.Point(3, 163);
            this.groupBoxCountyData.Name = "groupBoxCountyData";
            this.groupBoxCountyData.Size = new System.Drawing.Size(1080, 411);
            this.groupBoxCountyData.TabIndex = 1;
            this.groupBoxCountyData.TabStop = false;
            this.groupBoxCountyData.Text = "县级数据统计";
            // 
            // dataGridViewCounties
            // 
            this.dataGridViewCounties.AllowUserToAddRows = false;
            this.dataGridViewCounties.AllowUserToDeleteRows = false;
            this.dataGridViewCounties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCounties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCounties.Location = new System.Drawing.Point(3, 27);
            this.dataGridViewCounties.Name = "dataGridViewCounties";
            this.dataGridViewCounties.RowHeadersWidth = 62;
            this.dataGridViewCounties.RowTemplate.Height = 23;
            this.dataGridViewCounties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCounties.Size = new System.Drawing.Size(1074, 381);
            this.dataGridViewCounties.TabIndex = 0;
            // 
            // groupBoxProcessing
            // 
            this.groupBoxProcessing.Controls.Add(this.tableLayoutPanel3);
            this.groupBoxProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcessing.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxProcessing.Location = new System.Drawing.Point(3, 580);
            this.groupBoxProcessing.Name = "groupBoxProcessing";
            this.groupBoxProcessing.Size = new System.Drawing.Size(1080, 74);
            this.groupBoxProcessing.TabIndex = 2;
            this.groupBoxProcessing.TabStop = false;
            this.groupBoxProcessing.Text = "价格数据补充处理";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.Controls.Add(this.btnSupplementPrice, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.progressBar, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.statusLabel, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1074, 44);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // btnSupplementPrice
            // 
            this.btnSupplementPrice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSupplementPrice.BackColor = System.Drawing.Color.Orange;
            this.btnSupplementPrice.Enabled = false;
            this.btnSupplementPrice.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSupplementPrice.Location = new System.Drawing.Point(10, 7);
            this.btnSupplementPrice.Name = "btnSupplementPrice";
            this.btnSupplementPrice.Size = new System.Drawing.Size(130, 30);
            this.btnSupplementPrice.TabIndex = 0;
            this.btnSupplementPrice.Text = "价格数据估计补充";
            this.btnSupplementPrice.UseVisualStyleBackColor = false;
            this.btnSupplementPrice.Click += new System.EventHandler(this.btnSupplementPrice_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(153, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(638, 20);
            this.progressBar.TabIndex = 1;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(797, 10);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(46, 24);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "就绪";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Location = new System.Drawing.Point(997, 10);
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
            this.tabPagePriceMapping.Size = new System.Drawing.Size(1092, 663);
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
            this.splitContainer1.Size = new System.Drawing.Size(1086, 657);
            this.splitContainer1.SplitterDistance = 200;
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
            this.groupBoxTemplate.Size = new System.Drawing.Size(1086, 200);
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
            this.txtTemplate.Size = new System.Drawing.Size(1074, 125);
            this.txtTemplate.TabIndex = 0;
            this.txtTemplate.Text = "价格映射表格式说明：\r\n第1列：行政区名称\r\n第2列：行政区代码（6位数字）\r\n第3-7列：1-5级林地价格（万元/公顷）\r\n\r\n文件格式：CSV格式，逗号分隔\r\n编码要求：UTF-8编码";
            // 
            // btnExportTemplate
            // 
            this.btnExportTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportTemplate.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnExportTemplate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExportTemplate.ForeColor = System.Drawing.Color.White;
            this.btnExportTemplate.Location = new System.Drawing.Point(980, 161);
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
            this.groupBoxMappingData.Size = new System.Drawing.Size(1086, 453);
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
            this.dataGridViewPriceMapping.Size = new System.Drawing.Size(1074, 387);
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
            // ForestPriceSupplementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.mainTabControl);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ForestPriceSupplementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "森林价格数据补充处理";
            this.mainTabControl.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxDataInput.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBoxCountyData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCounties)).EndInit();
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
        private System.Windows.Forms.TextBox txtInputPath;
        private System.Windows.Forms.Button btnBrowseInputPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutputPath;
        private System.Windows.Forms.Button btnImportPriceMapping;
        private System.Windows.Forms.Button btnScanData;
        private System.Windows.Forms.GroupBox groupBoxCountyData;
        private System.Windows.Forms.DataGridView dataGridViewCounties;
        private System.Windows.Forms.GroupBox groupBoxProcessing;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnSupplementPrice;
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