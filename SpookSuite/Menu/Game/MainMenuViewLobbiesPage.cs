using SpookSuite.Components;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpookSuite.Menu.Game
{
    internal class MainMenuViewLobbiesPage : MainMenuPage
    {

        private Canvas canvas;
        private float screenWidth;
        private float screenHeight;

        private Button backBtn;

        void Awake()
        {
            return;
            canvas = FindObjectOfType<Canvas>();
            screenHeight = canvas.pixelRect.height;
            screenWidth = canvas.pixelRect.width;

            //anchor the transform to top center
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0);

            //create some dummy ui elements to show that the page is being created
            //create some dummy ui elements to show that the page is being created
            GameObject go = new GameObject("ViewLobbiesText", typeof(RectTransform));
            go.transform.SetParent(canvas.transform);
            go.AddComponent<TextMeshProUGUI>();
            go.GetComponent<TextMeshProUGUI>().text = "View Lobbies";
            go.GetComponent<TextMeshProUGUI>().fontSize = 24;
            go.GetComponent<TextMeshProUGUI>().color = Color.white;
            go.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            go.GetComponent<TextMeshProUGUI>().transform.position = new Vector3(screenWidth / 2, screenHeight - 300, 0);
            go.GetComponent<TextMeshProUGUI>().transform.localScale = new Vector3(1f, 1f, 1f);


            //add a button to go back to MainMenuMainPage
            backBtn = MainMenuAddon.CreateBtnCopy("backBtn", "Back To MainMenu", 100, transform).GetComponent<Button>();
            backBtn.onClick.AddListener(() =>
            {
                SpookPageUI.TransitionToPage<MainMenuMainPage>();
            });
             
            
        }

    }
}
