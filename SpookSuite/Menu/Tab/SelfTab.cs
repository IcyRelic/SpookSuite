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
        private float faceSize = .035f;
        private string faceText = "SS";
        private float faceRotation = 0;

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

            UI.Header("Talk To");
            GUILayout.BeginHorizontal();
            UI.Button("Everyone", NetworkVoiceHandler.ListenAndSendToAll, null);
            UI.Button("Alive", NetworkVoiceHandler.TalkToAlive, null);
            UI.Button("Dead", NetworkVoiceHandler.TalkToDead, null);
            GUILayout.EndHorizontal();

            UI.Textbox("Spoofed Name", ref NameSpoof.Value, length: 100, onChanged: NameSpoof.OnValueChanged);
            UI.Checkbox("Use Spoofed Name", Cheat.Instance<NameSpoof>());

            UI.HorizontalSpace("Face", () =>
            {
                UI.ExecuteSlider("Rotation", faceRotation.ToString(), () =>
                {
                    if (!Player.localPlayer.refs.visor)
                        return;

                    PlayerVisor v = Player.localPlayer.refs.visor;
                    v.SetAllFaceSettings(v.hue.Value, v.visorColorIndex, v.visorFaceText.text, faceRotation, v.FaceSize); //doesnt check limit
                }, ref faceRotation, 0, 360);
            });

            UI.HorizontalSpace(null, () =>
            {
                UI.Textbox("Text", ref faceText, false, 3);
                UI.Button("Set", () => Player.localPlayer.refs.view.RPC("RPCA_SetVisorText", RpcTarget.All, faceText));
            });

            UI.HorizontalSpace(null, () =>
            {
                UI.ExecuteSlider("Size", faceSize.ToString(), () =>
                {
                    if (!Player.localPlayer.refs.visor)
                        return;

                    PlayerVisor v = Player.localPlayer.refs.visor;
                    v.SetAllFaceSettings(v.hue.Value, v.visorColorIndex, v.visorFaceText.text, v.FaceRotation, faceSize); //doesnt check limit
                }, ref faceSize, .001f, 1f);
                UI.Button("Reset", () =>
                {
                    faceSize = .035f;
                    if (!Player.localPlayer.refs.visor)
                        return;

                    PlayerVisor v = Player.localPlayer.refs.visor;
                    v.SetAllFaceSettings(v.hue.Value, v.visorColorIndex, v.visorFaceText.text, v.FaceRotation, 0.035f);
                });
            });
        }

        private void Toggles()
        {
            UI.CheatToggleSlider(Cheat.Instance<SuperSpeed>(), "Super Speed", SuperSpeed.Value.ToString("#"), ref SuperSpeed.Value, 10f, 100f);
            UI.CheatToggleSlider(Cheat.Instance<SuperJump>(), "Super Jump", SuperJump.Value.ToString("#.#"), ref SuperJump.Value, 0.6f, 20f);
            UI.CheatToggleSlider(Cheat.Instance<NoClip>(), "NoClip", NoClip.Value.ToString(), ref NoClip.Value, 1f, 20f);
            UI.CheatToggleSlider(Cheat.Instance<RainbowFace>(), "Rainbow Face", RainbowFace.Value.ToString(), ref RainbowFace.Value, 0.1f, 30f);
            UI.CheatToggleSlider(Cheat.Instance<RollingFace>(), "Rolling Face", RollingFace.Value.ToString(), ref RollingFace.Value, 0.1f, 30f);
            UI.CheatToggleSlider(Cheat.Instance<Spinbot>(), "Spinbot", Spinbot.Value.ToString(), ref Spinbot.Value, 1f, 23f);

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
