using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calls Boss's idle animation function 
/// </summary>
public class BossIdle : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (animator.gameObject.GetComponent<Boss>() is Boss b) b.OnIdleAnimationEnter(); 
    }
}
