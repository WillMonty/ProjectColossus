using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUINew : MonoBehaviour {

    public PlayerManager player1;
    public PlayerManager player2;


    public float maxHealth;

    public float p1CurrentHealth;
    public Image p1HealthBar;

    public float p2CurrentHealth;
    public Image p2HealthBar;

    // Use this for initialization
    void Start ()
    {
        maxHealth = player1.GetComponent<PlayerManager>().MaxHealth;

        p1CurrentHealth = player1.GetComponent<PlayerManager>().Health;

        p2CurrentHealth = player2.GetComponent<PlayerManager>().Health;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateHealthBar();
	}

    void UpdateHealthBar()
    {
        if (p1CurrentHealth != player1.GetComponent<PlayerManager>().Health)
        {
            p1CurrentHealth = player1.GetComponent<PlayerManager>().Health;

            p1HealthBar.fillAmount = p1CurrentHealth / maxHealth;
        }

        if (p2CurrentHealth != player2.GetComponent<PlayerManager>().Health)
        {
            p2CurrentHealth = player2.GetComponent<PlayerManager>().Health;

            p2HealthBar.fillAmount = p2CurrentHealth / maxHealth;
        }
    }
}
