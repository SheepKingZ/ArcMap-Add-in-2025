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
            lblWorkspace.Text = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? "未选择工作空间" : SharedWorkflowState.WorkspacePath;
            lblWorkspace.ForeColor = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? Color.Black : Color.DarkGreen;

            if (SharedWorkflowState.SelectedCounties != null && SharedWorkflowState.SelectedCounties.Count > 0)
            {
                lblCounties.Text = $"已选择 {SharedWorkflowState.SelectedCounties.Count} 个县区：{string.Join(", ", SharedWorkflowState.SelectedCounties)}";
                lblCounties.ForeColor = Color.DarkGreen;
            }
            else
            {
                lblCounties.Text = "未选择县区";
                lblCounties.ForeColor = Color.Black;
            }

            lblPrerequisiteDataStatus.Text = SharedWorkflowState.PrerequisiteDataLoaded ? "前提数据加载完成" : "未加载前提数据";
            lblPrerequisiteDataStatus.ForeColor = SharedWorkflowState.PrerequisiteDataLoaded ? Color.DarkGreen : Color.Black;
            lblCountyEmptyTablesStatus.Text = SharedWorkflowState.CountyEmptyTablesCreated ? "县级空表创建完成" : "未创建县级空表";
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
                lblCounties.Text = $"已选择 {SharedWorkflowState.SelectedCounties.Count} 个县区：{string.Join(", ", SharedWorkflowState.SelectedCounties)}";
                lblCounties.ForeColor = Color.DarkGreen;
            }
            // Reflect status of prerequisite data and empty tables
            lblPrerequisiteDataStatus.Text = SharedWorkflowState.PrerequisiteDataLoaded ? "前提数据加载完成" : "未加载前提数据";
            lblPrerequisiteDataStatus.ForeColor = SharedWorkflowState.PrerequisiteDataLoaded ? Color.DarkGreen : Color.Black;
            lblCountyEmptyTablesStatus.Text = SharedWorkflowState.CountyEmptyTablesCreated ? "县级空表创建完成" : "未创建县级空表";
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
                dialog.Description = "选择目标数据库路径（File Geodatabase .gdb）";
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
                        MessageBox.Show("请选择有效的File Geodatabase (.gdb) 路径！", "错误",
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
                    lblCounties.Text = $"已选择 {SharedWorkflowState.SelectedCounties.Count} 个县区：{string.Join(", ", SharedWorkflowState.SelectedCounties)}";
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
            MessageBox.Show("正在加载前提数据...", "提示");
            // TODO: Implement prerequisite data loading logic
            // On success:
            SharedWorkflowState.PrerequisiteDataLoaded = true;
            lblPrerequisiteDataStatus.Text = "前提数据加载完成";
            lblPrerequisiteDataStatus.ForeColor = Color.DarkGreen;
            MessageBox.Show("前提数据加载完成.", "成功");
            UpdateButtonStates();
        }

        private void BtnCreateCountyEmptyTables_Click(object sender, EventArgs e)
        {
            // Placeholder for actual logic
            MessageBox.Show("正在创建县级空表...", "提示");
            // TODO: Implement creation of empty tables logic
            // On success:
            SharedWorkflowState.CountyEmptyTablesCreated = true;
            lblCountyEmptyTablesStatus.Text = "县级空表创建完成";
            lblCountyEmptyTablesStatus.ForeColor = Color.DarkGreen;
            MessageBox.Show("县级空表创建完成.", "成功");
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
                MessageBox.Show("请先完成所有基础数据准备步骤。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}