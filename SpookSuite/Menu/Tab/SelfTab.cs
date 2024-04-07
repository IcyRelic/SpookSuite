using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Menu.Core;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class SelfTab : MenuTab
    {
        public SelfTab() : base("Self") { }

        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {   
            GUILayout.BeginVertical();
            GUILayout.Label(name); //doing it like this so we could just copy paste it over
            MenuContent();
            GUILayout.EndVertical();
        }
        
        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            UI.Checkbox("Godmode", ref Cheat.Instance<Godmode>().Enabled);
            UI.Checkbox("Unlimited Oxygen", ref Cheat.Instance<UnlimitedOxygen>().Enabled);
            UI.Checkbox("Unlimited Stamina", ref Cheat.Instance<UnlimitedStamina>().Enabled);
            //UI.CheatToggleSlider(Cheats.SuperSpeed.Instance, "Super Speed", Cheats.SuperSpeed.Value.ToString("#"), ref Cheats.SuperSpeed.Value, 10f, 100f);
      
            GUILayout.EndScrollView();
        }
    }
}
