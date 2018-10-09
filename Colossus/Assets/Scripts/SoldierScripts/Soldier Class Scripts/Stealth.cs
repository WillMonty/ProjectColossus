using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : MonoBehaviour {


    public Material stealthMat;
    public List<GameObject> models;
    public List<Material> modelMats;

    const float COOL_DOWN = 7f;
    const float DURATION = 10f;

    float alpha;
    bool ready = true;
    bool active = false;

    
    // Use this for initialization
    void Start () {
        alpha = stealthMat.color.a;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GetComponent<PlayerInput>().ActionState == 1 && ready)
        {
            Activate();
        }

        if(active)
        {
            
        }
    }

    void Activate()
    {
        active = true;
        ready = false;
        foreach (GameObject obj in models)
            ReduceRecursively(obj);

        StartCoroutine(StartDuration());
    }

    void ReduceRecursively(GameObject obj)
    {
        if(obj.GetComponent<Renderer>() !=null)
        {
            modelMats.Add(obj.GetComponent<Renderer>().material);
            obj.GetComponent<Renderer>().material = stealthMat;
        }

        foreach (Transform child in obj.transform)
            ReduceRecursively(child.gameObject);
        
    }

    void ResetRecursively(GameObject obj)
    {
        if (obj.GetComponent<Renderer>())
        {

            obj.GetComponent<Renderer>().material = modelMats[0];
            modelMats.RemoveAt(0);
        }

        foreach (Transform child in obj.transform)
            ResetRecursively(child.gameObject);
    }

    IEnumerator StartCD()
    {
        
        yield return new WaitForSeconds(COOL_DOWN);
        ready = true;

    }

    IEnumerator StartDuration()
    {
        yield return new WaitForSeconds(DURATION);

        foreach (GameObject obj in models)
            ResetRecursively(obj);

        active = false;
        StartCoroutine(StartCD());

    }
}
