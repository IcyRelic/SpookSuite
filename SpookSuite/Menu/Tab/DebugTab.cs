using ExitGames.Client.Photon;
using Photon.Pun;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using UnityEngine;
using Zorro.Core.CLI;
using Steamworks;

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

            GUILayout.BeginHorizontal();
            GUILayout.Label("Add $1000");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {

                SurfaceNetworkHandler.RoomStats.AddMoney(1000);

                VerboseDebug.Log("Changing RoomStats: Money: " + SurfaceNetworkHandler.RoomStats.Money.ToString() + " Day: " + SurfaceNetworkHandler.RoomStats.CurrentDay.ToString());
                Hashtable propertiesToSet = new Hashtable();
                propertiesToSet.Add((object)"m", (object)SurfaceNetworkHandler.RoomStats.Money);
                propertiesToSet.Add((object)"d", (object)SurfaceNetworkHandler.RoomStats.CurrentDay);
                propertiesToSet.Add((object)"q", (object)SurfaceNetworkHandler.RoomStats.CurrentQuotaDay);
                propertiesToSet.Add((object)"qs", (object)SurfaceNetworkHandler.RoomStats.QuotaToReach);
                propertiesToSet.Add((object)"cq", (object)SurfaceNetworkHandler.RoomStats.CurrentQuota);
                propertiesToSet.Add((object)"s", (object)GameAPI.seed);
                propertiesToSet.Add((object)"cu", (object)SurfaceNetworkHandler.RoomStats.CurrentCamera.GetUpgradeData());
                PhotonNetwork.CurrentRoom.SetCustomProperties(propertiesToSet);
            }
            GUILayout.EndHorizontal();

            UI.Checkbox("No CLip", ref Cheat.Instance<NoClip>().Enabled);

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
