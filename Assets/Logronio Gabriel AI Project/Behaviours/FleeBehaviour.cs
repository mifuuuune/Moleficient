using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeBehaviour : GeneralBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    override public void ExecuteBehaviour(Collider[] Neighbors)
    {
        GameObject target = FindNearest(Neighbors);
        Vector3 EscapeDirection = transform.position - target.transform.position;
        agent.SetDestination(transform.position + (EscapeDirection.normalized) / 3f);
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
