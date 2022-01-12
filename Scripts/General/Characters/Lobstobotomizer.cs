using System.Collections.Generic;

public class Lobstobotomizer : Boss
{
    public override void SetBossValues()
    {
        health = 250.0f;
        damage = 0f;
        resistance = 1.0f;
        SpecialAttackWeights = new List<int> { 100 };
    }
}
