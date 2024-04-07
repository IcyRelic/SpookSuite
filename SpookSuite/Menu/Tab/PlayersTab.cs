using Photon.Pun;
using SpookSuite.Menu.Core;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("Players") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 playerListPos = Vector2.zero;
        public Player selectedPlayer = null;
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

            if (GUILayout.Button("TP To"))
                Player.localPlayer.transform.position = selectedPlayer.transform.position.normalized;
            if (GUILayout.Button("Bring"))
                selectedPlayer.transform.position = Player.localPlayer.HeadPosition();

            GUILayout.EndScrollView();
        }
    }
}
