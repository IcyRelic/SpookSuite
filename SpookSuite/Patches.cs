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
using SpookSuite.Menu.Tab;
using Steamworks;

using Object = UnityEngine.Object;
using System.Net;
namespace SpookSuite
{
    [HarmonyPatch]
    internal static class Patches
    {
        public static List<byte> waitingForItemSpawn = new List<byte>();
        public static List<Guid> allowedBombs = new List<Guid>();
        public static bool SpawnBigSlap = false;

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
        [HarmonyPatch(typeof(SteamLobbyHandler), "JoinRandom")]
        public static bool JoinRandom(SteamLobbyHandler __instance)
        {
            if (!Cheat.Instance<JoinWithPlugins>().Enabled)
                return true;

            if (__instance.Reflect().GetValue<bool>("m_isJoining") || __instance.Reflect().GetValue<bool>("m_Joined"))
            {
                Debug.Log("Already Joining");
                return true;
            }
            __instance.Reflect().SetValue("m_isJoining", true);
            string pchValueToMatch = new BuildVersion(Application.version).ToMatchmaking();
            SteamMatchmaking.AddRequestLobbyListStringFilter("ContentWarningVersion", pchValueToMatch, ELobbyComparison.k_ELobbyComparisonEqual);
            SteamMatchmaking.AddRequestLobbyListResultCountFilter(10);
            SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();

            __instance.Reflect().GetValue<CallResult<LobbyMatchList_t>>("m_OnMatchListCallResult").Set(hAPICall, null);
            return false;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamLobbyHandler), "OnMatchListReceived")]
        public static bool OnMatchListReceived(SteamLobbyHandler __instance, LobbyMatchList_t param, bool biofailure)
        {
            return true;

            if (biofailure)
            {
                Debug.LogError("Matchlist Biofail");
                return true;
            }
            if (param.m_nLobbiesMatching == 0U)
            {
                Debug.LogError("Found No Matches hosting Retrying 1");
                MainMenuHandler.SteamLobbyHandler.Reflect().SetValue("m_Joining", false);
                MainMenuHandler.SteamLobbyHandler.JoinRandom();
                return false;
            }
            List<ValueTuple<CSteamID, int>> list = new List<ValueTuple<CSteamID, int>>();
            int num = 0;
            while ((long)num < (long)((ulong)param.m_nLobbiesMatching))
            {
                CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(num);
                string lobbyData = SteamMatchmaking.GetLobbyData(lobbyByIndex, "ContentWarningVersion");
                string lobbyData2 = SteamMatchmaking.GetLobbyData(lobbyByIndex, "PhotonRegion");
                int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(lobbyByIndex);
                int lobbyMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(lobbyByIndex);
                string[] array = new string[10];
                array[0] = "Checking Lobby: ";
                int num2 = 1;
                CSteamID csteamID = lobbyByIndex;
                array[num2] = csteamID.ToString();
                array[2] = " GameVersion: ";
                array[3] = lobbyData;
                array[4] = "Region: ";
                array[5] = lobbyData2;
                array[6] = "Number of players: ";
                array[7] = numLobbyMembers.ToString();
                array[8] = " / ";
                array[9] = lobbyMemberLimit.ToString();
                VerboseDebug.Log(string.Concat(array));
                for (int i = 0; i < numLobbyMembers; i++)
                {
                    string str = "Checking Steam User In Lobby ";
                    csteamID = SteamMatchmaking.GetLobbyMemberByIndex(lobbyByIndex, i);
                    VerboseDebug.Log(str + csteamID.m_SteamID.ToString());
                }
                string cloudRegion = PhotonNetwork.CloudRegion;
                bool flag = !string.IsNullOrEmpty(lobbyData2) && lobbyData2 == cloudRegion;
                if (lobbyData == new BuildVersion(Application.version).ToMatchmaking() && flag && numLobbyMembers < 4)
                {
                    list.Add(new ValueTuple<CSteamID, int>(lobbyByIndex, num));
                }
                num++;
            }
            Debug.Log("Received SteamLobby Matchlist: " + param.m_nLobbiesMatching.ToString() + " Matching: " + list.Count.ToString());
            if (list.Count > 0)
            {
                CSteamID lobbyByIndex2 = SteamMatchmaking.GetLobbyByIndex(list.GetRandom<ValueTuple<CSteamID, int>>().Item2);
                __instance.Reflect().Invoke("JoinLobby", lobbyByIndex2);
                return false;
            }
            Debug.LogError("Found No Matches hosting Retyring 2");
            MainMenuHandler.SteamLobbyHandler.Reflect().SetValue("m_Joining", false);
            MainMenuHandler.SteamLobbyHandler.JoinRandom();

            return false;
        }

        private static float rot = 1;
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerSyncer), "OnPhotonSerializeView")] //the only reason this patch is like this is incase we want to add any other features that require spoofing
        public static bool OnPhotonSerializeView(PlayerSyncer __instance, ref PhotonStream stream, ref PhotonMessageInfo info)
        {
            if (!__instance.Reflect().GetValue<Player>("player").IsLocal || !PhotonNetwork.InRoom)
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
                    pos = new Vector3(1000, 100, 1000);
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

        //player prefs stuff, track what is being saved across sessions
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerPrefs), "GetInt", [typeof(string), typeof(int)])]
        public static void GetInt(string key, int defaultValue, ref int __result)
        {
            if (DebugTab.logPlayerPrefs)
                Log.Info($"Get Int called on {key} result is: {__result}");
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerPrefs), "SetInt")]
        public static void SetInt(string key, int value)
        {
            if (DebugTab.logPlayerPrefs)
                Log.Info($"Set Int called on {key} with arg {value}");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerPrefs), "GetFloat", [typeof(string), typeof(float)])]
        public static void GetFloat(string key, float defaultValue, ref float __result)
        {
            if (DebugTab.logPlayerPrefs)
                Log.Info($"Get Int called on {key} result is: {__result}");
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerPrefs), "SetFloat")]
        public static void SetFloat(string key, float value)
        {
            if (DebugTab.logPlayerPrefs)
                Log.Info($"Set Float called on {key} with arg {value}");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerPrefs), "GetString", [typeof(string), typeof(string)])]
        public static void GetString(string key, string defaultValue, ref string __result)
        {
            if (DebugTab.logPlayerPrefs)
                Log.Info($"Get String called on {key} result is: {__result}");
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerPrefs), "SetString")]
        public static void SetString(string key, string value)
        {
            if (DebugTab.logPlayerPrefs)
                Log.Info($"Set Float called on {key} with arg {value}");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PhotonNetwork), "ExecuteRpc")]
        public static bool ExecuteRPC(Hashtable rpcData, Photon.Realtime.Player sender)
        {
            if (sender is null || sender.GamePlayer() is null/* || sender.GamePlayer().Handle().IsDev()*/) return true;

            string rpc = rpcData.ContainsKey(keyByteFive) ?
                PhotonNetwork.PhotonServerSettings.RpcList[(int)(byte)rpcData[keyByteFive]] :
                (string)rpcData[keyByteThree];

            if (!sender.IsLocal && sender.GamePlayer().Handle().IsRPCBlocked())
            {
                Debug.LogError($"RPC {rpc} was blocked from {sender.NickName}.");
                return false;
            }

            Log.Warning($"Received RPC '{rpc}' From '{sender.NickName}'");

            if (sender.GamePlayer().Handle().OnReceivedRPC(rpc, rpcData))
                return true;

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UIPageHandler), "TransistionToPage", typeof(UIPage), typeof(PageTransistion))]
        public static void TransistionToPage(UIPage page, PageTransistion pageTransistion) => SpookPageUI.TryAttachToPageHandler();
    }
}
