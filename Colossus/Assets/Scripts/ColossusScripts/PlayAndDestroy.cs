using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAndDestroy : MonoBehaviour {


    AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        audio.Play();
        Destroy(gameObject, audio.clip.length);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
