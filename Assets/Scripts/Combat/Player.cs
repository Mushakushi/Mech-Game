using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        health = 100.0f;
        speed = 15.0f;
        damage = 5.0f;
        resistance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (TrySmoothMove())
        {
            if (isAttacking)
            {
                combat.DoBossDamage(damage);
                smoothMoveTarget = startPos;
                isAttacking = false;
                isSmoothMoving = true;
                // animation
            }
            else
            {
                // runs when done returning
            }
        }
        else if (!isSmoothMoving && combat.playerCanAttack)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                DoAttack();
            }
        }

    }

    public void DoAttack()
    {
        // animation
        smoothMoveTarget = MoveUtil.GetPosFromPos(startPos, 0f, 1.2f);
        isAttacking = true;
        isSmoothMoving = true;
    }

    override public void RunHitAnimation()
    {
    }

    override public void OnGetHit(float damage)
    {
    }

    override public void TakeDamage(float damage)
    {
    }
}
