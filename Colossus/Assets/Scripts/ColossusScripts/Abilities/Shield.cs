using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	public bool activated;
	private bool prevActivated;

	public GameObject hitPrefab;

	void Awake()
	{
		prevActivated = activated;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(prevActivated != activated)
		{
			if(activated) TurnOn(); 
			else TurnOff();

			prevActivated = activated;
		}	
	}

	void TurnOn()
	{

	}

	void TurnOff()
	{

	}

	void OnCollisionEnter(Collision collision)
	{
		//Determine if it's some sort of bullet
		if(collision.collider.tag == "projectile")
		{
			if(activated)
			{
				Reflect(collision.gameObject);
			}
			else
			{
				//Delete the projectile...
			}

			//Make audio object
			Instantiate(hitPrefab, collision.contacts[0].point, Quaternion.identity);
		}
	}

	//Re-instantiates a projectile hitting the shield to fire it back at an angle
	void Reflect(GameObject projectile)
	{
		
	}
}
