using SpookSuite.Menu.Core;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SpookSuite.Util
{
    public class MenuUtil
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        public static bool resizing = false;
        public static float MouseX => Input.mousePosition.x;
        public static float MouseY => Screen.height - Input.mousePosition.y;
        public static float maxWidth = Screen.width - (Screen.width * 0.1f);
        public static float maxHeight = Screen.height - (Screen.height * 0.1f);
        private static int oldWidth, oldHeight;

        private static CursorLockMode lockMode = CursorLockMode.Confined;

        public static void BeginResizeMenu()
        {
            if (resizing) return;
            WarpCursor();
            resizing = true;
            oldWidth = Settings.i_menuWidth;
            oldHeight = Settings.i_menuHeight;
        }

        public static void WarpCursor()
        {
            float currentX = SpookSuiteMenu.Instance.windowRect.x + SpookSuiteMenu.Instance.windowRect.width;
            float currentY = SpookSuiteMenu.Instance.windowRect.y + SpookSuiteMenu.Instance.windowRect.height;

            SetCursorPos((int)currentX, (int)currentY);
        }

        public static void ResizeMenu()
        {
            if (!resizing) return;
            
            if (Input.GetMouseButtonDown(0))
            {
                resizing = false;
                Settings.Config.SaveConfig();
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                resizing = false;
                Settings.i_menuWidth = oldWidth;
                Settings.i_menuHeight = oldHeight;

                Settings.Config.SaveConfig();
                return;
            }

            float currentX = SpookSuiteMenu.Instance.windowRect.x + SpookSuiteMenu.Instance.windowRect.width;
            float currentY = SpookSuiteMenu.Instance.windowRect.y + SpookSuiteMenu.Instance.windowRect.height;

            Settings.i_menuWidth = (int)Mathf.Clamp(MouseX - SpookSuiteMenu.Instance.windowRect.x, 500, maxWidth);
            Settings.i_menuHeight = (int)Mathf.Clamp(MouseY - SpookSuiteMenu.Instance.windowRect.y, 250, maxHeight);
            SpookSuiteMenu.Instance.Resize();
        }

        public static void ShowCursor()
        {
            //LethalMenu.localPlayer?.playerActions.Disable();
            Object.FindObjectOfType<CursorHandler>().enabled = false;
            Cursor.visible = true;
            lockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public static void HideCursor()
        {
            //LethalMenu.localPlayer?.playerActions.Enable();
            Cursor.visible = false;
            Cursor.lockState = lockMode;
            Object.FindObjectOfType<CursorHandler>().enabled = true;
        }

        public static void ToggleCursor()
        {
            if (Cursor.visible)
                HideCursor();
            else
                ShowCursor();
        }
    }
}
