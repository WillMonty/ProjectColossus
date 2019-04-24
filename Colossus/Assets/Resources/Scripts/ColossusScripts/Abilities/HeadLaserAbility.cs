﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadLaserAbility : ColossusAbility
{

    public bool debugLaser;

    [Header("Laser Components")]
	public GameObject container;
	//With these arrays, 0 index is left and 1 index is right
    public GameObject[] origins = new GameObject[2]; //Starting object beams come from
    public GameObject[] beams = new GameObject[2]; //Beam cylinders
    public RawImage indicatorImage;
    public Texture ready;
    public Texture notReady;

    //Raycast vars
    RaycastHit[] hits = new RaycastHit[2];
    float lastDistance = 0.0f; //Last distance recorded by raycast
    float maxDistance = 300.0f;

    //Laser state vars
    [Header("Laser Behavior")]
    public float cooldownTime;
    public float laserTime;
	public float damageAmt;

    float currLaserTime; //Time the laser has currently been firing
    float chargeTime = 0.4f; //Time before the laser actually starts firing to let the charge sound play

    bool firing = false;
    bool isCharging = false;
    bool laserReady = true; //Checks if the laser has a shot ready

    //Audio vars
    [Header("Audio")]
    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip misfireSound;
    public AudioClip readySound;

	[Header("UI")]
	public GameObject laserUI;

	public override void Enable()
	{
		abilityEnabled = true;

		container.SetActive(true);
		laserUI.SetActive(true);
	}

	public override void Disable()
	{
		abilityEnabled = false;

		StopLaser();
		container.SetActive(false);
		laserUI.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		if (debugLaser)
		{
			Enable();
			origins[0].SetActive(true);
			origins[1].SetActive(true);
			FireLaser();
			return;
		}

		if(!abilityEnabled) return;

        //If both controllers pads are pressed
		if (leftControllerTracked.padPressed && rightControllerTracked.padPressed)
        {
            TryShoot();
        }

        //Update the raycasts if firing
        if (firing)
        {
            FireLaser();
        }
    }

    //Called when the player tries to fire the laser. Checks to make sure the laser ability is available
    void TryShoot()
    {
        //Misfire
        if(!laserReady && !firing)
        {
            source.Stop();
            source.clip = misfireSound;
            source.Play();
            return;
        }

        //Starting a new laser
        if (!isCharging && !firing)
        {
            isCharging = true;
            source.clip = fireSound;
            source.Play();
            StartCoroutine(ChargeUp());
        }
    }

    //Raycasts the laser out and updates the time it has been firing
    void FireLaser()
    {
        for (int i = 0; i < 2; i++)
        {
			Physics.Raycast(origins[i].transform.position, origins[i].transform.forward, out hits[i], maxDistance);
            //Move and scale the beam to the appropriate position
            Vector3 currBeamLocalPosition = beams[i].transform.localPosition;
            Vector3 currBeamScale = beams[i].transform.localScale;

            //If the raycast is actually hitting something and changing each frame
            if (lastDistance != hits[i].distance)
            {
                beams[i].transform.localPosition = new Vector3(currBeamLocalPosition.x, currBeamLocalPosition.y, hits[i].distance / 2);
                beams[i].transform.localScale = new Vector3(currBeamScale.x, hits[i].distance / 2, currBeamScale.z);
            }
            else
            {
                beams[i].transform.localPosition = new Vector3(currBeamLocalPosition.x, currBeamLocalPosition.y, maxDistance / 2);
                beams[i].transform.localScale = new Vector3(currBeamScale.x, maxDistance / 2, currBeamScale.z);
            }


            lastDistance = hits[i].distance;

			CheckHit(hits[i]);
        }

        currLaserTime += Time.deltaTime;

        //Stop laser if the time has gone over the ability time
        if(!debugLaser && currLaserTime >= laserTime)
        {
            StopLaser();
        }
    }

    //Helper method to turn off the laser
    public void StopLaser()
    {
        firing = false;
        StopCoroutine(ChargeUp());
        currLaserTime = 0;

        origins[0].SetActive(false);
        origins[1].SetActive(false);

        source.Stop();

        StartCoroutine(LaserCoolDown());
    }

	void CheckHit(RaycastHit hit)
	{
		//If the raycast has hit something
		if(hit.collider != null)
		{
			GameObject hitObj = hit.collider.gameObject;

			if(hitObj.GetComponent<IHealth>() != null)
			{
				hitObj.GetComponent<IHealth>().DamageObject(damageAmt);
			}
		}
	}

    //Coroutine to charge the laser
    IEnumerator ChargeUp()
    {
        yield return new WaitForSeconds(chargeTime);
        origins[0].SetActive(true);
        origins[1].SetActive(true);
        laserReady = false;
        firing = true;
        isCharging = false;
        indicatorImage.texture = notReady;
    }

    //Coroutine to cool down the laser
    IEnumerator LaserCoolDown()
    {
        yield return new WaitForSeconds(cooldownTime);
        laserReady = true;
        indicatorImage.texture = ready;
        source.Stop();
        source.clip = readySound;
        source.Play();
    }
}