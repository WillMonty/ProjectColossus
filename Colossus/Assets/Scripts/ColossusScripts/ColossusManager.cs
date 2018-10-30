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
	public GameObject resultsCanvas;

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
	public GameObject positionIndicator; //Gameobject tracking the colossus body position

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
		leftController = GetComponent<SteamVR_ControllerManager>().left;
		rightController = GetComponent<SteamVR_ControllerManager>().right;

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

		RefreshTrackedControllers();
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
			//Check on canvas?

            //Check if either trigger is pressed to hop in
            bool leftTriggerPressed = leftController.GetComponent<SteamVR_TrackedController>().triggerPressed;
            bool rightTriggerPressed = rightController.GetComponent<SteamVR_TrackedController>().triggerPressed;

            if (leftTriggerPressed || rightTriggerPressed) ToggleColossus();
        }
        else
        {
			//X on canvas?
        }
    }

	/// <summary>
	/// Checks to see if the player is off the pillar or not during the game
	/// </summary>
	void CheckInBounds()
	{
		//Don't bother with bounds if debugging
		if(positionIndicator.GetComponent<ColossusPositionTrigger>().ColossusInTrigger || GameManagerScript.instance.forceStartGame)
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

	public void RefreshTrackedControllers()
	{
		ColossusAbility.leftControllerTracked = GetComponent<SteamVR_ControllerManager>().left.GetComponent<SteamVR_TrackedController>();
		ColossusAbility.rightControllerTracked = GetComponent<SteamVR_ControllerManager>().right.GetComponent<SteamVR_TrackedController>();
	}

    /// <summary>
    /// Method to toggle the player into the colossus
    /// </summary>
    public void ToggleColossus()
    {
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

		//Drop the map to accomadate player height
		EnvironmentManagerScript.instance.LowerMap(headset.transform.position.y);

        //Non-debug only
		if (!GameManagerScript.instance.forceStartGame)
        {
			GameManagerScript.instance.StartCountdown();
            headSource.clip = hopInSound;
            headSource.Play();
        }
		else
		{
			GameManagerScript.instance.StartGame();
		}
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
