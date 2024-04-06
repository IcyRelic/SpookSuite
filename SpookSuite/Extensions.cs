using SpookSuite.Util;
using UnityEngine;

namespace SpookSuite
{
    public static class Extensions
    {
        public static Camera GetCamera(this MainCamera mainCamera)
        {
            return mainCamera.Reflect().GetValue<Camera>("cam");
        }
    }
}
