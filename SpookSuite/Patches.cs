using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpookSuite
{
    [HarmonyPatch]
    internal static class Patches
    {
        public static List<byte> waitingForItemSpawn = new List<byte>();
        public static List<Guid> allowedBombs = new List<Guid>();

        private static readonly object keyByteZero = (object)(byte)0;
        private static readonly object keyByteOne = (object)(byte)1;
        private static readonly object keyByteTwo = (object)(byte)2;
        private static readonly object keyByteThree = (object)(byte)3;
        private static readonly object keyByteFour = (object)(byte)4;
        private static readonly object keyByteFive = (object)(byte)5;
        private static readonly object keyByteSix = (object)(byte)6;
        private static readonly object keyByteSeven = (object)(byte)7;
        private static readonly object keyByteEight = (object)(byte)8;


        [HarmonyPostfix]
        [HarmonyPatch(typeof(ConnectionStateHandler), nameof(ConnectionStateHandler.Disconnect))]
        public static void Disconnect()
        {
            Debug.Log("[SpookSuite] Disconnect Detected!");
            SpookPlayerHandler.ClearRPCHistory();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Pickup), "RPC_ConfigurePickup")]
        public static void InteractWithPickups(Pickup __instance, byte itemID, byte[] data)
        {
            if(itemID == GameUtil.GetItemByName("bomb").id)
            {
                Attacks_Bombs bombsEnemy = Object.FindAnyObjectByType<Attacks_Bombs>();

                if((GameObjectManager.divingBell is not null && GameObjectManager.divingBell.onSurface) || bombsEnemy is null)
                {
                    if (allowedBombs.Contains(__instance.itemInstance.m_guid.Value))
                    {
                        //SpookSuite Bomb
                        allowedBombs.Remove(__instance.itemInstance.m_guid.Value);
                        return;
                    }
                    //maybe add a check later to see if attack bombs has attacked
                    Debug.Log("Bombing Detected, Removing...");

                    __instance.m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient);
                }
                
            }

            if(!waitingForItemSpawn.Contains(itemID)) return;

            __instance.Interact(Player.localPlayer);
            waitingForItemSpawn.Remove(itemID);
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(PhotonNetwork), "ExecuteRpc")]
        public static bool ExecuteRPC(Hashtable rpcData, Photon.Realtime.Player sender)
        {
            string rpc = rpcData.ContainsKey(keyByteFive) ?
                PhotonNetwork.PhotonServerSettings.RpcList[(int)(byte)rpcData[keyByteFive]] :
                (string)rpcData[keyByteThree];


            if (!sender.IsLocal && sender.GamePlayer().Handle().IsRPCBlocked())
            {
                Debug.LogError($"RPC {rpc} was blocked from {sender.NickName}.");
                return false;
            }

            Debug.LogWarning($"[SpookSuite] Received RPC '{rpc}' From '{sender.NickName}'");
            sender.GamePlayer().Handle().OnReceivedRPC(rpc, rpcData);

            return true;
            //Debug.LogWarning($"[SpookSuite] Received RPC '{rpc}' From '{sender.NickName}'");
        }

 


    }
}
