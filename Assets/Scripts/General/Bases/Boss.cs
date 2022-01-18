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

    // Start is called before the first frame update
    void Start()
    {
        //triggerLayerMask.SetLayerMask(LayerMask.GetMask("Boss Attack"));
        SetBossValues();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (returnToIdle)
        {
            animator.applyRootMotion = false;
            returnToIdle = false;
            animator.ResetTrigger("GetHit");
            animator.ResetTrigger("RunSpecial");
            combat.fightStage = Combat.FIGHT_STAGE.PlayerAttack;
        }

        TryShake();

        if (Input.GetKeyDown(KeyCode.P))
        {
            combat.DoBossSpecial();
        }
    }

    public abstract void SetBossValues();
}