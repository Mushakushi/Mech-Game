using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSpecial : MonoBehaviour
{
    /// <summary>
    /// Elemental type of the attack.
    /// </summary>
    [SerializeField] public BossAttackType AttackType { get; set; }
    /// <summary>
    /// Damage done by the attack.
    /// </summary>
    [SerializeField] public float Damage { get; set; }
    /// <summary>
    /// Duration of the attack wind-up in seconds.
    /// </summary>
    [SerializeField] public float WindUpDuration { get; set; }
    /// <summary>
    /// Duration of linger after attack has completed.
    /// </summary>
    [SerializeField] public float LingerDuration { get; set; }
    /// <summary>
    /// Int value for the current stage of the attack.
    /// </summary>
    [SerializeField] public int AttackStage { get; set; }
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
        attackCoroutine = StartCoroutine(DoAttack());
    }

    public IEnumerator DoAttack()
    {
        yield return StartCoroutine(RunWindUpAnimation());
        yield return new WaitForSecondsRealtime(WindUpDuration);
        Boss.combat.DisablePlayerAttack();
        yield return StartCoroutine(RunAttackAnimation());
        yield return new WaitForSecondsRealtime(0.4f);
        Boss.combat.EnablePlayerAttack();
        yield return new WaitForSecondsRealtime((LingerDuration - 0.4f <= 0) ? 0 : LingerDuration - 0.4f);
        returnCoroutine = StartCoroutine(RunReturnAnimation());
    }

    public void CancelAttackIfExists()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        Boss.combat.EnablePlayerAttack();
    }

    public void CancelReturnIfExists()
    {
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);
        Boss.combat.EnablePlayerAttack();
    }

    public void Cancel()
    {
        CancelAttackIfExists();
        CancelReturnIfExists();
    }

    public abstract IEnumerator RunWindUpAnimation();

    public abstract IEnumerator RunAttackAnimation();

    public abstract IEnumerator RunReturnAnimation();
}
