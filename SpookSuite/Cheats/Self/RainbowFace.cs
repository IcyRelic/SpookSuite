using SpookSuite.Cheats.Core;
using SpookSuite.Util;

namespace SpookSuite.Cheats
{
    internal class RainbowFace : ToggleCheat, IVariableCheat<float>
    {
        RainbowController rgb = new RainbowController();

        public static float Value = 0.1f; //interval

        public override void Update()
        {
            if (!Enabled || Player.localPlayer is null) return;

            rgb.speed = Value;
            rgb.Update();

            Player.localPlayer.refs.visor.ApplyVisorColor(rgb.GetColor());
        }
    }
}
