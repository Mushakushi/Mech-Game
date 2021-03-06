using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;
using static FileUtility; 

public abstract class Boss : Character
{
    /// <summary>
    /// Active phases of boss
    /// </summary>
    public override Phase[] activePhases => new Phase[] { Phase.Boss_Guard, Phase.Boss, Phase.Player_Win };

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

    protected sealed override void OnInitialize()
    {
        // this.GetManager().jario.onJarioCountStart += 
        this.GetManager().jario.onJarioCount += () =>
        {
            animator.SetTrigger("Shake");
            animator.SetInteger("ShakesLeft", animator.GetInteger("ShakesLeft") - 1);
        };

        this.GetManager().jario.onJarioCountStop += () => animator.ResetTrigger("Shake");

        hitbox.SetOwner(this);

        healthBars = maxHealthBars;
        health = maxHealth; // TODO - i'm repeatiing this code to get the refresh working first time

        //healthSlider = FindObjectOfType<BossHealthSlider>();

        hurtClip = LoadFile<AudioClip>($"{voicelinesPath}/{GetType()}/Hurt1");
        knockClip = LoadFile<AudioClip>($"{voicelinesPath}/{GetType()}/Hurt2");
        dialogueClips = new List<AudioClip>() { hurtClip, knockClip };

        projectileManager = GetComponent<ProjectileManager>();
        projectileManager.Initialize(group);

        Accumulate(specialWeights);

        OnInitializeBoss();
    }

    /// <summary>
    /// Create and store a list of accumulated weights and their sum
    /// </summary>
    /// <param name="weights">List of unaccumulated weights</param>
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
                new ScoreData(timesHitBoss: 1).AddToPlayerScore(group);
                AudioPlayer.Play(hurtClip);
                break;
            case Phase.Boss_Guard:
                new ScoreData(timesBossBlocked: 1).AddToPlayerScore(group);
                animator.SetTrigger("Guard");
                break;
        }
    }

    public override void OnEnterHurtbox()
    {
        DisableHitbox();
    }

    /// <summary>
    /// Take away one hp bar on health deplete
    /// </summary>
    protected override void OnHealthDeplete()
    {
        this.GetOverlay().StartFlash();

        healthBars--;
        this.SwitchPhase(Phase.Boss_Collapse);
        AudioPlayer.Play(knockClip);

        int shakes = this.GetCounts();
        if (shakes <= 0)
        {
            OnHealthDepleteFull();
        }
        // tell Jario to count
        else this.GetManager().jario.StartCount();

        animator.SetInteger("ShakesLeft", shakes);
        animator.ResetTrigger("GetHit");
        animator.ResetTrigger("Shake");
        animator.SetTrigger("Collapse");
    }

    /// <summary>
    /// What happens when the boss recovers from being down
    /// </summary>
    public virtual void OnRecover()
    {
        health = maxHealth / (maxHealthBars - healthBars + 1);
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
                this.WaitForSeconds(1.25f, () => this.ExitPhase());
                break;
            case Phase.Player_Win:
                animator.SetBool("KO'd", true);
                break;
        }
    }

    protected override void PhaseUpdateBehavior() { }

    protected override void PhaseExitBehavior() { }

    /// <summary>
    /// Used within the animator as an animation event create AttackProjectile objects
    /// </summary>
    /// <param name="attackAsset">Should be a AttackProjectileAsset. Must be an Object for the animator</param>
    public void ProjectileAttack(Object attackAsset)
    {
        if (attackAsset is AttackProjectileAsset attack)
        {
            projectileManager.SpawnProjectile(attack);
        }
    }
}