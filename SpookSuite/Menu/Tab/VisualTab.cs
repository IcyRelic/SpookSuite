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

            UI.CheatToggleSlider(Cheats.FOV.Instance, "FOV", Cheats.FOV.Value.ToString(), ref Cheats.FOV.Value, 1, 300);

            GUILayout.EndScrollView();
        }

        private void ESPContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);


            GUILayout.Label("Player ESP");
            //UI.Checkbox("Enabled", ref Cheats.PlayerESP.enabled);
            //UI.Checkbox("Skeleton", ref Cheats.PlayerESP.skeletonESP); //Player.refs.ik bones
            //UI.Checkbox("Box", ref Cheats.PlayerESP.box);
            //UI.Checkbox("Looking Radius", ref Cheats.PlayerESP.LookingRadius);//make a semicircle based off their rotation and field of view in front of em on the ground.
            GUILayout.Label("Enemy ESP");

            GUILayout.EndScrollView();
        }
    }
}
