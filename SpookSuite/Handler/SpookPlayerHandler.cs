using DefaultNamespace.Artifacts;
using ExitGames.Client.Photon;
using Photon.Pun;
using SpookSuite.Components;
using SpookSuite.Manager;
using SpookSuite.Util;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Handler
{
    public class SpookPlayerHandler
    {
        private static List<ulong> spookSuiteClients = new List<ulong>();
        private static List<ulong> rpcBlockedClients = new List<ulong>();
        private static Dictionary<ulong, Queue<RPCData>> rpcHistory = new Dictionary<ulong, Queue<RPCData>>();

        private Player player;
        public Photon.Realtime.Player photonPlayer => player.refs.view.Owner;
        public ulong steamId => player.GetSteamID().m_SteamID;

        public SpookPlayerHandler(Player player)
        {
            this.player = player;
        }

        public static void ClearRPCHistory() => rpcHistory.Clear();

        public Bot GetClosestMonster() => GameObjectManager.monsters.OrderBy(x => Vector3.Distance(x.transform.position, player.transform.position)).FirstOrDefault();

        public Player GetClosestPlayer() => PlayerHandler.instance.playerAlive.Where(x => x.GetInstanceID() != player.GetInstanceID()).OrderBy(x => Vector3.Distance(x.transform.position, player.transform.position)).FirstOrDefault();

        public void RPC(string name, RpcTarget target, params object[] args) => player.refs.view.RPC(name, target, args);

        public bool IsRPCBlocked() => photonPlayer is not null && rpcBlockedClients.Contains(steamId) && !IsDev();

        public bool IsDev() => Player.localPlayer.GetSteamID().m_SteamID == ((long)76561199159991462 | (long)76561198093261109);   

        public bool IsSpookUser() => photonPlayer is not null && spookSuiteClients.Contains(steamId);

        public void BlockRPC()
        {
            if (IsRPCBlocked() || photonPlayer is null && IsDev()) return;
            rpcBlockedClients.Add(steamId);
        }

        public void UnblockRPC()
        {
            if(!IsRPCBlocked() || photonPlayer is null) return;
            rpcBlockedClients.Remove(steamId);
        }

        public void ToggleRPCBlock()
        {
            if(photonPlayer is null && IsDev()) return;
            if(IsRPCBlocked()) rpcBlockedClients.Remove(steamId);
            else rpcBlockedClients.Add(steamId);
        }

        private Queue<RPCData> GetRPCHistory()
        {
            if(!rpcHistory.ContainsKey(steamId)) 
                rpcHistory.Add(steamId, new Queue<RPCData>());
            return rpcHistory[steamId];
        }
        private List<RPCData> GetRPCHistory(string rpc) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc));

        private List<RPCData> GetRPCHistory(string rpc, int seconds) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds));
        private List<RPCData> GetRPCHistory(string rpc, int seconds, bool suspected) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.suspected == suspected);
        private RPCData GetRPCMatch(string rpc, int seconds, object data) => GetRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data));
        private RPCData GetRPCMatch(string rpc, int seconds, object data, bool suspected) => GetRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data) && r.suspected == suspected);
        private RPCData GetRPCMatch(string rpc, int seconds, Func<object, bool> predicate) => GetRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data));
        private RPCData GetRPCMatch(string rpc, int seconds, Func<object, bool> predicate, bool suspected) => GetRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data) && r.suspected == suspected);
        private bool HasSentRPC(string rpc, int seconds) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds)).Count > 0;
        private bool HasSentRPC(string rpc, int seconds, bool suspected) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.suspected == suspected).Count > 0;
        private bool HasSentRPC(string rpc, int seconds, object data) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data)).Count > 0;
        private bool HasSentRPC(string rpc, int seconds, Func<object, bool> predicate) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data)).Count > 0;
        private bool HasSentRPC(string rpc, int seconds, Func<object, bool> predicate, bool suspected) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data) && r.suspected == suspected).Count > 0;
        private List<RPCData> GetAllRPCHistory() => rpcHistory.Values.SelectMany(x => x).ToList();
        private List<RPCData> GetAllRPCHistory(int seconds) => GetAllRPCHistory().FindAll(r => r.IsRecent(seconds));
        private List<RPCData> GetAllRPCHistory(string rpc, int seconds) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds));
        private List<RPCData> GetAllRPCHistory(string rpc, int seconds, bool suspected) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.suspected == suspected);
        private RPCData GetAnyRPCMatch(string rpc, int seconds, object data) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data));
        private RPCData GetAnyRPCMatch(string rpc, int seconds, object data, bool suspected) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data) && r.suspected == suspected);
        private RPCData GetAnyRPCMatch(string rpc, int seconds, Func<object, bool> predicate) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data));
        private RPCData GetAnyRPCMatch(string rpc, int seconds, Func<object, bool> predicate, bool suspected) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data) && r.suspected == suspected);
        private bool HasAnySentRPC(string rpc, int seconds) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds)).Count > 0;
        private bool HasAnySentRPC(string rpc, int seconds, bool suspected) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.suspected == suspected).Count > 0;
        private bool HasAnySentRPC(string rpc, int seconds, object data) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data)).Count > 0;
        private bool HasAnySentRPC(string rpc, int seconds, Func<object, bool> predicate) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data)).Count > 0;
        private bool HasAnySentRPC(string rpc, int seconds, Func<object, bool> predicate, bool suspected) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data) && r.suspected == suspected).Count > 0;
        
        public void OnReceivedRPC(string rpc, Hashtable rpcHash)
        {
            if (player is null || photonPlayer is null) return;

            RPCData rpcData = new RPCData(photonPlayer, rpc, rpcHash);

            object[] parameters = (object[])null;
            if (rpcHash.ContainsKey(Patches.keyByteFour))
                parameters = (object[])rpcHash[Patches.keyByteFour];

            if (rpc.StartsWith("RPC_MakeSound") && (int)parameters[0] == int.MaxValue)
                spookSuiteClients.Add(steamId);

            if (rpc.StartsWith("RPC_RequestCreatePickup") && !HasSentRPC("RPC_ClearSlot", 3) && !player.IsLocal)
            {
                ItemInstanceData data = new ItemInstanceData(Guid.Empty);
                data.Deserialize((byte[])parameters[1]);

                rpcData.SetSuspected(data.m_guid);
                Log.Error($"{photonPlayer.NickName} is probably spawning items. => Item ID: {parameters[0]} | Guid: {data.m_guid}");
            }

            if(rpc.Equals("RPCA_SpawnDrone"))
            {
                rpcData.data = parameters[0];
                if(!HasSentRPC("RPCA_AddItemToCart", 60))
                {
                    rpcData.SetSuspected();

                    if(!player.IsLocal)
                        Log.Error($"{photonPlayer.NickName} is probably spawning items WITH DRONES.");
                }
            }

            if (rpc.Equals("RPC_ClearSlot"))
            {
                player.TryGetInventory(out PlayerInventory inventory);
                inventory.TryGetItemInSlot((int)parameters[0], out ItemDescriptor item);

                rpcData.data = item.item.id;
                Log.Info($"{photonPlayer.NickName} cleared slot {parameters[0]} with item {item.item.id}");
            }

            if (rpc.Equals("RPC_ConfigurePickup"))
            {
                ItemInstanceData data = new ItemInstanceData(Guid.Empty);
                data.Deserialize((byte[])parameters[1]);
                byte itemID = (byte)parameters[0];

                bool spawnSuspected = !HasAnySentRPC("RPC_ClearSlot", 5);
                bool droneSuspected = HasAnySentRPC("RPCA_SpawnDrone", 10, true);
                RPCData? suspected = GetRPCMatch("RPCA_SpawnDrone", 10, x => (x as byte[]).Contains(itemID));
                

                if (GameObjectManager.allowedSpawns.ContainsKey(data.m_guid))
                {
                    //SpookSuite Spawn
                    bool equip = GameObjectManager.allowedSpawns.GetValueOrDefault(data.m_guid);
                    if(equip)
                        SpookSuite.Invoke(() => GameUtil.GetPickupByGuid(data.m_guid).Interact(Player.localPlayer), 0.2f);

                    GameObjectManager.allowedSpawns.Remove(data.m_guid);
                } 
                else if (droneSuspected && GameObjectManager.allowedDroneSpawns.ContainsKey(itemID))
                {
                    //SpookSuite Spawn
                    GameObjectManager.allowedDroneSpawns.TryGetValue((byte)itemID, out object[] x);

                    x[1] = (int)x[1]-1;

                    if ((bool)x[0])
                        SpookSuite.Invoke(() => GameUtil.GetPickupByGuid(data.m_guid).Interact(Player.localPlayer), 0.2f);

                    if ((int)x[1] < 1) GameObjectManager.allowedDroneSpawns.Remove((byte)itemID);
                }

                else if(GetAnyRPCMatch("RPC_ClearSlot", 5, itemID) is null || (droneSuspected && suspected is not null && !suspected.sender.IsLocal))
                {
                    //Spawned Item, Delete it
                    rpcData.SetSuspected(itemID);
                    if(droneSuspected)
                        rpcData.parent = suspected;

                    bool goodSpawn = false;

                    float elapsedTime = Time.time - Time.timeSinceLevelLoad;

                    if ( (itemID == GameUtil.GetItemByName("camera").id && !HasAnySentRPC("RPC_ConfigurePickup", 15, itemID)) ||
                        (!GameObjectManager.divingBell.onSurface && elapsedTime > 30) )
                        goodSpawn = true;

                    if(!goodSpawn)
                    {
                        Log.Error($"Spawned Item Detected. => ItemID: {itemID} Suspected: {suspected?.rpc} Guid: {data.m_guid} | Spawned By: {suspected?.sender.NickName}");

                        SpookSuite.Invoke(() =>
                        {

                            Pickup pickup = GameUtil.GetPickupByGuid(data.m_guid);

                            Log.Error($"Sending RPC_Remove for spawned item {data.m_guid}");
                            pickup.m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient);


                        }, 1f);
                    } else Log.Warning($"Host Spawn Detected As Good Spawn => {data.m_guid}");

                    
                }
                else Log.Warning($"Good Spawn => {data.m_guid}");              
            }
   
            GetRPCHistory().Enqueue(rpcData);
            CleanupRPCHistory();
        }

        private void CleanupRPCHistory()
        {
            var queue = GetRPCHistory();
            while(queue.Count > 0 && queue.Peek().IsExpired()) queue.Dequeue();
        }
    }

    public static class PlayerExtensions
    {
        public static SpookPlayerHandler Handle(this Player player) => new SpookPlayerHandler(player);
        public static Photon.Realtime.Player PhotonPlayer(this Player player) => player.refs.view.Owner;

        public static CSteamID GetSteamID(this Player player) => player.refs.view.Owner.GetSteamID();
    }

    public static class PhotonPlayerExtensions
    {
        public static CSteamID GetSteamID(this Photon.Realtime.Player photonPlayer)
        {
            bool success = SteamAvatarHandler.TryGetSteamIDForPlayer(photonPlayer, out CSteamID steamid);
            return steamid;
        }
        public static Player GamePlayer(this Photon.Realtime.Player photonPlayer) => PlayerHandler.instance.players.Find(x => x.PhotonPlayer().ActorNumber == photonPlayer.ActorNumber);
    }
}
