using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject playerOneCanvas; // a reference to the players canvas
    public GameObject playerOneControls; // a reference to the players control buttons
    public GameObject playerOneHelpText; // a reference to the players help text

    public GameObject playerTwoCanvas; // a reference to the players canvas object
    public GameObject playerTwoControls; // a reference to the players control buttons
    public GameObject playerTwoHelpText; // a reference to the players help text

    public KnightManager knightManager;

    private void ShowHideCanvas()
    {
        // when tracking is found show the canvas

        // if the knight is in the idle state, lower the opacity of the controls
        // if the knight is in the fight state, show the controls
    }

    private void DisableControls()
    {
        // knightManager.currentCharacterState = knightManager.CharacterStates.Start;
    }

    private void EnableControls()
    {
        // CanvasObject.GetComponent<Canvas> ().enabled = true; 
    }

    private void UpdateHealthBar()
    {
        // gets the knight's max health and compares it to the current current health 

        // moves the green box position left as a percentage of the missing health

        // if current health reaches 0 or less it stops moving

        // if reset button is pressed then move the green box to the starting postion
    }

    private void PausePlayButton()
    {
        // work out what this should do?
    }

    public void HelpButton()
    {
        if(playerOneHelpText.activeInHierarchy) // will this work on mobile?
        {
            playerOneHelpText.SetActive(false);
        }
        else
        {
            playerOneHelpText.SetActive(true);
        }
    }

    private void RestartButton()
    {
        // this is handled in the knight manager script
    }
}
