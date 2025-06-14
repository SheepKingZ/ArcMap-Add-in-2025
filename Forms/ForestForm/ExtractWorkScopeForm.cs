using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using TestArcMapAddin2.Components;
using TestArcMapAddin2.Forms.ForestForm;
using Point = System.Drawing.Point;

namespace TestArcMapAddin2.Forms
{
    /// <summary>
    /// Form for extracting forest work scope based on forest-grassland-wetland inventory data
    /// </summary>
    public partial class ExtractWorkScopeForm : ForestProcessingFormBase
    {
        private bool processingCancelled = false;
        
        public ExtractWorkScopeForm()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "��ȡɭ�ֹ�����Χ";
            this.titleLabel.Text = "1. ��ȡɭ�ֹ�����Χ";
            
            this.descriptionTextBox.Text = 
                "���ֲ�ʪ���ղ�����Ϊ�������������򿪷��߽�ʸ�����ݣ�ɸѡ��ȡ����Ϊ�ֵأ�������Ȩ������Ϊ���е�ͼ�ߵؿ飬" +
                "�����Ϊ�ֵأ�����Ȩ������Ϊ���嵫��λ�ڳ��򿪷��߽��ڵ�ͼ�ߵؿ飬��Ϊ�أ�����ɭ����Դ�ʲ���鹤����Χ��";
            
            // Add custom controls specific to this form
            GroupBox dataSourceGroupBox = new GroupBox
            {
                Text = "������Դ",
                Location = new Point(15, 150),
                Size = new Size(600, 120)
            };
            
            Label lblInventoryData = new Label
            {
                Text = "�ֲ�ʪ���ղ�����:",
                Location = new Point(15, 25),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            TextBox txtInventoryData = new TextBox
            {
                Location = new Point(170, 25),
                Size = new Size(320, 20),
                ReadOnly = true
            };
            
            Button btnBrowseInventory = new Button
            {
                Text = "���...",
                Location = new Point(500, 24),
                Size = new Size(80, 23)
            };
            
            Label lblUrbanBoundary = new Label
            {
                Text = "���򿪷��߽�����:",
                Location = new Point(15, 55),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            TextBox txtUrbanBoundary = new TextBox
            {
                Location = new Point(170, 55),
                Size = new Size(320, 20),
                ReadOnly = true
            };
            
            Button btnBrowseBoundary = new Button
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
            dataSourceGroupBox.Controls.Add(lblInventoryData);
            dataSourceGroupBox.Controls.Add(txtInventoryData);
            dataSourceGroupBox.Controls.Add(btnBrowseInventory);
            dataSourceGroupBox.Controls.Add(lblUrbanBoundary);
            dataSourceGroupBox.Controls.Add(txtUrbanBoundary);
            dataSourceGroupBox.Controls.Add(btnBrowseBoundary);
            dataSourceGroupBox.Controls.Add(btnLoadCurrentMap);
            
            // Adjust the location of the existing log text box
            this.logTextBox.Location = new Point(15, 280);
            this.logTextBox.Size = new Size(600, 140);
            
            // Adjust other control positions
            this.statusLabel.Location = new Point(15, 430);
            this.progressBar.Location = new Point(15, 455);
            
            // Add the group box to the main panel
            this.mainPanel.Controls.Add(dataSourceGroupBox);
            
            // Wire up the browse button events
            btnBrowseInventory.Click += (sender, e) => BrowseForData(txtInventoryData, "ѡ���ֲ�ʪ���ղ�����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
            btnBrowseBoundary.Click += (sender, e) => BrowseForData(txtUrbanBoundary, "ѡ����򿪷��߽�����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
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
                // and load the relevant layers
                
                // For demo purposes, just fill in some sample paths
                var txtInventoryData = this.mainPanel.Controls.Find("txtInventoryData", true)[0] as TextBox;
                var txtUrbanBoundary = this.mainPanel.Controls.Find("txtUrbanBoundary", true)[0] as TextBox;
                
                txtInventoryData.Text = @"C:\GIS_Data\�ֲ�ʪ���ղ�����.shp";
                txtUrbanBoundary.Text = @"C:\GIS_Data\���򿪷��߽�.shp";
                
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
            var txtInventoryData = this.mainPanel.Controls.Find("txtInventoryData", true)[0] as TextBox;
            var txtUrbanBoundary = this.mainPanel.Controls.Find("txtUrbanBoundary", true)[0] as TextBox;
            
            if (string.IsNullOrEmpty(txtInventoryData.Text) || string.IsNullOrEmpty(txtUrbanBoundary.Text))
            {
                MessageBox.Show("����ѡ�����������ļ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessExtractWorkScope(txtInventoryData.Text, txtUrbanBoundary.Text));
        }
        
        private void ProcessExtractWorkScope(string inventoryDataPath, string boundaryDataPath)
        {
            try
            {
                UpdateStatus("����׼������...");
                Log("��ʼ��ȡ������Χ����");
                Log($"ʹ���ֲ�ʪ���ղ�����: {inventoryDataPath}");
                Log($"ʹ�ó��򿪷��߽�����: {boundaryDataPath}");
                
                // Simulate the processing steps
                for (int i = 0; i <= 100; i += 5)
                {
                    if (processingCancelled) return;
                    
                    switch (i)
                    {
                        case 0:
                            UpdateStatus("���ڼ����ֲ�ʪ���ղ�����...");
                            Log("�����ֲ�ʪ���ղ�������...");
                            break;
                        case 20:
                            UpdateStatus("���ڼ��س��򿪷��߽�����...");
                            Log("���س��򿪷��߽�������...");
                            break;
                        case 30:
                            UpdateStatus("������ȡ����Ϊ�ֵ�������Ȩ��Ϊ���е�ͼ��...");
                            Log("��ȡ����Ϊ�ֵ�������Ȩ��Ϊ���е�ͼ��...");
                            break;
                        case 50:
                            UpdateStatus("������ȡ����Ϊ�ֵ�������Ȩ��Ϊ������λ�ڳ��򿪷��߽��ڵ�ͼ��...");
                            Log("��ȡ����Ϊ�ֵ�������Ȩ��Ϊ������λ�ڳ��򿪷��߽��ڵ�ͼ��...");
                            break;
                        case 70:
                            UpdateStatus("���ںϲ�ɸѡ���...");
                            Log("�ϲ�ɸѡ���...");
                            break;
                        case 85:
                            UpdateStatus("����������ʱ����...");
                            Log("������ʱ����...");
                            break;
                        case 95:
                            UpdateStatus("���ڵ���������Χͼ��...");
                            Log("����������Χͼ��...");
                            break;
                    }
                    
                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }
                
                Log("������Χ��ȡ���");
                Log("����ȡ��283��ͼ�ߣ������1265.42����");
                Log("���У������ֵ�ͼ�� 245�������1086.23����");
                Log("�����ֵص��ڳ��򿪷��߽���ͼ�� 38�������179.19����");
                
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