using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public abstract class Boss : Character
{
    /// <summary>
    /// List of weights for Special Attacks. Count should not exceed number of special attacks.
    /// </summary>
    public List<int> SpecialAttackWeights { get; set; }
    public enum BOSS_STATE { Default, AttackNormal, AttackSpecial, FullStun }
    public BOSS_STATE currentState; // cannot be property to access in animator
    public Combat combat;

    [Header("UI")]
    /// <summary>
    /// Slider UI Component in scene that this object controls
    /// </summary>
    [SerializeField] private Slider healthSlider;

    // Start is called before the first frame update //TODO: We also have SetBossValues() below, is there a better way of initializing the classes?
    public override void OnStart()
    {
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

        //TryShake();

        if (Input.GetKeyDown(KeyCode.P))
        {
            combat.DoBossSpecial();
        }
    }

    /// <summary>
    /// Allows children to initialize data without hiding parent's Start
    /// </summary>
    public abstract void SetBossValues();

    /// <summary>
    /// Event that happens when Hitbox enters boss Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entry</param>
    public override void OnHitboxEnter(float damage)
    {
        base.OnHitboxEnter(damage);
        healthSlider.value = health / maxHealth;
    }
}