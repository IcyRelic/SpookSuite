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
                PlayerHandler.instance.playersAlive.Find(x => x.GetSteamID() != Player.localPlayer.GetSteamID()).PhotonPlayer() : Player.localPlayer.PhotonPlayer());
        }
    }
}