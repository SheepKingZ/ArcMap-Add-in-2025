namespace TestArcMapAddin2.Forms
{
    partial class BasicDataPreparationForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSelectWorkspace;
        private System.Windows.Forms.Label lblWorkspace;
        private System.Windows.Forms.Button btnSelectCounties;
        private System.Windows.Forms.Label lblCounties;
        private System.Windows.Forms.Button btnLoadPrerequisiteData;
        private System.Windows.Forms.Label lblPrerequisiteDataStatus;
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
            System.Drawing.Font defaultFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Font titleFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            System.Drawing.Color defaultForeColor = System.Drawing.Color.Black;

            this.topPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.btnSelectWorkspace = new System.Windows.Forms.Button();
            this.lblWorkspace = new System.Windows.Forms.Label();
            this.btnSelectCounties = new System.Windows.Forms.Button();
            this.lblCounties = new System.Windows.Forms.Label();
            this.btnLoadPrerequisiteData = new System.Windows.Forms.Button();
            this.lblPrerequisiteDataStatus = new System.Windows.Forms.Label();
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
            this.titleLabel.Size = new System.Drawing.Size(760, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "基础数据准备";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // topPanel
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Padding = new System.Windows.Forms.Padding(10);
            this.topPanel.Controls.Add(this.titleLabel);
            this.topPanel.Controls.Add(this.btnSelectWorkspace);
            this.topPanel.Controls.Add(this.lblWorkspace);
            this.topPanel.Controls.Add(this.btnSelectCounties);
            this.topPanel.Controls.Add(this.lblCounties);
            this.topPanel.Controls.Add(this.btnLoadPrerequisiteData);
            this.topPanel.Controls.Add(this.lblPrerequisiteDataStatus);
            this.topPanel.Controls.Add(this.btnCreateCountyEmptyTables);
            this.topPanel.Controls.Add(this.lblCountyEmptyTablesStatus);
            this.topPanel.Location = new System.Drawing.Point(0,0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(800, 300); // Adjusted size

            // btnSelectWorkspace
            this.btnSelectWorkspace.Font = defaultFont;
            this.btnSelectWorkspace.ForeColor = defaultForeColor;
            this.btnSelectWorkspace.Location = new System.Drawing.Point(20, 50);
            this.btnSelectWorkspace.Name = "btnSelectWorkspace";
            this.btnSelectWorkspace.Size = new System.Drawing.Size(110, 30);
            this.btnSelectWorkspace.TabIndex = 1;
            this.btnSelectWorkspace.Text = "选择工作空间";
            this.btnSelectWorkspace.UseVisualStyleBackColor = true;
            this.btnSelectWorkspace.Click += new System.EventHandler(this.BtnSelectWorkspace_Click);
            
            // lblWorkspace
            this.lblWorkspace.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblWorkspace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWorkspace.Font = defaultFont;
            this.lblWorkspace.ForeColor = defaultForeColor;
            this.lblWorkspace.Location = new System.Drawing.Point(140, 50);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(400, 30); // Adjusted height
            this.lblWorkspace.TabIndex = 2;
            this.lblWorkspace.Text = "未选择工作空间";
            this.lblWorkspace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // btnSelectCounties
            this.btnSelectCounties.Font = defaultFont;
            this.btnSelectCounties.ForeColor = defaultForeColor;
            this.btnSelectCounties.Location = new System.Drawing.Point(560, 50);
            this.btnSelectCounties.Name = "btnSelectCounties";
            this.btnSelectCounties.Size = new System.Drawing.Size(90, 30);
            this.btnSelectCounties.TabIndex = 3;
            this.btnSelectCounties.Text = "选择县区";
            this.btnSelectCounties.UseVisualStyleBackColor = true;
            this.btnSelectCounties.Click += new System.EventHandler(this.BtnSelectCounties_Click);
            
            // lblCounties
            this.lblCounties.Font = defaultFont;
            this.lblCounties.ForeColor = defaultForeColor;
            this.lblCounties.Location = new System.Drawing.Point(20, 90);
            this.lblCounties.Name = "lblCounties";
            this.lblCounties.Size = new System.Drawing.Size(750, 25);
            this.lblCounties.TabIndex = 4;
            this.lblCounties.Text = "未选择县区";
            this.lblCounties.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // btnLoadPrerequisiteData
            this.btnLoadPrerequisiteData.Font = defaultFont;
            this.btnLoadPrerequisiteData.ForeColor = defaultForeColor;
            this.btnLoadPrerequisiteData.Location = new System.Drawing.Point(20, 130);
            this.btnLoadPrerequisiteData.Name = "btnLoadPrerequisiteData";
            this.btnLoadPrerequisiteData.Size = new System.Drawing.Size(110, 30);
            this.btnLoadPrerequisiteData.TabIndex = 5;
            this.btnLoadPrerequisiteData.Text = "加载前提数据";
            this.btnLoadPrerequisiteData.UseVisualStyleBackColor = true;
            this.btnLoadPrerequisiteData.Click += new System.EventHandler(this.BtnLoadPrerequisiteData_Click);
            
            // lblPrerequisiteDataStatus
            this.lblPrerequisiteDataStatus.Font = defaultFont;
            this.lblPrerequisiteDataStatus.ForeColor = defaultForeColor;
            this.lblPrerequisiteDataStatus.Location = new System.Drawing.Point(140, 130);
            this.lblPrerequisiteDataStatus.Name = "lblPrerequisiteDataStatus";
            this.lblPrerequisiteDataStatus.Size = new System.Drawing.Size(630, 30); // Adjusted height
            this.lblPrerequisiteDataStatus.TabIndex = 6;
            this.lblPrerequisiteDataStatus.Text = "未加载前提数据";
            this.lblPrerequisiteDataStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // btnCreateCountyEmptyTables
            this.btnCreateCountyEmptyTables.Font = defaultFont;
            this.btnCreateCountyEmptyTables.ForeColor = defaultForeColor;
            this.btnCreateCountyEmptyTables.Location = new System.Drawing.Point(20, 170);
            this.btnCreateCountyEmptyTables.Name = "btnCreateCountyEmptyTables";
            this.btnCreateCountyEmptyTables.Size = new System.Drawing.Size(135, 30);
            this.btnCreateCountyEmptyTables.TabIndex = 7;
            this.btnCreateCountyEmptyTables.Text = "创建县级空表";
            this.btnCreateCountyEmptyTables.UseVisualStyleBackColor = true;
            this.btnCreateCountyEmptyTables.Click += new System.EventHandler(this.BtnCreateCountyEmptyTables_Click);
            
            // lblCountyEmptyTablesStatus
            this.lblCountyEmptyTablesStatus.Font = defaultFont;
            this.lblCountyEmptyTablesStatus.ForeColor = defaultForeColor;
            this.lblCountyEmptyTablesStatus.Location = new System.Drawing.Point(165, 170); // Adjusted X
            this.lblCountyEmptyTablesStatus.Name = "lblCountyEmptyTablesStatus";
            this.lblCountyEmptyTablesStatus.Size = new System.Drawing.Size(605, 30); // Adjusted height
            this.lblCountyEmptyTablesStatus.TabIndex = 8;
            this.lblCountyEmptyTablesStatus.Text = "未创建县级空表";
            this.lblCountyEmptyTablesStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // bottomPanel
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.btnOK);
            this.bottomPanel.Controls.Add(this.btnCancel);
            this.bottomPanel.Location = new System.Drawing.Point(0, 250); // Adjusted Y
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(800, 50); // Adjusted size
            this.bottomPanel.TabIndex = 9;

            // btnOK
            this.btnOK.Font = defaultFont;
            this.btnOK.Location = new System.Drawing.Point(600, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            // btnCancel
            this.btnCancel.Font = defaultFont;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(700, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            
            // BasicDataPreparationForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 300); // Adjusted size
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Font = defaultFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BasicDataPreparationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "基础数据准备";
            this.topPanel.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}