using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobstobotomizerSpecial1 : BossSpecial
{

    void Start()
    {
        AttackType = BossAttackType.WATER;
        Damage = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    override public void RunAttackAnimation()
    {
        //transform.position = startPos + Vector3.up * 1.0f;
        attackDelayCoroutine = StartCoroutine(DelayForSeconds(AttackDelay));
    }

    override public IEnumerator DelayForSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        //isAttacking = true;
    }
}
