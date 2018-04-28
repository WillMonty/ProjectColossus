﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Player Manager
    const int STARTING_LIVES = 2;
    const float RESPAWN_TIME = 5.0f;
    const float DAMAGING_OBJECT_MAGNITUDE = .02f;
    const float MAX_HEALTH = 100;
    const float MAX_FUEL = 5;

    // Basic Player Management variables
    private int lives;
    private float health;
    private float jetPackFuel;
    private Weapons currentWeapon;

    SoldierUI PlayerGUIManager;

    // Ammo Variables
    // Change this variable to private at some point
    public int assaultRifleAmmo;

	private GameObject[] spawnPoints;

    public int Lives
    {
        get { return lives; }
    }
    public float MaxHealth
    {
        get { return MAX_HEALTH; }
    }
    public float MaxFuel
    {
        get { return MAX_FUEL; }
    }
    public float Health
    {
        get { return health; }
    }
    public float JetPackFuel
    {
        get { return jetPackFuel; }
    }



    // Use this for initialization
    void Start()
    {

		spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
        // Start initializes 3 lives at start, can be changed later
        lives = STARTING_LIVES;
        health = 100.0f;
        jetPackFuel = MAX_FUEL;


        // Set ammo at the beginning
        currentWeapon = Weapons.AssaultRifle;
        assaultRifleAmmo = 250;

        PlayerGUIManager = transform.Find("Player" + GetComponent<PlayerInputManager>().playerNum + "GUI").GetComponent<SoldierUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for death first in the update loop
        if (health <= 0)
        {
            Death();
        }

    }

    /// <summary>
    /// Method handling death for the player
    /// </summary>
    void Death()
    {
		// Reset the player's Stats
		ResetPlayerValues();
		transform.position = GameManagerScript.instance.deathbox.transform.position;

        // Check how many lives the player has first
        if (lives > 1)
        {

            // Choose one of the spawn locations at random
            int spawnPoint = Random.Range(0, 6);

            Vector3 spawnLocation = Vector3.zero;

            // Turn on the respawn message
            PlayerGUIManager.SwitchActiveStatesRespawnMessage();

			spawnLocation = spawnPoints[spawnPoint].transform.position;

            // Call respawn after a player died (currently set to 0,0,0)
			StartCoroutine(Respawn(spawnLocation));
        }

		// Lower the life count
		lives--;
    }

    /// <summary>
    /// Damage method to damage the player
    /// </summary>
    /// <param name="damageFloat"></param>
    public void Damage(float damageFloat)
    {
        health -= damageFloat;
    }
    
    /// <summary>
    /// Heal the player
    /// </summary>
    /// <param name="damageFloat"></param>
    public bool Heal(float healFloat)
    {
        bool used = false;

        if (health < MAX_HEALTH)
        {
            health += healFloat;
            used = true;
        }

        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
        return used;
    }

    /// <summary>
    /// Add fuel back to the player's jetpack
    /// </summary>
    public bool Fuel(float fuelFloat)
    {
        bool used = false;

        if (jetPackFuel < MAX_FUEL)
        {
            jetPackFuel += fuelFloat;
            used = true;
        }

        if (jetPackFuel > MAX_FUEL)
        {
            jetPackFuel = MAX_FUEL;
        }
        return used;
    }

    /// <summary>
    /// Remove fuel back to the player's jetpack
    /// </summary>
    public void FuelDown()
    {
        jetPackFuel -= Time.deltaTime;
    }


    /// <summary>
    /// Method to respawn the player
    /// </summary>
    /// <param name="spawnlocation"></param>
    /// <returns></returns>
	IEnumerator Respawn(Vector3 spawnlocation)
    {
        // Check how many lives the player has first
        yield return new WaitForSeconds(RESPAWN_TIME);

        // Turn off the respawn message
        PlayerGUIManager.SwitchActiveStatesRespawnMessage();

        // Place the player in new spawn position
        gameObject.transform.position = spawnlocation;
    }

    /// <summary>
    /// Method to reset player values
    /// </summary>
    void ResetPlayerValues()
    {
        health = MAX_HEALTH;
        jetPackFuel = MAX_FUEL;
    }

	void OnTriggerEnter(Collider col)
	{
		
		GameObject collisionObject = col.gameObject;

		// If a player is hit with an object the robot throws
		if (collisionObject.tag == "throwable")
		{
			// Only damage the player if the object is moveing at a high velocity (number can be determined and changed through the player constant)
			if (collisionObject.GetComponent<Rigidbody>().velocity.magnitude > DAMAGING_OBJECT_MAGNITUDE)
			{
				float movingObjectDamage = collisionObject.GetComponent<Rigidbody>().velocity.magnitude;

				Damage(movingObjectDamage);

			}
			else if(collisionObject.GetComponent<Rigidbody>().isKinematic)
			{
				float movingObjectDamage =collisionObject.GetComponent<Valve.VR.InteractionSystem.VelocityEstimator>().GetVelocityEstimate().magnitude;
				if(movingObjectDamage<=0.0f)
					movingObjectDamage=15f;
				Damage(movingObjectDamage);
			}
		}
	}

}
