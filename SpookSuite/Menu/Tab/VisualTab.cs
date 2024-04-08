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
            UI.Checkbox("Display Dead", ref Settings.b_displayDead);

            GUILayout.EndScrollView();
        }

        private void ESPContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.Label("Player ESP");
            UI.Checkbox("Enable ESP", ref Cheat.Instance<ESP>().Enabled);
            UI.Checkbox("Display Players", ref ESP.displayPlayers);
            //UI.Checkbox("Enabled", ref Cheats.PlayerESP.enabled);
            //UI.Checkbox("Skeleton", ref Cheats.PlayerESP.skeletonESP); //Player.refs.ik bones
            //UI.Checkbox("Box", ref Cheats.PlayerESP.box);
            //UI.Checkbox("Looking Radius", ref Cheats.PlayerESP.LookingRadius);//make a semicircle based off their rotation and field of view in front of em on the ground.
            UI.Checkbox("Display Monsters", ref ESP.displayEnemies);
            UI.Checkbox("Display Items", ref ESP.displayItems);
            UI.Checkbox("Display Lasers", ref ESP.displayLasers);
            UI.Checkbox("Display Diving Bell", ref ESP.displayDivingBell);

            GUILayout.EndScrollView();
        }
    }
}
