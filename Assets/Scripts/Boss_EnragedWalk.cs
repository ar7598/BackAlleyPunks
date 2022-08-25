using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: Boss_EnragedWalk
// Author: Ajay Ramnarine
// Purpose: In the enraged state, the boss should move towards the players position
//          If the player is within a certain range, the boss should start attacking
//          If a certain amount of time has passed without being in the attack range, the boss should start firing projectiles
// Restrictions: Projectile firing is based on the Projectile, ProjectilePool, and ProjectileFire scripts
//               The attacking is handled within the animator of Unity and does not have its own state to transition to
public class Boss_EnragedWalk : StateMachineBehaviour
{
    public float speed = 5f;
    public float attackRange = 1f;

    public float firingStartTimer = 2f;
    float firingCurrentTimer = 0f;

    Transform player;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();

        // upon entering this state, set the firing current timer to the firing start timer
        firingCurrentTimer = firingStartTimer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // two vector 2 variables to get the target position to travel
        Vector2 targetPosition = new Vector2(player.position.x, player.position.y);
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (firingCurrentTimer <= 0)
        {
            // transition to the firing projectiles state
            animator.SetBool("IsFiring", true);
        }
        else if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            // transition to the attack state
            animator.SetTrigger("Attack");
        }
        else
        {
            // decrement the current timer of the charge
            firingCurrentTimer -= Time.fixedDeltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reset the attack trigger for the next time the boss enters this state
        animator.ResetTrigger("Attack");
    }
}
