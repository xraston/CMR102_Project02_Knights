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
    public GameObject knightMoveControls; // a reference to the UI controls
    public GameObject knightCombatControls; // a reference to the UI controls
    public GameObject knightHealthBar; // a reference to the health bar

    public GameObject bloodPrefab;
    public Transform bloodPosition;

    // references to the opposing knight
    public GameObject opponent;
    public int distanceToDetectOpponent;

    // health and damage variables
    private int attackLightDMG;
    private int attackHeavyDMG;
    public int knightHealthCurrent;
    public int knightHealthTotal;
    public int healAmount;

    public Sword sword;

    // public int damageAmount;

    Vector3 startPosition;

    // idle variables
    public float idleTime = 5; // the time the knights waits until the alternative animation plays
    private float currentIdleWaitTime; // the the current amount of time the knight has idled
    public float startIdleTime = 5; // the time the knights waits until the alternative animation plays
    private float currentStartWaitTime; // the the current amount of time the knight has idled

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
        currentStartWaitTime = Time.time + startIdleTime + AttackDamage(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(knightHealthCurrent + "opponent " + opponent.GetComponent<KnightManager>().knightHealthCurrent);
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
            // manage the UI
            knightHealthBar.SetActive(true);
            knightMoveControls.SetActive(true);
            knightCombatControls.SetActive(false);
            // when the knight gets close enough to the opponent enter the Idle State
            if (Vector3.Distance(transform.position, opponent.transform.position) < distanceToDetectOpponent)
            {
                // knightAnimator.SetBool("Idle", true);
                animationHandler.CurrentState = AnimationHandler.AnimationState.IDLE;
            }
            // plays extra animations
            else if (Time.time >= currentStartWaitTime)
            {
                knightAnimator.SetBool("ExitAnim2", false);
                Invoke("ExitAnimation", 5);
                currentStartWaitTime = Time.time + idleTime + 5;
            }
        }
    }

    private void HandleIdleState() // FIX ANIMATION
    {   
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.IDLE)
        {
            currentCharacterState = CharacterStates.Idle; // set the character state to idle
            // manage the UI
            knightHealthBar.SetActive(true);
            knightMoveControls.SetActive(true);
            knightCombatControls.SetActive(true);
            // always look at the opponent
            Vector3 directionToLookAt = opponent.transform.position - transform.position;
            directionToLookAt.y = 0; // doesn't change the Y postion
            Quaternion rotationOfDirection = Quaternion.LookRotation(directionToLookAt); // get a rotation value the character can look towards
            transform.rotation = rotationOfDirection; // set the current rotation to face the target position
            // plays extra animations
            if (Time.time >= currentIdleWaitTime)
                {
                    knightAnimator.Play("Taunt_Point");
                    //Invoke("EnterAnimation", 1);
                    if (currentCharacterState == CharacterStates.Taunting || currentCharacterState == CharacterStates.Attacking || currentCharacterState == CharacterStates.Blocking)
                    {
                        // make it so combat buttons can interrupt
                        knightAnimator.SetBool("ExitAnim1", true);
                        InterruptAnimation();
                    }
                    // Invoke("ExitAnimation", 2);
                    currentIdleWaitTime = Time.time + idleTime + 5;
            }
            // if the oppoenent is too far away return to the Start state
            if (Vector3.Distance(transform.position, opponent.transform.position) > distanceToDetectOpponent)
            {
                ResetToStartState();
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
                sword.damage = attackLightDMG =  AttackDamage(20, 25); // gets a random number in this range
                // Debug.Log("Light Attack Damage: " + attackLightDMG);
            }
            if (animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKHEAVY)
            {
                sword.damage = attackHeavyDMG = AttackDamage(30, 35);
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
            knightHealthCurrent = knightHealthCurrent - damageAmount + AttackDamage(20, 25); // reduces incoming damage by a random range
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
                knightAnimator.Play("Block_Hit");
                // audioSource.PlayOneShot(fireWorkSound);
            }
            else
            {
                HandleHitState();
            }
        }
    }

    private void HandleHitState() // FINISH
    {
        currentCharacterState = CharacterStates.Hit; // set the character state to walking
        knightAnimator.Play("Hit_Strong");
        GameObject clone = Instantiate(bloodPrefab, bloodPosition.position, bloodPosition.transform.rotation);
        Destroy(clone, 1); // this destroys the instantiated clone object after 5 seconds
        // audioSource.PlayOneShot(fireWorkSound);
        Invoke("ResetToIdle", 1);
        
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
            knightMoveControls.SetActive(false);
            knightCombatControls.SetActive(false);
            // opponent.GetComponent<AnimationHandler>().currentAnimationState = AnimationState.VICTORY;
            // play Defeat audio clip?
        }
        if (opponent.GetComponent<KnightManager>().knightHealthCurrent <= 0)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.VICTORY; // set the animation state to end
            currentCharacterState = CharacterStates.End; // set the character by default to end
            knightMoveControls.SetActive(false);
            knightCombatControls.SetActive(false);
            // play Victory audio clip
            // add a delay between animations
        }
    }

    public void ResetGame()
    {
        Debug.Log("Game Reset");
        transform.position = startPosition;
        knightHealthCurrent = 282;
        opponent.GetComponent<KnightManager>().knightHealthCurrent = 282;
        opponent.GetComponent<AnimationHandler>().CurrentState = AnimationHandler.AnimationState.START;
        knightAnimator.SetBool("VICTORY", false);
        knightAnimator.SetBool("DEATH", false);
        ResetToStartState();
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

    public void InterruptAnimation()
    {
        knightAnimator.SetBool("ExitAnim1", true);
        knightAnimator.SetBool("ExitAnim2", true);
        knightAnimator.SetBool("ATTACKLIGHT", true);
        knightAnimator.SetBool("ATTACKHEAVY", true);
        knightAnimator.SetBool("TAUNTING", true);
        knightAnimator.SetBool("BLOCKING", true);
    }

    public void ExitAnimation()
    {
        knightAnimator.SetBool("ExitAnim1", true);
        knightAnimator.SetBool("ExitAnim2", true);
    }
    public void EnterAnimation()
    {
        knightAnimator.SetBool("ExitAnim1", false);
        knightAnimator.SetBool("ExitAnim2", false);
    }
}
