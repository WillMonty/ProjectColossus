using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierUINew : MonoBehaviour {

    public PlayerData soldier1;
    public PlayerData soldier2;
    
    public float s1MaxHealth;
    public float s2MaxHealth;

    public float s1CurrentHealth;
    public Image s1Healthbar;

    public float s2CurrentHealth;
    public Image s2Healthbar;


    public float maxFuel;

    public float s1CurrentFuel;
    public Image s1Fuelbar;

    public float s2CurrentFuel;
    public Image s2Fuelbar;

    public Text s1CurrentMag;
    public Text s1MagMax;

    public GameObject s1Crosshair;
    public GameObject s1ReloadReticle;
    float s1ReloadDuration;
    float s1ReloadTime;
    bool s1Reloading;

    public Text s2CurrentMag;
    public Text s2MagMax;

    public GameObject s2Crosshair;
    public GameObject s2ReloadReticle;
    float s2ReloadDuration;
    float s2ReloadTime;
    bool s2Reloading;

    // Use this for initialization
    void Start ()
    {
        s1MaxHealth = soldier1.GetComponent<PlayerData>().MaxHealth;
        s2MaxHealth = soldier2.GetComponent<PlayerData>().MaxHealth;


        s1CurrentHealth = soldier1.GetComponent<PlayerData>().Health;
        s1Healthbar.fillAmount = s1CurrentHealth / s1MaxHealth;

        s2CurrentHealth = soldier2.GetComponent<PlayerData>().Health;
        s2Healthbar.fillAmount = s2CurrentHealth / s2MaxHealth;

        s1Reloading = false;
        s2Reloading = false;


        if (soldier1.soldierClass == SoldierClass.Assault)
        {
            maxFuel = soldier1.GetComponent<JetPack>().MaxFuel;
            s1CurrentFuel = soldier1.GetComponent<JetPack>().JetPackFuel;
            s1Fuelbar.fillAmount = s1CurrentFuel / maxFuel;
        }

        if (soldier2.soldierClass == SoldierClass.Assault)
        {
            maxFuel = soldier2.GetComponent<JetPack>().MaxFuel;
            s2CurrentFuel = soldier2.GetComponent<JetPack>().JetPackFuel;
            s2Fuelbar.fillAmount = s2CurrentFuel / maxFuel;
        }
    }


    // Update is called once per frame
    void Update ()
    {
        UpdateHealthbar();

        UpdateFuelbar();

        UpdateCurrentMag();

        if(s1MagMax.text == "test" || s2MagMax.text == "test")
        {
            UpdateMagMax();
        }

    }

    void UpdateHealthbar()
    {
        if (s1CurrentHealth != soldier1.GetComponent<PlayerData>().Health)
        {
            s1CurrentHealth = soldier1.GetComponent<PlayerData>().Health;

            s1Healthbar.fillAmount = s1CurrentHealth / s1MaxHealth;
        }

        if (s2CurrentHealth != soldier2.GetComponent<PlayerData>().Health)
        {
            s2CurrentHealth = soldier2.GetComponent<PlayerData>().Health;

            s2Healthbar.fillAmount = s2CurrentHealth / s2MaxHealth;
        }
    }

    void UpdateFuelbar()
    {
        if(soldier1.soldierClass == SoldierClass.Assault && s1CurrentFuel != soldier1.GetComponent<JetPack>().JetPackFuel)
        {
            s1CurrentFuel = soldier1.GetComponent<JetPack>().JetPackFuel;

            s1Fuelbar.fillAmount = s1CurrentFuel / maxFuel;
        }

        if (soldier2.soldierClass == SoldierClass.Assault && s2CurrentFuel != soldier2.GetComponent<JetPack>().JetPackFuel)
        {
            s2CurrentFuel = soldier2.GetComponent<JetPack>().JetPackFuel;

            s2Fuelbar.fillAmount = s2CurrentFuel / maxFuel;
        }
    }

    void UpdateCurrentMag()
    {
        s1CurrentMag.text = soldier1.GetComponent<PlayerData>().GunBase.BulletsInMag.ToString();

        s2CurrentMag.text = soldier2.GetComponent<PlayerData>().GunBase.BulletsInMag.ToString();
    }

    void UpdateMagMax()
    {
        s1MagMax.text = "/" + soldier1.GetComponent<PlayerData>().GunBase.MagSize;

        s2MagMax.text = "/" + soldier2.GetComponent<PlayerData>().GunBase.MagSize;

        s1CurrentMag.text = soldier1.GetComponent<PlayerData>().GunBase.BulletsInMag.ToString();

        s2CurrentMag.text = soldier2.GetComponent<PlayerData>().GunBase.BulletsInMag.ToString();
    }

    void StartReload(int playerNum)
    {
        switch (playerNum)
        {
            case 1:

                break;
            case 2:

                break;
        }
    }
}
