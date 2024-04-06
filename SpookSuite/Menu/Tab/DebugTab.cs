using SpookSuite.Menu.Core;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug") { }

        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();

        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Test Button");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {


            }
            GUILayout.EndHorizontal();



            GUILayout.EndScrollView();
        }
    }
}
