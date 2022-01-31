using System;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display vehicle usage
    /// </summary>
    public class VehiclesIndustryUsagePanel : UsagePanel
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
                CreateReturnFromDetailButton("Return to Vehicles");

                // create the usage groups, one for each vehicle usage type
                // ProcessingFacilityAI is used by both IndustryDLC and UrbanDLC (Sunset Harbor), so need to check for DLC and no need to check for building AI
                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC))
                {
                    CreateGroupHeading("Forestry Industry Trucks");
                    CreateUsageGroup(UsageType.VehiclesIndustryForestryExtractor);
                    CreateUsageGroup(UsageType.VehiclesIndustryForestryProcessor);
                    CreateUsageGroup(UsageType.VehiclesIndustryForestryStorage);

                    CreateGroupHeading("Farming Industry Trucks");
                    CreateUsageGroup(UsageType.VehiclesIndustryFarmingExtractor);
                    CreateUsageGroup(UsageType.VehiclesIndustryFarmingProcessor);
                    CreateUsageGroup(UsageType.VehiclesIndustryFarmingStorage);

                    CreateGroupHeading("Ore Industry Trucks");
                    CreateUsageGroup(UsageType.VehiclesIndustryOreExtractor);
                    CreateUsageGroup(UsageType.VehiclesIndustryOreProcessor);
                    CreateUsageGroup(UsageType.VehiclesIndustryOreStorage);

                    CreateGroupHeading("Oil Industry Trucks");
                    CreateUsageGroup(UsageType.VehiclesIndustryOilExtractor);
                    CreateUsageGroup(UsageType.VehiclesIndustryOilProcessor);
                    CreateUsageGroup(UsageType.VehiclesIndustryOilStorage);
                }

                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.UrbanDLC))
                {
                    CreateGroupHeading("Fishing Industry Trucks");
                    CreateUsageGroup(UsageType.VehiclesIndustryFishingExtractor);
                    CreateUsageGroup(UsageType.VehiclesIndustryFishingProcessor);
                }

                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.IndustryDLC))
                {
                    CreateGroupHeading("Other Industry Trucks");
                    CreateUsageGroup(UsageType.VehiclesIndustryUniqueFactory);
                    CreateUsageGroup(UsageType.VehiclesIndustryWarehouseGeneric);
                }

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<ExtractingFacilityAI>(UsageType.UseLogic1,                        GetUsageCountVehiclesExtractingFacility                      );
                AssociateBuildingAI<FishingHarborAI     >(UsageType.VehiclesIndustryFishingExtractor, GetUsageCountVehiclesFishingHarbor                           );
                AssociateBuildingAI<FishFarmAI          >(UsageType.VehiclesIndustryFishingExtractor, GetUsageCountVehiclesFishFarm                                );
                AssociateBuildingAI<ProcessingFacilityAI>(UsageType.UseLogic1,                        GetUsageCountVehiclesProcessingFacility<ProcessingFacilityAI>);
                AssociateBuildingAI<UniqueFactoryAI     >(UsageType.VehiclesIndustryUniqueFactory,    GetUsageCountVehiclesProcessingFacility<UniqueFactoryAI>     );
                AssociateBuildingAI<WarehouseAI         >(UsageType.UseLogic1,                        GetUsageCountVehiclesWarehouse                               );

                // associate each vehicle AI type with its usage type
                // associate all vehicle AIs even if corresponding DLC is not installed (there will simply be no vehicles with that AI)
                AssociateVehicleAI<CargoTruckAI >(UsageType.UseLogic1                       );
                AssociateVehicleAI<FishingBoatAI>(UsageType.VehiclesIndustryFishingExtractor);
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
                    case ItemClass.SubService.PlayerIndustryForestry: return UsageType.VehiclesIndustryForestryExtractor;
                    case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.VehiclesIndustryFarmingExtractor;
                    case ItemClass.SubService.PlayerIndustryOre:      return UsageType.VehiclesIndustryOreExtractor;
                    case ItemClass.SubService.PlayerIndustryOil:      return UsageType.VehiclesIndustryOilExtractor;
                }
            }
            else if (buildingAIType == typeof(ProcessingFacilityAI))
            {
                // convert building service or subservice to usage type
                if (data.Info.m_class.m_service == ItemClass.Service.Fishing)
                {
                    return UsageType.VehiclesIndustryFishingProcessor;
                }
                else
                {
                    switch (subService)
                    {
                        case ItemClass.SubService.PlayerIndustryForestry: return UsageType.VehiclesIndustryForestryProcessor;
                        case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.VehiclesIndustryFarmingProcessor;
                        case ItemClass.SubService.PlayerIndustryOre:      return UsageType.VehiclesIndustryOreProcessor;
                        case ItemClass.SubService.PlayerIndustryOil:      return UsageType.VehiclesIndustryOilProcessor;
                    }
                }
            }
            else if (buildingAIType == typeof(WarehouseAI))
            {
                // convert building subservice to usage type
                switch (subService)
                {
                    case ItemClass.SubService.PlayerIndustryForestry: return UsageType.VehiclesIndustryForestryStorage;
                    case ItemClass.SubService.PlayerIndustryFarming:  return UsageType.VehiclesIndustryFarmingStorage;
                    case ItemClass.SubService.PlayerIndustryOre:      return UsageType.VehiclesIndustryOreStorage;
                    case ItemClass.SubService.PlayerIndustryOil:      return UsageType.VehiclesIndustryOilStorage;
                    case ItemClass.SubService.None:                   return UsageType.VehiclesIndustryWarehouseGeneric;
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
            // logic depends on vehicle AI type
            Type vehicleAIType = data.Info.m_vehicleAI.GetType();
            if (vehicleAIType == typeof(CargoTruckAI))
            {
                // get first vehicle
                ushort firstVehicle = data.GetFirstVehicle(vehicleID);
                if (firstVehicle == 0)
                {
                    firstVehicle = vehicleID;
                }

                // get source building type
                Vehicle firstVehicleData = VehicleManager.instance.m_vehicles.m_buffer[firstVehicle];
                ushort buildingID = firstVehicleData.m_sourceBuilding;
                Building buildingData = BuildingManager.instance.m_buildings.m_buffer[buildingID];

                // usage type determined by looking at building AI type and subservice of the source building
                Type buildingAIType = buildingData.Info.m_buildingAI.GetType();
                ItemClass.SubService subService = buildingData.Info.m_class.m_subService;
                if (buildingAIType == typeof(OutsideConnectionAI))
                {
                    // ignore vehicle from outside connection
                    return UsageType.None;
                }
                else if (buildingAIType == typeof(ExtractingFacilityAI))
                {
                    // convert building subservice to usage type
                    switch (subService)
                    {
                        case ItemClass.SubService.PlayerIndustryForestry:   return UsageType.VehiclesIndustryForestryExtractor;
                        case ItemClass.SubService.PlayerIndustryFarming:    return UsageType.VehiclesIndustryFarmingExtractor;
                        case ItemClass.SubService.PlayerIndustryOre:        return UsageType.VehiclesIndustryOreExtractor;
                        case ItemClass.SubService.PlayerIndustryOil:        return UsageType.VehiclesIndustryOilExtractor;
                    }
                }
                else if (buildingAIType == typeof(FishingHarborAI) || buildingAIType == typeof(FishFarmAI))
                {
                    return UsageType.VehiclesIndustryFishingExtractor;
                }
                else if (buildingAIType == typeof(ProcessingFacilityAI))
                {
                    // convert building service or subservice to usage type
                    if (buildingData.Info.m_class.m_service == ItemClass.Service.Fishing)
                    {
                        return UsageType.VehiclesIndustryFishingProcessor;
                    }
                    else
                    {
                        switch (subService)
                        {
                            case ItemClass.SubService.PlayerIndustryForestry:   return UsageType.VehiclesIndustryForestryProcessor;
                            case ItemClass.SubService.PlayerIndustryFarming:    return UsageType.VehiclesIndustryFarmingProcessor;
                            case ItemClass.SubService.PlayerIndustryOre:        return UsageType.VehiclesIndustryOreProcessor;
                            case ItemClass.SubService.PlayerIndustryOil:        return UsageType.VehiclesIndustryOilProcessor;
                        }
                    }
                }
                else if (buildingAIType == typeof(WarehouseAI))
                {
                    // convert building subservice to usage type
                    switch (subService)
                    {
                        case ItemClass.SubService.PlayerIndustryForestry:   return UsageType.VehiclesIndustryForestryStorage;
                        case ItemClass.SubService.PlayerIndustryFarming:    return UsageType.VehiclesIndustryFarmingStorage;
                        case ItemClass.SubService.PlayerIndustryOre:        return UsageType.VehiclesIndustryOreStorage;
                        case ItemClass.SubService.PlayerIndustryOil:        return UsageType.VehiclesIndustryOilStorage;
                        case ItemClass.SubService.None:                     return UsageType.VehiclesIndustryWarehouseGeneric;
                    }
                }
                else
                {
                    return UsageType.None;
                }
            }

            LogUtil.LogError($"Unhandled vehicle AI type [{vehicleAIType}] when getting usage type with logic.");
            return UsageType.None;
        }

    }
}
