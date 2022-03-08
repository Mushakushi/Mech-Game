using System.Collections.Generic;
using UnityEngine;

public class Lobstobotomizer : Boss
{
    public override string characterName => "Lobstobotomizer";
    public override float maxHealth => 10; 
    public override float resistance => 1;
    public override Phase[] activePhases => base.activePhases;

    public override int maxHealthBars => 3; 
    public override List<float> specialWeights => new List<float> { 50, 50};

    protected override void OnInitializeBoss() {
        projectileManager.speed = 250f;
    }
}
