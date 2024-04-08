using SpookSuite.Menu.Core;
using UnityEngine;

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
            MenuContent();
            GUILayout.EndVertical();

        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            spawnableItemsPos = GUILayout.BeginScrollView(spawnableItemsPos);
            //foreach (Item item in ItemDatabase..lastLoadedItems)
            //{
            //    if (GUILayout.Button(item.displayName))
            //    {

            //    }
            //}
            GUILayout.EndScrollView();

            spawnedItemsPos = GUILayout.BeginScrollView(spawnedItemsPos);
            GUILayout.EndScrollView();

            GUILayout.EndScrollView();
        }
    }
}