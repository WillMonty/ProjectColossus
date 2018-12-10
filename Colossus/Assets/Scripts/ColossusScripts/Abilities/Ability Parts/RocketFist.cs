using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFist : MonoBehaviour, IDamage {

	public FistsAbility abilityControl;
	private int ownerNumber;
	public bool damageOff;

	public float Damage
	{
		get { 
			if(!damageOff)
				return GetComponent<Rigidbody>().velocity.magnitude * abilityControl.damageFactor; 
			else
				return 0;
		}
	}

	public int Owner
	{
		get { return ownerNumber; }
		set { ownerNumber = value; }
	}

	void OnCollisionEnter(Collision collision)
	{
		GetComponent<AudioSource>().Stop();
	}
}
