using UnityEngine;
using SpookSuite.Menu.Core;
using Zorro.Core;
using Random = UnityEngine.Random;
using Photon.Pun;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace SpookSuite.Menu.Tab
{
    internal class ItemTab : MenuTab
    {
        public ItemTab() : base("Item") { }

        private Vector2 scrollPos = Vector2.zero;
        private string searchText = "";
        private bool equipOnSpawn = false;
        public override void Draw()
        {
            GUILayout.BeginVertical();
            
            MenuContent();
            GUILayout.EndVertical();

        }

        private void MenuContent()
        {
            GUILayout.BeginHorizontal();
            
            UI.Textbox("Search", ref searchText);
            GUILayout.FlexibleSpace();
            UI.Checkbox("Equip on Spawn", ref equipOnSpawn);

            GUILayout.EndHorizontal();

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            List<Item> items = ItemDatabase.Instance.Objects.ToList().OrderBy(x => String.IsNullOrEmpty(x.displayName) ? x.name : x.displayName).ToList();

            UI.ButtonGrid<Item>(items, item => String.IsNullOrEmpty(item.displayName) ? item.name : item.displayName, searchText, item => SpawnItem(item.id, equipOnSpawn), 4);
            
            GUILayout.EndScrollView();
        }

        private void SpawnItem(byte itemId, bool equip = false)
        {
            Vector3 spawnPos = Player.localPlayer.data.groundPos;
            spawnPos += Player.localPlayer.transform.forward;
            spawnPos.y += 1;

            Pickup component = PhotonNetwork.Instantiate("PickupHolder", spawnPos, Random.rotation, 0, null).GetComponent<Pickup>();
            component.ConfigurePickup(itemId, new ItemInstanceData(Guid.NewGuid()));

            if (equip) component.Interact(Player.localPlayer);
        }
    }
}