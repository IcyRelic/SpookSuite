using SpookSuite.Menu.Core;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class SelfTab : MenuTab
    {
        public SelfTab() : base("Self") { }

        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.Label(name); //doing it like this so we could just copy paste it over
            MenuContent();
            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();

            UI.Checkbox("Godmode", ref Cheats.Godmode.Enabled);
            UI.Checkbox("Unlimited Oxygen", ref Cheats.UnlimitedOxygen.Enabled);
            UI.Checkbox("Unlimited Stamina", ref Cheats.UnlimitedStamina.Enabled);
            UI.ToggleSlider("Super Speed", "Speed", ref Cheats.SuperSpeed.Enabled, ref Cheats.SuperSpeed.Value, 10f, 100f);

        }
    }
}
