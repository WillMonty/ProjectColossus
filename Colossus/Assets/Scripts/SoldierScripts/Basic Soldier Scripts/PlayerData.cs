using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoldierClass
{
    Assault,
    Grenadier,
    Skulker,
}

public class PlayerData : MonoBehaviour
{
    // Player Manager
    const int STARTING_LIVES = 2;
    const float RESPAWN_TIME = 5.0f;
    const float DAMAGING_OBJECT_MAGNITUDE = .02f;
    const float MAX_HEALTH = 100;
    

    // Basic Player Management variables
    private int lives;
    private float health;

    public int playerNumber;
    public SoldierClass soldierClass;
    public GameObject eyes;
    public GameObject gun;

    IWeapon weaponData;
    public IWeapon WeaponData
    {
        get { return weaponData; }
    }
    private GameObject[] spawnPoints;

    public int Lives
    {
        get { return lives; }
    }
    public float MaxHealth
    {
        get { return MAX_HEALTH; }
    }
   
    public float Health
    {
        get { return health; }
    }
    



    // Use this for initialization
    void Start()
    {

		spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
        // Start initializes 3 lives at start, can be changed later

        lives = STARTING_LIVES;
        health = 100.0f;


        weaponData = gun.GetComponent<IWeapon>();
        weaponData.PlayerNum = playerNumber;

        SetCamera();
        StartCoroutine(LateStart(0.2f));
        //Sets render layer of models
        
        SetRenderCull();

    }


    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Sets the instance after the game manager has actually initializes
        switch (playerNumber)
        {
            case 1:
                GameManagerScript.instance.soldier1 = GetComponent<PlayerData>();
				tag = "soldier1";
                break;
            case 2:
                GameManagerScript.instance.soldier2 = GetComponent<PlayerData>();
				tag = "soldier2";
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Check for death first in the update loop
        if (health <= 0)
        {
            Death();
        }

        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            health -= 10;
        }
    }

    //Because of our current model/animation system we need to 
    //recusivly change render layers for the models 
    //AND change the camera culling mask which uses 32 bits to represent differnt flags
    //https://answers.unity.com/questions/8715/how-do-i-use-layermasks.html
    #region HackyStuff
    void SetRenderCull()
    {
        int layer = 8 + playerNumber;
        int newMask = eyes.GetComponent<Camera>().cullingMask;

        SetLayerRecursively(GetComponent<PlayerMovement>().StandingPose, layer);
        SetLayerRecursively(GetComponent<PlayerMovement>().RunningAnimation, layer);

        if (playerNumber == 2)
            newMask += 512; //flag 9
        else
            newMask += 1024; //flag 10

        eyes.GetComponent<Camera>().cullingMask = newMask;
    }

    //Recursive method thats sets a layer for the gameobject and all its children
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, newLayer);
    }

    #endregion

    /// <summary>
    /// Method for setting viewport based on player number
    /// </summary>
    void SetCamera()
    {
        if(playerNumber==2)
        {
            Rect temp = eyes.GetComponent<Camera>().rect;
            temp.x = 0.5f;
            eyes.GetComponent<Camera>().rect = temp;
        }
    }

    /// <summary>
    /// Method handling death for the player
    /// </summary>
    void Death()
    {
		// Reset the player's Stats
		ResetPlayerValues();
		transform.position = GameManagerScript.instance.deathbox.transform.position;

        // Check how many lives the player has first
        if (lives > 1)
        {

            // Choose one of the spawn locations at random
            int spawnPoint = Random.Range(0, 6);

            Vector3 spawnLocation = Vector3.zero;

			spawnLocation = spawnPoints[spawnPoint].transform.position;

            // Call respawn after a player died (currently set to 0,0,0)
			StartCoroutine(Respawn(spawnLocation));
        }

		// Lower the life count
		lives--;
    }

    /// <summary>
    /// Damage method to damage the player
    /// </summary>
    /// <param name="damageFloat"></param>
    public void Damage(float damageFloat)
    {
        health -= damageFloat;
    }
    
    /// <summary>
    /// Heal the player
    /// </summary>
    /// <param name="damageFloat"></param>
    public bool Heal(float healFloat)
    {
        bool used = false;

        if (health < MAX_HEALTH)
        {
            health += healFloat;
            used = true;
        }

        if (health > MAX_HEALTH)
        {
            health = MAX_HEALTH;
        }
        return used;
    }





    /// <summary>
    /// Method to respawn the player
    /// </summary>
    /// <param name="spawnlocation"></param>
    /// <returns></returns>
	IEnumerator Respawn(Vector3 spawnlocation)
    {
        // Check how many lives the player has first
        yield return new WaitForSeconds(RESPAWN_TIME);

        // Place the player in new spawn position
        gameObject.transform.position = spawnlocation;
    }

    /// <summary>
    /// Method to reset player values
    /// </summary>
    void ResetPlayerValues()
    {
        health = MAX_HEALTH;
       
    }

	void OnTriggerEnter(Collider col)
	{
		
		GameObject collisionObject = col.gameObject;
		// If a player is hit with an object the robot throws
		if (collisionObject.tag == "throwable")
		{
			// Only damage the player if the object is moveing at a high velocity (number can be determined and changed through the player constant)
			if (collisionObject.GetComponent<Rigidbody>().velocity.magnitude > DAMAGING_OBJECT_MAGNITUDE)
			{
				float movingObjectDamage = collisionObject.GetComponent<Rigidbody>().velocity.magnitude;

				Damage(movingObjectDamage);

			}
			else if(collisionObject.GetComponent<Rigidbody>().isKinematic)
			{
				float movingObjectDamage =collisionObject.GetComponent<Valve.VR.InteractionSystem.VelocityEstimator>().GetVelocityEstimate().magnitude;
				if(movingObjectDamage<=0.0f)
					movingObjectDamage=15f;
				Damage(movingObjectDamage);
			}
		}
	}

}
