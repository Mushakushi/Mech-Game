using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRecoverBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Boss>() is Boss b) b.OnRecover();
    }
}
