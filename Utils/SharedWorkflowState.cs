using System;

namespace TestArcMapAddin2
{
    /// <summary>
    /// �����ڲ�ͬ����֮�乲������״̬�ľ�̬��
    /// </summary>
    public static class SharedWorkflowState
    {
        /// <summary>
        /// �����ռ�·��
        /// </summary>
        public static string WorkspacePath { get; set; }

        /// <summary>
        /// ���GDB·���������ڻ�������׼���ͻ��������֮�乲��
        /// </summary>
        public static string OutputGDBPath { get; set; }

        /// <summary>
        /// ��ǰѡ�����
        /// </summary>
        public static string SelectedCounty { get; set; }

        /// <summary>
        /// ��������״̬
        /// </summary>
        public static void Clear()
        {
            WorkspacePath = null;
            OutputGDBPath = null;
            SelectedCounty = null;
        }
    }
}