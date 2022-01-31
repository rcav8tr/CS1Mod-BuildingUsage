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
                if (LoadingManager.instance.m_loadedEnvironment == "Winter")
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
                AddDetailPanel<StorageIndustryUsagePanel>(UsageType.StorageIndustry);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<SnowDumpAI              >(UsageType.StorageSnow,            GetUsageCountStorageSnowDump                              );
                AssociateBuildingAI<WaterFacilityAI         >(UsageType.UseLogic1,              GetUsageCountStorageWaterFacility                         );
                AssociateBuildingAI<LandfillSiteAI          >(UsageType.StorageGarbage,         GetUsageCountStorageLandfillSite<LandfillSiteAI>          );
                AssociateBuildingAI<UltimateRecyclingPlantAI>(UsageType.StorageGarbage,         GetUsageCountStorageLandfillSite<UltimateRecyclingPlantAI>);
                AssociateBuildingAI<ExtractingFacilityAI    >(UsageType.StorageIndustry,        GetUsageCountStorageExtractingFacility                    );
                AssociateBuildingAI<FishingHarborAI         >(UsageType.StorageIndustry,        GetUsageCountStorageFishingHarbor                         );
                AssociateBuildingAI<FishFarmAI              >(UsageType.StorageIndustry,        GetUsageCountStorageFishFarm                              );
                AssociateBuildingAI<ProcessingFacilityAI    >(UsageType.StorageIndustry,        GetUsageCountStorageProcessingFacilityTotal               );
                AssociateBuildingAI<UniqueFactoryAI         >(UsageType.StorageIndustry,        GetUsageCountStorageUniqueFactoryTotal                    );
                AssociateBuildingAI<WarehouseAI             >(UsageType.StorageIndustry,        GetUsageCountStorageWarehouse                             );
                AssociateBuildingAI<PostOfficeAI            >(UsageType.StoragePostUnsorted,    GetUsageCountStoragePostOfficeUnsorted                    ,
                                                              UsageType.StoragePostSorted,      GetUsageCountStoragePostOfficeSorted                      );

                // set mutually exclusive check boxes
                MakeCheckBoxesMutuallyExclusive(UsageType.StoragePostUnsorted, UsageType.StoragePostSorted);
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
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
            LogUtil.LogError($"Unhandled building AI type [{buildingAIType}] when getting usage type with logic.");
            return UsageType.None;
        }

        /// <summary>
        /// get the usage type for a building when logic 2 is required
        /// </summary>
        protected override UsageType GetUsageType2ForBuilding(ushort buildingID, ref Building data)
        {
            // usage type not determined with above logic
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            LogUtil.LogError($"Unhandled building AI type [{buildingAIType}] when getting usage type with logic.");
            return UsageType.None;
        }

        /// <summary>
        /// get the usage type for a vehicle when logic is required
        /// </summary>
        protected override UsageType GetUsageTypeForVehicle(ushort vehicleID, ref Vehicle data)
        {
            Type vehicleAIType = data.Info.m_vehicleAI.GetType();
            LogUtil.LogError($"Unhandled vehicle AI type [{vehicleAIType}] when getting usage type with logic.");
            return UsageType.None;
        }

    }
}
