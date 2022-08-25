using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: Boss_Enrage
// Author: Ajay Ramnarine
// Purpose: While the boss is transitioning to the enraged state they should not be able to take any damage
// Restrictions: None
public class Boss_Enrage : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<BossController>().isInvulnerable = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<BossController>().isInvulnerable = false;
    }
}
