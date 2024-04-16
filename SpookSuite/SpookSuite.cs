using HarmonyLib;
using Photon.Pun;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Menu.Tab;
using SpookSuite.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace SpookSuite
{
    public class SpookSuite : MonoBehaviour
    {
        private List<ToggleCheat> cheats;
        private Harmony harmony;
        private SpookSuiteMenu menu;
        private PhotonView view;

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
            SetupRPC();
            LoadCheats();
            DoPatching();
            this.StartCoroutine(GameObjectManager.Instance.CollectObjects());
        }

        private void DoPatching()
        {
            harmony = new Harmony("SpookSuite");
            Harmony.DEBUG = false;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void LoadCheats()
        {
            GameUtil.LoadMonterNames();
            cheats = new List<ToggleCheat>();
            menu = new SpookSuiteMenu();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "SpookSuite.Cheats", StringComparison.Ordinal) && t.IsSubclassOf(typeof(Cheat))))
            {
                if (type.IsSubclassOf(typeof(ToggleCheat)))
                    cheats.Add((ToggleCheat)Activator.CreateInstance(type));
                else Activator.CreateInstance(type);

                Log.Info($"Loaded Cheat: {type.Name}");
            }

            Settings.Config.SaveDefaultConfig();
            Settings.Config.LoadConfig();
        }
        
        
        public void FixedUpdate()
        {
            try
            {
                if(PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.FixedUpdate());
            }
            catch (Exception e)
            {
                //Debug.Log($"Error in FixedUpdate: {e}");
            }
        }

        public void Update()
        {
            try
            {
                if (Player.localPlayer is not null)
                    Player.localPlayer.GetComponent<PlayerController>().wallClimbGravityAdjustSpeed = float.MaxValue ; //spooksuite identifier

                if (Cheat.instances.Where(c => c.WaitingForKeybind).Count() == 0)
                    Cheat.instances.FindAll(c => c.HasKeybind && Input.GetKeyDown(c.keybind)).ForEach(c =>
                    {
                       if(c.GetType().IsSubclassOf(typeof(ToggleCheat))) ((ToggleCheat)c).Toggle();
                       else if(c.GetType().IsSubclassOf(typeof(ExecutableCheat))) ((ExecutableCheat)c).Execute();
                       else Log.Error($"Unknown Cheat Type: {c.GetType().Name}");
                    });

                if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.Update());
            }
            catch (Exception e)
            {
                Log.Error($"Error in Update: {e}");
            }
        }

        public void OnGUI()
        {
            try
            {
                if (Event.current.type == EventType.Repaint)
                {
                    VisualUtil.DrawString(new Vector2(5f, 2f), "SpookSuite", new RGBAColor(128, 0, 255, 1f), centered: false, bold: true, fontSize: 16);

                    if (MenuUtil.resizing)
                    {
                        string rTitle = $"Resizing Menu\nLeft Click to Confirm, Right Click to Cancel\n{SpookSuiteMenu.Instance.windowRect.width}x{SpookSuiteMenu.Instance.windowRect.height}";


                        VisualUtil.DrawString(new Vector2(Screen.width / 2, 35f), rTitle, Settings.c_espPlayers, true, true, true, 22);
                        MenuUtil.ResizeMenu();
                    }

                    if (PhotonNetwork.InRoom) cheats.ForEach(cheat => cheat.OnGui());
                }

                menu.Draw();
            }
            catch (Exception e)
            {
                Debug.Log($"Error in OnGUI: {e}");
            }
        }

        public static void Invoke(Action action, float delay = 0) => instance.StartCoroutine(DoInvoke(action, delay));

        private static IEnumerator DoInvoke(Action action, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        public static void Repeat(Action action, int times, float timeBetween = 0) => instance.StartCoroutine(DoRepeat(action, times, timeBetween));

        private static IEnumerator DoRepeat(Action action, int times, float timeBetween = 0)
        {
            for (int i = 0; i < times; i++)
            {
                action();
                yield return new WaitForSeconds(timeBetween);
            }
        }

        /**
         * RPC Stuff
         */

        private void SetupRPC()
        {
            view = this.AddComponent<PhotonView>();
            view.OwnershipTransfer = OwnershipOption.Fixed;
            view.Synchronization = ViewSynchronization.Off;
            view.ViewID = int.MaxValue;

            PhotonNetwork.PhotonServerSettings.RpcList.Add("RPC_8136b4f5cabbcf1e77629af5436130dc");
        }

        public static void RPC(string name, RpcTarget target, params object[] args) => Instance.view.RPC(name, target, args);

        public static void CallBroadcastSSUser(Player player)
        {
            RPC("RPC_8136b4f5cabbcf1e77629af5436130dc", RpcTarget.Others, player);
        }

        [PunRPC] //Broadcast SpookSuite User
        public void RPC_8136b4f5cabbcf1e77629af5436130dc(Player player)
        {
            Log.Warning($"{player.PhotonPlayer().NickName} is using SpookSuite");
            //Respond
            CallBroadcastSSUser(Player.localPlayer);
        }

    }
}
