using SpookSuite.Cheats.Core;
using SpookSuite.Menu.Core;
using SpookSuite.Menu.Tab;
using System;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class DebugMode : ExecutableCheat, IVariableCheat<bool>
    {
        public static bool Value = false;
        private DateTime lastExecute;
        private TimeSpan timeBetween = TimeSpan.FromSeconds(1);
        private int executeCount = 0;
        private int requiredConsecutive = 4;

        private int prevSelectedTab = 0;

        public DebugMode() : base(KeyCode.F1, true) { }

        public override void Execute()
        {
            bool consecutive = IsConsecutive();
            executeCount++;

            if (!consecutive) executeCount = 0;

            lastExecute = DateTime.Now;
            TryToggleDebugMode();
        }

        private void TryToggleDebugMode()
        {
            if(executeCount < requiredConsecutive) return;
            Value = !Value;
            executeCount = 0;
            Console.Beep();

            if (Value)
            {
                prevSelectedTab = SpookSuiteMenu.Instance.selectedTab;
                AddDebugTabs();
                SpookSuiteMenu.Instance.selectedTab = 0;
            }
            else
            {

                SpookSuiteMenu.Instance.tabs.RemoveAll(tab => tab.isDebug);
                SpookSuiteMenu.Instance.selectedTab = prevSelectedTab;
            }
        }

        public static void AddDebugTabs()
        {
            SpookSuiteMenu.Instance.tabs.Insert(0, new DebugTab());
        }

        private bool IsConsecutive() => DateTime.Now - lastExecute < timeBetween;
    }
}
