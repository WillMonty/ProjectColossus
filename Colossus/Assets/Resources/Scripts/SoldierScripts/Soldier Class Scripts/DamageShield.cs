using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShield : MonoBehaviour, IHealth
{
     float health = 200;
    const float DAMAGING_OBJECT_MAGNITUDE = .5f;

    public float Health
    {
        get
        {
            return health;
        }
    }

    public void DamageObject(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            Destroy(transform.parent.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        // If a player is hit with an object the robot throws
        if (collisionObject.tag == "throwable")
        {
            // Only damage the player if the object is moveing at a high velocity (number can be determined and changed through the player constant)
            if (collisionObject.GetComponent<Rigidbody>().velocity.magnitude > DAMAGING_OBJECT_MAGNITUDE)
            {
                float movingObjectDamage = collisionObject.GetComponent<Rigidbody>().velocity.magnitude;
                DamageObject(movingObjectDamage);
            }
            else if (collisionObject.GetComponent<Rigidbody>().isKinematic)
            {
                float movingObjectDamage = collisionObject.GetComponent<Valve.VR.InteractionSystem.VelocityEstimator>().GetVelocityEstimate().magnitude;
                if (movingObjectDamage <= 0.0f)
                    movingObjectDamage = 15f;
                DamageObject(movingObjectDamage);
            }
        }
    }
}
