using System.Collections.Generic;
using UnityEngine;

public class Shoto : Boss
{
    public override string characterName => "Shoto";
    public override float maxHealth => 10; 
    public override float resistance => 1;
    public override Phase[] activePhases => base.activePhases;

    public override int maxHealthBars => 3; 
    public override List<float> specialWeights => new List<float> { 100 };

    protected override void OnInitializeBoss() { }
}
