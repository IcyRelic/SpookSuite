using SpookSuite.Manager;
using SpookSuite.Util;
using System.Linq;
using UnityEngine;

namespace SpookSuite
{
    public static class Extensions
    {
        public static Camera GetCamera(this MainCamera mainCamera)
        {
            return mainCamera.Reflect().GetValue<Camera>("cam");
        }

        public static Vector3 GetClosestMonster(this Vector3 point) => GameObjectManager.monsters.OrderBy(x => Vector3.Distance(x.transform.position, point)).FirstOrDefault().transform.position;

        public static string GetName(this Item item)
        {
            return string.IsNullOrEmpty(item.displayName) ? item.name : item.displayName;
        }
    }
}
