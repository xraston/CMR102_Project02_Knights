using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{

    public Animator knightAnimator; // a reference to the animator component

    public enum AnimationState {START, IDLE, WALKING, ATTACKLIGHT, ATTACKHEAVY, TAUNTING, BLOCKING, HIT, DEATH, VICTORY} // the different animation states
    public AnimationState currentAnimationState; // the current state the animator is in

    public AnimationState CurrentState
    {
        get
        {
            return currentAnimationState;
        }
        set
        {
            currentAnimationState = value; // set the animation state to the value

            if(knightAnimator != null)
            {
                UpdateAnimator();
            }
            else
            {
                Debug.LogError("No animator has been assigned");
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
            case AnimationState.START:
                {
                    // reset the animator back to start animation
                    ResetToIdle();
                    knightAnimator.SetBool("START", true);
                    break;
                }
            case AnimationState.IDLE:
                {
                    // reset the animator back to idle animation
                    ResetToIdle();
                    break;
                }
            case AnimationState.WALKING:
                {
                    // set the animator to the walking animation
                    ResetToIdle();
                    knightAnimator.SetBool("WALKING", true);
                    break;
                }
            case AnimationState.ATTACKLIGHT:
                {
                    // set the animator to the light attack animation
                    ResetToIdle();
                    knightAnimator.SetBool("ATTACKLIGHT", true);
                    break;
                }
            case AnimationState.ATTACKHEAVY:
                {
                    // set the animator to the heavy attack animation
                    ResetToIdle();
                    knightAnimator.SetBool("ATTACKHEAVY", true);
                    break;
                }
            case AnimationState.TAUNTING:
                {
                    // set the animator to the taunting animation
                    ResetToIdle();
                    knightAnimator.SetBool("TAUNTING", true);
                    break;
                }
            case AnimationState.BLOCKING:
                {
                    // set the animator to the blocking animation
                    ResetToIdle();
                    knightAnimator.SetBool("BLOCKING", true);
                    break;
                }
            case AnimationState.HIT:
                {
                    // set the animator to the hit animation
                    ResetToIdle();
                    knightAnimator.SetBool("HIT", true);
                    break;
                }
            case AnimationState.DEATH:
                {
                    // set the animator to the death animation
                    ResetToIdle();
                    knightAnimator.SetBool("DEATH", true);
                    break;
                }
            case AnimationState.VICTORY:
                {
                    // set the animator to the victory animation
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
        knightAnimator.SetBool("WALKING", false);
        knightAnimator.SetBool("ATTACKLIGHT", false);
        knightAnimator.SetBool("ATTACKHEAVY", false);
        knightAnimator.SetBool("TAUNTING", false);
        knightAnimator.SetBool("BLOCKING", false);
        knightAnimator.SetBool("HIT", false);
        knightAnimator.SetBool("DEATH", false);
        knightAnimator.SetBool("VICTORY", false);
    }
}
