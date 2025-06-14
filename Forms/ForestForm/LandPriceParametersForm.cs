using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestArcMapAddin2.Forms.ForestForm;

namespace TestArcMapAddin2.Forms
{
    /// <summary>
    /// Form for extracting land benchmark price parameters
    /// </summary>
    public partial class LandPriceParametersForm : ForestProcessingFormBase
    {
        private bool processingCancelled = false;
        
        public LandPriceParametersForm()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "��ȡ�ֵػ�׼�۸����";
            this.titleLabel.Text = "3. �ֵػ�׼�۸������ȡ";
            
            this.descriptionTextBox.Text = 
                "������ȡ�أ������ֵض���ָ�꼰Ȩ����Ϣ����׼�۸���Ϣ��" +
                "��׼�۸��������Ӽ�Ȩ����Ϣ�����滹ԭ�ʵ����ݡ�";
            
            // Add custom controls specific to this form
            GroupBox dataSourceGroupBox = new GroupBox
            {
                Text = "������Դ",
                Location = new Point(15, 150),
                Size = new Size(600, 90)
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
            
            Button btnLoadCurrentMap = new Button
            {
                Text = "���ص�ǰ��ͼ����",
                Location = new Point(200, 55),
                Size = new Size(180, 25)
            };
            
            // Add controls to the group box
            dataSourceGroupBox.Controls.Add(lblWorkBaseMap);
            dataSourceGroupBox.Controls.Add(txtWorkBaseMap);
            dataSourceGroupBox.Controls.Add(btnBrowseWorkBaseMap);
            dataSourceGroupBox.Controls.Add(btnLoadCurrentMap);
            
            // Add parameters group box
            GroupBox parametersGroupBox = new GroupBox
            {
                Text = "�۸������ȡѡ��",
                Location = new Point(15, 250),
                Size = new Size(600, 120)
            };
            
            CheckBox chkLevelIndicators = new CheckBox
            {
                Text = "��ȡ�ֵض���ָ�꼰Ȩ����Ϣ",
                Location = new Point(20, 25),
                Size = new Size(250, 20),
                Checked = true
            };
            
            CheckBox chkBasePrice = new CheckBox
            {
                Text = "��ȡ��׼�۸���Ϣ",
                Location = new Point(20, 50),
                Size = new Size(250, 20),
                Checked = true
            };
            
            CheckBox chkModifiers = new CheckBox
            {
                Text = "��ȡ��׼�۸��������Ӽ�Ȩ����Ϣ",
                Location = new Point(20, 75),
                Size = new Size(250, 20),
                Checked = true
            };
            
            CheckBox chkYieldRate = new CheckBox
            {
                Text = "��ȡ���滹ԭ��",
                Location = new Point(300, 25),
                Size = new Size(200, 20),
                Checked = true
            };
            
            CheckBox chkExportExcel = new CheckBox
            {
                Text = "������������Excel",
                Location = new Point(300, 50),
                Size = new Size(200, 20),
                Checked = true
            };
            
            Button btnSelectExportFolder = new Button
            {
                Text = "ѡ�񵼳��ļ���",
                Location = new Point(300, 75),
                Size = new Size(150, 25),
                Enabled = true
            };
            
            parametersGroupBox.Controls.Add(chkLevelIndicators);
            parametersGroupBox.Controls.Add(chkBasePrice);
            parametersGroupBox.Controls.Add(chkModifiers);
            parametersGroupBox.Controls.Add(chkYieldRate);
            parametersGroupBox.Controls.Add(chkExportExcel);
            parametersGroupBox.Controls.Add(btnSelectExportFolder);
            
            // Adjust the location of the existing log text box
            this.logTextBox.Location = new Point(15, 380);
            this.logTextBox.Size = new Size(600, 110);
            
            // Adjust other control positions
            this.statusLabel.Location = new Point(15, 500);
            this.progressBar.Location = new Point(15, 525);
            
            // Add the group boxes to the main panel
            this.mainPanel.Controls.Add(dataSourceGroupBox);
            this.mainPanel.Controls.Add(parametersGroupBox);
            
            // Wire up the button events
            btnBrowseWorkBaseMap.Click += (sender, e) => BrowseForData(txtWorkBaseMap, "ѡ������ͼ����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
            btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
            btnSelectExportFolder.Click += BtnSelectExportFolder_Click;
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
                txtWorkBaseMap.Text = @"C:\GIS_Data\ɭ�ֹ�����ͼ.shp";
                
                Log("�����Ѵӵ�ǰ��ͼ����");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"����: {ex.Message}");
            }
        }
        
        private void BtnSelectExportFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "ѡ����������ļ���";
                dlg.ShowNewFolderButton = true;
                
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Log($"��ѡ�񵼳��ļ���: {dlg.SelectedPath}");
                }
            }
        }

        protected override void StartButton_Click(object sender, EventArgs e)
        {
            base.StartButton_Click(sender, e);
            
            // Get the file paths from the text boxes
            var txtWorkBaseMap = this.mainPanel.Controls.Find("txtWorkBaseMap", true)[0] as TextBox;
            
            if (string.IsNullOrEmpty(txtWorkBaseMap.Text))
            {
                MessageBox.Show("����ѡ������ͼ�����ļ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }
            
            // Get processing options
            var chkLevelIndicators = this.mainPanel.Controls.Find("chkLevelIndicators", true)[0] as CheckBox;
            var chkBasePrice = this.mainPanel.Controls.Find("chkBasePrice", true)[0] as CheckBox;
            var chkModifiers = this.mainPanel.Controls.Find("chkModifiers", true)[0] as CheckBox;
            var chkYieldRate = this.mainPanel.Controls.Find("chkYieldRate", true)[0] as CheckBox;
            var chkExportExcel = this.mainPanel.Controls.Find("chkExportExcel", true)[0] as CheckBox;
            
            bool extractLevelIndicators = chkLevelIndicators.Checked;
            bool extractBasePrice = chkBasePrice.Checked;
            bool extractModifiers = chkModifiers.Checked;
            bool extractYieldRate = chkYieldRate.Checked;
            bool exportToExcel = chkExportExcel.Checked;
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessExtractPriceParameters(
                txtWorkBaseMap.Text,
                extractLevelIndicators,
                extractBasePrice,
                extractModifiers,
                extractYieldRate,
                exportToExcel));
        }
        
        private void ProcessExtractPriceParameters(
            string workBaseMapPath,
            bool extractLevelIndicators,
            bool extractBasePrice,
            bool extractModifiers,
            bool extractYieldRate,
            bool exportToExcel)
        {
            try
            {
                UpdateStatus("����׼������...");
                Log("��ʼ��ȡ�ֵػ�׼�۸��������");
                Log($"ʹ�ù�����ͼ����: {workBaseMapPath}");
                
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
                        case 15:
                            if (extractLevelIndicators)
                            {
                                UpdateStatus("������ȡ�ֵض���ָ�꼰Ȩ����Ϣ...");
                                Log("��ȡ�ֵض���ָ�꼰Ȩ����Ϣ...");
                            }
                            break;
                        case 35:
                            if (extractBasePrice)
                            {
                                UpdateStatus("������ȡ��׼�۸���Ϣ...");
                                Log("��ȡ��׼�۸���Ϣ...");
                            }
                            break;
                        case 55:
                            if (extractModifiers)
                            {
                                UpdateStatus("������ȡ��׼�۸��������Ӽ�Ȩ����Ϣ...");
                                Log("��ȡ��׼�۸��������Ӽ�Ȩ����Ϣ...");
                            }
                            break;
                        case 75:
                            if (extractYieldRate)
                            {
                                UpdateStatus("������ȡ���滹ԭ��...");
                                Log("��ȡ���滹ԭ��...");
                            }
                            break;
                        case 90:
                            if (exportToExcel)
                            {
                                UpdateStatus("���ڵ���������Excel...");
                                Log("����������Excel...");
                            }
                            break;
                    }
                    
                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }
                
                Log("�ֵػ�׼�۸������ȡ���");
                if (extractLevelIndicators)
                    Log("- ��ȡ�ֵض���ָ��: �¶ȡ�������������ͨ�����ȵ�8��ָ�꼰Ȩ��");
                if (extractBasePrice)
                    Log("- ��ȡ��׼�۸���Ϣ: ��4���������۸�Χ 2.45-8.72��Ԫ/����");
                if (extractModifiers)
                    Log("- ��ȡ��������: ��λ�����֡������6�����Ӽ�Ȩ��");
                if (extractYieldRate)
                    Log("- ��ȡ���滹ԭ��: ƽ�����滹ԭ�� 3.86%");
                if (exportToExcel)
                    Log("- �ѵ���������Excel�ļ�");
                
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