using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAndDestroy : MonoBehaviour {

	public bool randomizePitch;
	[Range(0.0f, 1.0f)]
	public float pitchChangePercentage;

    AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();

		if(randomizePitch)
			PitchPlay();
		else
        	audio.Play();
		
        Destroy(gameObject, audio.clip.length);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PitchPlay()
	{
		float changeAmt = (3.0f * pitchChangePercentage) * Random.value;
		bool pitchUp = Random.value >= 0.5f ? true : false;

		if(pitchUp)
			audio.pitch += changeAmt;
		else
			audio.pitch -= changeAmt;

		audio.Play();
	}
}
