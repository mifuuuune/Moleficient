using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PortalBehaviour : NetworkBehaviour {

    private int count = 0;

	public void keyPickedUp()
    {
        count++;
        if (count == 3) RpcKeyPickedUp();
    }

    [ClientRpc]
    public void RpcKeyPickedUp()
    {
        transform.GetComponent<ParticleSystem>().Play();
    }
}
