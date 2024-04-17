﻿using CurvedUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using Steamworks;
using System;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("Players") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public static Player selectedPlayer = null;

        public int num;

        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.3f - SpookSuiteMenu.Instance.spaceFromLeft));
            PlayersList();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.7f - SpookSuiteMenu.Instance.spaceFromLeft));
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
            GeneralActions();
            PlayerActions();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void GeneralActions()
        {
            UI.Header("ALL Players");
            UI.Button("Kick All (NonHost)", () =>
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient) // need fixing
                {
                    for (int i = 0; i < GameObjectManager.players.Count; i++)
                    {
                        if (!GameObjectManager.players[i].IsLocal)
                            PhotonNetwork.NetworkingClient.CurrentRoom.Reflect().Invoke("RemovePlayer", GameObjectManager.players[i].refs.view.Owner);
                    }
                }
                else
                    Cheat.Instance<KickAll>().Execute();
            });
            UI.Button("Kill All", () => Cheat.Instance<KillAll>().Execute());
            UI.Button("Revive All", () => Cheat.Instance<ReviveAll>().Execute());
        }

        private void PlayerActions()
        {
            if (selectedPlayer is null) return;
            UI.Header("Selected Player Actions");

            SteamAvatarHandler.TryGetSteamIDForPlayer(selectedPlayer.refs.view.Owner, out CSteamID steamid);

            UI.Label("SteamID", steamid.m_SteamID.ToString());
            UI.Label("RPC Count (Last 60s)", selectedPlayer.Handle().RPCsOnFile().ToString());
            UI.Label("SpookSuite User", selectedPlayer.Handle().IsSpookUser().ToString());

            if (!selectedPlayer.IsLocal)
                UI.Button("Block RPCs", () => selectedPlayer.Handle().ToggleRPCBlock(), selectedPlayer.Handle().IsRPCBlocked() ? "UnBlock" : "Block");

            UI.Button("Kick Steam User", () => MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("RemoveSteamClient", steamid));
            UI.Button("Kick Photon User", () =>
            {
                PhotonNetwork.EnableCloseConnection = true;
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
                {
                    TargetActors = new int[1] { selectedPlayer.refs.view.Owner.ActorNumber }
                };
                PhotonNetwork.NetworkingClient.OpRaiseEvent((byte)203, (object)null, raiseEventOptions, SendOptions.SendReliable);
       
            });
            UI.Button("Teleport", () => PhotonNetwork.Instantiate("Player", selectedPlayer.data.groundPos, new Quaternion(0f, 0f, 0f, 0f)), "Teleport");
            UI.Button("Bring", () =>
            {
                if (!GameUtil.IsOverridingPhotonLocalPlayer())
                { GameUtil.ToggleOverridePhotonLocalPlayer(); }
                PhotonNetwork.Instantiate("Player", selectedPlayer.data.groundPos, new Quaternion(0f, 0f, 0f, 0f));
                GameUtil.ToggleOverridePhotonLocalPlayer();
            }, "Bring");
            UI.Button("Nearby Monsters Attack", () => selectedPlayer.Handle().GetClosestMonster().SetTargetPlayer(selectedPlayer), "Nearby Monsters Attack");
            UI.Button("All Monsters Attack", () => GameObjectManager.monsters.ForEach(m => m.SetTargetPlayer(selectedPlayer)), "All Monsters Attack");
            UI.Button("Spawn Bomb", () => GameUtil.SpawnItem(GameUtil.GetItemByName("bomb").id, selectedPlayer.refs.cameraPos), "Bomb");
            UI.Button("Kill", () => selectedPlayer.Reflect().Invoke("CallDie"), "Kill");
            UI.Button("Revive", () => selectedPlayer.CallRevive(), "Revive");
            UI.Button("Kick", () => PhotonNetwork.NetworkingClient.CurrentRoom.Reflect().Invoke("RemovePlayer", selectedPlayer.refs.view.Owner), "Kick");
            UI.Button("Ragdoll", () => selectedPlayer.Reflect().Invoke("CallTakeDamageAndAddForceAndFall", 0f, Vector3.zero, 2f), "Ragdoll");
            UI.Button("Tase", () => selectedPlayer.Reflect().Invoke("CallTakeDamageAndTase", 1f, 5f));
        }

        private void PlayersList()
        {
            float width = SpookSuiteMenu.Instance.contentWidth * 0.3f - SpookSuiteMenu.Instance.spaceFromLeft * 2;
            float height = SpookSuiteMenu.Instance.contentHeight - 20;

            Rect rect = new Rect(0, 0, width, height);
            GUI.Box(rect, "Player List");

            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));

            GUILayout.Space(25);
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (Player player in GameObjectManager.players)
            {
                //if (player.disconnectedMidGame || !player.IsSpawned) continue;
                if (player.ai) continue;

                if (selectedPlayer is null) selectedPlayer = player;

                if(selectedPlayer.Handle().IsSpookUser()) GUI.contentColor = Settings.c_primary.GetColor();
                if (selectedPlayer.GetInstanceID() == player.GetInstanceID()) GUI.contentColor = Settings.c_espPlayers.GetColor();

                if (GUILayout.Button(player.refs.view.Owner.NickName, GUI.skin.label)) selectedPlayer = player;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
