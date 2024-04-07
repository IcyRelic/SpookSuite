using HarmonyLib;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class Godmode : ToggleCheat
    {
        public override void Update()
        {
            if (Player.localPlayer is null && !enabled) return;

            
        }

    }
}
