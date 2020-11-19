using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControls : MonoBehaviour
{
    public Animator knightAnimator; // a reference to the animator
    public AnimationHandler animationHandler; // a reference to the animation handler script

    public float moveSpeed = 1.75f; // defines the move speed per frame

    public GameObject guiBlocker;

    public bool walkForwardButton = false;
    public bool walkBackwardButton = false;
    public bool walkLeftButton = false;
    public bool walkRightButton = false;

    // Update is called once per frame
    void Update()
    {
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

    public void AnimationAttackLight()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.ATTACKLIGHT;
        guiBlocker.SetActive(true);
        Invoke("UnblockUI", 1);
        Invoke("ResetToIdle", 1);
    }

    public void AnimationAttackHeavy()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.ATTACKHEAVY;
        guiBlocker.SetActive(true);
        Invoke("UnblockUI", 1);
        Invoke("ResetToIdle", 1);
    }

    public void AnimationTaunting()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.TAUNTING;
        guiBlocker.SetActive(true);
        Invoke("UnblockUI", 2);
        Invoke("ResetToIdle", 2);
    }

    public void AnimationBlocking()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.BLOCKING;
    }

    // resets animation state to idle
    public void ResetToIdle()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.IDLE;
    }

    public void UnblockUI()
    {
        guiBlocker.SetActive(false);
    }
}
