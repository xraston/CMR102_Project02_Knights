using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Vuforia;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class Tracker1 : MonoBehaviour, ITrackableEventHandler
{
    public TrackableBehaviour trackableOneBehaviour; // a reference to our AR marker script
    public bool trackerOneFound = false; // have we detected a tracker?

    // Start is called before the first frame update
    void Start()
    {
        if (trackableOneBehaviour != null) // if the reference has been assigned
        {
            trackableOneBehaviour.RegisterTrackableEventHandler(this);
        }
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
            }
        }
        else
        {
            if (trackerOneFound == false)
            {
                trackerOneFound = true;
            }
        }
    }
}
