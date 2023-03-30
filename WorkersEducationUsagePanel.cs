using System;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display education worker usage
    /// </summary>
    public class WorkersEducationUsagePanel : UsagePanel
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

                // create the usage groups
                if (IsBuildingAITypeDefined<SchoolAI, LibraryAI, HadronColliderAI>()) CreateGroupHeading("Basic Education");
                CreateUsageGroup<SchoolAI               >(UsageType.WorkersEducationElementarySchool);
                CreateUsageGroup<SchoolAI               >(UsageType.WorkersEducationHighSchool);
                CreateUsageGroup<SchoolAI               >(UsageType.WorkersEducationUniversity);
                CreateUsageGroup<LibraryAI              >(UsageType.WorkersEducationLibrary);
                CreateUsageGroup<HadronColliderAI       >(UsageType.WorkersEducationHadronCollider);

                if (IsBuildingAITypeDefined<MainCampusBuildingAI, CampusBuildingAI, UniqueFacultyAI>()) CreateGroupHeading("Trade School");
                CreateUsageGroup<MainCampusBuildingAI   >(UsageType.WorkersEducationTradeSchoolAdmin);
                CreateUsageGroup<CampusBuildingAI       >(UsageType.WorkersEducationTradeSchoolEducation);
                CreateUsageGroup<UniqueFacultyAI        >(UsageType.WorkersEducationTradeSchoolFaculty);

                if (IsBuildingAITypeDefined<MainCampusBuildingAI, CampusBuildingAI, UniqueFacultyAI>()) CreateGroupHeading("Liberal Arts");
                CreateUsageGroup<MainCampusBuildingAI   >(UsageType.WorkersEducationLiberalArtsAdmin);
                CreateUsageGroup<CampusBuildingAI       >(UsageType.WorkersEducationLiberalArtsEducation);
                CreateUsageGroup<UniqueFacultyAI        >(UsageType.WorkersEducationLiberalArtsFaculty);

                if (IsBuildingAITypeDefined<MainCampusBuildingAI, CampusBuildingAI, UniqueFacultyAI>()) CreateGroupHeading("University");
                CreateUsageGroup<MainCampusBuildingAI   >(UsageType.WorkersEducationUniversityAdmin);
                CreateUsageGroup<CampusBuildingAI       >(UsageType.WorkersEducationUniversityEducation);
                CreateUsageGroup<UniqueFacultyAI        >(UsageType.WorkersEducationUniversityFaculty);

                if (IsBuildingAITypeDefined<MuseumAI, VarsitySportsArenaAI>()) CreateGroupHeading("Other Campus");
                CreateUsageGroup<MuseumAI               >(UsageType.WorkersEducationMuseum);
                CreateUsageGroup<VarsitySportsArenaAI   >(UsageType.WorkersEducationVarsitySports);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<SchoolAI            >(UsageType.UseLogic1,                      GetUsageCountWorkersService<SchoolAI            >);
                AssociateBuildingAI<LibraryAI           >(UsageType.WorkersEducationLibrary,        GetUsageCountWorkersService<LibraryAI           >);
                AssociateBuildingAI<HadronColliderAI    >(UsageType.UseLogic1,                      GetUsageCountWorkersService<HadronColliderAI    >);
                AssociateBuildingAI<MainCampusBuildingAI>(UsageType.UseLogic1,                      GetUsageCountWorkersService<MainCampusBuildingAI>);
                AssociateBuildingAI<CampusBuildingAI    >(UsageType.UseLogic1,                      GetUsageCountWorkersService<CampusBuildingAI    >);
                AssociateBuildingAI<UniqueFacultyAI     >(UsageType.UseLogic1,                      GetUsageCountWorkersService<UniqueFacultyAI     >);
                AssociateBuildingAI<MuseumAI            >(UsageType.WorkersEducationMuseum,         GetUsageCountWorkersService<MuseumAI            >);
                AssociateBuildingAI<VarsitySportsArenaAI>(UsageType.WorkersEducationVarsitySports,  GetUsageCountWorkersService<VarsitySportsArenaAI>);
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
            if (buildingAIType == typeof(SchoolAI))
            {
                // convert level to usage type
                ItemClass.Level level = data.Info.m_class.m_level;
                if (level == ItemClass.Level.Level1)
                {
                    return UsageType.WorkersEducationElementarySchool;
                }
                if (level == ItemClass.Level.Level2)
                {
                    return UsageType.WorkersEducationHighSchool;
                }
                if (level == ItemClass.Level.Level3)
                {
                    return UsageType.WorkersEducationUniversity;
                }
            }
            else if (buildingAIType == typeof(HadronColliderAI))
            {
                // there are two monuments in Africa in Miniature CCP that have HadronColliderAI, so need to check service
                if (data.Info.m_class.m_service == ItemClass.Service.Education)
                {
                    return UsageType.WorkersEducationHadronCollider;
                }
                else
                {
                    // not an error, just don't include the buildings
                    return UsageType.None;
                }
            }
            else if (buildingAIType == typeof(MainCampusBuildingAI))
            {
                ItemClass.SubService subService = data.Info.m_class.m_subService;
                if (subService == ItemClass.SubService.PlayerEducationTradeSchool)
                {
                    return UsageType.WorkersEducationTradeSchoolAdmin;
                }
                if (subService == ItemClass.SubService.PlayerEducationLiberalArts)
                {
                    return UsageType.WorkersEducationLiberalArtsAdmin;
                }
                if (subService == ItemClass.SubService.PlayerEducationUniversity)
                {
                    return UsageType.WorkersEducationUniversityAdmin;
                }
            }
            else if (buildingAIType == typeof(CampusBuildingAI))
            {
                // cannot determine how to differentiate education buildings from supplementary buildings
                ItemClass.SubService subService = data.Info.m_class.m_subService;
                if (subService == ItemClass.SubService.PlayerEducationTradeSchool)
                {
                    return UsageType.WorkersEducationTradeSchoolEducation;
                }
                if (subService == ItemClass.SubService.PlayerEducationLiberalArts)
                {
                    return UsageType.WorkersEducationLiberalArtsEducation;
                }
                if (subService == ItemClass.SubService.PlayerEducationUniversity)
                {
                    return UsageType.WorkersEducationUniversityEducation;
                }
            }
            else if (buildingAIType == typeof(UniqueFacultyAI))
            {
                ItemClass.SubService subService = data.Info.m_class.m_subService;
                if (subService == ItemClass.SubService.PlayerEducationTradeSchool)
                {
                    return UsageType.WorkersEducationTradeSchoolFaculty;
                }
                if (subService == ItemClass.SubService.PlayerEducationLiberalArts)
                {
                    return UsageType.WorkersEducationLiberalArtsFaculty;
                }
                if (subService == ItemClass.SubService.PlayerEducationUniversity)
                {
                    return UsageType.WorkersEducationUniversityFaculty;
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

    }
}
