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

            Collider[] colliders = Player.localPlayer.GetComponentsInParent<Collider>();
            PlayerRagdoll ragdoll = Player.localPlayer.refs.ragdoll;
            if (Enabled)
            {
                if (movement is null) movement = Player.localPlayer.gameObject.AddComponent<KBInput>();

                colliders.ToList().ForEach(collider => collider.enabled = false);
                Player.localPlayer.GetComponent<PlayerController>().transform.position = movement.transform.position;

                //AddForce(Vector3 force, ForceMode forceMode)

                //ragdoll.Reflect().Invoke("AddForce", movement.movement, ForceMode.Force);
                Player.localPlayer.transform.position = movement.transform.position;
            }
            else
            {
                colliders.ToList().ForEach(collider => collider.enabled = true);
                Destroy(movement);
                movement = null;
            }
        }

    }
}
