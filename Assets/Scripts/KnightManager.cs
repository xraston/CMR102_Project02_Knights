using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class KnightManager : MonoBehaviour
{
    // references to things in Unity
    public GameObject thisKnight; // reference to the knight's rigidbody
    public AudioSource knightAudio; // reference to the knight's audio
    public AnimationHandler animationHandler; // a reference to the animation handler script
    public Animator knightAnimator; // a reference to the animator
    public GameObject knightControls; // a reference to the UI controls
    public GameObject knightHealthBar; // a reference to the health bar

    // references to the opposing knight
    public GameObject opponent;
    public int distanceToDetectOpponent = 2;

    // health and damage variables
    private int attackLightDMG;
    private int attackHeavyDMG;
    public int knightHealthCurrent = 100;
    public int knightHealthTotal = 100;
    public int healAmount = 10;

    public Sword sword;

    // public int damageAmount;

    Vector3 startPosition;

    // idle variables
    public float idleTime = 3; // the time the knights waits until the alternative animation plays
    private float currentIdleWaitTime; // the the current amount of time the knight has idled

    // the different state the knight can be in
    public enum CharacterStates {Start, Idle, Walking, Attacking, Taunting, Blocking, Hit, End }
    public CharacterStates currentCharacterState; // the current that that the character is in


    // Start is called before the first frame update
    void Start()
    {
        // replace with ontrackerenter for testing
        knightAnimator.Play("Sword_Draw"); 
        currentCharacterState = CharacterStates.Start; // set the character by default to idle
        animationHandler.CurrentState = AnimationHandler.AnimationState.START; // set the animation state to idle
        startPosition = transform.position; // change to ontracker?
        currentIdleWaitTime = Time.time + idleTime;
    }

    // Update is called once per frame
    void Update()
    {
        // references to the character states
        HandleStartState();
        HandleIdleState();
        HandleWalkingState();
        HandleTauntingState();
        HandleAttackingState();
        // HandleBlockingState();
        // HandleHitState();
        HandleEndState();
    }

    private void HandleStartState() // ADD on tracker start & ADD Animation
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.START)
        {
            currentCharacterState = CharacterStates.Start; // set the character state to start
            knightHealthBar.SetActive(true);
            knightControls.SetActive(false);
            // Invoke("PlayStretchAnimation", 6);
            // Invoke("ResetToStart", 6);
            if (Vector3.Distance(transform.position, opponent.transform.position) < distanceToDetectOpponent)
            {
                knightAnimator.Play("Taunt_Point");
                animationHandler.CurrentState = AnimationHandler.AnimationState.IDLE;
            }
        }

    }

    private void HandleIdleState() // FIX ANIMATION
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.IDLE)
        {
            currentCharacterState = CharacterStates.Idle; // set the character state to idle
            knightControls.SetActive(true);
            if (Vector3.Distance(transform.position, opponent.transform.position) < distanceToDetectOpponent) // if the opponent is close enough
            {
                // always look at the opponent
                Vector3 directionToLookAt = opponent.transform.position - transform.position;
                directionToLookAt.y = 0; // doesn't change the Y postion
                Quaternion rotationOfDirection = Quaternion.LookRotation(directionToLookAt); // get a rotation value the character can look towards
                transform.rotation = rotationOfDirection; // set the current rotation to face the target position
                
            }
            if (Vector3.Distance(transform.position, opponent.transform.position) > distanceToDetectOpponent)
            {
                ResetToStartState();
            }
            if(Time.time >= currentIdleWaitTime)
            {
                currentIdleWaitTime = Time.time + idleTime;
                // knightAnimator.Play("Idle_Look");
            }
        }  
    }

    private void HandleWalkingState() // FINISH
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.WALKING)
        {
            currentCharacterState = CharacterStates.Walking; // set the character state to walking
            // play audio
        }
    }

    private void HandleTauntingState() // FINISH
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.TAUNTING)
        {
            currentCharacterState = CharacterStates.Taunting; // set the character state to attack

            knightHealthCurrent = knightHealthCurrent + healAmount;
            // play audio
        }
    }

    public int AttackDamage(int min, int max)
    {
        return Random.Range(min, max);
    }

    private void HandleAttackingState()
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKLIGHT || animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKHEAVY)
        {
            currentCharacterState = CharacterStates.Attacking; // set the character state to attack
            if (animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKLIGHT)
            {
                sword.damage = attackLightDMG =  AttackDamage(10, 15); // gets a random number in this range
                // Debug.Log("Light Attack Damage: " + attackLightDMG);
            }
            if (animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKHEAVY)
            {
                sword.damage = attackHeavyDMG = AttackDamage(20, 25);
                // Debug.Log("Heavy Attack Damage: " + attackHeavyDMG);
            }
        }
    }

    public void ApplyDamage(int damageAmount)
    {
        if(currentCharacterState != CharacterStates.Blocking)
        {
            knightHealthCurrent = knightHealthCurrent - damageAmount;
            Debug.Log(damageAmount);
        }
        else
        {
            knightHealthCurrent = knightHealthCurrent - damageAmount + AttackDamage(9, 13); // reduces incoming damage by a random range
            Debug.Log(damageAmount);
        }

    }

    private void HandleBlockingState() // FINISH
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.BLOCKING)
        {
            currentCharacterState = CharacterStates.Blocking; // set the character state to walking
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Sword")
        {
            if (currentCharacterState == CharacterStates.Blocking)
            {
                // play block_hit animation once
            }
            else
            {
                HandleHitState();
            }
        }
    }

        private void HandleHitState() // FINISH
    {
        // play audio of hit
        // play the hit animation
        // activate particle effect
    }

    /// <summary>
    /// handles the end of the game when one character's health reaches zero or below
    /// </summary>
    private void HandleEndState() // FIX animations
    {
        if(knightHealthCurrent <= 0)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.DEATH; // set the animation state to end
            currentCharacterState = CharacterStates.End; // set the character by default to end
            knightControls.SetActive(false);
            // opponent.GetComponent<AnimationHandler>().currentAnimationState = AnimationState.VICTORY;
            // play Defeat audio clip?
        }
        if (opponent.GetComponent<KnightManager>().knightHealthCurrent <= 0)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.VICTORY; // set the animation state to end
            currentCharacterState = CharacterStates.End; // set the character by default to end
            knightControls.SetActive(false);
            // play Victory audio clip
            // add a delay between animations
        }
    }

    public void ResetGame()
    {
        Debug.Log("Game Over");
        transform.position = startPosition;
        knightHealthCurrent = knightHealthTotal;
        opponent.GetComponent<KnightManager>().knightHealthCurrent = knightHealthTotal;
        animationHandler.CurrentState = AnimationHandler.AnimationState.START;
    }


    public void ResetToIdleState()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.IDLE;
        currentCharacterState = CharacterStates.Idle;
    }

    public void ResetToStartState()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.START;
        currentCharacterState = CharacterStates.Start;
    }
}
