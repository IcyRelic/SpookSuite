using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Util;
using System.Linq;

namespace SpookSuite.Cheats
{
    internal class FreezeAll : ToggleCheat
    {
        public override void Update()
        {
            if (!Player.localPlayer.Handle().IsDev() || !Enabled)
                return;

            foreach (var p in GameObjectManager.players.Where(p => !p.IsLocal))
                p.Reflect().Invoke("CallSlowFor", -0f, 1f);
        }
    }
}
