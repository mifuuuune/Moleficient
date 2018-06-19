using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    public class CountPLayer : NetworkBehaviour
    {
        public static CountPLayer _instance = null;
        [Header("Prefabs Idle Lobby")]
        public GameObject playerLobby1;
        public GameObject playerLobby2;
        public GameObject playerLobby3;
        public GameObject playerLobby4;
        private GameObject pg = null, pg2=null, pg3=null, pg4=null;
        //protected List<LobbyPlayer> _players = new List<LobbyPlayer>();
        //GameObject pllob = null;

        public void Awake()
        {
            _instance = this;
        }

        public void SpawnLobbyPlayer(int player_num)
        {
            if (player_num == 1)
            {
                pg = GameObject.Instantiate<GameObject>(this.playerLobby1, new Vector3(-2.141f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                NetworkServer.Spawn(pg);
            }
            else if (player_num == 2)
            {
                pg2 = GameObject.Instantiate<GameObject>(this.playerLobby2, new Vector3(-0.548f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                NetworkServer.Spawn(pg2);
            }
            else if (player_num == 3)
            {
                pg3 = GameObject.Instantiate<GameObject>(this.playerLobby3, new Vector3(0.921f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                NetworkServer.Spawn(pg3);
            }
            else if (player_num == 4)
            {
                pg4 = GameObject.Instantiate<GameObject>(this.playerLobby4, new Vector3(2.365f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                NetworkServer.Spawn(pg4);
            }
        }

        public void DespawnLobbyPlayer(int player_num)
        {
            if (player_num == 1)
            {
                Destroy(pg);
                //pg = GameObject.Instantiate<GameObject>(this.playerLobby1, new Vector3(-2.141f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                NetworkServer.UnSpawn(pg);
            }
            else if (player_num == 2)
            {
                Destroy(pg2);
                // pg2 = GameObject.Instantiate<GameObject>(this.playerLobby2, new Vector3(-0.548f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                NetworkServer.UnSpawn(pg2);
            }
            else if (player_num == 3)
            {
                Destroy(pg3);
                //pg3 = GameObject.Instantiate<GameObject>(this.playerLobby3, new Vector3(0.921f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                NetworkServer.UnSpawn(pg3);
            }
            else if (player_num == 4)
            {
                Destroy(pg4);
                //pg4 = GameObject.Instantiate<GameObject>(this.playerLobby4, new Vector3(2.365f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                NetworkServer.UnSpawn(pg4);
            }
        }
    }
}
