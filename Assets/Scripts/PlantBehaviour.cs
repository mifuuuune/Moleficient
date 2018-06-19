using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlantBehaviour : NetworkBehaviour {

    public int speed = 5;
    private Vector3 heightstart;
    // Update is called once per frame
    private void Start()
    {
        heightstart = this.transform.position;
    }

    private void Update () {
        if (isServer)
        {
            if (this.tag == "Plant")
            {
                if (transform.position.y < (heightstart.y + 3))
                {
                    transform.Translate(transform.up * speed * Time.deltaTime);
                }
            }
        }
        /*else if (this.tag == "Terrain")
        {
            if (transform.position.y > (heightstart.y - 3))
            {
                transform.Translate(-transform.up);
            }
        }*/
	}
}
