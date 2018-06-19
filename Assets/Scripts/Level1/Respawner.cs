using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Respawner : NetworkBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 9)
        {
            
            BasicController playerController = col.gameObject.GetComponent<BasicController>();            
            playerController.Respawn(MoleficentGameManager.instance.LastCheckPoint());
            //Debug.Log("sono nell'if del respawner.cs");

        }
        else
        {
           // Debug.Log("sono nell'else del respawner.cs");
            col.gameObject.SetActive(false);
        }
    }
}
