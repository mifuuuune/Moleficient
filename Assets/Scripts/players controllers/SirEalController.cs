using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SirEalController : BasicController {

    public GameObject Plant;
    public GameObject Terrain;
    private float timer = 1.5f;
    private float AnimationStop = 1.1f;
    private bool attachedToWall = false;
    private bool TouchingPlant = false;
    private BoxCollider BoxColl;

    private float stepTime = 0.3f;
    private float lastStep = 0;

    protected override void Start()
    {
        base.Start();
        BoxColl = GetComponent<BoxCollider>();
    }

    [Command]
    protected override void CmdUseAbility()
    {
        base.CmdUseAbility();

        RaycastHit AbilityHit;
        Ray AbilityRay = new Ray(transform.position + new Vector3(0, 0.5f, 0), AimRayCast() - (transform.position + new Vector3(0, 0.5f, 0)));
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), AimRayCast() - (transform.position + new Vector3(0, 0.5f, 0)), Color.green);
        try
        {
            if (Physics.Raycast(AbilityRay, out AbilityHit, AbilityRange))
            {
                if (AbilityHit.collider.gameObject.tag == "Terrain" && Vector3.Distance(AbilityHit.collider.gameObject.transform.position, transform.position) > 0.65f)
                {
                    AkSoundEngine.PostEvent("Grano_Crescita", gameObject);
                    timer = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AbilityHit.collider.gameObject.transform.position.x - transform.position.x, 0, AbilityHit.collider.gameObject.transform.position.z - transform.position.z)), RotationSpeed * 10);
                    anim.SetTrigger("Plant");
                    var spawn = AbilityHit.collider.gameObject.transform.position;
                    var destroy = AbilityHit.collider.gameObject;
                    CmdSpawnPlant(spawn, destroy);
                    //Destroy(ProximityRayCast());

                }
                else
                if (AbilityHit.collider.gameObject.tag == "Plant" && !TouchingPlant)
                {
                    AkSoundEngine.PostEvent("Grano_Taglio", gameObject);
                    timer = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(AbilityHit.collider.gameObject.transform.position.x - transform.position.x, 0, AbilityHit.collider.gameObject.transform.position.z - transform.position.z)), RotationSpeed * 10);
                    anim.SetTrigger("Plant");
                    var spawn = AbilityHit.collider.gameObject.transform.position;
                    var destroy = AbilityHit.collider.gameObject;
                    CmdUnSpawnPlant(spawn, destroy);
                    //Destroy(ProximityRayCast());
                }
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("No object in range.");
        }

    }
    [Command]
    protected void CmdSpawnPlant(Vector3 spawnP, GameObject destroyT)
    {
        GameObject newPlant = Instantiate<GameObject>(Plant, spawnP - new Vector3(0, 3, 0), Quaternion.identity);
        newPlant.transform.parent = destroyT.transform.parent;
        NetworkServer.Spawn(newPlant);
        NetworkServer.UnSpawn(destroyT);
        Destroy(destroyT);

    }

    [Command]
    protected void CmdUnSpawnPlant(Vector3 spawnT, GameObject destroyP)
    {
        GameObject newTerrain = Instantiate<GameObject>(Terrain, spawnT - new Vector3(0, 0, 0), Quaternion.identity);
        newTerrain.transform.parent = destroyP.transform.parent;
        NetworkServer.UnSpawn(destroyP);
        NetworkServer.Spawn(newTerrain);
        Destroy(destroyP);

    }

    protected override void SpecialJump()
    {
        base.SpecialJump();
    }

    protected override void StatusUpdate(float CurrentInput)
    {
        base.StatusUpdate(CurrentInput);

        if (!IsGrounded)
        {
            //BoxColl.enabled = true;
            //coll.enabled = false;

            if (attachedToWall)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    anim.SetBool("IsStick", false);
                    rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                    rb.useGravity = true;
                    attachedToWall = false;
                    Jump();
                }
            }
            else
            {
                Jump();
            }
        }
        else
        {
            if (State == PlayerState.Jump)
                AkSoundEngine.PostEvent("Atterraggi", gameObject);
            //BoxColl.enabled = false;
            //coll.enabled = true;

            if (timer <= AnimationStop)
            {
                timer += Time.deltaTime;
                anim.SetBool("Moving", false);
                return;
            }

            if (CurrentInput > 0) Run();
            else Idle();

            if (Input.GetMouseButtonDown(0))
            {
                CmdUseAbility();
                return;
            }

            if (Input.GetButtonDown("Jump"))
            {
                AkSoundEngine.PostEvent("Salti", gameObject);
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                Jump();
            }
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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject coll = collision.gameObject;

        if (coll.tag.Equals("Wall") && !IsGrounded)
        {
            AkSoundEngine.PostEvent("Ventosa", gameObject);
            anim.SetBool("IsStick", true);
            attachedToWall = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-collision.contacts[0].normal), RotationSpeed * 10);
        }

        if (coll.tag.Equals("Plant")) TouchingPlant = true;
    }

    void OnCollisionExit(Collision collision)
    {
        GameObject coll = collision.gameObject;

        if (coll.tag.Equals("Wall") && !IsGrounded)
        {
            anim.SetBool("IsStick", false);
            rb.useGravity = true;
            attachedToWall = false;
        }

        if (coll.tag.Equals("Plant")) TouchingPlant = false;
    }
}
