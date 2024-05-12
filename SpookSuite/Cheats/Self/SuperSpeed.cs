using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class SuperSpeed : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 60f;

        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            Player.localPlayer.gameObject.GetComponent<PlayerController>().movementForce = Value;
        }

        public override void OnDisable()
        {
            Player.localPlayer.gameObject.GetComponent<PlayerController>().movementForce = 17f;
        }
    }
}
