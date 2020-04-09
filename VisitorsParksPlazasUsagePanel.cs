using UnityEngine;
using System;
using System.Collections.Generic;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display parks & plazas visitors usage
    /// </summary>
    public class VisitorsParksPlazasUsagePanel : UsagePanel
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
                CreateReturnFromDetailButton("Return to Visitors");

                // define list of usage types for this panel that are on building prefabs
                List<UsageType> usageTypes = new List<UsageType>();
                int buildingPrefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
                for (uint index = 0; index < buildingPrefabCount; index++)
                {
                    // get the prefab
                    BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(index);

                    // get the usage type for the prefab
                    UsageType usageType = UsageType.None;
                    Type buildingAIType = prefab.m_buildingAI.GetType();
                    if (buildingAIType == typeof(ParkAI))
                    {
                        usageType = GetVisitorsParksPlazasUsageType(prefab.category, prefab.name);
                    }
                    else if (buildingAIType == typeof(EdenProjectAI))
                    {
                        usageType = UsageType.VisitorsParksPlazasEdenProject;
                    }
                    else if (buildingAIType == typeof(ParkBuildingAI))
                    {
                        // this building AI is handled specially by Parklife DLC
                        usageType = UsageType.None;
                    }
                    else if (buildingAIType == typeof(TourBuildingAI))
                    {
                        usageType = UsageType.VisitorsParksPlazasTours;
                    }

                    // if not None and not already in the list, add it to the list
                    if (usageType != UsageType.None && !usageTypes.Contains(usageType))
                    {
                        usageTypes.Add(usageType);
                    }
                }

                // create the usage groups
                // at least one of Basic and Other are in the base game, so there is no logic on those headings
                CreateGroupHeading("Basic");
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasParks,           usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasPlazas,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasOtherParks,      usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasTourismLeisure,  usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasWinterkParks,    usageTypes);
                
                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.ParksDLC))
                {
                    CreateGroupHeading("Parklife");
                    CreateUsageGroup(UsageType.VisitorsParksPlazasCityPark);
                    CreateUsageGroup(UsageType.VisitorsParksPlazasAmusementPark);
                    CreateUsageGroup(UsageType.VisitorsParksPlazasZoo);
                    CreateUsageGroup(UsageType.VisitorsParksPlazasNatureReserve);
                    CreateUsageGroup(UsageType.VisitorsParksPlazasTours);
                }
                
                CreateGroupHeading("Other");
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasContentCreator,  usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasEdenProject,     usageTypes);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<ParkAI        >(UsageType.UseLogic1,                        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsPark<ParkAI>       (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<EdenProjectAI >(UsageType.VisitorsParksPlazasEdenProject,   (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsPark<EdenProjectAI>(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ParkBuildingAI>(UsageType.UseLogic1,                        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsParkBuilding       (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<TourBuildingAI>(UsageType.VisitorsParksPlazasTours,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsTourBuilding       (buildingID, ref data, ref used, ref allowed));
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
            if (buildingAIType == typeof(ParkAI))
            {
                return GetVisitorsParksPlazasUsageType(data.Info.category, data.Info.name);
            }
            else if (buildingAIType == typeof(ParkBuildingAI))
            {
                return GetVisitorsParksPlazasUsageType(ref data);
            }

            Debug.LogError($"Unhandled building AI type [{buildingAIType.ToString()}] when getting usage type with logic");
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
