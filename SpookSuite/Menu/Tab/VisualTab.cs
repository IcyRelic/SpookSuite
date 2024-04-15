using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Menu.Core;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class VisualTab : MenuTab
    {
        public VisualTab() : base("Visual") { }
        private Vector2 scrollPos = Vector2.zero;
        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.5f - SpookSuiteMenu.Instance.spaceFromLeft));
            VisualContent();
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.5f - SpookSuiteMenu.Instance.spaceFromLeft));
            ESPContent();
            GUILayout.EndVertical();
        }

        private void VisualContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            UI.CheatToggleSlider(Cheat.Instance<FOV>(), "FOV", Cheats.FOV.Value.ToString(), ref Cheats.FOV.Value, 1, 170);
            UI.CheatToggleSlider(Cheat.Instance<ThirdPerson>(), "Third Person", Cheats.ThirdPerson.Value.ToString(), ref Cheats.ThirdPerson.Value, 0, 20);
            UI.Checkbox("Display Dead", ref Cheat.Instance<DisplayDead>().Enabled);
            UI.Checkbox("Nameplates", ref Cheat.Instance<Nameplates>().Enabled);

            GUILayout.EndScrollView();
        }

        private void ESPContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            
            UI.Checkbox("Enable ESP", ref Cheat.Instance<ESP>().Enabled);
            UI.Button("Toggle All ESP", () => ESP.ToggleAll());
            UI.Checkbox("Display Players", ref ESP.displayPlayers);
            UI.Checkbox("Display Monsters", ref ESP.displayEnemies);
            UI.Checkbox("Display Items", ref ESP.displayItems);
            UI.Checkbox("Display Lasers", ref ESP.displayLasers);
            UI.Checkbox("Display Diving Bell", ref ESP.displayDivingBell);

            UI.SubHeader("Chams");
            UI.CheatToggleSlider(Cheat.Instance<ChamESP>(), "Enable Chams", ChamESP.Value.ToString(), ref ChamESP.Value, 0, 170);
            UI.Checkbox("Display Players", ref ChamESP.displayPlayers);
            UI.Checkbox("Display Monsters", ref ChamESP.displayEnemies);
            UI.Checkbox("Display Items", ref ChamESP.displayItems);
            UI.Checkbox("Display Lasers", ref ChamESP.displayLasers);
            UI.Checkbox("Display Diving Bell", ref ChamESP.displayDivingBell);

            GUILayout.EndScrollView();
        }
    }
}
