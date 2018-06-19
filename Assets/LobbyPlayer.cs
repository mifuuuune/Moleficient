using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    public class LobbyPlayer : NetworkLobbyPlayer
    {
            public override void OnClientEnterLobby()
            {
                base.OnClientEnterLobby();

                //CountPLayer._instance.AddPlayer(this);


            }
        
     }
}


