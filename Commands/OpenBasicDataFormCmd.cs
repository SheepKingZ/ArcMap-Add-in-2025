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
            // ע�⣺�������ԻᱻConfig.esriaddinx�е����ø���
            this.Caption = "������Ԥ������";
            this.Tooltip = "�򿪹㶫ʡȫ��������Ȼ��Դ��ɭ�֡��ݵء�ʪ�أ��ʲ���鹤��";
            this.Message = "���ڴ���Ȼ��Դ�ʲ���鹤��...";
            // ͼ��·����Config.esriaddinx���Ѿ�����
        }

        protected override void OnClick()
        {
            ArcMap.Application.CurrentTool = null; // ͣ���κλ����
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