using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployShield : MonoBehaviour
{
    public GameObject shieldWall;
    Rigidbody body;
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * 200f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "explosion")
        {
            Destroy(body);
            transform.up = Vector3.up;
            shieldWall.SetActive(true);
        }
    }
}
