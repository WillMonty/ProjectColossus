using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapons
{
    AssaultRifle,
    Sniper,
}

public class GunScript : MonoBehaviour
{
    // Total ammo a gun can have in general
    const int MAX_AMMO = 500;

    // Variables
    private Weapons weaponType;

    // Bullet prefab that will be shotout
    public GameObject bulletPrefab;

    // current gun Tracking variables
    int bulletsInClip;

    // Specific balencing shooting variables
    float fireRate;
    int clipSize;
    float reloadTime;

    // Variables to keep shooting from breaking and balanced
    bool justShot;
    bool reloadingBool;


    // Variables that handle input
    string shootTriggerString;
    float rightTriggerFloat;
    float reloadButton;

    public int playerNum;



	// Use this for initialization
	void Start ()
    {
        // Set bool variables initially
        justShot = false;

        // Set the input variable for the trigger to shoot
        shootTriggerString = "J" + playerNum + "TriggerRight";

        if (weaponType == Weapons.AssaultRifle)
        {
            fireRate = .5f;
            clipSize = 30;
            reloadTime = 5.0f;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (GameManagerScript.instance.currentGameState == GameState.InGame)
        //{ 
            // Update the infor for game Input
            GunInput();

            if (Input.GetMouseButton(0) || rightTriggerFloat !=0)
            {
            // Check if the player is reloading or if there are less than 1 bullet in the clip
                if (reloadingBool || bulletsInClip < 1)
                {
                    Shoot();
                }
            }
	}

    /// <summary>
    /// Handles input variables for weapons
    /// </summary>
    private void GunInput()
    {
        // Set the float variable of the right trigger
        rightTriggerFloat = Input.GetAxis(shootTriggerString);
        Debug.Log(shootTriggerString);

        // Set the float variable of the right trigger
        //reloadButton = Input.GetAxis("X_BUTTON");
    }


    private void Shoot()
    {
        // Instantiate the bullet and shoot it
        Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);

        bulletsInClip--;

        justShot = true;
        StartCoroutine("ShootingBoolReset", fireRate);
    }


    private void Reload()
    {
        // Check the player manager and see how much ammo they have before reloading
        //if(PlayerManager.ammo>clipSize)
        //{
            bulletsInClip += clipSize;
        //PlayerManager.ammo -= clipSize;
        //}
        /*
        else
        {
            bulletsInClip += PlayerManager.ammo;
            PlayerManager.ammo = 0;
        }
        */
        reloadingBool = true;
        StartCoroutine("ReloadingBoolReset", reloadTime);
    }

    /// <summary>
    /// Used to reset the shooting variable.
    /// Used with Couritines to prevent bullets to continuously shoot.
    /// </summary>
    public void ShootingBoolReset()
    {
        justShot = false;
    }

    /// <summary>
    /// Used to reset the reloading variable variable.
    /// Used with Couritines to prevent players from shooting during reloading
    /// </summary>
    public void ReloadingBoolReset()
    {
        reloadingBool = false;
    }

}
