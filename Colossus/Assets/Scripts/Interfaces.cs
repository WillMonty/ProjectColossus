﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interfect for weapons
public interface IWeapon
{
    Weapons WeaponType { get; }
    Vector3 SetAim { set; }
    int PlayerNum { set; }
    int BulletsInMag { get; }
    int MagSize { get; }
    float RightTrigger { set; }
    bool ReloadButton { set; }
    float ReloadTime { get; }
}

//Generic Interface for player projectiles
public interface IDamage
{
    int Owner { get; set; }
    float Damage { get; }
}

//Interface for objects with health that take damage
public interface IHealth
{
    float Health { get; }
    void DamageObject(float dmg);
}