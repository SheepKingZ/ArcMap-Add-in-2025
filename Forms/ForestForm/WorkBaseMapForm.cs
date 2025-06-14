using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using Point = System.Drawing.Point;

namespace TestArcMapAddin2.Forms.ForestForm
{
    /// <summary>
    /// Form for creating forest work base maps and linking with forest classification and pricing data
    /// </summary>
    public partial class WorkBaseMapForm : TestArcMapAddin2.Forms.ForestForm.ForestProcessingFormBase
    {
        private bool processingCancelled = false;
        
        public WorkBaseMapForm()
        {
            InitializeComponent(); // Calls the designer's InitializeComponent
            // Ensure cmbJoinMethod has a default selection if not set in Designer
            if (this.cmbJoinMethod.SelectedIndex == -1 && this.cmbJoinMethod.Items.Count > 0)
            {
                this.cmbJoinMethod.SelectedIndex = 0;
            }
        }
        
        // Removed InitializeComponents() method from here. It's now in the .Designer.cs file.

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

        // Event handlers for browse buttons, to be connected in Designer.cs
        private void BrowseWorkScope_Click(object sender, EventArgs e)
        {
            BrowseForData(this.txtWorkScope, "ѡ������Χ����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
        }

        private void BrowseGrading_Click(object sender, EventArgs e)
        {
            BrowseForData(this.txtForestGrading, "ѡ���ֵطֵ�����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
        }

        private void BrowsePrice_Click(object sender, EventArgs e)
        {
            BrowseForData(this.txtPriceData, "ѡ���׼�ؼ�����", "ͼ���ļ� (*.shp)|*.shp|�����ļ� (*.*)|*.*");
        }
        
        private void BtnLoadCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("���ڴӵ�ǰ��ͼ��������...");
                // In a real implementation, this would use ArcObjects to access the current map
                // and load the relevant layers
                
                // Access controls directly as fields
                this.txtWorkScope.Text = @"C:\GIS_Data\ɭ�ֹ�����Χ.shp";
                this.txtForestGrading.Text = @"C:\GIS_Data\�ֵطֵ�����.shp";
                this.txtPriceData.Text = @"C:\GIS_Data\�ֵػ�׼�ؼ�����.shp";
                
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
            
            // Access controls directly as fields
            if (string.IsNullOrEmpty(this.txtWorkScope.Text) || 
                string.IsNullOrEmpty(this.txtForestGrading.Text) || 
                string.IsNullOrEmpty(this.txtPriceData.Text))
            {
                MessageBox.Show("����ѡ�����������ļ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false); // Ensure this is called to re-enable start button etc.
                return;
            }
            
            bool useSpatialJoin = this.chkSpatialJoin.Checked;
            string joinMethod = this.cmbJoinMethod.SelectedItem?.ToString() ?? "�ཻ";
            bool overwriteExisting = this.chkOverwriteExisting.Checked;
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessCreateWorkBaseMap(
                this.txtWorkScope.Text, 
                this.txtForestGrading.Text, 
                this.txtPriceData.Text,
                useSpatialJoin,
                joinMethod,
                overwriteExisting));
        }
        
        private void ProcessCreateWorkBaseMap(
            string workScopePath, 
            string forestGradingPath, 
            string priceDataPath,
            bool useSpatialJoin,
            string joinMethod,
            bool overwriteExisting)
        {
            try
            {
                UpdateStatus("����׼������...");
                Log("��ʼ����������ͼ����");
                Log($"ʹ�ù�����Χ����: {workScopePath}");
                Log($"ʹ���ֵطֵ�����: {forestGradingPath}");
                Log($"ʹ�û�׼�ؼ�����: {priceDataPath}");
                Log($"�ռ����ӷ�ʽ: {(useSpatialJoin ? joinMethod : "��������")}");
                Log($"������������: {(overwriteExisting ? "��" : "��")}");
                
                // Simulate the processing steps
                for (int i = 0; i <= 100; i += 5)
                {
                    if (processingCancelled)
                    {
                        Log("�����û�ȡ����");
                        this.Invoke(new Action(() => OnProcessingComplete(false)));
                        return;
                    }
                    
                    switch (i)
                    {
                        case 0:
                            UpdateStatus("���ڼ��ع�����Χ����...");
                            Log("���ع�����Χ������...");
                            break;
                        case 10:
                            UpdateStatus("���ڼ����ֵطֵ�����...");
                            Log("�����ֵطֵ�������...");
                            break;
                        case 20:
                            UpdateStatus("���ڼ��ػ�׼�ؼ�����...");
                            Log("���ػ�׼�ؼ�������...");
                            break;
                        case 30:
                            UpdateStatus("���ڹ���������Χ���ֵطֵ�����...");
                            Log("����������Χ���ֵطֵ�����...");
                            break;
                        case 50:
                            UpdateStatus("���ڹ���������Χ���ֵض�������...");
                            Log("����������Χ���ֵض�������...");
                            break;
                        case 70:
                            UpdateStatus("���ڽ��л�׼�ؼۿռ�ҽ�...");
                            Log("���л�׼�ؼۿռ�ҽ�...");
                            break;
                        case 85:
                            UpdateStatus("���ڼ������һ����...");
                            Log("�������һ����...");
                            break;
                        case 95:
                            UpdateStatus("���ڵ���������ͼ...");
                            Log("����������ͼ...");
                            break;
                    }
                    
                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }
                
                Log("������ͼ�������");
                Log("�����ɹ�: 256ͼ��");
                Log("����ʧ��: 27ͼ�� (������һ���貹��)");
                
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
            // The base.CancelButton_Click already shows a confirmation and calls OnProcessingComplete(false) if "Yes" is clicked.
            // We just need to set the flag.
            var result = MessageBox.Show("ȷ��Ҫȡ����ǰ������", "ȷ��", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                processingCancelled = true;
                // The base class OnProcessingComplete(false) will be called by base.CancelButton_Click 
                // if the user confirms. If we call it here again, it might lead to double logging or status updates.
                // However, the current base implementation calls OnProcessingComplete(false) directly.
                // To ensure our processingCancelled flag is respected by the loop, we set it here.
                // The base method will handle UI updates.
                base.OnProcessingComplete(false); // Explicitly call to ensure UI reset if loop hasn't checked flag yet.
            }
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {

        }
    }
}