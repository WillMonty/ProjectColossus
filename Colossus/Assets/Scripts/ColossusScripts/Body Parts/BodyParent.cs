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
		//Determine if it's some sort of damaging object
		if(collision.gameObject.GetComponent<IDamage>() != null)
		{
			//Apply damage to Colossus
			colossus.Damage(collision.gameObject.GetComponent<IDamage>().Damage * damageMultiplier);

			//Make audio object
			Instantiate(hitPrefab, collision.contacts[0].point, Quaternion.identity);
		}
	}
}
