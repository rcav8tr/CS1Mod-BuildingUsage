using UnityEngine;
using System;

namespace BuildingUsage
{
    /// <summary>
    /// a panel to display visitor usage
    /// </summary>
    public class VisitorsUsagePanel : UsagePanel
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
                CreateUsageGroup<MarketAI                                                                               >(UsageType.VisitorsFishMarket);
                CreateUsageGroup<HospitalAI, ChildcareAI, EldercareAI, MedicalCenterAI                                  >(UsageType.VisitorsMedicalPatients);
                CreateUsageGroup<SaunaAI                                                                                >(UsageType.VisitorsMedicalVisitors);
                CreateUsageGroup<CemeteryAI                                                                             >(UsageType.VisitorsDeceased);
                CreateUsageGroup<ShelterAI                                                                              >(UsageType.VisitorsShelter);
                CreateUsageGroup<PoliceStationAI                                                                        >(UsageType.VisitorsCriminals);
                CreateUsageGroup<SchoolAI, LibraryAI, CampusBuildingAI, UniqueFacultyAI, MuseumAI, VarsitySportsArenaAI >(UsageType.VisitorsEducation);
                CreateUsageGroup<ParkAI, EdenProjectAI, ParkBuildingAI, TourBuildingAI                                  >(UsageType.VisitorsParksPlazas);
                CreateUsageGroup<MonumentAI, AnimalMonumentAI, PrivateAirportAI, ChirpwickCastleAI                      >(UsageType.VisitorsUnique);

                // add detail panels
                AddDetailPanel<VisitorsEducationUsagePanel  >(UsageType.VisitorsEducation,   this);
                AddDetailPanel<VisitorsParksPlazasUsagePanel>(UsageType.VisitorsParksPlazas, this);
                AddDetailPanel<VisitorsUniqueUsagePanel     >(UsageType.VisitorsUnique,      this);

                // associate each building AI type with its usage type(s) and usage count routine(s)
                // associate building AIs even if corresponding DLC is not installed (there will simply be no buildings with that AI)
                AssociateBuildingAI<MarketAI                >(UsageType.VisitorsFishMarket,      (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsMarket                        (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<HospitalAI              >(UsageType.VisitorsMedicalPatients, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsHospital<HospitalAI>          (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MedicalCenterAI         >(UsageType.VisitorsMedicalPatients, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsHospital<MedicalCenterAI>     (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ChildcareAI             >(UsageType.VisitorsMedicalPatients, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsChildcare                     (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<EldercareAI             >(UsageType.VisitorsMedicalPatients, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsEldercare                     (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SaunaAI                 >(UsageType.VisitorsMedicalVisitors, (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsSauna                         (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CemeteryAI              >(UsageType.VisitorsDeceased,        (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsCemetery                      (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ShelterAI               >(UsageType.VisitorsShelter,         (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsShelter                       (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<PoliceStationAI         >(UsageType.VisitorsCriminals,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsPoliceStation                 (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<SchoolAI                >(UsageType.VisitorsEducation,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsSchool<SchoolAI>              (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<LibraryAI               >(UsageType.VisitorsEducation,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsLibrary                       (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<CampusBuildingAI        >(UsageType.VisitorsEducation,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsSchool<CampusBuildingAI>      (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<UniqueFacultyAI         >(UsageType.VisitorsEducation,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsSchool<UniqueFacultyAI>       (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MuseumAI                >(UsageType.VisitorsEducation,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsMonument<MuseumAI>            (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<VarsitySportsArenaAI    >(UsageType.VisitorsEducation,       (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsVarsitySportsArena            (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ParkAI                  >(UsageType.VisitorsParksPlazas,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsPark<ParkAI>                  (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<EdenProjectAI           >(UsageType.VisitorsParksPlazas,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsPark<EdenProjectAI>           (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ParkBuildingAI          >(UsageType.VisitorsParksPlazas,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsParkBuilding                  (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<TourBuildingAI          >(UsageType.VisitorsParksPlazas,     (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsTourBuilding                  (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<MonumentAI              >(UsageType.VisitorsUnique,          (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsMonument<MonumentAI>          (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<AnimalMonumentAI        >(UsageType.VisitorsUnique,          (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsMonument<AnimalMonumentAI>    (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<PrivateAirportAI        >(UsageType.VisitorsUnique,          (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsMonument<PrivateAirportAI>    (buildingID, ref data, ref used, ref allowed));
                AssociateBuildingAI<ChirpwickCastleAI       >(UsageType.VisitorsUnique,          (ushort buildingID, ref Building data, ref int used, ref int allowed) => GetUsageCountVisitorsMonument<ChirpwickCastleAI>   (buildingID, ref data, ref used, ref allowed));
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
            // usage type not determined with above logic
            Type buildingAIType = data.Info.m_buildingAI.GetType();
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
