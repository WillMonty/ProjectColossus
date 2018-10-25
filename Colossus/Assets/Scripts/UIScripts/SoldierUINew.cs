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

    float damageAlphaTimerMax;

    public Image s1DamageInd;
    float s1DamageAlphaTimer;

    public Image s2DamageInd;
    float s2DamageAlphaTimer;

    bool isActive = false;
    

    /// <summary>
    /// 
    /// </summary>
    void Start ()
    {
        damageAlphaTimerMax = 1.1f;

        s1DamageInd.color = new Color(1f, 1f, 1f, 0f);
    }


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update ()
    {
        if (isActive)
        {
            UpdateHealthbar();

            UpdateAbilitybar();

            UpdateCurrentMag();


            if (s1MagMax.text == "test" || s2MagMax.text == "test")
            {
                UpdateMagMax();
            }


            UpdateReloading();

            UpdateDamageInd();
        }
        else
            Activate();
    }


    /// <summary>
    /// activates the soldier UI and gets the different soldier references
    /// </summary>
    void Activate()
    {
        soldier1 = GameManagerScript.instance.soldier1;
        soldier2 = GameManagerScript.instance.soldier2;

        if (soldier1 != null && soldier2!=null)
        {

            isActive = true;

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
    }


    /// <summary>
    /// Updates the reloading reticle for the players
    /// </summary>
    void UpdateReloading()
    {
        if (soldier1.GetComponent<PlayerData>().WeaponData.Reloading && !s1Reloading)
        {
            s1ReloadDuration = soldier1.GetComponent<PlayerData>().WeaponData.ReloadTime;
            s1ReloadTime = 0;
            s1Reloading = true;

            s1Crosshair.SetActive(false);
            s1ReloadReticleBG.SetActive(true);

            s1ReloadReticle.GetComponent<Image>().fillAmount = 0;
        }

        if (soldier2.GetComponent<PlayerData>().WeaponData.Reloading && !s2Reloading)
        {
            s2ReloadDuration = soldier2.GetComponent<PlayerData>().WeaponData.ReloadTime;
            s2ReloadTime = 0;
            s2Reloading = true;

            s2Crosshair.SetActive(false);
            s2ReloadReticleBG.SetActive(true);

            s2ReloadReticle.GetComponent<Image>().fillAmount = 0;
        }

        if (s1Reloading)
        {
            if (s1ReloadTime >= s1ReloadDuration)
            {
                s1ReloadTime = s1ReloadDuration;
                
                s1Reloading = false;
            }
            
			s1ReloadTime += Time.deltaTime;

            s1ReloadReticle.GetComponent<Image>().fillAmount = s1ReloadTime / s1ReloadDuration;
        }
        else
        {
            s1Crosshair.SetActive(true);
            s1ReloadReticleBG.SetActive(false);
        }

        if (s2Reloading)
        {
            if (s2ReloadTime >= s2ReloadDuration)
            {
                s2ReloadTime = s2ReloadDuration;

                s2Reloading = false;
            }

			s2ReloadTime += Time.deltaTime;

            s2ReloadReticle.GetComponent<Image>().fillAmount = s2ReloadTime / s2ReloadDuration;
        }
        else
        {
            s2Crosshair.SetActive(true);
            s2ReloadReticleBG.SetActive(false);
        }
    }


    /// <summary>
    /// updates the healthbars for both players if they take damage
    /// </summary>
    void UpdateHealthbar()
    {
        if (s1CurrentHealth != soldier1.GetComponent<PlayerData>().Health)
        {
            s1CurrentHealth = soldier1.GetComponent<PlayerData>().Health;

            s1Healthbar.fillAmount = s1CurrentHealth / s1MaxHealth;

            PlayerDamaged(1);
        }

        if (s2CurrentHealth != soldier2.GetComponent<PlayerData>().Health)
        {
            s2CurrentHealth = soldier2.GetComponent<PlayerData>().Health;

            s2Healthbar.fillAmount = s2CurrentHealth / s2MaxHealth;

            PlayerDamaged(2);
        }
    }


    /// <summary>
    /// updates the ability for the soldiers
    /// </summary>
    void UpdateAbilitybar()
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


    /// <summary>
    /// updates the magazine for both players
    /// </summary>
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


    /// <summary>
    /// indicates that the player has taken damage
    /// </summary>
    /// <param name="playerNum"></param>
    void PlayerDamaged(int playerNum)
    {
        Debug.Log("player " + playerNum + " is damaged");

        switch(playerNum)
        {
            case 1:
                s1DamageAlphaTimer = damageAlphaTimerMax;

                var tempColor1 = s1DamageInd.color;

                tempColor1.a = 1f;

                s1DamageInd.color = tempColor1;

                break;
            case 2:

                s2DamageAlphaTimer = damageAlphaTimerMax;

                var tempColor2 = s2DamageInd.color;

                tempColor2.a = 1f;

                s2DamageInd.color = tempColor2;

                break;
        }
    }


    /// <summary>
    /// updates the damage indicator
    /// </summary>
    void UpdateDamageInd()
    {
        if(s1DamageAlphaTimer > 0)
        {
            s1DamageAlphaTimer -= Time.deltaTime;

            var tempColor = s1DamageInd.color;

            tempColor.a = s1DamageAlphaTimer / damageAlphaTimerMax;

            s1DamageInd.color = tempColor;
        }
        
        if(s2DamageAlphaTimer > 0)
        {
            s2DamageAlphaTimer -= Time.deltaTime;

            var tempColor = s2DamageInd.color;

            tempColor.a = s2DamageAlphaTimer / damageAlphaTimerMax;

            s2DamageInd.color = tempColor;
        }
    }
}
