using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    // Attributes
    public int playerNum;


    // Player variables
    public float speed = 2f;
    public float sensitivity = 2f;
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
        // Movement variables/axis
        moveFB = Input.GetAxis(verticalAxis) * speed;
        moveLR = Input.GetAxis(horizontalAxis) * speed;

        // Look axis
        rotX = Input.GetAxis(moveXAxis) * sensitivity;
        rotY = Input.GetAxis(moveYAxis) * sensitivity;

        // Create a vector movement
        Vector3 movement = new Vector3(moveLR, 0, moveFB);

        // Rotate the player
        //transform.Rotate(0, rotX, 0);

        // apply the rotation to the player movement
        //movement = transform.rotation * movement;

        // Apply the final movement to the player
        player.Move(movement * Time.deltaTime);
    }
}
