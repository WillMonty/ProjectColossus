using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossusManager : MonoBehaviour {

    #region Robot Attributes
    // Robot Manager
    const int STARTING_HEALTH = 1000;
    const int LASEREYEABILITYDELAY = 1000;
    const float RESPAWN_TIME = 5.0f;

    // Basic Player Management variables
    private float health;

    // Robot Ability variables
    private float laserEyeAbilityCounter;
    private bool canUserLaserEyes;

    // Ammo Variables
    // Change this variable to private at some point
    public int assaultRifleAmmo;
    #endregion

    #region Properties
    // Properties

    /// <summary>
    /// Could be used for a boss health GUI element for soldiers
    /// </summary>
    public float Health
    {
        get { return health; }
    }

    #endregion

    #region Start Method
    // Use this for initialization
    void Start()
    {
        // Start initializes 3 lives at start, can be changed later
        health = STARTING_HEALTH;


    }

    #endregion

    #region Update Method
    // Update is called once per frame
    void Update ()
    {
	    // Managment of ability Variables
        


	}
    #endregion

    #region Combat Helper Methods
    // Damage helper method
    public void Damage(float damageFloat)
    {
        health -= damageFloat;
    }

    /// <summary>
    /// Method to handle all Colossus abilities
    /// </summary>
    private void abilityManagement()
    {

    }


    // Script for the laser eye ability
    private void LaserEye()
    {

    }
    #endregion

    private void OnCollisionEnter(Collider collider)
    {

        // IF the robot is hit with the bullet, it damages the robot and deletes the bullet
        if(collider.tag == "playerbullet")
        {
            Damage(collider.GetComponent<BulletScript>().Damage);
            collider.GetComponent<BulletScript>().deleteBullet();
        }
    }
}
