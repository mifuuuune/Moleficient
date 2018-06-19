using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngageBehaviour : GeneralBehaviour
{
    private float timer = 2f;
    public float PushForce = 7f;

    override public void ExecuteBehaviour(Collider[] Neighbors)
    {
        if (timer < 2f) timer += Time.deltaTime;
        GameObject target = FindNearest(Neighbors);
        if (timer > HensParametersManager.AttackTime)
        {
            target.GetComponent<Rigidbody>().AddForce(transform.forward * PushForce, ForceMode.Impulse);
            transform.LookAt(target.transform);
            timer = 0;
        }
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
