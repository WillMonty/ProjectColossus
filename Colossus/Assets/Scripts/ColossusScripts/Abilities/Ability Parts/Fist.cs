using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fist : MonoBehaviour {

	public FistsAbility abilityControl;
	public bool isLeftFist;

	public GameObject fistRocketPrefab;
	GameObject spawnedFistRocket;
	Vector3 initReturnPos;
	Vector3 initReturnRot;
	Vector3 destinationRot;
	
	public bool playerActive; //Is the player trying to shoot the fist?
	bool prevPlayerActive;

	//Fist status
	public enum FistStates {Ready, Charging, Charged, Shooting, Returning};
	public FistStates fistState;
	float currCharge;
	float currReturnLag;
	float currReturn;

	//Fist UI
	[Header("UI")]
	public Slider slider;
	public Image sliderFill;

	AudioSource source;
	[Header("Audio")]
	public AudioClip launchSound;
	public AudioClip chargeSound;
	public AudioClip chargeLoop;

	void Awake()
	{
		prevPlayerActive = playerActive;
	}

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();

		if(abilityControl == null)
			return;
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
			//Pulled when ready to charge
			if(playerActive && fistState == FistStates.Ready)
			{
				sliderFill.color = Color.red;
				fistState = FistStates.Charging;
			}

			//Released while charging
			if(!playerActive && fistState == FistStates.Charging)
			{
				source.Stop();
				fistState = FistStates.Ready;
			}

			//Release when charged
			if(!playerActive && fistState == FistStates.Charged)
			{
				source.Stop();
				Shoot();
			}

			prevPlayerActive = playerActive;
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

		currCharge = 0;
		slider.value = 0;
		sliderFill.color = Color.red;

		//Play shoot sound
		source.clip = launchSound;
		source.loop = false;
		source.Play();
	}

	void UpdateCharge()
	{
		float chargePct = currCharge / abilityControl.chargeUpTime;
		if(currCharge >= abilityControl.chargeUpTime)
		{
			sliderFill.color = Color.green;

			source.clip = chargeLoop;
			source.loop = true;
			source.Play();

			fistState = FistStates.Charged;
		}
		else
		{
			currCharge += Time.deltaTime;

			if(!source.isPlaying)
			{
				source.clip = chargeSound;
				source.time = Mathf.Clamp(source.clip.length * chargePct, 0.0f, 1.0f);
				source.Play();
			}	
		}

		slider.value = chargePct;
	}

	void UpdateLagTime()
	{
		if(currReturnLag >= abilityControl.returnLagTime)
		{
			currReturnLag = 0;
			initReturnPos = spawnedFistRocket.transform.position;
			initReturnRot = spawnedFistRocket.transform.eulerAngles;

			destinationRot = transform.rotation.eulerAngles;
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
			spawnedFistRocket.transform.rotation = Quaternion.Euler(Vector3.Lerp(initReturnRot, destinationRot, fracReturn));
		}
	}

	//Toggle original fist on and off
	void ToggleFist(bool status)
	{
		foreach(BoxCollider c in GetComponents<BoxCollider>())
		{
			c.enabled = status;
		}

		GetComponent<MeshRenderer>().enabled = status;

		//Turn off fingers
		transform.GetChild(0).gameObject.SetActive(status);

		//Turn off slider
		slider.gameObject.SetActive(status);
	}
}
