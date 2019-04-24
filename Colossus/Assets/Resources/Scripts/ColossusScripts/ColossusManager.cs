using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColossusManager : MonoBehaviour, IHealth {

    #region Robot Attributes
	float STARTING_HEALTH = 1500.0f;
    float health;

	//Abilities being used
	List<ColossusAbility> chosenAbilities;

    // Components needed for the toggling of the colossus
	[Header("Colossus Components")]
    public GameObject headset;
	public GameObject body;
	public RootMotion.FinalIK.VRIK ikColossus;

    [Header("Audio")]
	public bool noSound;
    public AudioSource headSource;
	public AudioSource alarmSource;
    public AudioClip hopInSound;
    public AudioClip damageSound;
	public AudioClip deathSound;

    [Header("UI")]
	public ColossusCanvas wallCanvas;
    public Slider armHealthbar;
    public Slider headHealthbar;
	public GameObject lavaText;

    [Header("Colossus Positioning")]
	public GameObject positionIndicator; //Gameobject tracking the colossus body position
	public float outOfBoundsDamage;

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

		//Factor health by game speed
		if(GameManagerScript.instance.gameSpeedMod > 1.0f)
		{
			STARTING_HEALTH = STARTING_HEALTH - (STARTING_HEALTH * 0.5f) * (GameManagerScript.instance.gameSpeedMod - 1);
			health = STARTING_HEALTH;	
		}
		RefreshTrackedControllers();

		body.SetActive(true); //Enable ik late
    }

    #endregion

    #region Update Method
    // Update is called once per frame
    void Update ()
    {
		GameState currentGameState = GameManagerScript.instance.currentGameState;
		if(currentGameState == GameState.Pregame)
		{
			CheckHopIn(); //Check if the player is jumping in the bot to start the game		
		}

		if (currentGameState == GameState.InGame)
		{
			CheckInBounds();
		}

		if(currentGameState == GameState.ResistanceWin)
		{
			wallCanvas.SetCanvas("Lose");
		}

		if(currentGameState == GameState.ResistanceLose)
		{
			wallCanvas.SetCanvas("Win");
		}

		//Stop colossus audio if desired
		if(noSound)
		{
			headSource.Stop();
			alarmSource.Stop();
		}
    }
    #endregion

    #region Helper Methods
	void GetChosenAbilities()
	{
		//Get head ability
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
		float healthPct = health/STARTING_HEALTH;
		armHealthbar.value = healthPct;
		headHealthbar.value = healthPct;
		if(healthPct < 0.1f)
		{
			EnvironmentManagerScript.instance.PlayAnnouncer("ColossusCriticalHealth");
		}

    }

    /// <summary>
    /// Helper method to check if the player is "hopping into" the colossus
    /// </summary>
    void CheckHopIn()
    {
        //Check if the colossus is in the pillar trigger
		if(positionIndicator.GetComponent<ColossusPositionTrigger>().ColossusInTrigger)
        {
			wallCanvas.SetCanvas("Inbounds");

            //Check if either trigger is pressed to hop in
			bool leftTriggerPressed = ColossusAbility.leftControllerTracked.triggerPressed;
			bool rightTriggerPressed = ColossusAbility.rightControllerTracked.triggerPressed;

            if (leftTriggerPressed || rightTriggerPressed)
			{
				wallCanvas.Clear();
				ToggleColossus();	
			}
        }
        else
        {
			wallCanvas.SetCanvas("Outbounds");
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
			lavaText.SetActive(false);
		}
		else
		{
			DamageObject(outOfBoundsDamage);
			if(!alarmSource.isPlaying)
			{
				if(GameManagerScript.instance.forceStartGame) //Don't play the sound when debugging
					return;
				alarmSource.Play();
			}
			lavaText.SetActive(true);
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
		EnvironmentManagerScript.instance.MoveMap(headset.transform.position);

		wallCanvas.Clear();

        //Start game
		if (!GameManagerScript.instance.forceStartGame)
        {
			GameManagerScript.instance.StartCountdown();
			headSource.clip = hopInSound;
			headSource.Play();
        }


    }

	public void KillColossus()
	{
		foreach(ColossusAbility ability in chosenAbilities)
		{
			ability.Disable();
		}

		headSource.Stop();
		headSource.clip = deathSound;
		headSource.Play();
		StartCoroutine(AnnounceDeath());
	}

	private IEnumerator AnnounceDeath()
	{
		yield return new WaitForSeconds(3f);

		EnvironmentManagerScript.instance.PlayAnnouncer("ColossusDestroyed");
	}

	#endregion
}
