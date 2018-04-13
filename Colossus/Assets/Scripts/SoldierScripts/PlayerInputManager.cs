using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    // Attributes
    public int playerNum;

    const float WALK_SPEED = 5.0f;
    const float RUN_SPEED = 10.0f;


    // Player variables
    public float speed = 2f;
    public float lookSensitivityX = 2f;
    public float lookSensitivityY = 2f;
    CharacterController player;


    // Movement Variables
    float moveFB; // Movement forward and backwards
    float moveLR; // Movement left and right

    float rotX; // rotation by the X axis
    float rotY; // rotation by the Y axis



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
    void Start ()
    {
        player = GetComponent<CharacterController>();
        SetControllerVariables();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
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

    private bool ButtonIsDown()
    {
        //switch()


        return false;    
    }


    /// <summary>
    /// Handles all movement calls
    /// </summary>
    private void Move()
    {
        /*
        // Rotate the player
        // Look axis
        rotX = Input.GetAxis(horizontalAxis) * lookSensitivityX;
        rotY = Input.GetAxis(verticalAxis) * lookSensitivityY;

        Vector3 targetRotCam = transform.rotation.eulerAngles;

        rotX = Mathf.Clamp(rotX, -90, 90);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        */

        // Handles Running
        if (Input.GetButtonDown(runButton))
        {
            speed = RUN_SPEED;
        }
        if (Input.GetButtonUp(runButton))
        {
            speed = WALK_SPEED;
        }

        
        // Movement variables/axis
        moveFB = Input.GetAxis(moveYAxis) * speed;
        //Debug.Log("Move FB: " + moveFB);
        moveLR = -Input.GetAxis(moveXAxis) * speed;
        //Debug.Log("Move LR: " + moveLR);
        
        // Create a vector movement
        Vector3 movement = new Vector3(moveLR, 0, moveFB);

        // apply the rotation to the player movement
        movement = transform.rotation * movement;

        // Apply the final movement to the player
        player.Move(movement * Time.deltaTime);
        
    }
}
