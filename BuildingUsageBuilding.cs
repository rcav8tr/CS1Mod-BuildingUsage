using ICities;

namespace BuildingUsage
{
    public class BuildingUsageBuilding : BuildingExtensionBase
    {
        public override void OnBuildingReleased(ushort id)
        {
            // remove the building from the usage counts
            // a building can be in usage counts on more than one panel, so must check all panels
            if (BuildingUsage.workersUsagePanel  != null) BuildingUsage.workersUsagePanel.RemoveUsageCount(id);
            if (BuildingUsage.visitorsUsagePanel != null) BuildingUsage.visitorsUsagePanel.RemoveUsageCount(id);
            if (BuildingUsage.storageUsagePanel  != null) BuildingUsage.storageUsagePanel.RemoveUsageCount(id);
            if (BuildingUsage.vehiclesUsagePanel != null) BuildingUsage.vehiclesUsagePanel.RemoveUsageCount(id);

            // do base processing
            base.OnBuildingReleased(id);
        }
    }
}
