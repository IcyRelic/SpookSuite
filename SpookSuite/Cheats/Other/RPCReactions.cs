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
            clownem
        }

        public void React(reactionType reaction, Player sender)
        {
            if (reaction == reactionType.none)
                return;
            if (reaction == reactionType.kick)
            {
                if (!Player.localPlayer.Handle().IsDev() && sender.Handle().IsSpookUser())
                    return; //prevent kick loop
                SurfaceNetworkHandler.Instance.photonView.RPC("RPC_LoadScene", sender.PhotonPlayer(), "NewMainMenu");  //otherwise kick if we a dev
            }                
            if (reaction == reactionType.disconnect)
                ConnectionStateHandler.Instance.Disconnect();
            if (reaction == reactionType.clownem)
                sender.Call_EquipHat(HatDatabase.instance.GetIndexOfHat(GameUtil.GetHatByName("clown hair")));
        }
    }
}
