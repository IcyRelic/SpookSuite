using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpookSuite
{
    internal class Settings
    {
        /* *
         * Keybinds
         *  */
        public static KeyCode MenuToggleKey = KeyCode.Insert;

        /* *    
         * Menu Settings
         * */
        public static int i_menuFontSize = 14;
        public static int i_menuWidth = 810;
        public static int i_menuHeight = 410;
        public static int i_sliderWidth = 100;
        public static int i_textboxWidth = 85;
        public static float f_menuAlpha = 1f;
        public static bool b_isMenuOpen = false;
        public static bool b_godmode = false;

        /* *    
         * Color Settings
         * */
        public static RGBAColor c_espPlayers = new RGBAColor(0, 255, 0, 1f);
        public static RGBAColor c_espItems = new RGBAColor(255, 255, 255, 1f);
        public static RGBAColor c_espMonsters = new RGBAColor(255, 0, 0, 1f);
        public static RGBAColor c_espDivingBells = new RGBAColor(0, 0, 255, 1f);

    }
}
