using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(BoxCollider))]

// Move objects within the box collider trigger on this object
public class MoveObjectTrigger : MonoBehaviour {

    public Vector3 movementForce;
    private BoxCollider conveyorMovementTrigger;

	// Use this for initialization
	void Start ()
    {
        conveyorMovementTrigger = GetComponent<BoxCollider>();
        conveyorMovementTrigger.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Move throwable objects across the trigger
    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.transform.parent.name);
        //Debug.Log(other.name);

        // Only move Throwable objects
		if (other.GetComponent<Throwable>() != null || other.transform.parent.GetComponent<Throwable>() != null || other.tag == "resistanceplayer")
        {
            // If the other object doesn't have a rigidbody...
            if (other.GetComponent<Rigidbody>() == null)
            {
                // ...but its parent does
                if (other.transform.parent.GetComponent<Rigidbody>() != null)
                {
                    //Debug.Log(other.name);
                    other.transform.parent.GetComponent<Rigidbody>().AddForce(transform.rotation * movementForce, ForceMode.Acceleration);
                    //other.transform.parent.GetComponent<Rigidbody>().vel(transform.rotation * movementForce);
                }
            }

            // If the other object does have a rigidbody, though...
            else
            {
                other.GetComponent<Rigidbody>().AddForce(transform.rotation * movementForce);
            }
        }
    }

    // Draw gizmos showing the direction a force is being applied in
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.rotation * movementForce);
    }
}
