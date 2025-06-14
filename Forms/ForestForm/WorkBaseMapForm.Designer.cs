using System.Windows.Forms;
using System.Drawing;
using System;
namespace TestArcMapAddin2.Forms.ForestForm
{
    partial class WorkBaseMapForm
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
            this.dataSourceGroupBox = new System.Windows.Forms.GroupBox();
            this.lblWorkScope = new System.Windows.Forms.Label();
            this.txtWorkScope = new System.Windows.Forms.TextBox();
            this.btnBrowseWorkScope = new System.Windows.Forms.Button();
            this.lblForestGrading = new System.Windows.Forms.Label();
            this.txtForestGrading = new System.Windows.Forms.TextBox();
            this.btnBrowseGrading = new System.Windows.Forms.Button();
            this.lblPriceData = new System.Windows.Forms.Label();
            this.txtPriceData = new System.Windows.Forms.TextBox();
            this.btnBrowsePrice = new System.Windows.Forms.Button();
            this.btnLoadCurrentMap = new System.Windows.Forms.Button();
            this.processingOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.chkSpatialJoin = new System.Windows.Forms.CheckBox();
            this.cmbJoinMethod = new System.Windows.Forms.ComboBox();
            this.chkOverwriteExisting = new System.Windows.Forms.CheckBox();
            this.mainPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.dataSourceGroupBox.SuspendLayout();
            this.processingOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.dataSourceGroupBox);
            this.mainPanel.Controls.Add(this.processingOptionsGroupBox);
            this.mainPanel.Size = new System.Drawing.Size(637, 538);
            this.mainPanel.Controls.SetChildIndex(this.processingOptionsGroupBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.dataSourceGroupBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.statusLabel, 0);
            this.mainPanel.Controls.SetChildIndex(this.logTextBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.descriptionTextBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.progressBar, 0);
            this.mainPanel.Controls.SetChildIndex(this.titleLabel, 0);
            // 
            // titleLabel
            // 
            this.titleLabel.Location = new System.Drawing.Point(26, 22);
            this.titleLabel.Size = new System.Drawing.Size(599, 45);
            this.titleLabel.Text = "2. 制作森林工作底图";
            this.titleLabel.Click += new System.EventHandler(this.titleLabel_Click);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(13, 76);
            this.descriptionTextBox.Size = new System.Drawing.Size(595, 67);
            this.descriptionTextBox.Text = "工作范围与林地分等数据关联，补充完善数据库；工作范围与林地定级数据关联，将基准地价通过空间挂接森林图班。";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(15, 380);
            this.logTextBox.Size = new System.Drawing.Size(600, 110);
            // 
            // statusLabel
            // 
            this.statusLabel.Location = new System.Drawing.Point(15, 500);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(122, 500);
            this.progressBar.Size = new System.Drawing.Size(493, 30);
            // 
            // startButton
            // 
            this.startButton.Size = new System.Drawing.Size(180, 37);
            // 
            // cancelButton
            // 
            this.cancelButton.Size = new System.Drawing.Size(180, 37);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(474, 23);
            this.closeButton.Size = new System.Drawing.Size(134, 36);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Location = new System.Drawing.Point(0, 538);
            this.bottomPanel.Size = new System.Drawing.Size(637, 63);
            // 
            // dataSourceGroupBox
            // 
            this.dataSourceGroupBox.Controls.Add(this.lblWorkScope);
            this.dataSourceGroupBox.Controls.Add(this.txtWorkScope);
            this.dataSourceGroupBox.Controls.Add(this.btnBrowseWorkScope);
            this.dataSourceGroupBox.Controls.Add(this.lblForestGrading);
            this.dataSourceGroupBox.Controls.Add(this.txtForestGrading);
            this.dataSourceGroupBox.Controls.Add(this.btnBrowseGrading);
            this.dataSourceGroupBox.Controls.Add(this.lblPriceData);
            this.dataSourceGroupBox.Controls.Add(this.txtPriceData);
            this.dataSourceGroupBox.Controls.Add(this.btnBrowsePrice);
            this.dataSourceGroupBox.Controls.Add(this.btnLoadCurrentMap);
            this.dataSourceGroupBox.Location = new System.Drawing.Point(15, 150);
            this.dataSourceGroupBox.Name = "dataSourceGroupBox";
            this.dataSourceGroupBox.Size = new System.Drawing.Size(600, 150);
            this.dataSourceGroupBox.TabIndex = 5;
            this.dataSourceGroupBox.TabStop = false;
            this.dataSourceGroupBox.Text = "数据来源";
            // 
            // lblWorkScope
            // 
            this.lblWorkScope.Location = new System.Drawing.Point(15, 25);
            this.lblWorkScope.Name = "lblWorkScope";
            this.lblWorkScope.Size = new System.Drawing.Size(150, 20);
            this.lblWorkScope.TabIndex = 0;
            this.lblWorkScope.Text = "工作范围数据:";
            this.lblWorkScope.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWorkScope
            // 
            this.txtWorkScope.Location = new System.Drawing.Point(170, 25);
            this.txtWorkScope.Name = "txtWorkScope";
            this.txtWorkScope.ReadOnly = true;
            this.txtWorkScope.Size = new System.Drawing.Size(320, 28);
            this.txtWorkScope.TabIndex = 1;
            // 
            // btnBrowseWorkScope
            // 
            this.btnBrowseWorkScope.Location = new System.Drawing.Point(500, 24);
            this.btnBrowseWorkScope.Name = "btnBrowseWorkScope";
            this.btnBrowseWorkScope.Size = new System.Drawing.Size(80, 29);
            this.btnBrowseWorkScope.TabIndex = 2;
            this.btnBrowseWorkScope.Text = "浏览...";
            this.btnBrowseWorkScope.UseVisualStyleBackColor = true;
            this.btnBrowseWorkScope.Click += new System.EventHandler(this.BrowseWorkScope_Click);
            // 
            // lblForestGrading
            // 
            this.lblForestGrading.Location = new System.Drawing.Point(15, 55);
            this.lblForestGrading.Name = "lblForestGrading";
            this.lblForestGrading.Size = new System.Drawing.Size(150, 20);
            this.lblForestGrading.TabIndex = 3;
            this.lblForestGrading.Text = "林地分等数据:";
            this.lblForestGrading.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtForestGrading
            // 
            this.txtForestGrading.Location = new System.Drawing.Point(170, 55);
            this.txtForestGrading.Name = "txtForestGrading";
            this.txtForestGrading.ReadOnly = true;
            this.txtForestGrading.Size = new System.Drawing.Size(320, 28);
            this.txtForestGrading.TabIndex = 4;
            // 
            // btnBrowseGrading
            // 
            this.btnBrowseGrading.Location = new System.Drawing.Point(500, 57);
            this.btnBrowseGrading.Name = "btnBrowseGrading";
            this.btnBrowseGrading.Size = new System.Drawing.Size(80, 26);
            this.btnBrowseGrading.TabIndex = 5;
            this.btnBrowseGrading.Text = "浏览...";
            this.btnBrowseGrading.UseVisualStyleBackColor = true;
            this.btnBrowseGrading.Click += new System.EventHandler(this.BrowseGrading_Click);
            // 
            // lblPriceData
            // 
            this.lblPriceData.Location = new System.Drawing.Point(15, 85);
            this.lblPriceData.Name = "lblPriceData";
            this.lblPriceData.Size = new System.Drawing.Size(150, 20);
            this.lblPriceData.TabIndex = 6;
            this.lblPriceData.Text = "基准地价数据:";
            this.lblPriceData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPriceData
            // 
            this.txtPriceData.Location = new System.Drawing.Point(170, 85);
            this.txtPriceData.Name = "txtPriceData";
            this.txtPriceData.ReadOnly = true;
            this.txtPriceData.Size = new System.Drawing.Size(320, 28);
            this.txtPriceData.TabIndex = 7;
            // 
            // btnBrowsePrice
            // 
            this.btnBrowsePrice.Location = new System.Drawing.Point(500, 85);
            this.btnBrowsePrice.Name = "btnBrowsePrice";
            this.btnBrowsePrice.Size = new System.Drawing.Size(80, 29);
            this.btnBrowsePrice.TabIndex = 8;
            this.btnBrowsePrice.Text = "浏览...";
            this.btnBrowsePrice.UseVisualStyleBackColor = true;
            this.btnBrowsePrice.Click += new System.EventHandler(this.BrowsePrice_Click);
            // 
            // btnLoadCurrentMap
            // 
            this.btnLoadCurrentMap.Location = new System.Drawing.Point(200, 115);
            this.btnLoadCurrentMap.Name = "btnLoadCurrentMap";
            this.btnLoadCurrentMap.Size = new System.Drawing.Size(180, 29);
            this.btnLoadCurrentMap.TabIndex = 9;
            this.btnLoadCurrentMap.Text = "加载当前地图数据";
            this.btnLoadCurrentMap.UseVisualStyleBackColor = true;
            this.btnLoadCurrentMap.Click += new System.EventHandler(this.BtnLoadCurrentMap_Click);
            // 
            // processingOptionsGroupBox
            // 
            this.processingOptionsGroupBox.Controls.Add(this.chkSpatialJoin);
            this.processingOptionsGroupBox.Controls.Add(this.cmbJoinMethod);
            this.processingOptionsGroupBox.Controls.Add(this.chkOverwriteExisting);
            this.processingOptionsGroupBox.Location = new System.Drawing.Point(15, 310);
            this.processingOptionsGroupBox.Name = "processingOptionsGroupBox";
            this.processingOptionsGroupBox.Size = new System.Drawing.Size(600, 60);
            this.processingOptionsGroupBox.TabIndex = 6;
            this.processingOptionsGroupBox.TabStop = false;
            this.processingOptionsGroupBox.Text = "处理选项";
            // 
            // chkSpatialJoin
            // 
            this.chkSpatialJoin.Checked = true;
            this.chkSpatialJoin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSpatialJoin.Location = new System.Drawing.Point(20, 25);
            this.chkSpatialJoin.Name = "chkSpatialJoin";
            this.chkSpatialJoin.Size = new System.Drawing.Size(150, 26);
            this.chkSpatialJoin.TabIndex = 0;
            this.chkSpatialJoin.Text = "使用空间连接方式";
            this.chkSpatialJoin.UseVisualStyleBackColor = true;
            // 
            // cmbJoinMethod
            // 
            this.cmbJoinMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJoinMethod.FormattingEnabled = true;
            this.cmbJoinMethod.Items.AddRange(new object[] {
            "相交",
            "包含",
            "完全包含",
            "最近"});
            this.cmbJoinMethod.Location = new System.Drawing.Point(180, 25);
            this.cmbJoinMethod.Name = "cmbJoinMethod";
            this.cmbJoinMethod.Size = new System.Drawing.Size(200, 26);
            this.cmbJoinMethod.TabIndex = 1;
            // 
            // chkOverwriteExisting
            // 
            this.chkOverwriteExisting.Checked = true;
            this.chkOverwriteExisting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverwriteExisting.Location = new System.Drawing.Point(402, 28);
            this.chkOverwriteExisting.Name = "chkOverwriteExisting";
            this.chkOverwriteExisting.Size = new System.Drawing.Size(150, 23);
            this.chkOverwriteExisting.TabIndex = 2;
            this.chkOverwriteExisting.Text = "覆盖现有数据";
            this.chkOverwriteExisting.UseVisualStyleBackColor = true;
            // 
            // WorkBaseMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 601);
            this.Name = "WorkBaseMapForm";
            this.Text = "制作森林工作底图";
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.dataSourceGroupBox.ResumeLayout(false);
            this.dataSourceGroupBox.PerformLayout();
            this.processingOptionsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox dataSourceGroupBox;
        private System.Windows.Forms.Label lblWorkScope;
        private System.Windows.Forms.TextBox txtWorkScope;
        private System.Windows.Forms.Button btnBrowseWorkScope;
        private System.Windows.Forms.Label lblForestGrading;
        private System.Windows.Forms.TextBox txtForestGrading;
        private System.Windows.Forms.Button btnBrowseGrading;
        private System.Windows.Forms.Label lblPriceData;
        private System.Windows.Forms.TextBox txtPriceData;
        private System.Windows.Forms.Button btnBrowsePrice;
        private System.Windows.Forms.Button btnLoadCurrentMap;
        private System.Windows.Forms.GroupBox processingOptionsGroupBox;
        private System.Windows.Forms.CheckBox chkSpatialJoin;
        private System.Windows.Forms.ComboBox cmbJoinMethod;
        private System.Windows.Forms.CheckBox chkOverwriteExisting;
    }
}