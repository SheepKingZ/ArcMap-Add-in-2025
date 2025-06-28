namespace TestArcMapAddin2.Forms
{
    partial class SurveyDataVerificationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // 控件声明
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label titleLabel;
        
        // 文件选择模块
        private System.Windows.Forms.GroupBox groupBoxFileSelection;
        private System.Windows.Forms.Label lblSurveyData;
        private System.Windows.Forms.TextBox txtSurveyDataPath;
        private System.Windows.Forms.Button btnBrowseSurveyData;
        private System.Windows.Forms.Button btnLoadFromCurrentMap;
        
        // 校对字段选择模块
        private System.Windows.Forms.GroupBox groupBoxFieldSelection;
        private System.Windows.Forms.Label lblLandTypeField;
        private System.Windows.Forms.ComboBox cboLandTypeField;
        private System.Windows.Forms.Label lblLandCodeField;
        private System.Windows.Forms.ComboBox cboLandCodeField;
        private System.Windows.Forms.Label lblLandCategoryField;
        private System.Windows.Forms.ComboBox cboLandCategoryField;
        private System.Windows.Forms.Label lblFieldSelectionNote;
        
        // 校验和处理按钮
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnDataProcess;
        
        // 统计模块
        private System.Windows.Forms.GroupBox groupBoxStatistics;
        private System.Windows.Forms.TextBox txtStatisticsResult;
        
        // 预览模块
        private System.Windows.Forms.GroupBox groupBoxPreview;
        private System.Windows.Forms.DataGridView dgvPreview;
        
        // 导出按钮
        private System.Windows.Forms.Button btnExport;

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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.groupBoxFileSelection = new System.Windows.Forms.GroupBox();
            this.lblSurveyData = new System.Windows.Forms.Label();
            this.txtSurveyDataPath = new System.Windows.Forms.TextBox();
            this.btnBrowseSurveyData = new System.Windows.Forms.Button();
            this.btnLoadFromCurrentMap = new System.Windows.Forms.Button();
            this.groupBoxFieldSelection = new System.Windows.Forms.GroupBox();
            this.lblLandTypeField = new System.Windows.Forms.Label();
            this.cboLandTypeField = new System.Windows.Forms.ComboBox();
            this.lblLandCodeField = new System.Windows.Forms.Label();
            this.cboLandCodeField = new System.Windows.Forms.ComboBox();
            this.lblLandCategoryField = new System.Windows.Forms.Label();
            this.cboLandCategoryField = new System.Windows.Forms.ComboBox();
            this.lblFieldSelectionNote = new System.Windows.Forms.Label();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnDataProcess = new System.Windows.Forms.Button();
            this.groupBoxStatistics = new System.Windows.Forms.GroupBox();
            this.txtStatisticsResult = new System.Windows.Forms.TextBox();
            this.groupBoxPreview = new System.Windows.Forms.GroupBox();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.groupBoxFileSelection.SuspendLayout();
            this.groupBoxFieldSelection.SuspendLayout();
            this.groupBoxStatistics.SuspendLayout();
            this.groupBoxPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.Controls.Add(this.titleLabel);
            this.mainPanel.Controls.Add(this.groupBoxFileSelection);
            this.mainPanel.Controls.Add(this.groupBoxFieldSelection);
            this.mainPanel.Controls.Add(this.btnValidate);
            this.mainPanel.Controls.Add(this.btnDataProcess);
            this.mainPanel.Controls.Add(this.groupBoxStatistics);
            this.mainPanel.Controls.Add(this.groupBoxPreview);
            this.mainPanel.Controls.Add(this.btnExport);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainPanel.Size = new System.Drawing.Size(1000, 700);
            this.mainPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point(10, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(980, 30);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "通过地类编码对普查数据进行核对处理";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxFileSelection
            // 
            this.groupBoxFileSelection.Controls.Add(this.lblSurveyData);
            this.groupBoxFileSelection.Controls.Add(this.txtSurveyDataPath);
            this.groupBoxFileSelection.Controls.Add(this.btnBrowseSurveyData);
            this.groupBoxFileSelection.Controls.Add(this.btnLoadFromCurrentMap);
            this.groupBoxFileSelection.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxFileSelection.Location = new System.Drawing.Point(10, 50);
            this.groupBoxFileSelection.Name = "groupBoxFileSelection";
            this.groupBoxFileSelection.Size = new System.Drawing.Size(950, 80);
            this.groupBoxFileSelection.TabIndex = 1;
            this.groupBoxFileSelection.TabStop = false;
            this.groupBoxFileSelection.Text = "文件选择";
            // 
            // lblSurveyData
            // 
            this.lblSurveyData.Location = new System.Drawing.Point(15, 25);
            this.lblSurveyData.Name = "lblSurveyData";
            this.lblSurveyData.Size = new System.Drawing.Size(120, 20);
            this.lblSurveyData.TabIndex = 0;
            this.lblSurveyData.Text = "林草湿荒普查数据:";
            this.lblSurveyData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSurveyDataPath
            // 
            this.txtSurveyDataPath.BackColor = System.Drawing.Color.White;
            this.txtSurveyDataPath.Location = new System.Drawing.Point(140, 25);
            this.txtSurveyDataPath.Name = "txtSurveyDataPath";
            this.txtSurveyDataPath.ReadOnly = true;
            this.txtSurveyDataPath.Size = new System.Drawing.Size(600, 23);
            this.txtSurveyDataPath.TabIndex = 1;
            // 
            // btnBrowseSurveyData
            // 
            this.btnBrowseSurveyData.Location = new System.Drawing.Point(750, 24);
            this.btnBrowseSurveyData.Name = "btnBrowseSurveyData";
            this.btnBrowseSurveyData.Size = new System.Drawing.Size(80, 23);
            this.btnBrowseSurveyData.TabIndex = 2;
            this.btnBrowseSurveyData.Text = "浏览...";
            this.btnBrowseSurveyData.UseVisualStyleBackColor = true;
            this.btnBrowseSurveyData.Click += new System.EventHandler(this.BtnBrowseSurveyData_Click);
            // 
            // btnLoadFromCurrentMap
            // 
            this.btnLoadFromCurrentMap.Location = new System.Drawing.Point(840, 24);
            this.btnLoadFromCurrentMap.Name = "btnLoadFromCurrentMap";
            this.btnLoadFromCurrentMap.Size = new System.Drawing.Size(100, 23);
            this.btnLoadFromCurrentMap.TabIndex = 3;
            this.btnLoadFromCurrentMap.Text = "从当前地图加载";
            this.btnLoadFromCurrentMap.UseVisualStyleBackColor = true;
            this.btnLoadFromCurrentMap.Click += new System.EventHandler(this.BtnLoadFromCurrentMap_Click);
            // 
            // groupBoxFieldSelection
            // 
            this.groupBoxFieldSelection.Controls.Add(this.lblLandTypeField);
            this.groupBoxFieldSelection.Controls.Add(this.cboLandTypeField);
            this.groupBoxFieldSelection.Controls.Add(this.lblLandCodeField);
            this.groupBoxFieldSelection.Controls.Add(this.cboLandCodeField);
            this.groupBoxFieldSelection.Controls.Add(this.lblLandCategoryField);
            this.groupBoxFieldSelection.Controls.Add(this.cboLandCategoryField);
            this.groupBoxFieldSelection.Controls.Add(this.lblFieldSelectionNote);
            this.groupBoxFieldSelection.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxFieldSelection.Location = new System.Drawing.Point(10, 140);
            this.groupBoxFieldSelection.Name = "groupBoxFieldSelection";
            this.groupBoxFieldSelection.Size = new System.Drawing.Size(950, 120);
            this.groupBoxFieldSelection.TabIndex = 2;
            this.groupBoxFieldSelection.TabStop = false;
            this.groupBoxFieldSelection.Text = "校对字段选择";
            // 
            // lblLandTypeField
            // 
            this.lblLandTypeField.Location = new System.Drawing.Point(15, 25);
            this.lblLandTypeField.Name = "lblLandTypeField";
            this.lblLandTypeField.Size = new System.Drawing.Size(80, 20);
            this.lblLandTypeField.TabIndex = 0;
            this.lblLandTypeField.Text = "国土地类:";
            this.lblLandTypeField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboLandTypeField
            // 
            this.cboLandTypeField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLandTypeField.Location = new System.Drawing.Point(100, 25);
            this.cboLandTypeField.Name = "cboLandTypeField";
            this.cboLandTypeField.Size = new System.Drawing.Size(200, 25);
            this.cboLandTypeField.TabIndex = 1;
            // 
            // lblLandCodeField
            // 
            this.lblLandCodeField.Location = new System.Drawing.Point(320, 25);
            this.lblLandCodeField.Name = "lblLandCodeField";
            this.lblLandCodeField.Size = new System.Drawing.Size(80, 20);
            this.lblLandCodeField.TabIndex = 2;
            this.lblLandCodeField.Text = "地类编码:";
            this.lblLandCodeField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboLandCodeField
            // 
            this.cboLandCodeField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLandCodeField.Location = new System.Drawing.Point(405, 25);
            this.cboLandCodeField.Name = "cboLandCodeField";
            this.cboLandCodeField.Size = new System.Drawing.Size(200, 25);
            this.cboLandCodeField.TabIndex = 3;
            // 
            // lblLandCategoryField
            // 
            this.lblLandCategoryField.Location = new System.Drawing.Point(630, 25);
            this.lblLandCategoryField.Name = "lblLandCategoryField";
            this.lblLandCategoryField.Size = new System.Drawing.Size(50, 20);
            this.lblLandCategoryField.TabIndex = 4;
            this.lblLandCategoryField.Text = "地类:";
            this.lblLandCategoryField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboLandCategoryField
            // 
            this.cboLandCategoryField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLandCategoryField.Location = new System.Drawing.Point(685, 25);
            this.cboLandCategoryField.Name = "cboLandCategoryField";
            this.cboLandCategoryField.Size = new System.Drawing.Size(200, 25);
            this.cboLandCategoryField.TabIndex = 5;
            // 
            // lblFieldSelectionNote
            // 
            this.lblFieldSelectionNote.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblFieldSelectionNote.ForeColor = System.Drawing.Color.Gray;
            this.lblFieldSelectionNote.Location = new System.Drawing.Point(15, 55);
            this.lblFieldSelectionNote.Name = "lblFieldSelectionNote";
            this.lblFieldSelectionNote.Size = new System.Drawing.Size(400, 20);
            this.lblFieldSelectionNote.TabIndex = 6;
            this.lblFieldSelectionNote.Text = "请从上面导入的要素类数据属性表的字段中选择对应的字段";
            // 
            // btnValidate
            // 
            this.btnValidate.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnValidate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnValidate.Location = new System.Drawing.Point(390, 270);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(100, 30);
            this.btnValidate.TabIndex = 3;
            this.btnValidate.Text = "数据校验";
            this.btnValidate.UseVisualStyleBackColor = false;
            this.btnValidate.Click += new System.EventHandler(this.BtnValidate_Click);
            // 
            // btnDataProcess
            // 
            this.btnDataProcess.BackColor = System.Drawing.Color.LightCoral;
            this.btnDataProcess.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnDataProcess.Location = new System.Drawing.Point(510, 270);
            this.btnDataProcess.Name = "btnDataProcess";
            this.btnDataProcess.Size = new System.Drawing.Size(100, 30);
            this.btnDataProcess.TabIndex = 4;
            this.btnDataProcess.Text = "数据处理";
            this.btnDataProcess.UseVisualStyleBackColor = false;
            this.btnDataProcess.Click += new System.EventHandler(this.BtnDataProcess_Click);
            // 
            // groupBoxStatistics
            // 
            this.groupBoxStatistics.Controls.Add(this.txtStatisticsResult);
            this.groupBoxStatistics.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxStatistics.Location = new System.Drawing.Point(10, 310);
            this.groupBoxStatistics.Name = "groupBoxStatistics";
            this.groupBoxStatistics.Size = new System.Drawing.Size(950, 120);
            this.groupBoxStatistics.TabIndex = 5;
            this.groupBoxStatistics.TabStop = false;
            this.groupBoxStatistics.Text = "统计结果";
            // 
            // txtStatisticsResult
            // 
            this.txtStatisticsResult.BackColor = System.Drawing.Color.White;
            this.txtStatisticsResult.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtStatisticsResult.Location = new System.Drawing.Point(15, 25);
            this.txtStatisticsResult.Multiline = true;
            this.txtStatisticsResult.Name = "txtStatisticsResult";
            this.txtStatisticsResult.ReadOnly = true;
            this.txtStatisticsResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatisticsResult.Size = new System.Drawing.Size(920, 80);
            this.txtStatisticsResult.TabIndex = 0;
            // 
            // groupBoxPreview
            // 
            this.groupBoxPreview.Controls.Add(this.dgvPreview);
            this.groupBoxPreview.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxPreview.Location = new System.Drawing.Point(10, 440);
            this.groupBoxPreview.Name = "groupBoxPreview";
            this.groupBoxPreview.Size = new System.Drawing.Size(950, 180);
            this.groupBoxPreview.TabIndex = 6;
            this.groupBoxPreview.TabStop = false;
            this.groupBoxPreview.Text = "数据预览";
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPreview.Location = new System.Drawing.Point(15, 25);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPreview.Size = new System.Drawing.Size(920, 140);
            this.dgvPreview.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.LightGreen;
            this.btnExport.Enabled = false;
            this.btnExport.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(450, 630);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 30);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "导出Excel/CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // SurveyDataVerificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SurveyDataVerificationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "普查数据地类核对与编辑";
            this.mainPanel.ResumeLayout(false);
            this.groupBoxFileSelection.ResumeLayout(false);
            this.groupBoxFileSelection.PerformLayout();
            this.groupBoxFieldSelection.ResumeLayout(false);
            this.groupBoxStatistics.ResumeLayout(false);
            this.groupBoxStatistics.PerformLayout();
            this.groupBoxPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}