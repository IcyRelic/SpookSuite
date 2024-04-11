using SpookSuite.Cheats.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace SpookSuite.Util
{
    public class KBUtil
    {
        internal class KBCallback
        {
            private Cheat cheat;

            public KBCallback(Cheat cheat)
            {
                this.cheat = cheat;
            }

            public async void Invoke(KeyCode key)
            {
                cheat.keybind = key;
                await Task.Delay(100);
                cheat.WaitingForKeybind = false;
                Settings.Config.SaveConfig();
            }

        }

        private static readonly KeyCode[] KeyCodeBlackList = new KeyCode[]
        {
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.Space,
            KeyCode.LeftControl,
            KeyCode.LeftShift
        };

        public static void BeginChangeKeybind(Cheat cheat, params Action[] callbacks)
        {
            if (Cheat.instances.Where(c => c.WaitingForKeybind).Count() > 0) return;

            cheat.WaitingForKeybind = true;
            _ = TryGetPressedKeyTask(new KBCallback(cheat).Invoke, callbacks);
        }

        private static async Task TryGetPressedKeyTask(Action<KeyCode> callback, params Action[] otherCallbacks)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(250);

                float startTime = Time.time;
                KeyCode key = KeyCode.None;
                do
                {
                    KeyCode pressed = GetPressedKey();


                    if (pressed != KeyCode.None && !KeyCodeBlackList.Contains(pressed)) key = pressed;


                    if (Time.time - startTime > 15f) break;
                } while (key == KeyCode.None);

                if (key == KeyCode.None) return;

                callback?.Invoke(key);
                otherCallbacks.ToList().ForEach(cb => cb?.Invoke());
            });


        }

        private static KeyCode GetPressedKey()
        {
            List<KeyCode> allKeys = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToList();

            foreach (KeyCode key in allKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    return key;
                }
            }

            return KeyCode.None;
        }
    }
}
