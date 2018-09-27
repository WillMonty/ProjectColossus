using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameState Enum
public enum OrbType { healthOrb, fuelOrb };

public class HealthOrb : MonoBehaviour
{
    const float RESPAWNTIME = 200f;

    private float respawnTimer;
    public float restoreAmount;
    public OrbType orbType;



    void Update()
    {
        if(respawnTimer>200)
        {

        }
    }

    void OnTriggerEnter(Collider col)
    {
        GameObject collisionObject = col.gameObject;

        if (collisionObject.gameObject.tag == "resistanceplayer")
        {
            bool used = false;

            switch(orbType)
            {
                case OrbType.healthOrb:
                    used = collisionObject.gameObject.GetComponent<PlayerData>().Heal(restoreAmount);
                    break;
                case OrbType.fuelOrb:
                  //  used = collisionObject.gameObject.GetComponent<PlayerData>().Fuel(restoreAmount);
                    break;
            }

            if (used)
            {
                // Turn off the health orb
                gameObject.SetActive(false);
            }
        }

    }
}
