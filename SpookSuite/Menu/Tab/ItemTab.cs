using UnityEngine;
using SpookSuite.Menu.Core;
using Zorro.Core;
using Random = UnityEngine.Random;
using Photon.Pun;
using System;
using System.Text;

namespace SpookSuite.Menu.Tab
{
    internal class ItemTab : MenuTab
    {
        public ItemTab() : base("Item") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 spawnableItemsPos = Vector2.zero;
        private Vector2 spawnedItemsPos = Vector2.zero;
        private Item selectedItem;
        private string selectedItemName = ""; //required so it dont crash
        private bool equipOnSpawn = false;
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

            spawnableItemsPos = GUILayout.BeginScrollView(spawnableItemsPos);
            foreach (Item item in ItemDatabase.Instance.Objects)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(item.displayName))
                {
                    selectedItem = item;
                    selectedItemName = item.displayName;
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            UI.Checkbox("Equip On Spawn", ref equipOnSpawn);
            if (GUILayout.Button("Spawn " + selectedItemName))
            {
                Pickup component = PhotonNetwork.Instantiate("PickupHolder", Player.localPlayer.data.groundPos, Random.rotation, 0, null).GetComponent<Pickup>();
                component.ConfigurePickup(selectedItem.id, new ItemInstanceData(Guid.NewGuid()));
                if (equipOnSpawn)
                    component.Interact(Player.localPlayer);         
            }

            GUILayout.EndScrollView();
        }
    }
}