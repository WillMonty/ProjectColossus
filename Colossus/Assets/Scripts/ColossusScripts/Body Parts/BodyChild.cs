using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyChild : MonoBehaviour {

	public BodyParent parent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "projectile")
		{
			parent.OnCollisionEnter(collision);
		}
			
	}
}
