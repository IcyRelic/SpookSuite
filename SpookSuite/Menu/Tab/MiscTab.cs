using SpookSuite.Menu.Core;
using System;
using System.Collections.Generic;
using System.Text;
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
            GUILayout.Label(name); //doing it like this so we could just copy paste it over
            MenuContent();
            GUILayout.EndVertical();
        }

        private void MenuContent()
        {

        }
    }
}
