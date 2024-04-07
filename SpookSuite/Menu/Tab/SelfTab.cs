using Photon.Pun;
using SpookSuite.Menu.Core;
using System;
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
            if(GUILayout.Button("Suicide"))
                Player.localPlayer.refs.view.RPC("RPCA_PlayerDie", RpcTarget.All, Array.Empty<object>());
            UI.Checkbox("Godmode", ref Cheats.Godmode.Instance.Enabled);
            UI.Checkbox("No Ragdoll", ref Cheats.NoRagdoll.Instance.Enabled);
            UI.Checkbox("Unlimited Oxygen", ref Cheats.UnlimitedOxygen.Instance.Enabled);
            UI.Checkbox("Unlimited Stamina", ref Cheats.UnlimitedStamina.Instance.Enabled);
            //UI.CheatToggleSlider(Cheats.SuperSpeed.Instance, "Super Speed", Cheats.SuperSpeed.Value.ToString("#"), ref Cheats.SuperSpeed.Value, 10f, 100f);
      
            GUILayout.EndScrollView();
        }
    }
}
