using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ColossusAbility {

	public GameObject leftShield;
	public GameObject rightShield;

	private GameObject activeShield;


	void Start () 
	{
		SetupTrackedControllers();
	}

	public override void Enable()
	{
		enabled = true;

		if(AbilityManagerScript.instance.leftHandColossus == ColossusHandAbilities.Hand)
		{
			leftShield.SetActive(true);	
		}

		if(AbilityManagerScript.instance.rightHandColossus == ColossusHandAbilities.Hand)
		{
			rightShield.SetActive(true);	
		}
	}

	public override void Disable()
	{
		enabled = false;

		leftShield.SetActive(false);
		rightShield.SetActive(false);
	}
	

	void Update () {
		
	}
}
