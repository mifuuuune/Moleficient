using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LambSpawner : NetworkBehaviour {

    public int maxNumber;
    public float spawnTime;
    public Transform prefab;
    public Transform prefabWithSpring;
    public float percentage;
    private float timeFromLastSpawned = 0;
    private List<Vector3> finalPoints = new List<Vector3>();
    private List<Vector3> targetPoints = new List<Vector3>();
    private int currentNumberOfLambs = 0;

	// Use this for initialization
	void Start () {
        finalPoints.Add(new Vector3(0,0,-16.0f));
        finalPoints.Add(new Vector3(0,0,16.0f));
        finalPoints.Add(new Vector3(16.0f,0,0));
        finalPoints.Add(new Vector3(-16.0f,0,0));
        finalPoints.Add(new Vector3(-11.0f,0,-11.0f));
        finalPoints.Add(new Vector3(-11.0f,0,11.0f));
        finalPoints.Add(new Vector3(11.0f,0,-11.0f));
        finalPoints.Add(new Vector3(11.0f,0,11.0f));

        targetPoints.Add(new Vector3(0, 0, -14.70f));
        targetPoints.Add(new Vector3(0, 0, 14.70f));
        targetPoints.Add(new Vector3(14.70f, 0, 0));
        targetPoints.Add(new Vector3(-14.70f, 0, 0));
        targetPoints.Add(new Vector3(-10.3f, 0, -10.3f));
        targetPoints.Add(new Vector3(-10.3f, 0, 10.3f));
        targetPoints.Add(new Vector3(10.3f, 0, -10.3f));
        targetPoints.Add(new Vector3(10.3f, 0, 10.3f));

        //maxNumber = 15;
    }
	
	// Update is called once per frame
	void Update () {
		if (currentNumberOfLambs < maxNumber && timeFromLastSpawned >= spawnTime)
        {
            Random rnd = new Random();
            int start = Random.Range(0, finalPoints.Count);
            int end;
            do
            {
                end = Random.Range(0, finalPoints.Count);
            }
            while (end == start);
            /*Debug.Log("start: " + finalPoints[start]);
            Debug.Log("end: " + finalPoints[end]);*/
            Transform lamb = null;
            if (Random.Range(0, 100) < percentage)
            {
                lamb = Instantiate(prefabWithSpring, finalPoints[start], Quaternion.LookRotation(finalPoints[end]));
                NetworkServer.Spawn(lamb.gameObject);
                //CmdSpawnLamb(prefabWithSpring.gameObject, finalPoints[start], Quaternion.LookRotation(finalPoints[end]));
                //RpcSpawnLamb(prefab.gameObject, finalPoints[start], Quaternion.LookRotation(finalPoints[end]));
            }
            else
            {
                lamb = Instantiate(prefab, finalPoints[start], Quaternion.LookRotation(finalPoints[end]));
                NetworkServer.Spawn(lamb.gameObject);
                //CmdSpawnLamb(prefab.gameObject, finalPoints[start], Quaternion.LookRotation(finalPoints[end]));
                //RpcSpawnLamb(prefab.gameObject, finalPoints[start], Quaternion.LookRotation(finalPoints[end]));
            }
            Pathfinding pf = lamb.GetComponent<Pathfinding>();
            pf.askGrid();
            pf.startingPoint = targetPoints[start];
            pf.finalPoint = finalPoints[end];
            pf.target = targetPoints[end];
            currentNumberOfLambs++;
            timeFromLastSpawned = 0;
        }
        else timeFromLastSpawned += Time.deltaTime;
    }

    public void lambArrived(Transform t)
    {
        int start = Random.Range(0, finalPoints.Count);
        int end;
        do
        {
            end = Random.Range(0, finalPoints.Count);
        }
        while (end == start);

        t.position = finalPoints[start];
        Pathfinding pf = t.GetComponent<Pathfinding>();
        pf.startingPoint = targetPoints[start];
        pf.finalPoint = finalPoints[end];
        pf.target = targetPoints[end];
    }
}
