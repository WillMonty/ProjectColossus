using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInputManager : MonoBehaviour
{

    // Attributes
    public int playerNum;


    //Gamepad Variables
    PlayerIndex playerIndex;
    bool playerIndexSet = false;

    GamePadState state;
    GamePadState prevState;


    const float WALK_SPEED = 5.0f;
    const float RUN_SPEED = 8.0f;
    const float JUMP_FORCE = 6.0f;
    const float JETPACK_FORCE = .5f;
    const float GRAVITY_FORCE = -.5f;

    // Player variables
    public float speed = 2f;
    public float lookSensitivityX = .001f;
    public float lookSensitivityY = .001f;
    float xAxisClamp;

    // Component Variables
    CharacterController player;
    public GameObject eyes;
    public GameObject rifle;

    // Movement Variables
    float moveFB; // Movement forward and backwards
    float moveLR; // Movement left and right

    float rotX; // rotation by the X axis
    float rotY; // rotation by the Y axis

    // Movement Behavior Variables
    float verticalVelocity;
    float timeFalling;

    // Animation Stuff
    public GameObject StandingPose;
    public GameObject RunningAnimation;

    //Gun variable 
    public GunScript gunState;

    // Use this for initialization
    void Start()
    {
       
        player = GetComponent<CharacterController>();
        gunState = rifle.GetComponent<GunScript>();
        // Give an instance to this soldier to the gamemanager
        StartCoroutine(LateStart(0.2f));

        playerIndex = (PlayerIndex)(playerNum - 1);
      
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Sets the instance after the game manager has actually initializes
        switch (playerNum)
        {
            case 1:
                GameManagerScript.instance.soldier1 = GetComponent<PlayerManager>();
                break;
            case 2:
                GameManagerScript.instance.soldier2 = GetComponent<PlayerManager>();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        GamePadState testState = GamePad.GetState(playerIndex);
        
        if (!playerIndexSet || !prevState.IsConnected)
        {
            playerIndexSet = false;
            if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", playerIndex));
                    playerIndexSet = true;
                }           
        }
       
        if (testState.IsConnected)
        {
            prevState = state;
            state = GamePad.GetState(playerIndex);

            if (GameManagerScript.instance.currentGameState == GameState.InGame)
            {
                Move();
                Jump();
            }
        }
        else
        {
            //Debug.Log(string.Format("GamePad at {0} disconnected", playerIndex));
            playerIndexSet = false;
        }

        //Sends required gamepad info to the gun script
        gunState.rightTrigger = state.Triggers.Right;
        gunState.reloadButton = (ButtonState.Pressed==state.Buttons.X);
        
    }
    
    /// <summary>
    /// Handles all movement calls
    /// </summary>
    private void Move()
    {
        #region Handles Running
        // Handles Running
        if (state.Triggers.Left > 0 )
        {
            speed = RUN_SPEED;
        }
        else
        {
            speed = WALK_SPEED;
        }
        #endregion

        #region Getting Input
        // Movement variables/axis
        
        if (state.ThumbSticks.Left.Y > .2 || state.ThumbSticks.Left.Y < -.2)
        {
            moveFB = state.ThumbSticks.Left.Y * speed;

            // Turn on running animation and turn off standing pose
            if (RunningAnimation.activeSelf == false)
            {
                RunningAnimation.SetActive(true);
                StandingPose.SetActive(false);
            }
        }
        else
        {
            moveFB = 0;
        }
        //Debug.Log("Move FB: " + moveFB);
        if (state.ThumbSticks.Left.X > .2 || state.ThumbSticks.Left.X < -.2)
        {
            moveLR = state.ThumbSticks.Left.X * speed;

            // Turn on running animation and turn off standing pose
            if (RunningAnimation.activeSelf == false)
            {
                RunningAnimation.SetActive(true);
                StandingPose.SetActive(false);
            }
        }
        else
        {
            moveLR = 0;
        }

        if (moveLR == 0 && moveFB == 0)
        {
            // Turn on standing pose
            if (RunningAnimation.activeSelf == true)
            {
                RunningAnimation.SetActive(false);
                StandingPose.SetActive(true);
            }
        }

        // Rotate the player
        // Look axis
        if (state.ThumbSticks.Right.X > .2 || state.ThumbSticks.Right.X < -.2)
        {
            rotX = state.ThumbSticks.Right.X * lookSensitivityX;
        }
        else
        {
            rotX = 0;
        }
        if (state.ThumbSticks.Right.Y > .2 || state.ThumbSticks.Right.Y < -.2)
        {
            rotY = -state.ThumbSticks.Right.Y * lookSensitivityY;
        }
        else
        {
            rotY = 0;
        }


        // Adding an xAxis
        xAxisClamp += rotY;

        // Set a temporary vector3 for rotation
        Vector3 targetCamRot = eyes.transform.rotation.eulerAngles;
        Vector3 targetBodyRot = transform.rotation.eulerAngles;

        // Up/Down Rotation
        targetCamRot.x += rotY;
        targetCamRot.z = 0;
        targetBodyRot.y += rotX;

        // Clamping the look rotation
        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetCamRot.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetCamRot.x = 270;
        }

        // Applying the rotations to the player
        eyes.transform.rotation = Quaternion.Euler(targetCamRot);
        rifle.transform.rotation = Quaternion.Euler(targetCamRot);
        transform.rotation = Quaternion.Euler(targetBodyRot);
        #endregion

        #region Applying movement
        // Create a vector movement
        Vector3 movement = new Vector3(moveLR, verticalVelocity, moveFB);

        //Debug.Log("Player " + playerNum + " Movement: " + movement);
        movement = transform.rotation * movement;

        //Debug.Log(rb.velocity);

        // Apply the final movement to the player
        player.Move(movement * Time.deltaTime);
        #endregion
    }

    #region Jumping helper method
    /// <summary>
    /// Method to handle jumping and jetpack
    /// </summary>
    private void Jump()
    {
    
        if (player.isGrounded)
        {

            if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
            {
               
                verticalVelocity = JUMP_FORCE;
                timeFalling = 0;
            }
            else
            {
                verticalVelocity += GRAVITY_FORCE;
            }
        }
        else if (state.Buttons.A == ButtonState.Pressed && GetComponent<PlayerManager>().JetPackFuel > 0)
        {
            verticalVelocity += JETPACK_FORCE;
            GetComponent<PlayerManager>().FuelDown();
            timeFalling -= Time.deltaTime;
        }
        else
        {
            if (timeFalling < 0)
            {
                timeFalling = 0;
            }
            timeFalling += Time.deltaTime;
            verticalVelocity += (GRAVITY_FORCE * timeFalling);
        }
        verticalVelocity = Mathf.Clamp(verticalVelocity, -30, 5);
    }
    #endregion

}
