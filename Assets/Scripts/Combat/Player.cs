using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private bool isAttacking = false;

    // Start is called before the first frame update
    override public void OnStart()
    {
        startPos = transform.position;
        health = 100.0f;
        defaultSpeed = 15.0f;
        damage = 5.0f;
        resistance = 0f;
        isSmoothMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (TrySmoothMove())
        {
            if (isAttacking)
            {
                combat.DoBossDamage(damage);
                isAttacking = false;
                BeginSmoothMoveToStart();
                
                // animation
            }
            else
            {
                // runs when done returning
            }
        }
        else if (combat.PlayerCanAttack() && !isSmoothMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                DoAttack();
            }
        }

    }

    public void DoAttack()
    {
        BeginSmoothMoveToPos(GetPosRelStart(0f, 1.2f));
        // animation
        isAttacking = true;
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
