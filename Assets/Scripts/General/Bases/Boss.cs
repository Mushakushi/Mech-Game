using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public abstract class Boss : Character
{
    /// <summary> 
    /// Amount of times hp must be depleted for defeat
    /// </summary>
    [SerializeField] public int healthBars; 

    [Header("UI")]
    /// <summary>
    /// Slider UI Component in scene that this object controls
    /// </summary>
    [SerializeField] private Slider healthSlider;

    // Start is called before the first frame update 
    protected override IList<Phase> InitializeCharacter()
    {
        BossData data = SetBossData();
        name = data.name;
        maxHealth = data.maxHealth;
        health = maxHealth;
        damage = data.damage;
        resistance = data.resistance;
        healthBars = data.healthBars;

        return new Phase[] { Phase.Boss }; 
    }

    // Update is called once per frame
    void Update()
    {
        /*if (returnToIdle)
        {
            animator.applyRootMotion = false;
            returnToIdle = false;
            animator.ResetTrigger("GetHit");
            animator.ResetTrigger("RunSpecial");
            //combat.fightStage = PhaseManager.FIGHT_STAGE.PlayerAttack;
        }

        if (currentState == BOSS_STATE.FullStun)
        {
            combat.DisablePlayerAttack();
        }*/

        //TryShake();

        //if (Input.GetKeyDown(KeyCode.P)) // TEMP FOR DIALOGUE TESTING
        {
            //combat.DoBossSpecial();
            //dialogue.DisplayNextLine();
        }
    }

    /// <summary>
    /// Allows children to initialize data without hiding parent's Start, uses struct to ensure proper data is supplied
    /// </summary>
    protected abstract BossData SetBossData();

    /// <summary>
    /// Event that happens when Hitbox enters boss Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entry</param>
    public override void OnHitboxEnter(float damage)
    {
        base.OnHitboxEnter(damage);
        RefreshSlider(); 
    }

    /// <summary>
    /// Sets the health slider to boss health
    /// </summary>
    private void RefreshSlider()
    {
        healthSlider.value = health / maxHealth; 
    }

    /// <summary>
    /// Take away one hp bar on health deplete
    /// </summary>
    protected override void OnHealthDeplete()
    {
        this.ExitPhase(Phase.Boss_Collapse); 
        health = maxHealth;

        animator.SetTrigger("Collapse"); 
    }

    protected virtual void OnHealthRegain()
    {
        RefreshSlider();
    }

    protected virtual void OnHealthDepleteFull()
    {
        this.ExitPhase(Phase.Boss_Defeat); 
    }

    
    protected override void PhaseEnterBehavior()
    {
        animator.SetTrigger("EnterPhase");
    }

    protected override void PhaseUpdateBehavior() { }
    protected override void PhaseExitBehavior() { }
}