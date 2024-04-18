using Photon.Realtime;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Util;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class NameplateData : MonoBehaviour
    {
        public Player player;
        public int playerId;
    }

    internal class Nameplate : MonoBehaviour
    {
        
        public TextMeshPro tmp;
        public NameplateData data;

        void Start()
        {
            data = gameObject.GetComponent<NameplateData>();
            tmp = this.AddComponent<TextMeshPro>();
            tmp.fontSize = 1.5f;
            tmp.color = data.player.Handle().IsDev() ? Color.blue : data.player.Handle().IsSpookUser() ? Settings.c_primary.GetColor() : Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            gameObject.transform.SetParent(data.player.refs.cameraPos);
        }

        public void Update()
        {
            tmp.text = data.player.PhotonPlayer().NickName + (data.player.Handle().IsDev() ? " [Dev]" : "");

            Vector3 pos = data.player.refs.cameraPos.transform.position;
            pos.y += 0.5f;

            tmp.transform.position = pos;
            tmp.transform.LookAt(MainCamera.instance.GetCamera().transform);
            tmp.transform.Rotate(Vector3.up, 180f);
        }
    }

    internal class Nameplates : ToggleCheat
    {
        private List<Nameplate> nameplates = new List<Nameplate>();

        public override void Update()
        {
            foreach (Nameplate np in nameplates)
            {
                if (np.IsDestroyed())
                { 
                    nameplates.Remove(np);
                    continue;
                }

                Player p = PlayerHandler.instance.playerAlive.Find(x => x.GetInstanceID() == np.data.playerId);
                if (p == null || !Enabled)
                {
                    Destroy(np.gameObject);
                    nameplates.Remove(np);
                }
            }

            if(Enabled) 
                PlayerHandler.instance.playerAlive.FindAll(p => !HasNameplate(p) && !p.IsLocal).ForEach(p => CreateNameplate(p));
        }


        private bool HasNameplate(Player p) => nameplates.Find(x => x.data.playerId == p.GetInstanceID()) is not null;

        private void CreateNameplate(Player p)
        {
            GameObject go = new GameObject($"SSNP-{p.GetInstanceID()}");
            NameplateData data = go.AddComponent<NameplateData>();
            data.player = p;
            data.playerId = p.GetInstanceID();

            nameplates.Add(go.AddComponent<Nameplate>());
        }

        
    }

    
}
