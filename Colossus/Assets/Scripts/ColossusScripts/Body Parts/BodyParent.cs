using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyParent : MonoBehaviour, IHealth {

	public ColossusManager colossus;
	public float health;
	public GameObject hitPrefab;
	public float damageMultiplier = 1f;

	public float Health
	{
		get
		{
			return health;
		}
	}

	public void DamageObject(float dmg)
	{
		health -= dmg;
		//if(health <= 0) body part falls off if applicable
		Instantiate(hitPrefab, gameObject.transform.position, Quaternion.identity);

		//Apply damage to Colossus
		colossus.DamageObject(dmg * damageMultiplier);
	}

	public void OnCollisionEnter(Collision collision)
	{
		//Determine if it's some sort of damaging object
		if(collision.gameObject.GetComponent<IDamage>() != null)
		{
			//Apply damage to Colossus
			colossus.DamageObject(collision.gameObject.GetComponent<IDamage>().Damage * damageMultiplier);

			//Make audio object
			Instantiate(hitPrefab, collision.contacts[0].point, Quaternion.identity);
		}
	}
}
