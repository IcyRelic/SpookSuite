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

        private List<RPCData> GetAllRPCHistory()
        {
            List<RPCData> hist = new List<RPCData>();

            rpcHistory.Values.ToList().ForEach(q => hist.AddRange(q.ToList()));

            return hist;
        }
        public int RPCsOnFile() => GetRPCHistory().Count;

        private List<RPCData> GetRecentRPCHistory(int seconds = 5) => GetRPCHistory().ToList().FindAll(r => r.IsRecent(seconds));
        private List<RPCData> GetAllRecentRPCHistory(int seconds = 5) => GetAllRPCHistory().FindAll(r => r.IsRecent(seconds));
        private List<RPCData> GetSuspectedRPCHistory(string rpc, int seconds = 5) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.suspected);

        private List<RPCData> GetAllSuspectedRPCHistory(string rpc, int seconds = 5) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.suspected);
        private RPCData GetAnySuspectedMatch(string rpc, int seconds = 5, object data = null) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.suspected && r.data == data);
        private RPCData GetAnyMatch(string rpc, int seconds = 5, object data = null) //=> GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.data == data);
        {
            foreach (var item in GetAllRecentRPCHistory(seconds))
            {
                if (item.rpc.StartsWith(rpc)) return item;
            }
            return null;
        }

        public bool HasSentRPCInLast(string rpc, int seconds) => GetRecentRPCHistory(seconds).FindAll(r => r.rpc.StartsWith(rpc)).Count > 0;
        public bool HasAnySentInLast(string rpc, int seconds) //GetAllRecentRPCHistory(seconds).FindAll(r => r.rpc.StartsWith(rpc)).Count > 0;
        {
            List<RPCData> data = GetAllRecentRPCHistory(seconds);
            foreach (var item in data)
            {
                if (item.rpc.StartsWith(rpc)) return true;
            }

            return false;
        }
        public bool HasBeenSuspectedInLast(string rpc, int seconds) => GetRecentRPCHistory(seconds).FindAll(r => r.rpc.StartsWith(rpc) && r.suspected).Count > 0;
        public bool HasAnyBeenSuspectedInLast(string rpc, int seconds) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.suspected).Count > 0;
        public void OnReceivedRPC(string rpc, Hashtable rpcHash)
        {
            if (player is null || photonPlayer is null) return;

            RPCData rpcData = new RPCData(photonPlayer, rpc, rpcHash);

            object[] parameters = (object[])null;
            if (rpcHash.ContainsKey(Patches.keyByteFour))
                parameters = (object[])rpcHash[Patches.keyByteFour];

            if (rpc.StartsWith("RPC_MakeSound") && (int)parameters[0] == int.MaxValue)
            {
                spookSuiteClients.Add(steamId);
            }

            if (rpc.StartsWith("RPC_RequestCreatePickup") && !HasSentRPCInLast("RPC_ClearSlot", 3) && !player.IsLocal)
            {
                ItemInstanceData data = new ItemInstanceData(Guid.Empty);
                data.Deserialize((byte[])parameters[1]);

                rpcData.SetSuspected(data.m_guid);
                Log.Error($"{photonPlayer.NickName} is probably spawning items. => Item ID: {parameters[0]} | Guid: {data.m_guid}");
            }

            if(rpc.Equals("RPCA_SpawnDrone") && !HasSentRPCInLast("RPCA_AddItemToCart", 60) && !player.IsLocal)
            {
                rpcData.SetSuspected(parameters[0]);
                Log.Error($"{photonPlayer.NickName} is probably spawning items WITH DRONES.");
            }

            if (rpc.Equals("RPC_ClearSlot"))
            {
                player.TryGetInventory(out PlayerInventory inventory);
                inventory.TryGetItemInSlot((int)parameters[0], out ItemDescriptor item);

                rpcData.data = item.item.id;
            }

            if (rpc.Equals("RPC_ConfigurePickup"))
            {
                ItemInstanceData data = new ItemInstanceData(Guid.Empty);
                data.Deserialize((byte[])parameters[1]);
                byte itemID = (byte)parameters[0];

                bool spawnSuspected = !HasAnySentInLast("RPC_ClearSlot", 5);
                bool droneSuspected = HasAnyBeenSuspectedInLast("RPCA_SpawnDrone", 10);

                RPCData? suspected = GetAnySuspectedMatch("RPCA_SpawnDrone", 10, data.m_guid);

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

                else if (droneSuspected || GetAnyMatch("RPC_ClearSlot", 5, itemID) is null)
                {
                    //Spawned Item, Delete it
                    rpcData.SetSuspected(data.m_guid);
                    if(droneSuspected)
                        rpcData.parent = suspected;

                    Log.Error($"Spawned Item Detected. => Suspected: {suspected?.rpc} Guid: {data.m_guid} | Spawned By: {suspected?.sender.NickName}");

                    SpookSuite.Invoke(() =>
                    {
                        
                        Pickup pickup = GameUtil.GetPickupByGuid(data.m_guid);

                        Log.Error($"Sending RPC_Remove for spawned item {data.m_guid}");
                        pickup.m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient);


                    }, 1f);
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
