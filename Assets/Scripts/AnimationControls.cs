using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControls : MonoBehaviour
{
    public Animator knightAnimator; // a reference to the animator
    public AnimationHandler animationHandler; // a reference to the animation handler script

    public float moveSpeed = 1.75f; // defines the move speed per frame

    public GameObject guiBlocker; // a UI element that temporarily blocks input on the screen during animations

    public bool walkForwardButton = false;
    public bool walkBackwardButton = false;
    public bool walkLeftButton = false;
    public bool walkRightButton = false;

    // Update is called once per frame
    void Update()
    {
        // walk controls may be re-implemented in future versions
        if(walkForwardButton == true)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.WALKING;
            knightAnimator.Play("Walk_Forward");
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (walkBackwardButton == true)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.WALKING;
            knightAnimator.Play("Walk_Backward");
            transform.position += -transform.forward * moveSpeed * Time.deltaTime;
        }
        if (walkLeftButton == true)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.WALKING;
            knightAnimator.Play("Walk_Left");
            transform.position += -transform.right * moveSpeed * Time.deltaTime;
        }
        if (walkRightButton == true)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.WALKING;
            knightAnimator.Play("Walk_Right");
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }
    }

    // walk controls may be re-implemented in future versions
    public void AnimationWalkForwardTrue()
    {
        walkForwardButton = true;
    }
    public void AnimationWalkForwardFalse()
    {
        walkForwardButton = false;
        ResetToIdle();
    }
    public void AnimationWalkBackwardTrue()
    {
        walkBackwardButton = true;
    }
    public void AnimationWalkBackwardFalse()
    {
        walkBackwardButton = false;
        ResetToIdle();
    }
    public void AnimationWalkLeftTrue()
    {
        walkLeftButton = true;
    }
    public void AnimationWalkLeftFalse()
    {
        walkLeftButton = false;
        ResetToIdle();
    }
    public void AnimationWalkRightTrue()
    {
        walkRightButton = true;
    }
    public void AnimationWalkRightFalse()
    {
        walkRightButton = false;
        ResetToIdle();
    }

    public void AnimationAttackLight() // triggers the light attack
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.ATTACKLIGHT; // calls the light attack animation state
        guiBlocker.SetActive(true); // activates the UI element to block input
        Invoke("UnblockUI", 1); // deactivates the UI block after 1 second
        Invoke("ResetToIdle", 1); // resets the animation state to Idle after 1 second
    }

    public void AnimationAttackHeavy() // triggers the heavy attack
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.ATTACKHEAVY;
        guiBlocker.SetActive(true);
        Invoke("UnblockUI", 1);
        Invoke("ResetToIdle", 1);
    }

    public void AnimationTaunting() // triggers the taunt
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.TAUNTING;
        guiBlocker.SetActive(true);
        Invoke("UnblockUI", 2);
        Invoke("ResetToIdle", 2);
    }

    public void AnimationBlocking() // triggers the block
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.BLOCKING;
    }


    public void ResetToIdle() // resets animation state to idle
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.IDLE;
    }

    public void UnblockUI() // deactivates the temporary UI input blocker
    {
        guiBlocker.SetActive(false);
    }
}
