using SpookSuite.Cheats.Core;
using SpookSuite.Manager;

namespace SpookSuite.Cheats
{
    internal class ReviveAll : ExecutableCheat
    {
        public override void Execute()
        {
            foreach (Player p in GameObjectManager.players)
            {
                if (p.data.dead) p.CallRevive();
            }
        }

    }
}
