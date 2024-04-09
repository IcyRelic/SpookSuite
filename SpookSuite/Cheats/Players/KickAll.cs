using Photon.Pun;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class KickAll : ExecutableCheat
    {
        public override void Execute()
        {
            PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

    }
}
