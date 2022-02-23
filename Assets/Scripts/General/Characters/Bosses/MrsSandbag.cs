using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrsSandbag : Boss
{
    public override string characterName => "Mrs. Sandbag";
    public override float maxHealth => 100;
    public override float resistance => 0;
    public override Phase[] activePhases => base.activePhases;

    public override int maxHealthBars => 3;
    public override List<float> specialWeights => new List<float> { 100 };

    protected override void OnInitializeBoss() { }

    /// <summary>
    /// Mrs sandbag is immortal
    /// </summary>
    public override void OnHitboxEnter(float damage)
    {
        base.OnHitboxEnter(damage);
        health += damage;
    }
}
