using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // ����CountySelectionForm

namespace TestArcMapAddin2.Forms
{
    public partial class BasicDataPreparationForm : Form
    {
        private string prerequisiteData1Path = "";
        private string prerequisiteData2Path = "";

        public BasicDataPreparationForm()
        {
            InitializeComponent();
            InitializeFormState();
            LoadStateFromShared();
        }

        private void InitializeFormState()
        {
            // ���ݵ�ǰ����״̬��Ĭ��ֵ��ʼ����ǩ
            lblWorkspace.Text = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? "δѡ�����ռ�" : SharedWorkflowState.WorkspacePath;
            lblWorkspace.ForeColor = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? Color.Black : Color.DarkGreen;

            if (SharedWorkflowState.SelectedCounties != null && SharedWorkflowState.SelectedCounties.Count > 0)
            {
                lblCounties.Text = $"��ѡ�� {SharedWorkflowState.SelectedCounties.Count} ��������{string.Join(", ", SharedWorkflowState.SelectedCounties)}";
                lblCounties.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblCounties.Text = "δѡ������";
                lblCounties.ForeColor = Color.Black;
            }

            // ����ǰ������1״̬
            if (!string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData1Path))
            {
                lblPrerequisiteData1Status.Text = SharedWorkflowState.PrerequisiteData1Path;
                lblPrerequisiteData1Status.ForeColor = Color.DarkGreen;
                prerequisiteData1Path = SharedWorkflowState.PrerequisiteData1Path;
            }
            else
            {
                lblPrerequisiteData1Status.Text = "δ����ǰ������1";
                lblPrerequisiteData1Status.ForeColor = Color.Black;
            }

            // ����ǰ������2״̬
            if (!string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData2Path))
            {
                lblPrerequisiteData2Status.Text = SharedWorkflowState.PrerequisiteData2Path;
                lblPrerequisiteData2Status.ForeColor = Color.DarkGreen;
                prerequisiteData2Path = SharedWorkflowState.PrerequisiteData2Path;
            }
            else
            {
                lblPrerequisiteData2Status.Text = "δ����ǰ������2";
                lblPrerequisiteData2Status.ForeColor = Color.Black;
            }

            lblCountyEmptyTablesStatus.Text = SharedWorkflowState.CountyEmptyTablesCreated ? "�ؼ��ձ������" : "δ�����ؼ��ձ�";
            lblCountyEmptyTablesStatus.ForeColor = SharedWorkflowState.CountyEmptyTablesCreated ? Color.DarkGreen : Color.Black;

            UpdateButtonStates();
        }

        private void LoadStateFromShared()
        {
            if (!string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
            {
                lblWorkspace.Text = SharedWorkflowState.WorkspacePath;
                lblWorkspace.ForeColor = Color.DarkGreen;
            }
            if (SharedWorkflowState.SelectedCounties != null && SharedWorkflowState.SelectedCounties.Any())
            {
                lblCounties.Text = $"��ѡ�� {SharedWorkflowState.SelectedCounties.Count} ��������{string.Join(", ", SharedWorkflowState.SelectedCounties)}";
                lblCounties.ForeColor = Color.DarkGreen;
            }

            // ����ǰ������1״̬
            if (!string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData1Path))
            {
                lblPrerequisiteData1Status.Text = SharedWorkflowState.PrerequisiteData1Path;
                lblPrerequisiteData1Status.ForeColor = Color.DarkGreen;
                prerequisiteData1Path = SharedWorkflowState.PrerequisiteData1Path;
            }

            // ����ǰ������2״̬
            if (!string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData2Path))
            {
                lblPrerequisiteData2Status.Text = SharedWorkflowState.PrerequisiteData2Path;
                lblPrerequisiteData2Status.ForeColor = Color.DarkGreen;
                prerequisiteData2Path = SharedWorkflowState.PrerequisiteData2Path;
            }

            // �����ؼ��ձ�״̬
            lblCountyEmptyTablesStatus.Text = SharedWorkflowState.CountyEmptyTablesCreated ? "�ؼ��ձ������" : "δ�����ؼ��ձ�";
            lblCountyEmptyTablesStatus.ForeColor = SharedWorkflowState.CountyEmptyTablesCreated ? Color.DarkGreen : Color.Black;

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasWorkspace = !string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath);
            bool hasCounties = SharedWorkflowState.SelectedCounties != null && SharedWorkflowState.SelectedCounties.Count > 0;
            bool hasBasicSetup = hasWorkspace && hasCounties;

            btnLoadPrerequisiteData1.Enabled = hasBasicSetup;
            btnLoadPrerequisiteData2.Enabled = hasBasicSetup;
            btnCreateCountyEmptyTables.Enabled = hasBasicSetup;

            // OK��ťֻ�е�����ǰ�Ჽ�趼��ɺ������
            bool prereqDataLoaded = !string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData1Path) &&
                                  !string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData2Path);
            btnOK.Enabled = hasBasicSetup && prereqDataLoaded && SharedWorkflowState.CountyEmptyTablesCreated;
        }

        private void BtnSelectWorkspace_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��Ŀ�����ݿ�·����File Geodatabase .gdb��";
                if (!string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
                {
                    dialog.SelectedPath = SharedWorkflowState.WorkspacePath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SelectedPath.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase) || System.IO.Directory.Exists(dialog.SelectedPath))
                    {
                        SharedWorkflowState.WorkspacePath = dialog.SelectedPath;
                        lblWorkspace.Text = SharedWorkflowState.WorkspacePath;
                        lblWorkspace.ForeColor = Color.DarkGreen;
                        // ��������ռ���ģ����ú��������־
                        SharedWorkflowState.ResetBasicDataFlags();
                        InitializeFormState(); // ���³�ʼ���Է�ӳ���õı�־
                    }
                    else
                    {
                        MessageBox.Show("��ѡ����Ч��File Geodatabase (.gdb) ·����", "����",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            UpdateButtonStates();
        }

        

        private void BtnLoadPrerequisiteData1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "�����ļ� (*.shp;*.gdb;*.mdb)|*.shp;*.gdb;*.mdb|�����ļ� (*.*)|*.*";
                dialog.Title = "ѡ��ǰ������1";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // ����·��
                    prerequisiteData1Path = dialog.FileName;
                    SharedWorkflowState.PrerequisiteData1Path = prerequisiteData1Path;

                    // ����UI
                    lblPrerequisiteData1Status.Text = prerequisiteData1Path;
                    lblPrerequisiteData1Status.ForeColor = Color.DarkGreen;

                    MessageBox.Show("ǰ������1������ɡ�", "�ɹ�");
                    UpdateButtonStates();
                }
            }
        }

        private void BtnLoadPrerequisiteData2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "�����ļ� (*.shp;*.gdb;*.mdb)|*.shp;*.gdb;*.mdb|�����ļ� (*.*)|*.*";
                dialog.Title = "ѡ��ǰ������2";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // ����·��
                    prerequisiteData2Path = dialog.FileName;
                    SharedWorkflowState.PrerequisiteData2Path = prerequisiteData2Path;

                    // ����UI
                    lblPrerequisiteData2Status.Text = prerequisiteData2Path;
                    lblPrerequisiteData2Status.ForeColor = Color.DarkGreen;

                    MessageBox.Show("ǰ������2������ɡ�", "�ɹ�");
                    UpdateButtonStates();
                }
            }
        }

        private void BtnCreateCountyEmptyTables_Click(object sender, EventArgs e)
        {
            // ʵ���߼���ռλ��
            MessageBox.Show("���ڴ����ؼ��ձ�...", "��ʾ");
            // ��ʵ�֣������ձ��߼�
            // �ɹ���
            SharedWorkflowState.CountyEmptyTablesCreated = true;
            lblCountyEmptyTablesStatus.Text = "�ؼ��ձ������";
            lblCountyEmptyTablesStatus.ForeColor = Color.DarkGreen;
            MessageBox.Show("�ؼ��ձ������.", "�ɹ�");
            UpdateButtonStates();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // ����Ƿ�����ǰ�����ݶ��Ѽ���
            //bool prerequisiteDataLoaded = !string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData1Path) &&
            //                              !string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData2Path);

            //SharedWorkflowState.IsBasicDataPrepared = prerequisiteDataLoaded && SharedWorkflowState.CountyEmptyTablesCreated &&
            //                                          !string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) &&
            //                                          SharedWorkflowState.SelectedCounties != null && SharedWorkflowState.SelectedCounties.Any();

            //if (SharedWorkflowState.IsBasicDataPrepared)
            //{
            //    this.DialogResult = DialogResult.OK;
            //    this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("����������л�������׼�����衣", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}