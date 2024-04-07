using SpookSuite.Manager;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Handler
{
    public static class PlayerHandler
    {

        public static Bot GetClosestMonster(this Player player) => GameObjectManager.monsters.OrderBy(x => Vector3.Distance(x.transform.position, player.transform.position)).FirstOrDefault();

    }
}
