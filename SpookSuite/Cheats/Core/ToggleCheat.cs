using SpookSuite.Util;
using System;
using Unity.VisualScripting;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace SpookSuite.Cheats.Core
{
    public class ToggleCheat : Cheat
    {
        public bool Enabled = false;
        private static ToggleCheat instance;

        public ToggleCheat()
        {
            instance = this;
        }

        public static ToggleCheat Instance
        {
            get
            {
                
                return instance;
            }
        }

        public virtual void OnGui() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }

    }
}
