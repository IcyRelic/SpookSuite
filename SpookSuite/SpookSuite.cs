using HarmonyLib;
using Photon.Pun;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SpookSuite
{
    public class SpookSuite : MonoBehaviour
    {
        private List<Cheat> cheats;
        private Harmony harmony;
        private SpookSuiteMenu menu;

        private static SpookSuite instance;
        public static SpookSuite Instance
        {
            get
            {
                if (instance == null) instance = new SpookSuite();
                return instance;
            }
        }

        public void Start()
        {
            instance = this;
            ThemeUtil.LoadTheme("Default");
            LoadCheats();
            DoPatching();
        }

        private void DoPatching()
        {
            harmony = new Harmony("SpookSuite");
            Harmony.DEBUG = false;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void LoadCheats()
        {
            cheats = new List<Cheat>();
            menu = new SpookSuiteMenu();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "SpookSuite.Cheats", StringComparison.Ordinal) && t.IsSubclassOf(typeof(Cheat))))
            {
                cheats.Add((Cheat)Activator.CreateInstance(type));
            }
        }

        public void FixedUpdate()
        {
            try
            {
                if(PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.FixedUpdate());
            }
            catch (Exception e)
            {
                Debug.Log($"Error in FixedUpdate: {e}");
            }
        }

        public void Update()
        {
            try
            {
                if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.Update());
            }
            catch (Exception e)
            {
                Debug.Log($"Error in Update: {e}");
            }
        }

        public void OnGUI()
        {
            try
            {
                if (Event.current.type == EventType.Repaint)
                {
                    VisualUtil.DrawString(new Vector2(5f, 2f), "SpookSuite", new RGBAColor(128, 0, 255, 1f), centered: false, bold: true, fontSize: 16);

                    if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.OnGui());
                }

                menu.Draw();
            }
            catch (Exception e)
            {
                Debug.Log($"Error in OnGUI: {e}");
            }
        }

    }
}
