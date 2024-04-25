using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Util;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class Shuv : ToggleCheat
    {
        private static float Charge { get; set; } = 0f;
        private const float ChargePower = 10f;
        private const float MaxFallTime = 1.5f;

        private bool shuvenemies = true;

        public Shuv() : base(KeyCode.E) { }

        public override void Update()
        {
            Player p = Player.localPlayer;

            if (Input.GetKey(keybind) && p.refs != null && p.refs.items != null)
            {
                Charge = Mathf.MoveTowards(Charge, 2f, Time.deltaTime);
                if (Time.time > p.refs.items.Reflect().GetValue<float>("shakeTime") + 0.1f)
                {
                    GamefeelHandler.instance.Reflect().GetValue<PerlinShake>("perlin").AddShake(Charge, 0.2f, 15f);
                    p.refs.items.Reflect().SetValue("shakeTime", Time.time);
                    return;
                }
            }
            if (!Input.GetKey(keybind) && Charge > 0.25f)
            {

                var rayHit = HelperFunctions.LineCheck(p.refs.cameraPos.position, p.refs.cameraPos.position + p.refs.cameraPos.forward * 2f,
                    HelperFunctions.LayerType.All, 0.5f);

                if (rayHit.collider != null)
                {
                    var player = rayHit.collider.transform.parent.GetComponentInParent<Player>();
                    if (player != null)
                    {
                        if (!player.ai || shuvenemies)
                        {

                            player.Reflect().Invoke("CallTakeDamageAndAddForceAndFall", 0f, p.refs.cameraPos.forward * Charge * (ChargePower / (player.ai ? 4f : 1f)), Charge * MaxFallTime + 0.5f);
                            player.Reflect().Invoke("CallMakeSound", 0);

                            SpookSuite.Invoke(() => player.Handle().RPC("RPCA_FALL", RpcTarget.All, 3), 1);
                        }
                    }
                }
                Charge = 0f;
            }
            if (!Input.GetKey(keybind) && Charge > 0) Charge = 0f;
        }
    }
}
