namespace TestArcMapAddin2.Forms
{
    partial class BasicDataPreparationForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSelectWorkspace;
        private System.Windows.Forms.Label lblWorkspace;
        
        // �޸Ŀؼ����� - �ϲ���������ԴΪһ��
        private System.Windows.Forms.Label lblDataSource;
        private System.Windows.Forms.TextBox txtDataPath;
        private System.Windows.Forms.Button btnBrowseData;
        private System.Windows.Forms.Label lblOutputGDBPath;
        private System.Windows.Forms.TextBox txtOutputGDBPath;
        private System.Windows.Forms.Button btnBrowseOutputGDB;
        private System.Windows.Forms.CheckBox chkCreateCountyFolders;
        
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Label titleLabel;


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
            this.topPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.btnSelectWorkspace = new System.Windows.Forms.Button();
            this.lblWorkspace = new System.Windows.Forms.Label();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.txtDataPath = new System.Windows.Forms.TextBox();
            this.btnBrowseData = new System.Windows.Forms.Button();
            this.lblOutputGDBPath = new System.Windows.Forms.Label();
            this.txtOutputGDBPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputGDB = new System.Windows.Forms.Button();
            this.chkCreateCountyFolders = new System.Windows.Forms.CheckBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.topPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.titleLabel);
            this.topPanel.Controls.Add(this.btnSelectWorkspace);
            this.topPanel.Controls.Add(this.lblWorkspace);
            this.topPanel.Controls.Add(this.lblDataSource);
            this.topPanel.Controls.Add(this.txtDataPath);
            this.topPanel.Controls.Add(this.btnBrowseData);
            this.topPanel.Controls.Add(this.lblOutputGDBPath);
            this.topPanel.Controls.Add(this.txtOutputGDBPath);
            this.topPanel.Controls.Add(this.btnBrowseOutputGDB);
            this.topPanel.Controls.Add(this.chkCreateCountyFolders);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.topPanel.Size = new System.Drawing.Size(667, 362);
            this.topPanel.TabIndex = 22;
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("����", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point(15, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(560, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "��������׼��";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSelectWorkspace
            // 
            this.btnSelectWorkspace.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectWorkspace.ForeColor = System.Drawing.Color.Black;
            this.btnSelectWorkspace.Location = new System.Drawing.Point(20, 50);
            this.btnSelectWorkspace.Name = "btnSelectWorkspace";
            this.btnSelectWorkspace.Size = new System.Drawing.Size(110, 30);
            this.btnSelectWorkspace.TabIndex = 1;
            this.btnSelectWorkspace.Text = "ѡ�����ռ�";
            this.btnSelectWorkspace.UseVisualStyleBackColor = true;
            this.btnSelectWorkspace.Click += new System.EventHandler(this.BtnSelectWorkspace_Click);
            // 
            // lblWorkspace
            // 
            this.lblWorkspace.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblWorkspace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWorkspace.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWorkspace.ForeColor = System.Drawing.Color.Black;
            this.lblWorkspace.Location = new System.Drawing.Point(140, 50);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(320, 30);
            this.lblWorkspace.TabIndex = 2;
            this.lblWorkspace.Text = "δѡ�����ռ�";
            this.lblWorkspace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDataSource
            // 
            this.lblDataSource.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDataSource.ForeColor = System.Drawing.Color.Black;
            this.lblDataSource.Location = new System.Drawing.Point(18, 91);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(373, 20);
            this.lblDataSource.TabIndex = 5;
            this.lblDataSource.Text = "��������Դ�������ֲ�ʪ���ղ���������򿪷��߽����ݣ���";
            this.lblDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDataPath
            // 
            this.txtDataPath.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDataPath.Location = new System.Drawing.Point(18, 116);
            this.txtDataPath.Name = "txtDataPath";
            this.txtDataPath.ReadOnly = true;
            this.txtDataPath.Size = new System.Drawing.Size(540, 21);
            this.txtDataPath.TabIndex = 6;
            this.txtDataPath.Text = "��ѡ������ֲ�ʪ���ղ�����򿪷��߽����ݵ��ļ���";
            // 
            // btnBrowseData
            // 
            this.btnBrowseData.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseData.Location = new System.Drawing.Point(573, 116);
            this.btnBrowseData.Name = "btnBrowseData";
            this.btnBrowseData.Size = new System.Drawing.Size(70, 23);
            this.btnBrowseData.TabIndex = 7;
            this.btnBrowseData.Text = "���...";
            this.btnBrowseData.UseVisualStyleBackColor = true;
            this.btnBrowseData.Click += new System.EventHandler(this.BtnBrowseData_Click);
            // 
            // lblOutputGDBPath
            // 
            this.lblOutputGDBPath.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOutputGDBPath.ForeColor = System.Drawing.Color.Black;
            this.lblOutputGDBPath.Location = new System.Drawing.Point(18, 151);
            this.lblOutputGDBPath.Name = "lblOutputGDBPath";
            this.lblOutputGDBPath.Size = new System.Drawing.Size(150, 20);
            this.lblOutputGDBPath.TabIndex = 11;
            this.lblOutputGDBPath.Text = "������GDB·����";
            this.lblOutputGDBPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOutputGDBPath
            // 
            this.txtOutputGDBPath.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOutputGDBPath.Location = new System.Drawing.Point(18, 176);
            this.txtOutputGDBPath.Name = "txtOutputGDBPath";
            this.txtOutputGDBPath.ReadOnly = true;
            this.txtOutputGDBPath.Size = new System.Drawing.Size(540, 21);
            this.txtOutputGDBPath.TabIndex = 12;
            this.txtOutputGDBPath.Text = "��ѡ��������GDB·��";
            // 
            // btnBrowseOutputGDB
            // 
            this.btnBrowseOutputGDB.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseOutputGDB.Location = new System.Drawing.Point(573, 176);
            this.btnBrowseOutputGDB.Name = "btnBrowseOutputGDB";
            this.btnBrowseOutputGDB.Size = new System.Drawing.Size(70, 23);
            this.btnBrowseOutputGDB.TabIndex = 13;
            this.btnBrowseOutputGDB.Text = "���...";
            this.btnBrowseOutputGDB.UseVisualStyleBackColor = true;
            this.btnBrowseOutputGDB.Click += new System.EventHandler(this.BtnBrowseOutputGDB_Click);
            // 
            // chkCreateCountyFolders
            // 
            this.chkCreateCountyFolders.AutoSize = true;
            this.chkCreateCountyFolders.Checked = true;
            this.chkCreateCountyFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateCountyFolders.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkCreateCountyFolders.Location = new System.Drawing.Point(18, 216);
            this.chkCreateCountyFolders.Name = "chkCreateCountyFolders";
            this.chkCreateCountyFolders.Size = new System.Drawing.Size(240, 16);
            this.chkCreateCountyFolders.TabIndex = 14;
            this.chkCreateCountyFolders.Text = "Ϊÿ���ش����ļ��в��������ֽ�����";
            this.chkCreateCountyFolders.UseVisualStyleBackColor = true;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.button1);
            this.bottomPanel.Controls.Add(this.btnOK);
            this.bottomPanel.Controls.Add(this.btnCancel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 312);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(667, 50);
            this.bottomPanel.TabIndex = 21;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(400, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "ȷ��";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(500, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "�ر�";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(306, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "�������ݿ�";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BasicDataPreparationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 362);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BasicDataPreparationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "��������׼��";
            this.Load += new System.EventHandler(this.BasicDataPreparationForm_Load);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button button1;
    }
}