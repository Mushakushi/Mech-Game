using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The default boss component attached to boss
/// </summary>
public class UnimplementedBoss : Boss
{
    public override Phase[] activePhases => throw new System.NotImplementedException(); 

    public override int maxHealthBars => throw new System.NotImplementedException();

    public override List<float> specialWeights => throw new System.NotImplementedException();

    public override string characterName => throw new System.NotImplementedException();

    public override float maxHealth => throw new System.NotImplementedException();

    public override float resistance => throw new System.NotImplementedException();

    protected override void OnInitializeBoss() => throw new System.NotImplementedException();
}
