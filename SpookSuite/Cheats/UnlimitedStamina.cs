using HarmonyLib;
using SpookSuite.Cheats.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpookSuite.Cheats
{
    [HarmonyPatch]
    internal class UnlimitedStamina : ToggleCheat
    {

        public override void Update()
        {
            if (Player.localPlayer is null) return;

            Player.localPlayer.data.currentStamina = Player.localPlayer.gameObject.GetComponent<PlayerController>().maxStamina;
        }

    }
}
