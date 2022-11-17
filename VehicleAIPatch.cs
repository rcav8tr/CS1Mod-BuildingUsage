using UnityEngine;

namespace BuildingUsage
{
    /// <summary>
    /// Harmony patching for vehicle AI
    /// </summary>
    public class VehicleAIPatch
    {
        // For vehicles not available because a DLC is not installed, the vehicle AI remains in the game logic.
        // The corresponding vehicle AI patch will simply never be called because there will be no vehicles of that type.
        // Therefore, there is no need to avoid patching a vehicle AI for missing DLC.

        // For a building with unlimited vehicles allowed, the building's vehicles allowed is set to 1, so the building appears 100% used if there are any vehicles.

        // For vehicle AIs that derive from other vehicle AIs (i.e. not derived from VehicleAI):
        //     If the derived vehicle AI has its own GetColor method, it is patched.
        //     If the derived vehicle AI has no GetColor method, Harmony won't allow it to be patched.
        //         But the base vehicle AI is patched and that patch will handle the derived vehicle AI.
        // Each vehicle AI that has its own GetColor method is marked with GC below.
        // Each vehicle AI that is used on a vehicle usage panel is marked with V below.


        // All AIs below derive from VehicleAI.

        // AircraftAI                           (base class with no buildings)
        //    CargoPlaneAI              GC  V   (unlimited:  Cargo Airport, Cargo Airport Hub)
        //    PassengerPlaneAI          GC  V   (unlimited:  Airport, International Airport)
        // BalloonAI                    GC  V   (unlimited:  Chirper Balloon Tours, Hot Air Balloon Tours)
        // BicycleAI                            (not generated from a service building)
        // BlimpAI                      GC      (base class with no buildings)
        //    PassengerBlimpAI          GC  V   (unlimited:  Blimp Depot)
        // CableCarBaseAI                       (base class with no buildings)
        //    CableCarAI                GC  V   (unlimited:  Cable Car Stop, End-of-Line Cable Car Stop)
        // CarAI                        GC      (base class with no buildings)
        //    AmbulanceAI               GC  V   Medical Clinic, Hospital, High-Capacity Hospital, Plastic Surgery Center (CCP), Medical Center (monument)
        //    BusAI                     GC  V   Small Emergency Shelter, Large Emergency Shelter,
        //                                      (unlimited:
        //                                          Bus Depot, Biofuel Bus Depot,
        //                                          Intercity Bus Station, Intercity Bus Terminal,
        //                                          Sightseeing Bus Depots,
        //                                          Bus-Intercity Bus Hub, Metro-Intercity Bus Hub)
        //    CargoTruckAI              GC  V   All buildings for ExtractingFacilityAI, FishFarmingAI, FishFarmAI, ProcessingFacilityAI, UniqueFactoryAI, WarehouseAI.
        //                                      (unlimited:  zoned industrial, Cargo Train Terminal, Cargo Harbor, Cargo Hub, Cargo Airport, Cargo Airport Hub)
        //    DisasterResponseVehicleAI GC  V   Disaster Response Unit
        //    FireTruckAI               GC  V   Fire House, Fire Station, High-Capacity Fire Station, Historical Fire Station (CCP), Fire Safety Center (CCP)
        //    GarbageTruckAI            GC  V   Landfill Site, Incineration Plant, Recycling Center, Eco-Friendly Incinerator Plant (CCP), Ultimate Recycling Plant (monument)
        //    HearseAI                  GC  V   Cemetery, Crematorium, Cryopreservatory (CCP), Crematorium Memorial Park (CCP)
        //    MaintenanceTruckAI        GC  V   Road Maintenance Depot
        //    ParkMaintenanceVehicleAI  GC  V   Park Maintenance Building
        //    PassengerCarAI            GC      (not generated from a service building)
        //    PoliceCarAI               GC  V   Police Station, Police Headquarters, High-Capacity Police Headquarters, Prison, Historical Police Station (CCP), Police Security Center (CCP)
        //    PostVanAI                 GC  V   Post Office, Post Sorting Facility
        //    SnowTruckAI               GC  V   Snow Dump
        //    TaxiAI                    GC  V   Taxi Depot
        //    TrolleybusAI              GC  V   Trolleybus Depot
        //    WaterTruckAI              GC  V   Pumping Service
        // CarTrailerAI                 GC      (the trailer for:  some CargoTruckAI, maybe others)
        // FerryAI                      GC      (base class with no buildings)
        //    FishingBoatAI             GC  V   Fishing Harbor, Anchovy Fishing Harbor, Salmon Fishing Harbor, Shellfish Fishing Harbor, Tuna Fishing Harbor
        //    PassengerFerryAI          GC  V   (unlimited:  Ferry Depot)
        // HelicopterAI                 GC      (base class with no buildings)
        //    AmbulanceCopterAI         GC  V   Medical Helicopter Depot
        //    DisasterResponseCopterAI  GC  V   Disaster Response Unit
        //    FireCopterAI              GC  V   Fire Helicopter Depot
        //    PoliceCopterAI            GC  V   Police Helicopter Depot
        // HelicopterDanglingAI         GC      (TBD is this for the bucket hanging under fire copters?)
        // MeteorAI                             (for meteor strike)
        // PassengerHelicopterAI        GC  V   (unlimited:  Helicopter Depot)
        // PrivatePlaneAI               GC  V   Aviation Club
        // RocketAI                     GC  V   ChirpX Launch Site (one rocket at a time)
        // ShipAI                               (base class with no buildings)
        //    CargoShipAI               GC  V   (unlimited:  Cargo Harbor, Cargo Hub)
        //    PassengerShipAI           GC  V   (unlimited:  Harbor)
        // TrainAI                      GC      (base class with no buildings)
        //    CargoTrainAI              GC  V   (unlimited:  Cargo Train Terminal, Cargo Hub, Cargo Airport Hub)
        //    PassengerTrainAI          GC  V   (unlimited:  All buildings with a passenger train or monorail station.)
        //       MetroTrainAI               V   (unlimited:  All buildings with a metro station.)
        // TramBaseAI                   GC      (base class with no buildings)
        //    TramAI                    GC  V   (unlimited:  Tram Depot)
        // VortexAI                             (TBD for tornado?)


        /// <summary>
        /// create a patch for every vehicle AI that has a GetColor method and that will be used by vehicle usage panels
        /// in the listings above, that is vehicle AIs marked with GC and V
        /// </summary>
        public static bool CreateGetColorPatches()
        {
            // patch each vehicle AI
            if (!CreateGetColorPatch<CargoPlaneAI                >()) return false;
            if (!CreateGetColorPatch<PassengerPlaneAI            >()) return false;
            if (!CreateGetColorPatch<BalloonAI                   >()) return false;
            if (!CreateGetColorPatch<PassengerBlimpAI            >()) return false;
            if (!CreateGetColorPatch<CableCarAI                  >()) return false;
            if (!CreateGetColorPatch<AmbulanceAI                 >()) return false;
            if (!CreateGetColorPatch<BusAI                       >()) return false;
            if (!CreateGetColorPatch<CargoTruckAI                >()) return false;
            if (!CreateGetColorPatch<DisasterResponseVehicleAI   >()) return false;
            if (!CreateGetColorPatch<FireTruckAI                 >()) return false;
            if (!CreateGetColorPatch<GarbageTruckAI              >()) return false;
            if (!CreateGetColorPatch<HearseAI                    >()) return false;
            if (!CreateGetColorPatch<MaintenanceTruckAI          >()) return false;
            if (!CreateGetColorPatch<ParkMaintenanceVehicleAI    >()) return false;
            if (!CreateGetColorPatch<PoliceCarAI                 >()) return false;
            if (!CreateGetColorPatch<PostVanAI                   >()) return false;
            if (!CreateGetColorPatch<SnowTruckAI                 >()) return false;
            if (!CreateGetColorPatch<TaxiAI                      >()) return false;
            if (!CreateGetColorPatch<TrolleybusAI                >()) return false;
            if (!CreateGetColorPatch<WaterTruckAI                >()) return false;
            if (!CreateGetColorPatch<FishingBoatAI               >()) return false;
            if (!CreateGetColorPatch<PassengerFerryAI            >()) return false;
            if (!CreateGetColorPatch<AmbulanceCopterAI           >()) return false;
            if (!CreateGetColorPatch<DisasterResponseCopterAI    >()) return false;
            if (!CreateGetColorPatch<FireCopterAI                >()) return false;
            if (!CreateGetColorPatch<PoliceCopterAI              >()) return false;
            if (!CreateGetColorPatch<PassengerHelicopterAI       >()) return false;
            if (!CreateGetColorPatch<PrivatePlaneAI              >()) return false;
            if (!CreateGetColorPatch<RocketAI                    >()) return false;
            if (!CreateGetColorPatch<CargoShipAI                 >()) return false;
            if (!CreateGetColorPatch<PassengerShipAI             >()) return false;
            if (!CreateGetColorPatch<CargoTrainAI                >()) return false;
            if (!CreateGetColorPatch<PassengerTrainAI            >()) return false;
            if (!CreateGetColorPatch<TramAI                      >()) return false;

            // success
            return true;
        }

        /// <summary>
        /// create a patch of the GetColor method for the specified vehicle AI type
        /// </summary>
        private static bool CreateGetColorPatch<T>() where T : VehicleAI
        {
            // same routine is used for all vehicle AI types
            return HarmonyPatcher.CreatePrefixPatchVehicleAI(typeof(T), "GetColor", typeof(VehicleAIPatch), "VehicleAIGetColor");
        }

        /// <summary>
        /// return the color of the vehicle
        /// </summary>
        /// <returns>whether or not to do base processing</returns>
        public static bool VehicleAIGetColor(ushort vehicleID, ref Vehicle data, InfoManager.InfoMode infoMode, ref Color __result)
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
                    case BuildingUsage.LevelsInfoViewTab.Visitors:
                    case BuildingUsage.LevelsInfoViewTab.Storage:
                        doBaseProcessing = false;
                        __result = InfoManager.instance.m_properties.m_neutralColor;
                        break;

                    case BuildingUsage.LevelsInfoViewTab.Vehicles:
                        doBaseProcessing = false;
                        __result = BuildingUsage.vehiclesUsagePanel.GetVehicleColor(vehicleID, ref data);
                        break;
                }
            }

            // return whether or not to do the base processing
            return doBaseProcessing;
        }
    }
}
