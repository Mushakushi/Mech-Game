using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(IPhaseController))]
/// <summary> 
/// Attach to last state machine to trigger phase exit
/// </summary>
public class PhaseExitBehavior : StateMachineBehaviour
{
    /// <summary>
    /// Notifies Character of state exit
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if (animator.gameObject.GetComponent<IPhaseController>() is IPhaseController c) c.OnPhaseExit();
        Combat.PhaseExit();
    }
}