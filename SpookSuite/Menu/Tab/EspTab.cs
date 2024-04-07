using SpookSuite.Menu.Core;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class EspTab : MenuTab
    {
        public EspTab() : base("Esp") { }

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
            GUILayout.Label("Player ESP");
            //UI.Checkbox("Enabled", ref Cheats.PlayerESP.enabled);
            //UI.Checkbox("Skeleton", ref Cheats.PlayerESP.skeletonESP); //Player.refs.ik bones
            //UI.Checkbox("Box", ref Cheats.PlayerESP.box);
            //UI.Checkbox("Looking Radius", ref Cheats.PlayerESP.LookingRadius);//make a semicircle based off their rotation and field of view in front of em on the ground.
            GUILayout.Label("Enemy ESP");

        }
    }
}
