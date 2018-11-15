using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour {

	public FistsAbility abilityControl;

	public GameObject fistRocketPrefab;
	GameObject spawnedFistRocket;
	
	public bool playerActive; //Is the player trying to shoot the fist?
	bool prevPlayerActive;

	//Fist status
	public enum FistStates {Ready, Charging, Shooting, Returning};
	public FistStates fistState;
	float currReturnLag;
	public float currCharge;

	AudioSource source;

	void Awake()
	{
		prevPlayerActive = playerActive;
	}

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
		currReturnLag = abilityControl.returnLagTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(fistState == FistStates.Shooting)
			UpdateLagTime();

		if(fistState == FistStates.Charging)
			UpdateCharge();

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
		if(fistState != FistStates.Shooting)
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
		spawnedFistRocket.GetComponent<Rigidbody>().AddForce(-transform.right.normalized * abilityControl.launchForce);

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

		if(currCharge > abilityControl.chargeUpTime)
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
		if(currReturnLag < abilityControl.returnLagTime)
		{
			currReturnLag += Time.deltaTime;
		}
		else
		{
			ToggleFist(true);
			currReturnLag = 0;
			//Tell spawned fist to start LERPing or something
			fistState = FistStates.Ready; //TEMP
		}
	}
}
