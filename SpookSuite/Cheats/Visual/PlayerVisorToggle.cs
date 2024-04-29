using SpookSuite.Cheats.Core;
using HarmonyLib;
using SpookSuite.Util;
using UnityEngine;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class PlayerVisorToggle : ToggleCheat
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(HelmetUIToggler), "Update")]
        private static bool Update(HelmetUIToggler __instance)
        {
            if (Cheat.Instance<PlayerVisorToggle>().Enabled)
            {
                __instance.Reflect().GetValue<Canvas>("canvas").enabled = false;
                return false;
            }
            return true;
        }
    }
}
