using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Cheats;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpookSuite.Manager;
using Steamworks;

namespace SpookSuite.Menu.Tab
{
    internal class MiscTab : MenuTab
    {
        public MiscTab() : base("Misc") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        private string searchText = "";
        private string moneyToSet = "0";
        private string metaToSet = "0";

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

        public static float x, y, w, h;

        private void MenuContent()
        {
            UI.Button("Advance Day", GameUtil.AdvanceDay);
            UI.TextboxAction("Money", ref moneyToSet, 10,
                new UIButton("Add", () => { int.TryParse(moneyToSet, out int o); GameUtil.SendHospitalBill(-o); }),
                new UIButton("Remove", () => { int.TryParse(moneyToSet, out int o); GameUtil.SendHospitalBill(o); })
            );
            UI.TextboxAction("MetaCoins (mult 50)", ref metaToSet, 10,
                new UIButton("Add", () =>
                {
                    if (int.TryParse(metaToSet, out int o))
                        SurfaceNetworkHandler.Instance.photonView.RPC("RPCA_OnNewWeek", RpcTarget.All, o);
                })
            );

            UI.Button("Refresh Hat Store", () => HatShop.instance.Reflect().GetValue<PhotonView>("view").RPC("RPCA_StockShop", RpcTarget.All, Guid.NewGuid().GetHashCode()));
            UI.Button("Give Views / Money", () =>
            {
                UnityEngine.Object.FindObjectOfType<ExtractVideoMachine>().Reflect()
                .GetValue<PhotonView>("m_photonView").RPC("RPC_ExtractDone", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, false);
            });
            UI.Button("Open/Close Diving Bell", () => GameObjectManager.divingBell.SetDoorStateInstant(!GameObjectManager.divingBell.opened));
            UI.Button("Activate Diving Bell", Cheat.Instance<UseDivingBell>().Execute);
            UI.Button("Unlock Island Upgrades", () => { GameObjectManager.unlocks.ForEach(u => { if (u.locked) UnityEngine.Object.FindObjectOfType<IslandUnlocks>().Reflect().GetValue<PhotonView>("view").RPC("RPCA_Activate", RpcTarget.All, new int[] { u.Reflect().Invoke<int>("GetID") }); }); });
            UI.Checkbox("Anti Pickup", Cheat.Instance<AntiPickup>());
            UI.Checkbox("Join Lobbies With Plugins", Cheat.Instance<JoinWithPlugins>());
            UI.Checkbox("AntiSpawner (Auto Remove Spawned Items From Other Players)", Cheat.Instance<AntiSpawner>());
            //UI.Checkbox("Hear Push To Talk Players Always", Cheat.Instance<NoPushToTalk>()); is broken since update?
        }

        private void HelmetTextContent()
        {
            UI.Header("Message Sender");
            UI.Textbox("Search", ref searchText);

            List<LocalizationKeys.Keys> keys = Enum.GetValues(typeof(LocalizationKeys.Keys)).Cast<LocalizationKeys.Keys>().ToList().Where(key => key.ToString().ToLower().Contains(searchText.ToLower())).ToList();

            int gridWidth = 3;
            int btnWidth = (int)(SpookSuiteMenu.Instance.contentWidth * 0.5f - SpookSuiteMenu.Instance.spaceFromLeft) / gridWidth;
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
            UI.ButtonGrid(keys, key => key.ToString(), "", key => sendHelmetText(key), gridWidth, btnWidth);
            GUILayout.EndScrollView();
        }

        private void sendHelmetText(LocalizationKeys.Keys key)
        {
            SurfaceNetworkHandler.Instance.Reflect().GetValue<PhotonView>("m_View").RPC("RPCA_HelmetText", RpcTarget.All, (int)key, (object)3);
        }
    }
}
