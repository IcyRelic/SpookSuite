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
        public static List<ItemInstance> items = new List<ItemInstance>();
        public static List<UseDivingBellButton> divingBells = new List<UseDivingBellButton>();
        public static List<Laser> lasers = new List<Laser>();

        public IEnumerator CollectObjects()
        {
            while (true)
            {
                CollectObjects(monsters);
                CollectObjects(players);
                CollectObjects(items);
                CollectObjects(divingBells);
                CollectObjects(lasers);


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
