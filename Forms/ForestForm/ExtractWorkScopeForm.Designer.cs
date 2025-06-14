namespace TestArcMapAddin2.Forms
{
    partial class ExtractWorkScopeForm
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
            this.lblInventoryData = new System.Windows.Forms.Label();
            this.txtInventoryData = new System.Windows.Forms.TextBox();
            this.btnBrowseInventory = new System.Windows.Forms.Button();
            this.lblUrbanBoundary = new System.Windows.Forms.Label();
            this.txtUrbanBoundary = new System.Windows.Forms.TextBox();
            this.btnBrowseBoundary = new System.Windows.Forms.Button();
            this.btnLoadCurrentMap = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.dataSourceGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.dataSourceGroupBox);
            this.mainPanel.Size = new System.Drawing.Size(951, 628);
            this.mainPanel.Controls.SetChildIndex(this.dataSourceGroupBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.statusLabel, 0);
            this.mainPanel.Controls.SetChildIndex(this.logTextBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.descriptionTextBox, 0);
            this.mainPanel.Controls.SetChildIndex(this.progressBar, 0);
            this.mainPanel.Controls.SetChildIndex(this.titleLabel, 0);
            // 
            // titleLabel
            // 
            this.titleLabel.Location = new System.Drawing.Point(96, 14);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.titleLabel.Size = new System.Drawing.Size(741, 62);
            this.titleLabel.Text = "1. ��ȡɭ�ֹ�����Χ";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.descriptionTextBox.Size = new System.Drawing.Size(898, 116);
            this.descriptionTextBox.Text = "���ֲ�ʪ���ղ�����Ϊ�������������򿪷��߽�ʸ�����ݣ�ɸѡ��ȡ����Ϊ�ֵأ�������Ȩ������Ϊ���е�ͼ�ߵؿ飬�����Ϊ�ֵأ�����Ȩ������Ϊ���嵫��λ�ڳ��򿪷��߽��ڵ�ͼ" +
    "�ߵؿ飬��Ϊ�أ�����ɭ����Դ�ʲ���鹤����Χ��";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(22, 388);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.logTextBox.Size = new System.Drawing.Size(898, 192);
            // 
            // statusLabel
            // 
            this.statusLabel.Location = new System.Drawing.Point(22, 580);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(135, 588);
            this.progressBar.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.progressBar.Size = new System.Drawing.Size(685, 30);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Location = new System.Drawing.Point(0, 628);
            this.bottomPanel.Size = new System.Drawing.Size(951, 80);
            // 
            // dataSourceGroupBox
            // 
            this.dataSourceGroupBox.Controls.Add(this.lblInventoryData);
            this.dataSourceGroupBox.Controls.Add(this.txtInventoryData);
            this.dataSourceGroupBox.Controls.Add(this.btnBrowseInventory);
            this.dataSourceGroupBox.Controls.Add(this.lblUrbanBoundary);
            this.dataSourceGroupBox.Controls.Add(this.txtUrbanBoundary);
            this.dataSourceGroupBox.Controls.Add(this.btnBrowseBoundary);
            this.dataSourceGroupBox.Controls.Add(this.btnLoadCurrentMap);
            this.dataSourceGroupBox.Location = new System.Drawing.Point(22, 208);
            this.dataSourceGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataSourceGroupBox.Name = "dataSourceGroupBox";
            this.dataSourceGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataSourceGroupBox.Size = new System.Drawing.Size(900, 166);
            this.dataSourceGroupBox.TabIndex = 0;
            this.dataSourceGroupBox.TabStop = false;
            this.dataSourceGroupBox.Text = "������Դ";
            // 
            // lblInventoryData
            // 
            this.lblInventoryData.Location = new System.Drawing.Point(22, 35);
            this.lblInventoryData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInventoryData.Name = "lblInventoryData";
            this.lblInventoryData.Size = new System.Drawing.Size(225, 28);
            this.lblInventoryData.TabIndex = 0;
            this.lblInventoryData.Text = "�ֲ�ʪ���ղ�����:";
            this.lblInventoryData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtInventoryData
            // 
            this.txtInventoryData.Location = new System.Drawing.Point(255, 35);
            this.txtInventoryData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtInventoryData.Name = "txtInventoryData";
            this.txtInventoryData.ReadOnly = true;
            this.txtInventoryData.Size = new System.Drawing.Size(478, 28);
            this.txtInventoryData.TabIndex = 1;
            // 
            // btnBrowseInventory
            // 
            this.btnBrowseInventory.Location = new System.Drawing.Point(750, 36);
            this.btnBrowseInventory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseInventory.Name = "btnBrowseInventory";
            this.btnBrowseInventory.Size = new System.Drawing.Size(120, 32);
            this.btnBrowseInventory.TabIndex = 2;
            this.btnBrowseInventory.Text = "���...";
            this.btnBrowseInventory.UseVisualStyleBackColor = true;
            // 
            // lblUrbanBoundary
            // 
            this.lblUrbanBoundary.Location = new System.Drawing.Point(22, 76);
            this.lblUrbanBoundary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUrbanBoundary.Name = "lblUrbanBoundary";
            this.lblUrbanBoundary.Size = new System.Drawing.Size(225, 28);
            this.lblUrbanBoundary.TabIndex = 3;
            this.lblUrbanBoundary.Text = "���򿪷��߽�����:";
            this.lblUrbanBoundary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUrbanBoundary
            // 
            this.txtUrbanBoundary.Location = new System.Drawing.Point(255, 76);
            this.txtUrbanBoundary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUrbanBoundary.Name = "txtUrbanBoundary";
            this.txtUrbanBoundary.ReadOnly = true;
            this.txtUrbanBoundary.Size = new System.Drawing.Size(478, 28);
            this.txtUrbanBoundary.TabIndex = 4;
            // 
            // btnBrowseBoundary
            // 
            this.btnBrowseBoundary.Location = new System.Drawing.Point(750, 76);
            this.btnBrowseBoundary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseBoundary.Name = "btnBrowseBoundary";
            this.btnBrowseBoundary.Size = new System.Drawing.Size(120, 32);
            this.btnBrowseBoundary.TabIndex = 5;
            this.btnBrowseBoundary.Text = "���...";
            this.btnBrowseBoundary.UseVisualStyleBackColor = true;
            // 
            // btnLoadCurrentMap
            // 
            this.btnLoadCurrentMap.Location = new System.Drawing.Point(300, 118);
            this.btnLoadCurrentMap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadCurrentMap.Name = "btnLoadCurrentMap";
            this.btnLoadCurrentMap.Size = new System.Drawing.Size(270, 35);
            this.btnLoadCurrentMap.TabIndex = 6;
            this.btnLoadCurrentMap.Text = "���ص�ǰ��ͼ����";
            this.btnLoadCurrentMap.UseVisualStyleBackColor = true;
            // 
            // ExtractWorkScopeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 708);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "ExtractWorkScopeForm";
            this.Text = "��ȡɭ�ֹ�����Χ";
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.dataSourceGroupBox.ResumeLayout(false);
            this.dataSourceGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox dataSourceGroupBox;
        private System.Windows.Forms.Label lblInventoryData;
        private System.Windows.Forms.TextBox txtInventoryData;
        private System.Windows.Forms.Button btnBrowseInventory;
        private System.Windows.Forms.Label lblUrbanBoundary;
        private System.Windows.Forms.TextBox txtUrbanBoundary;
        private System.Windows.Forms.Button btnBrowseBoundary;
        private System.Windows.Forms.Button btnLoadCurrentMap;
    }
}