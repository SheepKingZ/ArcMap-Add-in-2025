namespace TestArcMapAddin2.Forms
{
    partial class BasicDataPreparationForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSelectWorkspace;
        private System.Windows.Forms.Label lblWorkspace;
        
        // 新增控件声明
        private System.Windows.Forms.Label lblForestSurveyDataSource;
        private System.Windows.Forms.TextBox txtForestSurveyDataPath;
        private System.Windows.Forms.Button btnBrowseForestSurveyData;
        private System.Windows.Forms.Label lblUrbanBoundaryDataSource;
        private System.Windows.Forms.TextBox txtUrbanBoundaryDataPath;
        private System.Windows.Forms.Button btnBrowseUrbanBoundaryData;
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
            this.lblForestSurveyDataSource = new System.Windows.Forms.Label();
            this.txtForestSurveyDataPath = new System.Windows.Forms.TextBox();
            this.btnBrowseForestSurveyData = new System.Windows.Forms.Button();
            this.lblUrbanBoundaryDataSource = new System.Windows.Forms.Label();
            this.txtUrbanBoundaryDataPath = new System.Windows.Forms.TextBox();
            this.btnBrowseUrbanBoundaryData = new System.Windows.Forms.Button();
            this.lblOutputGDBPath = new System.Windows.Forms.Label();
            this.txtOutputGDBPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputGDB = new System.Windows.Forms.Button();
            this.chkCreateCountyFolders = new System.Windows.Forms.CheckBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.topPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.titleLabel);
            this.topPanel.Controls.Add(this.btnSelectWorkspace);
            this.topPanel.Controls.Add(this.lblWorkspace);
            this.topPanel.Controls.Add(this.lblForestSurveyDataSource);
            this.topPanel.Controls.Add(this.txtForestSurveyDataPath);
            this.topPanel.Controls.Add(this.btnBrowseForestSurveyData);
            this.topPanel.Controls.Add(this.lblUrbanBoundaryDataSource);
            this.topPanel.Controls.Add(this.txtUrbanBoundaryDataPath);
            this.topPanel.Controls.Add(this.btnBrowseUrbanBoundaryData);
            this.topPanel.Controls.Add(this.lblOutputGDBPath);
            this.topPanel.Controls.Add(this.txtOutputGDBPath);
            this.topPanel.Controls.Add(this.btnBrowseOutputGDB);
            this.topPanel.Controls.Add(this.chkCreateCountyFolders);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(15, 15, 15, 15);
            this.topPanel.Size = new System.Drawing.Size(1001, 543);
            this.topPanel.TabIndex = 22;
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point(22, 15);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(840, 45);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "基础数据准备";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSelectWorkspace
            // 
            this.btnSelectWorkspace.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectWorkspace.ForeColor = System.Drawing.Color.Black;
            this.btnSelectWorkspace.Location = new System.Drawing.Point(30, 75);
            this.btnSelectWorkspace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectWorkspace.Name = "btnSelectWorkspace";
            this.btnSelectWorkspace.Size = new System.Drawing.Size(165, 45);
            this.btnSelectWorkspace.TabIndex = 1;
            this.btnSelectWorkspace.Text = "选择工作空间";
            this.btnSelectWorkspace.UseVisualStyleBackColor = true;
            this.btnSelectWorkspace.Click += new System.EventHandler(this.BtnSelectWorkspace_Click);
            // 
            // lblWorkspace
            // 
            this.lblWorkspace.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblWorkspace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWorkspace.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWorkspace.ForeColor = System.Drawing.Color.Black;
            this.lblWorkspace.Location = new System.Drawing.Point(210, 75);
            this.lblWorkspace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(479, 44);
            this.lblWorkspace.TabIndex = 2;
            this.lblWorkspace.Text = "未选择工作空间";
            this.lblWorkspace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblForestSurveyDataSource
            // 
            this.lblForestSurveyDataSource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblForestSurveyDataSource.ForeColor = System.Drawing.Color.Black;
            this.lblForestSurveyDataSource.Location = new System.Drawing.Point(27, 137);
            this.lblForestSurveyDataSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForestSurveyDataSource.Name = "lblForestSurveyDataSource";
            this.lblForestSurveyDataSource.Size = new System.Drawing.Size(225, 30);
            this.lblForestSurveyDataSource.TabIndex = 5;
            this.lblForestSurveyDataSource.Text = "林草湿荒普查数据源：";
            this.lblForestSurveyDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtForestSurveyDataPath
            // 
            this.txtForestSurveyDataPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtForestSurveyDataPath.Location = new System.Drawing.Point(27, 174);
            this.txtForestSurveyDataPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtForestSurveyDataPath.Name = "txtForestSurveyDataPath";
            this.txtForestSurveyDataPath.ReadOnly = true;
            this.txtForestSurveyDataPath.Size = new System.Drawing.Size(808, 28);
            this.txtForestSurveyDataPath.TabIndex = 6;
            this.txtForestSurveyDataPath.Text = "请选择林草湿荒普查数据源文件夹";
            // 
            // btnBrowseForestSurveyData
            // 
            this.btnBrowseForestSurveyData.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseForestSurveyData.Location = new System.Drawing.Point(859, 174);
            this.btnBrowseForestSurveyData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseForestSurveyData.Name = "btnBrowseForestSurveyData";
            this.btnBrowseForestSurveyData.Size = new System.Drawing.Size(105, 34);
            this.btnBrowseForestSurveyData.TabIndex = 7;
            this.btnBrowseForestSurveyData.Text = "浏览...";
            this.btnBrowseForestSurveyData.UseVisualStyleBackColor = true;
            this.btnBrowseForestSurveyData.Click += new System.EventHandler(this.BtnBrowseForestSurveyData_Click);
            // 
            // lblUrbanBoundaryDataSource
            // 
            this.lblUrbanBoundaryDataSource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUrbanBoundaryDataSource.ForeColor = System.Drawing.Color.Black;
            this.lblUrbanBoundaryDataSource.Location = new System.Drawing.Point(27, 227);
            this.lblUrbanBoundaryDataSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUrbanBoundaryDataSource.Name = "lblUrbanBoundaryDataSource";
            this.lblUrbanBoundaryDataSource.Size = new System.Drawing.Size(225, 30);
            this.lblUrbanBoundaryDataSource.TabIndex = 8;
            this.lblUrbanBoundaryDataSource.Text = "城镇开发边界数据源：";
            this.lblUrbanBoundaryDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUrbanBoundaryDataPath
            // 
            this.txtUrbanBoundaryDataPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUrbanBoundaryDataPath.Location = new System.Drawing.Point(27, 264);
            this.txtUrbanBoundaryDataPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUrbanBoundaryDataPath.Name = "txtUrbanBoundaryDataPath";
            this.txtUrbanBoundaryDataPath.ReadOnly = true;
            this.txtUrbanBoundaryDataPath.Size = new System.Drawing.Size(808, 28);
            this.txtUrbanBoundaryDataPath.TabIndex = 9;
            this.txtUrbanBoundaryDataPath.Text = "请选择城镇开发边界数据源文件夹";
            // 
            // btnBrowseUrbanBoundaryData
            // 
            this.btnBrowseUrbanBoundaryData.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseUrbanBoundaryData.Location = new System.Drawing.Point(859, 264);
            this.btnBrowseUrbanBoundaryData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseUrbanBoundaryData.Name = "btnBrowseUrbanBoundaryData";
            this.btnBrowseUrbanBoundaryData.Size = new System.Drawing.Size(105, 34);
            this.btnBrowseUrbanBoundaryData.TabIndex = 10;
            this.btnBrowseUrbanBoundaryData.Text = "浏览...";
            this.btnBrowseUrbanBoundaryData.UseVisualStyleBackColor = true;
            this.btnBrowseUrbanBoundaryData.Click += new System.EventHandler(this.BtnBrowseUrbanBoundaryData_Click);
            // 
            // lblOutputGDBPath
            // 
            this.lblOutputGDBPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOutputGDBPath.ForeColor = System.Drawing.Color.Black;
            this.lblOutputGDBPath.Location = new System.Drawing.Point(27, 317);
            this.lblOutputGDBPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputGDBPath.Name = "lblOutputGDBPath";
            this.lblOutputGDBPath.Size = new System.Drawing.Size(225, 30);
            this.lblOutputGDBPath.TabIndex = 11;
            this.lblOutputGDBPath.Text = "输出结果GDB路径：";
            this.lblOutputGDBPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtOutputGDBPath
            // 
            this.txtOutputGDBPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOutputGDBPath.Location = new System.Drawing.Point(27, 354);
            this.txtOutputGDBPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOutputGDBPath.Name = "txtOutputGDBPath";
            this.txtOutputGDBPath.ReadOnly = true;
            this.txtOutputGDBPath.Size = new System.Drawing.Size(808, 28);
            this.txtOutputGDBPath.TabIndex = 12;
            this.txtOutputGDBPath.Text = "请选择输出结果GDB路径";
            // 
            // btnBrowseOutputGDB
            // 
            this.btnBrowseOutputGDB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseOutputGDB.Location = new System.Drawing.Point(859, 354);
            this.btnBrowseOutputGDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseOutputGDB.Name = "btnBrowseOutputGDB";
            this.btnBrowseOutputGDB.Size = new System.Drawing.Size(105, 34);
            this.btnBrowseOutputGDB.TabIndex = 13;
            this.btnBrowseOutputGDB.Text = "浏览...";
            this.btnBrowseOutputGDB.UseVisualStyleBackColor = true;
            this.btnBrowseOutputGDB.Click += new System.EventHandler(this.BtnBrowseOutputGDB_Click);
            // 
            // chkCreateCountyFolders
            // 
            this.chkCreateCountyFolders.AutoSize = true;
            this.chkCreateCountyFolders.Checked = true;
            this.chkCreateCountyFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateCountyFolders.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkCreateCountyFolders.Location = new System.Drawing.Point(27, 414);
            this.chkCreateCountyFolders.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCreateCountyFolders.Name = "chkCreateCountyFolders";
            this.chkCreateCountyFolders.Size = new System.Drawing.Size(358, 22);
            this.chkCreateCountyFolders.TabIndex = 14;
            this.chkCreateCountyFolders.Text = "为每个县创建文件夹并生成三种结果表格";
            this.chkCreateCountyFolders.UseVisualStyleBackColor = true;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.btnOK);
            this.bottomPanel.Controls.Add(this.btnCancel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 468);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(1001, 75);
            this.bottomPanel.TabIndex = 21;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(600, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 45);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(750, 15);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 45);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BasicDataPreparationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 543);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BasicDataPreparationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "基础数据准备";
            this.Load += new System.EventHandler(this.BasicDataPreparationForm_Load);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}