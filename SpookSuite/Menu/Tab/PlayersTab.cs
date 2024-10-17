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
using System.Linq;
using UnityEngine;
using Zorro.Core;
using Steamworks;
using System.Drawing;
using ExitGames.Client.Photon;
using Photon.Voice.Unity;

namespace SpookSuite.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("Players") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public static Player selectedPlayer = null;
        public static string faceText = "SS";
        public static string faceColor = "000000";
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
            if (!PhotonNetwork.InRoom) return;

            UI.Header("ALL Players");
            UI.Button("Kick All", () => Cheat.Instance<KickAll>().Execute());
            UI.Button("Kill All", () => Cheat.Instance<KillAll>().Execute());
            UI.Button("Revive All", () => Cheat.Instance<ReviveAll>().Execute());
            UI.Button("Limbo Others", () => Limbo.AddPlayers(GameObjectManager.players.Where(p => !p.IsLocal).ToList()));
            UI.HorizontalSpace(null, () =>
            {
                UI.Textbox("Bombs", ref BombAll.Value, false, 3); //max 999 otherwise too laggy
                UI.Button("Bomb All", () => Cheat.Instance<BombAll>().Execute(), null);
            });
            UI.CheatToggleSlider(Cheat.Instance<SuperSpeedOthers>(), "Super Speed", SuperSpeedOthers.Value.ToString(), ref SuperSpeedOthers.Value, 1, 6);
            UI.Checkbox("Reverse Others", Cheat.Instance<ReverseOthers>());

            if (Player.localPlayer.Handle().IsDev())
            {
                UI.Header("Dev Only Non SpookSuite Player Options");
                UI.Checkbox("Freeze Others", Cheat.Instance<FreezeAll>());
            }

            UI.CheatToggleSlider(Cheat.Instance<OthersFly>(), "Give Flight", OthersFly.Value.ToString(), ref OthersFly.Value, 1, 30);
        }

        private void PlayerActions()
        {
            if (selectedPlayer is null) return;

            if (selectedPlayer.Handle().IsSpookUser() && Player.localPlayer.Handle().IsDev())
            {
                UI.Header("SpookSuite Specialty");
                //add things that we could do to our users for fun, maybe disabling something in their menu?
                UI.Button("WASSUP", () => { });           
            }

            UI.Header("Selected Player Actions");

            GUILayout.TextArea("SteamID: " + (selectedPlayer.Handle().IsDev() ? 0 : selectedPlayer.GetSteamID().m_SteamID));
            UI.Button("Go To Profile", () => System.Diagnostics.Process.Start("https://steamcommunity.com/profiles/" + selectedPlayer.GetSteamID().m_SteamID));
            UI.Label("SpookSuite User", selectedPlayer.Handle().IsSpookUser().ToString());

            if (!Player.localPlayer.Handle().IsDev() && selectedPlayer.Handle().IsDev())
            {
                UI.Label("User IS Dev So You Cant Do Anything :)");
                return;
            } 

            if (!selectedPlayer.IsLocal)
                UI.Button("Block RPCs", () => selectedPlayer.Handle().ToggleRPCBlock(), selectedPlayer.Handle().IsRPCBlocked() ? "UnBlock" : "Block");

            UI.Button("Teleport", () => { Player.localPlayer.Reflect().Invoke("Teleport", selectedPlayer.refs.cameraPos.position, new Vector3(0, 0, 0)); }, "Teleport");
            UI.TextboxAction("Face Text", ref faceText, 3, new UIButton("Set", () => selectedPlayer.Handle().RPC("RPCA_SetVisorText", RpcTarget.All, faceText)));
            UI.TextboxAction("Face Color", ref faceColor, 8,
                new UIButton("Set", () =>
                {
                    while (faceColor.Length < 6) faceColor += "0";
                    selectedPlayer.refs.visor.ApplyVisorColor(new RGBAColor(faceColor).GetColor());
                }
            ));
            UI.Button("test", () => PhotonNetwork.RaiseEvent(20, selectedPlayer.photonView.Owner.ActorNumber, RaiseEventOptions.Default, SendOptions.SendReliable));
            UI.Button("Spawn Bomb", () => GameUtil.SpawnItem(GameUtil.GetItemByName("bomb").id, selectedPlayer.refs.cameraPos.position), "Bomb");
            UI.Button("Freeze", () => selectedPlayer.Reflect().Invoke("CallSlowFor", 0f, 4f), "Freeze");
            
            UI.Button("Kill", () => selectedPlayer.Reflect().Invoke("CallDie"), "Kill");
            UI.Button("Revive", () => selectedPlayer.CallRevive(), "Revive");

            UI.Button("Ragdoll", () => selectedPlayer.Reflect().Invoke("CallTakeDamageAndAddForceAndFall", 0f, Vector3.zero, 2f), "Ragdoll");
            UI.Button("Launch", () => selectedPlayer.Reflect().Invoke("CallTakeDamageAndAddForceAndFall", 0f, selectedPlayer.refs.cameraPos.up * 100, 0f), "Launch");
            UI.Button("Tase", () => selectedPlayer.Reflect().Invoke("CallTakeDamageAndTase", 1f, 5f));

            UI.Button("Force Sit", () => { Sittable s = GameObjectManager.sittables.GetRandom(); selectedPlayer.refs.view.RPC("RPCA_Sit", RpcTarget.All, s.Reflect().GetValue<PhotonView>("view").ViewID, s.Reflect().GetValue<int>("seatID")); });
            UI.Button("Heal", () => selectedPlayer.Handle().RPC("RPCA_Heal", RpcTarget.All, 100f));

            UI.Button("Kick", () => { SurfaceNetworkHandler.Instance.photonView.RPC("RPC_LoadScene", selectedPlayer.PhotonPlayer(), "NewMainMenu"); });
            UI.Button("Send Away", () => ShadowRealmHandler.instance.TeleportPlayerToRandomRealm(selectedPlayer));
            UI.HorizontalSpace("Limbo", () => 
            {
                UI.Button("Add To Limbo", () => { if (!Limbo.limboList.Contains(selectedPlayer)) Limbo.limboList.Add(selectedPlayer); Log.Info(Limbo.limboList.Count); }); 
                UI.Button("Remove From Limbo", () => { Limbo.limboList.Remove(selectedPlayer); Log.Info(Limbo.limboList.Count); });
            });

            UI.Header("Hat Stuff", true);
            UI.Button("Remove Hat", () => selectedPlayer.refs.view.RPC("RPCA_EquipHat", RpcTarget.All, -1));
            UI.ButtonGrid<Hat>(HatDatabase.instance.hats.ToList(), h => h.GetName(), "", h => selectedPlayer.refs.view.RPC("RPCA_EquipHat", RpcTarget.All, HatDatabase.instance.GetIndexOfHat(h)), 3);
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
