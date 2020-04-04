using ICities;

namespace BuildingUsage
{
    public class BuildingUsageBuilding : BuildingExtensionBase
    {
        public override void OnBuildingReleased(ushort id)
        {
            // remove the building from the usage counts
            // a building can be in usage counts on more than one panel, so must check all panels
            BuildingUsage.workersUsagePanel.RemoveUsageCount(id);
            BuildingUsage.visitorsUsagePanel.RemoveUsageCount(id);
            BuildingUsage.storageUsagePanel.RemoveUsageCount(id);
            BuildingUsage.vehiclesUsagePanel.RemoveUsageCount(id);

            // do base processing
            base.OnBuildingReleased(id);
        }
    }
}
