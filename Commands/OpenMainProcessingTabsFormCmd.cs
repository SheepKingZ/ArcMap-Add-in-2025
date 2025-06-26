using ESRI.ArcGIS.Desktop.AddIns;
using System.Windows.Forms;
using TestArcMapAddin2.Forms;
using Button = ESRI.ArcGIS.Desktop.AddIns.Button;

namespace TestArcMapAddin2.Commands
{
    public class OpenMainProcessingTabsFormCmd : Button
    {
        public OpenMainProcessingTabsFormCmd()
        {
            this.Caption = "打开数据预处理工具";
            this.Tooltip = "打开广东省全民所有自然资源（森林、草地、湿地）资产清查工具";
            this.Message = "正在打开自然资源资产清查工具...";
        }

        protected override void OnClick()
        {
            //if (!SharedWorkflowState.IsBasicDataPrepared)
            //{
            //    MessageBox.Show("请先打开“基础数据准备”窗口并完成所有设置。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    // Optionally, open the basic data form automatically:
            //    // BasicDataPreparationForm basicForm = new BasicDataPreparationForm();
            //    // if (basicForm.ShowDialog() != DialogResult.OK) return; // User cancelled or didn't complete
            //}

            // Proceed to open the main form if data is prepared (or if you allow opening it regardless)
            MainProcessingTabsForm mainForm = new MainProcessingTabsForm();
            mainForm.Show(); // Show non-modally or use ShowDialog() if preferred

            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}