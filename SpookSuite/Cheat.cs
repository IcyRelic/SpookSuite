using SpookSuite.Util;
using System;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace SpookSuite
{
    public class basefeature
    {
    }

    public class looped_feature : basefeature
    { }

    public class button_feature : basefeature
    { }


    public class Cheat : MonoBehaviour
    {
        protected static bool WorldToScreen(Vector3 world, out Vector3 screen)
        {
            screen = MainCamera.instance.GetCamera().WorldToViewportPoint(world);
            screen.x *= (float)Screen.width;
            screen.y *= (float)Screen.height;
            screen.y = (float)Screen.height - screen.y;
            return (double)screen.z > 0.0;
        }
        protected float GetDistanceToPlayer(Vector3 position)
        {
            return (float)Math.Round((double)Vector3.Distance(Player.localPlayer.refs.cameraPos.position, position));
        }
        public virtual void OnGui() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
