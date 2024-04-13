using System;
using System.Collections.Generic;
using System.Text;

namespace SpookSuite.Manager
{
    internal class SpookPlayerManager
    {
        public static List<SpookPlayer> players = new List<SpookPlayer>();
        public static List<SpookPlayer> deadPlayers = new List<SpookPlayer>();
        public static List<SpookPlayer> alivePlayers = new List<SpookPlayer>();

        public static void SetPlayerSpawnBlacklisted(SpookPlayer player)
        {

        }

        public static bool GetPlayerSpawnBlacklisted(SpookPlayer player)
        {
            if (player.IsSpawningFullyBlackListed)
                return false;

            return false;
        }
    }
}
