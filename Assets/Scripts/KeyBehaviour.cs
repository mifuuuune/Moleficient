using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KeyBehaviour : NetworkBehaviour {

    public GameObject Door;

    private void OnCollisionEnter(Collision collision)
    {
        /*GameObject coll = collision.gameObject;

        if (coll.tag.Equals("Player"))
        {*/
        CmdDestroyKey();
        // }Door.SetActive(false);
    }

    [Command]
    public void CmdDestroyKey() {
        NetworkServer.UnSpawn(Door);
        Destroy(Door);
        NetworkServer.UnSpawn(this.gameObject);
        Destroy(this.gameObject);
    }
}
