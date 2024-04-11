using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace SpookSuite
{
    public class UIButton
    {
        public string label;
        public Action action;
        private GUIStyle style = null;

        public UIButton(string label, Action action, GUIStyle style = null)
        {
            this.label = label;
            this.action = action;

            this.style = style;
        }

        public void Draw()
        {
            if (style != null ? GUILayout.Button(label, style) : GUILayout.Button(label)) action.Invoke();
        }
    }

    public class UIOption
    {
        public string label;
        public object value;
        public Action action;


        public UIOption(string label, object value)
        {
            this.label = label;
            this.value = value;
        }

        public UIOption(string label, Action action)
        {
            this.label = label;
            this.action = action;
        }

        public void Draw(ref object refValue)
        {
            if (GUILayout.Button(label)) refValue = value;
        }

        public void Draw()
        {
            if (GUILayout.Button(label)) action.Invoke();
        }
    }
    public class UI
    {

        public static void Header(string header, bool space = false)
        {
            if (space) GUILayout.Space(20);
            GUILayout.Label(header, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold });
        }

        public static void SubHeader(string header, bool space = false)
        {
            if (space) GUILayout.Space(20);
            GUILayout.Label(header, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        }

        public static void Label(string header, string label, RGBAColor color = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(header);
            GUILayout.FlexibleSpace();
            GUILayout.Label(color is null ? label : color.AsString(label));
            GUILayout.EndHorizontal();
        }

        public static void Label(string label, RGBAColor color = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(color is null ? label : color.AsString(label));
            GUILayout.EndHorizontal();
        }
        public static void Checkbox(string header, ref bool value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(header); //dont wanna add langauge shit yet
            GUILayout.FlexibleSpace();
            value = GUILayout.Toggle(value, "");
            GUILayout.EndHorizontal();
        }

        public static void ToggleSlider(string header, string displayValue, ref bool enable, ref float value, float min, float max, params object[] param)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(header + " ( " + displayValue + " )");
            GUILayout.FlexibleSpace();

            GUIStyle slider = new GUIStyle(GUI.skin.horizontalSlider) { alignment = TextAnchor.MiddleCenter, fixedWidth = Settings.i_sliderWidth };

            value = GUILayout.HorizontalSlider(value, min, max, slider, GUI.skin.horizontalSliderThumb);

            enable = GUILayout.Toggle(enable, "");

            GUILayout.EndHorizontal();
        }

        public static void CheatToggleSlider(ToggleCheat toggle, string header, string displayValue, ref float value, float min, float max, params object[] param)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(header + " ( " + displayValue + " )");
            GUILayout.FlexibleSpace();

            GUIStyle slider = new GUIStyle(GUI.skin.horizontalSlider) { alignment = TextAnchor.MiddleCenter, fixedWidth = Settings.i_sliderWidth };

            value = GUILayout.HorizontalSlider(value, min, max, slider, GUI.skin.horizontalSliderThumb);

            toggle.Enabled = GUILayout.Toggle(toggle.Enabled, "");

            GUILayout.EndHorizontal();
        }

        public static void ExecuteSlider(string header, string displayValue, Action executable, ref float value, float min, float max, params object[] param)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(header + " ( " + displayValue + " )");
            GUILayout.FlexibleSpace();

            GUIStyle slider = new GUIStyle(GUI.skin.horizontalSlider) { alignment = TextAnchor.MiddleCenter, fixedWidth = Settings.i_sliderWidth };

            value = GUILayout.HorizontalSlider(value, min, max, slider, GUI.skin.horizontalSliderThumb);

            if (GUILayout.Button("Execute"))
                executable.Invoke();

            GUILayout.EndHorizontal();
        }

        public static void Button(string header, Action action, string btnText = "Execute")
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(header);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(btnText)) action();
            GUILayout.EndHorizontal();
        }

        public static void Slider(string header, string displayValue, ref float value, float min, float max)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(header + " ( " + displayValue + " )");
            GUILayout.FlexibleSpace();
            value = GUILayout.HorizontalSlider(value, min, max, GUILayout.Width(Settings.i_sliderWidth));
            GUILayout.EndHorizontal();
        }

        public static void InputInt(string label, ref int var)
        {
            int newvar = var;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            if (int.TryParse(GUILayout.TextField(var.ToString()), out newvar))
                var = newvar;
            GUILayout.EndHorizontal();
        }

        public static void InputNum(string label, ref object var)
        {
            float fnewval;
            int inewval;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            if (var is float)
            {
                float.TryParse(GUILayout.TextField(var.ToString()), out fnewval);
                var = fnewval;
            }
            else if (var is int)
            {
                int.TryParse(GUILayout.TextField(var.ToString()), out inewval);
                var = inewval;
            }
            else
                Debug.Log($"Input num couldnt convert the type of {label}");

            GUILayout.EndHorizontal();
        }

        public static void NumSelect(string header, ref int value, int min, int max)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(header);
            GUILayout.FlexibleSpace();
            GUILayout.Label(value.ToString());
            if (GUILayout.Button("-")) value = Mathf.Clamp(value - 1, min, max);
            if (GUILayout.Button("+")) value = Mathf.Clamp(value + 1, min, max);
            GUILayout.EndHorizontal();
        }

        public static void Textbox(string label, ref string value, string regex = "", bool big = true)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label);
            GUILayout.FlexibleSpace();
            value = GUILayout.TextField(value, GUILayout.Width(big ? Settings.i_textboxWidth * 3 : Settings.i_textboxWidth));
            value = Regex.Replace(value, regex, "");
            GUILayout.EndHorizontal();
        }
        public static void Actions(params UIButton[] buttons)
        {
            GUILayout.BeginHorizontal();
            buttons.ToList().ForEach(btn => btn.Draw());
            GUILayout.EndHorizontal();
        }
        public static void ButtonGrid<T>(List<T> objects, Func<T, string> textSelector, string search, Action<T> action, int numPerRow, int btnWidth = 175)
        {
            List<T> filtered = objects.FindAll(x => textSelector(x).ToLower().Contains(search.ToLower()));

            int rows = Mathf.CeilToInt(filtered.Count / (float)numPerRow);

            for (int i = 0; i < rows; i++)
            {
                GUILayout.BeginHorizontal();
                for (int j = 0; j < numPerRow; j++)
                {
                    int index = i * numPerRow + j;
                    if (index >= filtered.Count) break;
                    var obj = filtered[index];

                    if (GUILayout.Button(textSelector((T)obj), GUILayout.Width(btnWidth))) action((T)obj);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
