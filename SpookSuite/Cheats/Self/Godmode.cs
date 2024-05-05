using DefaultNamespace;
using HarmonyLib;
using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using UnityEngine;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class Godmode : ToggleCheat
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "TakeDamage")]
        public static bool TakeDamage(Player __instance)
        {
            if (Instance<Godmode>().Enabled && __instance.IsLocal)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "Die")]
        public static bool Die(Player __instance)
        {
            if (Instance<Godmode>().Enabled && __instance.IsLocal)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), "CallDie")]
        public static bool CallDie(Player __instance)
        {
            if (Instance<Godmode>().Enabled && __instance.IsLocal)
                return false;
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Bot_Slurper), "RPCA_AttachBlob")]
        public static bool PreventSlurp(Bot_Slurper __instance, int viewID, int bodyPartID)
        {
            Player player = PlayerHandler.instance.Reflect().Invoke<Player>("TryGetPlayerFromViewID", args: viewID);

            if(player.IsLocal && Instance<Godmode>().Enabled)
            {
                __instance.Reflect().GetValue<PhotonView>("view_g").RPC("RPCA_ReleasePlayer", RpcTarget.Others);
                return false;
            }

            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "RPCA_PlayerDie")]
        public static void RPCA_PlayerDieRevive(Player __instance)
        {
            if (Instance<Godmode>().Enabled && __instance.IsLocal)
            {
                Debug.LogWarning("Sending Revive");
                __instance.CallRevive();
            }
        }
    }
}
