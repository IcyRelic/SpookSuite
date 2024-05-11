using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class SuperJump : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 10f;

        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;
           
            Player.localPlayer.gameObject.GetComponent<PlayerController>().jumpForceOverTime = Value;
        }

        public override void OnDisable()
        {
            Player.localPlayer.refs.controller.jumpForceOverTime = .6f;
        }
    }
}
