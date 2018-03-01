using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRobot : MonoBehaviour {

  

	// Use this for initialization
	void Start ()
    {
        //StartCoroutine(FixPosition());
    }
	
	// Update is called once per frame
	void Update () {
        //vrCamera.position = transform.position;
    }

    /*
    IEnumerator FixPosition()
    {
        yield return new WaitForSeconds(0.5f);
        this.transform.position = vrOrigin.position;
        this.transform.rotation = vrOrigin.rotation;
        vrCamera.position = transform.position;
        Debug.Log("VR Position: " + this.transform.position);
        Debug.Log("Origin Position: " + vrOrigin.position);
        
    }
    */
}
