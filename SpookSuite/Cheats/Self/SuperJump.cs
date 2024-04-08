using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class SuperJump : ToggleCheat, IVariableCheat<float>
    {
        public float Value = 10f;

        public override void Update()
        {
            if (Player.localPlayer is null) return;
           

            Player.localPlayer.gameObject.GetComponent<PlayerController>().jumpForceOverTime = Enabled ? Value : 0.6f;


        }
    }
}
