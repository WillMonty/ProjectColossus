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
    public GameObject neck;
    public GameObject colossusBody;
    public GameObject disabledColossus; //The starting stationary colossus that is turned off when playing
    Laser laser;
    //Will add variables for the rigged colossus model

    [Header("Map")]
    public GameObject map;
    public float lowerMapAmount;
    public GameObject lava;
    public GameObject resistanceContainer;


    //Audio
    [Header("Audio")]
    public AudioSource headSource;
    public AudioSource torsoSource;
    public AudioClip hopInSound;
    public AudioClip damageSound;

    [Header("UI")]
    public Slider heathBar;

    [Header("Player Positioning")]
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    public Material inPositionMat;
    public Material outPositionMat;

 
    
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
        health = STARTING_HEALTH;

        laser = gameObject.GetComponent<Laser>();

        if (debugColossus) ToggleColossus(); //If in debug mode let the VR player start immediately in the colossus

        StartCoroutine(LateStart(0.2f));

    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Sets the instance after the game manager has actually initialized
        GameManagerScript.instance.colossus = this;
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
        heathBar.value = (STARTING_HEALTH - health)/1000.0f;
        
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
            //Make the indicators green
            leftIndicator.GetComponent<MeshRenderer>().material = inPositionMat;
            rightIndicator.GetComponent<MeshRenderer>().material = inPositionMat;

            //Check if either trigger is pressed to hop in
            bool leftTriggerPressed = leftController.GetComponent<SteamVR_TrackedController>().triggerPressed;
            bool rightTriggerPressed = rightController.GetComponent<SteamVR_TrackedController>().triggerPressed;

            if (leftTriggerPressed || rightTriggerPressed) ToggleColossus();
        }
        else
        {
            //Make the indicators red
            leftIndicator.GetComponent<MeshRenderer>().material = outPositionMat;
            rightIndicator.GetComponent<MeshRenderer>().material = outPositionMat;
        }
    }

    /// <summary>
    /// Method to toggle the player into the colossus and remove the disabled version
    /// </summary>
    private void ToggleColossus()
    {
        //Turn off indicators
        leftIndicator.SetActive(false);
        rightIndicator.SetActive(false);

        //Enable hands
        GameObject leftHand = leftController.transform.GetChild(0).gameObject;
        GameObject rightHand = rightController.transform.GetChild(0).gameObject;
        leftHand.SetActive(true);
        rightHand.SetActive(true);

        //Enable Colossus Body
        colossusBody.SetActive(true);
        neck.SetActive(true);

        laser.enabled = true;

        disabledColossus.SetActive(false);

        playerInBot = true;

        lava.SetActive(true);

        LowerMap();

        //Start the game once the VR player is ready.
        GameManagerScript.instance.currentGameState = GameState.InGame;

        //Non-debug only
        if (!debugColossus)
        {
            //Turn off base controller prefab
            leftController.transform.GetChild(4).gameObject.SetActive(false);
            rightController.transform.GetChild(4).gameObject.SetActive(false);

            headSource.clip = hopInSound;
            headSource.Play();
        }
    }

    #endregion

    void LowerMap()
    {
        float playerY = headset.transform.position.y - lowerMapAmount;
        map.transform.position = new Vector3(map.transform.position.x, playerY, map.transform.position.z);
        resistanceContainer.transform.position = new Vector3(resistanceContainer.transform.position.x, playerY + resistanceContainer.transform.position.y, resistanceContainer.transform.position.z);
    }
}
