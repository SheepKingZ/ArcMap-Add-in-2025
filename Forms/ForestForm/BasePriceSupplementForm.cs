using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using TestArcMapAddin2.Forms.ForestForm;

namespace TestArcMapAddin2.Forms
{
    /// <summary>
    /// Form for supplementing forest benchmark prices
    /// </summary>
    public partial class BasePriceSupplementForm : ForestProcessingFormBase
    {
        private bool processingCancelled = false;
        
        public BasePriceSupplementForm()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "���������ֵػ�׼�ؼ�";
            this.titleLabel.Text = "4. ���������ֵػ�׼�ؼ�";
            
            this.descriptionTextBox.Text = 
                "���ɭ��ͼ���ڿռ�ҽӻ�׼�ؼ�ʱ��������յ���������������ֵض���ָ�꼰Ȩ����Ϣ��" +
                "��׼�۸���Ϣ�����в������㣬ȷ��ɭ��ͼ�߻�׼�۸���ȱ©��";
            
            // Add custom controls specific to this form
            GroupBox dataSourceGroupBox = new GroupBox
            {
                Text = "������Դ",
                Location = new Point(15, 150),
                Size = new Size(600, 120)
            };
            
            Label lblWorkBaseMap = new Label
            {
                Text = "������ͼ����:",
                Location = new Point(15, 25),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            TextBox txtWorkBaseMap = new TextBox
            {
                Location = new Point(170, 25),
                Size = new Size(320, 20),
                ReadOnly = true
            };
            
            Button btnBrowseWorkBaseMap = new Button
            {
                Text = "���...",
                Location = new Point(500, 24),
                Size = new Size(80, 23)
            };
            
            Label lblParameters = new Label
            {
                Text = "�۸��������:",
                Location = new Point(15, 55),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            TextBox txtParameters = new TextBox
            {
                Location = new Point(170, 55),
                Size = new Size(320, 20),
                ReadOnly = true
            };
            
            Button btnBrowseParameters = new Button
            {
                Text = "���...",
                Location = new Point(500, 54),
                Size = new Size(80, 23)
            };
            
            Button btnLoadCurrentMap = new Button
            {
                Text = "���ص�ǰ��ͼ����",
                Location = new Point(200, 85),
                Size = new Size(180, 25)
            };
            
            // Add controls to the group box
            dataSourceGroupBox.Controls.Add(lblWorkBaseMap);
            dataSourceGroupBox.Controls.Add(txtWorkBaseMap);
            dataSourceGroupBox.Controls.Add(btnBrowseWorkBaseMap);
            dataSourceGroupBox.Controls.Add(lblParameters);
            dataSourceGroupBox.Controls.Add(txtParameters);
            dataSourceGroupBox.Controls.Add(btnBrowseParameters);
            dataSourceGroupBox.Controls.Add(btnLoadCurrentMap);
            
            // Add supplementation method group box
            GroupBox methodGroupBox = new GroupBox
            {
                Text = "���䷽��",
                Location = new Point(15, 280),
                Size = new Size(600, 90)
            };
            
            RadioButton rbNearest = new RadioButton
            {
                Text = "���������ͼ�߼۸�",
                Location = new Point(20, 25),
                Size = new Size(200, 20),
                Checked = true
            };
            
            RadioButton rbInterpolation = new RadioButton
            {
                Text = "�ռ��ֵ��",
                Location = new Point(20, 55),
                Size = new Size(200, 20)
            };
            
            RadioButton rbRegression = new RadioButton
            {
                Text = "�ع������",
                Location = new Point(240, 25),
                Size = new Size(200, 20)
            };
            
            RadioButton rbAverage = new RadioButton
            {
                Text = "����ƽ��ֵ��",
                Location = new Point(240, 55),
                Size = new Size(200, 20)
            };
            
            CheckBox chkManualAdjust = new CheckBox
            {
                Text = "�����ֶ������쳣ֵ",
                Location = new Point(450, 25),
                Size = new Size(150, 20),
                Checked = true
            };
            
            methodGroupBox.Controls.Add(rbNearest);
            methodGroupBox.Controls.Add(rbInterpolation);
            methodGroupBox.Controls.Add(rbRegression);
            methodGroupBox.Controls.Add(rbAverage);
            methodGroupBox.Controls.Add(chkManualAdjust);
            
            // Adjust the location of the existing log text box
            this.logTextBox.Location = new Point(15, 380);
            this.logTextBox.Size = new Size(600, 110);
            
            // Adjust other control positions
            this.statusLabel.Location = new Point(15, 500);
            this.progressBar.Location = new Point(15, 525);
            
            // Add the group boxes to the main panel
            this.mainPanel.Controls.Add(dataSourceGroupBox);
            this.mainPanel.Controls.Add(methodGroupBox);
            
            // Wire up the button events
            btnBrowseWorkBaseMap.Click += (sender, e) => BrowseForData(txtWorkBaseMap, "ѡ������ͼ����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
            btnBrowseParameters.Click += (sender, e) => BrowseForData(txtParameters, "ѡ��۸��������", "Excel�ļ� (*.xlsx)|*.xlsx|�����ļ� (*.*)|*.*");
            btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
        }

        private void BrowseForData(TextBox textBox, string title, string filter)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = title;
                dlg.Filter = filter;
                
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = dlg.FileName;
                }
            }
        }
        
        private void BtnLoadCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڴӵ�ǰ��ͼ��������...");
                // In a real implementation, this would use ArcObjects to access the current map
                
                // For demo purposes, just fill in a sample path
                var txtWorkBaseMap = this.mainPanel.Controls.Find("txtWorkBaseMap", true)[0] as TextBox;
                var txtParameters = this.mainPanel.Controls.Find("txtParameters", true)[0] as TextBox;
                
                txtWorkBaseMap.Text = @"C:\GIS_Data\ɭ�ֹ�����ͼ.shp";
                txtParameters.Text = @"C:\GIS_Data\�ֵؼ۸����.xlsx";
                
                Log("�����Ѵӵ�ǰ��ͼ����");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }

        protected override void StartButton_Click(object sender, EventArgs e)
        {
            base.StartButton_Click(sender, e);
            
            // Get the file paths from the text boxes
            var txtWorkBaseMap = this.mainPanel.Controls.Find("txtWorkBaseMap", true)[0] as TextBox;
            var txtParameters = this.mainPanel.Controls.Find("txtParameters", true)[0] as TextBox;
            
            if (string.IsNullOrEmpty(txtWorkBaseMap.Text) || string.IsNullOrEmpty(txtParameters.Text))
            {
                MessageBox.Show("����ѡ�����������ļ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }
            
            // Get supplementation method
            string supplementMethod = "nearest"; // default
            
            var rbNearest = this.mainPanel.Controls.Find("rbNearest", true)[0] as RadioButton;
            var rbInterpolation = this.mainPanel.Controls.Find("rbInterpolation", true)[0] as RadioButton;
            var rbRegression = this.mainPanel.Controls.Find("rbRegression", true)[0] as RadioButton;
            var rbAverage = this.mainPanel.Controls.Find("rbAverage", true)[0] as RadioButton;
            var chkManualAdjust = this.mainPanel.Controls.Find("chkManualAdjust", true)[0] as CheckBox;
            
            if (rbNearest.Checked)
                supplementMethod = "nearest";
            else if (rbInterpolation.Checked)
                supplementMethod = "interpolation";
            else if (rbRegression.Checked)
                supplementMethod = "regression";
            else if (rbAverage.Checked)
                supplementMethod = "average";
                
            bool allowManualAdjustment = chkManualAdjust.Checked;
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessSupplementBasePrice(
                txtWorkBaseMap.Text,
                txtParameters.Text,
                supplementMethod,
                allowManualAdjustment));
        }
        
        private void ProcessSupplementBasePrice(
            string workBaseMapPath,
            string parametersPath,
            string supplementMethod,
            bool allowManualAdjustment)
        {
            try
            {
                UpdateStatus("����׼������...");
                Log("��ʼ���������ֵػ�׼�ؼ�����");
                Log($"ʹ�ù�����ͼ����: {workBaseMapPath}");
                Log($"ʹ�ü۸��������: {parametersPath}");
                Log($"���䷽��: {supplementMethod}");
                Log($"�����ֶ������쳣ֵ: {allowManualAdjustment}");
                
                // Simulate the processing steps
                for (int i = 0; i <= 100; i += 5)
                {
                    if (processingCancelled) return;
                    
                    switch (i)
                    {
                        case 0:
                            UpdateStatus("���ڼ��ع�����ͼ����...");
                            Log("���ع�����ͼ������...");
                            break;
                        case 10:
                            UpdateStatus("���ڼ��ؼ۸��������...");
                            Log("���ؼ۸����������...");
                            break;
                        case 20:
                            UpdateStatus("����ɸѡ�޻�׼�۸��ͼ��...");
                            Log("ɸѡ�޻�׼�۸��ͼ����...");
                            break;
                        case 30:
                            UpdateStatus("���ڼ��㲹��۸�...");
                            Log($"ʹ��{supplementMethod}�������㲹��۸���...");
                            break;
                        case 60:
                            UpdateStatus("������֤�۸������...");
                            Log("��֤�۸��������...");
                            break;
                        case 80:
                            if (allowManualAdjustment)
                            {
                                UpdateStatus("���ڵ����쳣�۸�...");
                                Log("�����쳣�۸���...");
                                // In a real app, might show a dialog here to adjust values
                            }
                            break;
                        case 90:
                            UpdateStatus("���ڸ���ͼ�߼۸�����...");
                            Log("����ͼ�߼۸�������...");
                            break;
                    }
                    
                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }
                
                Log("�ֵػ�׼�ؼ۲������");
                Log("����ɹ�: 27ͼ��");
                Log("�۸�Χ: 2.45-8.72��Ԫ/����");
                Log("ƽ���۸�: 4.83��Ԫ/����");
                
                // Complete the processing
                this.Invoke(new Action(() => OnProcessingComplete(true)));
            }
            catch (Exception ex)
            {
                Log($"����: {ex.Message}");
                this.Invoke(new Action(() => 
                {
                    MessageBox.Show($"��������г���: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    OnProcessingComplete(false);
                }));
            }
        }
        
        protected override void CancelButton_Click(object sender, EventArgs e)
        {
            base.CancelButton_Click(sender, e);
            processingCancelled = true;
        }
    }
}