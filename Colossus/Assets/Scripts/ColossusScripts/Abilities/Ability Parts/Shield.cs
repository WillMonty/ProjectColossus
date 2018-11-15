using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour {

	public ShieldsAbility abilityControl;

	public bool playerActive; //Is the player trying to activate the reflect?
	bool prevPlayerActive;
	bool reflecting; //Reflecting currently on

	Material mat;

	public Slider sliderReflect;

	float currReflect;
	float currChargeLag;

	AudioSource source;

	void Awake()
	{
		prevPlayerActive = playerActive;
	}

	// Use this for initialization
	void Start () {
		currReflect = abilityControl.reflectTime;
		mat = gameObject.GetComponent<Renderer>().material;
		source = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{		
		UpdateReflectTime();

		if(prevPlayerActive != playerActive)
		{
			if(playerActive) TryTurnOn(); 
			else TurnOff();

			prevPlayerActive = playerActive;
		}
	}

	void TryTurnOn()
	{
		if(currReflect <= 0.0f)
		{
			//Play noReflectSound
			return;
		}

		currChargeLag = 0.0f;

		//Set material alpha
		mat.color = new Color(1.0f, 1.0f, 1.0f, abilityControl.onAlpha);

		reflecting = true;

		source.Stop();
		source.clip = abilityControl.reflectOnSound;
		source.Play();
	}

	void TurnOff()
	{
		mat.color = new Color(1.0f, 1.0f, 1.0f, abilityControl.offAlpha);
		reflecting = false;

		if(!source.isPlaying)
		{
			if(currReflect > 0.0f)
			{
				source.clip = abilityControl.reflectOffSound;
				source.Play();
			}
			else
			{
				//Play noReflectSound
			}
		}
	}

	/// <summary>
	/// Updates the reflect time.
	/// </summary>
	void UpdateReflectTime()
	{
		if(reflecting && currReflect > 0.0f)
		{
			currReflect -= Time.deltaTime;
		}
		else if(!playerActive && currReflect < abilityControl.reflectTime)
		{
			//Make sure it doesn't immediately start charging
			if(currChargeLag >= abilityControl.lagBeforeCharge)
			{
				currReflect += Time.deltaTime;		
			}
			else
			{
				currChargeLag += Time.deltaTime;
			}
		}
			
		if(currReflect > abilityControl.reflectTime) currReflect = abilityControl.reflectTime;

		if(currReflect <= 0.0f)
		{
			currReflect = 0.0f;
			TurnOff();
		}

		//Update UI
		sliderReflect.value = 1 - (abilityControl.reflectTime - currReflect)/abilityControl.reflectTime;
	}

	/// <summary>
	/// Re-instantiates a projectile hitting the shield to fire it back at the appropriate angle
	/// </summary>
	/// <param name="collision">Collision.</param>
	void Reflect(Collision collision)
	{
		GameObject projectile = collision.gameObject;

		//Get reflection
		Vector3 reflectionV = Vector3.Reflect(-collision.impulse, collision.contacts[0].normal);

		//Put the projectile a bit away from the shield
		Vector3 spawnPos = collision.contacts[0].point + (reflectionV * 0.04f);

		//Instantiate new projectile
		GameObject createdProjectile = Instantiate(projectile, spawnPos, projectile.transform.localRotation);

		//Factor in shield speed
		float shieldVelocity = gameObject.GetComponent<Valve.VR.InteractionSystem.VelocityEstimator>().GetVelocityEstimate().magnitude;
		if(shieldVelocity >= 1)
			reflectionV *= shieldVelocity;

		createdProjectile.GetComponent<Rigidbody>().velocity = reflectionV;
		createdProjectile.GetComponent<IDamage>().Owner = 0;

		EnvironmentManagerScript.AddRubbishToList(createdProjectile);
	}

	void OnCollisionEnter(Collision collision)
	{
		//Determine if it's some sort of bullet
		if(collision.collider.tag == "projectile")
		{
			if(reflecting)
			{
				Reflect(collision);
				Instantiate(abilityControl.hitReflectPrefab, collision.contacts[0].point, Quaternion.identity);
			}
			else
			{
				Instantiate(abilityControl.hitPrefab, collision.contacts[0].point, Quaternion.identity);
			}
		}
	}
}
