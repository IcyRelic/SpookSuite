using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System.Linq;
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
            UI.CheatToggleSlider(Cheat.Instance<FOV>(), "FOV", Cheats.FOV.Value.ToString(), ref Cheats.FOV.Value, 1, 170);
            UI.CheatToggleSlider(Cheat.Instance<ThirdPerson>(), "Third Person", Cheats.ThirdPerson.Value.ToString(), ref Cheats.ThirdPerson.Value, 0, 20);
            UI.Checkbox("Disable Visor", Cheat.Instance<PlayerVisorToggle>());
            UI.Checkbox("Display Dead", Cheat.Instance<DisplayDead>());
            UI.Checkbox("Nameplates", Cheat.Instance<Nameplates>());
        }
        public static bool rainbow;
        private void ESPContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            
            UI.Checkbox("Enable ESP", Cheat.Instance<ESP>());
            UI.Button("Toggle All ESP", () => ESP.ToggleAll());
            UI.Checkbox("Display Players", ref ESP.displayPlayers);
            UI.Checkbox("Display Monsters", ref ESP.displayEnemies);
            UI.Checkbox("Display Items", ref ESP.displayItems);
            UI.Checkbox("Display Lasers", ref ESP.displayLasers);
            UI.Checkbox("Display Diving Bell", ref ESP.displayDivingBell);

            UI.SubHeader("Chams");
            UI.CheatToggleSlider(Cheat.Instance<ChamESP>(), "Enable Chams", $"Min Distance: {ChamESP.Value.ToString("#")}", ref ChamESP.Value, 0, 170);
            UI.ToggleSlider("Rainbow Mode", ChamESP.Speed.ToString(), ref ChamESP.rainbowMode, ref ChamESP.Speed, 0.1f, 30f);
            UI.Slider("Opacity", ChamESP.opacity.ToString(), ref ChamESP.opacity, 0, 1);
            UI.Button("Toggle All Chams", () => ChamESP.ToggleAll());
            UI.Checkbox("Display Players", ref ChamESP.displayPlayers);
            UI.Checkbox("Display Monsters", ref ChamESP.displayEnemies);
            UI.Checkbox("Display Items", ref ChamESP.displayItems);
            UI.Checkbox("Display Lasers", ref ChamESP.displayLasers);
            UI.Checkbox("Display Diving Bell", ref ChamESP.displayDivingBell);

            GUILayout.EndScrollView();
        }
    }
}
