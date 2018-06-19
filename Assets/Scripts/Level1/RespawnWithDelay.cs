using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RespawnWithDelay : NetworkBehaviour {

    public float Delay = 1.0f;
    private GameObject obj;

    public void MoleAbility()
    {
        CmdSeparate();
        CmdDisable();        
        Invoke("CmdReEnable", 3.0f);
    }

    [Command]
    void CmdSeparate()
    {
        RpcSeparate();
    }

    [ClientRpc]
    void RpcSeparate()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.layer == 9)
                //Debug.Log("stacco gli oggetti");
            child.parent = null;
        }
    }

    [Command]
    void CmdReEnable()
    {
        gameObject.SetActive(true);
        RpcReEnable();
    }

    [Command]
    void CmdDisable()
    {
        gameObject.SetActive(false);
        RpcDisable();
    } 
    
    [ClientRpc]
    void RpcReEnable()
    {
        gameObject.SetActive(true);
    }

    [ClientRpc]
    void RpcDisable()
    {
        gameObject.SetActive(false);
    }
}
