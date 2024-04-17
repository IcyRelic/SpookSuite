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
using Object = UnityEngine.Object;
using System.Collections.Generic;
using Photon.Realtime;
using Zorro.Core;
using System.Collections;
using SpookSuite.Handler;

namespace SpookSuite.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug") { }
        private ulong steamLobbyId = 0;
        private int steamLobbyIndex = 0;
        private Vector2 scrollPos = Vector2.zero;
        private CallResult<LobbyMatchList_t> matchList;
        public static bool rpcnoeffectdev;
        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();          
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            UI.Label("MasterClient", PhotonNetwork.IsMasterClient ? "Yes" : "No");


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

            for (int i = 0; i < SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll); i++)
            {
                if (Player.localPlayer.GetSteamID().m_SteamID == (long)76561199159991462 && SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll).m_SteamID == (long)76561198093261109)
                {
                    UI.Button("Invite Icy", () => { SteamMatchmaking.InviteUserToLobby(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll), MainMenuHandler.SteamLobbyHandler.Reflect().GetValue<CSteamID>("m_CurrentLobby")); });
                    UI.Button("Join Icy", () => { SteamFriends.GetFriendGamePlayed(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll), out var info);  MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("JoinLobby", info.m_steamIDLobby); });
                }
                else if (SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll).m_SteamID == (long)76561199159991462)
                {
                    UI.Button("Invite Bandit", () => { SteamMatchmaking.InviteUserToLobby(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll), MainMenuHandler.SteamLobbyHandler.Reflect().GetValue<CSteamID>("m_CurrentLobby")); });
                    UI.Button("Join Bandit", () => { SteamFriends.GetFriendGamePlayed(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll), out var info); MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("JoinLobby", info.m_steamIDLobby); });
                }
            }
            


            UI.Button("Host Public", () => MainMenuHandler.Instance.SilentHost());
            UI.Button("Host Private", () => MainMenuHandler.Instance.Host(1));

            UI.Button("Set Lobby Public", () => SetPublic(true));
            UI.Button("Set Lobby Private", () => SetPublic(false));

            UI.Button("Set Lobby Joinable", () => SetJoinable(true));
            UI.Button("Set Lobby NonJoinable", () => SetJoinable(false));
            UI.Button("Join Random", () => PhotonNetwork.JoinRandomRoom());

            UI.Button("Remove ALL Pickups", () => Object.FindObjectsOfType<Pickup>().ToList().ForEach(p => p.m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient)));

            UI.Header("Debugging Cheats");

            if (GUILayout.Button("drone test"))
            {
                ShopHandler.Instance.Reflect().Invoke("RPCA_SpawnDrone", GameUtil.GetItemByName("Camera")); //orders
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Go Underground");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                Object.FindAnyObjectByType<DivingBell>().Reflect().GetValue<PhotonView>("m_photonView").RPC("RPC_GoToUnderground", RpcTarget.MasterClient);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Go Surface");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                Object.FindAnyObjectByType<DivingBell>().Reflect().GetValue<PhotonView>("m_photonView").RPC("RPC_GoToSurface", RpcTarget.MasterClient);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Open Diving Bell");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                Object.FindAnyObjectByType<DivingBell>().AttemptSetOpen(true);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Close Diving Bell");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                Object.FindAnyObjectByType<DivingBell>().AttemptSetOpen(false);
            }
            GUILayout.EndHorizontal();

            UI.Button("Factory", () => {
                SurfaceNetworkHandler.Instance.photonView.RPC("RPC_LoadScene", RpcTarget.All, (object)"FactoryScene");
            }, "Factory");

            UI.Button("Surface", () => {
                SurfaceNetworkHandler.Instance.photonView.RPC("RPC_LoadScene", RpcTarget.All, (object)"SurfaceScene");
            });

            UI.Button("Get Lobby Data", () => {

                CSteamID id = MainMenuHandler.SteamLobbyHandler.Reflect().GetValue<CSteamID>("m_CurrentLobby");
                int count = SteamMatchmaking.GetLobbyDataCount(id);
                //GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, out string pchKey, int cchKeyBufferSize, out string pchValue, int cchValueBufferSize)
                
                Debug.Log($"Lobby ID: {id}");

                for (int i = 0; i < count; i++)
                {
                    SteamMatchmaking.GetLobbyDataByIndex(id, i, out string key, 265, out string value, 265);
                    Debug.Log($"Key: {key} Value: {value}");
                }

                SteamAvatarHandler.TryGetSteamIDForPlayer(PhotonNetwork.MasterClient, out CSteamID steamid);

                Debug.Log($"steam://joinlobby/2881650/{id}/{steamid}");
            });

            UI.Button("LobbyList (FRIEND)", () => {

                Debug.Log("Fetching Lobby List...");

                //SteamMatchmaking.AddRequestLobbyListStringFilter("ContentWarningVersion", new BuildVersion(Application.version).ToMatchmaking(), ELobbyComparison.k_ELobbyComparisonEqual);
                //SteamMatchmaking.AddRequestLobbyListStringFilter("Plugins", GameHandler.GetPluginHash(), ELobbyComparison.k_ELobbyComparisonEqual);

                //SteamMatchmaking.AddRequestLobbyListStringFilter("PrivateMatch", "true", ELobbyComparison.k_ELobbyComparisonEqual);
                //SteamMatchmaking.AddRequestLobbyListResultCountFilter(50);

                

                matchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(MatchListReceived));

                matchList.Set(SteamMatchmaking.RequestLobbyList());



            }, "Get Friends");

            UI.Button("LobbyList", () => {

                Debug.Log("Fetching Lobby List...");

                //SteamMatchmaking.AddRequestLobbyListStringFilter("ContentWarningVersion", new BuildVersion(Application.version).ToMatchmaking(), ELobbyComparison.k_ELobbyComparisonEqual);
                //SteamMatchmaking.AddRequestLobbyListStringFilter("Plugins", GameHandler.GetPluginHash(), ELobbyComparison.k_ELobbyComparisonEqual);

                //SteamMatchmaking.AddRequestLobbyListStringFilter("PrivateMatch", "true", ELobbyComparison.k_ELobbyComparisonEqual);
                //SteamMatchmaking.AddRequestLobbyListResultCountFilter(50);

                matchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(MatchListReceived));

                matchList.Set(SteamMatchmaking.RequestLobbyList());



            }, "Get Public");

            UI.Button("LobbyList (Private)", () => {

                Debug.Log("Fetching Lobby List...");

                //SteamMatchmaking.AddRequestLobbyListStringFilter("ContentWarningVersion", new BuildVersion(Application.version).ToMatchmaking(), ELobbyComparison.k_ELobbyComparisonEqual);
                //SteamMatchmaking.AddRequestLobbyListStringFilter("Plugins", GameHandler.GetPluginHash(), ELobbyComparison.k_ELobbyComparisonEqual);

                SteamMatchmaking.AddRequestLobbyListStringFilter("PrivateMatch", "true", ELobbyComparison.k_ELobbyComparisonEqual);
                //SteamMatchmaking.AddRequestLobbyListResultCountFilter(50);

                matchList = CallResult<LobbyMatchList_t>.Create(new CallResult<LobbyMatchList_t>.APIDispatchDelegate(MatchListReceived));

                matchList.Set(SteamMatchmaking.RequestLobbyList());



            }, "Get Private");

            UI.TextboxAction<ulong>("Join Lobby", ref steamLobbyId, 200, new UIButton("OK", () => {
                //this.JoinLobby(SteamMatchmaking.GetLobbyByIndex(array.GetRandom<(CSteamID, int)>().Item2));
                MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("JoinLobby", new CSteamID(steamLobbyId));


            }));

            UI.TextboxAction<int>("Join Lobby ILOBBY", ref steamLobbyIndex, 3, new UIButton("OK", () => {
                //this.JoinLobby(SteamMatchmaking.GetLobbyByIndex(array.GetRandom<(CSteamID, int)>().Item2));
                MainMenuHandler.SteamLobbyHandler.Reflect().Invoke("JoinLobby", SteamMatchmaking.GetLobbyByIndex(steamLobbyIndex));


            }));

            UI.Checkbox("RPC Dev Mode", ref rpcnoeffectdev);

            UI.Button("Join (Private)", () => SpookSuite.Instance.StartCoroutine(JoinRandomPrivateGame()), "Join Random Private");
            UI.Button("Reset Objects", () => PersistentObjectsHolder.Instance.ResetPersistantObjects(), "Reset Objects");

            UI.Button("Check Inventory", () => {

   

                Debug.Log("Find GlobalPlayerData...");

                GlobalPlayerData[] data = Object.FindObjectsByType<GlobalPlayerData>(FindObjectsSortMode.None);
                Debug.Log($"Found {data.Length} global player data objects.");

                data.ToList().ForEach(d =>
                {
          

                   // Debug.Log($"Parent: {parent.name}");
                });


                GlobalPlayerData d = Player.localPlayer.refs.view.GetComponentInParent<GlobalPlayerData>();

                Debug.Log($"D: {d == null}");



            }, "Check");

            UI.Button("List RPCs", () => PhotonNetwork.PhotonServerSettings.RpcList.ForEach(d => Debug.Log(d)));


            GUILayout.EndScrollView();
        }

        internal IEnumerator JoinRandomPrivateGame()
        {
            Debug.Log("Joining Random Private Photon Room");

            //Hosting a game, Allows me to kick myself from the photon room, the game then bugs out and places me in a random what seems to be in progress, full, or private lobbies or all 3
            MainMenuHandler.Instance.Host(1);

            //lets wait for the hosted lobby
            yield return new WaitForSeconds(4);

            Debug.Log("Self Kicking / going to private lobby");
            //enable kicking in the photon room (this is local even if you are master client thus why we host our own lobby)
            PhotonNetwork.EnableCloseConnection = true;

            //Send the kick notification to myself (with it enabled and being the master as it does check if the request is sent by the master)
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
            {
                TargetActors = new int[1] { PhotonNetwork.LocalPlayer.ActorNumber }
            };
            PhotonNetwork.NetworkingClient.OpRaiseEvent((byte)203, (object)null, raiseEventOptions, SendOptions.SendReliable);

            //Waiting for the kick to happen and be randomly sent to another lobby, this takes a few seconds
            yield return new WaitForSeconds(5);

            Debug.Log("Setting Nickname");
            //change the nicckname because this is sketch af
            PhotonNetwork.NickName = "uhhhh, Hello?";

            Debug.Log("Load Factory");
            //You gotta force load the factory to be able to move and not lag, this will spawn a bunch of clones everyone can see, but sometimes you need to do this and will be alone
            //in the diving bell because they are already in the factory.
            SurfaceNetworkHandler.Instance.photonView.RPC("RPC_LoadScene", RpcTarget.All, (object)"FactoryScene");
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

                    //PrintLobbyMembers(lobbyByIndex);

                    //Debug.Log("Checking Lobby: " + lobbyByIndex.ToString() + " GameVersion: " + lobbyData1 + "Region: " + lobbyData2 + " Number of players: " + numLobbyMembers.ToString() + " / " + lobbyMemberLimit.ToString());

                    //
                    

                   //Debug.Log($"steam://joinlobby/2881650/{lobbyByIndex}/{owner}");

   

                    string cloudRegion = PhotonNetwork.CloudRegion;
                    bool flag = !string.IsNullOrEmpty(lobbyData2) && lobbyData2 == cloudRegion;
                    if (lobbyData1 == new BuildVersion(Application.version).ToMatchmaking() & flag && numLobbyMembers < 4)
                        array.Add((lobbyByIndex, iLobby));
                }
                Debug.Log((object)("Received SteamLobby Matchlist: " + param.m_nLobbiesMatching.ToString() + " Matching: " + array.Count.ToString()));
                if (array.Count > 0)
                {
                    //this.JoinLobby(SteamMatchmaking.GetLobbyByIndex(array.GetRandom<(CSteamID, int)>().Item2));
                }
                else
                {
                    Debug.Log((object)"Found No Matches hosting");
                    //UnityEngine.Object.FindObjectOfType<MainMenuHandler>().SilentHost();
                }
            }

            

        }
        public void PrintLobbyMembers(CSteamID lobbyID)
        {
            // Get the owner of the lobby
            CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(lobbyID);
            Debug.Log("Lobby Owner Steam ID: " + lobbyOwner.m_SteamID);

            // Get the total number of members in the lobby
            int memberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyID);
            Debug.Log("Total Members in Lobby: " + memberCount);

            // Iterate through members
            for (int i = 0; i < memberCount; ++i)
            {
                CSteamID memberID = SteamMatchmaking.GetLobbyMemberByIndex(lobbyID, i);
                Debug.Log("Member Steam ID at index " + i + ": " + memberID.m_SteamID);

                // You can use GetFriendPersonaName to get the nickname of the member
                string memberName = SteamFriends.GetFriendPersonaName(memberID);
                Debug.Log("Member Name at index " + i + ": " + memberName);
            }
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
