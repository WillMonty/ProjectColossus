using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUI : MonoBehaviour {
    public GameObject playerCamera;
    public PlayerData player;
    int playerNum;

    // Values needed for health bar
    public float maxHealth;
    public float currentHealth;
    public Slider healthBar;

    // Values for Jetpack
    public float maxFuel;
    public float currentFuel;
    public Slider fuelBar;

    public GameObject PreGameMessage;
    public GameObject LoseMessage;
    public GameObject WinMessage;
    public GameObject RespawnMessage;
    public GameObject LivesText;
    public GameObject PauseMenu;

    private string pauseButton;

    // Use this for initialization
    public void Start()
    {
        // Set the different messages at instantiation
        PreGameMessage = transform.Find("PreGameMessage").gameObject;
        LoseMessage = transform.Find("LoseCanvas").gameObject;
        WinMessage = transform.Find("WinCanvas").gameObject;
        RespawnMessage = transform.Find("RespawnMessageCanvas").gameObject;
        LivesText = transform.Find("LivesText").gameObject;
        playerNum = player.playerNumber;
        // For Pause Menu
        if (playerNum == 1)
        {
            pauseButton = "J" + playerNum + "Pause";
			PauseMenu = transform.Find("PauseCanvas").gameObject;
        }

        // Set Player controller stuff
        player = GameObject.Find("Player" + playerNum + "ControllerFPS").GetComponent<PlayerData>();
        playerCamera = player.transform.Find("Player" + playerNum + "Eyes").gameObject;
        GetComponent<Canvas>().worldCamera= playerCamera.GetComponent<Camera>();
        UIBarInstantiate();
    }


    // Update is called once per frame
    void Update()
    {
        UIUpdate();

        if(playerNum == 1 && Input.GetButtonDown(pauseButton))
        {
            SwitchActiveStatesPauseMenu();
        }

    }

    /// <summary>
    /// Helper method instantiating the UI bars
    /// </summary>
    private void UIBarInstantiate()
    {
        // Set the healthBar Initial Values
        maxHealth = player.MaxHealth;
        healthBar.maxValue = maxHealth;

        // Set the healthBar Max value
       // maxFuel = player.MaxFuel;
        fuelBar.maxValue = maxFuel;
    }

    /// <summary>
    /// Updates the current UI bar
    /// </summary>
    private void UIUpdate()
    {
		if(GameManagerScript.instance.currentGameState == GameState.InGame)
		{
			PreGameMessage.SetActive(false);
		}

        if(GameManagerScript.instance.currentGameState == GameState.ResistanceWin)
        {
            WinMessage.SetActive(true);
        }

        if (GameManagerScript.instance.currentGameState == GameState.ResistanceLose)
        {
            LoseMessage.SetActive(true);
        }

        // Get current health and update the bar
        currentHealth = player.Health;
        healthBar.value = currentHealth;

        // Get current health and update the bar
        //currentFuel = player.JetPackFuel;
        fuelBar.value = currentFuel;

        LivesText.GetComponent<Text>().text = "Lives: " + player.Lives;
    }


    /// <summary>
    /// Switch the message for if respawning
    /// </summary>
    public void SwitchActiveStatesRespawnMessage()
    {
        if(RespawnMessage.activeSelf == false)
        {
            RespawnMessage.SetActive(true);
        }
        else
        {
            RespawnMessage.SetActive(false);
        }
    }

    /// <summary>
    /// Switch the state for the pause menu
    /// </summary>
    public void SwitchActiveStatesPauseMenu()
    {
        if (PauseMenu.activeSelf == false)
        {
            PauseMenu.SetActive(true);
        }
        else
        {
            PauseMenu.SetActive(false);
        }
    }
}
