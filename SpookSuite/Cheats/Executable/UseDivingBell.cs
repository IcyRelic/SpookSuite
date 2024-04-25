using SpookSuite.Cheats.Core;
using SpookSuite.Manager;
using System.Collections;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class UseDivingBell : ExecutableCheat
    {
        public override void Execute() => SpookSuite.Instance.StartCoroutine(UseCoroutine());

        public IEnumerator UseCoroutine()
        {
            if (GameObjectManager.divingBell.opened)
                Cheat.Instance<ToggleDivingBell>().Execute();

            yield return new WaitForSeconds(2.6f);

            if (GameObjectManager.divingBell.onSurface)
                GameObjectManager.divingBell.GoUnderground();
            else GameObjectManager.divingBell.GoToSurface();
        }
    }
}
