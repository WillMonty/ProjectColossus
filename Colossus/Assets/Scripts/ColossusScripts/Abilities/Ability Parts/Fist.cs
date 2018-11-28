﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour {

	public FistsAbility abilityControl;
	public bool isLeftFist;

	public GameObject fistRocketPrefab;
	GameObject spawnedFistRocket;
	Vector3 initReturnPos;
	Vector3 initReturnRot;
	
	public bool playerActive; //Is the player trying to shoot the fist?
	bool prevPlayerActive;

	//Fist status
	public enum FistStates {Ready, Charging, Shooting, Returning};
	public FistStates fistState;
	float currCharge;
	float currReturnLag;
	float currReturn;


	AudioSource source;

	void Awake()
	{
		prevPlayerActive = playerActive;
	}

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();

		if(abilityControl == null)
			return;
		
		currCharge = 0;
		currReturnLag = abilityControl.returnLagTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(fistState == FistStates.Charging)
			UpdateCharge();

		if(fistState == FistStates.Shooting)
			UpdateLagTime();

		if(fistState == FistStates.Returning)
			UpdateReturn();


		//Check trigger pull
		if(prevPlayerActive != playerActive)
		{
			if(playerActive) TryShoot();

			if(!playerActive && fistState == FistStates.Charging)
			{
				fistState = FistStates.Ready;
				currCharge = 0;
				//Play charge down sound
			}

			prevPlayerActive = playerActive;
		}
	}

	void TryShoot()
	{
		if(fistState == FistStates.Ready)
		{
			fistState = FistStates.Charging;
		}
		else
		{
			//Play not ready sound
		}
	}

	void Shoot()
	{
		ToggleFist(false);
		fistState = FistStates.Shooting;

		spawnedFistRocket = Instantiate(fistRocketPrefab, transform.position, transform.rotation);
		spawnedFistRocket.GetComponent<RocketFist>().abilityControl = abilityControl;
		spawnedFistRocket.GetComponent<RocketFist>().Owner = 0;

		if(isLeftFist)
			spawnedFistRocket.GetComponent<Rigidbody>().AddForce(-transform.right.normalized * abilityControl.launchForce);
		else
			spawnedFistRocket.GetComponent<Rigidbody>().AddForce(transform.right.normalized * abilityControl.launchForce);

		//Play shoot sound
	}

	//Toggle original fist on and off
	void ToggleFist(bool status)
	{
		foreach(BoxCollider c in GetComponents<BoxCollider>())
		{
			c.enabled = status;
		}

		GetComponent<MeshRenderer>().enabled = status;

		foreach(MeshRenderer childMesh in transform.GetComponentsInChildren<MeshRenderer>())
		{
			childMesh.enabled = status;
		}
	}

	void UpdateCharge()
	{
		if(currCharge >= abilityControl.chargeUpTime)
		{
			Shoot();
			currCharge = 0;
		}
		else
		{
			currCharge += Time.deltaTime;

			if(!source.isPlaying)
			{
				//Play charge up sound
			}	
		}
	}

	void UpdateLagTime()
	{
		if(currReturnLag >= abilityControl.returnLagTime)
		{
			currReturnLag = 0;
			initReturnPos = spawnedFistRocket.transform.position;
			initReturnRot = spawnedFistRocket.transform.eulerAngles;
			fistState = FistStates.Returning;
		}
		else
		{
			currReturnLag += Time.deltaTime;
		}
	}

	void UpdateReturn()
	{
		if(currReturn >= abilityControl.returnTime)
		{
			GameObject.Destroy(spawnedFistRocket);
			ToggleFist(true);
			currReturn = 0;
			fistState = FistStates.Ready;
		}
		else
		{
			currReturn += Time.deltaTime;
			float fracReturn = currReturn/abilityControl.returnTime;

			spawnedFistRocket.transform.position = Vector3.Lerp(initReturnPos, transform.position, fracReturn);
			Vector3 destinationRot;
			if(isLeftFist)
				destinationRot = new Vector3(0.0f, 90f, 0f);
			else
				destinationRot = new Vector3(0.0f, -90f, 0f);
			spawnedFistRocket.transform.rotation = Quaternion.Euler(Vector3.Lerp(initReturnRot, destinationRot, fracReturn));
		}
	}
}
