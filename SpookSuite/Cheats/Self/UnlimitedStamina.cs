using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class UnlimitedStamina : ToggleCheat
    {
        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled || Player.localPlayer.refs.controller is null) return;

            Player.localPlayer.data.currentStamina = Player.localPlayer.refs.controller.maxStamina;
        }

    }
}