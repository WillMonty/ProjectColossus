using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColossusManager : MonoBehaviour {

    #region Robot Attributes
    const float STARTING_HEALTH = 1500.0f;
    private bool playerInBot; //Is the player currently in the colossus and ready to play?
    private float health;
	private bool gameEnded;

    // Components needed for the toggling of the colossus
    [Header("Colossus Components")]
    private GameObject leftController;
    private GameObject rightController;
    public GameObject headset;
    public GameObject pregameIndicator; //Gameobjects showing the player where to go to start the game
	public GameObject resultsCanvas;

	//Ability scripts
	List<ColossusAbility> chosenAbilities;

    [Header("Map")]
    public GameObject map;
    public float lowerMapAmount;
    public GameObject resistanceContainer;

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
    public float Health
    {
        get { return health; }
        
    }

    #endregion

    #region Start Method
    // Use this for initialization
    void Start()
    {
		//Get controllers
		leftController = this.GetComponent<SteamVR_ControllerManager>().left;
		rightController = this.GetComponent<SteamVR_ControllerManager>().right;

		//Set up abilities
		chosenAbilities = new List<ColossusAbility>();
		GetChosenAbilities();

		health = STARTING_HEALTH;

        StartCoroutine(LateStart(0.2f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Sets the instance after the game manager has actually initialized
        GameManagerScript.instance.colossus = this;
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
	private void GetChosenAbilities()
	{
		//Get head abilities
		switch(AbilityManagerScript.instance.headColossus)
		{
			case(ColossusHeadAbilities.Laser):
				chosenAbilities.Add(this.GetComponent<HeadLaserAbility>());
				break;
		}

		//Arm abilities
		//Hands
		if(AbilityManagerScript.instance.leftHandColossus == ColossusHandAbilities.Hand || AbilityManagerScript.instance.rightHandColossus == ColossusHandAbilities.Hand)
		{
			chosenAbilities.Add(this.GetComponent<HandsAbility>());	
		}

		//Shield
		if(AbilityManagerScript.instance.leftHandColossus == ColossusHandAbilities.Shield || AbilityManagerScript.instance.rightHandColossus == ColossusHandAbilities.Shield)
		{
			chosenAbilities.Add(this.GetComponent<ShieldsAbility>());	
		}
	}

    // Damage helper method
    public void Damage(float damageFloat)
    {
        health -= damageFloat;
		float healthPct = (STARTING_HEALTH - health)/STARTING_HEALTH;
		armHealthbar.value = healthPct;
		headHealthbar.value = healthPct;

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

		//Turn off hands
		this.GetComponent<HandsAbility>().Disable();

        //Enable Colossus abilities
		for(int i = 0; i < chosenAbilities.Count; i++)
		{
			chosenAbilities[i].Enable();
		}

		playerInBot = true;
		armHealthbar.gameObject.SetActive(true);
        headHealthbar.gameObject.SetActive(true);
		pregameIndicator.SetActive(false);

        LowerMap();

		GameManagerScript.instance.StartGame();

        //Non-debug only
		if (!GameManagerScript.instance.forceStartGame)
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
			foreach(ColossusAbility ability in chosenAbilities)
			{
				ability.Disable();
			}

			headSource.Stop();
			headSource.clip = deathSound;
			headSource.Play();
		}

		gameEnded = true;
	}
}
