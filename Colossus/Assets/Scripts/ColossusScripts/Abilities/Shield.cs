using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	public bool playerActive; //Is the player trying to activate the reflect?
	private bool prevPlayerActive;
	private bool reflecting; //Reflecting currently on

	public float reflectTime;
	private float currReflect;


	public GameObject hitPrefab;
	public GameObject hitReflectPrefab;

	void Awake()
	{
		prevPlayerActive = playerActive;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(prevPlayerActive != playerActive)
		{
			if(playerActive) TurnOn(); 
			else TurnOff();

			prevPlayerActive = playerActive;
		}	
	}

	void TurnOn()
	{
		reflecting = true;
	}

	void TurnOff()
	{
		reflecting = false;
	}

	void OnCollisionEnter(Collision collision)
	{
		//Determine if it's some sort of bullet
		if(collision.collider.tag == "projectile")
		{
			if(reflecting)
			{
				Reflect(collision);
			}

			//Make audio object
			Instantiate(hitPrefab, collision.contacts[0].point, Quaternion.identity);
		}
	}

	//Re-instantiates a projectile hitting the shield to fire it back at the appropriate angle
	void Reflect(Collision collision)
	{
		GameObject projectile = collision.gameObject;

		GameObject createdProjectile = Instantiate(projectile, projectile.transform.position, projectile.transform.localRotation);
		Vector3 inV = projectile.GetComponent<BulletScript>().oldVelocity; //Do I need this?
		//inV.Scale(projectile.GetComponent<Rigidbody>().velocity);
		Vector3 reflectionV = Vector3.Reflect(inV, collision.contacts[0].normal);

		Debug.Log("In: " + inV);
		Debug.Log("Out: " + reflectionV);

		createdProjectile.GetComponent<Rigidbody>().velocity = reflectionV;

		TrashCollector.AddRubbishToList(createdProjectile);

	}
}
