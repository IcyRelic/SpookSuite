using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Util;
using System.Linq;

namespace SpookSuite.Cheats
{
    internal class RPCReactions : ToggleCheat
    {
        public enum reactionType
        {
            none,
            kick,
            disconnect,
            clownem,
            shadowrealm
        }

        public void React(reactionType reaction, Player sender)
        {
            switch (reaction)
            {
                case reactionType.none: return;
                case reactionType.kick:
                {
                    if (!Player.localPlayer.Handle().IsDev() && sender.Handle().IsSpookUser())
                        return; //prevent kick loop
                    SurfaceNetworkHandler.Instance.photonView.RPC("RPC_LoadScene", sender.PhotonPlayer(), "NewMainMenu");  //otherwise kick if we a dev
                    return;
                }
                case reactionType.disconnect: ConnectionStateHandler.Instance.Disconnect(); return;
                case reactionType.clownem: sender.Call_EquipHat(HatDatabase.instance.GetIndexOfHat(GameUtil.GetHatByName("clown hair"))); return;
                case reactionType.shadowrealm: if (!Player.localPlayer.Handle().IsDev() && sender.Handle().IsSpookUser()) return; ShadowRealmHandler.instance.TeleportPlayerToRandomRealm(sender); return;

                default: return;
            }
        }
    }
}
