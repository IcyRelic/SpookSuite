using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Realtime;
using SpookSuite.Cheats.Core;
using SpookSuite.Cheats;
using SpookSuite.Util;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SpookSuite.Manager
{
    [HarmonyPatch]
    internal class LobbyManager
    {
        public static CSteamID lastLobby = new CSteamID();
        public static List<CSteamID> lastPlayers = new List<CSteamID>();
        public static CSteamID currentLobby = new CSteamID();
        public static bool wasKickedFromPrevious = false;

        public static void JoinLastLobby() => JoinLobby(lastLobby);

        public static void JoinLobby(CSteamID steamID) => MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("JoinLobby", steamID);

        [HarmonyPrefix]
        [HarmonyPatch(typeof(LoadBalancingClient), "OnEvent")]
        public static bool OnEvent(LoadBalancingClient __instance, EventData photonEvent)
        {
            //if (photonEvent.Code != 203 || photonEvent.Code != 201)
            //    Log.Warning($"Received Event!\n\n Code: {photonEvent.Code},\n DataKey: {photonEvent.CustomDataKey},\n Params: {photonEvent.Parameters},\n Sender Key: {photonEvent.SenderKey} \n\n");
            if (photonEvent.Code == 21 && Cheat.Instance<AntiKick>().Enabled)
            {
                Log.Warning("Host tried kicking you!");
                wasKickedFromPrevious = true;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PhotonPeer), "Disconnect")]
        public static bool Disconnect(PhotonPeer __instance)
        {
            if (Cheat.Instance<AntiKick>().Enabled)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PhotonGameLobbyHandler), "OnDisconnected")]
        public static bool OnDisconnected(PhotonGameLobbyHandler __instance, DisconnectCause cause)
        {
            if (Cheat.Instance<AntiKick>().Enabled && cause == DisconnectCause.DisconnectByServerLogic && wasKickedFromPrevious)
            {
                wasKickedFromPrevious = false;
                SpawnHandler.Instance.Reflect().SetValue("m_Spawned", false);
                JoinLastLobby();

                return false;
            }
            return true;
        }
    }
}
