using ExitGames.Client.Photon;
using Photon.Pun;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using System.ComponentModel;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug") { }

        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();
             
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Masterclient: ");
            GUILayout.FlexibleSpace();
            GUILayout.Label(PhotonNetwork.IsMasterClient ? "Yes" : "No");
            GUILayout.EndHorizontal();
                

            if (GUILayout.Button("RPCA_BombThrowAttack"))
            {
                Pickup component = PhotonNetwork.Instantiate("PickupHolder", Player.localPlayer.transform.position, UnityEngine.Random.rotation, 0, null).GetComponent<Pickup>();
                component.ConfigurePickup(58, new ItemInstanceData(Guid.NewGuid()));
            }

            foreach (Item i in UnityEngine.Object.FindObjectsOfType<Item>())
            {
                GUILayout.Button($"{i.displayName}, id: {i.id}, pid: {i.persistentID}");
            }



            //PickupHandler.CreatePickup(this.itemToSpawn.id, new ItemInstanceData(Guid.NewGuid()), 
            //    this.player.refs.ragdoll.GetBodypart(BodypartType.Elbow_R).rig.transform.GetChild(0).position,
            //    Random.rotation, vel, Random.onUnitSphere * 5f);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Add $1000");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {

                SurfaceNetworkHandler.RoomStats.AddMoney(1000);

                VerboseDebug.Log("Changing RoomStats: Money: " + SurfaceNetworkHandler.RoomStats.Money.ToString() + " Day: " + SurfaceNetworkHandler.RoomStats.CurrentDay.ToString());
                Hashtable propertiesToSet = new Hashtable();
                propertiesToSet.Add((object)"m", (object)SurfaceNetworkHandler.RoomStats.Money);
                propertiesToSet.Add((object)"d", (object)SurfaceNetworkHandler.RoomStats.CurrentDay);
                propertiesToSet.Add((object)"q", (object)SurfaceNetworkHandler.RoomStats.CurrentQuotaDay);
                propertiesToSet.Add((object)"qs", (object)SurfaceNetworkHandler.RoomStats.QuotaToReach);
                propertiesToSet.Add((object)"cq", (object)SurfaceNetworkHandler.RoomStats.CurrentQuota);
                propertiesToSet.Add((object)"s", (object)GameAPI.seed);
                propertiesToSet.Add((object)"cu", (object)SurfaceNetworkHandler.RoomStats.CurrentCamera.GetUpgradeData());
                PhotonNetwork.CurrentRoom.SetCustomProperties(propertiesToSet);



            }
            GUILayout.EndHorizontal();

            UI.Checkbox("No CLip", ref Cheat.Instance<NoClip>().Enabled);


            GUILayout.EndScrollView();
        }
    }
}
