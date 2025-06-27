// ����ļ�λ�� Utilities/SharedWorkflowState.cs
using System.Collections.Generic;

public static class SharedWorkflowState
{
    // ��������
    public static string WorkspacePath { get; set; }
    public static List<string> SelectedCounties { get; set; }
    //public static bool IsBasicDataPrepared { get; set; }
    public static bool CountyEmptyTablesCreated { get; set; }

    // ��ǰ�ĵ�������
    // public static bool PrerequisiteDataLoaded { get; set; }

    // �����ԣ��滻PrerequisiteDataLoaded
    public static string PrerequisiteData1Path { get; set; }
    public static string PrerequisiteData2Path { get; set; }

    // Ϊ�������ݣ����Ա���������Զ���
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