using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase; // 用于 IFields

namespace TestArcMapAddin2.Forms
{
    public partial class FieldMappingForm : Form
    {
        public Dictionary<string, string> FieldMappings { get; private set; }

        // 构造函数将获取源字段和目标字段以填充窗体
        public FieldMappingForm(IFields sourceFields, IFields targetFields)
        {
            InitializeComponent();
            // 使用源字段和目标字段填充 DataGridView 或其他控件
            // 允许用户映射它们。为简单起见，这是一个占位符。
            // dgvFieldMappings.Columns.Add("SourceField", "源字段");
            // dgvFieldMappings.Columns.Add("TargetField", "目标字段");
            // ... 填充行 ...
        }

        private void InitializeComponent()
        {
            // 组件初始化代码保持不变
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.dataGridViewMappings = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMappings)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(12, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(135, 13);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "配置字段映射：";
            // 
            // dataGridViewMappings
            // 
            this.dataGridViewMappings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMappings.Location = new System.Drawing.Point(15, 25);
            this.dataGridViewMappings.Name = "dataGridViewMappings";
            this.dataGridViewMappings.Size = new System.Drawing.Size(357, 199);
            this.dataGridViewMappings.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(216, 230);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(297, 230);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // FieldMappingForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dataGridViewMappings);
            this.Controls.Add(this.labelInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FieldMappingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "字段映射";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMappings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.DataGridView dataGridViewMappings;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            FieldMappings = new Dictionary<string, string>();
            // 从 DataGridView 填充 FieldMappings
            // foreach (DataGridViewRow row in dataGridViewMappings.Rows)
            // {
            //    if (row.Cells["SourceField"].Value != null && row.Cells["TargetField"].Value != null)
            //    {
            //        FieldMappings[row.Cells["SourceField"].Value.ToString()] = row.Cells["TargetField"].Value.ToString();
            //    }
            // }
            this.DialogResult = DialogResult.OK;
        }
    }
}