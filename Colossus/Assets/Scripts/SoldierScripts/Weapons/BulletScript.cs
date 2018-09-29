using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    #region attributes
    // Attributes
    public float bulletSpeed = 12.5f;
    
    // Properties
    public Weapons gunOwnerType;
	public int ownerNumber;

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
        BulletMove();
        if ((transform.position - startPos).magnitude > 300)
        {
			Destroy(gameObject);
        }
	}


    // Move method for bullets
    /// <summary>
    /// Updates the bullets position
    /// </summary>
    void BulletMove()
    {
        rb.velocity = transform.forward * bulletSpeed;
        //Debug.Log("Move " + bulletSpeed);
    }

	void OnCollisionEnter(Collision collision)
	{
		//TODO: Factor in bullet owner eventually
			
		// Destroy the projectile
		if (collision.gameObject.tag != "projectile" && collision.gameObject.tag != "resistancebullet" && collision.gameObject.tag != "resistanceplayer")
		{
			Destroy(gameObject);
		}
	}

}
