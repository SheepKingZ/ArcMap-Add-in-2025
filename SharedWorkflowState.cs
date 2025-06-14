using System.Collections.Generic;

namespace TestArcMapAddin2
{
    public static class SharedWorkflowState
    {
        public static string WorkspacePath { get; set; }
        public static List<string> SelectedCounties { get; set; }
        public static bool IsBasicDataPrepared { get; set; } = false;

        // Flags to track completion of prerequisite data loading and empty table creation
        public static bool PrerequisiteDataLoaded { get; set; } = false;
        public static bool CountyEmptyTablesCreated { get; set; } = false;

        static SharedWorkflowState()
        {
            SelectedCounties = new List<string>();
        }

        public static void ResetBasicDataFlags()
        {
            PrerequisiteDataLoaded = false;
            CountyEmptyTablesCreated = false;
            // Do not reset WorkspacePath and SelectedCounties here unless intended
            // IsBasicDataPrepared should be set based on successful completion of these steps
        }
    }
}