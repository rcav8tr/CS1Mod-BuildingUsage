using UnityEngine;
using System;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display worker usage
    /// </summary>
    public class WorkersUsagePanel : UsagePanel
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

                // create the usage groups
                CreateUsageGroup<ResidentialBuildingAI                                  >(UsageType.HouseholdsResidential);
                CreateUsageGroup<CommercialBuildingAI                                   >(UsageType.WorkersCommercial);
                CreateUsageGroup<OfficeBuildingAI                                       >(UsageType.WorkersOffice);
                CreateUsageGroup<IndustrialBuildingAI, IndustrialExtractorAI            >(UsageType.WorkersIndustrial);
                CreateUsageGroup<MaintenanceDepotAI, SnowDumpAI                         >(UsageType.WorkersMaintenance);
                CreateUsageGroup<PowerPlantAI, SolarPowerPlantAI, FusionPowerPlantAI    >(UsageType.WorkersPowerPlant);
                CreateUsageGroup<HeatingPlantAI                                         >(UsageType.WorkersHeatingPlant);
                CreateUsageGroup<LandfillSiteAI, UltimateRecyclingPlantAI               >(UsageType.WorkersGarbage);
                CreateUsageGroup<MainIndustryBuildingAI, AuxiliaryBuildingAI, ExtractingFacilityAI, FishingHarborAI, FishFarmAI, MarketAI, ProcessingFacilityAI, UniqueFactoryAI, WarehouseAI>(UsageType.WorkersIndustry);
                CreateUsageGroup<HospitalAI, ChildcareAI, EldercareAI, MedicalCenterAI, SaunaAI, HelicopterDepotAI                                              >(UsageType.WorkersMedical);
                CreateUsageGroup<CemeteryAI                                             >(UsageType.WorkersCemetery);
                CreateUsageGroup<FireStationAI, HelicopterDepotAI                       >(UsageType.WorkersFireStation);
                CreateUsageGroup<DisasterResponseBuildingAI, ShelterAI, DoomsdayVaultAI, WeatherRadarAI, SpaceRadarAI                                           >(UsageType.WorkersDisaster);
                CreateUsageGroup<PoliceStationAI, HelicopterDepotAI                     >(UsageType.WorkersPoliceStation);
                CreateUsageGroup<SchoolAI, LibraryAI, HadronColliderAI, MainCampusBuildingAI, CampusBuildingAI, UniqueFacultyAI, MuseumAI, VarsitySportsArenaAI >(UsageType.WorkersEducation);
                CreateUsageGroup<CargoStationAI, CargoHarborAI, DepotAI, CableCarStationAI, TransportStationAI, HarborAI, SpaceElevatorAI                       >(UsageType.WorkersTransportation);
                CreateUsageGroup<PostOfficeAI                                           >(UsageType.WorkersPost);
                CreateUsageGroup<ParkGateAI, ParkBuildingAI                             >(UsageType.WorkersAmusementPark);
                CreateUsageGroup<ParkGateAI, ParkBuildingAI                             >(UsageType.WorkersZoo);
                CreateUsageGroup<MonumentAI, AnimalMonumentAI, PrivateAirportAI, ChirpwickCastleAI>(UsageType.WorkersUnique);

                // add detail panels
                AddDetailPanel<WorkersIndustryUsagePanel      >(UsageType.WorkersIndustry,       this);
                AddDetailPanel<WorkersEducationUsagePanel     >(UsageType.WorkersEducation,      this);
                AddDetailPanel<WorkersTransportationUsagePanel>(UsageType.WorkersTransportation, this);
                AddDetailPanel<WorkersUniqueUsagePanel        >(UsageType.WorkersUnique,         this);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<ResidentialBuildingAI       >(UsageType.HouseholdsResidential, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountHouseholds                                (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CommercialBuildingAI        >(UsageType.WorkersCommercial,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersZoned                              (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<OfficeBuildingAI            >(UsageType.WorkersOffice,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersZoned                              (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<IndustrialBuildingAI        >(UsageType.WorkersIndustrial,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersZoned                              (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<IndustrialExtractorAI       >(UsageType.WorkersIndustrial,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersZoned                              (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MaintenanceDepotAI          >(UsageType.WorkersMaintenance,    (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MaintenanceDepotAI        >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SnowDumpAI                  >(UsageType.WorkersMaintenance,    (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<SnowDumpAI                >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<PowerPlantAI                >(UsageType.WorkersPowerPlant,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<PowerPlantAI              >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SolarPowerPlantAI           >(UsageType.WorkersPowerPlant,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<SolarPowerPlantAI         >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<FusionPowerPlantAI          >(UsageType.WorkersPowerPlant,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<FusionPowerPlantAI        >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<HeatingPlantAI              >(UsageType.WorkersHeatingPlant,   (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<HeatingPlantAI            >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<LandfillSiteAI              >(UsageType.WorkersGarbage,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<LandfillSiteAI            >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<UltimateRecyclingPlantAI    >(UsageType.WorkersGarbage,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<UltimateRecyclingPlantAI  >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MainIndustryBuildingAI      >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MainIndustryBuildingAI    >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<AuxiliaryBuildingAI         >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<AuxiliaryBuildingAI       >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ExtractingFacilityAI        >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<ExtractingFacilityAI      >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<FishingHarborAI             >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<FishingHarborAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<FishFarmAI                  >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<FishFarmAI                >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MarketAI                    >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MarketAI                  >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ProcessingFacilityAI        >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<ProcessingFacilityAI      >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<UniqueFactoryAI             >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<UniqueFactoryAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<WarehouseAI                 >(UsageType.WorkersIndustry,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<WarehouseAI               >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<HospitalAI                  >(UsageType.WorkersMedical,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<HospitalAI                >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ChildcareAI                 >(UsageType.WorkersMedical,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<ChildcareAI               >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<EldercareAI                 >(UsageType.WorkersMedical,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<EldercareAI               >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MedicalCenterAI             >(UsageType.WorkersMedical,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MedicalCenterAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SaunaAI                     >(UsageType.WorkersMedical,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<SaunaAI                   >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<HelicopterDepotAI           >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<HelicopterDepotAI         >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CemeteryAI                  >(UsageType.WorkersCemetery,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CemeteryAI                >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<FireStationAI               >(UsageType.WorkersFireStation,    (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<FireStationAI             >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<DisasterResponseBuildingAI  >(UsageType.WorkersDisaster,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<DisasterResponseBuildingAI>(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ShelterAI                   >(UsageType.WorkersDisaster,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<ShelterAI                 >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<DoomsdayVaultAI             >(UsageType.WorkersDisaster,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<DoomsdayVaultAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<WeatherRadarAI              >(UsageType.WorkersDisaster,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<WeatherRadarAI            >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SpaceRadarAI                >(UsageType.WorkersDisaster,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<SpaceRadarAI              >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<PoliceStationAI             >(UsageType.WorkersPoliceStation,  (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<PoliceStationAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SchoolAI                    >(UsageType.WorkersEducation,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<SchoolAI                  >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<LibraryAI                   >(UsageType.WorkersEducation,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<LibraryAI                 >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<HadronColliderAI            >(UsageType.WorkersEducation,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<HadronColliderAI          >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MainCampusBuildingAI        >(UsageType.WorkersEducation,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MainCampusBuildingAI      >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CampusBuildingAI            >(UsageType.WorkersEducation,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CampusBuildingAI          >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<UniqueFacultyAI             >(UsageType.WorkersEducation,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<UniqueFacultyAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MuseumAI                    >(UsageType.WorkersEducation,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MuseumAI                  >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<VarsitySportsArenaAI        >(UsageType.WorkersEducation,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<VarsitySportsArenaAI      >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CargoStationAI              >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CargoStationAI            >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CargoHarborAI               >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CargoHarborAI             >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<DepotAI                     >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<DepotAI                   >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CableCarStationAI           >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CableCarStationAI         >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<TransportStationAI          >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<TransportStationAI        >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<HarborAI                    >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<HarborAI                  >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SpaceElevatorAI             >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<SpaceElevatorAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<PostOfficeAI                >(UsageType.WorkersPost,           (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<PostOfficeAI              >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ParkGateAI                  >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersPark                               (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ParkBuildingAI              >(UsageType.UseLogic1,             (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersPark                               (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MonumentAI                  >(UsageType.WorkersUnique,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MonumentAI                >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<AnimalMonumentAI            >(UsageType.WorkersUnique,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<AnimalMonumentAI          >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<PrivateAirportAI            >(UsageType.WorkersUnique,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<PrivateAirportAI          >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ChirpwickCastleAI           >(UsageType.WorkersUnique,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<ChirpwickCastleAI         >(buildingID, ref data, ref used, ref allowed));
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
            if (buildingAIType == typeof(HelicopterDepotAI))
            {
                // convert building service to usage type
                if (data.Info.m_class.m_service == ItemClass.Service.HealthCare)
                {
                    return UsageType.WorkersMedical;
                }
                if (data.Info.m_class.m_service == ItemClass.Service.FireDepartment)
                {
                    return UsageType.WorkersFireStation;
                }
                if (data.Info.m_class.m_service == ItemClass.Service.PoliceDepartment)
                {
                    return UsageType.WorkersPoliceStation;
                }
            }
            else if (buildingAIType == typeof(ParkGateAI) || buildingAIType == typeof(ParkBuildingAI))
            {
                // only Amusement Park and Zoo have workers
                // logic adapted from ParkBuildingAI.TargetWorkers
                if ((data.Info.m_doorMask & PropInfo.DoorType.HangAround) == PropInfo.DoorType.HangAround)
                {
                    DistrictPark.ParkType parkType = GetParkType(ref data);
                    switch (parkType)
                    {
                        case DistrictPark.ParkType.AmusementPark: return UsageType.WorkersAmusementPark;
                        case DistrictPark.ParkType.Zoo:           return UsageType.WorkersZoo;
                        default:                                  return UsageType.None;
                    }
                }
                else
                {
                    return UsageType.None;
                }
            }
            else if (buildingAIType == typeof(CargoStationAI    ) ||
                     buildingAIType == typeof(CargoHarborAI     ) ||
                     buildingAIType == typeof(DepotAI           ) ||
                     buildingAIType == typeof(CableCarStationAI ) ||
                     buildingAIType == typeof(TransportStationAI) ||
                     buildingAIType == typeof(HarborAI          ) ||
                     buildingAIType == typeof(SpaceElevatorAI   ))
            {
                if (GetWorkersTransportationUsageType(data.Info) == UsageType.None)
                {
                    return UsageType.None;
                }
                else
                {
                    return UsageType.WorkersTransportation;
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
