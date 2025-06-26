using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // 用于CountySelectionForm

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
            // 根据当前共享状态或默认值初始化标签
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

            // 更新前提数据1状态
            if (!string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData1Path))
            {
                lblPrerequisiteData1Status.Text = SharedWorkflowState.PrerequisiteData1Path;
                lblPrerequisiteData1Status.ForeColor = Color.DarkGreen;
                prerequisiteData1Path = SharedWorkflowState.PrerequisiteData1Path;
            }
            else
            {
                lblPrerequisiteData1Status.Text = "未加载前提数据1";
                lblPrerequisiteData1Status.ForeColor = Color.Black;
            }

            // 更新前提数据2状态
            if (!string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData2Path))
            {
                lblPrerequisiteData2Status.Text = SharedWorkflowState.PrerequisiteData2Path;
                lblPrerequisiteData2Status.ForeColor = Color.DarkGreen;
                prerequisiteData2Path = SharedWorkflowState.PrerequisiteData2Path;
            }
            else
            {
                lblPrerequisiteData2Status.Text = "未加载前提数据2";
                lblPrerequisiteData2Status.ForeColor = Color.Black;
            }

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

            // 加载前提数据1状态
            if (!string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData1Path))
            {
                lblPrerequisiteData1Status.Text = SharedWorkflowState.PrerequisiteData1Path;
                lblPrerequisiteData1Status.ForeColor = Color.DarkGreen;
                prerequisiteData1Path = SharedWorkflowState.PrerequisiteData1Path;
            }

            // 加载前提数据2状态
            if (!string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData2Path))
            {
                lblPrerequisiteData2Status.Text = SharedWorkflowState.PrerequisiteData2Path;
                lblPrerequisiteData2Status.ForeColor = Color.DarkGreen;
                prerequisiteData2Path = SharedWorkflowState.PrerequisiteData2Path;
            }

            // 加载县级空表状态
            lblCountyEmptyTablesStatus.Text = SharedWorkflowState.CountyEmptyTablesCreated ? "县级空表创建完成" : "未创建县级空表";
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

            // OK按钮只有当所有前提步骤都完成后才启用
            bool prereqDataLoaded = !string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData1Path) &&
                                  !string.IsNullOrEmpty(SharedWorkflowState.PrerequisiteData2Path);
            btnOK.Enabled = hasBasicSetup && prereqDataLoaded && SharedWorkflowState.CountyEmptyTablesCreated;
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
                        // 如果工作空间更改，重置后续步骤标志
                        SharedWorkflowState.ResetBasicDataFlags();
                        InitializeFormState(); // 重新初始化以反映重置的标志
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

        

        private void BtnLoadPrerequisiteData1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "数据文件 (*.shp;*.gdb;*.mdb)|*.shp;*.gdb;*.mdb|所有文件 (*.*)|*.*";
                dialog.Title = "选择前提数据1";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 保存路径
                    prerequisiteData1Path = dialog.FileName;
                    SharedWorkflowState.PrerequisiteData1Path = prerequisiteData1Path;

                    // 更新UI
                    lblPrerequisiteData1Status.Text = prerequisiteData1Path;
                    lblPrerequisiteData1Status.ForeColor = Color.DarkGreen;

                    MessageBox.Show("前提数据1加载完成。", "成功");
                    UpdateButtonStates();
                }
            }
        }

        private void BtnLoadPrerequisiteData2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "数据文件 (*.shp;*.gdb;*.mdb)|*.shp;*.gdb;*.mdb|所有文件 (*.*)|*.*";
                dialog.Title = "选择前提数据2";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 保存路径
                    prerequisiteData2Path = dialog.FileName;
                    SharedWorkflowState.PrerequisiteData2Path = prerequisiteData2Path;

                    // 更新UI
                    lblPrerequisiteData2Status.Text = prerequisiteData2Path;
                    lblPrerequisiteData2Status.ForeColor = Color.DarkGreen;

                    MessageBox.Show("前提数据2加载完成。", "成功");
                    UpdateButtonStates();
                }
            }
        }

        private void BtnCreateCountyEmptyTables_Click(object sender, EventArgs e)
        {
            // 实际逻辑的占位符
            MessageBox.Show("正在创建县级空表...", "提示");
            // 待实现：创建空表逻辑
            // 成功后：
            SharedWorkflowState.CountyEmptyTablesCreated = true;
            lblCountyEmptyTablesStatus.Text = "县级空表创建完成";
            lblCountyEmptyTablesStatus.ForeColor = Color.DarkGreen;
            MessageBox.Show("县级空表创建完成.", "成功");
            UpdateButtonStates();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // 检查是否两个前提数据都已加载
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
            //    MessageBox.Show("请先完成所有基础数据准备步骤。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}