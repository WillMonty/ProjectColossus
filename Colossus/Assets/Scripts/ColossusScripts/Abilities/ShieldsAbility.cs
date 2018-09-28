using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldsAbility : ColossusAbility {

	public GameObject leftShield;
	public GameObject rightShield;

	private Shield leftShieldScript;
	private Shield rightShieldScript;

	void Start () 
	{
		SetupTrackedControllers();
	}

	public override void Enable()
	{
		abilityEnabled = true;

		if(AbilityManagerScript.instance.leftHandColossus == ColossusHandAbilities.Shield)
		{
			leftShield.SetActive(true);	
		}

		if(AbilityManagerScript.instance.rightHandColossus == ColossusHandAbilities.Shield)
		{
			rightShield.SetActive(true);	
		}
	}

	public override void Disable()
	{
		abilityEnabled = false;

		leftShield.SetActive(false);
		rightShield.SetActive(false);
	}
	

	void Update () 
	{
		
	}
}
