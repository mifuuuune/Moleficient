using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class SirBeanController : BasicController
{
    public float Force = 200f;
    private BoxCollider BoxColl;
    private bool pushingBoulder = false;
    private float stepTime = 0.5f;
    private float lastStep = 0;


    protected override void Start()
    {
        base.Start();
        BoxColl = GetComponent<BoxCollider>();
    }

    protected override void StatusUpdate(float CurrentInput)
    {     
        base.StatusUpdate(CurrentInput);
        anim.SetBool("UsingAbility", false);
        BoxColl.enabled = false;

        if (!IsGrounded)
        {
            Jump();

            if ((!SpecialJumped) && (Input.GetButtonDown("Jump")))
            {
                AkSoundEngine.PostEvent("Peto", gameObject);
                SpecialJump();
            }
        }
        else
        {
            if (State == PlayerState.Jump)
                AkSoundEngine.PostEvent("Atterraggi", gameObject);
            MovementSpeed = 4.5f;
            JumpSlow = 1.25f;
            SpecialJumped = false;
            anim.SetBool("SpecialJumping", false);

            if (CurrentInput > 0)
            {
                Debug.Log("corro");
                Run();
            }
            else Idle();

            if (Input.GetMouseButton(0))
            {
                CmdUseAbility();
                return;
            }
            else
            {
                stepTime = 0.5f;
                pushingBoulder = false;
                AkSoundEngine.PostEvent("Sforzo_lungo_fine", gameObject);
            }

            if (Input.GetButtonDown("Jump"))
            {
                AkSoundEngine.PostEvent("Salti", gameObject);
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                Jump();
                return;
            }
        }
    }

    protected override void SpecialJump()
    {
        base.SpecialJump();
        JumpSlow = 4.5f;
        rb.AddForce(Vector3.up * (JumpForce - (rb.velocity.y * rb.mass)), ForceMode.Impulse);
        anim.SetBool("SpecialJumping", true);
    }

    
    protected override void CmdUseAbility()
    {
        base.CmdUseAbility();
        //Debug.Log(AimRayCast().tag);
        /*if (isClient)
            Force = 200;*/
       try
        {
            if (ProximityRayCast().tag == "Boulder" && ProximityRayCast() != null)
            {
                stepTime = 1.0f;
                if (!pushingBoulder)
                {
                    pushingBoulder = true;
                    AkSoundEngine.PostEvent("Sforzo_lungo", gameObject);
                }
                MovementSpeed = 2.5f;
                Rigidbody rb = ProximityRayCast().GetComponent<Rigidbody>();
                anim.SetBool("UsingAbility", true);
                Debug.Log("sono in boudler 1");
                BoxColl.enabled = true;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(ProximityRayCast().transform.position.x - transform.position.x, 0, ProximityRayCast().transform.position.z - transform.position.z)), RotationSpeed);
                //rb.AddForce(transform.forward * Force, ForceMode.Impulse);
                CmdPush(transform.forward, Force);

            }
            else if (ProximityRayCast().tag == "Rock" && ProximityRayCast() != null)
            {
                AkSoundEngine.PostEvent("Sforzo_breve", gameObject);
                anim.SetBool("UsingAbility", true);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(ProximityRayCast().transform.position.x - transform.position.x, 0, ProximityRayCast().transform.position.z - transform.position.z)), RotationSpeed);
                //rb.AddForce(transform.forward * Force, ForceMode.Impulse);
                

            }
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("No object in range");
            //Debug.Log(ex);
        }
       
    }

    protected override void Run()
    {
        base.Run();
        if (lastStep >= stepTime)
        {
            AkSoundEngine.PostEvent("Passi", gameObject);
            lastStep = 0;
        }
        else lastStep += Time.deltaTime;
    }

    [Command]
    public void CmdPush(Vector3 direction, float force)
    {
        RpcPush(direction, force);
        //rb.AddForce(transform.forward * Force, ForceMode.Impulse);
        //ProximityRayCast().transform.Translate(transform.forward * 1.5f * Time.deltaTime);
    }

    [ClientRpc]
    public void RpcPush(Vector3 direction, float force)
    {
        rb.AddForce(direction*force, ForceMode.Impulse);
    }
}