using SpookSuite.Util;
using System;
using Unity.VisualScripting;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace SpookSuite.Cheats.Core
{
    public abstract class ToggleCheat : Cheat
    {
        public bool Enabled = false;

        public ToggleCheat() { }
        public ToggleCheat(KeyCode defaultKeybind) : base(defaultKeybind) { }

        public void Toggle()
        {
            Enabled = !Enabled;
        }
        public virtual void OnGui() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }

    }
}
