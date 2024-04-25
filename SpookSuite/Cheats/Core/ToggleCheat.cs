using UnityEngine;

namespace SpookSuite.Cheats.Core
{
    public abstract class ToggleCheat : Cheat
    {
        private bool _enabled;
        public bool Enabled
        {
            get => _enabled;

            set
            {
                _enabled = value;
                if (value)
                    OnEnable();
                else
                    OnDisable();
            }
        }

        public ToggleCheat() { }
        public ToggleCheat(KeyCode defaultKeybind) : base(defaultKeybind) { }

        public void Toggle()
        {
            Enabled = !Enabled;
        }
        public virtual void OnGui() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }

    }
}
