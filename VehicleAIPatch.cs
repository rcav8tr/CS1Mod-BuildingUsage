using ColossalFramework;
using Harmony;
using UnityEngine;
using System;
using System.Collections.Generic;
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

        // All AIs below derive from VehicleAI.

        // AircraftAI                   (base class with no buildings)
        //    CargoPlaneAI              (unlimited:  Cargo Airport, Cargo Airport Hub)
        //    PassengerPlaneAI          (unlimited:  Airport, International Airport)
        // BalloonAI                    (unlimited:  Hot Air Balloon Tours)
        // BicycleAI                    (not generated from a service building)
        // BlimpAI                      (base class with no buildings)
        //    PassengerBlimpAI          (unlimited:  Blimp Depot)
        // CableCarBaseAI               (base class with no buildings)
        //    CableCarAI                (unlimited:  Cable Car Stop, End-of-Line Cable Car Stop)
        // CarAI                        (base class with no buildings)
        //    AmbulanceAI               Medical Clinic, Hospital, Medical Center (monument)
        //    BusAI                     Small Emergency Shelter, Large Emergency Shelter,
        //                              (unlimited:
        //                                  Bus Depot, Biofuel Bus Depot,
        //                                  Intercity Bus Station, Intercity Bus Terminal, 
        //                                  Sightseeing Bus Depots,
        //                                  Bus-Intercity Bus Hub, Metro-Intercity Bus Hub)
        //    CargoTruckAI              All buildings for ExtractingFacilityAI, FishFarmingAI, FishFarmAI, ProcessingFacilityAI, UniqueFactoryAI, WarehouseAI.
        //                              (unlimited:  zoned industrial, Cargo Train Terminal, Cargo Harbor, Cargo Hub, Cargo Airport, Cargo Airport Hub)
        //    DisasterResponseVehicleAI Disaster Response Unit
        //    FireTruckAI               Fire House, Fire Station
        //    GarbageTruckAI            Landfill Site, Incineration Plant, Recycling Center, Ultimate Recycling Plant (monument)
        //    HearseAI                  Cemetery, Crematorium, Cryopreservatory (CCP)
        //    MaintenanceTruckAI        Road Maintenance Depot
        //    ParkMaintenanceVehicleAI  Park Maintenance Building
        //    PassengerCarAI            (not generated from a service building)
        //    PoliceCarAI               Police Station, Police Headquarters, Prison
        //    PostVanAI                 Post Office, Post Sorting Facility
        //    SnowTruckAI               Snow Dump
        //    TaxiAI                    Taxi Depot
        //    TrolleybusAI              Trolleybus Depot
        //    WaterTruckAI              Pumping Service
        // CarTrailerAI                 (the trailer for:  some CargoTruckAI, maybe others)
        // FerryAI                      (base class with no buildings)
        //    FishingBoatAI             Fishing Harbor, Anchovy Fishing Harbor, Salmon Fishing Harbor, Shellfish Fishing Harbor, Tuna Fishing Harbor
        //    PassengerFerryAI          (unlimited:  Ferry Depot)
        // HelicopterAI                 (base class with no buildings)
        //    AmbulanceCopterAI         Medical Helicopter Depot
        //    DisasterResponseCopterAI  Disaster Response Unit
        //    FireCopterAI              Fire Helicopter Depot
        //    PoliceCopterAI            Police Helicopter Depot
        // HelicopterDanglingAI         (TBD what is this?)
        // MeteorAI                     (for meteor strike)
        // PassengerHelicopterAI        (unlimited:  Helicopter Depot)
        // PrivatePlaneAI               Aviation Club
        // RocketAI                     ChirpX Launch Site (one rocket at a time)
        // ShipAI                       (base class with no buildings)
        //    CargoShipAI               (unlimited:  Cargo Harbor, Cargo Hub)
        //    PassengerShipAI           (unlimited:  Harbor)
        // TrainAI                      (base class with no buildings)
        //    CargoTrainAI              (unlimited:  Cargo Train Terminal, Cargo Hub, Cargo Airport Hub)
        //    PassengerTrainAI          (unlimited:  Train Station, Multiplatform End Station, Multiplatform Train Station,
        //                                           Monorail Station, Monorail Station with Road, Monorail-Bus Hub, Metro-Monorail-Train Hub,
        //                                           Train-Metro Hub)
        //       MetroTrainAI           (unlimited:  Metro Station, Elevated Metro Station, Underground Metro Station,
        //                                           Bus-Metro Hub, Metro-Intercity Bus Hub, Train-Metro Hub, International Airport, Metro-Monorail-Train Hub)
        // TramBaseAI                   (base class with no buildings)
        //    TramAI                    (unlimited:  Tram Depot)
        // VortexAI                     (TBD for tornado?)


        // vehicle AI types that have been patched in this mod
        private static List<Type> _vehicleAITypes = new List<Type>();

        /// <summary>
        /// create a patch of the GetColor method for the specified vehicle AI type
        /// </summary>
        /// <remarks>
        /// Cannot use HarmonyPatch attribute because all the specific vehicle AI classes have two GetColor routines:
        /// There is a GetColor routine in the derived AI classes which has Vehicle as a parameter.
        /// There is a GetColor routine in the base clase VehicleAI which has VehicleParked as a parameter.
        /// Furthermore, MakeByRefType cannot be specified in the HarmonyPatch attribute (or any attribute) to allow the patch to be created automatically.
        /// This routine manually finds the GetColor routine with Vehicle as a ref type parameter and creates the patch for it.
        /// </remarks>
        public static void CreateGetColorPatch<T>() where T : VehicleAI
        {
            // each vehicle AI type is patched only once
            // the patch logic determines which panel is displayed and calls GetVehicleColor
            // this method avoids creating a separate patch for each panel that has the same vehicle AI
            Type vehicleAIType = typeof(T);
            if (_vehicleAITypes.Contains(vehicleAIType))
            {
                return;
            }

            // get the original GetColor method that takes ref Vehicle parameter
            MethodInfo original = vehicleAIType.GetMethod("GetColor", new Type[] { typeof(ushort), typeof(Vehicle).MakeByRefType(), typeof(InfoManager.InfoMode) });
            if (original == null)
            {
                Debug.LogError($"Unable to find GetColor method for vehicle AI type {vehicleAIType.ToString()}.");
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

            // a patch was created for this vehicle AI type
            _vehicleAITypes.Add(vehicleAIType);
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

        /// <summary>
        /// clear the vehicle AI patches
        /// </summary>
        public static void ClearPatches()
        {
            _vehicleAITypes.Clear();
        }

    }
}
