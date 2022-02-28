using System.Collections.Generic;
using UnityEngine;

public class Shoto : Boss
{
    public override string characterName => "Shoto";
    public override float maxHealth => 1; 
    public override float resistance => 1;
    public override Phase[] activePhases => base.activePhases;

    public override int maxHealthBars => 2; 
    public override List<float> specialWeights => new List<float> { 100 };

    protected override void OnInitializeBoss() { }
}
