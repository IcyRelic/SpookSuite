using Photon.Pun;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using UnityEngine;
using Steamworks;
using System.Collections.Generic;
using Zorro.Core;
using SpookSuite.Manager;
using SpookSuite.Handler;

namespace SpookSuite.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug", true) { }
        private ulong steamLobbyId = 0;
        private int steamLobbyIndex = 0;
        private Vector2 scrollPos = Vector2.zero;
        private CallResult<LobbyMatchList_t> matchList;
        public static bool logPlayerPrefs = false;
        public static bool disablePatch = false;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();          
        }
        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

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

            UI.Header("Scene Tools");
            UI.Button("Load Factory", () => LoadScene("FactoryScene"));
            UI.Button("Load Harbour", () => LoadScene("HarbourScene"));
            UI.Button("Load Surface", () => LoadScene("SurfaceScene"));

            UI.Header("Debugging Cheats");
            UI.Checkbox("Log Player Prefs", ref logPlayerPrefs);
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
