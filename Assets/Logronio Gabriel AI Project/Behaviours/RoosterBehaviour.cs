using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoosterBehaviour : MonoBehaviour {

    public enum RoosterStates { ROAMING, CHASING, VULNERABLE, RESETTING}
    public RoosterStates CurrentState;

    private NavMeshAgent agent;
    private Animator anim;

    public LayerMask PlayersLayer;
    private Collider[] NearbyPlayers = new Collider[4];

    private GameObject CurrentTarget;
    private Vector3 CurrentDestination;

    private float ResettingTimer = 0f;
    private float RoamingTimer = 0f;
	private float VulnerableTimer = 0f;
	private float AttackingTimer = 0f;
    public float WalkingTime = 7;
    public float WaitingTime = 6f;

	public int Lives = 3;

	private float FOWRange = 5f;
	private float FOWAngle = 110f;
	private float VisionRange = 15f;

    // Use this for initialization
    void Start () {

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        CurrentDestination = transform.position;
        CurrentTarget = null;
        CurrentState = RoosterStates.ROAMING;

    }

    // Update is called once per frame
    void Update () {


		if (CurrentState != RoosterStates.VULNERABLE) {
            if (CurrentState == RoosterStates.RESETTING)
                ResettingState();
            if (CurrentState == RoosterStates.CHASING && CanSee (CurrentTarget))
				ChasingState ();
			if (CurrentState == RoosterStates.ROAMING)
				RoamingState ();
		} else {
			VulnerableState ();
		}
    }

    public void Alarm(GameObject target)
    {
        CurrentTarget = target;
		CurrentState = RoosterStates.CHASING;
    }

    public void ChasingState()
    {
		CurrentState = RoosterStates.CHASING;

		anim.SetBool("Walking", true);

		if (Vector3.Distance (CurrentTarget.transform.position, transform.position) < 1f) 
		{
			Attack ();
		} 
		else 
		{
			agent.speed = 4f;
			agent.SetDestination (CurrentTarget.transform.position);
		
		}
    }

	public void Trap(){
		CurrentState = RoosterStates.VULNERABLE;
		anim.SetBool ("Vulnerable", true);
	}

	public void VulnerableState(){
		
		if (VulnerableTimer < 6f) {
			
			VulnerableTimer += Time.deltaTime;

			agent.SetDestination (transform.position);

			if (Physics.OverlapSphereNonAlloc (transform.position, 2.5f, NearbyPlayers, PlayersLayer) > 2) {				

				AttackingTimer += Time.deltaTime;

			} else
				AttackingTimer = 0f;

			if (AttackingTimer > 3f) {

				anim.SetBool ("Vulnerable", false);
				AttackingTimer = 0f;
				VulnerableTimer = 0f;
				CurrentState = RoosterStates.ROAMING;
				Lives--;
				if (Lives == 0) {
					//GG;
				} else {
					PushBack (false);
				}
			}
						
		} else {
			anim.SetBool ("Vulnerable", false);
			VulnerableTimer = 0f;
			CurrentState = RoosterStates.ROAMING;
			PushBack (true);
			//Spinge tutti via e toglie una vita ai giocatori
		}
	
	}

    private void ResettingState()
    {
        CurrentState = RoosterStates.RESETTING;
        ResettingTimer += Time.deltaTime;
        if (ResettingTimer <= 0.5f)
        {
            anim.SetBool("Walking", false);
            agent.SetDestination(transform.position);

        }
        if (ResettingTimer > 1f && ResettingTimer < 12f)
        {
            anim.SetBool("Walking", true);
            agent.speed = 8.5f;
            agent.SetDestination(CurrentDestination);
        }
        if (ResettingTimer >= 12f)
        {
            ResettingTimer = 0f;
            CurrentState = RoosterStates.ROAMING;
        }
    }

	private void PushBack(bool kill){

        anim.SetTrigger("Attack");

        foreach (Collider Player in NearbyPlayers)
		{
            if(Player != null) Player.gameObject.GetComponent<Rigidbody> ().AddForce ((Player.gameObject.transform.position - transform.position).normalized * 500f, ForceMode.Impulse);
			//if (kill) {
				//Player.gameObject.GetComponent<BasicController>().Die ();
			//}
				
		}
        int RandomDestination = Random.Range(0, 4);
        switch (RandomDestination)
        {
            case 0: CurrentDestination = new Vector3(85f, 0, 85f); break;

            case 1: CurrentDestination = new Vector3(-85f, 0, 85f); break;

            case 2: CurrentDestination = new Vector3(85f, 0, -85f); break;

            case 3: CurrentDestination = new Vector3(-85f, 0, -85f); break;

        }
        CurrentState = RoosterStates.RESETTING;

    }

    public void Attack()
    {
		anim.SetTrigger ("Attack");
        CurrentTarget.GetComponent<Rigidbody>().AddForce((CurrentTarget.transform.position - transform.position).normalized * 500f, ForceMode.Impulse);
        //CurrentTarget.GetComponent<BasicController>().Die ();
        CurrentState = RoosterStates.ROAMING;
    }

    public void RoamingState()
    {
        CurrentState = RoosterStates.ROAMING;

		if (EnemyInFOW()) {
			
			CurrentState = RoosterStates.CHASING;
			return;

		} else {
			
			if(RoamingTimer < WalkingTime + WaitingTime)
			{
				RoamingTimer += Time.deltaTime;
			}

			if (RoamingTimer >= WalkingTime + WaitingTime)
			{
				anim.SetBool("Walking", true);
				float RandomX = Random.Range(-1f, 1f);
				float RandomZ = Random.Range(-1f, 1f);
				Vector3 CurrentDirection = new Vector3(RandomX, transform.position.y, RandomZ).normalized;
				CurrentDestination = transform.position + CurrentDirection * 30;
				RoamingTimer = 0;
			}
			else if (RoamingTimer > WalkingTime && RoamingTimer < (WalkingTime + WaitingTime))
			{
				anim.SetBool("Walking", false);
				CurrentDestination = transform.position;
			}

			agent.speed = 1.5f;

			Debug.DrawLine(transform.position, CurrentDestination, Color.green);
			agent.SetDestination(CurrentDestination);
		}
			
    }

	private bool EnemyInFOW(){
	
		if (Physics.OverlapSphereNonAlloc (transform.position, FOWRange, NearbyPlayers, PlayersLayer) > 0) {
			GameObject target = FindNearest (NearbyPlayers);
			Vector3 DirToTarget = (target.transform.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, DirToTarget) < FOWAngle / 2) {
				float DistToTarget = Vector3.Distance (transform.position, target.transform.position);
				if (Physics.Raycast (transform.position, DirToTarget, DistToTarget, PlayersLayer)) {
					CurrentTarget = target;
					return true;
				}
			}
		}

		return false;		
	}

	private bool CanSee(GameObject target)
	{
		RaycastHit AbilityHit;
		Ray AbilityRay = new Ray(transform.position + new Vector3(0, 1.5f, 0), target.transform.position - (transform.position + new Vector3(0, 1.5f, 0)));
		if (Physics.Raycast(AbilityRay, out AbilityHit, VisionRange))
		{
			if (AbilityHit.collider.gameObject == target)
			{
				return true;
			}
		}
		return false;

	}

	private Vector3 DirFromAngle(float AngleInDeg){
		return new Vector3 (Mathf.Sin (AngleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos (AngleInDeg * Mathf.Deg2Rad));

	}

    private GameObject FindNearest(Collider[] Neighborgs)
    {
        float distance = 0f;
        float nearestDistance = float.MaxValue;
        GameObject NearestElement = null;

        foreach (Collider NearbyElement in Neighborgs)
        {
            if (NearbyElement != null)
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
