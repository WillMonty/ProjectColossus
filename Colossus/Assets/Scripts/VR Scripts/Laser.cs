using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    //With beam variable arrays, 0 index is left and 1 index is right
    public GameObject[] origins = new GameObject[2]; //Starting object beams come from
    public GameObject[] beams = new GameObject[2]; //Beam cylinders

    RaycastHit[] hits = new RaycastHit[2];
    float lastDistance = 0.0f; //Last distance recorded by raycast
    float maxDistance = 10.0f;

    bool firing = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Update the raycasts if firing
	    if(firing)
        {
            for(int i = 0; i < 2; i++)
            {
                Physics.Raycast(origins[i].transform.position, origins[i].transform.forward, out hits[i], maxDistance);
                //Vector3 hitDistanceLocal = beams[i].transform.InverseTransformPoint(hits[i].distance, hits[i].distance, hits[i].distance);
                //Vector3 maxDistanceLocal = beams[i].transform.InverseTransformPoint(maxDistance, maxDistance, maxDistance);

                //Move and scale the beam to the appropriate position
                Vector3 currBeamLocalPosition = beams[i].transform.localPosition;
                Vector3 currBeamScale = beams[i].transform.localScale;

                //If the raycast is actually hitting something and changing each frame
                if (lastDistance != hits[i].distance)
                {
                    beams[i].transform.localPosition = new Vector3(currBeamLocalPosition.x, currBeamLocalPosition.y, hits[i].distance/2); //origins[i].transform.forward * hits[i].distance / 2;
                    beams[i].transform.localScale = new Vector3(currBeamScale.x, hits[i].distance/2, currBeamScale.z);
                }
                else
                {
                    beams[i].transform.localPosition = new Vector3(currBeamLocalPosition.x, currBeamLocalPosition.y, maxDistance/2); //origins[i].transform.forward * maxDistance / 2;
                    beams[i].transform.localScale = new Vector3(currBeamScale.x, maxDistance/2, currBeamScale.z);
                }


                lastDistance = hits[i].distance;

                Debug.Log(hits[i].distance);
                // Need to convert the beam's local scale to the Raycast's globally scaled distance
            }
        }
	}
}
