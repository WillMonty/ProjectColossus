using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    #region attributes
    // Attributes
    const float BULLETSPEED = 100f;

    // Properties
    public Weapons gunOwnerType;
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

}
