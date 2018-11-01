using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour {

    public GameObject smokeTrail;

    const float MAX_FUEL = 1.5f;
    public float MaxFuel
    {
        get { return MAX_FUEL; }
    }
    const float JETPACK_FORCE = 0.5f;

    private float jetPackFuel;
    public float JetPackFuel
    {
        get { return jetPackFuel; }
    }

    // Use this for initialization
    void Start()
    {
        jetPackFuel = MAX_FUEL;
    }

     // Update is called once per frame
    void FixedUpdate ()
    {
        if (jetPackFuel > 0 && GetComponent<PlayerInput>().ActionState > 0)
        {
            
            if(GetComponent<PlayerInput>().ActionState == 1)
                smokeTrail.GetComponent<ParticleSystem>().Play();

            FuelDown();
			GetComponent<Rigidbody>().AddForce(0,9f,0,ForceMode.Acceleration);     
				

        }
        else if(jetPackFuel<MAX_FUEL)
        {
            smokeTrail.GetComponent<ParticleSystem>().Stop();
            jetPackFuel += Time.deltaTime/5f;
        }
    }

    /// <summary>
    /// Remove fuel back to the player's jetpack
    /// </summary>
    public void FuelDown()
    {
        jetPackFuel -= Time.deltaTime;
    }


}
