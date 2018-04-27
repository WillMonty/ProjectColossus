using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGUI : MonoBehaviour {

    // GUI attributes
    #region Attributes
    // Holds a reference to the instructions screen canvas
    public GameObject InstructionScreen;
    public List<GameObject> instructionPages;
    int instructionsPage;

    // String holding the player 1 controls for the main menu
    private string moveXAxis = "J1MoveX";
    private string moveYAxis = "J1MoveY";
    private string horizontalAxis = "J1Horizontal";
    private string verticalAxis = "J1Vertical";
    private string aButton = "J1A";
    private string bButton = "J1B";
    private string yButton = "J1Y";
    private string triggerLeft = "J1TriggerLeft";
    private string runButton = "J1LeftStickClick";
    #endregion

    #region Update
    void Update()
    {
		for(int i = 0; i < Input.GetJoystickNames().Length; i++){
			Debug.Log(Input.GetJoystickNames()[i]);	
		}
        InstructionsManagement();
    }
    #endregion


    #region Button Inputs
    /// <summary>
    /// Method for starting the game
    /// </summary>
    public void StartGame()
    {
        // Change the current GameState
        GameManagerScript.instance.currentGameState = GameState.Pregame;

        // Last thing: Load the game scene
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Method for opening the instructions screen
    /// </summary>
    public void InstructionsOpen()
    {
        // Set the instructions screen to true
        if (InstructionScreen.activeSelf == false && GameManagerScript.instance.currentGameState == GameState.MainMenu)
        {
            GameManagerScript.instance.currentGameState = GameState.Instructions;
            InstructionScreen.SetActive(true);
            instructionsPage = 0;
            instructionPages[instructionsPage].SetActive(true);
        }
    }

    void InstructionsClose()
    {
        GameManagerScript.instance.currentGameState = GameState.MainMenu;
        instructionPages[instructionsPage].SetActive(false);
        InstructionScreen.SetActive(false);
        instructionsPage = 0;
    }



    /// <summary>
    /// Method for exiting the game
    /// </summary>
    public void ExitGame()
    {
            // IF WE ARE RUNNING IN A UNITY STANDALONE
    #if UNITY_STANDALONE
            // Quit the Application
            Application.Quit();
    #endif

            // IF WE ARE RUNNING IN the unity Editor
    #if UNITY_EDITOR
            // Stop play mode in Unity
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
    #endregion

    void InstructionsManagement()
    {
        if (GameManagerScript.instance.currentGameState == GameState.Instructions)
        {
            if (Input.GetButtonDown(aButton))
            {
                // turn off current page
                for (int i = 0; i < instructionPages.Count; i++)
                {
                    if (instructionPages[instructionsPage])
                    {
                        instructionPages[instructionsPage].SetActive(false);
                    }
                }

                // Increase the page int
                instructionsPage++;
                instructionsPage %= instructionPages.Count;
                instructionPages[instructionsPage].SetActive(true);
            }
            if (Input.GetButtonDown(bButton))
            {
                InstructionsClose();
            }
        }
    }
}
