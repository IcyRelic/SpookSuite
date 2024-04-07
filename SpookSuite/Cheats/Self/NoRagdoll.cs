using HarmonyLib;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class NoRagdoll : ToggleCheat
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "Ragdoll")]
        public static bool TakeDamage(Player __instance)
        {
            if (Instance.Enabled)
                return false;
            return true;
        }
    }
}
