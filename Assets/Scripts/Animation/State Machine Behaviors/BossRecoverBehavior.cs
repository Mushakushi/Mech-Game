using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRecoverBehavior : StateMachineBehaviour
{
    /// <summary>
    /// Have boss recover via script
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Boss>() is Boss b) b.OnRecover();
    }
}
