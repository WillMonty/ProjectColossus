﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    #region attributes
    // Attributes
    const float BULLETSPEED = 50f;
    
    // Properties
    public Weapons gunOwnerType;
    public GameObject HitAudioObject;

    private Vector3 startPos;
    private float damage;

    Rigidbody rb;
    #endregion
    
    #region Properties
    // Properties
    public float Damage
    {
        get { return damage; }
    }
    #endregion


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
        if ((transform.position - startPos).magnitude > 300)
        {
            deleteBullet();
        }
	}


    // Move method for bullets
    /// <summary>
    /// Updates the bullets position
    /// </summary>
    void bulletMove()
    {
        rb.velocity = transform.forward * BULLETSPEED;
        //Debug.Log("Move " + bulletSpeed);
    }

    /// <summary>
    /// Deletes the bullet if it moves too far from it's start point to prevent too many bullets being on screen
    /// </summary>
    public void deleteBullet()
    {
        // Destroy this bullet if it gets too far
        Destroy(gameObject);

    }

	void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.tag == "colossusplayer")
		{
			Instantiate(HitAudioObject, transform.position, Quaternion.identity);
			GameManagerScript.instance.colossus.Damage(damage);
		}
		else if(collision.gameObject.tag == "colossusarms")
		{
			Instantiate(HitAudioObject, transform.position, Quaternion.identity);
			GameManagerScript.instance.colossus.Damage(damage*0.75f);
		}
		else if(collision.gameObject.tag == "colossushead")
		{
			Instantiate(HitAudioObject, transform.position, Quaternion.identity);
			GameManagerScript.instance.colossus.Damage(damage*1.20f);
		}
			
		// Destroy the projectile
		if (collision.gameObject.tag != "projectile" && collision.gameObject.tag != "resistancebullet" && collision.gameObject.tag != "resistanceplayer")
		{
			Destroy(gameObject);
		}
	}

}