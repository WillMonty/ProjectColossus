using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsAbility : ColossusAbility {

	public GameObject leftFist;
	public GameObject rightFist;

	[Header("Balancing")]
	public float chargeUpTime;
	public float launchForce;
	public float returnLagTime;

	public override void Enable ()
	{
		abilityEnabled = true;

		if(AbilityManagerScript.instance.leftHandColossus == ColossusHandAbilities.Fist)
		{
			leftFist.SetActive(true);	
		}

		if(AbilityManagerScript.instance.rightHandColossus == ColossusHandAbilities.Fist)
		{
			rightFist.SetActive(true);	
		}
	}

	public override void Disable ()
	{
		abilityEnabled = false;

		leftFist.SetActive(false);
		rightFist.SetActive(false);
	}

	void Update () 
	{
		if(leftFist.activeSelf)
		{
			CheckActivating(leftFist, leftControllerTracked);
		}

		if(rightFist.activeSelf)
		{
			CheckActivating(rightFist, rightControllerTracked);
		}
	}

	void CheckActivating(GameObject targetFist, SteamVR_TrackedController targetController)
	{
		if(targetController.triggerPressed)
		{
			targetFist.GetComponent<Fist>().playerActive = true;	
		}
		else
		{
			targetFist.GetComponent<Fist>().playerActive = false;	
		}
	}
}
