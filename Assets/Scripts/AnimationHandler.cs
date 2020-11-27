using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    public Animator knightAnimator; // a reference to the animator component

    public enum AnimationState {START, IDLE, WALKING, ATTACKLIGHT, ATTACKHEAVY, TAUNTING, BLOCKING, HIT, DEATH, VICTORY} // the different animation states
    public AnimationState currentAnimationState; // the current state the animator is in

    public AnimationState CurrentState // handles the current animation state
    {
        get
        {
            return currentAnimationState; // gets the current animation state
        }
        set
        {
            currentAnimationState = value; // set the animation state to the value

            if(knightAnimator != null)
            {
                UpdateAnimator(); // switches the animator between states
            }
            else
            {
                Debug.LogError("No animator has been assigned"); // prompts the user to assign an animator
            }
        }
    }

    /// <summary>
    ///  update the animator to match the current state of the character
    /// </summary>
    private void UpdateAnimator()
    {
        switch(currentAnimationState)
        {
            case AnimationState.START: // set the animator to the Start animation state
                {
                    ResetToIdle(); // reset the animator back to idle
                    knightAnimator.SetBool("START", true);
                    break;
                }
            case AnimationState.IDLE: // set the animator to the Idle animation state
                {
                    ResetToIdle();
                    break;
                }
            case AnimationState.WALKING: // set the animator to the Walking animation state (currently unused)
                {
                    ResetToIdle();
                    knightAnimator.SetBool("WALKING", true);
                    break;
                }
            case AnimationState.ATTACKLIGHT: // set the animator to the Light Attack animation state 
                {
                    ResetToIdle();
                    knightAnimator.SetBool("ATTACKLIGHT", true);
                    break;
                }
            case AnimationState.ATTACKHEAVY: // set the animator to the Heavy Attack animation state 
                {
                    ResetToIdle();
                    knightAnimator.SetBool("ATTACKHEAVY", true);
                    break;
                }
            case AnimationState.TAUNTING: // set the animator to the Taunting animation state 
                {
                    ResetToIdle();
                    knightAnimator.SetBool("TAUNTING", true);
                    break;
                }
            case AnimationState.BLOCKING: // set the animator to the Blocking animation state 
                {
                    ResetToIdle();
                    knightAnimator.SetBool("BLOCKING", true);
                    break;
                }
            case AnimationState.HIT: // set the animator to the Hit animation state 
                {
                    ResetToIdle();
                    knightAnimator.SetBool("HIT", true);
                    break;
                }
            case AnimationState.DEATH: // set the animator to the Death animation state 
                {
                    ResetToIdle();
                    knightAnimator.SetBool("DEATH", true);
                    break;
                }
            case AnimationState.VICTORY: // set the animator to the Victory animation state 
                {
                    ResetToIdle();
                    knightAnimator.SetBool("VICTORY", true);
                    break;
                }
        }
    }

    /// <summary>
    /// reset the animator to the idle state
    /// </summary>
    private void ResetToIdle()
    {
        knightAnimator.SetBool("START", false);
        // knightAnimator.SetBool("WALKING", false);
        knightAnimator.SetBool("ATTACKLIGHT", false);
        knightAnimator.SetBool("ATTACKHEAVY", false);
        knightAnimator.SetBool("TAUNTING", false);
        knightAnimator.SetBool("BLOCKING", false);
        knightAnimator.SetBool("HIT", false);
        knightAnimator.SetBool("DEATH", false);
        knightAnimator.SetBool("VICTORY", false);
    }
}
