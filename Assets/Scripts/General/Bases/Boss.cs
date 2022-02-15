using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class Boss : Character
{
    /// <summary>
    /// Active phases of boss
    /// </summary>
    public override Phase[] activePhases => new Phase[] { Phase.Boss_Guard, Phase.Boss };

    // TODO - There is a convoluted way to serialized properties in unity using custom inspector!
    [Header("Boss UI")]
    /// <summary>
    /// Slider UI Component in scene that this object controls
    /// </summary>
    [SerializeField] private BossHealthSlider healthSlider;

    //[Header("Boss Data")]
    /// <summary>
    /// Amount of times health bar must be depleted to be defeated
    /// </summary>
    public abstract int maxHealthBars { get; }

    /// <summary>
    /// Current health bars 
    /// </summary>
    public int healthBars { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public abstract List<float> specialWeights { get; }

    /// <summary>
    /// 
    /// </summary>
    public List<float> accumulatedWeights { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public float accumulatedWeightSum { get; set; }

    public ProjectileManager projectileManager { get; set; }

    //[Header("Boss Audio")]
    protected AudioClip hurtClip { get; private set; }
    protected AudioClip knockClip { get; private set; }
    protected List<AudioClip> dialogueClips { get; private set; }



    // Start is called before the first frame update 
    protected sealed override void OnInitialize()
    {
        DisableHitbox();

        OnInitializeBoss();
        healthBars = maxHealthBars;
        health = maxHealth; // TODO - i'm repeatiing this code to get the refresh working first time

        healthSlider = FindObjectOfType<BossHealthSlider>();
        RefreshSlider();

        // TODO - standardize naming these so we can add variables 
        hurtClip = FileUtility.LoadFile<AudioClip>($"Audio/Voicelines/{characterName}/snd_ugh");
        knockClip = FileUtility.LoadFile<AudioClip>($"Audio/Voicelines/{characterName}/snd_khan");
        dialogueClips = new List<AudioClip>() { hurtClip, knockClip };

        projectileManager = GetComponent<ProjectileManager>();
        projectileManager.Initialize(group);

        // these are needed to stop the thing from yelling at me. oh well
        accumulatedWeightSum = 0;
        accumulatedWeights = null;
        Accumulate(specialWeights);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="weights"></param>
    private void Accumulate(List<float> weights)
    {
        List<float> result = new List<float>();
        float accumulatedWeight = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            accumulatedWeight += weights[i];
            result.Insert(i, accumulatedWeight);
        }

        accumulatedWeightSum = accumulatedWeight;
        accumulatedWeights = result;
    }

    /// <summary>
    /// Allows children to initialize data without hiding parent's Start
    /// </summary>
    protected abstract void OnInitializeBoss();

    /// <summary>
    /// Event that happens when Hitbox enters boss Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entry</param>
    public override void OnHitboxEnter(float damage)
    {
        switch (this.GetManagerPhase())
        {
            case Phase.Player:
                base.OnHitboxEnter(damage);
                new ScoreData(timesBossHit: 1).AddToPlayerScore(group);
                RefreshSlider();
                AudioPlayer.Play(hurtClip);
                break;
            case Phase.Boss_Guard:
                animator.SetTrigger("Guard");
                break;
        }
    }

    /// <summary>
    /// Sets the health slider to boss health
    /// </summary>
    private void RefreshSlider()
    {
        healthSlider.SetValue(health / maxHealth);
    }

    /// <summary>
    /// Take away one hp bar on health deplete
    /// </summary>
    protected override void OnHealthDeplete()
    {
        this.GetUIShaderOverlay().StartFlash();

        this.SwitchPhase(Phase.Boss_Collapse);
        AudioPlayer.Play(knockClip);
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

    /// <summary>
    /// What happens when the boss recovers from being down
    /// </summary>
    public virtual void OnRecover()
    {
        RefreshSlider();
    }

    /// <summary>
    /// What happens when health and healthBars are depleted
    /// </summary>
    public virtual void OnHealthDepleteFull()
    {
        this.SwitchPhase(Phase.Player_Win);
    }

    protected override void PhaseEnterBehavior()
    {
        EnableHurtbox();
        switch (this.GetManagerPhase())
        {
            case Phase.Boss:
                animator.SetTrigger("EnterPhase");
                break;
            case Phase.Boss_Guard:
                animator.SetTrigger("ReturnToIdle");
                StartCoroutine(CoroutineUtility.WaitForSeconds(1.25f, () => this.ExitPhase()));
                break;
        }
    }

    protected override void PhaseUpdateBehavior() { }
    protected override void PhaseExitBehavior() { }

    public void ProjectileAttack(Object attackAsset)
    {
        if (attackAsset is AttackProjectileAsset attack)
        {
            projectileManager.SpawnProjectile(attack);
        }
    }
}