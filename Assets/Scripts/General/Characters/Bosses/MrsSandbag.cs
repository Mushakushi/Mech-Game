using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrsSandbag : Boss
{
    public override string characterName => "Mrs. Sandbag";
    public override float maxHealth => 100;
    public override float resistance => 0;
    public override Phase[] activePhases => new Phase[] { Phase.Player, Phase.Boss_Guard, Phase.Boss, Phase.Player_Win };

    public override int maxHealthBars => 3;
    public override List<float> specialWeights => new List<float> { 100 };

    protected override void OnInitializeBoss() { }
    protected override void PhaseEnterBehavior()
    {
        base.PhaseEnterBehavior();
        if (this.GetManagerPhase() == Phase.Player && this.GetDialogueController().GetDialogueStage() == 1)
        {
            this.SwitchPhase(Phase.Boss);
        }
        
        if (this.GetManagerPhase() == Phase.Player && this.GetDialogueController().GetRemainingLines() == 0)
        {
            StartCoroutine(Scene.Load("Menu Scene"));
        }
    }

    /// <summary>
    /// Mrs sandbag is immortal
    /// </summary>
    public override void OnHitboxEnter(float damage)
    {
        base.OnHitboxEnter(damage);
        health += damage;
    }
}
