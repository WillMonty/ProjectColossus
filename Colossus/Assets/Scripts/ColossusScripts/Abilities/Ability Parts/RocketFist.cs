using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFist : MonoBehaviour, IDamage {

	public FistsAbility abilityControl;
	private int ownerNumber;

	public float Damage
	{
		get { return GetComponent<Rigidbody>().velocity.magnitude * abilityControl.damageFactor; }
	}

	public int Owner
	{
		get { return ownerNumber; }
		set { ownerNumber = value; }
	}

	void OnCollisionEnter(Collision collision)
	{
		GetComponent<Rigidbody>().useGravity = true;
	}
}
