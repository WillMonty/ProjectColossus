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

    // Variables
    private Weapons weaponType;

    // Bullet prefab that will be shotout
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
    public GameObject snipePrefab;

    GameObject projectile;
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

        fireDelay = .05f;
        
        if (weaponType == Weapons.AssaultRifle)
        {
            fireDelay = .05f;
            magSize = 30;
            reloadTime = 2.0f;
            projectile = bulletPrefab;
        }

        else if (weaponType == Weapons.GrenadeLauncher)
        {
            fireDelay = 1f;
            magSize = 6;
            reloadTime = 4.0f;
            projectile = grenadePrefab;
        }

        else if (weaponType == Weapons.Sniper)
        {
            fireDelay = 0.5f;
            magSize = 10;
            reloadTime = 3.0f;
            projectile = snipePrefab;
        }

        bulletsInMag = magSize;

		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
            // Check for shooting
			if (rightTrigger > 0 && justShot == false && GameManagerScript.instance.currentGameState == GameState.InGame && !reloadingBool)
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
