using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SirLoinController : BasicController {

    private float FallingSpeed = -0.3f;
    private bool CanGlide = false;
    private bool AlternateThrow = false;
    private float timer = 1.5f;
    private float AnimationStop = 0.75f;
    public GameObject FirstKnife;
    public GameObject SecondKnife;
    private int number_of_knifes=0;
    private GameObject knife1, knife2;

    protected override void SpecialJump()
    {
        base.SpecialJump();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetDirection()), RotationSpeed);
        transform.position += (GetDirection() * MovementSpeed / JumpSlow * Time.deltaTime);
        rb.velocity = new Vector3(0, FallingSpeed, 0);
    }

    protected override void StatusUpdate(float CurrentInput)
    {
        base.StatusUpdate(CurrentInput);

        if (Input.GetButtonUp("Jump")) CanGlide = true;

        if (timer <= AnimationStop)
        {
            timer += Time.deltaTime;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            CmdUseAbility();
            return;
        }

        if (!IsGrounded)
        {
            if (CanGlide && Input.GetButton("Jump"))
            {
                anim.SetBool("Gliding", true);
                SpecialJump();
            }
            else
            {
                anim.SetBool("Gliding", false);
                Jump();
            }

        }
        else
        {
            anim.SetBool("Gliding", false);

            if (CurrentInput > 0) Run();
                else Idle();

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                CanGlide = false;
                Jump();
                return;
            }
        }
    }

    
    protected override void CmdUseAbility()
    {
        base.CmdUseAbility();

        //si gira il eprsonaggio
        Ray AbilityRay = new Ray(transform.position + new Vector3(0, 0.68f, 0), AimRayCast() - (transform.position + new Vector3(0, 0.68f, 0)));
        
        //controllo se è il primo o il secondo coltello 
        if (AlternateThrow)
        {
            //elimino se ci sono già 2 coltelli
            if (number_of_knifes >= 2)
            {
                CmdUnSpawnKnife(1);

                Debug.Log("----->>>Knifes: " + number_of_knifes);
            }
            AlternateThrow = false;
            anim.SetTrigger("FirstThrow");

            //il server fa apparire il coltello passando posizione rotazione della telecamera (è quella che ha il mirino) e se è il coltello 1 o 2
            //CmdSpawnKnife((this.transform.position + this.transform.forward + new Vector3(-0.25f,0.5f,0.25f)), cam.transform.rotation, 1);
            CmdSpawnKnife(getStartingPoint(), Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, AimRayCast().y - transform.position.y, AimRayCast().z - transform.position.z)), 1);
            number_of_knifes++;
            Debug.Log("Knifes: " + number_of_knifes);
            transform.rotation = Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z));
        }
        else
        {
            if (number_of_knifes >= 2)
            {
                CmdUnSpawnKnife(2);
                Debug.Log("----->>>Knifes: " + number_of_knifes);
            }
            AlternateThrow = true;
            anim.SetTrigger("SecondThrow");
            //CmdSpawnKnife((this.transform.position + this.transform.forward + new Vector3(-0.25f,0.5f, 0.25f)), cam.transform.rotation, 2);
            CmdSpawnKnife(getStartingPoint(), Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, AimRayCast().y - transform.position.y, AimRayCast().z - transform.position.z)), 2);
            number_of_knifes++;
            Debug.Log("Knifes: " + number_of_knifes);
            transform.rotation = Quaternion.LookRotation(new Vector3(AimRayCast().x - transform.position.x, 0, AimRayCast().z - transform.position.z));
        }
    }

    private Vector3 getStartingPoint()
    {
        
        Vector3 arc = AimRayCast() - transform.position;
        return transform.position + arc.normalized + new Vector3(0.0f, 1.0f, 0.0f);
    }

    [Command]
    private void CmdSpawnKnife(Vector3 knifepos, Quaternion kniferot, int i)
    {
       
        if (i == 1)
        {
            //Debug.Log("spawn1");
            knife1 = GameObject.Instantiate<GameObject>(this.FirstKnife, knifepos, kniferot);
            knife1.GetComponent<KnifeBehaviour>().SetDestination(AimRayCast());
            NetworkServer.Spawn(knife1);
        }
        else
        {
            //Debug.Log("spawn2");
            knife2 = GameObject.Instantiate<GameObject>(this.SecondKnife, knifepos, kniferot);
            knife2.GetComponent<KnifeBehaviour>().SetDestination(AimRayCast());
            NetworkServer.Spawn(knife2);
        }
    }

    [Command]
    private void CmdUnSpawnKnife(int i)
    {
        if (i == 1)
        {
            //Debug.Log("Unspawn1");
            number_of_knifes--;
            var knifesinscene = GameObject.FindGameObjectsWithTag("Knife");
            foreach(GameObject obj in knifesinscene)
            {
                
                if(obj.name== "LeftFish(Clone)")
                {
                    Debug.Log("sono nel for each 1 e ho trovaot first");
                    Destroy(obj);
                    NetworkServer.UnSpawn(obj);
                }
            }
           // GameObject obj = GameObject.Find("SirLoin's FirstKnife.pref(Clone)");
            
        }
        else
        {
            //Debug.Log("Unspawn2");
            number_of_knifes--;
            var knifesinscene = GameObject.FindGameObjectsWithTag("Knife");
            foreach (GameObject obj in knifesinscene)
            {
                if (obj.name == "RightFish(Clone)")
                {
                    Debug.Log("sono nel for each 1 e ho trovaot second");
                    Destroy(obj);
                    NetworkServer.UnSpawn(obj);
                }
            }
            /* GameObject obj = GameObject.Find("SirLoin's SecondKnife.pref(Clone)");
             Destroy(obj);
             NetworkServer.UnSpawn(obj);*/
        }
    }

    //DECOMMENTARE PER IMPLEMENTARE IL DESPAWN DEI COLTELLI AL TOCCO, LASCIARE PER GIOCARE CON I COLTELLI
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Knife")
        {
            col.gameObject.SetActive(false);
            number_of_knifes--;
            Debug.Log("Knifes: " + number_of_knifes);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * MaxDistance, Color.yellow);
    }
}
