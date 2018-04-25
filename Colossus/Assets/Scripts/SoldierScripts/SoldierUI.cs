using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUI : MonoBehaviour {
    GameObject player; 

    // Values needed for health bar
    public float maxHealth;
    public float currentHealth;
    public GameObject healthBarCanvas;
    public Slider healthBar;


    public void Start()
    {
        maxHealth = player.GetComponent<PlayerManager>().MaxHealth;
    }



    // Use this for initialization
    public void UIBarInstantiate()
    {
        // Set the healthBar Initial Values
        maxHealth = gameObject.GetComponent<PlayerManager>().MaxHealth;
        healthBar.maxValue = maxHealth;

        // Set the healthBar Max value
        maxHealth = gameObject.GetComponent<PlayerManager>().MaxHealth;
        healthBar.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        // Get current health and update the bar
        currentHealth = gameObject.GetComponent<PlayerManager>().Health;
        healthBar.value = currentHealth;
    }
}
