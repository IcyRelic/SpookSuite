﻿using Photon.Pun;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class SettingsTab : MenuTab
    {
        public SettingsTab() : base("Settings") { }

        private Vector2 scrollPos = Vector2.zero;
        private float f_leftWidth;

        public override void Draw()
        {
            f_leftWidth = SpookSuiteMenu.Instance.contentWidth * 0.55f - SpookSuiteMenu.Instance.spaceFromLeft;


            GUILayout.BeginVertical(GUILayout.Width(f_leftWidth));

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            MenuContent();

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.45f - SpookSuiteMenu.Instance.spaceFromLeft));

            //KeybindContent();

            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            UI.NumSelect("Font Size", ref Settings.i_menuFontSize, 5, 30);
            UI.Slider("Menu Opacity", Settings.f_menuAlpha.ToString("0.00"), ref Settings.f_menuAlpha, 0.1f, 1f);
            UI.Button("Resize Menu", () => MenuUtil.BeginResizeMenu(), "Resize");
            UI.Button("Reset Menu", () => SpookSuiteMenu.Instance.ResetMenuSize(), "Reset");
        }
    }
}