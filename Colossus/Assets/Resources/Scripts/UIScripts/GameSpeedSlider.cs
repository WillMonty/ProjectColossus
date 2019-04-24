using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameSpeedSlider : MonoBehaviour {

	public Slider slide;
	public Text txt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.UpArrow))
		{
			ChangeSpeed(true);
		}

		if(Input.GetKeyUp(KeyCode.DownArrow))
		{
			ChangeSpeed(false);
		}
	}

	void ChangeSpeed(bool direction)
	{
		float temp = GameManagerScript.instance.gameSpeedMod;
		if(temp >= 2.0f && direction)
			return;

		if(temp <= 1.0f && !direction)
			return;
		
		if(direction)
			temp += 0.1f;
		else
			temp -= 0.1f;


		float mult = Mathf.Pow(10.0f, 2.0f);
		temp = Mathf.Round(temp * mult) / mult;

		GameManagerScript.instance.gameSpeedMod = temp;
		slide.value = GameManagerScript.instance.gameSpeedMod;
		txt.text = GameManagerScript.instance.gameSpeedMod + "x";
	}
}
