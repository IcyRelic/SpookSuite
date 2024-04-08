using HarmonyLib;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class NoRagdoll : ToggleCheat
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "Ragdoll")]
        public static bool Ragdoll(Player __instance)
        {
            if (Instance<NoRagdoll>().Enabled)
                return false;
            return true;
        }
    }
}
