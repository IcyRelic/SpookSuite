using HarmonyLib;
using Photon.Pun;
using SpookSuite.Cheats.Core;
using System;
using UnityEngine;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class InfiniteJump : ToggleCheat //doesnt work
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerController), "TryJump")]
        public static bool TryJump(PlayerController __instance)
        {
            if (Instance<InfiniteJump>().Enabled)
            {
                Player.localPlayer.refs.view.RPC("RPCA_Jump", RpcTarget.All, Array.Empty<object>());
                return false;
            }
            return true;
        }
    }
}
