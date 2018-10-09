using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Weapons
{
    AssaultRifle,
    Sniper,
    GrenadeLauncher,
}

public class GunBase : MonoBehaviour, IWeapon
{
    #region variables
    protected Weapons weaponType;
    public Weapons WeaponType
    {
        get { return weaponType; }
    }

    // Bullet prefab that will be shot
    public GameObject projectile;
    GameObject projClone;

    //Audio Source
    protected AudioSource source;
    public AudioClip shootSound;

    // current gun Tracking variables
    protected int bulletsInMag;
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
    public float ReloadTime
    {
        get { return reloadTime; }
    }

    // Variables to keep shooting from breaking and balanced
    protected bool justShot;

    protected bool reloading;
    public bool Reloading
    {
        get { return reloading; }
    }

    protected bool autoFire;

    // Variables that handle input
    float rightTrigger;
    public float RightTrigger
    {
        set {  rightTrigger = value; }
    }

    float rightTriggerPrev;
    public float RightTriggerPrev
    {
        set { rightTriggerPrev = value; }
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


    #endregion

    // Use this for initialization
    protected virtual void Start ()
    {
        // Set bool variables initially
        justShot = false;
        reloading = false;
        
        if (weaponType == Weapons.AssaultRifle)
        {
            autoFire = true;
            fireDelay = .2f;
            magSize = 30;
            reloadTime = 2.0f;
        }

        else if (weaponType == Weapons.GrenadeLauncher)
        {
            autoFire = false;
            fireDelay = 1f;
            magSize = 6;
            reloadTime = 3.0f;
        }

        else if (weaponType == Weapons.Sniper)
        {
            autoFire = false;
            fireDelay = 0f;
            magSize = 1;
            reloadTime = 2.0f;
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
            if (rightTrigger > 0 && justShot == false && !reloading)
            {
                if (bulletsInMag > 0)
                {
                    Shoot();
                }

                else if(rightTriggerPrev <= 0)
                {
                    Reload();
                }

            }
            //Check for reloading
            else if (!reloading && ((reloadButton && bulletsInMag != magSize) || bulletsInMag == 0) && justShot == false)
            {
                Reload();
            }


        }

    }
   
    protected virtual void Shoot()
    {
		source.clip = shootSound;
		source.Play();

        // Instantiate the projectile and shoot it
        projClone=Instantiate(projectile, transform.position, transform.parent.rotation);

        projClone.GetComponent<IDamage>().Owner = playerNum;
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


        reloading = true;

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
        reloading = false;
    }

}
