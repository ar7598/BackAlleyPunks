using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class: Boss_Defeated
// Author: Ajay Ramnarine
// Purpose: Destory the boss game object after it has been defeated
// Restrictions: None
public class Boss_Defeated : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // destroy the game object of the boss after the animation has played
        Destroy(animator.gameObject, stateInfo.length);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
