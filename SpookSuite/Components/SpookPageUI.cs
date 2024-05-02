using SpookSuite.Menu.Game;
using UnityEngine;
using Zorro.UI;
using Object = UnityEngine.Object;

namespace SpookSuite.Components
{
    public class SpookPageUI : MonoBehaviour
    {
        private UIPageHandler pageHandler;
        
        private void Awake()
        {
            pageHandler = FindObjectOfType<UIPageHandler>();
        }

        private void Update()
        {
           if(pageHandler.currentPage is MainMenuMainPage main && main.GetComponent<MainMenuAddon>() is null)
            main.gameObject.AddComponent<MainMenuAddon>();
        }

        public static void TryAttachToPageHandler()
        {
            UIPageHandler h = Object.FindObjectOfType<UIPageHandler>();

            if (h is null || h.gameObject.GetComponent<SpookPageUI>() is not null) return;

            h.gameObject.AddComponent<SpookPageUI>();
        }
    }
}
