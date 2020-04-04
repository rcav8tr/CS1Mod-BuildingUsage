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
                        usageType = GetUsageTypeForParkAI(prefab.category, prefab.name);
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
                return GetUsageTypeForParkAI(data.Info.category, data.Info.name);
            }
            else if (buildingAIType == typeof(ParkBuildingAI))
            {
                return GetUsageTypeForParkBuildingAI(ref data);
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

        /// <summary>
        /// return the usage type for a ParkAI building
        /// </summary>
        private UsageType GetUsageTypeForParkAI(string category, string name)
        {
            // Building AI                      Building                                    Category                    Usage Type
            // --------------------------       ------------------------------------------  --------------------------  --------------
            // ParkAI                      -V-- Parks:
            //                                      Small Park                              BeautificationParks         Parks
            //                                      Small Playground                        BeautificationParks         Parks
            //                                      Park With Trees                         BeautificationParks         Parks
            //                                      Large Playground                        BeautificationParks         Parks
            //                                      Bouncy Castle Park                      BeautificationParks         Parks
            //                                      Botanical Garden                        BeautificationParks         Parks
            //                                      Dog Park                                BeautificationParks         Parks
            //                                      Carousel Park                           BeautificationParks         Parks
            //                                      Japanese Garden                         BeautificationParks         Parks
            //                                      Tropical Garden                         BeautificationParks         Parks
            //                                      Fishing Island                          BeautificationParks         Parks
            //                                      Floating Cafe                           BeautificationParks         Parks
            //                                  Plazas:
            //                                      Plaza with Trees                        BeautificationPlazas        Plazas
            //                                      Plaza with Picnic Tables                BeautificationPlazas        Plazas
            //                                      Paradox Plaza                           BeautificationPlazas        Plazas
            //                                  Other Parks:
            //                                      Basketball Court                        BeautificationOthers        OtherParks
            //                                      Tennis Court                            BeautificationOthers        OtherParks
            //                                  Tourism & Leisure:
            //                                      Fishing Pier                            BeautificationExpansion1    TourismLeisure
            //                                      Fishing Tours                           BeautificationExpansion1    TourismLeisure
            //                                      Jet Ski Rental                          BeautificationExpansion1    TourismLeisure
            //                                      Marina                                  BeautificationExpansion1    TourismLeisure
            //                                      Restaurant Pier                         BeautificationExpansion1    TourismLeisure
            //                                      Beach Volleyball Court                  BeautificationExpansion1    TourismLeisure
            //                                      Riding Stable                           BeautificationExpansion1    TourismLeisure
            //                                      Skatepark                               BeautificationExpansion1    TourismLeisure
            //                                      Snowmobile Track                        BeautificationExpansion1    TourismLeisure
            //                                      Winter Fishing Pier                     BeautificationExpansion1    TourismLeisure
            //                                      Ice Hockey Rink                         BeautificationExpansion1    TourismLeisure
            //                                  Winter Parks:
            //                                      Snowman Park                            BeautificationExpansion2    WinterParks
            //                                      Ice Sculpture Park                      BeautificationExpansion2    WinterParks
            //                                      Sledding Hill                           BeautificationExpansion2    WinterParks
            //                                      Curling Park                            BeautificationExpansion2    WinterParks
            //                                      Skating Rink                            BeautificationExpansion2    WinterParks
            //                                      Ski Lodge                               BeautificationExpansion2    WinterParks
            //                                      Cross-Country Skiing Park               BeautificationExpansion2    WinterParks
            //                                      Firepit Park                            BeautificationExpansion2    WinterParks
            //                                  Content Creator:
            //                                      Biodome                                 MonumentModderPack          ModderPacks
            //                                      Vertical Farm                           MonumentModderPack          ModderPacks

            // usage type depends on category
            switch (category)
            {
                case "BeautificationParks":      return UsageType.VisitorsParksPlazasParks;
                case "BeautificationPlazas":     return UsageType.VisitorsParksPlazasPlazas;
                case "BeautificationOthers":     return UsageType.VisitorsParksPlazasOtherParks;
                case "BeautificationExpansion1": return UsageType.VisitorsParksPlazasTourismLeisure;
                case "BeautificationExpansion2": return UsageType.VisitorsParksPlazasWinterkParks;
                case "MonumentModderPack":       return UsageType.VisitorsParksPlazasContentCreator;
                default:
                    Debug.LogError($"Unhandled building category [{category}] when determining usage type for building [{name}].");
                    return UsageType.None;
            }
        }

        /// <summary>
        /// return the usage type for a ParkBuildingAI building
        /// </summary>
        private UsageType GetUsageTypeForParkBuildingAI(ref Building data)
        {
            // usage type depends on park type
            DistrictPark.ParkType parkType = GetParkType(ref data);
            switch (parkType)
            {
                case DistrictPark.ParkType.Generic:         return UsageType.VisitorsParksPlazasCityPark;
                case DistrictPark.ParkType.AmusementPark:   return UsageType.VisitorsParksPlazasAmusementPark;
                case DistrictPark.ParkType.Zoo:             return UsageType.VisitorsParksPlazasZoo;
                case DistrictPark.ParkType.NatureReserve:   return UsageType.VisitorsParksPlazasNatureReserve;
                default:
                    Debug.LogError($"Unhandled park type [{parkType.ToString()}] when determining usage type for building [{data.Info.name}].");
                    return UsageType.None;
            }
        }

    }
}
