using UnityEngine;
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
                AssociateBuildingAI<SchoolAI            >(UsageType.UseLogic1,                      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<SchoolAI            >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<LibraryAI           >(UsageType.WorkersEducationLibrary,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<LibraryAI           >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<HadronColliderAI    >(UsageType.WorkersEducationHadronCollider, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<HadronColliderAI    >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MainCampusBuildingAI>(UsageType.UseLogic1,                      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MainCampusBuildingAI>(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CampusBuildingAI    >(UsageType.UseLogic1,                      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<CampusBuildingAI    >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<UniqueFacultyAI     >(UsageType.UseLogic1,                      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<UniqueFacultyAI     >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MuseumAI            >(UsageType.WorkersEducationMuseum,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<MuseumAI            >(buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<VarsitySportsArenaAI>(UsageType.WorkersEducationVarsitySports,  (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountWorkersService<VarsitySportsArenaAI>(buildingID, ref data, ref used, ref allowed));
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
