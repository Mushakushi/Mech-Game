using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private ACTION_TYPE currentActionType;
    [SerializeField] private float actionDelay;
    [SerializeField] private bool inActionDelay = false;
    private IEnumerator queuedAction;
    public bool allowQueueAction;
    private enum ACTION_TYPE { Attack, Dodge, Block, None }
    private enum DODGE_DIRECTION { Left, Right }

    // Start is called before the first frame update
    void Start()
    {
        health = 100.0f;
        damage = 5.0f;
        resistance = 0f;

        actionDelay = 0.35f;
        currentActionType = ACTION_TYPE.None;
        allowQueueAction = true;
        queuedAction = null;

        //triggerLayerMask.SetLayerMask(LayerMask.GetMask("Player Attack"));
    }

    // Update is called once per frame
    void Update()
    {
        if (returnToIdle)
        {
            animator.applyRootMotion = false;
            StartCoroutine(WaitActionDelay());
            returnToIdle = false;
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
                    queuedAction = DoDodge(DODGE_DIRECTION.Left);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    queuedAction = DoDodge(DODGE_DIRECTION.Right);
                }
            }
        }

        // run queue
        if (!inActionDelay && queuedAction != null)
        {
            inActionDelay = true;
            StartCoroutine(queuedAction);
            allowQueueAction = false;
            queuedAction = null;
        }
    }

    private IEnumerator DoAttack()
    {
        animator.SetTrigger("Attack");
        yield return null;
    }

    private IEnumerator DoDodge(DODGE_DIRECTION direction) // TODO: make this not use string (it is yucky)
    {
        animator.SetInteger("Dodge Direction", (int) direction);
        animator.SetTrigger("Dodge");
        yield return null;
    }

    public IEnumerator WaitActionDelay()
    {
        currentActionType = ACTION_TYPE.None;
        yield return new WaitForSecondsRealtime(actionDelay);
        inActionDelay = false;
    }
}
