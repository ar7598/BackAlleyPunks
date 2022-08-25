using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: Boss_Charge
// Author: Ajay Ramnarine
// Purpose: Once the boss is in this state, they should charge in a straight line towards the player for a certain amount of time
// Restrictions: The boss will either travel in the x or y direction, but not a combination of the two while in this state
//               This is done purposfully, I personally did not like it when the boss would charge in a diagonal
public class Boss_Charge : StateMachineBehaviour
{
    public float chargeSpeed = 200f;

    public float chargeTimer = 0.25f;
    public float currentChargeTimer;

    Transform player;
    BossController boss;
    Rigidbody2D rb;

    Vector2 chargeDirection;
    Vector2 newPosition;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // set the IsCharging bool to true once in this state
        animator.SetBool("IsCharging", true);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
        rb = animator.GetComponent<Rigidbody2D>();

        // set the current charge timer to the charge timer
        currentChargeTimer = chargeTimer;

        // get position to charge in the x or y direction
        if(Mathf.Abs(player.position.x - rb.position.x) >= Mathf.Abs(player.position.y - rb.position.y))
        {
            if(player.position.x >= rb.position.x)
            {
                chargeDirection = new Vector2(25f, 0f);
            }
            else
            {
                chargeDirection = new Vector2(-25f, 0f);
            }
        }
        else if(Mathf.Abs(player.position.x - rb.position.x) < Mathf.Abs(player.position.y - rb.position.y))
        {
            if(player.position.y >= rb.position.y)
            {
                chargeDirection = new Vector2(0f, 25f);
            }
            else
            {
                chargeDirection = new Vector2(0f, -25f);
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // velocity at which the boss will charge in the targeted direction
        rb.velocity = chargeDirection * chargeSpeed * Time.fixedDeltaTime;

        // decrement the current charge timer
        currentChargeTimer -= Time.fixedDeltaTime;

        // if the current charge timer reaches zero, set animator bool IsCharging to false
        if(currentChargeTimer <= 0)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsCharging", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    
}
