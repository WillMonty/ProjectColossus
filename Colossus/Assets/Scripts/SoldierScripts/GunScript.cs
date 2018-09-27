using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapons
{
    AssaultRifle,
    Sniper,
    GrenadeLauncher,
}

public class GunScript : MonoBehaviour
{
    public float rightTrigger;
    public bool reloadButton;

    // Total ammo a gun can have in general
    const int MAX_AMMO = 500;

    // Variables
    private Weapons weaponType;

    // Bullet prefab that will be shotout
    public GameObject bulletPrefab;

	//Audio Source
	private AudioSource source;
	public AudioClip shootSound;

    // current gun Tracking variables
    int bulletsInMag;

    public int BulletsInMag
    {
        get { return bulletsInMag; }
    }

    // Specific balencing shooting variables
    float fireDelay;
    int magSize;

    public int MagSize
    {
        get { return magSize; }
    }

    float reloadTime;

    // Variables to keep shooting from breaking and balanced
    bool justShot;
    bool reloadingBool;


    // Variables that handle input

    public int playerNum;



	// Use this for initialization
	void Start ()
    {
        // Set bool variables initially
        justShot = false;
        reloadingBool = false;


        // For testing
        //clipSize = 30;
        //bulletsInClip = 30;
        //reloadTime = 3.0f;
        fireDelay = .05f;
        
        if (weaponType == Weapons.AssaultRifle)
        {
            fireDelay = .05f;
            magSize = 30;
            reloadTime = 2.0f;
        }

        bulletsInMag = magSize;

		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (GameManagerScript.instance.currentGameState == GameState.InGame)
        //{        

            // Check for shooting
			if (rightTrigger > 0 && justShot == false && GameManagerScript.instance.currentGameState == GameState.InGame && !reloadingBool)
            {
            //Debug.Log("Player " + playerNum + " has " + bulletsInClip + " in their clip.");

            
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
        if(!reloadingBool && reloadButton)
        {
            Reload();
        }
	}



    private void Shoot()
    {
        //Debug.Log("Shooting bullets");

		if(!source.isPlaying)
		{
			source.clip = shootSound;
			//source.Play();
		}

        // Instantiate the bullet and shoot it
        Instantiate(bulletPrefab, transform.position, gameObject.transform.parent.GetComponent<PlayerInputManager>().eyes.transform.rotation);

        bulletsInMag--;

        justShot = true;
        StartCoroutine("ShootingBoolReset");
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


    
    private void Reload()
    {
        reloadingBool = true;

        bulletsInMag = 0;

        //Debug.Log("Reload Called: " + reloadTime);
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
        //bulletsInClip += clipSize;
        StartCoroutine("ReloadingBoolReset");
    }
    /// <summary>
    /// Used to reset the reloading variable variable.
    /// Used with Couritines to prevent players from shooting during reloading
    /// </summary>
    public IEnumerator ReloadingBoolReset()
    {
        yield return new WaitForSeconds(reloadTime);
        bulletsInMag = magSize;
        reloadingBool = false;
    }

}
