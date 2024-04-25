using Photon.Pun;
using Steamworks;
using SpookSuite.Cheats.Core;

namespace SpookSuite.Cheats
{
    internal class NameSpoof : ToggleCheat, IVariableCheat<string>
    {
        public static string Value = SteamFriends.GetPersonaName();

        public static void TrySetNickname()
        {
            if (!Instance<NameSpoof>().Enabled) return;

            PhotonNetwork.LocalPlayer.NickName = Value;
        }

        public override void OnEnable()
        {
            PhotonNetwork.LocalPlayer.NickName = Value;
        }
        public override void OnDisable()
        {
            PhotonNetwork.LocalPlayer.NickName = SteamFriends.GetPersonaName();
        }

        public static void OnValueChanged(string newValue)
        {
            PhotonNetwork.LocalPlayer.NickName = newValue;
        }
    }
}
