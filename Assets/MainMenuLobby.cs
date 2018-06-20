using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Prototype.NetworkLobby
{
    public class MainMenuLobby : NetworkBehaviour
    {
        public GameObject ready;
        public GameObject host;
        public GameObject search;
        public GameObject exit;
        public GameObject results;

        public GameObject Room;
        public GameObject Server;
        public GameObject NumPlr;
        public GameObject Join;

        private bool hoststart = false;
        private bool broadcastingclient = false;

        public void OnClickHost()
        {
            
            host.SetActive(false);
            search.SetActive(false);
            results.SetActive(false);
            ready.SetActive(true);
            exit.SetActive(true);
            //ServerStart = true;
            BroadCaster.sigl.InitializeNetworkDiscovery();
            BroadCaster.sigl.StartBroadcast();
            CostomLobbyManager2.s_Singleton.StartHost();
            hoststart = true;
        }

        

        public void MatchList()
        {
            if (!broadcastingclient)
            {
                BroadCaster.sigl.InitializeNetworkDiscovery();
                BroadCaster.sigl.ListenBroadcast();
                broadcastingclient = true;

            }
            //Debug.Log("-----------------------------------------------------------" + BroadCaster.sigl.onServerDetected.Count);
            if (BroadCaster.sigl.onServerDetected.Count > 0)
            {
                Debug.Log("ho trovaot almeno 1 elemento nella lista");
                int i = 1;
                foreach (var data in BroadCaster.sigl.onServerDetected)
                {
                    Server.GetComponent<Text>().text = data.Key;
                    NumPlr.GetComponent<Text>().text = data.Value;
                    GameObject room = Instantiate(Room, results.transform.position + new Vector3(0, 2.0f, 0), Quaternion.identity) as GameObject;
                    room.transform.parent = results.transform;
                    room.SetActive(true);
                    i++;
                }
            }
            else
            {
                Invoke("MatchList", 2.0f);
            }
        }

        public void JoinMatch()
        {
            BroadCaster.sigl.StopBroadcast();
            var room = GameObject.FindGameObjectsWithTag("Room");
            foreach (GameObject obj in room)
            {
                Destroy(obj);
            }
            broadcastingclient = false;
            host.SetActive(false);
            search.SetActive(false);
            results.SetActive(false);
            ready.SetActive(true);
            exit.SetActive(true);
            Debug.Log("bottone premuto   -----> "+EventSystem.current.currentSelectedGameObject.name);
            Text ip = EventSystem.current.currentSelectedGameObject.transform.parent.GetChild(0).GetComponent<Text>();
            Debug.Log("ip:  --->" + ip.text);
            CostomLobbyManager2.s_Singleton.networkAddress = ip.text;
            CostomLobbyManager2.s_Singleton.StartClient();            
        }

        public void ExitToMenu()
        {
            if (broadcastingclient == true)
                broadcastingclient = false;
            BroadCaster.sigl.shutDown();
            Debug.Log("exit");
            host.SetActive(true);
            search.SetActive(true);
            results.SetActive(true);
            ready.SetActive(false);
            exit.SetActive(false);
            if (hoststart)
            {
                CostomLobbyManager2.s_Singleton.StopHost();
                hoststart = false;
            }
               
        }

        public void Ready()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("LobbyPLayer");

            foreach (GameObject p in players)
            {
                if (p.GetComponent<NetworkLobbyPlayer>().isLocalPlayer)
                {
                    //Debug.Log("sono entrato nel lobbyplayer");
                    p.GetComponent<NetworkLobbyPlayer>().readyToBegin = true;
                    p.GetComponent<NetworkLobbyPlayer>().SendReadyToBeginMessage();
                    return;
                }
            }
        }
    }
}
