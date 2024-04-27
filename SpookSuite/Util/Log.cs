using UnityEngine;

namespace SpookSuite.Util
{
    public class Log
    {
        private static readonly string prefix = "[SpookSuite]";
        public static void Info(object message) => Debug.Log($"{prefix} {message}");
        public static void Warning(object message) => Debug.LogWarning($"{prefix} {message}");
        public static void Error(object message) => Debug.LogError($"{prefix} {message}");

    }
}
