using Photon.Pun;
using SpookSuite.Cheats.Core;
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
        private string search = "";
        private int themesselect = 0;

        public override void Draw()
        {
            f_leftWidth = SpookSuiteMenu.Instance.contentWidth * 0.55f - SpookSuiteMenu.Instance.spaceFromLeft;

            GUILayout.BeginVertical(GUILayout.Width(f_leftWidth));         
            MenuContent();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.45f - SpookSuiteMenu.Instance.spaceFromLeft));
            KeybindContent();
            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            UI.Actions(
                new UIButton("Reset Settings", () => Settings.Config.RegenerateConfig()),
                new UIButton("Save Settings", () => Settings.Config.SaveConfig()),
                new UIButton("Reload Settings", () => Settings.Config.LoadConfig())
            );

            UI.NumSelect("Font Size", ref Settings.i_menuFontSize, 5, 30);
            UI.Slider("Menu Opacity", Settings.f_menuAlpha.ToString("0.00"), ref Settings.f_menuAlpha, 0.1f, 1f);
            UI.Button("Resize Menu", () => MenuUtil.BeginResizeMenu(), "Resize");
            UI.Button("Reset Menu", () => SpookSuiteMenu.Instance.ResetMenuSize(), "Reset");

            UI.Select("Theme", ref themesselect,
                new UIOption("Default", () => ThemeUtil.ApplyTheme("Default")),
                new UIOption("Green", () => ThemeUtil.ApplyTheme("Green")),
                new UIOption("Blue", () => ThemeUtil.ApplyTheme("Blue"))
            );
        }

        private void KeybindContent()
        {
            UI.Header("Keybinds");
            GUILayout.BeginVertical();

            UI.Textbox("Search", ref search, big: false);
           
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            List<Cheat> cheats = Cheat.instances.FindAll(c => !c.Hidden);
            foreach (Cheat cheat in cheats)
            {
                //if (!hack.CanHaveKeyBind()) continue;

                if (!cheat.GetType().Name.ToLower().Contains(search.ToLower()))
                    continue;

                GUILayout.BeginHorizontal();

                KeyCode bind = cheat.keybind;

                string kb = cheat.HasKeybind ? bind.ToString() : "None";

                GUILayout.Label(cheat.GetType().Name);
                GUILayout.FlexibleSpace();

                //if (cheat.HasKeybind && hack != Hack.OpenMenu && hack != Hack.UnlockDoorAction && GUILayout.Button("-")) hack.RemoveKeyBind();

                string btnText = cheat.WaitingForKeybind ? "Waiting" : kb;               
                if (GUILayout.Button(btnText, GUILayout.Width(85)))
                {
                    GUI.FocusControl(null);
                    KBUtil.BeginChangeKeybind(cheat);
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
