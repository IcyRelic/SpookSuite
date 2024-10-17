using SpookSuite.Cheats;
using SpookSuite.Menu.Tab;
using SpookSuite.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Menu.Core
{
    internal class SpookSuiteMenu : MenuFragment
    {
        public Rect windowRect = new Rect(50f, 50f, 700f, 450f);

        private Vector2 scrollPos = Vector2.zero;
        public List<MenuTab> tabs = new List<MenuTab>();
        public int selectedTab = 0;
        public float contentWidth;
        public float contentHeight;
        public int spaceFromTop = 60;
        public int spaceFromLeft = 10;

        private static SpookSuiteMenu instance;
        public static SpookSuiteMenu Instance
        {
            get
            {
                if (instance == null) instance = new SpookSuiteMenu();
                return instance;
            }
        }

        public SpookSuiteMenu()
        {
            instance = this;
            if(DebugMode.Value) DebugMode.AddDebugTabs(); //yes we see u editing this, no we dont care as its mainly useless
            tabs.Add(new StartTab());
            tabs.Add(new SettingsTab());
            tabs.Add(new SelfTab());
            tabs.Add(new VisualTab());
            tabs.Add(new PlayersTab());
            tabs.Add(new MiscTab());
            tabs.Add(new EnemyTab());
            tabs.Add(new ItemTab());
            tabs.Add(new LobbyTab());

            Resize();
            selectedTab = tabs.IndexOf(tabs.Where(x => x.name == "Start").First());
        }

        public void Resize()
        {
            windowRect.width = Settings.i_menuWidth;
            windowRect.height = Settings.i_menuHeight;
            contentWidth = windowRect.width - (spaceFromLeft * 2);
            contentHeight = windowRect.height - spaceFromTop;
        }

        public void ResetMenuSize()
        {
            Settings.i_menuFontSize = 14;
            Settings.i_menuWidth = 810;
            Settings.i_menuHeight = 410;
            Settings.i_sliderWidth = 100;
            Settings.i_textboxWidth = 85;
            Settings.Config.SaveConfig();
        }

        public void Stylize()
        {
            GUI.skin = ThemeUtil.Skin;
            GUI.color = Color.white;

            GUI.skin.label.fontSize = Settings.i_menuFontSize;
            GUI.skin.button.fontSize = Settings.i_menuFontSize;
            GUI.skin.toggle.fontSize = Settings.i_menuFontSize;
            //GUI.skin.window.fontSize = Settings.i_menuFontSize;
            GUI.skin.box.fontSize = Settings.i_menuFontSize;
            GUI.skin.textField.fontSize = Settings.i_menuFontSize;
            GUI.skin.horizontalSlider.fontSize = Settings.i_menuFontSize;
            GUI.skin.horizontalSliderThumb.fontSize = Settings.i_menuFontSize;
            GUI.skin.verticalSlider.fontSize = Settings.i_menuFontSize;
            GUI.skin.verticalSliderThumb.fontSize = Settings.i_menuFontSize;

            GUI.skin.customStyles.Where(x => x.name == "TabBtn").First().fontSize = Settings.i_menuFontSize;
            GUI.skin.customStyles.Where(x => x.name == "SelectedTab").First().fontSize = Settings.i_menuFontSize;

            Resize();
        }

        public void Draw()
        {
            if (!Settings.b_isMenuOpen) return;

            Stylize();
            GUI.color = new Color(1f, 1f, 1f, Settings.f_menuAlpha);

            windowRect = GUILayout.Window(0, windowRect, new GUI.WindowFunction(DrawContent), "SpookSuite");
            GUI.color = Color.white;
        }

        private void DrawContent(int windowID)
        {
            GUI.color = new Color(1f, 1f, 1f, 0.1f);
            GUIStyle watermark = new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold };
            string text = "IcyRelic & TGB";

            GUI.Label(new Rect(windowRect.width - watermark.CalcSize(new GUIContent(text)).x - 10, windowRect.height - watermark.CalcSize(new GUIContent(text)).y - 10, watermark.CalcSize(new GUIContent(text)).x, watermark.CalcSize(new GUIContent(text)).y), text, watermark);
            GUI.color = new Color(1f, 1f, 1f, Settings.f_menuAlpha);

            GUILayout.BeginVertical();

            GUILayout.BeginArea(new Rect(0, 25, windowRect.width, 25), style: "Toolbar");

            GUILayout.BeginHorizontal();
            selectedTab = GUILayout.Toolbar(selectedTab, tabs.Select(x => x.name).ToArray(), style: "TabBtn");
            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.Space(spaceFromTop);

            GUILayout.BeginArea(new Rect(spaceFromLeft, spaceFromTop, windowRect.width - spaceFromLeft, contentHeight - 15));

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            tabs[selectedTab].Draw();
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();

            GUILayout.EndArea();

            GUI.color = Color.white;

            GUI.DragWindow();
        }
    }
}
