using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class SuperSpeed : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 60f;
        private static bool wasEnabled = false;
        public override void Update()
        {
            if (Player.localPlayer is null) return;

            if(Enabled) 
            {
                wasEnabled = true;
                Player.localPlayer.gameObject.GetComponent<PlayerController>().movementForce = Value; 
            }
            else if (wasEnabled)
            {
                wasEnabled = false;
                Player.localPlayer.gameObject.GetComponent<PlayerController>().movementForce = 10f;
            }
        }

    }
}
