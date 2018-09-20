using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUINew : MonoBehaviour {

    public PlayerManager soldier1;
    public PlayerManager soldier2;
    
    public float maxHealth;

    public float s1CurrentHealth;
    public Image s1Healthbar;

    public float s2CurrentHealth;
    public Image s2Healthbar;


    public float maxFuel;

    public float s1CurrentFuel;
    public Image s1Fuelbar;

    public float s2CurrentFuel;
    public Image s2Fuelbar;

    public Text s1currentMag;

    public Text s2currentMag;

    // Use this for initialization
    void Start ()
    {
        maxHealth = soldier1.GetComponent<PlayerManager>().MaxHealth;

        s1CurrentHealth = soldier1.GetComponent<PlayerManager>().Health;
        s1Healthbar.fillAmount = s1CurrentHealth / maxHealth;

        s2CurrentHealth = soldier2.GetComponent<PlayerManager>().Health;
        s2Healthbar.fillAmount = s2CurrentHealth / maxHealth;

        maxFuel = soldier1.GetComponent<PlayerManager>().MaxFuel;

        s1CurrentFuel = soldier1.GetComponent<PlayerManager>().JetPackFuel;
        s1Fuelbar.fillAmount = s1CurrentFuel / maxFuel;

        s2CurrentFuel = soldier2.GetComponent<PlayerManager>().JetPackFuel;
        s2Fuelbar.fillAmount = s2CurrentFuel / maxFuel;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateHealthbar();

        UpdateFuelbar();

        UpdateCurrentMag();

    }

    void UpdateHealthbar()
    {
        if (s1CurrentHealth != soldier1.GetComponent<PlayerManager>().Health)
        {
            s1CurrentHealth = soldier1.GetComponent<PlayerManager>().Health;

            s1Healthbar.fillAmount = s1CurrentHealth / maxHealth;
        }

        if (s2CurrentHealth != soldier2.GetComponent<PlayerManager>().Health)
        {
            s2CurrentHealth = soldier2.GetComponent<PlayerManager>().Health;

            s2Healthbar.fillAmount = s2CurrentHealth / maxHealth;
        }
    }

    void UpdateFuelbar()
    {
        if(s1CurrentFuel != soldier1.GetComponent<PlayerManager>().JetPackFuel)
        {
            s1CurrentFuel = soldier1.GetComponent<PlayerManager>().JetPackFuel;

            s1Fuelbar.fillAmount = s1CurrentFuel / maxFuel;
        }

        if (s2CurrentFuel != soldier2.GetComponent<PlayerManager>().JetPackFuel)
        {
            s2CurrentFuel = soldier2.GetComponent<PlayerManager>().JetPackFuel;

            s2Fuelbar.fillAmount = s2CurrentFuel / maxFuel;
        }
    }

    void UpdateCurrentMag()
    {
        s1currentMag.text = soldier1.GetComponent<PlayerInputManager>().gunState.BulletsInMag.ToString();

        s2currentMag.text = soldier2.GetComponent<PlayerInputManager>().gunState.BulletsInMag.ToString();
    }
}
