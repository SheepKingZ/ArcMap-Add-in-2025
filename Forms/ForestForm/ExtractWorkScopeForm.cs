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
            InitializeComponent(); // Changed from InitializeComponents
            
            // Wire up the browse button events
            this.btnBrowseInventory.Click += (sender, e) => BrowseForData(this.txtInventoryData, "ѡ���ֲ�ʪ���ղ�����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
            this.btnBrowseBoundary.Click += (sender, e) => BrowseForData(this.txtUrbanBoundary, "ѡ����򿪷��߽�����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
            this.btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
        }
        
        // Removed InitializeComponents() method as it's now in the Designer.cs file

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
                this.txtInventoryData.Text = @"C:\GIS_Data\�ֲ�ʪ���ղ�����.shp"; // New way
                this.txtUrbanBoundary.Text = @"C:\GIS_Data\���򿪷��߽�.shp"; // New way
                
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
            if (string.IsNullOrEmpty(this.txtInventoryData.Text) || string.IsNullOrEmpty(this.txtUrbanBoundary.Text)) // New way
            {
                MessageBox.Show("����ѡ�����������ļ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessExtractWorkScope(this.txtInventoryData.Text, this.txtUrbanBoundary.Text)); // New way
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