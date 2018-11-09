﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShield : MonoBehaviour {

    const float COOL_DOWN = 5f;
    public float cooldown = COOL_DOWN;

    public GameObject wallSpawner;
    bool ready = true;
    bool isDeployed;
    GameObject deployedWall;
    GameObject eyes;
    GameObject gun;

    public AbilityState state;


    // Use this for initialization
    void Start ()
    {
        state = AbilityState.None;

        eyes = GetComponent<PlayerData>().eyes;
        gun = GetComponent<PlayerData>().gun;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {


        if (GetComponent<PlayerInput>().ActionState == 1 && ready)
        {
            Launch();
        }
    }

    void Launch()
    {
        if(deployedWall!=null)
        {
            Destroy(deployedWall);
        }

        deployedWall = Instantiate(wallSpawner,  eyes.transform.position + gun.transform.forward/4f, eyes.transform.rotation);

        ready = false;
        StartCoroutine(StartCD());
    }

    IEnumerator StartCD()
    {
        state = AbilityState.Cooldown;

        yield return new WaitForSeconds(COOL_DOWN);
        ready = true;

        state = AbilityState.None;
    }
}
