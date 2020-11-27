using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class KnightManager : MonoBehaviour
{
    // Animators and the animation script references
    public AnimationHandler animationHandler; // the animation handler script
    public Animator knightAnimator; // the animator

    // UI Canvas
    public GameObject knightCombatControls; // the Knights' combat buttons
    public GameObject trackerHelp; // the tracker instructions

    // blood particle effect
    public GameObject bloodPrefab; // the blood effect prefab
    public Transform bloodPosition; // the postion & rotation the particle start from

    // sound effects
    public AudioSource audioSource; // the knight's audio
    public AudioSource swordSwoosh; // the sword's audio
    public AudioClip bodyhit; // the on hit sound
    public AudioClip shieldhit; // the shield hit sound
    public AudioClip sworddraw; // the sword draw sound
    // voice lines
    public AudioSource voiceStart; // voiceline at the start
    public AudioSource voiceIdle; // voice for idle's extra animation 
    public AudioSource voiceHit; // plays when the Knight gets hit
    public AudioSource voiceVictory; // plays when the Knight wins
    public AudioSource voiceDeath; // plays when the Knight dies
    public AudioSource voiceTaunt; // plays when the taunt button is pressed
    public AudioSource voiceBlock; // plays when the taunt button is held

    // the opposing knight
    public GameObject opponent; // the opposite knight
    public float distanceToDetectOpponent; // used to measure the distance between Knights

    // health and damage variables
    public GameObject knightHealthBar; // a reference to the health bar
    public int knightHealthCurrent; // the current health that is updated for damage
    public int knightHealthTotal; // the starting health pool
    private int attackLightDMG; // unused
    private int attackHeavyDMG; // unused
    public int blockAmount = 20; // damage reduction when the knight is blocking

    // sword references
    public Sword sword; // the sword's script
    public GameObject swordCollider; // the sword's collider

    // idle animation variables
    public float idleTime = 5; // the time the Knights waits until the alternative animation plays
    private float currentIdleWaitTime; // the the current amount of time the Knight has idled

    // the different states the knight can be in
    public enum CharacterStates {Start, Idle, Walking, Attacking, Taunting, Blocking, Hit, End }
    public CharacterStates currentCharacterState; // the current state that the Knight is in

    // reference to the knight's tracker
    public GameObject knightTracker; // the image target
    public GameManager gameManager; // the game manager
    private bool trackingFound; // is the image target found or lost?


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDisable() // changes the tracker's status
    {
        trackingFound = false;
    }

    public void KnightFound() // if the Knight's tracker is found
    {
        if (trackingFound != true)
        {
            trackingFound = true;
        }
        else
        {
            return;
        }
        knightAnimator.Play("Sword_Draw"); // play the sword draw animation at the start
        audioSource.PlayOneShot(sworddraw); // play the sword draw sound
        voiceStart.Play(); // play the opening voiceline
        animationHandler.CurrentState = AnimationHandler.AnimationState.START; // set the animation state to idle
        Invoke("EnterAnimation", 1);
        if (currentCharacterState == CharacterStates.Taunting || currentCharacterState == CharacterStates.Attacking || currentCharacterState == CharacterStates.Blocking)
        {
            //knightAnimator.SetBool("ExitAnim2", true);
            InterruptAnimation(); // resets the transition conditions 
        }
    }

    // Update is called once per frame
    void Update()
    {
        // references to the character states
        HandleStartState();
        HandleIdleState();
        HandleAttackingState();
        HandleBlockingState();
        HandleTauntingState();
        HandleEndState();
    }

    /// <summary>
    /// Handles the Start Animation State
    /// </summary>
    private void HandleStartState()
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.START) // if the character enters the Start animation state
        {
            currentCharacterState = CharacterStates.Start; // set the character state to start
            swordCollider.SetActive(true); // activates the sword collider if it is off
            // manage the UI
            knightHealthBar.SetActive(true); // activates the health bar
            knightCombatControls.SetActive(false); // deactivates the combar controls if the Knight is too far away
            trackerHelp.SetActive(true); // displayers the 'move trackers closer' help text
            // when the knight gets close enough to the opponent enter the Idle State
            if (Vector3.Distance(transform.position, opponent.transform.position) < distanceToDetectOpponent)
            {
                knightAnimator.SetBool("Idle", true);
                animationHandler.CurrentState = AnimationHandler.AnimationState.IDLE; // moves to the Idle state
            }
            if (currentCharacterState == CharacterStates.Taunting || currentCharacterState == CharacterStates.Attacking || currentCharacterState == CharacterStates.Blocking)
            {
                //knightAnimator.SetBool("ExitAnim2", true);
                InterruptAnimation(); // resets the transition conditions 
            }
            // Invoke("ExitAnimation", 6); // exits the extra animation after it's finished playing
        }
    }

    private void HandleIdleState() // FIX ANIMATION
    {   
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.IDLE) // if the character enters the Idle animation state
        {
            currentCharacterState = CharacterStates.Idle; // set the character state to idle
            swordCollider.SetActive(true); // activates the sword
            // manage the UI
            trackerHelp.SetActive(false); // deactivates the tracker help text
            knightCombatControls.SetActive(true); // activates the combat controls
            // always look at the opponent
            Vector3 directionToLookAt = opponent.transform.position - transform.position; // updates the postion of the Knight (for movement)
            directionToLookAt.y = 0; // doesn't change the Y postion
            Quaternion rotationOfDirection = Quaternion.LookRotation(directionToLookAt); // get a rotation value the character can look towards
            transform.rotation = rotationOfDirection; // set the current rotation to face the target position
            // allows combat buttons to interrupt the animation state
            if (currentCharacterState == CharacterStates.Taunting || currentCharacterState == CharacterStates.Attacking || currentCharacterState == CharacterStates.Blocking)
            {
                InterruptAnimation(); // resets the transition conditions 
            }
            // plays extra animations
            if (Time.time >= currentIdleWaitTime) // if enough time has passed
            {
                knightAnimator.Play("Taunt_Point"); // the Knights point at each other
                voiceIdle.Play(); // plays the Idle voiceline
                // allows combat buttons to interrupt this extra animation
                if (currentCharacterState == CharacterStates.Taunting || currentCharacterState == CharacterStates.Attacking || currentCharacterState == CharacterStates.Blocking)
                    {
                        knightAnimator.SetBool("ExitAnim1", true);
                        InterruptAnimation(); // resets the transition conditions 
                    }
                currentIdleWaitTime = Time.time + idleTime + 6; // updates the time to next play the extra animation
            }
            // if the oppoenent is too far away return to the Start state
            if (Vector3.Distance(transform.position, opponent.transform.position) > distanceToDetectOpponent)
            {
                ResetToStartState();
            }
        }  
    }

    private void HandleWalkingState() // this is now defunct but may be re-implemented in the future
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.WALKING)
        {
            currentCharacterState = CharacterStates.Walking; // set the character state to walking
        }
    }

    /// <summary>
    /// Handles the Taunting animation state
    /// </summary>
    private void HandleTauntingState()
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.TAUNTING)
        {
            currentCharacterState = CharacterStates.Taunting; // set the character state to attack    
            if (voiceTaunt.isPlaying == false) // if the audio is not already playing
            {
                StopAllAudio(); // resets the audio
                voiceTaunt.Play(); // play the voice taunt
            }        
        }
    }

    /// <summary>
    /// Handles the Attacking animation states
    /// </summary>
    private void HandleAttackingState()
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKLIGHT || animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKHEAVY) // light or heavy attack
        {
            swordCollider.SetActive(true); // activates the sword collider if it is off
            currentCharacterState = CharacterStates.Attacking; // set the character state to attacking
            if (swordSwoosh.isPlaying == false)
            {
                StopAllAudio();
                swordSwoosh.Play();
            }
            if (animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKLIGHT) // for the light attack
            {
                sword.damage = attackLightDMG =  AttackDamage(20, 25); // gets a random number in this range
                // Debug.Log("Light Attack Damage: " + attackLightDMG);
            }
            if (animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKHEAVY) // for the heavy attack
            {
                sword.damage = attackHeavyDMG = AttackDamage(30, 35);
                // Debug.Log("Heavy Attack Damage: " + attackHeavyDMG);
            }
        }
    }

    /// <summary>
    /// Generates a random number within the input range for attacks
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int AttackDamage(int min, int max)
    {
        return Random.Range(min, max);
    }

    /// <summary>
    /// Handles the amount of damage the Knight takes
    /// </summary>
    /// <param name="damageAmount"></param>
    public void ApplyDamage(int damageAmount) // takes amount of damage from the Sword script when it enters the collider
    {
        if (currentCharacterState == CharacterStates.Blocking) // if the Knight is blocking
        {
            knightAnimator.Play("Block_Hit"); // plays the shield block animation
            audioSource.PlayOneShot(shieldhit, 0.25f); // plays the shield hit sound at half volume
            knightHealthCurrent = knightHealthCurrent - (damageAmount - blockAmount); // reduces the incoming damage by the amount specified
            // Debug.Log("HIT " + damageAmount + " " + blockAmount);
        }
        else // if the Knight is not blocking
        {
            knightHealthCurrent = knightHealthCurrent - damageAmount; // takes the damage amount from the knights current health and updates it
            HandleHitState(); // executes the on hit animation state
            // Debug.Log("HIT " + damageAmount);
        }
        if (knightHealthCurrent > 282) // if the Knight somehow goes over full health
        {
            knightHealthCurrent = 282; // return to max health
        }
    }

    /// <summary>
    /// Handles the Hit animation state when the Knight's collider is hit by the sword collider
    /// </summary>
    private void HandleHitState()
    {
        currentCharacterState = CharacterStates.Hit; // set the character state to being hit
        knightAnimator.Play("Hit_Strong"); // plays the hit animation
        voiceHit.Play(); // plays the voiceline
        audioSource.PlayOneShot(bodyhit); // plays the sound effect of the knight being wounded
        GameObject clone = Instantiate(bloodPrefab, bloodPosition.position, bloodPosition.transform.rotation); // create an instance of the blood splatter
        Destroy(clone, 1); // this destroys the instantiated blood splatter after 1 second
        Invoke("ResetToIdle", 1); // resets to the idle state after the hit animation has played
    }

    /// <summary>
    /// Handles the Blocking animation state
    /// </summary>
    private void HandleBlockingState()
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.BLOCKING)
        {
            currentCharacterState = CharacterStates.Blocking; // set the character state to walking
            if (voiceBlock.isPlaying == false) // if the audio is not already playing
            {
                StopAllAudio(); // resets the audio
                voiceBlock.Play(); // play the voice taunt
            }
        }
    }

    /// <summary>
    /// Handles the end of the game when a Knight's health reaches zero or below
    /// </summary>
    private void HandleEndState()
    {
        if(knightHealthCurrent <= 0) // if the Knight's health gets to 0 or below
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.DEATH; // set it to the Death animation state
            currentCharacterState = CharacterStates.End; // set the Knight to the end state
            swordCollider.SetActive(false); // turns off the sword collider
            knightCombatControls.SetActive(false); // turns off the combat controls
            if (voiceDeath.isPlaying == false) // if the voice line is not playing
            {
                StopAllAudio(); // reset other audio
                voiceDeath.Play(); // play it
            }
        }
        else if (opponent.GetComponent<KnightManager>().knightHealthCurrent <= 0) // if the opposing Knight's health get to 0 or below
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.VICTORY; // set it to the Victory animation state
            currentCharacterState = CharacterStates.End;
            swordCollider.SetActive(false);
            knightCombatControls.SetActive(false);
            if (voiceVictory.isPlaying == false)
            {
                StopAllAudio();
                voiceVictory.PlayDelayed(2); // delays the voiceline so they don't play over each other
            }
        }
    }

    /// <summary>
    /// Resets the game to the start when a button is pressed
    /// </summary>
    public void ResetGame()
    {
        // Debug.Log("Game Reset");
        StopAllAudio(); // resets the audio
        transform.position = knightTracker.transform.position; // if the Knight has moved return it to the tracker's postion
        knightHealthCurrent = 282; // reset the health to max health
        opponent.GetComponent<KnightManager>().knightHealthCurrent = 282; // resets the opponent's health to max health
        opponent.GetComponent<AnimationHandler>().CurrentState = AnimationHandler.AnimationState.START; // resets the opponents animation state to Start
        knightAnimator.SetBool("VICTORY", false);
        knightAnimator.SetBool("DEATH", false);
        ResetToStartState(); // resets the Knight's state to Start
    }

    /// <summary>
    /// Reset the Knight to the Idle state
    /// </summary>
    public void ResetToIdleState()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.IDLE;
        currentCharacterState = CharacterStates.Idle;
    }

    /// <summary>
    /// Resets the Knight to the Start state
    /// </summary>
    public void ResetToStartState()
    {
        animationHandler.CurrentState = AnimationHandler.AnimationState.START;
        currentCharacterState = CharacterStates.Start;
    }

    /// <summary>
    /// Allows combar buttons to interrupt extra animation
    /// </summary>
    public void InterruptAnimation()
    {
        knightAnimator.SetBool("ExitAnim1", true);
        knightAnimator.SetBool("ExitAnim2", true);
        knightAnimator.SetBool("ATTACKLIGHT", true);
        knightAnimator.SetBool("ATTACKHEAVY", true);
        knightAnimator.SetBool("TAUNTING", true);
        knightAnimator.SetBool("BLOCKING", true);
    }

    // Quick functions for entering and exiting other animations
    public void ExitAnimation()
    {
        knightAnimator.SetBool("ExitAnim1", true);
        knightAnimator.SetBool("ExitAnim2", true);
    }
    public void EnterAnimation()
    {
        // knightAnimator.SetBool("ExitAnim1", false);
        knightAnimator.SetBool("ExitAnim2", false);
    }

    /// <summary>
    /// Stops the Knight's audio sources
    /// </summary>
    void StopAllAudio()
    {
        voiceStart.Stop();
        voiceIdle.Stop();   
        voiceHit.Stop();
        voiceVictory.Stop();
        voiceDeath.Stop();
        voiceTaunt.Stop();
    }
}
