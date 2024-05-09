using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Util;
using System.Linq;

namespace SpookSuite.Cheats
{
    internal class SuperSpeedOthers : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 1f;
        public override void Update()
        {
            if (!Enabled || Cheat.Instance<FreezeAll>().Enabled)
                return;

            if (Cheat.Instance<ReverseOthers>().Enabled)
                return;

            foreach (var p in GameObjectManager.players.Where(p => !p.IsLocal))
                p.Reflect().Invoke("CallSlowFor", Value, 1f);
        }
    }
}
