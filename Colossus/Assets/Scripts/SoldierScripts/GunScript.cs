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
    // Variables
    private Weapons weaponType;

    // Bullet prefab that will be shotout
    public GameObject bulletPrefab;

    // Specific balencing shooting variables
    float fireRate;
    float rightTriggerFloat;

    // Variables to keep shooting from breaking
    bool justShot;


	// Use this for initialization
	void Start ()
    {
        // Set bool variables initially
        justShot = false;


		if(weaponType == Weapons.AssaultRifle)
        {
            fireRate = .5f;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (GameManagerScript.instance.currentGameState == GameState.InGame)
        //{ 
            // Update the infor for game Input
            GunInput();


            if (Input.GetMouseButton(0)||rightTriggerFloat !=0)
            {
                Shoot();
            }
        //}
	}

    /// <summary>
    /// Handles input variables for weapons
    /// </summary>
    private void GunInput()
    {
        // Set the float variable of the right trigger
        rightTriggerFloat = Input.GetAxis("CONTROLLER_RIGHT_TRIGGER");
    }


    private void Shoot()
    {
        // Instantiate the bullet and shoot it
        Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation);

        justShot = true;
        StartCoroutine("ShootingBoolReset", fireRate);
    }


    /// <summary>
    /// Used to reset the shooting variable.
    /// Used with Couritines to prevent bullets to continuously shoot.
    /// </summary>
    public void ShootingBoolReset()
    {
      
    }

}
