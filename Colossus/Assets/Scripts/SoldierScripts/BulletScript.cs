using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Attributes
    const float bulletSpeed = 100f;

    // Properties
    public Weapons gunOwnerType;
    Vector3 startPos;
    float damage;

    Rigidbody rb;

	// Use this for initialization
	void Start()
    {
        // Set the start position of the bullet. This is to delete it if the bullet gets too far from the player.
        startPos = transform.position;

        rb = GetComponent<Rigidbody>();

        switch (gunOwnerType)
        {
            case Weapons.AssaultRifle:
                damage = 1;
                break;
        }
	}

	// Update is called once per frame
	void Update ()
    {
        bulletMove();
        deleteBullet();
	}


    // Move method for bullets
    /// <summary>
    /// Updates the bullets position
    /// </summary>
    void bulletMove()
    {
        rb.velocity = transform.forward * bulletSpeed;
        //Debug.Log("Move " + bulletSpeed);
    }

    /// <summary>
    /// Deletes the bullet if it moves too far from it's start point to prevent too many bullets being on screen
    /// </summary>
    void deleteBullet()
    {
        if((transform.position - startPos).magnitude>300)
        {
            // Destroy this bullet if it gets too far
            Destroy(gameObject);
        }
    }

}
