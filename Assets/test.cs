using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 9)
        {
           
            GameObject obj = col.gameObject.GetComponent<Rigidbody>().gameObject;
           
            /*Debug.Log("1-->" + obj.GetComponent<Rigidbody>().velocity);
            Debug.Log("2-->" + obj.GetComponent<Rigidbody>().velocity * obj.GetComponent<Rigidbody>().mass);
            Debug.Log("3-->" + col.relativeVelocity);
            Debug.Log("3-->" + col.relativeVelocity * obj.GetComponent<Rigidbody>().mass);
            /*Vector3 EnteringForce = obj.GetComponent<Rigidbody>().velocity * obj.GetComponent<Rigidbody>().mass;
            CmdTrampoline(EnteringForce, obj);*/

            //rb.AddForce(transform.up * BasicController.JumpForce, ForceMode.Impulse);
        }
    }
}
