using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MakeBalls : MonoBehaviour {

    public GameObject ballPrefab;
    private Hand hand;
    private Transform controllerTip;
    public Vector3 controllerVelocity;

    // Use this for initialization
    void Start () {
        hand = GetComponent<Hand>();
        
        controllerTip = hand.transform.GetChild(0).transform;
        controllerVelocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (hand.controller != null)
        {
            controllerVelocity = GetComponent<VelocityEstimator>().GetVelocityEstimate();

            if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                GameObject newBall = Instantiate(ballPrefab, controllerTip.position, Quaternion.identity);
                newBall.GetComponent<Rigidbody>().velocity = hand.GetTrackedObjectVelocity();
            }
        }
    }
}
