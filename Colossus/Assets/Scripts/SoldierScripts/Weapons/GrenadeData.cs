using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeData : MonoBehaviour {

    public GameObject explosion;
    public GameObject ring1;
    public GameObject ring2;
    public Material ringMat;
    Material tempMat;
    Material ringMat2;
    float count;

	// Use this for initialization
	void Start ()
    {
        
        Rigidbody body = GetComponent<Rigidbody>();       
        body.AddForce(transform.forward * 1000f);
        tempMat = new Material(ringMat);
        ring1.GetComponent<Renderer>().material = tempMat;
        ring2.GetComponent<Renderer>().material = tempMat;

        StartCoroutine("Dim");
        
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        //Instantiate(explosion,transform.position,Quaternion.identity);
        if(collision.gameObject.tag!="resistanceplayer")
            Destroy(gameObject);
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
