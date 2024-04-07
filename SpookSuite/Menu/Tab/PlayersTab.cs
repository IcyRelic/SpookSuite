using Photon.Pun;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System.ComponentModel;
using System;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("Players") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 playerListPos = Vector2.zero;
        public Player selectedPlayer = null;

        public int num;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.Label(name); //doing it like this so we could just copy paste it over
            MenuContent();
            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.Label("ALL Players");
            if(GUILayout.Button("Kick All (NonHost)"))
                Cheats.KickAll.Execute();

            playerListPos = GUILayout.BeginScrollView(playerListPos);
            foreach (Player p in PlayerHandler.instance.players)//PlayerHandler.instance.players
            {
                if (GUILayout.Button(p.refs.view.Owner.NickName))
                    selectedPlayer = p;
            }
            GUILayout.EndScrollView();

            UI.InputInt("tospawnid", ref num);

            if (GUILayout.Button("Spawn " + num))
            {
                Pickup component = PhotonNetwork.Instantiate("PickupHolder", Player.localPlayer.transform.position, UnityEngine.Random.rotation, 0, null).GetComponent<Pickup>();
                component.ConfigurePickup((byte)num, new ItemInstanceData(Guid.NewGuid()));
            }
            //58 bomb
            if (GUILayout.Button("TP To"))
                Player.localPlayer.transform.position = selectedPlayer.transform.position;
            if (GUILayout.Button("Bring"))
                selectedPlayer.transform.position = Player.localPlayer.HeadPosition();
            if (GUILayout.Button("Nearest Monster Attack"))
                GameObjectManager.Instance.GetClosestMonsterToPlayer(selectedPlayer).SetTargetPlayer(selectedPlayer);
            if (GUILayout.Button("All Monsters Attack"))
                GameObjectManager.monsters.ForEach(m => m.SetTargetPlayer(selectedPlayer));

            GUILayout.EndScrollView();
        }
    }
}
