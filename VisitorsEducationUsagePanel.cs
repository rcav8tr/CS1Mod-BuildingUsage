using System;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display visitor education usage
    /// </summary>
    public class VisitorsEducationUsagePanel : UsagePanel
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

                // create the usage groups
                if (IsBuildingAITypeDefined<SchoolAI, LibraryAI>()) CreateGroupHeading("Basic Education");
                CreateUsageGroup<SchoolAI               >(UsageType.VisitorsEducationElementarySchool);
                CreateUsageGroup<SchoolAI               >(UsageType.VisitorsEducationHighSchool);
                CreateUsageGroup<SchoolAI               >(UsageType.VisitorsEducationUniversity);
                CreateUsageGroup<LibraryAI              >(UsageType.VisitorsEducationLibrary);

                if (IsBuildingAITypeDefined<CampusBuildingAI, UniqueFacultyAI>()) CreateGroupHeading("Trade School");
                CreateUsageGroup<CampusBuildingAI       >(UsageType.VisitorsEducationTradeSchoolEducation);
                CreateUsageGroup<UniqueFacultyAI        >(UsageType.VisitorsEducationTradeSchoolFaculty);

                if (IsBuildingAITypeDefined<CampusBuildingAI, UniqueFacultyAI>()) CreateGroupHeading("Liberal Arts");
                CreateUsageGroup<CampusBuildingAI       >(UsageType.VisitorsEducationLiberalArtsEducation);
                CreateUsageGroup<UniqueFacultyAI        >(UsageType.VisitorsEducationLiberalArtsFaculty);

                if (IsBuildingAITypeDefined<CampusBuildingAI, UniqueFacultyAI>()) CreateGroupHeading("University");
                CreateUsageGroup<CampusBuildingAI       >(UsageType.VisitorsEducationUniversityEducation);
                CreateUsageGroup<UniqueFacultyAI        >(UsageType.VisitorsEducationUniversityFaculty);

                if (IsBuildingAITypeDefined<MuseumAI, VarsitySportsArenaAI>()) CreateGroupHeading("Other Campus");
                CreateUsageGroup<MuseumAI               >(UsageType.VisitorsEducationMuseum);
                CreateUsageGroup<VarsitySportsArenaAI   >(UsageType.VisitorsEducationVarsitySports);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<SchoolAI            >(UsageType.UseLogic1,                      GetUsageCountVisitorsSchool<SchoolAI>        );
                AssociateBuildingAI<LibraryAI           >(UsageType.VisitorsEducationLibrary,       GetUsageCountVisitorsLibrary                 );
                AssociateBuildingAI<CampusBuildingAI    >(UsageType.UseLogic1,                      GetUsageCountVisitorsSchool<CampusBuildingAI>);
                AssociateBuildingAI<UniqueFacultyAI     >(UsageType.UseLogic1,                      GetUsageCountVisitorsSchool<UniqueFacultyAI> );
                AssociateBuildingAI<MuseumAI            >(UsageType.VisitorsEducationMuseum,        GetUsageCountVisitorsMonument<MuseumAI>      );
                AssociateBuildingAI<VarsitySportsArenaAI>(UsageType.VisitorsEducationVarsitySports, GetUsageCountVisitorsVarsitySportsArena      );
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
                    return UsageType.VisitorsEducationElementarySchool;
                }
                if (level == ItemClass.Level.Level2)
                {
                    return UsageType.VisitorsEducationHighSchool;
                }
                if (level == ItemClass.Level.Level3)
                {
                    return UsageType.VisitorsEducationUniversity;
                }
            }
            else if (buildingAIType == typeof(CampusBuildingAI))
            {
                // cannot determine how to differentiate education buildings from supplementary buildings
                ItemClass.SubService subService = data.Info.m_class.m_subService;
                if (subService == ItemClass.SubService.PlayerEducationTradeSchool)
                {
                    return UsageType.VisitorsEducationTradeSchoolEducation;
                }
                if (subService == ItemClass.SubService.PlayerEducationLiberalArts)
                {
                    return UsageType.VisitorsEducationLiberalArtsEducation;
                }
                if (subService == ItemClass.SubService.PlayerEducationUniversity)
                {
                    return UsageType.VisitorsEducationUniversityEducation;
                }
            }
            else if (buildingAIType == typeof(UniqueFacultyAI))
            {
                ItemClass.SubService subService = data.Info.m_class.m_subService;
                if (subService == ItemClass.SubService.PlayerEducationTradeSchool)
                {
                    return UsageType.VisitorsEducationTradeSchoolFaculty;
                }
                if (subService == ItemClass.SubService.PlayerEducationLiberalArts)
                {
                    return UsageType.VisitorsEducationLiberalArtsFaculty;
                }
                if (subService == ItemClass.SubService.PlayerEducationUniversity)
                {
                    return UsageType.VisitorsEducationUniversityFaculty;
                }
            }

            // usage type not determined with above logic
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
