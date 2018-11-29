using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuGUI : MonoBehaviour {

    // GUI attributes
    #region Attributes
    // Holds a reference to the instructions screen canvas
    public GameObject instructionScreen;
	public GameObject characterSelectScreen;
    public List<GameObject> instructionPages;
    int instructionsPage;

    public Color normalColor;
    public Color highlightedColor;

    int selectedButton = 0;
    public GameObject[] buttons;

    #endregion

    private void Start()
    {
        buttons[selectedButton].transform.GetChild(0).GetComponent<Text>().color = highlightedColor;
    }

    void FixedUpdate()
    {
        InputManagement();
        InstructionsManagement();
    }


    #region Button Inputs
	public void PlayButton()
	{
		// Change the current GameState
		GameManagerScript.instance.currentGameState = GameState.CharacterSelect;

		characterSelectScreen.SetActive(true);
		gameObject.SetActive(false);
	}

    /// <summary>
    /// Method for opening the instructions screen
    /// </summary>
    public void InstructionsOpen()
    {
        // Set the instructions screen to true
        if (instructionScreen.activeSelf == false && GameManagerScript.instance.currentGameState == GameState.MainMenu)
        {
            GameManagerScript.instance.currentGameState = GameState.Instructions;
            instructionScreen.SetActive(true);
            instructionsPage = 0;
            instructionPages[instructionsPage].SetActive(true);
        }
    }

    void InstructionsClose()
    {
        GameManagerScript.instance.currentGameState = GameState.MainMenu;
        instructionPages[instructionsPage].SetActive(false);
        instructionScreen.SetActive(false);
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
            if (/*Input.GetButtonDown(aButton)*/
                ControllerInput.controllers[0].A == 1
                || ControllerInput.controllers[1].A == 1)
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
            if (/*Input.GetButtonDown(bButton)*/
                ControllerInput.controllers[0].B == 1
                || ControllerInput.controllers[1].B == 1)
            {
                InstructionsClose();
            }
        }
    }

    void InputManagement()
    {
        if(GameManagerScript.instance.currentGameState == GameState.MainMenu)
        {
            Debug.Log(ControllerInput.controllers[0].Up.ToString());

            //player 1
            if (ControllerInput.controllers[0].Down == 1)
            {
                int prevButton = selectedButton;

                selectedButton = (selectedButton + 1) % buttons.Length;

                buttons[selectedButton].transform.GetChild(0).GetComponent<Text>().color = highlightedColor;
                buttons[prevButton].transform.GetChild(0).GetComponent<Text>().color = normalColor;
            }
            else if (ControllerInput.controllers[0].Up == 1)
            {
                int prevButton = selectedButton;

                selectedButton = (selectedButton + buttons.Length - 1) % buttons.Length;

                buttons[selectedButton].transform.GetChild(0).GetComponent<Text>().color = highlightedColor;
                buttons[prevButton].transform.GetChild(0).GetComponent<Text>().color = normalColor;
            }

            if (ControllerInput.controllers[0].A == 1)
            {
                buttons[selectedButton].GetComponent<Button>().onClick.Invoke();
            }


            //player 2
            if (ControllerInput.controllers[1].Down == 1)
            {
                int prevButton = selectedButton;

                selectedButton = (selectedButton + 1) % buttons.Length;

                buttons[selectedButton].transform.GetChild(0).GetComponent<Text>().color = highlightedColor;
                buttons[prevButton].transform.GetChild(0).GetComponent<Text>().color = normalColor;
            }
            else if (ControllerInput.controllers[1].Up == 1)
            {
                int prevButton = selectedButton;

                selectedButton = (selectedButton + buttons.Length - 1) % buttons.Length;

                buttons[selectedButton].transform.GetChild(0).GetComponent<Text>().color = highlightedColor;
                buttons[prevButton].transform.GetChild(0).GetComponent<Text>().color = normalColor;
            }

            if (ControllerInput.controllers[1].A == 1)
            {
                buttons[selectedButton].GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}
