using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : GunBase
{

    // Use this for initialization
    protected override void Start()
    {
        weaponType = Weapons.GrenadeLauncher;
        base.Start();
    }

    protected override void Reload()
    {
        base.Reload();
    }

}
