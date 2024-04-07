using UnityEngine;

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
    }
}
