﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployShield : MonoBehaviour
{
    
    public GameObject shieldWall;
    Rigidbody body;
    Vector3 ogForward;
 

    // Use this for initialization
    void Start()
    {
        
        body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * 50f);
        ogForward = transform.forward;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "explosion")
        {
            Destroy(body);                
            shieldWall.SetActive(true);
            Vector3 rot = transform.eulerAngles;
            rot.x = rot.z = 0;
            transform.eulerAngles = rot;

        }
    }

   
}