using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to SUBSTATE machine to control phase behavior
/// </summary>
public class PhaseBehavior : StateMachineBehaviour
{
    /// <summary>
    /// Phase that follows this phase
    /// </summary>
    Phase targetPhase;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (animator.gameObject.GetComponent<Character>() is Character c)
    }

    /// <summary>
    /// Triggers combat's change phase to target phase
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        Combat.ChangePhase(targetPhase);
    }
}
