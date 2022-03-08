using System.Collections.Generic;
using UnityEngine;

public class Shoto : Boss
{
    public override string characterName => "Shoto";
    public override float maxHealth => 1; 
    public override float resistance => 1;
    public override Phase[] activePhases => new Phase[] { Phase.Player, Phase.Boss_Guard, Phase.Boss, Phase.Player_Win };

    public override int maxHealthBars => 1; 
    public override List<float> specialWeights => new List<float> { 75, 25 };

    protected override void OnInitializeBoss() {
        projectileManager.speed = 1.15f;
    }

    protected override void PhaseEnterBehavior()
    {
        base.PhaseEnterBehavior();
        // skip player phase until final line of dialogue
        if (this.GetManagerPhase() == Phase.Player && this.GetDialogueController().GetRemainingLines() != 0)
        {
            this.SwitchPhase(Phase.Boss);
        }
        
    }
}
