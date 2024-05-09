using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Components;
using SpookSuite.Util;
using Steamworks;
using System.Linq;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class NoClip : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 10f;

        private KBInput movement = null;

        public override void Update()
        {
            if (Player.localPlayer is null || !Enabled) return;

            if (movement is null) movement = Player.localPlayer.gameObject.AddComponent<KBInput>();

            movement.Configure(Player.localPlayer.data.lookDirection, Player.localPlayer.data.lookDirectionRight, Player.localPlayer.data.lookDirectionUp);
            movement.movementSpeed = Value;
            Player.localPlayer.refs.ragdoll.GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = false);
            Player.localPlayer.refs.controller.gravity = 0;
            Player.localPlayer.refs.controller.constantGravity = 0;
            Player.localPlayer.refs.ragdoll.Reflect().Invoke("AddForce", movement.movement, ForceMode.Impulse);
        }

        public override void OnDisable()
        {
            Destroy(movement);
            movement = null;
            Player.localPlayer.refs.ragdoll.GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = true);
            Player.localPlayer.data.sinceGrounded = 0; //prevent fast fall into ground upon disabling
            Player.localPlayer.refs.controller.gravity = 80;
            Player.localPlayer.refs.controller.constantGravity = 2;
        }
    }
}
