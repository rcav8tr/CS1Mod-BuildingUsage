using System.Reflection;

namespace BuildingUsage
{
    /// <summary>
    /// Harmony patching for District
    /// </summary>
    public class DistrictPatch
    {
        /// <summary>
        /// create patch for District.SimulationStep
        /// </summary>
        public static bool CreateSimulationStepPatch()
        {
            // patch with the prefix routine
            return HarmonyPatcher.CreatePrefixPatch(typeof(District), "SimulationStep", BindingFlags.Instance | BindingFlags.Public, typeof(DistrictPatch), "DistrictSimulationStep");
        }

        /// <summary>
        /// patch for District.SimulationStep
        /// </summary>
        /// <returns>whether or not to do base processing</returns>
        public static bool DistrictSimulationStep(byte districtID)
        {
            // get detail data by zone and level
            // need to get detail data right before normal processing because this is when the detail data is valid
            BuildingUsage.levelsDetailPanel.GetDetailData(districtID);

            // always do base processing
            return true;
        }
    }
}
