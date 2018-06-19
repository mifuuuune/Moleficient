using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrahannyBehaviourStage2 : NetworkBehaviour {

    public Transform board1;
    public Transform board2;

    public float distanceA;
    public float distanceB;
    public float distanceC;

    private BoardBehaviour bb1;
    private BoardBehaviour bb2;
    private PlayerFollower pf;
    private Animator anim;

    private DecisionTree dt;
    private List<string> playerTags = new List<string>();

    // Use this for initialization
    void Start () {
        board1 = GameObject.Find("Board1").transform;
        board2 = GameObject.Find("Board2").transform;
        anim = GetComponent<Animator>();
        bb1 = board1.GetComponent<BoardBehaviour>();
        bb2 = board2.GetComponent<BoardBehaviour>();
        pf = GetComponent<PlayerFollower>();

        playerTags.Add("Bean");
        playerTags.Add("Eal");
        playerTags.Add("Loin");
        playerTags.Add("Sage");

        //DTAction a1 = new DTAction(FollowPlayer);
        //dt = new DecisionTree(a1);
        if (isServer)
        {
            //define decisions
            DTDecision d1 = new DTDecision(IsABoardActiveFor2Seconds);
            DTDecision d2 = new DTDecision(MoleIsAlive);
            DTDecision d3 = new DTDecision(IsABoardActiveFor5Seconds);
            DTDecision d4 = new DTDecision(AtLeastOneInAreaA);
            DTDecision d5 = new DTDecision(AtLeastTwoInAreaB);
            DTDecision d6 = new DTDecision(AtLeastOneInAreaC);
            DTDecision d7 = new DTDecision(AtLeastTwoInAreaC);
            DTDecision d8 = new DTDecision(MoleIsAlive);
            DTDecision d9 = new DTDecision(MoleIsAlive);

            //define actions
            DTAction a1 = new DTAction(TpAtActiveBoard);
            DTAction a2 = new DTAction(LoadRoundAttack);
            DTAction a3 = new DTAction(LoadFrontAttack);
            DTAction a4 = new DTAction(FollowClosestPlayer);
            DTAction a5 = new DTAction(FollowPlayerWithLessLives);
            DTAction a6 = new DTAction(FollowPlayerWithLessLivesInAreaC);

            d1.AddLink(true, d2);
            d1.AddLink(false, d4);

            d2.AddLink(true, d3);
            d2.AddLink(false, a1);

            d3.AddLink(true, a1);
            d3.AddLink(false, d4);

            d4.AddLink(true, d5);
            d4.AddLink(false, d6);

            d5.AddLink(true, a2);
            d5.AddLink(false, a3);

            d6.AddLink(true, d7);
            d6.AddLink(false, d8);

            d7.AddLink(true, d9);
            d7.AddLink(false, a4);

            d8.AddLink(true, a4);
            d8.AddLink(false, a5);

            d9.AddLink(true, a4);
            d9.AddLink(false, a6);

            dt = new DecisionTree(d1);

            StartCoroutine(Patrol());
        }
	}
	
	public IEnumerator Patrol()
    {
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run") || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                dt.walk();
                yield return new WaitForSeconds(.5f);
            }
            yield return new WaitForSeconds(.8f);
        }
    }

    //decisions
    public object IsABoardActiveFor2Seconds(object o)
    {
        return (bb1.getActiveTime() >= 2 || bb2.getActiveTime() >= 2);
    }

    public object IsABoardActiveFor5Seconds(object o)
    {
        return (bb1.getActiveTime() >= 5 || bb2.getActiveTime() >= 5);
    }

    public object MoleIsAlive(object o)
    {
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (playerTags.Contains(go.tag))
            {
                BasicController bc = go.GetComponent<BasicController>();
                //Debug.Log("ho trovaot il basic controller---->");
                if (bc.checkIsMole()) return true;
                
            }
        }
        return false;
    }

    public object AtLeastOneInAreaA(object o)
    {
        return (NumOfPlayersInArea(distanceA) >= 1);
    }

    public object AtLeastTwoInAreaB(object o)
    {
        return (NumOfPlayersInArea(distanceB) >= 2);
    }

    public object AtLeastOneInAreaC(object o)
    {
        return (NumOfPlayersInArea(distanceC) >= 1);
    }

    public object AtLeastTwoInAreaC(object o)
    {
        return (NumOfPlayersInArea(distanceC) >= 2);
    }

    //actions
    public object TpAtActiveBoard(object o)
    {
        if (bb1.getActive()) transform.position = board1.position;
        else transform.position = board2.position;
        return null;
    }

    public object LoadRoundAttack(object o)
    {
        StopFollowing();
        anim.SetTrigger("RoundAttack");
        RpcPlayAnim("RoundAttack");
        Invoke("RoundAttack", .5f);
        return null;
    }

    public object LoadFrontAttack(object o)
    {
        StopFollowing();
        if (Random.Range(0, 5) <= 1)
        {
            anim.SetTrigger("RoundAttack");
            RpcPlayAnim("RoundAttack");
            Invoke("RoundAttack", .5f);
        }
        else
        {
            anim.SetTrigger("FrontAttack");
            RpcPlayAnim("FrontAttack");
            Invoke("FrontAttack", .5f);
        }
        return null;
    }

    [ClientRpc]
    public void RpcPlayAnim(string s)
    {
        anim.SetTrigger(s);
    }



    public object FollowClosestPlayer(object o)
    {
        Transform player = null;
        float distance = float.MaxValue;
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (playerTags.Contains(go.tag))
            {
                if ((transform.position - go.transform.position).magnitude <= distance)
                {
                    player = go.transform;
                    distance = (transform.position - go.transform.position).magnitude;
                }
            }
        }
        anim.SetBool("Running", true);
        pf.setFollowing(true);
        pf.ChangeTarget(player);
        return null;
    }

    public object FollowPlayerWithLessLives(object o)
    {
        FollowPlayerWithLessLivesInRange(float.MaxValue);
        return null;
    }

    public object FollowPlayerWithLessLivesInAreaC(object o)
    {
        FollowPlayerWithLessLivesInRange(distanceC);
        return null;
    }

    //other methods
    private int NumOfPlayersInArea(float distance)
    {
        int count = 0;
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (playerTags.Contains(go.tag))
                if ((transform.position - go.transform.position).magnitude <= distance)
                    count++;
        }
        return count;
    }

    public object FollowPlayer(object o)
    {
        anim.SetBool("Running", true);
        GameObject go = GameObject.FindWithTag("Player");
        pf.setFollowing(true);
        pf.ChangeTarget(go.transform);
        return null;
    }

    private void RoundAttack()
    {
        transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
        transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Play();
        transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().Play();
        if(isServer)
            RpcTriggerAnimation(0);
        //fa perdere una vita ai giocatori in range B e li fa respawnare
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (playerTags.Contains(go.tag))
            {
                if ((transform.position - go.transform.position).magnitude <= distanceB)
                {
                    
                    RpcAttack(go);
                    // Cmdtest(go);
                }
            }
        }
    }

    
    [ClientRpc]
    public void RpcAttack(GameObject go)
    {
        /*if (isLocalPlayer)
        {*/
        BasicController playerController = go.GetComponent<BasicController>();
        playerController.DecreaseLives();
        playerController.Respawn(Vector3.zero);
        /* }*/
    }

    private void FrontAttack()
    {
        transform.GetChild(1).transform.GetComponent<ParticleSystem>().Play();
        if(isServer)
            RpcTriggerAnimation(1);
        //fa perdere una vita al giocatore in range B davanti a sè e lo fa respawnare
        RaycastHit hit;
        if(Physics.SphereCast(transform.position + new Vector3(0,1,0), .5f, transform.forward, out hit, distanceB))
        {
            if (playerTags.Contains(hit.transform.tag))
            {
                GameObject playerobj = hit.transform.gameObject;
                RpcAttack(playerobj);
                
            }
        }
    }

    

    [ClientRpc]
    public void RpcTriggerAnimation(int x)
    {
        if (x == 0)
        {
            transform.GetChild(0).GetChild(0).transform.GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).GetChild(1).transform.GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).GetChild(2).transform.GetComponent<ParticleSystem>().Play();
        }
        else
            transform.GetChild(1).transform.GetComponent<ParticleSystem>().Play();
    }
    private void FollowPlayerWithLessLivesInRange(float range)
    {
        Transform player = null;
        int lives = int.MaxValue;
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if (playerTags.Contains(go.tag))
            {
                if (go.GetComponent<BasicController>().GetLives() < lives && (transform.position - go.transform.position).magnitude <= range)
                {
                    player = go.transform;
                    lives = go.GetComponent<BasicController>().GetLives();
                }
            }
        }
        anim.SetBool("Running", true);
        pf.setFollowing(true);
        pf.ChangeTarget(player);
    }

    private void StopFollowing()
    {
        anim.SetBool("Running", false);
        pf.setFollowing(false);
    }

}
