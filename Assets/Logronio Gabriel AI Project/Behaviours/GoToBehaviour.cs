using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToBehaviour : GeneralBehaviour {

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    override public void ExecuteBehaviour(Collider[] Neighbors)
    {
        GameObject target = FindNearest(Neighbors);
        if (Vector3.Distance(transform.position, target.transform.position) > 0.5f) agent.SetDestination(target.transform.position);
        else agent.SetDestination(transform.position);
    }

    private GameObject FindNearest(Collider[] Neighborgs)
    {
        float distance = 0f;
        float nearestDistance = float.MaxValue;
        GameObject NearestElement = null;

        foreach (Collider NearbyElement in Neighborgs)
        {
            if (NearbyElement != null && NearbyElement.gameObject != gameObject)
            {
                distance = Vector3.Distance(NearbyElement.transform.position, transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    NearestElement = NearbyElement.gameObject;
                }
            }
        }
        return NearestElement;
    }
}
