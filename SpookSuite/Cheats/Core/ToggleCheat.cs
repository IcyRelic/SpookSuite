using UnityEngine;

namespace SpookSuite.Cheats.Core
{
    public abstract class ToggleCheat : Cheat
    {
        private bool _enabled;
        private bool lastenabled;
        public bool Enabled
        {
            get => _enabled;

            set
            {
                _enabled = value;
                RunOns();      
            }
        }

        private void RunOns()
        {
            if (!lastenabled && _enabled)
            {
                OnEnable();
                lastenabled = true;
            }
            else if (lastenabled && !_enabled)
            {
                OnDisable();
                lastenabled = false;
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
