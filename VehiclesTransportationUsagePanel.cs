using ColossalFramework;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display vehicle transportation usage
    /// </summary>
    public class VehiclesTransportationUsagePanel : UsagePanel
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

                // define list of usage types for this panel that are on building prefabs
                List<UsageType> usageTypes = new List<UsageType>();
                int buildingPrefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
                for (uint index = 0; index < buildingPrefabCount; index++)
                {
                    // get the prefab
                    BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(index);

                    // get the usage type for the prefab
                    UsageType usageType = GetVehiclesTransportationUsageType(prefab);

                    // if not None and not already in the list, add it to the list
                    if (usageType != UsageType.None && !usageTypes.Contains(usageType))
                    {
                        usageTypes.Add(usageType);
                    }
                }

                // both People and Cargo are in the base game, so there is no logic on those headings
                CreateGroupHeading("People Transportation");
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationBus,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationIntercityBus, usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationTrolleybus,   usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationTram,         usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationMetro,        usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationTrainPeople,  usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationShipPeople,   usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationAirPeople,    usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationMonorail,     usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationCableCar,     usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationTaxi,         usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationTours,        usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationHubs,         usageTypes);

                CreateGroupHeading("Cargo Transportation (includes trucks)");
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationTrainCargo,   usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationShipCargo,    usageTypes);
                CreateUsageGroupIfDefined(UsageType.VehiclesTransportationAirCargo,     usageTypes);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<DepotAI             >(UsageType.UseLogic1,  GetUsageCountVehiclesTransportation);
                AssociateBuildingAI<CableCarStationAI   >(UsageType.UseLogic1,  GetUsageCountVehiclesTransportation);
                AssociateBuildingAI<TransportStationAI  >(UsageType.UseLogic1,  GetUsageCountVehiclesTransportation);
                AssociateBuildingAI<HarborAI            >(UsageType.UseLogic1,  GetUsageCountVehiclesTransportation);
                AssociateBuildingAI<TourBuildingAI      >(UsageType.UseLogic1,  GetUsageCountVehiclesTransportation);
                AssociateBuildingAI<CargoStationAI      >(UsageType.UseLogic1,  GetUsageCountVehiclesTransportation);
                AssociateBuildingAI<CargoHarborAI       >(UsageType.UseLogic1,  GetUsageCountVehiclesTransportation);

                // associate each vehicle AI type with its usage type
                // associate all vehicle AIs even if corresponding DLC is not installed (there will simply be no vehicles with that AI)
                AssociateVehicleAI<BusAI                >(UsageType.UseLogic1);
                AssociateVehicleAI<TrolleybusAI         >(UsageType.UseLogic1);
                AssociateVehicleAI<TramAI               >(UsageType.UseLogic1);
                AssociateVehicleAI<MetroTrainAI         >(UsageType.UseLogic1);
                AssociateVehicleAI<PassengerTrainAI     >(UsageType.UseLogic1);
                AssociateVehicleAI<PassengerShipAI      >(UsageType.UseLogic1);
                AssociateVehicleAI<PassengerFerryAI     >(UsageType.UseLogic1);
                AssociateVehicleAI<PassengerPlaneAI     >(UsageType.UseLogic1);
                AssociateVehicleAI<PassengerHelicopterAI>(UsageType.UseLogic1);
                AssociateVehicleAI<PassengerBlimpAI     >(UsageType.UseLogic1);
                AssociateVehicleAI<CableCarAI           >(UsageType.UseLogic1);
                AssociateVehicleAI<TaxiAI               >(UsageType.UseLogic1);
                AssociateVehicleAI<BalloonAI            >(UsageType.UseLogic1);
                AssociateVehicleAI<CargoTruckAI         >(UsageType.UseLogic1);
                AssociateVehicleAI<CargoTrainAI         >(UsageType.UseLogic1);
                AssociateVehicleAI<CargoShipAI          >(UsageType.UseLogic1);
                AssociateVehicleAI<CargoPlaneAI         >(UsageType.UseLogic1);
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
            // get the usage type for the prefab
            Type buildingAIType = data.Info.m_buildingAI.GetType();
            if (buildingAIType == typeof(DepotAI            ) ||
                buildingAIType == typeof(CableCarStationAI  ) ||
                buildingAIType == typeof(TransportStationAI ) ||
                buildingAIType == typeof(HarborAI           ) ||
                buildingAIType == typeof(TourBuildingAI     ) ||
                buildingAIType == typeof(CargoStationAI     ) ||
                buildingAIType == typeof(CargoHarborAI      ))
            {
                return GetVehiclesTransportationUsageType(data.Info);
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
            // get the usage type
            Type vehicleAIType = data.Info.m_vehicleAI.GetType();
            if (vehicleAIType == typeof(BusAI                   ) ||
                vehicleAIType == typeof(TrolleybusAI            ) ||
                vehicleAIType == typeof(TramAI                  ) ||
                vehicleAIType == typeof(MetroTrainAI            ) ||
                vehicleAIType == typeof(PassengerTrainAI        ) ||
                vehicleAIType == typeof(PassengerShipAI         ) ||
                vehicleAIType == typeof(PassengerFerryAI        ) ||
                vehicleAIType == typeof(PassengerPlaneAI        ) ||
                vehicleAIType == typeof(PassengerHelicopterAI   ) ||
                vehicleAIType == typeof(PassengerBlimpAI        ) ||
                vehicleAIType == typeof(CableCarAI              ) ||
                vehicleAIType == typeof(TaxiAI                  ) ||
                vehicleAIType == typeof(BalloonAI               ) ||
                vehicleAIType == typeof(CargoTruckAI            ) ||
                vehicleAIType == typeof(CargoTrainAI            ) ||
                vehicleAIType == typeof(CargoShipAI             ) ||
                vehicleAIType == typeof(CargoPlaneAI            ))
            {
                // get first vehicle (e.g. engine on a train)
                ushort firstVehicle = data.GetFirstVehicle(vehicleID);
                if (firstVehicle == 0)
                {
                    firstVehicle = vehicleID;
                }

                // get source building of first vehicle
                Vehicle firstVehicleData = Singleton<VehicleManager>.instance.m_vehicles.m_buffer[firstVehicle];
                ushort buildingID = firstVehicleData.m_sourceBuilding;
                Building buildingData = Singleton<BuildingManager>.instance.m_buildings.m_buffer[buildingID];
                Type buildingAIType = buildingData.Info.m_buildingAI.GetType();

                // ignore vehicle from outside connection
                if (buildingAIType == typeof(OutsideConnectionAI))
                {
                    return UsageType.None;
                }
                else
                {
                    // get usage type according to source building
                    return GetVehiclesTransportationUsageType(buildingData.Info);
                }
            }

            // usage type not determined with above logic
            Debug.LogError($"Unhandled vehicle AI type [{vehicleAIType.ToString()}] when getting usage type with logic.");
            return UsageType.None;
        }

    }
}
