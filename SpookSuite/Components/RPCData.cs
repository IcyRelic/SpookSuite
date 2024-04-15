using ExitGames.Client.Photon;
using System;

namespace SpookSuite.Components
{
    internal class RPCData
    {
        private static readonly TimeSpan MAX_LIFE_TIME = TimeSpan.FromSeconds(60);
        public readonly Photon.Realtime.Player sender;
        public readonly string rpc;
        public readonly Hashtable data;
        public readonly DateTime timestamp;

        public RPCData(Photon.Realtime.Player sender, string rpc, Hashtable data)
        {
            this.sender = sender;
            this.rpc = rpc;
            this.data = data;
            this.timestamp = DateTime.Now;
        }

        public bool IsExpired() => (DateTime.Now - MAX_LIFE_TIME) > timestamp;
        public bool IsRecent(int seconds) => (DateTime.Now - TimeSpan.FromSeconds(seconds)) > timestamp;
    }
}
