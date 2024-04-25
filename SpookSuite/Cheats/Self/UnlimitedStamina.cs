using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class UnlimitedStamina : ToggleCheat
    {
        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            Player.localPlayer.data.currentStamina = Player.localPlayer.gameObject.GetComponent<PlayerController>().maxStamina;
        }

    }
}
