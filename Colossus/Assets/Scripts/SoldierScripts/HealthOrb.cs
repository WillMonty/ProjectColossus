using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    public float healingAmount;


    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision!");
        GameObject collisionObject = col.gameObject;

        if (collisionObject.gameObject.tag == "resistanceplayer")
        {
            collisionObject.gameObject.GetComponent<PlayerManager>().Heal(healingAmount);
        }

        // Turn off the health orb
        gameObject.SetActive(false);
    }
}
