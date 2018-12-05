using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUIReady : VRUIToggle {

	// Use this for initialization
	void Start () {
		
	}

	public void ToggleClicked()
	{
		//Yo dipshit make VRToggle abstract then make VRToggleAbility and VRToggleReady stem from that.
		Debug.Log("yo");
	}
}
