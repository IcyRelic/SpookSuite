using Photon.Pun;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class SelfTab : MenuTab
    {
        public SelfTab() : base("Self") { }

        private Vector2 scrollPos = Vector2.zero;
        private string s_faceColor = "";
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
            UI.Button("Suicide", () => Player.localPlayer.refs.view.RPC("RPCA_PlayerDie", RpcTarget.All, Array.Empty<object>()), null);
            UI.Button("Revive", () => Player.localPlayer.refs.view.RPC("RPCA_PlayerRevive", RpcTarget.All, Array.Empty<object>()), null);
            GUILayout.EndHorizontal();

            UI.Textbox("Spoofed Name", ref NameSpoof.Value, length: 100, onChanged: NameSpoof.OnValueChanged);
            UI.Checkbox("Use Spoofed Name", Cheat.Instance<NameSpoof>());
        }

        private void Toggles()
        {
            UI.CheatToggleSlider(Cheat.Instance<SuperSpeed>(), "Super Speed", SuperSpeed.Value.ToString("#"), ref SuperSpeed.Value, 10f, 100f);
            UI.CheatToggleSlider(Cheat.Instance<SuperJump>(), "Super Jump", SuperJump.Value.ToString("#.#"), ref SuperJump.Value, 0.6f, 20f);
            UI.CheatToggleSlider(Cheat.Instance<NoClip>(), "NoClip", NoClip.Value.ToString(), ref NoClip.Value, 1f, 20f);
            UI.CheatToggleSlider(Cheat.Instance<RainbowFace>(), "Rainbow Face", RainbowFace.Value.ToString(), ref RainbowFace.Value, 0.1f, 1f);

            UI.Checkbox("Godmode", Cheat.Instance<Godmode>());
            UI.Checkbox("Invisibility", Cheat.Instance<Invisibility>());
            UI.Checkbox("Infinte Jump", Cheat.Instance<InfiniteJump>());
            UI.Checkbox("No Ragdoll", Cheat.Instance<NoRagdoll>());
            UI.Checkbox("Unlimited Oxygen", Cheat.Instance<UnlimitedOxygen>());
            UI.Checkbox("Unlimited Stamina", Cheat.Instance<UnlimitedStamina>());
            UI.Checkbox("Unlimited Battery", Cheat.Instance<UnlimitedBattery>());
            UI.Checkbox("Unlimited Film", Cheat.Instance<UnlimitedFilm>());
        }
    }
}
