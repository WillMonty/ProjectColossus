using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInput : MonoBehaviour
{
    //Gamepad Variables
    int playerIndex;

    bool playerIndexSet = false;
    public bool PlayerIndexSet
    {
        get { return playerIndexSet; }
    }
    

    //A button state for jumping.
    public int JumpState
    {
        get
        {
            if (playerIndexSet)
            {
                return ControllerInput.controllers[playerIndex].A;
            }

            return 0;
            
        }
    }

    //Left Trigger State for soldier abilities
    public int ActionState
    {
        get
        {
            if (playerIndexSet)
            {
                return ControllerInput.controllers[playerIndex].LeftTriggerPressed;
            }

            return 0;
        }
    }
    


    // Use this for initialization
    void Start()
    {         
        playerIndex = GetComponent<PlayerData>().playerNumber - 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerIndexSet || !ControllerInput.controllers[playerIndex].connectedPrev /*prevState.IsConnected*/)
        {
            playerIndexSet = false;
            if (ControllerInput.controllers[playerIndex].connected
                /*testState.IsConnected*/)
                {
                    Debug.Log(string.Format("GamePad found {0}", playerIndex));
                    playerIndexSet = true;
                }           
        }
       
        if (!ControllerInput.controllers[playerIndex].connected)
        { 
            //Debug.Log(string.Format("GamePad at {0} disconnected", playerIndex));
            playerIndexSet = false;
        }


        //Shooting button states
        GetComponent<PlayerData>().WeaponData.RightTrigger = ControllerInput.controllers[GetComponent<PlayerData>().playerNumber - 1].RightTrigger;
        GetComponent<PlayerData>().WeaponData.RightTriggerPrev = ControllerInput.controllers[GetComponent<PlayerData>().playerNumber - 1].RightTriggerPrev;
        GetComponent<PlayerData>().WeaponData.ReloadButton = (ControllerInput.controllers[GetComponent<PlayerData>().playerNumber - 1].X == 1);
        
    }
}
