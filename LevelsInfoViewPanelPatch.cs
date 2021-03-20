using System.Reflection;

namespace BuildingUsage
{
    /// <summary>
    /// Harmony patching for LevelsInfoViewPanel
    /// </summary>
    public class LevelsInfoViewPanelPatch
    {
        // whether or not usage counts have been initialized
        private static bool _usageCountsInitialized = false;

        /// <summary>
        /// create patch for LevelsInfoViewPanel.UpdatePanel
        /// </summary>
        public static bool CreateUpdatePanelPatch()
        {
            // not initialized
            _usageCountsInitialized = false;

            // patch with the prefix routine
            return HarmonyPatcher.CreatePrefixPatch<LevelsInfoViewPanel>("UpdatePanel", BindingFlags.Instance | BindingFlags.NonPublic, typeof(LevelsInfoViewPanelPatch), "LevelsInfoViewPanelUpdatePanel");
        }

        /// <summary>
        /// update only the currently displayed panel
        /// </summary>
        /// <returns>whether or not to do base processing</returns>
        public static bool LevelsInfoViewPanelUpdatePanel()
        {
            // check which tab is selected
            bool doBaseProcessing = true;
            switch (BuildingUsage.selectedTab)
            {
                case BuildingUsage.LevelsInfoViewTab.Levels:
                    // when the Levels info view is first displayed, initialize the usage counts for all the panels
                    if (!_usageCountsInitialized)
                    {
                        BuildingUsage.workersUsagePanel.InitializeBuildingUsageCounts();
                        BuildingUsage.visitorsUsagePanel.InitializeBuildingUsageCounts();
                        BuildingUsage.storageUsagePanel.InitializeBuildingUsageCounts();
                        BuildingUsage.vehiclesUsagePanel.InitializeBuildingUsageCounts();
                        _usageCountsInitialized = true;
                    }
                    doBaseProcessing = true;
                    break;

                case BuildingUsage.LevelsInfoViewTab.Workers:
                    doBaseProcessing = false;
                    BuildingUsage.workersUsagePanel.UpdatePanel();
                    break;

                case BuildingUsage.LevelsInfoViewTab.Visitors:
                    doBaseProcessing = false;
                    BuildingUsage.visitorsUsagePanel.UpdatePanel();
                    break;

                case BuildingUsage.LevelsInfoViewTab.Storage:
                    doBaseProcessing = false;
                    BuildingUsage.storageUsagePanel.UpdatePanel();
                    break;

                case BuildingUsage.LevelsInfoViewTab.Vehicles:
                    doBaseProcessing = false;
                    BuildingUsage.vehiclesUsagePanel.UpdatePanel();
                    break;
            }

            // return whether or not to do the base processing
            return doBaseProcessing;
        }
    }
}
