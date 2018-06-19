using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrahannyRespawnerBehaviour : NetworkBehaviour
{
    //toglie le vite
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {

            BasicController playerController = collision.gameObject.GetComponent<BasicController>();
            playerController.DecreaseLives();
            playerController.Respawn(Vector3.zero);

        }
        else
        {
            collision.gameObject.SetActive(false);
        }
    }
}
