using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public abstract class Boss : Character
{
    /// <summary>
    /// List of weights for Special Attacks. Count should not exceed number of special attacks.
    /// </summary>
    public List<int> SpecialAttackWeights { get; set; }

    [Header("UI")]
    /// <summary>
    /// Slider UI Component in scene that this object controls
    /// </summary>
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Dialogue dialogue;

    // Start is called before the first frame update //TODO: We also have SetBossValues() below, is there a better way of initializing the classes?
    public override void OnStart()
    {
        SetBossValues();
        health = maxHealth;
        dialogue.SetLanguage(DialogueUtil.LANGUAGE.TokiPona);
        dialogue.InitializeBossDialogue(this);
        dialogue.DisplayNextLine();
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
            //combat.fightStage = Combat.FIGHT_STAGE.PlayerAttack;
        }

        if (currentState == BOSS_STATE.FullStun)
        {
            combat.DisablePlayerAttack();
        }*/

        //TryShake();

        if (Input.GetKeyDown(KeyCode.P))
        {
            //combat.DoBossSpecial();
            dialogue.DisplayNextLine();
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

    /// <summary>
    /// Selects special for current phase based on weights
    /// </summary>
    protected override void PhaseEnterBehavior()
    {
        int weightSum = 0;
        for (int i = 0; i < SpecialAttackWeights.Count; i++)
        {
            weightSum += SpecialAttackWeights[i];
        }
        int rand = Random.Range(0, weightSum);
        for (int i = 0; i < SpecialAttackWeights.Count; i++)
        {
            if (rand < SpecialAttackWeights[i])
            {
                animator.SetFloat("SpecialIndex", i + 1);
                //animator.SetTrigger("RunSpecial");
            }

            rand -= SpecialAttackWeights[i];
        }
    }

    protected override void PhaseUpdateBehavior() { }
    protected override void PhaseExitBehavior() { }
}