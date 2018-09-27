using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerMovement : MonoBehaviour {

    const float WALK_SPEED = 5.0f;
    const float RUN_SPEED = 8.0f;
    const float JUMP_FORCE = 6.0f;
    const float GRAVITY_FORCE = -.5f;

    CharacterController player;

    // Player variables
    public float speed = 2f;
    public float lookSensitivityX = .001f;
    public float lookSensitivityY = .001f;

    float xAxisClamp;

    GamePadState state;
    GamePadState prevState;

    // Movement Behavior Variables
    float verticalVelocity;
    public float VerticalVelocity
    {
        get{ return verticalVelocity; }
        set { verticalVelocity = value; }
    }

    float timeFalling;
    public float TimeFalling
    {
        get { return timeFalling; }
        set { timeFalling = value; }
    }

    // Animation Stuff
    public GameObject StandingPose;
    public GameObject RunningAnimation;

    // Movement Variables
    float moveFB; // Movement forward and backwards
    float moveLR; // Movement left and right

    float rotX; // rotation by the X axis
    float rotY; // rotation by the Y axis

    // Use this for initialization
    void Start ()
    {
        player = GetComponent<CharacterController>();

    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        state = GetComponent<PlayerInput>().State;
        prevState = GetComponent<PlayerInput>().PrevState;
        if (GameManagerScript.instance.currentGameState == GameState.InGame && GetComponent<PlayerInput>().PlayerIndexSet)
        {
            Move();
            Jump();
        }
    }

    /// <summary>
    /// Handles all movement calls
    /// </summary>
    private void Move()
    {
        #region Handles Running
        // Handles Running
        if (state.Triggers.Left > 0)
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
        Vector3 targetCamRot = GetComponent<PlayerData>().eyes.transform.rotation.eulerAngles;
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
        GetComponent<PlayerData>().eyes.transform.rotation = Quaternion.Euler(targetCamRot);
        GetComponent<PlayerData>().gun.transform.rotation = Quaternion.Euler(targetCamRot);
        transform.rotation = Quaternion.Euler(targetBodyRot);
        #endregion

        #region Applying movement
        // Create a vector movement
        Vector3 movement = new Vector3(moveLR, verticalVelocity, moveFB);
        movement = transform.rotation * movement;

        // Apply the final movement to the player
        player.Move(movement * Time.deltaTime);
        #endregion
    }

    
    /// <summary>
    /// Method to handle jumping and jetpack
    /// </summary>
    private void Jump()
    {

        if (player.isGrounded)
        {
            if (GetComponent<PlayerInput>().JumpState==1)
            {
            
                verticalVelocity = JUMP_FORCE;
                timeFalling = 0;
            }
            else if (GetComponent<PlayerInput>().JumpState == 0)
            {
                verticalVelocity += GRAVITY_FORCE;
            }
            
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
   
}
