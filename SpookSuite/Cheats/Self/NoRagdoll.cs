using HarmonyLib;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class NoRagdoll : ToggleCheat
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "Ragdoll")]
        public static bool Ragdoll(Player __instance, ref bool __result)
        {
            if (Instance<NoRagdoll>().Enabled && !__instance.data.dead)
            {
                __result = false;
                return false;
            }              
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "NoControl")]
        public static bool NoControl(Player __instance, ref bool __result)
        {
            if (Settings.b_isMenuOpen)
            {
                __result = true;
                return false;
            }

            if (Instance<NoRagdoll>().Enabled && !__instance.data.dead)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
