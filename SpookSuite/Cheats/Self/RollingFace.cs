using SpookSuite.Cheats.Core;
using System;

namespace SpookSuite.Cheats
{
    internal class RollingFace : ToggleCheat, IVariableCheat<int>
    {
        public static float Value = 1; // speed
        private float rot;

        public override void Update()
        {
            if (!Enabled || Player.localPlayer is null) return;

            PlayerVisor v = Player.localPlayer.refs.visor;
            
            v.SetAllFaceSettings(v.hue.Value, v.visorColorIndex, v.visorFaceText.text, rot, v.FaceSize);

            rot += Value;
            Math.Clamp(rot, 0, 360);
        }
    }
}
