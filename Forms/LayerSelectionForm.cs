using System.Windows.Forms;
using ESRI.ArcGIS.Carto; // Ϊ ILayer ������

namespace TestArcMapAddin2.Forms
{
    public partial class LayerSelectionForm : Form
    {
        public ILayer SelectedLayer { get; private set; }
        // ��ʵ�ʳ����У�����ʹ�� ArcMap.Document.FocusMap �е�ͼ����� ListBox �� ComboBox
        public LayerSelectionForm(IMap map)
        {
            InitializeComponent();
            PopulateLayers(map);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxLayers = new System.Windows.Forms.ListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ѡ��Դͼ�㣺";
            // 
            // listBoxLayers
            // 
            this.listBoxLayers.FormattingEnabled = true;
            this.listBoxLayers.Location = new System.Drawing.Point(15, 25);
            this.listBoxLayers.Name = "listBoxLayers";
            this.listBoxLayers.Size = new System.Drawing.Size(257, 199);
            this.listBoxLayers.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(116, 230);
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
            this.buttonCancel.Location = new System.Drawing.Point(197, 230);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "ȡ��";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // LayerSelectionForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.listBoxLayers);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayerSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ѡ��ͼ��";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxLayers;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;

        private void PopulateLayers(IMap map)
        {
            if (map == null) return;
            for (int i = 0; i < map.LayerCount; i++)
            {
                ILayer layer = map.get_Layer(i);
                // ��ѡ���ض�ͼ�����ͣ����� IFeatureLayer������ɸѡ
                if (layer is IFeatureLayer) // �����Ҫ��ͼ��
                {
                     listBoxLayers.Items.Add(layer); // �洢ͼ�������
                }
            }
            listBoxLayers.DisplayMember = "Name"; // ��ʾ ILayer �� Name ����
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            if (listBoxLayers.SelectedItem != null)
            {
                SelectedLayer = listBoxLayers.SelectedItem as ILayer;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("��ѡ��һ��ͼ�㡣", "��Ҫѡ��");
            }
        }
    }
}