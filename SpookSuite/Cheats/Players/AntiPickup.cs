using SpookSuite.Cheats.Core;
using SpookSuite.Manager;
using SpookSuite.Util;
using Steamworks;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class AntiPickup : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;

            foreach (var p in GameObjectManager.players.Where(p => !p.IsLocal))
            {
                if (p.TryGetGlobalPlayerData(out var o))
                {
                    for (int i = 0; i < o.inventory.slots.Length; i++)
                    {
                        if (o.inventory.slots[i].ItemInSlot.item is not null)
                            o.inventory.SyncClearSlot(i); //for some reason giving null ref
                        else 
                            continue;
                    }
                }
            }             
        }
    }
}
