using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhaseEnterBehavior : StateMachineBehaviour
{
    private Boss boss;

    /// <summary>
    /// Selects special for current phase based on weights
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetInteger("SpecialIndex", 1);
        return; // temp
        if (boss == null)
        {
            boss = animator.gameObject.GetComponent<Boss>();
        }
        List<float> accumulatedWeights = boss.accumulatedWeights;
        float accumulatedWeightSum = boss.accumulatedWeightSum;

        double rand = Random.Range(0, accumulatedWeightSum);
        for (int i = 0; i < accumulatedWeights.Count; i++)
        {
            if (rand <= accumulatedWeights[i])
            {
                animator.SetInteger("SpecialIndex", i + 1);
                return;
            }
        }
    }
}
