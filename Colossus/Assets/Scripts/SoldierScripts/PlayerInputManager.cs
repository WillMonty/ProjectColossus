using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    // Attributes
    public int playerNum;


    // Controller Variables
    private string moveXAxis;
    private string moveYAxis;
    private string horizontalAxis;
    private string verticalAxis;
    private string aButton;
    private string bButton;
    private string xButton;
    private string yButton;
    private string triggerRight;
    private string triggerLeft;




    // Use this for initialization
    void Start ()
    {
        SetControllerVariables();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
        xButton = "J" + playerNum + "X";
        yButton = "J" + playerNum + "Y";
        triggerLeft = "J" + playerNum + "TriggerLeft";
        triggerRight = "J" + playerNum + "TriggerRight";
    }

    private bool ButtonIsDown()
    {
        //switch()


        return false;    
    }

    private void Move()
    {

    }
}
