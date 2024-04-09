using SpookSuite.Cheats.Core;
using SpookSuite.Components;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class NoClip : ToggleCheat
    {

        private KBInput movement = null;

        public override void Update()
        {
            if (Player.localPlayer is null) return;

            if (Enabled)
            {
                if (movement is null) movement = Player.localPlayer.gameObject.AddComponent<KBInput>();

                movement.Configure(Player.localPlayer.data.lookDirection, Player.localPlayer.data.lookDirectionRight, Player.localPlayer.data.lookDirectionUp);

                Player.localPlayer.refs.ragdoll.GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = false);
                Player.localPlayer.refs.controller.gravity = 0;
                Player.localPlayer.refs.controller.constantGravity = 0;
                Player.localPlayer.refs.ragdoll.Reflect().Invoke("AddForce", movement.movement, ForceMode.Impulse);
            }
            else
            {
                //collider.enabled = true;
                Destroy(movement);
                movement = null;
                Player.localPlayer.refs.ragdoll.GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = true);
                Player.localPlayer.refs.controller.gravity = 80;
                Player.localPlayer.refs.controller.constantGravity = 2;
            }
        }

    }
}
