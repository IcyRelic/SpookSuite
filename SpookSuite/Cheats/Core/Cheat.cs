using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpookSuite.Cheats.Core
{
    public class Cheat : MonoBehaviour
    {
        public static List<Cheat> instances = new List<Cheat>();
        public static T? Instance<T>() where T : Cheat => instances.Find(x => x is T) as T;

        public KeyCode defaultKeybind = KeyCode.None;
        public KeyCode keybind = KeyCode.None;
        public bool HasKeybind => keybind != KeyCode.None;
        public bool WaitingForKeybind = false;
        public bool Hidden = false;

        public Cheat()
        {
            instances.Add(this);
        }

        public Cheat(KeyCode defaultKeybind)
        {
            this.defaultKeybind = defaultKeybind;
            this.keybind = defaultKeybind;
            instances.Add(this);
        }

        public Cheat(KeyCode defaultKeybind, bool hidden)
        {
            this.defaultKeybind = defaultKeybind;
            this.keybind = defaultKeybind;
            this.Hidden = hidden;
            instances.Add(this);
        }

        protected static bool WorldToScreen(Vector3 world, out Vector3 screen)
        {
            screen = MainCamera.instance.GetCamera().WorldToViewportPoint(world);
            screen.x *= Screen.width;
            screen.y *= Screen.height;
            screen.y = Screen.height - screen.y;
            return screen.z > 0.0;
        }
        protected float GetDistanceToPos(Vector3 position)
        {
            return (float)Math.Round((double)Vector3.Distance(Player.localPlayer.refs.cameraPos.position, position));
        }
    }
}
