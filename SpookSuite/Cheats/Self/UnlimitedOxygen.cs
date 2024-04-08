using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class UnlimitedOxygen : ToggleCheat
    {
        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            Player.localPlayer.data.remainingOxygen = Player.localPlayer.data.maxOxygen;
        }
    }
}
