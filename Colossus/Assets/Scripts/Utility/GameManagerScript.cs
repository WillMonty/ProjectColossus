﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;


// GameState Enum
public enum GameState { MainMenu, Instructions, CharacterSelect, InGame, Paused, Pregame, ResistanceWin, ResistanceLose };


public class GameManagerScript : MonoBehaviour
{
    
    #region Attributes
    // Static instance of the GameManager which allows it to be accessed from any script
    public static GameManagerScript instance = null;

	[Header("Debug")]
	public bool forceStartGame;

	[Header("State")]
	public GameState currentGameState;

	[Header("Players")]
    public ColossusManager colossus = null;

    public PlayerData soldier1 = null;
    public PlayerData soldier2 = null;

	[Header("Game Pieces")]
	public List<GameObject> gamePieces = new List<GameObject>(); //Pieces of the scene only shown/used InGame

	[Header("UI and Pausing")]
	public GameObject soldierUICanvas;
    public GameObject pauseMenu;
    public GameObject pauseMenuDefaultButton;

    public enum PauseOwner { Player1, Player2, Colossus, None}
    public PauseOwner currentPauseOwner;
    private Dictionary<int, PauseOwner> playerNumToPauseOwner = new Dictionary<int, PauseOwner>()
    {
        { 0, PauseOwner.Colossus },
        { 1, PauseOwner.Player1},
        { 2, PauseOwner.Player2 }
    };

    GamePadState state1;
    GamePadState prevState1;

    GamePadState state2;
    GamePadState prevState2;

	public GameObject deathbox;

    

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
				//Method to spawn and sets player number of soldiers
				break;	
		}

		StartCoroutine(LateStart(0.2f));
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
				currentGameState = GameState.InGame;
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

		if (!soldierUICanvas.activeSelf && currentGameState == GameState.InGame)
        {
            EnableSoldierUI();
        }
	}
    #endregion

	//General function to set up the game's pieces and state
	public void StartGame()
	{
		GamePiecesSwitch();
		currentGameState = GameState.InGame;
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
        if (instance.currentGameState == GameState.InGame)
        {
            PauseGame();

            instance.currentPauseOwner = playerNumToPauseOwner[playerNum];
        }

        else if(instance.currentGameState == GameState.Paused 
                && playerNumToPauseOwner[playerNum] == instance.currentPauseOwner)
        {
            ResumeGame();

            instance.currentPauseOwner = PauseOwner.None;
        }
    }

    // Game Management Pausing and Resuming Methods
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        
        instance.currentGameState = GameState.Paused;

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        instance.currentGameState = GameState.InGame;

        Time.timeScale = 1;
    }

	/// <summary>
	/// Turns objects present in the gamePieces array on or off
	/// </summary>
	public void GamePiecesSwitch()
	{
		for(int i = 0; i < gamePieces.Count; i++)
		{
			if(gamePieces[i] != null)
			{
				gamePieces[i].SetActive(!gamePieces[i].gameObject.activeSelf);
			}
		}
	}

    /// <summary>
    /// Check to see who wins the game
    /// </summary>
    public void CheckWinCondition()
    {
        if(colossus != null && soldier1 != null && soldier2 != null)
        {
            if (instance.currentGameState == GameState.InGame && colossus.Health <= 0)
            {
                instance.currentGameState = GameState.ResistanceWin;
                StartCoroutine(ReturnToMainMenu(7f));
            }
            else if (instance.currentGameState == GameState.InGame && soldier1.Lives <= 0 && soldier2.Lives <= 0)
            {
                instance.currentGameState = GameState.ResistanceLose;
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

    
    public void SetSoldier1Class(int indexIn)
    {
        switch(indexIn)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    public void SetSoldier2Class(int indexIn)
    {
        switch (indexIn)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    /// <summary>
    /// Emergency Restart for the game
    /// </summary>
    public void OOOOOOOF()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
			currentGameState = GameState.MainMenu;
            SceneManager.LoadScene(0);
        }
    }

    public void EnableSoldierUI()
    {
        soldierUICanvas.SetActive(true);
    }

    public void ExitGame()
    {
        // Last thing: Load the main menu
        SceneManager.LoadScene(0);
    }
}