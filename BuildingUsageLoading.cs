using ColossalFramework.UI;
using ColossalFramework;
using ICities;
using UnityEngine;
using Harmony;
using System;

namespace BuildingUsage
{
    /// <summary>
    /// handle game loading and unloading
    /// </summary>
    /// <remarks>A new instance of BuildingUsageLoading is NOT created when loading a game from the Pause Menu.</remarks>
    public class BuildingUsageLoading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            // do base processing
            base.OnLevelLoaded(mode);

            try
            {
                // check for new or loaded game
                if (mode == LoadMode.NewGame || mode == LoadMode.NewGameFromScenario || mode == LoadMode.LoadGame)
                {
                    // initialize Harmony
                    BuildingUsage.harmony = HarmonyInstance.Create("com.github.rcav8tr.BuildingUsage");
                    if (BuildingUsage.harmony == null)
                    {
                        Debug.LogError("Unable to create Harmony instance.");
                        return;
                    }

                    // get the LevelsInfoViewPanel panel (displayed when the user clicks on the Levels info view button)
                    LevelsInfoViewPanel levelsPanel = UIView.library.Get<LevelsInfoViewPanel>(typeof(LevelsInfoViewPanel).Name);
                    if (levelsPanel == null)
                    {
                        Debug.LogError("Unable to find LevelsInfoViewPanel.");
                        return;
                    }

                    // remove the bottom anchor from the legend
                    foreach (UIComponent comp in levelsPanel.component.components)
                    {
                        if (comp.name == "Legend")
                        {
                            comp.anchor &= ~UIAnchorStyle.Bottom;
                            break;
                        }
                    }

                    // resize the Levels panel taller to make room for the tab strip and more info
                    const float TabStripHeight = 26f;
                    const float TabStripSpace = TabStripHeight + 8f;
                    levelsPanel.component.size = new Vector2(levelsPanel.component.size.x, levelsPanel.component.size.y + TabStripSpace + 200f);

                    // move existing panels and legend down to make room for the tab strip
                    foreach (UIComponent comp in levelsPanel.component.components)
                    {
                        if (comp.name == "Panel" || comp.name == "Legend")
                        {
                            comp.relativePosition = new Vector3(comp.relativePosition.x, comp.relativePosition.y + TabStripSpace);
                        }
                    }

                    // add a tab strip to the panel
                    UITabstrip tabStrip = levelsPanel.component.AddUIComponent<UITabstrip>();
                    if (tabStrip == null)
                    {
                        Debug.LogError("Unable to create tab strip.");
                        return;
                    }
                    tabStrip.name = "Tabstrip";
                    tabStrip.relativePosition = new Vector3(8f, 50f);
                    tabStrip.size = new Vector2(levelsPanel.component.width - 16f, TabStripHeight);
                    tabStrip.eventSelectedIndexChanged += TabStrip_eventSelectedIndexChanged;
                    BuildingUsage.tabStrip = tabStrip;

                    // add tabs to the tab strip
                    // the order the tabs are added determines the index of each tab starting at zero, which matches the values in LevelsInfoViewTab enum
                    if (!AddTab(tabStrip, "LevelsTab",        "Levels"  )) return;
                    if (!AddTab(tabStrip, "WorkersUsageTab",  "Workers" )) return;
                    if (!AddTab(tabStrip, "VisitorsUsageTab", "Visitors")) return;
                    if (!AddTab(tabStrip, "StorageUsageTab",  "Storage" )) return;
                    if (!AddTab(tabStrip, "VehiclesUsageTab", "Vehicles")) return;

                    // Levels tab is selected by default
                    BuildingUsage.selectedTab = BuildingUsage.LevelsInfoViewTab.Levels;

                    // add the main usage panels to the Levels panel
                    BuildingUsage.workersUsagePanel  = UsagePanel.AddUsagePanel<WorkersUsagePanel >();
                    BuildingUsage.visitorsUsagePanel = UsagePanel.AddUsagePanel<VisitorsUsagePanel>();
                    BuildingUsage.storageUsagePanel  = UsagePanel.AddUsagePanel<StorageUsagePanel >();
                    BuildingUsage.vehiclesUsagePanel = UsagePanel.AddUsagePanel<VehiclesUsagePanel>();

                    // create the UpdatePanel patches
                    LevelsInfoViewPanelPatch.CreateUpdatePanelPatch();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Add a tab to the tab strip
        /// </summary>
        /// <returns>whether or not the tab was added</returns>
        private bool AddTab(UITabstrip tabStrip, string tabName, string tabText)
        {
            // add the tab
            UIButton tab = tabStrip.AddTab(tabText);
            if (tab == null)
            {
                Debug.LogError($"Unable to create [{tabText}] tab.");
                return false;
            }

            // these settings are consistent with tabs on other info view panels
            tab.name = tabName;
            tab.textScale = 0.6875f;
            tab.textHorizontalAlignment = UIHorizontalAlignment.Center;
            tab.textVerticalAlignment = UIVerticalAlignment.Middle;
            tab.textPadding = new RectOffset(1, 0, 4, 0);
            tab.size = new Vector2(tabStrip.width / Enum.GetValues(typeof(BuildingUsage.LevelsInfoViewTab)).Length, tabStrip.height);
            tab.disabledBgSprite = "GenericTabDisabled";
            tab.focusedBgSprite = "GenericTabFocused";
            tab.hoveredBgSprite = "GenericTabHovered";
            tab.pressedBgSprite = "GenericTabPressed";
            tab.normalBgSprite = "GenericTab";

            return true;
        }

        private void TabStrip_eventSelectedIndexChanged(UIComponent component, int value)
        {
            try
            {
                // save the newly selected tab
                // assumes that the indexes of the tabs match the values of LevelsInfoViewTab
                BuildingUsage.selectedTab = (BuildingUsage.LevelsInfoViewTab)value;

                // make all invisible, including detail panels
                SetLevelsVisibility(false);
                BuildingUsage.workersUsagePanel.HidePanel();
                BuildingUsage.visitorsUsagePanel.HidePanel();
                BuildingUsage.storageUsagePanel.HidePanel();
                BuildingUsage.vehiclesUsagePanel.HidePanel();

                // set visibility according to the selected tab
                switch (BuildingUsage.selectedTab) 
                {
                    case BuildingUsage.LevelsInfoViewTab.Levels:   SetLevelsVisibility(true);                    break;
                    case BuildingUsage.LevelsInfoViewTab.Workers:  BuildingUsage.workersUsagePanel.ShowPanel();  break;
                    case BuildingUsage.LevelsInfoViewTab.Visitors: BuildingUsage.visitorsUsagePanel.ShowPanel(); break;
                    case BuildingUsage.LevelsInfoViewTab.Storage:  BuildingUsage.storageUsagePanel.ShowPanel();  break;
                    case BuildingUsage.LevelsInfoViewTab.Vehicles: BuildingUsage.vehiclesUsagePanel.ShowPanel(); break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// set the visibility of the native components on the Levels info view
        /// </summary>
        private void SetLevelsVisibility(bool visible) 
        {
            // get the LevelsInfoViewPanel panel (displayed when the user clicks on the Levels info view button)
            LevelsInfoViewPanel levelsPanel = UIView.library.Get<LevelsInfoViewPanel>(typeof(LevelsInfoViewPanel).Name);
            if (levelsPanel == null)
            {
                Debug.LogError("Unable to find LevelsInfoViewPanel.");
                return;
            }

            // loop thru each component on the Levels panel
            foreach (UIComponent comp in levelsPanel.component.components)
            {
                // set the visiblity on the four "Panel"s and the "Legend"
                if (comp.name == "Panel" || comp.name == "Legend")
                {
                    comp.isVisible = visible;
                }
            }

            // update colors on all buildings
            if (visible)
            {
                Singleton<BuildingManager>.instance.UpdateBuildingColors();
            }
        }

        public override void OnLevelUnloading()
        {
            // do base processing
            base.OnLevelUnloading();

            try
            {
                // remove Harmony patches
                if (BuildingUsage.harmony != null)
                {
                    BuildingUsage.harmony.UnpatchAll();
                    BuildingUsage.harmony = null;
                }
                BuildingAIPatch.ClearPatches();
                VehicleAIPatch.ClearPatches();

                // destroy the objects added directly to the LevelsInfoViewPanel
                // must do this explicitly because loading a saved game from the Pause Menu
                // does not destroy the objects implicitly like returning to the Main Menu to load a saved game
                if (BuildingUsage.workersUsagePanel != null)
                {
                    UnityEngine.Object.Destroy(BuildingUsage.workersUsagePanel);
                    BuildingUsage.workersUsagePanel = null;
                }
                if (BuildingUsage.visitorsUsagePanel != null)
                {
                    UnityEngine.Object.Destroy(BuildingUsage.visitorsUsagePanel);
                    BuildingUsage.visitorsUsagePanel = null;
                }
                if (BuildingUsage.storageUsagePanel != null)
                {
                    UnityEngine.Object.Destroy(BuildingUsage.storageUsagePanel);
                    BuildingUsage.storageUsagePanel = null;
                }
                if (BuildingUsage.vehiclesUsagePanel != null)
                {
                    UnityEngine.Object.Destroy(BuildingUsage.vehiclesUsagePanel);
                    BuildingUsage.vehiclesUsagePanel = null;
                }
                if (BuildingUsage.tabStrip != null)
                {
                    BuildingUsage.tabStrip.eventSelectedIndexChanged -= TabStrip_eventSelectedIndexChanged;
                    UnityEngine.Object.Destroy(BuildingUsage.tabStrip);
                    BuildingUsage.tabStrip = null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

    }
}