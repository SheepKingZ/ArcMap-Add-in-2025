using System;

namespace TestArcMapAddin2
{
    /// <summary>
    /// 用于在不同窗体之间共享工作流状态的静态类
    /// </summary>
    public static class SharedWorkflowState
    {
        /// <summary>
        /// 工作空间路径
        /// </summary>
        public static string WorkspacePath { get; set; }

        /// <summary>
        /// 合并后的数据源路径，包含林草湿荒普查与城镇开发边界数据
        /// </summary>
        public static string DataSourcePath { get; set; }

        /// <summary>
        /// 输出GDB路径，用于在基础数据准备和基础处理表单之间共享
        /// </summary>
        public static string OutputGDBPath { get; set; }

        /// <summary>
        /// 当前选择的县
        /// </summary>
        public static string SelectedCounty { get; set; }

        /// <summary>
        /// 清理所有状态
        /// </summary>
        public static void Clear()
        {
            WorkspacePath = null;
            DataSourcePath = null;
            OutputGDBPath = null;
            SelectedCounty = null;
        }
    }
}