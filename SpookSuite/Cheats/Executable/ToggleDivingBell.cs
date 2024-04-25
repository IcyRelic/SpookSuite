using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class ToggleDivingBell : ExecutableCheat
    {
        public override void Execute()
        {
            DivingBell bell = FindObjectOfType<DivingBell>();

            if (bell is null) return;

            bell.AttemptSetOpen(!bell.opened);
        }
    }
}
