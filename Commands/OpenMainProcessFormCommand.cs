using ESRI.ArcGIS.Desktop.AddIns;
using System;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // ����MainProcessForm���ڵ������ռ�

namespace TestArcMapAddin2.Commands
{
    public class OpenMainProcessFormCommand : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public OpenMainProcessFormCommand()
        {
            // ע�⣺�������ԻᱻConfig.esriaddinx�е����ø���
            this.Caption = "���ʲ���鹤��"; 
            this.Tooltip = "�򿪹㶫ʡȫ��������Ȼ��Դ��ɭ�֡��ݵء�ʪ�أ��ʲ���鹤��";
            this.Message = "���ڴ���Ȼ��Դ�ʲ���鹤��...";
            // ͼ��·����Config.esriaddinx���Ѿ�����
        }

        protected override void OnClick()
        {
            try
            {
                ArcMap.Application.CurrentTool = null; // ͣ���κλ����

                // ��������ʾ��������
                MainProcessForm form = new MainProcessForm();
                form.ShowDialog(new ArcMapWindow(ArcMap.Application.hWnd));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�򿪴���ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnUpdate()
        {
            // ֻ�е�ArcMapӦ�ó������ʱ�����ô˰�ť
            Enabled = ArcMap.Application != null;
        }
    }

    // �����࣬������ȷ�ؽ�Windows������ArcMap����
    internal class ArcMapWindow : IWin32Window
    {
        private readonly int _hwnd;

        public ArcMapWindow(int hwnd)
        {
            _hwnd = hwnd;
        }

        public IntPtr Handle => (IntPtr)_hwnd;
    }
}