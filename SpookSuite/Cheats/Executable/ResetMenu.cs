using SpookSuite.Cheats.Core;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class ResetMenu : ExecutableCheat
    {
        public ResetMenu() : base(KeyCode.F9) { }

        public override void Execute()
        {
            Modal.Show("Are you sure?", "Resseting the menu will cause you to lose anything you have saved, including keybinds, toggles, or other values such as speeds.", new ModalOption[] { new ModalOption("Yes", Settings.Config.SaveDefaultConfig), new ModalOption("No")});
            Settings.Config.SaveDefaultConfig();
        }
    }
}
