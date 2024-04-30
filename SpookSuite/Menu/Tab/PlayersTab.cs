using Photon.Pun;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
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
            UI.Button("Kick All", () =>
            {
                Cheat.Instance<KickAll>().Execute();
            });
            UI.Button("Kill All", () => Cheat.Instance<KillAll>().Execute());
            UI.Button("Revive All", () => Cheat.Instance<ReviveAll>().Execute());
        }

        private void PlayerActions()
        {
            if (selectedPlayer is null) return;
            UI.Header("Selected Player Actions");

            UI.Label("SteamID", selectedPlayer.GetSteamID().ToString());
            UI.Label("SpookSuite User", selectedPlayer.Handle().IsSpookUser().ToString());

            if (!selectedPlayer.IsLocal)
                UI.Button("Block RPCs", () => selectedPlayer.Handle().ToggleRPCBlock(), selectedPlayer.Handle().IsRPCBlocked() ? "UnBlock" : "Block");

            UI.Button("Teleport", () => { PhotonNetwork.DestroyPlayerObjects(Player.localPlayer.PhotonPlayer()); PhotonNetwork.Instantiate("Player", selectedPlayer.data.groundPos, new Quaternion(0f, 0f, 0f, 0f)); }, "Teleport");

            UI.Button("Spawn Bomb", () => GameUtil.SpawnItem(GameUtil.GetItemByName("bomb").id, selectedPlayer.refs.cameraPos.position), "Bomb");
            UI.Button("Goo Em", () => { GameUtil.SpawnItem(GameUtil.GetItemByName("Goo Ball").id, selectedPlayer.refs.cameraPos.position); Object.FindObjectOfType<ItemGooBall>().Reflect().GetValue<OnOffEntry>("usedEntry").on = true; }, "Goo"); //todo fix not exploding, figure out how to interact
            UI.Button("Kill", () => selectedPlayer.Reflect().Invoke("CallDie"), "Kill");
            UI.Button("Revive", () => selectedPlayer.CallRevive(), "Revive");

            UI.Button("Ragdoll", () => selectedPlayer.Reflect().Invoke("CallTakeDamageAndAddForceAndFall", 0f, Vector3.zero, 2f), "Ragdoll");
            UI.Button("Launch", () => selectedPlayer.Reflect().Invoke("CallTakeDamageAndAddForceAndFall", 0f, selectedPlayer.refs.cameraPos.up * 100, 0f), "Launch");
            UI.Button("Tase", () => selectedPlayer.Reflect().Invoke("CallTakeDamageAndTase", 1f, 5f));

            if (selectedPlayer.Handle().IsSpookUser() && Player.localPlayer.Handle().IsDev())
            {
                UI.Header("SpookSuite Specialty");
                //add things that we could do to our users for fun, maybe disabling something in their menu?
                UI.Button("", () => { });
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
                if (!player.IsValid()) continue;
                if (selectedPlayer is null) selectedPlayer = player;
                if (player.Handle().IsSpookUser()) GUI.contentColor = Settings.c_primary.GetColor();
                if (selectedPlayer.GetInstanceID() == player.GetInstanceID()) GUI.contentColor = Settings.c_espPlayers.GetColor();
                if (GUILayout.Button(player.refs.view.Owner.NickName, GUI.skin.label)) selectedPlayer = player;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
