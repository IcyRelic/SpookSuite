using HarmonyLib;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class NoPushToTalk : ToggleCheat
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(VoiceChatModeSetting), "CanTalk")]
        public static bool CanTalk(VoiceChatModeSetting __instance, ref bool __result)
        {
            if (Cheat.Instance<NoPushToTalk>().Enabled)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
}
