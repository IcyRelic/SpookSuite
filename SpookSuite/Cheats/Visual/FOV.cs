using SpookSuite.Cheats.Core;
using SpookSuite.Util;

namespace SpookSuite.Cheats
{
    internal class FOV : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 70f;

        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled)
                return;

            MainCamera.instance.Reflect().SetValue("baseFOV", Value);
        }
        public override void OnDisable()
        {
            MainCamera.instance.Reflect().SetValue("baseFOV", 70f);
        }
    }
}
