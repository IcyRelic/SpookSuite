using SpookSuite.Cheats.Core;
using SpookSuite.Manager;
using SpookSuite.Util;
using System.Linq;

namespace SpookSuite.Cheats
{
    internal class ReverseOthers : ToggleCheat
    {
        private float Speed;

        public override void Update()
        {
            if (!Enabled || Cheat.Instance<FreezeAll>().Enabled) return;

            if (Cheat.Instance<SuperSpeedOthers>().Enabled)
                Speed = -SuperSpeedOthers.Value;
            else
                Speed = -1f;

            foreach (var p in GameObjectManager.players.Where(p => !p.IsLocal))
                p.Reflect().Invoke("CallSlowFor", Speed, 1f);
        }
    }
}
