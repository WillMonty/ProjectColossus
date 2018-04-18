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
    float fireDelay;
    int clipSize;
    float reloadTime;

    // Variables to keep shooting from breaking and balanced
    bool justShot;
    bool reloadingBool;


    // Variables that handle input
    string shootTriggerString;
    float rightTriggerFloat;
    string reloadButtonString;
    float reloadButton;

    public int playerNum;



	// Use this for initialization
	void Start ()
    {
        // Set bool variables initially
        justShot = false;
        reloadingBool = false;

        // Set the input variable for the trigger to shoot
        shootTriggerString = "J" + playerNum + "TriggerRight";
        reloadButtonString = "J" + playerNum + "X";


        // For testing
        clipSize = 30;
        bulletsInClip = 30;
        reloadTime = 3.0f;
        fireDelay = .05f;
        /*
        if (weaponType == Weapons.AssaultRifle)
        {
            fireDelay = .5f;
            clipSize = 30;
            reloadTime = 5.0f;
        }
        */
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (GameManagerScript.instance.currentGameState == GameState.InGame)
        //{ 
            // Update the infor for game Input
            GunInput();
            

            // Check for shooting
            if (rightTriggerFloat > 0 && justShot == false)
            {
            Debug.Log("Player " + playerNum + " has " + bulletsInClip + " in their clip.");
            
            // If not reloading you can shoot or if you have greater than zero bullets
                if((reloadingBool==false) && bulletsInClip > 0)
                {
                    Shoot();
                }
                // If not reloading you can shoot or if you have greater than zero bullets
                if ((reloadingBool == false) && bulletsInClip < 1)
                {
                    Reload();
                }
            }

            // Check for reloading
        if(reloadingBool == false && reloadButton>0)
        {
            Debug.Log("Manually Reloaded");
            Reload();
        }
	}

    /// <summary>
    /// Handles input variables for weapons
    /// </summary>
    private void GunInput()
    {
        // Set the float variable of the right trigger
        rightTriggerFloat = Input.GetAxis(shootTriggerString);
        //Debug.Log(shootTriggerString);

        // Set the float variable of the right trigger
        reloadButton = Input.GetAxis(reloadButtonString);
    }


    private void Shoot()
    {
        Debug.Log("Shooting bullets");

        // Instantiate the bullet and shoot it
        Instantiate(bulletPrefab, transform.position + transform.forward, gameObject.GetComponent<PlayerInputManager>().eyes.transform.rotation);

        bulletsInClip--;

        justShot = true;
        StartCoroutine("ShootingBoolReset");
    }


    private void Reload()
    {
        reloadingBool = true;
        Debug.Log("Reload Called: " + reloadTime);
        // Check the player manager and see how much ammo they have before reloading
        //if(PlayerManager.ammo>clipSize)
        //{
        //PlayerManager.ammo -= clipSize;
        //}
        /*
        else
        {
            bulletsInClip += PlayerManager.ammo;
            PlayerManager.ammo = 0;
        }
        */
        bulletsInClip += clipSize;
        StartCoroutine("ReloadingBoolReset");
    }

    /// <summary>
    /// Used to reset the shooting variable.
    /// Used with Couritines to prevent bullets to continuously shoot.
    /// </summary>
    public IEnumerator ShootingBoolReset()
    {
        yield return new WaitForSeconds(fireDelay);

        justShot = false;
    }

    /// <summary>
    /// Used to reset the reloading variable variable.
    /// Used with Couritines to prevent players from shooting during reloading
    /// </summary>
    public IEnumerator ReloadingBoolReset()
    {
        yield return new WaitForSeconds(reloadTime);
        reloadingBool = false;
    }

}
