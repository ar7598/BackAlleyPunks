using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: Boss_Walk
// Author: Ajay Ramnarine
// Purpose: When the boss enters this state from idle, they will begin to walk towards the player
//          After the boss is within range to attack, the boss should attack the player
//          If a certain amount of time has passed without getting into an attack range, the boss should transition to the charge state
// Restrictions: Attacking is called in the animator of Unity, and does not have a state of its own to transition to
public class Boss_Walk : StateMachineBehaviour
{
    public float speed = 3f;
    public float attackRange = 3f;

    public float chargeStartTimer = 2f;
    float chargeCurrentTimer = 0f;

    Transform player;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();

        // upon entering this state, set the charge current timer to the charge start timer
        chargeCurrentTimer = chargeStartTimer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // two vector 2 variables to get the target position to travel
        Vector2 targetPosition = new Vector2(player.position.x, player.position.y);
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if(chargeCurrentTimer <= 0)
        {
            // transition to the charge state
            animator.SetTrigger("Charge");
        }
        else if(Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            // transition to the attack state
            animator.SetTrigger("Attack");
        }
        else
        {
            // decrement the current timer of the charge
            chargeCurrentTimer -= Time.fixedDeltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reset the charge and attack triggers for the next time the boss enters this state
        animator.ResetTrigger("Charge");
        animator.ResetTrigger("Attack");
    }
}
