using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Data = System.Collections.Generic.KeyValuePair<string, string>;



namespace Prototype.NetworkLobby
{
    public class BroadCaster : NetworkDiscovery
    {
        public static BroadCaster sigl;
        public Dictionary<string, string> onServerDetected;
        private bool init;

        void Start()
        {
            this.showGUI = false;
            this.offsetX = 500;
            onServerDetected = new Dictionary<string, string>();
            init = false;
            if (sigl == null)
                sigl = this;
            broadcastData = "0";
            InitializeNetworkDiscovery();
        }

        public void ChangePayload()
        {
            if (isServer)
            {
                broadcastData = CountPLayer._instance.public_num_players.ToString();
                shutDown();
                Invoke("StartBroadcast", 1.0f);
            }        
        }

        public bool InitializeNetworkDiscovery()
        {
            if (!init)
            {
                init = Initialize();
                Debug.Log(init);
            }
            return Initialize();
        }
        
        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            //CostomLobbyManager2.s_Singleton.networkAddress = fromAddress;
            //Debug.Log("------------------  " + fromAddress.Split(':')[3] + "  --------------  " + data);
            if(!onServerDetected.ContainsKey(fromAddress.Split(':')[3]))
                onServerDetected.Add(fromAddress.Split(':')[3], data);
        }

        public void StartBroadcast()
        {
            Debug.Log("invio");
            StartAsServer();
        }

        public void ListenBroadcast()
        {
            Debug.Log("ascolto");
            StartAsClient();
        }

        public void shutDown()
        {
            StopBroadcast();
        }
    }
}

