using ColossalFramework;
using UnityEngine;
using System;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display storage usage
    /// </summary>
    public class StorageUsagePanel : UsagePanel
    {
        /// <summary>
        /// Start is called once after the panel is created
        /// set up and populate the panel with UI components
        /// </summary>
        public override void Start()
        {
            // do base processing
            base.Start();

            try
            {
                // set the panel name
                name = GetType().ToString();

                // create the usage groups
                // StorageSnow should only be included only for a winter map
                if (Singleton<LoadingManager>.instance.m_loadedEnvironment == "Winter")
                {
                    CreateUsageGroup<SnowDumpAI                             >(UsageType.StorageSnow);
                }
                // WaterFacilityAI is in the base game, but buildings for StorageWater are introduced in NaturalDisastersDLC
                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.NaturalDisastersDLC))
                {
                    CreateUsageGroup<WaterFacilityAI                        >(UsageType.StorageWater);
                }
                CreateUsageGroup<LandfillSiteAI, UltimateRecyclingPlantAI   >(UsageType.StorageGarbage);
                CreateUsageGroup<ExtractingFacilityAI, FishingHarborAI, FishFarmAI, ProcessingFacilityAI, UniqueFactoryAI, WarehouseAI>(UsageType.StorageIndustry);
                CreateUsageGroup<PostOfficeAI                               >(UsageType.StoragePostUnsorted);
                CreateUsageGroup<PostOfficeAI                               >(UsageType.StoragePostSorted);

                // add detail panel
                AddDetailPanel<StorageIndustryUsagePanel>(UsageType.StorageIndustry, this);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<SnowDumpAI              >(UsageType.StorageSnow,            (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageSnowDump                              (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<WaterFacilityAI         >(UsageType.UseLogic1,              (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageWaterFacility                         (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<LandfillSiteAI          >(UsageType.StorageGarbage,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageLandfillSite<LandfillSiteAI>          (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<UltimateRecyclingPlantAI>(UsageType.StorageGarbage,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageLandfillSite<UltimateRecyclingPlantAI>(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ExtractingFacilityAI    >(UsageType.StorageIndustry,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageExtractingFacility                    (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<FishingHarborAI         >(UsageType.StorageIndustry,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageFishingHarbor                         (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<FishFarmAI              >(UsageType.StorageIndustry,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageFishFarm                              (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ProcessingFacilityAI    >(UsageType.StorageIndustry,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageProcessingFacilityTotal               (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<UniqueFactoryAI         >(UsageType.StorageIndustry,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageUniqueFactoryTotal                    (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<WarehouseAI             >(UsageType.StorageIndustry,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStorageWarehouse                             (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<PostOfficeAI            >(UsageType.StoragePostUnsorted,    (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStoragePostOfficeUnsorted                    (buildingID, ref data, ref used, ref allowed),
                                                              UsageType.StoragePostSorted,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountStoragePostOfficeSorted                      (buildingID, ref data, ref used, ref allowed));

                // set mutually exclusive check boxes
                MakeCheckBoxesMutuallyExclusive(UsageType.StoragePostUnsorted, UsageType.StoragePostSorted);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// get the usage type for a building when logic 1 is required
        /// </summary>
        protected override UsageType GetUsageType1ForBuilding(ushort buildingID, ref Building data)
        {
            // logic depends on building AI type
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            if (buildingAIType == typeof(WaterFacilityAI))
            {
                // logic adapted from WaterFacilityAI.GetLocalizedStats
                WaterFacilityAI buildingAI = data.Info.m_buildingAI as WaterFacilityAI;
                if (buildingAI.m_waterIntake != 0 && buildingAI.m_waterOutlet != 0 && buildingAI.m_waterStorage != 0)
                {
                    // building is Tank Reservoir
                    return UsageType.StorageWater;
                }
                else if (buildingAI.m_sewageOutlet != 0 && buildingAI.m_sewageStorage != 0 && buildingAI.m_pumpingVehicles != 0)
                {
                    // building is Pumping Service
                    return UsageType.StorageWater;
                }
                else
                {
                    // other water building with no storage
                    return UsageType.None;
                }

            }

            // usage type not determined with above logic
            Debug.LogError($"Unhandled building AI type [{buildingAIType.ToString()}] when getting usage type with logic.");
            return UsageType.None;
        }

        /// <summary>
        /// get the usage type for a building when logic 2 is required
        /// </summary>
        protected override UsageType GetUsageType2ForBuilding(ushort buildingID, ref Building data)
        {
            // usage type not determined with above logic
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            Debug.LogError($"Unhandled building AI type [{buildingAIType.ToString()}] when getting usage type with logic.");
            return UsageType.None;
        }

        /// <summary>
        /// get the usage type for a vehicle when logic is required
        /// </summary>
        protected override UsageType GetUsageTypeForVehicle(ushort vehicleID, ref Vehicle data)
        {
            Type vehicleAIType = data.Info.m_vehicleAI.GetType();
            Debug.LogError($"Unhandled vehicle AI type [{vehicleAIType.ToString()}] when getting usage type with logic.");
            return UsageType.None;
        }

    }
}
