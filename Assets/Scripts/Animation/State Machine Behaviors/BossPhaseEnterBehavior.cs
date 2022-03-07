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
        // Get boss's weights
        if (boss == null)
        {
            boss = animator.gameObject.GetComponent<Boss>();
        }
        List<float> accumulatedWeights = boss.accumulatedWeights;
        float accumulatedWeightSum = boss.accumulatedWeightSum;

        // choose a random attack based on weight list
        double rand = Random.Range(0, accumulatedWeightSum);
        for (int i = 0; i < accumulatedWeights.Count; i++)
        {
            if (rand <= accumulatedWeights[i])
            {
                if (i == 2)
                {
                    animator.SetInteger("SpecialIndex", 2);
                    return;
                }
                animator.SetInteger("SpecialIndex", i + 1);
                return;
            }
        }
    }
}
