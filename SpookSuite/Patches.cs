using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Components;
using SpookSuite.Handler;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core.Compression;
using Zorro.Core.Serizalization;
using Zorro.Core;
using Zorro.UI;
using Steamworks;

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

        private static float rot = 1;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerSyncer), "OnPhotonSerializeView")] //the only reason this patch is like this is incase we want to add any other features that require spoofing
        public static bool OnPhotonSerializeView(PlayerSyncer __instance, ref PhotonStream stream, ref PhotonMessageInfo info)
        {
            if (!__instance.Reflect().GetValue<Player>("player").IsLocal || !MainMenuHandler.SteamLobbyHandler.Reflect().GetValue<CSteamID>("m_CurrentLobby").IsValid())
                return true;

            if (stream.IsWriting)
            {
                rot += Spinbot.Value;
                if(rot > 360)
                    rot = rot - 360;

                Vector3 pos = new Vector3();

                if (Cheat.Instance<NoClip>().Enabled && !Cheat.Instance<Invisibility>().Enabled)
                    pos = Player.localPlayer.refs.cameraPos.position;
                else if (Cheat.Instance<Invisibility>().Enabled)
                    pos = new Vector3(1000, 1000, 1000);
                else
                    pos = Player.localPlayer.Reflect().Invoke<Vector3>("GetRelativePosition_Rig", false, BodypartType.Hip, Vector3.zero);

                PlayerSyncerBoolFlag playerSyncerBoolFlag = PlayerSyncerBoolFlag.NONE;
                if (Player.localPlayer.data.isSprinting)
                    playerSyncerBoolFlag |= PlayerSyncerBoolFlag.SPRINT;
                if (Player.localPlayer.data.isCrouching)
                    playerSyncerBoolFlag |= PlayerSyncerBoolFlag.CROUCH;
                if (Player.localPlayer.input.movementInput.x > 0.01f)
                    playerSyncerBoolFlag |= PlayerSyncerBoolFlag.WALK_RIGHT;
                if (Player.localPlayer.input.movementInput.x < -0.01f)
                    playerSyncerBoolFlag |= PlayerSyncerBoolFlag.WALK_LEFT;
                if (Player.localPlayer.input.movementInput.y > 0.01f)
                    playerSyncerBoolFlag |= PlayerSyncerBoolFlag.WALK_FORWARD;
                if (Player.localPlayer.input.movementInput.y < -0.01f)
                    playerSyncerBoolFlag |= PlayerSyncerBoolFlag.WALK_BACKWARD;
                if (Player.localPlayer.input.aimIsPressed)
                    playerSyncerBoolFlag |= PlayerSyncerBoolFlag.AIMING;

                BinarySerializer binarySerializer = new BinarySerializer(20, Allocator.Temp);
                Vector2 playerLookValues = Cheat.Instance<Spinbot>().Enabled ? new Vector2(rot, Player.localPlayer.data.playerLookValues.y) : Player.localPlayer.data.playerLookValues;

                binarySerializer.WriteHalf(Cheat.Instance<NoClip>().Enabled ? (half)0 : (half)Player.localPlayer.data.sinceGrounded);
                binarySerializer.WriteHalf((half)pos.x);
                binarySerializer.WriteHalf((half)pos.y);
                binarySerializer.WriteHalf((half)pos.z);
                binarySerializer.WriteHalf((half)playerLookValues.x); //target look
                binarySerializer.WriteHalf((half)playerLookValues.y);
                binarySerializer.WriteByte(FloatCompression.CompressZeroOne(Player.localPlayer.data.microphoneValue)); //earrape?
                binarySerializer.WriteByte((byte)playerSyncerBoolFlag);
                binarySerializer.WriteByte((Player.localPlayer.data.selectedItemSlot == -1) ? byte.MaxValue : ((byte)Player.localPlayer.data.selectedItemSlot));
                
                byte[] obj = binarySerializer.buffer.ToByteArray();
                stream.SendNext(obj);
                binarySerializer.Dispose();
                return false; //this just does return normally
            }
            //return true; //dont do anything else for now
            __instance.Reflect().SetValue("hasReceived", true);

            if (Player.localPlayer is null || Player.localPlayer.data is null)
                return false;

            __instance.Reflect().SetValue("sinceLastPackage", 0f);
            __instance.Reflect().SetValue("lastPos", __instance.Reflect().GetValue<Vector3>("targetPos"));
            BinaryDeserializer binaryDeserializer = new BinaryDeserializer((byte[])stream.ReceiveNext(), Allocator.Temp);
            Player.localPlayer.data.sinceGrounded = binaryDeserializer.ReadHalf();
            half d = binaryDeserializer.ReadHalf();
            half d2 = binaryDeserializer.ReadHalf();
            half d3 = binaryDeserializer.ReadHalf();
            __instance.Reflect().SetValue("targetPos", new Vector3(d, d2, d3));
            if (__instance.Reflect().GetValue<bool>("first"))
            {
                __instance.Reflect().SetValue("first", false);
                __instance.Reflect().SetValue("lastPos", __instance.Reflect().GetValue<Vector3>("targetPos"));
            }

            half d4 = binaryDeserializer.ReadHalf(); //playerlookvalues
            half d5 = binaryDeserializer.ReadHalf();
            __instance.Reflect().SetValue("targetLook", new Vector2(d4, d5));

            __instance.Reflect().SetValue("lookDistance", Vector2.Distance(new Vector2(rot, Player.localPlayer.data.playerLookValues.y), __instance.Reflect().GetValue<Vector2>("targetLook")));

            Player.localPlayer.data.microphoneValue = FloatCompression.DecompressZeroOne(binaryDeserializer.ReadByte());
            PlayerSyncerBoolFlag lhs = (PlayerSyncerBoolFlag)binaryDeserializer.ReadByte();
            byte b = binaryDeserializer.ReadByte();
            if (b == 255)
                Player.localPlayer.data.selectedItemSlot = -1;
            else
                Player.localPlayer.data.selectedItemSlot = (int)b;

            Player.localPlayer.data.isSprinting = lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.SPRINT);
            Player.localPlayer.data.isCrouching = lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.CROUCH);
            Vector2 zero = Vector2.zero;
            if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.WALK_RIGHT))
                zero.x += 1f;
            if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.WALK_LEFT))
                zero.x -= 1f;
            if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.WALK_FORWARD))
                zero.y += 1f;
            if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.WALK_BACKWARD))
                zero.y -= 1f;
            if (lhs.HasFlagUnsafe(PlayerSyncerBoolFlag.AIMING))
                Player.localPlayer.input.aimIsPressed = true;
            else
                Player.localPlayer.input.aimIsPressed = false;

            Player.localPlayer.input.movementInput = zero;
            binaryDeserializer.Dispose();
            return false;
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
