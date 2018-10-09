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
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.name == "ColossusPosition")
		{
			colossusInTrigger = true;
			source.Stop();
			source.clip = inBoundsSound;
			source.Play();	
		}
    }

    void OnTriggerStay(Collider other)
    {
		if (other.name == "ColossusPosition") colossusInTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
		if (other.name == "ColossusPosition")
		{
			colossusInTrigger = false;
			source.Stop();
			source.clip = outBoundsSound;
			source.Play();
		}
    }
}
