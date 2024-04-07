using SpookSuite.Cheats.Core;
using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace SpookSuite
{
    internal class UI
    {
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

        public static void InputNum<T>(string label, ref object var)
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
    }
}
