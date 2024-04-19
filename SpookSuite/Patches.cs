using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using SpookSuite.Cheats;
using SpookSuite.Components;
using SpookSuite.Handler;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zorro.UI;

namespace SpookSuite
{
    [HarmonyPatch]
    internal static class Patches
    {
        public static List<byte> waitingForItemSpawn = new List<byte>();
        public static List<Guid> allowedBombs = new List<Guid>();

        internal static readonly object keyByteZero = (object)(byte)0;
        internal static readonly object keyByteOne = (object)(byte)1;
        internal static readonly object keyByteTwo = (object)(byte)2;
        internal static readonly object keyByteThree = (object)(byte)3;
        internal static readonly object keyByteFour = (object)(byte)4;
        internal static readonly object keyByteFive = (object)(byte)5;
        internal static readonly object keyByteSix = (object)(byte)6;
        internal static readonly object keyByteSeven = (object)(byte)7;
        internal static readonly object keyByteEight = (object)(byte)8;


        [HarmonyPostfix]
        [HarmonyPatch(typeof(ConnectionStateHandler), nameof(ConnectionStateHandler.Disconnect))]
        public static void Disconnect()
        {
            Log.Info("Disconnect Detected!");
            SpookPlayerHandler.ClearRPCHistory();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LoadBalancingClient), nameof(LoadBalancingClient.OpJoinOrCreateRoom))]
        public static void Connect()
        {
            Log.Error("Connection Detected!");
            NameSpoof.TrySetNickname();

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PhotonNetwork), "ExecuteRpc")]
        public static bool ExecuteRPC(Hashtable rpcData, Photon.Realtime.Player sender)
        {
            if (sender is null || sender.GamePlayer() is null) return true;

            string rpc = rpcData.ContainsKey(keyByteFive) ?
                PhotonNetwork.PhotonServerSettings.RpcList[(int)(byte)rpcData[keyByteFive]] :
                (string)rpcData[keyByteThree];


            if (!sender.IsLocal && sender.GamePlayer().Handle().IsRPCBlocked())
            {
                Debug.LogError($"RPC {rpc} was blocked from {sender.NickName}.");
                return false;
            }

            Log.Warning($"Received RPC '{rpc}' From '{sender.NickName}'");

            sender.GamePlayer().Handle().OnReceivedRPC(rpc, rpcData);


            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIPageHandler), "TransistionToPage", typeof(UIPage), typeof(PageTransistion))]
        public static void TransistionToPage(UIPage page, PageTransistion pageTransistion) => SpookPageUI.TryAttachToPageHandler();
    }
}
