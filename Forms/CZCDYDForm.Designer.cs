namespace TestArcMapAddin2.Forms
{
    partial class CZCDYDForm
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
            this.lblSlzyzcPath = new System.Windows.Forms.Label();
            this.txtSlzyzcPath = new System.Windows.Forms.TextBox();
            this.btnBrowseSlzyzc = new System.Windows.Forms.Button();
            this.lblCyzyzcPath = new System.Windows.Forms.Label();
            this.txtCyzyzcPath = new System.Windows.Forms.TextBox();
            this.btnBrowseCyzyzc = new System.Windows.Forms.Button();
            this.lblSdzyzcPath = new System.Windows.Forms.Label();
            this.txtSdzyzcPath = new System.Windows.Forms.TextBox();
            this.btnBrowseSdzyzc = new System.Windows.Forms.Button();
            this.lblCzcdydPath = new System.Windows.Forms.Label();
            this.txtCzcdydPath = new System.Windows.Forms.TextBox();
            this.btnBrowseCzcdyd = new System.Windows.Forms.Button();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            // 新增：输出结果路径选择框
            this.lblResultOutputPath = new System.Windows.Forms.Label();
            this.txtResultOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseResultOutput = new System.Windows.Forms.Button();
            this.lblCountySelection = new System.Windows.Forms.Label();
            this.checkedListBoxCounties = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSlzyzcPath
            // 
            this.lblSlzyzcPath.AutoSize = true;
            this.lblSlzyzcPath.Location = new System.Drawing.Point(18, 22);
            this.lblSlzyzcPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSlzyzcPath.Name = "lblSlzyzcPath";
            this.lblSlzyzcPath.Size = new System.Drawing.Size(224, 18);
            this.lblSlzyzcPath.TabIndex = 0;
            this.lblSlzyzcPath.Text = "选择SLZYZC_DLTB源文件夹:";
            // 
            // txtSlzyzcPath
            // 
            this.txtSlzyzcPath.ForeColor = System.Drawing.Color.Gray;
            this.txtSlzyzcPath.Location = new System.Drawing.Point(18, 52);
            this.txtSlzyzcPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtSlzyzcPath.Name = "txtSlzyzcPath";
            this.txtSlzyzcPath.ReadOnly = true;
            this.txtSlzyzcPath.Size = new System.Drawing.Size(580, 28);
            this.txtSlzyzcPath.TabIndex = 1;
            this.txtSlzyzcPath.Text = "请选择包含SLZYZC_DLTB数据的文件夹";
            // 
            // btnBrowseSlzyzc
            // 
            this.btnBrowseSlzyzc.Location = new System.Drawing.Point(612, 50);
            this.btnBrowseSlzyzc.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseSlzyzc.Name = "btnBrowseSlzyzc";
            this.btnBrowseSlzyzc.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseSlzyzc.TabIndex = 2;
            this.btnBrowseSlzyzc.Text = "浏览...";
            this.btnBrowseSlzyzc.UseVisualStyleBackColor = true;
            this.btnBrowseSlzyzc.Click += new System.EventHandler(this.btnBrowseSlzyzc_Click);
            // 
            // lblCyzyzcPath
            // 
            this.lblCyzyzcPath.AutoSize = true;
            this.lblCyzyzcPath.Location = new System.Drawing.Point(18, 100);
            this.lblCyzyzcPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCyzyzcPath.Name = "lblCyzyzcPath";
            this.lblCyzyzcPath.Size = new System.Drawing.Size(224, 18);
            this.lblCyzyzcPath.TabIndex = 3;
            this.lblCyzyzcPath.Text = "选择CYZYZC_DLTB源文件夹:";
            // 
            // txtCyzyzcPath
            // 
            this.txtCyzyzcPath.ForeColor = System.Drawing.Color.Gray;
            this.txtCyzyzcPath.Location = new System.Drawing.Point(18, 130);
            this.txtCyzyzcPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtCyzyzcPath.Name = "txtCyzyzcPath";
            this.txtCyzyzcPath.ReadOnly = true;
            this.txtCyzyzcPath.Size = new System.Drawing.Size(580, 28);
            this.txtCyzyzcPath.TabIndex = 4;
            this.txtCyzyzcPath.Text = "请选择包含CYZYZC_DLTB数据的文件夹";
            // 
            // btnBrowseCyzyzc
            // 
            this.btnBrowseCyzyzc.Location = new System.Drawing.Point(612, 128);
            this.btnBrowseCyzyzc.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseCyzyzc.Name = "btnBrowseCyzyzc";
            this.btnBrowseCyzyzc.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseCyzyzc.TabIndex = 5;
            this.btnBrowseCyzyzc.Text = "浏览...";
            this.btnBrowseCyzyzc.UseVisualStyleBackColor = true;
            this.btnBrowseCyzyzc.Click += new System.EventHandler(this.btnBrowseCyzyzc_Click);
            // 
            // lblSdzyzcPath
            // 
            this.lblSdzyzcPath.AutoSize = true;
            this.lblSdzyzcPath.Location = new System.Drawing.Point(18, 178);
            this.lblSdzyzcPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSdzyzcPath.Name = "lblSdzyzcPath";
            this.lblSdzyzcPath.Size = new System.Drawing.Size(224, 18);
            this.lblSdzyzcPath.TabIndex = 6;
            this.lblSdzyzcPath.Text = "选择SDZYZC_DLTB源文件夹:";
            // 
            // txtSdzyzcPath
            // 
            this.txtSdzyzcPath.ForeColor = System.Drawing.Color.Gray;
            this.txtSdzyzcPath.Location = new System.Drawing.Point(18, 208);
            this.txtSdzyzcPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtSdzyzcPath.Name = "txtSdzyzcPath";
            this.txtSdzyzcPath.ReadOnly = true;
            this.txtSdzyzcPath.Size = new System.Drawing.Size(580, 28);
            this.txtSdzyzcPath.TabIndex = 7;
            this.txtSdzyzcPath.Text = "请选择包含SDZYZC_DLTB数据的文件夹";
            // 
            // btnBrowseSdzyzc
            // 
            this.btnBrowseSdzyzc.Location = new System.Drawing.Point(612, 206);
            this.btnBrowseSdzyzc.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseSdzyzc.Name = "btnBrowseSdzyzc";
            this.btnBrowseSdzyzc.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseSdzyzc.TabIndex = 8;
            this.btnBrowseSdzyzc.Text = "浏览...";
            this.btnBrowseSdzyzc.UseVisualStyleBackColor = true;
            this.btnBrowseSdzyzc.Click += new System.EventHandler(this.btnBrowseSdzyzc_Click);
            // 
            // lblCzcdydPath
            // 
            this.lblCzcdydPath.AutoSize = true;
            this.lblCzcdydPath.Location = new System.Drawing.Point(18, 256);
            this.lblCzcdydPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCzcdydPath.Name = "lblCzcdydPath";
            this.lblCzcdydPath.Size = new System.Drawing.Size(215, 18);
            this.lblCzcdydPath.TabIndex = 9;
            this.lblCzcdydPath.Text = "选择CZCDYD数据源文件夹:";
            // 
            // txtCzcdydPath
            // 
            this.txtCzcdydPath.ForeColor = System.Drawing.Color.Gray;
            this.txtCzcdydPath.Location = new System.Drawing.Point(18, 286);
            this.txtCzcdydPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtCzcdydPath.Name = "txtCzcdydPath";
            this.txtCzcdydPath.ReadOnly = true;
            this.txtCzcdydPath.Size = new System.Drawing.Size(580, 28);
            this.txtCzcdydPath.TabIndex = 10;
            this.txtCzcdydPath.Text = "请选择包含CZCDYD数据的文件夹";
            // 
            // btnBrowseCzcdyd
            // 
            this.btnBrowseCzcdyd.Location = new System.Drawing.Point(612, 284);
            this.btnBrowseCzcdyd.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseCzcdyd.Name = "btnBrowseCzcdyd";
            this.btnBrowseCzcdyd.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseCzcdyd.TabIndex = 11;
            this.btnBrowseCzcdyd.Text = "浏览...";
            this.btnBrowseCzcdyd.UseVisualStyleBackColor = true;
            this.btnBrowseCzcdyd.Click += new System.EventHandler(this.btnBrowseCzcdyd_Click);
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Location = new System.Drawing.Point(18, 334);
            this.lblOutputPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(215, 18);
            this.lblOutputPath.TabIndex = 12;
            this.lblOutputPath.Text = "选择CZCDYDQC结果输出路径:";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.ForeColor = System.Drawing.Color.Gray;
            this.txtOutputPath.Location = new System.Drawing.Point(18, 364);
            this.txtOutputPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(580, 28);
            this.txtOutputPath.TabIndex = 13;
            this.txtOutputPath.Text = "请选择CZCDYDQC结果输出文件夹";
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(612, 362);
            this.btnBrowseOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseOutput.TabIndex = 14;
            this.btnBrowseOutput.Text = "浏览...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // lblResultOutputPath
            // 
            this.lblResultOutputPath.AutoSize = true;
            this.lblResultOutputPath.Location = new System.Drawing.Point(18, 412);
            this.lblResultOutputPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResultOutputPath.Name = "lblResultOutputPath";
            this.lblResultOutputPath.Size = new System.Drawing.Size(233, 18);
            this.lblResultOutputPath.TabIndex = 15;
            this.lblResultOutputPath.Text = "选择现有CZCDYDQC结果文件夹:";
            // 
            // txtResultOutputPath
            // 
            this.txtResultOutputPath.ForeColor = System.Drawing.Color.Gray;
            this.txtResultOutputPath.Location = new System.Drawing.Point(18, 442);
            this.txtResultOutputPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtResultOutputPath.Name = "txtResultOutputPath";
            this.txtResultOutputPath.ReadOnly = true;
            this.txtResultOutputPath.Size = new System.Drawing.Size(580, 28);
            this.txtResultOutputPath.TabIndex = 16;
            this.txtResultOutputPath.Text = "请选择包含CZCDYDQC结果的文件夹";
            // 
            // btnBrowseResultOutput
            // 
            this.btnBrowseResultOutput.Location = new System.Drawing.Point(612, 440);
            this.btnBrowseResultOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseResultOutput.Name = "btnBrowseResultOutput";
            this.btnBrowseResultOutput.Size = new System.Drawing.Size(100, 32);
            this.btnBrowseResultOutput.TabIndex = 17;
            this.btnBrowseResultOutput.Text = "浏览...";
            this.btnBrowseResultOutput.UseVisualStyleBackColor = true;
            this.btnBrowseResultOutput.Click += new System.EventHandler(this.btnBrowseResultOutput_Click);
            // 
            // lblCountySelection
            // 
            this.lblCountySelection.AutoSize = true;
            this.lblCountySelection.Location = new System.Drawing.Point(18, 490);
            this.lblCountySelection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCountySelection.Name = "lblCountySelection";
            this.lblCountySelection.Size = new System.Drawing.Size(179, 18);
            this.lblCountySelection.TabIndex = 18;
            this.lblCountySelection.Text = "选择要处理的县(区):";
            // 
            // checkedListBoxCounties
            // 
            this.checkedListBoxCounties.CheckOnClick = true;
            this.checkedListBoxCounties.FormattingEnabled = true;
            this.checkedListBoxCounties.Location = new System.Drawing.Point(18, 520);
            this.checkedListBoxCounties.Margin = new System.Windows.Forms.Padding(4);
            this.checkedListBoxCounties.Name = "checkedListBoxCounties";
            this.checkedListBoxCounties.Size = new System.Drawing.Size(580, 301);
            this.checkedListBoxCounties.TabIndex = 19;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(612, 520);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 32);
            this.btnSelectAll.TabIndex = 20;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(612, 560);
            this.btnSelectNone.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(100, 32);
            this.btnSelectNone.TabIndex = 21;
            this.btnSelectNone.Text = "全不选";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(18, 865);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(694, 25);
            this.progressBar.TabIndex = 22;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(500, 907);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 40);
            this.btnOK.TabIndex = 23;
            this.btnOK.Text = "开始处理";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(612, 907);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 40);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblStatus.Location = new System.Drawing.Point(18, 837);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(71, 18);
            this.lblStatus.TabIndex = 25;
            this.lblStatus.Text = "就绪...";
            // 
            // CZCDYDForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(730, 960);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.checkedListBoxCounties);
            this.Controls.Add(this.lblCountySelection);
            this.Controls.Add(this.btnBrowseResultOutput);
            this.Controls.Add(this.txtResultOutputPath);
            this.Controls.Add(this.lblResultOutputPath);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.lblOutputPath);
            this.Controls.Add(this.btnBrowseCzcdyd);
            this.Controls.Add(this.txtCzcdydPath);
            this.Controls.Add(this.lblCzcdydPath);
            this.Controls.Add(this.btnBrowseSdzyzc);
            this.Controls.Add(this.txtSdzyzcPath);
            this.Controls.Add(this.lblSdzyzcPath);
            this.Controls.Add(this.btnBrowseCyzyzc);
            this.Controls.Add(this.txtCyzyzcPath);
            this.Controls.Add(this.lblCyzyzcPath);
            this.Controls.Add(this.btnBrowseSlzyzc);
            this.Controls.Add(this.txtSlzyzcPath);
            this.Controls.Add(this.lblSlzyzcPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CZCDYDForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "城镇村等用地数据处理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSlzyzcPath;
        private System.Windows.Forms.TextBox txtSlzyzcPath;
        private System.Windows.Forms.Button btnBrowseSlzyzc;
        private System.Windows.Forms.Label lblCyzyzcPath;
        private System.Windows.Forms.TextBox txtCyzyzcPath;
        private System.Windows.Forms.Button btnBrowseCyzyzc;
        private System.Windows.Forms.Label lblSdzyzcPath;
        private System.Windows.Forms.TextBox txtSdzyzcPath;
        private System.Windows.Forms.Button btnBrowseSdzyzc;
        private System.Windows.Forms.Label lblCzcdydPath;
        private System.Windows.Forms.TextBox txtCzcdydPath;
        private System.Windows.Forms.Button btnBrowseCzcdyd;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Label lblResultOutputPath;
        private System.Windows.Forms.TextBox txtResultOutputPath;
        private System.Windows.Forms.Button btnBrowseResultOutput;
        private System.Windows.Forms.Label lblCountySelection;
        private System.Windows.Forms.CheckedListBox checkedListBoxCounties;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
    }
}