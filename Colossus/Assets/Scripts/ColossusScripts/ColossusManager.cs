using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColossusManager : MonoBehaviour, IHealth {

    #region Robot Attributes
    const float STARTING_HEALTH = 1500.0f;
    float health;
	public float outOfBoundsDamage;

	//Abilities being used
	List<ColossusAbility> chosenAbilities;

    // Components needed for the toggling of the colossus
    GameObject leftController;
    GameObject rightController;
	[Header("Colossus Components")]
    public GameObject headset;
	public RootMotion.FinalIK.VRIK ikColossus;
    public GameObject positionIndicator; //Gameobjects showing the player where to go to start the game
	public GameObject resultsCanvas;

    [Header("Environment")]
    public GameObject map;
    public float lowerMapAmount;
    public GameObject resistanceContainer;

    [Header("Audio")]
    public AudioSource headSource;
	public AudioSource alarmSource;
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

	bool fixedIK;

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
		GameState currentGameState = GameManagerScript.instance.currentGameState;
		if (currentGameState == GameState.InGame)
		{
			CheckInBounds();
			return;
		}

		CheckHopIn(); //Check if the player is jumping in the bot to start the game	

		if(currentGameState == GameState.ResistanceWin)
		{
			resultsCanvas.SetActive(true);
			resultsCanvas.transform.GetChild(2).gameObject.SetActive(true);
			KillColossus();
		}

		if(currentGameState == GameState.ResistanceLose)
		{
			resultsCanvas.SetActive(true);
			resultsCanvas.transform.GetChild(1).gameObject.SetActive(true);
		}

		//IK arm fix?
		if(!fixedIK && leftController.GetComponent<SteamVR_TrackedController>().menuPressed)
		{
			fixedIK = true;
			Debug.Log("Trying IK");
			ikColossus.GetIKSolver().Initiate(ikColossus.GetIKSolver().GetRoot());
		}
    }
    #endregion

    #region Helper Methods
	void GetChosenAbilities()
	{
		//Get head abilities
		switch(AbilityManagerScript.instance.headColossus)
		{
			case(ColossusHeadAbilities.Laser):
				chosenAbilities.Add(this.GetComponent<HeadLaserAbility>());
				break;
		}

		//Arm abilities
		ColossusHandAbilities leftHand = AbilityManagerScript.instance.leftHandColossus;
		ColossusHandAbilities rightHand = AbilityManagerScript.instance.rightHandColossus;

		//Fists
		if(leftHand == ColossusHandAbilities.Fist || rightHand == ColossusHandAbilities.Fist)
		{
			chosenAbilities.Add(this.GetComponent<FistsAbility>());	
		}

		//Hands
		if(leftHand == ColossusHandAbilities.Hand || rightHand == ColossusHandAbilities.Hand)
		{
			chosenAbilities.Add(this.GetComponent<HandsAbility>());	
		}

		//Shield
		if(leftHand == ColossusHandAbilities.Shield || rightHand == ColossusHandAbilities.Shield)
		{
			chosenAbilities.Add(this.GetComponent<ShieldsAbility>());	
		}
	}

    // Damage helper method
    public void DamageObject(float damage)
    {
        health -= damage;
		float healthPct = (STARTING_HEALTH - health)/STARTING_HEALTH;
		armHealthbar.value = healthPct;
		headHealthbar.value = healthPct;

    }

    /// <summary>
    /// Helper method to check if the player is "hopping into" the colossus
    /// </summary>
    void CheckHopIn()
    {
        //Check if the colossus is in the pillar trigger
		if(positionIndicator.GetComponent<ColossusPositionTrigger>().ColossusInTrigger)
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
	/// Checks to see if the player is off the pillar or not during the game
	/// </summary>
	void CheckInBounds()
	{
		if(positionIndicator.GetComponent<ColossusPositionTrigger>().ColossusInTrigger)
		{
			alarmSource.Stop();
		}
		else
		{
			DamageObject(outOfBoundsDamage);
			if(!headSource.isPlaying)
			{
				if(GameManagerScript.instance.forceStartGame) //Don't play the sound when debugging
					return;
				
				alarmSource.Play();
			}
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
		positionIndicator.transform.GetChild(0).gameObject.SetActive(false); //Turn off green position cube

		//Turn off starter hands
		this.GetComponent<HandsAbility>().Disable();

        //Enable Colossus abilities
		for(int i = 0; i < chosenAbilities.Count; i++)
		{
			chosenAbilities[i].Enable();
		}
			
		armHealthbar.gameObject.SetActive(true);
        headHealthbar.gameObject.SetActive(true);

        LowerMap();

		GameManagerScript.instance.StartGame();

        //Non-debug only
		if (!GameManagerScript.instance.forceStartGame)
        {
            headSource.clip = hopInSound;
            headSource.Play();
        }
    }

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
		foreach(ColossusAbility ability in chosenAbilities)
		{
			ability.Disable();
		}

		headSource.Stop();
		headSource.clip = deathSound;
		headSource.Play();
	}

	#endregion
}
