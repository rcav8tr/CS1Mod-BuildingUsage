using UnityEngine;
using CitiesHarmony.API;
using HarmonyLib;
using System.Reflection;
using System;

namespace BuildingUsage
{
    /// <summary>
    /// Harmony patching
    /// </summary>
    internal class HarmonyPatcher
    {
        private const string HarmonyId = "com.github.rcav8tr.BuildingUsage";

        /// <summary>
        /// create Harmony patches
        /// </summary>
        public static bool CreatePatches()
        {
            try
            {
                // check Harmony
                if (!HarmonyHelper.IsHarmonyInstalled)
                {
                    ColossalFramework.UI.UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("Missing Dependency",
                        $"The Bulding Usage mod requires the 'Harmony (Mod Dependency)' mod.  \n\nPlease subscribe to the 'Harmony (Mod Dependency)' mod and restart the game.", error: false);
                    return false;
                }

                // create the patches
                if (!BuildingAIPatch.CreateGetColorPatches()) return false;
                if (!VehicleAIPatch.CreateGetColorPatches()) return false;
                if (!LevelsInfoViewPanelPatch.CreateUpdatePanelPatch()) return false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }

            // success
            return true;
        }

        /// <summary>
        /// create a prefix patch (i.e. called before the base processing)
        /// </summary>
        /// <typeparam name="T">type to be patched</typeparam>
        /// <param name="originalMethodName">name of the method to be patched</param>
        /// <param name="bindingFlags">bindings flags of the method to be patched</param>
        /// <param name="prefixType">type that contains the prefix method</param>
        /// <param name="prefixMethodName">name of the prefix method</param>
        /// <returns>success status</returns>
        public static bool CreatePrefixPatch<T>(string originalMethodName, BindingFlags bindingFlags, Type prefixType, string prefixMethodName)
        {
            // get the original method
            MethodInfo originalMethod = typeof(T).GetMethod(originalMethodName, bindingFlags);
            if (originalMethod == null)
            {
                Debug.LogError($"Unable to find original method {typeof(T).Name}.{originalMethodName}.");
                return false;
            }

            // finish creating the patch
            return CreatePrefixPatch(originalMethod, prefixType, prefixMethodName);
        }

        /// <summary>
        /// create a prefix patch for a vehicle AI (i.e. called before the base processing)
        /// </summary>
        /// <typeparam name="T">type to be patched</typeparam>
        /// <param name="originalMethodName">name of the method to be patched</param>
        /// <param name="prefixType">type that contains the prefix method</param>
        /// <param name="prefixMethodName">name of the prefix method</param>
        /// <returns>success status</returns>
        public static bool CreatePrefixPatchVehicleAI<T>(string originalMethodName, Type prefixType, string prefixMethodName)
        {
            // get the original vehicle AI method
            // There is a GetColor routine in the derived AI classes which has Vehicle as a ref parameter.
            // There is a GetColor routine in the base class VehicleAI which has VehicleParked as a ref parameter.
            // The GetColor in the derived class is the one to be patched, so need to pass type Vehicle as a ref parameter.
            MethodInfo originalMethod = typeof(T).GetMethod(originalMethodName, new Type[] { typeof(ushort), typeof(Vehicle).MakeByRefType(), typeof(InfoManager.InfoMode) });
            if (originalMethod == null)
            {
                Debug.LogError($"Unable to find original method {typeof(T).Name}.{originalMethodName}.");
                return false;
            }

            // finish creating the patch
            return CreatePrefixPatch(originalMethod, prefixType, prefixMethodName);
        }

        /// <summary>
        /// create a prefix patch (i.e. called before the base processing)
        /// </summary>
        /// <param name="originalMethod">the original method to be patched</param>
        /// <param name="prefixType">type that contains the prefix method</param>
        /// <param name="prefixMethodName">name of the prefix method</param>
        /// <returns>success status</returns>
        private static bool CreatePrefixPatch(MethodInfo originalMethod, Type prefixType, string prefixMethodName)
        {
            // get the prefix method
            MethodInfo prefixMethod = prefixType.GetMethod(prefixMethodName, BindingFlags.Static | BindingFlags.Public);
            if (prefixMethod == null)
            {
                Debug.LogError($"Unable to find patch prefix method {prefixType.Name}.{prefixMethodName}.");
                return false;
            }

            // create the patch
            new Harmony(HarmonyId).Patch(originalMethod, new HarmonyMethod(prefixMethod), null);

            // success
            return true;
        }

        /// <summary>
        /// remove Harmony patches
        /// </summary>
        public static void RemovePatches()
        {
            try
            {
                if (HarmonyHelper.IsHarmonyInstalled)
                {
                    new Harmony(HarmonyId).UnpatchAll(HarmonyId);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
