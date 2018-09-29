using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldsAbility : ColossusAbility {

	public GameObject leftShield;
	public GameObject rightShield;

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
		if(leftShield.activeSelf)
		{
			CheckActivating(leftShield, leftControllerTracked);
		}

		if(rightShield.activeSelf)
		{
			CheckActivating(rightShield, rightControllerTracked);
		}


	}

	void CheckActivating(GameObject targetShield, SteamVR_TrackedController targetController)
	{
		if(targetController.triggerPressed)
		{
			targetShield.GetComponent<Shield>().playerActive = true;	
		}
		else
		{
			targetShield.GetComponent<Shield>().playerActive = false;	
		}
	}

}


