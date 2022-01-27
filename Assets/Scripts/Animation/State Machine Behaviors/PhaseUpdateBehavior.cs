using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(IPhaseObserver), typeof(PhaseExitBehavior))]
/// <summary>
/// Attach to substatemachine to notify of update
/// </summary>
public class PhaseUpdateBehavior : StateMachineBehaviour
{
    /// <summary>
    /// Notifies Character of state update
    /// </summary>
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (animator.GetComponent<IPhaseObserver>() is IPhaseObserver c) c.OnPhaseUpdate(); 
    }
}
