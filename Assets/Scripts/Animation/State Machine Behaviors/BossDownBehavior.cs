using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDownBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("ShakesLeft", animator.GetInteger("ShakesLeft") - 1);
    }
}
