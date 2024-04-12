using HarmonyLib;
using Newtonsoft.Json.Linq;
using Photon.Pun;
using Photon.Voice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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
        public static void SpawnItem(byte itemId, bool equip = false, bool drone = false) => SpawnItem(itemId, Player.localPlayer.data.groundPos, equip, drone);
        public static void SpawnItem(byte itemId, Vector3 spawnPos, bool equip = false, bool drone = false)
        {
            if(drone)
            {
                if (!ShopHandler.Instance) return;
                ShopHandler.Instance.GetComponent<PhotonView>().RPC("RPCA_SpawnDrone", RpcTarget.All, new byte[] {itemId});
            }
            else
            {
                spawnPos += Player.localPlayer.transform.forward;
                spawnPos.y += 1;
                ItemInstanceData data = new ItemInstanceData(Guid.NewGuid());
                byte[] array = data.Serialize(false);

                Patches.allowedBombs.Add(data.m_guid);

                if(equip) Patches.waitingForItemSpawn.Add(itemId);

                Player.localPlayer.refs.view.RPC("RPC_RequestCreatePickup", RpcTarget.MasterClient, (object)itemId, (object)array, (object)spawnPos, (object)Random.rotation);                
            }
        }

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
