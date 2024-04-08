using UnityEngine;
using SpookSuite.Menu.Core;
using Zorro.Core;
using Random = UnityEngine.Random;

namespace SpookSuite.Menu.Tab
{
    internal class ItemTab : MenuTab
    {
        public ItemTab() : base("Item") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 spawnableItemsPos = Vector2.zero;
        private Vector2 spawnedItemsPos = Vector2.zero;
        private Item selectedItem;

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


            foreach(Item item in ItemDatabase.Instance.Objects)
            {
                GUILayout.BeginHorizontal();
                if(GUILayout.Button(item.name))
                {

                    Pickup component = PhotonNetwork.Instantiate("PickupHolder", Player.localPlayer.data.groundPos, Random.rotation, 0, null).GetComponent<Pickup>();
                    component.ConfigurePickup(item.id, new ItemInstanceData(Guid.NewGuid()));
                }
                GUILayout.EndHorizontal();
            }

            spawnedItemsPos = GUILayout.BeginScrollView(spawnedItemsPos);
            GUILayout.EndScrollView();

            GUILayout.EndScrollView();
        }
    }
}