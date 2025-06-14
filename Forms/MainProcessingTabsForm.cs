using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestArcMapAddin2.Forms
{
    public partial class MainProcessingTabsForm : Form
    {
        public MainProcessingTabsForm()
        {
            InitializeComponent();
            InitializeFormState();
        }

        private void InitializeFormState()
        {
            UpdateProgress("�ȴ���ʼ����");
            lblForestProcessingStatus.Text = "�ȴ�ɭ����Դ��鴦��";
            lblGrasslandProcessingStatus.Text = "�ȴ��ݵ���Դ��鴦��";
            lblWetlandProcessingStatus.Text = "�ȴ�ʪ����Դ��鴦��";
            lblFinalOutputStatus.Text = "�ȴ����ճɹ�����";
            
            // Set initial colors
            lblForestProcessingStatus.ForeColor = System.Drawing.Color.Black;
            lblGrasslandProcessingStatus.ForeColor = System.Drawing.Color.Black;
            lblWetlandProcessingStatus.ForeColor = System.Drawing.Color.Black;
            lblFinalOutputStatus.ForeColor = System.Drawing.Color.Black;
            lblProgress.ForeColor = System.Drawing.Color.Black;

            UpdateButtonStates();
        }

        private void UpdateProgress(string message)
        {
            if (lblProgress.InvokeRequired)
            {
                lblProgress.Invoke(new Action(() => lblProgress.Text = $"���ȣ�{message}"));
            }
            else
            {
                lblProgress.Text = $"���ȣ�{message}";
            }
            Application.DoEvents(); // Use with caution, can lead to reentrancy issues.
        }

        private void UpdateButtonStates()
        {
            bool basicDataReady = SharedWorkflowState.IsBasicDataPrepared;

            // Enable/disable tabs or controls based on basicDataReady
            // For simplicity, enabling all process buttons if basic data is ready.
            // In a real scenario, this would be more granular, depending on previous step completions.

            // TabPage 2: Forest
            btnForestExtractScope.Enabled = basicDataReady;
            btnForestCreateBasemapLinkPrice.Enabled = basicDataReady;
            btnForestSupplementPrice.Enabled = basicDataReady;
            btnForestCalculateValue.Enabled = basicDataReady;
            btnForestCleanQA.Enabled = basicDataReady;
            btnForestBuildDBTables.Enabled = basicDataReady;

            // TabPage 3: Grassland
            btnGrasslandExtractScope.Enabled = basicDataReady;
            btnGrasslandCreateBasemapLinkPrice.Enabled = basicDataReady;
            btnGrasslandSupplementPrice.Enabled = basicDataReady;
            btnGrasslandCalculateValue.Enabled = basicDataReady;
            btnGrasslandCleanQA.Enabled = basicDataReady;
            btnGrasslandBuildDBTables.Enabled = basicDataReady;

            // TabPage 4: Wetland
            btnWetlandExtractScopeBasemap.Enabled = basicDataReady;
            btnWetlandCleanQA.Enabled = basicDataReady;
            btnWetlandBuildDBTables.Enabled = basicDataReady;

            // TabPage 5: Final Output
            btnOverallQualityCheck.Enabled = basicDataReady; // Should depend on prior steps completion
            btnStatisticalAggregation.Enabled = basicDataReady;
            btnDataAnalysis.Enabled = basicDataReady;
            btnExportDatasetDB.Enabled = basicDataReady;
            btnExportSummaryTables.Enabled = basicDataReady;
            btnGenerateReport.Enabled = basicDataReady;
            btnGenerateThematicMaps.Enabled = basicDataReady;
        }


        #region Event Handlers (Copied and adapted from MainProcessForm)

        // Forest resource handlers
        private void BtnForestExtractScope_Click(object sender, EventArgs e)
        {
            UpdateProgress("������ȡɭ�ֹ�����Χ...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "ɭ�ֹ�����Χ��ȡ���";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ɭ�ֹ�����Χ��ȡ���");
        }

        private void BtnForestCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڴ���ɭ�ֹ�����ͼ��۸����...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "ɭ�ֵ�ͼ��۸�������";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ɭ�ֵ�ͼ��۸�������");
        }

        private void BtnForestSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڲ���ɭ�ֻ�׼�۸�...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "ɭ�ֻ�׼�۸񲹳����";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen; // Keep DarkGreen for success
            UpdateProgress("ɭ�ֻ�׼�۸񲹳����");
        }

        private void BtnForestCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڼ���ɭ���ʲ���ֵ...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "ɭ���ʲ���ֵ�������";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ɭ���ʲ���ֵ�������");
        }

        private void BtnForestCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ���ɭ��������ϴ���ʼ�...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "ɭ��������ϴ���ʼ����";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ɭ��������ϴ���ʼ����");
        }

        private void BtnForestBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڹ���ɭ�ֿ��...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "ɭ�ֿ�������";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ɭ�ֿ�������");
        }

        // Grassland resource handlers
        private void BtnGrasslandExtractScope_Click(object sender, EventArgs e)
        {
            UpdateProgress("������ȡ�ݵع�����Χ...");
            lblGrasslandProcessingStatus.Text = "�ݵع�����Χ��ȡ���";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵع�����Χ��ȡ���");
        }

        private void BtnGrasslandCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڴ���ݵع�����ͼ��۸����...");
            lblGrasslandProcessingStatus.Text = "�ݵص�ͼ��۸�������";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵص�ͼ��۸�������");
        }

        private void BtnGrasslandSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڲ���ݵػ�׼�۸�...");
            lblGrasslandProcessingStatus.Text = "�ݵػ�׼�۸񲹳����";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵػ�׼�۸񲹳����");
        }

        private void BtnGrasslandCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڼ���ݵ��ʲ���ֵ...");
            lblGrasslandProcessingStatus.Text = "�ݵ��ʲ���ֵ�������";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵ��ʲ���ֵ�������");
        }

        private void BtnGrasslandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ��вݵ�������ϴ���ʼ�...");
            lblGrasslandProcessingStatus.Text = "�ݵ�������ϴ���ʼ����";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵ�������ϴ���ʼ����");
        }

        private void BtnGrasslandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڹ����ݵؿ��...");
            lblGrasslandProcessingStatus.Text = "�ݵؿ�������";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵؿ�������");
        }

        // Wetland resource handlers
        private void BtnWetlandExtractScopeBasemap_Click(object sender, EventArgs e)
        {
            UpdateProgress("��������ʪ�ع�����Χ���ͼ...");
            lblWetlandProcessingStatus.Text = "ʪ�ع�����Χ���ͼ�������";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ʪ�ع�����Χ���ͼ�������");
        }

        private void BtnWetlandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ���ʪ��������ϴ���ʼ�...");
            lblWetlandProcessingStatus.Text = "ʪ��������ϴ���ʼ����";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ʪ��������ϴ���ʼ����");
        }

        private void BtnWetlandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڹ���ʪ�ؿ��...");
            lblWetlandProcessingStatus.Text = "ʪ�ؿ�������";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ʪ�ؿ�������");
        }

        // Final output handlers
        private void BtnOverallQualityCheck_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ����ۺ��������...");
            lblFinalOutputStatus.Text = "�ۺ�����������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ۺ�����������");
        }

        private void BtnStatisticalAggregation_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ�������ͳ�ƻ���...");
            lblFinalOutputStatus.Text = "����ͳ�ƻ������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("����ͳ�ƻ������");
        }

        private void BtnDataAnalysis_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ������ݷ������ھ�...");
            lblFinalOutputStatus.Text = "���ݷ������ھ����";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("���ݷ������ھ����");
        }

        private void BtnExportDatasetDB_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڵ���������ݼ�...");
            lblFinalOutputStatus.Text = "������ݼ��������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("������ݼ��������");
        }

        private void BtnExportSummaryTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڵ������ܱ�...");
            lblFinalOutputStatus.Text = "���ܱ������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("���ܱ������");
        }
        
        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            UpdateProgress("�������ɳɹ�����...");
            lblFinalOutputStatus.Text = "�ɹ������������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ɹ������������");
        }

        private void BtnGenerateThematicMaps_Click(object sender, EventArgs e)
        {
            UpdateProgress("��������ר��ͼ...");
            lblFinalOutputStatus.Text = "ר��ͼ�������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ר��ͼ�������");
        }


        private void BtnHelp_Click(object sender, EventArgs e)
        {
            // Copied from original MainProcessForm
            string helpText = @"�㶫ʡȫ��������Ȼ��Դ��ɭ�֡��ݵء�ʪ�أ��ʲ���鹤��ʹ��˵��

��������׼�� (ͨ�� '��������׼��' �������)
1. ѡ�����ռ䣺ѡ�����ڴ��������ݵ�File Geodatabase (.gdb)��
2. ѡ��������ѡ����Ҫ��������һ������������
3. ����ǰ�����ݣ����ر�Ҫ�ı������ݡ�
4. �����ؼ��ձ�Ϊѡ�������������ݱ�ṹ��

--- ���²����ڱ����ڽ��� ---

ɭ����Դ�ʲ����
1. ��ȡɭ�ֹ�����Χ�������ղ����ݺͿ����߽磬ɸѡ�����ֵؼ��߽��ڼ����ֵء�
... (rest of the help text as in original, ensure it's complete) ...

ʪ����Դ�ʲ����
...

�ۺ��ʼ졢ͳ����ɹ����
...

ע�����
- �������'��������׼��'�����е����в��衣
- �뾡��������˳��ִ�С�
- ÿ��������ɺ������Ӧ��ʾ��
- ���ֲ������ܺ�ʱ�ϳ��������ĵȴ���";

            MessageBox.Show(helpText, "ʹ�ð���", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void BottomPanel_Resize(object sender, EventArgs e)
        {
            AdjustButtonPositions();
        }

        private void AdjustButtonPositions()
        {
            // Ensure btnHelp and btnClose are not null if this is called before InitializeComponent fully finishes (e.g. during load)
            if (btnHelp != null && btnClose != null && bottomPanel != null)
            {
                 // Adjust positions relative to the right edge of bottomPanel
                btnClose.Location = new Point(bottomPanel.Width - btnClose.Width - 10, (bottomPanel.Height - btnClose.Height) / 2);
                btnHelp.Location = new Point(btnClose.Location.X - btnHelp.Width - 5, (bottomPanel.Height - btnHelp.Height) / 2);
            }
        }
        #endregion
    }
}