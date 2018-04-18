using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColossusManager : MonoBehaviour {

    #region Robot Attributes
    //Debug variable to allow the game to start with the colossus on
    public bool debugColossus;

    // Robot Manager
    const int STARTING_HEALTH = 1000;
    const float RESPAWN_TIME = 5.0f;
    private bool playerInBot; //Is the player currently in the colossus and ready to play?
    private float health;

    // Public components needed for the toggling of the colossus
    [Header("Colossus Components")]
    public GameObject leftController;
    public GameObject rightController;
    public GameObject headset;
    public GameObject disabledColossus; //The starting stationary colossus that is turned off when playing
    Laser laser;
        //Will add variables for the rigged colossus model

    //Audio
    [Header("Audio")]
    public AudioSource source;
    public AudioClip hopInSound;

    [Header("UI")]
    public Slider heathBar;
 

    // Ammo Variables
    // Change this variable to private at some point
    //public int assaultRifleAmmo;
    #endregion

    #region Properties
    // Properties

    /// <summary>
    /// Could be used for a boss health GUI element for soldiers
    /// </summary>
    public float Health
    {
        get { return health; }
    }

    #endregion

    #region Start Method
    // Use this for initialization
    void Start()
    {
        // Start initializes 3 lives at start, can be changed later
        health = STARTING_HEALTH;

        laser = gameObject.GetComponent<Laser>();

        if (debugColossus) ToggleColossus(); //If in debug mode let the VR player start immediately in the colossus


    }

    #endregion

    #region Update Method
    // Update is called once per frame
    void Update ()
    {
        if (!playerInBot) CheckHopIn(); //Check if the player is jumping in the bot

	}
    #endregion

    #region Helper Methods
    // Damage helper method
    public void Damage(float damageFloat)
    {
        health -= damageFloat;
        heathBar.value = 100 - health;
        
    }

    /// <summary>
    /// Method to handle all Colossus abilities
    /// </summary>
    private void AbilityManagement()
    {

    }

    /// <summary>
    /// Helper method to check if the player is "hopping into" the colossus
    /// </summary>
    private void CheckHopIn()
    {
        //Check if the headset is in the collider
        bool headsetIn = disabledColossus.GetComponent<DisabledColossusTrigger>().HeadsetInTrigger;
        if(headsetIn)
        {
            //Check if either trigger is pressed to hop in
            bool leftTriggerPressed = leftController.GetComponent<SteamVR_TrackedController>().triggerPressed;
            bool rightTriggerPressed = rightController.GetComponent<SteamVR_TrackedController>().triggerPressed;

            if (leftTriggerPressed || rightTriggerPressed) ToggleColossus();
        }
    }

    /// <summary>
    /// Method to toggle the player into the colossus and remove the disabled version
    /// </summary>
    private void ToggleColossus()
    {
        //Enable hands
        GameObject leftHand = leftController.transform.GetChild(0).gameObject;
        GameObject rightHand = rightController.transform.GetChild(0).gameObject;
        leftHand.SetActive(true);
        rightHand.SetActive(true);
        //Remove controller models TODO (Hand 

        laser.enabled = true;

        disabledColossus.SetActive(false);

        playerInBot = true;

        leftController.transform.GetChild(3).gameObject.SetActive(false);
        rightController.transform.GetChild(3).gameObject.SetActive(false);

        //Play hop in sound if not in debug
        if (debugColossus) return;
        source.clip = hopInSound;
        source.Play();
    }


    #endregion

    private void OnCollisionEnter(Collider collider)
    {

        // IF the robot is hit with the bullet, it damages the robot and deletes the bullet
        if(collider.tag == "playerbullet")
        {
            Damage(collider.GetComponent<BulletScript>().Damage);
            collider.GetComponent<BulletScript>().deleteBullet();
        }
    }
}
