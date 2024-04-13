using System;
using System.Collections.Generic;
using System.Text;

namespace SpookSuite.Manager
{
    internal class SpookPlayer
    {
        //instiantate with player from game's class
        SpookPlayer(Player player)
        {
            gamePlayer = player; 
            photonPlayer = player.refs.view.Owner;
        }
        //game player class
        public Player gamePlayer { get; private set; }
        //photon player class
        public Photon.Realtime.Player photonPlayer { get; private set; }
        
        public bool isAlive = false;
        public bool isSpawningBlacklisted = false;
        public bool isSpawningFullyBlacklisted = false;

        //call an rpc from the player
        public void CallRPC(string name, Photon.Pun.RpcTarget target, params object[] args)
        {
            gamePlayer.refs.view.RPC(name, target, args);
        }

        public void OnReceiveRPC
    }
}
