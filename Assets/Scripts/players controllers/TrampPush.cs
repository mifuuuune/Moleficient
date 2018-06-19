using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampPush : MonoBehaviour {

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 9)
        {

            GameObject obj = col.gameObject.GetComponent<Rigidbody>().gameObject;

            Debug.Log("3-->" + col.relativeVelocity);/*
            Debug.Log("3-->" + col.relativeVelocity * obj.GetComponent<Rigidbody>().mass);*/
            Vector3 EnteringForce = col.relativeVelocity * obj.GetComponent<Rigidbody>().mass;
            Debug.Log(EnteringForce);
            obj.GetComponent<Rigidbody>().AddForce(-transform.up * EnteringForce.y, ForceMode.Impulse);
        }
    }
}
