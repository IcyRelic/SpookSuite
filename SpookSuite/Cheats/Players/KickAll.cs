using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;

namespace SpookSuite.Cheats
{
    internal class KickAll : ExecutableCheat
    {
        public override void Execute()
        {
            PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer.IsMasterClient ?
                PlayerHandler.instance.playerAlive.Find(x => x.GetSteamID() != Player.localPlayer.GetSteamID()).PhotonPlayer() : Player.localPlayer.PhotonPlayer());
        }

    }
}
