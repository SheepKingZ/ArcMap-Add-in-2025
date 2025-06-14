using System.Windows.Forms;
using System.Collections.Generic; // ���� List<string> ����������ռ�
using System.Linq; // ���� LINQ ��������������ռ�

namespace TestArcMapAddin2.Forms
{
    public partial class CountySelectionForm : Form
    {
        // ֧�ֶ�ѡ
        public List<string> SelectedCounties { get; private set; }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBoxCounties; // �� ComboBox ����
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;

        public CountySelectionForm()
        {
            InitializeComponent();
            // ʹ��������� CheckedListBox
            // ʾ�������������Ҫ�����滻Ϊʵ������Դ
            checkedListBoxCounties.Items.Add("��A");
            checkedListBoxCounties.Items.Add("��B");
            checkedListBoxCounties.Items.Add("��C");
            // ������Ҫ��Ӹ�����
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBoxCounties = new System.Windows.Forms.CheckedListBox(); // �Ѹ���
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
            this.label1.Text = "ѡ��һ�������أ�"; // �ı��Ѹ���
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
            this.buttonOK.Text = "ȷ��";
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
            this.buttonCancel.Text = "ȡ��";
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
            this.Text = "ѡ����";
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
                MessageBox.Show("������ѡ��һ���ء�", "��Ҫѡ��");
            }
        }
    }
}