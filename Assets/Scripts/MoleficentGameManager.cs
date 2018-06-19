using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleficentGameManager : MonoBehaviour {

    public static MoleficentGameManager instance = null;
    private static Vector3 LastCheckpoint = Vector3.zero;
    public GameObject[] Players = new GameObject[4];

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    public void UpdateCheckpoint(Vector3 NewCheckpoint)
    {
        LastCheckpoint = NewCheckpoint;
    }

    public Vector3 LastCheckPoint()
    {
        return LastCheckpoint;
    }

}
