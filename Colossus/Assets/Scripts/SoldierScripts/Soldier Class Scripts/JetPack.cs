using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour {

    const float MAX_FUEL = 5;
    public float MaxFuel
    {
        get { return MAX_FUEL; }
    }
    const float JETPACK_FORCE = 1f;

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
    void Update ()
    {

        
        if (jetPackFuel > 0 && GetComponent<PlayerInput>().JumpState==2)
        {           
            FuelDown();
            GetComponent<PlayerMovement>().TimeFalling -= Time.deltaTime;
            GetComponent<PlayerMovement>().VerticalVelocity += JETPACK_FORCE;
            
        }
        else if(jetPackFuel<MAX_FUEL)
        {
            jetPackFuel += Time.deltaTime/4f;
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
