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
   

    // Variables
    private Weapons weaponType;

    // Bullet prefab that will be shotout
    public GameObject projectile;
    PlayerInput playerInput; 
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

    int playerNum;

    public int PlayerNum
    {
        set { playerNum = value; }
    }

    // Use this for initialization
    void Start ()
    {
        // Set bool variables initially
        justShot = false;
        reloadingBool = false;
        playerInput = GetComponent<PlayerInput>();
  
        
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
            fireDelay = 0.5f;
            magSize = 1;
            reloadTime = 3.0f;
        }

        bulletsInMag = magSize;

		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
            // Check for shooting
			if (playerInput.RightTrigger>0 == false && GameManagerScript.instance.currentGameState == GameState.InGame && !reloadingBool)
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
        if(!reloadingBool && playerInput.ReloadButton)
        {
            Reload();
        }
	}



    private void Shoot()
    {
		if(!source.isPlaying)
		{
			source.clip = shootSound;
			//source.Play();
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
