using HarmonyLib;

namespace SpookSuite
{
    [HarmonyPatch]
    internal class Patches
    {
        public class Player_Patches
        {
            [HarmonyPostfix]
            [HarmonyPatch(typeof(Player), "TakeDamage")]
            public static bool TakeDamage(Player __instance)
            {
                if (Cheats.Godmode.Enabled)
                    return false;
                return true;
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(Player), "Die")]
            public static bool Die(Player __instance)
            {
                if (Cheats.Godmode.Enabled)
                    return false;
                return true;
            }
        }
    }
}
