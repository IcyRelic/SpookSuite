using ExitGames.Client.Photon;
using Photon.Pun;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using UnityEngine;
using Zorro.Core.CLI;
using Steamworks;
using System.Linq;
using System.Reflection;
using System;
using Object = UnityEngine.Object;
using SpookSuite.Handler;

namespace SpookSuite.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug") { }

        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();          
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Masterclient: ");
            GUILayout.FlexibleSpace();
            GUILayout.Label(PhotonNetwork.IsMasterClient ? "Yes" : "No");
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Open Console"))
            {
                foreach (DebugUIHandler item in Object.FindObjectsOfType<DebugUIHandler>())
                    item.Show();
            }

            if (GUILayout.Button("Close Console"))
            {
                foreach (DebugUIHandler item in Object.FindObjectsOfType<DebugUIHandler>())
                    item.Hide();
            }

            UI.Button("Host Public", () => MainMenuHandler.Instance.SilentHost());
            UI.Button("Host Private", () => MainMenuHandler.Instance.Host(1));

            UI.Button("Set Lobby Public", () => SetPublic(true));
            UI.Button("Set Lobby Private", () => SetPublic(false));

            UI.Button("Set Lobby Joinable", () => SetJoinable(true));
            UI.Button("Set Lobby NonJoinable", () => SetJoinable(false));

            UI.Header("Debugging Cheats");

            if (GUILayout.Button("drone test"))
            {
                ShopHandler.Instance.Reflect().Invoke("RPCA_SpawnDrone", GameUtil.GetItemByName("Camera")); //orders
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Chams?");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                foreach (ItemInstance player in Object.FindObjectsOfType<ItemInstance>())
                {
                    if (player == null)
                        continue;

                    foreach (Renderer renderer in player?.gameObject?.GetComponentsInChildren<Renderer>())
                    {

                        renderer.material = ChamHandler.m_chamMaterial;
                        //renderer.material = ;
                    }

                    /*Highlighter h = player.GetOrAddComponent<Highlighter>();

                    if (h) {
                        h.FlashingOff();
                        h.ConstantOnImmediate(Color.red);
                    }*/
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }
        internal static bool SetPublic(bool value)
        {
            CSteamID id = MainMenuHandler.SteamLobbyHandler.Reflect().GetValue<CSteamID>("m_CurrentLobby");
            MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("SetLobbyType", id, value ? ELobbyType.k_ELobbyTypePublic : ELobbyType.k_ELobbyTypeFriendsOnly);
            return true;
        }

        internal static bool SetJoinable(bool value)
        {
            CSteamID id = MainMenuHandler.SteamLobbyHandler.Reflect().GetValue<CSteamID>("m_CurrentLobby");
            MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("SetLobbyJoinable", id, value);
            if (PhotonNetwork.CurrentRoom != null)
            {
                PhotonNetwork.CurrentRoom.IsOpen = value;
                PhotonNetwork.CurrentRoom.IsVisible = value;
            }
            return true;
        }
        //internal static bool IsJoinable()
        //{
        //    return PhotonNetwork.CurrentRoom.IsOpen && PhotonNetwork.CurrentRoom.IsVisible;
        //}
        //internal static bool IsPublic()
        //{
        //    return 
        //}
    }
}
