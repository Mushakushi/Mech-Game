using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSpecial : MonoBehaviour
{
    public BossAttackType AttackType { get; set; }
    public float Damage { get; set; }
    public float AttackDelay { get; set; }
    public Coroutine attackDelayCoroutine;

    public abstract IEnumerator DelayForSeconds(float seconds);

    public abstract void RunAttackAnimation();
}
