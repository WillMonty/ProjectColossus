using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUI : MonoBehaviour {
    public GameObject playerCamera;
    public PlayerManager player;
    public int playerNum;

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


    // Use this for initialization
    public void Start()
    {
        // Set the different messages at instantiation
        PreGameMessage = transform.Find("PreGameMessage").gameObject;
        LoseMessage = transform.Find("LoseCanvas").gameObject;
        WinMessage = transform.Find("WinCanvas").gameObject;
        RespawnMessage = transform.Find("RespawnMessageCanvas").gameObject;
        LivesText = transform.Find("LivesText").gameObject;

        // Set Player controller stuff
        player = GameObject.Find("Player" + playerNum + "ControllerFPS").GetComponent<PlayerManager>();
        playerCamera = player.transform.Find("Player" + playerNum + "Eyes").gameObject;
        GetComponent<Canvas>().worldCamera= playerCamera.GetComponent<Camera>();
        UIBarInstantiate();
    }


    // Update is called once per frame
    void Update()
    {
        UIUpdate();

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
        maxFuel = player.MaxFuel;
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

        // Get current health and update the bar
        currentHealth = player.Health;
        healthBar.value = currentHealth;

        // Get current health and update the bar
        currentFuel = player.JetPackFuel;
        fuelBar.value = currentFuel;

        LivesText.GetComponent<Text>().text = "Lives: " + player.Lives;
    }


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
}
