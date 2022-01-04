using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobstobotomizerSpecial1 : BossSpecial
{

    void Start()
    {
        AttackType = BossAttackType.WATER;
        Damage = 5.0f;
        WindUpDuration = 1.0f;
        LingerDuration = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    override public void RunWindUpAnimation()
    {
        Boss.SetPosRelStart(.7f, .7f);
    }

    override public IEnumerator RunAttackAnimation()
    {
        Boss.smoothMoveTarget = Boss.GetPosRelStart(0, -.5f);
        Boss.isSmoothMoving = true;

        new WaitUntil(() => AttackStage == 1);

        yield return null;
    }

    override public void RunReturnAnimation()
    {
        Boss.BeginSmoothMoveToStart();
    }
}
