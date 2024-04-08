using CurvedUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
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
                    Cheats.KickAll.Execute();
            }
        }
        private void PlayerActions()
        {
            GUILayout.Label("Selected Player Actions");
            if (GUILayout.Button("TP To"))
                Player.localPlayer.transform.position = new Vector3(selectedPlayer.data.groundPos.x, selectedPlayer.data.groundPos.y, selectedPlayer.data.groundPos.z + selectedPlayer.data.headHeight);
            if (GUILayout.Button("Bring"))
                selectedPlayer.transform.position = Player.localPlayer.HeadPosition();
            if (GUILayout.Button("Nearest Monster Attack"))
                selectedPlayer.GetClosestMonster().SetTargetPlayer(selectedPlayer);
            if (GUILayout.Button("All Monsters Attack"))
                GameObjectManager.monsters.ForEach(m => m.SetTargetPlayer(selectedPlayer));
            if (GUILayout.Button("Bomb"))
            {
                Pickup component = PhotonNetwork.Instantiate("PickupHolder", selectedPlayer.data.groundPos, UnityEngine.Random.rotation, 0, null).GetComponent<Pickup>();
                component.ConfigurePickup(58, new ItemInstanceData(Guid.NewGuid()));
            }
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
