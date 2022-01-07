using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private ACTION_TYPE currentActionType;
    [SerializeField] private int currentActionStage = 0;
    [SerializeField] private float actionDelay;
    [SerializeField] private float attackDuration;
    [SerializeField] private float dodgeDuration;
    [SerializeField] private bool inActionDelay = false;
    private IEnumerator queuedAction;
    private bool allowQueueAction;
    private enum ACTION_TYPE { Attack, Dodge, Block, None }

    // Start is called before the first frame update
    override public void OnStart()
    {
        startPos = transform.position;
        health = 100.0f;
        defaultSpeed = 20f;
        actionDelay = 0.15f;
        attackDuration = 0.2f;
        currentActionType = ACTION_TYPE.None;
        allowQueueAction = true;
        queuedAction = null;
        dodgeDuration = 0.7f;
        damage = 5.0f;
        resistance = 0f;
        isSmoothMoving = false;
    }

    // Update is called once per frame
    override public void OnUpdate()
    {
        if (TrySmoothMove())
        {
            if (currentActionStage == 1)
            {
                // runs when at furthest point
                switch (currentActionType)
                {
                    case ACTION_TYPE.Attack:
                        combat.DoBossDamage(damage);
                        break;
                    case ACTION_TYPE.Dodge:
                        break;
                }
                currentActionStage++;
                allowQueueAction = true;
            }
            else if (currentActionStage == 2)
            {
                // runs when done returning
                currentActionStage = 0;
                StartCoroutine(WaitActionDelay());
            }
        }

        // input queueing
        if (allowQueueAction)
        {
            if (combat.PlayerCanAttack() && (currentActionType == ACTION_TYPE.Attack || currentActionType == ACTION_TYPE.None) && Input.GetKeyDown(KeyCode.W))
            {
                queuedAction = DoAttack();
            }
            else if ((currentActionType == ACTION_TYPE.Dodge || currentActionType == ACTION_TYPE.None))
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    queuedAction = DoDodge("left");
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    queuedAction = DoDodge("right");
                }
            }
        }

        if (!inActionDelay && queuedAction != null)
        {
            inActionDelay = true;
            StartCoroutine(queuedAction);
            allowQueueAction = false;
            queuedAction = null;
        }
    }

    public IEnumerator DoAttack()
    {
        animator.SetTrigger("Attack");
        yield return null;
        /*currentActionType = ACTION_TYPE.Attack;
        currentActionStage = 1;
        // animation
        BeginSmoothMoveToPos(GetPosRelStart(0.25f, 2f), 30f);
        yield return new WaitUntil(() => currentActionStage == 2);
        yield return new WaitForSecondsRealtime(attackDuration);
        BeginSmoothMoveToStart();*/
    }

    public IEnumerator DoDodge(string direction) // TODO: make this not use string (it is yucky)
    {
        currentActionType = ACTION_TYPE.Dodge;
        currentActionStage = 1;
        if (direction.Equals("left"))
            BeginSmoothMoveToPos(GetPosRelStart(-2f, .1f));
        else if (direction.Equals("right"))
            BeginSmoothMoveToPos(GetPosRelStart(2f, .1f));
        yield return new WaitUntil(() => currentActionStage == 2);
        yield return new WaitForSecondsRealtime(dodgeDuration);
        BeginSmoothMoveToStart();
    }

    public IEnumerator WaitActionDelay()
    {
        currentActionType = ACTION_TYPE.None;
        yield return new WaitForSecondsRealtime(actionDelay);
        inActionDelay = false;
    }

    override public void OnGetHit(float damage)
    {
    }

    override public void TakeDamage(float damage)
    {
    }
}
