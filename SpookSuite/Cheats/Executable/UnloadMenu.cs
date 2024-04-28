using SpookSuite.Cheats.Core;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class UnloadMenu : ExecutableCheat
    {
        public UnloadMenu() : base(KeyCode.End) { }

        public override void Execute() => Loader.Unload();
    }
}
