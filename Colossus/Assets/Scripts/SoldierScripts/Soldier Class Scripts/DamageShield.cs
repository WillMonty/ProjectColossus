using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageShield : MonoBehaviour, IHealth
{
    float health = 200;

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
}
