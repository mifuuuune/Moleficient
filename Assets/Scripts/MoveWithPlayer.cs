using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        col.gameObject.transform.parent = transform;
    }

    void OnCollisionExit(Collision col)
    {
        col.gameObject.transform.parent = null;
        DontDestroyOnLoad(col.gameObject);
    }
}
