using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoardBehaviour : NetworkBehaviour {

    public Transform platformPrefab;
    public float timeActive = 0;
    [SyncVar(hook = "SyncActive")]
    private bool active;
    public Transform otherBoard;
    private BoardBehaviour otherB;
    private ParticleSystem ps;
    ParticleSystem.MainModule settings;

    public Camera cam;

    // Use this for initialization
    void Start () {
        otherB = otherBoard.GetComponent<BoardBehaviour>();
        settings = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        active = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            timeActive += Time.deltaTime;
            settings.startColor = new ParticleSystem.MinMaxGradient(Color.green);
        }
        else if (otherB.getActive()) settings.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
        else settings.startColor = new ParticleSystem.MinMaxGradient(Color.red);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            active = true;
            // Destroy(GameObject.FindWithTag("Grahanny"));
            if (otherB.getActive())
            {
            Debug.Log("debug.log----->" + gameObject.name);
                //Instantiate(platformPrefab, new Vector3(0,10,0), Quaternion.identity);
                CmdSpawn();
            }

        }
    }

    //[Command]
    public void CmdFinish()
    {
        //Debug.Log("ci sono entrato infine");
       
        cam.gameObject.SetActive(true);
        cam.enabled = true;
        GameObject.Find("EndGameManager").GetComponent<EndGameManager>().EndGame();       
    }

    [Command]
    public void CmdSpawn()
    {
       Transform x =  Instantiate(platformPrefab, new Vector3(0, 10, 0), Quaternion.identity) as Transform;
       NetworkServer.Spawn(x.gameObject);
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == 9)
        {
            active = false;
            timeActive = 0;
        }
    }

    public bool getActive()
    {
        return active;
    }

    public void setActive(bool b)
    {
        active = b;
    }

    public float getActiveTime()
    {
        return timeActive;
    }

    public void SyncActive(bool active)
    {
        //Debug.Log("sono----->" + gameObject.name + "    e sono------>    " + this.active);
        active = this.active;
    }

}
