namespace ForestResourcePlugin
{
    partial class Basic
    {
        private System.ComponentModel.IContainer components = null;

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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDataSource = new System.Windows.Forms.TabPage();
            this.groupBoxFiles = new System.Windows.Forms.GroupBox();
            this.cbxSelectAllCounty = new System.Windows.Forms.CheckBox();
            this.btnRefreshLayers = new System.Windows.Forms.Button();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.lblCZKFBJ = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.chkListCounties = new System.Windows.Forms.CheckedListBox();
            this.cmbCZKFBJPath = new System.Windows.Forms.ComboBox();
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
            this.tabDataSource.Controls.Add(this.groupBoxFiles);
            this.tabDataSource.Controls.Add(this.groupBoxOptions);
            this.tabDataSource.Location = new System.Drawing.Point(4, 28);
            this.tabDataSource.Margin = new System.Windows.Forms.Padding(4);
            this.tabDataSource.Name = "tabDataSource";
            this.tabDataSource.Padding = new System.Windows.Forms.Padding(4);
            this.tabDataSource.Size = new System.Drawing.Size(1188, 590);
            this.tabDataSource.TabIndex = 0;
            this.tabDataSource.Text = "����Դ����";
            this.tabDataSource.UseVisualStyleBackColor = true;
            // 
            // groupBoxFiles
            // 
            this.groupBoxFiles.Controls.Add(this.cbxSelectAllCounty);
            this.groupBoxFiles.Controls.Add(this.btnRefreshLayers);
            this.groupBoxFiles.Controls.Add(this.btnBrowseOutput);
            this.groupBoxFiles.Controls.Add(this.txtOutputPath);
            this.groupBoxFiles.Controls.Add(this.lblCZKFBJ);
            this.groupBoxFiles.Controls.Add(this.lblOutput);
            this.groupBoxFiles.Controls.Add(this.chkListCounties);
            this.groupBoxFiles.Controls.Add(this.cmbCZKFBJPath);
            this.groupBoxFiles.Location = new System.Drawing.Point(9, 8);
            this.groupBoxFiles.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxFiles.Name = "groupBoxFiles";
            this.groupBoxFiles.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxFiles.Size = new System.Drawing.Size(1170, 240);
            this.groupBoxFiles.TabIndex = 0;
            this.groupBoxFiles.TabStop = false;
            this.groupBoxFiles.Text = "�ļ�ѡ��";
            // 
            // cbxSelectAllCounty
            // 
            this.cbxSelectAllCounty.AutoSize = true;
            this.cbxSelectAllCounty.Location = new System.Drawing.Point(1031, 40);
            this.cbxSelectAllCounty.Name = "cbxSelectAllCounty";
            this.cbxSelectAllCounty.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbxSelectAllCounty.Size = new System.Drawing.Size(70, 22);
            this.cbxSelectAllCounty.TabIndex = 11;
            this.cbxSelectAllCounty.Text = "ȫѡ";
            this.cbxSelectAllCounty.UseVisualStyleBackColor = true;
            this.cbxSelectAllCounty.Click += new System.EventHandler(this.cbxSelectAllCounty_Click);
            // 
            // btnRefreshLayers
            // 
            this.btnRefreshLayers.Location = new System.Drawing.Point(22, 201);
            this.btnRefreshLayers.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshLayers.Name = "btnRefreshLayers";
            this.btnRefreshLayers.Size = new System.Drawing.Size(150, 32);
            this.btnRefreshLayers.TabIndex = 9;
            this.btnRefreshLayers.Text = "ˢ������Դ";
            this.btnRefreshLayers.UseVisualStyleBackColor = true;
            this.btnRefreshLayers.Click += new System.EventHandler(this.btnRefreshLayers_Click);
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(596, 162);
            this.btnBrowseOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(112, 32);
            this.btnBrowseOutput.TabIndex = 8;
            this.btnBrowseOutput.Text = "���...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(210, 162);
            this.txtOutputPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(360, 28);
            this.txtOutputPath.TabIndex = 7;
            // 
            // lblCZKFBJ
            // 
            this.lblCZKFBJ.AutoSize = true;
            this.lblCZKFBJ.Location = new System.Drawing.Point(8, 82);
            this.lblCZKFBJ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCZKFBJ.Name = "lblCZKFBJ";
            this.lblCZKFBJ.Size = new System.Drawing.Size(197, 18);
            this.lblCZKFBJ.TabIndex = 3;
            this.lblCZKFBJ.Text = "���򿪷��߽�(CZKFBJ):";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(14, 166);
            this.lblOutput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(89, 18);
            this.lblOutput.TabIndex = 6;
            this.lblOutput.Text = "���·��:";
            // 
            // chkListCounties
            // 
            this.chkListCounties.CheckOnClick = true;
            this.chkListCounties.Location = new System.Drawing.Point(750, 38);
            this.chkListCounties.Margin = new System.Windows.Forms.Padding(4);
            this.chkListCounties.Name = "chkListCounties";
            this.chkListCounties.Size = new System.Drawing.Size(274, 165);
            this.chkListCounties.TabIndex = 10;
            // 
            // cmbCZKFBJPath
            // 
            this.cmbCZKFBJPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCZKFBJPath.FormattingEnabled = true;
            this.cmbCZKFBJPath.Location = new System.Drawing.Point(210, 80);
            this.cmbCZKFBJPath.Margin = new System.Windows.Forms.Padding(4);
            this.cmbCZKFBJPath.Name = "cmbCZKFBJPath";
            this.cmbCZKFBJPath.Size = new System.Drawing.Size(523, 26);
            this.cmbCZKFBJPath.TabIndex = 4;
            this.cmbCZKFBJPath.SelectedIndexChanged += new System.EventHandler(this.cmbCZKFBJPath_SelectedIndexChanged);
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
            this.groupBoxOptions.Text = "����ѡ��";
            // 
            // chkTopologyCheck
            // 
            this.chkTopologyCheck.AutoSize = true;
            this.chkTopologyCheck.Location = new System.Drawing.Point(22, 34);
            this.chkTopologyCheck.Margin = new System.Windows.Forms.Padding(4);
            this.chkTopologyCheck.Name = "chkTopologyCheck";
            this.chkTopologyCheck.Size = new System.Drawing.Size(214, 22);
            this.chkTopologyCheck.TabIndex = 0;
            this.chkTopologyCheck.Text = "������ɺ���ӵ���ͼ";
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
            this.chkGeometryValidation.Text = "������֤";
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
            this.chkCreateBackup.Text = "������֤";
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
            this.chkGenerateReport.Text = "���ɱ���";
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
            this.tabFilter.Text = "ɸѡ��������";
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
            this.groupBoxFieldSelect.Text = "�ֶ�ѡ��";
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
            this.lblLandTypeField.Text = "��������ֶ�:";
            // 
            // lblLandOwnerField
            // 
            this.lblLandOwnerField.AutoSize = true;
            this.lblLandOwnerField.Location = new System.Drawing.Point(525, 39);
            this.lblLandOwnerField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLandOwnerField.Name = "lblLandOwnerField";
            this.lblLandOwnerField.Size = new System.Drawing.Size(125, 18);
            this.lblLandOwnerField.TabIndex = 2;
            this.lblLandOwnerField.Text = "����Ȩ���ֶ�:";
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
            this.groupBoxConditions.Text = "ɸѡ����";
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
            this.chkForestLand.Text = "����Ϊ�ֵ�";
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
            this.chkStateOwned.Text = "����Ȩ������Ϊ\"����\"";
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
            this.chkCollectiveInBoundary.Text = "����Ȩ������Ϊ\"����\"��λ�ڿ����߽���";
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
            this.groupBoxPreview.Text = "Ԥ�����";
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
            this.btnPreview.Text = "����Ԥ��";
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
            this.lblPreviewCount.Text = "Ԥ�������0 ��ͼ��";
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
            this.tabMapping.Text = "�ֶ�ӳ������";
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
            this.groupBoxMapping.Text = "�ֶ�ӳ��";
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
            this.btnAutoMapping.Text = "�Զ�ӳ��";
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
            this.groupBoxTemplate.Text = "ӳ��ģ��";
            // 
            // btnLoadTemplate
            // 
            this.btnLoadTemplate.Location = new System.Drawing.Point(22, 34);
            this.btnLoadTemplate.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadTemplate.Name = "btnLoadTemplate";
            this.btnLoadTemplate.Size = new System.Drawing.Size(150, 32);
            this.btnLoadTemplate.TabIndex = 0;
            this.btnLoadTemplate.Text = "����ģ��";
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
            this.btnSaveTemplate.Text = "����ģ��";
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
            this.lblTemplateInfo.Text = "��ǰģ�壺δ����ģ��";
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
            this.lblTotalStatus.Text = "��״̬��";
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
            this.lblStatus.Text = "״̬������";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(822, 16);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(112, 32);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "ִ��";
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
            this.btnCancel.Text = "ȡ��";
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
            this.btnClose.Text = "�ر�";
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
            this.Text = "�������";
            this.Load += new System.EventHandler(this.Basic_Load);
            this.tabControl.ResumeLayout(false);
            this.tabDataSource.ResumeLayout(false);
            this.groupBoxFiles.ResumeLayout(false);
            this.groupBoxFiles.PerformLayout();
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

        // ����Դ����ҳ�ؼ�
        private System.Windows.Forms.GroupBox groupBoxFiles;
        private System.Windows.Forms.Button btnRefreshLayers; // ˢ��ͼ�㰴ť
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label lblCZKFBJ;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.CheckedListBox chkListCounties;
        private System.Windows.Forms.ComboBox cmbCZKFBJPath;

        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.CheckBox chkTopologyCheck;
        private System.Windows.Forms.CheckBox chkGeometryValidation;
        private System.Windows.Forms.CheckBox chkCreateBackup;
        private System.Windows.Forms.CheckBox chkGenerateReport;
        private System.Windows.Forms.Label lblBufferDistance;

        // ɸѡ����ҳ�ؼ�
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

        // �ֶ�ӳ��ҳ�ؼ�
        private System.Windows.Forms.GroupBox groupBoxMapping;
        private System.Windows.Forms.DataGridView dgvMapping;
        private System.Windows.Forms.Button btnAutoMapping;

        private System.Windows.Forms.GroupBox groupBoxTemplate;
        private System.Windows.Forms.Button btnLoadTemplate;
        private System.Windows.Forms.Button btnSaveTemplate;
        private System.Windows.Forms.Label lblTemplateInfo;

        // �ײ��������
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox cbxSelectAllCounty;
        private System.Windows.Forms.Label lblTotalStatus;
        private System.Windows.Forms.ProgressBar totalProgressBar;
    }
}
