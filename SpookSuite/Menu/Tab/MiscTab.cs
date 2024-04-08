using SpookSuite.Manager;
using SpookSuite.Menu.Core;
using SpookSuite.Util;
using System.Collections.Generic;
using UnityEngine;

namespace SpookSuite.Menu.Tab
{
    internal class MiscTab : MenuTab
    {
        public MiscTab() : base("Misc") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 monsterScrollPos = Vector2.zero;
        private Vector2 monstersScrollPos = Vector2.zero;
        private string selectedMonsterName = "";
        private Bot selectedMonster = null;

        public List<string> monsterNames = new List<string>
        {
            "BarnacleBall", "BigSlap", "Bombs", "Dog", "Ear", "EyeGuy", "Flicker", "Ghost", "Jelly", "Knifo",
            "Larva", "Mouthe", "Slurper", "Snatcho", "Spider", "Snail", "Toolkit_Fan", "Toolkit_Hammer",
            "Toolkit_Iron", "Toolkit_Vaccuum", "Toolkit_Whisk", "Toolkit_Wisk", "Weeping"
        };

        public override void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.Label(name); //doing it like this so we could just copy paste it over
            MenuContent();
            GUILayout.EndVertical();
        }
        string helemtText = "";
        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            UI.Textbox("Helmet Text", ref helemtText);
            if (GUILayout.Button("Apply Text"))
                Player.localPlayer.refs.visor.visorFaceText.text = helemtText;
            GUILayout.EndHorizontal();

            GUILayout.Label("Monster Spawner");
            monsterScrollPos = GUILayout.BeginScrollView(monsterScrollPos);
            foreach(string monster in monsterNames)
            {
                if (GUILayout.Button(monster))
                    selectedMonsterName = monster;
            }
            GUILayout.EndScrollView();
            if(GUILayout.Button("Spawn " + selectedMonsterName))
                MonsterSpawner.SpawnMonster(selectedMonsterName);

            monstersScrollPos = GUILayout.BeginScrollView(monstersScrollPos);
            foreach (Bot monster in GameObjectManager.monsters)
            {
                if (GUILayout.Button(monster.name))
                    selectedMonster = monster;
            }
            //add stuff to do with selected

            GUILayout.EndScrollView();

            GUILayout.EndScrollView();
        }
    }
}
