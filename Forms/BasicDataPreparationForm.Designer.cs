namespace TestArcMapAddin2.Forms
{
    partial class BasicDataPreparationForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSelectWorkspace;
        private System.Windows.Forms.Label lblWorkspace;
        private System.Windows.Forms.Button btnSelectCounties;
        private System.Windows.Forms.Label lblCounties;
        private System.Windows.Forms.Button btnLoadPrerequisiteData1; // �޸�����
        private System.Windows.Forms.Label lblPrerequisiteData1Status; // �޸�����
        private System.Windows.Forms.Button btnLoadPrerequisiteData2; // ������ť
        private System.Windows.Forms.Label lblPrerequisiteData2Status; // ������ǩ
        private System.Windows.Forms.Button btnCreateCountyEmptyTables;
        private System.Windows.Forms.Label lblCountyEmptyTablesStatus;
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
            System.Drawing.Font defaultFont = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font titleFont = new System.Drawing.Font("����", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Color defaultForeColor = System.Drawing.Color.Black;

            this.topPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.btnSelectWorkspace = new System.Windows.Forms.Button();
            this.lblWorkspace = new System.Windows.Forms.Label();
            this.btnSelectCounties = new System.Windows.Forms.Button();
            this.lblCounties = new System.Windows.Forms.Label();
            this.btnLoadPrerequisiteData1 = new System.Windows.Forms.Button(); // �޸�����
            this.lblPrerequisiteData1Status = new System.Windows.Forms.Label(); // �޸�����
            this.btnLoadPrerequisiteData2 = new System.Windows.Forms.Button(); // ������ť
            this.lblPrerequisiteData2Status = new System.Windows.Forms.Label(); // ������ǩ
            this.btnCreateCountyEmptyTables = new System.Windows.Forms.Button();
            this.lblCountyEmptyTablesStatus = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.topPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();

            // titleLabel
            this.titleLabel.Font = titleFont;
            this.titleLabel.ForeColor = defaultForeColor;
            this.titleLabel.Location = new System.Drawing.Point(15, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(560, 30);  // ��760��Ϊ560
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "��������׼��";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // topPanel
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Padding = new System.Windows.Forms.Padding(10);
            this.topPanel.Controls.Add(this.titleLabel);
            this.topPanel.Controls.Add(this.btnSelectWorkspace);
            this.topPanel.Controls.Add(this.lblWorkspace);
            this.topPanel.Controls.Add(this.btnSelectCounties);
            this.topPanel.Controls.Add(this.lblCounties);
            this.topPanel.Controls.Add(this.btnLoadPrerequisiteData1); // �޸�����
            this.topPanel.Controls.Add(this.lblPrerequisiteData1Status); // �޸�����
            this.topPanel.Controls.Add(this.btnLoadPrerequisiteData2); // ������ť
            this.topPanel.Controls.Add(this.lblPrerequisiteData2Status); // ������ǩ
            this.topPanel.Controls.Add(this.btnCreateCountyEmptyTables);
            this.topPanel.Controls.Add(this.lblCountyEmptyTablesStatus);
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(600, 380);  // ��ȴ�800��Ϊ600���߶ȴ�340��Ϊ380

            // btnSelectWorkspace
            this.btnSelectWorkspace.Font = defaultFont;
            this.btnSelectWorkspace.ForeColor = defaultForeColor;
            this.btnSelectWorkspace.Location = new System.Drawing.Point(20, 50);
            this.btnSelectWorkspace.Name = "btnSelectWorkspace";
            this.btnSelectWorkspace.Size = new System.Drawing.Size(110, 30);
            this.btnSelectWorkspace.TabIndex = 1;
            this.btnSelectWorkspace.Text = "ѡ�����ռ�";
            this.btnSelectWorkspace.UseVisualStyleBackColor = true;
            this.btnSelectWorkspace.Click += new System.EventHandler(this.BtnSelectWorkspace_Click);

            // lblWorkspace
            this.lblWorkspace.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblWorkspace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWorkspace.Font = defaultFont;
            this.lblWorkspace.ForeColor = defaultForeColor;
            this.lblWorkspace.Location = new System.Drawing.Point(140, 50);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(320, 30);  // ��400��Ϊ320
            this.lblWorkspace.TabIndex = 2;
            this.lblWorkspace.Text = "δѡ�����ռ�";
            this.lblWorkspace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // btnSelectCounties
            this.btnSelectCounties.Font = defaultFont;
            this.btnSelectCounties.ForeColor = defaultForeColor;
            this.btnSelectCounties.Location = new System.Drawing.Point(20, 90);  // ��(560, 50)��Ϊ(20, 90)
            this.btnSelectCounties.Name = "btnSelectCounties";
            this.btnSelectCounties.Size = new System.Drawing.Size(90, 30);
            this.btnSelectCounties.TabIndex = 3;
            this.btnSelectCounties.Text = "ѡ������";
            this.btnSelectCounties.UseVisualStyleBackColor = true;
            this.btnSelectCounties.Click += new System.EventHandler(this.BtnSelectCounties_Click);

            // lblCounties
            this.lblCounties.Font = defaultFont;
            this.lblCounties.ForeColor = defaultForeColor;
            this.lblCounties.Location = new System.Drawing.Point(20, 130);  // ��(20, 90)��Ϊ(20, 130)
            this.lblCounties.Name = "lblCounties";
            this.lblCounties.Size = new System.Drawing.Size(550, 25);  // ��750��Ϊ550
            this.lblCounties.TabIndex = 4;
            this.lblCounties.Text = "δѡ������";
            this.lblCounties.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // btnLoadPrerequisiteData1
            this.btnLoadPrerequisiteData1.Font = defaultFont;
            this.btnLoadPrerequisiteData1.ForeColor = defaultForeColor;
            this.btnLoadPrerequisiteData1.Location = new System.Drawing.Point(20, 170);  // ��(20, 130)��Ϊ(20, 170)
            this.btnLoadPrerequisiteData1.Name = "btnLoadPrerequisiteData1";
            this.btnLoadPrerequisiteData1.Size = new System.Drawing.Size(110, 30);
            this.btnLoadPrerequisiteData1.TabIndex = 5;
            this.btnLoadPrerequisiteData1.Text = "����ǰ������1";  // �޸��ı�
            this.btnLoadPrerequisiteData1.UseVisualStyleBackColor = true;
            this.btnLoadPrerequisiteData1.Click += new System.EventHandler(this.BtnLoadPrerequisiteData1_Click);  // �޸��¼�������

            // lblPrerequisiteData1Status
            this.lblPrerequisiteData1Status.Font = defaultFont;
            this.lblPrerequisiteData1Status.ForeColor = defaultForeColor;
            this.lblPrerequisiteData1Status.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblPrerequisiteData1Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPrerequisiteData1Status.Location = new System.Drawing.Point(140, 170);  // ��(140, 130)��Ϊ(140, 170)
            this.lblPrerequisiteData1Status.Name = "lblPrerequisiteData1Status";
            this.lblPrerequisiteData1Status.Size = new System.Drawing.Size(430, 30);  // ��630��Ϊ430
            this.lblPrerequisiteData1Status.TabIndex = 6;
            this.lblPrerequisiteData1Status.Text = "δ����ǰ������1";  // �޸��ı�
            this.lblPrerequisiteData1Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // btnLoadPrerequisiteData2 - �°�ť
            this.btnLoadPrerequisiteData2.Font = defaultFont;
            this.btnLoadPrerequisiteData2.ForeColor = defaultForeColor;
            this.btnLoadPrerequisiteData2.Location = new System.Drawing.Point(20, 210);  // ��(20, 170)��Ϊ(20, 210)
            this.btnLoadPrerequisiteData2.Name = "btnLoadPrerequisiteData2";
            this.btnLoadPrerequisiteData2.Size = new System.Drawing.Size(110, 30);
            this.btnLoadPrerequisiteData2.TabIndex = 7;
            this.btnLoadPrerequisiteData2.Text = "����ǰ������2";
            this.btnLoadPrerequisiteData2.UseVisualStyleBackColor = true;
            this.btnLoadPrerequisiteData2.Click += new System.EventHandler(this.BtnLoadPrerequisiteData2_Click);

            // lblPrerequisiteData2Status - �±�ǩ
            this.lblPrerequisiteData2Status.Font = defaultFont;
            this.lblPrerequisiteData2Status.ForeColor = defaultForeColor;
            this.lblPrerequisiteData2Status.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblPrerequisiteData2Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPrerequisiteData2Status.Location = new System.Drawing.Point(140, 210);  // ��(140, 170)��Ϊ(140, 210)
            this.lblPrerequisiteData2Status.Name = "lblPrerequisiteData2Status";
            this.lblPrerequisiteData2Status.Size = new System.Drawing.Size(430, 30);  // ��630��Ϊ430
            this.lblPrerequisiteData2Status.TabIndex = 8;
            this.lblPrerequisiteData2Status.Text = "δ����ǰ������2";
            this.lblPrerequisiteData2Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // btnCreateCountyEmptyTables - λ������
            this.btnCreateCountyEmptyTables.Font = defaultFont;
            this.btnCreateCountyEmptyTables.ForeColor = defaultForeColor;
            this.btnCreateCountyEmptyTables.Location = new System.Drawing.Point(20, 250);  // ��(20, 210)��Ϊ(20, 250)
            this.btnCreateCountyEmptyTables.Name = "btnCreateCountyEmptyTables";
            this.btnCreateCountyEmptyTables.Size = new System.Drawing.Size(135, 30);
            this.btnCreateCountyEmptyTables.TabIndex = 9;  // �޸�TabIndex
            this.btnCreateCountyEmptyTables.Text = "�����ؼ��ձ�";
            this.btnCreateCountyEmptyTables.UseVisualStyleBackColor = true;
            this.btnCreateCountyEmptyTables.Click += new System.EventHandler(this.BtnCreateCountyEmptyTables_Click);

            // lblCountyEmptyTablesStatus - λ������
            this.lblCountyEmptyTablesStatus.Font = defaultFont;
            this.lblCountyEmptyTablesStatus.ForeColor = defaultForeColor;
            this.lblCountyEmptyTablesStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCountyEmptyTablesStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCountyEmptyTablesStatus.Location = new System.Drawing.Point(165, 250);  // ��(165, 210)��Ϊ(165, 250)
            this.lblCountyEmptyTablesStatus.Name = "lblCountyEmptyTablesStatus";
            this.lblCountyEmptyTablesStatus.Size = new System.Drawing.Size(405, 30);  // ��605��Ϊ405
            this.lblCountyEmptyTablesStatus.TabIndex = 10;  // �޸�TabIndex
            this.lblCountyEmptyTablesStatus.Text = "δ�����ؼ��ձ�";
            this.lblCountyEmptyTablesStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // bottomPanel
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.btnOK);
            this.bottomPanel.Controls.Add(this.btnCancel);
            this.bottomPanel.Location = new System.Drawing.Point(0, 330);  // ��(0, 290)��Ϊ(0, 330)
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(600, 50);  // ��ȴ�800��Ϊ600
            this.bottomPanel.TabIndex = 11;  // �޸�TabIndex

            // btnOK
            this.btnOK.Font = defaultFont;
            this.btnOK.Location = new System.Drawing.Point(400, 10);  // ��(600, 10)��Ϊ(400, 10)
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "ȷ��";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            // btnCancel
            this.btnCancel.Font = defaultFont;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(500, 10);  // ��(700, 10)��Ϊ(500, 10)
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "�ر�";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            // BasicDataPreparationForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 380);  // ��(800, 340)��Ϊ(600, 380)
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Font = defaultFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BasicDataPreparationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "��������׼��";
            this.topPanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}