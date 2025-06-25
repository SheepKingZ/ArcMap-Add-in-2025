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
            this.btnBrowseLCXZGX = new System.Windows.Forms.Button();
            this.btnBrowseCZKFBJ = new System.Windows.Forms.Button();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtLCXZGXPath = new System.Windows.Forms.TextBox();
            this.txtCZKFBJPath = new System.Windows.Forms.TextBox();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.lblLCXZGX = new System.Windows.Forms.Label();
            this.lblCZKFBJ = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.chkTopologyCheck = new System.Windows.Forms.CheckBox();
            this.chkGeometryValidation = new System.Windows.Forms.CheckBox();
            this.chkCreateBackup = new System.Windows.Forms.CheckBox();
            this.chkGenerateReport = new System.Windows.Forms.CheckBox();
            this.numBufferDistance = new System.Windows.Forms.NumericUpDown();
            this.lblBufferDistance = new System.Windows.Forms.Label();
            this.groupBoxCoordSystem = new System.Windows.Forms.GroupBox();
            this.cmbCoordSystem = new System.Windows.Forms.ComboBox();
            this.lblCoordSystem = new System.Windows.Forms.Label();
            this.chkReproject = new System.Windows.Forms.CheckBox();
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabDataSource.SuspendLayout();
            this.groupBoxFiles.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBufferDistance)).BeginInit();
            this.groupBoxCoordSystem.SuspendLayout();
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
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 450);
            this.tabControl.TabIndex = 0;
            // 
            // tabDataSource
            // 
            this.tabDataSource.Controls.Add(this.groupBoxFiles);
            this.tabDataSource.Controls.Add(this.groupBoxOptions);
            this.tabDataSource.Controls.Add(this.groupBoxCoordSystem);
            this.tabDataSource.Location = new System.Drawing.Point(4, 22);
            this.tabDataSource.Name = "tabDataSource";
            this.tabDataSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabDataSource.Size = new System.Drawing.Size(792, 424);
            this.tabDataSource.TabIndex = 0;
            this.tabDataSource.Text = "����Դ����";
            this.tabDataSource.UseVisualStyleBackColor = true;
            // 
            // groupBoxFiles
            // 
            this.groupBoxFiles.Controls.Add(this.btnBrowseLCXZGX);
            this.groupBoxFiles.Controls.Add(this.btnBrowseCZKFBJ);
            this.groupBoxFiles.Controls.Add(this.btnBrowseOutput);
            this.groupBoxFiles.Controls.Add(this.txtLCXZGXPath);
            this.groupBoxFiles.Controls.Add(this.txtCZKFBJPath);
            this.groupBoxFiles.Controls.Add(this.txtOutputPath);
            this.groupBoxFiles.Controls.Add(this.lblLCXZGX);
            this.groupBoxFiles.Controls.Add(this.lblCZKFBJ);
            this.groupBoxFiles.Controls.Add(this.lblOutput);
            this.groupBoxFiles.Location = new System.Drawing.Point(6, 6);
            this.groupBoxFiles.Name = "groupBoxFiles";
            this.groupBoxFiles.Size = new System.Drawing.Size(780, 120);
            this.groupBoxFiles.TabIndex = 0;
            this.groupBoxFiles.TabStop = false;
            this.groupBoxFiles.Text = "�ļ�ѡ��";
            // 
            // btnBrowseLCXZGX
            // 
            this.btnBrowseLCXZGX.Location = new System.Drawing.Point(690, 25);
            this.btnBrowseLCXZGX.Name = "btnBrowseLCXZGX";
            this.btnBrowseLCXZGX.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseLCXZGX.TabIndex = 2;
            this.btnBrowseLCXZGX.Text = "���...";
            this.btnBrowseLCXZGX.UseVisualStyleBackColor = true;
            this.btnBrowseLCXZGX.Click += new System.EventHandler(this.btnBrowseLCXZGX_Click);
            // 
            // btnBrowseCZKFBJ
            // 
            this.btnBrowseCZKFBJ.Location = new System.Drawing.Point(690, 55);
            this.btnBrowseCZKFBJ.Name = "btnBrowseCZKFBJ";
            this.btnBrowseCZKFBJ.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseCZKFBJ.TabIndex = 5;
            this.btnBrowseCZKFBJ.Text = "���...";
            this.btnBrowseCZKFBJ.UseVisualStyleBackColor = true;
            this.btnBrowseCZKFBJ.Click += new System.EventHandler(this.btnBrowseCZKFBJ_Click);
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(690, 85);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOutput.TabIndex = 8;
            this.btnBrowseOutput.Text = "���...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtLCXZGXPath
            // 
            this.txtLCXZGXPath.Location = new System.Drawing.Point(140, 27);
            this.txtLCXZGXPath.Name = "txtLCXZGXPath";
            this.txtLCXZGXPath.ReadOnly = true;
            this.txtLCXZGXPath.Size = new System.Drawing.Size(544, 20);
            this.txtLCXZGXPath.TabIndex = 1;
            // 
            // txtCZKFBJPath
            // 
            this.txtCZKFBJPath.Location = new System.Drawing.Point(140, 57);
            this.txtCZKFBJPath.Name = "txtCZKFBJPath";
            this.txtCZKFBJPath.ReadOnly = true;
            this.txtCZKFBJPath.Size = new System.Drawing.Size(544, 20);
            this.txtCZKFBJPath.TabIndex = 4;
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(140, 87);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(544, 20);
            this.txtOutputPath.TabIndex = 7;
            // 
            // lblLCXZGX
            // 
            this.lblLCXZGX.AutoSize = true;
            this.lblLCXZGX.Location = new System.Drawing.Point(15, 30);
            this.lblLCXZGX.Name = "lblLCXZGX";
            this.lblLCXZGX.Size = new System.Drawing.Size(119, 13);
            this.lblLCXZGX.TabIndex = 0;
            this.lblLCXZGX.Text = "�ֲ���״ͼ��(LCXZGX):";
            // 
            // lblCZKFBJ
            // 
            this.lblCZKFBJ.AutoSize = true;
            this.lblCZKFBJ.Location = new System.Drawing.Point(15, 60);
            this.lblCZKFBJ.Name = "lblCZKFBJ";
            this.lblCZKFBJ.Size = new System.Drawing.Size(119, 13);
            this.lblCZKFBJ.TabIndex = 3;
            this.lblCZKFBJ.Text = "���򿪷��߽�(CZKFBJ):";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(15, 90);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(55, 13);
            this.lblOutput.TabIndex = 6;
            this.lblOutput.Text = "���·��:";
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.chkTopologyCheck);
            this.groupBoxOptions.Controls.Add(this.chkGeometryValidation);
            this.groupBoxOptions.Controls.Add(this.chkCreateBackup);
            this.groupBoxOptions.Controls.Add(this.chkGenerateReport);
            this.groupBoxOptions.Controls.Add(this.numBufferDistance);
            this.groupBoxOptions.Controls.Add(this.lblBufferDistance);
            this.groupBoxOptions.Location = new System.Drawing.Point(6, 132);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(380, 140);
            this.groupBoxOptions.TabIndex = 1;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "����ѡ��";
            // 
            // chkTopologyCheck
            // 
            this.chkTopologyCheck.AutoSize = true;
            this.chkTopologyCheck.Location = new System.Drawing.Point(15, 25);
            this.chkTopologyCheck.Name = "chkTopologyCheck";
            this.chkTopologyCheck.Size = new System.Drawing.Size(74, 17);
            this.chkTopologyCheck.TabIndex = 0;
            this.chkTopologyCheck.Text = "���˼��";
            this.chkTopologyCheck.UseVisualStyleBackColor = true;
            // 
            // chkGeometryValidation
            // 
            this.chkGeometryValidation.AutoSize = true;
            this.chkGeometryValidation.Location = new System.Drawing.Point(15, 48);
            this.chkGeometryValidation.Name = "chkGeometryValidation";
            this.chkGeometryValidation.Size = new System.Drawing.Size(74, 17);
            this.chkGeometryValidation.TabIndex = 1;
            this.chkGeometryValidation.Text = "������֤";
            this.chkGeometryValidation.UseVisualStyleBackColor = true;
            // 
            // chkCreateBackup
            // 
            this.chkCreateBackup.AutoSize = true;
            this.chkCreateBackup.Location = new System.Drawing.Point(200, 25);
            this.chkCreateBackup.Name = "chkCreateBackup";
            this.chkCreateBackup.Size = new System.Drawing.Size(74, 17);
            this.chkCreateBackup.TabIndex = 2;
            this.chkCreateBackup.Text = "��������";
            this.chkCreateBackup.UseVisualStyleBackColor = true;
            // 
            // chkGenerateReport
            // 
            this.chkGenerateReport.AutoSize = true;
            this.chkGenerateReport.Location = new System.Drawing.Point(200, 48);
            this.chkGenerateReport.Name = "chkGenerateReport";
            this.chkGenerateReport.Size = new System.Drawing.Size(74, 17);
            this.chkGenerateReport.TabIndex = 3;
            this.chkGenerateReport.Text = "���ɱ���";
            this.chkGenerateReport.UseVisualStyleBackColor = true;
            // 
            // numBufferDistance
            // 
            this.numBufferDistance.DecimalPlaces = 2;
            this.numBufferDistance.Location = new System.Drawing.Point(120, 75);
            this.numBufferDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numBufferDistance.Name = "numBufferDistance";
            this.numBufferDistance.Size = new System.Drawing.Size(80, 20);
            this.numBufferDistance.TabIndex = 5;
            this.numBufferDistance.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lblBufferDistance
            // 
            this.lblBufferDistance.AutoSize = true;
            this.lblBufferDistance.Location = new System.Drawing.Point(15, 77);
            this.lblBufferDistance.Name = "lblBufferDistance";
            this.lblBufferDistance.Size = new System.Drawing.Size(99, 13);
            this.lblBufferDistance.TabIndex = 4;
            this.lblBufferDistance.Text = "����������(��):";
            // 
            // groupBoxCoordSystem
            // 
            this.groupBoxCoordSystem.Controls.Add(this.cmbCoordSystem);
            this.groupBoxCoordSystem.Controls.Add(this.lblCoordSystem);
            this.groupBoxCoordSystem.Controls.Add(this.chkReproject);
            this.groupBoxCoordSystem.Location = new System.Drawing.Point(406, 132);
            this.groupBoxCoordSystem.Name = "groupBoxCoordSystem";
            this.groupBoxCoordSystem.Size = new System.Drawing.Size(380, 140);
            this.groupBoxCoordSystem.TabIndex = 2;
            this.groupBoxCoordSystem.TabStop = false;
            this.groupBoxCoordSystem.Text = "����ϵ����";
            // 
            // cmbCoordSystem
            // 
            this.cmbCoordSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCoordSystem.FormattingEnabled = true;
            this.cmbCoordSystem.Location = new System.Drawing.Point(15, 45);
            this.cmbCoordSystem.Name = "cmbCoordSystem";
            this.cmbCoordSystem.Size = new System.Drawing.Size(350, 21);
            this.cmbCoordSystem.TabIndex = 1;
            // 
            // lblCoordSystem
            // 
            this.lblCoordSystem.AutoSize = true;
            this.lblCoordSystem.Location = new System.Drawing.Point(15, 25);
            this.lblCoordSystem.Name = "lblCoordSystem";
            this.lblCoordSystem.Size = new System.Drawing.Size(79, 13);
            this.lblCoordSystem.TabIndex = 0;
            this.lblCoordSystem.Text = "Ŀ������ϵ:";
            // 
            // chkReproject
            // 
            this.chkReproject.AutoSize = true;
            this.chkReproject.Location = new System.Drawing.Point(15, 75);
            this.chkReproject.Name = "chkReproject";
            this.chkReproject.Size = new System.Drawing.Size(98, 17);
            this.chkReproject.TabIndex = 2;
            this.chkReproject.Text = "����ͶӰת��";
            this.chkReproject.UseVisualStyleBackColor = true;
            // 
            // tabFilter
            // 
            this.tabFilter.Controls.Add(this.groupBoxFieldSelect);
            this.tabFilter.Controls.Add(this.groupBoxConditions);
            this.tabFilter.Controls.Add(this.groupBoxPreview);
            this.tabFilter.Location = new System.Drawing.Point(4, 22);
            this.tabFilter.Name = "tabFilter";
            this.tabFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tabFilter.Size = new System.Drawing.Size(792, 424);
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
            this.groupBoxFieldSelect.Location = new System.Drawing.Point(6, 6);
            this.groupBoxFieldSelect.Name = "groupBoxFieldSelect";
            this.groupBoxFieldSelect.Size = new System.Drawing.Size(780, 80);
            this.groupBoxFieldSelect.TabIndex = 0;
            this.groupBoxFieldSelect.TabStop = false;
            this.groupBoxFieldSelect.Text = "�ֶ�ѡ��";
            // 
            // cmbLandTypeField
            // 
            this.cmbLandTypeField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLandTypeField.FormattingEnabled = true;
            this.cmbLandTypeField.Location = new System.Drawing.Point(100, 25);
            this.cmbLandTypeField.Name = "cmbLandTypeField";
            this.cmbLandTypeField.Size = new System.Drawing.Size(200, 21);
            this.cmbLandTypeField.TabIndex = 1;
            // 
            // cmbLandOwnerField
            // 
            this.cmbLandOwnerField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLandOwnerField.FormattingEnabled = true;
            this.cmbLandOwnerField.Location = new System.Drawing.Point(450, 25);
            this.cmbLandOwnerField.Name = "cmbLandOwnerField";
            this.cmbLandOwnerField.Size = new System.Drawing.Size(200, 21);
            this.cmbLandOwnerField.TabIndex = 3;
            // 
            // lblLandTypeField
            // 
            this.lblLandTypeField.AutoSize = true;
            this.lblLandTypeField.Location = new System.Drawing.Point(15, 28);
            this.lblLandTypeField.Name = "lblLandTypeField";
            this.lblLandTypeField.Size = new System.Drawing.Size(79, 13);
            this.lblLandTypeField.TabIndex = 0;
            this.lblLandTypeField.Text = "�����ֶ�:";
            // 
            // lblLandOwnerField
            // 
            this.lblLandOwnerField.AutoSize = true;
            this.lblLandOwnerField.Location = new System.Drawing.Point(350, 28);
            this.lblLandOwnerField.Name = "lblLandOwnerField";
            this.lblLandOwnerField.Size = new System.Drawing.Size(94, 13);
            this.lblLandOwnerField.TabIndex = 2;
            this.lblLandOwnerField.Text = "����Ȩ���ֶ�:";
            // 
            // groupBoxConditions
            // 
            this.groupBoxConditions.Controls.Add(this.chkForestLand);
            this.groupBoxConditions.Controls.Add(this.chkStateOwned);
            this.groupBoxConditions.Controls.Add(this.chkCollectiveInBoundary);
            this.groupBoxConditions.Location = new System.Drawing.Point(6, 92);
            this.groupBoxConditions.Name = "groupBoxConditions";
            this.groupBoxConditions.Size = new System.Drawing.Size(780, 80);
            this.groupBoxConditions.TabIndex = 1;
            this.groupBoxConditions.TabStop = false;
            this.groupBoxConditions.Text = "ɸѡ����";
            // 
            // chkForestLand
            // 
            this.chkForestLand.AutoSize = true;
            this.chkForestLand.Checked = true;
            this.chkForestLand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkForestLand.Location = new System.Drawing.Point(15, 25);
            this.chkForestLand.Name = "chkForestLand";
            this.chkForestLand.Size = new System.Drawing.Size(98, 17);
            this.chkForestLand.TabIndex = 0;
            this.chkForestLand.Text = "����Ϊ�ֵ�";
            this.chkForestLand.UseVisualStyleBackColor = true;
            // 
            // chkStateOwned
            // 
            this.chkStateOwned.AutoSize = true;
            this.chkStateOwned.Checked = true;
            this.chkStateOwned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStateOwned.Location = new System.Drawing.Point(15, 48);
            this.chkStateOwned.Name = "chkStateOwned";
            this.chkStateOwned.Size = new System.Drawing.Size(158, 17);
            this.chkStateOwned.TabIndex = 1;
            this.chkStateOwned.Text = "����Ȩ������Ϊ\"����\"";
            this.chkStateOwned.UseVisualStyleBackColor = true;
            // 
            // chkCollectiveInBoundary
            // 
            this.chkCollectiveInBoundary.AutoSize = true;
            this.chkCollectiveInBoundary.Checked = true;
            this.chkCollectiveInBoundary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCollectiveInBoundary.Location = new System.Drawing.Point(250, 48);
            this.chkCollectiveInBoundary.Name = "chkCollectiveInBoundary";
            this.chkCollectiveInBoundary.Size = new System.Drawing.Size(242, 17);
            this.chkCollectiveInBoundary.TabIndex = 2;
            this.chkCollectiveInBoundary.Text = "����Ȩ������Ϊ\"����\"��λ�ڳ��򿪷��߽���";
            this.chkCollectiveInBoundary.UseVisualStyleBackColor = true;
            // 
            // groupBoxPreview
            // 
            this.groupBoxPreview.Controls.Add(this.dgvPreview);
            this.groupBoxPreview.Controls.Add(this.btnPreview);
            this.groupBoxPreview.Controls.Add(this.lblPreviewCount);
            this.groupBoxPreview.Location = new System.Drawing.Point(6, 178);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Size = new System.Drawing.Size(780, 240);
            this.groupBoxPreview.TabIndex = 2;
            this.groupBoxPreview.TabStop = false;
            this.groupBoxPreview.Text = "Ԥ�����";
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.Location = new System.Drawing.Point(15, 25);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.Size = new System.Drawing.Size(750, 180);
            this.dgvPreview.TabIndex = 0;
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(15, 211);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(100, 23);
            this.btnPreview.TabIndex = 1;
            this.btnPreview.Text = "����Ԥ��";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // lblPreviewCount
            // 
            this.lblPreviewCount.AutoSize = true;
            this.lblPreviewCount.Location = new System.Drawing.Point(130, 216);
            this.lblPreviewCount.Name = "lblPreviewCount";
            this.lblPreviewCount.Size = new System.Drawing.Size(91, 13);
            this.lblPreviewCount.TabIndex = 2;
            this.lblPreviewCount.Text = "Ԥ�������0 ��ͼ��";
            // 
            // tabMapping
            // 
            this.tabMapping.Controls.Add(this.groupBoxMapping);
            this.tabMapping.Controls.Add(this.groupBoxTemplate);
            this.tabMapping.Location = new System.Drawing.Point(4, 22);
            this.tabMapping.Name = "tabMapping";
            this.tabMapping.Size = new System.Drawing.Size(792, 424);
            this.tabMapping.TabIndex = 2;
            this.tabMapping.Text = "�ֶ�ӳ������";
            this.tabMapping.UseVisualStyleBackColor = true;
            // 
            // groupBoxMapping
            // 
            this.groupBoxMapping.Controls.Add(this.dgvMapping);
            this.groupBoxMapping.Controls.Add(this.btnAutoMapping);
            this.groupBoxMapping.Location = new System.Drawing.Point(6, 6);
            this.groupBoxMapping.Name = "groupBoxMapping";
            this.groupBoxMapping.Size = new System.Drawing.Size(780, 320);
            this.groupBoxMapping.TabIndex = 0;
            this.groupBoxMapping.TabStop = false;
            this.groupBoxMapping.Text = "�ֶ�ӳ��";
            // 
            // dgvMapping
            // 
            this.dgvMapping.AllowUserToAddRows = false;
            this.dgvMapping.AllowUserToDeleteRows = false;
            this.dgvMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMapping.Location = new System.Drawing.Point(15, 25);
            this.dgvMapping.Name = "dgvMapping";
            this.dgvMapping.Size = new System.Drawing.Size(750, 260);
            this.dgvMapping.TabIndex = 0;
            // 
            // btnAutoMapping
            // 
            this.btnAutoMapping.Location = new System.Drawing.Point(15, 291);
            this.btnAutoMapping.Name = "btnAutoMapping";
            this.btnAutoMapping.Size = new System.Drawing.Size(100, 23);
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
            this.groupBoxTemplate.Location = new System.Drawing.Point(6, 332);
            this.groupBoxTemplate.Name = "groupBoxTemplate";
            this.groupBoxTemplate.Size = new System.Drawing.Size(780, 80);
            this.groupBoxTemplate.TabIndex = 1;
            this.groupBoxTemplate.TabStop = false;
            this.groupBoxTemplate.Text = "ӳ��ģ��";
            // 
            // btnLoadTemplate
            // 
            this.btnLoadTemplate.Location = new System.Drawing.Point(15, 25);
            this.btnLoadTemplate.Name = "btnLoadTemplate";
            this.btnLoadTemplate.Size = new System.Drawing.Size(100, 23);
            this.btnLoadTemplate.TabIndex = 0;
            this.btnLoadTemplate.Text = "����ģ��";
            this.btnLoadTemplate.UseVisualStyleBackColor = true;
            this.btnLoadTemplate.Click += new System.EventHandler(this.btnLoadTemplate_Click);
            // 
            // btnSaveTemplate
            // 
            this.btnSaveTemplate.Location = new System.Drawing.Point(130, 25);
            this.btnSaveTemplate.Name = "btnSaveTemplate";
            this.btnSaveTemplate.Size = new System.Drawing.Size(100, 23);
            this.btnSaveTemplate.TabIndex = 1;
            this.btnSaveTemplate.Text = "����ģ��";
            this.btnSaveTemplate.UseVisualStyleBackColor = true;
            this.btnSaveTemplate.Click += new System.EventHandler(this.btnSaveTemplate_Click);
            // 
            // lblTemplateInfo
            // 
            this.lblTemplateInfo.AutoSize = true;
            this.lblTemplateInfo.Location = new System.Drawing.Point(15, 55);
            this.lblTemplateInfo.Name = "lblTemplateInfo";
            this.lblTemplateInfo.Size = new System.Drawing.Size(139, 13);
            this.lblTemplateInfo.TabIndex = 2;
            this.lblTemplateInfo.Text = "��ǰģ�壺δ����ģ��";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.progressBar);
            this.panelBottom.Controls.Add(this.lblStatus);
            this.panelBottom.Controls.Add(this.btnExecute);
            this.panelBottom.Controls.Add(this.btnCancel);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 456);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(800, 80);
            this.panelBottom.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(500, 23);
            this.progressBar.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 42);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(67, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "״̬������";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(550, 12);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "ִ��";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(631, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "ȡ��";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(712, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "�ر�";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // Basic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 536);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Basic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ɭ����Դ�ʲ���鹤����Χ���ɹ���";
            this.tabControl.ResumeLayout(false);
            this.tabDataSource.ResumeLayout(false);
            this.groupBoxFiles.ResumeLayout(false);
            this.groupBoxFiles.PerformLayout();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBufferDistance)).EndInit();
            this.groupBoxCoordSystem.ResumeLayout(false);
            this.groupBoxCoordSystem.PerformLayout();
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
        private System.Windows.Forms.Button btnBrowseLCXZGX;
        private System.Windows.Forms.Button btnBrowseCZKFBJ;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.TextBox txtLCXZGXPath;
        private System.Windows.Forms.TextBox txtCZKFBJPath;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label lblLCXZGX;
        private System.Windows.Forms.Label lblCZKFBJ;
        private System.Windows.Forms.Label lblOutput;

        private System.Windows.Forms.GroupBox groupBoxOptions;
        private System.Windows.Forms.CheckBox chkTopologyCheck;
        private System.Windows.Forms.CheckBox chkGeometryValidation;
        private System.Windows.Forms.CheckBox chkCreateBackup;
        private System.Windows.Forms.CheckBox chkGenerateReport;
        private System.Windows.Forms.NumericUpDown numBufferDistance;
        private System.Windows.Forms.Label lblBufferDistance;

        private System.Windows.Forms.GroupBox groupBoxCoordSystem;
        private System.Windows.Forms.ComboBox cmbCoordSystem;
        private System.Windows.Forms.Label lblCoordSystem;
        private System.Windows.Forms.CheckBox chkReproject;

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
    }
}
