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
            this.Caption = "�ղ����ݺ˶Դ���";
            this.Tooltip = "���ղ����ݵ���˶���༭���棬ͨ�����������ղ����ݽ��к˶Դ���";
            this.Message = "���ڴ��ղ����ݺ˶Դ�����...";
        }

        protected override void OnClick()
        {
            ArcMap.Application.CurrentTool = null; // ͣ���κλ����
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