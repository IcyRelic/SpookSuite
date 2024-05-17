using HarmonyLib;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class AntiKick : ToggleCheat
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameHandler), "OnKickNotifactionReceived")]
        public static bool OnKickNotifactionReceived(GameHandler __instance, KickPlayerNotificationPackage obj)
        {
            if (Cheat.Instance<AntiKick>().Enabled)
                return false;
            return true;
        }
    }
}
