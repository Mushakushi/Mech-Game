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
        StunStages = new (float x, float y, float duration)[] { (.35f, .35f, 2.5f), (-.25f, .55f, 1.5f), (.25f, .75f, 0.5f) };
    }

    override public void DoNormalAttack()
    {
        return;
    }
}
