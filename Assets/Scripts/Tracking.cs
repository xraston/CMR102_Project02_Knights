using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Vuforia;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class Tracking : MonoBehaviour, ITrackableEventHandler
{
    public TrackableBehaviour trackableBehaviour; // a reference to our AR marker script
    public bool trackerFound = false; // have we detected a tracker?

    // Start is called before the first frame update
    void Start()
    {
        // if the reference has been assigned
        if (trackableBehaviour != null)
        {
            trackableBehaviour.RegisterTrackableEventHandler(this);
        }
        OnTrackingLost(); // hide everything
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
            if (trackerFound == true)
            {
                trackerFound = false;
                OnTrackingFound(); // we got tracking
            }
        }
        else
        {
            if (trackerFound == false)
            {
                trackerFound = true;
                OnTrackingLost(); // we lost tracking
            }
        }
    }

    /// <summary>
    /// what we want to do when the marker is found
    /// </summary>
    private void OnTrackingFound()
    {
        // do something
        Time.timeScale = 1; // resume the game
        AudioListener.pause = false; // resume the audio
    }

    /// <summary>
    /// what we want to do when the marker is lost
    /// </summary>
    private void OnTrackingLost()
    {
        // do something
        Time.timeScale = 0; // pause the game
        AudioListener.pause = true; // pause the audio
    }
}
