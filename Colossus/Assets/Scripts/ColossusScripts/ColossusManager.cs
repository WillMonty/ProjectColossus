using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColossusManager : MonoBehaviour {

    #region Robot Attributes
    //Debug variable to allow the game to start with the game running
    public bool debugColossus;

    const float STARTING_HEALTH = 1500.0f;
    private bool playerInBot; //Is the player currently in the colossus and ready to play?
    private float health;
	private bool gameEnded;

    // Components needed for the toggling of the colossus
    [Header("Colossus Components")]
    public GameObject leftController;
    public GameObject rightController;
    public GameObject headset;
    public GameObject pregameIndicator; //Gameobjects showing the player where to go to start the game
	public GameObject resultsCanvas;
    Laser laser;

	//Objects dealing with the environment
    [Header("Map")]
    public GameObject map;
    public float lowerMapAmount;
    public GameObject lava;
    public GameObject resistanceContainer;
	public GameObject conveyors;


    //Audio
    [Header("Audio")]
    public AudioSource headSource;
    public AudioClip hopInSound;
    public AudioClip damageSound;
	public AudioClip deathSound;	

    [Header("UI")]
    public Slider armHealthbar;
    public Slider headHealthbar;

    [Header("Colossus Positioning")]
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    public Material inPositionMat;
    public Material outPositionMat;

    #endregion

    #region Properties
    // Properties

    /// <summary>
    /// Could be used for a boss health GUI element for soldiers
    /// </summary>
    public float Health
    {
        get { return health; }
        
    }

    #endregion

    #region Start Method
    // Use this for initialization
    void Start()
    {
        health = STARTING_HEALTH;

        laser = gameObject.GetComponent<Laser>();
        StartCoroutine(LateStart(0.2f));

    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Sets the instance after the game manager has actually initialized
        GameManagerScript.instance.colossus = this;

		if (debugColossus) ToggleColossus(); //If in debug mode let the VR player start immediately in the colossus
    }

    #endregion

    #region Update Method
    // Update is called once per frame
    void Update ()
    {
        if (!playerInBot) CheckHopIn(); //Check if the player is jumping in the bot

		if(GameManagerScript.instance.currentGameState == GameState.ResistanceWin)
		{
			resultsCanvas.SetActive(true);
			resultsCanvas.transform.GetChild(2).gameObject.SetActive(true);
			KillColossus();
		}

		if(GameManagerScript.instance.currentGameState == GameState.ResistanceLose)
		{
			resultsCanvas.SetActive(true);
			resultsCanvas.transform.GetChild(1).gameObject.SetActive(true);
			KillColossus();
		}
    }
    #endregion

    #region Helper Methods
    // Damage helper method
    public void Damage(float damageFloat)
    {
        health -= damageFloat;
        armHealthbar.value = (STARTING_HEALTH - health)/STARTING_HEALTH;
        headHealthbar.value = (STARTING_HEALTH - health) / STARTING_HEALTH;

    }

    /// <summary>
    /// Helper method to check if the player is "hopping into" the colossus
    /// </summary>
    private void CheckHopIn()
    {
        //Check if the headset is in the collider
		bool headsetIn = pregameIndicator.GetComponent<ColossusPositionTrigger>().HeadsetInTrigger;
        if(headsetIn)
        {
            //Make the indicators green
            leftIndicator.GetComponent<MeshRenderer>().material = inPositionMat;
            rightIndicator.GetComponent<MeshRenderer>().material = inPositionMat;

            //Check if either trigger is pressed to hop in
            bool leftTriggerPressed = leftController.GetComponent<SteamVR_TrackedController>().triggerPressed;
            bool rightTriggerPressed = rightController.GetComponent<SteamVR_TrackedController>().triggerPressed;

            if (leftTriggerPressed || rightTriggerPressed) ToggleColossus();
        }
        else
        {
            //Make the indicators red
            leftIndicator.GetComponent<MeshRenderer>().material = outPositionMat;
            rightIndicator.GetComponent<MeshRenderer>().material = outPositionMat;
        }
    }

    /// <summary>
    /// Method to toggle the player into the colossus
    /// </summary>
    public void ToggleColossus()
    {
        //Turn off indicators
        leftIndicator.SetActive(false);
        rightIndicator.SetActive(false);

		//Turn off base dummy hand prefab
		leftController.transform.GetChild(1).gameObject.SetActive(false);
		rightController.transform.GetChild(1).gameObject.SetActive(false);

        //Enable regular hands
		leftController.transform.GetChild(0).gameObject.SetActive(true);
		rightController.transform.GetChild(0).gameObject.SetActive(true);

        //Enable Colossus
		playerInBot = true;
        laser.enabled = true;
		armHealthbar.gameObject.SetActive(true);
        headHealthbar.gameObject.SetActive(true);

		pregameIndicator.SetActive(false);
        //lava.SetActive(true);
		//conveyors.SetActive(true);

        LowerMap();

		GameManagerScript.instance.StartGame();

        //Non-debug only
        if (!debugColossus)
        {
            headSource.clip = hopInSound;
            headSource.Play();
        }
    }

    #endregion

    void LowerMap()
    {
        float playerY = headset.transform.position.y - lowerMapAmount;
        map.transform.position = new Vector3(map.transform.position.x, playerY, map.transform.position.z);

		//Don't bother if there aren't resistance in the scene
		if(resistanceContainer != null)
        	resistanceContainer.transform.position = new Vector3(resistanceContainer.transform.position.x, playerY + resistanceContainer.transform.position.y, resistanceContainer.transform.position.z);
    }

	void KillColossus()
	{
		if(!gameEnded)
		{
            laser.StopLaser();
            laser.enabled = false;

			headSource.Stop();
			headSource.clip = deathSound;
			headSource.Play();
		}

		gameEnded = true;
	}
}
