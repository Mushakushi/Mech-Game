using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarioCountingBehavior : StateMachineBehaviour
{ 
    /// <summary>
    /// Subtracts one from CountLeft integer in animator every time the animation is played.
    /// Note - this doesn't have anything to do with phases so it gets its own script
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetInteger("CountLeft", animator.GetInteger("CountLeft") - 1); 
    }
}
