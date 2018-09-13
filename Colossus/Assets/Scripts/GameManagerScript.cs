using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// GameState Enum
public enum GameState { MainMenu, Instructions, InGame, Paused, Pregame, ResistanceWin, ResistanceLose };


public class GameManagerScript : MonoBehaviour
{
    
    #region Attributes
    // Static instance of the GameManager which allows it to be accessed from any script
    public static GameManagerScript instance = null;
    public ColossusManager colossus = null;
    public PlayerManager soldier1 = null;
    public PlayerManager soldier2 = null;
	public GameObject deathbox;

    public GameObject soldierUI;
    public GameObject pauseMenu;

    List<GameObject> escapeScreens;
    public GameState currentGameState;

    public enum PauseOwner { Player1, Player2, Colossus, None}
    public PauseOwner currentPauseOwner;

    #endregion

    #region Start
    // Use this for initialization
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
            // The gamestate should be set as mainmenu originally
            currentGameState = GameState.MainMenu;

            instance = this;
        }
        else if(instance!=this) // If a new Gamemanger is loaded and it isn't the one that is loaded already than delete it
        {
            Destroy(gameObject);
        }
        #endregion
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update ()
    {
		CheckWinCondition();
		/*for(int i = 0; i < Input.GetJoystickNames().Length; i++)
		{
			Debug.Log(Input.GetJoystickNames()[i]);	
		}*/

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        OOOOOOOF();

	}
    #endregion

    #region Pause and Play Game

    void TogglePause()
    {
        if (instance.currentGameState == GameState.InGame)
        {
            PauseGame();

            //set pause owner here
        }

        else if(instance.currentGameState == GameState.Paused)
        {
            ResumeGame();

            currentPauseOwner = PauseOwner.None;
        }
    }

    // Game Management Pausing and Resuming Methods
    public void PauseGame()
    {
        //for (int i = 0; i < escapeScreens.Count; i++)
        //{
        //    escapeScreens[i].SetActive(true);
        //}

        pauseMenu.SetActive(true);
        
        instance.currentGameState = GameState.Paused;
        
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        //for (int i = 0; i < escapeScreens.Count; i++)
        //{
        //    escapeScreens[i].SetActive(false);
        //}

        pauseMenu.SetActive(false);

        instance.currentGameState = GameState.InGame;

        Time.timeScale = 1;
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

    /// <summary>
    /// Emergency Restart for the game
    /// </summary>
    public void OOOOOOOF()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void ExitGame()
    {
        // Last thing: Load the main menu
        SceneManager.LoadScene(0);
    }
}
