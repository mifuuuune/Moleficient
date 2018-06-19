using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingBehaviour : GeneralBehaviour {

    public GameObject Rooster;
    private Vector3 CurrentDirection;
    private Vector3 LastRoosterPosition;
    private NavMeshAgent agent;
    private float TimeToChange = 0f;
    private float StopTime = 0.75f;
    private float timer = 0f;
    bool reset = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        CurrentDirection = Rooster.transform.position;
        LastRoosterPosition = Rooster.transform.position;
        TimeToChange = Random.Range(1.5f, 2.5f);
        StopTime = Random.Range(0.5f, 0.75f);
    }

    override public void ExecuteBehaviour(Collider[] Neighbors)
    {
        Vector3 RoosterMovement = Rooster.transform.position - LastRoosterPosition;
        timer += Time.deltaTime;

        if (RoosterMovement.magnitude <= 0.001f)
        {
            if (timer >= TimeToChange || reset)
            {
                do
                {
                    float RandomX = Random.Range(0.5f, 7.5f);
                    if (RandomX > 4f) RandomX -= 8;
                    float RandomZ = Random.Range(0.5f, 7.5f);
                    if (RandomZ > 4f) RandomZ -= 8;
                    CurrentDirection = new Vector3(Rooster.transform.position.x + RandomX, Rooster.transform.position.y, Rooster.transform.position.z + RandomZ);

                } while (Vector3.Distance(CurrentDirection, Rooster.transform.position) > 4f);
                timer = 0;
            }
            else if (timer > (TimeToChange - StopTime) && timer < TimeToChange) CurrentDirection = transform.position;

            agent.speed = 1.5f; 
        }

        else
        {
            agent.speed = RoosterMovement.magnitude / Time.deltaTime;
            CurrentDirection = CurrentDirection + (RoosterMovement);
        }

        Debug.DrawLine(transform.position, CurrentDirection, Color.green);
        agent.SetDestination(CurrentDirection);
        LastRoosterPosition = Rooster.transform.position;
        reset = false;

    }//--------------------VERSIONE CAMBIA DIREZIONE OGNI TOT SECONDI

    public void ResetDirection()
    {
        reset = true;
    }

    /*
    override public void ExecuteBehaviour(Collider[] Neighbors)
    {
        Vector3 RoosterMovement = Rooster.transform.position - LastRoosterPosition;

        if (RoosterMovement.magnitude <= 0.001f)
        {

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                do
                {
                    CurrentDirection = new Vector3(Rooster.transform.position.x + Random.Range(-4f, 4f), Rooster.transform.position.y, Rooster.transform.position.z + Random.Range(-4f, 4f));

                } while (Vector3.Distance(CurrentDirection, Rooster.transform.position) > 4f);
            }
        }
       
        else
        {
            CurrentDirection = CurrentDirection + (RoosterMovement);
        }

        agent.speed = 2f;
        Debug.DrawLine(transform.position, CurrentDirection, Color.green);
        agent.SetDestination(CurrentDirection);
        LastRoosterPosition = Rooster.transform.position;

    }--------------------VERSIONE CAMBIA DIREZIONE QUANDO L'HAI RAGGIUNTA
    */

    /*
    override public void ExecuteBehaviour(Collider[] Neighbors) {

        Vector3 RoamingDirection = Vector3.zero;

        //RoamingDirection += Align();
        //RoamingDirection += Cohesion();
        RoamingDirection += Separation(Neighbors);

        if (RoamingDirection.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation((RoamingDirection.normalized + transform.forward) / 2f);
            if (gameObject.layer == 12) transform.position += transform.forward * HensParametersManager.HenSpeed * Time.deltaTime;
            else if (gameObject.layer == 13) transform.position += transform.forward * ChicksParametersManager.ChickSpeed * Time.deltaTime;

        }

    }

    private Vector3 Align()
    {
        if (Vector3.Distance(transform.position, Rooster.transform.position) > 1f)
        {
            if (gameObject.layer == 12) return Rooster.transform.forward.normalized * HensParametersManager.HenAlignWeight;
            else if (gameObject.layer == 13) return Rooster.transform.forward.normalized * ChicksParametersManager.ChickAlignWeight;

        }
        return Vector3.zero;

    }

    private Vector3 Cohesion()
    {
        if(Vector3.Distance(transform.position, Rooster.transform.position) > 1f)
        {
            Vector3 cohesion = Rooster.transform.position;
            cohesion -= transform.position;

            if (gameObject.layer == 12) return cohesion.normalized * HensParametersManager.HenCohesionWeight;
            else if (gameObject.layer == 13) return cohesion.normalized * ChicksParametersManager.ChickCohesionWeight;

        }
        return Vector3.zero;

    }

    private Vector3 Separation(Collider[] Neighbors)
    {
        Vector3 separation = Vector3.zero;
        Vector3 tmp;
        for (int i = 0; i < Neighbors.Length; i += 1)
        {
			if (Neighbors [i] != null) {
				tmp = (transform.position - Neighbors[i].transform.position);
                Debug.Log("FOUND ONE!");
				separation += tmp.normalized / (tmp.magnitude + 0.0001f);
			}
        }
        if (gameObject.layer == 12) return separation.normalized * HensParametersManager.HenSeparationWeight;
        else if (gameObject.layer == 13) return separation.normalized * ChicksParametersManager.ChickSeparationWeight;
        return separation;

    }*/
}
