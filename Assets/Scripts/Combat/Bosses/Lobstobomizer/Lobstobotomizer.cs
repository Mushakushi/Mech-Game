using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobstobotomizer : Boss
{
    public override void SetBossValues()
    {
        health = 250.0f;
        damage = 0f;
        resistance = 1.0f;
        defaultSpeed = 5.0f;
        SpecialAttackWeights = new List<int> { 100 };
    }
}
