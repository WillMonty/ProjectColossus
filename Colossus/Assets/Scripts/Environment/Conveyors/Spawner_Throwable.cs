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

    private float spawnTimer = 0.0f;
    private float spawnGoal;

    public float spawnRadius;
    private Vector3 spawnRotation;
    private Vector3 spawnLocation;

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
            spawnTimer %= spawnGoal;
            SelectNewSpawnTime();
        }
	}

	/// <summary>
	/// Quick shortcut to spawn common object
	/// </summary>
	private void SpawnCommon()
	{
		GameObject newCommon = Instantiate(
			commonSpawnablePrefabs[Random.Range(0, commonSpawnablePrefabs.Count)],
			spawnLocation,
			Quaternion.Euler(spawnRotation)) as GameObject;
		newCommon.name = "Common Rubbish";

		newCommon.GetComponent<Rigidbody>().AddForce(transform.rotation * spawnForce);

		TrashCollector.AddRubbishToList(newCommon);
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
				newThrowable = Instantiate(
					rareSpawnablePrefabs[Random.Range(0, rareSpawnablePrefabs.Count)],
					spawnLocation,
					Quaternion.Euler(spawnRotation)) as GameObject;
				newThrowable.name = "Rare Rubbish";

				// Spawn the object with an initial force
				newThrowable.GetComponent<Rigidbody>().AddForce(transform.rotation * spawnForce);
				TrashCollector.AddRubbishToList(newThrowable);

				TrashCollector.AddRubbishToList(newThrowable);
			}
			else
			{
				SpawnCommon();
			}
		}
		else if(spawnValue > 70)
		{
			if (uncommonSpawnablePrefabs.Count > 0)
			{
				newThrowable = Instantiate(
					uncommonSpawnablePrefabs[Random.Range(0, uncommonSpawnablePrefabs.Count)],
					spawnLocation,
					Quaternion.Euler(spawnRotation)) as GameObject;
				newThrowable.name = "Uncommon Rubbish";
				newThrowable.GetComponent<Rigidbody>().AddForce(transform.rotation * spawnForce);

				TrashCollector.AddRubbishToList(newThrowable);
			}
			else
			{
				SpawnCommon();
			}
		}
		else
		{
			SpawnCommon();
		}
			
    }

    // Choose a new time within random variation to spawn the next object
    private void SelectNewSpawnTime()
    {
        spawnGoal = spawnTimeInterval + Random.Range(0.0f, spawnTimeVariation) * (Random.Range(0, 3) * 2 - 1);
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
