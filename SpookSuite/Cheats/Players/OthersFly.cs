using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Manager;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class OthersFly : ToggleCheat
    {
        public static float Value;
        public override void Update()
        {
            if (!Enabled) return;

            foreach (Player p in GameObjectManager.players.Where(p => !p.IsLocal))
            {
                Vector3 input = new Vector3();

                if (p.input.movementInput.y > 0) input += p.refs.cameraPos.forward * Value;
                if (p.input.movementInput.y < 0) input -= p.refs.cameraPos.forward * Value;
                if (p.input.movementInput.x > 0) input += p.refs.cameraPos.right * Value;
                if (p.input.movementInput.x < 0) input -= p.refs.cameraPos.right * Value;
                if (p.input.jumpIsPressed) input += p.refs.cameraPos.up * Value;
                if (p.input.crouchIsPressed) input -= p.refs.cameraPos.up * Value;

                if (input.Equals(Vector3.zero))
                    return;

                //sprintMultiplier = p.input.sprintIsPressed ? Mathf.Min(sprintMultiplier + (5f * Time.deltaTime), 5f) : 1f;
                //p.refs.view.RPC("RPCA_Fall", RpcTarget.All, 0f); //prevent ragdoll stuff
                p.refs.view.RPC("RPCA_AddForceToBodyParts", RpcTarget.All, new int[] { p.refs.ragdoll.Reflect().Invoke<int>("GetBodyPartID", false, BodypartType.Hip)}, new Vector3[] { input });
            }
        }
    }
}
