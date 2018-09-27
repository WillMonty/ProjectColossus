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

    bool reloadButton;
    public bool ReloadButton
    {
        get { return reloadButton; }
    }

    

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

    float rightTrigger;
    public float RightTrigger
    {
        get { return rightTrigger; }
    }

    // Use this for initialization
    void Start()
    {         
        playerIndex = (PlayerIndex)(GetComponent<PlayerData>().playerNumber - 1);
      
    }

    // Update is called once per frame
    void Update()
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
        rightTrigger = state.Triggers.Right;
        reloadButton = (ButtonState.Pressed==state.Buttons.X);
        
    }
   



}
