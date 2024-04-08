using HarmonyLib;
using SpookSuite.Cheats.Core;
using UnityEngine;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class Godmode : ToggleCheat
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "TakeDamage")]
        public static bool TakeDamage(Player __instance)
        {
            if (Instance<Godmode>().Enabled)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "Die")]
        public static bool Die(Player __instance)
        {
            if (Instance<Godmode>().Enabled)
                return false;
            return true;
        }
    }
}
