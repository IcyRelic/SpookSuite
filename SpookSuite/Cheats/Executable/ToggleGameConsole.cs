using SpookSuite.Cheats.Core;
using SpookSuite.Util;
using UnityEngine;
using UnityEngine.UIElements;
using Zorro.Core.CLI;

namespace SpookSuite.Cheats
{
    internal class ToggleGameConsole : ExecutableCheat
    {
        private DebugUIHandler handler;


        public ToggleGameConsole() : base(KeyCode.BackQuote) { }

        public override void Execute()
        {
            Log.Info("Toggling Game Console");
            handler = FindObjectOfType<DebugUIHandler>();

            if(handler is null)
            {
                Log.Error("GameConsole not found!");
                return;
            }

            if (handler.Reflect().GetValue<UIDocument>("m_document").enabled) handler.Hide();
            else handler.Show();

        }
    }
}
