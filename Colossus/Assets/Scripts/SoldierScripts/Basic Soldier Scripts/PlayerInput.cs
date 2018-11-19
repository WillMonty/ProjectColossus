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
                /*
                if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
                    return 1; //Pressed 
                else if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Pressed)
                    return 2; //Held   
                */
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
                if (ControllerInput.controllers[playerIndex].LeftTrigger > 0 
                    && ControllerInput.controllers[playerIndex].LeftTriggerPrev == 0)
                    return 1; //Pressed 
                else if (ControllerInput.controllers[playerIndex].LeftTrigger > 0)
                    return 2; //Held      
                /*
                if (state.Triggers.Left > 0 && prevState.Triggers.Left == 0)
                    return 1; //Pressed 
                else if (state.Triggers.Left > 0)
                    return 2; //Held       
                */
            }

            return 0;
        }
    }

    //Up DPad State
    public int Up
    {
        get
        {
            if(playerIndexSet)
            {
                return ControllerInput.controllers[playerIndex].DPadUp;
                /*
                if (state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released)
                    return 1; //Pressed
                else if (state.DPad.Up == ButtonState.Pressed)
                    return 2; //Held
                */
            }

            return 0;
        }
    }

    //Down DPad State
    public int Down
    {
        get
        {
            if (playerIndexSet)
            {
                return ControllerInput.controllers[playerIndex].DPadDown;
                /*
                if (state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released)
                    return 1; //Pressed
                else if (state.DPad.Down == ButtonState.Pressed)
                    return 2; //Held
                    */
            }

            return 0;
        }
    }

    //Right DPad State
    public int Right
    {
        get
        {
            if (playerIndexSet)
            {
                return ControllerInput.controllers[playerIndex].DPadRight;
                /*
                if (state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released)
                    return 1; //Pressed
                else if (state.DPad.Right == ButtonState.Pressed)
                    return 2; //Held
                    */
            }

            return 0;
        }
    }

    //Left DPad State
    public int Left
    {
        get
        {
            if (playerIndexSet)
            {
                return ControllerInput.controllers[playerIndex].DPadLeft;
                /*
                if (state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released)
                    return 1; //Pressed
                else if (state.DPad.Left == ButtonState.Pressed)
                    return 2; //Held
                    */
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
