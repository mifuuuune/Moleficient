using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SirSageController : BasicController {

    private bool inTrampolineState = false;
    private bool RedHooked = false;
    private bool GreenHooked = false;
    private GameObject HookPosition;
    private float flyingSpeed = 7;
    private float StopAt = 0.5f;
    private float timer = 2f;
    private float AnimationStop = 0.75f;
    private BoxCollider BoxColl;
    public GameObject tramp;
    GameObject obj;
    protected override void Start()
    {
        base.Start();
        BoxColl = GetComponent<BoxCollider>();
    }
    

    protected override void StatusUpdate(float CurrentInput)
    {
        base.StatusUpdate(CurrentInput);

        if ((Input.GetButtonUp("Jump")))
        {
            inTrampolineState = false;
            coll.enabled = true;
            BoxColl.enabled = false;
            anim.SetBool("Trampoline", false);
            Cmddestroy();

        }

        if (Input.GetMouseButtonDown(0))
        {
            CmdUseAbility();
        }

        if (!inTrampolineState)
        {
            if (RedHooked && HookPosition.activeSelf == true && Vector3.Distance(HookPosition.transform.position, transform.position) > 0f)
            {
                transform.position = Vector3.MoveTowards(transform.position, HookPosition.transform.position, flyingSpeed * Time.deltaTime);
                rb.useGravity = false;
            }
            else
            {
                RedHooked = false;
                rb.useGravity = true;

                if (!IsGrounded)
                {
                    Jump();

                    if ((Input.GetButtonDown("Jump")))
                    {
                        SpecialJump();
                    }
                }
                else
                {

                    if (timer <= AnimationStop)
                    {
                        timer += Time.deltaTime;
                        anim.SetBool("Moving", false);
                        return;
                    }

                    if (CurrentInput > 0) Run();
                    else Idle();

                    if (Input.GetButtonDown("Jump"))
                    {
                        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                        Jump();
                        return;
                    }
                }
            }
        }
    }

    [Command]
    public void Cmddestroy()
    {
        var knifesinscene = GameObject.FindGameObjectsWithTag("Trampolino");
        foreach (GameObject trampol in knifesinscene)
        {
            Destroy(trampol);
            NetworkServer.UnSpawn(trampol);
        }
           
    }

    protected override void SpecialJump()
    {
        base.SpecialJump();
        inTrampolineState = true;
        CmdSetTramp();
        anim.SetBool("Trampoline", true);

    }

    [Command]
    public void CmdSetTramp()
    {
        RpcSetTramp();
        Invoke("SetTramp", 0.8f);
    }

    public void SetTramp()
    {
        obj = GameObject.Instantiate<GameObject>(tramp, new Vector3(this.transform.position.x+0.07f, this.transform.position.y+0.12f, transform.position.z-0.50f), Quaternion.identity);
        Physics.IgnoreCollision(obj.GetComponent<BoxCollider>(), BoxColl);
        NetworkServer.Spawn(obj);
    }

    [ClientRpc]
    public void RpcSetTramp()
    {
        coll.enabled = false;
        BoxColl.enabled = true;
    }

    [Command]
    protected override void CmdUseAbility()
    {
        base.CmdUseAbility();

        RaycastHit AbilityHit;
        Ray AbilityRay = new Ray(transform.position + new Vector3(0, 0.68f, 0), AimRayCast() - (transform.position + new Vector3(0, 0.68f, 0)));
        Debug.DrawRay(transform.position + new Vector3(0, 0.68f, 0), AimRayCast() - (transform.position + new Vector3(0, 0.68f, 0)), Color.green);
        if (Physics.Raycast(AbilityRay, out AbilityHit, AbilityRange))
        {

            if (AbilityHit.collider.gameObject.tag == "RedHook")
            {
                timer = 0;
                HookPosition = AbilityHit.collider.gameObject.transform.GetChild(0).gameObject;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z)), RotationSpeed * 10);
                anim.SetTrigger("Ability");
                StartCoroutine(GoTo());

            }
            else
            if (AbilityHit.collider.gameObject.tag == "GreenHook")
            {
                timer = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z)), RotationSpeed * 10);
                anim.SetTrigger("Ability");
            }

        }
    }

    IEnumerator GoTo()
    {
        yield return new WaitForSeconds(AnimationStop);
        RedHooked = true;

    }

    void OnCollisionEnter(Collision col)
    {
        RedHooked = false;
    }



    /*[ClientRpc]
      public void RpcTrampoline(Vector3 Inforce, GameObject obj)
      {

         Debug.Log("inforce--->"+ Inforce);
          //Vector3 x = new Vector3(0, obj.GetComponent<SirLoinController>().fall.y, 0);
          Debug.Log("trs--->" + transform.up);
          Debug.Log("result" + -transform.up * Inforce.y);

          obj.GetComponent<Rigidbody>().AddForce(-transform.up * Inforce.y, ForceMode.Impulse);


      }*/
}
