using Photon.Pun;
using SpookSuite.Manager;
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
        private string spoofName = "";

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
        string helemtText = "";
        private void MenuContent()
        {
            GUILayout.BeginHorizontal();
            UI.Textbox("Helmet Text", ref helemtText);
            if (GUILayout.Button("Apply Text"))
                Player.localPlayer.refs.visor.visorFaceText.text = helemtText;
            GUILayout.EndHorizontal();

            UI.TextboxAction("Name Spoof", ref spoofName, "", 100, new UIButton("Set", () => PhotonNetwork.NickName = spoofName));
            UI.Button("Advance Day", () => GameUtil.AdvanceDay());
            UI.Button("Add $300", () => GameUtil.SendHospitalBill(-300));
            UI.Button("Remove $300", () => GameUtil.SendHospitalBill(300));
        }

        private void HelmetTextContent()
        {
            GUILayout.BeginHorizontal();

            UI.Textbox("Search", ref searchText);

            GUILayout.EndHorizontal();

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
