using ColossalFramework.UI;
using Harmony;
using ICities;

namespace BuildingUsage
{
    public class BuildingUsage : IUserMod
    {
        // required name and description of this mod
        public string Name => "Building Usage";
        public string Description => "Display how much a building is being used as a percent of its capacity";

        // Harmony instance
        public static HarmonyInstance harmony;

        // keep track of which tab is selected on the tab strip
        // cannot use visibility of the usage panels because the Levels info view window can be closed (i.e. usage panel is invisible)
        // but the game remains in the Levels info mode where the buildings are still colored
        // the indexes of the tabs must match the values used by this enum
        // detail panels are not included here because they don't have their own tab
        public enum LevelsInfoViewTab
        {
            Levels,
            Workers,
            Visitors,
            Storage,
            Vehicles
        }
        public static LevelsInfoViewTab selectedTab;

        // instances of main display objects
        public static UITabstrip tabStrip = null;
        public static WorkersUsagePanel  workersUsagePanel  = null;
        public static VisitorsUsagePanel visitorsUsagePanel = null;
        public static StorageUsagePanel  storageUsagePanel  = null;
        public static VehiclesUsagePanel vehiclesUsagePanel = null;
    }
}