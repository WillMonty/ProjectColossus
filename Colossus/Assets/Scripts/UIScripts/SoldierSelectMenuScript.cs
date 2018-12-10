using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class SoldierSelectMenuScript : MonoBehaviour {

    public GameObject SoldierSelectUI;

    public Color normalColor;
    public Color highlightedColor;
    public Color readyColor;

    private SoldierClass p1SelectedClass = SoldierClass.Assault;
    private SoldierClass p2SelectedClass = SoldierClass.Assault;

    public GameObject p1ClassName;
    public GameObject p2ClassName;

    public GameObject[] p1Buttons;
    public GameObject[] p2Buttons;

    private int p1SelectedButton = 0;
    private int p2SelectedButton = 0;

    public GameObject WaitingPanel;
    
    private bool p1Ready = false;
    private bool p2Ready = false;
    private bool soldiersReady = false;

    public GameObject p1DisabledScreen;
    public GameObject p2DisabledScreen;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		if (GameManagerScript.instance.currentGameState == GameState.CharacterSelect)
        {
            if (p1Buttons[p1SelectedButton].GetComponent<Image>().color != highlightedColor
                && p1Buttons[p1SelectedButton].GetComponent<Image>().color != readyColor)
            {
                p1Buttons[p1SelectedButton].GetComponent<Image>().color = highlightedColor;
            }

            if(p2Buttons[p2SelectedButton].GetComponent<Image>().color != highlightedColor
                && p2Buttons[p2SelectedButton].GetComponent<Image>().color != readyColor)
            {
                p2Buttons[p2SelectedButton].GetComponent<Image>().color = highlightedColor;
            }

            //player 1
            if (ControllerInput.controllers[0].Down == 1 && !p1Ready)
            {
                int prevButton = p1SelectedButton;

                p1SelectedButton = (p1SelectedButton + 1) % p1Buttons.Length;

                p1Buttons[p1SelectedButton].GetComponent<Image>().color = highlightedColor;
                p1Buttons[prevButton].GetComponent<Image>().color = normalColor;
            }

            else if (ControllerInput.controllers[0].Up == 1 && !p1Ready)
            {
                int prevButton = p1SelectedButton;

                p1SelectedButton = (p1SelectedButton + p1Buttons.Length - 1) % p1Buttons.Length;

                p1Buttons[p1SelectedButton].GetComponent<Image>().color = highlightedColor;
                p1Buttons[prevButton].GetComponent<Image>().color = normalColor;
            }

            if (ControllerInput.controllers[0].A == 1)
            {
                p1Buttons[p1SelectedButton].GetComponent<Button>().onClick.Invoke();
            }

            if (ControllerInput.controllers[0].B == 1)
            {
                if(p1Ready)
                    p1Buttons[p1SelectedButton].GetComponent<Button>().onClick.Invoke();
            }


            //player 2
            if (ControllerInput.controllers[1].Down == 1 && !p2Ready)
            {
                int prevButton = p2SelectedButton;

                p2SelectedButton = (p2SelectedButton + 1) % p2Buttons.Length;

                p2Buttons[p2SelectedButton].GetComponent<Image>().color = highlightedColor;
                p2Buttons[prevButton].GetComponent<Image>().color = normalColor;
            }
            else if (ControllerInput.controllers[1].Up == 1 && !p2Ready)
            {
                int prevButton = p2SelectedButton;

                p2SelectedButton = (p2SelectedButton + p2Buttons.Length - 1) % p2Buttons.Length;

                p2Buttons[p2SelectedButton].GetComponent<Image>().color = highlightedColor;
                p2Buttons[prevButton].GetComponent<Image>().color = normalColor;
            }

            if (ControllerInput.controllers[1].A == 1)
            {
                p2Buttons[p2SelectedButton].GetComponent<Button>().onClick.Invoke();
            }
            if (ControllerInput.controllers[1].B == 1)
            {
                if(p2Ready)
                    p2Buttons[p2SelectedButton].GetComponent<Button>().onClick.Invoke();
            }

            EnablePlayerDisabledScreens();

            CheckReady();
            EnableWaitingPanel();
            CheckChangeState();
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

                if(p1Ready)
                    p1Buttons[p1SelectedButton].GetComponent<Image>().color = readyColor;
                else
                    p1Buttons[p1SelectedButton].GetComponent<Image>().color = highlightedColor;

                break;
            case 2:
                p2Ready = !p2Ready;

                if (p2Ready)
                    p2Buttons[p2SelectedButton].GetComponent<Image>().color = readyColor;
                else
                    p2Buttons[p2SelectedButton].GetComponent<Image>().color = highlightedColor;

                break;
        }
    }

    void CheckReady()
    {
        if(    ( p1Ready || !ControllerInput.controllers[0].connected) 
            && ( p2Ready || !ControllerInput.controllers[1].connected)
            && ( ControllerInput.controllers[0].connected || ControllerInput.controllers[1].connected))
            soldiersReady = true;
		else
            soldiersReady = false;
    }

    void CheckChangeState()
    {
        if(soldiersReady)
        {
            AbilityManagerScript.instance.SetSoldierClass(1, p1SelectedClass);
            AbilityManagerScript.instance.SetSoldierClass(2, p2SelectedClass);

            //Tell the game manager soldiers are ready
            GameManagerScript.instance.readySoldiers = true;
        }
        else
        {
            GameManagerScript.instance.readySoldiers = false;
        }
    }

    void EnableWaitingPanel()
    {
        if(WaitingPanel.activeSelf != soldiersReady)
        {
            WaitingPanel.SetActive(soldiersReady);
        }
    }

    void EnablePlayerDisabledScreens()
    {
        if(p1DisabledScreen.activeSelf == ControllerInput.controllers[0].connected)
        {
            p1DisabledScreen.SetActive(!ControllerInput.controllers[0].connected);
        }

        if (p2DisabledScreen.activeSelf == ControllerInput.controllers[1].connected)
        {
            p2DisabledScreen.SetActive(!ControllerInput.controllers[1].connected);
        }
    }
}
