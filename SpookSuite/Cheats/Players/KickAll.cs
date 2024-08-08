using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;

namespace SpookSuite.Cheats
{
    internal class KickAll : ExecutableCheat
    {
        public override void Execute()
        {
            SurfaceNetworkHandler.Instance.photonView.RPC("RPC_LoadScene", PhotonNetwork.LocalPlayer.IsMasterClient ?
                GameObjectManager.players.Find(p => p != Player.localPlayer).PhotonPlayer() : PhotonNetwork.MasterClient, "NewMainMenu");
        }
    }
}