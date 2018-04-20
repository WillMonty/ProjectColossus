using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    // Attributes
    public int playerNum;

    const float WALK_SPEED = 5.0f;
    const float RUN_SPEED = 10.0f;
    const float JUMP_FORCE = 7.0f;


    // Player variables
    public float speed = 2f;
    public float lookSensitivityX = .001f;
    public float lookSensitivityY = .001f;
    float xAxisClamp;


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
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Jump();
        
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
        moveFB = -Input.GetAxis(moveYAxis) * speed;
        //Debug.Log("Move FB: " + moveFB);
        moveLR = Input.GetAxis(moveXAxis) * speed;

        // Rotate the player
        // Look axis
        rotX = Input.GetAxis(horizontalAxis) * lookSensitivityX;
        rotY = Input.GetAxis(verticalAxis) * lookSensitivityY;

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
        transform.rotation = Quaternion.Euler(targetBodyRot);
        #endregion

        #region Applying movement
        // Create a vector movement
        Vector3 movement = new Vector3(moveLR, rb.velocity.y, moveFB);

        //Debug.Log("Player " + playerNum + " Movement: " + movement);
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
        if (Input.GetButtonDown(aButton))
        {
            Debug.Log("Jump!" + playerNum);
            rb.velocity += JUMP_FORCE * Vector3.up;
        }
    }

}
