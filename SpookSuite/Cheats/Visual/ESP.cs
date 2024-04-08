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
        public override void OnGui()
        {
            //if (!Cheat.Instance<ESP>().Enabled) return;
            DisplayPlayers();
            DisplayItems();
            DisplayDivingBells();
            DisplayLasers();
            DisplayMonsters();

        }

        private void DisplayObjects<T>(IEnumerable<T> objects, Func<T, string> labelSelector, Func<T, RGBAColor> colorSelector) where T : Component
        {
            foreach (T obj in objects)
            {
                if (obj != null && obj.gameObject.activeSelf)
                {
                    float distance = GetDistanceToPlayer(obj.transform.position);

                    if (!WorldToScreen(obj.transform.position, out Vector3 screen)) continue;

                    VisualUtil.DrawDistanceString(screen, labelSelector(obj), colorSelector(obj), distance);
                }
            }
        }

        private void DisplayPlayers()
        {
            foreach(Player p in GameObjectManager.players)
            {
                float distance = GetDistanceToPlayer(p.data.groundPos);

                if (!WorldToScreen(p.data.groundPos, out Vector3 screen)) continue;

                VisualUtil.DrawDistanceString(screen, p.refs.view.Owner.NickName, Settings.c_espPlayers, distance);
            }
        }

        private void DisplayItems()
        {
            DisplayObjects(GameObjectManager.items, item => item.item.displayName, item => Settings.c_espItems);
        }

        private void DisplayDivingBells()
        {
            DisplayObjects(GameObjectManager.divingBells, divingBell => divingBell.name, divingBell => Settings.c_espDivingBells);
        }
        private void DisplayLasers()
        {
            DisplayObjects(GameObjectManager.lasers, laser => laser.name, laser => Settings.c_espMonsters);
        }
        private void DisplayMonsters()
        {
            DisplayObjects(GameObjectManager.monsters, monster => monster.name, monster => Settings.c_espMonsters);
        }
    }
}
