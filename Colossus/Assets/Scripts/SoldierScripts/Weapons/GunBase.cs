﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IWeapon
{
    Weapons WeaponType { get; }
    int PlayerNum { set; }
    int BulletsInMag { get; }
    int MagSize { get; }
    float RightTrigger { set; }
    bool ReloadButton { set; }
}

public enum Weapons
{
    AssaultRifle,
    Sniper,
    GrenadeLauncher,
}

public class GunBase : MonoBehaviour, IWeapon
{
    // Variables
    protected Weapons weaponType;
    public Weapons WeaponType
    {
        get { return weaponType; }
    }

    // Bullet prefab that will be shot
    public GameObject projectile;

    //Audio Source
    protected AudioSource source;
    public AudioClip shootSound;

    // current gun Tracking variables
    int bulletsInMag;
    public int BulletsInMag
    {
        get { return bulletsInMag; }
    }

    // Specific balencing shooting variables
    protected float fireDelay;
    protected int magSize;

    public int MagSize
    {
        get { return magSize; }
    }

    protected float reloadTime;

    // Variables to keep shooting from breaking and balanced
    protected bool justShot;
    protected bool reloadingBool;

    // Variables that handle input
    float rightTrigger;
    public float RightTrigger
    {
        set {  rightTrigger=value; }
    }

    bool reloadButton;
    public bool ReloadButton
    {
        set { reloadButton = value; }
    }

    int playerNum;
    public int PlayerNum
    {
        set { playerNum = value; }
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        // Set bool variables initially
        justShot = false;
        reloadingBool = false;
        
        if (weaponType == Weapons.AssaultRifle)
        {
            fireDelay = .05f;
            magSize = 30;
            reloadTime = 2.0f;
        }

        else if (weaponType == Weapons.GrenadeLauncher)
        {
            fireDelay = 1f;
            magSize = 6;
            reloadTime = 4.0f;
        }

        else if (weaponType == Weapons.Sniper)
        {
            fireDelay = 0f;
            magSize = 1;
            reloadTime = 3.0f;
        }

        bulletsInMag = magSize;

		source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        if (GameManagerScript.instance.currentGameState == GameState.InGame)
        {

            // Check for shooting
            if (rightTrigger > 0 && justShot == false && !reloadingBool)
            {

                if (bulletsInMag > 0)
                {
                    Shoot();
                }

                else
                {
                    Reload();
                }

            }

            // Check for reloading
            if (!reloadingBool && reloadButton)
            {
                Reload();
            }
        }
	}



    protected void Shoot()
    {
		if(!source.isPlaying)
		{
			source.clip = shootSound;
			source.Play();
		}

        // Instantiate the projectile and shoot it
        Instantiate(projectile, transform.position, gameObject.transform.parent.GetComponent<PlayerData>().eyes.transform.rotation);

        bulletsInMag--;

        justShot = true;
        StartCoroutine("ShootingBoolReset");
    }



    /// <summary>
    /// Used to reset the shooting variable.
    /// Used with Couritines to prevent bullets to continuously shoot.
    /// </summary>
    protected IEnumerator ShootingBoolReset()
    {
        yield return new WaitForSeconds(fireDelay);

        justShot = false;
    }


    
    protected virtual void Reload()
    {
        reloadingBool = true;

        bulletsInMag = 0;     
        StartCoroutine("ReloadingBoolReset");
    }

    /// <summary>
    /// Used to reset the reloading variable variable.
    /// Used with Couritines to prevent players from shooting during reloading
    /// </summary>
    protected IEnumerator ReloadingBoolReset()
    {
        yield return new WaitForSeconds(reloadTime);
        bulletsInMag = magSize;
        reloadingBool = false;
    }

}