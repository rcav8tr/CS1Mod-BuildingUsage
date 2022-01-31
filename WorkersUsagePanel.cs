using UnityEngine;
using System;
using ColossalFramework.UI;
using ColossalFramework.Globalization;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display worker usage
    /// </summary>
    public class WorkersUsagePanel : UsagePanel
    {
        // UI elements unique to Workers
        private class WorkerDataUI
        {
            public UILabel description;
            public UILabel eduLevel0;
            public UILabel eduLevel1;
            public UILabel eduLevel2;
            public UILabel eduLevel3;
            public UILabel total;
        }
        private WorkerDataUI _employed;
        private WorkerDataUI _totalJobs;
        private WorkerDataUI _unfilledJobs;
        private WorkerDataUI _overEducated;
        private WorkerDataUI _unemployed;
        private WorkerDataUI _eligible;
        private WorkerDataUI _unemploymentRate;

        private readonly Color32 EducationTextColor = new Color32(206, 248, 0, 255);

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
                CreateUsageGroup<ResidentialBuildingAI                                                                      >(UsageType.HouseholdsResidential);     // includes PloppableRICO
                CreateUsageGroup<CommercialBuildingAI                                                                       >(UsageType.WorkersCommercial);         // includes PloppableRICO
                CreateUsageGroup<OfficeBuildingAI                                                                           >(UsageType.WorkersOffice);             // includes PloppableRICO
                CreateUsageGroup<IndustrialBuildingAI, IndustrialExtractorAI, LivestockExtractorAI                          >(UsageType.WorkersIndustrial);         // includes PloppableRICO
                CreateUsageGroup<MaintenanceDepotAI, SnowDumpAI                                                             >(UsageType.WorkersMaintenance);
                CreateUsageGroup<WindTurbineAI, PowerPlantAI, DamPowerHouseAI, SolarPowerPlantAI, FusionPowerPlantAI        >(UsageType.WorkersPowerPlant);
                CreateUsageGroup<WaterFacilityAI                                                                            >(UsageType.WorkersWaterSewage);
                CreateUsageGroup<HeatingPlantAI                                                                             >(UsageType.WorkersHeatingPlant);
                CreateUsageGroup<LandfillSiteAI, WaterCleanerAI, UltimateRecyclingPlantAI                                   >(UsageType.WorkersGarbage);
                CreateUsageGroup<MainIndustryBuildingAI, AuxiliaryBuildingAI, ExtractingFacilityAI, FishingHarborAI,
                                 FishFarmAI, MarketAI, ProcessingFacilityAI, UniqueFactoryAI, WarehouseAI                   >(UsageType.WorkersIndustry);
                CreateUsageGroup<HospitalAI, ChildcareAI, EldercareAI, MedicalCenterAI, SaunaAI, HelicopterDepotAI          >(UsageType.WorkersMedical);
                CreateUsageGroup<CemeteryAI                                                                                 >(UsageType.WorkersCemetery);
                CreateUsageGroup<FireStationAI, HelicopterDepotAI, FireStationAI                                            >(UsageType.WorkersFireStation);
                CreateUsageGroup<DisasterResponseBuildingAI, ShelterAI, RadioMastAI, EarthquakeSensorAI,
                                 DoomsdayVaultAI, WeatherRadarAI, SpaceRadarAI                                              >(UsageType.WorkersDisaster);
                CreateUsageGroup<PoliceStationAI, HelicopterDepotAI                                                         >(UsageType.WorkersPoliceStation);
                CreateUsageGroup<SchoolAI, LibraryAI, HadronColliderAI, MainCampusBuildingAI, CampusBuildingAI,
                                 UniqueFacultyAI, MuseumAI, VarsitySportsArenaAI                                            >(UsageType.WorkersEducation);
                CreateUsageGroup<AirportAuxBuildingAI, AirportEntranceAI, AirportGateAI, AirportCargoGateAI,
                                 CargoStationAI, CargoHarborAI, DepotAI, CableCarStationAI, TransportStationAI,
                                 HarborAI, SpaceElevatorAI                                                                  >(UsageType.WorkersTransportation);
                CreateUsageGroup<PostOfficeAI                                                                               >(UsageType.WorkersPost);
                CreateUsageGroup<ParkGateAI, ParkBuildingAI                                                                 >(UsageType.WorkersAmusementPark);
                CreateUsageGroup<ParkGateAI, ParkBuildingAI                                                                 >(UsageType.WorkersZoo);
                CreateUsageGroup<MonumentAI, AirlineHeadquartersAI, AnimalMonumentAI, PrivateAirportAI, ChirpwickCastleAI   >(UsageType.WorkersUnique);

                // add detail panels
                AddDetailPanel<WorkersIndustryUsagePanel      >(UsageType.WorkersIndustry      );
                AddDetailPanel<WorkersEducationUsagePanel     >(UsageType.WorkersEducation     );
                AddDetailPanel<WorkersTransportationUsagePanel>(UsageType.WorkersTransportation);
                AddDetailPanel<WorkersUniqueUsagePanel        >(UsageType.WorkersUnique        );

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<ResidentialBuildingAI                 >(UsageType.HouseholdsResidential, GetUsageCountHouseholds                                );
                AssociateBuildingAI("PloppableRICO.GrowableResidentialAI",  UsageType.HouseholdsResidential, GetUsageCountHouseholds                                );
                AssociateBuildingAI("PloppableRICO.PloppableResidentialAI", UsageType.HouseholdsResidential, GetUsageCountHouseholds                                );
                AssociateBuildingAI<CommercialBuildingAI                  >(UsageType.WorkersCommercial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI("PloppableRICO.GrowableCommercialAI",   UsageType.WorkersCommercial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI("PloppableRICO.PloppableCommercialAI",  UsageType.WorkersCommercial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI<OfficeBuildingAI                      >(UsageType.WorkersOffice,         GetUsageCountWorkersZoned                              );
                AssociateBuildingAI("PloppableRICO.GrowableOfficeAI",       UsageType.WorkersOffice,         GetUsageCountWorkersZoned                              );
                AssociateBuildingAI("PloppableRICO.PloppableOfficeAI",      UsageType.WorkersOffice,         GetUsageCountWorkersZoned                              );
                AssociateBuildingAI<IndustrialBuildingAI                  >(UsageType.WorkersIndustrial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI("PloppableRICO.GrowableIndustrialAI",   UsageType.WorkersIndustrial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI("PloppableRICO.PloppableIndustrialAI",  UsageType.WorkersIndustrial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI<IndustrialExtractorAI                 >(UsageType.WorkersIndustrial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI<LivestockExtractorAI                  >(UsageType.WorkersIndustrial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI("PloppableRICO.GrowableExtractorAI",    UsageType.WorkersIndustrial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI("PloppableRICO.PloppableExtractorAI",   UsageType.WorkersIndustrial,     GetUsageCountWorkersZoned                              );
                AssociateBuildingAI<MaintenanceDepotAI                    >(UsageType.WorkersMaintenance,    GetUsageCountWorkersService<MaintenanceDepotAI        >);
                AssociateBuildingAI<SnowDumpAI                            >(UsageType.WorkersMaintenance,    GetUsageCountWorkersService<SnowDumpAI                >);
                AssociateBuildingAI<WindTurbineAI                         >(UsageType.WorkersPowerPlant,     GetUsageCountWorkersService<WindTurbineAI             >);
                AssociateBuildingAI<PowerPlantAI                          >(UsageType.WorkersPowerPlant,     GetUsageCountWorkersService<PowerPlantAI              >);
                AssociateBuildingAI<DamPowerHouseAI                       >(UsageType.WorkersPowerPlant,     GetUsageCountWorkersService<DamPowerHouseAI           >);
                AssociateBuildingAI<SolarPowerPlantAI                     >(UsageType.WorkersPowerPlant,     GetUsageCountWorkersService<SolarPowerPlantAI         >);
                AssociateBuildingAI<FusionPowerPlantAI                    >(UsageType.WorkersPowerPlant,     GetUsageCountWorkersService<FusionPowerPlantAI        >);
                AssociateBuildingAI<WaterFacilityAI                       >(UsageType.WorkersWaterSewage,    GetUsageCountWorkersService<WaterFacilityAI           >);
                AssociateBuildingAI<HeatingPlantAI                        >(UsageType.WorkersHeatingPlant,   GetUsageCountWorkersService<HeatingPlantAI            >);
                AssociateBuildingAI<LandfillSiteAI                        >(UsageType.WorkersGarbage,        GetUsageCountWorkersService<LandfillSiteAI            >);
                AssociateBuildingAI<WaterCleanerAI                        >(UsageType.WorkersGarbage,        GetUsageCountWorkersService<WaterCleanerAI            >);
                AssociateBuildingAI<UltimateRecyclingPlantAI              >(UsageType.WorkersGarbage,        GetUsageCountWorkersService<UltimateRecyclingPlantAI  >);
                AssociateBuildingAI<MainIndustryBuildingAI                >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<MainIndustryBuildingAI    >);
                AssociateBuildingAI<AuxiliaryBuildingAI                   >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<AuxiliaryBuildingAI       >);
                AssociateBuildingAI<ExtractingFacilityAI                  >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<ExtractingFacilityAI      >);
                AssociateBuildingAI<FishingHarborAI                       >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<FishingHarborAI           >);
                AssociateBuildingAI<FishFarmAI                            >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<FishFarmAI                >);
                AssociateBuildingAI<MarketAI                              >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<MarketAI                  >);
                AssociateBuildingAI<ProcessingFacilityAI                  >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<ProcessingFacilityAI      >);
                AssociateBuildingAI<UniqueFactoryAI                       >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<UniqueFactoryAI           >);
                AssociateBuildingAI<WarehouseAI                           >(UsageType.WorkersIndustry,       GetUsageCountWorkersService<WarehouseAI               >);
                AssociateBuildingAI<HospitalAI                            >(UsageType.WorkersMedical,        GetUsageCountWorkersService<HospitalAI                >);
                AssociateBuildingAI<ChildcareAI                           >(UsageType.WorkersMedical,        GetUsageCountWorkersService<ChildcareAI               >);
                AssociateBuildingAI<EldercareAI                           >(UsageType.WorkersMedical,        GetUsageCountWorkersService<EldercareAI               >);
                AssociateBuildingAI<MedicalCenterAI                       >(UsageType.WorkersMedical,        GetUsageCountWorkersService<MedicalCenterAI           >);
                AssociateBuildingAI<SaunaAI                               >(UsageType.WorkersMedical,        GetUsageCountWorkersService<SaunaAI                   >);
                AssociateBuildingAI<HelicopterDepotAI                     >(UsageType.UseLogic1,             GetUsageCountWorkersService<HelicopterDepotAI         >);
                AssociateBuildingAI<CemeteryAI                            >(UsageType.WorkersCemetery,       GetUsageCountWorkersService<CemeteryAI                >);
                AssociateBuildingAI<FireStationAI                         >(UsageType.WorkersFireStation,    GetUsageCountWorkersService<FireStationAI             >);
                AssociateBuildingAI<FirewatchTowerAI                      >(UsageType.WorkersFireStation,    GetUsageCountWorkersService<FirewatchTowerAI          >);
                AssociateBuildingAI<DisasterResponseBuildingAI            >(UsageType.WorkersDisaster,       GetUsageCountWorkersService<DisasterResponseBuildingAI>);
                AssociateBuildingAI<ShelterAI                             >(UsageType.WorkersDisaster,       GetUsageCountWorkersService<ShelterAI                 >);
                AssociateBuildingAI<RadioMastAI                           >(UsageType.WorkersDisaster,       GetUsageCountWorkersService<RadioMastAI               >);
                AssociateBuildingAI<EarthquakeSensorAI                    >(UsageType.WorkersDisaster,       GetUsageCountWorkersService<EarthquakeSensorAI        >);
                AssociateBuildingAI<DoomsdayVaultAI                       >(UsageType.WorkersDisaster,       GetUsageCountWorkersService<DoomsdayVaultAI           >);
                AssociateBuildingAI<WeatherRadarAI                        >(UsageType.WorkersDisaster,       GetUsageCountWorkersService<WeatherRadarAI            >);
                AssociateBuildingAI<SpaceRadarAI                          >(UsageType.WorkersDisaster,       GetUsageCountWorkersService<SpaceRadarAI              >);
                AssociateBuildingAI<PoliceStationAI                       >(UsageType.WorkersPoliceStation,  GetUsageCountWorkersService<PoliceStationAI           >);
                AssociateBuildingAI<SchoolAI                              >(UsageType.WorkersEducation,      GetUsageCountWorkersService<SchoolAI                  >);
                AssociateBuildingAI<LibraryAI                             >(UsageType.WorkersEducation,      GetUsageCountWorkersService<LibraryAI                 >);
                AssociateBuildingAI<HadronColliderAI                      >(UsageType.WorkersEducation,      GetUsageCountWorkersService<HadronColliderAI          >);
                AssociateBuildingAI<MainCampusBuildingAI                  >(UsageType.WorkersEducation,      GetUsageCountWorkersService<MainCampusBuildingAI      >);
                AssociateBuildingAI<CampusBuildingAI                      >(UsageType.WorkersEducation,      GetUsageCountWorkersService<CampusBuildingAI          >);
                AssociateBuildingAI<UniqueFacultyAI                       >(UsageType.WorkersEducation,      GetUsageCountWorkersService<UniqueFacultyAI           >);
                AssociateBuildingAI<MuseumAI                              >(UsageType.WorkersEducation,      GetUsageCountWorkersService<MuseumAI                  >);
                AssociateBuildingAI<VarsitySportsArenaAI                  >(UsageType.WorkersEducation,      GetUsageCountWorkersService<VarsitySportsArenaAI      >);
                AssociateBuildingAI<AirportAuxBuildingAI                  >(UsageType.UseLogic1,             GetUsageCountWorkersService<AirportAuxBuildingAI      >);
                AssociateBuildingAI<AirportEntranceAI                     >(UsageType.UseLogic1,             GetUsageCountWorkersService<AirportEntranceAI         >);
                AssociateBuildingAI<AirportGateAI                         >(UsageType.UseLogic1,             GetUsageCountWorkersService<AirportGateAI             >);
                AssociateBuildingAI<AirportCargoGateAI                    >(UsageType.UseLogic1,             GetUsageCountWorkersService<AirportCargoGateAI        >);
                AssociateBuildingAI<CargoStationAI                        >(UsageType.UseLogic1,             GetUsageCountWorkersService<CargoStationAI            >);
                AssociateBuildingAI<CargoHarborAI                         >(UsageType.UseLogic1,             GetUsageCountWorkersService<CargoHarborAI             >);
                AssociateBuildingAI<DepotAI                               >(UsageType.UseLogic1,             GetUsageCountWorkersService<DepotAI                   >);
                AssociateBuildingAI<CableCarStationAI                     >(UsageType.UseLogic1,             GetUsageCountWorkersService<CableCarStationAI         >);
                AssociateBuildingAI<TransportStationAI                    >(UsageType.UseLogic1,             GetUsageCountWorkersService<TransportStationAI        >);
                AssociateBuildingAI<HarborAI                              >(UsageType.UseLogic1,             GetUsageCountWorkersService<HarborAI                  >);
                AssociateBuildingAI<SpaceElevatorAI                       >(UsageType.UseLogic1,             GetUsageCountWorkersService<SpaceElevatorAI           >);
                AssociateBuildingAI<PostOfficeAI                          >(UsageType.WorkersPost,           GetUsageCountWorkersService<PostOfficeAI              >);
                AssociateBuildingAI<ParkGateAI                            >(UsageType.UseLogic1,             GetUsageCountWorkersPark                               );
                AssociateBuildingAI<ParkBuildingAI                        >(UsageType.UseLogic1,             GetUsageCountWorkersPark                               );
                AssociateBuildingAI<MonumentAI                            >(UsageType.WorkersUnique,         GetUsageCountWorkersService<MonumentAI                >);
                AssociateBuildingAI<AirlineHeadquartersAI                 >(UsageType.WorkersUnique,         GetUsageCountWorkersService<AirlineHeadquartersAI     >);
                AssociateBuildingAI<AnimalMonumentAI                      >(UsageType.WorkersUnique,         GetUsageCountWorkersService<AnimalMonumentAI          >);
                AssociateBuildingAI<PrivateAirportAI                      >(UsageType.WorkersUnique,         GetUsageCountWorkersService<PrivateAirportAI          >);
                AssociateBuildingAI<ChirpwickCastleAI                     >(UsageType.WorkersUnique,         GetUsageCountWorkersService<ChirpwickCastleAI         >);

                // associate nursing home building AI type with its usage type(s) and usage count routine(s)
                // associate building AI even if corresponding mod is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI("SeniorCitizenCenterMod.NursingHomeAi", UsageType.WorkersMedical,        GetUsageCountWorkersNursingHome,
                                                                            UsageType.HouseholdsResidential, GetUsageCountHouseholdsNursingHome);

                // get the last usage group
                UsageGroup lastUsageGroup = null;
                foreach (UsageGroup usageGroup in _usageGroups.Values)
                {
                    lastUsageGroup = usageGroup;
                }

                // create the worker summary panel under last usage group
                UIPanel workerSummaryPanel = AddUIComponent<UIPanel>();
                if (workerSummaryPanel == null)
                {
                    LogUtil.LogError($"Unable to create worker summary panel.");
                    return;
                }
                workerSummaryPanel.name = "WorkerSummaryPanel";
                workerSummaryPanel.relativePosition = new Vector3(5f, lastUsageGroup.checkBox.relativePosition.y + lastUsageGroup.checkBox.size.y + 5f);
                workerSummaryPanel.size = new Vector2(this.width - 10f, 138f);
                workerSummaryPanel.atlas = _ingameAtlas;
                workerSummaryPanel.backgroundSprite = "GenericPanel";

                // create employment heading and data rows
                const float RowHeight = 14f;
                float top = 2f;
                if (!CreateHeading(workerSummaryPanel, top, "EmpHeading",   "Employment"                             )) return; top += RowHeight + 2f;
                if (!CreateDataRow(workerSummaryPanel, top, "Employed",     "Employed",         out _employed        )) return; top += RowHeight;
                if (!CreateDataRow(workerSummaryPanel, top, "TotalJobs",    "Total Jobs",       out _totalJobs       )) return; top += RowHeight;
                if (!CreateDataRow(workerSummaryPanel, top, "UnfilledJobs", "Unfilled Jobs",    out _unfilledJobs    )) return; top += RowHeight;
                if (!CreateDataRow(workerSummaryPanel, top, "OverEducated", "Over Educated",    out _overEducated    )) return; top += RowHeight;

                // create unemployment heading and data rows
                top += 5f;
                if (!CreateHeading(workerSummaryPanel, top, "UnempHeading", "Unemployment"                           )) return; top += RowHeight + 2f;
                if (!CreateDataRow(workerSummaryPanel, top, "Unemployed",   "Unemployed",       out _unemployed      )) return; top += RowHeight;
                if (!CreateDataRow(workerSummaryPanel, top, "Eligible",     "Eligible Workers", out _eligible        )) return; top += RowHeight;
                if (!CreateDataRow(workerSummaryPanel, top, "UnemplRate",   "Unemploy Rate",    out _unemploymentRate)) return; top += RowHeight;

                // add tool tips to row descriptions
                _employed.description.tooltip = "Citizens with a job";
                _totalJobs.description.tooltip = "Total number of jobs in buildings";
                _unfilledJobs.description.tooltip = "Total Jobs minus Employed";
                _overEducated.description.tooltip = "Workers with more education than job requires";

                _unemployed.description.tooltip = "Workers not in a job";
                _eligible.description.tooltip = "Citizens eligible to work";
                _unemploymentRate.description.tooltip = "Unemployment rate (Unemployed divided by Eligible)";

                // uneducated workers can never be over educated, so hide that label
                _overEducated.eduLevel0.isVisible = false;
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
        }


        /// <summary>
        /// create a heading row with lines under each heading
        /// </summary>
        private bool CreateHeading(UIPanel workerSummaryPanel, float top, string namePrefix, string text)
        {
            // create a data row and then adjust properties
            if (!CreateDataRow(workerSummaryPanel, top, namePrefix, "", out WorkerDataUI heading)) return false;

            // set texts
            heading.description.text = text;
            heading.eduLevel0.text = "Unedu";
            heading.eduLevel1.text = "Educated";
            heading.eduLevel2.text = "Well Edu";
            heading.eduLevel3.text = "High Edu";
            heading.total.text = "Total";

            // set tool tips
            heading.eduLevel0.tooltip = "Uneducated - Elementary School not completed";
            heading.eduLevel1.tooltip = "Educated - completed Elementary School";
            heading.eduLevel2.tooltip = "Well Educated - completed High School";
            heading.eduLevel3.tooltip = "Highly Educated - completed University";
            heading.total.tooltip = "Sum of the education levels";

            // set text size
            heading.description.textScale =
                heading.eduLevel0.textScale =
                heading.eduLevel1.textScale =
                heading.eduLevel2.textScale =
                heading.eduLevel3.textScale =
                heading.total.textScale = 0.5625f;

            // set text color
            heading.description.textColor =
                heading.eduLevel0.textColor =
                heading.eduLevel1.textColor =
                heading.eduLevel2.textColor =
                heading.eduLevel3.textColor =
                heading.total.textColor = EducationTextColor;

            // create a line under each heading
            top += 13f;
            if (!CreateLineUnderHeading(workerSummaryPanel, top, heading.description, out UISprite lineDescription)) return false;
            if (!CreateLineUnderHeading(workerSummaryPanel, top, heading.eduLevel0,   out UISprite _              )) return false;
            if (!CreateLineUnderHeading(workerSummaryPanel, top, heading.eduLevel1,   out UISprite _              )) return false;
            if (!CreateLineUnderHeading(workerSummaryPanel, top, heading.eduLevel2,   out UISprite _              )) return false;
            if (!CreateLineUnderHeading(workerSummaryPanel, top, heading.eduLevel3,   out UISprite _              )) return false;
            if (!CreateLineUnderHeading(workerSummaryPanel, top, heading.total,       out UISprite _              )) return false;

            // adjust description line size and position
            lineDescription.size = new Vector2(lineDescription.size.x + 4f, lineDescription.size.y);
            lineDescription.relativePosition = new Vector3(lineDescription.relativePosition.x - 5f, lineDescription.relativePosition.y);

            // success
            return true;
        }

        /// <summary>
        /// create a line under a heading
        /// </summary>
        private bool CreateLineUnderHeading(UIPanel workerSummaryPanel, float top, UILabel headingLabel, out UISprite line)
        {
            line = workerSummaryPanel.AddUIComponent<UISprite>();
            if (line == null)
            {
                LogUtil.LogError($"Unable to create line under [{headingLabel.name}] on panel [{workerSummaryPanel.name}].");
                return false;
            }
            line.name = headingLabel.name + "Line";
            line.autoSize = false;
            line.size = new Vector2(headingLabel.size.x, 1f);
            line.relativePosition = new Vector3(headingLabel.relativePosition.x + 2f, top);
            line.atlas = _ingameAtlas;
            line.spriteName = "EmptySprite";
            const float ColorMult = 0.8f;
            line.color = new Color32((byte)(EducationTextColor.r * ColorMult), (byte)(EducationTextColor.g * ColorMult), (byte)(EducationTextColor.b * ColorMult), 255);
            line.isVisible = true;

            // success
            return true;
        }

        /// <summary>
        /// create a row for displaying data
        /// </summary>
        private bool CreateDataRow(UIPanel workerSummaryPanel, float top, string namePrefix, string text, out WorkerDataUI workerData)
        {
            // create new worker data
            workerData = new WorkerDataUI();

            // common attributes
            const float CountWidth = 50f;
            const float TextHeight = 14f;
            const float LabelSpacing = 2f;

            // create label for description
            workerData.description = workerSummaryPanel.AddUIComponent<UILabel>();
            if (workerData.description == null)
            {
                LogUtil.LogError($"Unable to create description label for [{namePrefix}] on panel [{workerSummaryPanel.name}].");
                return false;
            }
            workerData.description.name = namePrefix + "Description";
            workerData.description.font = _textFont;
            workerData.description.text = text;
            workerData.description.textAlignment = UIHorizontalAlignment.Left;
            workerData.description.verticalAlignment = UIVerticalAlignment.Bottom;
            workerData.description.textScale = 0.625f;
            workerData.description.textColor = new Color32(254, 254, 254, 255);
            workerData.description.autoSize = false;
            workerData.description.size = new Vector2(105f, TextHeight);
            workerData.description.relativePosition = new Vector3(5f, top);
            workerData.description.isVisible = true;

            // create label for education level 0
            workerData.eduLevel0 = workerSummaryPanel.AddUIComponent<UILabel>();
            if (workerData.eduLevel0 == null)
            {
                LogUtil.LogError($"Unable to create education level 0 label for [{namePrefix}] on panel [{workerSummaryPanel.name}].");
                return false;
            }
            workerData.eduLevel0.name = namePrefix + "Level0";
            workerData.eduLevel0.font = _textFont;
            workerData.eduLevel0.text = "000,000";
            workerData.eduLevel0.textAlignment = UIHorizontalAlignment.Right;
            workerData.eduLevel0.verticalAlignment = UIVerticalAlignment.Bottom;
            workerData.eduLevel0.textScale = 0.625f;
            workerData.eduLevel0.textColor = new Color32(254, 254, 254, 255);
            workerData.eduLevel0.autoSize = false;
            workerData.eduLevel0.size = new Vector2(CountWidth, TextHeight);
            workerData.eduLevel0.relativePosition = new Vector3(workerData.description.relativePosition.x + workerData.description.size.x + LabelSpacing, top);
            workerData.eduLevel0.isVisible = true;

            // create label for education level 1
            workerData.eduLevel1 = workerSummaryPanel.AddUIComponent<UILabel>();
            if (workerData.eduLevel1 == null)
            {
                LogUtil.LogError($"Unable to create education level 1 label for [{namePrefix}] on panel [{workerSummaryPanel.name}].");
                return false;
            }
            workerData.eduLevel1.name = namePrefix + "Level1";
            workerData.eduLevel1.font = _textFont;
            workerData.eduLevel1.text = "000,000";
            workerData.eduLevel1.textAlignment = UIHorizontalAlignment.Right;
            workerData.eduLevel1.verticalAlignment = UIVerticalAlignment.Bottom;
            workerData.eduLevel1.textScale = 0.625f;
            workerData.eduLevel1.textColor = new Color32(254, 254, 254, 255);
            workerData.eduLevel1.autoSize = false;
            workerData.eduLevel1.size = new Vector2(CountWidth, TextHeight);
            workerData.eduLevel1.relativePosition = new Vector3(workerData.eduLevel0.relativePosition.x + workerData.eduLevel0.size.x + LabelSpacing, top);
            workerData.eduLevel1.isVisible = true;

            // create label for education level 2
            workerData.eduLevel2 = workerSummaryPanel.AddUIComponent<UILabel>();
            if (workerData.eduLevel2 == null)
            {
                LogUtil.LogError($"Unable to create education level 2 label for [{namePrefix}] on panel [{workerSummaryPanel.name}].");
                return false;
            }
            workerData.eduLevel2.name = namePrefix + "Level2";
            workerData.eduLevel2.font = _textFont;
            workerData.eduLevel2.text = "000,000";
            workerData.eduLevel2.textAlignment = UIHorizontalAlignment.Right;
            workerData.eduLevel2.verticalAlignment = UIVerticalAlignment.Bottom;
            workerData.eduLevel2.textScale = 0.625f;
            workerData.eduLevel2.textColor = new Color32(254, 254, 254, 255);
            workerData.eduLevel2.autoSize = false;
            workerData.eduLevel2.size = new Vector2(CountWidth, TextHeight);
            workerData.eduLevel2.relativePosition = new Vector3(workerData.eduLevel1.relativePosition.x + workerData.eduLevel1.size.x + LabelSpacing, top);
            workerData.eduLevel2.isVisible = true;

            // create label for education level 3
            workerData.eduLevel3 = workerSummaryPanel.AddUIComponent<UILabel>();
            if (workerData.eduLevel3 == null)
            {
                LogUtil.LogError($"Unable to create education level 3 label for [{namePrefix}] on panel [{workerSummaryPanel.name}].");
                return false;
            }
            workerData.eduLevel3.name = namePrefix + "Level3";
            workerData.eduLevel3.font = _textFont;
            workerData.eduLevel3.text = "000,000";
            workerData.eduLevel3.textAlignment = UIHorizontalAlignment.Right;
            workerData.eduLevel3.verticalAlignment = UIVerticalAlignment.Bottom;
            workerData.eduLevel3.textScale = 0.625f;
            workerData.eduLevel3.textColor = new Color32(254, 254, 254, 255);
            workerData.eduLevel3.autoSize = false;
            workerData.eduLevel3.size = new Vector2(CountWidth, TextHeight);
            workerData.eduLevel3.relativePosition = new Vector3(workerData.eduLevel2.relativePosition.x + workerData.eduLevel2.size.x + LabelSpacing, top);
            workerData.eduLevel3.isVisible = true;

            // create label for total
            workerData.total = workerSummaryPanel.AddUIComponent<UILabel>();
            if (workerData.total == null)
            {
                LogUtil.LogError($"Unable to create total jobs label for [{namePrefix}] on panel [{workerSummaryPanel.name}].");
                return false;
            }
            workerData.total.name = namePrefix + "Total";
            workerData.total.font = _textFont;
            workerData.total.text = "000,000";
            workerData.total.textAlignment = UIHorizontalAlignment.Right;
            workerData.total.verticalAlignment = UIVerticalAlignment.Bottom;
            workerData.total.textScale = 0.625f;
            workerData.total.textColor = new Color32(254, 254, 254, 255);
            workerData.total.autoSize = false;
            workerData.total.size = new Vector2(CountWidth, TextHeight);
            workerData.total.relativePosition = new Vector3(workerData.eduLevel3.relativePosition.x + workerData.eduLevel3.size.x + LabelSpacing, top);
            workerData.total.isVisible = true;

            // success
            return true;
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
            else if (buildingAIType == typeof(AirportAuxBuildingAI) ||
                     buildingAIType == typeof(AirportEntranceAI   ) ||
                     buildingAIType == typeof(AirportGateAI       ) ||
                     buildingAIType == typeof(AirportCargoGateAI  ) ||
                     buildingAIType == typeof(CargoStationAI      ) ||
                     buildingAIType == typeof(CargoHarborAI       ) ||
                     buildingAIType == typeof(DepotAI             ) ||
                     buildingAIType == typeof(CableCarStationAI   ) ||
                     buildingAIType == typeof(TransportStationAI  ) ||
                     buildingAIType == typeof(HarborAI            ) ||
                     buildingAIType == typeof(SpaceElevatorAI     ))
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


        /// <summary>
        /// update the panel
        /// </summary>
        public override bool UpdatePanel()
        {
            // do base processing, proceed with processing here only if base was done
            if (!base.UpdatePanel())
            {
                return false;
            }

            try
            {
                // compute the totals by looping over the usage groups
                EducationLevelCount employed     = new EducationLevelCount();
                EducationLevelCount totalJobs    = new EducationLevelCount();
                EducationLevelCount overEducated = new EducationLevelCount();
                EducationLevelCount unemployed   = new EducationLevelCount();
                EducationLevelCount eligible     = new EducationLevelCount();
                foreach (UsageGroup usageGroup in _usageGroups.Values)
                {
                    // for each selected usage group, add running total for employed, total jobs, and overeducated
                    if (IsCheckBoxChecked(usageGroup.checkBox))
                    {
                        employed.Add(usageGroup.employedRunningTotal);
                        totalJobs.Add(usageGroup.totalJobsRunningTotal);
                        overEducated.Add(usageGroup.overEdRunningTotal);
                    }

                    // always add running total for unemployed and eligible
                    unemployed.Add(usageGroup.unemployedRunningTotal);
                    eligible.Add(usageGroup.eligibleRunningTotal);
                }

                // unfilled jobs is total jobs minus employed
                EducationLevelCount unfilledJobs = new EducationLevelCount();
                unfilledJobs.Copy(totalJobs);
                unfilledJobs.Subtract(employed);

                // display employed
                _employed.eduLevel0.text = employed.level0.ToString("N0", LocaleManager.cultureInfo);
                _employed.eduLevel1.text = employed.level1.ToString("N0", LocaleManager.cultureInfo);
                _employed.eduLevel2.text = employed.level2.ToString("N0", LocaleManager.cultureInfo);
                _employed.eduLevel3.text = employed.level3.ToString("N0", LocaleManager.cultureInfo);
                _employed.total.text = (employed.level0 + employed.level1 + employed.level2 + employed.level3).ToString("N0", LocaleManager.cultureInfo);

                // display total jobs
                _totalJobs.eduLevel0.text = totalJobs.level0.ToString("N0", LocaleManager.cultureInfo);
                _totalJobs.eduLevel1.text = totalJobs.level1.ToString("N0", LocaleManager.cultureInfo);
                _totalJobs.eduLevel2.text = totalJobs.level2.ToString("N0", LocaleManager.cultureInfo);
                _totalJobs.eduLevel3.text = totalJobs.level3.ToString("N0", LocaleManager.cultureInfo);
                _totalJobs.total.text = (totalJobs.level0 + totalJobs.level1 + totalJobs.level2 + totalJobs.level3).ToString("N0", LocaleManager.cultureInfo);

                // display unfilled jobs
                _unfilledJobs.eduLevel0.text = unfilledJobs.level0.ToString("N0", LocaleManager.cultureInfo);
                _unfilledJobs.eduLevel1.text = unfilledJobs.level1.ToString("N0", LocaleManager.cultureInfo);
                _unfilledJobs.eduLevel2.text = unfilledJobs.level2.ToString("N0", LocaleManager.cultureInfo);
                _unfilledJobs.eduLevel3.text = unfilledJobs.level3.ToString("N0", LocaleManager.cultureInfo);
                _unfilledJobs.total.text = (unfilledJobs.level0 + unfilledJobs.level1 + unfilledJobs.level2 + unfilledJobs.level3).ToString("N0", LocaleManager.cultureInfo);

                // display over educated (education level 0 are never over educated)
                _overEducated.eduLevel1.text = overEducated.level1.ToString("N0", LocaleManager.cultureInfo);
                _overEducated.eduLevel2.text = overEducated.level2.ToString("N0", LocaleManager.cultureInfo);
                _overEducated.eduLevel3.text = overEducated.level3.ToString("N0", LocaleManager.cultureInfo);
                _overEducated.total.text = (overEducated.level1 + overEducated.level2 + overEducated.level3).ToString("N0", LocaleManager.cultureInfo);

                // display unemployed
                int unemployedTotal = unemployed.level0 + unemployed.level1 + unemployed.level2 + unemployed.level3;
                _unemployed.eduLevel0.text = unemployed.level0.ToString("N0", LocaleManager.cultureInfo);
                _unemployed.eduLevel1.text = unemployed.level1.ToString("N0", LocaleManager.cultureInfo);
                _unemployed.eduLevel2.text = unemployed.level2.ToString("N0", LocaleManager.cultureInfo);
                _unemployed.eduLevel3.text = unemployed.level3.ToString("N0", LocaleManager.cultureInfo);
                _unemployed.total.text = unemployedTotal.ToString("N0", LocaleManager.cultureInfo);

                // display eligible
                int eligibleTotal = eligible.level0 + eligible.level1 + eligible.level2 + eligible.level3;
                _eligible.eduLevel0.text = eligible.level0.ToString("N0", LocaleManager.cultureInfo);
                _eligible.eduLevel1.text = eligible.level1.ToString("N0", LocaleManager.cultureInfo);
                _eligible.eduLevel2.text = eligible.level2.ToString("N0", LocaleManager.cultureInfo);
                _eligible.eduLevel3.text = eligible.level3.ToString("N0", LocaleManager.cultureInfo);
                _eligible.total.text = eligibleTotal.ToString("N0", LocaleManager.cultureInfo);

                // display unemployment rate
                float unemploymentRate0 = (eligible.level0 == 0 ? 0f : 100f * unemployed.level0 / eligible.level0);
                float unemploymentRate1 = (eligible.level1 == 0 ? 0f : 100f * unemployed.level1 / eligible.level1);
                float unemploymentRate2 = (eligible.level2 == 0 ? 0f : 100f * unemployed.level2 / eligible.level2);
                float unemploymentRate3 = (eligible.level3 == 0 ? 0f : 100f * unemployed.level3 / eligible.level3);
                float unemploymentRateTotal = (eligibleTotal == 0 ? 0f : 100f * unemployedTotal / eligibleTotal);
                _unemploymentRate.eduLevel0.text = unemploymentRate0.ToString("F1", LocaleManager.cultureInfo) + "%";
                _unemploymentRate.eduLevel1.text = unemploymentRate1.ToString("F1", LocaleManager.cultureInfo) + "%";
                _unemploymentRate.eduLevel2.text = unemploymentRate2.ToString("F1", LocaleManager.cultureInfo) + "%";
                _unemploymentRate.eduLevel3.text = unemploymentRate3.ToString("F1", LocaleManager.cultureInfo) + "%";
                _unemploymentRate.total.text = unemploymentRateTotal.ToString("F1", LocaleManager.cultureInfo) + "%";
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }

            // panel was updated
            return true;
        }

    }
}
