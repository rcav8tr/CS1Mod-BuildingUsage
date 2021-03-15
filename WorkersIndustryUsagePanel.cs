using UnityEngine;
using System;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display industry worker usage
    /// </summary>
    public class WorkersIndustryUsagePanel : UsagePanel
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
                CreateReturnFromDetailButton("Return to Workers");

                // create the usage groups
                // ProcessingFacilityAI is used by both IndustryDLC and UrbanDLC (Sunset Harbor), so need to check for DLC and no need to check for building AI
                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC))
                {
                    CreateGroupHeading("Forestry Industry");
                    CreateUsageGroup(UsageType.WorkersIndustryForestryMainAux);
                    CreateUsageGroup(UsageType.WorkersIndustryForestryExtractor);
                    CreateUsageGroup(UsageType.WorkersIndustryForestryProcessor);
                    CreateUsageGroup(UsageType.WorkersIndustryForestryStorage);

                    CreateGroupHeading("Farming Industry");
                    CreateUsageGroup(UsageType.WorkersIndustryFarmingMainAux);
                    CreateUsageGroup(UsageType.WorkersIndustryFarmingExtractor);
                    CreateUsageGroup(UsageType.WorkersIndustryFarmingProcessor);
                    CreateUsageGroup(UsageType.WorkersIndustryFarmingStorage);

                    CreateGroupHeading("Ore Industry");
                    CreateUsageGroup(UsageType.WorkersIndustryOreMainAux);
                    CreateUsageGroup(UsageType.WorkersIndustryOreExtractor);
                    CreateUsageGroup(UsageType.WorkersIndustryOreProcessor);
                    CreateUsageGroup(UsageType.WorkersIndustryOreStorage);

                    CreateGroupHeading("Oil Industry");
                    CreateUsageGroup(UsageType.WorkersIndustryOilMainAux);
                    CreateUsageGroup(UsageType.WorkersIndustryOilExtractor);
                    CreateUsageGroup(UsageType.WorkersIndustryOilProcessor);
                    CreateUsageGroup(UsageType.WorkersIndustryOilStorage);
                }

                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.UrbanDLC))
                {
                    CreateGroupHeading("Fishing Industry");
                    CreateUsageGroup(UsageType.WorkersIndustryFishingExtractor);
                    CreateUsageGroup(UsageType.WorkersIndustryFishingProcessor);
                    CreateUsageGroup(UsageType.WorkersIndustryFishingMarket);
                }

                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC))
                {
                    CreateGroupHeading("Other Industry");
                    CreateUsageGroup(UsageType.WorkersIndustryWarehouse);
                    CreateUsageGroup(UsageType.WorkersIndustryUniqueFactory);
                }

                // associate each building AI type with its usage type(s) and usage count routine(s)
                AssociateBuildingAI<MainIndustryBuildingAI>(UsageType.UseLogic1,                        GetUsageCountWorkersService<MainIndustryBuildingAI>);
                AssociateBuildingAI<AuxiliaryBuildingAI   >(UsageType.UseLogic1,                        GetUsageCountWorkersService<AuxiliaryBuildingAI   >);
                AssociateBuildingAI<ExtractingFacilityAI  >(UsageType.UseLogic1,                        GetUsageCountWorkersService<ExtractingFacilityAI  >);
                AssociateBuildingAI<FishingHarborAI       >(UsageType.WorkersIndustryFishingExtractor,  GetUsageCountWorkersService<FishingHarborAI       >);
                AssociateBuildingAI<FishFarmAI            >(UsageType.WorkersIndustryFishingExtractor,  GetUsageCountWorkersService<FishFarmAI            >);
                AssociateBuildingAI<ProcessingFacilityAI  >(UsageType.UseLogic1,                        GetUsageCountWorkersService<ProcessingFacilityAI  >);
                AssociateBuildingAI<MarketAI              >(UsageType.WorkersIndustryFishingMarket,     GetUsageCountWorkersService<MarketAI              >);
                AssociateBuildingAI<WarehouseAI           >(UsageType.UseLogic1,                        GetUsageCountWorkersService<WarehouseAI           >);
                AssociateBuildingAI<UniqueFactoryAI       >(UsageType.WorkersIndustryUniqueFactory,     GetUsageCountWorkersService<UniqueFactoryAI       >);
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
            // logic depends on building AI type and subservice
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            ItemClass.SubService subService = data.Info.m_class.m_subService;
            if (buildingAIType == typeof(MainIndustryBuildingAI) || buildingAIType == typeof(AuxiliaryBuildingAI))
            {
                // convert building subservice to usage type
                switch (subService)
                {
                    case ItemClass.SubService.PlayerIndustryForestry: return UsageType.WorkersIndustryForestryMainAux;
                    case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.WorkersIndustryFarmingMainAux;
                    case ItemClass.SubService.PlayerIndustryOre:      return UsageType.WorkersIndustryOreMainAux;
                    case ItemClass.SubService.PlayerIndustryOil:      return UsageType.WorkersIndustryOilMainAux;
                }
            }
            else if (buildingAIType == typeof(ExtractingFacilityAI))
            {
                // convert building subservice to usage type
                switch (subService)
                {
                    case ItemClass.SubService.PlayerIndustryForestry: return UsageType.WorkersIndustryForestryExtractor;
                    case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.WorkersIndustryFarmingExtractor;
                    case ItemClass.SubService.PlayerIndustryOre:      return UsageType.WorkersIndustryOreExtractor;
                    case ItemClass.SubService.PlayerIndustryOil:      return UsageType.WorkersIndustryOilExtractor;
                }
            }
            else if (buildingAIType == typeof(ProcessingFacilityAI))
            {
                // convert building service or subservice to usage type
                if (data.Info.m_class.m_service == ItemClass.Service.Fishing)
                {
                    return UsageType.WorkersIndustryFishingProcessor;
                }
                else
                {
                    switch (subService)
                    {
                        case ItemClass.SubService.PlayerIndustryForestry: return UsageType.WorkersIndustryForestryProcessor;
                        case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.WorkersIndustryFarmingProcessor;
                        case ItemClass.SubService.PlayerIndustryOre:      return UsageType.WorkersIndustryOreProcessor;
                        case ItemClass.SubService.PlayerIndustryOil:      return UsageType.WorkersIndustryOilProcessor;
                    }
                }
            }
            else if (buildingAIType == typeof(WarehouseAI))
            {
                // convert building subservice to usage type
                switch (subService)
                {
                    case ItemClass.SubService.PlayerIndustryForestry: return UsageType.WorkersIndustryForestryStorage;
                    case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.WorkersIndustryFarmingStorage;
                    case ItemClass.SubService.PlayerIndustryOre:      return UsageType.WorkersIndustryOreStorage;
                    case ItemClass.SubService.PlayerIndustryOil:      return UsageType.WorkersIndustryOilStorage;
                    case ItemClass.SubService.None:                   return UsageType.WorkersIndustryWarehouse;
                }
            }

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
