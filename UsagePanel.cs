using ColossalFramework.UI;
using ColossalFramework.Globalization;
using ColossalFramework.Math;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BuildingUsage
{
    public abstract class UsagePanel : UIPanel
    {
        // define the usage types
        public enum UsageType
        {
            // special usage types
            None,           // no usage type
            UseLogic1,      // must use logic to determine a usage type 1
            UseLogic2,      // must use logic to determine a usage type 2

            // workers usage types
            HouseholdsResidential,
            WorkersCommercial,
            WorkersOffice,
            WorkersIndustrial,
            WorkersMaintenance,
            WorkersPowerPlant,
            WorkersWaterSewage,
            WorkersHeatingPlant,
            WorkersGarbage,
            WorkersIndustry,
            WorkersMedical,
            WorkersCemetery,
            WorkersFireStation,
            WorkersDisaster,
            WorkersPoliceStation,
            WorkersBank,
            WorkersEducation,
            WorkersTransportation,
            WorkersPost,
            WorkersAmusementPark,
            WorkersZoo,
            WorkersHotels,
            WorkersUnique,

            // workers industry detail usage types
            WorkersIndustryForestryMainAux,
            WorkersIndustryForestryExtractor,
            WorkersIndustryForestryProcessor,
            WorkersIndustryForestryStorage,
            WorkersIndustryFarmingMainAux,
            WorkersIndustryFarmingExtractor,
            WorkersIndustryFarmingProcessor,
            WorkersIndustryFarmingStorage,
            WorkersIndustryOreMainAux,
            WorkersIndustryOreExtractor,
            WorkersIndustryOreProcessor,
            WorkersIndustryOreStorage,
            WorkersIndustryOilMainAux,
            WorkersIndustryOilExtractor,
            WorkersIndustryOilProcessor,
            WorkersIndustryOilStorage,
            WorkersIndustryFishingExtractor,
            WorkersIndustryFishingProcessor,
            WorkersIndustryFishingMarket,
            WorkersIndustryWarehouse,
            WorkersIndustryUniqueFactory,

            // workers education detail usage types
            WorkersEducationElementarySchool,
            WorkersEducationHighSchool,
            WorkersEducationUniversity,
            WorkersEducationLibrary,
            WorkersEducationHadronCollider,
            WorkersEducationTradeSchoolAdmin,
            WorkersEducationTradeSchoolEducation,
            WorkersEducationTradeSchoolFaculty,
            WorkersEducationLiberalArtsAdmin,
            WorkersEducationLiberalArtsEducation,
            WorkersEducationLiberalArtsFaculty,
            WorkersEducationUniversityAdmin,
            WorkersEducationUniversityEducation,
            WorkersEducationUniversityFaculty,
            WorkersEducationMuseum,
            WorkersEducationVarsitySports,

            // workers transportation detail usage types
            WorkersTransportationBus,
            WorkersTransportationIntercityBus,
            WorkersTransportationTrolleybus,
            WorkersTransportationTram,
            WorkersTransportationMetro,
            WorkersTransportationTrainPeople,
            WorkersTransportationShipPeople,
            WorkersTransportationAirPeople,
            WorkersTransportationMonorail,
            WorkersTransportationCableCar,
            WorkersTransportationTaxi,
            WorkersTransportationTours,
            WorkersTransportationHubs,
            WorkersTransportationSpaceElevator,
            WorkersTransportationTrainCargo,
            WorkersTransportationShipCargo,
            WorkersTransportationAirCargo,

            // workers unique building detail usage types
            WorkersUniqueFinancial,
            WorkersUniqueLandmark,
            WorkersUniqueTourismLeisure,
            WorkersUniqueWinterUnique,
            WorkersUniquePedestrianArea,
            WorkersUniqueFootball,
            WorkersUniqueConcert,
            WorkersUniqueAirports,
            WorkersUniqueChirpwickCastle,
            WorkersUniqueLevel1,
            WorkersUniqueLevel2,
            WorkersUniqueLevel3,
            WorkersUniqueLevel4,
            WorkersUniqueLevel5,
            WorkersUniqueLevel6,
            WorkersUniqueCCPArtDeco,
            WorkersUniqueCCPHighTech,
            WorkersUniqueCCPModernJapan,
            WorkersUniqueCCPSeasideResorts,
            WorkersUniqueCCPSkyscrapers,
            WorkersUniqueCCPHeartOfKorea,
            WorkersUniqueCCPShoppingMalls,
            WorkersUniqueCCPSportsVenues,
            WorkersUniqueCCPAfricaInMiniature,
            WorkersUniqueCCPRailroadsOfJapan,


            // visitors usage types
            VisitorsFishMarket,
            VisitorsMedicalPatients,
            VisitorsMedicalVisitors,
            VisitorsDeceased,
            VisitorsShelter,
            VisitorsCriminals,
            VisitorsEducation,
            VisitorsAirportArea,
            VisitorsParksPlazas,
            VisitorsUnique,

            // visitors education detail usage types
            VisitorsEducationElementarySchool,
            VisitorsEducationHighSchool,
            VisitorsEducationUniversity,
            VisitorsEducationLibrary,
            VisitorsEducationTradeSchoolEducation,
            VisitorsEducationTradeSchoolFaculty,
            VisitorsEducationLiberalArtsEducation,
            VisitorsEducationLiberalArtsFaculty,
            VisitorsEducationUniversityEducation,
            VisitorsEducationUniversityFaculty,
            VisitorsEducationMuseum,
            VisitorsEducationVarsitySports,

            // visitors parks and plazas detail usage types
            VisitorsParksPlazasParks,
            VisitorsParksPlazasPlazas,
            VisitorsParksPlazasOtherParks,
            VisitorsParksPlazasTourismLeisure,
            VisitorsParksPlazasWinterkParks,
            VisitorsParksPlazasPedestrianPlazas,
            VisitorsParksPlazasHotels,
            VisitorsParksPlazasEdenProject,
            VisitorsParksPlazasCityPark,
            VisitorsParksPlazasAmusementPark,
            VisitorsParksPlazasZoo,
            VisitorsParksPlazasNatureReserve,
            VisitorsParksPlazasTours,
            VisitorsParksPlazasCCPHighTech,
            VisitorsParksPlazasCCPBridgesPiers,
            VisitorsParksPlazasCCPMidCenturyModern,
            VisitorsParksPlazasCCPSportsVenues,
            VisitorsParksPlazasCCPAfricaInMiniature,
            VisitorsParksPlazasCCPRailroadsOfJapan,

            // visitors unique building detail usage types
            VisitorsUniqueFinancial,
            VisitorsUniqueLandmark,
            VisitorsUniqueTourismLeisure,
            VisitorsUniqueWinterUnique,
            VisitorsUniquePedestrianArea,
            VisitorsUniqueFootball,
            VisitorsUniqueConcert,
            VisitorsUniqueAirports,
            VisitorsUniqueChirpwickCastle,
            VisitorsUniqueLevel1,
            VisitorsUniqueLevel2,
            VisitorsUniqueLevel3,
            VisitorsUniqueLevel4,
            VisitorsUniqueLevel5,
            VisitorsUniqueLevel6,
            VisitorsUniqueCCPArtDeco,
            VisitorsUniqueCCPHighTech,
            VisitorsUniqueCCPModernJapan,
            VisitorsUniqueCCPSeasideResorts,
            VisitorsUniqueCCPSkyscrapers,
            VisitorsUniqueCCPHeartOfKorea,
            VisitorsUniqueCCPShoppingMalls,
            VisitorsUniqueCCPSportsVenues,
            VisitorsUniqueCCPAfricaInMiniature,
            VisitorsUniqueCCPRailroadsOfJapan,


            // storage usage types
            StorageSnow,
            StorageWater,
            StorageGarbage,
            StorageIndustry,
            StoragePostUnsorted,
            StoragePostSorted,
            StorageCommercialCash,

            // storage industry usage types
            StorageIndustryForestryExtractor,
            StorageIndustryForestryProcessorInput,
            StorageIndustryForestryProcessorOutput,
            StorageIndustryForestryStorage,
            StorageIndustryFarmingExtractor,
            StorageIndustryFarmingProcessorInput,
            StorageIndustryFarmingProcessorOutput,
            StorageIndustryFarmingStorage,
            StorageIndustryOreExtractor,
            StorageIndustryOreProcessorInput,
            StorageIndustryOreProcessorOutput,
            StorageIndustryOreStorage,
            StorageIndustryOilExtractor,
            StorageIndustryOilProcessorInput,
            StorageIndustryOilProcessorOutput,
            StorageIndustryOilStorage,
            StorageIndustryFishingExtractor,
            StorageIndustryFishingProcessorInput,
            StorageIndustryFishingProcessorOutput,
            StorageIndustryUniqueFactoryInput,
            StorageIndustryUniqueFactoryOutput,
            StorageIndustryWarehouseGeneric,


            // vehicles usage types
            VehiclesIndustrialTrucks,
            VehiclesMaintenanceTrucks,
            VehiclesVacuumTrucks,
            VehiclesGarbageTrucks,
            VehiclesIndustryVehicles,
            VehiclesAmbulances,
            VehiclesMedicalHelis,
            VehiclesHearses,
            VehiclesFireEngines,
            VehiclesFireHelis,
            VehiclesDisasterVehicles,
            VehiclesEvacuationBuses,
            VehiclesPoliceCars,
            VehiclesPoliceHelis,
            VehiclesPrisonVans,
            VehiclesBankCashVans,
            VehiclesPostVansTrucks,
            VehiclesPrivatePlanes,
            VehiclesRockets,
            VehiclesTransportation,

            // vehicles industry usage types
            VehiclesIndustryForestryExtractor,
            VehiclesIndustryForestryProcessor,
            VehiclesIndustryForestryStorage,
            VehiclesIndustryFarmingExtractor,
            VehiclesIndustryFarmingProcessor,
            VehiclesIndustryFarmingStorage,
            VehiclesIndustryOreExtractor,
            VehiclesIndustryOreProcessor,
            VehiclesIndustryOreStorage,
            VehiclesIndustryOilExtractor,
            VehiclesIndustryOilProcessor,
            VehiclesIndustryOilStorage,
            VehiclesIndustryFishingExtractor,
            VehiclesIndustryFishingProcessor,
            VehiclesIndustryUniqueFactory,
            VehiclesIndustryWarehouseGeneric,

            // vehicles transportation usage types
            VehiclesTransportationBus,
            VehiclesTransportationIntercityBus,
            VehiclesTransportationTrolleybus,
            VehiclesTransportationTram,
            VehiclesTransportationMetro,
            VehiclesTransportationTrainPeople,
            VehiclesTransportationShipPeople,
            VehiclesTransportationAirPeople,
            VehiclesTransportationMonorail,
            VehiclesTransportationCableCar,
            VehiclesTransportationTaxi,
            VehiclesTransportationTours,
            VehiclesTransportationHubs,
            VehiclesTransportationTrainCargo,
            VehiclesTransportationShipCargo,
            VehiclesTransportationAirCargo,
        }

        // define a class to hold info for a thumbnail image
        private class ThumbnailInfo
        {
            public enum AtlasType
            {
                InGame,
                Thumbnails,
                Expansion9
            }
            public AtlasType atlasType;
            public string spriteNameNormal;
            public string spriteNameDisabled => (spriteNameNormal == "SubBarFireDepartmentFire") ? "SubBarFireDepartmentFireFocused" : spriteNameNormal + "Disabled";
            public float width;
            public float height;

            private ThumbnailInfo() { }

            public ThumbnailInfo(AtlasType atlasType, string spriteNameNormal, float width, float height)
            {
                this.atlasType = atlasType;
                this.spriteNameNormal = spriteNameNormal;
                this.width = width;
                this.height = height;
            }
        }

        #region Thumbnails
        // define the thumbnail images for usage groups
        private static readonly ThumbnailInfo thumbnailInfoResidential          = new ThumbnailInfo(ThumbnailInfo.AtlasType.Thumbnails, "ZoningResidentialLow",                     109f, 75f);
        private static readonly ThumbnailInfo thumbnailInfoCommercial           = new ThumbnailInfo(ThumbnailInfo.AtlasType.Thumbnails, "ZoningCommercialLow",                      109f, 75f);
        private static readonly ThumbnailInfo thumbnailInfoOffice               = new ThumbnailInfo(ThumbnailInfo.AtlasType.Thumbnails, "ZoningOffice",                             109f, 75f);
        private static readonly ThumbnailInfo thumbnailInfoIndustrial           = new ThumbnailInfo(ThumbnailInfo.AtlasType.Thumbnails, "ZoningIndustrial",                         109f, 75f);
        private static readonly ThumbnailInfo thumbnailInfoRoadMaintenance      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarRoadsMaintenance",                   32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoElectricy            = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarElectricityDefault",                 32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoWater                = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "InfoIconWater",                            36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoGarbage              = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "InfoIconGarbage",                          36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoIndustry             = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ToolbarIconGarbage",                       41f, 41f);
        private static readonly ThumbnailInfo thumbnailInfoIndustryMainAux      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "UIFilterIndustryMainAndAuxiliary",         36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoExtractor            = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "UIFilterExtractorBuildings",               36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoFishingExtractor     = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "UIFilterFishingExtractorBuildings",        36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoIndustryFishing      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarIndustryFishing",                    32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoProcessing           = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "UIFilterProcessingBuildings",              36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoUniqueFactory        = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarIndustryUniqueFactory",              36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoStorage              = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "UIFilterStorageBuildings",                 36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoWarehouses           = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarIndustryWarehouses",                 36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoHealthCare           = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarHealthcareDefault",                  32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoFire                 = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarFireDepartmentFire",                 32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoDisaster             = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarFireDepartmentDisaster",             32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoPolice               = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPoliceDefault",                      32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBanks                = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBanks",                              32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoMoney                = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ToolbarIconMoney",                         41f, 41f);
        private static readonly ThumbnailInfo thumbnailInfoEducation            = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarEducationDefault",                   32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBigBuildings         = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "UIFilterBigBuildings",                     36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoEducationBuildings   = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "UIFilterEducationBuildings",               36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoFaculties            = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "UIFilterFaculties",                        36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoMusemum              = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarCampusAreaMuseums",                  32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoVarsitySports        = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarCampusAreaVarsitySports",            32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportation       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "InfoIconPublicTransport",                  36f, 36f);
        private static readonly ThumbnailInfo thumbnailInfoTransportBus         = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportBus",                 32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportTrolleybus  = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportTrolleybus",          32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportTram        = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportTram",                32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportMetro       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportMetro",               32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportTrain       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportTrain",               32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportShip        = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportShip",                32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportPlane       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportPlane",               32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportAirports    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportAirportArea",         32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportMonorail    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportMonorail",            32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportCableCar    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportCableCar",            32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportTaxi        = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportTaxi",                32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportTours       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportTours",               32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportPost        = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportPost",                32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoTransportHubs        = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarPublicTransportHubs",                32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautification       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ToolbarIconBeautification",                41f, 41f);
        private static readonly ThumbnailInfo thumbnailInfoBeautParks           = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationParks",                32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautPlazas          = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationPlazas",               32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautOtherParks      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationOthers",               32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautExpansion1      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationExpansion1",           32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautExpansion2      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationExpansion2",           32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautCityPark        = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationCityPark",             32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautAmusementPark   = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationAmusementPark",        32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautZoo             = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationZoo",                  32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautNatureReserve   = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationNatureReserve",        32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautPedestrian      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationPedestrianZonePlazas", 32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoBeautHotels          = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarBeautificationHotels",               32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoUniqueBuilding       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ToolbarIconMonuments",                     41f, 41f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentPudding      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentPudding",                    32f, 22f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentLandmarks    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentLandmarks",                  32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentExpansion1   = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentExpansion1",                 32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentExpansion2   = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentExpansion2",                 32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentCupcake      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentCupcake",                    32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentFootball     = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentFootball",                   32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentConcerts     = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentConcerts",                   32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentCategory1    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentCategory1",                  32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentCategory2    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentCategory2",                  32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentCategory3    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentCategory3",                  32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentCategory4    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentCategory4",                  32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentCategory5    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentCategory5",                  32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoMonumentCategory6    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SubBarMonumentCategory6",                  32f, 32f);
        private static readonly ThumbnailInfo thumbnailInfoWonders              = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ToolbarIconWonders",                       41f, 41f);
        private static readonly ThumbnailInfo thumbnailInfoAviationClub         = new ThumbnailInfo(ThumbnailInfo.AtlasType.Expansion9, "Thumb_Aviation Club",                      109f, 100f);
        private static readonly ThumbnailInfo thumbnailInfoChirpXLaunchSite     = new ThumbnailInfo(ThumbnailInfo.AtlasType.Thumbnails, "ThumbChirpX",                              109f, 100f);
        private static readonly ThumbnailInfo thumbnailInfoBuyArtDeco           = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ArtDecoBuyButton",                         52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuyHighTech          = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "HighTechBuyButton",                        52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuyModernJapan       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "Modderpack6BuyButton",                     52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuyBridgesPiers      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "Modderpack8BuyButton",                     52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuyMidCenturyModern  = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "MidCenturyModernBuyButton",                52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuySeasideResorts    = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "SeasideResortsBuyButton",                  52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuySkyscrapers       = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ProjectSkyscrapersBuyButton",              52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuyHeartOfKorea      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "HeartOfKoreaBuyButton",                    52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuyShoppingMalls     = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ModderPack16BuyButton",                    52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuySportsVenues      = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ModderPack17BuyButton",                    52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuyAfricaMiniature   = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ModderPack18BuyButton",                    52f, 52f);
        private static readonly ThumbnailInfo thumbnailInfoBuyRailroadsOfJapan  = new ThumbnailInfo(ThumbnailInfo.AtlasType.InGame,     "ModderPack19BuyButton",                    52f, 52f);
        #endregion

        // deinfe a class to hold info for a usage type
        private class UsageTypeInfo
        {
            public string descriptionText;
            public ThumbnailInfo thumbnailInfo;

            private UsageTypeInfo() { }

            public UsageTypeInfo(string descriptionText, ThumbnailInfo thumbnailInfo)
            {
                this.descriptionText = descriptionText;
                this.thumbnailInfo = thumbnailInfo;
            }
        }

        // define the description text and thumbnail image info to use with each usage type
        private static readonly Dictionary<UsageType, UsageTypeInfo> _usageTypeInfos = new Dictionary<UsageType, UsageTypeInfo>
        {
            // workers usage types
            { UsageType.HouseholdsResidential,                  new UsageTypeInfo("Residential Zone",   thumbnailInfoResidential        ) },
            { UsageType.WorkersCommercial,                      new UsageTypeInfo("Commercial Zone",    thumbnailInfoCommercial         ) },
            { UsageType.WorkersOffice,                          new UsageTypeInfo("Office Zone",        thumbnailInfoOffice             ) },
            { UsageType.WorkersIndustrial,                      new UsageTypeInfo("Industrial Zone",    thumbnailInfoIndustrial         ) },
            { UsageType.WorkersMaintenance,                     new UsageTypeInfo("Maintenance",        thumbnailInfoRoadMaintenance    ) },
            { UsageType.WorkersPowerPlant,                      new UsageTypeInfo("Power Plant",        thumbnailInfoElectricy          ) },
            { UsageType.WorkersWaterSewage,                     new UsageTypeInfo("Water and Sewage",   thumbnailInfoWater              ) },
            { UsageType.WorkersHeatingPlant,                    new UsageTypeInfo("Heating Plant",      thumbnailInfoWater              ) },
            { UsageType.WorkersGarbage,                         new UsageTypeInfo("Garbage",            thumbnailInfoGarbage            ) },
            { UsageType.WorkersIndustry,                        new UsageTypeInfo("Industry",           thumbnailInfoIndustry           ) },
            { UsageType.WorkersMedical,                         new UsageTypeInfo("Medical",            thumbnailInfoHealthCare         ) },
            { UsageType.WorkersCemetery,                        new UsageTypeInfo("Cemetery",           thumbnailInfoHealthCare         ) },
            { UsageType.WorkersFireStation,                     new UsageTypeInfo("Fire Station",       thumbnailInfoFire               ) },
            { UsageType.WorkersDisaster,                        new UsageTypeInfo("Disaster",           thumbnailInfoDisaster           ) },
            { UsageType.WorkersPoliceStation,                   new UsageTypeInfo("Police Station",     thumbnailInfoPolice             ) },
            { UsageType.WorkersBank,                            new UsageTypeInfo("Bank",               thumbnailInfoBanks              ) },
            { UsageType.WorkersEducation,                       new UsageTypeInfo("Education",          thumbnailInfoEducation          ) },
            { UsageType.WorkersTransportation,                  new UsageTypeInfo("Transportation",     thumbnailInfoTransportation     ) },
            { UsageType.WorkersPost,                            new UsageTypeInfo("Post",               thumbnailInfoTransportPost      ) },
            { UsageType.WorkersAmusementPark,                   new UsageTypeInfo("Amusement Park",     thumbnailInfoBeautAmusementPark ) },
            { UsageType.WorkersZoo,                             new UsageTypeInfo("Zoo",                thumbnailInfoBeautZoo           ) },
            { UsageType.WorkersHotels,                          new UsageTypeInfo("Hotels",             thumbnailInfoBeautHotels        ) },
            { UsageType.WorkersUnique,                          new UsageTypeInfo("Unique Buildings",   thumbnailInfoUniqueBuilding     ) },

            // workers industry usage types
            { UsageType.WorkersIndustryForestryMainAux,         new UsageTypeInfo("Main & Auxiliary",   thumbnailInfoIndustryMainAux    ) },
            { UsageType.WorkersIndustryForestryExtractor,       new UsageTypeInfo("Extractor",          thumbnailInfoExtractor          ) },
            { UsageType.WorkersIndustryForestryProcessor,       new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.WorkersIndustryForestryStorage,         new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.WorkersIndustryFarmingMainAux,          new UsageTypeInfo("Main & Auxiliary",   thumbnailInfoIndustryMainAux    ) },
            { UsageType.WorkersIndustryFarmingExtractor,        new UsageTypeInfo("Extractor",          thumbnailInfoExtractor          ) },
            { UsageType.WorkersIndustryFarmingProcessor,        new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.WorkersIndustryFarmingStorage,          new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.WorkersIndustryOreMainAux,              new UsageTypeInfo("Main & Auxiliary",   thumbnailInfoIndustryMainAux    ) },
            { UsageType.WorkersIndustryOreExtractor,            new UsageTypeInfo("Extractor",          thumbnailInfoExtractor          ) },
            { UsageType.WorkersIndustryOreProcessor,            new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.WorkersIndustryOreStorage,              new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.WorkersIndustryOilMainAux,              new UsageTypeInfo("Main & Auxiliary",   thumbnailInfoIndustryMainAux    ) },
            { UsageType.WorkersIndustryOilExtractor,            new UsageTypeInfo("Extractor",          thumbnailInfoExtractor          ) },
            { UsageType.WorkersIndustryOilProcessor,            new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.WorkersIndustryOilStorage,              new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.WorkersIndustryFishingExtractor,        new UsageTypeInfo("Extractor",          thumbnailInfoFishingExtractor   ) },
            { UsageType.WorkersIndustryFishingProcessor,        new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.WorkersIndustryFishingMarket,           new UsageTypeInfo("Market",             thumbnailInfoIndustryFishing    ) },
            { UsageType.WorkersIndustryWarehouse,               new UsageTypeInfo("Warehouse",          thumbnailInfoWarehouses         ) },
            { UsageType.WorkersIndustryUniqueFactory,           new UsageTypeInfo("Unique Factory",     thumbnailInfoUniqueFactory      ) },

            // workers education usage types
            { UsageType.WorkersEducationElementarySchool,       new UsageTypeInfo("Elementary School",  thumbnailInfoEducation          ) },
            { UsageType.WorkersEducationHighSchool,             new UsageTypeInfo("High School",        thumbnailInfoEducation          ) },
            { UsageType.WorkersEducationUniversity,             new UsageTypeInfo("University",         thumbnailInfoEducation          ) },
            { UsageType.WorkersEducationLibrary,                new UsageTypeInfo("Library",            thumbnailInfoEducation          ) },
            { UsageType.WorkersEducationHadronCollider,         new UsageTypeInfo("Hadron Collider",    thumbnailInfoEducation          ) },
            { UsageType.WorkersEducationTradeSchoolAdmin,       new UsageTypeInfo("Administration",     thumbnailInfoBigBuildings       ) },
            { UsageType.WorkersEducationTradeSchoolEducation,   new UsageTypeInfo("Education & Supp",   thumbnailInfoEducationBuildings ) },
            { UsageType.WorkersEducationTradeSchoolFaculty,     new UsageTypeInfo("Faculties",          thumbnailInfoFaculties          ) },
            { UsageType.WorkersEducationLiberalArtsAdmin,       new UsageTypeInfo("Administration",     thumbnailInfoBigBuildings       ) },
            { UsageType.WorkersEducationLiberalArtsEducation,   new UsageTypeInfo("Education & Supp",   thumbnailInfoEducationBuildings ) },
            { UsageType.WorkersEducationLiberalArtsFaculty,     new UsageTypeInfo("Faculties",          thumbnailInfoFaculties          ) },
            { UsageType.WorkersEducationUniversityAdmin,        new UsageTypeInfo("Administration",     thumbnailInfoBigBuildings       ) },
            { UsageType.WorkersEducationUniversityEducation,    new UsageTypeInfo("Education & Supp",   thumbnailInfoEducationBuildings ) },
            { UsageType.WorkersEducationUniversityFaculty,      new UsageTypeInfo("Faculties",          thumbnailInfoFaculties          ) },
            { UsageType.WorkersEducationMuseum,                 new UsageTypeInfo("Museum",             thumbnailInfoMusemum            ) },
            { UsageType.WorkersEducationVarsitySports,          new UsageTypeInfo("Varsity Sports",     thumbnailInfoVarsitySports      ) },

            // workers transportation usage types
            { UsageType.WorkersTransportationBus,               new UsageTypeInfo("Bus",                thumbnailInfoTransportBus       ) },
            { UsageType.WorkersTransportationIntercityBus,      new UsageTypeInfo("Intercity Bus",      thumbnailInfoTransportBus       ) },
            { UsageType.WorkersTransportationTrolleybus,        new UsageTypeInfo("Trolleybus",         thumbnailInfoTransportTrolleybus) },
            { UsageType.WorkersTransportationTram,              new UsageTypeInfo("Tram",               thumbnailInfoTransportTram      ) },
            { UsageType.WorkersTransportationMetro,             new UsageTypeInfo("Metro",              thumbnailInfoTransportMetro     ) },
            { UsageType.WorkersTransportationTrainPeople,       new UsageTypeInfo("Train",              thumbnailInfoTransportTrain     ) },
            { UsageType.WorkersTransportationShipPeople,        new UsageTypeInfo("Ship",               thumbnailInfoTransportShip      ) },
            { UsageType.WorkersTransportationAirPeople,         new UsageTypeInfo("Air",                thumbnailInfoTransportPlane     ) },
            { UsageType.WorkersTransportationMonorail,          new UsageTypeInfo("Monorail",           thumbnailInfoTransportMonorail  ) },
            { UsageType.WorkersTransportationCableCar,          new UsageTypeInfo("Cable Car",          thumbnailInfoTransportCableCar  ) },
            { UsageType.WorkersTransportationTaxi,              new UsageTypeInfo("Taxi",               thumbnailInfoTransportTaxi      ) },
            { UsageType.WorkersTransportationTours,             new UsageTypeInfo("Tours",              thumbnailInfoTransportTours     ) },
            { UsageType.WorkersTransportationHubs,              new UsageTypeInfo("Hubs",               thumbnailInfoTransportHubs      ) },
            { UsageType.WorkersTransportationSpaceElevator,     new UsageTypeInfo("Space Elevator",     thumbnailInfoWonders            ) },
            { UsageType.WorkersTransportationTrainCargo,        new UsageTypeInfo("Train",              thumbnailInfoTransportTrain     ) },
            { UsageType.WorkersTransportationShipCargo,         new UsageTypeInfo("Ship",               thumbnailInfoTransportShip      ) },
            { UsageType.WorkersTransportationAirCargo,          new UsageTypeInfo("Air",                thumbnailInfoTransportPlane     ) },

            // workers unique building usage types
            { UsageType.WorkersUniqueFinancial,                 new UsageTypeInfo("Financial",          thumbnailInfoMonumentPudding    ) },
            { UsageType.WorkersUniqueLandmark,                  new UsageTypeInfo("Landmarks",          thumbnailInfoMonumentLandmarks  ) },
            { UsageType.WorkersUniqueTourismLeisure,            new UsageTypeInfo("Tourism & Leisure",  thumbnailInfoMonumentExpansion1 ) },
            { UsageType.WorkersUniqueWinterUnique,              new UsageTypeInfo("Winter Unique",      thumbnailInfoMonumentExpansion2 ) },
            { UsageType.WorkersUniquePedestrianArea,            new UsageTypeInfo("Pedestrian Area",    thumbnailInfoMonumentCupcake    ) },
            { UsageType.WorkersUniqueFootball,                  new UsageTypeInfo("Football",           thumbnailInfoMonumentFootball   ) },
            { UsageType.WorkersUniqueConcert,                   new UsageTypeInfo("Concerts",           thumbnailInfoMonumentConcerts   ) },
            { UsageType.WorkersUniqueAirports,                  new UsageTypeInfo("Airports",           thumbnailInfoTransportAirports  ) },
            { UsageType.WorkersUniqueChirpwickCastle,           new UsageTypeInfo("Chirpwick Castle",   thumbnailInfoWonders            ) },
            { UsageType.WorkersUniqueLevel1,                    new UsageTypeInfo("Level 1 Unique",     thumbnailInfoMonumentCategory1  ) },
            { UsageType.WorkersUniqueLevel2,                    new UsageTypeInfo("Level 2 Unique",     thumbnailInfoMonumentCategory2  ) },
            { UsageType.WorkersUniqueLevel3,                    new UsageTypeInfo("Level 3 Unique",     thumbnailInfoMonumentCategory3  ) },
            { UsageType.WorkersUniqueLevel4,                    new UsageTypeInfo("Level 4 Unique",     thumbnailInfoMonumentCategory4  ) },
            { UsageType.WorkersUniqueLevel5,                    new UsageTypeInfo("Level 5 Unique",     thumbnailInfoMonumentCategory5  ) },
            { UsageType.WorkersUniqueLevel6,                    new UsageTypeInfo("Level 6 Unique",     thumbnailInfoMonumentCategory6  ) },
            { UsageType.WorkersUniqueCCPArtDeco,                new UsageTypeInfo("Art Deco",           thumbnailInfoBuyArtDeco         ) },
            { UsageType.WorkersUniqueCCPHighTech,               new UsageTypeInfo("High-Tech",          thumbnailInfoBuyHighTech        ) },
            { UsageType.WorkersUniqueCCPModernJapan,            new UsageTypeInfo("Modern Japan",       thumbnailInfoBuyModernJapan     ) },
            { UsageType.WorkersUniqueCCPSeasideResorts,         new UsageTypeInfo("Seaside Resorts",    thumbnailInfoBuySeasideResorts  ) },
            { UsageType.WorkersUniqueCCPSkyscrapers,            new UsageTypeInfo("Skyscrapers",        thumbnailInfoBuySkyscrapers     ) },
            { UsageType.WorkersUniqueCCPHeartOfKorea,           new UsageTypeInfo("Heart of Korea",     thumbnailInfoBuyHeartOfKorea    ) },
            { UsageType.WorkersUniqueCCPShoppingMalls,          new UsageTypeInfo("Shopping Malls",     thumbnailInfoBuyShoppingMalls   ) },
            { UsageType.WorkersUniqueCCPSportsVenues,           new UsageTypeInfo("Sports Venues",      thumbnailInfoBuySportsVenues    ) },
            { UsageType.WorkersUniqueCCPAfricaInMiniature,      new UsageTypeInfo("Africa in Miniature",thumbnailInfoBuyAfricaMiniature ) },
            { UsageType.WorkersUniqueCCPRailroadsOfJapan,       new UsageTypeInfo("Railroads of Japan", thumbnailInfoBuyRailroadsOfJapan) },


            // visitors usage types
            { UsageType.VisitorsFishMarket,                     new UsageTypeInfo("Fish Market",        thumbnailInfoIndustryFishing    ) },
            { UsageType.VisitorsMedicalPatients,                new UsageTypeInfo("Medical Patients",   thumbnailInfoHealthCare         ) },
            { UsageType.VisitorsMedicalVisitors,                new UsageTypeInfo("Medical Visitors",   thumbnailInfoHealthCare         ) },
            { UsageType.VisitorsDeceased,                       new UsageTypeInfo("Deceased",           thumbnailInfoHealthCare         ) },
            { UsageType.VisitorsShelter,                        new UsageTypeInfo("Sheltered",          thumbnailInfoDisaster           ) },
            { UsageType.VisitorsCriminals,                      new UsageTypeInfo("Criminals",          thumbnailInfoPolice             ) },
            { UsageType.VisitorsEducation,                      new UsageTypeInfo("Education",          thumbnailInfoEducation          ) },
            { UsageType.VisitorsAirportArea,                    new UsageTypeInfo("Airport Area",       thumbnailInfoTransportAirports  ) },
            { UsageType.VisitorsParksPlazas,                    new UsageTypeInfo("Parks & Plazas",     thumbnailInfoBeautification     ) },
            { UsageType.VisitorsUnique,                         new UsageTypeInfo("Unique Buildings",   thumbnailInfoUniqueBuilding     ) },

            // visitors education usage types
            { UsageType.VisitorsEducationElementarySchool,      new UsageTypeInfo("Elementary School",  thumbnailInfoEducation          ) },
            { UsageType.VisitorsEducationHighSchool,            new UsageTypeInfo("High School",        thumbnailInfoEducation          ) },
            { UsageType.VisitorsEducationUniversity,            new UsageTypeInfo("University",         thumbnailInfoEducation          ) },
            { UsageType.VisitorsEducationLibrary,               new UsageTypeInfo("Library",            thumbnailInfoEducation          ) },
            { UsageType.VisitorsEducationTradeSchoolEducation,  new UsageTypeInfo("Education & Supp",   thumbnailInfoEducationBuildings ) },
            { UsageType.VisitorsEducationTradeSchoolFaculty,    new UsageTypeInfo("Faculties",          thumbnailInfoFaculties          ) },
            { UsageType.VisitorsEducationLiberalArtsEducation,  new UsageTypeInfo("Education & Supp",   thumbnailInfoEducationBuildings ) },
            { UsageType.VisitorsEducationLiberalArtsFaculty,    new UsageTypeInfo("Faculties",          thumbnailInfoFaculties          ) },
            { UsageType.VisitorsEducationUniversityEducation,   new UsageTypeInfo("Education & Supp",   thumbnailInfoEducationBuildings ) },
            { UsageType.VisitorsEducationUniversityFaculty,     new UsageTypeInfo("Faculties",          thumbnailInfoFaculties          ) },
            { UsageType.VisitorsEducationMuseum,                new UsageTypeInfo("Museum",             thumbnailInfoMusemum            ) },
            { UsageType.VisitorsEducationVarsitySports,         new UsageTypeInfo("Varsity Sports",     thumbnailInfoVarsitySports      ) },

            // visitors parks & plazas usage types
            { UsageType.VisitorsParksPlazasParks,               new UsageTypeInfo("Parks",              thumbnailInfoBeautParks         ) },
            { UsageType.VisitorsParksPlazasPlazas,              new UsageTypeInfo("Plazas",             thumbnailInfoBeautPlazas        ) },
            { UsageType.VisitorsParksPlazasOtherParks,          new UsageTypeInfo("Other Parks",        thumbnailInfoBeautOtherParks    ) },
            { UsageType.VisitorsParksPlazasTourismLeisure,      new UsageTypeInfo("Tourism & Leisure",  thumbnailInfoBeautExpansion1    ) },
            { UsageType.VisitorsParksPlazasWinterkParks,        new UsageTypeInfo("Winter Parks",       thumbnailInfoBeautExpansion2    ) },
            { UsageType.VisitorsParksPlazasPedestrianPlazas,    new UsageTypeInfo("Pedestrian Plazas",  thumbnailInfoBeautPedestrian    ) },
            { UsageType.VisitorsParksPlazasHotels,              new UsageTypeInfo("Hotels",             thumbnailInfoBeautHotels        ) },
            { UsageType.VisitorsParksPlazasEdenProject,         new UsageTypeInfo("Eden Project",       thumbnailInfoWonders            ) },
            { UsageType.VisitorsParksPlazasCityPark,            new UsageTypeInfo("City Park",          thumbnailInfoBeautCityPark      ) },
            { UsageType.VisitorsParksPlazasAmusementPark,       new UsageTypeInfo("Amusement Park",     thumbnailInfoBeautAmusementPark ) },
            { UsageType.VisitorsParksPlazasZoo,                 new UsageTypeInfo("Zoo",                thumbnailInfoBeautZoo           ) },
            { UsageType.VisitorsParksPlazasNatureReserve,       new UsageTypeInfo("Nature Reserve",     thumbnailInfoBeautNatureReserve ) },
            { UsageType.VisitorsParksPlazasTours,               new UsageTypeInfo("Tours",              thumbnailInfoTransportTours     ) },
            { UsageType.VisitorsParksPlazasCCPHighTech,         new UsageTypeInfo("High-Tech",          thumbnailInfoBuyHighTech        ) },
            { UsageType.VisitorsParksPlazasCCPBridgesPiers,     new UsageTypeInfo("Bridges & Piers",    thumbnailInfoBuyBridgesPiers    ) },
            { UsageType.VisitorsParksPlazasCCPMidCenturyModern, new UsageTypeInfo("Mid-Century Modern", thumbnailInfoBuyMidCenturyModern) },
            { UsageType.VisitorsParksPlazasCCPSportsVenues,     new UsageTypeInfo("Sports Venues",      thumbnailInfoBuySportsVenues    ) },
            { UsageType.VisitorsParksPlazasCCPAfricaInMiniature,new UsageTypeInfo("Africa in Miniature",thumbnailInfoBuyAfricaMiniature ) },
            { UsageType.VisitorsParksPlazasCCPRailroadsOfJapan, new UsageTypeInfo("Railroads of Japan", thumbnailInfoBuyRailroadsOfJapan) },

            // visitors unique building usage types
            { UsageType.VisitorsUniqueFinancial,                new UsageTypeInfo("Financial",          thumbnailInfoMonumentPudding    ) },
            { UsageType.VisitorsUniqueLandmark,                 new UsageTypeInfo("Landmarks",          thumbnailInfoMonumentLandmarks  ) },
            { UsageType.VisitorsUniqueTourismLeisure,           new UsageTypeInfo("Tourism & Leisure",  thumbnailInfoMonumentExpansion1 ) },
            { UsageType.VisitorsUniqueWinterUnique,             new UsageTypeInfo("Winter Unique",      thumbnailInfoMonumentExpansion2 ) },
            { UsageType.VisitorsUniquePedestrianArea,           new UsageTypeInfo("Pedestrian Area",    thumbnailInfoMonumentCupcake    ) },
            { UsageType.VisitorsUniqueFootball,                 new UsageTypeInfo("Football",           thumbnailInfoMonumentFootball   ) },
            { UsageType.VisitorsUniqueConcert,                  new UsageTypeInfo("Concerts",           thumbnailInfoMonumentConcerts   ) },
            { UsageType.VisitorsUniqueAirports,                 new UsageTypeInfo("Airports",           thumbnailInfoTransportAirports  ) },
            { UsageType.VisitorsUniqueChirpwickCastle,          new UsageTypeInfo("Chirpwick Castle",   thumbnailInfoWonders            ) },
            { UsageType.VisitorsUniqueLevel1,                   new UsageTypeInfo("Level 1 Unique",     thumbnailInfoMonumentCategory1  ) },
            { UsageType.VisitorsUniqueLevel2,                   new UsageTypeInfo("Level 2 Unique",     thumbnailInfoMonumentCategory2  ) },
            { UsageType.VisitorsUniqueLevel3,                   new UsageTypeInfo("Level 3 Unique",     thumbnailInfoMonumentCategory3  ) },
            { UsageType.VisitorsUniqueLevel4,                   new UsageTypeInfo("Level 4 Unique",     thumbnailInfoMonumentCategory4  ) },
            { UsageType.VisitorsUniqueLevel5,                   new UsageTypeInfo("Level 5 Unique",     thumbnailInfoMonumentCategory5  ) },
            { UsageType.VisitorsUniqueLevel6,                   new UsageTypeInfo("Level 6 Unique",     thumbnailInfoMonumentCategory6  ) },
            { UsageType.VisitorsUniqueCCPArtDeco,               new UsageTypeInfo("Art Deco",           thumbnailInfoBuyArtDeco         ) },
            { UsageType.VisitorsUniqueCCPHighTech,              new UsageTypeInfo("High-Tech",          thumbnailInfoBuyHighTech        ) },
            { UsageType.VisitorsUniqueCCPModernJapan,           new UsageTypeInfo("Modern Japan",       thumbnailInfoBuyModernJapan     ) },
            { UsageType.VisitorsUniqueCCPSeasideResorts,        new UsageTypeInfo("Seaside Resorts",    thumbnailInfoBuySeasideResorts  ) },
            { UsageType.VisitorsUniqueCCPSkyscrapers,           new UsageTypeInfo("Skyscrapers",        thumbnailInfoBuySkyscrapers     ) },
            { UsageType.VisitorsUniqueCCPHeartOfKorea,          new UsageTypeInfo("Heart of Korea",     thumbnailInfoBuyHeartOfKorea    ) },
            { UsageType.VisitorsUniqueCCPShoppingMalls,         new UsageTypeInfo("Shopping Malls",     thumbnailInfoBuyShoppingMalls   ) },
            { UsageType.VisitorsUniqueCCPSportsVenues,          new UsageTypeInfo("Sports Venues",      thumbnailInfoBuySportsVenues    ) },
            { UsageType.VisitorsUniqueCCPAfricaInMiniature,     new UsageTypeInfo("Africa in Miniature",thumbnailInfoBuyAfricaMiniature ) },
            { UsageType.VisitorsUniqueCCPRailroadsOfJapan,      new UsageTypeInfo("Railroads of Japan", thumbnailInfoBuyRailroadsOfJapan) },


            // storage usage types
            { UsageType.StorageSnow,                            new UsageTypeInfo("Snow",               thumbnailInfoRoadMaintenance    ) },
            { UsageType.StorageWater,                           new UsageTypeInfo("Water",              thumbnailInfoWater              ) },
            { UsageType.StorageGarbage,                         new UsageTypeInfo("Garbage",            thumbnailInfoGarbage            ) },
            { UsageType.StorageIndustry,                        new UsageTypeInfo("Industry",           thumbnailInfoIndustry           ) },
            { UsageType.StoragePostUnsorted,                    new UsageTypeInfo("Unsorted Mail",      thumbnailInfoTransportPost      ) },
            { UsageType.StoragePostSorted,                      new UsageTypeInfo("Sorted Mail",        thumbnailInfoTransportPost      ) },
            { UsageType.StorageCommercialCash,                  new UsageTypeInfo("Commercial Cash",    thumbnailInfoMoney              ) },

            // storage industry usage types
            { UsageType.StorageIndustryForestryExtractor,       new UsageTypeInfo("Extractor Output",   thumbnailInfoExtractor          ) },
            { UsageType.StorageIndustryForestryProcessorInput,  new UsageTypeInfo("Processor Input",    thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryForestryProcessorOutput, new UsageTypeInfo("Processor Output",   thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryForestryStorage,         new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.StorageIndustryFarmingExtractor,        new UsageTypeInfo("Extractor Output",   thumbnailInfoExtractor          ) },
            { UsageType.StorageIndustryFarmingProcessorInput,   new UsageTypeInfo("Processor Input",    thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryFarmingProcessorOutput,  new UsageTypeInfo("Processor Output",   thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryFarmingStorage,          new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.StorageIndustryOreExtractor,            new UsageTypeInfo("Extractor Output",   thumbnailInfoExtractor          ) },
            { UsageType.StorageIndustryOreProcessorInput,       new UsageTypeInfo("Processor Input",    thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryOreProcessorOutput,      new UsageTypeInfo("Processor Output",   thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryOreStorage,              new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.StorageIndustryOilExtractor,            new UsageTypeInfo("Extractor Output",   thumbnailInfoExtractor          ) },
            { UsageType.StorageIndustryOilProcessorInput,       new UsageTypeInfo("Processor Input",    thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryOilProcessorOutput,      new UsageTypeInfo("Processor Output",   thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryOilStorage,              new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.StorageIndustryFishingExtractor,        new UsageTypeInfo("Extractor Output",   thumbnailInfoFishingExtractor   ) },
            { UsageType.StorageIndustryFishingProcessorInput,   new UsageTypeInfo("Processor Input",    thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryFishingProcessorOutput,  new UsageTypeInfo("Processor Output",   thumbnailInfoProcessing         ) },
            { UsageType.StorageIndustryUniqueFactoryInput,      new UsageTypeInfo("Input",              thumbnailInfoUniqueFactory      ) },
            { UsageType.StorageIndustryUniqueFactoryOutput,     new UsageTypeInfo("Output",             thumbnailInfoUniqueFactory      ) },
            { UsageType.StorageIndustryWarehouseGeneric,        new UsageTypeInfo("Generic",            thumbnailInfoWarehouses         ) },


            // vehicles usage types
            { UsageType.VehiclesIndustrialTrucks,               new UsageTypeInfo("Industrial Trucks",  thumbnailInfoIndustrial         ) },
            { UsageType.VehiclesMaintenanceTrucks,              new UsageTypeInfo("Maintenance Trucks", thumbnailInfoRoadMaintenance    ) },
            { UsageType.VehiclesVacuumTrucks,                   new UsageTypeInfo("Vacuum Trucks",      thumbnailInfoWater              ) },
            { UsageType.VehiclesGarbageTrucks,                  new UsageTypeInfo("Garbage Trucks",     thumbnailInfoGarbage            ) },
            { UsageType.VehiclesIndustryVehicles,               new UsageTypeInfo("Industry Vehicles",  thumbnailInfoIndustry           ) },
            { UsageType.VehiclesAmbulances,                     new UsageTypeInfo("Ambulances",         thumbnailInfoHealthCare         ) },
            { UsageType.VehiclesMedicalHelis,                   new UsageTypeInfo("Medical Helicopters",thumbnailInfoHealthCare         ) },
            { UsageType.VehiclesHearses,                        new UsageTypeInfo("Hearses",            thumbnailInfoHealthCare         ) },
            { UsageType.VehiclesFireEngines,                    new UsageTypeInfo("Fire Engines",       thumbnailInfoFire               ) },
            { UsageType.VehiclesFireHelis,                      new UsageTypeInfo("Fire Helicopters",   thumbnailInfoFire               ) },
            { UsageType.VehiclesDisasterVehicles,               new UsageTypeInfo("Disaster Response",  thumbnailInfoDisaster           ) },
            { UsageType.VehiclesEvacuationBuses,                new UsageTypeInfo("Evacuation Buses",   thumbnailInfoDisaster           ) },
            { UsageType.VehiclesPoliceCars,                     new UsageTypeInfo("Police Cars",        thumbnailInfoPolice             ) },
            { UsageType.VehiclesPoliceHelis,                    new UsageTypeInfo("Police Helicopters", thumbnailInfoPolice             ) },
            { UsageType.VehiclesPrisonVans,                     new UsageTypeInfo("Prison Vans",        thumbnailInfoPolice             ) },
            { UsageType.VehiclesBankCashVans,                   new UsageTypeInfo("Bank Cash Vans",     thumbnailInfoBanks              ) },
            { UsageType.VehiclesPostVansTrucks,                 new UsageTypeInfo("Post Vans & Trucks", thumbnailInfoTransportPost      ) },
            { UsageType.VehiclesPrivatePlanes,                  new UsageTypeInfo("Private Planes",     thumbnailInfoAviationClub       ) },
            { UsageType.VehiclesRockets,                        new UsageTypeInfo("Rockets",            thumbnailInfoChirpXLaunchSite   ) },
            { UsageType.VehiclesTransportation,                 new UsageTypeInfo("Transportation",     thumbnailInfoTransportation     ) },

            // vehicles industry usage types
            { UsageType.VehiclesIndustryForestryExtractor,      new UsageTypeInfo("Extractor",          thumbnailInfoExtractor          ) },
            { UsageType.VehiclesIndustryForestryProcessor,      new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.VehiclesIndustryForestryStorage,        new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.VehiclesIndustryFarmingExtractor,       new UsageTypeInfo("Extractor",          thumbnailInfoExtractor          ) },
            { UsageType.VehiclesIndustryFarmingProcessor,       new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.VehiclesIndustryFarmingStorage,         new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.VehiclesIndustryOreExtractor,           new UsageTypeInfo("Extractor",          thumbnailInfoExtractor          ) },
            { UsageType.VehiclesIndustryOreProcessor,           new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.VehiclesIndustryOreStorage,             new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.VehiclesIndustryOilExtractor,           new UsageTypeInfo("Extractor",          thumbnailInfoExtractor          ) },
            { UsageType.VehiclesIndustryOilProcessor,           new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.VehiclesIndustryOilStorage,             new UsageTypeInfo("Storage",            thumbnailInfoStorage            ) },
            { UsageType.VehiclesIndustryFishingExtractor,       new UsageTypeInfo("Extractor",          thumbnailInfoFishingExtractor   ) },
            { UsageType.VehiclesIndustryFishingProcessor,       new UsageTypeInfo("Processor",          thumbnailInfoProcessing         ) },
            { UsageType.VehiclesIndustryUniqueFactory,          new UsageTypeInfo("Unique Factory",     thumbnailInfoUniqueFactory      ) },
            { UsageType.VehiclesIndustryWarehouseGeneric,       new UsageTypeInfo("Warehouse",          thumbnailInfoWarehouses         ) },

            // vehicles transportation usage types
            { UsageType.VehiclesTransportationBus,              new UsageTypeInfo("Bus",                thumbnailInfoTransportBus       ) },
            { UsageType.VehiclesTransportationIntercityBus,     new UsageTypeInfo("Intercity Bus",      thumbnailInfoTransportBus       ) },
            { UsageType.VehiclesTransportationTrolleybus,       new UsageTypeInfo("Trolleybus",         thumbnailInfoTransportTrolleybus) },
            { UsageType.VehiclesTransportationTram,             new UsageTypeInfo("Tram",               thumbnailInfoTransportTram      ) },
            { UsageType.VehiclesTransportationMetro,            new UsageTypeInfo("Metro",              thumbnailInfoTransportMetro     ) },
            { UsageType.VehiclesTransportationTrainPeople,      new UsageTypeInfo("Train",              thumbnailInfoTransportTrain     ) },
            { UsageType.VehiclesTransportationShipPeople,       new UsageTypeInfo("Ship",               thumbnailInfoTransportShip      ) },
            { UsageType.VehiclesTransportationAirPeople,        new UsageTypeInfo("Air",                thumbnailInfoTransportPlane     ) },
            { UsageType.VehiclesTransportationMonorail,         new UsageTypeInfo("Monorail",           thumbnailInfoTransportMonorail  ) },
            { UsageType.VehiclesTransportationCableCar,         new UsageTypeInfo("Cable Car",          thumbnailInfoTransportCableCar  ) },
            { UsageType.VehiclesTransportationTaxi,             new UsageTypeInfo("Taxi",               thumbnailInfoTransportTaxi      ) },
            { UsageType.VehiclesTransportationTours,            new UsageTypeInfo("Tours",              thumbnailInfoTransportTours     ) },
            { UsageType.VehiclesTransportationHubs,             new UsageTypeInfo("Hubs",               thumbnailInfoTransportHubs      ) },
            { UsageType.VehiclesTransportationTrainCargo,       new UsageTypeInfo("Train",              thumbnailInfoTransportTrain     ) },
            { UsageType.VehiclesTransportationShipCargo,        new UsageTypeInfo("Ship",               thumbnailInfoTransportShip      ) },
            { UsageType.VehiclesTransportationAirCargo,         new UsageTypeInfo("Air",                thumbnailInfoTransportPlane     ) },
        };

        // counts by education level
        protected class EducationLevelCount
        {
            public int level0;  // uneducated        - Elementary School not completed
            public int level1;  // educated          - Elementary School completed
            public int level2;  // well uneducated   - High School completed
            public int level3;  // highly uneducated - University completed

            public void Copy(EducationLevelCount value)
            {
                level0 = value.level0;
                level1 = value.level1;
                level2 = value.level2;
                level3 = value.level3;
            }

            public void Add(EducationLevelCount value)
            {
                level0 += value.level0;
                level1 += value.level1;
                level2 += value.level2;
                level3 += value.level3;
            }

            public void Subtract(EducationLevelCount value)
            {
                level0 -= value.level0;
                level1 -= value.level1;
                level2 -= value.level2;
                level3 -= value.level3;
            }
        }

        // usage counts for one building
        protected class UsageCount
        {
            public bool include;                        // whether or not this building should be included in usage counts
            public int used;                            // number of households/workers/visitors/storage/vehicles used by the building
            public int allowed;                         // number of households/workers/visitors/storage/vehicles allowed by the building
            public EducationLevelCount employed;        // number of employed workers       in the building by education level
            public EducationLevelCount totalJobs;       // number of total jobs             in the building by education level
            public EducationLevelCount overEducated;    // number of over educated workers  in the building by education level
            public EducationLevelCount unemployed;      // number of unemployed             in the building by education level
            public EducationLevelCount eligible;        // number of eligible workers       in the building by education level
        }

        // usage counts for all buildings of one usage type
        // dictionary key is the building ID
        protected class UsageCounts : Dictionary<ushort, UsageCount> { }

        // define a group of items for tracking and showing one usage type
        protected class UsageGroup
        {
            public UISprite checkBox;                           // checkbox to show/hide building colors
            public UISprite thumbnail;                          // graphic image representing usage type
            public string spriteNameNormal;                     // sprite name for normal thumbnail
            public string spriteNameDisabled;                   // sprite name for disabled thumbnail
            public UILabel description;                         // textual description
            public UISprite detailButton;                       // button to show detail panel
            public UsagePanel detailPanel;                      // the detail panel to show when button is clicked
            public UITextureSprite legend;                      // color legend
            public Color color0;                                // color for 0 percent usage
            public Color color1;                                // color for 100 percent usage
            public UISprite indicator;                          // graphical indicator of usage percent
            public UILabel percent;                             // usage percent as text
            public UsageCounts usageCounts;                     // usage counts for all buildings of this type
            public int usedRunningTotal;                        // running total of used count
            public int allowedRunningTotal;                     // running total of allowed count
            public EducationLevelCount employedRunningTotal;    // running total of employed worker counts
            public EducationLevelCount totalJobsRunningTotal;   // running total of total jobs counts
            public EducationLevelCount overEdRunningTotal;      // running total of over educated worker counts
            public EducationLevelCount unemployedRunningTotal;  // running total of unemployed counts
            public EducationLevelCount eligibleRunningTotal;    // running total of eligible worker counts
        }

        // the usage groups, there will be one for each usage type on this panel
        protected Dictionary<UsageType, UsageGroup> _usageGroups = new Dictionary<UsageType, UsageGroup>();

        // for creating usage groups
        private int _usageGroupCounter;                 // counter while usage groups are being created
        private const float SideSpace = 10f;            // blank space at each of left and right side of each group
        private const float UsageGroupHeight = 19f;     // height of a usage group including space between usage groups
        private const float TextVerticalOffset = 2.5f;  // amount to offset text to make it appear centered vertically
        private const float DescriptionWidth = 135f;    // width of description text
        private const string CheckBoxNameSuffix = "CheckBox";
        private Dictionary<string, string> _mutuallyExclusiveCheckBoxes = new Dictionary<string, string>();


        // define the signature of the methods that calculate usage count, employed/total jobs count, and unemployed/eligible count
        // the nothing parameter is included to differentiate UnemployedEligibleCountMethod from EmployedTotalJobsCountMethod
        protected delegate void UsageCountMethod             (ushort buildingID, ref Building data, ref int used, ref int allowed);
        protected delegate void EmployedTotalJobsCountMethod (ushort buildingID, ref Building data, ref int used, ref int allowed, ref EducationLevelCount employed,   ref EducationLevelCount totalJobs);
        protected delegate void UnemployedEligibleCountMethod(ushort buildingID, ref Building data, ref int used, ref int allowed, ref EducationLevelCount unemployed, ref EducationLevelCount eligible, ref int nothing);

        // define a class to hold the usage type(s), usage count method(s), employed/total jobs count method, and unemployed/eligible count method
        // most building AI types have only one usage type and method, but a few have two
        // can never have two methods for employed or unemployed because that would double count
        private class UsageTypeMethod
        {
            public UsageType usageType1;
            public UsageCountMethod usageCountMethod1;
            public EmployedTotalJobsCountMethod employedTotalJobsCountMethod1;
            public UnemployedEligibleCountMethod unemployedEligibleCountMethod1;
            public UsageType usageType2;
            public UsageCountMethod usageCountMethod2;
        }

        // create a dictionary to associate a building AI type with its usage type(s) and usage count method(s)
        private Dictionary<Type, UsageTypeMethod> _buildingAIUsages = new Dictionary<Type, UsageTypeMethod>();

        // create a dictionary to associate a vehicle AI type with its usage type
        private Dictionary<Type, UsageType> _vehicleAIUsages = new Dictionary<Type, UsageType>();


        // buttons on a panel
        private const float ButtonHeight = 20f;
        private UIButton _selectAll;
        private UIButton _deselectAll;
        private UIButton _returnFromDetail;

        // main and detail panels
        private List<UsagePanel> _detailPanels = new List<UsagePanel>();    // a list of detail panels supported by this main panel
        private UsagePanel _detailPanel = null;                             // the detail panel current being displayed
        private UsagePanel _mainPanel = null;                               // the main panel to which this detail panel belongs

        // miscellaneous for this panel
        private bool _initialized = false;
        private long _previousTicks = 0;
        private int _updateImmediateCounter = 0;

        // miscellaneous common to all panels
        private static List<Type> _buildingAITypes = null;
        private static bool _hadronColliderBuilt = false;
        private static bool _hadronColliderDetected = false;

        // image atlases
        protected static UITextureAtlas _ingameAtlas = null;
        private static UITextureAtlas _thumbnailsAtlas = null;
        private static UITextureAtlas _expansion9Atlas = null;

        // gradient material and texture
        private static Material _gradientMaterial = null;
        private static Texture _gradientTexture = null;

        // text
        private const float DisabledTextMultiplier = 0.6f;
        private static readonly Color32 _textColorNormal = new Color32(185, 221, 254, 255);
        private static readonly Color32 _textColorDisabled = new Color32((byte)(_textColorNormal.r * DisabledTextMultiplier), (byte)(_textColorNormal.g * DisabledTextMultiplier), (byte)(_textColorNormal.b * DisabledTextMultiplier), 255);
        protected static UIFont _textFont;

        // gradient colors
        private static Color _gradientColorDisabled0 = Color.clear;
        private static Color _gradientColorDisabled1 = Color.clear;

        // Hubs category
        private const string CategoryHubs = "PublicTransportHubs";

        /// <summary>
        /// add a usage panel to the Levels info view panel
        /// </summary>
        public static T AddUsagePanel<T>() where T : UsagePanel
        {
            // get the LevelsInfoViewPanel panel (displayed when the user clicks on the Levels info view button)
            LevelsInfoViewPanel levelsPanel = UIView.library.Get<LevelsInfoViewPanel>(typeof(LevelsInfoViewPanel).Name);
            if (levelsPanel == null)
            {
                LogUtil.LogError($"Unable to find LevelsInfoViewPanel when creating usage panel of type [{typeof(T).Name}].");
                return null;
            }

            // create the usage panel
            T usagePanel = (T)levelsPanel.component.AddUIComponent(typeof(T));
            if (usagePanel == null)
            {
                LogUtil.LogError($"Unable to create usage panel of type [{typeof(T).Name}].");
                return null;
            }

            // return the usage panel
            return usagePanel;
        }

        #region "Base Class Overrides"

        /// <summary>
        /// Start is called once after the panel is created
        /// set up and populate things common to all panels
        /// </summary>
        public override void Start()
        {
            // do base processing
            base.Start();

            // get the LevelsInfoViewPanel panel (displayed when the user clicks on the Levels info view button)
            LevelsInfoViewPanel levelsPanel = UIView.library.Get<LevelsInfoViewPanel>(typeof(LevelsInfoViewPanel).Name);
            if (levelsPanel == null)
            {
                LogUtil.LogError("Unable to find LevelsInfoViewPanel.");
                return;
            }

            // set panel properties
            canFocus = false;
            opacity = 1f;
            relativePosition = new Vector3(0, BuildingUsage.tabStrip.relativePosition.y + BuildingUsage.tabStrip.height);   // immediately below tab strip
            size = new Vector2(levelsPanel.component.width, levelsPanel.component.height - relativePosition.y - 5f);        // leave a little space at the bottom
            isVisible = false;      // start hidden because the Levels tab is the default

            // find atlases
            if (_ingameAtlas == null || _thumbnailsAtlas == null || _expansion9Atlas == null)
            {
                UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
                for (int i = 0; i < atlases.Length; i++)
                {
                    if (atlases[i] != null)
                    {
                        if (atlases[i].name == "Ingame")
                        {
                            _ingameAtlas = atlases[i];
                        }
                        if (atlases[i].name == "Thumbnails")
                        {
                            _thumbnailsAtlas = atlases[i];
                        }
                        if (atlases[i].name == "ThumbnailsExpansion9")
                        {
                            _expansion9Atlas = atlases[i];
                        }
                    }
                }
                if (_ingameAtlas == null)
                {
                    LogUtil.LogError("Unable to find Ingame atlas.");
                    return;
                }
                if (_thumbnailsAtlas == null)
                {
                    LogUtil.LogError("Unable to find Thumbnails atlas.");
                    return;
                }
                if (_expansion9Atlas == null)
                {
                    LogUtil.LogError("Unable to find ThumbnailsExpansion9 atlas.");
                    return;
                }
            }

            // get the material and texture from the existing residential gradient legend
            if (_gradientMaterial == null || _gradientTexture == null)
            {
                UITextureSprite gradientTemplate = levelsPanel.Find<UITextureSprite>("ResidentialGradient");
                if (gradientTemplate == null)
                {
                    LogUtil.LogError("Unable to find ResidentialGradient.");
                    return;
                }
                _gradientMaterial = gradientTemplate.material;
                _gradientTexture = gradientTemplate.texture;
            }

            // compute gradient disabled colors
            if (_gradientColorDisabled0 == Color.clear || _gradientColorDisabled1 == Color.clear)
            {
                _gradientColorDisabled0 = _textColorDisabled; _gradientColorDisabled0 *= 0.7f;
                _gradientColorDisabled1 = _textColorDisabled; _gradientColorDisabled1 *= 0.5f;
            }

            // get text font from existing residential level label
            if (_textFont == null)
            {
                UILabel fontTemplate = levelsPanel.Find<UILabel>("ResidentialLevel");
                if (fontTemplate == null)
                {
                    LogUtil.LogError("Unable to find ResidentialLevel.");
                    return;
                }
                _textFont = fontTemplate.font;
            }

            // create buttons to select/deselect all check boxes
            _selectAll = CreateSelectionButton("SelectAll", "Select All", 10f);
            if (_selectAll == null) return;
            _selectAll.eventClicked += SelectAll_eventClicked;
            _deselectAll = CreateSelectionButton("DeselectAll", "Deselect All", _selectAll.relativePosition.x + _selectAll.size.x + 10f);
            if (_deselectAll == null) return;
            _deselectAll.eventClicked += DeselectAll_eventClicked;

            // determine if Hadron Collider is completed
            // there are two monuments in Africa in Miniature CCP that have HadronColliderAI, so need to check service
            if (!_hadronColliderDetected)
            {
                _hadronColliderBuilt = false;
                Building[] buffer = BuildingManager.instance.m_buildings.m_buffer;
                for (ushort buildingID = 1; buildingID < buffer.Length; buildingID++)
                {
                    Building building = buffer[buildingID];
                    if (building.Info != null &&
                        building.Info.m_buildingAI != null &&
                        building.Info.m_buildingAI.GetType() == typeof(HadronColliderAI) &&
                        ((building.m_flags & Building.Flags.Completed) == Building.Flags.Completed) &&
                        building.Info.m_class.m_service == ItemClass.Service.Education)
                    {
                        _hadronColliderBuilt = true;
                        break;
                    }
                }
                _hadronColliderDetected = true;
            }
        }

        /// <summary>
        /// Update is called every frame
        /// Update is used here only to complete the initialization of the panel after some Managers are up and running
        /// </summary>
        public override void Update()
        {
            // do base processing
            base.Update();

            try
            {
                // finish the initialization
                if (!_initialized)
                {
                    // managers ust exist
                    if (!LoadingManager.exists || !LoadingManager.instance.m_loadingComplete || !ZoneManager.exists || !InfoManager.exists)
                    {
                        return;
                    }

                    // do each usage group
                    Color neutralColor = InfoManager.instance.m_properties.m_neutralColor;
                    Color[] zoneColors = ZoneManager.instance.m_properties.m_zoneColors;
                    foreach (KeyValuePair<UsageType, UsageGroup> entry in _usageGroups)
                    {
                        // get the usage group
                        UsageGroup usageGroup = entry.Value;

                        // set color for 100 percent according to the usage type
                        switch (entry.Key)
                        {
                            // worker usage types
                            // residential and commercial 100 percent color are half as bright as the zone color halfway between low and high density
                            // office and industrial 100 percent color are half as bright as the zone color
                            // 0 percent color is some percent between neutral and the 100 percent color
                            // these are the same colors used by the Levels info view
                            case UsageType.HouseholdsResidential:
                                usageGroup.color1 = Color.Lerp(zoneColors[(int)ItemClass.Zone.ResidentialLow], zoneColors[(int)ItemClass.Zone.ResidentialHigh], 0.5f) * 0.5f;
                                usageGroup.color0 = Color.Lerp(neutralColor, usageGroup.color1, 0.20f);
                                break;
                            case UsageType.WorkersCommercial:
                                usageGroup.color1 = Color.Lerp(zoneColors[(int)ItemClass.Zone.CommercialLow], zoneColors[(int)ItemClass.Zone.CommercialHigh], 0.5f) * 0.5f;
                                usageGroup.color0 = Color.Lerp(neutralColor, usageGroup.color1, 0.33f);
                                break;
                            case UsageType.WorkersOffice:
                                usageGroup.color1 = zoneColors[(int)ItemClass.Zone.Office] * 0.5f;
                                usageGroup.color0 = Color.Lerp(neutralColor, usageGroup.color1, 0.33f);
                                break;
                            case UsageType.WorkersIndustrial:
                            case UsageType.VehiclesIndustrialTrucks:
                                usageGroup.color1 = zoneColors[(int)ItemClass.Zone.Industrial] * 0.5f;
                                usageGroup.color0 = Color.Lerp(neutralColor, usageGroup.color1, 0.33f);
                                break;

                            // all other usage types use the same color
                            // 100 percent color is half of the brightness of the garbage info mode active color
                            // 0 percent color is 25% between neutral and the 100 percent color
                            default:
                                usageGroup.color1 = InfoManager.instance.m_properties.m_modeProperties[(int)InfoManager.InfoMode.Garbage].m_activeColor * 0.5f;
                                usageGroup.color0 = Color.Lerp(neutralColor, usageGroup.color1, 0.25f);
                                break;
                        }

                        // start with the color gradient disabled
                        SetColorGradient(usageGroup.legend, _gradientColorDisabled0, _gradientColorDisabled1);
                    }

                    // initialized
                    _initialized = true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
        }

        /// <summary>
        /// called when panel is destroyed
        /// </summary>
        public override void OnDestroy()
        {
            // do base processing
            base.OnDestroy();

            // remove event handlers
            if (_selectAll != null)
            {
                _selectAll.eventClicked -= SelectAll_eventClicked;
            }
            if (_deselectAll != null)
            {
                _deselectAll.eventClicked -= DeselectAll_eventClicked;
            }
            if (_returnFromDetail != null)
            {
                _returnFromDetail.eventClicked -= ReturnFromDetail_eventClicked;
            }
            if (_usageGroups != null)
            {
                foreach (UsageGroup usageGroup in _usageGroups.Values)
                {
                    if (usageGroup.checkBox != null)
                    {
                        usageGroup.checkBox.eventClicked -= CheckBox_eventClicked;
                    }
                    if (usageGroup.thumbnail != null)
                    {
                        usageGroup.thumbnail.eventClicked -= Thumbnail_eventClicked;
                    }
                    if (usageGroup.description != null)
                    {
                        usageGroup.description.eventClicked -= Description_eventClicked;
                    }
                    if (usageGroup.detailButton != null)
                    {
                        usageGroup.detailButton.eventClicked -= DetailButton_eventClicked;
                    }
                }
            }

            // destroy detail panels
            foreach (UsagePanel detailPanel in _detailPanels)
            {
                Destroy(detailPanel);
            }
            _detailPanels.Clear();

            // hadron collider is not detected
            _hadronColliderDetected = false;
        }
        #endregion

        #region "Check Boxes"

        /// <summary>
        ///  create a selection button
        /// </summary>
        private UIButton CreateSelectionButton(string buttonName, string buttonText, float xPosition)
        {
            // create a new button and set its properties
            UIButton button = AddUIComponent<UIButton>();
            if (button == null)
            {
                LogUtil.LogError($"Unable to create [{buttonText}] button.");
                return null;
            }
            button.name = buttonName;
            button.text = "  " + buttonText + "  ";
            button.textScale = 0.75f;
            button.horizontalAlignment = UIHorizontalAlignment.Center;
            button.verticalAlignment = UIVerticalAlignment.Middle;
            button.autoSize = true;
            float width = button.size.x;
            button.autoSize = false;
            button.size = new Vector2(width, ButtonHeight);
            button.relativePosition = new Vector3(xPosition, 5f);
            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.isVisible = true;

            // return the button
            return button;
        }

        private void SelectAll_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            SetAllCheckBoxes(true);
        }

        private void DeselectAll_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            SetAllCheckBoxes(false);
        }

        /// <summary>
        /// set the checked status for all check boxes
        /// </summary>
        private void SetAllCheckBoxes(bool value)
        {
            // set each check box
            foreach (UsageGroup usageGroup in _usageGroups.Values)
            {
                SetCheckBox(usageGroup.checkBox, value);
            }

            // save config
            BuildingUsageConfig.Save();

            // update panel immediately
            UpdatePanelImmediately();
        }

        /// <summary>
        /// set the check box (i.e. sprite) status
        /// </summary>
        private void SetCheckBox(UISprite checkBox, bool value)
        {
            // change sprite based on value
            if (value)
            {
                // set check box to checked
                checkBox.spriteName = "check-checked";

                // find mutually exclusive check box, if any
                if (_mutuallyExclusiveCheckBoxes.TryGetValue(checkBox.name, out string otherCheckBoxName))
                {
                    // get the other check box
                    UISprite otherCheckBox = Find<UISprite>(otherCheckBoxName);

                    // this is a recursive call, but the recursion should only be one because it is being called with false
                    SetCheckBox(otherCheckBox, false);
                }
            }
            else
            {
                // set check box to unchecked
                checkBox.spriteName = "check-unchecked";
            }
        }

        /// <summary>
        /// handled click on check box
        /// </summary>
        private void CheckBox_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            // set check box to its opposite state
            SetCheckBox((UISprite)component, !IsCheckBoxChecked((UISprite)component));

            // save config
            BuildingUsageConfig.Save();

            // update panel immediately
            UpdatePanelImmediately();
        }

        /// <summary>
        /// clicked on thumbnail is same as clicked on corresponding check box
        /// </summary>
        private void Thumbnail_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            foreach (UsageGroup usageGroup in _usageGroups.Values)
            {
                if (usageGroup.thumbnail == component)
                {
                    CheckBox_eventClicked(usageGroup.checkBox, eventParam);
                    return;
                }
            }
            LogUtil.LogError($"Usage group not found for component [{component.name}].");
        }

        /// <summary>
        /// clicked on description is same as clicked on corresponding check box
        /// </summary>
        private void Description_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            foreach (UsageGroup usageGroup in _usageGroups.Values)
            {
                if (usageGroup.description == component)
                {
                    CheckBox_eventClicked(usageGroup.checkBox, eventParam);
                    return;
                }
            }
            LogUtil.LogError($"Usage group not found for component [{component.name}].");
        }

        /// <summary>
        /// return the check box selection status for the usage type on this main panel or its detail panels
        /// </summary>
        public bool IsCheckBoxChecked(UsageType usageType)
        {
            // get the usage group, if any
            if (_usageGroups.TryGetValue(usageType, out UsageGroup usageGroup))
            {
                return IsCheckBoxChecked(usageGroup.checkBox);
            }

            // check each detail panel
            foreach (UsagePanel detailPanel in _detailPanels)
            {
                if (detailPanel.HasUsageType(usageType))
                {
                    // this is a recursive call, but to a different panel
                    return detailPanel.IsCheckBoxChecked(usageType);
                }
            }

            return true;
        }

        /// <summary>
        /// return whether or not the check box (i.e. sprite) is checked
        /// </summary>
        protected bool IsCheckBoxChecked(UISprite checkBox)
        {
            return checkBox.spriteName == "check-checked";
        }

        /// <summary>
        /// return whether the usage panel or its detail panels have the usage type
        /// </summary>
        public bool HasUsageType(UsageType usageType)
        {
            // check if usage type is on this panel
            if (_usageGroups.ContainsKey(usageType))
            {
                return true;
            }

            // check if usage type is on any detail panel
            foreach (UsagePanel detailPanel in _detailPanels)
            {
                // this is a recursive call, but to a different panel
                if (detailPanel.HasUsageType(usageType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// make two check boxes mutually exclusive
        /// </summary>
        public void MakeCheckBoxesMutuallyExclusive(UsageType usageType1, UsageType usageType2)
        {
            // construct keys
            string key1 = usageType1.ToString() + CheckBoxNameSuffix;
            string key2 = usageType2.ToString() + CheckBoxNameSuffix;

            // check if already defined
            if (_mutuallyExclusiveCheckBoxes.ContainsKey(key1) || _mutuallyExclusiveCheckBoxes.ContainsKey(key2))
            {
                LogUtil.LogError($"Mutually exclusive check boxes already contains key [{key1}] or [{key2}].");
                return;
            }

            // get the check boxes
            UISprite checkBox1 = Find<UISprite>(key1);
            UISprite checkBox2 = Find<UISprite>(key2);
            if (checkBox1 == null || checkBox2 == null)
            {
                // this is not an error, DLC may not be loaded
                return;
            }

            // if both check boxes are currently set (this can happen when there is no config file), then clear the second check box
            if (IsCheckBoxChecked(checkBox1) && IsCheckBoxChecked(checkBox2))
            {
                SetCheckBox(checkBox2, false);
            }

            // add the keys in both directions
            _mutuallyExclusiveCheckBoxes.Add(key1, key2);
            _mutuallyExclusiveCheckBoxes.Add(key2, key1);
        }
        #endregion

        #region "Return From Detail"

        /// <summary>
        /// create a button to return to the main panel
        /// </summary>
        protected void CreateReturnFromDetailButton(string buttonText)
        {
            // create the button and set its properties
            _returnFromDetail = AddUIComponent<UIButton>();
            if (_returnFromDetail == null)
            {
                LogUtil.LogError($"Unable to create Return From Detail button on [{this.name}] panel.");
                return;
            }
            _returnFromDetail.name = "ReturnFromDetail";
            _returnFromDetail.text = "  " + buttonText + "  ";
            _returnFromDetail.textScale = 0.75f;
            _returnFromDetail.horizontalAlignment = UIHorizontalAlignment.Center;
            _returnFromDetail.verticalAlignment = UIVerticalAlignment.Middle;
            _returnFromDetail.autoSize = true;
            float width = _returnFromDetail.size.x;
            _returnFromDetail.autoSize = false;
            _returnFromDetail.size = new Vector2(width, ButtonHeight);
            _returnFromDetail.relativePosition = new Vector3(size.x - SideSpace - _returnFromDetail.size.x, 5f);
            _returnFromDetail.normalBgSprite = "ButtonMenu";
            _returnFromDetail.hoveredBgSprite = "ButtonMenuHovered";
            _returnFromDetail.pressedBgSprite = "ButtonMenuPressed";
            _returnFromDetail.isVisible = true;
            _returnFromDetail.eventClicked += ReturnFromDetail_eventClicked;
        }

        private void ReturnFromDetail_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            // return to the main panel from this detail panel
            _mainPanel.ReturnFromDetailPanel();
        }

        /// <summary>
        /// return from a detail panel, must be called on the main panel
        /// </summary>
        private void ReturnFromDetailPanel()
        {
            // hide the detail panel
            _detailPanel.isVisible = false;
            _detailPanel = null;

            // show and refresh this main panel
            ShowPanel();
        }

        private void DetailButton_eventClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            // determine which show detail button was clicked
            foreach (UsageGroup usageGroup in _usageGroups.Values)
            {
                if (usageGroup.detailButton.name == component.name)
                {
                    // check if detail button is enabled
                    if (usageGroup.allowedRunningTotal != 0)
                    {
                        // replace this main panel with detail panel
                        isVisible = false;
                        _detailPanel = usageGroup.detailPanel;
                        _detailPanel.ShowPanel();
                    }

                    // found the usage group
                    return;
                }
            }
            LogUtil.LogError($"Usage group not found for component [{component.name}].");
        }
        #endregion

        #region "Show/Hide Panels"

        /// <summary>
        /// show and refresh this panel
        /// </summary>
        public void ShowPanel()
        {
            // if there is a detail panel currently selected, show it instead
            if (_detailPanel != null)
            {
                // this is a recursive call, but to a different panel
                _detailPanel.ShowPanel();
            }
            else
            {
                // show this panel
                isVisible = true;

                // update panel immediately
                UpdatePanelImmediately();
            }
        }

        /// <summary>
        /// hide this panel and its detail panel
        /// </summary>
        public void HidePanel()
        {
            // hide this panel
            isVisible = false;

            // if there is a detail panel currently selected, hide it, but remember that it was selected
            if (_detailPanel != null)
            {
                _detailPanel.isVisible = false;
            }
        }
        #endregion

        #region "Usage Groups"

        /// <summary>
        /// create a usage group for the specified usage type
        /// </summary>
        protected void CreateUsageGroup(UsageType usageType)
        {
            // check for blank usage group
            if (usageType == UsageType.None)
            {
                // just increment the usage group counter and return
                _usageGroupCounter++;
                return;
            }

            // get the usage type info
            if (!_usageTypeInfos.TryGetValue(usageType, out UsageTypeInfo usageTypeInfo))
            {
                LogUtil.LogError($"Usage type info not defined for usage type [{usageType}].");
                return;
            }

            // get group top position
            float groupTop = GroupTopPosition();

            // set prefix to use for component names for this group
            string groupName = usageType.ToString();

            // create the checkbox (i.e. a sprite)
            const float CheckBoxHeight = 15f;
            UISprite checkBox = AddUIComponent<UISprite>();
            if (checkBox == null)
            {
                LogUtil.LogError($"Unable to create check box sprite for usage type [{usageType}].");
                return;
            }
            checkBox.name = groupName + CheckBoxNameSuffix;
            checkBox.autoSize = false;
            checkBox.size = new Vector2(CheckBoxHeight, CheckBoxHeight);    // width is same as height
            checkBox.relativePosition = new Vector3(SideSpace, groupTop);
            checkBox.atlas = _ingameAtlas;
            SetCheckBox(checkBox, BuildingUsageConfig.GetCheckBoxSetting(usageType));
            checkBox.isVisible = true;
            checkBox.eventClicked += CheckBox_eventClicked;

            // create the thumbnail
            ThumbnailInfo thumbnailInfo = usageTypeInfo.thumbnailInfo;
            const float ThumbnailMaxWidth = 35f;    // maximum width allowed for thumbnail including space on left and right, thumbnail gets centered in this width
            UISprite thumbnail = AddUIComponent<UISprite>();
            if (thumbnail == null)
            {
                LogUtil.LogError($"Unable to create thumbnail sprite for usage type [{usageType}].");
                return;
            }
            thumbnail.name = groupName + "Thumbnail";
            thumbnail.autoSize = false;
            thumbnail.size = new Vector2(CheckBoxHeight * thumbnailInfo.width / thumbnailInfo.height, CheckBoxHeight);   // make thumbnail same height as checkbox and compute proportional width
            thumbnail.relativePosition = new Vector3(checkBox.relativePosition.x + checkBox.size.x + ThumbnailMaxWidth / 2 - thumbnail.size.x / 2, groupTop);
            switch (thumbnailInfo.atlasType)
            {
                case ThumbnailInfo.AtlasType.InGame:        thumbnail.atlas = _ingameAtlas;     break;
                case ThumbnailInfo.AtlasType.Thumbnails:    thumbnail.atlas = _thumbnailsAtlas; break;
                case ThumbnailInfo.AtlasType.Expansion9:    thumbnail.atlas = _expansion9Atlas; break;
                default:
                    LogUtil.LogError($"Unhandled atlastype [{thumbnailInfo.atlasType}] when creating usage group for usage type [{usageType}].");
                    return;
            }
            thumbnail.spriteName = thumbnailInfo.spriteNameDisabled;
            thumbnail.isVisible = true;
            thumbnail.eventClicked += Thumbnail_eventClicked;

            // create the description
            UILabel description = AddUIComponent<UILabel>();
            if (description == null)
            {
                LogUtil.LogError($"Unable to create description label for usage type [{usageType}].");
                return;
            }
            description.name = groupName + "Description";
            description.font = _textFont;
            description.text = usageTypeInfo.descriptionText;
            description.textAlignment = UIHorizontalAlignment.Left;
            description.verticalAlignment = UIVerticalAlignment.Middle;
            description.textScale = 0.75f;
            description.textColor = _textColorDisabled;
            description.autoSize = false;
            description.size = new Vector2(DescriptionWidth, CheckBoxHeight);   // make same height as check box
            description.relativePosition = new Vector3(checkBox.relativePosition.x + checkBox.size.x + ThumbnailMaxWidth, groupTop + TextVerticalOffset);
            description.isVisible = true;
            description.eventClicked += Description_eventClicked;

            // create the show detail button
            // show the button only if there is a detail panel for this usage group
            const float SpaceBeforeAfter = 5f;
            UISprite detailButton = AddUIComponent<UISprite>();
            if (detailButton == null)
            {
                LogUtil.LogError($"Unable to create detail button sprite for usage type [{usageType}].");
                return;
            }
            detailButton.name = groupName + "Detail";
            detailButton.autoSize = false;
            detailButton.size = new Vector2(CheckBoxHeight, CheckBoxHeight);    // make same height as check box
            detailButton.relativePosition = new Vector3(description.relativePosition.x + description.size.x + SpaceBeforeAfter, groupTop);
            detailButton.atlas = _ingameAtlas;
            detailButton.spriteName = "CityInfoDisabled";
            detailButton.isVisible = false;     // hide the detail button until a detail panel is added

            // create the usage percent label
            // set position on far right
            UILabel percent = AddUIComponent<UILabel>();
            if (percent == null)
            {
                LogUtil.LogError($"Unable to create percent label for usage type [{usageType}].");
                return;
            }
            percent.name = groupName + "Usage";
            percent.font = _textFont;
            percent.text = "100%";
            percent.tooltip = null;
            percent.textAlignment = UIHorizontalAlignment.Right;
            percent.verticalAlignment = UIVerticalAlignment.Middle;
            percent.textScale = 0.75f;
            percent.textColor = _textColorDisabled;
            percent.autoSize = false;
            percent.size = new Vector2(40f, CheckBoxHeight);
            percent.relativePosition = new Vector3(size.x - SideSpace - percent.size.x, groupTop + TextVerticalOffset);
            percent.isVisible = true;
            percent.eventTooltipHover += TooltipHover;

            // create the legend, colors get set in Update
            // set position and size to fit between detail button and percent label
            UITextureSprite legend = AddUIComponent<UITextureSprite>();
            if (legend == null)
            {
                LogUtil.LogError($"Unable to create legend sprite for usage type [{usageType}].");
                return;
            }
            legend.name = groupName + "Legend";
            legend.tooltip = null;
            legend.autoSize = false;
            legend.size = new Vector2(percent.relativePosition.x - SpaceBeforeAfter - (detailButton.relativePosition.x + detailButton.size.x + SpaceBeforeAfter), 12f);
            legend.relativePosition = new Vector3(detailButton.relativePosition.x + detailButton.size.x + SpaceBeforeAfter, groupTop + (CheckBoxHeight - legend.size.y) / 2);
            legend.material = _gradientMaterial;
            legend.texture = _gradientTexture;
            legend.isVisible = true;
            legend.eventTooltipHover += TooltipHover;

            // create the indicator that goes over the legend
            UISprite indicator = legend.AddUIComponent<UISprite>();
            if (indicator == null)
            {
                LogUtil.LogError($"Unable to create indicator sprite for usage type [{usageType}].");
                return;
            }
            indicator.name = groupName + "Indicator";
            indicator.autoSize = false;
            indicator.size = new Vector2(14f, 14f);
            indicator.relativePosition = ComputeIndicatorRelativePosition(indicator, legend, 0f);
            indicator.spriteName = "MeterIndicator";
            indicator.isVisible = false;

            // create and populate a new usage group
            // color0 and color1 get set later in the Update routine
            UsageGroup usageGroup = new UsageGroup
            {
                checkBox = checkBox,
                thumbnail = thumbnail,
                spriteNameNormal = thumbnailInfo.spriteNameNormal,
                spriteNameDisabled = thumbnailInfo.spriteNameDisabled,
                description = description,
                detailButton = detailButton,
                detailPanel = null,
                legend = legend,
                indicator = indicator,
                percent = percent,
                usageCounts = new UsageCounts(),
                usedRunningTotal = 0,
                allowedRunningTotal = 0,
                employedRunningTotal   = new EducationLevelCount(),
                totalJobsRunningTotal  = new EducationLevelCount(),
                overEdRunningTotal     = new EducationLevelCount(),
                unemployedRunningTotal = new EducationLevelCount(),
                eligibleRunningTotal   = new EducationLevelCount()
            };

            // add the usage group to the list
            _usageGroups.Add(usageType, usageGroup);

            // increment usage group counter for next group
            _usageGroupCounter++;
        }

        /// <summary>
        /// create a usage group if the usage type is defined the list of usage types
        /// </summary>
        protected void CreateUsageGroupIfDefined(UsageType usageType, List<UsageType> usageTypes)
        {
            if (usageTypes.Contains(usageType))
            {
                CreateUsageGroup(usageType);
            }
        }

        /// <summary>
        /// create a usage group only if any of the specified building AI types are defined in the building prefabs
        /// </summary>
        protected void CreateUsageGroup<T1>(UsageType usageType)
            where T1 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3, T4>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3, T4>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3, T4, T5>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3, T4, T5>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3, T4, T5, T6>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3, T4, T5, T6, T7>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3, T4, T5, T6, T7, T8>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI where T8 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3, T4, T5, T6, T7, T8, T9>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI where T8 : CommonBuildingAI where T9 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8, T9>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI where T8 : CommonBuildingAI where T9 : CommonBuildingAI where T10 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()) { CreateUsageGroup(usageType); } }

        protected void CreateUsageGroup<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(UsageType usageType)
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI where T8 : CommonBuildingAI where T9 : CommonBuildingAI where T10 : CommonBuildingAI where T11 : CommonBuildingAI
        { if (IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()) { CreateUsageGroup(usageType); } }

        /// <summary>
        /// return whether or not the specified building AI type is defined in the building prefabs
        /// </summary>
        protected bool IsBuildingAITypeDefined<T>() where T : CommonBuildingAI
        {
            // populate only once
            if (_buildingAITypes == null)
            {
                // initialize the list
                _buildingAITypes = new List<Type>();

                // do each building prefab
                int buildingPrefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
                for (uint index = 0; index < buildingPrefabCount; index++)
                {
                    // get the building prefab
                    BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(index);
                    if (prefab != null)
                    {
                        // get the prefab building AI type
                        Type prefabBuildingAIType = prefab.m_buildingAI.GetType();
                        if (prefabBuildingAIType != null)
                        {
                            // add building AI type to the list
                            if (!_buildingAITypes.Contains(prefabBuildingAIType))
                            {
                                _buildingAITypes.Add(prefabBuildingAIType);
                            }
                        }
                    }
                }
            }

            return _buildingAITypes.Contains(typeof(T));
        }

        /// <summary>
        /// return whether or not any of the specified building AI types are defined in the bulding prefabs
        /// </summary>
        protected bool IsBuildingAITypeDefined<T1, T2>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1>() || IsBuildingAITypeDefined<T2>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2>() || IsBuildingAITypeDefined<T3>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3, T4>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2, T3>() || IsBuildingAITypeDefined<T4>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3, T4, T5>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2, T3, T4>() || IsBuildingAITypeDefined<T5>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2, T3, T4, T5>() || IsBuildingAITypeDefined<T6>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6>() || IsBuildingAITypeDefined<T7>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI where T8 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7>() || IsBuildingAITypeDefined<T8>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI where T8 : CommonBuildingAI where T9 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8>() || IsBuildingAITypeDefined<T9>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI where T8 : CommonBuildingAI where T9 : CommonBuildingAI where T10 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8, T9>() || IsBuildingAITypeDefined<T10>(); }

        protected bool IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
            where T1 : CommonBuildingAI where T2 : CommonBuildingAI where T3 : CommonBuildingAI where T4 : CommonBuildingAI where T5 : CommonBuildingAI where T6 : CommonBuildingAI where T7 : CommonBuildingAI where T8 : CommonBuildingAI where T9 : CommonBuildingAI where T10 : CommonBuildingAI where T11 : CommonBuildingAI
        { return IsBuildingAITypeDefined<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() || IsBuildingAITypeDefined<T11>(); }

        /// <summary>
        /// create a heading line above a set of usage groups
        /// </summary>
        protected void CreateGroupHeading(string headingText)
        {
            // get group top position
            float groupTop = GroupTopPosition();

            // set prefix to use for component names for this group
            string groupName = headingText.Replace(" ", "");

            // put the heading text on top of the line
            UILabel heading = AddUIComponent<UILabel>();
            if (heading == null)
            {
                LogUtil.LogError($"Unable to create heading label for group heading [{headingText}].");
                return;
            }
            heading.name = groupName + "Heading";
            heading.font = _textFont;
            heading.text = headingText + "  ";
            heading.textAlignment = UIHorizontalAlignment.Left;
            heading.verticalAlignment = UIVerticalAlignment.Middle;
            heading.textScale = 0.75f;
            heading.textColor = _textColorNormal;
            heading.autoSize = true;
            heading.relativePosition = new Vector3(SideSpace, groupTop + TextVerticalOffset);
            heading.isVisible = true;

            // draw a line across the panel starting after the heading text
            UISprite line = AddUIComponent<UISprite>();
            if (line == null)
            {
                LogUtil.LogError($"Unable to create line sprite for group heading [{headingText}].");
                return;
            }
            line.name = groupName + "Line";
            line.autoSize = false;
            line.size = new Vector2(size.x - 2 * SideSpace - heading.size.x, 5f);
            line.relativePosition = new Vector3(heading.relativePosition.x + heading.size.x, groupTop + (heading.size.y - line.size.y) / 2);
            line.atlas = _ingameAtlas;
            line.spriteName = "ButtonMenuMain";
            line.isVisible = true;

            // increment usage group counter for next group
            _usageGroupCounter++;
        }

        /// <summary>
        /// add a detail panel to this main panel
        /// </summary>
        protected void AddDetailPanel<TDetailPanel>(UsageType usageType) where TDetailPanel : UsagePanel
        {
            // add the detail panel only if a usage group exists for this usage type
            if (_usageGroups.TryGetValue(usageType, out UsageGroup usageGroup))
            {
                // show the detail button
                usageGroup.detailButton.isVisible = true;
                usageGroup.detailButton.eventClicked += DetailButton_eventClicked;

                // create and save the detail panel
                UsagePanel panel = AddUsagePanel<TDetailPanel>();
                panel._mainPanel = this;
                usageGroup.detailPanel = panel;
                _detailPanels.Add(panel);
            }

            // if usage type not found, just return without doing anything (DLC may not be enabled)
        }

        /// <summary>
        /// return the top position for the group
        /// </summary>
        private float GroupTopPosition()
        {
            // 5 is space between button and first usage group
            return _selectAll.relativePosition.y + _selectAll.size.y + 5f + _usageGroupCounter * UsageGroupHeight;
        }

        /// <summary>
        /// associate a building AI type with its usage type and usage count method and optional second usage type and usage count method
        /// </summary>
        protected void AssociateBuildingAI<T>(UsageType usageType1, UsageCountMethod usageCountMethod1, UsageType usageType2 = UsageType.None, UsageCountMethod usageCountMethod2 = null) where T : CommonBuildingAI
        {
            AssociateBuildingAICommon(typeof(T), usageType1, usageCountMethod1, null, null, usageType2, usageCountMethod2);
        }

        /// <summary>
        /// associate a building AI type with its usage type and employed/total jobs count method
        /// </summary>
        protected void AssociateBuildingAI<T>(UsageType usageType1, EmployedTotalJobsCountMethod employedTotalJobsCountMethod1) where T : CommonBuildingAI
        {
            AssociateBuildingAICommon(typeof(T), usageType1, null, employedTotalJobsCountMethod1, null, UsageType.None, null);
        }

        /// <summary>
        /// associate a building AI type with its usage type and unemployed/eligible count method
        /// </summary>
        protected void AssociateBuildingAI<T>(UsageType usageType1, UnemployedEligibleCountMethod unemployedEligibleCountMethod1) where T : CommonBuildingAI
        {
            AssociateBuildingAICommon(typeof(T), usageType1, null, null, unemployedEligibleCountMethod1, UsageType.None, null);
        }

        /// <summary>
        /// associate a building AI type (specified as string) with its usage type and usage count method and optional second usage type and usage count method
        /// </summary>
        /// <param name="buildingAI">building AI formatted as:  Namespace.BuildingAIType</param>
        protected void AssociateBuildingAI(string buildingAI, UsageType usageType1, UsageCountMethod usageCountMethod1, UsageType usageType2 = UsageType.None, UsageCountMethod usageCountMethod2 = null)
        {
            // if the building AI is valid, associate it
            if (BuildingAIIsValid(buildingAI, out Type type))
            {
                AssociateBuildingAICommon(type, usageType1, usageCountMethod1, null, null, usageType2, usageCountMethod2);
            }
        }

        /// <summary>
        /// associate a building AI type (specified as string) with its usage type and employed/total jobs count method and optional second usage type and usage count method
        /// </summary>
        /// <param name="buildingAI">building AI formatted as:  Namespace.BuildingAIType</param>
        protected void AssociateBuildingAI(string buildingAI, UsageType usageType1, EmployedTotalJobsCountMethod employedTotalJobsCountMethod1, UsageType usageType2 = UsageType.None, UsageCountMethod usageCountMethod2 = null)
        {
            // if the building AI is valid, associate it
            if (BuildingAIIsValid(buildingAI, out Type type))
            {
                AssociateBuildingAICommon(type, usageType1, null, employedTotalJobsCountMethod1, null, usageType2, usageCountMethod2);
            }
        }

        /// <summary>
        /// associate a building AI type (specified as string) with its usage type and unemployed/eligible count method
        /// </summary>
        /// <param name="buildingAI">building AI formatted as:  Namespace.BuildingAIType</param>
        protected void AssociateBuildingAI(string buildingAI, UsageType usageType1, UnemployedEligibleCountMethod unemployedEligibleCountMethod1)
        {
            // if the building AI is valid, associate it
            if (BuildingAIIsValid(buildingAI, out Type type))
            {
                AssociateBuildingAICommon(type, usageType1, null, null, unemployedEligibleCountMethod1, UsageType.None, null);
            }
        }

        /// <summary>
        /// return whether or not the building AI is valid, also return the building AI as a Type
        /// </summary>
        public static bool BuildingAIIsValid(string buildingAI, out Type type)
        {
            // initialize output
            type = null;

            // loop over all the assemblies
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // check if the string building AI is in the assembly
                // the AI type will be defined if the mod is subscribed, even if the mod is not enabled
                // it is okay to associate the AI if the mod is not enabled, there simply will be no buildings of that type
                Type tempType = assembly.GetType(buildingAI, false);
                if (tempType != null)
                {
                    // type must derive from CommonBuildingAI
                    if (tempType.IsSubclassOf(typeof(CommonBuildingAI)))
                    {
                        // string BuildingAI is valid
                        type = tempType;
                        return true;
                    }
                    else
                    {
                        // string building AI was found, but it is not valid
                        LogUtil.LogError($"Building AI [{buildingAI}] does not derive from CommonBuildingAI.");
                        return false;
                    }
                }
            }

            // if got here, then the string building AI was not found
            // this is not an error, it just means the mod is not subscribed
            return false;
        }

        /// <summary>
        /// associate a building AI type with its usage count, employed/total jobs count, and unemployed/eligible count method(s)
        /// </summary>
        private void AssociateBuildingAICommon(Type buildingAIType,
            UsageType usageType1, UsageCountMethod usageCountMethod1, EmployedTotalJobsCountMethod employedTotalJobsCountMethod1, UnemployedEligibleCountMethod unemployedEligibleCountMethod1,
            UsageType usageType2, UsageCountMethod usageCountMethod2)
        {
            // associate the building AI type with its count method(s)
            _buildingAIUsages.Add(buildingAIType, new UsageTypeMethod
            {
                usageType1 = usageType1, usageCountMethod1 = usageCountMethod1, employedTotalJobsCountMethod1 = employedTotalJobsCountMethod1, unemployedEligibleCountMethod1 = unemployedEligibleCountMethod1,
                usageType2 = usageType2, usageCountMethod2 = usageCountMethod2,
            });
        }

        /// <summary>
        /// associate a vehicle AI type with its usage type
        /// </summary>
        protected void AssociateVehicleAI<T>(UsageType usageType) where T : VehicleAI
        {
            // associate the vehicle AI type
            _vehicleAIUsages.Add(typeof(T), usageType);
        }

        /// <summary>
        /// handle hover on legend or percent
        /// </summary>
        private void TooltipHover(UIComponent component, UIMouseEventParameter eventParam)
        {
            // the tool tip text gets set in UpdatePanel, just refresh the tool tip
            component.RefreshTooltip();
        }

        /// <summary>
        /// return the bottom position of the bottom-most usage group on this panel and any detail panels
        /// </summary>
        protected float GetUsageGroupBottomPosition()
        {
            // get the bottom position of the bottom-most usage group on this panel
            float bottomPosition = 0f;
            foreach (UsageGroup usageGroup in _usageGroups.Values)
            {
                bottomPosition = Math.Max(bottomPosition, relativePosition.y + usageGroup.checkBox.relativePosition.y + usageGroup.checkBox.size.y);
            }

            // check all the detail panels (if any)
            foreach (UsagePanel usagePanel in _detailPanels)
            {
                // this is a recursive call, but to a different panel
                bottomPosition = Math.Max(bottomPosition, usagePanel.GetUsageGroupBottomPosition());
            }

            // return the bottom position
            return bottomPosition;
        }

        #endregion

        #region "Usage Counts"

        /// <summary>
        /// initialize the building usage counts for all buildings
        /// </summary>
        public void InitializeBuildingUsageCounts()
        {
            // loop over every building
            BuildingManager instance = BuildingManager.instance;
            ushort numBuildings = (ushort)instance.m_buildings.m_buffer.Length;
            for (ushort buildingID = 0; buildingID < numBuildings; buildingID++)
            {
                // do only created buildings
                Building data = instance.m_buildings.m_buffer[buildingID];
                if ((data.m_flags & Building.Flags.Created) == Building.Flags.Created)
                {
                    // update usage counts, ignore return color
                    GetBuildingColor(buildingID, ref data);
                }
            }

            // initialize building usage counts on detail panels
            foreach (UsagePanel detailPanel in _detailPanels)
            {
                // this is a recursive call, but to a different panel
                detailPanel.InitializeBuildingUsageCounts();
            }
        }

        /// <summary>
        /// find and update the usage group with the counts and without education level info
        /// </summary>
        private void UpdateUsageGroup(
            UsageType usageType,
            ushort buildingID,
            bool include,
            int used,
            int allowed)
        {
            // get the usage group based on the usage type
            if (_usageGroups.TryGetValue(usageType, out UsageGroup usageGroup))
            {
                // if included now, add new counts to running totals
                if (include)
                {
                    usageGroup.usedRunningTotal += used;
                    usageGroup.allowedRunningTotal += allowed;
                }

                // find the existing usage count, if any
                if (usageGroup.usageCounts.TryGetValue(buildingID, out UsageCount usageCount))
                {
                    // if included before, subtract previous counts from running totals
                    if (usageCount.include)
                    {
                        usageGroup.usedRunningTotal -= usageCount.used;
                        usageGroup.allowedRunningTotal -= usageCount.allowed;
                    }

                    // update the usage count
                    usageCount.include = include;
                    usageCount.used = used;
                    usageCount.allowed = allowed;
                }
                else
                {
                    // create a new usage count for this building
                    usageGroup.usageCounts.Add(buildingID, new UsageCount()
                    {
                        include = include,
                        used = used,
                        allowed = allowed,
                        employed = new EducationLevelCount(),
                        totalJobs = new EducationLevelCount(),
                        overEducated = new EducationLevelCount(),
                        unemployed = new EducationLevelCount(),
                        eligible = new EducationLevelCount()
                    });
                }
            }
        }

        /// <summary>
        /// find and update the usage group with the counts and with education level info
        /// </summary>
        private void UpdateUsageGroup(
            UsageType usageType,
            ushort buildingID,
            bool include,
            int used,
            int allowed,
            EducationLevelCount employed,
            EducationLevelCount totalJobs,
            EducationLevelCount overEducated,
            EducationLevelCount unemployed,
            EducationLevelCount eligible)
        {
            // get the usage group based on the usage type
            if (_usageGroups.TryGetValue(usageType, out UsageGroup usageGroup))
            {
                // if included now, add new counts to running totals
                if (include)
                {
                    usageGroup.usedRunningTotal += used;
                    usageGroup.allowedRunningTotal += allowed;
                    usageGroup.employedRunningTotal.Add(employed);
                    usageGroup.totalJobsRunningTotal.Add(totalJobs);
                    usageGroup.overEdRunningTotal.Add(overEducated);
                    usageGroup.unemployedRunningTotal.Add(unemployed);
                    usageGroup.eligibleRunningTotal.Add(eligible);
                }

                // find the existing usage count, if any
                if (usageGroup.usageCounts.TryGetValue(buildingID, out UsageCount usageCount))
                {
                    // if included before, subtract previous counts from running totals
                    if (usageCount.include)
                    {
                        usageGroup.usedRunningTotal -= usageCount.used;
                        usageGroup.allowedRunningTotal -= usageCount.allowed;
                        usageGroup.employedRunningTotal.Subtract(usageCount.employed);
                        usageGroup.totalJobsRunningTotal.Subtract(usageCount.totalJobs);
                        usageGroup.overEdRunningTotal.Subtract(usageCount.overEducated);
                        usageGroup.unemployedRunningTotal.Subtract(usageCount.unemployed);
                        usageGroup.eligibleRunningTotal.Subtract(usageCount.eligible);
                    }

                    // update the usage count
                    usageCount.include = include;
                    usageCount.used = used;
                    usageCount.allowed = allowed;
                    usageCount.employed.Copy(employed);
                    usageCount.totalJobs.Copy(totalJobs);
                    usageCount.overEducated.Copy(overEducated);
                    usageCount.unemployed.Copy(unemployed);
                    usageCount.eligible.Copy(eligible);
                }
                else
                {
                    // create a new usage count for this building
                    usageGroup.usageCounts.Add(buildingID, new UsageCount()
                    {
                        include = include,
                        used = used,
                        allowed = allowed,
                        employed = employed,
                        totalJobs = totalJobs,
                        overEducated = overEducated,
                        unemployed = unemployed,
                        eligible = eligible
                    });
                }
            }
        }

        /// <summary>
        /// remove the building from whichever usage group it might be in
        /// </summary>
        public void RemoveUsageCount(ushort buildingID)
        {
            // try each usage group
            foreach (UsageGroup usageGroup in _usageGroups.Values)
            {
                // get the usage count
                if (usageGroup.usageCounts.TryGetValue(buildingID, out UsageCount usageCount))
                {
                    // counts were included, update the running totals
                    if (usageCount.include)
                    {
                        usageGroup.usedRunningTotal -= usageCount.used;
                        usageGroup.allowedRunningTotal -= usageCount.allowed;
                        usageGroup.employedRunningTotal.Subtract(usageCount.employed);
                        usageGroup.totalJobsRunningTotal.Subtract(usageCount.totalJobs);
                        usageGroup.overEdRunningTotal.Subtract(usageCount.overEducated);
                        usageGroup.unemployedRunningTotal.Subtract(usageCount.unemployed);
                        usageGroup.eligibleRunningTotal.Subtract(usageCount.eligible);
                    }

                    // remove the building
                    usageGroup.usageCounts.Remove(buildingID);

                    // check if removed building is Hadron Collider
                    // there are two monuments in Africa in Miniature CCP that have HadronColliderAI, so need to check service
                    BuildingManager instance = BuildingManager.instance;
                    Building building = instance.m_buildings.m_buffer[buildingID];
                    if (building.Info != null &&
                        building.Info.m_buildingAI != null &&
                        building.Info.m_buildingAI.GetType() == typeof(HadronColliderAI) &&
                        building.Info.m_class.m_service == ItemClass.Service.Education)
                    {
                        // not built
                        _hadronColliderBuilt = false;

                        // update panel immediately
                        UpdatePanelImmediately();
                    }

                    // found the usage group
                    break;
                }
            }

            // remove usage count from detail panels
            foreach (UsagePanel detailPanel in _detailPanels)
            {
                // this is a recursive call, but to a different panel
                detailPanel.RemoveUsageCount(buildingID);
            }
        }
        #endregion

        #region "Workers Usage Counts"

        /// <summary>
        /// in the zoned residential building:
        ///      return the number of households used and allowed
        ///      return the number of citizens unemployed and eligible
        /// the nothing parameter exists only to differentiate this method from GetUsageCountWorkersZoned and GetUsageCountWorkersService
        /// </summary>
        protected void GetUsageCountHouseholds(ushort buildingID, ref Building data, ref int used, ref int allowed, ref EducationLevelCount unemployed, ref EducationLevelCount eligible, ref int nothing)
        {
            // CitizenManager must be initialized
            if (!CitizenManager.exists)
            {
                return;
            }

            // The number of households being used and the number of households allowed are displayed on the ZonedBuildingWorldInfoPanel panel.
            // ZonedBuildingWorldInfoPanel.UpdateBindings gets the household info by calling GetLocalizedStatus method of the building AI associated with the building.
            // Presumably, the buildingAI is of type ResidentialBuildingAI for a residential building.
            // ResidentialBuildingAI does not have GetLocalizedStatus, so the base class PrivateBuildingAI.GetLocalizedStatus is called.
            // If status is abandoned, collapsed, etc, then PrivateBuildingAI.GetLocalizedStatus returns a status string based on that.
            // Otherwise, PrivateBuildingAI.GetLocalizedStatus will call either GetLocalizedStatusInactive or GetLocalizedStatusActive, which are in ResidentialBuildingAI.
            // Both GetLocalizedStatusInactive and GetLocalizedStatusActive in ResidentialBuildingAI call GetHomeBehaviour to get the household info.
            // ResidentialBuildingAI and PrivateBuildingAI do not have GetHomeBehaviour, so the further base class CommonBuildingAI.GetHomeBehaviour is called.
            // CommonBuildingAI.GetHomeBehaviour cannot be called because GetHomeBehaviour is not public and is not static.
            // The logic below is a copy (using ILSpy) of CommonBuildingAI.GetHomeBehaviour.
            // The logic was then simplified to include only the parts for computing household info.

            // determine if this is a nursing home from the Nursing Homes for Senior Citizens mod
            bool isNursingHome = (data.Info.m_buildingAI.GetType().Name == "NursingHomeAi");

            // do each citizen unit in the building
            int unitCounter = 0;
            CitizenManager instance = CitizenManager.instance;
            uint maximumCitizenUnits = instance.m_units.m_size;
            uint citizenUnitID = data.m_citizenUnits;
            while (citizenUnitID != 0)
            {
                // not sure if Flags will ever be other than Home for ResidentialBuildingAI, but check anyway
                CitizenUnit citizenUnit = instance.m_units.m_buffer[citizenUnitID];
                if ((citizenUnit.m_flags & CitizenUnit.Flags.Home) != 0)
                {
                    // do each of the up to 5 citizens in the citizen unit
                    int aliveCount = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        // get the citizen ID
                        uint citizenID = citizenUnit.GetCitizen(i);
                        if (citizenID != 0)
                        {
                            // citizen must be not dead and not moving in
                            Citizen citizen = instance.m_citizens.m_buffer[citizenID];
                            if (!citizen.Dead && (citizen.m_flags & Citizen.Flags.MovingIn) == 0)
                            {
                                // count alive
                                aliveCount++;

                                // count unemployed by education level
                                // exclude residents of nursing home who should never be unemployed but are sometimes marked as unemployed
                                if (citizen.Unemployed != 0 && !isNursingHome)
                                {
                                    switch (instance.m_citizens.m_buffer[citizenID].EducationLevel)
                                    {
                                        case Citizen.Education.Uneducated:   unemployed.level0++; break;
                                        case Citizen.Education.OneSchool:    unemployed.level1++; break;
                                        case Citizen.Education.TwoSchools:   unemployed.level2++; break;
                                        case Citizen.Education.ThreeSchools: unemployed.level3++; break;
                                    }
                                }

                                // count eligible to work by education level
                                // if Hadron Collider is built, then include teens
                                Citizen.AgeGroup ageGroup = Citizen.GetAgeGroup(citizen.Age);
                                if (ageGroup == Citizen.AgeGroup.Young || ageGroup == Citizen.AgeGroup.Adult || (_hadronColliderBuilt && ageGroup == Citizen.AgeGroup.Teen))
                                {
                                    switch (citizen.EducationLevel)
                                    {
                                        case Citizen.Education.Uneducated:   eligible.level0++; break;
                                        case Citizen.Education.OneSchool:    eligible.level1++; break;
                                        case Citizen.Education.TwoSchools:   eligible.level2++; break;
                                        case Citizen.Education.ThreeSchools: eligible.level3++; break;
                                    }
                                }
                            }
                        }
                    }

                    // if anyone is alive in the citizen unit, the household is used
                    if (aliveCount != 0)
                    {
                        used++;
                    }

                    // a home citizen unit is an allowed household
                    allowed++;
                }

                // get the next citizen unit
                citizenUnitID = citizenUnit.m_nextUnit;

                // check for error (e.g. circular reference)
                if (++unitCounter > maximumCitizenUnits)
                {
                    LogUtil.LogError("Invalid list detected!" + Environment.NewLine + Environment.StackTrace);
                    break;
                }
            }
        }

        /// <summary>
        /// in the nursing home building (from the Nursing Homes for Senior Citizens mod), return the number of households used and allowed
        /// </summary>
        protected void GetUsageCountHouseholdsNursingHome(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // do normal household count logic, but ignore unemployed and eligible return values
            EducationLevelCount unemployed = new EducationLevelCount();
            EducationLevelCount eligible   = new EducationLevelCount();
            int nothing = 0;
            GetUsageCountHouseholds(buildingID, ref data, ref used, ref allowed, ref unemployed, ref eligible, ref nothing);
        }

        /// <summary>
        /// in the zoned non-residential building:
        ///     return the number of workers used and allowed
        ///     return the number of citizens employed and the total number of jobs
        /// </summary>
        protected void GetUsageCountWorkersZoned(ushort buildingID, ref Building data, ref int used, ref int allowed, ref EducationLevelCount employed, ref EducationLevelCount totalJobs)
        {
            // CitizenManager must be initialized
            if (!CitizenManager.exists)
            {
                return;
            }

            // get the workers employed
            GetWorkersEmployed(ref data, ref employed);

            // workers used is sum of employed
            used = employed.level0 + employed.level1 + employed.level2 + employed.level3;

            // The number of workers allowed is displayed on the ZonedBuildingWorldInfoPanel panel.
            // For non-residential zoned buildings, ZonedBuildingWorldInfoPanel.UpdatedBindings calls ZonedBuildingWorldInfoPanel.UpdateWorkers.
            // The logic below is a copy (using ILSpy) of the portion of UpdateWorkers that computes the number of workers allowed.
            // The logic was then simplified to include only the required parts.

            // get the BuildingAI stored with the building
            PrivateBuildingAI buildingAI = data.Info.m_buildingAI as PrivateBuildingAI;
            if (buildingAI != null)
            {
                // make the calls to get the workers allowed for each education level
                buildingAI.CalculateWorkplaceCount((ItemClass.Level)data.m_level, new Randomizer(buildingID), data.Width, data.Length, out totalJobs.level0, out totalJobs.level1, out totalJobs.level2, out totalJobs.level3);
                buildingAI.AdjustWorkplaceCount(buildingID, ref data, ref totalJobs.level0, ref totalJobs.level1, ref totalJobs.level2, ref totalJobs.level3);

                // the number of workers allowed is the sum of the workers at each education level
                allowed = totalJobs.level0 + totalJobs.level1 + totalJobs.level2 + totalJobs.level3;
            }
        }

        /// <summary>
        /// in the park gate or park building:
        ///     return the number of workers used and allowed
        ///     return the number of citizens employed and the total number of jobs
        /// </summary>
        protected void GetUsageCountWorkersPark(ushort buildingID, ref Building data, ref int used, ref int allowed, ref EducationLevelCount employed, ref EducationLevelCount totalJobs)
        {
            // CitizenManager must be initialized
            if (!CitizenManager.exists)
            {
                return;
            }

            // logic adapted from ParkGateAI and ParkBuildingAI private methods CountWorkers and TargetWorkers

            // only Amusement Park and Zoo have workers
            DistrictPark.ParkType parkType = GetParkType(data.Info);
            if ((data.Info.m_doorMask & PropInfo.DoorType.HangAround) == PropInfo.DoorType.HangAround && (parkType == DistrictPark.ParkType.AmusementPark || parkType == DistrictPark.ParkType.Zoo))
            {
                // get workers used
                int citizenCounter = 0;
                CitizenManager instance = CitizenManager.instance;
                uint maximumCitizenUnits = instance.m_units.m_size;
                uint citizen = data.m_sourceCitizens;
                while (citizen != 0)
                {
                    // exclude animals
                    CitizenInfo info = instance.m_instances.m_buffer[citizen].Info;
                    if (!info.m_citizenAI.IsAnimal() && info.m_class.m_service == ItemClass.Service.Beautification)
                    {
                        // count by education level
                        switch (instance.m_citizens.m_buffer[citizen].EducationLevel)
                        {
                            case Citizen.Education.Uneducated:   employed.level0++; break;
                            case Citizen.Education.OneSchool:    employed.level1++; break;
                            case Citizen.Education.TwoSchools:   employed.level2++; break;
                            case Citizen.Education.ThreeSchools: employed.level3++; break;
                        }
                    }

                    // get the next citizen
                    citizen = instance.m_instances.m_buffer[citizen].m_nextSourceInstance;

                    // check for error (e.g. circular reference)
                    if (++citizenCounter > maximumCitizenUnits)
                    {
                        LogUtil.LogError("Invalid list detected!" + Environment.NewLine + Environment.StackTrace);
                        break;
                    }
                }

                // workers used is sum of employed
                used = employed.level0 + employed.level1 + employed.level2 + employed.level3;

                // get workers allowed
                Randomizer randomizer = new Randomizer(buildingID);
                allowed = (data.Width * data.Length * 5 + randomizer.Int32(100u)) / 100;

                // the game logic does not separate park jobs by education level, so put all jobs in uneducated
                totalJobs.level0 += allowed;
            }
        }

        /// <summary>
        /// get the park type for the specified building
        /// </summary>
        protected DistrictPark.ParkType GetParkType(BuildingInfo buildingInfo)
        {
            if (buildingInfo.m_buildingAI.GetType() == typeof(ParkGateAI))
            {
                ParkGateAI buildingAI = buildingInfo.m_buildingAI as ParkGateAI;
                return buildingAI.m_parkType;
            }

            if (buildingInfo.m_buildingAI.GetType() == typeof(ParkBuildingAI))
            {
                ParkBuildingAI buildingAI = buildingInfo.m_buildingAI as ParkBuildingAI;
                return buildingAI.m_parkType;
            }

            LogUtil.LogError($"Unhandled building AI type [{buildingInfo.m_buildingAI.GetType().ToString()}] while trying to get park type.");
            return DistrictPark.ParkType.None;
        }

        /// <summary>
        /// in a nursing home building (from the Nursing Homes for Senior Citizens mod):
        ///     return the number of workers used and allowed
        ///     return the number of citizens employed and the total number of jobs
        /// </summary>
        protected void GetUsageCountWorkersNursingHome(ushort buildingID, ref Building data, ref int used, ref int allowed, ref EducationLevelCount employed, ref EducationLevelCount totalJobs)
        {
            // CitizenManager must be initialized
            if (!CitizenManager.exists)
            {
                return;
            }

            // get the workers employed
            GetWorkersEmployed(ref data, ref employed);

            // workers used is sum of employed
            used = employed.level0 + employed.level1 + employed.level2 + employed.level3;

            // get the BuildingAI stored with the building
            BuildingAI buildingAI = data.Info.m_buildingAI;
            if (buildingAI != null)
            {
                // get the number of workers at each education level
                Type buildingAIType = buildingAI.GetType();
                totalJobs.level0 = (int)buildingAIType.GetField("numUneducatedWorkers"    ).GetValue(buildingAI);
                totalJobs.level1 = (int)buildingAIType.GetField("numEducatedWorkers"      ).GetValue(buildingAI);
                totalJobs.level2 = (int)buildingAIType.GetField("numWellEducatedWorkers"  ).GetValue(buildingAI);
                totalJobs.level3 = (int)buildingAIType.GetField("numHighlyEducatedWorkers").GetValue(buildingAI);

                // the number of workers allowed is the sum of the workers at each education level
                allowed = totalJobs.level0 + totalJobs.level1 + totalJobs.level2 + totalJobs.level3;
            }
        }

        /// <summary>
        /// in the non-zoned service building:
        ///     return the number of workers used and allowed
        ///     return the number of citizens employed and the total number of jobs
        /// </summary>
        protected void GetUsageCountWorkersService<T>(ushort buildingID, ref Building data, ref int used, ref int allowed, ref EducationLevelCount employed, ref EducationLevelCount totalJobs) where T : PlayerBuildingAI
        {
            // CitizenManager must be initialized
            if (!CitizenManager.exists)
            {
                return;
            }

            // get the workers employed
            GetWorkersEmployed(ref data, ref employed);

            // workers used is sum of employed
            used = employed.level0 + employed.level1 + employed.level2 + employed.level3;

            // The number of workers allowed is not displayed.
            // The building AI computes the workers allowed by calling HandleWorkAndVisitPlaces.
            // HandleWorkAndVisitPlaces sums the allowed workers from each education level.

            // get the BuildingAI stored with the building
            T buildingAI = data.Info.m_buildingAI as T;
            if (buildingAI != null)
            {
                // get the number of workers at each education level
                Type buildingAIType = typeof(T);
                totalJobs.level0 = (int)buildingAIType.GetField("m_workPlaceCount0").GetValue(buildingAI);
                totalJobs.level1 = (int)buildingAIType.GetField("m_workPlaceCount1").GetValue(buildingAI);
                totalJobs.level2 = (int)buildingAIType.GetField("m_workPlaceCount2").GetValue(buildingAI);
                totalJobs.level3 = (int)buildingAIType.GetField("m_workPlaceCount3").GetValue(buildingAI);

                // the number of workers allowed is the sum of the workers at each education level
                allowed = totalJobs.level0 + totalJobs.level1 + totalJobs.level2 + totalJobs.level3;

                // check for Hadron Collider completed
                // there are two monuments in Africa in Miniature CCP that have HadronColliderAI, so need to check service
                if (buildingAI.GetType() == typeof(HadronColliderAI) &&
                   ((data.m_flags & Building.Flags.Completed) == Building.Flags.Completed) &&
                   data.Info.m_class.m_service == ItemClass.Service.Education)
                {
                    if (!_hadronColliderBuilt)
                    {
                        // built
                        _hadronColliderBuilt = true;

                        // update panel immediately
                        UpdatePanelImmediately();
                    }
                }
            }
        }

        /// <summary>
        /// return the number of workers employed in the building
        /// </summary>
        private void GetWorkersEmployed(ref Building data, ref EducationLevelCount employed)
        {
            // The number of workers being employed in a zoned building is displayed on the ZonedBuildingWorldInfoPanel panel.
            // ZonedBuildingWorldInfoPanel.UpdatedBindings calls ZonedBuildingWorldInfoPanel.UpdateWorkers.
            // The logic below is a copy (using ILSpy) of the portion of UpdateWorkers that computes number of workers employed.
            // The logic was then simplified to include only the required parts.

            // The number of workers in a non-zoned service building is not displayed.
            // The building AI calls HandleWorkAndVisitPlaces.
            // HandleWorkAndVisitPlaces calls base class CommonBuildingAI.GetWorkBehaviour.
            // The logic in CommonBuildingAI.GetWorkBehaviour has the same result as UpdateWorkers.

            // do each citizen unit in the building
            int unitCounter = 0;
            CitizenManager instance = CitizenManager.instance;
            uint maximumCitizenUnits = instance.m_units.m_size;
            uint citizenUnitID = data.m_citizenUnits;
            while (citizenUnitID != 0)
            {
                // do only work citizen units
                CitizenUnit citizenUnit = instance.m_units.m_buffer[citizenUnitID];
                if ((citizenUnit.m_flags & CitizenUnit.Flags.Work) != 0)
                {
                    // do each of the up to 5 citizens in the citizen unit
                    for (int i = 0; i < 5; i++)
                    {
                        // get citizen ID
                        uint citizenID = citizenUnit.GetCitizen(i);
                        if (citizenID != 0)
                        {
                            // citizen must be not dead and not moving in
                            Citizen citizen = instance.m_citizens.m_buffer[citizenID];
                            if (!citizen.Dead && (citizen.m_flags & Citizen.Flags.MovingIn) == 0)
                            {
                                // count employed by education level
                                switch (citizen.EducationLevel)
                                {
                                    case Citizen.Education.Uneducated:   employed.level0++; break;
                                    case Citizen.Education.OneSchool:    employed.level1++; break;
                                    case Citizen.Education.TwoSchools:   employed.level2++; break;
                                    case Citizen.Education.ThreeSchools: employed.level3++; break;
                                }
                            }
                        }
                    }
                }

                // get the next citizen unit
                citizenUnitID = citizenUnit.m_nextUnit;

                // check for error (e.g. circular reference)
                if (++unitCounter > maximumCitizenUnits)
                {
                    LogUtil.LogError("Invalid list detected!" + Environment.NewLine + Environment.StackTrace);
                    break;
                }
            }
        }
        #endregion

        #region "Visitors Usage Counts"

        // In all the GetUsageCountVisitors* routines below:
        // The number of visitors used and the number of visitors allowed are computed by calling GetLocalizedStats method of the building AI associated with the building.
        // The logic in each routine is a copy (using ILSpy) of [building AI].GetLocalizedStats.
        // The logic was then simplified to include only the parts for computing visitor info.

        /// <summary>
        /// get the usage count of a MarketAI building
        /// </summary>
        protected void GetUsageCountVisitorsMarket(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get citizens "visiting" the building who are not dead
            GetVisitorsUsedNotDead(ref data, ref used);

            // allowed is sum of low, medium, and high wealth
            MarketAI buildingAI = data.Info.m_buildingAI as MarketAI;
            allowed = buildingAI.m_visitPlaceCount;
        }

        /// <summary>
        /// get the usage count of a HospitalAI or derived building
        /// </summary>
        protected void GetUsageCountVisitorsHospital<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : HospitalAI
        {
            // get sick citizens "visiting" the building
            GetVisitorsUsed(ref data, Citizen.Flags.Sick, ref used);

            // get patient capacity
            T buildingAI = data.Info.m_buildingAI as T;
            allowed = buildingAI.PatientCapacity;
        }

        /// <summary>
        /// get the usage count of a ChildcareAI building
        /// </summary>
        protected void GetUsageCountVisitorsChildcare(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get patients used and allowed
            // logic adapted from ChildcareAI.GetVisitorCount
            used = data.m_customBuffer1;
            ChildcareAI buildingAI = data.Info.m_buildingAI as ChildcareAI;
            int budget = EconomyManager.instance.GetBudget(buildingAI.m_info.m_class);
            int productionRate = PlayerBuildingAI.GetProductionRate(100, budget);
            allowed = Mathf.Min((productionRate * buildingAI.PatientCapacity + 99) / 100, buildingAI.PatientCapacity * 5 / 4);
        }

        /// <summary>
        /// get the usage count of an EldercareAI building
        /// </summary>
        protected void GetUsageCountVisitorsEldercare(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get patients used and allowed
            // logic adapted from EldercareAI.GetVisitorCount
            used = data.m_customBuffer1;
            EldercareAI buildingAI = data.Info.m_buildingAI as EldercareAI;
            int budget = EconomyManager.instance.GetBudget(buildingAI.m_info.m_class);
            int productionRate = PlayerBuildingAI.GetProductionRate(100, budget);
            allowed = Mathf.Min((productionRate * buildingAI.PatientCapacity + 99) / 100, buildingAI.PatientCapacity * 5 / 4);
        }

        /// <summary>
        /// get the usage count of a SaunaAI building
        /// </summary>
        protected void GetUsageCountVisitorsSauna(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get citizens "visiting" the building who are not dead
            GetVisitorsUsedNotDead(ref data, ref used);

            // allowed is sum of low, medium, and high wealth
            SaunaAI buildingAI = data.Info.m_buildingAI as SaunaAI;
            allowed = buildingAI.m_visitPlaceCount0 + buildingAI.m_visitPlaceCount1 + buildingAI.m_visitPlaceCount2;
        }

        /// <summary>
        /// get the usage count of a CemeteryAI building
        /// </summary>
        protected void GetUsageCountVisitorsCemetery(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get dead citizens "visiting" the building
            GetVisitorsUsed(ref data, Citizen.Flags.Dead, ref used);

            // adjust used count and get grave or corpse capacity
            CemeteryAI buildingAI = data.Info.m_buildingAI as CemeteryAI;
            if (buildingAI.m_graveCount != 0)
            {
                used = Mathf.Min(used + data.m_customBuffer1, buildingAI.m_graveCount);
                allowed = buildingAI.m_graveCount;
            }
            else
            {
                used = Mathf.Min(used + data.m_customBuffer1, buildingAI.m_corpseCapacity);
                allowed = buildingAI.m_corpseCapacity;
            }
        }

        /// <summary>
        /// get the usage count of a ShelterAI building
        /// </summary>
        protected void GetUsageCountVisitorsShelter(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get citizens "visiting" the building who are not dead
            GetVisitorsUsedNotDead(ref data, ref used);

            // allowed logic is adapted from ShelterAI.GetLocalizedStats
            ShelterAI buildingAI = data.Info.m_buildingAI as ShelterAI;
            allowed = buildingAI.m_capacity + buildingAI.m_capacity * 3 * Mathf.Min(data.m_finalExport, 100) / 1000;
        }

        /// <summary>
        /// get the usage count of a PoliceStationAI building
        /// </summary>
        protected void GetUsageCountVisitorsPoliceStation(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get jailed citizens "visiting" the building
            GetVisitorsUsed(ref data, Citizen.Flags.Criminal | Citizen.Flags.Arrested, ref used);

            // get jail capacity
            PoliceStationAI buildingAI = data.Info.m_buildingAI as PoliceStationAI;
            allowed = buildingAI.JailCapacity;
        }

        /// <summary>
        /// get the usage count of a SchoolAI or derived building
        /// </summary>
        protected void GetUsageCountVisitorsSchool<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : SchoolAI
        {
            // get students used and allowed
            // logic adapted from SchoolAI.GetStudentCount
            used = data.m_customBuffer1;
            T buildingAI = data.Info.m_buildingAI as T;
            int budget = EconomyManager.instance.GetBudget(buildingAI.m_info.m_class);
            int productionRate = PlayerBuildingAI.GetProductionRate(100, budget);
            allowed = Mathf.Min((productionRate * buildingAI.StudentCount + 99) / 100, buildingAI.StudentCount * 5 / 4);
        }

        /// <summary>
        /// get the usage count of a LibraryAI building
        /// </summary>
        protected void GetUsageCountVisitorsLibrary(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get visitors used and allowed
            // logic adapted from LibraryAI.GetVisitorCount
            used = data.m_customBuffer1;
            LibraryAI buildingAI = data.Info.m_buildingAI as LibraryAI;
            int budget = EconomyManager.instance.GetBudget(buildingAI.m_info.m_class);
            int productionRate = PlayerBuildingAI.GetProductionRate(100, budget);
            allowed = Mathf.Min((productionRate * buildingAI.VisitorCount + 99) / 100, buildingAI.VisitorCount * 5 / 4);
        }

        /// <summary>
        /// get the usage count of a VarsitySportsArenaAI building
        /// logic adapted from EventAI.CountVisitors
        /// </summary>
        protected void GetUsageCountVisitorsVarsitySportsArena(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get citizens "visiting" the building
            int unitCounter = 0;
            CitizenManager instance = CitizenManager.instance;
            uint maximumCitizenUnits = instance.m_units.m_size;
            uint citizenUnit = data.m_citizenUnits;
            while (citizenUnit != 0)
            {
                if ((instance.m_units.m_buffer[citizenUnit].m_flags & CitizenUnit.Flags.Visit) != 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        // include only citizens who are at the building
                        uint citizen = instance.m_units.m_buffer[citizenUnit].GetCitizen(i);
                        if (citizen != 0)
                        {
                            if (instance.m_citizens.m_buffer[citizen].GetBuildingByLocation() == buildingID)
                            {
                                used++;
                            }
                        }
                    }

                    // allowed is sum of visiting citizen units in the building
                    allowed += 5;
                }

                // get the next citizen unit
                citizenUnit = instance.m_units.m_buffer[citizenUnit].m_nextUnit;

                // check for error (e.g. circular reference)
                if (++unitCounter > maximumCitizenUnits)
                {
                    LogUtil.LogError("Invalid list detected!" + Environment.NewLine + Environment.StackTrace);
                    break;
                }
            }
        }

        /// <summary>
        /// get the usage count of a AirportBuildingAI or derived building
        /// </summary>
        protected void GetUsageCountVisitorsAirportArea<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : AirportBuildingAI
        {
            // get visitors used
            // logic adapted from AirportBuildingAI.GetLocalizedStats
            used = data.m_customBuffer2;

            // allowed is sum of low, medium, and high wealth
            T buildingAI = data.Info.m_buildingAI as T;
            allowed = buildingAI.m_visitPlaceCount0 + buildingAI.m_visitPlaceCount1 + buildingAI.m_visitPlaceCount2;
        }

        /// <summary>
        /// get the usage count of a ParkAI or derived building
        /// </summary>
        protected void GetUsageCountVisitorsPark<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : ParkAI
        {
            // get citizens and tourists "visiting" the building who are not dead
            GetVisitorsUsedNotDead(ref data, ref used);

            // allowed is sum of low, medium, and high wealth
            T buildingAI = data.Info.m_buildingAI as T;
            allowed = buildingAI.m_visitPlaceCount0 + buildingAI.m_visitPlaceCount1 + buildingAI.m_visitPlaceCount2;
        }

        /// <summary>
        /// get the usage count of a TourBuildingAI or derived building
        /// </summary>
        protected void GetUsageCountVisitorsTourBuilding<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : TourBuildingAI
        {
            // get citizens and tourists "visiting" the building who are not dead
            GetVisitorsUsedNotDead(ref data, ref used);

            // allowed is sum of low, medium, and high wealth
            T buildingAI = data.Info.m_buildingAI as T;
            allowed = buildingAI.m_visitPlaceCount0 + buildingAI.m_visitPlaceCount1 + buildingAI.m_visitPlaceCount2;
        }

        /// <summary>
        /// get the usage count of a HotelAI
        /// logic adapted from HotelWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountVisitorsHotel(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // all guests are considered "visiting"
            used = HotelAI.GetCurrentGuests(ref data);

            // allowed is number of rooms
            HotelAI buildingAI = data.Info.m_buildingAI as HotelAI;
            allowed = buildingAI.m_rooms;

            // don't understand why HotelAI allocates 1 CitizenUnit for each room
            // seems like a waste of CitizenUnits when each CitizenUnit can hold 5 Citizens (i.e. 5 guests)
        }

        /// <summary>
        /// get the usage count of a ParkBuildingAI or derived building
        /// </summary>
        protected void GetUsageCountVisitorsParkBuilding<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : ParkBuildingAI
        {
            // get citizens and tourists "visiting" the building who are not dead
            GetVisitorsUsedNotDead(ref data, ref used);

            // allowed is sum of low, medium, and high wealth
            T buildingAI = data.Info.m_buildingAI as T;
            allowed = buildingAI.m_visitPlaceCount0 + buildingAI.m_visitPlaceCount1 + buildingAI.m_visitPlaceCount2;
        }

        /// <summary>
        /// get the usage count of a MonumentAI or derived building
        /// </summary>
        protected void GetUsageCountVisitorsMonument<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : MonumentAI
        {
            // get citizens and tourists "visiting" the building who are not dead
            GetVisitorsUsedNotDead(ref data, ref used);

            // allowed is sum of low, medium, and high wealth
            T buildingAI = data.Info.m_buildingAI as T;
            allowed = buildingAI.m_visitPlaceCount0 + buildingAI.m_visitPlaceCount1 + buildingAI.m_visitPlaceCount2;
        }

        /// <summary>
        /// get the number of visitors at the building
        /// </summary>
        private void GetVisitorsUsed(ref Building data, Citizen.Flags flags, ref int used)
        {
            // get citizens "visiting" the building
            int unitCounter = 0;
            CitizenManager instance = CitizenManager.instance;
            uint maximumCitizenUnits = instance.m_units.m_size;
            uint citizenUnit = data.m_citizenUnits;
            while (citizenUnit != 0)
            {
                if ((instance.m_units.m_buffer[citizenUnit].m_flags & CitizenUnit.Flags.Visit) != 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        // include only citizens with the specified flag
                        uint citizen = instance.m_units.m_buffer[citizenUnit].GetCitizen(i);
                        if (citizen != 0 && instance.m_citizens.m_buffer[citizen].CurrentLocation == Citizen.Location.Visit && (instance.m_citizens.m_buffer[citizen].m_flags & flags) != 0)
                        {
                            used++;
                        }
                    }
                }

                // get the next citizen unit
                citizenUnit = instance.m_units.m_buffer[citizenUnit].m_nextUnit;

                // check for error (e.g. circular reference)
                if (++unitCounter > maximumCitizenUnits)
                {
                    LogUtil.LogError("Invalid list detected!" + Environment.NewLine + Environment.StackTrace);
                    break;
                }
            }
        }

        /// <summary>
        /// get the number of visitors at the building who are not dead
        /// </summary>
        private void GetVisitorsUsedNotDead(ref Building data, ref int used)
        {
            // get citizens "visiting" the building
            int unitCounter = 0;
            CitizenManager instance = CitizenManager.instance;
            uint maximumCitizenUnits = instance.m_units.m_size;
            uint citizenUnit = data.m_citizenUnits;
            while (citizenUnit != 0)
            {
                if ((instance.m_units.m_buffer[citizenUnit].m_flags & CitizenUnit.Flags.Visit) != 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        // include only citizens who are not dead
                        uint citizen = instance.m_units.m_buffer[citizenUnit].GetCitizen(i);
                        if (citizen != 0 && instance.m_citizens.m_buffer[citizen].CurrentLocation == Citizen.Location.Visit && (instance.m_citizens.m_buffer[citizen].m_flags & Citizen.Flags.Dead) == 0)
                        {
                            used++;
                        }
                    }
                }

                // get the next citizen unit
                citizenUnit = instance.m_units.m_buffer[citizenUnit].m_nextUnit;

                // check for error (e.g. circular reference)
                if (++unitCounter > maximumCitizenUnits)
                {
                    LogUtil.LogError("Invalid list detected!" + Environment.NewLine + Environment.StackTrace);
                    break;
                }
            }
        }
        #endregion

        #region "Storage Usage Counts"

        // In all the GetUsageCountStorage* routines below (unless otherwise specified):
        // The number of things being used and the number of things allowed are computed by calling GetLocalizedStats method of the building AI associated with the building.
        // The logic in each routine is a copy (using ILSpy) of [building AI].GetLocalizedStats.
        // The logic was then simplified to include only the parts for computing storage info.


        /// <summary>
        /// get the usage count of a SnowDumpAI building
        /// </summary>
        protected void GetUsageCountStorageSnowDump(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get snow at the building
            SnowDumpAI buildingAI = data.Info.m_buildingAI as SnowDumpAI;
            used = buildingAI.GetSnowAmount(buildingID, ref data);

            // get snow capacity
            allowed = buildingAI.m_snowCapacity;
        }

        /// <summary>
        /// get the usage count of a WaterFacilityAI building
        /// </summary>
        protected void GetUsageCountStorageWaterFacility(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // check building type
            WaterFacilityAI buildingAI = data.Info.m_buildingAI as WaterFacilityAI;
            if (buildingAI.m_waterIntake != 0 && buildingAI.m_waterOutlet != 0 && buildingAI.m_waterStorage != 0)
            {
                // building is Tank Reservoir
                used = data.m_customBuffer1 * 1000;     // game logic includes data.m_waterBuffer, but this is always small compared to m_customBuffer1 and can put the used over the allowed
                allowed = buildingAI.m_waterStorage;
            }
            else if (buildingAI.m_sewageOutlet != 0 && buildingAI.m_sewageStorage != 0 && buildingAI.m_pumpingVehicles != 0)
            {
                // building is Pumping Service
                used = data.m_customBuffer2 * 1000;    // game logic includes data.m_sewageBuffer, but this is always small compared to m_customBuffer2 and can put the used over the allowed
                allowed = buildingAI.m_sewageStorage;
            }
            else
            {
                // other water building with no storage
                used = 0;
                allowed = 0;
            }

            // make sure used is not more than allowed
            if (used > allowed)
            {
                used = allowed;
            }
        }

        /// <summary>
        /// get the usage count of a LandfillSiteAI or derived building
        /// </summary>
        protected void GetUsageCountStorageLandfillSite<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : LandfillSiteAI
        {
            // get garbage at the building
            T buildingAI = data.Info.m_buildingAI as T;
            used = buildingAI.GetGarbageAmount(buildingID, ref data);

            // get garbage capacity/reserves
            allowed = buildingAI.m_garbageCapacity;
        }

        /// <summary>
        /// get the usage count of an ExtractingFacilityAI building
        /// logic adapted from CityServiceWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountStorageExtractingFacility(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get resources stored
            used = ComputeResourceAmount(data.m_customBuffer1);

            // get resources allowed
            ExtractingFacilityAI buildingAI = data.Info.m_buildingAI as ExtractingFacilityAI;
            allowed = ComputeResourceAmount(buildingAI.GetOutputBufferSize(buildingID, ref data));
        }

        /// <summary>
        /// get the usage count of a FishingHarborAI building
        /// logic adapted from CityServiceWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountStorageFishingHarbor(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get resources stored
            used = ComputeResourceAmount(data.m_customBuffer2 * 100);

            // get resources allowed
            FishingHarborAI buildingAI = data.Info.m_buildingAI as FishingHarborAI;
            allowed = ComputeResourceAmount(buildingAI.m_storageBufferSize);
        }

        /// <summary>
        /// get the usage count of a FishFarmAI building
        /// logic adapted from CityServiceWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountStorageFishFarm(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get resources stored
            used = ComputeResourceAmount(data.m_customBuffer2 * 100);

            // get resources allowed
            FishFarmAI buildingAI = data.Info.m_buildingAI as FishFarmAI;
            allowed = ComputeResourceAmount(buildingAI.m_storageBufferSize);
        }

        /// <summary>
        /// get the input usage count of a ProcessingFacilityAI building
        /// logic adapted from CityServiceWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountStorageProcessingFacilityInput(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get input resources stored
            used = ComputeResourceAmount(data.m_customBuffer2);

            // get input resources allowed
            ProcessingFacilityAI buildingAI = data.Info.m_buildingAI as ProcessingFacilityAI;
            allowed = ComputeResourceAmount(buildingAI.GetInputBufferSize1(buildingID, ref data));
        }

        /// <summary>
        /// get the output usage count of a ProcessingFacilityAI building
        /// logic adapted from CityServiceWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountStorageProcessingFacilityOutput(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get output resources stored
            used = ComputeResourceAmount(data.m_customBuffer1);

            // get output resources allowed
            ProcessingFacilityAI buildingAI = data.Info.m_buildingAI as ProcessingFacilityAI;
            allowed = ComputeResourceAmount(buildingAI.GetOutputBufferSize(buildingID, ref data));
        }

        /// <summary>
        /// get the total (input + output) usage count of a ProcessingFacilityAI building
        /// </summary>
        protected void GetUsageCountStorageProcessingFacilityTotal(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get input resources
            int usedInput = 0;
            int allowedInput = 0;
            GetUsageCountStorageProcessingFacilityInput(buildingID, ref data, ref usedInput, ref allowedInput);

            // get output resources
            int usedOutput = 0;
            int allowedOutput = 0;
            GetUsageCountStorageProcessingFacilityOutput(buildingID, ref data, ref usedOutput, ref allowedOutput);

            // add input and output
            used = usedInput + usedOutput;
            allowed = allowedInput + allowedOutput;
        }

        /// <summary>
        /// get the input usage count of a UniqueFactoryAI building
        /// logic adapted from UniqueFactoryWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountStorageUniqueFactoryInput(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get input resources stored and allowed
            UniqueFactoryAI buildingAI = data.Info.m_buildingAI as UniqueFactoryAI;
            if (buildingAI.m_inputResource1 != TransferManager.TransferReason.None)
            {
                used += ComputeResourceAmount(data.m_customBuffer2);
                allowed += ComputeResourceAmount(buildingAI.GetInputBufferSize1(buildingID, ref data));
            }
            if (buildingAI.m_inputResource2 != TransferManager.TransferReason.None)
            {
                used += ComputeResourceAmount((data.m_teens << 8) | data.m_youngs);
                allowed += ComputeResourceAmount(buildingAI.GetInputBufferSize2(buildingID, ref data));
            }
            if (buildingAI.m_inputResource3 != TransferManager.TransferReason.None)
            {
                used += ComputeResourceAmount((data.m_adults << 8) | data.m_seniors);
                allowed += ComputeResourceAmount(buildingAI.GetInputBufferSize3(buildingID, ref data));
            }
            if (buildingAI.m_inputResource4 != TransferManager.TransferReason.None)
            {
                used += ComputeResourceAmount((data.m_education1 << 8) | data.m_education2);
                allowed += ComputeResourceAmount(buildingAI.GetInputBufferSize4(buildingID, ref data));
            }
        }

        /// <summary>
        /// get the output usage count of a UniqueFactoryAI building
        /// logic adapted from UniqueFactoryWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountStorageUniqueFactoryOutput(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get output resources stored
            used = ComputeResourceAmount(data.m_customBuffer1);

            // get output resources allowed
            UniqueFactoryAI buildingAI = data.Info.m_buildingAI as UniqueFactoryAI;
            allowed = ComputeResourceAmount(buildingAI.GetOutputBufferSize(buildingID, ref data));
        }

        /// <summary>
        /// get the total (input + output) usage count of a UniqueFactoryAI building
        /// </summary>
        protected void GetUsageCountStorageUniqueFactoryTotal(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get input resources
            int usedInput = 0;
            int allowedInput = 0;
            GetUsageCountStorageUniqueFactoryInput(buildingID, ref data, ref usedInput, ref allowedInput);

            // get output resources
            int usedOutput = 0;
            int allowedOutput = 0;
            GetUsageCountStorageUniqueFactoryOutput(buildingID, ref data, ref usedOutput, ref allowedOutput);

            // add input and output
            used = usedInput + usedOutput;
            allowed = allowedInput + allowedOutput;
        }

        /// <summary>
        /// get the usage count of a WarehouseAI building
        /// logic adapted from WarehouseWorldInfoPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountStorageWarehouse(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get resources stored
            used = ComputeResourceAmount(data.m_customBuffer1 * 100);

            // get resources allowed
            WarehouseAI buildingAI = data.Info.m_buildingAI as WarehouseAI;
            allowed = ComputeResourceAmount(buildingAI.m_storageCapacity);
        }

        /// <summary>
        /// get the unsorted usage count of a PostOfficeAI building
        /// </summary>
        protected void GetUsageCountStoragePostOfficeUnsorted(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get unsorted stored
            used = data.m_customBuffer1 * 100;

            // get unsorted allowed
            allowed = ComputePostAllowed(ref data);
        }

        /// <summary>
        /// get the sorted usage count of a PostOfficeAI building
        /// </summary>
        protected void GetUsageCountStoragePostOfficeSorted(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // get sorted stored
            used = data.m_customBuffer2 * 100;

            // get sorted allowed
            allowed = ComputePostAllowed(ref data);
        }

        /// <summary>
        /// get the cash usage count of a Commercial building
        /// </summary>
        protected void GetUsageCountStorageCommercialCash(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // initialize
            used = 0;
            allowed = 0;

            // building must be created
            Building building = BuildingManager.instance.m_buildings.m_buffer[buildingID];
            if ((building.m_flags & Building.Flags.Created) != 0)
            {
                // building AI must be defined
                if (building.Info != null && building.Info.m_buildingAI != null)
                {
                    // building AI must be CommercialBuildingAI or derived from it
                    // this will include PloppableRICO.PloppableCommercialAI and PloppableRICO.GrowableCommercialAI which derive from CommercialBuildingAI
                    BuildingAI buildingAI = building.Info.m_buildingAI;
                    Type buildingAIType = buildingAI.GetType();
                    if (buildingAIType == typeof(CommercialBuildingAI) || buildingAIType.IsSubclassOf(typeof(CommercialBuildingAI)))
                    {
                        // get the method info for the cash capacity function
                        MethodInfo getCashCapacity = typeof(CommercialBuildingAI).GetMethod("GetCashCapacity", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (getCashCapacity == null)
                        {
                            LogUtil.LogError("Unable to get method [CommercialBuildingAI.GetCashCapacity]");
                            return;
                        }

                        // get the building cash amount and capacity
                        // logic adapted from CommercialBuildingAI.GetDebugString
                        used = building.m_cashBuffer / 10;
                        allowed = (int)getCashCapacity.Invoke((CommercialBuildingAI)buildingAI, new object[] { buildingID, building }) / 10;
                    }
                }
            }
        }

        /// <summary>
        /// compute a resource amount from a buffer amount
        /// logic adapted from IndustryWorldInfoPanel.FormatResource
        /// </summary>
        private int ComputeResourceAmount(int bufferAmount)
        {
            return (int)Mathf.Round(bufferAmount / 1000f);
        }

        /// <summary>
        /// compute the amount allowed for PostOfficeAI
        /// </summary>
        private int ComputePostAllowed(ref Building data)
        {
            // get mail allowed
            PostOfficeAI buildingAI = data.Info.m_buildingAI as PostOfficeAI;
            int allowed = buildingAI.m_mailCapacity;

            // check for district modifier
            DistrictManager instance = DistrictManager.instance;
            byte district = instance.GetDistrict(data.m_position);
            DistrictPolicies.Services servicePolicies = instance.m_districts.m_buffer[district].m_servicePolicies;
            if ((servicePolicies & DistrictPolicies.Services.AutomatedSorting) != 0)
            {
                allowed += allowed / 10;
            }

            return allowed / 10;
        }
        #endregion

        #region "Vehicles Usage Counts"


        // In all the GetUsageCountVehicles* routines below:
        // The number of vehicles being used and the number vehicles allowed are displayed on the CityServiceWorldInfoPanel or WarehouseWorldInfoPanel.
        // For both info panels, UpdateBindings gets the vehicle info by calling GetLocalizedStats method of the building AI associated with the building.
        // The logic in each routine is a copy (using ILSpy) of [building AI].GetLocalizedStats.
        // The logic was then simplified to include only the parts for computing vehicle info.


        /// <summary>
        /// get the usage count of a MaintenanceDepotAI building
        /// </summary>
        protected void GetUsageCountVehiclesMaintenanceDepot(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // determine transfer reason from service
            TransferManager.TransferReason transferReason = TransferManager.TransferReason.None;
            ItemClass.Service service = data.Info.m_class.m_service;
            if (service == ItemClass.Service.Road)
            {
                transferReason = TransferManager.TransferReason.RoadMaintenance;
            }
            else if (service == ItemClass.Service.Beautification)
            {
                transferReason = TransferManager.TransferReason.ParkMaintenance;
            }

            // compute vehicles used
            CalculateOwnVehicles(ref data, transferReason, ref used);

            // get production rate
            MaintenanceDepotAI buildingAI = data.Info.m_buildingAI as MaintenanceDepotAI;
            int budget = EconomyManager.instance.GetBudget(buildingAI.m_info.m_class);
            int productionRate = PlayerBuildingAI.GetProductionRate(100, budget);

            // check for park maintenance boost
            if (transferReason == TransferManager.TransferReason.ParkMaintenance)
            {
                DistrictManager instance = DistrictManager.instance;
                byte district = instance.GetDistrict(data.m_position);
                DistrictPolicies.Services servicePolicies = instance.m_districts.m_buffer[district].m_servicePolicies;
                if ((servicePolicies & DistrictPolicies.Services.ParkMaintenanceBoost) != 0)
                {
                    productionRate *= 2;
                }
            }

            // compute vehicles allowed
            allowed = (productionRate * buildingAI.m_maintenanceTruckCount + 99) / 100;
        }

        /// <summary>
        /// get the usage count of a SnowDumpAI building
        /// </summary>
        protected void GetUsageCountVehiclesSnowDump(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            if ((data.m_flags & Building.Flags.Downgrading) != 0)
            {
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.SnowMove, ref used);
            }
            else
            {
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.Snow, ref used);
            }

            // compute vehicles allowed
            CalculateAllowedVehicles<SnowDumpAI>(ref data, "m_snowTruckCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a WaterFacilityAI building
        /// </summary>
        protected void GetUsageCountVehiclesWaterFacility(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.FloodWater, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<WaterFacilityAI>(ref data, "m_pumpingVehicles", ref allowed);
        }

        /// <summary>
        /// get the usage count of a LandfillSiteAI or derived building
        /// </summary>
        protected void GetUsageCountVehiclesLandfillSite<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : LandfillSiteAI
        {
            // compute vehicles used
            if ((data.m_flags & Building.Flags.Downgrading) != 0)
            {
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.GarbageMove, ref used);
            }
            else
            {
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.Garbage, ref used);
            }

            // compute vehicles allowed
            CalculateAllowedVehicles<T>(ref data, "m_garbageTruckCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of an ExtractingFacilityAI building
        /// </summary>
        protected void GetUsageCountVehiclesExtractingFacility(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            ExtractingFacilityAI buildingAI = data.Info.m_buildingAI as ExtractingFacilityAI;
            CalculateOwnVehicles(ref data, buildingAI.m_outputResource, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<ExtractingFacilityAI>(ref data, "m_outputVehicleCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a FishingHarborAI building
        /// </summary>
        protected void GetUsageCountVehiclesFishingHarbor(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute boats and vehicles used
            CalculateOwnVehicles(ref data, ref used);

            // compute boats and vehicles allowed
            CalculateAllowedVehicles<FishingHarborAI>(ref data, "m_boatCount",          ref allowed);
            CalculateAllowedVehicles<FishingHarborAI>(ref data, "m_outputVehicleCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a FishFarmAI building
        /// </summary>
        protected void GetUsageCountVehiclesFishFarm(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            FishFarmAI buildingAI = data.Info.m_buildingAI as FishFarmAI;
            CalculateOwnVehicles(ref data, buildingAI.m_outputResource, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<FishFarmAI>(ref data, "m_outputVehicleCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a ProcessingFacilityAI or derived building
        /// </summary>
        protected void GetUsageCountVehiclesProcessingFacility<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : ProcessingFacilityAI
        {
            // compute vehicles used
            T buildingAI = data.Info.m_buildingAI as T;
            CalculateOwnVehicles(ref data, buildingAI.m_outputResource, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<T>(ref data, "m_outputVehicleCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a WarehouseAI building
        /// </summary>
        protected void GetUsageCountVehiclesWarehouse(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            WarehouseAI buildingAI = data.Info.m_buildingAI as WarehouseAI;
            TransferManager.TransferReason actualTransferReason = buildingAI.GetActualTransferReason(buildingID, ref data);
            CalculateOwnVehicles(ref data, actualTransferReason, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<WarehouseAI>(ref data, "m_truckCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a HospitalAI or derived building
        /// </summary>
        protected void GetUsageCountVehiclesHospital<T>(ushort buildingID, ref Building data, ref int used, ref int allowed) where T : HospitalAI
        {
            // compute vehicles used
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.Sick, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<T>(ref data, "AmbulanceCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a HelicopterDepotAI building
        /// </summary>
        protected void GetUsageCountVehiclesHelicopterDepot(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // convert building service to transfer reason(s)
            if (data.Info.m_class.m_service == ItemClass.Service.HealthCare)
            {
                // VehiclesMedicalHelis
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.Sick2, ref used);
            }
            else if (data.Info.m_class.m_service == ItemClass.Service.FireDepartment)
            {
                // VehiclesFireHelis
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.Fire2, ref used);
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.ForestFire, ref used);
            }
            else if (data.Info.m_class.m_service == ItemClass.Service.PoliceDepartment)
            {
                // VehiclesPoliceHelis
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.Crime, ref used);
            }

            // compute vehicles allowed
            CalculateAllowedVehicles<HelicopterDepotAI>(ref data, "m_helicopterCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a CemeteryAI building
        /// </summary>
        protected void GetUsageCountVehiclesCemetery(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            if ((data.m_flags & Building.Flags.Downgrading) != 0)
            {
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.DeadMove, ref used);
            }
            else
            {
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.Dead, ref used);
            }

            // compute vehicles allowed
            CalculateAllowedVehicles<CemeteryAI>(ref data, "m_hearseCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a FireStationAI building
        /// </summary>
        protected void GetUsageCountVehiclesFireStation(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.Fire, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<FireStationAI>(ref data, "m_fireTruckCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a DisasterResponseBuildingAI building
        /// </summary>
        protected void GetUsageCountVehiclesDisasterResponseBuilding(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // vehicles and helis are combined because they come from the same building

            // compute vehicles and helicopters used
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.Collapsed,  ref used);   // vehicles
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.Collapsed2, ref used);   // helicopters

            // compute vehicles and helicopters allowed
            CalculateAllowedVehicles<DisasterResponseBuildingAI>(ref data, "m_vehicleCount",    ref allowed);
            CalculateAllowedVehicles<DisasterResponseBuildingAI>(ref data, "m_helicopterCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a ShelterAI building
        /// </summary>
        protected void GetUsageCountVehiclesShelter(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.EvacuateA,    ref used);
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.EvacuateB,    ref used);
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.EvacuateC,    ref used);
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.EvacuateD,    ref used);
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.EvacuateVipA, ref used);
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.EvacuateVipB, ref used);
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.EvacuateVipC, ref used);
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.EvacuateVipD, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<ShelterAI>(ref data, "m_evacuationBusCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of PoliceStationAI building
        /// </summary>
        protected void GetUsageCountVehiclesPoliceStation(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used based on usage type
            if (GetUsageType1ForBuilding(buildingID, ref data) == UsageType.VehiclesPrisonVans)
            {
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.CriminalMove, ref used);      // prison vans
            }
            else
            {
                CalculateOwnVehicles(ref data, TransferManager.TransferReason.Crime, ref used);             // police cars
            }

            // compute vehicles allowed
            CalculateAllowedVehicles<PoliceStationAI>(ref data, "PoliceCarCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a BankOfficeAI building
        /// </summary>
        protected void GetUsageCountVehiclesBank(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.Cash, ref used);

            // compute vehicles allowed
            CalculateAllowedVehicles<BankOfficeAI>(ref data, "CollectingVanCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a PostOfficeAI building
        /// </summary>
        protected void GetUsageCountVehiclesPostVansTrucks(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // vans and trucks are combined because they can come from the same building

            // compute vehicles used
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.Mail,         ref used);   // van
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.UnsortedMail, ref used);   // truck
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.SortedMail,   ref used);   // truck
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.OutgoingMail, ref used);   // truck
            CalculateOwnVehicles(ref data, TransferManager.TransferReason.IncomingMail, ref used);   // truck

            // compute vehicles allowed
            CalculateAllowedVehicles<PostOfficeAI>(ref data, "m_postVanCount", ref allowed);
            CalculateAllowedVehicles<PostOfficeAI>(ref data, "m_postTruckCount", ref allowed);
        }

        /// <summary>
        /// get the usage count of a PrivateAirportAI building
        /// logic adapted from PrivateAirportAI.CheckVehicles
        /// </summary>
        protected void GetUsageCountVehiclesPrivatePlanes(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used
            CalculateOwnVehicles(ref data, ref used);

            // compute vehicles allowed
            PrivateAirportAI buildingAI = data.Info.m_buildingAI as PrivateAirportAI;
            allowed = ((data.m_flags & (Building.Flags.Evacuating | Building.Flags.Active)) == Building.Flags.Active) ? buildingAI.m_vehicleCount : 0;

            // check for policy modifier
            DistrictManager instance = DistrictManager.instance;
            byte district = instance.GetDistrict(data.m_position);
            DistrictPolicies.CityPlanning cityPlanningPolicies = instance.m_districts.m_buffer[district].m_cityPlanningPolicies;
            if ((cityPlanningPolicies & DistrictPolicies.CityPlanning.AirplaneTours) != 0)
            {
                allowed = (allowed * 500 + 99) / 100;
            }
        }

        /// <summary>
        /// return whether or not the ChirpX Launch site is available in the game
        /// </summary>
        protected bool ChirpXLaunchSiteAvailable()
        {
            // loop over all the building prefabs
            int buildingPrefabCount = PrefabCollection<BuildingInfo>.LoadedCount();
            for (uint index = 0; index < buildingPrefabCount; index++)
            {
                // find the MonumentAI prefabs
                BuildingInfo prefab = PrefabCollection<BuildingInfo>.GetLoaded(index);
                Type buildingAIType = prefab.m_buildingAI.GetType();
                if (buildingAIType == typeof(MonumentAI))
                {
                    // check if building AI supports the launch event
                    MonumentAI buildingAI = (MonumentAI)prefab.m_buildingAI as MonumentAI;
                    if (buildingAI.m_supportEvents == EventManager.EventType.RocketLaunch)
                    {
                        return true;
                    }
                }
            }

            // no monument building found that supports launch event
            return false;
        }

        /// <summary>
        /// get the usage count of the ChirpX Launch Site building
        /// logic adapted from ChirpXPanel.UpdateBindings
        /// </summary>
        protected void GetUsageCountVehiclesChirpXLaunchSite(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // check if building AI supports the launch event
            MonumentAI buildingAI = (MonumentAI)data.Info.m_buildingAI as MonumentAI;
            if (buildingAI.m_supportEvents == EventManager.EventType.RocketLaunch)
            {
                // if rocket is ready to launch, then used is 1
                ushort currentEventID = BuildingManager.instance.m_buildings.m_buffer[buildingID].m_eventIndex;
                EventData eventData = EventManager.instance.m_events.m_buffer[currentEventID];
                if ((eventData.m_flags & EventData.Flags.Ready) != 0)
                {
                    used = 1;
                }

                // the fact that the building is built means 1 rocket is allowed
                allowed = 1;
            }
        }

        /// <summary>
        /// get the usage count of buildings with transportation vehicles
        /// transportation buildings all have unlimited number of vehicles
        /// </summary>
        protected void GetUsageCountVehiclesTransportation(ushort buildingID, ref Building data, ref int used, ref int allowed)
        {
            // compute vehicles used, regardless of vehicle type
            CalculateOwnVehicles(ref data, ref used);

            // for transportation buildings, always 1 allowed
            allowed = 1;
        }

        /// <summary>
        /// return number of vehicles used (i.e. owned) by the building with any transfer reason
        /// </summary>
        private void CalculateOwnVehicles(ref Building data, ref int used)
        {
            // The logic below is a copy (using ILSpy) of CommonBuildingAI.CalculateOwnVehicles.
            // The logic was then simplified to include only the parts for computing vehicle info.

            // do each vehicle owned by the building
            int vehicleCounter = 0;
            VehicleManager instance = VehicleManager.instance;
            uint maximumVehicles = instance.m_vehicles.m_size;
            ushort vehicleID = data.m_ownVehicles;
            while (vehicleID != 0)
            {
                // every vehicle owned by the building is considered used
                used++;

                // get the next vehicle
                vehicleID = instance.m_vehicles.m_buffer[vehicleID].m_nextOwnVehicle;

                // check for error (e.g. circular reference)
                if (++vehicleCounter > maximumVehicles)
                {
                    LogUtil.LogError("Invalid list detected!" + Environment.NewLine + Environment.StackTrace);
                    break;
                }
            }
        }

        /// <summary>
        /// return number of vehicles used (i.e. owned) by the building with a specific transfer reason
        /// </summary>
        private void CalculateOwnVehicles(ref Building data, TransferManager.TransferReason transferReason, ref int used)
        {
            // The logic below is a copy (using ILSpy) of CommonBuildingAI.CalculateOwnVehicles.
            // The logic was then simplified to include only the parts for computing vehicle info.

            // do each vehicle owned by the building
            int vehicleCounter = 0;
            VehicleManager instance = VehicleManager.instance;
            uint maximumVehicles = instance.m_vehicles.m_size;
            ushort vehicleID = data.m_ownVehicles;
            while (vehicleID != 0)
            {
                // check if vehicle should be counted
                if ((TransferManager.TransferReason)instance.m_vehicles.m_buffer[vehicleID].m_transferType == transferReason)
                {
                    used++;
                }

                // get the next vehicle
                vehicleID = instance.m_vehicles.m_buffer[vehicleID].m_nextOwnVehicle;

                // check for error (e.g. circular reference)
                if (++vehicleCounter > maximumVehicles)
                {
                    LogUtil.LogError("Invalid list detected!" + Environment.NewLine + Environment.StackTrace);
                    break;
                }
            }
        }

        /// <summary>
        /// return the number of vehicles allowed for the building
        /// </summary>
        private void CalculateAllowedVehicles<T>(ref Building data, string vehicleCountFieldPropertyName, ref int allowed) where T : PlayerBuildingAI
        {
            // The number vehicles allowed are displayed on the CityServiceWorldInfoPanel or WarehouseWorldInfoPanel.
            // For both info panels, UpdateBindings gets the vehicle info by calling GetLocalizedStats method of the building AI associated with the building.
            // The logic below is a copy (using ILSpy) of [building AI].GetLocalizedStats used by most of the building AIs.
            // The logic was then simplified to include only the parts for computing vehicle info.

            // get the vehicle count value, try field first
            int value;
            T buildingAI = data.Info.m_buildingAI as T;
            FieldInfo field = typeof(T).GetField(vehicleCountFieldPropertyName);
            if (field != null)
            {
                // field found, get its value
                value = (int)field.GetValue(buildingAI);
            }
            else
            {
                // field not found, get property
                PropertyInfo property = typeof(T).GetProperty(vehicleCountFieldPropertyName);
                if (property != null)
                {
                    // property found, get its value
                    value = (int)property.GetValue(buildingAI, null);
                }
                else
                {
                    LogUtil.LogError($"Unable to get field or property [{vehicleCountFieldPropertyName}] for building AI [{typeof(T).Name}].");
                    return;
                }
            }

            // do final calculation, which is the same for all cases
            int budget = EconomyManager.instance.GetBudget(buildingAI.m_info.m_class);
            int productionRate = PlayerBuildingAI.GetProductionRate(100, budget);
            allowed += (productionRate * value + 99) / 100;
        }
        #endregion

        #region "Get Building Color"

        /// <summary>
        /// get the usage type for a building when logic is required
        /// </summary>
        protected abstract UsageType GetUsageType1ForBuilding(ushort buildingID, ref Building data);
        protected abstract UsageType GetUsageType2ForBuilding(ushort buildingID, ref Building data);

        /// <summary>
        /// update the building usage counts and return the building color accordingly
        /// </summary>
        public Color GetBuildingColor(ushort buildingID, ref Building data)
        {
            // if a detail panel is currently displayed, get building color from the detail panel instead
            if (_detailPanel != null)
            {
                // this is a recursive call, but to a different panel
                return _detailPanel.GetBuildingColor(buildingID, ref data);
            }

            // get neutral color
            Color neutralColor = InfoManager.instance.m_properties.m_neutralColor;

            try
            {
                // check if the building AI type is being tracked by this panel
                Type buildingAIType = data.Info.m_buildingAI.GetType();
                if (_buildingAIUsages.TryGetValue(buildingAIType, out UsageTypeMethod usageTypeMethod))
                {
                    // include the building if it is completed or upgrading and not abandoned or collapsed (collapsed and burned down are the same)
                    bool include = ((data.m_flags & (Building.Flags.Completed | Building.Flags.Upgrading)) != 0) &&
                                   ((data.m_flags & (Building.Flags.Abandoned | Building.Flags.Collapsed)) == 0);

                    // get usage type
                    UsageType usageType1 = usageTypeMethod.usageType1;
                    if (usageType1 == UsageType.UseLogic1)
                    {
                        usageType1 = GetUsageType1ForBuilding(buildingID, ref data);
                    }

                    // call the usage count method or the employed/total jobs mnethod or the unemployed/eligible count method
                    int used1 = 0;
                    int allowed1 = 0;
                    if (usageTypeMethod.usageCountMethod1 != null)
                    {
                        usageTypeMethod.usageCountMethod1(buildingID, ref data, ref used1, ref allowed1);
                        UpdateUsageGroup(usageType1, buildingID, include, used1, allowed1);
                    }
                    else if(usageTypeMethod.employedTotalJobsCountMethod1 != null)
                    {
                        EducationLevelCount employed1  = new EducationLevelCount();
                        EducationLevelCount totalJobs1 = new EducationLevelCount();
                        EducationLevelCount overEd1    = new EducationLevelCount();
                        usageTypeMethod.employedTotalJobsCountMethod1(buildingID, ref data, ref used1, ref allowed1, ref employed1, ref totalJobs1);
                        ComputeOverEducatedWorkers(employed1, totalJobs1, ref overEd1);
                        UpdateUsageGroup(usageType1, buildingID, include, used1, allowed1, employed1, totalJobs1, overEd1, new EducationLevelCount(), new EducationLevelCount());
                    }
                    else if (usageTypeMethod.unemployedEligibleCountMethod1 != null)
                    {
                        EducationLevelCount unemployed1 = new EducationLevelCount();
                        EducationLevelCount eligible1   = new EducationLevelCount();
                        int nothing1 = 0;
                        usageTypeMethod.unemployedEligibleCountMethod1(buildingID, ref data, ref used1, ref allowed1, ref unemployed1, ref eligible1, ref nothing1);
                        UpdateUsageGroup(usageType1, buildingID, include, used1, allowed1, new EducationLevelCount(), new EducationLevelCount(), new EducationLevelCount(), unemployed1, eligible1);
                    }

                    // get the color
                    Color color1 = GetUsageColor(usageType1, include, used1, allowed1);

                    // check if there is a second usage type
                    UsageType usageType2 = usageTypeMethod.usageType2;
                    Color color2 = neutralColor;
                    if (usageType2 != UsageType.None)
                    {
                        // get usage type
                        if (usageType2 == UsageType.UseLogic2)
                        {
                            usageType2 = GetUsageType2ForBuilding(buildingID, ref data);
                        }

                        // call the usage count method
                        // usage type 2 does not have employed/total jobs counting or unemployed/eligible counting because it would be a double count
                        int used2 = 0;
                        int allowed2 = 0;
                        if (usageTypeMethod.usageCountMethod2 != null)
                        {
                            usageTypeMethod.usageCountMethod2(buildingID, ref data, ref used2, ref allowed2);
                            UpdateUsageGroup(usageType2, buildingID, include, used2, allowed2);
                        }

                        // get the color
                        color2 = GetUsageColor(usageType2, include, used2, allowed2);
                    }

                    // return whichever color is not neutral
                    if (color1 != neutralColor)
                    {
                        return color1;
                    }
                    else
                    {
                        return color2;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }

            // for any building AI type not handled above, use neutral color
            return neutralColor;
        }

        /// <summary>
        /// return over educated workers
        /// logic is copied from ZonedBuildingWorldInfoPanel.UpdateWorkers
        /// </summary>
        private void ComputeOverEducatedWorkers(EducationLevelCount employed, EducationLevelCount totalJobs, ref EducationLevelCount overEducated)
        {
            // uneducated workers can never be over educated

            // compute educated workers that are over educated
            int num10 = totalJobs.level0 - employed.level0;
            if (employed.level1 > totalJobs.level1)
            {
                overEducated.level1 = Mathf.Max(0, Mathf.Min(num10, employed.level1 - totalJobs.level1));
            }

            // compute well educated workers that are over educated
            num10 += totalJobs.level1 - employed.level1;
            if (employed.level2 > totalJobs.level2)
            {
                overEducated.level2 = Mathf.Max(0, Mathf.Min(num10, employed.level2 - totalJobs.level2));
            }

            // compute highly educated workers that are over educated
            num10 += totalJobs.level2 - employed.level2;
            if (employed.level3 > totalJobs.level3)
            {
                overEducated.level3 = Mathf.Max(0, Mathf.Min(num10, employed.level3 - totalJobs.level3));
            }
        }
        #endregion

        #region "Get Vehicle Color"

        /// <summary>
        /// get the usage type for a vehicle when logic is required
        /// </summary>
        protected abstract UsageType GetUsageTypeForVehicle(ushort vehicleID, ref Vehicle data);

        /// <summary>
        /// return the vehicle color
        /// </summary>
        public Color GetVehicleColor(ushort vehicleID, ref Vehicle data)
        {
            // if a detail panel is currently displayed, get vehicle color from the detail panel instead
            if (_detailPanel != null)
            {
                // this is a recursive call, but to a different panel
                return _detailPanel.GetVehicleColor(vehicleID, ref data);
            }

            try
            {
                // check that the vehicle AI type is defined
                Type vehicleAIType = data.Info.m_vehicleAI.GetType();
                if (_vehicleAIUsages.TryGetValue(vehicleAIType, out UsageType usageType))
                {
                    // check the usage type
                    if (usageType == UsageType.UseLogic1 || usageType == UsageType.UseLogic2)
                    {
                        usageType = GetUsageTypeForVehicle(vehicleID, ref data);
                    }

                    // return the color based on 100 percent usage
                    return GetUsageColor(usageType, true, 1, 1);
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }

            // for any vehicle AI not handled above, use neutral color
            return InfoManager.instance.m_properties.m_neutralColor;
        }
        #endregion

        #region "Transportation Building Usage Types"

        // Here are the combinations of building AI, AI name (case insensitive), and/or metro presence that define the usage type for each transportation building
        //
        // Building AI                      Building Name                               AI Name Contains                        Metro Subbuilding   WorkersTransportation   VehiclesTransportation
        // --------------------------       ------------------------------------------  ------------------                      ------------------  ----------------------  ----------------------
        // AirportAuxBuildingAI        WV-- Parked Cargo Plane (2 variations)           Cargo                                   No                  AirCargo                None
        //                                  Concourse Hub (3 styles)                    not Cargo                               Yes                 AirPeople               Metro
        //                                  (all others, many)                          not Cargo                               No                  AirPeople               None


        // Here are the combinations of building AI, class level, and/or metro presence that define the usage type for each transportation building
        //
        // Building AI                      Building Name                               Class Level                             Metro Subbuilding   WorkersTransportation   VehiclesTransportation
        // --------------------------       ------------------------------------------  ------------------                      ------------------  ----------------------  ----------------------
        // AirportEntranceAI           WV-- Cargo Airport Terminal                      4                                       No                  AirCargo                None
        //                                  Airport Terminal (3 styles)                 1                                       No                  AirPeople               None
        //                                  Two-Story Terminal (3 styles)               1                                       Yes                 AirPeople               Metro
        //                                  Large Terminal (3 styles)                   1                                       No                  AirPeople               None


        // Here are the combinations of building AI, vehicle reasons, and/or metro presence that define the usage type for each transportation building
        //
        // Building AI                      Building Name                               Vehicle Reason 1    Vehicle Reason 2    Metro Subbuilding   WorkersTransportation   VehiclesTransportation
        // --------------------------       ------------------------------------------  ------------------  ------------------  ------------------  ----------------------  ----------------------
        // CargoStationAI              W--U Cargo Train Terminal                        PassengerTrain      null                                    TrainCargo              TrainCargo
        //                                  Airport Cargo Train Station                 PassengerTrain      PassengerTrain                          TrainCargo              TrainCargo
        //                                  Cargo Airport                               PassengerPlane      null                                    AirCargo                AirCargo
        //                                  Cargo Airport Hub                           PassengerPlane      PassengerTrain                          AirCargo                AirCargo
        //    AirportCargoGateAI            Cargo Aircraft Stand                        PassengerPlane      null                                    AirCargo                AirCargo
        //    CargoHarborAI            W--U Cargo Harbor                                PassengerShip       null                                    ShipCargo               ShipCargo
        //                                  Cargo Hub                                   PassengerTrain      PassengerShip                           ShipCargo               ShipCargo
        //    WarehouseStationAI       ---- Warehouse with Railway Connection           ----                ----                                    None                    None
        // DepotAI                     W--U Bus Depot                                   Bus                 null                                    Bus                     Bus
        //                                  Biofuel Bus Depot                           Bus                 null                                    Bus                     Bus
        //                                  ROJ Japanese Bus Depot                      Bus                 null                                    Bus                     Bus
        //                                  Trolleybus Depot                            TrolleyBus          null                                    Trolleybus              TrolleyBus
        //                                  Tram Depot                                  Tram                null                                    Tram                    Tram
        //                                  Ferry Depot                                 Ferry               null                                    ShipPeople              ShipPeople
        //                                  Helicopter Depot                            PassengerHelicopter null                                    AirPeople               AirPeople
        //                                  Blimp Depot                                 Blimp               null                                    AirPeople               AirPeople
        //                                  Sightseeing Bus Depot                       TouristBus          null                                    Tours                   Tours
        //                                  Taxi Depot                                  Taxi                null                                    Taxi                    Taxi
        //    CableCarStationAI        W--U Cable Car Stop                              CableCar            null                                    CableCar                CableCar
        //                                  End-of-Line Cable Car Stop                  CableCar            null                                    CableCar                CableCar
        //    TransportStationAI       W--- Bus Station                                 Bus                 null                                    Bus                     None
        //                                  Compact Bus Station                         Bus                 null                                    Bus                     None
        //                                  Helicopter Stop                             PassengerHelicopter null                                    AirPeople               None
        //                                  Blimp Stop                                  Blimp               null                                    AirPeople               None
        //    TransportStationAI       W--U Intercity Bus Station                       IntercityBus        null                                    IntercityBus            IntercityBus
        //                                  Intercity Bus Terminal                      IntercityBus        null                                    IntercityBus            IntercityBus
        //                                  Metro Station                               MetroTrain          null                                    Metro                   Metro
        //                                  Elevated Metro Station                      MetroTrain          null                                    Metro                   Metro
        //                                  Elevated Metro Station With Shops           MetroTrain          null                                    Metro                   Metro
        //                                  Underground Metro Station                   MetroTrain          null                                    Metro                   Metro
        //                                  Parallel Underground Metro Station          MetroTrain          null                                    Metro                   Metro
        //                                  Large Underground Metro Station             MetroTrain          null                                    Metro                   Metro
        //                                  Metro Plaza Station                         MetroTrain          null                                    Metro                   Metro
        //                                  Sunken Island Platform Metro Station        MetroTrain          null                                    Metro                   Metro
        //                                  Sunken Dual Island Platform Metro Station   MetroTrain          null                                    Metro                   Metro
        //                                  Sunken Bypass Metro Station                 MetroTrain          null                                    Metro                   Metro
        //                                  Elevated Island Platform Metro Station      MetroTrain          null                                    Metro                   Metro
        //                                  Elevated Dual Island Platform Metro Station MetroTrain          null                                    Metro                   Metro
        //                                  Elevated Bypass Metro Station               MetroTrain          null                                    Metro                   Metro
        //                                  Stadium Station                             MetroTrain          null                                    Metro                   Metro
        //                                  ROJ Metro Ground Station                    MetroTrain          null                                    Metro                   Metro
        //                                  ROJ Metro Entrance                          MetroTrain          null                                    Metro                   Metro
        //                                  ROJ Metro Elevated Station                  MetroTrain          null                                    Metro                   Metro
        //                                  ROJ Modern Metro Terminal                   MetroTrain          null                                    Metro                   Metro
        //                                  ROJ Small Metro Terminal                    MetroTrain          null                                    Metro                   Metro
        //                                  ROJ Classic Metro Terminal                  MetroTrain          null                                    Metro                   Metro
        //                                  Train Station                               PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Elevated Train Station                      PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Crossover Train Station Hub                 PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Old Market Station                          PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Ground Island Platform Train Station        PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Ground Dual Island Platform Train Station   PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Ground Bypass Train Station                 PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Elevated Island Platform Train Station      PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Elevated Dual Island Platform Train Station PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Elevated Bypass Train Station               PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Historical Train Station                    PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  ROJ Small Elevated Station                  PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  ROJ Small Ground Station                    PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  ROJ Large Ground Station                    PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  ROJ Large Elevated Station                  PassengerTrain      null                No                  TrainPeople             TrainPeople
        //                                  Glass Box Transport Hub                     PassengerTrain      null                Yes                 Hubs                    Hubs
        //                                  Airport                                     PassengerPlane      null                                    AirPeople               AirPeople
        //                                  Monorail Station                            Monorail            null                                    Monorail                Monorail
        //                                  Monorail Station with Road                  Monorail            null                                    Monorail                Monorail
        //                              None of the above TransportStationAI have category PublicTransportHubs.
        //                              All  of the below TransportStationAI have category PublicTransportHubs.
        //                              Therefore, first category is used to identify Hubs below, then reason and metro are used for all above.
        //                                  Bus-Intercity Bus Hub                                                                                   Hubs                    Hubs
        //                                  Bus-Metro Hub                                                                                           Hubs                    Hubs
        //                                  Bus-Train-Tram Hub                                                                                      Hubs                    Hubs
        //                                  Metro-Intercity Bus Hub                                                                                 Hubs                    Hubs
        //                                  Metro-Tram Hub with Road                                                                                Hubs                    Hubs
        //                                  Multi-level Metro Hub                                                                                   Hubs                    Hubs
        //                                  Train-Metro Hub                                                                                         Hubs                    Hubs
        //                                  Multiplatform End Station                                                                               Hubs                    Hubs
        //                                  Multiplatform Train Station                                                                             Hubs                    Hubs
        //                                  International Airport                                                                                   Hubs                    Hubs
        //                                  Metropolitan Airport                                                                                    Hubs                    Hubs
        //                                  Monorail-Bus Hub                                                                                        Hubs                    Hubs
        //                                  Monorail-Tram Hub with Road                                                                             Hubs                    Hubs
        //                                  Metro-Monorail-Train Hub                                                                                Hubs                    Hubs
        //                                  Metro-Train-Monorail-Tram Hub with Road                                                                 Hubs                    Hubs
        //       AirportGateAI         W--- Airport Bus Station                         Bus                                                         Bus                     None
        //       AirportGateAI         W--U Small Aircraft Stand                        PassengerPlane                                              AirPeople               AirPeople
        //                                  Medium Aircraft Stand                       PassengerPlane                                              AirPeople               AirPeople
        //                                  Large Aircraft Stand                        PassengerPlane                                              AirPeople               AirPeople
        //                                  Elevated Airport Metro Station              MetroTrain                                                  Metro                   Metro
        //                                  Airport Train Station                       PassengerTrain                                              TrainPeople             TrainPeople
        //       HarborAI              W--- Ferry Stop                                  Ferry               null                                    ShipPeople              None
        //                                  Ferry Pier                                  Ferry               null                                    ShipPeople              None
        //                                  Ferry and Bus Exchange Stop                 Ferry               Bus                                     Hubs                    None        category PublicTransportHubs
        //                                  Ferry-Tram Hub                              Ferry               null                                    Hubs                    None        category PublicTransportHubs
        //       HarborAI              W--U Harbor                                      PassengerShip       null                                    ShipPeople              ShipPeople
        //                                  Harbor-Ferry Hub                            PassengerShip       null                                    Hubs                    Hubs        category PublicTransportHubs
        //                                  Harbor-Bus Hub                              PassengerShip       Bus                                     Hubs                    Hubs        category PublicTransportHubs
        //                                  Harbor-Bus-Monorail Hub                     PassengerShip       Bus                                     Hubs                    Hubs        category PublicTransportHubs
        // SpaceElevatorAI             W--- Space Elevator (monument)                   ----                ----                                    SpaceElevator           None
        // TourBuildingAI              -V-U Hot Air Balloon Tours                       None                ----                                    None                    Tours
        //    ChiperTourAI             -V-U Chirper Balloon Tours                       None                ----                                    None                    Tours


        /// <summary>
        /// get the usage type for a workers transportation building
        /// </summary>
        protected UsageType GetWorkersTransportationUsageType(BuildingInfo buildingInfo)
        {
            // use building AI type, AI name, class level, vehicle reasons, and/or metro presence to determine the usage type
            Type buildingAIType = buildingInfo.m_buildingAI.GetType();
            if (buildingAIType == typeof(AirportAuxBuildingAI))
            {
                return (buildingInfo.m_buildingAI.name.ToUpper().Contains("CARGO") ? UsageType.WorkersTransportationAirCargo : UsageType.WorkersTransportationAirPeople);
            }
            else if (buildingAIType == typeof(AirportEntranceAI))
            {
                return (buildingInfo.m_class.m_level == ItemClass.Level.Level4 ? UsageType.WorkersTransportationAirCargo : UsageType.WorkersTransportationAirPeople);
            }
            else if (buildingAIType == typeof(CargoStationAI))
            {
                GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                if (reason1 == TransferManager.TransferReason.PassengerTrain) { return UsageType.WorkersTransportationTrainCargo; }
                if (reason1 == TransferManager.TransferReason.PassengerPlane) { return UsageType.WorkersTransportationAirCargo;   }
            }
            else if (buildingAIType == typeof(AirportCargoGateAI))
            {
                return UsageType.WorkersTransportationAirCargo;
            }
            else if (buildingAIType == typeof(CargoHarborAI))
            {
                return UsageType.WorkersTransportationShipCargo;
            }
            else if (buildingAIType == typeof(DepotAI))
            {
                GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                if (reason1 == TransferManager.TransferReason.Bus                ) { return UsageType.WorkersTransportationBus;        }
                if (reason1 == TransferManager.TransferReason.Trolleybus         ) { return UsageType.WorkersTransportationTrolleybus; }
                if (reason1 == TransferManager.TransferReason.Tram               ) { return UsageType.WorkersTransportationTram;       }
                if (reason1 == TransferManager.TransferReason.Ferry              ) { return UsageType.WorkersTransportationShipPeople; }
                if (reason1 == TransferManager.TransferReason.PassengerHelicopter) { return UsageType.WorkersTransportationAirPeople;  }
                if (reason1 == TransferManager.TransferReason.Blimp              ) { return UsageType.WorkersTransportationAirPeople;  }
                if (reason1 == TransferManager.TransferReason.TouristBus         ) { return UsageType.WorkersTransportationTours;      }
                if (reason1 == TransferManager.TransferReason.Taxi               ) { return UsageType.WorkersTransportationTaxi;       }
            }
            else if (buildingAIType == typeof(CableCarStationAI))
            {
                return UsageType.WorkersTransportationCableCar;
            }
            else if (buildingAIType == typeof(TransportStationAI))
            {
                // check first for Hubs
                if (buildingInfo.category == CategoryHubs)
                {
                    return UsageType.WorkersTransportationHubs;
                }
                else
                {
                    // not a Hub, check reason and metro
                    GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                    if (reason1 == TransferManager.TransferReason.Bus                ) { return UsageType.WorkersTransportationBus;          }
                    if (reason1 == TransferManager.TransferReason.PassengerHelicopter) { return UsageType.WorkersTransportationAirPeople;    }
                    if (reason1 == TransferManager.TransferReason.Blimp              ) { return UsageType.WorkersTransportationAirPeople;    }
                    if (reason1 == TransferManager.TransferReason.IntercityBus       ) { return UsageType.WorkersTransportationIntercityBus; }
                    if (reason1 == TransferManager.TransferReason.MetroTrain         ) { return UsageType.WorkersTransportationMetro;        }
                    if (reason1 == TransferManager.TransferReason.PassengerTrain     ) { return HasMetroSubBuilding(buildingInfo) ? UsageType.WorkersTransportationHubs : UsageType.WorkersTransportationTrainPeople; }
                    if (reason1 == TransferManager.TransferReason.PassengerPlane     ) { return UsageType.WorkersTransportationAirPeople;    }
                    if (reason1 == TransferManager.TransferReason.Monorail           ) { return UsageType.WorkersTransportationMonorail;     }
                }
            }
            else if (buildingAIType == typeof(AirportGateAI))
            {
                GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                if (reason1 == TransferManager.TransferReason.Bus           ) { return UsageType.WorkersTransportationBus;         }
                if (reason1 == TransferManager.TransferReason.PassengerPlane) { return UsageType.WorkersTransportationAirPeople;   }
                if (reason1 == TransferManager.TransferReason.MetroTrain    ) { return UsageType.WorkersTransportationMetro;       }
                if (reason1 == TransferManager.TransferReason.PassengerTrain) { return UsageType.WorkersTransportationTrainPeople; }
            }
            else if (buildingAIType == typeof(HarborAI))
            {
                return buildingInfo.category == CategoryHubs ? UsageType.WorkersTransportationHubs : UsageType.WorkersTransportationShipPeople;
            }
            else if (buildingAIType == typeof(SpaceElevatorAI))
            {
                return UsageType.WorkersTransportationSpaceElevator;
            }

            // usage type not found above, not an error
            return UsageType.None;
        }

        /// <summary>
        /// get the usage type for a vehicles transportation building
        /// </summary>
        protected UsageType GetVehiclesTransportationUsageType(Building building)
        {
            // exclude usage type for subbuilding (i.e. integrated metro station)
            if (building.m_parentBuilding != 0)
            {
                return UsageType.None;
            }

            // get usage type normally
            return GetVehiclesTransportationUsageType(building.Info);
        }

        /// <summary>
        /// get the usage type for a vehicles transportation building info
        /// </summary>
        protected UsageType GetVehiclesTransportationUsageType(BuildingInfo buildingInfo)
        {
            // use building AI type, vehicle reasons, and/or metro presence to determine the usage type
            Type buildingAIType = buildingInfo.m_buildingAI.GetType();
            if (buildingAIType == typeof(AirportAuxBuildingAI))
            {
                return (HasMetroSubBuilding(buildingInfo) ? UsageType.VehiclesTransportationMetro : UsageType.None);
            }
            else if (buildingAIType == typeof(AirportEntranceAI))
            {
                return (HasMetroSubBuilding(buildingInfo) ? UsageType.VehiclesTransportationMetro : UsageType.None);
            }
            else if (buildingAIType == typeof(CargoStationAI))
            {
                GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                if (reason1 == TransferManager.TransferReason.PassengerTrain) { return UsageType.VehiclesTransportationTrainCargo; }
                if (reason1 == TransferManager.TransferReason.PassengerPlane) { return UsageType.VehiclesTransportationAirCargo;   }
            }
            else if (buildingAIType == typeof(AirportCargoGateAI))
            {
                return UsageType.VehiclesTransportationAirCargo;
            }
            else if (buildingAIType == typeof(CargoHarborAI))
            {
                return UsageType.VehiclesTransportationShipCargo;
            }
            else if (buildingAIType == typeof(DepotAI))
            {
                GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                if (reason1 == TransferManager.TransferReason.Bus                ) { return UsageType.VehiclesTransportationBus;        }
                if (reason1 == TransferManager.TransferReason.Trolleybus         ) { return UsageType.VehiclesTransportationTrolleybus; }
                if (reason1 == TransferManager.TransferReason.Tram               ) { return UsageType.VehiclesTransportationTram;       }
                if (reason1 == TransferManager.TransferReason.Ferry              ) { return UsageType.VehiclesTransportationShipPeople; }
                if (reason1 == TransferManager.TransferReason.PassengerHelicopter) { return UsageType.VehiclesTransportationAirPeople;  }
                if (reason1 == TransferManager.TransferReason.Blimp              ) { return UsageType.VehiclesTransportationAirPeople;  }
                if (reason1 == TransferManager.TransferReason.TouristBus         ) { return UsageType.VehiclesTransportationTours;      }
                if (reason1 == TransferManager.TransferReason.Taxi               ) { return UsageType.VehiclesTransportationTaxi;       }
            }
            else if (buildingAIType == typeof(CableCarStationAI))
            {
                return UsageType.VehiclesTransportationCableCar;
            }
            else if (buildingAIType == typeof(TransportStationAI))
            {
                // check first for Hubs
                if (buildingInfo.category == CategoryHubs)
                {
                    return UsageType.VehiclesTransportationHubs;
                }
                else
                {
                    // not a Hub, check reason and metro
                    GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                    if (reason1 == TransferManager.TransferReason.Bus                ) { return UsageType.None;                               }
                    if (reason1 == TransferManager.TransferReason.PassengerHelicopter) { return UsageType.None;                               }
                    if (reason1 == TransferManager.TransferReason.Blimp              ) { return UsageType.None;                               }
                    if (reason1 == TransferManager.TransferReason.IntercityBus       ) { return UsageType.VehiclesTransportationIntercityBus; }
                    if (reason1 == TransferManager.TransferReason.MetroTrain         ) { return UsageType.VehiclesTransportationMetro;        }
                    if (reason1 == TransferManager.TransferReason.PassengerTrain     ) { return HasMetroSubBuilding(buildingInfo) ? UsageType.VehiclesTransportationHubs : UsageType.VehiclesTransportationTrainPeople; }
                    if (reason1 == TransferManager.TransferReason.PassengerShip      ) { return UsageType.VehiclesTransportationShipPeople;   }
                    if (reason1 == TransferManager.TransferReason.PassengerPlane     ) { return UsageType.VehiclesTransportationAirPeople;    }
                    if (reason1 == TransferManager.TransferReason.Monorail           ) { return UsageType.VehiclesTransportationMonorail;     }
                }
            }
            else if (buildingAIType == typeof(AirportGateAI))
            {
                GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                if (reason1 == TransferManager.TransferReason.Bus           ) { return UsageType.None;                              }
                if (reason1 == TransferManager.TransferReason.PassengerPlane) { return UsageType.VehiclesTransportationAirPeople;   }
                if (reason1 == TransferManager.TransferReason.MetroTrain    ) { return UsageType.VehiclesTransportationMetro;       }
                if (reason1 == TransferManager.TransferReason.PassengerTrain) { return UsageType.VehiclesTransportationTrainPeople; }
            }
            else if (buildingAIType == typeof(HarborAI))
            {
                GetVehicleReasons(buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason _);
                if (reason1 == TransferManager.TransferReason.Ferry) { return UsageType.None;                       }
                if (buildingInfo.category == CategoryHubs)  { return UsageType.VehiclesTransportationHubs; }
                return UsageType.VehiclesTransportationShipPeople;
            }
            else if (buildingAIType == typeof(TourBuildingAI) || buildingAIType == typeof(ChirperTourAI))
            {
                return UsageType.VehiclesTransportationTours;
            }

            // usage type not found above, not an error
            return UsageType.None;
        }

        /// <summary>
        /// get the vehicle reasons for a transportation building
        /// </summary>
        protected void GetVehicleReasons(BuildingInfo buildingInfo, out TransferManager.TransferReason reason1, out TransferManager.TransferReason reason2)
        {
            // set default values
            reason1 = TransferManager.TransferReason.None;
            reason2 = TransferManager.TransferReason.None;

            // get reasons based on building AI type
            if (buildingInfo.m_buildingAI.GetType() == typeof(CargoStationAI) || buildingInfo.m_buildingAI.GetType() == typeof(CargoHarborAI))
            {
                CargoStationAI buildingAI = buildingInfo.m_buildingAI as CargoStationAI;
                if (buildingAI.m_transportInfo != null)
                {
                    reason1 = buildingAI.m_transportInfo.m_vehicleReason;
                }
                if (buildingAI.m_transportInfo2 != null)
                {
                    reason2 = buildingAI.m_transportInfo2.m_vehicleReason;
                }
            }
            else
            {
                DepotAI buildingAI = buildingInfo.m_buildingAI as DepotAI;
                if (buildingAI.m_transportInfo != null)
                {
                    reason1 = buildingAI.m_transportInfo.m_vehicleReason;
                }
                if (buildingAI.m_secondaryTransportInfo != null)
                {
                    reason2 = buildingAI.m_secondaryTransportInfo.m_vehicleReason;
                }
            }
        }

        /// <summary>
        /// return whether or not the building has a subbuilding that is a metro station
        /// the metro station may be underground integrated, at ground level, or above ground
        /// </summary>
        private bool HasMetroSubBuilding(BuildingInfo buildingInfo)
        {
            // check if any sub building is a metro station
            if (buildingInfo != null && buildingInfo.m_subBuildings != null)
            {
                foreach (BuildingInfo.SubInfo subBuilding in buildingInfo.m_subBuildings)
                {
                    // metro station subbuildings always have TransportStationAI
                    if (subBuilding != null && subBuilding.m_buildingInfo != null && subBuilding.m_buildingInfo.m_buildingAI != null && subBuilding.m_buildingInfo.m_buildingAI.GetType() == typeof(TransportStationAI))
                    {
                        GetVehicleReasons(subBuilding.m_buildingInfo, out TransferManager.TransferReason subReason1, out TransferManager.TransferReason subReason2);
                        if (subReason1 == TransferManager.TransferReason.MetroTrain || subReason2 == TransferManager.TransferReason.MetroTrain)
                        {
                            return true;
                        }
                    }
                }
            }

            // no subuilding is metro station
            return false;
        }

        #endregion

        #region "Parks Plazas Usage Types"

        // NOTE:
        // The Parkify mod replaces ParkAI with ParkBuildingAI for most (all?) ParkAI building prefabs.
        // Buildings that would have gotten UsageType based on ParkAI will instead get UsageType based on ParkBuildingAI.

        /// <summary>
        /// return the usage type for a ParkAI building
        /// </summary>
        protected UsageType GetVisitorsParksPlazasUsageType(BuildingInfo buildingInfo, out bool contentCreatorPack)
        {
            // Building AI                      Building                                    Category                    Modder Pack     Usage Type
            // --------------------------       ------------------------------------------  --------------------------  --------------  --------------
            // ParkAI                      -V-- Parks:
            //                                      Small Park                              BeautificationParks                         Parks
            //                                      Small Playground                        BeautificationParks                         Parks
            //                                      Park With Trees                         BeautificationParks                         Parks
            //                                      Large Playground                        BeautificationParks                         Parks
            //                                      Bouncy Castle Park                      BeautificationParks                         Parks
            //                                      Botanical Garden                        BeautificationParks                         Parks
            //                                      Dog Park                                BeautificationParks                         Parks
            //                                      Carousel Park                           BeautificationParks                         Parks
            //                                      Japanese Garden                         BeautificationParks                         Parks
            //                                      Tropical Garden                         BeautificationParks                         Parks
            //                                      Fishing Island                          BeautificationParks                         Parks
            //                                      Floating Cafe                           BeautificationParks                         Parks
            //                                  Plazas:
            //                                      Plaza with Trees                        BeautificationPlazas                        Plazas
            //                                      Plaza with Picnic Tables                BeautificationPlazas                        Plazas
            //                                      Paradox Plaza                           BeautificationPlazas                        Plazas
            //                                  Other Parks:
            //                                      Basketball Court                        BeautificationOthers                        OtherParks
            //                                      Tennis Court                            BeautificationOthers                        OtherParks
            //                                      Birthday Plaza                          BeautificationOthers                        OtherParks
            //                                      Small Decorative Parking Lot            BeautificationOthers                        OtherParks
            //                                      Large Decorative Parking Lot            BeautificationOthers                        OtherParks
            //                                      Multistory Decorative Parking Lot       BeautificationOthers                        OtherParks
            //                                  Tourism & Leisure:
            //                                      Fishing Pier                            BeautificationExpansion1                    TourismLeisure
            //                                      Fishing Tours                           BeautificationExpansion1                    TourismLeisure
            //                                      Jet Ski Rental                          BeautificationExpansion1                    TourismLeisure
            //                                      Marina                                  BeautificationExpansion1                    TourismLeisure
            //                                      Restaurant Pier                         BeautificationExpansion1                    TourismLeisure
            //                                      Beach Volleyball Court                  BeautificationExpansion1                    TourismLeisure
            //                                      Riding Stable                           BeautificationExpansion1                    TourismLeisure
            //                                      Skatepark                               BeautificationExpansion1                    TourismLeisure
            //                                      Snowmobile Track                        BeautificationExpansion1                    TourismLeisure
            //                                      Winter Fishing Pier                     BeautificationExpansion1                    TourismLeisure
            //                                      Ice Hockey Rink                         BeautificationExpansion1                    TourismLeisure
            //                                  Winter Parks:
            //                                      Snowman Park                            BeautificationExpansion2                    WinterParks
            //                                      Ice Sculpture Park                      BeautificationExpansion2                    WinterParks
            //                                      Sledding Hill                           BeautificationExpansion2                    WinterParks
            //                                      Curling Park                            BeautificationExpansion2                    WinterParks
            //                                      Skating Rink                            BeautificationExpansion2                    WinterParks
            //                                      Ski Lodge                               BeautificationExpansion2                    WinterParks
            //                                      Cross-Country Skiing Park               BeautificationExpansion2                    WinterParks
            //                                      Firepit Park                            BeautificationExpansion2                    WinterParks
            //                                  Content Creator:
            //                                      Biodome                                 MonumentModderPack          Pack2           CCPHighTech
            //                                      Vertical Farm                           MonumentModderPack          Pack2           CCPHighTech
            //                                      Seine Pier                              MonumentModderPack          Pack8           CCPBridgesPiers
            //                                      Rhine Pier                              MonumentModderPack          Pack8           CCPBridgesPiers
            //                                      Car Port 2 Slot                         MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Car Port 4 Slot                         MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Car Port 6 Slot                         MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Car Port 12 Slot                        MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Car Port 24 Slot                        MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Hotel Oasis A                           MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Hotel Oasis B                           MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Motel Palm Springs                      MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Roadside Diner                          MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Roadside Diner (subbuildings)           BeautificationModderPack    Pack11          CCPMidCenturyModern
            //                                      Mothership                              MonumentModderPack          Pack11          CCPMidCenturyModern
            //                                      Mothership (subbuildings)               BeautificationModderPack    Pack11          CCPMidCenturyModern
            //                                      Small Soccer Field                      MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Community Soccer Park                   MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Community Australian Football Field     MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Community Australian Football Park      MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Community Baseball Field                MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Community Baseball Complex              MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Suburban American Football Field        MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Community American Football Park        MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Suburban Cricket Pitch                  MonumentModderPack          Pack17          CCPSportsVenues
            //                                      Community Cricket Pitch                 MonumentModderPack          Pack17          CCPSportsVenues
            //                                      The Botanical Museum                    MonumentModderPack          Pack18          CCPAfricaInMiniature
            //                                      ROJ Paid Parking Small                  MonumentModderPack          Pack19          CCPRailroadsOfJapan
            //                                      ROJ Paid Parking Large                  MonumentModderPack          Pack19          CCPRailroadsOfJapan
            //                                      ROJ Small Station Front Plaza           MonumentModderPack          Pack19          CCPRailroadsOfJapan
            //                                      ROJ Large Station Front Plaza           MonumentModderPack          Pack19          CCPRailroadsOfJapan
            //                                      ROJ Small Station Market                MonumentModderPack          Pack19          CCPRailroadsOfJapan

            // initialize
            contentCreatorPack = false;

            // usage type depends on category
            switch (buildingInfo.category)
            {
                case "BeautificationParks":         return UsageType.VisitorsParksPlazasParks;
                case "BeautificationPlazas":        return UsageType.VisitorsParksPlazasPlazas;
                case "BeautificationOthers":        return UsageType.VisitorsParksPlazasOtherParks;
                case "BeautificationExpansion1":    return UsageType.VisitorsParksPlazasTourismLeisure;
                case "BeautificationExpansion2":    return UsageType.VisitorsParksPlazasWinterkParks;
                case "MonumentModderPack":
                case "BeautificationModderPack":
                        contentCreatorPack = true;
                        SteamHelper.ModderPackBitMask requiredModderPack = buildingInfo.m_requiredModderPack;
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack2 ) != 0) { return UsageType.VisitorsParksPlazasCCPHighTech;          }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack8 ) != 0) { return UsageType.VisitorsParksPlazasCCPBridgesPiers;      }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack11) != 0) { return UsageType.VisitorsParksPlazasCCPMidCenturyModern;  }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack17) != 0) { return UsageType.VisitorsParksPlazasCCPSportsVenues;      }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack18) != 0) { return UsageType.VisitorsParksPlazasCCPAfricaInMiniature; }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack19) != 0) { return UsageType.VisitorsParksPlazasCCPRailroadsOfJapan;  }
                        LogUtil.LogError($"Unhandled required modder pack [0x{requiredModderPack:X}] when determining usage type for building [{buildingInfo.name}].");
                        return UsageType.None;
                case "Default":                     return UsageType.None;      // the Train Stations CCP has one subbuilding with this category, so ignore this category
                default:
                    LogUtil.LogError($"Unhandled building category [{buildingInfo.category}] when determining usage type for building [{buildingInfo.name}].");
                    return UsageType.None;
            }
        }

        /// <summary>
        /// return the usage type for a ParkBuildingAI building
        /// </summary>
        protected UsageType GetVisitorsParksPlazasUsageType(BuildingInfo buildingInfo)
        {
            // usage type depends on park type
            DistrictPark.ParkType parkType = GetParkType(buildingInfo);
            switch (parkType)
            {
                case DistrictPark.ParkType.Generic:         return UsageType.VisitorsParksPlazasCityPark;
                case DistrictPark.ParkType.AmusementPark:   return UsageType.VisitorsParksPlazasAmusementPark;
                case DistrictPark.ParkType.Zoo:             return UsageType.VisitorsParksPlazasZoo;
                case DistrictPark.ParkType.NatureReserve:   return UsageType.VisitorsParksPlazasNatureReserve;
                case DistrictPark.ParkType.PedestrianZone:  return UsageType.VisitorsParksPlazasPedestrianPlazas;
                default:
                    LogUtil.LogError($"Unhandled park type [{parkType}] when determining usage type for building [{buildingInfo.name}].");
                    return UsageType.None;
            }
        }
        #endregion

        #region "Unique Building Usage Types"

        /// <summary>
        /// get the usage type for a workers unique building
        /// </summary>
        protected UsageType GetWorkersUniqueUsageType(BuildingInfo buildingInfo, out bool contentCreatorPack)
        {
            // initialize
            contentCreatorPack = false;

            // use building AI type and category to determine the usage type
            Type buildingAIType = buildingInfo.m_buildingAI.GetType();
            if (buildingAIType == typeof(MonumentAI))
            {
                // use category to determine usage type
                switch (buildingInfo.category)
                {
                    case "MonumentPudding":             return UsageType.WorkersUniqueFinancial;
                    case "MonumentLandmarks":           return UsageType.WorkersUniqueLandmark;
                    case "MonumentExpansion1":          return UsageType.WorkersUniqueTourismLeisure;
                    case "MonumentExpansion2":          return UsageType.WorkersUniqueWinterUnique;
                    case "MonumentCupcake":             return UsageType.WorkersUniquePedestrianArea;
                    case "MonumentFootball":            return UsageType.WorkersUniqueFootball;
                    case "MonumentConcerts":            return UsageType.WorkersUniqueConcert;
                    case "PublicTransportAirportArea":  return UsageType.WorkersUniqueAirports;
                    case "MonumentCategory1":           return UsageType.WorkersUniqueLevel1;
                    case "MonumentCategory2":           return UsageType.WorkersUniqueLevel2;
                    case "MonumentCategory3":           return UsageType.WorkersUniqueLevel3;
                    case "MonumentCategory4":           return UsageType.WorkersUniqueLevel4;
                    case "MonumentCategory5":           return UsageType.WorkersUniqueLevel5;
                    case "MonumentCategory6":           return UsageType.WorkersUniqueLevel6;
                    case "MonumentModderPack":
                        contentCreatorPack = true;
                        SteamHelper.ModderPackBitMask requiredModderPack = buildingInfo.m_requiredModderPack;
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack1 ) != 0) { return UsageType.WorkersUniqueCCPArtDeco;           }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack2 ) != 0) { return UsageType.WorkersUniqueCCPHighTech;          }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack6 ) != 0) { return UsageType.WorkersUniqueCCPModernJapan;       }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack12) != 0) { return UsageType.WorkersUniqueCCPSeasideResorts;    }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack13) != 0) { return UsageType.WorkersUniqueCCPSkyscrapers;       }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack14) != 0) { return UsageType.WorkersUniqueCCPHeartOfKorea;      }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack16) != 0) { return UsageType.WorkersUniqueCCPShoppingMalls;     }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack17) != 0) { return UsageType.WorkersUniqueCCPSportsVenues;      }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack18) != 0) { return UsageType.WorkersUniqueCCPAfricaInMiniature; }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack19) != 0) { return UsageType.WorkersUniqueCCPRailroadsOfJapan;  }
                        LogUtil.LogError($"Unhandled required modder pack [0x{requiredModderPack:X}] when determining usage type for building [{buildingInfo.name}].");
                        return UsageType.None;
                    default:
                        LogUtil.LogError($"Unhandled building category [{buildingInfo.category}] when determining usage type for building [{buildingInfo.name}].");
                        return UsageType.None;
                }
            }
            else if (buildingAIType == typeof(FestivalAreaAI))
            {
                return UsageType.WorkersUniqueConcert;
            }
            else if (buildingAIType == typeof(AirlineHeadquartersAI))
            {
                return UsageType.WorkersUniqueAirports;
            }
            else if (buildingAIType == typeof(AnimalMonumentAI))
            {
                return UsageType.WorkersUniqueWinterUnique;
            }
            else if (buildingAIType == typeof(PrivateAirportAI))
            {
                return UsageType.WorkersUniqueLevel5;
            }
            else if (buildingAIType == typeof(ChirpwickCastleAI))
            {
                return UsageType.WorkersUniqueChirpwickCastle;
            }
            else if (buildingAIType == typeof(StockExchangeAI) || buildingAIType == typeof(InternationalTradeBuildingAI))
            {
                return UsageType.WorkersUniqueFinancial;
            }
            else if (buildingAIType == typeof(HadronColliderAI))
            {
                // there are two monuments in Africa in Miniature CCP that have HadronColliderAI
                if (buildingInfo.m_class.m_service == ItemClass.Service.Education)
                {
                    // not an error, just don't include the education building
                    return UsageType.None;
                }
                else
                {
                    return UsageType.WorkersUniqueCCPAfricaInMiniature;
                }
            }

            // usage type not found, not an error
            return UsageType.None;
        }

        /// <summary>
        /// get the usage type for a visitors unique building
        /// </summary>
        protected UsageType GetVisitorsUniqueUsageType(BuildingInfo buildingInfo, out bool contentCreatorPack)
        {
            // initialize
            contentCreatorPack = false;

            // use building AI type and category to determine the usage type
            Type buildingAIType = buildingInfo.m_buildingAI.GetType();
            if (buildingAIType == typeof(MonumentAI))
            {
                // use category to determine usage type
                switch (buildingInfo.category)
                {
                    case "MonumentPudding":             return UsageType.VisitorsUniqueFinancial;
                    case "MonumentLandmarks":           return UsageType.VisitorsUniqueLandmark;
                    case "MonumentExpansion1":          return UsageType.VisitorsUniqueTourismLeisure;
                    case "MonumentExpansion2":          return UsageType.VisitorsUniqueWinterUnique;
                    case "MonumentCupcake":             return UsageType.VisitorsUniquePedestrianArea;
                    case "MonumentFootball":            return UsageType.VisitorsUniqueFootball;
                    case "MonumentConcerts":            return UsageType.VisitorsUniqueConcert;
                    case "PublicTransportAirportArea":  return UsageType.VisitorsUniqueAirports;
                    case "MonumentCategory1":           return UsageType.VisitorsUniqueLevel1;
                    case "MonumentCategory2":           return UsageType.VisitorsUniqueLevel2;
                    case "MonumentCategory3":           return UsageType.VisitorsUniqueLevel3;
                    case "MonumentCategory4":           return UsageType.VisitorsUniqueLevel4;
                    case "MonumentCategory5":           return UsageType.VisitorsUniqueLevel5;
                    case "MonumentCategory6":           return UsageType.VisitorsUniqueLevel6;
                    case "MonumentModderPack":
                        contentCreatorPack = true;
                        SteamHelper.ModderPackBitMask requiredModderPack = buildingInfo.m_requiredModderPack;
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack1 ) != 0) { return UsageType.VisitorsUniqueCCPArtDeco;           }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack2 ) != 0) { return UsageType.VisitorsUniqueCCPHighTech;          }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack6 ) != 0) { return UsageType.VisitorsUniqueCCPModernJapan;       }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack12) != 0) { return UsageType.VisitorsUniqueCCPSeasideResorts;    }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack13) != 0) { return UsageType.VisitorsUniqueCCPSkyscrapers;       }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack14) != 0) { return UsageType.VisitorsUniqueCCPHeartOfKorea;      }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack16) != 0) { return UsageType.VisitorsUniqueCCPShoppingMalls;     }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack17) != 0) { return UsageType.VisitorsUniqueCCPSportsVenues;      }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack18) != 0) { return UsageType.VisitorsUniqueCCPAfricaInMiniature; }
                        if ((requiredModderPack & SteamHelper.ModderPackBitMask.Pack19) != 0) { return UsageType.VisitorsUniqueCCPRailroadsOfJapan;  }
                        LogUtil.LogError($"Unhandled required modder pack [0x{requiredModderPack:X}] when determining usage type for building [{buildingInfo.name}].");
                        return UsageType.None;
                    default:
                        LogUtil.LogError($"Unhandled building category [{buildingInfo.category}] when determining usage type for building [{buildingInfo.name}].");
                        return UsageType.None;
                }
            }
            else if (buildingAIType == typeof(FestivalAreaAI))
            {
                return UsageType.VisitorsUniqueConcert;
            }
            else if (buildingAIType == typeof(AirlineHeadquartersAI))
            {
                return UsageType.VisitorsUniqueAirports;
            }
            else if (buildingAIType == typeof(AnimalMonumentAI))
            {
                return UsageType.VisitorsUniqueWinterUnique;
            }
            else if (buildingAIType == typeof(PrivateAirportAI))
            {
                return UsageType.VisitorsUniqueLevel5;
            }
            else if (buildingAIType == typeof(ChirpwickCastleAI))
            {
                return UsageType.VisitorsUniqueChirpwickCastle;
            }
            else if (buildingAIType == typeof(StockExchangeAI) || buildingAIType == typeof(InternationalTradeBuildingAI))
            {
                return UsageType.VisitorsUniqueFinancial;
            }

            // usage type not found, not an error
            return UsageType.None;
        }

        #endregion

        #region "Update Panel"

        /// <summary>
        /// update the panel immediately
        /// </summary>
        private void UpdatePanelImmediately()
        {
            // update colors on all buildings to force immediate recompute of employment data
            BuildingManager.instance.UpdateBuildingColors();

            // set counter to update panel in 2 frames (i.e. immediately)
            _updateImmediateCounter = 2;
        }

        /// <summary>
        /// update the panel
        /// </summary>
        public virtual bool UpdatePanel()
        {
            // if a detail panel is currently displayed, update the detail panel instead
            if (_detailPanel != null)
            {
                // this is a recursive call, but to a different panel
                return _detailPanel.UpdatePanel();
            }

            try
            {
                // check conditions
                if (!_initialized)
                {
                    return false;
                }
                if (!InfoManager.exists)
                {
                    return false;
                }

                // info mode can change, make sure info mode is still BuildingLevel
                if (InfoManager.instance.CurrentMode != InfoManager.InfoMode.BuildingLevel)
                {
                    return false;
                }

                // update every 1 second or if specified for update immediately
                long currentTicks = DateTime.Now.Ticks;
                if (currentTicks - _previousTicks >= 1 * TimeSpan.TicksPerSecond || --_updateImmediateCounter == 0)
                {
                    // do each usage group
                    foreach (UsageGroup usageGroup in _usageGroups.Values)
                    {
                        // compute overall usage as a ratio from 0 to 1
                        float usageRatio = ComputeUsageRatio(usageGroup.usedRunningTotal, usageGroup.allowedRunningTotal);

                        // update usage percent
                        usageGroup.percent.text = (100f * usageRatio).ToString("0") + "%";

                        // disable or enable the group
                        string toolTipText;
                        if (usageGroup.allowedRunningTotal == 0)
                        {
                            // disable the group
                            usageGroup.thumbnail.spriteName = usageGroup.spriteNameDisabled;
                            usageGroup.description.textColor = _textColorDisabled;
                            usageGroup.detailButton.spriteName = "CityInfoDisabled";
                            usageGroup.detailButton.tooltip = "";
                            SetColorGradient(usageGroup.legend, _gradientColorDisabled0, _gradientColorDisabled1);
                            usageGroup.indicator.isVisible = false;
                            usageGroup.percent.textColor = _textColorDisabled;

                            // set tool tip text
                            toolTipText = "No buildings";
                        }
                        else
                        {
                            // enable the group
                            usageGroup.thumbnail.spriteName = usageGroup.spriteNameNormal;
                            usageGroup.description.textColor = _textColorNormal;
                            usageGroup.detailButton.spriteName = "CityInfo";
                            usageGroup.detailButton.tooltip = "Show Detail";
                            SetColorGradient(usageGroup.legend, usageGroup.color0, usageGroup.color1);
                            usageGroup.indicator.isVisible = true;
                            usageGroup.percent.textColor = _textColorNormal;

                            // update indicator position to show the usage ratio
                            usageGroup.indicator.relativePosition = ComputeIndicatorRelativePosition(usageGroup.indicator, usageGroup.legend, usageRatio);

                            // set tool tip text
                            toolTipText = $"{usageGroup.usedRunningTotal.ToString("N0", LocaleManager.cultureInfo)} / {usageGroup.allowedRunningTotal.ToString("N0", LocaleManager.cultureInfo)}";
                        }

                        // update tool tip
                        usageGroup.legend.tooltip = toolTipText;
                        usageGroup.percent.tooltip = toolTipText;
                    }

                    // save ticks for next time
                    _previousTicks = currentTicks;

                    // panel was updated
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }

            // panel was not updated
            return false;
        }

        /// <summary>
        ///  set the color gradient of the legend
        /// </summary>
        private void SetColorGradient(UITextureSprite legend, Color colorA, Color colorB)
        {
            legend.renderMaterial.SetColor("_ColorA", colorA);
            legend.renderMaterial.SetColor("_ColorB", colorB);
            legend.renderMaterial.SetFloat("_Step", 0.01f);
            legend.renderMaterial.SetFloat("_Scalar", 1f);
            legend.renderMaterial.SetFloat("_Offset", 0f);
        }

        /// <summary>
        /// return the relative position of where the usage indicator should be displayed
        /// </summary>
        private Vector3 ComputeIndicatorRelativePosition(UISprite indicator, UITextureSprite legend, float usage01)
        {
            // return a new relative position
            return new Vector3(legend.size.x * usage01 - indicator.size.x / 2, -1);
        }

        /// <summary>
        /// return the color associated with the usage type and usage count
        /// </summary>
        private Color GetUsageColor(UsageType usageType, bool include, int used, int allowed)
        {
            // return color only for included buildings
            if (include)
            {
                // return color only if building has allowed
                if (allowed != 0)
                {
                    // get the usage group
                    if (_usageGroups.TryGetValue(usageType, out UsageGroup usageGroup))
                    {
                        // check box must be checked
                        if (IsCheckBoxChecked(usageGroup.checkBox))
                        {
                            return Color.Lerp(usageGroup.color0, usageGroup.color1, ComputeUsageRatio(used, allowed));
                        }
                    }
                }
            }

            // if not handled above, use neutral color
            return InfoManager.instance.m_properties.m_neutralColor;
        }

        /// <summary>
        /// return the usage ratio from 0 to 1
        /// </summary>
        private float ComputeUsageRatio(int used, int allowed)
        {
            if (allowed == 0)
            {
                return 0f;
            }
            else if (used >= allowed)
            {
                return 1f;
            }
            else
            {
                return Mathf.Clamp01((float)used / (float)allowed);
            }
        }
        #endregion

    }
}
