namespace TestArcMapAddin2.Forms
{
    partial class BasicDataPreparationForm
    {
        private System.ComponentModel.IContainer components = null;

        // 修改控件声明 - 分离两个数据源
        private System.Windows.Forms.Label lblDataSource;
        private System.Windows.Forms.TextBox txtDataPath;
        private System.Windows.Forms.Button btnBrowseData;

        // 新增：数据类型选择控件
        private System.Windows.Forms.GroupBox grpDataType;
        private System.Windows.Forms.CheckBox chkForest;
        private System.Windows.Forms.CheckBox chkGrassland;

        // 修改：城镇开发边界和地类图斑数据源控件（重命名以更准确反映功能）
        private System.Windows.Forms.Label lblCzkfbjDltbDataSource;
        private System.Windows.Forms.TextBox txtCzkfbjDltbPath;
        private System.Windows.Forms.Button btnBrowseCzkfbjDltbData;

        private System.Windows.Forms.Label lblOutputGDBPath;
        private System.Windows.Forms.TextBox txtOutputGDBPath;
        private System.Windows.Forms.Button btnBrowseOutputGDB;

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
            this.lblDataSource = new System.Windows.Forms.Label();
            this.txtDataPath = new System.Windows.Forms.TextBox();
            this.btnBrowseData = new System.Windows.Forms.Button();
            this.grpDataType = new System.Windows.Forms.GroupBox();
            this.chkForest = new System.Windows.Forms.CheckBox();
            this.chkGrassland = new System.Windows.Forms.CheckBox();
            this.lblCzkfbjDltbDataSource = new System.Windows.Forms.Label();
            this.txtCzkfbjDltbPath = new System.Windows.Forms.TextBox();
            this.btnBrowseCzkfbjDltbData = new System.Windows.Forms.Button();
            this.lblOutputGDBPath = new System.Windows.Forms.Label();
            this.txtOutputGDBPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputGDB = new System.Windows.Forms.Button();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.buttonResultStructure = new System.Windows.Forms.Button();
            this.btnResultExcel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.topPanel.SuspendLayout();
            this.grpDataType.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.titleLabel);
            this.topPanel.Controls.Add(this.lblDataSource);
            this.topPanel.Controls.Add(this.txtDataPath);
            this.topPanel.Controls.Add(this.btnBrowseData);
            this.topPanel.Controls.Add(this.grpDataType);
            this.topPanel.Controls.Add(this.lblCzkfbjDltbDataSource);
            this.topPanel.Controls.Add(this.txtCzkfbjDltbPath);
            this.topPanel.Controls.Add(this.btnBrowseCzkfbjDltbData);
            this.topPanel.Controls.Add(this.lblOutputGDBPath);
            this.topPanel.Controls.Add(this.txtOutputGDBPath);
            this.topPanel.Controls.Add(this.btnBrowseOutputGDB);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Margin = new System.Windows.Forms.Padding(4);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(15);
            this.topPanel.Size = new System.Drawing.Size(1000, 543);
            this.topPanel.TabIndex = 22;
            this.topPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.topPanel_Paint);
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
            // lblDataSource
            // 
            this.lblDataSource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDataSource.ForeColor = System.Drawing.Color.Black;
            this.lblDataSource.Location = new System.Drawing.Point(19, 74);
            this.lblDataSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(560, 30);
            this.lblDataSource.TabIndex = 5;
            this.lblDataSource.Text = "林草湿荒普查数据源（LCXZGX_P）：";
            this.lblDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDataPath
            // 
            this.txtDataPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDataPath.Location = new System.Drawing.Point(19, 112);
            this.txtDataPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtDataPath.Name = "txtDataPath";
            this.txtDataPath.ReadOnly = true;
            this.txtDataPath.Size = new System.Drawing.Size(808, 28);
            this.txtDataPath.TabIndex = 6;
            this.txtDataPath.Text = "请选择包含林草湿荒普查数据的文件夹";
            // 
            // btnBrowseData
            // 
            this.btnBrowseData.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseData.Location = new System.Drawing.Point(852, 112);
            this.btnBrowseData.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseData.Name = "btnBrowseData";
            this.btnBrowseData.Size = new System.Drawing.Size(105, 34);
            this.btnBrowseData.TabIndex = 7;
            this.btnBrowseData.Text = "浏览...";
            this.btnBrowseData.UseVisualStyleBackColor = true;
            this.btnBrowseData.Click += new System.EventHandler(this.BtnBrowseData_Click);
            // 
            // grpDataType
            // 
            this.grpDataType.Controls.Add(this.chkForest);
            this.grpDataType.Controls.Add(this.chkGrassland);
            this.grpDataType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpDataType.Location = new System.Drawing.Point(19, 154);
            this.grpDataType.Margin = new System.Windows.Forms.Padding(4);
            this.grpDataType.Name = "grpDataType";
            this.grpDataType.Padding = new System.Windows.Forms.Padding(4);
            this.grpDataType.Size = new System.Drawing.Size(400, 80);
            this.grpDataType.TabIndex = 8;
            this.grpDataType.TabStop = false;
            this.grpDataType.Text = "选择处理的数据类型";
            // 
            // chkForest
            // 
            this.chkForest.AutoSize = true;
            this.chkForest.Checked = true;
            this.chkForest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkForest.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkForest.Location = new System.Drawing.Point(20, 34);
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
            this.chkGrassland.Location = new System.Drawing.Point(150, 34);
            this.chkGrassland.Margin = new System.Windows.Forms.Padding(4);
            this.chkGrassland.Name = "chkGrassland";
            this.chkGrassland.Size = new System.Drawing.Size(70, 22);
            this.chkGrassland.TabIndex = 1;
            this.chkGrassland.Text = "草地";
            this.chkGrassland.UseVisualStyleBackColor = true;
            this.chkGrassland.CheckedChanged += new System.EventHandler(this.chkGrassland_CheckedChanged);
            // 
            // lblCzkfbjDltbDataSource
            // 
            this.lblCzkfbjDltbDataSource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCzkfbjDltbDataSource.ForeColor = System.Drawing.Color.Black;
            this.lblCzkfbjDltbDataSource.Location = new System.Drawing.Point(19, 242);
            this.lblCzkfbjDltbDataSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCzkfbjDltbDataSource.Name = "lblCzkfbjDltbDataSource";
            this.lblCzkfbjDltbDataSource.Size = new System.Drawing.Size(900, 30);
            this.lblCzkfbjDltbDataSource.TabIndex = 9;
            this.lblCzkfbjDltbDataSource.Text = "城镇开发边界与森林资源地类图斑数据源（CZKFBJ、SLZY_DLTB）：";
            this.lblCzkfbjDltbDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCzkfbjDltbPath
            // 
            this.txtCzkfbjDltbPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCzkfbjDltbPath.Location = new System.Drawing.Point(19, 280);
            this.txtCzkfbjDltbPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtCzkfbjDltbPath.Name = "txtCzkfbjDltbPath";
            this.txtCzkfbjDltbPath.ReadOnly = true;
            this.txtCzkfbjDltbPath.Size = new System.Drawing.Size(808, 28);
            this.txtCzkfbjDltbPath.TabIndex = 10;
            this.txtCzkfbjDltbPath.Text = "请选择包含城镇开发边界和地类图斑数据的文件夹";
            // 
            // btnBrowseCzkfbjDltbData
            // 
            this.btnBrowseCzkfbjDltbData.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseCzkfbjDltbData.Location = new System.Drawing.Point(852, 280);
            this.btnBrowseCzkfbjDltbData.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseCzkfbjDltbData.Name = "btnBrowseCzkfbjDltbData";
            this.btnBrowseCzkfbjDltbData.Size = new System.Drawing.Size(105, 34);
            this.btnBrowseCzkfbjDltbData.TabIndex = 11;
            this.btnBrowseCzkfbjDltbData.Text = "浏览...";
            this.btnBrowseCzkfbjDltbData.UseVisualStyleBackColor = true;
            this.btnBrowseCzkfbjDltbData.Click += new System.EventHandler(this.BtnBrowseCzkfbjDltbData_Click);
            // 
            // lblOutputGDBPath
            // 
            this.lblOutputGDBPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOutputGDBPath.ForeColor = System.Drawing.Color.Black;
            this.lblOutputGDBPath.Location = new System.Drawing.Point(19, 322);
            this.lblOutputGDBPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputGDBPath.Name = "lblOutputGDBPath";
            this.lblOutputGDBPath.Size = new System.Drawing.Size(225, 30);
            this.lblOutputGDBPath.TabIndex = 12;
            this.lblOutputGDBPath.Text = "输出结果路径：";
            this.lblOutputGDBPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOutputGDBPath.Click += new System.EventHandler(this.lblOutputGDBPath_Click);
            // 
            // txtOutputGDBPath
            // 
            this.txtOutputGDBPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOutputGDBPath.Location = new System.Drawing.Point(19, 360);
            this.txtOutputGDBPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputGDBPath.Name = "txtOutputGDBPath";
            this.txtOutputGDBPath.ReadOnly = true;
            this.txtOutputGDBPath.Size = new System.Drawing.Size(808, 28);
            this.txtOutputGDBPath.TabIndex = 13;
            this.txtOutputGDBPath.Text = "请选择输出结果路径";
            // 
            // btnBrowseOutputGDB
            // 
            this.btnBrowseOutputGDB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowseOutputGDB.Location = new System.Drawing.Point(852, 360);
            this.btnBrowseOutputGDB.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseOutputGDB.Name = "btnBrowseOutputGDB";
            this.btnBrowseOutputGDB.Size = new System.Drawing.Size(105, 34);
            this.btnBrowseOutputGDB.TabIndex = 14;
            this.btnBrowseOutputGDB.Text = "浏览...";
            this.btnBrowseOutputGDB.UseVisualStyleBackColor = true;
            this.btnBrowseOutputGDB.Click += new System.EventHandler(this.BtnBrowseOutputGDB_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.LightGray;
            this.bottomPanel.Controls.Add(this.buttonResultStructure);
            this.bottomPanel.Controls.Add(this.btnResultExcel);
            this.bottomPanel.Controls.Add(this.button1);
            this.bottomPanel.Controls.Add(this.btnOK);
            this.bottomPanel.Controls.Add(this.btnCancel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 468);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(4);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(1000, 75);
            this.bottomPanel.TabIndex = 21;
            // 
            // buttonResultStructure
            // 
            this.buttonResultStructure.Location = new System.Drawing.Point(88, 22);
            this.buttonResultStructure.Name = "buttonResultStructure";
            this.buttonResultStructure.Size = new System.Drawing.Size(156, 34);
            this.buttonResultStructure.TabIndex = 4;
            this.buttonResultStructure.Text = "创建目录结构";
            this.buttonResultStructure.UseVisualStyleBackColor = true;
            this.buttonResultStructure.Click += new System.EventHandler(this.buttonResultStructure_Click);
            // 
            // btnResultExcel
            // 
            this.btnResultExcel.Location = new System.Drawing.Point(255, 22);
            this.btnResultExcel.Name = "btnResultExcel";
            this.btnResultExcel.Size = new System.Drawing.Size(156, 34);
            this.btnResultExcel.TabIndex = 3;
            this.btnResultExcel.Text = "创建结果表格";
            this.btnResultExcel.UseVisualStyleBackColor = true;
            this.btnResultExcel.Click += new System.EventHandler(this.btnResultExcel_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(418, 22);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "创建Shapefile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(600, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
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
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
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
            this.ClientSize = new System.Drawing.Size(1000, 543);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BasicDataPreparationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "基础数据准备";
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.grpDataType.ResumeLayout(false);
            this.grpDataType.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnResultExcel;
        private System.Windows.Forms.Button buttonResultStructure;
    }
}