using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Components;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class AntiSpawner : ToggleCheat
    {
        internal void Process(SpookPlayerHandler handler, object[] parameters, ref RPCData rpcData)
        {
            ItemInstanceData data = new ItemInstanceData(Guid.Empty);
            data.Deserialize((byte[])parameters[1]);
            byte itemID = (byte)parameters[0];

            bool spawnSuspected = !handler.HasAnySentRPC("RPC_ClearSlot", 5);
            bool droneSuspected = handler.HasAnySentRPC("RPCA_SpawnDrone", 10, true);
            RPCData? suspected = handler.GetRPCMatch("RPCA_SpawnDrone", 10, x => (x as byte[]).Contains(itemID));


            if (GameObjectManager.allowedSpawns.ContainsKey(data.m_guid))
            {
                //SpookSuite Spawn
                bool equip = GameObjectManager.allowedSpawns.GetValueOrDefault(data.m_guid);
                if (equip)
                    SpookSuite.Invoke(() => GameUtil.GetPickupByGuid(data.m_guid).Interact(Player.localPlayer), 0.2f);

                GameObjectManager.allowedSpawns.Remove(data.m_guid);
            }
            else if (droneSuspected && GameObjectManager.allowedDroneSpawns.ContainsKey(itemID))
            {
                //SpookSuite Spawn
                GameObjectManager.allowedDroneSpawns.TryGetValue((byte)itemID, out object[] x);

                x[1] = (int)x[1] - 1;

                if ((bool)x[0])
                    SpookSuite.Invoke(() => GameUtil.GetPickupByGuid(data.m_guid).Interact(Player.localPlayer), 0.2f);

                if ((int)x[1] < 1) GameObjectManager.allowedDroneSpawns.Remove((byte)itemID);
            }

            else if (handler.GetAnyRPCMatch("RPC_ClearSlot", 5, itemID) is null || (droneSuspected && suspected is not null && !suspected.sender.IsLocal))
            {
                //Spawned Item, Delete it
                rpcData.SetSuspected(itemID);
                if (droneSuspected)
                    rpcData.parent = suspected;

                bool goodSpawn = false;

                float elapsedTime = Time.time - Time.timeSinceLevelLoad;

                if ((itemID == GameUtil.GetItemByName("camera").id && !handler.HasAnySentRPC("RPC_ConfigurePickup", 15, itemID)) ||
                    (!GameObjectManager.divingBell.onSurface && elapsedTime > 30))
                    goodSpawn = true;

                if (!goodSpawn)
                {
                    Log.Error($"Spawned Item Detected. => ItemID: {itemID} Suspected: {suspected?.rpc} Guid: {data.m_guid} | Spawned By: {suspected?.sender.NickName}");

                    if(Enabled) InvokeRemoveSpawn(data);
                    else Log.Warning($"AntiSpawning Disabled; Allowing Spawn => {data.m_guid}");                  
                }
                else Log.Warning($"Host Spawn Detected As Good Spawn => {data.m_guid}");
            }
            else Log.Warning($"Good Spawn => {data.m_guid}");
        }

        private void InvokeRemoveSpawn(ItemInstanceData data)
        {
            SpookSuite.Invoke(() =>
            {
                Pickup pickup = GameUtil.GetPickupByGuid(data.m_guid);

                Log.Error($"Sending RPC_Remove for spawned item {data.m_guid}");
                pickup.m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient);
            }, 1f);
        }
    }
}
