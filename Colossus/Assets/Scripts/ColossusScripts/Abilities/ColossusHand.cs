using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossusHand : ColossusAbility {

	public GameObject leftHand;
	public GameObject rightHand;

	public override void Enable ()
	{
		enabled = true;

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
		enabled = false;

		leftHand.SetActive(false);
		rightHand.SetActive(false);
	}
}
