using HarmonyLib;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class Godmode : ToggleCheat
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "TakeDamage")]
        public static bool TakeDamage(Player __instance)
        {
            if (Instance.Enabled)
                return false;
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "Die")]
        public static bool Die(Player __instance)
        {
            if (Instance.Enabled)
                return false;
            return true;
        }
    }
}
