using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpookSuite.Cheats
{
    internal class ChamESP : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 0f;
        public static float Speed = 1f;

        public static bool displayPlayers = false;
        public static bool displayEnemies = false;
        public static bool displayItems = false;
        public static bool displayDivingBell = false;
        public static bool displayLasers = false;
        public static bool rainbowMode = false;
        public static float opacity = .1f;
        private RainbowController rgb = new RainbowController();

        public ChamESP() => ChamHandler.SetupChamMaterial();

        public override void Update()
        {
            rgb.speed = Speed;
            rgb.Update();
            DisplayChams(GameObjectManager.pickups, _ => rainbowMode ? rgb.GetRGBA() : Settings.c_chamItems);
            DisplayChams(GameObjectManager.players, _ => rainbowMode ? rgb.GetRGBA() : Settings.c_chamPlayers);
            DisplayChams(GameObjectManager.enemyPlayer, _ => rainbowMode ? rgb.GetRGBA() : Settings.c_chamMonsters);
            DisplayChams(new List<Object> { GameObjectManager.divingBellButton, GameObjectManager.divingBell }, _ => rainbowMode ? rgb.GetRGBA() : Settings.c_chamDivingBell);
            DisplayChams(GameObjectManager.lasers, _ => rainbowMode ? rgb.GetRGBA() : Settings.c_chamDivingBell);
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

                    float distance = GetDistanceToPos(transform.position);
                    RGBAColor c = colorSelector((T)o);
                    c.a = opacity;
                    o.GetChamHandler().ProcessCham(distance, c);
                }
                catch (Exception e) { }
            });
        }
    }
}
