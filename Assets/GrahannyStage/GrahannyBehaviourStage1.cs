using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrahannyBehaviourStage1 : NetworkBehaviour {

    private float counter = 0.0f;
    private bool counting = false;
    public float timeLimit;
    public float rangeForNextStage;
    public float rangeForPushAway;
    public float forcePower;
    private Animator anim;

    private List<string> playerTags = new List<string>();

    private FSM fsm;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

        playerTags.Add("Bean");
        playerTags.Add("Eal");
        playerTags.Add("Loin");
        playerTags.Add("Sage");

        FSMState waiting = new FSMState();
        waiting.enterActions.Add(StopCounting);

        FSMState angry = new FSMState();
        angry.enterActions.Add(StartCounting);
        angry.stayActions.Add(UpdateCounter);

        FSMState hit = new FSMState();
        hit.enterActions.Add(Hit);

        FSMState end = new FSMState();
        end.enterActions.Add(ToSecondStage);

        FSMTransition t1 = new FSMTransition(PlayersInRange);
        FSMTransition t2 = new FSMTransition(NoPlayersInRange);
        FSMTransition t3 = new FSMTransition(TimeOver);
        FSMTransition t4 = new FSMTransition(EnoughPlayersInRange);

        waiting.AddTransition(t1, angry);
        angry.AddTransition(t2, waiting);
        angry.AddTransition(t3, hit);
        angry.AddTransition(t4, end);
        hit.AddTransition(t2, waiting);

        fsm = new FSM(waiting);
	}
	
	// Update is called once per frame
	void Update () {
        fsm.Update();
        Debug.Log("updating...");
        //Debug.Log((GameObject.FindWithTag("Bean").transform.position - transform.position).magnitude);
	}

    private void StopCounting()
    {
        counting = false;
        counter = 0.0f;
    }

    private void StartCounting()
    {
        counting = true;
        Debug.Log("inizio a contare");
    }

    private void UpdateCounter()
    {
        if (counting) counter += Time.deltaTime;
        Debug.Log(counter);
    }

    private void Hit()
    {
        //animazione
        anim.SetTrigger("RoundAttack");

        //respingi
        Invoke("PushAway", .5f);
    }

    //spinge via i giocatori in un certo range
    private void PushAway()
    {
        transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
        transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Play();
        transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().Play();
        foreach (string t in playerTags)
        {
            GameObject go = GameObject.FindGameObjectWithTag(t);
            if (go)
                if ((go.transform.position - transform.position).magnitude <= rangeForPushAway)
                    //CmdPushAway(go, forcePower);
                    go.GetComponent<Rigidbody>().AddForce((go.transform.position - transform.position).normalized * forcePower);
        }
    }

   /* [Command]
    public void CmdPushAway(GameObject go, float forcePower)
    {
        RpcPushAway(go, forcePower);
    }

    [ClientRpc]
    public void RpcPushAway(GameObject go, float forcePower)
    {
        go.GetComponent<Rigidbody>().AddForce((go.transform.position - transform.position).normalized * forcePower);
    }*/

    private void ToSecondStage()
    {
        GetComponent<ChangeSceneManager>().changeScene("GrahannyStage2");
    }

    private int NumPlayersInRange(float range)
    {
        int count = 0;
        foreach (string t in playerTags)
        {
            GameObject go = GameObject.FindGameObjectWithTag(t);
            if (go)
            {
                Debug.Log("magnitude con " + t + ": " + (go.transform.position - transform.position).magnitude);
                if ((go.transform.position - transform.position).magnitude <= range)
                    count++;
            }
        }
        Debug.Log("num players in range: " + count);
        return count;
    }

    private bool PlayersInRange()
    {
        return NumPlayersInRange(rangeForNextStage) > 0;
    }

    private bool NoPlayersInRange()
    {
        return NumPlayersInRange(rangeForPushAway) == 0;
    }

    private bool EnoughPlayersInRange()
    {
        //Debug.Log(NumPlayersInRange());
        return NumPlayersInRange(rangeForNextStage) >= 3;
    }

    private bool TimeOver()
    {
        return counter >= timeLimit;
    }

}
