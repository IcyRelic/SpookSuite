using SpookSuite.Cheats.Core;
using System.Collections.Generic;

namespace SpookSuite.Cheats
{
    internal class Limbo : ToggleCheat
    {
        public static List<Player> limboList = new List<Player>();
        public override void Update()
        {
            foreach (Player player in limboList)
            {
                if (player is null) 
                { 
                    limboList.Remove(player);
                    return;
                }
                
                if (!player.data.playerIsInRealm)
                    ShadowRealmHandler.instance.TeleportPlayerToRandomRealm(player);
            }
        }

        public static void AddPlayers(List<Player> players)
        {
            foreach (Player player in players)
            {
                if (!limboList.Contains(player))
                    limboList.Add(player);
            }
        }
    }
}
