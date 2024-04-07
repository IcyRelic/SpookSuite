using SpookSuite.Util;
using System;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace SpookSuite.Cheats.Core
{
    public class ToggleCheat : Cheat
    {
        public bool Enabled = false;
        private static ToggleCheat instance;

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

        public void Start()
        {
            instance = this;
        }
    }
}
