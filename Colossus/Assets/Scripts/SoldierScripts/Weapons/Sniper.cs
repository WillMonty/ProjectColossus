using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : GunBase {

    protected override void Start()
    {
        weaponType = Weapons.Sniper;
        base.Start();
    }

    protected override void Shoot()
    {
        
    }
}
