using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EndGameManager : NetworkBehaviour {
    
    public void EndGame()
    {
        CmdEndGame();
        
    }

    [Command]
    public void CmdEndGame()
    {
        RpcEndGame();
    }

    [ClientRpc]
    public void RpcEndGame()
    {
        BasicController bc;
        if (GameObject.FindGameObjectWithTag("Loin"))
        {
            bc = GameObject.FindGameObjectWithTag("Loin").GetComponent<BasicController>();
            bc.FinalScene();
        }
        if (GameObject.FindGameObjectWithTag("Eal"))
        {
            bc = GameObject.FindGameObjectWithTag("Eal").GetComponent<BasicController>();
            bc.FinalScene();
        }
        if (GameObject.FindGameObjectWithTag("Sage"))
        {
            bc = GameObject.FindGameObjectWithTag("Sage").GetComponent<BasicController>();
            bc.FinalScene();
        }
        if (GameObject.FindGameObjectWithTag("Bean"))
        {
            bc = GameObject.FindGameObjectWithTag("Bean").GetComponent<BasicController>();
            bc.FinalScene();
        }

    }

}
