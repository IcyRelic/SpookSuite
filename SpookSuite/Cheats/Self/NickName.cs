using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class NickName : ToggleCheat
    {
        public static string Value = SteamFriends.GetPersonaName();

        [HarmonyPrefix]
        [HarmonyPatch(typeof(LoadBalancingClient), nameof(LoadBalancingClient.OpJoinOrCreateRoom))]
        public static void Connect()
        {
            if (Instance<NickName>().Enabled)
                PhotonNetwork.LocalPlayer.NickName = Value;
        }
    }
}
