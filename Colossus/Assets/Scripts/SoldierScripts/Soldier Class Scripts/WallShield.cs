using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShield : MonoBehaviour {

    const float COOL_DOWN = 5f;
    public GameObject wallSpawner;
    public bool ready=true;
    bool isDeployed;
    GameObject deployedWall;
    GameObject eyes;
    GameObject gun;
    // Use this for initialization
    void Start ()
    {
        eyes = GetComponent<PlayerData>().eyes;
        gun = GetComponent<PlayerData>().gun;
    }
	
	// Update is called once per frame
	void Update ()
    {


        if (GetComponent<PlayerInput>().ActionState == 1 && ready)
        {
            Launch();
        }
    }

    void Launch()
    {
        if(deployedWall!=null)
        {
            Destroy(deployedWall);
        }

        deployedWall = Instantiate(wallSpawner,  eyes.transform.position + gun.transform.forward, eyes.transform.rotation);

        ready = false;
        StartCoroutine(StartCD());

    }

    IEnumerator StartCD()
    {
        yield return new WaitForSeconds(COOL_DOWN);
        ready = true;
       
    }
}
