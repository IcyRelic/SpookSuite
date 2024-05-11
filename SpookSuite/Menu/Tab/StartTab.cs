using SpookSuite.Menu.Core;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class StartTab : MenuTab
    {
        Vector2 scrollPos;
        public StartTab() : base("Start") { }

        public override void Draw()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void MenuContent()
        {
            string intoText = "SpookSuite is developed by IcyRelic, The Green Bandit (TGB, He enjoys patting himself on the back.)\nThis menu is jam packed with everything we could think of at the moment. " +
                "There are still a couple of planned features and QOL updates in the pipeline. Let us know if you have any ideas or suggestions on UnknownCheats or GitHub. Enjoy!";

            UI.Header(Settings.c_primary.AsString("Welcome to SpookSuite!"), 30);
            GUILayout.Space(20);
            UI.Label(intoText);
            GUILayout.Space(20);

            UI.Label("Noteworthy Contributors: \nSerpent, Toa");
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            
            foreach (string line in Settings.Changelog.changes)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);

                if (line.StartsWith("v")) style.fontStyle = FontStyle.Bold;
                GUILayout.Label(line.StartsWith("v") ? "Changelog " + line : line, style);
            }

            GUILayout.EndScrollView();
        }

    }
}