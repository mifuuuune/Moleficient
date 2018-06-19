using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TutorialCheck : NetworkBehaviour {

    public GameObject TutorialBlock;
    private bool SirBeanArrived;
    private bool SirEalArrived;
    private bool SirLoinArrived;
    private bool SirSageArrived;

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.layer == 9)
        {
            if (!SirBeanArrived && col.gameObject.tag == "Bean") SirBeanArrived = true;
            if (!SirEalArrived && col.gameObject.tag == "Eal") SirEalArrived = true;
            if (!SirLoinArrived && col.gameObject.tag == "Loin") SirLoinArrived = true;
            if (!SirSageArrived && col.gameObject.tag == "Sage") SirSageArrived = true;
            //if (SirBeanArrived && SirEalArrived && SirLoinArrived && SirSageArrived) //TutorialBlock.SetActive(false);
                
            
            if (SirLoinArrived || SirBeanArrived || SirEalArrived || SirSageArrived) CmdDestroyBlock();
        }
        
    }

    [Command]
    public void CmdDestroyBlock()
    {
        Destroy(TutorialBlock);
        NetworkServer.UnSpawn(TutorialBlock);
    }
}
