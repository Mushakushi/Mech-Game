using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseEnterBehavior : StateMachineBehaviour
{
    /// <summary>
    /// List of weights for Special Attacks. Count should not exceed number of special attacks.
    /// </summary>
    [SerializeField] private List<int> SpecialAttackWeights { get; set; }

    /// <summary>
    /// Selects special for current phase based on weights
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        /*if (animator.GetBool("EnterPhase"))
        {
            int weightSum = 0;
            for (int i = 0; i < SpecialAttackWeights.Count; i++)
            {
                weightSum += SpecialAttackWeights[i];
            }
            int rand = Random.Range(0, weightSum);
            for (int i = 0; i < SpecialAttackWeights.Count; i++)
            {
                if (rand < SpecialAttackWeights[i])
                {
                    animator.SetInteger("SpecialIndex", i + 1);
                }

                rand -= SpecialAttackWeights[i];
            }
        }*/

        animator.SetInteger("SpecialIndex", 1);
    }
}
