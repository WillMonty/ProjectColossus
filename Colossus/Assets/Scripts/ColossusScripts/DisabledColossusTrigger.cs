﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledColossusTrigger : MonoBehaviour {

    bool headsetInTrigger;

    AudioSource source;
    [Header("Audio")]
    public AudioClip inBoundsSound;

    public bool HeadsetInTrigger
    {
        get
        {
            return headsetInTrigger;
        }
    }

    // Use this for initialization
    void Start () {
        source = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        source.Stop();
        source.clip = inBoundsSound;
        source.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Camera (eye)") headsetInTrigger = true;
    }
}
