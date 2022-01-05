using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobstobotomizerSpecial1 : BossSpecial
{

    void Start()
    {
        AttackType = BossAttackType.Physical;
        Damage = 5.0f;
        WindUpDuration = 1.0f;
        LingerDuration = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    override public IEnumerator RunWindUpAnimation()
    {
        Boss.BeginSmoothMoveToPos(Boss.GetPosRelStart(0f, 1f), 10f);

        yield return null;
    }

    override public IEnumerator RunAttackAnimation()
    {
        Boss.BeginSmoothMoveToPos(Boss.GetPosRelStart(0, -.8f), 15.0f);

        new WaitUntil(() => AttackStage == 1); // increases by one when smooth move finishes

        yield return null;
    }

    override public void RunReturnAnimation()
    {
        Boss.BeginSmoothMoveToStart();
    }
}
