using SpookSuite.Menu.Game;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zorro.UI;
using Object = UnityEngine.Object;

namespace SpookSuite.Components
{
    public class SpookPageUI : MonoBehaviour
    {
        private UIPageHandler pageHandler;
        private bool hasAddedPages = false;


        private void Awake()
        {
            pageHandler = FindObjectOfType<UIPageHandler>();
            
        }

        private void Update()
        {
            if (pageHandler.currentPage is not MainMenuMainPage main) return;
 
            if(main.GetComponent<MainMenuAddon>() is null)
                main.gameObject.AddComponent<MainMenuAddon>();

            AddPages();
        }

        private void AddPages()
        {
            if(hasAddedPages) return;
            //create a new page (MainMenuViewLobbiesPage) and add it to the canvas
            GameObject go = new GameObject("MainMenuViewLobbiesPage", typeof(RectTransform));
            go.transform.SetParent(FindObjectOfType<UIPageHandler>().transform, false);
            UIPage page = go.AddComponent<MainMenuViewLobbiesPage>();

            SpookPageUI.TryRegisterPage(page);

            hasAddedPages = true;
        }

        public static void TryAttachToPageHandler()
        {
            Log.Warning("Attempting To Attach SpookPageUI to UIPageHandler");
            UIPageHandler h = Object.FindObjectOfType<UIPageHandler>();

            if (h is null || h.gameObject.GetComponent<SpookPageUI>() is not null) return;

            h.gameObject.AddComponent<SpookPageUI>(); 
        }

        public static void TryRegisterPage(UIPage page) => FindObjectOfType<UIPageHandler>().Reflect().GetValue<Dictionary<Type, UIPage>>("_pages").Add(page.GetType(), page);
        public static void TransitionToPage<T>() where T : UIPage => FindObjectOfType<UIPageHandler>().TransistionToPage<T>();
    }
}
