using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // For CountySelectionForm

namespace TestArcMapAddin2.Forms
{
    public partial class BasicDataPreparationForm : Form
    {
        public BasicDataPreparationForm()
        {
            InitializeComponent();
            InitializeFormState();
            LoadStateFromShared();
        }

        private void InitializeFormState()
        {
            // Initialize labels based on current shared state or defaults
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

            lblPrerequisiteDataStatus.Text = SharedWorkflowState.PrerequisiteDataLoaded ? "ǰ�����ݼ������" : "δ����ǰ������";
            lblPrerequisiteDataStatus.ForeColor = SharedWorkflowState.PrerequisiteDataLoaded ? Color.DarkGreen : Color.Black;
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
            // Reflect status of prerequisite data and empty tables
            lblPrerequisiteDataStatus.Text = SharedWorkflowState.PrerequisiteDataLoaded ? "ǰ�����ݼ������" : "δ����ǰ������";
            lblPrerequisiteDataStatus.ForeColor = SharedWorkflowState.PrerequisiteDataLoaded ? Color.DarkGreen : Color.Black;
            lblCountyEmptyTablesStatus.Text = SharedWorkflowState.CountyEmptyTablesCreated ? "�ؼ��ձ������" : "δ�����ؼ��ձ�";
            lblCountyEmptyTablesStatus.ForeColor = SharedWorkflowState.CountyEmptyTablesCreated ? Color.DarkGreen : Color.Black;
            
            UpdateButtonStates();
        }


        private void UpdateButtonStates()
        {
            bool hasWorkspace = !string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath);
            bool hasCounties = SharedWorkflowState.SelectedCounties != null && SharedWorkflowState.SelectedCounties.Count > 0;
            bool hasBasicSetup = hasWorkspace && hasCounties;

            btnLoadPrerequisiteData.Enabled = hasBasicSetup;
            btnCreateCountyEmptyTables.Enabled = hasBasicSetup;
            
            // OK button should only be enabled if all prerequisite steps are done
            btnOK.Enabled = hasBasicSetup && SharedWorkflowState.PrerequisiteDataLoaded && SharedWorkflowState.CountyEmptyTablesCreated;
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
                        // Reset subsequent step flags if workspace changes
                        SharedWorkflowState.ResetBasicDataFlags(); 
                        InitializeFormState(); // Re-initialize to reflect reset flags
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

        private void BtnSelectCounties_Click(object sender, EventArgs e)
        {
            using (CountySelectionForm form = new CountySelectionForm(SharedWorkflowState.SelectedCounties))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SharedWorkflowState.SelectedCounties = form.SelectedCounties;
                    lblCounties.Text = $"��ѡ�� {SharedWorkflowState.SelectedCounties.Count} ��������{string.Join(", ", SharedWorkflowState.SelectedCounties)}";
                    lblCounties.ForeColor = Color.DarkGreen;
                     // Reset subsequent step flags if counties change
                    SharedWorkflowState.ResetBasicDataFlags();
                    InitializeFormState(); // Re-initialize to reflect reset flags
                }
            }
            UpdateButtonStates();
        }

        private void BtnLoadPrerequisiteData_Click(object sender, EventArgs e)
        {
            // Placeholder for actual logic
            MessageBox.Show("���ڼ���ǰ������...", "��ʾ");
            // TODO: Implement prerequisite data loading logic
            // On success:
            SharedWorkflowState.PrerequisiteDataLoaded = true;
            lblPrerequisiteDataStatus.Text = "ǰ�����ݼ������";
            lblPrerequisiteDataStatus.ForeColor = Color.DarkGreen;
            MessageBox.Show("ǰ�����ݼ������.", "�ɹ�");
            UpdateButtonStates();
        }

        private void BtnCreateCountyEmptyTables_Click(object sender, EventArgs e)
        {
            // Placeholder for actual logic
            MessageBox.Show("���ڴ����ؼ��ձ�...", "��ʾ");
            // TODO: Implement creation of empty tables logic
            // On success:
            SharedWorkflowState.CountyEmptyTablesCreated = true;
            lblCountyEmptyTablesStatus.Text = "�ؼ��ձ������";
            lblCountyEmptyTablesStatus.ForeColor = Color.DarkGreen;
            MessageBox.Show("�ؼ��ձ������.", "�ɹ�");
            UpdateButtonStates();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            SharedWorkflowState.IsBasicDataPrepared = SharedWorkflowState.PrerequisiteDataLoaded && SharedWorkflowState.CountyEmptyTablesCreated &&
                                                      !string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) &&
                                                      SharedWorkflowState.SelectedCounties != null && SharedWorkflowState.SelectedCounties.Any();
            if (SharedWorkflowState.IsBasicDataPrepared)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("����������л�������׼�����衣", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}