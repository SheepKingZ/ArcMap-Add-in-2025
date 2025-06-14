using System.Windows.Forms;
using System.Collections.Generic; // 导入 List<string> 所需的命名空间
using System.Linq; // 导入 LINQ 操作所需的命名空间

namespace TestArcMapAddin2.Forms
{
    public partial class CountySelectionForm : Form
    {
        // 支持多选
        public List<string> SelectedCounties { get; private set; }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBoxCounties; // 从 ComboBox 更改
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;

        public CountySelectionForm()
        {
            InitializeComponent();
            // 使用县名填充 CheckedListBox
            // 示例县名，如果需要，请替换为实际数据源
            checkedListBoxCounties.Items.Add("县A");
            checkedListBoxCounties.Items.Add("县B");
            checkedListBoxCounties.Items.Add("县C");
            // 根据需要添加更多县
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBoxCounties = new System.Windows.Forms.CheckedListBox(); // 已更改
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择一个或多个县："; // 文本已更新
            // 
            // checkedListBoxCounties
            // 
            this.checkedListBoxCounties.FormattingEnabled = true;
            this.checkedListBoxCounties.Location = new System.Drawing.Point(15, 35); 
            this.checkedListBoxCounties.Name = "checkedListBoxCounties";
            this.checkedListBoxCounties.Size = new System.Drawing.Size(257, 94);
            this.checkedListBoxCounties.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(116, 135);
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
            this.buttonCancel.Location = new System.Drawing.Point(197, 135);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // CountySelectionForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(284, 171);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkedListBoxCounties);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CountySelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择县";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            if (checkedListBoxCounties.CheckedItems.Count > 0)
            {
                SelectedCounties = new List<string>();
                foreach (var item in checkedListBoxCounties.CheckedItems)
                {
                    SelectedCounties.Add(item.ToString());
                }
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("请至少选择一个县。", "需要选择");
            }
        }
    }
}