using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System.Collections.Generic;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class MiscTab : MenuTab
    {
        public MiscTab() : base("Misc") { }

        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            MenuContent();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        string helemtText = "";
        private void MenuContent()
        {
            GUILayout.BeginHorizontal();
            UI.Textbox("Helmet Text", ref helemtText);
            if (GUILayout.Button("Apply Text"))
                Player.localPlayer.refs.visor.visorFaceText.text = helemtText;
            GUILayout.EndHorizontal();
        }
    }
}
