using UnityEngine;
using System;
using System.Collections.Generic;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display transportation worker usage
    /// </summary>
    public class WorkersTransportationUsagePanel : UsagePanel
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

                // define list of usage types for this panel that are on building prefabs
                List<UsageType> usageTypes = new List<UsageType>();
                int buildingPrefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
                for (uint index = 0; index < buildingPrefabCount; index++)
                {
                    // get the prefab
                    BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(index);

                    // get the usage type for the prefab
                    UsageType usageType = GetWorkersTransportationUsageType(prefab);

                    // if not None and not already in the list, add it to the list
                    if (usageType != UsageType.None && !usageTypes.Contains(usageType))
                    {
                        usageTypes.Add(usageType);
                    }
                }

                // create the usage groups
                // both People and Cargo are in the base game, so there is no logic on those headings
                CreateGroupHeading("People Transportation");
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationBus,           usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationIntercityBus,  usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationTrolleybus,    usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationTram,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationMetro,         usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationTrainPeople,   usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationShipPeople,    usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationAirPeople,     usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationMonorail,      usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationCableCar,      usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationTaxi,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationTours,         usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationHubs,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationSpaceElevator, usageTypes);

                CreateGroupHeading("Cargo Transportation");
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationTrainCargo,    usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationShipCargo,     usageTypes);
                CreateUsageGroupIfDefined(UsageType.WorkersTransportationAirCargo,      usageTypes);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<DepotAI           >(UsageType.UseLogic1, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<DepotAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CableCarStationAI >(UsageType.UseLogic1, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CableCarStationAI >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<TransportStationAI>(UsageType.UseLogic1, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<TransportStationAI>(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<HarborAI          >(UsageType.UseLogic1, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<HarborAI          >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SpaceElevatorAI   >(UsageType.UseLogic1, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<SpaceElevatorAI   >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CargoStationAI    >(UsageType.UseLogic1, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CargoStationAI    >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CargoHarborAI     >(UsageType.UseLogic1, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CargoHarborAI     >(buildingID, ref data, ref used, ref allowed));
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
                buildingAIType == typeof(SpaceElevatorAI    ) ||
                buildingAIType == typeof(CargoStationAI     ) ||
                buildingAIType == typeof(CargoHarborAI      ))
            {
                return GetWorkersTransportationUsageType(data.Info);
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
