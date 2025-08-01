namespace ForestResourcePlugin
{
    partial class Basic
    {
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDataSource = new System.Windows.Forms.TabPage();
            this.buttonA6 = new System.Windows.Forms.Button();
            this.buttonA4 = new System.Windows.Forms.Button();
            this.buttonForestExcel = new System.Windows.Forms.Button();
            this.groupBoxFiles = new System.Windows.Forms.GroupBox();
            this.grpDataType = new System.Windows.Forms.GroupBox();
            this.chkForest = new System.Windows.Forms.CheckBox();
            this.chkGrassland = new System.Windows.Forms.CheckBox();
            this.chkWetland = new System.Windows.Forms.CheckBox();
            this.buttonHSJGPath = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxSelectAllCounty = new System.Windows.Forms.CheckBox();
            this.btnRefreshLayers = new System.Windows.Forms.Button();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.chkListCounties = new System.Windows.Forms.CheckedListBox();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.chkTopologyCheck = new System.Windows.Forms.CheckBox();
            this.chkGeometryValidation = new System.Windows.Forms.CheckBox();
            this.chkCreateBackup = new System.Windows.Forms.CheckBox();
            this.chkGenerateReport = new System.Windows.Forms.CheckBox();
            this.lblBufferDistance = new System.Windows.Forms.Label();
            this.tabFilter = new System.Windows.Forms.TabPage();
            this.groupBoxFieldSelect = new System.Windows.Forms.GroupBox();
            this.cmbLandTypeField = new System.Windows.Forms.ComboBox();
            this.cmbLandOwnerField = new System.Windows.Forms.ComboBox();
            this.lblLandTypeField = new System.Windows.Forms.Label();
            this.lblLandOwnerField = new System.Windows.Forms.Label();
            this.groupBoxConditions = new System.Windows.Forms.GroupBox();
            this.chkForestLand = new System.Windows.Forms.CheckBox();
            this.chkStateOwned = new System.Windows.Forms.CheckBox();
            this.chkCollectiveInBoundary = new System.Windows.Forms.CheckBox();
            this.groupBoxPreview = new System.Windows.Forms.GroupBox();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.btnPreview = new System.Windows.Forms.Button();
            this.lblPreviewCount = new System.Windows.Forms.Label();
            this.tabMapping = new System.Windows.Forms.TabPage();
            this.groupBoxMapping = new System.Windows.Forms.GroupBox();
            this.dgvMapping = new System.Windows.Forms.DataGridView();
            this.btnAutoMapping = new System.Windows.Forms.Button();
            this.groupBoxTemplate = new System.Windows.Forms.GroupBox();
            this.btnLoadTemplate = new System.Windows.Forms.Button();
            this.btnSaveTemplate = new System.Windows.Forms.Button();
            this.lblTemplateInfo = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.totalProgressBar = new System.Windows.Forms.ProgressBar();
            this.lblTotalStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabDataSource.SuspendLayout();
            this.groupBoxFiles.SuspendLayout();
            this.grpDataType.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            this.tabFilter.SuspendLayout();
            this.groupBoxFieldSelect.SuspendLayout();
            this.groupBoxConditions.SuspendLayout();
            this.groupBoxPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.tabMapping.SuspendLayout();
            this.groupBoxMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapping)).BeginInit();
            this.groupBoxTemplate.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabDataSource);
            this.tabControl.Controls.Add(this.tabFilter);
            this.tabControl.Controls.Add(this.tabMapping);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1196, 622);
            this.tabControl.TabIndex = 0;
            // 
            // tabDataSource
            // 
            this.tabDataSource.Controls.Add(this.buttonA6);
            this.tabDataSource.Controls.Add(this.buttonA4);
            this.tabDataSource.Controls.Add(this.buttonForestExcel);
            this.tabDataSource.Controls.Add(this.groupBoxFiles);
            this.tabDataSource.Controls.Add(this.groupBoxOptions);
            this.tabDataSource.Location = new System.Drawing.Point(4, 28);
            this.tabDataSource.Margin = new System.Windows.Forms.Padding(4);
            this.tabDataSource.Name = "tabDataSource";
            this.tabDataSource.Padding = new System.Windows.Forms.Padding(4);
            this.tabDataSource.Size = new System.Drawing.Size(1188, 590);
            this.tabDataSource.TabIndex = 0;
            this.tabDataSource.Text = "数据源配置";
            this.tabDataSource.UseVisualStyleBackColor = true;
            // 
            // buttonA6
            // 
            this.buttonA6.Location = new System.Drawing.Point(940, 364);
            this.buttonA6.Margin = new System.Windows.Forms.Padding(4);
            this.buttonA6.Name = "buttonA6";
            this.buttonA6.Size = new System.Drawing.Size(233, 41);
            this.buttonA6.TabIndex = 7;
            this.buttonA6.Text = "导出A6数据表格";
            this.buttonA6.UseVisualStyleBackColor = true;
            this.buttonA6.Click += new System.EventHandler(this.buttonA6_Click);
            // 
            // buttonA4
            // 
            this.buttonA4.Location = new System.Drawing.Point(940, 314);
            this.buttonA4.Margin = new System.Windows.Forms.Padding(4);
            this.buttonA4.Name = "buttonA4";
            this.buttonA4.Size = new System.Drawing.Size(233, 41);
            this.buttonA4.TabIndex = 6;
            this.buttonA4.Text = "导出A4数据表格";
            this.buttonA4.UseVisualStyleBackColor = true;
            this.buttonA4.Click += new System.EventHandler(this.buttonA4_Click);
            // 
            // buttonForestExcel
            // 
            this.buttonForestExcel.Location = new System.Drawing.Point(940, 258);
            this.buttonForestExcel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonForestExcel.Name = "buttonForestExcel";
            this.buttonForestExcel.Size = new System.Drawing.Size(233, 41);
            this.buttonForestExcel.TabIndex = 5;
            this.buttonForestExcel.Text = "导出A2数据表格";
            this.buttonForestExcel.UseVisualStyleBackColor = true;
            this.buttonForestExcel.Click += new System.EventHandler(this.buttonForestExcel_Click);
            // 
            // groupBoxFiles
            // 
            this.groupBoxFiles.Controls.Add(this.grpDataType);
            this.groupBoxFiles.Controls.Add(this.buttonHSJGPath);
            this.groupBoxFiles.Controls.Add(this.textBox1);
            this.groupBoxFiles.Controls.Add(this.label1);
            this.groupBoxFiles.Controls.Add(this.cbxSelectAllCounty);
            this.groupBoxFiles.Controls.Add(this.btnRefreshLayers);
            this.groupBoxFiles.Controls.Add(this.btnBrowseOutput);
            this.groupBoxFiles.Controls.Add(this.txtOutputPath);
            this.groupBoxFiles.Controls.Add(this.lblOutput);
            this.groupBoxFiles.Controls.Add(this.chkListCounties);
            this.groupBoxFiles.Location = new System.Drawing.Point(9, 8);
            this.groupBoxFiles.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxFiles.Name = "groupBoxFiles";
            this.groupBoxFiles.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxFiles.Size = new System.Drawing.Size(1170, 240);
            this.groupBoxFiles.TabIndex = 0;
            this.groupBoxFiles.TabStop = false;
            this.groupBoxFiles.Text = "文件选择";
            // 
            // grpDataType
            // 
            this.grpDataType.Controls.Add(this.chkForest);
            this.grpDataType.Controls.Add(this.chkGrassland);
            this.grpDataType.Controls.Add(this.chkWetland);
            this.grpDataType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpDataType.Location = new System.Drawing.Point(365, 114);
            this.grpDataType.Margin = new System.Windows.Forms.Padding(4);
            this.grpDataType.Name = "grpDataType";
            this.grpDataType.Padding = new System.Windows.Forms.Padding(4);
            this.grpDataType.Size = new System.Drawing.Size(377, 70);
            this.grpDataType.TabIndex = 4;
            this.grpDataType.TabStop = false;
            this.grpDataType.Text = "数据类型选择";
            // 
            // chkForest
            // 
            this.chkForest.AutoSize = true;
            this.chkForest.Checked = true;
            this.chkForest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkForest.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkForest.Location = new System.Drawing.Point(20, 30);
            this.chkForest.Margin = new System.Windows.Forms.Padding(4);
            this.chkForest.Name = "chkForest";
            this.chkForest.Size = new System.Drawing.Size(70, 22);
            this.chkForest.TabIndex = 0;
            this.chkForest.Text = "林地";
            this.chkForest.UseVisualStyleBackColor = true;
            this.chkForest.CheckedChanged += new System.EventHandler(this.chkForest_CheckedChanged);
            // 
            // chkGrassland
            // 
            this.chkGrassland.AutoSize = true;
            this.chkGrassland.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkGrassland.Location = new System.Drawing.Point(150, 30);
            this.chkGrassland.Margin = new System.Windows.Forms.Padding(4);
            this.chkGrassland.Name = "chkGrassland";
            this.chkGrassland.Size = new System.Drawing.Size(70, 22);
            this.chkGrassland.TabIndex = 1;
            this.chkGrassland.Text = "草地";
            this.chkGrassland.UseVisualStyleBackColor = true;
            this.chkGrassland.CheckedChanged += new System.EventHandler(this.chkGrassland_CheckedChanged);
            // 
            // chkWetland
            // 
            this.chkWetland.AutoSize = true;
            this.chkWetland.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkWetland.Location = new System.Drawing.Point(280, 30);
            this.chkWetland.Margin = new System.Windows.Forms.Padding(4);
            this.chkWetland.Name = "chkWetland";
            this.chkWetland.Size = new System.Drawing.Size(70, 22);
            this.chkWetland.TabIndex = 2;
            this.chkWetland.Text = "湿地";
            this.chkWetland.UseVisualStyleBackColor = true;
            this.chkWetland.CheckedChanged += new System.EventHandler(this.chkWetland_CheckedChanged);
            // 
            // buttonHSJGPath
            // 
            this.buttonHSJGPath.Location = new System.Drawing.Point(596, 34);
            this.buttonHSJGPath.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHSJGPath.Name = "buttonHSJGPath";
            this.buttonHSJGPath.Size = new System.Drawing.Size(112, 32);
            this.buttonHSJGPath.TabIndex = 2;
            this.buttonHSJGPath.Text = "浏览...";
            this.buttonHSJGPath.UseVisualStyleBackColor = true;
            this.buttonHSJGPath.Click += new System.EventHandler(this.buttonHSJGPath_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(210, 35);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(360, 28);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "核算价格(HSJG)路径:";
            // 
            // cbxSelectAllCounty
            // 
            this.cbxSelectAllCounty.AutoSize = true;
            this.cbxSelectAllCounty.Location = new System.Drawing.Point(1031, 40);
            this.cbxSelectAllCounty.Name = "cbxSelectAllCounty";
            this.cbxSelectAllCounty.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbxSelectAllCounty.Size = new System.Drawing.Size(70, 22);
            this.cbxSelectAllCounty.TabIndex = 7;
            this.cbxSelectAllCounty.Text = "全选";
            this.cbxSelectAllCounty.UseVisualStyleBackColor = true;
            this.cbxSelectAllCounty.Click += new System.EventHandler(this.cbxSelectAllCounty_Click);
            // 
            // btnRefreshLayers
            // 
            this.btnRefreshLayers.Location = new System.Drawing.Point(22, 201);
            this.btnRefreshLayers.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshLayers.Name = "btnRefreshLayers";
            this.btnRefreshLayers.Size = new System.Drawing.Size(150, 32);
            this.btnRefreshLayers.TabIndex = 5;
            this.btnRefreshLayers.Text = "刷新数据源";
            this.btnRefreshLayers.UseVisualStyleBackColor = true;
            this.btnRefreshLayers.Click += new System.EventHandler(this.btnRefreshLayers_Click);
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(596, 74);
            this.btnBrowseOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(112, 32);
            this.btnBrowseOutput.TabIndex = 3;
            this.btnBrowseOutput.Text = "浏览...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(210, 77);
            this.txtOutputPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(360, 28);
            this.txtOutputPath.TabIndex = 2;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(14, 80);
            this.lblOutput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(89, 18);
            this.lblOutput.TabIndex = 1;
            this.lblOutput.Text = "输出路径:";
            // 
            // chkListCounties
            // 
            this.chkListCounties.CheckOnClick = true;
            this.chkListCounties.Location = new System.Drawing.Point(750, 38);
            this.chkListCounties.Margin = new System.Windows.Forms.Padding(4);
            this.chkListCounties.Name = "chkListCounties";
            this.chkListCounties.Size = new System.Drawing.Size(274, 154);
            this.chkListCounties.TabIndex = 6;
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.chkTopologyCheck);
            this.groupBoxOptions.Controls.Add(this.chkGeometryValidation);
            this.groupBoxOptions.Controls.Add(this.chkCreateBackup);
            this.groupBoxOptions.Controls.Add(this.chkGenerateReport);
            this.groupBoxOptions.Controls.Add(this.lblBufferDistance);
            this.groupBoxOptions.Location = new System.Drawing.Point(9, 258);
            this.groupBoxOptions.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxOptions.Size = new System.Drawing.Size(460, 194);
            this.groupBoxOptions.TabIndex = 1;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "处理选项";
            // 
            // chkTopologyCheck
            // 
            this.chkTopologyCheck.AutoSize = true;
            this.chkTopologyCheck.Location = new System.Drawing.Point(22, 34);
            this.chkTopologyCheck.Margin = new System.Windows.Forms.Padding(4);
            this.chkTopologyCheck.Name = "chkTopologyCheck";
            this.chkTopologyCheck.Size = new System.Drawing.Size(214, 22);
            this.chkTopologyCheck.TabIndex = 0;
            this.chkTopologyCheck.Text = "处理完成后添加到地图";
            this.chkTopologyCheck.UseVisualStyleBackColor = true;
            // 
            // chkGeometryValidation
            // 
            this.chkGeometryValidation.AutoSize = true;
            this.chkGeometryValidation.Location = new System.Drawing.Point(22, 66);
            this.chkGeometryValidation.Margin = new System.Windows.Forms.Padding(4);
            this.chkGeometryValidation.Name = "chkGeometryValidation";
            this.chkGeometryValidation.Size = new System.Drawing.Size(106, 22);
            this.chkGeometryValidation.TabIndex = 1;
            this.chkGeometryValidation.Text = "几何验证";
            this.chkGeometryValidation.UseVisualStyleBackColor = true;
            // 
            // chkCreateBackup
            // 
            this.chkCreateBackup.AutoSize = true;
            this.chkCreateBackup.Location = new System.Drawing.Point(300, 34);
            this.chkCreateBackup.Margin = new System.Windows.Forms.Padding(4);
            this.chkCreateBackup.Name = "chkCreateBackup";
            this.chkCreateBackup.Size = new System.Drawing.Size(106, 22);
            this.chkCreateBackup.TabIndex = 2;
            this.chkCreateBackup.Text = "数据验证";
            this.chkCreateBackup.UseVisualStyleBackColor = true;
            // 
            // chkGenerateReport
            // 
            this.chkGenerateReport.AutoSize = true;
            this.chkGenerateReport.Location = new System.Drawing.Point(300, 66);
            this.chkGenerateReport.Margin = new System.Windows.Forms.Padding(4);
            this.chkGenerateReport.Name = "chkGenerateReport";
            this.chkGenerateReport.Size = new System.Drawing.Size(106, 22);
            this.chkGenerateReport.TabIndex = 3;
            this.chkGenerateReport.Text = "生成报告";
            this.chkGenerateReport.UseVisualStyleBackColor = true;
            // 
            // lblBufferDistance
            // 
            this.lblBufferDistance.AutoSize = true;
            this.lblBufferDistance.Location = new System.Drawing.Point(22, 106);
            this.lblBufferDistance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBufferDistance.Name = "lblBufferDistance";
            this.lblBufferDistance.Size = new System.Drawing.Size(0, 18);
            this.lblBufferDistance.TabIndex = 4;
            // 
            // tabFilter
            // 
            this.tabFilter.Controls.Add(this.groupBoxFieldSelect);
            this.tabFilter.Controls.Add(this.groupBoxConditions);
            this.tabFilter.Controls.Add(this.groupBoxPreview);
            this.tabFilter.Location = new System.Drawing.Point(4, 28);
            this.tabFilter.Margin = new System.Windows.Forms.Padding(4);
            this.tabFilter.Name = "tabFilter";
            this.tabFilter.Padding = new System.Windows.Forms.Padding(4);
            this.tabFilter.Size = new System.Drawing.Size(1188, 590);
            this.tabFilter.TabIndex = 1;
            this.tabFilter.Text = "筛选条件设置";
            this.tabFilter.UseVisualStyleBackColor = true;
            // 
            // groupBoxFieldSelect
            // 
            this.groupBoxFieldSelect.Controls.Add(this.cmbLandTypeField);
            this.groupBoxFieldSelect.Controls.Add(this.cmbLandOwnerField);
            this.groupBoxFieldSelect.Controls.Add(this.lblLandTypeField);
            this.groupBoxFieldSelect.Controls.Add(this.lblLandOwnerField);
            this.groupBoxFieldSelect.Location = new System.Drawing.Point(9, 8);
            this.groupBoxFieldSelect.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxFieldSelect.Name = "groupBoxFieldSelect";
            this.groupBoxFieldSelect.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxFieldSelect.Size = new System.Drawing.Size(1170, 111);
            this.groupBoxFieldSelect.TabIndex = 0;
            this.groupBoxFieldSelect.TabStop = false;
            this.groupBoxFieldSelect.Text = "字段选择";
            // 
            // cmbLandTypeField
            // 
            this.cmbLandTypeField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLandTypeField.FormattingEnabled = true;
            this.cmbLandTypeField.Location = new System.Drawing.Point(167, 34);
            this.cmbLandTypeField.Margin = new System.Windows.Forms.Padding(4);
            this.cmbLandTypeField.Name = "cmbLandTypeField";
            this.cmbLandTypeField.Size = new System.Drawing.Size(298, 26);
            this.cmbLandTypeField.TabIndex = 1;
            // 
            // cmbLandOwnerField
            // 
            this.cmbLandOwnerField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLandOwnerField.FormattingEnabled = true;
            this.cmbLandOwnerField.Location = new System.Drawing.Point(675, 34);
            this.cmbLandOwnerField.Margin = new System.Windows.Forms.Padding(4);
            this.cmbLandOwnerField.Name = "cmbLandOwnerField";
            this.cmbLandOwnerField.Size = new System.Drawing.Size(298, 26);
            this.cmbLandOwnerField.TabIndex = 3;
            // 
            // lblLandTypeField
            // 
            this.lblLandTypeField.AutoSize = true;
            this.lblLandTypeField.Location = new System.Drawing.Point(22, 39);
            this.lblLandTypeField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLandTypeField.Name = "lblLandTypeField";
            this.lblLandTypeField.Size = new System.Drawing.Size(125, 18);
            this.lblLandTypeField.TabIndex = 0;
            this.lblLandTypeField.Text = "地类编码字段:";
            // 
            // lblLandOwnerField
            // 
            this.lblLandOwnerField.AutoSize = true;
            this.lblLandOwnerField.Location = new System.Drawing.Point(525, 39);
            this.lblLandOwnerField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLandOwnerField.Name = "lblLandOwnerField";
            this.lblLandOwnerField.Size = new System.Drawing.Size(125, 18);
            this.lblLandOwnerField.TabIndex = 2;
            this.lblLandOwnerField.Text = "土地权属字段:";
            // 
            // groupBoxConditions
            // 
            this.groupBoxConditions.Controls.Add(this.chkForestLand);
            this.groupBoxConditions.Controls.Add(this.chkStateOwned);
            this.groupBoxConditions.Controls.Add(this.chkCollectiveInBoundary);
            this.groupBoxConditions.Location = new System.Drawing.Point(9, 128);
            this.groupBoxConditions.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxConditions.Name = "groupBoxConditions";
            this.groupBoxConditions.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxConditions.Size = new System.Drawing.Size(1170, 111);
            this.groupBoxConditions.TabIndex = 1;
            this.groupBoxConditions.TabStop = false;
            this.groupBoxConditions.Text = "筛选条件";
            // 
            // chkForestLand
            // 
            this.chkForestLand.AutoSize = true;
            this.chkForestLand.Checked = true;
            this.chkForestLand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkForestLand.Location = new System.Drawing.Point(22, 34);
            this.chkForestLand.Margin = new System.Windows.Forms.Padding(4);
            this.chkForestLand.Name = "chkForestLand";
            this.chkForestLand.Size = new System.Drawing.Size(124, 22);
            this.chkForestLand.TabIndex = 0;
            this.chkForestLand.Text = "地类为林地";
            this.chkForestLand.UseVisualStyleBackColor = true;
            // 
            // chkStateOwned
            // 
            this.chkStateOwned.AutoSize = true;
            this.chkStateOwned.Checked = true;
            this.chkStateOwned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStateOwned.Location = new System.Drawing.Point(22, 66);
            this.chkStateOwned.Margin = new System.Windows.Forms.Padding(4);
            this.chkStateOwned.Name = "chkStateOwned";
            this.chkStateOwned.Size = new System.Drawing.Size(214, 22);
            this.chkStateOwned.TabIndex = 1;
            this.chkStateOwned.Text = "土地权属性质为\"国有\"";
            this.chkStateOwned.UseVisualStyleBackColor = true;
            // 
            // chkCollectiveInBoundary
            // 
            this.chkCollectiveInBoundary.AutoSize = true;
            this.chkCollectiveInBoundary.Location = new System.Drawing.Point(375, 66);
            this.chkCollectiveInBoundary.Margin = new System.Windows.Forms.Padding(4);
            this.chkCollectiveInBoundary.Name = "chkCollectiveInBoundary";
            this.chkCollectiveInBoundary.Size = new System.Drawing.Size(358, 22);
            this.chkCollectiveInBoundary.TabIndex = 2;
            this.chkCollectiveInBoundary.Text = "土地权属性质为\"集体\"且位于开发边界内";
            this.chkCollectiveInBoundary.UseVisualStyleBackColor = true;
            // 
            // groupBoxPreview
            // 
            this.groupBoxPreview.Controls.Add(this.dgvPreview);
            this.groupBoxPreview.Controls.Add(this.btnPreview);
            this.groupBoxPreview.Controls.Add(this.lblPreviewCount);
            this.groupBoxPreview.Location = new System.Drawing.Point(9, 246);
            this.groupBoxPreview.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxPreview.Size = new System.Drawing.Size(1170, 332);
            this.groupBoxPreview.TabIndex = 2;
            this.groupBoxPreview.TabStop = false;
            this.groupBoxPreview.Text = "预览结果";
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.Location = new System.Drawing.Point(22, 34);
            this.dgvPreview.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.RowHeadersWidth = 62;
            this.dgvPreview.Size = new System.Drawing.Size(1125, 249);
            this.dgvPreview.TabIndex = 0;
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(22, 292);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(150, 32);
            this.btnPreview.TabIndex = 1;
            this.btnPreview.Text = "生成预览";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.MouseCaptureChanged += new System.EventHandler(this.btnPreview_MouseCaptureChanged);
            // 
            // lblPreviewCount
            // 
            this.lblPreviewCount.AutoSize = true;
            this.lblPreviewCount.Location = new System.Drawing.Point(195, 298);
            this.lblPreviewCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreviewCount.Name = "lblPreviewCount";
            this.lblPreviewCount.Size = new System.Drawing.Size(170, 18);
            this.lblPreviewCount.TabIndex = 2;
            this.lblPreviewCount.Text = "预览结果：0 个图斑";
            // 
            // tabMapping
            // 
            this.tabMapping.Controls.Add(this.groupBoxMapping);
            this.tabMapping.Controls.Add(this.groupBoxTemplate);
            this.tabMapping.Location = new System.Drawing.Point(4, 28);
            this.tabMapping.Margin = new System.Windows.Forms.Padding(4);
            this.tabMapping.Name = "tabMapping";
            this.tabMapping.Size = new System.Drawing.Size(1188, 590);
            this.tabMapping.TabIndex = 2;
            this.tabMapping.Text = "字段映射配置";
            this.tabMapping.UseVisualStyleBackColor = true;
            // 
            // groupBoxMapping
            // 
            this.groupBoxMapping.Controls.Add(this.dgvMapping);
            this.groupBoxMapping.Controls.Add(this.btnAutoMapping);
            this.groupBoxMapping.Location = new System.Drawing.Point(9, 8);
            this.groupBoxMapping.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxMapping.Name = "groupBoxMapping";
            this.groupBoxMapping.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxMapping.Size = new System.Drawing.Size(1170, 442);
            this.groupBoxMapping.TabIndex = 0;
            this.groupBoxMapping.TabStop = false;
            this.groupBoxMapping.Text = "字段映射";
            // 
            // dgvMapping
            // 
            this.dgvMapping.AllowUserToAddRows = false;
            this.dgvMapping.AllowUserToDeleteRows = false;
            this.dgvMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMapping.Location = new System.Drawing.Point(22, 34);
            this.dgvMapping.Margin = new System.Windows.Forms.Padding(4);
            this.dgvMapping.Name = "dgvMapping";
            this.dgvMapping.RowHeadersWidth = 62;
            this.dgvMapping.Size = new System.Drawing.Size(1125, 360);
            this.dgvMapping.TabIndex = 0;
            // 
            // btnAutoMapping
            // 
            this.btnAutoMapping.Location = new System.Drawing.Point(22, 404);
            this.btnAutoMapping.Margin = new System.Windows.Forms.Padding(4);
            this.btnAutoMapping.Name = "btnAutoMapping";
            this.btnAutoMapping.Size = new System.Drawing.Size(150, 32);
            this.btnAutoMapping.TabIndex = 1;
            this.btnAutoMapping.Text = "自动映射";
            this.btnAutoMapping.UseVisualStyleBackColor = true;
            this.btnAutoMapping.Click += new System.EventHandler(this.btnAutoMapping_Click);
            // 
            // groupBoxTemplate
            // 
            this.groupBoxTemplate.Controls.Add(this.btnLoadTemplate);
            this.groupBoxTemplate.Controls.Add(this.btnSaveTemplate);
            this.groupBoxTemplate.Controls.Add(this.lblTemplateInfo);
            this.groupBoxTemplate.Location = new System.Drawing.Point(9, 460);
            this.groupBoxTemplate.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxTemplate.Name = "groupBoxTemplate";
            this.groupBoxTemplate.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxTemplate.Size = new System.Drawing.Size(1170, 111);
            this.groupBoxTemplate.TabIndex = 1;
            this.groupBoxTemplate.TabStop = false;
            this.groupBoxTemplate.Text = "映射模板";
            // 
            // btnLoadTemplate
            // 
            this.btnLoadTemplate.Location = new System.Drawing.Point(22, 34);
            this.btnLoadTemplate.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadTemplate.Name = "btnLoadTemplate";
            this.btnLoadTemplate.Size = new System.Drawing.Size(150, 32);
            this.btnLoadTemplate.TabIndex = 0;
            this.btnLoadTemplate.Text = "加载模板";
            this.btnLoadTemplate.UseVisualStyleBackColor = true;
            this.btnLoadTemplate.Click += new System.EventHandler(this.btnLoadTemplate_Click);
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Location = new System.Drawing.Point(195, 34);
            this.btnSaveTemplate.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(150, 32);
            this.btnSaveTemplate.TabIndex = 1;
            this.btnSaveTemplate.Text = "保存模板";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // lblTemplateInfo
            // 
            this.lblTemplateInfo.AutoSize = true;
            this.lblTemplateInfo.Location = new System.Drawing.Point(22, 76);
            this.lblTemplateInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTemplateInfo.Name = "lblTemplateInfo";
            this.lblTemplateInfo.Size = new System.Drawing.Size(188, 18);
            this.lblTemplateInfo.TabIndex = 2;
            this.lblTemplateInfo.Text = "当前模板：未加载模板";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.totalProgressBar);
            this.panelBottom.Controls.Add(this.lblTotalStatus);
            this.panelBottom.Controls.Add(this.progressBar);
            this.panelBottom.Controls.Add(this.lblStatus);
            this.panelBottom.Controls.Add(this.btnExecute);
            this.panelBottom.Controls.Add(this.btnCancel);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 631);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(4);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1196, 111);
            this.panelBottom.TabIndex = 1;
            // 
            // totalProgressBar
            // 
            this.totalProgressBar.Location = new System.Drawing.Point(388, 84);
            this.totalProgressBar.Margin = new System.Windows.Forms.Padding(4);
            this.totalProgressBar.Name = "totalProgressBar";
            this.totalProgressBar.Size = new System.Drawing.Size(789, 22);
            this.totalProgressBar.TabIndex = 6;
            // 
            // lblTotalStatus
            // 
            this.lblTotalStatus.AutoSize = true;
            this.lblTotalStatus.Location = new System.Drawing.Point(18, 88);
            this.lblTotalStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalStatus.Name = "lblTotalStatus";
            this.lblTotalStatus.Size = new System.Drawing.Size(80, 18);
            this.lblTotalStatus.TabIndex = 5;
            this.lblTotalStatus.Text = "总状态：";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(18, 26);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(750, 22);
            this.progressBar.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(18, 58);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(98, 18);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "状态：就绪";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(822, 16);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(112, 32);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "执行";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(944, 16);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 32);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1065, 16);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(112, 32);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // Basic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 742);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Basic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据入库";
            this.Load += new System.EventHandler(this.Basic_Load);
            this.tabControl.ResumeLayout(false);
            this.tabDataSource.ResumeLayout(false);
            this.groupBoxFiles.ResumeLayout(false);
            this.groupBoxFiles.PerformLayout();
            this.grpDataType.ResumeLayout(false);
            this.grpDataType.PerformLayout();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.tabFilter.ResumeLayout(false);
            this.groupBoxFieldSelect.ResumeLayout(false);
            this.groupBoxFieldSelect.PerformLayout();
            this.groupBoxConditions.ResumeLayout(false);
            this.groupBoxConditions.PerformLayout();
            this.groupBoxPreview.ResumeLayout(false);
            this.groupBoxPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.tabMapping.ResumeLayout(false);
            this.groupBoxMapping.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapping)).EndInit();
            this.groupBoxTemplate.ResumeLayout(false);
            this.groupBoxTemplate.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDataSource;
        private System.Windows.Forms.TabPage tabFilter;
        private System.Windows.Forms.TabPage tabMapping;

        // 数据源配置页控件
        private System.Windows.Forms.GroupBox groupBoxFiles;
        private System.Windows.Forms.Button btnRefreshLayers; // 刷新图层按钮
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.CheckedListBox chkListCounties;

        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.CheckBox chkTopologyCheck;
        private System.Windows.Forms.CheckBox chkGeometryValidation;
        private System.Windows.Forms.CheckBox chkCreateBackup;
        private System.Windows.Forms.CheckBox chkGenerateReport;
        private System.Windows.Forms.Label lblBufferDistance;

        // 筛选条件页控件
        private System.Windows.Forms.GroupBox groupBoxFieldSelect;
        private System.Windows.Forms.ComboBox cmbLandTypeField;
        private System.Windows.Forms.ComboBox cmbLandOwnerField;
        private System.Windows.Forms.Label lblLandTypeField;
        private System.Windows.Forms.Label lblLandOwnerField;

        private System.Windows.Forms.GroupBox groupBoxConditions;
        private System.Windows.Forms.CheckBox chkForestLand;
        private System.Windows.Forms.CheckBox chkStateOwned;
        private System.Windows.Forms.CheckBox chkCollectiveInBoundary;

        private System.Windows.Forms.GroupBox groupBoxPreview;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Label lblPreviewCount;

        // 字段映射页控件
        private System.Windows.Forms.GroupBox groupBoxMapping;
        private System.Windows.Forms.DataGridView dgvMapping;
        private System.Windows.Forms.Button btnAutoMapping;

        private System.Windows.Forms.GroupBox groupBoxTemplate;
        private System.Windows.Forms.Button btnLoadTemplate;
        private System.Windows.Forms.Button btnSaveTemplate;
        private System.Windows.Forms.Label lblTemplateInfo;

        // 底部控制面板
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox cbxSelectAllCounty;
        private System.Windows.Forms.Label lblTotalStatus;
        private System.Windows.Forms.ProgressBar totalProgressBar;
        private System.Windows.Forms.Button buttonHSJGPath;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonForestExcel;
        private System.Windows.Forms.Button buttonA6;
        private System.Windows.Forms.Button buttonA4;

        // 数据类型选择控件组
        private System.Windows.Forms.GroupBox grpDataType;
        private System.Windows.Forms.CheckBox chkForest;
        private System.Windows.Forms.CheckBox chkGrassland;
        private System.Windows.Forms.CheckBox chkWetland;
    }
}