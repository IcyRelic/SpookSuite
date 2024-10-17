using Photon.Pun;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

namespace SpookSuite.Menu.Tab
{
    internal class LobbyTab : MenuTab
    {
        public LobbyTab() : base("Lobby") { }
        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        private CallResult<LobbyMatchList_t> matchList;
        private List<CSteamID> lobbyList = new List<CSteamID>();
        private CSteamID selectedLobby;
        
        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.3f - SpookSuiteMenu.Instance.spaceFromLeft));
            Lobbies();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.7f - SpookSuiteMenu.Instance.spaceFromLeft));
            UI.ScrollView(ref scrollPos, () =>
            {
                GeneralActions();
                LobbyActions();
            });
            GUILayout.EndVertical();
        }

        private void Lobbies()
        {
            float width = SpookSuiteMenu.Instance.contentWidth * 0.3f - SpookSuiteMenu.Instance.spaceFromLeft * 2;
            float height = SpookSuiteMenu.Instance.contentHeight - 20;
            //fuck this shit, idc about it at all
            //UI.Button("Refresh", () => {
            //    lobbyList.Clear();
            //    matchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(MatchListReceived));
            //    matchList.Set(SteamMatchmaking.RequestLobbyList());
            //}, null);

            //Rect rect = new Rect(0, 30, width, height);
            //GUI.Box(rect, "Lobby List");

            //GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));
            
            //GUILayout.Space(25);
            //UI.ScrollView(ref scrollPos2, () => {
            //    foreach (CSteamID lobby in lobbyList)
            //    {
            //        if (!lobby.IsValid()) selectedLobby = lobby;
            //        if (selectedLobby.m_SteamID == lobby.m_SteamID) GUI.contentColor = Settings.c_espPlayers.GetColor();
            //        if (GUILayout.Button($"{SteamMatchmaking.GetNumLobbyMembers(lobby)}/{SteamMatchmaking.GetLobbyMemberLimit(lobby)}" + (SteamMatchmaking.GetLobbyData(lobby, "PrivateMatch").Equals("true") ? "Private" : ""), GUI.skin.label)) selectedLobby = lobby;

            //        GUI.contentColor = Settings.c_menuText.GetColor();
            //    }
            //});
            //GUILayout.EndVertical();
        }

        private void LobbyActions()
        {
            UI.Header("Lobby Actions");
            UI.Button("Join", () => LobbyManager.JoinLobby(selectedLobby));
        }

        private void GeneralActions()
        {
            UI.Header("General Actions");
            UI.Checkbox("AntiKick (Experimental)", Cheat.Instance<AntiKick>());
            UI.Button("Rejoin Previous", LobbyManager.JoinLastLobby);
            UI.Button("Leave Current", ConnectionStateHandler.Instance.Disconnect);
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
                        lobbyList.Add(lobbyByIndex);
                }
            }
        }
    }
}
