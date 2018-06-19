using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2BoulderBehaviour : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.tag == "Grahanny")
        {
            Destroy(go);
            //fine partita
            GameObject.Find("Board1").GetComponent<BoardBehaviour>().CmdFinish();

        }
        else
        {
            Destroy(gameObject);
            
        }
    }
}
