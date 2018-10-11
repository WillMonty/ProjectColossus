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

public class PlayerData : MonoBehaviour, IHealth
{
    // Player Manager
    const int STARTING_LIVES = 2;
    const float RESPAWN_TIME = 5.0f;
    const float DAMAGING_OBJECT_MAGNITUDE = .02f;
    const float MAX_HEALTH = 100;
    

    // Basic Player Management variables
    private int lives;
    private float health;
    private float healthPrev;
    bool justDied=false;
    Vector3 prevPos;
    

    public int playerNumber;
    public SoldierClass soldierClass;
    public GameObject eyes;
    public GameObject gun;
    public GameObject model;
    public GameObject ragdoll;

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

    public void DamageObject(float dmg)
    {
        health -= dmg;

    }

    public bool DamageTaken
    {
        get { if (health > healthPrev)
                return true;
            else
                return false;
        }
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
        StartCoroutine(LateStart(1f));
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

                tag = "soldier1";
                break;
            case 2:

                tag = "soldier2";
                break;
        }
        weaponData.PlayerNum = playerNumber;
        SetCamera();
    }


    // Update is called once per frame
    void Update()
    {
        
        
        if (justDied)
            SpawnRagdoll();

        prevPos = transform.position;
        // Check for death first in the update loop
        if (health <= 0)
        {
            Death();
        }

        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            health -= 10;
        }




        healthPrev = health;
        
    }


    //https://answers.unity.com/questions/8715/how-do-i-use-layermasks.html
    #region SetRender&Layers
    void SetRenderCull()
    {
       //Setting layers 
       
        int layer = 8 + playerNumber;
        if (model!=null)
            SetLayerRecursively(model, layer);

        layer = 11 + playerNumber;

        SetLayerRecursively(eyes.transform.GetChild(1).gameObject, layer);

        //Setting cull mask for camera
        int newMask = eyes.GetComponent<Camera>().cullingMask;
        if (playerNumber == 2)
        {
            newMask += 512; //flag 9
            newMask += 8192; //flag 13
        }
        else
        {
            newMask += 1024;//flag 10
            newMask += 4096; //flag 12
            
        }

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
        justDied = true;
     
      
        ResetPlayerValues();
		transform.position = GameManagerScript.instance.deathbox.transform.position;

        

        // Check how many lives the player has first
        if (lives > 1)
        {

            // Choose one of the spawn locations at random
            int spawnPoint = Random.Range(0, 6);

            Vector3 spawnLocation = Vector3.zero;

			//spawnLocation = spawnPoints[spawnPoint].transform.position;

            // Call respawn after a player died (currently set to 0,0,0)
			//StartCoroutine(Respawn(spawnLocation));
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

    void SpawnRagdoll()
    {
        Physics.IgnoreLayerCollision(15, 15, true);
        prevPos.y -= 0.2f;
        GameObject ragdollClone = Instantiate(ragdoll, prevPos, transform.rotation, transform.parent);
        justDied = false;
        SetRagDollPos(model.transform.GetChild(0),ragdollClone.transform.GetChild(0));
    }

    void SetRagDollPos(Transform main, Transform rgPart)    
    {
        rgPart.localPosition = main.localPosition;
        rgPart.localRotation= main.localRotation;

        for(int i=0; i < rgPart.childCount; i++)
        {
            Transform mainChild = main.GetChild(i);
            Transform rgChild = rgPart.GetChild(i);

            if(mainChild!=null && rgChild != null)
                SetRagDollPos(mainChild, rgChild);

        }

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
