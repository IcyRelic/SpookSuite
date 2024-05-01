using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class ToggleMenuCheat : ExecutableCheat
    {
        public ToggleMenuCheat() : base(KeyCode.Insert) { }

        public override void Execute() 
        {
            Settings.b_isMenuOpen = !Settings.b_isMenuOpen;

            if (!PhotonNetwork.InRoom) return;

            if (Settings.b_isMenuOpen)
                MenuUtil.ShowCursor();
            else
                MenuUtil.HideCursor();
        }

    }
}
