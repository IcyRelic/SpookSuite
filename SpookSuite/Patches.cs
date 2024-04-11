using HarmonyLib;
using Photon.Pun;
using SpookSuite.Menu.Tab;
using SpookSuite.Util;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;

namespace SpookSuite
{
    [HarmonyPatch]
    internal static class Patches
    {
        public static List<byte> waitingForItemSpawn = new List<byte>();

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Pickup), "RPC_ConfigurePickup")]
        public static void interactWithPickups(Pickup __instance, byte itemID, byte[] data)
        {
            if(!waitingForItemSpawn.Contains(itemID)) return;

            __instance.Interact(Player.localPlayer);
            waitingForItemSpawn.Remove(itemID);
        }



    }
}
