using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Cheats;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class MiscTab : MenuTab
    {
        public MiscTab() : base("Misc") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        private string searchText = "";
        private string moneyToSet = "";

        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.5f - SpookSuiteMenu.Instance.spaceFromLeft));
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            MenuContent();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.5f - SpookSuiteMenu.Instance.spaceFromLeft));
            HelmetTextContent();
            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            UI.Button("Advance Day", GameUtil.AdvanceDay);
            UI.TextboxAction("Money", ref moneyToSet, 10,
                new UIButton("Add", () => { int.TryParse(moneyToSet, out int o); GameUtil.SendHospitalBill(-o); }),
                new UIButton("Remove", () => { int.TryParse(moneyToSet, out int o); GameUtil.SendHospitalBill(o); })
            );

            UI.Button("Open/Close Diving Bell", Cheat.Instance<ToggleDivingBell>().Execute);
            UI.Button("Activate Diving Bell", Cheat.Instance<UseDivingBell>().Execute);
            UI.Checkbox("AntiSpawner (Auto Remove Spawned Items From Other Players)", Cheat.Instance<AntiSpawner>());
        }

        private void HelmetTextContent()
        {
            UI.Header("Message Sender");
            UI.Textbox("Search", ref searchText);

            List<LocalizationKeys.Keys> keys = Enum.GetValues(typeof(LocalizationKeys.Keys)).Cast<LocalizationKeys.Keys>().ToList().Where(key => key.ToString().ToLower().Contains(searchText.ToLower())).ToList();

            int gridWidth = 3;
            int btnWidth = (int)(SpookSuiteMenu.Instance.contentWidth * 0.5f - SpookSuiteMenu.Instance.spaceFromLeft) / gridWidth;
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
            UI.ButtonGrid<LocalizationKeys.Keys>(keys, key => key.ToString(), "", key => sendHelmetText(key), gridWidth, btnWidth);
            GUILayout.EndScrollView();
        }

        private void sendHelmetText(LocalizationKeys.Keys key)
        {
            SurfaceNetworkHandler.Instance.Reflect().GetValue<PhotonView>("m_View").RPC("RPCA_HelmetText", RpcTarget.All, (int)key, (object)3);
        }
    }
}
