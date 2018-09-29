using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : GunBase
{ 
    

	// Use this for initialization
	protected override void Start ()
    {
        weaponType = Weapons.AssaultRifle;
        base.Start();
	}

}
