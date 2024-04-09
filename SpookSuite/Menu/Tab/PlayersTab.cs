using CurvedUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("Players") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public Player selectedPlayer = null;

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
            GUILayout.Label("ALL Players");
            if (GUILayout.Button("Kick All (NonHost)"))
            {
                //if (PhotonNetwork.LocalPlayer.IsMasterClient) // need fixing
                //{
                //    for (int i = 0; i < GameObjectManager.players.Count; i++)
                //    {
                //        //if (!GameObjectManager.players[i].IsLocal)
                //            PhotonNetwork.CloseConnection(GameObjectManager.players[i].refs.view.Owner);
                //    }                  
                //}
                //else
                    Cheat.Instance<KickAll>().Execute();
            }
        }
        private void PlayerActions()
        {
            UI.Header("Selected Player Actions");
            UI.Button("Teleport", () => Player.localPlayer.data.groundPos = selectedPlayer.data.groundPos, "Teleport");
            UI.Button("Bring", () => selectedPlayer.data.groundPos = Player.localPlayer.data.groundPos, "Bring");
            UI.Button("Nearby Monsters Attack", () => selectedPlayer.GetClosestMonster().SetTargetPlayer(selectedPlayer), "Nearby Monsters Attack");
            UI.Button("All Monsters Attack", () => GameObjectManager.monsters.ForEach(m => m.SetTargetPlayer(selectedPlayer)), "All Monsters Attack");
            UI.Button("Spawn Bomb", () => GameUtil.SpawnItem(58, selectedPlayer.data.groundPos), "Bomb");
            UI.Button("Kill", () => selectedPlayer.Reflect().Invoke("CallDie"), "Kill");
            UI.Button("Revive", () => selectedPlayer.CallRevive(), "Revive");
            UI.Button("Kick", () => PlayerHandler.instance.RemovePlayer(selectedPlayer), "Kick");
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

                if (selectedPlayer.GetInstanceID() == player.GetInstanceID()) GUI.contentColor = Settings.c_espPlayers.GetColor();

                if (GUILayout.Button(player.refs.view.Owner.NickName, GUI.skin.label)) selectedPlayer = player;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
