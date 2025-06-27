// 这个文件位于 Utilities/SharedWorkflowState.cs
using System.Collections.Generic;

public static class SharedWorkflowState
{
    // 现有属性
    public static string WorkspacePath { get; set; }
    public static List<string> SelectedCounties { get; set; }
    //public static bool IsBasicDataPrepared { get; set; }
    public static bool CountyEmptyTablesCreated { get; set; }

    // 以前的单个属性
    // public static bool PrerequisiteDataLoaded { get; set; }

    // 新属性，替换PrerequisiteDataLoaded
    public static string PrerequisiteData1Path { get; set; }
    public static string PrerequisiteData2Path { get; set; }

    // 为了向后兼容，可以保留这个属性定义
    public static bool PrerequisiteDataLoaded
    {
        get { return !string.IsNullOrEmpty(PrerequisiteData1Path) && !string.IsNullOrEmpty(PrerequisiteData2Path); }
    }

    public static void ResetBasicDataFlags()
    {
        PrerequisiteData1Path = "";
        PrerequisiteData2Path = "";
        CountyEmptyTablesCreated = false;
        //IsBasicDataPrepared = false;
    }
}