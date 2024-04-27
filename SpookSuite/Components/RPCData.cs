using ExitGames.Client.Photon;
using System;

namespace SpookSuite.Components
{
    public class RPCData
    {
        private static readonly TimeSpan MAX_LIFE_TIME = TimeSpan.FromSeconds(60);
        public readonly Photon.Realtime.Player sender;
        public readonly string rpc;
        public readonly Hashtable hash;
        public readonly DateTime timestamp;
        public bool suspected = false;
        public object data = null;
        public RPCData parent = null;

        public RPCData(Photon.Realtime.Player sender, string rpc, Hashtable hash)
        {
            this.sender = sender;
            this.rpc = rpc;
            this.hash = hash;
            this.timestamp = DateTime.Now;
        }

        public bool IsExpired() => (DateTime.Now - MAX_LIFE_TIME) > timestamp;
        public bool IsRecent(int seconds) => (DateTime.Now - TimeSpan.FromSeconds(seconds)) < timestamp;

        public int AgeInSeconds() => (int)(DateTime.Now - timestamp).TotalSeconds;
        public void SetSuspected(object data)
        {
            suspected = true;
            this.data = data;
        }

        public void SetSuspected() => suspected = true;
    }
}
