using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Checkpoint : NetworkBehaviour {

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.layer == 9)
        {
            MoleficentGameManager.instance.UpdateCheckpoint(transform.position);
            gameObject.SetActive(false);
        }
    }
}
