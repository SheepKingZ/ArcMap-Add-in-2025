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
        }

        protected override void OnClick()
        {
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