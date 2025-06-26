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
            this.Caption = "������Ԥ������";
            this.Tooltip = "�򿪹㶫ʡȫ��������Ȼ��Դ��ɭ�֡��ݵء�ʪ�أ��ʲ���鹤��";
            this.Message = "���ڴ���Ȼ��Դ�ʲ���鹤��...";
        }

        protected override void OnClick()
        {
            //if (!SharedWorkflowState.IsBasicDataPrepared)
            //{
            //    MessageBox.Show("���ȴ򿪡���������׼�������ڲ�����������á�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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