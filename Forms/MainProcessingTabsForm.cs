using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
// Add reference to ForestResourcePlugin namespace to access the Basic class
using ForestResourcePlugin;

namespace TestArcMapAddin2.Forms
{
    public partial class MainProcessingTabsForm : Form
    {
        // ׷�ٲ������״̬
        private Dictionary<string, bool> forestTasksCompleted = new Dictionary<string, bool>();
        private Dictionary<string, bool> grasslandTasksCompleted = new Dictionary<string, bool>();
        private Dictionary<string, bool> wetlandTasksCompleted = new Dictionary<string, bool>();
        private Dictionary<string, bool> outputTasksCompleted = new Dictionary<string, bool>();

        public MainProcessingTabsForm()
        {
            InitializeComponent();
            InitializeFormState();
            SetupTaskTracking();
            UpdateWorkflowState();
        }

        private void SetupTaskTracking()
        {
            // ��ʼ��ɭ����Դ��鲽��
            forestTasksCompleted.Add("extractScope", false);
            forestTasksCompleted.Add("createBasemapLink", false);
            forestTasksCompleted.Add("supplementPrice", false);
            forestTasksCompleted.Add("calculateValue", false);
            forestTasksCompleted.Add("cleanQA", false);
            forestTasksCompleted.Add("buildDBTables", false);

            // ��ʼ���ݵ���Դ��鲽��
            grasslandTasksCompleted.Add("extractScope", false);
            grasslandTasksCompleted.Add("createBasemapLink", false);
            grasslandTasksCompleted.Add("supplementPrice", false);
            grasslandTasksCompleted.Add("calculateValue", false);
            grasslandTasksCompleted.Add("cleanQA", false);
            grasslandTasksCompleted.Add("buildDBTables", false);

            // ��ʼ��ʪ����Դ��鲽��
            wetlandTasksCompleted.Add("extractScopeBasemap", false);
            wetlandTasksCompleted.Add("cleanQA", false);
            wetlandTasksCompleted.Add("buildDBTables", false);

            // ��ʼ���ۺ��������
            outputTasksCompleted.Add("overallQualityCheck", false);
            outputTasksCompleted.Add("statisticalAggregation", false);
            outputTasksCompleted.Add("dataAnalysis", false);
            outputTasksCompleted.Add("exportDatasetDB", false);
            outputTasksCompleted.Add("exportSummaryTables", false);
            outputTasksCompleted.Add("generateReport", false);
            outputTasksCompleted.Add("generateThematicMaps", false);
        }

        private void InitializeFormState()
        {
            UpdateProgress("�ȴ���ʼ����");
            lblForestProcessingStatus.Text = "�ȴ�ɭ����Դ��鴦��";
            lblGrasslandProcessingStatus.Text = "�ȴ��ݵ���Դ��鴦��";
            lblWetlandProcessingStatus.Text = "�ȴ�ʪ����Դ��鴦��";
            lblFinalOutputStatus.Text = "�ȴ����ճɹ�����";

            // ���ó�ʼ��ɫ
            lblForestProcessingStatus.ForeColor = Color.Black;
            lblGrasslandProcessingStatus.ForeColor = Color.Black;
            lblWetlandProcessingStatus.ForeColor = Color.Black;
            lblFinalOutputStatus.ForeColor = Color.Black;
            lblProgress.ForeColor = Color.Black;

            UpdateButtonStates();

            // ���½�����
            //forestProgressBar.Value = 0;
            grasslandProgressBar.Value = 0;
            wetlandProgressBar.Value = 0;
            outputProgressBar.Value = 0;
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
            Application.DoEvents(); // ����ʹ�ã����ܵ�����������
        }

        private void UpdateButtonStates()
        {
            // Remove basic data preparation requirement - all buttons can now be used independently
            // �Ƴ���������׼��Ҫ�� - ���а�ť���ڶ����Զ���ʹ��

            // ɭ��ѡ���ť - �Ƴ����������������������ʹ��
            btnForestExtractScope.Enabled = true;
            btnForestCreateBasemapLinkPrice.Enabled = true; // �Ƴ� forestTasksCompleted["extractScope"] ����
            btnForestSupplementPrice.Enabled = true; // �Ƴ� forestTasksCompleted["createBasemapLink"] ����
            
            // ����֮ǰ��ע�͵�ɭ�ֹ�������ť
            //if (btnForestCalculateValue != null)
            //    btnForestCalculateValue.Enabled = true;
            //if (btnForestCleanQA != null)
            //    btnForestCleanQA.Enabled = true;
            //if (btnForestBuildDBTables != null)
            //    btnForestBuildDBTables.Enabled = true;

            // �ݵ�ѡ���ť - �Ƴ����������������������ʹ��
            btnGrasslandExtractScope.Enabled = true;
            btnGrasslandCreateBasemapLinkPrice.Enabled = true; // �Ƴ�����
            btnGrasslandSupplementPrice.Enabled = true; // ��������
            btnGrasslandCalculateValue.Enabled = true; // �Ƴ�����
            btnGrasslandCleanQA.Enabled = true; // �Ƴ�����
            btnGrasslandBuildDBTables.Enabled = true; // �Ƴ�����

            // ʪ��ѡ���ť - �Ƴ����������������������ʹ��
            btnWetlandExtractScopeBasemap.Enabled = true;
            btnWetlandCleanQA.Enabled = true; // �Ƴ�����
            btnWetlandBuildDBTables.Enabled = true; // ��������

            // �ɹ����ѡ���ť - �Ƴ�����������������������ʹ��
            btnOverallQualityCheck.Enabled = true;
            btnStatisticalAggregation.Enabled = true; // �Ƴ�����
            btnDataAnalysis.Enabled = true; // �Ƴ�����
            btnExportDatasetDB.Enabled = true; // �Ƴ�����
            btnExportSummaryTables.Enabled = true; // �Ƴ�����
            btnGenerateReport.Enabled = true; // �Ƴ�����
            btnGenerateThematicMaps.Enabled = true; // �Ƴ�����
        }

        private void UpdateWorkflowState()
        {
            // ����ɭ�ֹ���������
            int forestSteps = forestTasksCompleted.Count;
            int forestCompleted = forestTasksCompleted.Count(x => x.Value);
            //forestProgressBar.Value = forestSteps > 0 ? (forestCompleted * 100) / forestSteps : 0;
            //forestStepLabel.Text = $"����� {forestCompleted}/{forestSteps} ����";

            // ���²ݵع���������
            int grasslandSteps = grasslandTasksCompleted.Count;
            int grasslandCompleted = grasslandTasksCompleted.Count(x => x.Value);
            grasslandProgressBar.Value = grasslandSteps > 0 ? (grasslandCompleted * 100) / grasslandSteps : 0;
            grasslandStepLabel.Text = $"����� {grasslandCompleted}/{grasslandSteps} ����";

            // ����ʪ�ع���������
            int wetlandSteps = wetlandTasksCompleted.Count;
            int wetlandCompleted = wetlandTasksCompleted.Count(x => x.Value);
            wetlandProgressBar.Value = wetlandSteps > 0 ? (wetlandCompleted * 100) / wetlandSteps : 0;
            wetlandStepLabel.Text = $"����� {wetlandCompleted}/{wetlandSteps} ����";

            // �����������������
            int outputSteps = outputTasksCompleted.Count;
            int outputCompleted = outputTasksCompleted.Count(x => x.Value);
            outputProgressBar.Value = outputSteps > 0 ? (outputCompleted * 100) / outputSteps : 0;
            outputStepLabel.Text = $"����� {outputCompleted}/{outputSteps} ����";

            // �����������
            int totalSteps = forestSteps + grasslandSteps + wetlandSteps + outputSteps;
            int totalCompleted = forestCompleted + grasslandCompleted + wetlandCompleted + outputCompleted;
            int overallPercentage = totalSteps > 0 ? (totalCompleted * 100) / totalSteps : 0;

            // ����ɭ����Դͼ��
            UpdateForestResourceChart();
        }

        private void UpdateForestResourceChart()
        {
            // ģ������ - ��ʵ��Ӧ����Ӧ�滻Ϊ��ʵ����
            forestResourceChart.Series.Clear();
            forestResourceChart.Titles.Clear();

            forestResourceChart.Titles.Add("ɭ����Դ������");
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("ɭ����Դ");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            if (forestTasksCompleted["extractScope"])
                series.Points.AddXY("��ȡ��Χ", 20);
            if (forestTasksCompleted["createBasemapLink"])
                series.Points.AddXY("��ͼ��۸����", 15);
            if (forestTasksCompleted["supplementPrice"])
                series.Points.AddXY("����۸�", 15);
            if (forestTasksCompleted["calculateValue"])
                series.Points.AddXY("��ֵ����", 20);
            if (forestTasksCompleted["cleanQA"])
                series.Points.AddXY("������ϴ���ʼ�", 15);
            if (forestTasksCompleted["buildDBTables"])
                series.Points.AddXY("�������", 15);

            if (series.Points.Count == 0)
                series.Points.AddXY("δ��ʼ", 100);

            forestResourceChart.Series.Add(series);
        }

        #region �¼��������

        // ɭ����Դ�������
        private void BtnForestExtractScope_Click(object sender, EventArgs e)
        {
            try
            {
                // Create and show the Basic form as a dialog
                Basic basicForm = new Basic();
                basicForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�򿪻������ݴ���ʱ����: {ex.Message}", "����",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnForestCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            try
            {
                // ��ɭ�ֵ�ͼ��۸����������
                var forestBasemapPriceForm = new ForestBasemapPriceAssociationForm();
                var result = forestBasemapPriceForm.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    // ������ɺ����״̬
                    forestTasksCompleted["createBasemapLink"] = true;
                    lblForestProcessingStatus.Text = "ɭ�ֵ�ͼ��۸�������";
                    lblForestProcessingStatus.ForeColor = Color.DarkGreen;
                    UpdateProgress("ɭ�ֵ�ͼ��۸�������");

                    // ���¹�����״̬�Ͱ�ť״̬
                    UpdateWorkflowState();
                    UpdateButtonStates();

                    // ������ϸ����ı�
                    if (forestResultsTextBox != null)
                    {
                        forestResultsTextBox.AppendText("=== ɭ�ֵ�ͼ��۸���� ===\r\n");
                        forestResultsTextBox.AppendText("- ������Χ���ֵطֵ����ݹ������\r\n");
                        forestResultsTextBox.AppendText("- ������Χ���ֵض������ݹ������\r\n");
                        forestResultsTextBox.AppendText("- ��׼�ؼ���ͼ�߿ռ�ҽ����\r\n");
                        forestResultsTextBox.AppendText("- ���ɺ���۸����Ա����\r\n\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ɭ�ֵ�ͼ��۸��������ʱ����: {ex.Message}", "����",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // ���½�����ʾΪʧ��״̬
                UpdateProgress("ɭ�ֵ�ͼ��۸��������ʧ��");
                lblForestProcessingStatus.Text = "ɭ�ֵ�ͼ��۸��������ʧ��";
                lblForestProcessingStatus.ForeColor = Color.Red;
            }
        }

        private void BtnForestSupplementPrice_Click(object sender, EventArgs e)
        {
            try
            {
                // ��ɭ�ּ۸����ݲ��䴦����
                var forestPriceSupplementForm = new ForestPriceSupplementForm();
                var result = forestPriceSupplementForm.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    // ������ɺ����״̬
                    forestTasksCompleted["supplementPrice"] = true;
                    lblForestProcessingStatus.Text = "ɭ�ֻ�׼�۸񲹳����";
                    lblForestProcessingStatus.ForeColor = Color.DarkGreen;
                    UpdateProgress("ɭ�ֻ�׼�۸񲹳����");

                    // ���¹�����״̬�Ͱ�ť״̬
                    UpdateWorkflowState();
                    UpdateButtonStates();

                    // ������ϸ����ı�
                    if (forestResultsTextBox != null)
                    {
                        forestResultsTextBox.AppendText("=== ɭ�ֻ�׼�۸񲹳� ===\r\n");
                        forestResultsTextBox.AppendText("- ��ȡ����۸�shp�������\r\n");
                        forestResultsTextBox.AppendText("- �۸����ݹ��Ʋ������\r\n");
                        forestResultsTextBox.AppendText("- ����ȱʧͼ�߼۸���Ϣ\r\n");
                        forestResultsTextBox.AppendText("- ����������������\r\n\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��ɭ�ּ۸����ݲ��䴰��ʱ����: {ex.Message}", "����",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // ���½�����ʾΪʧ��״̬
                UpdateProgress("ɭ�ֻ�׼�۸񲹳䴦��ʧ��");
                lblForestProcessingStatus.Text = "ɭ�ֻ�׼�۸񲹳䴦��ʧ��";
                lblForestProcessingStatus.ForeColor = Color.Red;
            }
        }

        private void BtnForestCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڼ���ɭ���ʲ���ֵ...");

            // ��ʵ�֣�ʵ���߼�
            forestTasksCompleted["calculateValue"] = true;
            lblForestProcessingStatus.Text = "ɭ���ʲ���ֵ�������";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ɭ���ʲ���ֵ�������");

            // ���¹�����״̬�Ͱ�ť״̬
            UpdateWorkflowState();
            UpdateButtonStates();

            // ������ϸ����ı�
            forestResultsTextBox.AppendText("=== ɭ���ʲ���ֵ���� ===\r\n");
            forestResultsTextBox.AppendText("- Ӧ�û�׼�۸��������Ӻ�Ȩ��\r\n");
            forestResultsTextBox.AppendText("- �����ڵؼ۸�\r\n");
            forestResultsTextBox.AppendText("- Ӧ��������������������\r\n");
            forestResultsTextBox.AppendText("- �ܼ�ֵ: 10652896Ԫ\r\n");
            forestResultsTextBox.AppendText("- ƽ������: 8.42��Ԫ/����\r\n\r\n");
        }

        private void BtnForestCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ���ɭ��������ϴ���ʼ�...");

            // ��ʵ�֣�ʵ���߼�
            forestTasksCompleted["cleanQA"] = true;
            lblForestProcessingStatus.Text = "ɭ��������ϴ���ʼ����";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ɭ��������ϴ���ʼ����");

            // ���¹�����״̬�Ͱ�ť״̬
            UpdateWorkflowState();
            UpdateButtonStates();

            // ������ϸ����ı�
            forestResultsTextBox.AppendText("=== ɭ��������ϴ���ʼ� ===\r\n");
            forestResultsTextBox.AppendText("- ��������ֶκ���ʱ����\r\n");
            forestResultsTextBox.AppendText("- �����ֶθ�ʽ�����ݾ���\r\n");
            forestResultsTextBox.AppendText("- ���������Ч����������\r\n");
            forestResultsTextBox.AppendText("- �޸��쳣����: 5��\r\n");
            forestResultsTextBox.AppendText("- �ʼ�ͨ����: 98.2%\r\n\r\n");
        }

        private void BtnForestBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڹ���ɭ�ֿ��...");

            // ��ʵ�֣�ʵ���߼�
            forestTasksCompleted["buildDBTables"] = true;
            lblForestProcessingStatus.Text = "ɭ�ֿ�������";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ɭ�ֿ�������");

            // ���¹�����״̬�Ͱ�ť״̬
            UpdateWorkflowState();
            UpdateButtonStates();

            // ������ϸ����ı�
            forestResultsTextBox.AppendText("=== ɭ�ֿ���� ===\r\n");
            forestResultsTextBox.AppendText("- ��ȡ���ɹ�ԭ��������ֶ�\r\n");
            forestResultsTextBox.AppendText("- �������ϻ㽻�淶�����ݼ�\r\n");
            forestResultsTextBox.AppendText("- ���ɻ��������ͳ�Ʊ�\r\n");
            forestResultsTextBox.AppendText("- �������ݱ�: 3��\r\n");
            forestResultsTextBox.AppendText("- ����ͳ�Ʊ�: 2��\r\n\r\n");
        }

        // �ݵ���Դ�������
        private void BtnGrasslandExtractScope_Click(object sender, EventArgs e)
        {
            UpdateProgress("������ȡ�ݵع�����Χ...");
            grasslandTasksCompleted["extractScope"] = true;
            lblGrasslandProcessingStatus.Text = "�ݵع�����Χ��ȡ���";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵع�����Χ��ȡ���");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڴ���ݵع�����ͼ��۸����...");
            grasslandTasksCompleted["createBasemapLink"] = true;
            lblGrasslandProcessingStatus.Text = "�ݵص�ͼ��۸�������";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵص�ͼ��۸�������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڲ���ݵػ�׼�۸�...");
            grasslandTasksCompleted["supplementPrice"] = true;
            lblGrasslandProcessingStatus.Text = "�ݵػ�׼�۸񲹳����";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵػ�׼�۸񲹳����");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڼ���ݵ��ʲ���ֵ...");
            grasslandTasksCompleted["calculateValue"] = true;
            lblGrasslandProcessingStatus.Text = "�ݵ��ʲ���ֵ�������";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵ��ʲ���ֵ�������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ��вݵ�������ϴ���ʼ�...");
            grasslandTasksCompleted["cleanQA"] = true;
            lblGrasslandProcessingStatus.Text = "�ݵ�������ϴ���ʼ����";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵ�������ϴ���ʼ����");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڹ����ݵؿ��...");
            grasslandTasksCompleted["buildDBTables"] = true;
            lblGrasslandProcessingStatus.Text = "�ݵؿ�������";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ݵؿ�������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        // ʪ����Դ�������
        private void BtnWetlandExtractScopeBasemap_Click(object sender, EventArgs e)
        {
            UpdateProgress("��������ʪ�ع�����Χ���ͼ...");
            wetlandTasksCompleted["extractScopeBasemap"] = true;
            lblWetlandProcessingStatus.Text = "ʪ�ع�����Χ���ͼ�������";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ʪ�ع�����Χ���ͼ�������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnWetlandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ���ʪ��������ϴ���ʼ�...");
            wetlandTasksCompleted["cleanQA"] = true;
            lblWetlandProcessingStatus.Text = "ʪ��������ϴ���ʼ����";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ʪ��������ϴ���ʼ����");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnWetlandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڹ���ʪ�ؿ��...");
            wetlandTasksCompleted["buildDBTables"] = true;
            lblWetlandProcessingStatus.Text = "ʪ�ؿ�������";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ʪ�ؿ�������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        // ��������������
        private void BtnOverallQualityCheck_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ����ۺ��������...");
            outputTasksCompleted["overallQualityCheck"] = true;
            lblFinalOutputStatus.Text = "�ۺ�����������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ۺ�����������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnStatisticalAggregation_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ�������ͳ�ƻ���...");
            outputTasksCompleted["statisticalAggregation"] = true;
            lblFinalOutputStatus.Text = "����ͳ�ƻ������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("����ͳ�ƻ������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnDataAnalysis_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڽ������ݷ������ھ�...");
            outputTasksCompleted["dataAnalysis"] = true;
            lblFinalOutputStatus.Text = "���ݷ������ھ����";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("���ݷ������ھ����");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnExportDatasetDB_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڵ���������ݼ�...");
            outputTasksCompleted["exportDatasetDB"] = true;
            lblFinalOutputStatus.Text = "������ݼ��������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("������ݼ��������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnExportSummaryTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("���ڵ������ܱ�...");
            outputTasksCompleted["exportSummaryTables"] = true;
            lblFinalOutputStatus.Text = "���ܱ������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("���ܱ������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            UpdateProgress("�������ɳɹ�����...");
            outputTasksCompleted["generateReport"] = true;
            lblFinalOutputStatus.Text = "�ɹ������������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("�ɹ������������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGenerateThematicMaps_Click(object sender, EventArgs e)
        {
            UpdateProgress("��������ר��ͼ...");
            outputTasksCompleted["generateThematicMaps"] = true;
            lblFinalOutputStatus.Text = "ר��ͼ�������";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("ר��ͼ�������");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            // ���°����ı����Ƴ���������׼��Ҫ��
            string helpText = @"�㶫ʡȫ��������Ȼ��Դ��ɭ�֡��ݵء�ʪ�أ��ʲ���鹤��ʹ��˵��

ע�⣺���й������ڶ����Զ���ʹ�ã���������ɻ�������׼�����衣

ɭ����Դ�ʲ����
1. ��ȡɭ�ֹ�����Χ�������ղ����ݺͿ����߽磬ɸѡ�����ֵؼ��߽��ڼ����ֵء�
2. ɭ�ֵ�ͼ��۸��������������Χ���ֵطֵ����ݹ��������ҽӻ�׼�ؼۡ�
3. ����ɭ�ֻ�׼�۸���Թҽ�ʧ�ܵ�ͼ�ߣ���������ȷ����׼�۸���ȱ©��
4. ����ɭ���ʲ���ֵ��Ӧ���������Ӽ����ڵؼ۸񣬽������պ����������õ�����۸�
5. ɭ��������ϴ���ʼ죺����������ϴ��������飬ȷ�����ϼ����淶��
6. ɭ�ֿ�������������ϻ㽻�淶�����ݼ������������ͳ�Ʊ�

�ݵ���Դ�ʲ����
����������ɭ����Դ�ʲ���飬������ͬ���������衣

ʪ����Դ�ʲ����
1. ����ʪ�ع�����Χ���ͼ����ȡʪ�ط�Χ������������ͼ��
2. ʪ��������ϴ���ʼ죺����������ϴ��������顣
3. ʪ�ؿ����������ʪ��������ݱ�
ע��ʪ����Դ�ʲ����������ʵ������飬�����м�ֵ�����㡣

�ۺ��ʼ졢ͳ����ɹ����
1. �ۺ�������飺��ɭ�֡��ݵء�ʪ��������ݽ���ȫ���ʼ졣
2. ����ͳ�ƻ��ܣ������������ͳ�ƻ��ܡ�
3. ���ݷ������ھ򣺽����������ݷ������ھ����ݹ��ɺ���Ϣ��
4. ����������ݼ����������ϻ㽻�淶��������ݼ���
5. �������ܱ�����������ܱ�
6. ���ɳɹ����棺���ɹ����ܽᱨ��������Լ챨�档
7. ����ר��ͼ�����ɸ���ר��ͼ��

ʹ��˵����
- ���а�ť���ڶ����Զ���ʹ�ã����谴�ض�˳��ִ�С�
- ���鰴�߼�˳��ִ���Ի����ѽ����������ǿ��Ҫ��
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
            // ȷ���� InitializeComponent ��ȫ���ǰ���ô˷���ʱ��btnHelp �� btnClose ��Ϊ null
            if (btnHelp != null && btnClose != null && bottomPanel != null)
            {
                // ������ť����� bottomPanel �ұ�Ե��λ��
                btnClose.Location = new Point(bottomPanel.Width - btnClose.Width - 10, (bottomPanel.Height - btnClose.Height) / 2);
                btnHelp.Location = new Point(btnClose.Location.X - btnHelp.Width - 5, (bottomPanel.Height - btnHelp.Height) / 2);
            }
        }
        #endregion

        private void ShowForestWorkflowDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Keep existing toggle functionality
            forestDetailPanel.Visible = !forestDetailPanel.Visible;
            forestWorkflowExplanation.Visible = forestDetailPanel.Visible;
            forestResultsTextBox.Visible = forestDetailPanel.Visible;
            forestResourceChart.Visible = forestDetailPanel.Visible;
            forestWorkflowImage.Visible = forestDetailPanel.Visible;

            showForestWorkflowDetails.Text = forestDetailPanel.Visible ?
                "������ϸ����˵�� ��" : "��ʾ��ϸ����˵�� ��";
            
            // Add code to display the Basic.cs window
            //try
            //{
            //    // Create and show the Basic form as a dialog
            //    Basic basicForm = new Basic();
            //    basicForm.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"�򿪻������ݴ���ʱ����: {ex.Message}", "����", 
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void forestStepLabel_Click(object sender, EventArgs e)
        {

        }
    }
}