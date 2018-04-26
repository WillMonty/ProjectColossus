using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUI : MonoBehaviour {
    public GameObject playerCamera;
    public GameObject player;
    public int playerNum;

    // Values needed for health bar
    public float maxHealth;
    public float currentHealth;
    public Slider healthBar;

    // Values for Jetpack
    public float maxFuel;
    public float currentFuel;
    public Slider fuelBar;


    // Use this for initialization
    public void Start()
    {
        player = GameObject.Find("Player" + playerNum + "ControllerFPS");
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
        maxHealth = player.GetComponent<PlayerManager>().MaxHealth;
        healthBar.maxValue = maxHealth;

        // Set the healthBar Max value
        maxFuel = player.GetComponent<PlayerManager>().MaxFuel;
        fuelBar.maxValue = maxFuel;
    }

    /// <summary>
    /// Updates the current UI bar
    /// </summary>
    private void UIUpdate()
    {
        // Get current health and update the bar
        currentHealth = player.GetComponent<PlayerManager>().Health;
        healthBar.value = currentHealth;

        // Get current health and update the bar
        currentFuel = player.GetComponent<PlayerManager>().JetPackFuel;
        fuelBar.value = currentFuel;
    }
}
