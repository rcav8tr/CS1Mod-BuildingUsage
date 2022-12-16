using System;
using System.Reflection;

namespace BuildingUsage
{
    /// <summary>
    /// Harmony patching for LevelsInfoViewPanel
    /// </summary>
    public class LevelsInfoViewPanelPatch
    {
        // whether or not Levels Info View Panel has been sized
        private static bool _levelsInfoViewPanelSized = false;

        // whether or not usage counts have been initialized
        private static bool _usageCountsInitialized = false;

        /// <summary>
        /// create patch for LevelsInfoViewPanel.UpdatePanel
        /// </summary>
        public static bool CreateUpdatePanelPatch()
        {
            // initialize flags
            _levelsInfoViewPanelSized = false;
            _usageCountsInitialized = false;

            // patch with the prefix routine
            return HarmonyPatcher.CreatePrefixPatch(typeof(LevelsInfoViewPanel), "UpdatePanel", BindingFlags.Instance | BindingFlags.NonPublic, typeof(LevelsInfoViewPanelPatch), "LevelsInfoViewPanelUpdatePanel");
        }

        /// <summary>
        /// update only the currently displayed panel
        /// </summary>
        /// <returns>whether or not to do base processing</returns>
        public static bool LevelsInfoViewPanelUpdatePanel()
        {
            // check if Levels Info View panel was not sized
            if (!_levelsInfoViewPanelSized)
            {
                // get the LevelsInfoViewPanel
                LevelsInfoViewPanel levelsPanel = ColossalFramework.UI.UIView.library.Get<LevelsInfoViewPanel>(typeof(LevelsInfoViewPanel).Name);
                if (levelsPanel == null)
                {
                    LogUtil.LogError("Unable to find LevelsInfoViewPanel.");
                    return true;
                }
                else
                {
                    // get the bottom position of the bottom-most UI element from among the 5 panels
                    float bottomPosition =
                        Math.Max(BuildingUsage.levelsDetailPanel.BottomPosition,
                        Math.Max(BuildingUsage.workersUsagePanel.BottomPosition,
                        Math.Max(BuildingUsage.visitorsUsagePanel.BottomPosition,
                        Math.Max(BuildingUsage.storageUsagePanel.BottomPosition,
                                 BuildingUsage.vehiclesUsagePanel.BottomPosition))));

                    // set the height of the Levels Info View panel
                    levelsPanel.component.size = new UnityEngine.Vector2(levelsPanel.component.size.x, bottomPosition + 8f);
                    _levelsInfoViewPanelSized = true;
                }
            }

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
                    BuildingUsage.levelsDetailPanel.UpdatePanel();
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
