using UnityEngine;

namespace SpookSuite.Util
{
    public class Log
    {
        private static readonly string prefix = "[SpookSuite]";
        public static void Info(string message) => Debug.Log($"{prefix} {message}");
        public static void Warning(string message) => Debug.LogWarning($"{prefix} {message}");
        public static void Error(string message) => Debug.LogError($"{prefix} {message}");

    }
}
