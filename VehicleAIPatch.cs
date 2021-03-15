using ColossalFramework;
using HarmonyLib;
using UnityEngine;
using System;
using System.Reflection;

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
        // BalloonAI                    GC  V   (unlimited:  Hot Air Balloon Tours)
        // BicycleAI                            (not generated from a service building)
        // BlimpAI                      GC      (base class with no buildings)
        //    PassengerBlimpAI          GC  V   (unlimited:  Blimp Depot)
        // CableCarBaseAI                       (base class with no buildings)
        //    CableCarAI                GC  V   (unlimited:  Cable Car Stop, End-of-Line Cable Car Stop)
        // CarAI                        GC      (base class with no buildings)
        //    AmbulanceAI               GC  V   Medical Clinic, Hospital, Medical Center (monument)
        //    BusAI                     GC  V   Small Emergency Shelter, Large Emergency Shelter,
        //                                      (unlimited:
        //                                          Bus Depot, Biofuel Bus Depot,
        //                                          Intercity Bus Station, Intercity Bus Terminal, 
        //                                          Sightseeing Bus Depots,
        //                                          Bus-Intercity Bus Hub, Metro-Intercity Bus Hub)
        //    CargoTruckAI              GC  V   All buildings for ExtractingFacilityAI, FishFarmingAI, FishFarmAI, ProcessingFacilityAI, UniqueFactoryAI, WarehouseAI.
        //                                      (unlimited:  zoned industrial, Cargo Train Terminal, Cargo Harbor, Cargo Hub, Cargo Airport, Cargo Airport Hub)
        //    DisasterResponseVehicleAI GC  V   Disaster Response Unit
        //    FireTruckAI               GC  V   Fire House, Fire Station
        //    GarbageTruckAI            GC  V   Landfill Site, Incineration Plant, Recycling Center, Ultimate Recycling Plant (monument)
        //    HearseAI                  GC  V   Cemetery, Crematorium, Cryopreservatory (CCP)
        //    MaintenanceTruckAI        GC  V   Road Maintenance Depot
        //    ParkMaintenanceVehicleAI  GC  V   Park Maintenance Building
        //    PassengerCarAI            GC      (not generated from a service building)
        //    PoliceCarAI               GC  V   Police Station, Police Headquarters, Prison
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
        // HelicopterDanglingAI         GC      (TBD what is this?)
        // MeteorAI                             (for meteor strike)
        // PassengerHelicopterAI        GC  V   (unlimited:  Helicopter Depot)
        // PrivatePlaneAI               GC  V   Aviation Club
        // RocketAI                     GC  V   ChirpX Launch Site (one rocket at a time)
        // ShipAI                               (base class with no buildings)
        //    CargoShipAI               GC  V   (unlimited:  Cargo Harbor, Cargo Hub)
        //    PassengerShipAI           GC  V   (unlimited:  Harbor)
        // TrainAI                      GC      (base class with no buildings)
        //    CargoTrainAI              GC  V   (unlimited:  Cargo Train Terminal, Cargo Hub, Cargo Airport Hub)
        //    PassengerTrainAI          GC  V   (unlimited:  Train Station, Multiplatform End Station, Multiplatform Train Station,
        //                                                   Monorail Station, Monorail Station with Road, Monorail-Bus Hub, Metro-Monorail-Train Hub,
        //                                                   Train-Metro Hub)
        //       MetroTrainAI               V   (unlimited:  Metro Station, Elevated Metro Station, Underground Metro Station,
        //                                                   Bus-Metro Hub, Metro-Intercity Bus Hub, Train-Metro Hub, International Airport, Metro-Monorail-Train Hub)
        // TramBaseAI                   GC      (base class with no buildings)
        //    TramAI                    GC  V   (unlimited:  Tram Depot)
        // VortexAI                             (TBD for tornado?)


        /// <summary>
        /// create a patch for every vehicle AI that has a GetColor method and that will be used by vehicle usage panels
        /// in the listings above, that is vehicle AIs marked with GC and V
        /// </summary>
        public static void CreateGetColorPatches()
        {
            CreateGetColorPatch<CargoPlaneAI                >();
            CreateGetColorPatch<PassengerPlaneAI            >();
            CreateGetColorPatch<BalloonAI                   >();
            CreateGetColorPatch<PassengerBlimpAI            >();
            CreateGetColorPatch<CableCarAI                  >();
            CreateGetColorPatch<AmbulanceAI                 >();
            CreateGetColorPatch<BusAI                       >();
            CreateGetColorPatch<CargoTruckAI                >();
            CreateGetColorPatch<DisasterResponseVehicleAI   >();
            CreateGetColorPatch<FireTruckAI                 >();
            CreateGetColorPatch<GarbageTruckAI              >();
            CreateGetColorPatch<HearseAI                    >();
            CreateGetColorPatch<MaintenanceTruckAI          >();
            CreateGetColorPatch<ParkMaintenanceVehicleAI    >();
            CreateGetColorPatch<PoliceCarAI                 >();
            CreateGetColorPatch<PostVanAI                   >();
            CreateGetColorPatch<SnowTruckAI                 >();
            CreateGetColorPatch<TaxiAI                      >();
            CreateGetColorPatch<TrolleybusAI                >();
            CreateGetColorPatch<WaterTruckAI                >();
            CreateGetColorPatch<FishingBoatAI               >();
            CreateGetColorPatch<PassengerFerryAI            >();
            CreateGetColorPatch<AmbulanceCopterAI           >();
            CreateGetColorPatch<DisasterResponseCopterAI    >();
            CreateGetColorPatch<FireCopterAI                >();
            CreateGetColorPatch<PoliceCopterAI              >();
            CreateGetColorPatch<PassengerHelicopterAI       >();
            CreateGetColorPatch<PrivatePlaneAI              >();
            CreateGetColorPatch<RocketAI                    >();
            CreateGetColorPatch<CargoShipAI                 >();
            CreateGetColorPatch<PassengerShipAI             >();
            CreateGetColorPatch<CargoTrainAI                >();
            CreateGetColorPatch<PassengerTrainAI            >();
            CreateGetColorPatch<TramAI                      >();
        }

        /// <summary>
        /// create a patch of the GetColor method for the specified vehicle AI type
        /// </summary>
        /// <remarks>
        /// Cannot use HarmonyPatch attribute because all the specific vehicle AI classes have two GetColor routines:
        /// There is a GetColor routine in the derived AI classes which has Vehicle as a parameter.
        /// There is a GetColor routine in the base class VehicleAI which has VehicleParked as a parameter.
        /// Furthermore, MakeByRefType cannot be specified in the HarmonyPatch attribute (or any attribute) to allow the patch to be created automatically.
        /// This routine manually finds the GetColor routine with Vehicle as a ref type parameter and creates the patch for it.
        /// </remarks>
        private static void CreateGetColorPatch<T>() where T : VehicleAI
        {
            // get the original GetColor method that takes ref Vehicle parameter
            MethodInfo original = typeof(T).GetMethod("GetColor", new Type[] { typeof(ushort), typeof(Vehicle).MakeByRefType(), typeof(InfoManager.InfoMode) });
            if (original == null)
            {
                Debug.LogError($"Unable to find GetColor method for vehicle AI type {typeof(T).Name}.");
                return;
            }

            // find the Prefix method
            MethodInfo prefix = typeof(VehicleAIPatch).GetMethod("Prefix", BindingFlags.Static | BindingFlags.Public);
            if (prefix == null)
            {
                Debug.LogError($"Unable to find VehicleAIPatch.Prefix method.");
                return;
            }

            // create the patch
            BuildingUsage.harmony.Patch(original, new HarmonyMethod(prefix), null, null);
        }

        /// <summary>
        /// return the color of the vehicle
        /// same Prefix routine is used for all vehicle AI types
        /// </summary>
        /// <returns>whether or not to do base processing</returns>
        public static bool Prefix(ushort vehicleID, ref Vehicle data, InfoManager.InfoMode infoMode, ref Color __result)
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
                        __result = Singleton<InfoManager>.instance.m_properties.m_neutralColor;
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
