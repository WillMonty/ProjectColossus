using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns throwable objects at random rotations and locations within the collider attached to it
public class Spawner_Throwable : MonoBehaviour {

    public float spawnTimeInterval;     // Average rate of spawning throwable objects
    public float spawnTimeVariation;    // +/- variation to mean spawn time interval

    public List<GameObject> commonSpawnablePrefabs;
    public List<GameObject> uncommonSpawnablePrefabs;
    public List<GameObject> rareSpawnablePrefabs;

    public Vector3 spawnForce;

    float spawnTimer;
    float spawnGoal;

    public float spawnRadius;
    Vector3 spawnRotation;
    Vector3 spawnLocation;

	// Use this for initialization
	void Start ()
    {
		// Ensure valid radius
        if (spawnRadius <= 0)
        {
            spawnRadius = 1f;
        }

        // Ensure valid variation time
        if (spawnTimeVariation < 0)
        {
            spawnTimeVariation = 0;
        }

		//StartCoroutine(LateStart(0.2f));
	}

	IEnumerator LateStart(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		SpawnObject(); //Spawn an initial object
		SelectNewSpawnTime();
	}

	// Update is called once per frame
	void Update ()
    {
        spawnTimer += Time.deltaTime;

        // Once time is ready, spawn a random object, reset the timer, and get a new spawn time
        if (spawnTimer >= spawnGoal)
        {
            SpawnObject();
            spawnTimer = 0.0f;
            SelectNewSpawnTime();
        }
	}
		

    // Spawn an object in the range of the collider
    private void SpawnObject()
    {
        spawnLocation = transform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            Random.Range(-spawnRadius, spawnRadius),
            Random.Range(-spawnRadius, spawnRadius));

		spawnRotation = Vector3.zero;

		GameObject newThrowable;
		int spawnValue = Random.Range(0, 100);

		if(spawnValue > 90)
		{
			if (rareSpawnablePrefabs.Count > 0)
			{
				SetupObject(rareSpawnablePrefabs);
			}
			else
			{
				SetupObject(commonSpawnablePrefabs);
			}
		}
		else if(spawnValue > 70)
		{
			if (uncommonSpawnablePrefabs.Count > 0)
			{
				SetupObject(uncommonSpawnablePrefabs);
			}
			else
			{
				SetupObject(commonSpawnablePrefabs);
			}
		}
		else
		{
			SetupObject(commonSpawnablePrefabs);
		}
    }

	/// <summary>
	/// Helper method to setup the new throwable object from a passed list
	/// </summary>
	private void SetupObject(List<GameObject> objList)
	{
		GameObject newThrowable = Instantiate(
			objList[Random.Range(0, objList.Count)],
			spawnLocation,
			Quaternion.Euler(Vector3.zero)) as GameObject;
		
		newThrowable.name = "Rubbish";
		newThrowable.GetComponent<Rigidbody>().AddForce(transform.rotation * spawnForce);

		TrashCollector.AddRubbishToList(newThrowable);
	}

    // Choose a new time within random variation to spawn the next object
    private void SelectNewSpawnTime()
    {
		spawnGoal = spawnTimeInterval + Random.Range(-1 * spawnTimeVariation, spawnTimeVariation);
    }
    
    // Show the force it spawns objects at, and show the spawn sphere
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawLine(transform.position, transform.position + transform.rotation * spawnForce);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
