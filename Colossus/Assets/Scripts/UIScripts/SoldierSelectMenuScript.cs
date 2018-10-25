using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class SoldierSelectMenuScript : MonoBehaviour {

    public GameObject SoldierSelectUI;

    public Color normalColor;
    public Color highlightedColor;

    //Player Inputs
    GamePadState state1;
    GamePadState prevState1;
    GamePadState state2;
    GamePadState prevState2;

    PlayerIndex p1Index = (PlayerIndex)0;
    PlayerIndex p2Index = (PlayerIndex)1;

    public SoldierClass p1SelectedClass = SoldierClass.Assault;
    public SoldierClass p2SelectedClass = SoldierClass.Assault;

    public GameObject p1ClassName;
    public GameObject p2ClassName;

    public GameObject[] p1Buttons;
    public GameObject[] p2Buttons;

    public int p1SelectedButton = 0;
    public int p2SelectedButton = 0;

    public bool p1Connected = false;
    public bool p2Connected = false;

    public bool p1Ready = false;
    public bool p2Ready = false;
    public bool colReady = true; //implement this later

    #region Player 1 Inputs

    //Up DPad State
    int p1Up
    {
        get
        {
            if (state1.DPad.Up == ButtonState.Pressed && prevState1.DPad.Up == ButtonState.Released)
                return 1; //Pressed
            /*else if (state1.DPad.Up == ButtonState.Pressed)
                return 2; //Held */
        
            return 0;
        }
    }

    //Down DPad State
    int p1Down
    {
        get
        {
            if (state1.DPad.Down == ButtonState.Pressed && prevState1.DPad.Down == ButtonState.Released)
                return 1; //Pressed
            /*else if (state1.DPad.Down == ButtonState.Pressed)
                return 2; //Held */
        
            return 0;
        }
    }

    //Right DPad State
    int p1Right
    {
        get
        {
            if (state1.DPad.Right == ButtonState.Pressed && prevState1.DPad.Right == ButtonState.Released)
                return 1; //Pressed
            /*else if (state1.DPad.Right == ButtonState.Pressed)
                return 2; //Held*/

            return 0;
        }
    }

    //Left DPad State
    int p1Left
    {
        get
        {
            if (state1.DPad.Left == ButtonState.Pressed && prevState1.DPad.Left == ButtonState.Released)
                return 1; //Pressed
            /*else if (state1.DPad.Left == ButtonState.Pressed)
                return 2; //Held*/

            return 0;
        }
    }


    int p1A
    {
        get
        {
            if (state1.Buttons.A == ButtonState.Pressed && prevState1.Buttons.A == ButtonState.Released)
                return 1;

            return 0;
        }
    }

    #endregion

    #region Player 2 Inputs

    //Up DPad State
    public int p2Up
    {
        get
        {
            if (state2.DPad.Up == ButtonState.Pressed && prevState2.DPad.Up == ButtonState.Released)
                return 1; //Pressed
                          /*else if (state2.DPad.Up == ButtonState.Pressed)
                              return 2; //Held */

            return 0;
        }
    }

    //Down DPad State
    public int p2Down
    {
        get
        {
            if (state2.DPad.Down == ButtonState.Pressed && prevState2.DPad.Down == ButtonState.Released)
                return 1; //Pressed
                          /*else if (state2.DPad.Down == ButtonState.Pressed)
                              return 2; //Held */

            return 0;
        }
    }

    //Right DPad State
    public int p2Right
    {
        get
        {
            if (state2.DPad.Right == ButtonState.Pressed && prevState2.DPad.Right == ButtonState.Released)
                return 1; //Pressed
            /*else if (state2.DPad.Right == ButtonState.Pressed)
                return 2; //Held*/

            return 0;
        }
    }

    //Left DPad State
    public int p2Left
    {
        get
        {
            if (state2.DPad.Left == ButtonState.Pressed && prevState2.DPad.Left == ButtonState.Released)
                return 1; //Pressed
            /*else if (state2.DPad.Left == ButtonState.Pressed)
                return 2; //Held*/

            return 0;
        }
    }

    int p2A
    {
        get
        {
            if (state2.Buttons.A == ButtonState.Pressed && prevState2.Buttons.A == ButtonState.Released)
                return 1;

            return 0;
        }
    }

    #endregion

    // Use this for initialization
    void Start ()
    {
        p1Buttons[p1SelectedButton].GetComponent<Image>().color = highlightedColor;
        p2Buttons[p2SelectedButton].GetComponent<Image>().color = highlightedColor;

        state1 = GamePad.GetState(p1Index);
        state2 = GamePad.GetState(p2Index);

        p1Connected = state1.IsConnected;
        p2Connected = state2.IsConnected;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(GameManagerScript.instance.currentGameState == GameState.CharacterSelect)
        {
            GamePadState testState = GamePad.GetState(p1Index);

            if(testState.IsConnected)
            {
                prevState1 = state1;
                state1 = testState;

                if(!p1Connected)
                {
                    p1Connected = true;
                }
            }
            else
            {
                if(p1Connected)
                {
                    p1Connected = false;
                }
            }

            testState = GamePad.GetState(p2Index);

            if(testState.IsConnected)
            {
                prevState2 = state2;
                state2 = testState;

                if(!p2Connected)
                {
                    p2Connected = true;
                }
            }
            else
            {
                if (p2Connected)
                {
                    p2Connected = false;
                }
            }

            if(p1Down == 1 && !p1Ready)
            {
                int prevButton = p1SelectedButton;

                p1SelectedButton = (p1SelectedButton + 1) % 4;

                p1Buttons[p1SelectedButton].GetComponent<Image>().color = highlightedColor;
                p1Buttons[prevButton].GetComponent<Image>().color = normalColor;
            }
            else if(p1Up == 1 && !p1Ready)
            {
                int prevButton = p1SelectedButton;

                p1SelectedButton = (p1SelectedButton + 3) % 4;

                p1Buttons[p1SelectedButton].GetComponent<Image>().color = highlightedColor;
                p1Buttons[prevButton].GetComponent<Image>().color = normalColor;
            }
            else if(p1A == 1)
            {
                p1Buttons[p1SelectedButton].GetComponent<Button>().onClick.Invoke();
            }

            if (p2Down == 1 && !p2Ready)
            {
                int prevButton = p2SelectedButton;

                p2SelectedButton = (p2SelectedButton + 1) % 4;

                p2Buttons[p2SelectedButton].GetComponent<Image>().color = highlightedColor;
                p2Buttons[prevButton].GetComponent<Image>().color = normalColor;
            }
            else if (p2Up == 1 && !p2Ready)
            {
                int prevButton = p2SelectedButton;

                p2SelectedButton = (p2SelectedButton + 3) % 4;

                p2Buttons[p2SelectedButton].GetComponent<Image>().color = highlightedColor;
                p2Buttons[prevButton].GetComponent<Image>().color = normalColor;
            }
            else if (p2A == 1)
            {
                p2Buttons[p1SelectedButton].GetComponent<Button>().onClick.Invoke();
            }

            StartGame();
        }
	}

    public void SetSoldierClass(int player)
    {
        switch(player)
        {
            case 1:
                if(p1SelectedButton != 3)
                {
                    p1SelectedClass = (SoldierClass)p1SelectedButton;

                    p1ClassName.GetComponent<Text>().text = p1SelectedClass.ToString();
                }
                break;
            case 2:
                if (p2SelectedButton != 3)
                {
                    p2SelectedClass = (SoldierClass)p2SelectedButton;

                    p2ClassName.GetComponent<Text>().text = p2SelectedClass.ToString();
                }
                break;
        }
    }

    public void TogglePlayerReady(int player)
    {
        switch (player)
        {
            case 1:
                p1Ready = !p1Ready;
                break;
            case 2:
                p2Ready = !p2Ready;
                break;
        }
    }

    void StartGame()
    {
        if( ((p1Connected == p1Ready) || !p1Connected) 
            && ((p2Connected == p2Ready) || !p2Connected)
            && (p1Connected || p2Connected)
            && colReady)
        {
            Debug.Log("ready");

            AbilityManagerScript.instance.SetSoldierClass(1, p1SelectedClass);
            AbilityManagerScript.instance.SetSoldierClass(2, p2SelectedClass);

            SoldierSelectUI.SetActive(false);

            GameManagerScript.instance.BeginGame();

            GameManagerScript.instance.SpawnSoldiers();
        }
    }
}
