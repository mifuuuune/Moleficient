using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour {

    public void changeToscene(string scene)
    {
        Application.LoadLevel(scene);
    }
}
