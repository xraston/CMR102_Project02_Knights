using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class KnightManager_move : MonoBehaviour
{
    // references animators and the animation script
    public AnimationHandler animationHandler; // a reference to the animation handler script
    public Animator knightAnimator; // a reference to the animator

    // UI Canvas
    public GameObject knightCombatControls; // a reference to the UI controls
    public GameObject trackerHelp; // a reference to tracker instructions

    // blood particle effect
    public GameObject bloodPrefab;
    public Transform bloodPosition;

    // sound effects
    public AudioSource audioSource; // reference to the knight's audio
    public AudioSource swordSwoosh;
    public AudioSource armourRustle;
    public AudioClip bodyhit;
    public AudioClip shieldhit;
    public AudioClip sworddraw;
    // voice lines
    public AudioSource voiceStart;
    public AudioSource voiceIdle;
    public AudioSource voiceHit;
    public AudioSource voiceVictory;
    public AudioSource voiceDeath;
    public AudioSource voiceTaunt;

    // the opposing knight
    public GameObject opponent;
    public int distanceToDetectOpponent;

    // health and damage variables
    public GameObject knightHealthBar; // a reference to the health bar
    public int knightHealthCurrent;
    public int knightHealthTotal;
    private int attackLightDMG;
    private int attackHeavyDMG;
    public int blockAmount = 20;

    public Sword sword; // a reference to the sword script
    public GameObject swordCollider; // a reference to the sword collider

    // start & idle variables
    public float idleTime = 5; // the time the knights waits until the alternative animation plays
    private float currentIdleWaitTime; // the the current amount of time the knight has idled
    public float startIdleTime = 5; // the time the knights waits until the alternative animation plays
    private float currentStartWaitTime; // the the current amount of time the knight has idled

    // the different states the knight can be in
    public enum CharacterStates {Start, Idle, Walking, Attacking, Taunting, Blocking, Hit, End }
    public CharacterStates currentCharacterState; // the current that that the character is in

    // reference to the knight's tracker
    public GameObject knightTracker;
    public GameManager gameManager;
    private bool trackingFound;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDisable()
    {
        trackingFound = false;
    }

    public void KnightFound()
    {
        if (trackingFound != true)
        {
            trackingFound = true;
        }
        else
        {
            return;
        }
        knightAnimator.Play("Sword_Draw");
        currentCharacterState = CharacterStates.Start; // set the character by default to start
        voiceStart.Play();
        animationHandler.CurrentState = AnimationHandler.AnimationState.START; // set the animation state to idle
                                                                               // startPosition = knightTracker.transform.position;
        currentIdleWaitTime = Time.time + idleTime;
        currentStartWaitTime = Time.time + startIdleTime + AttackDamage(1, 3);
        audioSource.PlayOneShot(sworddraw);
    }

    // Update is called once per frame
    void Update()
    {
        // references to the character states
        HandleStartState();
        HandleIdleState();
        // HandleWalkingState();
        HandleAttackingState();
        HandleBlockingState();
        HandleTauntingState();
        HandleEndState();
    }

    private void HandleStartState() // ADD on tracker start & ADD Animation
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.START)
        {
            StopAllAudio();
            currentCharacterState = CharacterStates.Start; // set the character state to start
            swordCollider.SetActive(true);
            // manage the UI
            knightHealthBar.SetActive(true);
            // knightMoveControls.SetActive(true);
            knightCombatControls.SetActive(false);
            trackerHelp.SetActive(true);
            //

            // when the knight gets close enough to the opponent enter the Idle State
            if (Vector3.Distance(transform.position, opponent.transform.position) < distanceToDetectOpponent)
            {
                animationHandler.CurrentState = AnimationHandler.AnimationState.IDLE;
            }
            // plays extra animations
            else if (Time.time >= currentStartWaitTime)
            {
                knightAnimator.SetBool("ExitAnim2", false);
                Invoke("ExitAnimation", 5);
                currentStartWaitTime = Time.time + idleTime + 5;
                if (currentCharacterState == CharacterStates.Taunting || currentCharacterState == CharacterStates.Attacking || currentCharacterState == CharacterStates.Blocking)
                {
                    // make it so combat buttons can interrupt
                    knightAnimator.SetBool("ExitAnim2", true);
                    InterruptAnimation();
                }
            }
        }
    }

    private void HandleIdleState() // FIX ANIMATION
    {   
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.IDLE)
        {
            currentCharacterState = CharacterStates.Idle; // set the character state to idle
            swordCollider.SetActive(true);
            // manage the UI
            trackerHelp.SetActive(false);
            // knightHealthBar.SetActive(true);
            // knightMoveControls.SetActive(true);
            knightCombatControls.SetActive(true);
            // always look at the opponent
            Vector3 directionToLookAt = opponent.transform.position - transform.position;
            directionToLookAt.y = 0; // doesn't change the Y postion
            Quaternion rotationOfDirection = Quaternion.LookRotation(directionToLookAt); // get a rotation value the character can look towards
            transform.rotation = rotationOfDirection; // set the current rotation to face the target position
            
            // transform.rotation.y = knightTracker.transform.rotation.y; FIX
            
            // plays extra animations
            if (Time.time >= currentIdleWaitTime)
            {
                knightAnimator.Play("Taunt_Point");
                voiceIdle.Play();
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

    private void HandleWalkingState() // this is now defunct
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.WALKING)
        {
            currentCharacterState = CharacterStates.Walking; // set the character state to walking
            armourRustle.Play(); // this only plays on release of button?
        }
    }

    private void HandleTauntingState() // FINISH
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.TAUNTING)
        {
            currentCharacterState = CharacterStates.Taunting; // set the character state to attack    
            if (voiceTaunt.isPlaying == false)
            {
                StopAllAudio();
                voiceTaunt.Play(); // too slow
            }        
        }
    }

    private void HandleAttackingState()
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKLIGHT || animationHandler.CurrentState == AnimationHandler.AnimationState.ATTACKHEAVY)
        {
            currentCharacterState = CharacterStates.Attacking; // set the character state to attack
            if (swordSwoosh.isPlaying == false)
            {
                StopAllAudio();
                swordSwoosh.Play();
            }
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

    public int AttackDamage(int min, int max)
    {
        return Random.Range(min, max);
    }

    public void ApplyDamage(int damageAmount)
    {
        if (currentCharacterState == CharacterStates.Blocking)
        {
            audioSource.PlayOneShot(shieldhit);
            knightAnimator.Play("Block_Hit");
            knightHealthCurrent = knightHealthCurrent - (damageAmount - blockAmount);
        }
        else
        {
            knightHealthCurrent = knightHealthCurrent - damageAmount;
            HandleHitState();
        }
        if (knightHealthCurrent > 282)
        {
            knightHealthCurrent = 282;
        }
        // Debug.Log(damageAmount);
    }

    private void HandleBlockingState() // FINISH
    {
        if (animationHandler.CurrentState == AnimationHandler.AnimationState.BLOCKING)
        {
            currentCharacterState = CharacterStates.Blocking; // set the character state to walking
        }
    }

    private void HandleHitState()
    {
        currentCharacterState = CharacterStates.Hit; // set the character state to being hit
        knightAnimator.Play("Hit_Strong"); // plays the hit animation
        voiceHit.Play();
        audioSource.PlayOneShot(bodyhit);
        GameObject clone = Instantiate(bloodPrefab, bloodPosition.position, bloodPosition.transform.rotation); // create an instance of the blood splatter
        Destroy(clone, 1); // this destroys the instantiated clone object after 1 second
        // audioSource.PlayOneShot(fireWorkSound);
        Invoke("ResetToIdle", 1); // resets to the idle state after the hit animation has played
        
    }

    /// <summary>
    /// handles the end of the game when one character's health reaches zero or below
    /// </summary>
    private void HandleEndState() // FIX audio
    {
        if(knightHealthCurrent <= 0)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.DEATH; // set the animation state to end
            currentCharacterState = CharacterStates.End; // set the character by default to end
            swordCollider.SetActive(false);
            knightCombatControls.SetActive(false);
            if (voiceDeath.isPlaying == false)
            {
                StopAllAudio();
                voiceDeath.Play();
            }
        }
        else if (opponent.GetComponent<KnightManager>().knightHealthCurrent <= 0)
        {
            animationHandler.CurrentState = AnimationHandler.AnimationState.VICTORY; // set the animation state to end
            currentCharacterState = CharacterStates.End; // set the character by default to end
            swordCollider.SetActive(false);
            knightCombatControls.SetActive(false);
            if (voiceVictory.isPlaying == false)
            {
                StopAllAudio();
                voiceVictory.Play();
            }
        }
    }

    public void ResetGame()
    {
        Debug.Log("Game Reset");
        transform.position = knightTracker.transform.position;
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
