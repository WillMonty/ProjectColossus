using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Player Manager
    const int STARTING_LIVES = 3;
    const float RESPAWN_TIME = 5.0f;
    const float DAMAGING_OBJECT_MAGNITUDE = 5.0f;

    // Basic Player Management variables
    private int lives;
    private float health;
    private Weapons currentWeapon;

    // Ammo Variables
    // Change this variable to private at some point
    public int assaultRifleAmmo;


    // Use this for initialization
    void Start()
    {
        // Start initializes 3 lives at start, can be changed later
        lives = STARTING_LIVES;
        health = 100.0f;


        // Set ammo at the beginning
        currentWeapon = Weapons.AssaultRifle;
        assaultRifleAmmo = 250;
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
        // Check how many lives the player has first
        if (lives > 1)
        {
            // Lower the life count
            lives--;



            // Choose one of the spawn locations at random
            int spawnLocation = Random.Range(0, 10);

            // Call respawn after a player died (currently set to 0,0,0)
            StartCoroutine("Respawn", new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// Damage method to damage the player
    /// </summary>
    /// <param name="spawnlocation"></param>
    /// <returns></returns>
    void Damage(float damageFloat)
    {
        health -= damageFloat;
    }

    IEnumerable Respawn(Vector3 spawnlocation)
    {
        // Check how many lives the player has first
        yield return new WaitForSeconds(RESPAWN_TIME);
        gameObject.transform.position = spawnlocation;
    }

    private void OnCollisionEnter(Collider collider)
    {

        // IF the robot is hit with the bullet, it damages the robot and deletes the bullet
        if (collider.tag == "object")
        {
            // Only damage the player if the object is moveing at a high velocity (number can be determined and changed through the player constant)
            if (collider.GetComponent<Rigidbody>().velocity.magnitude > DAMAGING_OBJECT_MAGNITUDE)
            {

                float movingObjectDamage = collider.GetComponent<Rigidbody>().velocity.magnitude;

                Damage(movingObjectDamage);
                collider.GetComponent<BulletScript>().deleteBullet();
            }
        }
    }
}
