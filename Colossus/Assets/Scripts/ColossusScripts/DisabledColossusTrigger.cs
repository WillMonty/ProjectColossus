using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledColossusTrigger : MonoBehaviour {

    bool headsetInTrigger;

    public bool HeadsetInTrigger
    {
        get
        {
            return headsetInTrigger;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Camera (eye)") headsetInTrigger = true;
    }
}
