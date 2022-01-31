using System;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display storage usage
    /// </summary>
    public class StorageIndustryUsagePanel : UsagePanel
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

                // create a button to return to main panel
                CreateReturnFromDetailButton("Return to Storage");

                // create the usage groups
                // ProcessingFacilityAI is used by both IndustryDLC and UrbanDLC (Sunset Harbor), so need to check for DLC and no need to check for building AI
                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC))
                {
                    CreateGroupHeading("Forestry Industry");
                    CreateUsageGroup(UsageType.StorageIndustryForestryExtractor);
                    CreateUsageGroup(UsageType.StorageIndustryForestryProcessorInput);
                    CreateUsageGroup(UsageType.StorageIndustryForestryProcessorOutput);
                    CreateUsageGroup(UsageType.StorageIndustryForestryStorage);

                    CreateGroupHeading("Farming Industry");
                    CreateUsageGroup(UsageType.StorageIndustryFarmingExtractor);
                    CreateUsageGroup(UsageType.StorageIndustryFarmingProcessorInput);
                    CreateUsageGroup(UsageType.StorageIndustryFarmingProcessorOutput);
                    CreateUsageGroup(UsageType.StorageIndustryFarmingStorage);

                    CreateGroupHeading("Ore Industry");
                    CreateUsageGroup(UsageType.StorageIndustryOreExtractor);
                    CreateUsageGroup(UsageType.StorageIndustryOreProcessorInput);
                    CreateUsageGroup(UsageType.StorageIndustryOreProcessorOutput);
                    CreateUsageGroup(UsageType.StorageIndustryOreStorage);

                    CreateGroupHeading("Oil Industry");
                    CreateUsageGroup(UsageType.StorageIndustryOilExtractor);
                    CreateUsageGroup(UsageType.StorageIndustryOilProcessorInput);
                    CreateUsageGroup(UsageType.StorageIndustryOilProcessorOutput);
                    CreateUsageGroup(UsageType.StorageIndustryOilStorage);
                }

                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.UrbanDLC))
                {
                    CreateGroupHeading("Fishing Industry");
                    CreateUsageGroup(UsageType.StorageIndustryFishingExtractor);
                    CreateUsageGroup(UsageType.StorageIndustryFishingProcessorInput);
                    CreateUsageGroup(UsageType.StorageIndustryFishingProcessorOutput);
                }

                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC))
                {
                    CreateGroupHeading("Unique Factory");
                    CreateUsageGroup(UsageType.StorageIndustryUniqueFactoryInput);
                    CreateUsageGroup(UsageType.StorageIndustryUniqueFactoryOutput);

                    CreateGroupHeading("Warehouse");
                    CreateUsageGroup(UsageType.StorageIndustryWarehouseGeneric);
                }

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<ExtractingFacilityAI    >(UsageType.UseLogic1,                          GetUsageCountStorageExtractingFacility      );
                AssociateBuildingAI<FishingHarborAI         >(UsageType.StorageIndustryFishingExtractor,    GetUsageCountStorageFishingHarbor           );
                AssociateBuildingAI<FishFarmAI              >(UsageType.StorageIndustryFishingExtractor,    GetUsageCountStorageFishFarm                );
                AssociateBuildingAI<ProcessingFacilityAI    >(UsageType.UseLogic1,                          GetUsageCountStorageProcessingFacilityInput ,
                                                              UsageType.UseLogic2,                          GetUsageCountStorageProcessingFacilityOutput);
                AssociateBuildingAI<UniqueFactoryAI         >(UsageType.StorageIndustryUniqueFactoryInput,  GetUsageCountStorageUniqueFactoryInput      ,
                                                              UsageType.StorageIndustryUniqueFactoryOutput, GetUsageCountStorageUniqueFactoryOutput     );
                AssociateBuildingAI<WarehouseAI             >(UsageType.UseLogic1,                          GetUsageCountStorageWarehouse               );

                // set mutually exclusive check boxes
                MakeCheckBoxesMutuallyExclusive(UsageType.StorageIndustryForestryProcessorInput,    UsageType.StorageIndustryForestryProcessorOutput);
                MakeCheckBoxesMutuallyExclusive(UsageType.StorageIndustryFarmingProcessorInput,     UsageType.StorageIndustryFarmingProcessorOutput);
                MakeCheckBoxesMutuallyExclusive(UsageType.StorageIndustryOreProcessorInput,         UsageType.StorageIndustryOreProcessorOutput);
                MakeCheckBoxesMutuallyExclusive(UsageType.StorageIndustryOilProcessorInput,         UsageType.StorageIndustryOilProcessorOutput);
                MakeCheckBoxesMutuallyExclusive(UsageType.StorageIndustryFishingProcessorInput,     UsageType.StorageIndustryFishingProcessorOutput);
                MakeCheckBoxesMutuallyExclusive(UsageType.StorageIndustryUniqueFactoryInput,        UsageType.StorageIndustryUniqueFactoryOutput);
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
            // logic depends on building AI type and subservice
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            ItemClass.SubService subService = data.Info.m_class.m_subService;
            if (buildingAIType == typeof(ExtractingFacilityAI))
            {
                // convert building subservice to usage type
                switch (subService)
                {
                    case ItemClass.SubService.PlayerIndustryForestry: return UsageType.StorageIndustryForestryExtractor;
                    case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.StorageIndustryFarmingExtractor;
                    case ItemClass.SubService.PlayerIndustryOre:      return UsageType.StorageIndustryOreExtractor;
                    case ItemClass.SubService.PlayerIndustryOil:      return UsageType.StorageIndustryOilExtractor;
                }
            }
            else if (buildingAIType == typeof(ProcessingFacilityAI))
            {
                // convert building service or subservice to usage type
                if (data.Info.m_class.m_service == ItemClass.Service.Fishing)
                {
                    return UsageType.StorageIndustryFishingProcessorInput;
                }
                else
                {
                    switch (subService)
                    {
                        case ItemClass.SubService.PlayerIndustryForestry: return UsageType.StorageIndustryForestryProcessorInput;
                        case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.StorageIndustryFarmingProcessorInput;
                        case ItemClass.SubService.PlayerIndustryOre:      return UsageType.StorageIndustryOreProcessorInput;
                        case ItemClass.SubService.PlayerIndustryOil:      return UsageType.StorageIndustryOilProcessorInput;
                    }
                }
            }
            else if (buildingAIType == typeof(WarehouseAI))
            {
                // convert building subservice to usage type
                switch (subService)
                {
                    case ItemClass.SubService.PlayerIndustryForestry: return UsageType.StorageIndustryForestryStorage;
                    case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.StorageIndustryFarmingStorage;
                    case ItemClass.SubService.PlayerIndustryOre:      return UsageType.StorageIndustryOreStorage;
                    case ItemClass.SubService.PlayerIndustryOil:      return UsageType.StorageIndustryOilStorage;
                    case ItemClass.SubService.None:                   return UsageType.StorageIndustryWarehouseGeneric;
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
            // logic depends on building AI type and subservice
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            ItemClass.SubService subService = data.Info.m_class.m_subService;
            if (buildingAIType == typeof(ProcessingFacilityAI))
            {
                // convert building service or subservice to usage type
                if (data.Info.m_class.m_service == ItemClass.Service.Fishing)
                {
                    return UsageType.StorageIndustryFishingProcessorOutput;
                }
                else
                {
                    switch (subService)
                    {
                        case ItemClass.SubService.PlayerIndustryForestry: return UsageType.StorageIndustryForestryProcessorOutput;
                        case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.StorageIndustryFarmingProcessorOutput;
                        case ItemClass.SubService.PlayerIndustryOre:      return UsageType.StorageIndustryOreProcessorOutput;
                        case ItemClass.SubService.PlayerIndustryOil:      return UsageType.StorageIndustryOilProcessorOutput;
                    }
                }
            }

            // usage type not determined with above logic
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
