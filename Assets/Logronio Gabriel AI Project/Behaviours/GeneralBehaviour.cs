using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralBehaviour : MonoBehaviour {

    public abstract void ExecuteBehaviour(Collider[] Neighbors);

}
