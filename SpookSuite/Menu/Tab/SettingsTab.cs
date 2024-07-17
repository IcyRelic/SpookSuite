using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class SettingsTab : MenuTab
    {
        public SettingsTab() : base("Settings") { }

        private Vector2 scrollPos = Vector2.zero;
        private float f_leftWidth;
        private string search = "";
        private int themesselect = 0;
        private string s_Primary = Settings.c_primary.GetHexCode();
        private string s_Text = Settings.c_menuText.GetHexCode();
        private string s_PlayerChams = Settings.c_chamPlayers.GetHexCode();
        private string s_ItemChams = Settings.c_chamItems.GetHexCode();
        private string s_MonsterChams = Settings.c_chamMonsters.GetHexCode();
        private string s_DivingBellCham = Settings.c_chamDivingBell.GetHexCode();
        private string s_PlayerEsp = Settings.c_espPlayers.GetHexCode();
        private string s_ItemEsp = Settings.c_espItems.GetHexCode();
        private string s_MonsterEsp = Settings.c_espMonsters.GetHexCode();
        private string s_DivingBellEsp = Settings.c_espDivingBells.GetHexCode();
        private bool dropdown_makesound = false;
        private bool dropdown_dronespawn = false;
        private bool dropdown_speedmanipulation = false;
        private bool dropdown_kick = false;
        private bool dropdown_shadowrealm = false;
        private bool dropdown_crash = false;
        private bool dropdown_blackscreen = false;

        public override void Draw()
        {
            f_leftWidth = SpookSuiteMenu.Instance.contentWidth * 0.55f - SpookSuiteMenu.Instance.spaceFromLeft;

            GUILayout.BeginVertical(GUILayout.Width(f_leftWidth));         
            MenuContent();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.45f - SpookSuiteMenu.Instance.spaceFromLeft));
            KeybindContent();
            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            UI.Actions(
                new UIButton("Reset Settings", () => Cheat.Instance<ResetMenu>().Execute()),
                new UIButton("Save Settings", () => Settings.Config.SaveConfig()),
                new UIButton("Reload Settings", () => Settings.Config.LoadConfig())
            );
            UI.Checkbox("Notifcations", Cheat.Instance<Notifications>());
            UI.NumSelect("Font Size", ref Settings.i_menuFontSize, 5, 30);
            UI.Slider("Menu Opacity", Settings.f_menuAlpha.ToString("0.00"), ref Settings.f_menuAlpha, 0.1f, 1f);
            UI.Button("Resize Menu", () => MenuUtil.BeginResizeMenu(), "Resize");
            UI.Button("Reset Menu", () => SpookSuiteMenu.Instance.ResetMenuSize(), "Reset");

            UI.Select("Theme", ref themesselect,
                new UIOption("Default", () => ThemeUtil.ApplyTheme("Default")),
                new UIOption("Green", () => ThemeUtil.ApplyTheme("Green")),
                new UIOption("Blue", () => ThemeUtil.ApplyTheme("Blue"))
            );

            UI.Header("Menu Colors");
            UI.TextboxAction("Primary", ref s_Primary, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_primary, s_Primary))
            );
            UI.TextboxAction("Text", ref s_Text, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_menuText, s_Text))
            );

            UI.Header("Esp Colors");
            UI.TextboxAction("Items", ref s_ItemEsp, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_espItems, s_ItemEsp))
            );
            UI.TextboxAction("Players", ref s_PlayerEsp, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_espPlayers, s_PlayerEsp))
            );
            UI.TextboxAction("Monsters", ref s_MonsterEsp, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_espMonsters, s_MonsterEsp))
            );
            UI.TextboxAction("Diving Bell", ref s_DivingBellEsp, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_espDivingBells, s_DivingBellEsp))
            );

            UI.Header("Cham Colors");
            UI.TextboxAction("Items", ref s_ItemChams, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_chamItems, s_ItemChams))
            );
            UI.TextboxAction("Players", ref s_PlayerChams, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_chamPlayers, s_PlayerChams))
            );
            UI.TextboxAction("Monsters", ref s_MonsterChams, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_chamMonsters, s_MonsterChams))
            );
            UI.TextboxAction("Diving Bell", ref s_DivingBellCham, 8,
                new UIButton("Set", () => SetColor(ref Settings.c_chamDivingBell, s_DivingBellCham))
            );

            UI.Header("Reactions");
            UI.Checkbox("Toggle", Cheat.Instance<RPCReactions>());
            UI.Checkbox("Notify On Reaction", ref RPCReactions.Value);

            ReactionSetter("Sound Spam", ref Settings.reaction_makesound, ref dropdown_makesound);
            ReactionSetter("Drone Spawning", ref Settings.reaction_dronespawn, ref dropdown_dronespawn);
            ReactionSetter("Speed Manipulation", ref Settings.reaction_speedmanipulation, ref dropdown_speedmanipulation);
            ReactionSetter("Kick", ref Settings.reaction_kick, ref dropdown_kick);
            ReactionSetter("Crash", ref Settings.reaction_crash, ref dropdown_crash);
            ReactionSetter("Shadow Realm", ref Settings.reaction_shadowrealm, ref dropdown_shadowrealm);
            ReactionSetter("Black Screen", ref Settings.reaction_blackscreen, ref dropdown_blackscreen);
        }

        private void ReactionSetter(string label, ref RPCReactions.reactionType reaction, ref bool drop)
        {
            RPCReactions.reactionType r = reaction;

            UI.Dropdown(label, ref drop,
                new UIButton("None", () => r = RPCReactions.reactionType.none, r == RPCReactions.reactionType.none ? new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold} : null),
                new UIButton("Kick", () => r = RPCReactions.reactionType.kick, r == RPCReactions.reactionType.kick ? new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold } : null),
                new UIButton("Disconnect", () => r = RPCReactions.reactionType.disconnect, r == RPCReactions.reactionType.disconnect ? new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold } : null),
                new UIButton("Clown Em", () => r = RPCReactions.reactionType.clownem, r == RPCReactions.reactionType.clownem ? new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold } : null),
                new UIButton("Send Away", () => r = RPCReactions.reactionType.shadowrealm, r == RPCReactions.reactionType.shadowrealm ? new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold } : null)

                );

            reaction = r;
        }

        private void SetColor(ref RGBAColor color, string hexCode)
        {
            while (hexCode.Length < 6) hexCode += "0";
            color = new RGBAColor(hexCode);
            Settings.Config.SaveConfig();
        }

        private void KeybindContent()
        {
            UI.Header("Keybinds");
            GUILayout.BeginVertical();

            UI.Textbox("Search", ref search, big: false);
           
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            List<Cheat> cheats = Cheat.instances.FindAll(c => !c.Hidden);
            foreach (Cheat cheat in cheats)
            {
                //if (!hack.CanHaveKeyBind()) continue;

                if (!cheat.GetType().Name.ToLower().Contains(search.ToLower()))
                    continue;

                GUILayout.BeginHorizontal();

                KeyCode bind = cheat.keybind;

                string kb = cheat.HasKeybind ? bind.ToString() : "None";

                GUILayout.Label(cheat.GetType().Name);
                GUILayout.FlexibleSpace();

                //if (cheat.HasKeybind && hack != Hack.OpenMenu && hack != Hack.UnlockDoorAction && GUILayout.Button("-")) hack.RemoveKeyBind();

                string btnText = cheat.WaitingForKeybind ? "Waiting" : kb;               
                if (GUILayout.Button(btnText, GUILayout.Width(85)))
                {
                    GUI.FocusControl(null);
                    KBUtil.BeginChangeKeybind(cheat);
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
