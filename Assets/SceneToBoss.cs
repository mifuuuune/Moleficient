using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SceneToBoss : NetworkBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            CmdChange("GrahannyStage1");
        }
    }

    [Command]
    public void CmdChange(string scene)
    {
        RpcSaveSpawn();

        //Invoke("Resetcamera", 1.0f);
        Prototype.NetworkLobby.CostomLobbyManager2.s_Singleton.ServerChangeScene(scene);
        /*if (GameObject.FindWithTag("Bean").GetComponent<NetworkIdentity>().hasAuthority)
        {
            
           Debug.Log("bean -> ho autorità");
            GameObject.FindWithTag("Bean").transform.position = new Vector3(-9.5f, 5.0f, -33.5f);
        }*/
    }

    //affinchè i player possano respawnare devono rendere gli spawn point permanenti
    [ClientRpc]
    public void RpcSaveSpawn()
    {
        //sposto i personaggi, cambiare le coordinate dei vector3 (attenzione a dove si mette!!!!!)


        if (GameObject.FindWithTag("Loin"))
        {
            Debug.Log("Loin -> ho autorità");
            GameObject.Find("sir_loin_spawn").transform.position = new Vector3(-3.5f, 5.0f, -8.2f);
            GameObject.FindWithTag("Loin").transform.position = new Vector3(-3.5f, 1.0f, -8.2f);
            GameObject.FindWithTag("Loin").GetComponent<BasicController>().BossUI();

            /*camer = GameObject.FindWithTag("Loin").transform.GetChild(3).GetChild(0).GetComponent<Camera>();
            camer.clearFlags = CameraClearFlags.SolidColor;
            camer.backgroundColor = Color.black;
            camer.cullingMask = 0;*/
            // Invoke("Resetcamera", 1.0f);

        }
        if (GameObject.FindWithTag("Eal")/*.GetComponent<NetworkIdentity>().hasAuthority*/)
        {
            Debug.Log("eal -> ho autorità");
            GameObject.FindWithTag("Eal").transform.position = new Vector3(-9.5f, 1.0f, -10.5f);
            GameObject.Find("sir_eal_spawn").transform.position = new Vector3(-9.5f, 1.0f, -10.5f);
            GameObject.FindWithTag("Eal").GetComponent<BasicController>().BossUI();
            /*camer = GameObject.FindWithTag("Eal").transform.GetChild(3).GetChild(0).GetComponent<Camera>();
            camer.clearFlags = CameraClearFlags.SolidColor;
            camer.backgroundColor = Color.black;
            camer.cullingMask=0;*/
            //Invoke("Resetcamera", 1.0f);
        }
        if (GameObject.FindWithTag("Bean"))
        {
            Debug.Log("bean -> ho autorità");
            GameObject.FindWithTag("Bean").transform.position = new Vector3(-7.5f, 1.0f, -10.5f);
            GameObject.Find("sir_bean_spawn").transform.position = new Vector3(-7.5f, 1.0f, -10.5f);
            GameObject.FindWithTag("Bean").GetComponent<BasicController>().BossUI();
            /*camer = GameObject.FindWithTag("Bean").transform.GetChild(3).GetChild(0).GetComponent<Camera>();
            camer.clearFlags = CameraClearFlags.SolidColor;
            camer.backgroundColor = Color.black;
            camer.cullingMask = 0;*/
            //Invoke("Resetcamera", 1.0f);
        }
        if (GameObject.FindWithTag("Sage"))
        {
            Debug.Log("Sage -> ho autorità");
            GameObject.FindWithTag("Sage").transform.position = new Vector3(-9.5f, 1.0f, -5.5f);
            GameObject.Find("sir_sage_spawn").transform.position = new Vector3(-9.5f, 1.0f, -5.5f);
            GameObject.FindWithTag("Sage").GetComponent<BasicController>().BossUI();
            /*camer = GameObject.FindWithTag("Sage").transform.GetChild(3).GetChild(0).GetComponent<Camera>();
            camer.clearFlags = CameraClearFlags.SolidColor;
            camer.backgroundColor = Color.black;
            camer.cullingMask = 0;*/
            // Invoke("Resetcamera", 1.0f);
        }
        //sposto gli spawnpoints
        /* GameObject.Find("sir_loin_spawn").transform.position = new Vector3(-3.5f, 5.0f, -8.2f);
         GameObject.Find("sir_eal_spawn").transform.position = new Vector3(-9.5f, 5.0f, -33.5f);
         GameObject.Find("sir_bean_spwan").transform.position = new Vector3(-9.5f, 5.0f, -33.5f);
         GameObject.Find("sir_sage_spawn").transform.position = new Vector3(-9.5f, 5.0f, -33.5f);*/
    }
}
