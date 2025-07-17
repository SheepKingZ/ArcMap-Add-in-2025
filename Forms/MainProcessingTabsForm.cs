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
        // 追踪步骤完成状态 - 调整为跳过LCXZGX生成步骤
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
            // 初始化森林资源清查步骤（跳过LCXZGX生成步骤）
            // ⚠️ 调整：移除 extractScope 步骤，因为LCXZGX现在从原始数据直接转换生成
            forestTasksCompleted.Add("createBasemapLink", false);
            forestTasksCompleted.Add("supplementPrice", false);
            forestTasksCompleted.Add("calculateValue", false);
            forestTasksCompleted.Add("cleanQA", false);
            forestTasksCompleted.Add("buildDBTables", false);

            // 初始化草地资源清查步骤（跳过LCXZGX生成步骤）
            grasslandTasksCompleted.Add("createBasemapLink", false);
            grasslandTasksCompleted.Add("supplementPrice", false);
            grasslandTasksCompleted.Add("calculateValue", false);
            grasslandTasksCompleted.Add("cleanQA", false);
            grasslandTasksCompleted.Add("buildDBTables", false);

            // 初始化湿地资源清查步骤（跳过LCXZGX生成步骤）
            wetlandTasksCompleted.Add("extractScopeBasemap", false);
            wetlandTasksCompleted.Add("cleanQA", false);
            wetlandTasksCompleted.Add("buildDBTables", false);

            // 初始化综合输出步骤
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
            UpdateProgress("等待开始处理 - 基础数据已准备就绪");
            lblForestProcessingStatus.Text = "等待森林资源清查处理 - 基础数据已就绪";
            lblGrasslandProcessingStatus.Text = "等待草地资源清查处理 - 基础数据已就绪";
            lblWetlandProcessingStatus.Text = "等待湿地资源清查处理 - 基础数据已就绪";
            lblFinalOutputStatus.Text = "等待最终成果处理";

            // 设置初始颜色
            lblForestProcessingStatus.ForeColor = Color.DarkBlue; // 调整：使用深蓝色表示已准备状态
            lblGrasslandProcessingStatus.ForeColor = Color.DarkBlue;
            lblWetlandProcessingStatus.ForeColor = Color.DarkBlue;
            lblFinalOutputStatus.ForeColor = Color.Black;
            lblProgress.ForeColor = Color.DarkBlue;

            UpdateButtonStates();

            // 更新进度条 - 调整初始进度显示
            // forestProgressBar 由于跳过了extractScope步骤，初始值设为0
            grasslandProgressBar.Value = 0;
            wetlandProgressBar.Value = 0;
            outputProgressBar.Value = 0;
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
            Application.DoEvents(); // 谨慎使用，可能导致重入问题
        }

        private void UpdateButtonStates()
        {
            // Remove basic data preparation requirement - all buttons can now be used independently
            // 移除基础数据准备要求 - 所有按钮现在都可以独立使用

            // 森林选项卡按钮 - 调整：所有按钮直接启用，因为LCXZGX数据已从基础数据准备阶段生成
            btnForestExtractScope.Enabled = true; // 保留按钮但功能调整为数据准备入口
            btnForestCreateBasemapLinkPrice.Enabled = true; // 直接启用，无需依赖extractScope
            btnForestSupplementPrice.Enabled = true; // 直接启用，无需依赖createBasemapLink

            // 启用之前被注释的森林工作流按钮
            //if (btnForestCalculateValue != null)
            //    btnForestCalculateValue.Enabled = true;
            //if (btnForestCleanQA != null)
            //    btnForestCleanQA.Enabled = true;
            //if (btnForestBuildDBTables != null)
            //    btnForestBuildDBTables.Enabled = true;

            // 草地选项卡按钮 - 调整：移除依赖，直接启用
            btnGrasslandExtractScope.Enabled = true; // 调整为数据准备入口
            btnGrasslandCreateBasemapLinkPrice.Enabled = true; // 直接启用
            btnGrasslandSupplementPrice.Enabled = true; // 直接启用
            btnGrasslandCalculateValue.Enabled = true; // 直接启用
            btnGrasslandCleanQA.Enabled = true; // 直接启用
            btnGrasslandBuildDBTables.Enabled = true; // 直接启用

            // 湿地选项卡按钮 - 调整：移除依赖，直接启用
            btnWetlandExtractScopeBasemap.Enabled = true; // 调整为数据准备入口
            btnWetlandCleanQA.Enabled = true; // 直接启用
            btnWetlandBuildDBTables.Enabled = true; // 直接启用

            // 成果输出选项卡按钮 - 移除工作流完成依赖，允许独立使用
            btnOverallQualityCheck.Enabled = true;
            btnStatisticalAggregation.Enabled = true; // 移除依赖
            btnDataAnalysis.Enabled = true; // 移除依赖
            btnExportDatasetDB.Enabled = true; // 移除依赖
            btnExportSummaryTables.Enabled = true; // 移除依赖
            btnGenerateReport.Enabled = true; // 移除依赖
            btnGenerateThematicMaps.Enabled = true; // 移除依赖
        }

        private void UpdateWorkflowState()
        {
            // 更新森林工作流进度 - 调整进度计算逻辑
            int forestSteps = forestTasksCompleted.Count;
            int forestCompleted = forestTasksCompleted.Count(x => x.Value);

            // 调整：由于跳过了extractScope步骤，进度计算需要相应调整
            // 如果有 forestProgressBar 控件，更新其值
            // forestProgressBar.Value = forestSteps > 0 ? (forestCompleted * 100) / forestSteps : 0;
            // forestStepLabel.Text = $"已完成 {forestCompleted}/{forestSteps} 步骤（基础数据已就绪）";

            // 更新草地工作流进度 - 调整进度计算
            int grasslandSteps = grasslandTasksCompleted.Count;
            int grasslandCompleted = grasslandTasksCompleted.Count(x => x.Value);
            grasslandProgressBar.Value = grasslandSteps > 0 ? (grasslandCompleted * 100) / grasslandSteps : 0;
            grasslandStepLabel.Text = $"已完成 {grasslandCompleted}/{grasslandSteps} 步骤（基础数据已就绪）";

            // 更新湿地工作流进度
            int wetlandSteps = wetlandTasksCompleted.Count;
            int wetlandCompleted = wetlandTasksCompleted.Count(x => x.Value);
            wetlandProgressBar.Value = wetlandSteps > 0 ? (wetlandCompleted * 100) / wetlandSteps : 0;
            wetlandStepLabel.Text = $"已完成 {wetlandCompleted}/{wetlandSteps} 步骤（基础数据已就绪）";

            // 更新输出工作流进度
            int outputSteps = outputTasksCompleted.Count;
            int outputCompleted = outputTasksCompleted.Count(x => x.Value);
            outputProgressBar.Value = outputSteps > 0 ? (outputCompleted * 100) / outputSteps : 0;
            outputStepLabel.Text = $"已完成 {outputCompleted}/{outputSteps} 步骤";

            // 更新总体进度 - 调整总体进度计算
            int totalSteps = forestSteps + grasslandSteps + wetlandSteps + outputSteps;
            int totalCompleted = forestCompleted + grasslandCompleted + wetlandCompleted + outputCompleted;
            int overallPercentage = totalSteps > 0 ? (totalCompleted * 100) / totalSteps : 0;

            // 更新森林资源图表
            UpdateForestResourceChart();
        }

        private void UpdateForestResourceChart()
        {
            // 模拟数据 - 在实际应用中应替换为真实数据
            // 调整：移除extractScope相关的图表项
            forestResourceChart.Series.Clear();
            forestResourceChart.Titles.Clear();

            forestResourceChart.Titles.Add("森林资源清查进度（基础数据已就绪）");
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("森林资源");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            // 调整：移除提取范围步骤，直接从底图与价格关联开始
            if (forestTasksCompleted["createBasemapLink"])
                series.Points.AddXY("底图与价格关联", 20);
            if (forestTasksCompleted["supplementPrice"])
                series.Points.AddXY("补充价格", 20);
            if (forestTasksCompleted["calculateValue"])
                series.Points.AddXY("价值计算", 25);
            if (forestTasksCompleted["cleanQA"])
                series.Points.AddXY("数据清洗与质检", 20);
            if (forestTasksCompleted["buildDBTables"])
                series.Points.AddXY("构建库表", 15);

            if (series.Points.Count == 0)
                series.Points.AddXY("基础数据已就绪，等待处理", 100);

            forestResourceChart.Series.Add(series);
        }

        #region 事件处理程序

        // 森林资源处理程序 - 调整为数据准备入口
        private void BtnForestExtractScope_Click(object sender, EventArgs e)
        {
            try
            {
                // 调整：由于LCXZGX生成步骤已跳过，这个按钮现在作为数据准备的入口
                UpdateProgress("打开基础数据准备窗口...");

                // Create and show the Basic form as a dialog
                Basic basicForm = new Basic();
                var result = basicForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    UpdateProgress("基础数据准备完成，可以开始处理");
                    lblForestProcessingStatus.Text = "基础数据准备完成，可开始森林资源清查";
                    lblForestProcessingStatus.ForeColor = Color.DarkGreen;

                    // 更新其他资源类型的状态
                    lblGrasslandProcessingStatus.Text = "基础数据准备完成，可开始草地资源清查";
                    lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
                    lblWetlandProcessingStatus.Text = "基础数据准备完成，可开始湿地资源清查";
                    lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
                }
                else
                {
                    UpdateProgress("基础数据准备未完成或已取消");
                }
            }
            catch (Exception ex)
            {
                UpdateProgress("基础数据准备失败");
                MessageBox.Show($"打开基础数据窗口时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnForestCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在处理森林工作底图与价格关联...");

            // 调整：添加LCXZGX数据验证
            if (!ValidateLCXZGXDataExists())
            {
                UpdateProgress("LCXZGX数据未就绪");
                MessageBox.Show("请先完成基础数据准备，确保LCXZGX数据已生成。", "数据未就绪",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 待实现：实际逻辑
            forestTasksCompleted["createBasemapLink"] = true;
            lblForestProcessingStatus.Text = "森林底图与价格关联完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林底图与价格关联完成");

            // 更新工作流状态和按钮状态
            UpdateWorkflowState();
            UpdateButtonStates();

            // 更新详细结果文本
            if (forestResultsTextBox != null)
            {
                forestResultsTextBox.AppendText("=== 森林底图与价格关联 ===\r\n");
                forestResultsTextBox.AppendText("- 基于已生成的LCXZGX数据进行处理\r\n");
                forestResultsTextBox.AppendText("- 工作范围与林地分等数据关联完成\r\n");
                forestResultsTextBox.AppendText("- 工作范围与林地定级数据关联完成\r\n");
                forestResultsTextBox.AppendText("- 基准地价与图斑空间挂接完成\r\n");
                forestResultsTextBox.AppendText("- 关联成功: 256图斑\r\n");
                forestResultsTextBox.AppendText("- 关联失败: 27图斑\r\n\r\n");
            }
        }

        private void BtnForestSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在补充森林基准价格...");

            // 待实现：实际逻辑
            forestTasksCompleted["supplementPrice"] = true;
            lblForestProcessingStatus.Text = "森林基准价格补充完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林基准价格补充完成");

            // 更新工作流状态和按钮状态
            UpdateWorkflowState();
            UpdateButtonStates();

            // 更新详细结果文本
            if (forestResultsTextBox != null)
            {
                forestResultsTextBox.AppendText("=== 森林基准价格补充 ===\r\n");
                forestResultsTextBox.AppendText("- 提取林地定级指标及权重信息\r\n");
                forestResultsTextBox.AppendText("- 提取基准价格信息\r\n");
                forestResultsTextBox.AppendText("- 补充落空图斑价格\r\n");
                forestResultsTextBox.AppendText("- 补充成功: 27图斑\r\n");
                forestResultsTextBox.AppendText("- 基准价格范围: 2.45-8.72万元/公顷\r\n\r\n");
            }
        }

        private void BtnForestCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在计算森林资产价值...");

            // 待实现：实际逻辑
            forestTasksCompleted["calculateValue"] = true;
            lblForestProcessingStatus.Text = "森林资产价值计算完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林资产价值计算完成");

            // 更新工作流状态和按钮状态
            UpdateWorkflowState();
            UpdateButtonStates();

            // 更新详细结果文本
            if (forestResultsTextBox != null)
            {
                forestResultsTextBox.AppendText("=== 森林资产价值计算 ===\r\n");
                forestResultsTextBox.AppendText("- 应用基准价格修正因子和权重\r\n");
                forestResultsTextBox.AppendText("- 计算宗地价格\r\n");
                forestResultsTextBox.AppendText("- 应用期日修正和年期修正\r\n");
                forestResultsTextBox.AppendText("- 总价值: 10652896元\r\n");
                forestResultsTextBox.AppendText("- 平均单价: 8.42万元/公顷\r\n\r\n");
            }
        }

        private void BtnForestCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行森林数据清洗与质检...");

            // 待实现：实际逻辑
            forestTasksCompleted["cleanQA"] = true;
            lblForestProcessingStatus.Text = "森林数据清洗与质检完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林数据清洗与质检完成");

            // 更新工作流状态和按钮状态
            UpdateWorkflowState();
            UpdateButtonStates();

            // 更新详细结果文本
            if (forestResultsTextBox != null)
            {
                forestResultsTextBox.AppendText("=== 森林数据清洗与质检 ===\r\n");
                forestResultsTextBox.AppendText("- 清理多余字段和临时数据\r\n");
                forestResultsTextBox.AppendText("- 调整字段格式和数据精度\r\n");
                forestResultsTextBox.AppendText("- 检查数据有效性与完整性\r\n");
                forestResultsTextBox.AppendText("- 修复异常数据: 5条\r\n");
                forestResultsTextBox.AppendText("- 质检通过率: 98.2%\r\n\r\n");
            }
        }

        private void BtnForestBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建森林库表...");

            // 待实现：实际逻辑
            forestTasksCompleted["buildDBTables"] = true;
            lblForestProcessingStatus.Text = "森林库表构建完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林库表构建完成");

            // 更新工作流状态和按钮状态
            UpdateWorkflowState();
            UpdateButtonStates();

            // 更新详细结果文本
            if (forestResultsTextBox != null)
            {
                forestResultsTextBox.AppendText("=== 森林库表构建 ===\r\n");
                forestResultsTextBox.AppendText("- 提取清查成果原数据相关字段\r\n");
                forestResultsTextBox.AppendText("- 构建符合汇交规范的数据集\r\n");
                forestResultsTextBox.AppendText("- 生成基础数表和统计表\r\n");
                forestResultsTextBox.AppendText("- 创建数据表: 3个\r\n");
                forestResultsTextBox.AppendText("- 创建统计表: 2个\r\n\r\n");
            }
        }

        // 草地资源处理程序 - 调整为数据准备入口
        private void BtnGrasslandExtractScope_Click(object sender, EventArgs e)
        {
            try
            {
                // 调整：作为草地资源的数据准备入口
                UpdateProgress("打开基础数据准备窗口（草地资源）...");

                Basic basicForm = new Basic();
                var result = basicForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    UpdateProgress("草地基础数据准备完成");
                    lblGrasslandProcessingStatus.Text = "草地基础数据准备完成，可开始处理";
                    lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
                }
                else
                {
                    UpdateProgress("草地基础数据准备未完成");
                }
            }
            catch (Exception ex)
            {
                UpdateProgress("草地基础数据准备失败");
                MessageBox.Show($"草地数据准备失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGrasslandCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在处理草地工作底图与价格关联...");

            if (!ValidateLCXZGXDataExists())
            {
                UpdateProgress("草地LCXZGX数据未就绪");
                MessageBox.Show("请先完成基础数据准备，确保草地LCXZGX数据已生成。", "数据未就绪",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            grasslandTasksCompleted["createBasemapLink"] = true;
            lblGrasslandProcessingStatus.Text = "草地底图与价格关联完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地底图与价格关联完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在补充草地基准价格...");
            grasslandTasksCompleted["supplementPrice"] = true;
            lblGrasslandProcessingStatus.Text = "草地基准价格补充完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地基准价格补充完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandCalculateValue_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在计算草地资产价值...");
            grasslandTasksCompleted["calculateValue"] = true;
            lblGrasslandProcessingStatus.Text = "草地资产价值计算完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地资产价值计算完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行草地数据清洗与质检...");
            grasslandTasksCompleted["cleanQA"] = true;
            lblGrasslandProcessingStatus.Text = "草地数据清洗与质检完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地数据清洗与质检完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建草地库表...");
            grasslandTasksCompleted["buildDBTables"] = true;
            lblGrasslandProcessingStatus.Text = "草地库表构建完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地库表构建完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        // 湿地资源处理程序 - 调整为数据准备入口
        private void BtnWetlandExtractScopeBasemap_Click(object sender, EventArgs e)
        {
            try
            {
                // 调整：作为湿地资源的数据准备入口
                UpdateProgress("打开基础数据准备窗口（湿地资源）...");

                Basic basicForm = new Basic();
                var result = basicForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    wetlandTasksCompleted["extractScopeBasemap"] = true;
                    UpdateProgress("湿地基础数据准备完成");
                    lblWetlandProcessingStatus.Text = "湿地工作范围与底图制作完成";
                    lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
                    UpdateWorkflowState();
                    UpdateButtonStates();
                }
                else
                {
                    UpdateProgress("湿地基础数据准备未完成");
                }
            }
            catch (Exception ex)
            {
                UpdateProgress("湿地基础数据准备失败");
                MessageBox.Show($"湿地数据准备失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnWetlandCleanQA_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行湿地数据清洗与质检...");
            wetlandTasksCompleted["cleanQA"] = true;
            lblWetlandProcessingStatus.Text = "湿地数据清洗与质检完成";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("湿地数据清洗与质检完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnWetlandBuildDBTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在构建湿地库表...");
            wetlandTasksCompleted["buildDBTables"] = true;
            lblWetlandProcessingStatus.Text = "湿地库表构建完成";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("湿地库表构建完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        // 最终输出处理程序
        private void BtnOverallQualityCheck_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行综合质量检查...");
            outputTasksCompleted["overallQualityCheck"] = true;
            lblFinalOutputStatus.Text = "综合质量检查完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("综合质量检查完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnStatisticalAggregation_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行数据统计汇总...");
            outputTasksCompleted["statisticalAggregation"] = true;
            lblFinalOutputStatus.Text = "数据统计汇总完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("数据统计汇总完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnDataAnalysis_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在进行数据分析与挖掘...");
            outputTasksCompleted["dataAnalysis"] = true;
            lblFinalOutputStatus.Text = "数据分析与挖掘完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("数据分析与挖掘完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnExportDatasetDB_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在导出清查数据集...");
            outputTasksCompleted["exportDatasetDB"] = true;
            lblFinalOutputStatus.Text = "清查数据集导出完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("清查数据集导出完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnExportSummaryTables_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在导出汇总表...");
            outputTasksCompleted["exportSummaryTables"] = true;
            lblFinalOutputStatus.Text = "汇总表导出完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("汇总表导出完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在生成成果报告...");
            outputTasksCompleted["generateReport"] = true;
            lblFinalOutputStatus.Text = "成果报告生成完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("成果报告生成完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGenerateThematicMaps_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在生成专题图...");
            outputTasksCompleted["generateThematicMaps"] = true;
            lblFinalOutputStatus.Text = "专题图生成完成";
            lblFinalOutputStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("专题图生成完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            // 更新帮助文本，说明LCXZGX生成步骤的调整
            string helpText = @"广东省全民所有自然资源（森林、草地、湿地）资产清查工具使用说明";
       

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
            // 确保在 InitializeComponent 完全完成前调用此方法时，btnHelp 和 btnClose 不为 null
            if (btnHelp != null && btnClose != null && bottomPanel != null)
            {
                // 调整按钮相对于 bottomPanel 右边缘的位置
                btnClose.Location = new Point(bottomPanel.Width - btnClose.Width - 10, (bottomPanel.Height - btnClose.Height) / 2);
                btnHelp.Location = new Point(btnClose.Location.X - btnHelp.Width - 5, (bottomPanel.Height - btnHelp.Height) / 2);
            }
        }

        /// <summary>
        /// 验证LCXZGX数据是否存在
        /// 新增方法：用于验证基础数据是否已准备完成
        /// </summary>
        /// <returns>如果LCXZGX数据存在返回true，否则返回false</returns>
        private bool ValidateLCXZGXDataExists()
        {
            try
            {
                // 检查SharedDataManager中是否有可用的源数据
                var sourceDataFiles = SharedDataManager.GetSourceDataFiles();
                if (sourceDataFiles != null && sourceDataFiles.Count > 0)
                {
                    return true;
                }

                // 检查是否有输出路径设置
                if (!string.IsNullOrEmpty(SharedWorkflowState.OutputGDBPath))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"验证LCXZGX数据存在性时出错: {ex.Message}");
                return false;
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
                "隐藏详细流程说明 ▲" : "显示详细流程说明 ▼";

            // Add code to display the Basic.cs window
            //try
            //{
            //    // Create and show the Basic form as a dialog
            //    Basic basicForm = new Basic();
            //    basicForm.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"打开基础数据窗口时出错: {ex.Message}", "错误", 
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void forestStepLabel_Click(object sender, EventArgs e)
        {

        }

        private void buttonForestExcel_Click(object sender, EventArgs e)
        {

        }
    }
}