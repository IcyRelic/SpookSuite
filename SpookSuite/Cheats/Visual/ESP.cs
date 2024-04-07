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
            DisplayObjects(GameObjectManager.players, player => player.name, player => Settings.c_espPlayers);
        }

    }
}
