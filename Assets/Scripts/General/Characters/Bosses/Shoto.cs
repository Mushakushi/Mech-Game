using System.Collections.Generic;
using UnityEngine;

public class Shoto : Boss
{
    public override string characterName => "Shoto";
    public override float maxHealth => 1; 
    public override float resistance => 1;
    public override Phase[] activePhases => new Phase[] { Phase.Player, Phase.Boss_Guard, Phase.Boss, Phase.Player_Win };

    public override int maxHealthBars => 1; 
    public override List<float> specialWeights => new List<float> { 100 };

    protected override void OnInitializeBoss() { }

    protected override void PhaseEnterBehavior()
    {
        base.PhaseEnterBehavior();
        if (this.GetManagerPhase() == Phase.Player /*&& hasn't reached the last*/) this.SwitchPhase(Phase.Boss);
    }
}
