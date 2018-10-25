﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;


// GameState Enum
public enum GameState { MainMenu, Instructions, CharacterSelect, GameCountdown, InGame, Paused, Pregame, ResistanceWin, ResistanceLose };


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
    public GameObject soldierCountdownUI;
    public GameObject soldierCoutdownTimer;

    public enum PauseOwner { Player1, Player2, Colossus, None}
    public PauseOwner currentPauseOwner = PauseOwner.None;
    private Dictionary<int, PauseOwner> playerNumToPauseOwner = new Dictionary<int, PauseOwner>()
    {
        { 0, PauseOwner.Colossus },
        { 1, PauseOwner.Player1},
        { 2, PauseOwner.Player2 }
    };

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
	}

    void Start ()
    {
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

		//Set game state
		string currScene = SceneManager.GetActiveScene().name;
		switch(currScene){
			case "MainMenu":
				currentGameState = GameState.MainMenu;
				break;
			case "MainGame":
				currentGameState = GameState.Pregame;
                break;	
		}
        
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
			else
			{
				StartGame();
            }
		}


		if ((currentGameState == GameState.Pregame || currentGameState == GameState.InGame) && !onlyVR)
        {
			//Set up resistance objects
            spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
            deathCam1 = GameObject.Find("DeathCam1");
            deathCam2 = GameObject.Find("DeathCam2");
            deathCam1.SetActive(false);
            deathCam2.SetActive(false);

			SpawnSoldiers();
        }

    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update ()
    {
		PauseInputs();
		CheckWinCondition();
        OOOOOOOF();

        Countdown();

		if (!soldierUICanvas.activeSelf && currentGameState == GameState.InGame)
        {
            soldierUICanvas.SetActive(true);
        }

        if(!soldierSelectMenu.activeSelf && currentGameState == GameState.CharacterSelect)
        {
            soldierSelectMenu.SetActive(true);
        }

        if (soldierSelectMenu.activeSelf && currentGameState != GameState.CharacterSelect)
        {
            soldierSelectMenu.SetActive(false);
        }
    }
    #endregion

	//General function to set up the game's pieces and state
	public void StartGame()
	{
		currentGameState = GameState.InGame;
		EnvironmentManagerScript.instance.GamePiecesSwitch();
    }

    //called from character select menu
    public void BeginGame()
    {
        currentGameState = GameState.GameCountdown;

        soldierCountdownUI.SetActive(true);

        EnvironmentManagerScript.instance.GamePiecesSwitch();
    }


    #region Pause and Play Game

    void PauseInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(0);
        }

        prevState1 = state1;
        state1 = GamePad.GetState((PlayerIndex)(0));

        prevState2 = state2;
        state2 = GamePad.GetState((PlayerIndex)(1));

        if(state1.Buttons.Start == ButtonState.Pressed && prevState1.Buttons.Start == ButtonState.Released)
        {
            TogglePause(1);
        }

        if(state2.Buttons.Start == ButtonState.Pressed && prevState2.Buttons.Start == ButtonState.Released)
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

            currentPauseOwner = PauseOwner.None;
        }
    }

    // Game Management Pausing and Resuming Methods
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        
        currentGameState = GameState.Paused;

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        currentGameState = GameState.InGame;

        Time.timeScale = 1;
    }

    /// <summary>
    /// Check to see who wins the game
    /// </summary>
    public void CheckWinCondition()
    {
        if(colossus != null && soldier1 != null && soldier2 != null)
        {
            if (currentGameState == GameState.InGame && colossus.Health <= 0)
            {
                currentGameState = GameState.ResistanceWin;
				//EnvironmentManagerScript.instance.
                StartCoroutine(ReturnToMainMenu(7f));
            }
            else if (currentGameState == GameState.InGame && soldier1.Lives <= 0 && soldier2.Lives <= 0)
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
    #endregion


    public void SpawnSoldiers()
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

        if(pNum==1)
            soldier1 = soldierClone.GetComponent<PlayerData>();
        else
            soldier2 = soldierClone.GetComponent<PlayerData>();
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

    void ExitGame()
    {
        // Last thing: Load the main menu
        SceneManager.LoadScene(0);
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

    void Countdown()
    {
        if(currentGameState == GameState.GameCountdown)
        {
            if(!soldierCountdownUI.activeSelf)
            {
                soldierCountdownUI.SetActive(true);
            }

            gameCountdownTimer -= Time.deltaTime;

            if(gameCountdownTimer > 0)
            {
                soldierCoutdownTimer.GetComponent<Text>().text = Mathf.Ceil(gameCountdownTimer).ToString();

                Color timerColor = soldierCoutdownTimer.GetComponent<Text>().color;

                timerColor.a = gameCountdownTimer % 1f;

                soldierCoutdownTimer.GetComponent<Text>().color = timerColor;
            }

            if(gameCountdownTimer <= 0)
            {
                soldierCountdownUI.SetActive(false);
                currentGameState = GameState.InGame;
                gameCountdownTimer = gameCountdownTimerDefault;
            }
        }
    }
}
