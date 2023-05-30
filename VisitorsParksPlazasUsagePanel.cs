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
                bool contentCreatorPack = false;
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
                        usageType = GetVisitorsParksPlazasUsageType(prefab, out bool ccp);
                        contentCreatorPack |= ccp;
                    }
                    else if (buildingAIType == typeof(EdenProjectAI))
                    {
                        usageType = UsageType.VisitorsParksPlazasEdenProject;
                    }
                    else if (buildingAIType == typeof(ParkBuildingAI))
                    {
                        usageType = GetVisitorsParksPlazasUsageType(prefab);
                    }
                    else if (buildingAIType == typeof(IceCreamStandAI))
                    {
                        usageType = UsageType.VisitorsParksPlazasPedestrianPlazas;
                    }
                    else if (buildingAIType == typeof(HotelAI))
                    {
                        usageType = UsageType.VisitorsParksPlazasHotels;
                    }
                    else if (buildingAIType == typeof(TourBuildingAI))
                    {
                        usageType = UsageType.VisitorsParksPlazasTours;
                    }
                    else if (buildingAIType == typeof(ChirperTourAI))
                    {
                        usageType = UsageType.VisitorsParksPlazasOtherParks;
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
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasParks,               usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasPlazas,              usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasOtherParks,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasTourismLeisure,      usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasWinterkParks,        usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasPedestrianPlazas,    usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasHotels,              usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasEdenProject,         usageTypes);

                if (SteamHelper.IsDLCOwned(SteamHelper.DLC.ParksDLC))
                {
                    CreateGroupHeading("Parklife");
                    CreateUsageGroup(UsageType.VisitorsParksPlazasCityPark);
                    CreateUsageGroup(UsageType.VisitorsParksPlazasAmusementPark);
                    CreateUsageGroup(UsageType.VisitorsParksPlazasZoo);
                    CreateUsageGroup(UsageType.VisitorsParksPlazasNatureReserve);
                    CreateUsageGroup(UsageType.VisitorsParksPlazasTours);
                }

                if (contentCreatorPack) { CreateGroupHeading("Content Creator Pack"); }
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasCCPHighTech,         usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasCCPBridgesPiers,     usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasCCPMidCenturyModern, usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasCCPSportsVenues,     usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasCCPAfricaInMiniature,usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsParksPlazasCCPRailroadsOfJapan, usageTypes);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<ParkAI         >(UsageType.UseLogic1,                           GetUsageCountVisitorsPark<ParkAI>                 );
                AssociateBuildingAI<EdenProjectAI  >(UsageType.VisitorsParksPlazasEdenProject,      GetUsageCountVisitorsPark<EdenProjectAI>          );
                AssociateBuildingAI<ParkBuildingAI >(UsageType.UseLogic1,                           GetUsageCountVisitorsParkBuilding<ParkBuildingAI> );
                AssociateBuildingAI<IceCreamStandAI>(UsageType.VisitorsParksPlazasPedestrianPlazas, GetUsageCountVisitorsParkBuilding<IceCreamStandAI>);
                AssociateBuildingAI<HotelAI        >(UsageType.VisitorsParksPlazasHotels,           GetUsageCountVisitorsHotel                        );
                AssociateBuildingAI<TourBuildingAI >(UsageType.VisitorsParksPlazasTours,            GetUsageCountVisitorsTourBuilding<TourBuildingAI> );
                AssociateBuildingAI<ChirperTourAI  >(UsageType.VisitorsParksPlazasOtherParks,       GetUsageCountVisitorsTourBuilding<ChirperTourAI>  );
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
            if (buildingAIType == typeof(ParkAI))
            {
                return GetVisitorsParksPlazasUsageType(data.Info, out bool _);
            }
            else if (buildingAIType == typeof(ParkBuildingAI))
            {
                return GetVisitorsParksPlazasUsageType(data.Info);
            }

            LogUtil.LogError($"Unhandled building AI type [{buildingAIType}] when getting usage type with logic");
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
