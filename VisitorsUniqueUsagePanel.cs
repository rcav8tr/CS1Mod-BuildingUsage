using System;
using System.Collections.Generic;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display unique buildings visitors usage
    /// </summary>
    public class VisitorsUniqueUsagePanel : UsagePanel
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
                    UsageType usageType = GetVisitorsUniqueUsageType(prefab);

                    // if not None and not already in the list, add it to the list
                    if (usageType != UsageType.None && !usageTypes.Contains(usageType))
                    {
                        usageTypes.Add(usageType);
                    }
                }

                // create the usage groups
                // at least one of Basic and Level are in the base game, so there is no logic on those headings
                CreateGroupHeading("Basic Unique");
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueFinancial,        usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueLandmark,         usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueTourismLeisure,   usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueWinterUnique,     usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniquePedestrianArea,   usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueFootball,         usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueConcert,          usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueAirports,         usageTypes);

                CreateGroupHeading("Level Unique");
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueLevel1,           usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueLevel2,           usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueLevel3,           usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueLevel4,           usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueLevel5,           usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueLevel6,           usageTypes);

                if (usageTypes.Contains(UsageType.VisitorsUniqueContentCreator) || usageTypes.Contains(UsageType.VisitorsUniqueCastle)) CreateGroupHeading("Other Unique");
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueContentCreator,   usageTypes);
                CreateUsageGroupIfDefined(UsageType.VisitorsUniqueCastle,           usageTypes);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<MonumentAI                      >(UsageType.UseLogic1, GetUsageCountVisitorsMonument<MonumentAI                     >);
                AssociateBuildingAI<AirlineHeadquartersAI           >(UsageType.UseLogic1, GetUsageCountVisitorsMonument<AirlineHeadquartersAI          >);
                AssociateBuildingAI<AnimalMonumentAI                >(UsageType.UseLogic1, GetUsageCountVisitorsMonument<AnimalMonumentAI               >);
                AssociateBuildingAI<PrivateAirportAI                >(UsageType.UseLogic1, GetUsageCountVisitorsMonument<PrivateAirportAI               >);
                AssociateBuildingAI<ChirpwickCastleAI               >(UsageType.UseLogic1, GetUsageCountVisitorsMonument<ChirpwickCastleAI              >);
                AssociateBuildingAI<StockExchangeAI                 >(UsageType.UseLogic1, GetUsageCountVisitorsMonument<StockExchangeAI                >);
                AssociateBuildingAI<InternationalTradeBuildingAI    >(UsageType.UseLogic1, GetUsageCountVisitorsMonument<InternationalTradeBuildingAI   >);
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
            if (buildingAIType == typeof(MonumentAI                  ) ||
                buildingAIType == typeof(AirlineHeadquartersAI       ) ||
                buildingAIType == typeof(AnimalMonumentAI            ) ||
                buildingAIType == typeof(PrivateAirportAI            ) ||
                buildingAIType == typeof(ChirpwickCastleAI           ) ||
                buildingAIType == typeof(StockExchangeAI             ) ||
                buildingAIType == typeof(InternationalTradeBuildingAI))
            {
                return GetVisitorsUniqueUsageType(data.Info);
            }

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
