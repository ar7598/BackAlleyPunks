using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: Boss_Idle
// Author: Ajay Ramnarine
// Purpose: After a short amount of time in the idle position, the boss should transition to the walking state
// Restrictions: None
public class Boss_Idle : StateMachineBehaviour
{
    public float walkTimer = 5f;
    float currentTimer;

    Transform player;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();

        // at the start of this state, the current timer should be set equal to the walk timer
        currentTimer = walkTimer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // decrement currentTimer until it reaches zero
        // once the timer reaches zero, transition to the walking state
        if(currentTimer <= 0)
        {
            // set the trigger to transition to the walk state
            animator.SetTrigger("Walk");
        }
        else
        {
            // decrement currentTimer
            currentTimer -= Time.fixedDeltaTime;
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reset the walk trigger when exiting this state
        animator.ResetTrigger("Walk");
    }

}
