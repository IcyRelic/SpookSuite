using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class SuperJump : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 10f;

        public override void Update()
        {
            if (Player.localPlayer is null) return;
           
            Player.localPlayer.gameObject.GetComponent<PlayerController>().jumpForceOverTime = Enabled ? Value : 0.6f;
        }
    }
}
