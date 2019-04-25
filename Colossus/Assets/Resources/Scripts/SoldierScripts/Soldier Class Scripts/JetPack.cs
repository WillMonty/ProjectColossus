using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour {

    public GameObject smokeTrail;
	public AudioSource startSource;
	public AudioSource flySource;

	bool fadingFly;

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
		int ltState = GetComponent<PlayerInput>().ActionState;

		if (jetPackFuel > 0 && ltState > 0)
        {
			//First frame of press
			if(ltState == 2)
			{
				startSource.Play();
				smokeTrail.GetComponent<ParticleSystem>().Play();
			}
            
			//Trigger held
			if(ltState == 1)
			{
				if(!flySource.isPlaying)
					flySource.Play();	
			}
                
            FuelDown();
			GetComponent<Rigidbody>().AddForce(0,9f,0,ForceMode.Acceleration);     
        }

		if(ltState == 0 && jetPackFuel<MAX_FUEL)
		{
			smokeTrail.GetComponent<ParticleSystem>().Stop();
			jetPackFuel += Time.deltaTime/5f;
		}

		if(ltState == 0 && flySource.isPlaying)
		{
			smokeTrail.GetComponent<ParticleSystem>().Stop();
			if(!fadingFly)
				StartCoroutine(AudioFadeOut.FadeOut(flySource, 0.2f));

			fadingFly = true;
		}

		if(!flySource.isPlaying)
			fadingFly = false;
    }

    /// <summary>
    /// Remove fuel back to the player's jetpack
    /// </summary>
    public void FuelDown()
    {
        jetPackFuel -= Time.deltaTime;
    }


}
