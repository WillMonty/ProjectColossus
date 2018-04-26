using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    // Attributes
    public int playerNum;

    const float WALK_SPEED = 5.0f;
    const float RUN_SPEED = 10.0f;
    const float JUMP_FORCE = 6.0f;
    const float JETPACK_FORCE = .5f;
    const float GRAVITY_FORCE = -1f;


    // Player variables
    public float speed = 2f;
    public float lookSensitivityX = .001f;
    public float lookSensitivityY = .001f;
    float xAxisClamp;


    // Component Variables
    CharacterController player;
    public GameObject eyes;
    



    // Movement Variables
    float moveFB; // Movement forward and backwards
    float moveLR; // Movement left and right

    float rotX; // rotation by the X axis
    float rotY; // rotation by the Y axis

    // Movement Behavior Variables
    float verticalVelocity;
    float timeFalling;

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
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Debug.Log(player.isGrounded);
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
        if (Input.GetAxis(moveYAxis) > .2  || Input.GetAxis(moveYAxis) < -.2 )
        {
            moveFB = -Input.GetAxis(moveYAxis) * speed;
        }
        else
        {
            moveFB = 0;
        }
        //Debug.Log("Move FB: " + moveFB);
        if (Input.GetAxis(moveXAxis) > .2 || Input.GetAxis(moveXAxis) < -.2)
        {
            moveLR = Input.GetAxis(moveXAxis) * speed;
        }
        else
        {
            moveLR = 0;
        }

        // Rotate the player
        // Look axis
        if (Input.GetAxis(horizontalAxis) > .2 || Input.GetAxis(horizontalAxis)<-.2)
        {
            rotX = Input.GetAxis(horizontalAxis) * lookSensitivityX;
        }
        else
        {
            rotX = 0;
        }
        if (Input.GetAxis(verticalAxis) > .2 || Input.GetAxis(verticalAxis) < -.2)
        {
            rotY = Input.GetAxis(verticalAxis) * lookSensitivityY;
        }
        else
        {
            rotY = 0;
        }
        //Debug.Log("Rot X: " + rotX + " Rot Y: " + rotY + " Move FB: " + moveFB + " Move LR: " + moveLR);

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
        else if(xAxisClamp<-90)
        {
            xAxisClamp = -90;
            targetCamRot.x = 270;
        }

        // Applying the rotations to the player
        eyes.transform.rotation = Quaternion.Euler(targetCamRot);
        transform.GetChild(0).rotation = Quaternion.Euler(targetCamRot);
        Debug.Log(transform.GetChild(0));
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
        Debug.Log("Vert Velocity" + verticalVelocity);
        Debug.Log("Player Grounded: " + player.isGrounded);
        if (player.isGrounded)
        {
            if (Input.GetButtonDown(aButton))
            {
                //Debug.Log("Jump!" + playerNum);
                verticalVelocity = JUMP_FORCE;
                timeFalling = 0;
            }
            else
            {
                verticalVelocity += GRAVITY_FORCE;
            }
        }
        else if (Input.GetButton(aButton) && GetComponent<PlayerManager>().JetPackFuel>0)
        {
            verticalVelocity+=JETPACK_FORCE;
            GetComponent<PlayerManager>().FuelDown();
            timeFalling -= Time.deltaTime;
        }
        else
        {
            if(timeFalling<0)
            {
                timeFalling = 0;
            }
            timeFalling += Time.deltaTime;
            verticalVelocity += (GRAVITY_FORCE*timeFalling);
        }
         verticalVelocity = Mathf.Clamp(verticalVelocity, -30, 5);
    }
    #endregion

}
