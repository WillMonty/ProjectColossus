using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossusPositionTrigger : MonoBehaviour {

    bool colossusInTrigger;

    AudioSource source;
    [Header("Audio")]
    public AudioClip inBoundsSound;
    public AudioClip outBoundsSound;

	public bool ColossusInTrigger
    {
        get
        {
            return colossusInTrigger;
        }
    }

    // Use this for initialization
    void Start () {
        source = gameObject.GetComponent<AudioSource>();
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.name == "ColossusPosition")
		{
			colossusInTrigger = true;

			//Don't play in/out sounds if the game is going
			if(GameManagerScript.instance.currentGameState != GameState.InGame)
			{
				if(GameManagerScript.instance.forceStartGame) //Stops first frame sound firing when debugging
					return;
				
				source.Stop();
				source.clip = inBoundsSound;
				source.Play();			
			}
		}
    }

    void OnTriggerExit(Collider other)
    {
		if (other.name == "ColossusPosition")
		{
			colossusInTrigger = false;

			if(GameManagerScript.instance.currentGameState != GameState.InGame)
			{
				if(GameManagerScript.instance.forceStartGame)
					return;

				source.Stop();
				source.clip = outBoundsSound;
				source.Play();			
			}
		}
    }
}
