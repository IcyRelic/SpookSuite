using SpookSuite.Cheats.Core;
using SpookSuite.Manager;
using SpookSuite.Util;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class DisplayDead : ToggleCheat
    {
        public override void OnGui()
        {
            if(!Enabled) return;

            float y = 20f;

            GameObjectManager.players.Where(p => p.data.dead).ToList().ForEach(p =>
            {
                VisualUtil.DrawString(new Vector2(5, y), p.refs.view.Owner.NickName, false);
                y += 15f;
            });
        }

    }
}
