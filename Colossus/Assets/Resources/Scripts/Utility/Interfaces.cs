using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interfect for weapons
public interface IWeapon
{
    Weapons WeaponType { get; }
    int PlayerNum { set; }
    int BulletsInMag { get; }
    int MagSize { get; }
    float RightTrigger { set; }
    float RightTriggerPrev { set; }
    bool ReloadButton { set; }
    float ReloadTime { get; }
    bool Reloading { get; }
    int CurrentReloadNum { get; }
    bool Shooting { get; }
    void ResetStats();

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