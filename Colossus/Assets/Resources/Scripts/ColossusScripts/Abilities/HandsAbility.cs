using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsAbility : ColossusAbility {

	public GameObject leftHand;
	public GameObject rightHand;

	public override void Enable ()
	{
		abilityEnabled = true;

		if(AbilityManagerScript.instance.leftHandColossus == ColossusHandAbilities.Hand)
		{
			leftHand.SetActive(true);	
		}

		if(AbilityManagerScript.instance.rightHandColossus == ColossusHandAbilities.Hand)
		{
			rightHand.SetActive(true);	
		}
	}

	public override void Disable ()
	{
		abilityEnabled = false;

		leftHand.SetActive(false);
		rightHand.SetActive(false);
	}
}
