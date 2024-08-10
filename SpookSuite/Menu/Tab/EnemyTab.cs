using Photon.Pun;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class EnemyTab : MenuTab
    {
        public EnemyTab() : base("Monsters") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public static Player selectedEnemy;
        public static string selectedSpawnEnemy;

        public int selectedTab = 0;
        private string[] tabs = { "Enemy Manager", "Enemy Spawner" };

        public override void Draw() //todo add stuff to these
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            selectedTab = GUILayout.Toolbar(selectedTab, tabs, style: "TabBtn");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if(PhotonNetwork.InRoom)
            {
                switch (selectedTab)
                {
                    case 0:
                        EnemyManagerTab();
                        break;
                    case 1:
                        EnemySpawnerTab();
                        break;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();        
        }

        private void EnemyManagerTab()
        {
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.3f - SpookSuiteMenu.Instance.spaceFromLeft));
            LivingEnemyList();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.7f - SpookSuiteMenu.Instance.spaceFromLeft));
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
            GeneralActions();
            EnemyActions();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void EnemySpawnerTab()
        {
            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.3f - SpookSuiteMenu.Instance.spaceFromLeft));
            SpawnMonsterList();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(SpookSuiteMenu.Instance.contentWidth * 0.7f - SpookSuiteMenu.Instance.spaceFromLeft));
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
            SpawnActions();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void GeneralActions()
        {
            UI.Header("General Actions");
            UI.Label("Features Coming Soon!");

        }
        private void EnemyActions()
        {
            UI.Header("Selected Monster Actions");
            UI.Label("Features Coming Soon!");
        }

        private void SpawnActions()
        {
            if (!PhotonNetwork.MasterClient.IsLocal)
            {
                UI.Label("Monster Spawner Requires Host");
                UI.Label("This is the only known monster you can spawn as Non Host");
                UI.Button("Spawn BigSlap", () =>
                {
                    if (!GameUtil.DoesItemExist("Old Painting"))
                        GameUtil.SpawnItem(GameUtil.GetItemByName("Old Painting").id, new Vector3(1000, 1000, 1000));

                    Object.FindObjectOfType<ArtifactBigSlapPainting>().itemInstance.CallRPC(ItemRPC.RPC0, new Zorro.Core.Serizalization.BinarySerializer());
                });
                return;
            }
            UI.Button($"Spawn {selectedSpawnEnemy}", () => MonsterSpawner.SpawnMonster(selectedSpawnEnemy));
            UI.Button($"Spawn {selectedSpawnEnemy} Randomly", () => {
                RoundSpawner rs = Object.FindObjectOfType<RoundSpawner>();
                //rs.Reflect().Invoke("SpawnMonstersOutOfSight", new List<IBudgetCost>() { });
                });
            
        }

        private void LivingEnemyList()
        {
            float width = SpookSuiteMenu.Instance.contentWidth * 0.3f - SpookSuiteMenu.Instance.spaceFromLeft * 2;
            float height = SpookSuiteMenu.Instance.contentHeight - 50;

            Rect rect = new Rect(0, 30, width, height);
            GUI.Box(rect, "Monster List");

            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));

            GUILayout.Space(25);
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (Player monster in GameObjectManager.enemyPlayer)
            {
                if (selectedEnemy is null) selectedEnemy = monster;

                if (selectedEnemy.GetInstanceID() == monster.GetInstanceID()) GUI.contentColor = Settings.c_espMonsters.GetColor();

                if (GUILayout.Button(monster.name.Subtract(7), GUI.skin.label)) selectedEnemy = monster;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void SpawnMonsterList()
        {
            float width = SpookSuiteMenu.Instance.contentWidth * 0.3f - SpookSuiteMenu.Instance.spaceFromLeft * 2;
            float height = SpookSuiteMenu.Instance.contentHeight - 50;

            Rect rect = new Rect(0, 30, width, height);
            GUI.Box(rect, "Monster List");

            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));

            GUILayout.Space(25);
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (string monster in GameUtil.monterNames)
            {
                if (string.IsNullOrEmpty(selectedSpawnEnemy)) selectedSpawnEnemy = monster;

                if (selectedSpawnEnemy == monster) GUI.contentColor = Settings.c_espMonsters.GetColor();

                if (GUILayout.Button(monster, GUI.skin.label)) selectedSpawnEnemy = monster;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
