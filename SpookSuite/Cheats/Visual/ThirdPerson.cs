using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using Vector3 = UnityEngine.Vector3;

namespace SpookSuite.Cheats
{
    internal class ThirdPerson : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 10;

        public override void Update()
        {
            if (Player.localPlayer is null)
                return;

            Player.localPlayer.GetComponentInChildren<HeadFollower>().Reflect().SetValue("offset", new Vector3(0, Enabled ? 0 : 1.53f, Enabled ? -Value : 0));
        }
    }
}
