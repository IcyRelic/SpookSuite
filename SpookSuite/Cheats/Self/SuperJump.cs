using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpookSuite.Cheats
{
    internal class SuperJump : ToggleCheat, IVariableCheat<float>
    {
        public float Value { get; set; } = 10f;

        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;
            

            Player.localPlayer.gameObject.GetComponent<PlayerController>().jumpForceOverTime = Value;


        }
    }
}
