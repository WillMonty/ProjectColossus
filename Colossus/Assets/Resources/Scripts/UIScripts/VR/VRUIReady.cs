﻿using UnityEngine;
using UnityEngine.UI;

public class VRUIReady : VRUIToggle {

	public Text waitingText;

	public override void ToggleChanged(bool enabled)
	{
		Text readyText = transform.GetChild(0).GetComponent<Text>();
		if(enabled)
		{
			GameManagerScript.instance.readyColossus = true;
			readyText.color = Color.black;

			if(!GameManagerScript.instance.readySoldiers)
			{
				waitingText.enabled = true;
			}
		}
		else
		{
			GameManagerScript.instance.readyColossus = false;
			readyText.color = Color.white;
			image.color = offColor;
			waitingText.enabled = false;
		}
	}
}