using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(IPhaseObserver))]
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
        if (animator.gameObject.GetComponent<IPhaseObserver>() is IPhaseObserver c)
        {
            c.OnPhaseExit();
            if (c is IPhaseController) Combat.ExitPhase();
        }

    }
}