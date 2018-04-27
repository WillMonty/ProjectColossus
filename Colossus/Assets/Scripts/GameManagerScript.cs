using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// GameState Enum
public enum GameState { MainMenu, Instructions, InGame, Paused, Pregame, ResistanceWin, ResistanceLose };


public class GameManagerScript : MonoBehaviour
{
    
    #region Attributes
    // Static instance of the GameManager which allows it to be accessed from any script
    public static GameManagerScript instance = null;
    public ColossusManager colossus =null;
    public PlayerManager soldier1 = null;
    public PlayerManager soldier2 = null;

    List<GameObject> escapeScreens;
    public GameState currentGameState;

    #endregion

    #region Start
    // Use this for initialization
    void Start ()
    {
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
        
		//Debug.Log(UnityEngine.Input.GetJoystickNames());
	}
    #endregion

    #region Pause and Play Game
    // Game Management Pausing and Resuming Methods
    public void PauseGame()
    {
        if (instance.currentGameState == GameState.InGame)
        {
            for (int i = 0; i < escapeScreens.Count; i++)
            {
                escapeScreens[i].SetActive(true);
            }
            instance.currentGameState = GameState.Paused;
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        if (instance.currentGameState == GameState.Paused)
        {
            for (int i = 0; i < escapeScreens.Count; i++)
            {
                escapeScreens[i].SetActive(false);
            }
            instance.currentGameState = GameState.InGame;
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Check to see who wins the game
    /// </summary>
    public void CheckWinCondition()
    {
        if(instance.currentGameState == GameState.InGame && colossus.Health == 0)
        {
            soldier1.gameObject.GetComponent<SoldierUI>().WinMessage.SetActive(true);
            soldier2.gameObject.GetComponent<SoldierUI>().WinMessage.SetActive(true);
            instance.currentGameState = GameState.ResistanceWin;
            StartCoroutine(ReturnToMainMenu(7f));
        }
        else if(instance.currentGameState == GameState.InGame && soldier1.Lives == 0 && soldier2.Lives == 0)
        {
            soldier1.gameObject.GetComponent<SoldierUI>().LoseMessage.SetActive(true);
            soldier2.gameObject.GetComponent<SoldierUI>().LoseMessage.SetActive(true);
            instance.currentGameState = GameState.ResistanceLose;
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
}
