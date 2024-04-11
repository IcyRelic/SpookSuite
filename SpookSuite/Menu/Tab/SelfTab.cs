using Photon.Pun;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
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
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.5f - SpookSuiteMenu.Instance.spaceFromLeft));
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            MenuContent();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.5f - SpookSuiteMenu.Instance.spaceFromLeft));
            Toggles();
            GUILayout.EndVertical();
        }
        
        private void MenuContent()
        {
            GUILayout.BeginHorizontal();
            UI.Button("Suicide", Player.localPlayer.RPCA_PlayerDie, null);
            UI.Button("Revive", () => Player.localPlayer.refs.view.RPC("RPCA_PlayerRevive", RpcTarget.All, Array.Empty<object>()), null);
            GUILayout.EndHorizontal();
            
            
        }

        private void Toggles()
        {
            UI.CheatToggleSlider(Cheat.Instance<SuperSpeed>(), "Super Speed", SuperSpeed.Value.ToString("#"), ref SuperSpeed.Value, 10f, 100f);
            UI.CheatToggleSlider(Cheat.Instance<SuperJump>(), "Super Jump", SuperJump.Value.ToString("#.#"), ref SuperJump.Value, 0.6f, 20f);
            UI.Checkbox("Godmode", ref Cheat.Instance<Godmode>().Enabled);
            UI.Checkbox("No Clip", ref Cheat.Instance<NoClip>().Enabled);
            UI.Checkbox("Infinte Jump", ref Cheat.Instance<InfiniteJump>().Enabled);
            UI.Checkbox("No Ragdoll", ref Cheat.Instance<NoRagdoll>().Enabled);
            UI.Checkbox("Unlimited Oxygen", ref Cheat.Instance<UnlimitedOxygen>().Enabled);
            UI.Checkbox("Unlimited Stamina", ref Cheat.Instance<UnlimitedStamina>().Enabled);
            UI.Checkbox("Unlimited Battery", ref Cheat.Instance<UnlimitedBattery>().Enabled);
            UI.Checkbox("Unlimited Film", ref Cheat.Instance<UnlimitedFilm>().Enabled);
        }
    }
}
