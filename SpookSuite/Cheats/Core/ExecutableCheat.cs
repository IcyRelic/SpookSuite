using UnityEngine;

namespace SpookSuite.Cheats.Core
{
    internal abstract class ExecutableCheat : Cheat
    {
        public ExecutableCheat() { }
        public ExecutableCheat(KeyCode defaultKeybind) : base(defaultKeybind) { }
        public abstract void Execute();
    }
}
