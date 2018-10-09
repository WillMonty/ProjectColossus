using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsAbility : ColossusAbility {

	public GameObject leftFist;
	public GameObject rightFist;

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
}
