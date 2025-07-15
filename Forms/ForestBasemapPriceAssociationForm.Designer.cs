namespace TestArcMapAddin2.Forms
{
    partial class ForestBasemapPriceAssociationForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxDataInput = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLandGradePricePath = new System.Windows.Forms.TextBox();
            this.btnBrowseLandGradePrice = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtForestResourcePath = new System.Windows.Forms.TextBox();
            this.btnBrowseForestResource = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.btnImportPriceMapping = new System.Windows.Forms.Button();
            this.btnPairData = new System.Windows.Forms.Button();
            this.groupBoxDataPairs = new System.Windows.Forms.GroupBox();
            this.dataGridViewPairs = new System.Windows.Forms.DataGridView();
            this.groupBoxProcessing = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnProcessData = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxDataInput.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxDataPairs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPairs)).BeginInit();
            this.groupBoxProcessing.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxDataInput, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxDataPairs, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxProcessing, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1090, 700);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBoxDataInput
            // 
            this.groupBoxDataInput.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxDataInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDataInput.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxDataInput.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDataInput.Name = "groupBoxDataInput";
            this.groupBoxDataInput.Size = new System.Drawing.Size(1084, 194);
            this.groupBoxDataInput.TabIndex = 0;
            this.groupBoxDataInput.TabStop = false;
            this.groupBoxDataInput.Text = "数据导入";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLandGradePricePath, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseLandGradePrice, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtForestResourcePath, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseForestResource, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtOutputPath, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseOutput, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnImportPriceMapping, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnPairData, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1078, 164);
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
            this.label1.Text = "林地基准地价定级数据:";
            // 
            // txtLandGradePricePath
            // 
            this.txtLandGradePricePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLandGradePricePath.ForeColor = System.Drawing.Color.Gray;
            this.txtLandGradePricePath.Location = new System.Drawing.Point(153, 3);
            this.txtLandGradePricePath.Name = "txtLandGradePricePath";
            this.txtLandGradePricePath.ReadOnly = true;
            this.txtLandGradePricePath.Size = new System.Drawing.Size(842, 31);
            this.txtLandGradePricePath.TabIndex = 1;
            this.txtLandGradePricePath.Text = "请选择林地基准地价定级数据文件夹路径";
            // 
            // btnBrowseLandGradePrice
            // 
            this.btnBrowseLandGradePrice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseLandGradePrice.Location = new System.Drawing.Point(1001, 6);
            this.btnBrowseLandGradePrice.Name = "btnBrowseLandGradePrice";
            this.btnBrowseLandGradePrice.Size = new System.Drawing.Size(74, 23);
            this.btnBrowseLandGradePrice.TabIndex = 2;
            this.btnBrowseLandGradePrice.Text = "浏览...";
            this.btnBrowseLandGradePrice.UseVisualStyleBackColor = true;
            this.btnBrowseLandGradePrice.Click += new System.EventHandler(this.btnBrowseLandGradePrice_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 35);
            this.label2.TabIndex = 3;
            this.label2.Text = "森林资源地类图斑数据:";
            // 
            // txtForestResourcePath
            // 
            this.txtForestResourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtForestResourcePath.ForeColor = System.Drawing.Color.Gray;
            this.txtForestResourcePath.Location = new System.Drawing.Point(153, 38);
            this.txtForestResourcePath.Name = "txtForestResourcePath";
            this.txtForestResourcePath.ReadOnly = true;
            this.txtForestResourcePath.Size = new System.Drawing.Size(842, 31);
            this.txtForestResourcePath.TabIndex = 4;
            this.txtForestResourcePath.Text = "请选择森林资源地类图斑数据文件夹路径";
            // 
            // btnBrowseForestResource
            // 
            this.btnBrowseForestResource.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseForestResource.Location = new System.Drawing.Point(1001, 41);
            this.btnBrowseForestResource.Name = "btnBrowseForestResource";
            this.btnBrowseForestResource.Size = new System.Drawing.Size(74, 23);
            this.btnBrowseForestResource.TabIndex = 5;
            this.btnBrowseForestResource.Text = "浏览...";
            this.btnBrowseForestResource.UseVisualStyleBackColor = true;
            this.btnBrowseForestResource.Click += new System.EventHandler(this.btnBrowseForestResource_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "输出路径:";
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputPath.ForeColor = System.Drawing.Color.Gray;
            this.txtOutputPath.Location = new System.Drawing.Point(153, 73);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(842, 31);
            this.txtOutputPath.TabIndex = 7;
            this.txtOutputPath.Text = "请选择输出结果文件夹路径";
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseOutput.Location = new System.Drawing.Point(1001, 76);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(74, 23);
            this.btnBrowseOutput.TabIndex = 8;
            this.btnBrowseOutput.Text = "浏览...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // btnImportPriceMapping
            // 
            this.btnImportPriceMapping.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnImportPriceMapping.BackColor = System.Drawing.Color.LightBlue;
            this.btnImportPriceMapping.Location = new System.Drawing.Point(10, 119);
            this.btnImportPriceMapping.Name = "btnImportPriceMapping";
            this.btnImportPriceMapping.Size = new System.Drawing.Size(130, 30);
            this.btnImportPriceMapping.TabIndex = 9;
            this.btnImportPriceMapping.Text = "导入价格映射表";
            this.btnImportPriceMapping.UseVisualStyleBackColor = false;
            this.btnImportPriceMapping.Click += new System.EventHandler(this.btnImportPriceMapping_Click);
            // 
            // btnPairData
            // 
            this.btnPairData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPairData.BackColor = System.Drawing.Color.LightGreen;
            this.btnPairData.Enabled = false;
            this.btnPairData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPairData.Location = new System.Drawing.Point(514, 119);
            this.btnPairData.Name = "btnPairData";
            this.btnPairData.Size = new System.Drawing.Size(120, 30);
            this.btnPairData.TabIndex = 10;
            this.btnPairData.Text = "配对读取";
            this.btnPairData.UseVisualStyleBackColor = false;
            this.btnPairData.Click += new System.EventHandler(this.btnPairData_Click);
            // 
            // groupBoxDataPairs
            // 
            this.groupBoxDataPairs.Controls.Add(this.dataGridViewPairs);
            this.groupBoxDataPairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDataPairs.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxDataPairs.Location = new System.Drawing.Point(3, 203);
            this.groupBoxDataPairs.Name = "groupBoxDataPairs";
            this.groupBoxDataPairs.Size = new System.Drawing.Size(1084, 394);
            this.groupBoxDataPairs.TabIndex = 1;
            this.groupBoxDataPairs.TabStop = false;
            this.groupBoxDataPairs.Text = "数据配对结果";
            // 
            // dataGridViewPairs
            // 
            this.dataGridViewPairs.AllowUserToAddRows = false;
            this.dataGridViewPairs.AllowUserToDeleteRows = false;
            this.dataGridViewPairs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPairs.Location = new System.Drawing.Point(3, 27);
            this.dataGridViewPairs.Name = "dataGridViewPairs";
            this.dataGridViewPairs.RowHeadersWidth = 62;
            this.dataGridViewPairs.RowTemplate.Height = 23;
            this.dataGridViewPairs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPairs.Size = new System.Drawing.Size(1078, 364);
            this.dataGridViewPairs.TabIndex = 0;
            // 
            // groupBoxProcessing
            // 
            this.groupBoxProcessing.Controls.Add(this.tableLayoutPanel3);
            this.groupBoxProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcessing.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxProcessing.Location = new System.Drawing.Point(3, 603);
            this.groupBoxProcessing.Name = "groupBoxProcessing";
            this.groupBoxProcessing.Size = new System.Drawing.Size(1084, 94);
            this.groupBoxProcessing.TabIndex = 2;
            this.groupBoxProcessing.TabStop = false;
            this.groupBoxProcessing.Text = "数据处理";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.Controls.Add(this.btnProcessData, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.progressBar, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.statusLabel, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1078, 64);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // btnProcessData
            // 
            this.btnProcessData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnProcessData.BackColor = System.Drawing.Color.Orange;
            this.btnProcessData.Enabled = false;
            this.btnProcessData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProcessData.Location = new System.Drawing.Point(10, 17);
            this.btnProcessData.Name = "btnProcessData";
            this.btnProcessData.Size = new System.Drawing.Size(130, 30);
            this.btnProcessData.TabIndex = 0;
            this.btnProcessData.Text = "生成核算价格属性表";
            this.btnProcessData.UseVisualStyleBackColor = false;
            this.btnProcessData.Click += new System.EventHandler(this.btnProcessData_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(153, 22);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(642, 20);
            this.progressBar.TabIndex = 1;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(801, 20);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(46, 24);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "就绪";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Location = new System.Drawing.Point(1001, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ForestBasemapPriceAssociationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1090, 700);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ForestBasemapPriceAssociationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "森林底图与价格关联处理";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxDataInput.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBoxDataPairs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPairs)).EndInit();
            this.groupBoxProcessing.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxDataInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLandGradePricePath;
        private System.Windows.Forms.Button btnBrowseLandGradePrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtForestResourcePath;
        private System.Windows.Forms.Button btnBrowseForestResource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Button btnImportPriceMapping;
        private System.Windows.Forms.Button btnPairData;
        private System.Windows.Forms.GroupBox groupBoxDataPairs;
        private System.Windows.Forms.DataGridView dataGridViewPairs;
        private System.Windows.Forms.GroupBox groupBoxProcessing;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnProcessData;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button btnClose;
    }
}