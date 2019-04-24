﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeData : MonoBehaviour,IDamage
{

    public GameObject explosion;
    public GameObject ring1;
    public GameObject ring2;
    public Material ringMat;

    Material tempMat;
    float count;
    bool isProj = true;
    Rigidbody body;
    float damage = 7f;
    int ownerNumber;

    public int Owner
    {
        get
        {
            return ownerNumber;
        }

        set
        {
            ownerNumber = value;
        }
    }

    public float Damage
    {
        get{ return damage; }
    }

    void Start ()
    {
        
        body = GetComponent<Rigidbody>();       
        body.AddForce(transform.forward * 1000f);
        body.AddTorque(new Vector3(20f, 20f, 0));

        tempMat = new Material(ringMat);
        ring1.GetComponent<Renderer>().material = tempMat;
        ring2.GetComponent<Renderer>().material = tempMat;

        StartCoroutine("Dim");
        
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (!isProj)
        {
            if (other.gameObject.GetComponent<IHealth>() != null)
            {
                other.gameObject.GetComponent<IHealth>().DamageObject(damage);
               
            }
           
            if (other.gameObject.GetComponent<Rigidbody>()!=null)
            {
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 0.75f, 2f, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
		//Don't hit yourself or another explosion
        if (collision.gameObject.tag != ("soldier"+ownerNumber) && collision.gameObject.tag != "explosion" && isProj)
        {
			//Don't explode on colossus shield hit
			if(collision.gameObject.tag != "colossusshield")
			{
				gameObject.tag = "explosion"; //Set prefab to explosion before it occurs
				GameObject expClone =Instantiate(explosion, transform.position, Quaternion.identity);
				expClone.transform.localScale=expClone.transform.localScale * 0.25f;
				GetComponent<SphereCollider>().enabled = true;
				Destroy(expClone, 2);	
			}

            isProj = false;
            StopAllCoroutines();
            ring1.GetComponent<MeshRenderer>().enabled = false;
            ring2.GetComponent<MeshRenderer>().enabled = false;

            for (int i = 0; i < 3; i++)
                Destroy(transform.GetChild(0).gameObject);
           Destroy(body);

            Destroy(gameObject, 0.25f);
        }

       
    }

    
 
    IEnumerator Dim()
    {
        count = 0;
        while (count<2)
        {
            count+=0.01f;
            Color final = Color.red * Mathf.LinearToGammaSpace(1 - count);
            tempMat.SetColor("_EmissionColor", final);
            yield return null;
        }
        StartCoroutine("Brighten");
    }

    IEnumerator Brighten()
    {
        count = 0;
        while (count < 1)
        {
            count += 0.01f;
            Color final = Color.red * Mathf.LinearToGammaSpace(count);
            tempMat.SetColor("_EmissionColor", final);
            yield return null;
        }
        StartCoroutine("Dim");
    }

}