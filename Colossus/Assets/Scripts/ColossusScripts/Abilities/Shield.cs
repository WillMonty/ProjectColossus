using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour {

	public bool playerActive; //Is the player trying to activate the reflect?
	bool prevPlayerActive;
	bool reflecting; //Reflecting currently on

	public float reflectTime;
	public float lagBeforeCharge;
	float currReflect;
	float currChargeLag;

	public Slider sliderReflect;

	[Header("Material Settings")]
	private Material mat;

	[Range(0.0f,1.0f)]
	public float offAlpha;
	[Range(0.0f,1.0f)]
	public float onAlpha;

	[Header("Audio Objects")]
	public GameObject hitPrefab;
	public GameObject hitReflectPrefab;
	public AudioClip reflectOnSound;
	public AudioClip noReflectSound;

	AudioSource source;

	void Awake()
	{
		prevPlayerActive = playerActive;
	}

	// Use this for initialization
	void Start () {
		currReflect = reflectTime;
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
			//Play noReflectSound if it isnt playing
			return;
		}

		currChargeLag = 0.0f;

		//Set material alpha
		mat.color = new Color(1.0f, 1.0f, 1.0f, onAlpha);

		reflecting = true;

		//Play reflectOnSound. Loop it?
	}

	void TurnOff()
	{
		mat.color = new Color(1.0f, 1.0f, 1.0f, offAlpha);
		reflecting = false;
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
		else if(!playerActive && currReflect < reflectTime)
		{
			//Make sure it doesn't immediately start charging
			if(currChargeLag >= lagBeforeCharge)
			{
				currReflect += Time.deltaTime;		
			}
			else
			{
				currChargeLag += Time.deltaTime;
			}
		}
			
		if(currReflect > reflectTime) currReflect = reflectTime;

		if(currReflect <= 0.0f)
		{
			currReflect = 0.0f;
			TurnOff();
		}

		//Update UI
		sliderReflect.value = 1 - (reflectTime - currReflect)/reflectTime;
	}

	/// <summary>
	/// Re-instantiates a projectile hitting the shield to fire it back at the appropriate angle
	/// </summary>
	/// <param name="collision">Collision.</param>
	void Reflect(Collision collision)
	{
		GameObject projectile = collision.gameObject;

		//Get reflection
		Vector3 inV = projectile.GetComponent<BulletScript>().oldVelocity; //Do I need this?
		Vector3 reflectionV = Vector3.Reflect(inV, collision.contacts[0].normal);

		Vector3 spawnPos = collision.contacts[0].point + (reflectionV * 0.04f);

		//Instantiate new projectile
		GameObject createdProjectile = Instantiate(projectile, spawnPos, projectile.transform.localRotation);

		float shieldVelocity = gameObject.GetComponent<Valve.VR.InteractionSystem.VelocityEstimator>().GetVelocityEstimate().magnitude;

		if(shieldVelocity >= 1)
			reflectionV *= shieldVelocity;

		createdProjectile.GetComponent<Rigidbody>().velocity = reflectionV;
		createdProjectile.GetComponent<IDamage>().Owner = 0;

		TrashCollector.AddRubbishToList(createdProjectile);

	}

	void OnCollisionEnter(Collision collision)
	{
		//Determine if it's some sort of bullet
		if(collision.collider.tag == "projectile")
		{
			if(reflecting)
			{
				Reflect(collision);
				Instantiate(hitReflectPrefab, collision.contacts[0].point, Quaternion.identity);
			}
			else
			{
				Instantiate(hitPrefab, collision.contacts[0].point, Quaternion.identity);
			}
		}
	}
}
