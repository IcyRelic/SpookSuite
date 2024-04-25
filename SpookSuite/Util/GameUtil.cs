using Photon.Pun;
using SpookSuite.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpookSuite.Util
{
    public class GameUtil
    {
        public static List<string> monterNames;
        private static bool OverridingPhotonLocalPlayer = false;

        public static void LoadMonterNames()
        {
            monterNames = new List<string>();
            GameObject[] enemies = Resources.LoadAll<GameObject>("").Where(x => x.GetComponent<Player>() != null && x.GetComponent<Player>().ai).ToArray();

            enemies.ToList().ForEach(x => monterNames.Add(x.name));
            Debug.Log($"Loaded {monterNames.Count} monsters");
        }
        public static void SpawnItem(byte itemId, bool equip = false, bool drone = false, int amount = 1) => SpawnItem(itemId, Player.localPlayer.data.groundPos, equip, drone, amount);
        public static void SpawnItem(byte itemId, Vector3 spawnPos, bool equip = false, bool drone = false, int amount = 1)
        {
            amount = Math.Clamp(amount, 1, 1000);
            if(drone)
            {
                if (!ShopHandler.Instance) return;

                byte[] items = new byte[amount];

                for (int i = 0; i < amount; i++) items[i] = (byte)itemId;

                bool b = GameObjectManager.allowedDroneSpawns.TryGetValue((byte) itemId, out object[] x);
                
                if (b) x[1] = (int)x[1] + amount;
                else x = new object[] { equip, amount };

                GameObjectManager.allowedDroneSpawns.TryAdd(itemId, x);    
                ShopHandler.Instance.GetComponent<PhotonView>().RPC("RPCA_SpawnDrone", RpcTarget.All, items);
            }
            else
            {
                spawnPos += Player.localPlayer.transform.forward;
                spawnPos.y += 1;

                for(int i = 0;i < amount; i++)
                {
                    ItemInstanceData data = new ItemInstanceData(Guid.NewGuid());
                    byte[] array = data.Serialize(false);
                    Debug.Log($"Spawning item {itemId} at {spawnPos} => equip: {equip}"); 
                    GameObjectManager.allowedSpawns.Add(data.m_guid, equip);
                    Player.localPlayer.refs.view.RPC("RPC_RequestCreatePickup", RpcTarget.MasterClient, (object)itemId, (object)array, (object)spawnPos, (object)Random.rotation);
                }                                    
            }
        }

        public static Pickup GetPickupByGuid(Guid guid) => PickupHandler.Instance.Reflect().GetValue<List<Pickup>>("m_pickup").Find(p => p.itemInstance.m_guid.Value == guid);

        public static Item GetItemByName(string name)
        {
            return ItemDatabase.Instance.Objects.ToList().Find(x => x.GetName().ToLower() == name.ToLower());
        }

        public static Item GetItemById(byte id)
        {
            return ItemDatabase.Instance.Objects.ToList().Find(x => x.id == id);
        }

        public static bool IsOverridingPhotonLocalPlayer()
        {
            return OverridingPhotonLocalPlayer;
        }

        public static void ToggleOverridePhotonLocalPlayer()
        {
            OverridingPhotonLocalPlayer = !OverridingPhotonLocalPlayer;
        }

        public static void AdvanceDay()
        {
            if (!SurfaceNetworkHandler.Instance) return;

            if (!SurfaceNetworkHandler.Instance.Reflect().GetValue<bool>("m_Started", true))
                SurfaceNetworkHandler.Instance.RequestStartGame();

            SurfaceNetworkHandler.Instance.Reflect().GetValue<PhotonView>("m_View").RPC("RPCA_Sleep", RpcTarget.All);
        }

        public static void SendHospitalBill(int amount)
        {
            var p = PhotonNetwork.PlayerListOthers.ToList().Count > 0 ? PhotonNetwork.PlayerListOthers.ToList().First() : PhotonNetwork.LocalPlayer;
            List<(int, int)> bill = [(p.ActorNumber, amount)];

            SurfaceNetworkHandler.Instance.Reflect().Invoke("SendHospitalBill", bill);
        }
    }
}
