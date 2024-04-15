using ExitGames.Client.Photon;
using Photon.Pun;
using SpookSuite.Components;
using SpookSuite.Manager;
using SpookSuite.Util;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SpookSuite.Handler
{
    public class SpookPlayerHandler
    {
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

        public bool IsRPCBlocked() => photonPlayer is not null && rpcBlockedClients.Contains(steamId);

        public void BlockRPC()
        {
            if (IsRPCBlocked() || photonPlayer is null) return;
            rpcBlockedClients.Add(steamId);
        }

        public void UnblockRPC()
        {
            if(!IsRPCBlocked() || photonPlayer is null) return;
            rpcBlockedClients.Remove(steamId);
        }

        public void ToggleRPCBlock()
        {
            if(photonPlayer is null) return;
            if(IsRPCBlocked()) rpcBlockedClients.Remove(steamId);
            else rpcBlockedClients.Add(steamId);
        }

        private Queue<RPCData> GetRPCHistory()
        {
            if(!rpcHistory.ContainsKey(steamId)) 
                rpcHistory.Add(steamId, new Queue<RPCData>());
            return rpcHistory[steamId];
        }

        public int RPCsOnFile() => GetRPCHistory().Count;

        private List<RPCData> GetRecentRPCHistory(int seconds = 5) => GetRPCHistory().ToList().FindAll(r => r.IsRecent(seconds));
        private List<RPCData> GetSuspectedRPCHistory(string rpc, int seconds = 5) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.suspected);

        public bool HasSentRPCInLast(string rpc, int seconds) => GetRecentRPCHistory(seconds).FindAll(r => r.rpc.StartsWith(rpc)).Count > 0;
        public bool HasBeenSuspectedInLast(string rpc, int seconds) => GetRecentRPCHistory(seconds).FindAll(r => r.rpc.StartsWith(rpc) && r.suspected).Count > 0;
        public void OnReceivedRPC(string rpc, Hashtable rpcHash)
        {
            if (player is null || player.IsLocal) return;

            RPCData rpcData = new RPCData(photonPlayer, rpc, rpcHash);

            

            object[] parameters = (object[])null;
            if (rpcHash.ContainsKey(Patches.keyByteFour))
                parameters = (object[])rpcHash[Patches.keyByteFour];

    
            if (rpc.StartsWith("RPC_RequestCreatePickup") && !HasSentRPCInLast("RPC_ClearSlot", 3))
            {
               
                rpcData.SetSuspected(parameters[0]);
                Log.Error($"{photonPlayer.NickName} is probably spawning items.");
            }

            if(rpc.Equals("RPCA_SpawnDrone") && !HasSentRPCInLast("RPCA_AddItemToCart", 60))
            {
                rpcData.SetSuspected(parameters[0]);
                Log.Error($"{photonPlayer.NickName} is probably spawning items WITH DRONES.");
            }

            if (rpc.Equals("RPC_ConfigurePickup"))
            {
                bool spawnSuspect = HasBeenSuspectedInLast("RPC_RequestCreatePickup", 3);
                bool droneSuspect = HasBeenSuspectedInLast("RPCA_SpawnDrone", 10);

                if (spawnSuspect || droneSuspect)
                {
                    ItemInstanceData data = new ItemInstanceData(Guid.Empty);
                    data.Deserialize((byte[])parameters[1]);
                    byte itemID = (byte)parameters[0];

                    RPCData suspect = spawnSuspect ?
                        GetSuspectedRPCHistory("RPC_RequestCreatePickup", 6).Find(r => (byte)r.suspectedData == itemID) :
                        GetSuspectedRPCHistory("RPCA_SpawnDrone", 6).Find(r => ((byte[])r.suspectedData).Contains(itemID));

                    if (suspect is not null)
                    {
                        //Spawned Item, Delete it
                        rpcData.SetSuspected(data.m_guid);
                        Log.Error("Spawned Item Detected. Sending RPC_Remove...");

                        SpookSuite.Invoke(() =>
                        {
                            Pickup pickup = GameUtil.GetPickupByGuid(data.m_guid);
                            pickup.m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient);
                        }, 1);
                    }

                }
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
