using HarmonyLib;
using Photon.Pun;
using SpookSuite.Manager;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;
using Object = UnityEngine.Object;

namespace SpookSuite
{
    [HarmonyPatch]
    internal static class Patches
    {
        public static List<byte> waitingForItemSpawn = new List<byte>();
        public static List<Guid> allowedBombs = new List<Guid>();

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Pickup), "RPC_ConfigurePickup")]
        public static void interactWithPickups(Pickup __instance, byte itemID, byte[] data)
        {
            if(itemID == GameUtil.GetItemByName("bomb").id)
            {
                Attacks_Bombs bombsEnemy = Object.FindAnyObjectByType<Attacks_Bombs>();

                if((GameObjectManager.divingBell is not null && GameObjectManager.divingBell.onSurface) || bombsEnemy is null)
                {
                    if (allowedBombs.Contains(__instance.itemInstance.m_guid.Value))
                    {
                        //SpookSuite Bomb
                        allowedBombs.Remove(__instance.itemInstance.m_guid.Value);
                        return;
                    }
                    //maybe add a check later to see if attack bombs has attacked
                    Debug.Log("Bombing Detected, Removing...");

                    __instance.m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient);
                }
                
            }

            if(!waitingForItemSpawn.Contains(itemID)) return;

            __instance.Interact(Player.localPlayer);
            waitingForItemSpawn.Remove(itemID);
        }





    }
}
