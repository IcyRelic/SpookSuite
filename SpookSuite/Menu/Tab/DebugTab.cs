using Photon.Pun;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using UnityEngine;
using Steamworks;
using System.Collections.Generic;
using Zorro.Core;
using SpookSuite.Manager;
using SpookSuite.Handler;
using SpookSuite.Components;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug", true) { }
        private ulong steamLobbyId = 0;
        private string customNotification = "";
        private string customNotificationDesc = "";
        private int steamLobbyIndex = 0;
        private Vector2 scrollPos = Vector2.zero;
        private CallResult<LobbyMatchList_t> matchList;
        public static bool logPlayerPrefs = false;
        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();          
        }
        public float x = 1600, y = 10, w = 300, h = 100;
        public void RectEdit(string title, ref Rect rect)
        {
            UI.HorizontalSpace(title, () =>
            {
                UI.Textbox("X", ref x, false);
                UI.Textbox("Y", ref y, false);
                UI.Textbox("W", ref w, false);
                UI.Textbox("H", ref h, false);
            });
            Notifications.defaultRect = new Rect (x, y, w, h);
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            if (!Player.localPlayer.Handle().IsDev())
            {
                UI.Header("WARNING! \n Expect nothing in this tab to work. This tab is for the developers to test shit. Do not complain about anything here not working. \nThank You", true);
                GUI.Box(new Rect(0, 73, SpookSuiteMenu.Instance.contentWidth, -5), "");
            }

            UI.Header("Debugging Info");

            if (PhotonNetwork.InRoom)
            {
                UI.Label("IsMasterClient", PhotonNetwork.IsMasterClient ? "Yes" : "No");
                UI.Label("MasterClient Nickname", PhotonNetwork.MasterClient.NickName);
            }

            UI.Header("Lobby Tools");
            UI.Button("Host Public", () => MainMenuHandler.Instance.SilentHost());
            UI.Button("Host Private", () => MainMenuHandler.Instance.Host(1));
            UI.Button("Set Lobby Public", () => SetPublic(true));
            UI.Button("Set Lobby Private", () => SetPublic(false));
            UI.Button("Set Lobby Joinable", () => SetJoinable(true));
            UI.Button("Set Lobby NonJoinable", () => SetJoinable(false));

            UI.Header("Notifcations Stuff");
            UI.Checkbox("Notifcations", Cheat.Instance<Notifications>());
            RectEdit("Default Rect", ref Notifications.defaultRect);
            UI.Textbox("Spacing", ref Notifications.spacing, false);
            UI.Textbox("Width", ref Notifications.width, false);
            UI.Textbox("Height", ref Notifications.height, false);
            UI.Button("Test Info", () => { Notifications.PushNotifcation(new Notifcation("Title", "Info", NotificationType.Info)); });
            UI.Button("Test Warning", () => { Notifications.PushNotifcation(new Notifcation("Title", "Warning", NotificationType.Warning)); });
            UI.Button("Test Error", () => { Notifications.PushNotifcation(new Notifcation("Title", "Error", NotificationType.Error)); });
            UI.Button("Test Dev", () => { Notifications.PushNotifcation(new Notifcation("Title", "Dev", NotificationType.Dev)); });
            UI.HorizontalSpace(null, () => { UI.Textbox("Title", ref customNotification); UI.Textbox("Desc", ref customNotificationDesc);
                UI.Button("Test Custom", () => Notifications.PushNotifcation(new Notifcation(customNotification, customNotificationDesc, NotificationType.Info))); });
            UI.Button("Clear ALL", () => { Log.Info($"Cleared {Notifications.notifcations.Count}"); Notifications.notifcations.Clear(); });

            UI.Header("Scene Tools");
            UI.Button("Load Factory", () => LoadScene("FactoryScene"));
            UI.Button("Load Harbour", () => LoadScene("HarbourScene"));
            UI.Button("Load Mines", () => LoadScene("MinesScene"));
            UI.Button("Load Surface", () => LoadScene("SurfaceScene"));
         
            UI.Header("Debugging Cheats");

            UI.Checkbox("Log Player Prefs", ref logPlayerPrefs);
            UI.Button("Log RPCS", () => { foreach (string s in PhotonNetwork.PhotonServerSettings.RpcList) Debug.Log(s); });

            UI.Button("Load Main Menu", () => LoadScene("NewMainMenu"));
            UI.Button("Switch To Main Menu", () => SpookPageUI.TransitionToPage<MainMenuMainPage>());
            
            UI.Button("Teleport All Items", () => {
                GameObjectManager.pickups.ForEach(x => GameUtil.TeleportItem(x));
            });
            
            UI.Button("Use Diving Bell dontcare", () => GameObjectManager.divingBellButton.Interact(Player.localPlayer));
            UI.Button("Use Diving Bell Underground", () => GameObjectManager.divingBell.GoUnderground());
            UI.Button("Use Diving Bell Surface", () => GameObjectManager.divingBell.GoToSurface());
            CSteamID id = MainMenuHandler.SteamLobbyHandler.Reflect().GetValue<CSteamID>("m_CurrentLobby");

            UI.Button("Get Lobby Data", () => {

                int count = SteamMatchmaking.GetLobbyDataCount(id);
                //GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, out string pchKey, int cchKeyBufferSize, out string pchValue, int cchValueBufferSize)
                
                Debug.Log($"Lobby ID: {id}");

                for (int i = 0; i < count; i++)
                {
                    SteamMatchmaking.GetLobbyDataByIndex(id, i, out string key, 265, out string value, 265);
                    Debug.Log($"Key: {key} Value: {value}");
                }
                
                Debug.Log($"steam://joinlobby/2881650/{id}/{PhotonNetwork.MasterClient.GetSteamID()}");
            });
            if(id.IsValid())
                GUILayout.TextArea($"steam://joinlobby/2881650/{id}/{PhotonNetwork.MasterClient.GetSteamID()}");

            UI.Button("LobbyList", () => {

                Debug.Log("Fetching Lobby List...");

                //SteamMatchmaking.AddRequestLobbyListStringFilter("ContentWarningVersion", new BuildVersion(Application.version).ToMatchmaking(), ELobbyComparison.k_ELobbyComparisonEqual);
                //SteamMatchmaking.AddRequestLobbyListStringFilter("Plugins", GameHandler.GetPluginHash(), ELobbyComparison.k_ELobbyComparisonEqual);

                //SteamMatchmaking.AddRequestLobbyListStringFilter("PrivateMatch", "true", ELobbyComparison.k_ELobbyComparisonEqual);
                //SteamMatchmaking.AddRequestLobbyListResultCountFilter(50);

                matchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(MatchListReceived));

                matchList.Set(SteamMatchmaking.RequestLobbyList());
            }, "Get Public");

            

            UI.TextboxAction<ulong>("Join Lobby", ref steamLobbyId, 200, new UIButton("OK", () => {
                //this.JoinLobby(SteamMatchmaking.GetLobbyByIndex(array.GetRandom<(CSteamID, int)>().Item2));
                MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("JoinLobby", new CSteamID(steamLobbyId));
            }));

            UI.TextboxAction<int>("Join Lobby ILOBBY", ref steamLobbyIndex, 3, new UIButton("OK", () => {
                //this.JoinLobby(SteamMatchmaking.GetLobbyByIndex(array.GetRandom<(CSteamID, int)>().Item2));
                MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("JoinLobby", SteamMatchmaking.GetLobbyByIndex(steamLobbyIndex));
            }));

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
            if (PhotonNetwork.CurrentRoom != null)
            {
                SteamMatchmaking.SetLobbyJoinable(id, value);
                PhotonNetwork.CurrentRoom.IsOpen = value;
                PhotonNetwork.CurrentRoom.IsVisible = value;
            }
            return true;
        }

        internal void MatchListReceived(LobbyMatchList_t param, bool biofailure)
        {
            if (biofailure)
                Debug.LogError((object)"Matchlist Biofail");
            else if (param.m_nLobbiesMatching == 0U)
            {
                Debug.Log((object)"Found No Matches hosting");
                //UnityEngine.Object.FindObjectOfType<MainMenuHandler>().SilentHost();
            }
            else
            {
                List<(CSteamID, int)> array = new List<(CSteamID, int)>();
                for (int iLobby = 0; (long)iLobby < (long)param.m_nLobbiesMatching; ++iLobby)
                {
                    CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(iLobby);
                    string lobbyData1 = SteamMatchmaking.GetLobbyData(lobbyByIndex, "ContentWarningVersion");
                    string lobbyData2 = SteamMatchmaking.GetLobbyData(lobbyByIndex, "PhotonRegion");
                    string isPrivate = SteamMatchmaking.GetLobbyData(lobbyByIndex, "PrivateMatch");
                    int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(lobbyByIndex);
                    int lobbyMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(lobbyByIndex);                

                    CSteamID owner = SteamMatchmaking.GetLobbyOwner(lobbyByIndex);
                    Debug.Log($"LobbyID: {lobbyByIndex.ToString()} ILobby: {iLobby} Players: {numLobbyMembers.ToString()}/{lobbyMemberLimit.ToString()} Private: {isPrivate}");

                    string cloudRegion = PhotonNetwork.CloudRegion;
                    bool flag = !string.IsNullOrEmpty(lobbyData2) && lobbyData2 == cloudRegion;
                    if (lobbyData1 == new BuildVersion(Application.version).ToMatchmaking() & flag && numLobbyMembers < 4)
                        array.Add((lobbyByIndex, iLobby));
                }
                Debug.Log((object)("Received SteamLobby Matchlist: " + param.m_nLobbiesMatching.ToString() + " Matching: " + array.Count.ToString()));
            }

        }

        private void LoadScene(string name) => SurfaceNetworkHandler.Instance.photonView.RPC("RPC_LoadScene", RpcTarget.All, (object)name);
    }
}
