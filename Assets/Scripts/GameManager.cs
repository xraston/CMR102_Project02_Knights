using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // references to the tracker scripts attached to the markers
    public Tracker1 trackerOneScript;
    public Tracker2 trackerTwoScript;
    // private bool trackingFound; // is the image target found or lost?

    // referneces to each Knight
    public GameObject knightA;
    public GameObject knightB;

    public AudioSource musicIntro; // introduction music
    public AudioSource musicLoop; // repeatable loop

    // UI references
    public GameObject canvasHelpText; // a reference to the players help textxt
    public GameObject trackerOneHelper; // reference to tracker one's explainer text
    public GameObject trackerTwoHelper; // reference to tracker two's explainer text
    public GameObject gameButtons; // reference to the 3 game buttons at the top centre of the UI
    private bool helpText = false; // switches the help text on and off

    // Update is called once per frame
    void Update()
    {
        TrackingHandler();
    }

    /// <summary>
    /// Handles the various states of both trackers
    /// </summary>
    void TrackingHandler()
    {
        // handles Tracker One
        if (trackerOneScript.trackerOneFound == false) // if the tracker is found
        {
            trackerOneHelper.SetActive(false); // turns off the tracker help text when found
            // knightA.SetActive(true); // actives the Knight
            knightA.GetComponent<KnightManager>().KnightFound(); // intiates the KnightManager startup function
        }
        if (trackerOneScript.trackerOneFound == true) // if the tracker is lost
        {
            trackerOneHelper.SetActive(true); // turns off the help text
            // knightA.SetActive(false); // deactivates the Knight
        }

        // handles Tracker Two
        if (trackerTwoScript.trackerTwoFound == false)
        {
            trackerTwoHelper.SetActive(false);
            // knightB.SetActive(true);
            knightB.GetComponent<KnightManager>().KnightFound();
        }
        if (trackerTwoScript.trackerTwoFound == true)
        {
            trackerTwoHelper.SetActive(true);
            // knightB.SetActive(false);
        }

        // if Tracker One & Two are found
        if (trackerOneScript.trackerOneFound == false && trackerTwoScript.trackerTwoFound == false) 
        {
            gameButtons.SetActive(true); // display the game buttons
        }
        // if either trackers are found
        if (trackerOneScript.trackerOneFound == false || trackerTwoScript.trackerTwoFound == false)
        {
            AudioListener.pause = false; // resume the audio
            Time.timeScale = 1; //  start or resume the game
            PlayMusic();
        }
        // if no trackers are found
        if (trackerOneScript.trackerOneFound == true && trackerTwoScript.trackerTwoFound == true)
        {
            Time.timeScale = 0; // pause the game
            AudioListener.pause = true; // pause the audio
            gameButtons.SetActive(false); // hide the game buttons
        }
    }

    public void PlayMusic()
    {
        if (musicIntro.isPlaying == false && musicLoop.isPlaying == false)
        {
            musicIntro.Play();
            musicLoop.PlayDelayed(musicIntro.clip.length);
        }
        else if (musicIntro.isPlaying == true && musicLoop.isPlaying == false)
        {
            musicLoop.PlayDelayed(musicIntro.clip.length);
        }
    }

    /// <summary>
    /// Turns the Button Help Text on and off
    /// </summary>
    public void HelpButton()
    {
        if (helpText == false) 
        {
            canvasHelpText.SetActive(true); // shows the button help text
            helpText = true; // switches the bool
        }
        else if (helpText == true)
        {
            canvasHelpText.SetActive(false); // hides the button help text
            helpText = false;
        }
    }

    public void QuitApplication() // quits the Android application with a button press
    {
        Application.Quit();
    }

}
