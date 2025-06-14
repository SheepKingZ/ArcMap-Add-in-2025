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

namespace TestArcMapAddin2.Forms
{
    public partial class MainProcessingTabsForm : Form
    {
        // 追踪步骤完成状态
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
            // 初始化森林资源清查步骤
            forestTasksCompleted.Add("extractScope", false);
            forestTasksCompleted.Add("createBasemapLink", false);
            forestTasksCompleted.Add("supplementPrice", false);
            forestTasksCompleted.Add("calculateValue", false);
            forestTasksCompleted.Add("cleanQA", false);
            forestTasksCompleted.Add("buildDBTables", false);

            // 初始化草地资源清查步骤
            grasslandTasksCompleted.Add("extractScope", false);
            grasslandTasksCompleted.Add("createBasemapLink", false);
            grasslandTasksCompleted.Add("supplementPrice", false);
            grasslandTasksCompleted.Add("calculateValue", false);
            grasslandTasksCompleted.Add("cleanQA", false);
            grasslandTasksCompleted.Add("buildDBTables", false);

            // 初始化湿地资源清查步骤
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
            UpdateProgress("等待开始处理");
            lblForestProcessingStatus.Text = "等待森林资源清查处理";
            lblGrasslandProcessingStatus.Text = "等待草地资源清查处理";
            lblWetlandProcessingStatus.Text = "等待湿地资源清查处理";
            lblFinalOutputStatus.Text = "等待最终成果处理";

            // 设置初始颜色
            lblForestProcessingStatus.ForeColor = Color.Black;
            lblGrasslandProcessingStatus.ForeColor = Color.Black;
            lblWetlandProcessingStatus.ForeColor = Color.Black;
            lblFinalOutputStatus.ForeColor = Color.Black;
            lblProgress.ForeColor = Color.Black;

            UpdateButtonStates();

            // 更新进度条
            forestProgressBar.Value = 0;
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
            bool basicDataReady = SharedWorkflowState.IsBasicDataPrepared;

            // 根据基础数据是否准备好启用/禁用各个选项卡中的控件
            // 在实际场景中，这会更加精细化，取决于前序步骤的完成情况

            // 森林选项卡按钮
            btnForestExtractScope.Enabled = basicDataReady;
            btnForestCreateBasemapLinkPrice.Enabled = basicDataReady && forestTasksCompleted["extractScope"];
            btnForestSupplementPrice.Enabled = basicDataReady && forestTasksCompleted["createBasemapLink"];
            btnForestCalculateValue.Enabled = basicDataReady && forestTasksCompleted["supplementPrice"];
            btnForestCleanQA.Enabled = basicDataReady && forestTasksCompleted["calculateValue"];
            btnForestBuildDBTables.Enabled = basicDataReady && forestTasksCompleted["cleanQA"];

            // 草地选项卡按钮
            btnGrasslandExtractScope.Enabled = basicDataReady;
            btnGrasslandCreateBasemapLinkPrice.Enabled = basicDataReady && grasslandTasksCompleted["extractScope"];
            btnGrasslandSupplementPrice.Enabled = basicDataReady && grasslandTasksCompleted["createBasemapLink"];
            btnGrasslandCalculateValue.Enabled = basicDataReady && grasslandTasksCompleted["calculateValue"];
            btnGrasslandCleanQA.Enabled = basicDataReady && grasslandTasksCompleted["cleanQA"];
            btnGrasslandBuildDBTables.Enabled = basicDataReady && grasslandTasksCompleted["cleanQA"];

            // 湿地选项卡按钮
            btnWetlandExtractScopeBasemap.Enabled = basicDataReady;
            btnWetlandCleanQA.Enabled = basicDataReady && wetlandTasksCompleted["extractScopeBasemap"];
            btnWetlandBuildDBTables.Enabled = basicDataReady && wetlandTasksCompleted["cleanQA"];

            // 成果输出选项卡按钮
            bool forestComplete = basicDataReady && forestTasksCompleted["buildDBTables"];
            bool grasslandComplete = basicDataReady && grasslandTasksCompleted["buildDBTables"];
            bool wetlandComplete = basicDataReady && wetlandTasksCompleted["buildDBTables"];
            bool allResourcesComplete = forestComplete && grasslandComplete && wetlandComplete;

            btnOverallQualityCheck.Enabled = allResourcesComplete;
            btnStatisticalAggregation.Enabled = allResourcesComplete && outputTasksCompleted["overallQualityCheck"];
            btnDataAnalysis.Enabled = allResourcesComplete && outputTasksCompleted["statisticalAggregation"];
            btnExportDatasetDB.Enabled = allResourcesComplete && outputTasksCompleted["dataAnalysis"];
            btnExportSummaryTables.Enabled = allResourcesComplete && outputTasksCompleted["exportDatasetDB"];
            btnGenerateReport.Enabled = allResourcesComplete && outputTasksCompleted["exportSummaryTables"];
            btnGenerateThematicMaps.Enabled = allResourcesComplete && outputTasksCompleted["generateReport"];
        }

        private void UpdateWorkflowState()
        {
            // 更新森林工作流进度
            int forestSteps = forestTasksCompleted.Count;
            int forestCompleted = forestTasksCompleted.Count(x => x.Value);
            forestProgressBar.Value = forestSteps > 0 ? (forestCompleted * 100) / forestSteps : 0;
            forestStepLabel.Text = $"已完成 {forestCompleted}/{forestSteps} 步骤";

            // 更新草地工作流进度
            int grasslandSteps = grasslandTasksCompleted.Count;
            int grasslandCompleted = grasslandTasksCompleted.Count(x => x.Value);
            grasslandProgressBar.Value = grasslandSteps > 0 ? (grasslandCompleted * 100) / grasslandSteps : 0;
            grasslandStepLabel.Text = $"已完成 {grasslandCompleted}/{grasslandSteps} 步骤";

            // 更新湿地工作流进度
            int wetlandSteps = wetlandTasksCompleted.Count;
            int wetlandCompleted = wetlandTasksCompleted.Count(x => x.Value);
            wetlandProgressBar.Value = wetlandSteps > 0 ? (wetlandCompleted * 100) / wetlandSteps : 0;
            wetlandStepLabel.Text = $"已完成 {wetlandCompleted}/{wetlandSteps} 步骤";

            // 更新输出工作流进度
            int outputSteps = outputTasksCompleted.Count;
            int outputCompleted = outputTasksCompleted.Count(x => x.Value);
            outputProgressBar.Value = outputSteps > 0 ? (outputCompleted * 100) / outputSteps : 0;
            outputStepLabel.Text = $"已完成 {outputCompleted}/{outputSteps} 步骤";

            // 更新总体进度
            int totalSteps = forestSteps + grasslandSteps + wetlandSteps + outputSteps;
            int totalCompleted = forestCompleted + grasslandCompleted + wetlandCompleted + outputCompleted;
            int overallPercentage = totalSteps > 0 ? (totalCompleted * 100) / totalSteps : 0;

            // 更新森林资源图表
            UpdateForestResourceChart();
        }

        private void UpdateForestResourceChart()
        {
            // 模拟数据 - 在实际应用中应替换为真实数据
            forestResourceChart.Series.Clear();
            forestResourceChart.Titles.Clear();

            forestResourceChart.Titles.Add("森林资源清查进度");
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("森林资源");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            if (forestTasksCompleted["extractScope"])
                series.Points.AddXY("提取范围", 20);
            if (forestTasksCompleted["createBasemapLink"])
                series.Points.AddXY("底图与价格关联", 15);
            if (forestTasksCompleted["supplementPrice"])
                series.Points.AddXY("补充价格", 15);
            if (forestTasksCompleted["calculateValue"])
                series.Points.AddXY("价值计算", 20);
            if (forestTasksCompleted["cleanQA"])
                series.Points.AddXY("数据清洗与质检", 15);
            if (forestTasksCompleted["buildDBTables"])
                series.Points.AddXY("构建库表", 15);

            if (series.Points.Count == 0)
                series.Points.AddXY("未开始", 100);

            forestResourceChart.Series.Add(series);
        }

        #region 事件处理程序

        // 森林资源处理程序
        private void BtnForestExtractScope_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在提取森林工作范围...");

            // 显示流程图
            forestWorkflowImage.Visible = true;

            // 待实现：实际逻辑
            forestTasksCompleted["extractScope"] = true;
            lblForestProcessingStatus.Text = "森林工作范围提取完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林工作范围提取完成");

            // 更新工作流状态和按钮状态
            UpdateWorkflowState();
            UpdateButtonStates();

            // 更新详细结果文本
            forestResultsTextBox.AppendText("=== 森林工作范围提取 ===\r\n");
            forestResultsTextBox.AppendText("- 基于林草湿荒普查数据提取范围\r\n");
            forestResultsTextBox.AppendText("- 包含国有林地及城镇开发边界内集体林地\r\n");
            forestResultsTextBox.AppendText("- 符合条件图斑数量: 283\r\n");
            forestResultsTextBox.AppendText("- 总面积: 1265.42公顷\r\n\r\n");
        }

        private void BtnForestCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在处理森林工作底图与价格关联...");

            // 待实现：实际逻辑
            forestTasksCompleted["createBasemapLink"] = true;
            lblForestProcessingStatus.Text = "森林底图与价格关联完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("森林底图与价格关联完成");

            // 更新工作流状态和按钮状态
            UpdateWorkflowState();
            UpdateButtonStates();

            // 更新详细结果文本
            forestResultsTextBox.AppendText("=== 森林底图与价格关联 ===\r\n");
            forestResultsTextBox.AppendText("- 工作范围与林地分等数据关联完成\r\n");
            forestResultsTextBox.AppendText("- 工作范围与林地定级数据关联完成\r\n");
            forestResultsTextBox.AppendText("- 基准地价与图斑空间挂接完成\r\n");
            forestResultsTextBox.AppendText("- 关联成功: 256图斑\r\n");
            forestResultsTextBox.AppendText("- 关联失败: 27图斑\r\n\r\n");
        }

        private void BtnForestSupplementPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在补充森林基准价格...");

            // 待实现：实际逻辑
            forestTasksCompleted["supplementPrice"] = true;
            lblForestProcessingStatus.Text = "森林基准价格补充完成";
            lblForestProcessingStatus.ForeColor = Color.DarkGreen; // 保持 DarkGreen 表示成功
            UpdateProgress("森林基准价格补充完成");

            // 更新工作流状态和按钮状态
            UpdateWorkflowState();
            UpdateButtonStates();

            // 更新详细结果文本
            forestResultsTextBox.AppendText("=== 森林基准价格补充 ===\r\n");
            forestResultsTextBox.AppendText("- 提取林地定级指标及权重信息\r\n");
            forestResultsTextBox.AppendText("- 提取基准价格信息\r\n");
            forestResultsTextBox.AppendText("- 补充落空图斑价格\r\n");
            forestResultsTextBox.AppendText("- 补充成功: 27图斑\r\n");
            forestResultsTextBox.AppendText("- 基准价格范围: 2.45-8.72万元/公顷\r\n\r\n");
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
            forestResultsTextBox.AppendText("=== 森林资产价值计算 ===\r\n");
            forestResultsTextBox.AppendText("- 应用基准价格修正因子和权重\r\n");
            forestResultsTextBox.AppendText("- 计算宗地价格\r\n");
            forestResultsTextBox.AppendText("- 应用期日修正和年期修正\r\n");
            forestResultsTextBox.AppendText("- 总价值: 10652896元\r\n");
            forestResultsTextBox.AppendText("- 平均单价: 8.42万元/公顷\r\n\r\n");
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
            forestResultsTextBox.AppendText("=== 森林数据清洗与质检 ===\r\n");
            forestResultsTextBox.AppendText("- 清理多余字段和临时数据\r\n");
            forestResultsTextBox.AppendText("- 调整字段格式和数据精度\r\n");
            forestResultsTextBox.AppendText("- 检查数据有效性与完整性\r\n");
            forestResultsTextBox.AppendText("- 修复异常数据: 5条\r\n");
            forestResultsTextBox.AppendText("- 质检通过率: 98.2%\r\n\r\n");
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
            forestResultsTextBox.AppendText("=== 森林库表构建 ===\r\n");
            forestResultsTextBox.AppendText("- 提取清查成果原数据相关字段\r\n");
            forestResultsTextBox.AppendText("- 构建符合汇交规范的数据集\r\n");
            forestResultsTextBox.AppendText("- 生成基础数表和统计表\r\n");
            forestResultsTextBox.AppendText("- 创建数据表: 3个\r\n");
            forestResultsTextBox.AppendText("- 创建统计表: 2个\r\n\r\n");
        }

        // 草地资源处理程序
        private void BtnGrasslandExtractScope_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在提取草地工作范围...");
            grasslandTasksCompleted["extractScope"] = true;
            lblGrasslandProcessingStatus.Text = "草地工作范围提取完成";
            lblGrasslandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("草地工作范围提取完成");
            UpdateWorkflowState();
            UpdateButtonStates();
        }

        private void BtnGrasslandCreateBasemapLinkPrice_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在处理草地工作底图与价格关联...");
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

        // 湿地资源处理程序
        private void BtnWetlandExtractScopeBasemap_Click(object sender, EventArgs e)
        {
            UpdateProgress("正在制作湿地工作范围与底图...");
            wetlandTasksCompleted["extractScopeBasemap"] = true;
            lblWetlandProcessingStatus.Text = "湿地工作范围与底图制作完成";
            lblWetlandProcessingStatus.ForeColor = Color.DarkGreen;
            UpdateProgress("湿地工作范围与底图制作完成");
            UpdateWorkflowState();
            UpdateButtonStates();
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
            // 复制自原始 MainProcessForm
            string helpText = @"广东省全民所有自然资源（森林、草地、湿地）资产清查工具使用说明

基础数据准备 (通过 '基础数据准备' 窗口完成)
1. 选择工作空间：选择用于存放清查数据的File Geodatabase (.gdb)。
2. 选择县区：选择需要进行清查的一个或多个县区。
3. 加载前提数据：加载必要的背景数据。
4. 创建县级空表：为选定县区创建数据表结构。

--- 以下操作在本窗口进行 ---

森林资源资产清查
1. 提取森林工作范围：根据普查数据和开发边界，筛选国有林地及边界内集体林地。
2. 森林底图与价格关联：将工作范围与林地分等数据关联，并挂接基准地价。
3. 补充森林基准价格：针对挂接失败的图斑，补充运算确保基准价格无缺漏。
4. 计算森林资产价值：应用修正因子计算宗地价格，进行期日和年期修正得到核算价格。
5. 森林数据清洗与质检：进行数据清洗和质量检查，确保符合技术规范。
6. 森林库表构建：构建符合汇交规范的数据集、基础数表和统计表。

草地资源资产清查流程类似于森林资源资产清查。
湿地资源资产清查仅需进行实物量清查，不进行价值量核算。

综合质检、统计与成果输出
1. 综合质量检查：对森林、草地、湿地清查数据进行全面质检。
2. 数据统计汇总：对清查结果进行统计汇总。
3. 数据分析与挖掘：进行深入数据分析，挖掘数据规律和信息。
4. 导出清查数据集：导出符合汇交规范的清查数据集。
5. 导出汇总表：导出各类汇总表。
6. 生成成果报告：生成工作总结报告和数据自检报告。
7. 生成专题图：生成各类专题图。

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
            // 确保在 InitializeComponent 完全完成前调用此方法时，btnHelp 和 btnClose 不为 null
            if (btnHelp != null && btnClose != null && bottomPanel != null)
            {
                // 调整按钮相对于 bottomPanel 右边缘的位置
                btnClose.Location = new Point(bottomPanel.Width - btnClose.Width - 10, (bottomPanel.Height - btnClose.Height) / 2);
                btnHelp.Location = new Point(btnClose.Location.X - btnHelp.Width - 5, (bottomPanel.Height - btnHelp.Height) / 2);
            }
        }
        #endregion

        private void ShowForestWorkflowDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            forestDetailPanel.Visible = !forestDetailPanel.Visible;
            forestWorkflowExplanation.Visible = forestDetailPanel.Visible;
            forestResultsTextBox.Visible = forestDetailPanel.Visible;
            forestResourceChart.Visible = forestDetailPanel.Visible;
            forestWorkflowImage.Visible = forestDetailPanel.Visible;

            showForestWorkflowDetails.Text = forestDetailPanel.Visible ?
                "隐藏详细流程说明 ▲" : "显示详细流程说明 ▼";
        }
    }
}