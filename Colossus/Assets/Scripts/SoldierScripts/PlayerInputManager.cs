﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    // Attributes
    public int playerNum;

    const float WALK_SPEED = 5.0f;
    const float RUN_SPEED = 10.0f;
    const float JUMP_FORCE = 3.0f;


    // Player variables
    public float speed = 2f;
    public float lookSensitivityX = 2f;
    public float lookSensitivityY = 2f;


    // Component Variables
    CharacterController player;
    public GameObject eyes;
    private Rigidbody rb;



    // Movement Variables
    float moveFB; // Movement forward and backwards
    float moveLR; // Movement left and right

    float rotX; // rotation by the X axis
    float rotY; // rotation by the Y axis

    // Movement Behavior Variables
    bool grounded;
    float jumpVelocity;



    // Controller Variables
    private string moveXAxis;
    private string moveYAxis;
    private string horizontalAxis;
    private string verticalAxis;
    private string aButton;
    private string bButton;
    private string yButton;
    private string triggerRight;
    private string triggerLeft;
    private string runButton;




    // Use this for initialization
    void Start()
    {
        SetControllerVariables();
        player = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetButtonDown(aButton))
        {
            Jump();
        }
    }


    // Helper method to help set up the player controller
    void SetControllerVariables()
    {
        moveXAxis = "J" + playerNum + "MoveX";
        moveYAxis = "J" + playerNum + "MoveY";
        horizontalAxis = "J" + playerNum + "Horizontal";
        verticalAxis = "J" + playerNum + "Vertical";
        aButton = "J" + playerNum + "A";
        bButton = "J" + playerNum + "B";
        yButton = "J" + playerNum + "Y";
        triggerLeft = "J" + playerNum + "TriggerLeft";
        runButton = "J" + playerNum + "LeftStickClick";
    }

    /// <summary>
    /// Handles all movement calls
    /// </summary>
    private void Move()
    {
        #region Handles Running
        // Handles Running
        if (Input.GetButtonDown(runButton))
        {
            speed = RUN_SPEED;
        }
        if (Input.GetButtonUp(runButton))
        {
            speed = WALK_SPEED;
        }
        #endregion

        #region Getting Input
        // Movement variables/axis
        moveFB = Input.GetAxis(moveYAxis) * speed;
        //Debug.Log("Move FB: " + moveFB);
        moveLR = -Input.GetAxis(moveXAxis) * speed;

        // Rotate the player
        // Look axis
        rotX = Input.GetAxis(horizontalAxis) * lookSensitivityX;
        rotY = -Input.GetAxis(verticalAxis) * lookSensitivityY;

        // Clamp the rotation
        rotY = Mathf.Clamp(rotY, -60f, 60f);
        rotX = Mathf.Clamp(rotX, -360f, 360f);
        #endregion

        #region Applying movement
        // Create a vector movement
        Vector3 movement = new Vector3(moveLR, rb.velocity.y, moveFB);

        // Handling the FPS rotation
        //transform.Rotate(0, rotX, 0);
        //eyes.transform.Rotate(-rotY, 0, 0);

        // apply the rotation to the player movement
        movement = transform.rotation * movement;

        //Debug.Log("Player " + playerNum + " Movement: " + movement);

        // Apply the final movement to the player
        player.Move(movement * Time.deltaTime);
        #endregion
    }

    /// <summary>
    /// Method to handle jumping and jetpack
    /// </summary>
    private void Jump()
    {
        Debug.Log("Jump!");
        rb.velocity += JUMP_FORCE * Vector3.up;
    }

}
