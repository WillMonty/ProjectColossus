﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add new abilities and classes here
public enum ColossusHandAbilities {Hand, Shield};
public enum ColossusHeadAbilities {None, Laser};
public enum ResistanceClasses {Assault, Skulker, Grenadier};

public class AbilityManagerScript : MonoBehaviour {

	// Static instance of the GameManager which allows it to be accessed from any script
	public static AbilityManagerScript instance = null;

	[Header("Colossus Abilities")]
	public ColossusHandAbilities leftHandColossus;
	public ColossusHandAbilities rightHandColossus;
	public ColossusHeadAbilities headColossus;

	[Header("Resistance Classes")]
	public ResistanceClasses resistance1;
	public ResistanceClasses resistance2;


	void Awake ()
	{
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start () 
	{
		// Check for an instance, if it does exist, than set to this
		if (instance == null)
		{
			instance = this;
		}
		else if(instance!=this)
		{
			if (FindObjectsOfType(GetType()).Length > 1)
				Destroy(gameObject);
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
