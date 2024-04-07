using SpookSuite.Util;
using System;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace SpookSuite.Cheats.Core
{
    public class ToggleCheat : Cheat
    {
        public bool Enabled { get; set; }



        protected static bool WorldToScreen(Vector3 world, out Vector3 screen)
        {
            screen = MainCamera.instance.GetCamera().WorldToViewportPoint(world);
            screen.x *= Screen.width;
            screen.y *= Screen.height;
            screen.y = Screen.height - screen.y;
            return screen.z > 0.0;
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
