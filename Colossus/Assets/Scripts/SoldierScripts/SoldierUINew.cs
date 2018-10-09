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
    public GameObject s1ReloadReticleBG;
    float s1ReloadDuration;
    float s1ReloadTime;
    bool s1Reloading;

    public Text s2CurrentMag;
    public Text s2MagMax;

    public GameObject s2Crosshair;
    public GameObject s2ReloadReticle;
    public GameObject s2ReloadReticleBG;
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


        if(soldier1.GetComponent<PlayerData>().WeaponData.Reloading && !s1Reloading)
        {
            s1ReloadDuration = soldier1.GetComponent<PlayerData>().WeaponData.ReloadTime;
            s1ReloadTime = s1ReloadDuration;
            s1Reloading = true;

            s1Crosshair.SetActive(false);
            s1ReloadReticleBG.SetActive(true);
        }

        if (soldier2.GetComponent<PlayerData>().WeaponData.Reloading && !s2Reloading)
        {
            s2ReloadDuration = soldier2.GetComponent<PlayerData>().WeaponData.ReloadTime;
            s2ReloadTime = s2ReloadDuration;
            s2Reloading = true;

            s2Crosshair.SetActive(false);
            s2ReloadReticleBG.SetActive(true);
        }

        if (s1Reloading)
        {                     
            if(s1ReloadTime < 0)
            {
                s1ReloadTime = 0;

                s1Crosshair.SetActive(true);
                s1ReloadReticleBG.SetActive(false);

                s1Reloading = false;
            }
            s1ReloadReticle.GetComponent<Image>().fillAmount = s1ReloadTime / s1ReloadDuration;
            s1ReloadTime -= Time.deltaTime;
        }

        if (s2Reloading)
        {          
            if (s2ReloadTime < 0)
            {
                s2ReloadTime = 0;

                s2Crosshair.SetActive(true);
                s2ReloadReticleBG.SetActive(false);

                s2Reloading = false;
            }
            s2ReloadReticle.GetComponent<Image>().fillAmount = s2ReloadTime / s2ReloadDuration;
            s2ReloadTime -= Time.deltaTime;
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
        s1CurrentMag.text = soldier1.GetComponent<PlayerData>().WeaponData.BulletsInMag.ToString();

        s2CurrentMag.text = soldier2.GetComponent<PlayerData>().WeaponData.BulletsInMag.ToString();
    }

    void UpdateMagMax()
    {
        s1MagMax.text = "/" + soldier1.GetComponent<PlayerData>().WeaponData.MagSize;

        s2MagMax.text = "/" + soldier2.GetComponent<PlayerData>().WeaponData.MagSize;

        s1CurrentMag.text = soldier1.GetComponent<PlayerData>().WeaponData.BulletsInMag.ToString();

        s2CurrentMag.text = soldier2.GetComponent<PlayerData>().WeaponData.BulletsInMag.ToString();
    }
}
