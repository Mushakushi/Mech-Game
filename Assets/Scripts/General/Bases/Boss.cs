using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public abstract class Boss : Character
{
    [Header("Boss Stats")]
    /// <summary> 
    /// Amount of times hp must be depleted for defeat
    /// </summary>
    public int maxHealthBars;

    /// <summary>
    /// Current count of health bars
    /// </summary>
    public int healthBars; 

    [Header("Boss UI")]
    /// <summary>
    /// Slider UI Component in scene that this object controls
    /// </summary>
    [SerializeField] private Slider healthSlider;

    [Header("Boss Special Weights")]
    [SerializeField] public List<float> accumulatedWeights;
    [SerializeField] public float accumulatedWeightSum;

    // Start is called before the first frame update 
    protected override IList<Phase> InitializeCharacter()
    {
        BossData data = SetBossData();
        name = data.name;
        maxHealth = data.maxHealth;
        health = maxHealth;
        damage = data.damage;
        resistance = data.resistance;
        maxHealthBars = data.maxHealthBars;
        healthBars = maxHealthBars;
        accumulatedWeights = data.accumulatedWeights;
        accumulatedWeightSum = data.accumulatedWeightSum;

        return new Phase[] { Phase.Boss }; 
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
        new ScoreData(timesBossHit: 1).AddToPlayerScore(group);
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
        this.GetUIShaderOverlay().StartFlash();

        this.SwitchPhase(Phase.Boss_Collapse);
        healthBars--;
        health = maxHealth / (maxHealthBars - healthBars + 1); 
        
        int shakes = HealthConversion.ConvertBarsToCount(healthBars, maxHealthBars) + 1;
        if (shakes <= 0)
        {
            OnHealthDepleteFull();
        }

        animator.SetInteger("ShakesLeft", shakes);
        animator.ResetTrigger("GetHit");
        animator.SetTrigger("Collapse");
    }

    public virtual void OnRecover()
    {
        RefreshSlider();
    }

    public virtual void OnHealthDepleteFull()
    {
        this.SwitchPhase(Phase.Player_Win); 
    }

    
    protected override void PhaseEnterBehavior()
    {
        animator.SetTrigger("EnterPhase");
    }

    protected override void PhaseUpdateBehavior() { }
    protected override void PhaseExitBehavior() { }
}