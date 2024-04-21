using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpookSuite.Cheats
{
    internal class ChamESP : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 0f;

        public static bool displayPlayers = false;
        public static bool displayEnemies = false;
        public static bool displayItems = false;
        public static bool displayDivingBell = false;
        public static bool displayLasers = false;

        public ChamESP() => ChamHandler.SetupChamMaterial();

        public override void Update()
        {
            DisplayChams(GameObjectManager.pickups, _ => Settings.c_chams);
            DisplayChams(GameObjectManager.players, _ => Settings.c_chams);
            DisplayChams(GameObjectManager.enemyPlayer, _ => Settings.c_chams);
            DisplayChams(new List<Object> { GameObjectManager.divingBellButton, GameObjectManager.divingBell }, _ => Settings.c_chams);
            DisplayChams(GameObjectManager.lasers, _ => Settings.c_chams);
        }

        public static void ToggleAll()
        {
            displayPlayers = !displayPlayers;
            displayEnemies = !displayEnemies;
            displayItems = !displayItems;
            displayDivingBell = !displayDivingBell;
            displayLasers = !displayLasers;
        }

        private void DisplayChams<T>(IEnumerable<T> objects, Func<T, RGBAColor> colorSelector) where T : Object
        {
            objects.ToList().ForEach(o =>
            {
                try
                {
                    if (o is null) return;
                    Transform transform;

                    if (o is Component component && component.transform is not null) transform = component.transform;
                    else if (o is GameObject gameObject && gameObject.transform is not null) transform = gameObject.transform;
                    else return;

                    if (transform is null) return;

                    float distance = GetDistanceToPlayer(transform.position);
                    o.GetChamHandler().ProcessCham(distance);
                }
                catch (Exception e) { }
            });
        }
    }
}
