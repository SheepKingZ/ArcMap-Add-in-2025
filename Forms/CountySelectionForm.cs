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
        private System.Windows.Forms.CheckBox checkBoxSelectAll; // 添加全选复选框

        public CountySelectionForm()
        {
            InitializeComponent();
            // 使用县名填充 CheckedListBox
            // 示例县名，如果需要，请替换为实际数据源
            checkedListBoxCounties.Items.Add("县A");
            checkedListBoxCounties.Items.Add("县B");
            checkedListBoxCounties.Items.Add("县C");
            // 根据需要添加更多县
            SelectedCounties = new List<string>(); // 初始化SelectedCounties
        }

        public CountySelectionForm(List<string> previouslySelectedCounties) : this() // 调用无参构造函数
        {
            SelectedCounties = previouslySelectedCounties ?? new List<string>();

            // 根据previouslySelectedCounties预选中CheckedListBox中的项目
            for (int i = 0; i < checkedListBoxCounties.Items.Count; i++)
            {
                if (SelectedCounties.Contains(checkedListBoxCounties.Items[i].ToString()))
                {
                    checkedListBoxCounties.SetItemChecked(i, true);
                }
            }

            // 更新全选复选框的状态
            UpdateSelectAllCheckBox();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBoxCounties = new System.Windows.Forms.CheckedListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择一个或多个县：";
            // 
            // checkedListBoxCounties
            // 
            this.checkedListBoxCounties.FormattingEnabled = true;
            this.checkedListBoxCounties.Location = new System.Drawing.Point(22, 48);
            this.checkedListBoxCounties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkedListBoxCounties.Name = "checkedListBoxCounties";
            this.checkedListBoxCounties.Size = new System.Drawing.Size(384, 404);
            this.checkedListBoxCounties.TabIndex = 1;
            this.checkedListBoxCounties.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxCounties_ItemCheck);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(186, 476);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(112, 32);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(306, 476);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(112, 32);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // checkBoxSelectAll
            // 
            this.checkBoxSelectAll.AutoSize = true;
            this.checkBoxSelectAll.Location = new System.Drawing.Point(306, 13);
            this.checkBoxSelectAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxSelectAll.Name = "checkBoxSelectAll";
            this.checkBoxSelectAll.Size = new System.Drawing.Size(70, 22);
            this.checkBoxSelectAll.TabIndex = 4;
            this.checkBoxSelectAll.Text = "全选";
            this.checkBoxSelectAll.UseVisualStyleBackColor = true;
            this.checkBoxSelectAll.CheckedChanged += new System.EventHandler(this.checkBoxSelectAll_CheckedChanged);
            this.checkBoxSelectAll.Click += new System.EventHandler(this.checkBoxSelectAll_Click);
            // 
            // CountySelectionForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(426, 521);
            this.Controls.Add(this.checkBoxSelectAll);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkedListBoxCounties);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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

        // 全选复选框点击事件
        private void checkBoxSelectAll_Click(object sender, System.EventArgs e)
        {
            bool isChecked = checkBoxSelectAll.Checked;

            // 暂时取消 ItemCheck 事件以避免递归触发
            checkedListBoxCounties.ItemCheck -= checkedListBoxCounties_ItemCheck;

            // 设置所有项目的选中状态
            for (int i = 0; i < checkedListBoxCounties.Items.Count; i++)
            {
                checkedListBoxCounties.SetItemChecked(i, isChecked);
            }

            // 重新绑定 ItemCheck 事件
            checkedListBoxCounties.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxCounties_ItemCheck);
        }

        // 列表项选中状态改变事件
        private void checkedListBoxCounties_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 使用 BeginInvoke 确保在 ItemCheck 完成后更新全选框状态
            this.BeginInvoke(new MethodInvoker(() => {
                UpdateSelectAllCheckBox();
            }));
        }

        // 更新全选复选框的状态
        private void UpdateSelectAllCheckBox()
        {
            bool allChecked = checkedListBoxCounties.Items.Count > 0 &&
                              checkedListBoxCounties.CheckedItems.Count == checkedListBoxCounties.Items.Count;

            // 避免重复触发事件
            if (checkBoxSelectAll.Checked != allChecked)
            {
                checkBoxSelectAll.Checked = allChecked;
            }
        }

        private void checkBoxSelectAll_CheckedChanged(object sender, System.EventArgs e)
        {

        }
    }
}