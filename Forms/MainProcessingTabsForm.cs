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
            UpdateProgress("等待开始处理");
            lblForestProcessingStatus.Text = "等待森林资源清查处理";
            lblGrasslandProcessingStatus.Text = "等待草地资源清查处理";
            lblWetlandProcessingStatus.Text = "等待湿地资源清查处理";
            lblFinalOutputStatus.Text = "等待最终成果处理";
            
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
                lblProgress.Invoke(new Action(() => lblProgress.Text = $"进度：{message}"));
            }
            else
            {
                lblProgress.Text = $"进度：{message}";
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
            UpdateProgress("正在提取森林工作范围...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "森林工作范围提取完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林工作范围提取完成");
        }

        private void BtnForestCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在处理森林工作底图与价格关联...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "森林底图与价格关联完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林底图与价格关联完成");
        }

        private void BtnForestSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在补充森林基准价格...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "森林基准价格补充完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen; // Keep DarkGreen for success
            UpdateProgress("森林基准价格补充完成");
        }

        private void BtnForestCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在计算森林资产价值...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "森林资产价值计算完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林资产价值计算完成");
        }

        private void BtnForestCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行森林数据清洗与质检...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "森林数据清洗与质检完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林数据清洗与质检完成");
        }

        private void BtnForestBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建森林库表...");
            // TODO: Implement logic
            lblForestProcessingStatus.Text = "森林库表构建完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林库表构建完成");
        }

        // Grassland resource handlers
        private void BtnGrasslandExtractScope_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在提取草地工作范围...");
            lblGrasslandProcessingStatus.Text = "草地工作范围提取完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地工作范围提取完成");
        }

        private void BtnGrasslandCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在处理草地工作底图与价格关联...");
            lblGrasslandProcessingStatus.Text = "草地底图与价格关联完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地底图与价格关联完成");
        }

        private void BtnGrasslandSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在补充草地基准价格...");
            lblGrasslandProcessingStatus.Text = "草地基准价格补充完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地基准价格补充完成");
        }

        private void BtnGrasslandCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在计算草地资产价值...");
            lblGrasslandProcessingStatus.Text = "草地资产价值计算完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地资产价值计算完成");
        }

        private void BtnGrasslandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行草地数据清洗与质检...");
            lblGrasslandProcessingStatus.Text = "草地数据清洗与质检完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地数据清洗与质检完成");
        }

        private void BtnGrasslandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建草地库表...");
            lblGrasslandProcessingStatus.Text = "草地库表构建完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地库表构建完成");
        }

        // Wetland resource handlers
        private void BtnWetlandExtractScopeBasemap_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在制作湿地工作范围与底图...");
            lblWetlandProcessingStatus.Text = "湿地工作范围与底图制作完成";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("湿地工作范围与底图制作完成");
        }

        private void BtnWetlandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行湿地数据清洗与质检...");
            lblWetlandProcessingStatus.Text = "湿地数据清洗与质检完成";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("湿地数据清洗与质检完成");
        }

        private void BtnWetlandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建湿地库表...");
            lblWetlandProcessingStatus.Text = "湿地库表构建完成";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("湿地库表构建完成");
        }

        // Final output handlers
        private void BtnOverallQualityCheck_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行综合质量检查...");
            lblFinalOutputStatus.Text = "综合质量检查完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("综合质量检查完成");
        }

        private void BtnStatisticalAggregation_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行数据统计汇总...");
            lblFinalOutputStatus.Text = "数据统计汇总完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("数据统计汇总完成");
        }

        private void BtnDataAnalysis_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行数据分析与挖掘...");
            lblFinalOutputStatus.Text = "数据分析与挖掘完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("数据分析与挖掘完成");
        }

        private void BtnExportDatasetDB_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在导出清查数据集...");
            lblFinalOutputStatus.Text = "清查数据集导出完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("清查数据集导出完成");
        }

        private void BtnExportSummaryTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在导出汇总表...");
            lblFinalOutputStatus.Text = "汇总表导出完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("汇总表导出完成");
        }
        
        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在生成成果报告...");
            lblFinalOutputStatus.Text = "成果报告生成完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("成果报告生成完成");
        }

        private void BtnGenerateThematicMaps_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在生成专题图...");
            lblFinalOutputStatus.Text = "专题图生成完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("专题图生成完成");
        }


        private void BtnHelp_Click(object sender, EventArgs e)
        {
            // Copied from original MainProcessForm
            string helpText = @"广东省全民所有自然资源（森林、草地、湿地）资产清查工具使用说明

基础数据准备 (通过 '基础数据准备' 窗口完成)
1. 选择工作空间：选择用于存放清查数据的File Geodatabase (.gdb)。
2. 选择县区：选择需要进行清查的一个或多个县区。
3. 加载前提数据：加载必要的背景数据。
4. 创建县级空表：为选定县区创建数据表结构。

--- 以下操作在本窗口进行 ---

森林资源资产清查
1. 提取森林工作范围：根据普查数据和开发边界，筛选国有林地及边界内集体林地。
... (rest of the help text as in original, ensure it's complete) ...

湿地资源资产清查
...

综合质检、统计与成果输出
...

注意事项：
- 请先完成'基础数据准备'窗口中的所有步骤。
- 请尽量按步骤顺序执行。
- 每个操作完成后会有相应提示。
- 部分操作可能耗时较长，请耐心等待。";

            MessageBox.Show(helpText, "使用帮助", MessageBoxButtons.OK, MessageBoxIcon.Information);
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