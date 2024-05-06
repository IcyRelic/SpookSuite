using SpookSuite.Cheats.Core;
using SpookSuite.Manager;
using SpookSuite.Util;
using System.Linq;

namespace SpookSuite.Cheats
{
    internal class BombAll : ExecutableCheat
    {
        public static int Value = 1;
        public override void Execute()
        {
            foreach (var p in GameObjectManager.players.Where(p => !p.IsLocal))
                if (!p.data.dead) GameUtil.SpawnItem(GameUtil.GetItemByName("Bomb").id, p.refs.cameraPos.position, false, false, Value);
        }
    }
}
