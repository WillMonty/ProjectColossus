using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// GameState Enum
public enum GameState { MainMenu, InGame, Paused, Win, Lose };


public class GameManagerScript : MonoBehaviour
{
    #region Attributes
    // Static instance of the GameManager which allows it to be accessed from any script
    public static GameManagerScript instance = null;
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

    // Update is called once per frame
    void Update ()
    {
		//Debug.Log(UnityEngine.Input.GetJoystickNames());
	}
}
