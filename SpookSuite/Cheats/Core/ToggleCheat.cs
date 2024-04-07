using SpookSuite.Util;
using System;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace SpookSuite.Cheats.Core
{
    public class ToggleCheat : Cheat
    {
        public static bool Enabled;

        public virtual void OnGui() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
