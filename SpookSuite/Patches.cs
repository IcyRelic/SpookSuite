using HarmonyLib;
using Photon.Pun;
using SpookSuite.Menu.Tab;
using SpookSuite.Util;

namespace SpookSuite
{
    [HarmonyPatch]
    internal static class Patches
    {
        [HarmonyPatch]
        public static class PlayerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(typeof(PhotonNetwork), "LocalPlayer", MethodType.Getter)]
            static bool LocalPlayer(ref Photon.Realtime.Player __result)
            {
                if (GameUtil.IsOverridingPhotonLocalPlayer())
                {
                    __result = PlayersTab.selectedPlayer.refs.view.Owner;
                    return false;
                }
                return true;
            }
        }
    }
}
