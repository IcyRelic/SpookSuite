using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class SuperSpeed : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 60f;
        public static readonly float defaultValue = 16f;

        public override void Update()
        {
            if (Player.localPlayer is null) return;

            Player.localPlayer.gameObject.GetComponent<PlayerController>().movementForce = Enabled ? Value : defaultValue;
        }

        public override void OnDisable()
        {
            Player.localPlayer.gameObject.GetComponent<PlayerController>().movementForce = defaultValue;
        }
    }
}
