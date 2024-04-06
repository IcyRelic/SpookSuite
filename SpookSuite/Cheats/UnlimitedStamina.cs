using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class UnlimitedStamina : Cheat
    {

        public override void Update()
        {
            if (Player.localPlayer is null) return;

            Player.localPlayer.data.currentStamina = Player.localPlayer.gameObject.GetComponent<PlayerController>().maxStamina;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerController), "LateUpdate")]
        public static void PlayerControllerLateUpdate(PlayerController __instance)
        {
            if (Player.localPlayer is null) return;

            Player.localPlayer.data.currentStamina = __instance.maxStamina;
        }

    }
}
