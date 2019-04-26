using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour, IHealth
{
    // Player Manager
    const int STARTING_LIVES = 1;
    const float RESPAWN_TIME = 5.0f;
    const float DAMAGING_OBJECT_MAGNITUDE = .5f;
    float MAX_HEALTH = 200.0f;
    

    // Basic Player Management variables
    private int lives;
    public float health;
    private float healthPrev;
    

    public int playerNumber;
    public SoldierClass soldierClass;
    public GameObject eyes;
    public GameObject gun;
    public GameObject model;
    public GameObject ragdoll;

    public Slider healthUI;
    IWeapon weaponData;
    public IWeapon WeaponData
    {
        get { return weaponData; }
    }
   

    bool alive = true;
    public bool Alive
    {
        get { return alive; }
        set { alive = value; }
    }

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
        healthUI.value = health / MaxHealth;
        if (health <= 0)
        {
            Death();
        }
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
        // Start initializes 3 lives at start, can be changed later

        lives = STARTING_LIVES;
		health = MAX_HEALTH;


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

		//Factor health by game speed
		if(GameManagerScript.instance.gameSpeedMod > 1.0f)
		{
			MAX_HEALTH = MAX_HEALTH - (MAX_HEALTH * 0.5f) * (GameManagerScript.instance.gameSpeedMod - 1);
			health = MAX_HEALTH;	
		}

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
    void FixedUpdate()
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

        SpawnRagdoll(transform.position);
        
        ResetPlayerValues();

        // Lower the life count
        lives--;
        alive = false;

        GameManagerScript.instance.KillSoldier(playerNumber);
    }

    /// <summary>
    /// Damage method to damage the player
    /// </summary>
    /// <param name="damageFloat"></param>
    public void Damage(float damageFloat)
    {

        health -= damageFloat;
        healthUI.value = health / MaxHealth;
        if (health <= 0)
        {
            Death();
        }
    }


    /// <summary>
    /// Method to reset player values
    /// </summary>
    void ResetPlayerValues()
    {
        health = MAX_HEALTH;
    }

    void SpawnRagdoll(Vector3 pos)
    {
        GameObject ragdollClone = Instantiate(ragdoll, pos, transform.rotation,transform.parent);
        
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

    private void OnEnable()
    {
        alive = true;
    }

	void OnCollisionEnter(Collision collision)
	{
		GameObject collisionObject = collision.gameObject;
		// If a player is hit with an object the robot throws
		if (collisionObject.tag == "throwable")
		{
			// Only damage the player if the object is moveing at a high velocity (number can be determined and changed through the player constant)
			if (collisionObject.GetComponent<Rigidbody>().velocity.magnitude > DAMAGING_OBJECT_MAGNITUDE)
			{
				float movingObjectDamage = collisionObject.GetComponent<Rigidbody>().velocity.magnitude;
				DamageObject(movingObjectDamage);
			}
			else if(collisionObject.GetComponent<Rigidbody>().isKinematic)
			{
				float movingObjectDamage =collisionObject.GetComponent<Valve.VR.InteractionSystem.VelocityEstimator>().GetVelocityEstimate().magnitude;
				if(movingObjectDamage<=0.0f)
					movingObjectDamage=15f;
				DamageObject(movingObjectDamage);
			}
		}
	}

}
