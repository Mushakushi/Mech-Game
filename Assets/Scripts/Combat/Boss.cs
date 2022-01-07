using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Character
{
    /// <summary>
    /// List of weights for Special Attacks. Count should not exceed number of special attacks.
    /// </summary>
    public List<int> SpecialAttackWeights { get; set; }
    public enum BOSS_STATE { AttackNormal, AttackSpecial, Stun, Default }
    public BOSS_STATE currentState;

    public Vector3 lastPos;

    private void InitSpecials()
    {

    }

    // Start is called before the first frame update
   override public void OnStart()
    {
        startPos = transform.position;
        InitSpecials();
        SetBossValues();
    }

    // Update is called once per frame
    override public void OnUpdate()
    {
        TryShake();

        if (Input.GetKeyDown(KeyCode.P))
        {
            combat.DoBossSpecial();
        }
    }

    override public void OnGetHit(float damage)
    {
        animator.SetTrigger("GetHit");
        TakeDamage(damage);
    }

    override public void TakeDamage(float damage)
    {
        health -= damage * (1 / resistance);
    }

    public void DoNormalAttack()
    {

        return;
    }
    public abstract void SetBossValues();
}