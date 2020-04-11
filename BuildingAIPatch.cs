using Harmony;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BuildingUsage
{
    /// <summary>
    /// Harmony patching for building AI
    /// </summary>
    public class BuildingAIPatch
    {
        // DLC = Down Loadable Content
        // CCP = Content Creator Pack

        // If a building is not available because a DLC/CCP is not installed, the building AI still remains in the game logic.
        // The corresponding building AI patch will simply never be called because there will be no buildings of that type.
        // Therefore, there is no need to avoid patching a building AI for missing DLC/CCP.

        // buildings introduced in DLC:
        // BG = Base Game               03/10/15
        // AD = After Dark              09/24/15 AfterDarkDLC
        // SF = Snowfall                02/18/16 SnowFallDLC
        // MD = Match Day               06/09/16 Football
        // ND = Natural Disasters       11/29/16 NaturalDisastersDLC
        // MT = Mass Transit            05/18/17 InMotionDLC
        // CO = Concerts                08/17/17 MusicFestival
        // GC = Green Cities            10/19/17 GreenCitiesDLC
        // PL = Park Life               05/24/18 ParksDLC
        // IN = Industries              10/23/18 IndustryDLC
        // CA = Campus                  05/21/19 CampusDLC
        // SH = Sunset Harbor           03/26/20 UrbanDLC

        // buildings introduced in CCP:
        // DE = Deluxe Edition          03/10/15 DeluxeDLC
        // AR = Art Deco                09/01/16 ModderPack1
        // HT = High Tech Buildings     11/29/16 ModderPack2
        // PE = Pearls From the East    03/22/17 OrientalBuildings
        // ES = European Suburbia       10/19/17 ModderPack3 - no unique buildings, new style, "80 new special residential buildings and props"
        // UC = University City         05/21/19 ModderPack4 - no unique buildings, "adds 36 low-density residential buildings, 32 low-density commercial buildings, and 15 props"
        // MC = Modern City Center      11/07/19 ModderPack5 - no unique buildings, new style, "adds 39 unique models featuring new modern commercial wall-to-wall buildings"
        // MJ = Modern Japan            03/26/20 ModderPack6


        // zoned building AIs are derived from PrivateBuildingAI, used by Workers panel only

        // ResidentialBuildingAI       W--- Zoned Generic Low Density BG, Zoned Generic High Density BG, Zoned Specialized Residential (Self-Sufficient Buildings GC)
        // CommercialBuildingAI        W--- Zoned Generic Low Density BG, Zoned Generic High Density BG, Zoned Specialized Commercial (Tourism AD, Leisure AD, Organic and Local Produce GC)
        // OfficeBuildingAI            W--- Zoned Generic Office BG, Zoned Specialized Office (IT Cluster GC)
        // IndustrialBuildingAI        W--- Zoned Generic Industrial BG
        // IndustrialExtractorAI       W--- Zoned Specialized Industrial (Forest BG, Farming BG, Ore BG, Oil BG)

        // the following building AIs from the Ploppable RICO Revisited mod derive from the above zoned building AIs
        // PloppableRICO.PloppableResidential
        // PloppableRICO.PloppableCommercial
        // PloppableRICO.PloppableOffice
        // PloppableRICO.PloppableIndustrial
        // PloppableRICO.PloppableExtractor


        // service building AIs are derived from PlayerBuildingAI
        // each building AI is used by panel:  W = Workers, V = Visitors, S = Storage, V = Vehicles, U = Unlimited vehicles, - = not used by that panel
        // Some of the buildings with the same AI normally have no workers, visitors, storage, or vehicles, but can be made to have them with a mode like Customize It Extended.
        // Buildings with and without are all listed together and the ones without will simply not be shown in color.

        // CargoStationAI              W--U Cargo Train Terminal BG, Cargo Airport IN, Cargo Airport Hub IN
        //    CargoHarborAI            W--U Cargo Harbor BG, Cargo Hub AD
        // CemeteryAI                  WV-V Cemetery BG, Crematorium BG, Cryopreservatory HT (CCP)
        // ChildcareAI                 WV-- Child Health Center BG
        // DepotAI                     W--V Taxi Depot AD (vehicle count can be deteremined, but Taxi Depot is treated like it has unlimited)
        // DepotAI                     W--U Bus Depot BG, Biofuel Bus Depot GC, Trolleybus Depot SH, Tram Depot SF, Ferry Depot MT, Helicopter Depot SH, Blimp Depot MT, Sightseeing Bus Depot PL
        //    CableCarStationAI        W--U Cable Car Stop MT, End-of-Line Cable Car Stop MT
        //    TransportStationAI       W--- Bus Station AD, Helicopter Stop SH, Blimp Stop MT
        //    TransportStationAI       W--U Intercity Bus Station SH, Intercity Bus Terminal SH, Metro Station BG, Elevated Metro Station BG, Underground Metro Station BG, 
        //                                  Train Station BG, Airport BG, Monorail Station MT, Monorail Station with Road MT, 
        //                                  Bus-Intercity Bus Hub SH (aka Transport Hub 02 A), Bus-Metro Hub SH (aka Transport Hub 05 A), Metro-Intercity Bus Hub SH (aka Transport Hub 01 A),
        //                                  Train-Metro Hub SH (aka Transport Hub 03 A), Multiplatform End Station MT, Multiplatform Train Station MT,
        //                                  International Airport AD, Metropolitan Airport SH (aka Transport Hub 04 A), Monorail-Bus Hub MT, Metro-Monorail-Train Hub MT
        //       HarborAI              W--- Ferry Stop MT, Ferry Pier MT, Ferry and Bus Exchange Stop MT
        //       HarborAI              W--U Harbor BG
        // DisasterResponseBuildingAI  W--V Disaster Response Unit ND
        // DoomsdayVaultAI             W--- Doomsday Vault ND (monument)
        // EarthquakeSensorAI          W--- Earthquake Sensor ND
        // EldercareAI                 WV-- Eldercare BG
        // FireStationAI               W--V Fire House BG, Fire Station BG
        // FirewatchTowerAI            W--- Firewatch Tower ND
        // FishFarmAI                  W-SV Fish Farm SH, Algae Farm SH, Seaweed Farm SH
        // FishingHarborAI             W-SV Fishing Harbor SH, Anchovy Fishing Harbor SH, Salmon Fishing Harbor SH, Shellfish Fishing Harbor SH, Tuna Fishing Harbor SH
        // HadronColliderAI            W--- Hadron Collider BG (monument)
        // HeatingPlantAI              W--- Boiler Station SF, Geothermal Heating Plant SF
        // HelicopterDepotAI           W--V Medical Helicopter Depot ND, Fire Helicopter Depot ND, Police Helicopter Depot ND
        // HospitalAI                  WV-- Medical Laboratory HT (CCP)
        // HospitalAI                  WV-V Medical Clinic BG, Hospital BG, General Hospital SH (CCP)
        //    MedicalCenterAI          WV-V Medical Center BG (monument)
        // IndustryBuildingAI          ---- (base clase with no buildings)
        //    AuxiliaryBuildingAI      W--- Forestry:  IN: Forestry Workers’ Barracks, Forestry Maintenance Building
        //                                  Farming:   IN: Farm Workers’ Barracks, Farm Maintenance Building
        //                                  Ore:       IN: Ore Industry Workers’ Barracks, Ore Industry Maintenance Building
        //                                  Oil:       IN: Oil Industry Workers’ Barracks, Oil Industry Maintenance Building
        //    ExtractingFacilityAI     W-SV Forestry:  IN: Small Tree Plantation, Medium Tree Plantation, Large Tree Plantation, Small Tree Sapling Greenhouse, Large Tree Sapling Greenhouse
        //                                  Farming:   IN: Small Crops Greenhouse, Medium Crops Greenhouse, Large Crops Greenhouse, Small Fruit Greenhouse, Medium Fruit Greenhouse, Large Fruit Greenhouse
        //                                  Ore:       IN: Small Ore Mine, Medium Ore Mine, Large Ore Mine, Small Ore Mine Underground, Large Ore Mine Underground, Seabed Mining Vessel
        //                                  Oil:       IN: Small Oil Pump, Large Oil Pump, Small Oil Drilling Rig, Large Oil Drilling Rig, Offshore Oil Drilling Platform
        //    ProcessingFacilityAI     W-SV Forestry:  IN: Sawmill, Biomass Pellet Plant, Engineered Wood Plant, Pulp Mill
        //                                  Farming:   IN: Small Animal Pasture, Large Animal Pasture, Flour Mill, Cattle Shed, Milking Parlor, 
        //                                  Ore:       IN: Ore Grinding Mill, Glass Manufacturing Plant, Rotary Kiln Plant, Fiberglass Plant
        //                                  Oil:       IN: Oil Sludge Pyrolysis Plant, Petrochemical Plant, Waste Oil Refining Plant, Naphtha Cracker Plant
        //                                  Fishing:   SH: Fish Factory
        //       UniqueFactoryAI       W-SV IN: Furniture Factory, Bakery, Industrial Steel Plant, Household Plastic Factory, Toy Factory, Printing Press, Lemonade Factory, Electronics Factory,
        //                                      Clothing Factory, Petroleum Refinery, Soft Paper Factory, Car Factory, Food Factory, Sneaker Factory, Modular House Factory, Shipyard
        // LandfillSiteAI              W-SV Landfill Site BG, Incineration Plant BG, Recycling Center GC, Waste Transfer Facility SH, Waste Processing Complex SH, Waste Disposal Unit SH (CCP)
        //    UltimateRecyclingPlantAI W-SV Ultimate Recycling Plant GC (monument)
        // LibraryAI                   WV-- Public Library BG
        // MainCampusBuildingAI        W--- Trade School Administration Building CA, Liberal Arts Administration Building CA, University Administration Building CA
        // MainIndustryBuildingAI      W--- Forestry Main Building IN, Farm Main Building IN, Ore Industry Main Building IN, Oil Industry Main Building IN
        // MaintenanceDepotAI          W--V Road Maintenance Depot SF, Park Maintenance Building PL
        // MarketAI                    WV-- Fish Market SH
        // MonumentAI                  WV-V Landmarks:          ChirpX Launch Site BG
        // MonumentAI                  WV-- Landmarks:          Hypermarket BG, Government Offices BG, The Gherkin BG, London Eye BG, Sports Arena BG, Theatre BG, Shopping Center BG,
        //                                                      Cathedral BG, Amsterdam Palace BG, Winter Market BG, Department Store BG, City Hall BG, Cinema BG,
        //                                                      Panda Sanctuary PE, Oriental Pearl Tower PE, Temple Complex PE,
        //                                                      Traffic Park MT, Boat Museum MT, Locomotive Halls MT
        //                                  Deluxe Edition:     Statue of Liberty DE, Eiffel Tower DE, Grand Central Terminal DE, Arc de Triomphe DE, Brandenburg Gate DE
        //                                  Tourism & Leisure:  Icefishing Pond AD+SF, Casino AD, Driving Range AD, Fantastic Fountain AD, Frozen Fountain AD+SF, Luxury Hotel AD, Zoo AD
        //                                  Winter Unique:      Ice Hockey Arena SF, Ski Resort SF, Snowcastle Restaurant SF, Spa Hotel SF, Sleigh Ride SF, Snowboard Arena SF, The Christmas Tree SF, Igloo Hotel SF
        //                                  Match Day:          Football Stadium MD
        //                                  Concerts:           Festival Area CO, Media Broadcast Building CO, Music Club CO, Fan Zone Park CO
        //                                  Level 1 Unique:     Statue of Industry BG, Statue of Wealth BG, Lazaret Plaza BG, Statue of Shopping BG, Plaza of the Dead BG,
        //                                                      Meteorite Park ND, Bird and Bee Haven GC, City Arch PL
        //                                  Level 2 Unique:     Fountain of Life and Death BG, Friendly Neighborhood Park BG, Transport Tower BG, Mall of Moderation BG, Posh Mall BG,
        //                                                      Disaster Memorial ND, Climate Research Station GC, Clock Tower PL
        //                                  Level 3 Unique:     Colossal Order Offices BG, Official Park BG, Court House BG, Grand Mall BG, Tax Office BG,
        //                                                      Helicopter Park ND, Lungs of the City GC, Old Market Street PL
        //                                  Level 4 Unique:     Business Park BG, Grand Library BG, Observatory BG, Opera House BG, Oppression Office BG,
        //                                                      Pyramid Of Safety ND, Floating Gardens GC, Sea Fortress PL
        //                                  Level 5 Unique:     Servicing Services Offices BG, Academic Library BG, Science Center BG, Expo Center BG, High Interest Tower BG, Aquarium BG,
        //                                                      Sphinx Of Scenarios ND, Ziggurat Garden GC, Observation Tower PL
        //                                  Level 6 Unique:     Cathedral of Plenitude BG, Stadium BG, MAM Modern Art Museum BG, Sea-and-Sky Scraper BG, Theater of Wonders BG,
        //                                                      Sparkly Unicorn Rainbow Park ND, Central Park GC, The Statue of Colossalus PL
        //                                  Content Creator:    Eddie Kovanago AR, Pinoa Street AR, The Majesty AR, Electric Car Factory HT, Nanotechnology Center HT, Research Center HT,
        //                                                      Robotics Institute HT, Semiconductor Plant HT, Software Development Studio HT, Space Shuttle Launch Site HT, Television Station HT,
        //                                                      Drive-in Restaurant MJ, Drive-in Oriental Restaurant MJ, Oriental Market MJ, Noodle Restaurant MJ, Ramen Restaurant MJ,
        //                                                      Service Station and Restaurant MJ, Small Office Building MJ, City Office Building MJ, District Office Building MJ,
        //                                                      Local Register Office MJ, Resort Hotel MJ, Downtown Hotel MJ, Temple MJ, High-rise Office Building MJ,
        //                                                      Company Headquarters MJ, Office Skyscraper MJ, The Station Department Store MJ, The Rail Yard Shopping Center MJ
        //    AnimalMonumentAI         WV-- Winter Unique:   Santa Claus' Workshop SF
        //    ChirpwickCastleAI        WV-- Castle Of Lord Chirpwick PL (monument)
        //    MuseumAI                 WV-- The Technology Museum CA, The Art Gallery CA, The Science Center CA
        //    PrivateAirportAI         WV-V Aviation Club SH (Level 5 Unique)
        //    VarsitySportsArenaAI     WV-- Aquatics Center CA, Basketball Arena CA, Track And Field Stadium CA, Baseball Park CA, American Football Stadium CA
        // ParkAI                      -V-- Parks:  Small Park BG, Small Playground BG, Park With Trees BG, Large Playground BG, Bouncy Castle Park BG, Botanical Garden BG,
        //                                          Dog Park BG, Carousel Park BG, Japanese Garden BG, Tropical Garden BG, Fishing Island BG, Floating Cafe BG,
        //                                          Snowmobile Track AD+SF, Winter Fishing Pier AD+SF, Ice Hockey Rink AD+SF
        //                                  Plazas:             Plaza with Trees BG, Plaza with Picnic Tables BG, Paradox Plaza BG (special)
        //                                  Other Parks:        Basketball Court BG, Tennis Court BG
        //                                  Tourism & Leisure:  Fishing Pier AD, Fishing Tours AD, Jet Ski Rental AD, Marina AD, Restaurant Pier AD, Beach Volleyball Court AD, Riding Stable AD, Skatepark AD
        //                                  Winter Parks:       Snowman Park SF, Ice Sculpture Park SF, Sledding Hill SF, Curling Park SF, Skating Rink SF, Ski Lodge SF, Cross-Country Skiing Park SF, Firepit Park SF
        //                                  Content Creator:    Biodome HT, Vertical Farm HT
        //    EdenProjectAI            -V-- Eden Project BG (monument)
        // ParkBuildingAI              WV-- Only Amusement Park and Zoo have workers.
        //                                  City Park:       PL: Park Plaza, Park Cafe #1, Park Restrooms #1, Park Info Booth #1, Park Chess Board #1, Park Pier #1, Park Pier #2
        //                                  Amusement Park:  PL: Amusement Park Plaza, Amusement Park Cafe #1, Amusement Park Souvenir Shop #1, Amusement Park Restrooms #1, Game Booth #1, Game Booth #2,
        //                                                       Carousel, Piggy Train, Rotating Tea Cups, Swinging Boat, House Of Horrors, Bumper Cars, Drop Tower Ride, Pendulum Ride, Ferris Wheel, Rollercoaster
        //                                  Zoo:             PL: Zoo Plaza, Zoo Cafe #1, Zoo Souvenir Shop #1, Zoo Restrooms #1, Moose And Reindeer Enclosure, Bird House, Antelope Enclosure, Bison Enclosure,
        //                                                       {Insect, Amphibian and Reptile House}, Flamingo Enclosure, Elephant Enclosure, Sealife Enclosure, Giraffe Enclosure, Monkey Palace, Rhino Enclosure, Lion Enclosure
        //                                  Nature Reserve:  PL: Campfire Site #1, Campfire Site #2, Tent #1, Tent #2, Tent #3, Viewing Deck #1, Viewing Deck #2, Tent Camping Site #1, Lean-To Shelter #1, Lean-To Shelter #2,
        //                                                       Lookout Tower #1, Lookout Tower #2, Camping Site #1, Fishing Cabin #1, Fishing Cabin #2, Hunting Cabin #1, Hunting Cabin #2, Bouldering Site #1
        // ParkGateAI                  W--- City Park:       PL: Park Main Gate, Small Park Main Gate, Park Side Gate
        //                                  Amusement Park:  PL: Amusement Park Main Gate, Small Amusement Park Main Gate, Amusement Park Side Gate
        //                                  Zoo:             PL: Zoo Main Gate, Small Zoo Main Gate, Zoo Side Gate
        //                                  Nature Reserve:  PL: Nature Reserve Main Gate, Small Nature Reserve Main Gate, Nature Reserve Side Gate
        // PoliceStationAI             WV-V Police Station BG, Police Headquarters BG, Prison AD, Intelligence Agency HT (CCP)
        // PostOfficeAI                W-SV Post Office IN, Post Sorting Facility IN
        // PowerPlantAI                W--- Coal Power Plant BG, Oil Power Plant BG, Nuclear Power Plant BG, Geothermal Power Plant GC, Ocean Thermal Energy Conversion Plant GC
        //                                  (unlimited coal/oil reserves so cannot compute storage)
        //    DamPowerHouseAI          W--- Hydro Power Plant BG
        //    FusionPowerPlantAI       W--- Fusion Power Plant BG (monument)
        //    SolarPowerPlantAI        W--- Solar Power Plant BG, Solar Updraft Tower GC
        //    WindTurbineAI            W--- Wind Turbine BG, Advanced Wind Turbine BG, Wave Power Plant HT (CCP)
        // RadioMastAI                 W--- Short Radio Mast ND, Tall Radio Mast ND
        // SaunaAI                     WV-- Sauna SF, Sports Hall and Gymnasium GC, Community Pool GC, Yoga Garden GC
        // SchoolAI                    WV-- Elementary School BG, High School BG, University BG, Community School GC, Institute of Creative Arts GC, Modern Technology Institute GC, Faculty HT (CCP)
        //    CampusBuildingAI         WV-- Trade School:   CA: Trade School Dormitory, Trade School Study Hall, Trade School Groundskeeping, Book Club, Trade School Outdoor Study, Trade School Gymnasium, Trade School Cafeteria,
        //                                                      Trade School Fountain, Trade School Library, IT Club, Trade School Commencement Office, Trade School Academic Statue 1, Trade School Auditorium, Trade School Laboratories,
        //                                                      Trade School Bookstore, Trade School Media Lab, Beach Volleyball Club, Trade School Academic Statue 2
        //                                  Liberal Arts:   CA: Liberal Arts Dormitory, Liberal Arts Study Hall, Liberal Arts Groundskeeping, Drama Club, Liberal Arts Outdoor Study, Liberal Arts Gymnasium, Liberal Arts Cafeteria,
        //                                                      Liberal Arts Fountain, Liberal Arts Library, Art Club, Liberal Arts Commencement Office, Liberal Arts Academic Statue 1, Liberal Arts Auditorium, Liberal Arts Laboratories,
        //                                                      Liberal Arts Bookstore, Liberal Arts Media Lab, Dance Club, Liberal Arts Academic Statue 2
        //                                  University:     CA: University Dormitory, University Study Hall, University Groundskeeping, Futsal Club, University Outdoor Study, University Gymnasium, University Cafeteria
        //                                                      University Fountain, University Library, Math Club, University Commencement Office, University Academic Statue 1, University Auditorium, University Laboratories,
        //                                                      University Bookstore, University Media Lab, Chess Club, University Academic Statue 2
        //       UniqueFacultyAI       WV-- Trade School:   CA: Police Academy, School of Tourism And Travel, School of Engineering
        //                                  Liberal Arts:   CA: School of Education, School of Environmental Studies, School of Economics
        //                                  University:     CA: School of Law, School of Medicine, School of Science
        // ShelterAI                   WV-V Small Emergency Shelter ND, Large Emergency Shelter ND
        // SnowDumpAI                  W-SV Snow Dump SF
        // SpaceElevatorAI             W--- Space Elevator BG (monument)
        // SpaceRadarAI                W--- Deep Space Radar ND
        // TaxiStandAI                 ---- Taxi Stand AD (taxis wait at a Taxi Stand for a customer, taxis are not generated by a Taxi Stand)
        // TollBoothAI                 ---- Two-Way Toll Booth BG, One-Way Toll Booth BG, Two-Way Large Toll Booth BG, One-Way Large Toll Booth BG
        // TourBuildingAI              -V-U Hot Air Balloon Tours PL
        // TsunamiBuoyAI               ---- Tsunami Warning Buoy ND
        // WarehouseAI                 W-SV Forestry:  IN: Small Log Yard, Saw Dust Storage, Large Log Yard, Wood Chip Storage
        //                                  Farming:   IN: Small Grain Silo, Large Grain Silo, Small Barn, Large Barn
        //                                  Ore:       IN: Sand Storage, Ore Storage, Ore Industry Storage, Raw Mineral Storage
        //                                  Oil:       IN: Small Crude Oil Tank Farm, Large Crude Oil Tank Farm, Crude Oil Storage Cavern, Oil Industry Storage
        //                                  Generic:   IN: Warehouse Yard, Small Warehouse, Medium Warehouse, Large Warehouse
        // WaterCleanerAI              W--- Floating Garbage Collector GC
        // WaterFacilityAI             W--- Water Pumping Station BG, Water Tower BG, Large Water Tower SH, Water Drain Pipe BG, Water Treatment Plant BG,
        //                                  Inland Water Treatment Plant SH, Advanced Inland Water Treatment Plant SH, Eco Water Outlet GC, Eco Water Treatment Plant GC,
        //                                  Eco Inland Water Treatment Plant SH, Eco Advanced Inland Water Treatment Plant SH, Fresh Water Outlet ND
        // WaterFacilityAI             W-S- Tank Reservoir ND
        // WaterFacilityAI             W-SV Pumping Service ND
        // WeatherRadarAI              W--- Weather Radar ND

        // building AI types that have been patched in this mod
        private static List<Type> _buildingAITypes = new List<Type>();

        /// <summary>
        /// create a patch of the GetColor method for the specified building AI type
        /// </summary>
        public static void CreateGetColorPatch(Type buildingAIType)
        {
            // each building AI type is patched only once
            // the patch logic determines which panel is displayed and calls GetBuildingColor on that panel
            // this way of doing it avoids creating a separate patch for each panel that has the same building AI
            if (_buildingAITypes.Contains(buildingAIType))
            {
                return;
            }

            // get the original GetColor method
            MethodInfo original = buildingAIType.GetMethod("GetColor");
            if (original == null)
            {
                Debug.LogError($"Unable to find GetColor method for building AI type {buildingAIType.ToString()}.");
                return;
            }

            // find the Prefix method
            MethodInfo prefix = typeof(BuildingAIPatch).GetMethod("Prefix", BindingFlags.Public | BindingFlags.Static);
            if (prefix == null)
            {
                Debug.LogError($"Unable to find BuildingAIPatch.Prefix method.");
                return;
            }

            // create the patch
            BuildingUsage.harmony.Patch(original, new HarmonyMethod(prefix), null, null);

            // a patch was created for this building AI type
            _buildingAITypes.Add(buildingAIType);
        }

        /// <summary>
        /// return the color of the building
        /// same Prefix routine is used for all building AI types
        /// </summary>
        /// <returns>whether or not to do base processing</returns>
        public static bool Prefix(ushort buildingID, ref Building data, InfoManager.InfoMode infoMode, ref Color __result)
        {
            // do processing for this mod only for Levels info view
            bool doBaseProcessing = true;
            if (infoMode == InfoManager.InfoMode.BuildingLevel)
            {
                // check which tab is selected
                switch (BuildingUsage.selectedTab)
                {
                    case BuildingUsage.LevelsInfoViewTab.Levels:
                        doBaseProcessing = true;
                        break;

                    case BuildingUsage.LevelsInfoViewTab.Workers:
                        doBaseProcessing = false;
                        __result = BuildingUsage.workersUsagePanel.GetBuildingColor(buildingID, ref data);
                        break;

                    case BuildingUsage.LevelsInfoViewTab.Visitors:
                        doBaseProcessing = false;
                        __result = BuildingUsage.visitorsUsagePanel.GetBuildingColor(buildingID, ref data);
                        break;

                    case BuildingUsage.LevelsInfoViewTab.Storage:
                        doBaseProcessing = false;
                        __result = BuildingUsage.storageUsagePanel.GetBuildingColor(buildingID, ref data);
                        break;

                    case BuildingUsage.LevelsInfoViewTab.Vehicles:
                        doBaseProcessing = false;
                        __result = BuildingUsage.vehiclesUsagePanel.GetBuildingColor(buildingID, ref data);
                        break;
                }
            }

            // return whether or not to do the base processing
            return doBaseProcessing;
        }

        /// <summary>
        /// clear the building AI patches
        /// </summary>
        public static void ClearPatches()
        {
            _buildingAITypes.Clear();
        }
    }
}
