using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class RainbowFace : ToggleCheat, IVariableCheat<float>
    {

        private List<RGBAColor> colors = new List<RGBAColor>()
        {
            new RGBAColor(255, 0, 0, 1), //RED
            new RGBAColor(255, 165, 0, 1), //ORANGE
            new RGBAColor(255, 255, 0, 1), //YELLOW
            new RGBAColor(0, 128, 0, 1), //GREEN
            new RGBAColor(0, 0, 255, 1), //BLUE
            new RGBAColor(75, 0, 130, 1), //INDIGO
            new RGBAColor(238, 130, 238, 1) //VIOLET
        };

        public static float Value = 0.1f; //interval
        private int index = 0;
        private float lastChange = 0;

        public override void Update()
        {
            if(!Enabled || Player.localPlayer is null) return;

            if(Time.time - lastChange > Value)
            {
                lastChange = Time.time;
                Player.localPlayer.refs.visor.ApplyVisorColor(colors[index].GetColor());
                index = (index + 1) % colors.Count;
            }

        }
    }
}
