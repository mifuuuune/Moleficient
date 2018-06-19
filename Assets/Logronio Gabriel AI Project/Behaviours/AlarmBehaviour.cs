using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmBehaviour : GeneralBehaviour
{
    public GameObject Rooster;
    private float timer = 5f;

    override public void ExecuteBehaviour(Collider[] Neighbors)
    {
        if (timer < 6f) timer += Time.deltaTime;
        if (timer > ChicksParametersManager.AlarmTime)
        {
            GameObject target = FindNearest(Neighbors);
            Rooster.GetComponent<RoosterBehaviour>().Alarm(target);
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
