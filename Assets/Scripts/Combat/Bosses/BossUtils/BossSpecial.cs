using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSpecial : MonoBehaviour
{
    /// <summary>
    /// Elemental type of the attack.
    /// </summary>
    public BossAttackType AttackType { get; set; }
    /// <summary>
    /// Damage done by the attack.
    /// </summary>
    public float Damage { get; set; }
    /// <summary>
    /// Duration of the attack wind-up in seconds.
    /// </summary>
    public float WindUpDuration { get; set; }
    /// <summary>
    /// Duration of linger after attack has completed.
    /// </summary>
    public float LingerDuration { get; set; }
    /// <summary>
    /// Int value for the current stage of the attack.
    /// </summary>
    public int AttackStage { get; set; }
    /// <summary>
    /// Coroutine containing delay and attack animation. Can be cancelled to stop the attack.
    /// </summary>
    public Coroutine attackCoroutine;
    /// <summary>
    /// Coroutine containing return animation.
    /// </summary>
    public Coroutine returnCoroutine;
    /// <summary>
    /// Parent Boss script.
    /// </summary>
    public Boss Boss { get; set; }

    public void RunSpecial()
    {
        AttackStage = 0;
        RunWindUpAnimation();
        attackCoroutine = StartCoroutine(AttackAfterSeconds(WindUpDuration));
    }

    public IEnumerator AttackAfterSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        yield return StartCoroutine(RunAttackAnimation());
        returnCoroutine = StartCoroutine(ReturnAfterSeconds(LingerDuration));
    }

    public IEnumerator ReturnAfterSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        RunReturnAnimation();
    }

    public abstract void RunWindUpAnimation();

    public abstract IEnumerator RunAttackAnimation();

    public abstract void RunReturnAnimation();
}
