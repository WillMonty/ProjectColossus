﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInput : MonoBehaviour
{
    //Gamepad Variables
    PlayerIndex playerIndex;

    bool playerIndexSet = false;
    public bool PlayerIndexSet
    {
        get { return playerIndexSet; }
    }

    GamePadState state;
    public GamePadState State
    {
        get { return state; }
    }

    GamePadState prevState;
    public GamePadState PrevState
    {
        get { return prevState; }
    }



    //A button state for jumping.
    public int JumpState
    {
        get{
            if (playerIndexSet)
            {
                if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
                    return 1; //Pressed 
                else if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Pressed)
                    return 2; //Held             
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
                if (state.Triggers.Left > 0  && prevState.Triggers.Left == 0)
                    return 1; //Pressed 
                else if (state.Triggers.Left > 0)
                    return 2; //Held             
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
                if (state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released)
                    return 1; //Pressed
                else if (state.DPad.Up == ButtonState.Pressed)
                    return 2; //Held
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
                if (state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released)
                    return 1; //Pressed
                else if (state.DPad.Down == ButtonState.Pressed)
                    return 2; //Held
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
                if (state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released)
                    return 1; //Pressed
                else if (state.DPad.Right == ButtonState.Pressed)
                    return 2; //Held
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
                if (state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released)
                    return 1; //Pressed
                else if (state.DPad.Left == ButtonState.Pressed)
                    return 2; //Held
            }

            return 0;
        }
    }


    // Use this for initialization
    void Start()
    {         
        playerIndex = (PlayerIndex)(GetComponent<PlayerData>().playerNumber - 1);
      
    }

    // Update is called once per frame
    void FixedUpdate()
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
        }
        else
        {
            //Debug.Log(string.Format("GamePad at {0} disconnected", playerIndex));
            playerIndexSet = false;
        }
        //Shooting button states
        GetComponent<PlayerData>().WeaponData.RightTrigger = state.Triggers.Right;
        GetComponent<PlayerData>().WeaponData.RightTriggerPrev = prevState.Triggers.Right;
        GetComponent<PlayerData>().WeaponData.ReloadButton = (ButtonState.Pressed==state.Buttons.X);
        
    }
}
