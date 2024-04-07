using SpookSuite.Cheats.Core;
using SpookSuite.Util;

namespace SpookSuite.Cheats
{
    internal class FOV : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 70f;
        private static bool Wasenabled = false;
        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled)
            {
                if (Wasenabled)
                {
                    MainCamera.instance.Reflect().SetValue("baseFOV", 70f);
                    Wasenabled = false;
                    return;
                }
                else return;
            }

            Wasenabled = true;
            MainCamera.instance.Reflect().SetValue("baseFOV", Value);
        }
    }
}
