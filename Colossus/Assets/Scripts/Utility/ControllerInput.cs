using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Controller
{
    PlayerIndex index;

    //Player Inputs
    GamePadState state = new GamePadState();
    GamePadState prevState = new GamePadState();
    
    public bool connected = false;
    public bool connectedPrev = false;

    #region Inputs

    //Up DPad State
    public int Up
    {
        get
        {
            if (state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released)
                return 1; //Pressed
            else if (state.DPad.Up == ButtonState.Pressed)
                return 2; //Held 

            return 0;
        }
    }

    //Down DPad State
    public int Down
    {
        get
        {
            if (state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released)
                return 1;   //Pressed
            else if (state.DPad.Down == ButtonState.Pressed)
                return 2;   //Held 

            return 0;
        }
    }

    //Right DPad State
    public int Right
    {
        get
        {
            if (state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released)
                return 1;   //Pressed
            else if (state.DPad.Right == ButtonState.Pressed)
                return 2;   //Held

            return 0;
        }
    }

    //Left DPad State
    public int Left
    {
        get
        {
            if (state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released)
                return 1;   //Pressed
            else if (state.DPad.Left == ButtonState.Pressed)
                return 2;   //Held

            return 0;
        }
    }


    public int A
    {
        get
        {
            if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.A == ButtonState.Pressed)
                return 2;   //Held

            return 0;
        }
    }

    public int B
    {
        get
        {
            if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.B == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    public int X
    {
        get
        {
            if (state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.X == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    public int Y
    {
        get
        {
            if (state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.Y == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    public int Start
    {
        get
        {
            if (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.Start == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    public int Back
    {
        get
        {
            if (state.Buttons.Back == ButtonState.Pressed && prevState.Buttons.Back == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.Back == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    public int RightShoulder
    {
        get
        {
            if (state.Buttons.RightShoulder == ButtonState.Pressed && prevState.Buttons.RightShoulder == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.RightShoulder == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    public int LeftShoulder
    {
        get
        {
            if (state.Buttons.LeftShoulder == ButtonState.Pressed && prevState.Buttons.LeftShoulder == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.LeftShoulder == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    public float RightTrigger
    {
        get
        {
            return state.Triggers.Right;
        }
    }

    public float RightTriggerPrev
    {
        get
        {
            return prevState.Triggers.Right;
        }
    }

    public float LeftTrigger
    {
        get
        {
            return state.Triggers.Left;
        }
    }

    public float LeftTriggerPrev
    {
        get
        {
            return prevState.Triggers.Left;
        }
    }

    public float RightStickX
    {
        get
        {
            return state.ThumbSticks.Right.X;
        }
    }

    public float RightStickY
    {
        get
        {
            return state.ThumbSticks.Right.Y;
        }
    }

    public float LeftStickX
    {
        get
        {
            return state.ThumbSticks.Left.X;
        }
    }

    public float LeftStickY
    {
        get
        {
            return state.ThumbSticks.Left.Y;
        }
    }

    #endregion

    public Controller(int playerNum)
    {
        index = (PlayerIndex)playerNum;
    }

    public void ResetStates()
    {
        state = GamePad.GetState(index);
        prevState = new GamePadState();
        connected = state.IsConnected;
    }

    public void UpdateStates()
    {
        GamePadState testState = GamePad.GetState(index);

        connectedPrev = connected;
        connected = testState.IsConnected;
        
        if (connected)
        {
            prevState = state;
            state = testState;
        }
    }
}

public static class ControllerInput  {

    static int controllerNum = 2;

    public static Controller[] controllers;

    public static bool controllersSet = false;
    
    public static void SetUpControllers()
    {
        controllers = new Controller[controllerNum];

        controllersSet = true;

        for(int index = 0; index < controllerNum; index++)
        {
            controllers[index] = new Controller(index);
        }
    }

    // Use this for initialization
    public static void ResetControllers ()
    {
        foreach(Controller c in controllers)
        {
            c.ResetStates();
        }
    }
	
	// Update is called once per frame
	public static void UpdateControllers()
    {
        foreach(Controller c in controllers)
        {
            c.UpdateStates();
        }
    }
}
