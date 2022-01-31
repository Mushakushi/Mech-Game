using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] private ACTION_TYPE currentActionType;
    [SerializeField] private float actionDelay;
    private IEnumerator queuedAction;
    private Coroutine delayCoroutine = null;
    public bool allowQueueAction;
    public bool canAttack;
    private enum ACTION_TYPE { Attack, Dodge, Block, None }
    private enum DODGE_DIRECTION { Left, Right }

    // Start is called before the first frame update
    public override IList<Phase> InitializeCharacter()
    {
        health = 100.0f;
        damage = 5.0f;
        resistance = 0f;

        actionDelay = 0.25f;
        currentActionType = ACTION_TYPE.None;
        allowQueueAction = true;
        queuedAction = null;

        canAttack = true;

        //triggerLayerMask.SetLayerMask(LayerMask.GetMask("Player Attack"));

        return new Phase[]{ Phase.Player, Phase.Boss }; 
    }

    public IEnumerator WaitActionDelay()
    {
        yield return new WaitForSecondsRealtime(actionDelay);
        delayCoroutine = null;
    }

    protected override void PhaseEnterBehavior()
    {
        if (this.GetManagerPhase() == Phase.Player)
        {
            canAttack = true;
            EnableHitbox();
        }
    }
    protected override void PhaseUpdateBehavior() 
    {
        if (returnToIdle)
        {
            animator.applyRootMotion = false;
            currentActionType = ACTION_TYPE.None;
            delayCoroutine = StartCoroutine(WaitActionDelay());
            returnToIdle = false;
        }

        // input queueing - appears to be broken (animation issue - currently uses returnToIdle ^)
        if (allowQueueAction)
        {
            if (this.GetManagerPhase() == Phase.Player && canAttack && (currentActionType == ACTION_TYPE.Attack || currentActionType == ACTION_TYPE.None) && Input.GetKeyDown(KeyCode.W))
            {
                queuedAction = DoAttack();
            }
            else if (currentActionType == ACTION_TYPE.Dodge || currentActionType == ACTION_TYPE.None)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    queuedAction = DoDodge(DODGE_DIRECTION.Left);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    queuedAction = DoDodge(DODGE_DIRECTION.Right);
                }
            }
        }

        // run queue
        if (delayCoroutine == null && queuedAction != null)
        {
            StartCoroutine(queuedAction);
            //allowQueueAction = false; (case in point)
            queuedAction = null;
        }
    }
    protected override void PhaseExitBehavior()
    {
        canAttack = false;
        DisableHitbox(); 
    }

    protected override void OnHealthDeplete()
    {
        print("player defeated"); 
    }

    private IEnumerator DoAttack()
    {
        animator.SetTrigger("Attack");
        yield return null;
    }

    private IEnumerator DoDodge(DODGE_DIRECTION direction)
    {
        animator.SetInteger("Dodge Direction", (int)direction);
        animator.SetTrigger("Dodge");
        yield return null;
    }
}
