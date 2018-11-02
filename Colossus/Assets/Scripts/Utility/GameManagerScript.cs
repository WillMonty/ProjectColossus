﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;


// GameState Enum
public enum GameState { MainMenu, Instructions, CharacterSelect, Countdown, InGame, Paused, Pregame, ResistanceWin, ResistanceLose };


public class GameManagerScript : MonoBehaviour
{
    
    #region Attributes
    // Static instance of the GameManager which allows it to be accessed from any script
    public static GameManagerScript instance = null;

	[Header("Debug")]
	public bool forceStartGame;
	public bool onlyVR;

	[Header("State")]
	public GameState currentGameState;

	[Header("Players")]
    public ColossusManager colossus = null;
    public PlayerData soldier1 = null;
    public PlayerData soldier2 = null;

    [Header("UI and Pausing")]
	public GameObject soldierUICanvas;
    public GameObject soldierSelectMenu;
    public GameObject pauseMenu;
    public GameObject[] pauseMenuButtons;
    public int pauseMenuSelectedButton = 0;
    public GameObject soldierCountdownUI;
    public GameObject soldierCountdownTimer;

    public enum PauseOwner { Player1, Player2, Colossus, None}
    public PauseOwner currentPauseOwner = PauseOwner.None;
    private Dictionary<int, PauseOwner> playerNumToPauseOwner = new Dictionary<int, PauseOwner>()
    {
        { 0, PauseOwner.Colossus },
        { 1, PauseOwner.Player1},
        { 2, PauseOwner.Player2 }
    };
    public Color normalColor;
    public Color highlightedColor;

    //Player Inputs
    GamePadState state1;
    GamePadState prevState1;
    GamePadState state2;
    GamePadState prevState2;
    
    //Soldier properties
    private GameObject[] spawnPoints;
    GameObject deathCam1;
    GameObject deathCam2;

    float gameCountdownTimer;
    float gameCountdownTimerDefault = 3;

    #endregion

    #region Awake/Start
    void Awake ()
	{
		DontDestroyOnLoad(this);

		//Set up resistance objects
		spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
		soldierUICanvas = GameObject.Find("/ResistanceContainer/Soldier UI");
		soldierCountdownUI = GameObject.Find("/ResistanceContainer/Soldier UI/CountdownBackground");
		soldierCountdownTimer = GameObject.Find("/ResistanceContainer/Soldier UI/CountdownBackground/TimerBackground/Timer");
		deathCam1 = GameObject.Find("/ResistanceContainer/DeathCams/DeathCam1");
		deathCam2 = GameObject.Find("/ResistanceContainer/DeathCams/DeathCam2");
	}

    void Start ()
    {
        ControllerInput.SetUpControllers();

		//Activate normal monitor
		if(Display.displays.Length > 1)
		{
			Display.displays[1].Activate();	
		}

        currentPauseOwner = PauseOwner.None;

        #region Singleton Design Pattern
        // Check for an instance, if it does exist, than set to this
        if (instance == null)
        {
            instance = this;
        }
        else if(instance!=this) // If a new Gamemanger is loaded and it isn't the one that is loaded already than delete it
        {
			if (FindObjectsOfType(GetType()).Length > 1)
				Destroy(gameObject);
        }
        #endregion
        
        gameCountdownTimer = gameCountdownTimerDefault;

        StartCoroutine(LateStart(0.3f));
    }

	IEnumerator LateStart(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	
		//Check for debug
		if(forceStartGame)
		{
			if(colossus != null)
				colossus.ToggleColossus();

			if(!onlyVR) SoldierSetup();
			
			currentGameState = GameState.InGame;

			EnvironmentManagerScript.instance.GamePiecesSwitch();
		}

		//colossus.ToggleColossus();
    }
    #endregion

    // Update is called once per frame
    void FixedUpdate ()
    {
        OOOOOOOF();

        if (currentGameState == GameState.Paused)
        {
            CheckInputs();
            Pause();
        }

		if(currentGameState == GameState.Countdown)
        	Countdown();

        if (currentGameState == GameState.InGame)
        {
            CheckWinCondition();
            CheckInputs();
        }
    }

	#region State Handling

    //called from character select menu
    public void StartCountdown()
    {
		SoldierSetup();

		deathCam1.SetActive(false);
		deathCam2.SetActive(false);

        EnvironmentManagerScript.instance.GamePiecesSwitch();

		currentGameState = GameState.Countdown;
    }

	void Countdown()
	{
		if(!soldierCountdownUI.activeSelf)
		{
			soldierCountdownUI.SetActive(true);
			colossus.wallCanvas.SetCanvas("Countdown");
		}

		gameCountdownTimer -= Time.deltaTime;

		GameObject colossusTimer = colossus.wallCanvas.timer;

		if(gameCountdownTimer > 0)
		{
			soldierCountdownTimer.GetComponent<Text>().text = Mathf.Ceil(gameCountdownTimer).ToString();
			colossusTimer.GetComponent<Text>().text = Mathf.Ceil(gameCountdownTimer).ToString();

			Color timerColor = Color.black;

			timerColor.a = gameCountdownTimer % 1f;

			soldierCountdownTimer.GetComponent<Text>().color = timerColor;
			colossusTimer.GetComponent<Text>().color = timerColor;
		}

		if(gameCountdownTimer <= 0)
		{
			soldierCountdownUI.SetActive(false);
			colossus.wallCanvas.Clear();
			currentGameState = GameState.InGame;
			gameCountdownTimer = gameCountdownTimerDefault;
		}
	}

    /// <summary>
    /// Check to see who wins the game
    /// </summary>
    public void CheckWinCondition()
    {
        if(colossus != null && soldier1 != null && soldier2 != null)
        {
            if (colossus.Health <= 0)
            {
                currentGameState = GameState.ResistanceWin;
				colossus.KillColossus();
                StartCoroutine(ReturnToMainMenu(7f));
            }
            else if (soldier1.Lives <= 0 && soldier2.Lives <= 0)
            {
                currentGameState = GameState.ResistanceLose;
                StartCoroutine(ReturnToMainMenu(7f));
            }
        }
    }

    IEnumerator ReturnToMainMenu(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameManagerScript.instance.currentGameState = GameState.MainMenu;

        // Last thing: Load the main menu
        SceneManager.LoadScene(0);
    }

	/// <summary>
	/// Emergency Restart for the game
	/// </summary>
	void OOOOOOOF()
	{
		//Input based debug
		if(Input.GetKeyDown(KeyCode.Slash))
		{
			colossus.DamageObject(99999f);
		}

		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			currentGameState = GameState.MainMenu;
			SceneManager.LoadScene(0);
		}
	}

	#endregion

	#region Soldier Handling
	void SoldierSetup()
	{
		CreateSoldier(1, AbilityManagerScript.instance.soldier1);
		soldier1.playerNumber = 1;

		CreateSoldier(2, AbilityManagerScript.instance.soldier2);
		soldier2.playerNumber = 2;
	}
    
    void CreateSoldier(int pNum, SoldierClass sClass)
    {
        GameObject soldierClone = null;
        GameObject classPrefab = null;
        Vector3 spawnPos;

        switch (sClass)
        {
            case SoldierClass.Assault:
                classPrefab = Resources.Load<GameObject>("Soldier/Classes/Assault");
                break;
            case SoldierClass.Grenadier:
                classPrefab = Resources.Load<GameObject>("Soldier/Classes/Grenadier");
                break;
            case SoldierClass.Skulker:
                classPrefab = Resources.Load<GameObject>("Soldier/Classes/Skulker");
                break;
        }


        if (pNum == 1)
            spawnPos = GameObject.Find("InitialSpawn1").transform.position;
        else
			spawnPos = GameObject.Find("InitialSpawn2").transform.position;


		soldierClone = Instantiate(classPrefab, spawnPos, Quaternion.Euler(0f, 180f, 0f), GameObject.Find("ResistanceContainer").transform);

        if(pNum == 1)
            soldier1 = soldierClone.GetComponent<PlayerData>();
        else
            soldier2 = soldierClone.GetComponent<PlayerData>();
    }

	//Disable a soldier, activate it's deathcam and respawn if necessary
	public void KillSoldier(int pNum)
	{
		PlayerData soldierClone;
		if (pNum == 1)
			soldierClone = soldier1;
		else
			soldierClone = soldier2;


		soldierClone.gameObject.SetActive(false);

		SetDeathCam();

		if (soldierClone.Lives > 0)
			StartCoroutine(RespawnSoldier(5, soldierClone));
	}

	IEnumerator RespawnSoldier(float time, PlayerData soldierClone)
	{
		yield return new WaitForSeconds(time);

		//Activate soldier, deactivate deathcam
		soldierClone.gameObject.SetActive(true);
		soldierClone.Alive = true;

		SetDeathCam();
		//Set soldier position to random spawnpoint
		soldierClone.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

	}

	void SetDeathCam()
	{
		Rect temp;

		if(soldier1.Alive)
			deathCam1.SetActive(false);
		else
			deathCam1.SetActive(true);

		if (soldier2.Alive)
			deathCam2.SetActive(false);
		else
			deathCam2.SetActive(true);

		if (!soldier1.Alive && !soldier2.Alive)
		{
			deathCam2.SetActive(false);
			temp = deathCam1.GetComponent<Camera>().rect;
			temp.width = 1.0f;
			deathCam1.GetComponent<Camera>().rect = temp;
		}
		else
		{
			temp = deathCam1.GetComponent<Camera>().rect;
			temp.width = 0.5f;
			deathCam1.GetComponent<Camera>().rect = temp;
		}
	}

	#endregion

	#region Pause and Play Game
    

	void Pause()
	{
		if(currentPauseOwner == PauseOwner.Player1)
		{
			if(ControllerInput.controllers[0].Down == 1)
			{
				int prevButton = pauseMenuSelectedButton;

				pauseMenuSelectedButton = (pauseMenuSelectedButton + 1) % pauseMenuButtons.Length;

				pauseMenuButtons[pauseMenuSelectedButton].GetComponent<Image>().color = highlightedColor;
				pauseMenuButtons[prevButton].GetComponent<Image>().color = normalColor;
			}
			else if(ControllerInput.controllers[0].Up == 1)
			{
				int prevButton = pauseMenuSelectedButton;

				pauseMenuSelectedButton = (pauseMenuSelectedButton + pauseMenuButtons.Length - 1) % pauseMenuButtons.Length;

				pauseMenuButtons[pauseMenuSelectedButton].GetComponent<Image>().color = highlightedColor;
				pauseMenuButtons[prevButton].GetComponent<Image>().color = normalColor;
			}
			else if(ControllerInput.controllers[0].A == 1)
			{
				pauseMenuButtons[pauseMenuSelectedButton].GetComponent<Button>().onClick.Invoke();
			}
		}
		else if(currentPauseOwner == PauseOwner.Player2)
		{
			if (ControllerInput.controllers[1].Down == 1)
			{
				int prevButton = pauseMenuSelectedButton;

				pauseMenuSelectedButton = (pauseMenuSelectedButton + 1) % pauseMenuButtons.Length;

				pauseMenuButtons[pauseMenuSelectedButton].GetComponent<Image>().color = highlightedColor;
				pauseMenuButtons[prevButton].GetComponent<Image>().color = normalColor;
			}
			else if (ControllerInput.controllers[1].Up == 1)
			{
				int prevButton = pauseMenuSelectedButton;

				pauseMenuSelectedButton = (pauseMenuSelectedButton + pauseMenuButtons.Length - 1) % pauseMenuButtons.Length;

				pauseMenuButtons[pauseMenuSelectedButton].GetComponent<Image>().color = highlightedColor;
				pauseMenuButtons[prevButton].GetComponent<Image>().color = normalColor;
			}
			else if (ControllerInput.controllers[1].A == 1)
			{
				pauseMenuButtons[pauseMenuSelectedButton].GetComponent<Button>().onClick.Invoke();
			}
		}
	}

	void CheckInputs()
	{
        ControllerInput.UpdateControllers();

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePause(0);
		}
        
		if(ControllerInput.controllers[0].Start == 1)
		{
			TogglePause(1);
		}

		if(ControllerInput.controllers[1].Start == 1)
		{
			TogglePause(2);
		}
	}

	public void TogglePause(int playerNum)
	{
		if (currentGameState == GameState.InGame)
		{
			PauseGame();

			currentPauseOwner = playerNumToPauseOwner[playerNum];
		}

		else if(currentGameState == GameState.Paused 
			&& playerNumToPauseOwner[playerNum] == currentPauseOwner)
		{
			ResumeGame();
		}
	}

	// Game Management Pausing and Resuming Methods
	void PauseGame()
	{
		pauseMenu.SetActive(true);

		currentGameState = GameState.Paused;

		pauseMenuButtons[0].GetComponent<Image>().color = highlightedColor;

		pauseMenuSelectedButton = 0;

		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		pauseMenu.SetActive(false);

		currentGameState = GameState.InGame;

		currentPauseOwner = PauseOwner.None;

		pauseMenuButtons[pauseMenuSelectedButton].GetComponent<Image>().color = normalColor;

		Time.timeScale = 1;
	}

	#endregion  
}
