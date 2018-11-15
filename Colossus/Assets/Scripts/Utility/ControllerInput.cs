using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Controller
{
    PlayerIndex index;

    //Player Inputs
    GamePadState state = new GamePadState();
    GamePadState statePrev = new GamePadState();
    
    public bool connected = false;
    public bool connectedPrev = false;

    #region Inputs
    
    /// <summary>
    /// Up DPad State
    /// </summary>
    public int Up
    {
        get
        {
            if (state.DPad.Up == ButtonState.Pressed && statePrev.DPad.Up == ButtonState.Released)
                return 1; //Pressed
            else if (state.DPad.Up == ButtonState.Pressed)
                return 2; //Held 

            return 0;
        }
    }
    
    /// <summary>
    /// Down DPad State
    /// </summary>
    public int Down
    {
        get
        {
            if (state.DPad.Down == ButtonState.Pressed && statePrev.DPad.Down == ButtonState.Released)
                return 1;   //Pressed
            else if (state.DPad.Down == ButtonState.Pressed)
                return 2;   //Held 

            return 0;
        }
    }
    
    /// <summary>
    /// Right DPad State
    /// </summary>
    public int Right
    {
        get
        {
            if (state.DPad.Right == ButtonState.Pressed && statePrev.DPad.Right == ButtonState.Released)
                return 1;   //Pressed
            else if (state.DPad.Right == ButtonState.Pressed)
                return 2;   //Held

            return 0;
        }
    }
    
    /// <summary>
    /// Left DPad State
    /// </summary>
    public int Left
    {
        get
        {
            if (state.DPad.Left == ButtonState.Pressed && statePrev.DPad.Left == ButtonState.Released)
                return 1;   //Pressed
            else if (state.DPad.Left == ButtonState.Pressed)
                return 2;   //Held

            return 0;
        }
    }
    
    /// <summary>
    /// A Button State
    /// </summary>
    public int A
    {
        get
        {
            if (state.Buttons.A == ButtonState.Pressed && statePrev.Buttons.A == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.A == ButtonState.Pressed)
                return 2;   //Held

            return 0;
        }
    }
    
    /// <summary>
    /// B Button State
    /// </summary>
    public int B
    {
        get
        {
            if (state.Buttons.B == ButtonState.Pressed && statePrev.Buttons.B == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.B == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }
    
    /// <summary>
    /// X Button State
    /// </summary>
    public int X
    {
        get
        {
            if (state.Buttons.X == ButtonState.Pressed && statePrev.Buttons.X == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.X == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }
    
    /// <summary>
    /// Y Button State
    /// </summary>
    public int Y
    {
        get
        {
            if (state.Buttons.Y == ButtonState.Pressed && statePrev.Buttons.Y == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.Y == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }
    
    /// <summary>
    /// Start Button State
    /// </summary>
    public int Start
    {
        get
        {
            if (state.Buttons.Start == ButtonState.Pressed && statePrev.Buttons.Start == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.Start == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    /// <summary>
    /// Back Button State
    /// </summary>
    public int Back
    {
        get
        {
            if (state.Buttons.Back == ButtonState.Pressed && statePrev.Buttons.Back == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.Back == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    /// <summary>
    /// Right Shoulder Button State
    /// </summary>
    public int RightShoulder
    {
        get
        {
            if (state.Buttons.RightShoulder == ButtonState.Pressed && statePrev.Buttons.RightShoulder == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.RightShoulder == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    /// <summary>
    /// Left Shoulder Button State
    /// </summary>
    public int LeftShoulder
    {
        get
        {
            if (state.Buttons.LeftShoulder == ButtonState.Pressed && statePrev.Buttons.LeftShoulder == ButtonState.Released)
                return 1;   //Pressed
            else if (state.Buttons.LeftShoulder == ButtonState.Pressed)
                return 2;

            return 0;
        }
    }

    /// <summary>
    /// Right Trigger State
    /// </summary>
    public float RightTrigger
    {
        get
        {
            return state.Triggers.Right;
        }
    }

    /// <summary>
    /// Previous Right Trigger State
    /// </summary>
    public float RightTriggerPrev
    {
        get
        {
            return statePrev.Triggers.Right;
        }
    }

    /// <summary>
    /// Left Trigger State
    /// </summary>
    public float LeftTrigger
    {
        get
        {
            return state.Triggers.Left;
        }
    }

    /// <summary>
    /// Previous Left Trigger State
    /// </summary>
    public float LeftTriggerPrev
    {
        get
        {
            return statePrev.Triggers.Left;
        }
    }

    /// <summary>
    /// Right Stick X-axis State
    /// </summary>
    public float RightStickX
    {
        get
        {
            return state.ThumbSticks.Right.X;
        }
    }

    /// <summary>
    /// Previous Right Stick X-axis State
    /// </summary>
    public float RightStickXPrev
    {
        get
        {
            return statePrev.ThumbSticks.Right.X;
        }
    }
    

    /// <summary>
    /// Right Stick Y-axis State
    /// </summary>
    public float RightStickY
    {
        get
        {
            return state.ThumbSticks.Right.Y;
        }
    }

    /// <summary>
    /// Previous Right Stick Y-axis State
    /// </summary>
    public float RightStickYPrev
    {
        get
        {
            return statePrev.ThumbSticks.Right.Y;
        }
    }

    /// <summary>
    /// Left Stick X-axis State
    /// </summary>
    public float LeftStickX
    {
        get
        {
            return state.ThumbSticks.Left.X;
        }
    }

    /// <summary>
    /// Previous Left Stick X-axis State
    /// </summary>
    public float LeftStickXPrev
    {
        get
        {
            return statePrev.ThumbSticks.Left.X;
        }
    }
    
    /// <summary>
    /// Left Stick Y-axis State
    /// </summary>
    public float LeftStickY
    {
        get
        {
            return state.ThumbSticks.Left.Y;
        }
    }

    /// <summary>
    /// Left Stick Y-axis State
    /// </summary>
    public float LeftStickYPrev
    {
        get
        {
            return statePrev.ThumbSticks.Left.Y;
        }
    }


    /// <summary>
    /// Left Stick Up State
    /// </summary>
    public int LeftStickUp
    {
        get
        {
            if (state.ThumbSticks.Left.Y >= .1 && statePrev.ThumbSticks.Left.Y < .1)
                return 1;
            if (state.ThumbSticks.Left.Y >= .1)
                return 2;

            return 0;
        }
    }

    /// <summary>
    /// Left Stick Down State
    /// </summary>
    public int LeftStickDown
    {
        get
        {
            if (state.ThumbSticks.Left.Y <= -.1 && statePrev.ThumbSticks.Left.Y > -.1)
                return 1;
            if (state.ThumbSticks.Left.Y <= -.1)
                return 2;

            return 0;
        }
    }

    /// <summary>
    /// Left Stick Right State
    /// </summary>
    public int LeftStickRight
    {
        get
        {
            if (state.ThumbSticks.Left.X >= .1 && statePrev.ThumbSticks.Left.X < .1)
                return 1;
            if (state.ThumbSticks.Left.X >= .1)
                return 2;

            return 0;
        }
    }

    /// <summary>
    /// Left Stick Left State
    /// </summary>
    public int LeftStickLeft
    {
        get
        {
            if (state.ThumbSticks.Left.X <= -.1 && statePrev.ThumbSticks.Left.X > -.1)
                return 1;
            if (state.ThumbSticks.Left.X <= -.1)
                return 2;

            return 0;
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
        statePrev = new GamePadState();
        connected = state.IsConnected;
    }

    public void UpdateStates()
    {
        GamePadState testState = GamePad.GetState(index);

        connectedPrev = connected;
        connected = testState.IsConnected;
        
        if (connected)
        {
            statePrev = state;
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
