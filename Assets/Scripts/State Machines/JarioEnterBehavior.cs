using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarioEnterBehavior : StateMachineBehaviour
{
    /// <summary>
    /// Jario decides how much times he needs to count
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        int count;
        if (Combat.phase == Phase.Intro) count = 3;
        else count = 10; 

        animator.SetInteger("CountLeft", count); 
    }
}
