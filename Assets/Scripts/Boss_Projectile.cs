using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: Boss_Projectile
// Author: Ajay Ramnarine
// Purpose: Have the boss fire projctiles for a certain amount of time
// Restrictions: The projeciles are called from the Projectile, ProjectilePool, and the ProjectileFire scripts
//               MUST disable the ProjectileFire script after the timer is up so as to not continue firing projectiles after leaving this state
public class Boss_Projectile : StateMachineBehaviour
{
    public float fireTimer = 5f;
    float currentFireTimer;

    BossController boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // get access to the boss controller script
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();

        // enable the projectile fire script
        boss.GetComponent<ProjectileFire>().enabled = true;

        // set the currentFireTimer to the fireTimer
        currentFireTimer = fireTimer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // decrement the currentFireTimer
        currentFireTimer -= Time.fixedDeltaTime;

        // once currentFireTimer <= 0 then set the IsFiring bool to false
        if(currentFireTimer <= 0)
        {
            // disable the projectile firing 
            boss.GetComponent<ProjectileFire>().enabled = false;

            animator.SetBool("IsFiring", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
