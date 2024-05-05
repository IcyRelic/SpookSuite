using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpookSuite.Manager
{
    public class GameObjectManager
    {

        private static GameObjectManager instance;
        public static GameObjectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObjectManager();
                }
                return instance;
            }
        }

        private float collectInterval = 1f; 

        public static List<Bot> monsters = new List<Bot>();
        public static List<Player> players = new List<Player>();
        public static List<Player> enemyPlayer = new List<Player>();
        public static List<PlayerRagdoll> playerRagdolls = new List<PlayerRagdoll>();
        public static List<ItemInstance> items = new List<ItemInstance>();
        public static List<Pickup> pickups = new List<Pickup>();
        public static List<Laser> lasers = new List<Laser>();
        public static List<IslandUnlock> unlocks = new List<IslandUnlock>();
        public static List<Sittable> sittables = new List<Sittable>();

        public static DivingBell divingBell;
        public static UseDivingBellButton divingBellButton;

        public static Dictionary<Guid, bool> allowedSpawns = new Dictionary<Guid, bool>();
        public static Dictionary<byte, object[]> allowedDroneSpawns = new Dictionary<byte, object[]>();

        public IEnumerator CollectObjects()
        {
            while (true)
            {
                CollectObjects(monsters);
                CollectObjects(players, obj => !obj.ai);
                CollectObjects(enemyPlayer, obj => obj.ai);
                CollectObjects(playerRagdolls);
                CollectObjects(items);
                CollectObjects(pickups);
                CollectObjects(lasers);
                CollectObjects(unlocks);
                CollectObjects(sittables);

                divingBell = Object.FindObjectOfType<DivingBell>();
                divingBellButton = Object.FindObjectOfType<UseDivingBellButton>();

                yield return new WaitForSeconds(collectInterval);
            }
        }

        private void CollectObjects<T>(List<T> list, Func<T, bool> filter = null) where T : MonoBehaviour
        {
            list.Clear();
            list.AddRange(filter == null ? Object.FindObjectsOfType<T>() : Object.FindObjectsOfType<T>().Where(filter));
            //Debug.Log($"Collected {list.Count} objects of type {typeof(T).Name}");
        }
    }
}
