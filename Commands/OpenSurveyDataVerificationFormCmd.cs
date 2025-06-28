using ESRI.ArcGIS.Desktop.AddIns;
using System.Windows.Forms;
using TestArcMapAddin2.Forms;
using Button = ESRI.ArcGIS.Desktop.AddIns.Button;

namespace TestArcMapAddin2.Commands
{
    public class OpenSurveyDataVerificationFormCmd : Button
    {
        public OpenSurveyDataVerificationFormCmd()
        {
            this.Caption = "普查数据核对处理";
            this.Tooltip = "打开普查数据地类核对与编辑界面，通过地类编码对普查数据进行核对处理";
            this.Message = "正在打开普查数据核对处理工具...";
        }

        protected override void OnClick()
        {
            ArcMap.Application.CurrentTool = null; // 停用任何活动工具
            SurveyDataVerificationForm verificationForm = new SurveyDataVerificationForm();
            verificationForm.ShowDialog(); // Show as modal dialog
            ArcMap.Application.CurrentTool = null;
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}