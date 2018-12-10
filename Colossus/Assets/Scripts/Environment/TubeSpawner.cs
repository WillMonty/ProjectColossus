using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeSpawner : MonoBehaviour {

	float spawnGoal;
    float spawnTimer;

	//Determines if trigger area has been cleared of the last item spawned
	bool triggerClear = true;
	GameObject currSpawned;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(LateStart(0.2f));
	}

	IEnumerator LateStart(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		//Get spawn time and spawn initial object
		spawnGoal = EnvironmentManagerScript.instance.throwableSpawnTime;
		SpawnObject();
		spawnTimer = 0.0f;
	}

	// Update is called once per frame
	void Update ()
    {
		if(triggerClear && spawnGoal != 0f)
		{
			spawnTimer += Time.deltaTime;

			// Once timer is ready, spawn a random object and reset the timer
			if (spawnTimer >= spawnGoal)
			{
				SpawnObject();
				spawnTimer = 0.0f;
			}	
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(currSpawned == null) return;

		if(other.gameObject == currSpawned)
		{
			triggerClear = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(currSpawned == null) return;

		if(other.gameObject == currSpawned)
			triggerClear = false;
	}
		

    // Spawn an object from one of the environment manager's item lists
    void SpawnObject()
    {
		EnvironmentManagerScript enviroInstance = EnvironmentManagerScript.instance;
		int spawnValue = Random.Range(0, 100);

		if(spawnValue > 90)
		{
			if (enviroInstance.rareSpawnablePrefabs.Count > 0)
			{
				SetupObject(enviroInstance.rareSpawnablePrefabs);
			}
			else
			{
				SetupObject(enviroInstance.commonSpawnablePrefabs);
			}
		}
		else if(spawnValue > 70)
		{
			if (enviroInstance.uncommonSpawnablePrefabs.Count > 0)
			{
				SetupObject(enviroInstance.uncommonSpawnablePrefabs);
			}
			else
			{
				SetupObject(enviroInstance.commonSpawnablePrefabs);
			}
		}
		else
		{
			SetupObject(enviroInstance.commonSpawnablePrefabs);
		}
    }

	/// <summary>
	/// Helper method to setup the new throwable object from a passed list
	/// </summary>
	void SetupObject(List<GameObject> objList)
	{
		currSpawned = Instantiate(
			objList[Random.Range(0, objList.Count)],
			transform.GetChild(0).transform.position,
			Quaternion.Euler(Vector3.zero)) as GameObject;

		//currSpawned.GetComponent<Rigidbody>().isKinematic = true;
		currSpawned.name = "ThrowableFromTube";

		triggerClear = false;
		EnvironmentManagerScript.AddRubbishToList(currSpawned);
	}
    
	//Show spawn point
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.GetChild(0).transform.position, 0.1f);
    }
}
