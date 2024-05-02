using Photon.Realtime;
using SpookSuite.Cheats.Core;
using SpookSuite.Handler;
using SpookSuite.Util;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace SpookSuite.Cheats
{
    internal class NameplateData : MonoBehaviour
    {
        public Player player;
        public ulong playerSteamID;
    }

    internal class Nameplate : MonoBehaviour
    {
        public TextMeshPro tmp;
        public NameplateData data;
        public bool isDestroyed = false;

        void Start()
        {
            data = gameObject.GetComponent<NameplateData>();
            tmp = this.gameObject.AddComponent<TextMeshPro>();
            tmp.fontSize = 1.5f;
            tmp.color = data.player.Handle().IsDev() ? Color.blue : data.player.Handle().IsSpookUser() ? Settings.c_primary.GetColor() : Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            gameObject.transform.SetParent(data.player.refs.cameraPos);
        }

        public void Update()
        {
            tmp.text = data.player.PhotonPlayer().NickName;

            Vector3 pos = data.player.refs.cameraPos.transform.position;
            pos.y += 0.5f;

            tmp.transform.position = pos;
            tmp.transform.LookAt(MainCamera.instance.GetCamera().transform);
            tmp.transform.Rotate(Vector3.up, 180f);
        }

        void OnDestroy()
        {
            isDestroyed = true;
        }
    }

    internal class Nameplates : ToggleCheat
    {
        private List<Nameplate> nameplates = new List<Nameplate>();

        public override void Update()
        {
            nameplates.ToList().FindAll(x => x.isDestroyed || !PlayerHandler.instance.playersAlive.Any(p => x.data.playerSteamID == p.GetSteamID().m_SteamID)).ForEach(x => DestroyNameplate(x));

            if (Enabled)
                PlayerHandler.instance.playersAlive.FindAll(p => !HasNameplate(p) && !p.IsLocal).ForEach(p => CreateNameplate(p));
            else nameplates.ToList().ForEach(x => DestroyNameplate(x));
        }

        private void DestroyNameplate(Nameplate np)
        {
            if (!np.isDestroyed)
                Destroy(np.gameObject);

            nameplates.Remove(np);
        }

        private bool HasNameplate(Player p) => nameplates.ToList().Any(x => x.data.playerSteamID == p.GetSteamID().m_SteamID);

        private void CreateNameplate(Player p)
        {
            GameObject go = new GameObject($"SSNP-{p.GetSteamID().m_SteamID}");
            NameplateData data = go.AddComponent<NameplateData>();
            data.player = p;
            data.playerSteamID = p.GetSteamID().m_SteamID;

            nameplates.Add(go.AddComponent<Nameplate>());
        }       
    }   
}
