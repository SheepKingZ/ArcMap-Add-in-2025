using ESRI.ArcGIS.Desktop.AddIns;
using System.Windows.Forms;
using TestArcMapAddin2.Forms;
using Button = ESRI.ArcGIS.Desktop.AddIns.Button;

namespace TestArcMapAddin2.Commands
{
    public class OpenBasicDataFormCmd : Button
    {
        public OpenBasicDataFormCmd()
        {
            // 注意：以下属性会被Config.esriaddinx中的配置覆盖
            this.Caption = "打开数据预处理工具";
            this.Tooltip = "打开广东省全民所有自然资源（森林、草地、湿地）资产清查工具";
            this.Message = "正在打开自然资源资产清查工具...";
            // 图标路径在Config.esriaddinx中已经设置
        }

        protected override void OnClick()
        {
            ArcMap.Application.CurrentTool = null; // 停用任何活动工具
            BasicDataPreparationForm basicForm = new BasicDataPreparationForm();
            basicForm.ShowDialog(); // Show as modal dialog
            // State is saved to SharedWorkflowState by the form itself upon OK.
            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}