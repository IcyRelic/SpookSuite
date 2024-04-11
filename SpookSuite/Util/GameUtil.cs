using HarmonyLib;
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
        public static void SpawnItem(byte itemId, bool equip = false) => SpawnItem(itemId, Player.localPlayer.data.groundPos, equip);
        public static void SpawnItem(byte itemId, Vector3 spawnPos, bool equip = false)
        {
            spawnPos += Player.localPlayer.transform.forward;
            spawnPos.y += 1;

            Pickup component = PhotonNetwork.Instantiate("PickupHolder", spawnPos, Random.rotation, 0, null).GetComponent<Pickup>();

            if(component == null)
            {
                Debug.LogError("Failed to spawn item");
                return;
            }

            component.ConfigurePickup(itemId, new ItemInstanceData(Guid.NewGuid()));



            if (equip) component.Interact(Player.localPlayer);
        }

        public static Item GetItemByName(String name)
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

    }
}
