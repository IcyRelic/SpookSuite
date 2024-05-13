using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using SpookSuite.Manager;

namespace SpookSuite.Cheats
{
    internal class ESP : ToggleCheat
    {
        public static bool displayPlayers = false;
        public static bool displayEnemies = false;
        public static bool displayItems = false;
        public static bool displayDivingBell = false;
        public static bool displayLasers = false;

        public override void OnGui()
        {
            if (!Cheat.Instance<ESP>().Enabled) return;
            if (displayPlayers)
                DisplayPlayers();
            if (displayItems)
                DisplayItems();
            if (displayDivingBell)
                DisplayDivingBells();
            if (displayLasers)
                DisplayLasers();
            if (displayEnemies)
                DisplayMonsters();
        }

        public static void ToggleAll()
        {
            displayPlayers = !displayPlayers;
            displayEnemies = !displayEnemies;
            displayItems = !displayItems;
            displayDivingBell = !displayDivingBell;
            displayLasers = !displayLasers;
        }
        private void DisplayObjects<T>(IEnumerable<T> objects, Func<T, string> labelSelector, Func<T, RGBAColor> colorSelector) where T : Component
        {
            try
            {
                foreach (T obj in objects)
                {
                    if (obj != null && obj.gameObject.activeSelf)
                    {
                        float distance = GetDistanceToPos(obj.transform.position);

                        if (!WorldToScreen(obj.transform.position, out Vector3 screen)) continue;

                        VisualUtil.DrawDistanceString(screen, labelSelector(obj), colorSelector(obj), distance);
                    }
                }
            }
            catch (Exception e) { }
        }

        private void DisplayPlayers()
        {
            foreach (Player p in GameObjectManager.players)
            {
                if (p.ai || p.IsLocal || p is null) continue;

                float distance = GetDistanceToPos(p.refs.cameraPos.position);

                if (!WorldToScreen(p.refs.cameraPos.position, out Vector3 screen)) continue;

                VisualUtil.DrawDistanceString(screen, p.refs.view.Owner.NickName, Settings.c_espPlayers, distance);
            }
        }

        private void DisplayItems()
        {
            DisplayObjects(GameObjectManager.items, item => item.item.GetName(), item => Settings.c_espItems);
        }

        private void DisplayDivingBells()
        {
            DisplayObjects(new[] { GameObjectManager.divingBell }, divingBell => divingBell.name, divingBell => Settings.c_espDivingBells);
            DisplayObjects(new[] { GameObjectManager.divingBellButton }, divingBell => divingBell.name, divingBell => Settings.c_espDivingBells);
        }
        private void DisplayLasers()
        {
            DisplayObjects(GameObjectManager.lasers, laser => laser.name, laser => Settings.c_espMonsters);
        }
        private void DisplayMonsters()
        {
            foreach (Player p in GameObjectManager.enemyPlayer)
            {
                if (!p.ai || p.IsLocal || p is null) continue;

                float distance = GetDistanceToPos(p.data.groundPos);

                if (!WorldToScreen(p.HeadPosition(), out Vector3 screen)) continue;

                VisualUtil.DrawDistanceString(screen, p.name.Subtract(7), Settings.c_espMonsters, distance);
            }
        }
    }
}
