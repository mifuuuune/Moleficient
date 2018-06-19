using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KnifeBehaviour : NetworkBehaviour {

    public float time = 1.5f;
    private bool wooded;
    private Vector3 Destination = Vector3.zero;

	// Use this for initialization
	private void Start () {

        wooded = false;
        this.GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine("NewDelay");
        
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Wood")
        {
            //Debug.Log(this.transform.rotation.eulerAngles);
            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
            transform.Translate(Vector3.forward * 0.25f);
            this.GetComponent<Rigidbody>().isKinematic = true;
            //Vector3 angles = this.transform.rotation.eulerAngles;
           // Debug.Log(this.transform.rotation.eulerAngles);
            wooded = true;
        }
        else
        {
            //Debug.Log("sono nell'else");
            DestroyObject();
        }
    }

    private IEnumerator NewDelay()
    {
        yield return new WaitForSeconds(1.5f);
        DestroyObject();
    }

    public void SetDestination(Vector3 Destination)
    {
        transform.LookAt(Destination);
        this.GetComponent<Rigidbody>().velocity = transform.forward * 7.0f;
    }

    private void DestroyObject()
    {
        //Debug.Log("destroy:  " + wooded);
        if (!wooded)
        {
            try
            {
                GameObject.Destroy(this.gameObject);
                NetworkServer.UnSpawn(this.gameObject);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
       
    }
}
