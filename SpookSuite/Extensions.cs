using SpookSuite.Manager;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookSuite
{
    public static class Extensions
    {
        private static readonly Dictionary<Type, Delegate> TryParseMethods = new Dictionary<Type, Delegate>()
        {
            { typeof(int), new TryParseDelegate<int>(int.TryParse) },
            { typeof(float), new TryParseDelegate<float>(float.TryParse) },
            { typeof(double), new TryParseDelegate<double>(double.TryParse) },
            { typeof(decimal), new TryParseDelegate<decimal>(decimal.TryParse) },
            { typeof(bool), new TryParseDelegate<bool>(bool.TryParse) },
            { typeof(DateTime), new TryParseDelegate<DateTime>(DateTime.TryParse) },
            { typeof(TimeSpan), new TryParseDelegate<TimeSpan>(TimeSpan.TryParse) },
            { typeof(ulong), new TryParseDelegate<ulong>(ulong.TryParse) },
            { typeof(long), new TryParseDelegate<long>(long.TryParse) },
        };

        public static string Subtract(this string s, int num) => s.Substring(0, s.Length - num);

        public static Camera GetCamera(this MainCamera mainCamera)
        {
            return mainCamera.Reflect().GetValue<Camera>("cam");
        }
        public static Vector3 GetClosestMonster(this Vector3 point) => GameObjectManager.monsters.OrderBy(x => Vector3.Distance(x.transform.position, point)).FirstOrDefault().transform.position;

        public static string GetName(this Item item)
        {
            return string.IsNullOrEmpty(item.displayName) ? item.name : item.displayName;
        }

        public static string GetName(this Hat hat)
        {
            return string.IsNullOrEmpty(hat.displayName) ? hat.name : hat.displayName;
        }

        public static bool Parse<T>(this string s, out T result) where T : struct, IConvertible ,IComparable<T>
        {
            result = default(T);
            bool success = false;

            if (TryParseMethods.TryGetValue(typeof(T), out var method))
                success = ((TryParseDelegate<T>)method)(s, out result);

            return success;
        }

        private delegate bool TryParseDelegate<T>(string input, out T result);
    }
}
