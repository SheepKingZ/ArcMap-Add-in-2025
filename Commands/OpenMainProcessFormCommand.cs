using ESRI.ArcGIS.Desktop.AddIns;
using System;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // 引用MainProcessForm所在的命名空间

namespace TestArcMapAddin2.Commands
{
    public class OpenMainProcessFormCommand : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public OpenMainProcessFormCommand()
        {
            // 注意：以下属性会被Config.esriaddinx中的配置覆盖
            this.Caption = "打开资产清查工具"; 
            this.Tooltip = "打开广东省全民所有自然资源（森林、草地、湿地）资产清查工具";
            this.Message = "正在打开自然资源资产清查工具...";
            // 图标路径在Config.esriaddinx中已经设置
        }

        protected override void OnClick()
        {
            try
            {
                ArcMap.Application.CurrentTool = null; // 停用任何活动工具

                // 创建并显示主处理窗体
                MainProcessForm form = new MainProcessForm();
                form.ShowDialog(new ArcMapWindow(ArcMap.Application.hWnd));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnUpdate()
        {
            // 只有当ArcMap应用程序可用时才启用此按钮
            Enabled = ArcMap.Application != null;
        }
    }

    // 帮助类，用于正确地将Windows窗体与ArcMap关联
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