using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Prototype.NetworkLobby
{ 

    public class ServerList : NetworkBehaviour {

        
        public static ServerList _serverlist;

        // Use this for initialization
        void Start() {
            _serverlist = this;
        }

        void getList()
        {
            foreach(MatchInfoSnapshot i in CostomLobbyManager2.s_Singleton.matches)
            {
                
            }
        }
    }
}
