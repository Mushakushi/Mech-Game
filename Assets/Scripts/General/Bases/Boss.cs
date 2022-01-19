using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Character
{
    /// <summary>
    /// List of weights for Special Attacks. Count should not exceed number of special attacks.
    /// </summary>
    public List<int> SpecialAttackWeights { get; set; }
    public enum BOSS_STATE { Default, AttackNormal, AttackSpecial, FullStun }
    public BOSS_STATE currentState; // cannot be property to access in animator
    public Combat combat;

    // Start is called before the first frame update
    public override void OnStart()
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
            //combat.fightStage = Combat.FIGHT_STAGE.PlayerAttack;
        }

        if (currentState == BOSS_STATE.FullStun)
        {
            combat.DisablePlayerAttack();
        }

        TryShake();

        if (Input.GetKeyDown(KeyCode.P))
        {
            combat.DoBossSpecial();
        }
    }

    public abstract void SetBossValues();
}