using CurvedUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using SpookSuite.Cheats;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System;
using UnityEngine;
using static Photon.Pun.UtilityScripts.TabViewManager;

namespace SpookSuite.Menu.Tab
{
    internal class EnemyTab : MenuTab
    {
        public EnemyTab() : base("Monsters") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public static Bot selectedEnemy;
        public static string selectedSpawnEnemy;

        public int selectedTab = 0;
        private string[] tabs = new string[] { "Enemy Manager", "Enemy Spawner" };

        public override void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            selectedTab = GUILayout.Toolbar(selectedTab, tabs, style: "TabBtn");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            switch(selectedTab)
            {
                case 0:
                    EnemyManagerTab();
                    break;
                case 1:
                    EnemySpawnerTab();
                    break;
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
            UI.Button("Destroy All Monsters", () => BotHandler.instance.DestroyAll());

        }
        private void EnemyActions()
        {
            UI.Header("Selected Monster Actions");


        }

        private void SpawnActions()
        {
            UI.Button("Spawn Monster", () => MonsterSpawner.SpawnMonster(selectedSpawnEnemy));
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

            foreach (Bot monster in GameObjectManager.monsters)
            {
                if (selectedEnemy is null) selectedEnemy = monster;

                if (selectedEnemy.GetInstanceID() == monster.GetInstanceID()) GUI.contentColor = Settings.c_espMonsters.GetColor();

                if (GUILayout.Button(monster.name, GUI.skin.label)) selectedEnemy = monster;

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
