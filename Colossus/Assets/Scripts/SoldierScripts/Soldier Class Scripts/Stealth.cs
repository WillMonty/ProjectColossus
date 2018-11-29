using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityState { None, Active, Cooldown };

public class Stealth : MonoBehaviour {
    
    public Material stealthMat;
    public List<GameObject> models;
    List<Material> modelMats;

    public const float COOL_DOWN = 7f;
    public float cooldown = COOL_DOWN;
    const float DURATION = 10f;

    float distortion = 0;
    bool ready = true;
    bool active = false;
    
    public AbilityState state;

    // Use this for initialization
    void Start ()
    {
        modelMats = new List<Material>();

        state = AbilityState.None;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (GetComponent<PlayerInput>().ActionState == 1 && ready)
        {
            Activate();
        }

        if(active)
        {
            CheckInterrupt();
        }

        if(distortion>0)
        {
            distortion -= Time.deltaTime/2.0f;
            if (distortion < 0)
                distortion = 0;
            stealthMat.SetFloat("_Distortion", distortion);
        }
    }

    void Activate()
    {
        distortion = 0;
        stealthMat.SetFloat("_Distortion", 0f);
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

    void CheckInterrupt()
    {
        if (GetComponent<PlayerData>().WeaponData.Shooting || GetComponent<PlayerData>().DamageTaken)
        {
            distortion = 1;
            stealthMat.SetFloat("_Distortion", 1f);
        }
    }


    IEnumerator StartCD()
    {
        state = AbilityState.Cooldown;

        yield return new WaitForSeconds(COOL_DOWN);
        ready = true;

        state = AbilityState.None;
    }

    IEnumerator StartDuration()
    {
        state = AbilityState.Active;

        yield return new WaitForSeconds(DURATION);

        foreach (GameObject obj in models)
            ResetRecursively(obj);

        active = false;
        StartCoroutine(StartCD());

    }
}
