using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to SUBSTATE machine to control phase behavior AFTER state entrance (that's in Combat)
/// </summary>
public class PhaseBehavior : StateMachineBehaviour
{
    /// <summary>
    /// Notifies Character of state update
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (animator.GetComponent<IPhaseController>() is IPhaseController c) c.OnPhaseUpdate(); 
    }

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
