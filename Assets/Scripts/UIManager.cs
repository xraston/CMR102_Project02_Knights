using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Vuforia;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class UIManager : MonoBehaviour
{
    public TrackableBehaviour trackableBehaviour; // a reference to our AR marker script
    public bool trackerOneFound = false; // have we detected the first tracker?
    public bool trackerTwoFound = false; // have we detected the second tracker?

    public GameObject playerOneHelpText; // a reference to the players help text
    public GameObject playerTwoHelpText; // a reference to the players help text

    // Start is called before the first frame update
    void Start()
    {
        // if the reference has been assigned
        if (trackableBehaviour != null)
        {
            // trackableBehaviour.RegisterTrackableEventHandler(this);
        }
        OnTrackerOneLost(); // hide knight 1 and pause game and audio
        OnTrackerTwoLost(); // hide knight 2
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// A Vuforia function we need to implement to detect changes with the makers
    /// </summary>
    /// <param name="previousStatus"></param>
    /// <param name="newStatus"></param>
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        // checking the current status of the marker
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (trackerOneFound == true)
            {
                trackerOneFound = false;
                OnTrackerOneFound(); // we got tracking
            }
            if (trackerTwoFound == true)
            {
                trackerTwoFound = false;
                OnTrackerTwoFound(); // we got tracking
            }
        }
        else
        {
            if (trackerOneFound == false)
            {
                trackerOneFound = true;
                OnTrackerOneLost(); // we lost tracking
            }
            if (trackerTwoFound == false)
            {
                trackerTwoFound = true;
                OnTrackerTwoLost(); // we lost tracking
            }
        }
    }

    /// <summary>
    /// what we want to do when marker ONE is found
    /// </summary>
    private void OnTrackerOneFound()
    {
        // enable knight 1 and it's health bar
        Time.timeScale = 1; // resume the game
        AudioListener.pause = false; // resume the audio

    }

    /// <summary>
    /// what we want to do when marker ONE is lost
    /// </summary>
    private void OnTrackerOneLost()
    {
        /// disable knight 1 and it's health bar
        Time.timeScale = 0; // pause the game
        AudioListener.pause = true; // pause the audio
    }

    /// <summary>
    /// what we want to do when marker TWO is found
    /// </summary>
    private void OnTrackerTwoFound()
    {
        // enable knight 2, it's health bar and the game buttons

    }

    /// <summary>
    /// what we want to do when marker TWO is lost
    /// </summary>
    private void OnTrackerTwoLost()
    {
        // disable knight 2, it's health bar and the game buttons
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
