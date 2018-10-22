using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add new abilities and classes here
public enum ColossusHandAbilities {Fist, Hand, Shield}; //Fist is default when no other abilities are chosen in the menu
public enum ColossusHeadAbilities {None, Laser};
public enum SoldierClass {Assault, Grenadier, Skulker};

public class AbilityManagerScript : MonoBehaviour {

	// Static instance of the GameManager which allows it to be accessed from any script
	public static AbilityManagerScript instance = null;

	[Header("Colossus Abilities")]
	public ColossusHandAbilities leftHandColossus;
	public ColossusHandAbilities rightHandColossus;
	public ColossusHeadAbilities headColossus;

	[Header("Soldier Classes")]
	public SoldierClass soldier1;
	public SoldierClass soldier2;


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

    public void SetSoldierClass(int soldierNum, SoldierClass newClass)
    {
        switch(soldierNum)
        {
            case 1:
                soldier1 = newClass;
                break;
            case 2:
                soldier2 = newClass;
                break;
        }
    }
}
