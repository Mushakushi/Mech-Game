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

    /// <summary>
    /// Runs completely after WindUpDelay. Used for whole attack animation, including anything before/during LingerDelay.
    /// </summary>
    /// <returns></returns>
    override public IEnumerator RunAttackAnimation()
    {
        //Boss.StartShake(.5f, .1f);
        yield return new WaitForSecondsRealtime(.5f);

        Boss.BeginSmoothMoveToPos(Boss.GetPosRelStart(0, -1.5f), 15.0f);
        yield return new WaitUntil(() => AttackStage == 2); // increases by one when smooth move finishes

        //Boss.StartShake(.2f, .03f);

        yield return null;
    }

    /// <summary>
    /// Runs completely after LingerDelay. Used only for returning to the start position.
    /// </summary>
    /// <returns></returns>
    override public IEnumerator RunReturnAnimation()
    {
        Boss.BeginSmoothMoveToStart();

        yield return null;
    }
}
