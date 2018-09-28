using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParent : MonoBehaviour {

	public ColossusManager colossus;
	public GameObject hitPrefab;
	public float damageMultiplier = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCollisionEnter(Collision collision)
	{
		//Determine if it's some sort of bullet
		if(collision.collider.tag == "projectile")
		{

			Debug.Log(gameObject.name + " group hit.");

			//Get damage from projectile

			//Apply damage to Colossus
			//colossus.Damage(0.0f * damageMultiplier);

			//Make audio object
			Instantiate(hitPrefab, collision.contacts[0].point, Quaternion.identity);
		}
	}
}
