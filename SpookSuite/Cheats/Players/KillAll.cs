using SpookSuite.Cheats.Core;
using SpookSuite.Manager;
using SpookSuite.Util;

namespace SpookSuite.Cheats
{
    internal class KillAll : ExecutableCheat
    {

        public override void Execute()
        {
            foreach (Player p in GameObjectManager.players)
            {
                if (!p.data.dead) p.Reflect().Invoke("CallDie");
            }
        }

    }
}
